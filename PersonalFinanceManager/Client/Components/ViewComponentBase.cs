using Microsoft.AspNetCore.Components;
using PersonalFinanceManager.Client.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PersonalFinanceManager.Components.ViewModels
{
    public abstract class ViewComponentBase<T> : ComponentBase where T : IViewModel
    {
        protected override async Task OnInitializedAsync()
        {
            await ViewModel.OnInit();
            ViewModel.ChangeState += (s, e) => StateHasChanged();
        }

        public abstract T ViewModel { get; set; }
    }
}
