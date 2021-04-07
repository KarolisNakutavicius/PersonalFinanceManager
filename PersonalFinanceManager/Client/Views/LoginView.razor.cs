using Microsoft.AspNetCore.Components;
using PersonalFinanceManager.Client.ViewModels;
using PersonalFinanceManager.Components.ViewModels;


namespace PersonalFinanceManager.Client.Views
{
    public class LoginViewComponent : ViewComponentBase<LoginViewModel>
    {
        [Inject] public override LoginViewModel ViewModel { get; set; }
    }
}
