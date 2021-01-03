using Common.Dtos;
using Core.Services.Interfaces;
using Domain.Context;
using Domain.Entities.Concrete;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services.Concrete
{
    public class UserService:IUserService
    {
        private FacebookDbContext context;
        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;
        public UserService(FacebookDbContext fbContext, UserManager<User> _userManager, SignInManager<User> _signInManager)
        {
            context = fbContext;
            userManager = _userManager;
            signInManager = _signInManager;
        }
        public async Task<IdentityResult> CreateUser(UserDto userDto)
        {
            User user = new User();
            user.Email = userDto.Email;
            user.UserName = userDto.Email;
            user.Name = userDto.Name;
            user.Surname = userDto.Surname;
            user.CreatedDate = DateTime.Now;
            user.PasswordHash = userDto.Password;
            user.IsActive = false;
            IdentityResult result = await userManager.CreateAsync(user, user.PasswordHash);
            //bool check= result.Succeeded;
            
            //return check;
            if(result.Succeeded)
            {
                return null;
            }
            return result;

        }
        public async Task<bool> SignIn(UserDto user)
        {
            var identityResult = await signInManager.PasswordSignInAsync(user.Email, user.Password, true, false);
            var identityResultsuccess = identityResult.Succeeded;

            return identityResultsuccess;
        }
        public UserDto GetUserByName(string name)
        {
            UserDto user = context.Users.Where(x => x.Email == name).Select(x => new UserDto
            {
                Id=x.Id,
                Name = x.Name,
                Surname = x.Surname,
                Email = x.Email,
                CreatedDate = x.CreatedDate,
                IsActive = x.IsActive,
                Password = x.PasswordHash,
                BackgroundPhoto=x.BackGroundPhoto,
                ProfilePhoto=x.ProfilePhoto
                
            }).FirstOrDefault();
           

            return user;
        }
        public UserDto GetUserByFriendName(string name)
        {
            UserDto user = context.Users.Where(x => x.Name == name).Select(x => new UserDto
            {
                Id = x.Id,
                Name = x.Name,
                Surname = x.Surname,
                Email = x.Email,
                CreatedDate = x.CreatedDate,
                IsActive = x.IsActive,
                Password = x.PasswordHash

            }).FirstOrDefault();

            return user;
        }
        //profil resmi yüklerken
        public void LoadImage(string email,string path)
        {
            User user = context.Users.Where(x => x.Email == email).FirstOrDefault();
            user.ProfilePhoto = path;
            context.Users.Update(user);
            context.SaveChanges();
        }

        public void AddConnection(string email, string emailUser)
        {

            UserDto friend = GetUserByName(email);
            UserDto user = GetUserByName(emailUser);

            var connection = context.Connections.FirstOrDefault(x => x.UserId == user.Id && x.FriendId == friend.Id && x.IsPending == false && x.isDeleted == true);
            if (connection != null)
            {
                connection.IsPending = true;
                connection.isDeleted = false;
                context.Connections.Update(connection);
            }
            else
            {
                Connection newconnection = new Connection();
                newconnection.isDeleted = true;
                newconnection.IsPending = false;
                newconnection.UserId = user.Id;
                newconnection.FriendId = friend.Id;
                context.Connections.Add(newconnection);
            }
            User me = context.Users.Find(user.Id);
            if (me != null)
            {
                me.FriendsCount += 1;
                context.Users.Update(me);
            }
            User frnd = context.Users.Find(friend.Id);
            if (frnd != null)
            {
                frnd.FriendsCount += 1;
                context.Users.Update(frnd);
            }
            //user.isFollow = context.Connections.Any(y => y.FriendId == friend.Id && y.UserId == user.Id && y.IsPending == false && y.isDeleted == true);

            context.SaveChanges();
            //Connection newconnection = new Connection();
            //UserDto friend = GetUserByName(email);   //takipçi  follower    
            //UserDto user = GetUserByName(emailUser); //takip eden, şu an ki kullanıcı followee

            //var connection = context.Connections.FirstOrDefault(x => x.UserId == user.Id && x.FriendId == friend.Id && x.IsPending == false && x.isDeleted == true);
            //if (connection != null)
            //{
            //    connection.IsPending = true;
            //    connection.isDeleted = false;
            //    context.Connections.Update(connection);
            //}
            //else
            //{

            //    connection = new Connection()
            //    {
            //        UserId = user.Id,
            //        FriendId = friend.Id,
            //        IsPending = true,
            //        isDeleted = false
            //    };
            //    context.Add(connection);
            //}

            //User receiver = context.Users.Find(friend.Id); //alan --friend
            //if (receiver != null)
            //{
            //    receiver.FriendsCount += 1;
            //    context.Users.Update(receiver);
            //}


            //User sender = context.Users.Find(user.Id);//gönderen -- ben
            //if (sender != null)
            //{
            //    sender.FriendsCount += 1;
            //    context.Users.Update(sender);
            //}


            ////_user.isFollowing = _context.Connections.Any(y => y.FolloweeId == followee.UserId && y.FollowingId == follower.UserId && y.IsActive == true && y.IsDeleted == false);

            //context.SaveChanges();

        }


        public void DeleteConnection(string email, string emailUser)
        {
            //Connection newconnection = new Connection();
            //UserDto friend = GetUserByName(email);   //takipçi  follower    
            //UserDto user = GetUserByName(emailUser); //takip eden, şu an ki kullanıcı followee

            //var connection = context.Connections.FirstOrDefault(x => x.UserId == user.Id && x.FriendId == friend.Id && x.IsPending == true && x.isDeleted == false);
            //if (connection != null)
            //{
            //    connection.IsPending = false;
            //    connection.isDeleted = true;
            //    context.Connections.Update(connection);
            //}
            //else
            //{

            //    connection = new Connection()
            //    {
            //        IsPending = true,
            //        isDeleted = false,
            //        UserId = user.Id,
            //        FriendId = friend.Id

            //    };
            //    context.Remove(connection);
            //}

            //User receiver = context.Users.Find(friend.Id); //alan --friend
            //if (receiver != null)
            //{
            //    receiver.FriendsCount -= 1;
            //    context.Users.Update(receiver);
            //}


            //User sender = context.Users.Find(user.Id);//gönderen -- ben
            //if (sender != null)
            //{
            //    sender.FriendsCount -= 1;
            //    context.Users.Update(sender);
            //}


            ////_user.isFollowing = _context.Connections.Any(y => y.FolloweeId == followee.UserId && y.FollowingId == follower.UserId && y.IsActive == true && y.IsDeleted == false);

            //context.SaveChanges();


            UserDto friend = GetUserByName(email);
            UserDto user = GetUserByName(emailUser);

            var connection = context.Connections.FirstOrDefault(x => x.UserId == user.Id && x.FriendId == friend.Id && x.IsPending == true && x.isDeleted == false);
            if (connection != null)
            {
                connection.IsPending = false;
                connection.isDeleted = true;
                context.Connections.Update(connection);
            }
            else
            {
                Connection newconnection = new Connection();
                newconnection.isDeleted = false;
                newconnection.IsPending = true;
                newconnection.UserId = user.Id;
                newconnection.FriendId = friend.Id;
                context.Connections.Remove(newconnection);
            }
            User me = context.Users.Find(user.Id);
            if (me != null)
            {
                me.FriendsCount -= 1;
                context.Users.Update(me);
            }
            User frnd = context.Users.Find(friend.Id);
            if (frnd != null)
            {
                frnd.FriendsCount -= 1;
                context.Users.Update(frnd);
            }

            context.SaveChanges();

        }

        public bool IsFollow(string name, string identity)
        {
            UserDto friend = GetUserByName(name);
            UserDto user = GetUserByName(identity);

            bool connection = context.Connections.Any(x => x.UserId == user.Id && x.FriendId == friend.Id);
            if (connection)
            {
                return true;
            }
            return false;
        }

    }
}
