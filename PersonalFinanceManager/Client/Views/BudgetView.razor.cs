using Microsoft.AspNetCore.Components;
using PersonalFinanceManager.Client.ViewModels;
using PersonalFinanceManager.Components.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PersonalFinanceManager.Client.Views
{
    public class BudgetComponent : ViewComponentBase<BudgetsViewModel>
    {
        [Inject] public override BudgetsViewModel ViewModel { get; set; }
    }
}
