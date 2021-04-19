using ChartJs.Blazor.Common;
using ChartJs.Blazor.PieChart;
using PersonalFinanceManager.Client.Components;
using PersonalFinanceManager.Client.Contracts;
using PersonalFinanceManager.Client.Enums;
using PersonalFinanceManager.Client.Properties;
using PersonalFinanceManager.Client.Services;
using PersonalFinanceManager.Client.ViewModels;
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
        private readonly AddViewModel _addViewModel;
        private readonly CategoryManager _categoryManager;
        private DateTime? _dateFrom;
        private DateTime? _dateTo;
        private IList<Category> _categories;

        public StatementsBaseViewModel(AddViewModel addViewModel, CategoryManager categoryManager)
        {
            _addViewModel = addViewModel;
            _categoryManager = categoryManager;
        }

        public int ValueToAdd { get; set; }

        public IList<Category> SortedCategories { get; set; } = new List<Category>();

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

        public AddModal AddModal { get; set; }

        public float CurrentAmount => SortedCategories.Sum(c=> c.Statements.Where(s => s.DateTime < DateTo && s.DateTime > DateFrom).Sum(c => c.Amount));

        public abstract StatementType Type { get; }

        public PieConfig Config;

        public async Task Add()
        {
            await _addViewModel.Open(Type);
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
                        Text = Type == StatementType.Expense ?
                               "Your expenses" :
                               "Your incomes"
                    }
                }
            };

            _categories = _categoryManager.GetCategories(Type);

            if (_categories != null)
            {
                GeneratePie();
            }
        }

        public void GeneratePie()
        {
            Config.Data.Labels.Clear();
            Config.Data.Datasets.Clear();

            PieDataset<float> dataset = new PieDataset<float>();
            IList<string> colors = new List<string>();

            var sortedCategories = _categories.Where(c => c.Statements?.Any(s => s.DateTime < DateFrom && s.DateTime > DateTo) ?? false).ToList();

            if (sortedCategories == null || sortedCategories.Count == 0)
            {
                return;
            }

            SortedCategories = sortedCategories;

            foreach (var category in SortedCategories)
            {
                Config.Data.Labels.Add(category.Name);                
                float totalAmount = category.Statements.Where((s => s.DateTime < DateTo && s.DateTime > DateFrom)).Sum(s => s.Amount);
                dataset.Add(totalAmount);
                colors.Add(category.ColorHex);
            }

            dataset.BackgroundColor = colors.ToArray();
            Config.Data.Datasets.Add(dataset);
        }
    }
}

