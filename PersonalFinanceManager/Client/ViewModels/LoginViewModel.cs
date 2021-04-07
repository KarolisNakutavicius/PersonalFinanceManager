using Microsoft.AspNetCore.Components;
using PersonalFinanceManager.Client.Contracts;
using PersonalFinanceManager.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PersonalFinanceManager.Client.ViewModels
{
    public class LoginViewModel : IViewModel
    {
        private readonly IAuthService _authService;
        private readonly NavigationManager _navigationManager;

        public LoginViewModel(IAuthService authService,
            NavigationManager navigationManager)
        {
            _authService = authService;
            _navigationManager = navigationManager;
        }

        public LoginModel LoginModel = new LoginModel();

        public bool ShowErrors;
        public string Error = "";

        public async Task OnInit()
        {

        }

        public async Task HandleLogin()
        {
            ShowErrors = false;

            var result = await _authService.Login(LoginModel);

            if (result.Successful)
            {
                _navigationManager.NavigateTo("/");
            }
            else
            {
                Error = result.Error;
                ShowErrors = true;
            }
        }
    }
}
