using Microsoft.AspNetCore.Components;
using PersonalFinanceManager.Client.ViewModels;
using PersonalFinanceManager.Components.ViewModels;

namespace PersonalFinanceManager.Client.Views
{
    public class ExpensesViewComponent : ViewComponentBase<IncomeViewModel>
    {
        [Inject] public override IncomeViewModel ViewModel { get; set; }
    }
}
