using Microsoft.AspNetCore.Components;
using PersonalFinanceManager.Client.Contracts;
using PersonalFinanceManager.Shared.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PersonalFinanceManager.Client.ViewModels
{
    public class RegisterViewModel : IViewModel
    {
        private readonly IAuthService _authService;
        private readonly NavigationManager _navigationManager;

        public RegisterViewModel(IAuthService authService,
            NavigationManager navigationManager)
        {
            _authService = authService;
            _navigationManager = navigationManager;
        }
        public bool ShowErrors { get; set; }
        public IEnumerable<string> Errors { get; set; } = new List<string>();

        public RegisterModel RegisterModel = new RegisterModel();

        public async Task OnInit()
        {
            RegisterModel = new RegisterModel();
            ShowErrors = false;
        }

        public async Task HandleRegistration()
        {
            ShowErrors = false;

            var result = await _authService.Register(RegisterModel);

            if (result.Success)
            {
                _navigationManager.NavigateTo("/");
                return;
            }

            Errors = result.Errors;
            ShowErrors = true;
        }
    }
}
