﻿using ChartJs.Blazor;
using ChartJs.Blazor.BarChart;
using ChartJs.Blazor.BarChart.Axes;
using ChartJs.Blazor.Common;
using ChartJs.Blazor.Common.Axes;
using ChartJs.Blazor.Common.Enums;
using ChartJs.Blazor.Util;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using PersonalFinanceManager.Client.Contracts;
using PersonalFinanceManager.Client.Enums;
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
    public class BudgetsViewModel : IViewModel
    {
        private readonly HttpClient _apiClient;
        private readonly AddViewModel _addViewModel;
        private readonly EditBudgetViewModel _editBudgetViewModel;

        private string _selectedBudgetName;
        private List<Statement> _expenses = new List<Statement>();

        public BudgetsViewModel(HttpClient apiClient,
            AddViewModel addViewModel,
            EditBudgetViewModel editBudgetViewModel)
        {
            _apiClient = apiClient;
            _addViewModel = addViewModel;
            _editBudgetViewModel = editBudgetViewModel;

            _addViewModel.OnBudgetAdded = OnBudgetAdded;
            _editBudgetViewModel.BudgetModified += (s, e) => _ = RetrieveBudgets();
        }

        public BarConfig Config { get; set; } 

        public Chart Chart { get; set; }
        public List<Budget> Budgets { get; set; } = new List<Budget>();

        public event EventHandler ChangeState;

        public string SelectedBudgetName
        {
            get => _selectedBudgetName;
            set
            {
                _selectedBudgetName = value;
                _ = GetSelectedExpenses();
            }
        }

        public string Title { get; set; }

        public async Task OnInit()
        {
            InitBarConfig();
            await RetrieveBudgets();
        }

        public async Task RetrieveBudgets()
        {
            Budgets.Clear();
          
            try
            {
                using (var cts = new CancellationTokenSource(Constants.ApiTimeOut))
                {
                    Budgets = await _apiClient.GetFromJsonAsync<List<Budget>>($"Budgets/all", cts.Token);
                }
            }
            catch (Exception ex)
            {
                //do nothing
            }

            await GetSelectedExpenses();
        }

        

        private async Task GetSelectedExpenses()
        {
            if (Budgets.Count == 0)
            {
                Title = "You don't have any budgets";
                return;
            }

            Title = "Your selected budgets";

            var selectedBudget = Budgets.FirstOrDefault(b => b.Name == SelectedBudgetName);

            if (selectedBudget == null)
            {
                selectedBudget = Budgets.First();
                SelectedBudgetName = selectedBudget.Name;
            }

            _expenses.Clear();

            foreach (var category in selectedBudget.Categories)
            {
                _expenses.AddRange(category.Statements.ToList());
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
                int budget = Budgets.Where(b => b.Name == SelectedBudgetName).FirstOrDefault().Amount;

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

            this.ChangeState?.Invoke(this, EventArgs.Empty);
            await Chart.Update();
        }

        public async Task Add()
            => await _addViewModel.Open(StatementType.Budget);

        public async Task EditBudgets()
            => await _editBudgetViewModel.Open(); 

        private void OnBudgetAdded(Budget newBudget)
        {            
            Budgets.Insert(0, newBudget);
            Title = "Your selected budgets";
            this.ChangeState.Invoke(this, EventArgs.Empty);
            _ = GetSelectedExpenses();
        }

        private void InitBarConfig()
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
