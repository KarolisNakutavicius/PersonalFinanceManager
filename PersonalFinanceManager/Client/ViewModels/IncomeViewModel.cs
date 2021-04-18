using PersonalFinanceManager.Client.Abstract;
using PersonalFinanceManager.Client.Contracts;
using PersonalFinanceManager.Client.Properties;
using PersonalFinanceManager.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;

namespace PersonalFinanceManager.Client.ViewModels
{
    public class IncomeViewModel :StatementsBaseViewModel
    {
        public override bool IsExpensePage => false;
        public IncomeViewModel(HttpClient apiClient) : base(apiClient)
        {
        }
    }
}
