using Microsoft.AspNetCore.Components;
using PersonalFinanceManager.Client.ViewModels;
using PersonalFinanceManager.Components.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PersonalFinanceManager.Client.Views
{
    public class EditCategoriesComponent : ViewComponentBase<EditCategoriesViewModel>
    {

        [Inject]
        public override EditCategoriesViewModel ViewModel { get; set; }
    }
}
