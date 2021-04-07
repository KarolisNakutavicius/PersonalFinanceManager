using Microsoft.AspNetCore.Components;
using PersonalFinanceManager.Client.ViewModels;
using PersonalFinanceManager.Components.ViewModels;

namespace PersonalFinanceManager.Client.Views
{
    public class RegisterViewComponent : ViewComponentBase<RegisterViewModel>
    {
        [Inject] public override RegisterViewModel ViewModel { get; set; }
    }

}
