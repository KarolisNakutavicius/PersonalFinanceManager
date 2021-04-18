using ChartJs.Blazor.Common;
using ChartJs.Blazor.PieChart;
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

namespace PersonalFinanceManager.Client.Abstract
{
    public abstract class StatementsBaseViewModel : IViewModel
    {
        private readonly HttpClient _apiClient;
        private DateTime? _dateFrom;
        private DateTime? _dateTo;
        private IList<Statement> _statements;

        public StatementsBaseViewModel(HttpClient apiClient)
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

        public float CurrentAmount => _statements.Sum(e => e.Amount);

        public abstract bool IsExpensePage { get; }

        public PieConfig Config;

        public async Task Add()
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
                    _statements.Add(data);
                }
            }
        }

        public async Task OnInit()
        {
            Config = new PieConfig
            {
                Options = new PieOptions
                {
                    Responsive = true,
                    Title = new OptionsTitle
                    {
                        Display = true,
                        Text = IsExpensePage ?
                               "Your expenses" :
                               "Your incomes"
                    }
                }
            };
            await GetExpenses();

            if (_statements != null)
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

            _statements = new List<Statement>
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

            IList<Statement> statementsInTime = _statements.Where(e => e.DateTime > DateFrom && e.DateTime < DateTo).ToList();
            PieDataset<float> dataset = new PieDataset<float>();
            Random rnd = new Random();
            IList<string> colors = new List<string>();

            foreach (var expense in statementsInTime)
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

