using PersonalFinanceManager.Client.Contracts;
using PersonalFinanceManager.Client.Properties;
using PersonalFinanceManager.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;

namespace PersonalFinanceManager.Client.ViewModels
{
    public class IncomeViewModel : IViewModel
    {
        private readonly HttpClient _apiClient;
        private int _valueToAdd;

        public IncomeViewModel(HttpClient apiClient)
        {
            _apiClient = apiClient;
        }

        public int ValueToAdd
        {
            get { return _valueToAdd; }
            set { _valueToAdd = value; }
        }

        public float CurrentAmount { get; set; }

        public async Task OnInit()
            => await GetCurrentIncomes();

        public async Task AddIncome()
        {
            if (ValueToAdd == 0)
            {
                return;
            }

            var data = new IncomeModel { Amount = ValueToAdd };

            using (var cts = new CancellationTokenSource(Constants.ApiTimeOut))
            {
                var result = await _apiClient.PostAsJsonAsync("Users/1/Incomes", data, cts.Token);

                if (result.IsSuccessStatusCode)
                {
                    CurrentAmount += ValueToAdd;
                }
            }
        }

        private async Task GetCurrentIncomes()
        {
            IEnumerable<IncomeModel> currentIncomes;

            using (var cts = new CancellationTokenSource(Constants.ApiTimeOut))
            {
                currentIncomes = await _apiClient.GetFromJsonAsync<IEnumerable<IncomeModel>>("Users/1/Incomes", cts.Token);
            }

            CurrentAmount = currentIncomes?.FirstOrDefault()?.Amount ?? 0;
        }
    }
}
