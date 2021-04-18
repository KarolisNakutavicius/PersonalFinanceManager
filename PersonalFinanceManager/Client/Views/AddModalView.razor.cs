using Microsoft.AspNetCore.Components;
using PersonalFinanceManager.Client.ViewModels;
using PersonalFinanceManager.Components.ViewModels;

namespace PersonalFinanceManager.Client.Views
{
    public class AddModalViewComponent : ViewComponentBase<AddViewModel>
    {
        [Inject] public override AddViewModel ViewModel { get; set; }
    }
}
