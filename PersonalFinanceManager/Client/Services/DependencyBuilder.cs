using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.DependencyInjection;
using PersonalFinanceManager.Client.Authentication;
using PersonalFinanceManager.Client.Contracts;
using PersonalFinanceManager.Client.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace PersonalFinanceManager.Client.Services
{
    public static class DependencyBuilder
    {
        public static void Build(this IServiceCollection services)
        {            
            services.AddBlazoredLocalStorage();
            services.AddAuthorizationCore();

            services.AddScoped<AuthenticationStateProvider, ApiAuthenticationStateProvider>();
            services.AddScoped<IAuthService, AuthService>();

            services.AddScoped<HomeViewModel>();
            services.AddScoped<CategoryManager>();
            services.AddScoped<IncomeViewModel>();
            services.AddScoped<LoginViewModel>();
            services.AddScoped<RegisterViewModel>();
            services.AddScoped<ExpensesViewModel>();
            services.AddScoped<BudgetsViewModel>();
            services.AddScoped<AddViewModel>();
            services.AddScoped<ImportViewModel>();
            services.AddScoped<EditCategoriesViewModel>();
            services.AddScoped<EditBudgetViewModel>();
        }
    }
}
