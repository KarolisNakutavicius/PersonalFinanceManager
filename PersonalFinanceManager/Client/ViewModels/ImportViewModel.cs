using FileHelpers;
using Microsoft.AspNetCore.Components.Forms;
using PersonalFinanceManager.Client.Contracts;
using PersonalFinanceManager.Client.Helpers.CSV.Models;
using PersonalFinanceManager.Client.Properties;
using PersonalFinanceManager.Client.Services;
using PersonalFinanceManager.Shared.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;

namespace PersonalFinanceManager.Client.ViewModels
{
    public class ImportViewModel : IViewModel
    {
        private readonly CategoryManager _categoryManager;
        private readonly HttpClient _apiClient;

        public ImportViewModel(HttpClient httpClient, CategoryManager categoryManager)
        {
            _apiClient = httpClient;
            _categoryManager = categoryManager;
        }

        public List<LuminorStatement> CsvStatements { get; set; } = new List<LuminorStatement>();

        public bool IsAnyStatements { get; set; }

        public string Title { get; set; } = "Import csv file";

        public event EventHandler ChangeState;
        public async Task OnInit()
        {
        }

        public async Task OnFileAdded(InputFileChangeEventArgs eventArgs)
        {
            var browserFile = eventArgs.File;

            var engine = new FileHelperEngine(typeof(LuminorStatement));

            using (var reader = new StreamReader(browserFile.OpenReadStream()))
            {
                string content = await reader.ReadToEndAsync();

                var statements = engine.ReadStringAsList(content);

                foreach (var statement in statements)
                {
                    CsvStatements.Add((LuminorStatement)statement);
                }
            }

            if (CsvStatements.Count != 0)
            {
                IsAnyStatements = true;
            }

        }

        public async Task Import()
        {
            foreach (var csvStatement in CsvStatements)
            {
                var apiStatement = new Statement
                {
                    Amount = csvStatement.Amount,
                    DateTime = csvStatement.Date,
                    Details = csvStatement.Details
                };

                string requestUri = string.Empty;

                if (csvStatement.IsExpense)
                {
                    apiStatement.Category = new Category
                    {
                        Name = "Expense",
                        ColorHex = "#FF0000" // red hex
                    };
                    requestUri = "Expenses";
                }
                else
                {
                    apiStatement.Category = new Category
                    {
                        Name = "Income",
                        ColorHex = "#008000" // green hex
                    };
                    requestUri = "Incomes";
                }


                using (var cts = new CancellationTokenSource(Constants.ApiTimeOut))
                {
                    var result = await _apiClient.PostAsJsonAsync(requestUri, apiStatement, cts.Token);

                    if (!result.IsSuccessStatusCode)
                    {
                        Title = "Something went wrong while saving statements to data base. Please try again later";
                        return;
                    }
                }

            }

            await _categoryManager.GetAllCategories();

            Title = "Statements imported successfully";
        }
    }
}
