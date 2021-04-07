using PersonalFinanceManager.Client.Contracts;
using PersonalFinanceManager.Client.Properties;
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
    public class ExpensesViewModel : IViewModel
    {
        private readonly HttpClient _apiClient;
        private int _valueToAdd;

        public ExpensesViewModel (HttpClient apiClient)
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
            => await GetCurrentExpenses();

        public async Task AddExpense()
        {
            if (ValueToAdd == 0)
            {
                return;
            }

            var data = new Expense { Amount = ValueToAdd };

            using (var cts = new CancellationTokenSource(Constants.ApiTimeOut))
            {
                var result = await _apiClient.PostAsJsonAsync("Expenses", data, cts.Token);

                if (result.IsSuccessStatusCode)
                {
                    CurrentAmount += ValueToAdd;
                }
            }
        }

        private async Task GetCurrentExpenses()
        {
            IEnumerable<Expense> currentExpenses;

            using (var cts = new CancellationTokenSource(Constants.ApiTimeOut))
            {
                currentExpenses = await _apiClient.GetFromJsonAsync<IEnumerable<Expense>>("Expenses", cts.Token);
            }

            float totalAmount = 0;

            foreach (var income in currentExpenses)
            {
                totalAmount += income.Amount;
            }

            CurrentAmount = totalAmount;
        }
    }
}
