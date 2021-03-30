using Microsoft.AspNetCore.Components;
using PersonalFinanceManager.Client.ViewModels;
using PersonalFinanceManager.Components.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PersonalFinanceManager.Client.Pages
{
    public class IncomeManagementComponent : ViewModelComponentBase<IncomeViewModel>
    {
        [Inject] public override IncomeViewModel ViewModel { get; set; }
    }
}
