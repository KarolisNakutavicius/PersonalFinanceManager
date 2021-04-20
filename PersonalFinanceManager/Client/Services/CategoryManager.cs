using PersonalFinanceManager.Client.Enums;
using PersonalFinanceManager.Client.ViewModels;
using PersonalFinanceManager.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;

namespace PersonalFinanceManager.Client.Services
{
    public class CategoryManager
    {
        private readonly HttpClient _apiClient;

        private SemaphoreSlim _categorySemaphore = new SemaphoreSlim(1, 1);

        private IList<Category> _expenseCategories = new List<Category>();
        private IList<Category> _incomeCategories = new List<Category>();

        public event EventHandler CategoryUpdated;

        public CategoryManager(HttpClient apiClient)
        {
            _apiClient = apiClient;

            GetAllCategories();
        }

        public async Task GetAllCategories()
        {
            await _categorySemaphore.WaitAsync();
            try
            {
                _expenseCategories = await _apiClient.GetFromJsonAsync<List<Category>>($"Expenses/Categories");
                _incomeCategories = await _apiClient.GetFromJsonAsync<List<Category>>($"Income/Categories");
            }
            catch(Exception ex)
            {

            }
            finally
            {
                _categorySemaphore.Release();
                this.CategoryUpdated?.Invoke(this, EventArgs.Empty);
            }
        }

        public IList<Category> GetExpenseCategories()
            => _expenseCategories;

        public IList<Category> GetIncomeCategories()
            => _incomeCategories;

        public async Task<IList<Category>> GetCategories(StatementType type)
        {
            await _categorySemaphore.WaitAsync();

            var categories =  type == StatementType.Expense ?
                _expenseCategories :
                _incomeCategories;

            _categorySemaphore.Release();

            return categories;
        }

    }
}
