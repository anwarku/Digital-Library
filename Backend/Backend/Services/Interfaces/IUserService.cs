using Backend.DTOs;
using Backend.Models;
using Microsoft.AspNetCore.Identity;

namespace Backend.Services.Interfaces
{
    public interface IUserService
    {
        UserHasLoggedDto GetUserById(Guid id);
        string UserLogin(UserLoginDto userLoginDto);
        void UserRegistration(UserRegisterDto userRegisterDto);
        bool UserExistByUsername(string username);
    }
}
