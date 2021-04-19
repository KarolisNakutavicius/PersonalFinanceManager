using ChartJs.Blazor;
using ChartJs.Blazor.Common;
using ChartJs.Blazor.PieChart;
using ChartJs.Blazor.Util;
using Microsoft.AspNetCore.Components;
using PersonalFinanceManager.Client.Abstract;
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
    public class ExpensesViewModel : StatementsBaseViewModel
    {
        public override StatementType Type => StatementType.Expense;

        public ExpensesViewModel(HttpClient httpClient, AddViewModel addViewModel) : base(httpClient, addViewModel)
        {
        }
    }
}
