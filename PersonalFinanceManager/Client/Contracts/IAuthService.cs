using PersonalFinanceManager.Shared.Models;
using PersonalFinanceManager.Shared.Responses;
using System.Threading.Tasks;

namespace PersonalFinanceManager.Client.Contracts
{
    public interface IAuthService
    {
        Task<LoginResult> Login(LoginModel loginModel);
        Task Logout();
        Task<RegisterResult> Register(RegisterModel registerModel);
    }
}
