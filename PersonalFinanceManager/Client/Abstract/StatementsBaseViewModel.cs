using ChartJs.Blazor;
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
        private readonly EditCategoriesViewModel _editCategoriesViewModel;
        private readonly CategoryManager _categoryManager;
        private DateTime? _dateFrom;
        private DateTime? _dateTo;
        private IList<Category> _sortedCategories;

        public StatementsBaseViewModel(AddViewModel addViewModel,
            CategoryManager categoryManager,
            EditCategoriesViewModel editCategoriesViewModel)
        {
            _addViewModel = addViewModel;
            _categoryManager = categoryManager;
            _editCategoriesViewModel = editCategoriesViewModel;

            _categoryManager.CategoryUpdated += (s, e) => _ = GeneratePie();
        }

        public string Title { get; set; }

        public Chart Chart { get; set; }

        public IList<Category> SortedCategories
        {
            get => _sortedCategories;
            set
            {
                _sortedCategories = value;

                UpdateTitle();
            }
        }
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

        public event EventHandler ChangeState;
        public async Task OnInit()
        {
            Config = new PieConfig
            {
                Options = new PieOptions
                {
                    Responsive = true,
                    Title = new OptionsTitle
                    {
                        Display = false
                    }
                }
            };

            await GeneratePie();
        }

        public async Task GeneratePie()
        {
            try
            {
                var categories = await _categoryManager.GetCategories(Type);
                Config.Data.Labels.Clear();
                Config.Data.Datasets.Clear();

                PieDataset<float> dataset = new PieDataset<float>();
                IList<string> colors = new List<string>();

                SortedCategories = categories.Where(c => c.Statements?.Any(s => s.DateTime > DateFrom && s.DateTime < DateTo) ?? false).ToList();

                if (SortedCategories == null || SortedCategories.Count == 0)
                {
                    return;
                }

                foreach (var category in SortedCategories)
                {
                    Config.Data.Labels.Add(category.Name);
                    float totalAmount = category.Statements.Where((s => s.DateTime < DateTo && s.DateTime > DateFrom)).Sum(s => s.Amount);
                    dataset.Add(totalAmount);
                    colors.Add(category.ColorHex);
                }

                dataset.BackgroundColor = colors.ToArray();
                Config.Data.Datasets.Add(dataset);
                UpdateTitle();
                await Chart.Update();
            }
            catch(Exception ex)
            {
                //do nothin
            }         
        }

        public async Task EditCategories()
            => await _editCategoriesViewModel.Open(Type);

        private void UpdateTitle()
        {
            var amount = SortedCategories.Sum(c => c.Statements.Where(s => s.DateTime < DateTo && s.DateTime > DateFrom).Sum(c => c.Amount));
            if (amount == 0)
            {
                Title = $"There are no {Type.GetDescription()} in current time frame";
                return;
            }
            Title = $"Your total {Type.GetDescription()} : {amount} €";

            this.ChangeState.Invoke(this, EventArgs.Empty);
        }

    }
}

