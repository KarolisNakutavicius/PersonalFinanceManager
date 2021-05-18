using Microsoft.AspNetCore.Components;
using PersonalFinanceManager.Client.Contracts;
using PersonalFinanceManager.Client.Enums;
using PersonalFinanceManager.Client.Helpers;
using PersonalFinanceManager.Client.Properties;
using PersonalFinanceManager.Client.Services;
using PersonalFinanceManager.Shared.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;

namespace PersonalFinanceManager.Client.ViewModels
{
    public class AddViewModel : IViewModel
    {
        private readonly HttpClient _apiClient;
        private readonly CategoryManager _categoryManager;
        private string _newCategory;
        private StatementType _type;

        public bool IsBudget { get; set; }

        [Required]
        public StatementType StatementType
        {
            get => _type;
            set
            {
                _type = value;
                _ = GetCategories();
            }
        }

        [Required]        
        [Range(1, double.MaxValue)]
        public float Value { get; set; }

        [RequiredIf("IsBudget", false)]
        public DateTime Date { get; set; }

        [RequiredIf("IsBudget", false)]
        public string NewColorHex { get; set; }

        public string NewCategory
        {
            get => _newCategory;
            set 
            {
                _newCategory = value;

                if(_newCategory != string.Empty)
                {
                    SelectedCategory = string.Empty;
                }
            }
        }

        public string SelectedCategory { get; set; } = string.Empty;

        [RequiredIf("IsBudget", true)]
        public string BudgetName { get; set; } = string.Empty;

        public event EventHandler OnAddSuccess;

        public event EventHandler OpenRequested;

        public Action<Budget> OnBudgetAdded;

        public IList<Category> Categories { get; set; } = new List<Category>();

        public AddViewModel(HttpClient apiClient, CategoryManager categoryManager)
        {
            _apiClient = apiClient;
            _categoryManager = categoryManager;
        }

        public async Task OnInit()
        {}

        public event EventHandler ChangeState;

        public async Task Add()
        {
            if (IsBudget)
            {
                await AddBudget();
                return;
            }

            await AddStatement();
        }

        private async Task AddStatement()
        {
            Statement newStatement = new Statement
            {
                Amount = Value,
                DateTime = Date,
                Category = new Category
                {
                    ColorHex = NewColorHex,
                    Name = SelectedCategory != string.Empty ?
                           SelectedCategory :
                           NewCategory
                }
            };

            using (var cts = new CancellationTokenSource(Constants.ApiTimeOut))
            {
                var result = await _apiClient.PostAsJsonAsync(StatementType.GetDescription(), newStatement, cts.Token);

                if (result.IsSuccessStatusCode)
                {
                    await _categoryManager.GetAllCategories();
                    this.OnAddSuccess?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        private async Task AddBudget()
        {
            Budget newBudget = new Budget
            {
                Amount = (int)Value,
                Name = BudgetName
            };

            if (SelectedCategory == Constants.AllCategories)
            {
                newBudget.Categories = await _categoryManager.GetExpenseCategories();
            }
            else if (!string.IsNullOrEmpty(SelectedCategory))
            {
                newBudget.Categories = Categories.Where(c => c.Name == SelectedCategory).ToList();
            }

            using (var cts = new CancellationTokenSource())
            {
                var result = await _apiClient.PostAsJsonAsync("Budgets", newBudget, cts.Token);

                if (result.IsSuccessStatusCode)
                {
                    this.OnAddSuccess?.Invoke(this, EventArgs.Empty);
                    this.OnBudgetAdded(newBudget);
                }
            }
        }

        public async Task Open(StatementType type)
        {
            StatementType = type;            
            Date = DateTime.Now;
            NewColorHex = Constants.DefaultColorHex;
            Value = 0;

            await GetCategories();
            OnSelectionChanged();
            this.OpenRequested?.Invoke(this, EventArgs.Empty);
        }

        public void OnSelectionChanged()
        {
            if (Categories.Count == 0)
            {
                return;
            }

            NewCategory = string.Empty;
            var categoryObj = Categories.FirstOrDefault(c => c.Name.Equals(SelectedCategory));
            if (categoryObj == null)
            {
                categoryObj = Categories.First();
            }
            
            SelectedCategory = categoryObj.Name;
            NewColorHex = categoryObj.ColorHex;
        }

        private async Task GetCategories()
        {
            Categories.Clear();

            Categories = await _categoryManager.GetCategories(_type);
            IsBudget = _type == StatementType.Budget;

            if (IsBudget)
            {
                Categories.Add(new Category
                {
                    Name = Constants.AllCategories
                });
            }
        }
    }
}
