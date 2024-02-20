using DemoCookieAuthSystem.DTOs;
using DemoCookieAuthSystem.Responses;

namespace DemoCookieAuthSystem.Services
{
    public interface IAccount
    {
        Task<ServiceResponse> RegisterAsync(RegisterDto model);
        Task<ServiceResponse> LoginAsync(LoginDto model);
    }
}
