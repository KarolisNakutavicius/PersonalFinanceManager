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
                Categories = _categoryManager.GetCategories(_type);
            }
        }


        [Required]
        public float Value { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public string NewColorHex { get; set; }


        public string NewCategory
        {
            get => _newCategory;
            set 
            {
                _newCategory = value;

                if(_newCategory != string.Empty)
                {
                    SelectedCategory = _newCategory;
                }
            }
        }

        public string SelectedCategory { get; set; }


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
                    Name = NewCategory
                }
            };

            using (var cts = new CancellationTokenSource())
            {
                var result = await _apiClient.PostAsJsonAsync(StatementType.GetDescription(), newStatement, cts.Token);

                if(result.IsSuccessStatusCode)
                {
                    this.OnAddSuccess?.Invoke(this, EventArgs.Empty);
                }
            }            
        }

        public async Task Open(StatementType type)
        {
            StatementType = type;            
            Date = DateTime.Now;
            NewColorHex = "#CD32C8";
            Categories = _categoryManager.GetCategories(type);
            OnSelectionChanged();
            this.OpenRequested?.Invoke(this, EventArgs.Empty);
        }

        public void OnSelectionChanged()
        {
            if (SelectedCategory != string.Empty)
            {
                NewCategory = string.Empty;
                var categoryObj = Categories.FirstOrDefault(c => c.Name.Equals(SelectedCategory));
                if (categoryObj != null)
                {
                    NewColorHex = categoryObj.ColorHex;
                }
            }
        }
    }
}
