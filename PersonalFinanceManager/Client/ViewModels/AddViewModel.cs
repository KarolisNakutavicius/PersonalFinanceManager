using Microsoft.AspNetCore.Components;
using PersonalFinanceManager.Client.Contracts;
using PersonalFinanceManager.Client.Enums;
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

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public string NewColorHex { get; set; }


        public bool IsBudget { get; set; }

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

        public event EventHandler OnAddSuccess;

        public event EventHandler OpenRequested;

        public IList<Category> Categories { get; set; } = new List<Category>();

        public AddViewModel(HttpClient apiClient, CategoryManager categoryManager)
        {
            _apiClient = apiClient;
            _categoryManager = categoryManager;
        }

        public async Task OnInit()
        {}

        public async Task Add()
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

            using (var cts = new CancellationTokenSource())
            {
                var result = await _apiClient.PostAsJsonAsync(StatementType.GetDescription(), newStatement, cts.Token);

                if(result.IsSuccessStatusCode)
                {
                    await _categoryManager.GetAllCategories();
                    this.OnAddSuccess?.Invoke(this, EventArgs.Empty);
                }
            }            
        }

        public async Task Open(StatementType type)
        {
            StatementType = type;            
            Date = DateTime.Now;
            NewColorHex = "#CD32C8";
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
                    Name = "All categories"
                });
            }
        }
    }
}
