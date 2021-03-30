using Microsoft.AspNetCore.Components;
using PersonalFinanceManager.Client.ViewModels;
using PersonalFinanceManager.Components.ViewModels;

namespace PersonalFinanceManager.Client.Pages
{
    public class ExpensesManagementComponent : ViewModelComponentBase<IncomeViewModel>
    {
        [Inject] public override IncomeViewModel ViewModel { get; set; }
    }
}
