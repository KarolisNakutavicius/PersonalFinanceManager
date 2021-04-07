using Microsoft.AspNetCore.Components;
using PersonalFinanceManager.Client.Contracts;
using PersonalFinanceManager.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PersonalFinanceManager.Client.ViewModels
{
    public class RegisterViewModel : IViewModel
    {
        private readonly IAuthService _authService;
        private readonly NavigationManager _navigationManager;

        public bool ShowErrors;
        public IEnumerable<string> Errors;

        public RegisterViewModel(IAuthService authService,
            NavigationManager navigationManager)
        {
            _authService = authService;
            _navigationManager = navigationManager;
        }

        public RegisterModel RegisterModel = new RegisterModel();

        public async Task OnInit()
        {

        }

        public async Task HandleRegistration()
        {
            ShowErrors = false;

            var result = await _authService.Register(RegisterModel);

            if (result.Successful)
            {
                _navigationManager.NavigateTo("/login");
            }
            else
            {
                Errors = result.Errors;
                ShowErrors = true;
            }
        }
    }
}
