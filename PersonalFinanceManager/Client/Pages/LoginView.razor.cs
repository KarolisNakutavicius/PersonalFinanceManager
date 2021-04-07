using Microsoft.AspNetCore.Components;
using PersonalFinanceManager.Client.ViewModels;
using PersonalFinanceManager.Components.ViewModels;


namespace PersonalFinanceManager.Client.Pages
{
    public class LoginViewComponent : ViewModelComponentBase<LoginViewModel>
    {
        [Inject] public override LoginViewModel ViewModel { get; set; }
    }
}
