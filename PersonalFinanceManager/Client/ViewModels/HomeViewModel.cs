using ChartJs.Blazor;
using ChartJs.Blazor.BarChart;
using ChartJs.Blazor.Common;
using ChartJs.Blazor.Common.Enums;
using ChartJs.Blazor.Util;
using PersonalFinanceManager.Client.Contracts;
using PersonalFinanceManager.Shared.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;

namespace PersonalFinanceManager.Client.ViewModels
{
    public class HomeViewModel : IViewModel
    {
        private const int MonthsInYear = 12;

        public BarConfig Config { get; set; } = new BarConfig
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
        public Chart Chart { get; set; }


        private IList<Expense> _expenses;

        private IList<IncomeModel> _incomes;

        public async Task OnInit()
        {
            _expenses = new List<Expense>
            {
                new Expense { Amount = 200, DateTime = DateTime.Now.AddMonths(-1)},
                 new Expense { Amount = 300, DateTime = DateTime.Now.AddMonths(-5)},
                  new Expense { Amount = 200, DateTime = DateTime.Now},
                   new Expense { Amount = 250, DateTime = DateTime.Now.AddMonths(-3)},
                    new Expense { Amount = 400, DateTime = DateTime.Now.AddMonths(-2)},
            };

            _incomes = new List<IncomeModel>
            {
                new IncomeModel { Amount = 200, DateTime = DateTime.Now.AddMonths(-1)},
                 new IncomeModel { Amount = 300, DateTime = DateTime.Now.AddMonths(-5)},
                  new IncomeModel { Amount = 200, DateTime = DateTime.Now},
                   new IncomeModel { Amount = 250, DateTime = DateTime.Now.AddMonths(-3)},
                    new IncomeModel { Amount = 400, DateTime = DateTime.Now.AddMonths(-2)},
            };

            GenerateBarChart();
        }

        private void GenerateBarChart()
        {
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
                int expenseAmount = -(int)_expenses.Where(e => e.DateTime.Month == i).Sum(e => e.Amount);
                expenseDataSet.Add(expenseAmount);

                int incomeAmount = (int)_incomes.Where(e => e.DateTime.Month == i).Sum(e => e.Amount);
                incomeDataSet.Add(incomeAmount);
            }

            ((List<string>)Config.Data.Labels).AddRange(Months.Take(MonthsInYear));
            Config.Data.Datasets.Add(expenseDataSet);
            Config.Data.Datasets.Add(incomeDataSet);
        }

        public static IReadOnlyList<string> Months { get; } = new ReadOnlyCollection<string>(new[]
{
            "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"
        });

    }
}
