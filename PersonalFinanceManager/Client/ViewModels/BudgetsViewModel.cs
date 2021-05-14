using ChartJs.Blazor;
using ChartJs.Blazor.BarChart;
using ChartJs.Blazor.BarChart.Axes;
using ChartJs.Blazor.Common;
using ChartJs.Blazor.Common.Axes;
using ChartJs.Blazor.Common.Enums;
using ChartJs.Blazor.Util;
using PersonalFinanceManager.Client.Contracts;
using PersonalFinanceManager.Client.Enums;
using PersonalFinanceManager.Client.Properties;
using PersonalFinanceManager.Client.Services;
using PersonalFinanceManager.Shared.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;

namespace PersonalFinanceManager.Client.ViewModels
{
    public class BudgetsViewModel : IViewModel
    {
        private readonly HttpClient _apiClient;
        private readonly CategoryManager _categoryManager;
        private readonly AddViewModel _addViewModel;

        private IList<Budget> _budgets;
        private IList<Expense> _expenses;

        public BudgetsViewModel(HttpClient apiClient,
            CategoryManager categoryManager,
            AddViewModel addViewModel)
        {
            _apiClient = apiClient;
            _categoryManager = categoryManager;
            _addViewModel = addViewModel;
        }

        public BarConfig Config { get; set; }

        public Chart Chart { get; set; }


        public async Task OnInit()
        {
            InitializeBarConfig();

            try
            {
                using (var cts = new CancellationTokenSource(Constants.ApiTimeOut))
                {
                    _budgets = await _apiClient.GetFromJsonAsync<List<Budget>>($"Budgets/all", cts.Token);
                    _expenses = await _apiClient.GetFromJsonAsync<List<Expense>>($"Expenses", cts.Token);
                }
            }
            catch (Exception ex)
            {
                //do nothing
            }

            await GenerateBarChart();
        }

        private async Task GenerateBarChart()
        {
            Config.Data.Labels.Clear();
            Config.Data.Datasets.Clear();

            IDataset<int> overSpentSet = new BarDataset<int>()
            {
                Label = "Over Spent",
                BackgroundColor = ColorUtil.FromDrawingColor(Color.FromArgb(128, Color.Red)),
                BorderColor = ColorUtil.FromDrawingColor(Color.Red),
                BorderWidth = 1
            };

            IDataset<int> budgetSet = new BarDataset<int>()
            {
                Label = "Budget",
                BackgroundColor = ColorUtil.FromDrawingColor(Color.FromArgb(128, Color.Blue)),
                BorderColor = ColorUtil.FromDrawingColor(Color.Blue),
                BorderWidth = 1
            };

            IDataset<int> underSpentSet = new BarDataset<int>()
            {
                Label = "Under Spent",
                BackgroundColor = ColorUtil.FromDrawingColor(Color.FromArgb(128, Color.Green)),
                BorderColor = ColorUtil.FromDrawingColor(Color.Green),
                BorderWidth = 1
            };

            for (int i = 0; i < Constants.MonthsInYear; i++)
            {
                int budget = _budgets.FirstOrDefault().Amount;

                int expenseAmount = (int)_expenses.Where(e => e.DateTime.Month == i).Sum(e => e.Amount);

                if (expenseAmount > budget)
                {
                    int overSpend = expenseAmount - budget;

                    overSpentSet.Add(overSpend);
                    budgetSet.Add(budget);
                    underSpentSet.Add(0);
                    continue;
                }

                overSpentSet.Add(0);
                underSpentSet.Add(expenseAmount);

                int leftBudget = budget - expenseAmount;
                budgetSet.Add(leftBudget);
            }

            ((List<string>)Config.Data.Labels).AddRange(Constants.Months);
            Config.Data.Datasets.Add(underSpentSet);
            Config.Data.Datasets.Add(budgetSet);
            Config.Data.Datasets.Add(overSpentSet);

            await Chart.Update();
        }

        public async Task Add()
            => await _addViewModel.Open(StatementType.Budget);

        private void InitializeBarConfig()
        {
            Config = new BarConfig
            {
                Options = new BarOptions
                {
                    Responsive = true,
                    Legend = new Legend
                    {
                        Position = Position.Top
                    },
                    Title = new OptionsTitle
                    {
                        Display = false,
                        Text = "Budget"
                    },
                    Tooltips = new Tooltips
                    {
                        Enabled = false
                    },
                    Scales = new BarScales
                    {
                        XAxes = new List<CartesianAxis>
                        {
                            new BarCategoryAxis
                            {
                                Stacked = true
                            }
                        },
                        YAxes = new List<CartesianAxis>
                        {
                            new BarLinearCartesianAxis
                            {
                                Stacked = true
                            }
                        }
                    }
                }
            };
        }
        
    }
}
