using Common.Dtos;
using Core.Services.Interfaces;
using Domain.Context;
using Domain.Entities.Concrete;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Core.Services.Concrete
{
    public class PostService:IPostService
    {
        private FacebookDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly IUserService _userService;
        public PostService(FacebookDbContext context, UserManager<User> userManager,IUserService userservice)
        {
            _context = context;
            _userManager = userManager;
            _userService = userservice;
        }
        public bool Post(PostDto postdto)
        {
            Post post = new Post();
            post.Text = postdto.Text;
            post.UserId = postdto.UserId;
            post.LikeCount = 0;
            post.IsDeleted = false;
            post.Image= postdto.Image;
            post.LikeCount = 0;
            post.CreatedDate = DateTime.Now;
            _context.Posts.Add(post);
            var result = _context.SaveChanges();
            if (result > default(int))
                return true;
            return false;
        }
        public List<PostDto> GetPostList(string name)
        {
            User user = _context.Users.Where(x => x.Email == name).FirstOrDefault();           
          
            List<PostDto> listdto = new List<PostDto>();
            List<Post> Friendlist = _context.Posts.Where(x => x.UserId == user.Id).ToList();
            //List<Connection> Friendlist = context.Connections.Where(x => x.UserId == user.Id  && x.IsPending==false).ToList(); //ispending: beklemede
            //foreach (var friend in Friendlist)
            //{                
            //    foreach (var item in friend.User.posts)//USER->FRİEND
            //    {
            //        post.Text = item.Text;
            //        post.CreatedDate = item.CreatedDate;
            //        post.Image = item.Image;
            //        post.LikeCount = item.LikeCount;
            //        post.IsDeleted = item.IsDeleted;
            //        post.PostId = item.PostId;
            //        listdto.Add(post);
            //    }
            //}
            foreach(var item in Friendlist)
            {
                PostDto post = new PostDto();
                post.Text = item.Text;
                post.PostId = item.PostId;
                post.CreatedDate = item.CreatedDate;
                post.Image = item.Image;
                post.UserName = item.User.Name;
                post.UserSurname = item.User.Surname;
                post.BackgroundPhoto = item.User.BackGroundPhoto;
                post.ProfilPhoto = item.User.ProfilePhoto;
                post.LikeCount = item.LikeCount;
                post.CommentCount = item.CommentCount;
                listdto.Add(post);
            }
            listdto = listdto.OrderByDescending(x => x.CreatedDate).ToList();

            return listdto;
        }

        //hem benim hem arkadaşlarımın postu için
    
        public List<PostDto> GetAllPostList(string name)
        {
            User user = _context.Users.Where(x => x.Email == name).FirstOrDefault();


            List<PostDto> listdto = new List<PostDto>();

            List<Connection> Friendlist = _context.Connections.Where(x => x.UserId == user.Id).ToList(); //ispending: beklemede
            foreach (var friend in Friendlist)
            {
                //kullanıcı idsine göre propertyleri listeler
                foreach (var item in GetListByName(friend.FriendId))
                {
                    PostDto post = new PostDto();
                    post.Text = item.Text;
                    post.CreatedDate = item.CreatedDate;
                    post.Image = item.Image;
                    post.LikeCount = item.LikeCount;
                    post.IsDeleted = item.IsDeleted;
                    post.PostId = item.PostId;
                    post.ProfilPhoto = item.ProfilPhoto;
                    post.BackgroundPhoto = item.BackgroundPhoto;
                    post.CommentCount = item.CommentCount;
                    post.PostId = item.PostId;
                    post.UserName = item.UserName;
                    post.UserSurname = item.UserSurname;
                    listdto.Add(post);
                }
            }


            List<Post> userspost = _context.Posts.Where(x => x.UserId == user.Id).ToList();

            foreach (var item in userspost)
            {
                PostDto post = new PostDto();
                post.Text = item.Text;
                post.PostId = item.PostId;
                post.CreatedDate = item.CreatedDate;
                post.Image = item.Image;
                post.UserName = item.User.Name;
                post.UserSurname = item.User.Surname;
                post.BackgroundPhoto = item.User.BackGroundPhoto;
                post.ProfilPhoto = item.User.ProfilePhoto;
                post.LikeCount = item.LikeCount;
                post.CommentCount = item.CommentCount;
                listdto.Add(post);
            }
            listdto = listdto.OrderByDescending(x => x.CreatedDate).ToList();
            return listdto;
        }

        //kullanıcının idsine göre tüm propertylerini
        public List<PostDto> GetListByName(int user)
        {

            int id = user;

            var posts = (from u in _context.Users
                         join p in _context.Posts
                         on
                          u.Id equals p.UserId
                         select new
                         {
                             u.UserName,
                             u.Id,
                             u.Name,
                             u.ProfilePhoto,
                             u.Surname,
                             p.LikeCount,
                             u.BackGroundPhoto,
                             p.CommentCount,
                             p.Text,
                             p.CreatedDate,
                             p.CommentTo,
                             p.Comments,
                             p.PostId,
                             p.Image
                         }).OrderByDescending(x => x.CreatedDate);

            var list = posts.Where(x => x.Id == id).ToList();
            List<PostDto> listdto = new List<PostDto>();
            foreach (var item in list)
            {
                PostDto post = new PostDto();
                post.Text = item.Text;
                post.PostId = item.PostId;
                post.CreatedDate = item.CreatedDate;
                post.Image = item.Image;
                post.UserName = item.Name;
                post.UserSurname = item.Surname;
                post.BackgroundPhoto = item.BackGroundPhoto;
                post.ProfilPhoto = item.ProfilePhoto;
                post.LikeCount = item.LikeCount;
                post.CommentCount = item.CommentCount;
                post.UserName = item.Name;
                //post.UserSurname = item.Surname;
                listdto.Add(post);
            }

            return listdto;

        }

        public void Like(int userid, int postid)
        {
            Post post = _context.Posts.Find(postid);
            LikePost like = _context.LikePosts.Where(x => x.UserId == userid && x.PostId == postid).FirstOrDefault();
            if (like == null)
            {

                LikePost like1 = new LikePost();
                like1.UserId = userid;
                like1.PostId = postid;
                post.LikeCount += 1;
                _context.LikePosts.Add(like1);

            }
            else
            {
                _context.LikePosts.Remove(like);
                post.LikeCount -= 1;
                _context.SaveChanges();
            }
           _context.SaveChanges();

        }


    }
}
