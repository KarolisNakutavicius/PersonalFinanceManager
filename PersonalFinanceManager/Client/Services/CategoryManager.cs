using PersonalFinanceManager.Client.Enums;
using PersonalFinanceManager.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace PersonalFinanceManager.Client.Services
{
    public class CategoryManager
    {
        private readonly HttpClient _apiClient;

        private IList<Category> _expenseCategories = new List<Category>();
        private IList<Category> _incomeCategories = new List<Category>();

        public CategoryManager(HttpClient apiClient)
        {
            _apiClient = apiClient;
            Task.Run(() => GetAllCategories());
        }

        private async Task GetAllCategories()
        {
            _expenseCategories = await _apiClient.GetFromJsonAsync<List<Category>>($"Expenses/Categories");
            _incomeCategories = await _apiClient.GetFromJsonAsync<List<Category>>($"Income/Categories");
        }

        public IList<Category> GetExpenseCategories()
            => _expenseCategories;

        public IList<Category> GetIncomeCategories()
            => _incomeCategories;

        public IList<Category> GetCategories(StatementType type)
        {
            return type == StatementType.Expense ?
                _expenseCategories :
                _incomeCategories;
        }

    }
}
