using Microsoft.AspNetCore.Components;
using PersonalFinanceManager.Client.ViewModels;
using PersonalFinanceManager.Components.ViewModels;

namespace PersonalFinanceManager.Client.Pages
{
    public class RegisterViewComponent : ViewModelComponentBase<RegisterViewModel>
    {
        [Inject] public override RegisterViewModel ViewModel { get; set; }
    }

}
