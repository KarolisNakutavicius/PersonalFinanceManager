using ChartJs.Blazor;
using ChartJs.Blazor.Common;
using ChartJs.Blazor.PieChart;
using ChartJs.Blazor.Util;
using Microsoft.AspNetCore.Components;
using PersonalFinanceManager.Client.Contracts;
using PersonalFinanceManager.Client.Properties;
using PersonalFinanceManager.Shared.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
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
        private DateTime? _dateFrom;
        private DateTime? _dateTo;
        private IList<Expense> _expenses;

        public ExpensesViewModel(HttpClient apiClient)
        {
            _apiClient = apiClient;
        }

        public int ValueToAdd;

        public DateTime DateFrom
        {
            get => _dateFrom ?? DateTime.Now.AddMonths(-1);
            set
            {
                _dateFrom = value;
                GeneratePie();
            }
        }

        public DateTime DateTo
        {
            get => _dateTo ?? DateTime.Now;
            set
            {
                _dateTo = value;
                GeneratePie();
            }
        }


        public float CurrentAmount => _expenses.Sum(e => e.Amount);

        public PieConfig Config = new PieConfig
        {
            Options = new PieOptions
            {
                Responsive = true,
                Title = new OptionsTitle
                {
                    Display = true,
                    Text = "Your expenses"
                }
            }
        };

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
                    _expenses.Add(data);
                }
            }
        }

        public async Task OnInit()
        {
            await GetExpenses();

            if (_expenses != null)
            {
                GeneratePie();
            }
        }


        private async Task GetExpenses()
        {
            //using (var cts = new CancellationTokenSource(Constants.ApiTimeOut))
            //{
            //    Expenses = await _apiClient.GetFromJsonAsync<IEnumerable<Expense>>("Expenses", cts.Token);
            //}

            _expenses = new List<Expense>
            {
                new Expense {
                    Amount = 200,
                    Category = new Category { Name = "Other", ColorHex = "#a10ef1" },
                    DateTime = DateTime.Now.AddDays(-3)
                },
                new Expense {
                    Amount = 260,
                    Category = new Category { Name = "Clothes", ColorHex = "#ceff00" },
                    DateTime = DateTime.Now.AddDays(-3)
                },
                new Expense {
                    Amount = 200,
                    Category = new Category { Name = "Other", ColorHex = "#a10ef1" },
                    DateTime = DateTime.Now.AddMonths(-2)
                },
                new Expense {
                    Amount = 20,
                    Category = new Category { Name = "Gym", ColorHex = "#0041ff" },
                    DateTime = DateTime.Now.AddMonths(-2)
                }
            };
        }

        public void GeneratePie()
        {
            Config.Data.Labels.Clear();
            Config.Data.Datasets.Clear();

            IList<Expense> expensesInCurrentDateTime = _expenses.Where(e => e.DateTime > DateFrom && e.DateTime < DateTo).ToList();
            PieDataset<float> dataset = new PieDataset<float>();
            Random rnd = new Random();
            IList<string> colors = new List<string>();

            foreach (var expense in expensesInCurrentDateTime)
            {
                var category = expense.Category.Name;

                if (!Config.Data.Labels.Contains(category))
                {
                    Config.Data.Labels.Add(category);
                    dataset.Add(expense.Amount);
                    colors.Add(expense.Category.ColorHex);
                }

                int indexOfLabel = Config.Data.Labels.IndexOf(category);

                dataset[indexOfLabel] += expense.Amount;
            }

            dataset.BackgroundColor = colors.ToArray();

            Config.Data.Datasets.Add(dataset);
        }
    }
}
