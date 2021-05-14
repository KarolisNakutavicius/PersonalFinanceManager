using ChartJs.Blazor;
using ChartJs.Blazor.BarChart;
using ChartJs.Blazor.Common;
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
        private IList<Category> _expenseCategories;

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
                        Display = true,
                        Text = "Budget"
                    }
                }
            };

            try
            {
                using (var cts = new CancellationTokenSource(Constants.ApiTimeOut))
                {
                    _budgets = await _apiClient.GetFromJsonAsync<List<Budget>>($"all/Budgets", cts.Token);
                }

                _expenseCategories = await _categoryManager.GetExpenseCategories();
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

            IDataset<int> expenseDataSet = new BarDataset<int>()
            {
                Label = "Expenses",
                BackgroundColor = ColorUtil.FromDrawingColor(Color.FromArgb(128, Color.Red)),
                BorderColor = ColorUtil.FromDrawingColor(Color.Red),
                BorderWidth = 1
            };

            IDataset<int> incomeDataSet = new BarDataset<int>()
            {
                Label = "Incomes",
                BackgroundColor = ColorUtil.FromDrawingColor(Color.FromArgb(128, Color.Green)),
                BorderColor = ColorUtil.FromDrawingColor(Color.Green),
                BorderWidth = 1
            };

            for (int i = 0; i < Constants.MonthsInYear; i++)
            {
                //int expenseAmount = (int)_expenses.Where(e => e.DateTime.Month == i).Sum(e => e.Amount);
                //expenseDataSet.Add(expenseAmount);

                //int incomeAmount = (int)_incomes.Where(e => e.DateTime.Month == i).Sum(e => e.Amount);
                //incomeDataSet.Add(incomeAmount);
            }

            ((List<string>)Config.Data.Labels).AddRange(Constants.Months);
            Config.Data.Datasets.Add(expenseDataSet);
            Config.Data.Datasets.Add(incomeDataSet);

            await Chart.Update();
        }

        public async Task Add()
        {
            await _addViewModel.Open(StatementType.Budget);
        }
    }
}
