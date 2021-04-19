using Microsoft.AspNetCore.Components;
using PersonalFinanceManager.Client.Contracts;
using PersonalFinanceManager.Client.Enums;
using PersonalFinanceManager.Client.Properties;
using PersonalFinanceManager.Shared.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;

namespace PersonalFinanceManager.Client.ViewModels
{
    public class AddViewModel : IViewModel
    {
        private readonly HttpClient _apiClient;

        [Required]
        public StatementType StatementType { get; set; }

        [Required]
        public float Value { get; set; }

        [Required]
        public DateTime Date { get; set; }

        public string NewColorHex { get; set; }

        [Required]
        public string NewCategory { get; set; }

        public AddViewModel(HttpClient apiClient)
        {
            _apiClient = apiClient;    
        }

        public async Task OnInit()
        {
            Date = DateTime.Now;
        }

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

            using (var cts = new CancellationTokenSource(Constants.ApiTimeOut))
            {
                var result = await _apiClient.PostAsJsonAsync(StatementType.GetDescription(), newStatement, cts.Token);
            }            
        }

    }
}
