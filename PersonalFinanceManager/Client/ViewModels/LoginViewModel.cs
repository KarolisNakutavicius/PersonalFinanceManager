using Microsoft.AspNetCore.Components;
using PersonalFinanceManager.Client.Contracts;
using PersonalFinanceManager.Shared.Models;
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

        public LoginModel LoginModel { get; set; }

        public bool ShowErrors { get; set; }

        public string Error { get; set; }

        public async Task OnInit()
        {
            LoginModel = new LoginModel();
            ShowErrors = false;
            Error = string.Empty;
        }

        public async Task HandleLogin()
        {
            ShowErrors = false;

            var result = await _authService.Login(LoginModel);

            if (result.Successful)
            {
                _navigationManager.NavigateTo("/");
                return;
            }

            Error = result.Error;
            ShowErrors = true;

        }
    }
}
