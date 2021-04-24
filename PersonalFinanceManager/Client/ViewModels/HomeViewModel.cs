using ChartJs.Blazor;
using ChartJs.Blazor.BarChart;
using ChartJs.Blazor.Common;
using ChartJs.Blazor.Common.Enums;
using ChartJs.Blazor.Util;
using PersonalFinanceManager.Client.Contracts;
using PersonalFinanceManager.Client.Properties;
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

        private const int MonthsInYear = 12;
        private static IReadOnlyList<string> Months { get; } = new ReadOnlyCollection<string>(new[]
        {
            "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"
        });

        public HomeViewModel(HttpClient apiClient)
        {
            _apiClient = apiClient;
        }

        public BarConfig Config { get; set; }

        public Chart Chart { get; set; }

        private IList<Expense> _expenses = new List<Expense>();

        private IList<IncomeModel> _incomes = new List<IncomeModel>();

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
                        Text = "Overview"
                    }
                }
            };

            try
            {
                using (var cts = new CancellationTokenSource(Constants.ApiTimeOut))
                {
                    _expenses = await _apiClient.GetFromJsonAsync<List<Expense>>($"Expenses", cts.Token);
                    _incomes = await _apiClient.GetFromJsonAsync<List<IncomeModel>>($"Incomes", cts.Token);
                }
            }
            catch (Exception ex)
            {
                //do nothing
            }


            //_expenses = new List<Expense>
            //{
            //    new Expense { Amount = 200, DateTime = DateTime.Now.AddMonths(-1)},
            //     new Expense { Amount = 300, DateTime = DateTime.Now.AddMonths(-5)},
            //      new Expense { Amount = 200, DateTime = DateTime.Now},
            //       new Expense { Amount = 250, DateTime = DateTime.Now.AddMonths(-3)},
            //        new Expense { Amount = 400, DateTime = DateTime.Now.AddMonths(-2)},
            //};

            //_incomes = new List<IncomeModel>
            //{
            //    new IncomeModel { Amount = 200, DateTime = DateTime.Now.AddMonths(-1)},
            //     new IncomeModel { Amount = 300, DateTime = DateTime.Now.AddMonths(-5)},
            //      new IncomeModel { Amount = 200, DateTime = DateTime.Now},
            //       new IncomeModel { Amount = 250, DateTime = DateTime.Now.AddMonths(-3)},
            //        new IncomeModel { Amount = 400, DateTime = DateTime.Now.AddMonths(-2)},
            //};

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

            for (int i = 0; i < MonthsInYear; i++)
            {
                int expenseAmount = (int)_expenses.Where(e => e.DateTime.Month == i).Sum(e => e.Amount);
                expenseDataSet.Add(expenseAmount);

                int incomeAmount = (int)_incomes.Where(e => e.DateTime.Month == i).Sum(e => e.Amount);
                incomeDataSet.Add(incomeAmount);
            }

            ((List<string>)Config.Data.Labels).AddRange(Months.Take(MonthsInYear));
            Config.Data.Datasets.Add(expenseDataSet);
            Config.Data.Datasets.Add(incomeDataSet);

            await Chart.Update();
        }

    }
}
