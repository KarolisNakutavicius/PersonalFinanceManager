using Microsoft.JSInterop;
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
        private readonly IJSRuntime _jsRuntime;
        private int _valueToAdd;

        public IList<Expense> Expenses { get; set; }

        public ExpensesViewModel (HttpClient apiClient, IJSRuntime jsRuntime)
        {
            _apiClient = apiClient;
            _jsRuntime = jsRuntime;
        }

        public int ValueToAdd
        {
            get { return _valueToAdd; }
            set { _valueToAdd = value; }
        }

        public float CurrentAmount { get; set; }

        public async Task OnInit()
            => _ = Task.Run(() => GetCurrentExpenses());

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

            Expenses = new List<Expense>
            {
                new Expense { Amount = 100, Category = "Other"},
                new Expense { Amount = 150, Category = "Clothes"},
                new Expense { Amount = 239, Category = "Other"}
            };

            await _jsRuntime.InvokeVoidAsync("GeneratePieChart", Expenses);
            //using (var cts = new CancellationTokenSource(Constants.ApiTimeOut))
            //{
            //    currentExpenses = await _apiClient.GetFromJsonAsync<IEnumerable<Expense>>("Expenses", cts.Token);
            //}

            //float totalAmount = 0;

            //foreach (var income in currentExpenses)
            //{
            //    totalAmount += income.Amount;
            //}

            //CurrentAmount = totalAmount;
        }
    }
}
