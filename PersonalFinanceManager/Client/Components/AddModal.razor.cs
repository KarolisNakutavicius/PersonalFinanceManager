using Microsoft.AspNetCore.Components;
using PersonalFinanceManager.Client.ViewModels;
using PersonalFinanceManager.Components.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PersonalFinanceManager.Client.Components
{
    public class AddModalComponent : ViewComponentBase<AddViewModel>
    {
        [Inject]
        public override AddViewModel ViewModel { get; set; }
    }
}
