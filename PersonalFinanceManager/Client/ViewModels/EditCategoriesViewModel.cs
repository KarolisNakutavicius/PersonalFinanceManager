using PersonalFinanceManager.Client.Contracts;
using PersonalFinanceManager.Client.Enums;
using PersonalFinanceManager.Client.Services;
using PersonalFinanceManager.Shared.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace PersonalFinanceManager.Client.ViewModels
{
    public class EditCategoriesViewModel : IViewModel
    {
        private readonly CategoryManager _categoryManager;
        private readonly HttpClient _httpClient;

        public EditCategoriesViewModel(CategoryManager categoryManager, HttpClient httpClient)
        {
            _categoryManager = categoryManager;
            _httpClient = httpClient;
        }

        public string ErrorMessage { get; set; } = string.Empty;

        public event EventHandler ChangeState;

        public event EventHandler OpenRequested;

        public IList<Category> Categories { get; set; } = new List<Category>();

        public async Task OnInit()
        { }

        public async Task Open(StatementType type)
        {
            ErrorMessage = string.Empty;

            Categories = await _categoryManager.GetCategories(type);
            this.OpenRequested?.Invoke(this, EventArgs.Empty);
        }

        public async Task DeleteCategory(Category categoryToDelete)
        {
            ErrorMessage = string.Empty;

            var result = await _httpClient.DeleteAsync($"Categories/{categoryToDelete.CategoryId}");

            if (!result.IsSuccessStatusCode)
            {
                ErrorMessage = "Could not delete a category. There was a problem connecting to api service.";
                return;
            }

            Categories.Remove(categoryToDelete);
            await _categoryManager.GetAllCategories();
        }

        public async Task RenameCategory(Category categoryToRename)
        {
            ErrorMessage = string.Empty;

            var result = await _httpClient.PutAsJsonAsync<Category>($"Categories/{categoryToRename.CategoryId}", categoryToRename);

            if (!result.IsSuccessStatusCode)
            {
                ErrorMessage = "Could not update a category. There was a problem connecting to api service.";
                return;
            }

            await _categoryManager.GetAllCategories();
        }
    }
}
