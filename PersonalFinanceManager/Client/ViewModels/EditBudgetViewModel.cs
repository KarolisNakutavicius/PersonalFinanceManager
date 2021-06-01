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
    public class EditBudgetViewModel : IViewModel
    {
        private readonly HttpClient _httpClient;

        public EditBudgetViewModel(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public string ErrorMessage { get; set; }

        public string SelectedCategory { get; set; }

        public IList<Budget> Budgets { get; set; } = new List<Budget>();

        public event EventHandler ChangeState;

        public event EventHandler OpenRequested;

        public async Task OnInit()
        { }

        public async Task Open()
        {
            using (var cts = new CancellationTokenSource(Constants.ApiTimeOut))
            {
                Budgets = await _httpClient.GetFromJsonAsync<List<Budget>>($"Budgets/all", cts.Token);
            }

            this.OpenRequested?.Invoke(this, EventArgs.Empty);
        }

        public async Task ModifyBudget(Budget modifiedBudget)
        {

        }

        public async Task DeleteBudget(Budget budgetToDelete)
        {

        }


    }
}
