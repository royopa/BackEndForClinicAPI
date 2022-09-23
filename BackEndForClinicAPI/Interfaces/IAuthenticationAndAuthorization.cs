using BackEndForClinicAPI.Models;

namespace BackEndForClinicAPI.Interfaces
{
    public interface IAuthenticationAndAuthorization
    {
        UserModel GetCurrentUser();
    }
}
