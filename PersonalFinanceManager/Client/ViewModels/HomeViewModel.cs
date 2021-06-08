using ChartJs.Blazor;
using ChartJs.Blazor.BarChart;
using ChartJs.Blazor.Common;
using ChartJs.Blazor.Common.Enums;
using ChartJs.Blazor.Util;
using PersonalFinanceManager.Client.Contracts;
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
    public class HomeViewModel : IViewModel
    {
        private readonly HttpClient _apiClient;
        private readonly CategoryManager _categoryManager;

        public HomeViewModel(HttpClient apiClient,
            CategoryManager categoryManager)
        {
            _apiClient = apiClient;
            _categoryManager = categoryManager;
        }

        public BarConfig Config { get; set; }

        public Chart Chart { get; set; }

        public IList<Expense> Expenses { get; set;} = new List<Expense>();

        public IList<IncomeModel> Incomes { get; set; } = new List<IncomeModel>();

        public event EventHandler ChangeState;
        public async Task OnInit()
        {
            Expenses.Clear();
            Incomes.Clear();

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
                        Text = "Overview"
                    }
                }
            };

            try
            {
                using (var cts = new CancellationTokenSource(Constants.ApiTimeOut))
                {
                    Expenses = await _apiClient.GetFromJsonAsync<List<Expense>>($"Expenses", cts.Token);
                    Incomes = await _apiClient.GetFromJsonAsync<List<IncomeModel>>($"Incomes", cts.Token);
                }
            }
            catch (Exception ex)
            {
                //do nothing
            }

            await GenerateBarChart();
        }

        public async Task DeleteIncome(IncomeModel income)
        {
            var result = await _apiClient.DeleteAsync($"Incomes/{income.StatementId}");

            if (result.IsSuccessStatusCode)
            {
                Incomes.Remove(income);
            }

            await OnDeleted();
        }

        public async Task DeleteExpense(Expense expense)
        {
            var result = await _apiClient.DeleteAsync($"Expenses/{expense.StatementId}");

            if (!result.IsSuccessStatusCode)
                return;

            Expenses.Remove(expense);
            await OnDeleted();
        }

        private async Task OnDeleted()
        {
            await GenerateBarChart();
            await _categoryManager.GetAllCategories();
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
                int expenseAmount = (int)Expenses.Where(e => e.DateTime.Month == i).Sum(e => e.Amount);
                expenseDataSet.Add(expenseAmount);

                int incomeAmount = (int)Incomes.Where(e => e.DateTime.Month == i).Sum(e => e.Amount);
                incomeDataSet.Add(incomeAmount);
            }

            ((List<string>)Config.Data.Labels).AddRange(Constants.Months);
            Config.Data.Datasets.Add(expenseDataSet);
            Config.Data.Datasets.Add(incomeDataSet);

            await Chart.Update();
        }


    }
}
