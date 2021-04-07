using Microsoft.AspNetCore.Components;
using PersonalFinanceManager.Client.ViewModels;
using PersonalFinanceManager.Components.ViewModels;

namespace PersonalFinanceManager.Client.Views
{
    public class ExpensesViewComponent : ViewComponentBase<ExpensesViewModel>
    {
        [Inject] public override ExpensesViewModel ViewModel { get; set; }
    }
}
