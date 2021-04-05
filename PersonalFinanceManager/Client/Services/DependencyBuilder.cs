using Microsoft.Extensions.DependencyInjection;
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
            services.AddScoped<IncomeViewModel>();
            services.AddScoped<ExpensesViewModel>();
        }
    }
}
