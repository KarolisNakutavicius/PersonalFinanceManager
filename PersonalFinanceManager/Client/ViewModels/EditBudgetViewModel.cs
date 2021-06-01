using PersonalFinanceManager.Client.Contracts;
using PersonalFinanceManager.Client.Properties;
using PersonalFinanceManager.Client.Services;
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
        private readonly CategoryManager _categoryManager;

        public EditBudgetViewModel(HttpClient httpClient,
            CategoryManager categoryManager)
        {
            _httpClient = httpClient;
            _categoryManager = categoryManager;
        }

        public string ErrorMessage { get; set; }

        public string SelectedCategory { get; set; }

        public IList<Budget> Budgets { get; set; } = new List<Budget>();

        public IList<Category> Categories { get; set; } = new List<Category>();

        public event EventHandler ChangeState;

        public event EventHandler OpenRequested;

        public event EventHandler BudgetModified;

        public async Task OnInit()
        { }

        public async Task Open()
        {
            ErrorMessage = string.Empty;
            using (var cts = new CancellationTokenSource(Constants.ApiTimeOut))
            {
                Budgets = await _httpClient.GetFromJsonAsync<List<Budget>>($"Budgets/all", cts.Token);
            }

            Categories = await _categoryManager.GetExpenseCategories();

            this.OpenRequested?.Invoke(this, EventArgs.Empty);
        }

        public async Task ModifyBudget(Budget modifiedBudget)
        {
            ErrorMessage = string.Empty;

            ModifyBudgetCategories(modifiedBudget);

            var result = await _httpClient.PutAsJsonAsync<Budget>($"Budgets/{modifiedBudget.BudgetId}", modifiedBudget);

            if (!result.IsSuccessStatusCode)
            {
                ErrorMessage = "Could not update category. Try again later";
                return;
            }

            await Task.Delay(2000);

            this.BudgetModified?.Invoke(this, EventArgs.Empty);
        }

        public async Task DeleteBudget(Budget budgetToDelete)
        {
            ErrorMessage = string.Empty;

            var result = await _httpClient.DeleteAsync($"Budgets/{budgetToDelete.BudgetId}");

            if (!result.IsSuccessStatusCode)
            {
                ErrorMessage = "Could not delete budget. Try again later";
                return;
            }

            Budgets.Remove(budgetToDelete);

            this.BudgetModified?.Invoke(this, EventArgs.Empty);
        }

        private void ModifyBudgetCategories(Budget modifiedBudget)
        {
            if (modifiedBudget.Categories.FirstOrDefault().Name != modifiedBudget.NewCategoryName)
            {
                var category = Categories.FirstOrDefault(c => c.Name == modifiedBudget.NewCategoryName);

                if (category != null)
                {
                    modifiedBudget.Categories.Clear();
                    modifiedBudget.Categories.Add(category);
                }
            }
        }

    }
}
