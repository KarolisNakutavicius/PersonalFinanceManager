﻿using Microsoft.AspNetCore.Components.Authorization;
using PersonalFinanceManager.Client.Authentication;
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
        private readonly AuthenticationStateProvider _apiAuthenticationStateProvider;

        private SemaphoreSlim _categorySemaphore = new SemaphoreSlim(1, 1);

        private IList<Category> _expenseCategories = new List<Category>();
        private IList<Category> _incomeCategories = new List<Category>();

        public event EventHandler CategoryUpdated;

        public CategoryManager(HttpClient apiClient,
            AuthenticationStateProvider apiAuthenticationStateProvider)
        {
            _apiClient = apiClient;
            _apiAuthenticationStateProvider = apiAuthenticationStateProvider;

            _apiAuthenticationStateProvider.AuthenticationStateChanged += OnAuthenticationChanged;

            _ = GetAllCategories();
        }

        public async Task GetAllCategories()
        {
            try
            {
                await _categorySemaphore.WaitAsync();

                _expenseCategories = await _apiClient.GetFromJsonAsync<List<Category>>($"Expenses/Categories");
                _incomeCategories = await _apiClient.GetFromJsonAsync<List<Category>>($"Incomes/Categories");
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

        public async Task<List<Category>> GetExpenseCategories()
        {
            if (_expenseCategories == null)
            {
                await GetAllCategories();
            }

            return new List<Category>(_expenseCategories);
        }

        public IList<Category> GetIncomeCategories()
            => _incomeCategories;

        public async Task<IList<Category>> GetCategories(StatementType type)
        {
            await _categorySemaphore.WaitAsync();

            var categories = type == StatementType.Income ?
                _incomeCategories :
                _expenseCategories;

            _categorySemaphore.Release();

            return new List<Category>(categories);
        }

        private async void OnAuthenticationChanged(Task<AuthenticationState> task)
        {
            var authState = await task;

            if (authState.User.Identity?.IsAuthenticated == false)
            {
                _expenseCategories.Clear();
                _incomeCategories.Clear();
                return;
            }

            await GetAllCategories();
        }
    }
}
