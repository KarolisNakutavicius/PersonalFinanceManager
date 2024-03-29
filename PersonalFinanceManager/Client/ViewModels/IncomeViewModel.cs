﻿using PersonalFinanceManager.Client.Abstract;
using PersonalFinanceManager.Client.Contracts;
using PersonalFinanceManager.Client.Enums;
using PersonalFinanceManager.Client.Properties;
using PersonalFinanceManager.Client.Services;
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
    public class IncomeViewModel : StatementsBaseViewModel
    {
        public override StatementType Type => StatementType.Income;
        public IncomeViewModel(CategoryManager categoryManager,
            AddViewModel addViewModel,
            EditCategoriesViewModel editCategoriesViewModel) : base(addViewModel, categoryManager, editCategoriesViewModel)
        {
        }
    }
}
