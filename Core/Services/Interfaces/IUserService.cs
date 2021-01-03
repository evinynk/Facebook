using Common.Dtos;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services.Interfaces
{
    public interface IUserService
    {
       Task<IdentityResult> CreateUser(UserDto userDto);
       Task<bool> SignIn(UserDto user);
       UserDto GetUserByName(string name);
        UserDto GetUserByFriendName(string name);
        void LoadImage(string email, string path);
        void AddConnection(string email, string emailUser);
        void DeleteConnection(string email, string emailUser);
        bool IsFollow(string email, string name);
    }
}
