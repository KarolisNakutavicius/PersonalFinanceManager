using PersonalFinanceManager.Client.Contracts;
using PersonalFinanceManager.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;

namespace PersonalFinanceManager.Client.ViewModels
{
    public class IncomeViewModel : IViewModel
    {
        private readonly HttpClient _apiClient;
        public float Amount { get; set; }

        public IncomeViewModel(HttpClient apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task OnInit()
        {
            IEnumerable<IncomeModel> currentIncomes;

            using (var cts = new CancellationTokenSource(2000))
            {
                currentIncomes = await _apiClient.GetFromJsonAsync<IEnumerable<IncomeModel>>("Users/1/Incomes", cts.Token);
            }

            Amount = currentIncomes?.FirstOrDefault()?.Amount ?? 600;
        }
    }
}
