using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Common.Dtos;
using Core.Services.Interfaces;
using Domain.Entities.Concrete;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Facebook.Controllers
{
    public class FacebookController : Controller
    {
        public UserManager<User> userManager { get; set; }
        private readonly ILogger<FacebookController> logger;        
        private readonly IUserService userService;
        private readonly IPostService postService;
        public FacebookController(ILogger<FacebookController> _logger, IUserService _userService,IPostService _postService)
        {
            logger = _logger;
            userService = _userService;
            postService = _postService;
        }
        public IActionResult Home()
        {
        
            return View();
        }
        public IActionResult GetPosts()
        {
            string email = User.Identity.Name;
            List<PostDto> posts = postService.GetAllPostList(email);

            return PartialView("PostPartialView", posts);
        }
        public IActionResult Post(PostDto post)
        {
            UserDto user = userService.GetUserByName(User.Identity.Name);
            post.UserId = user.Id;
            string email = User.Identity.Name;
            string image = null;
           
            
                image=HttpContext.Session.GetString("path");
                post.Image = image;
            
           
            postService.Post(post);
            List<PostDto> posts = postService.GetPostList(email);
            HttpContext.Session.Clear();
            return PartialView("PostPartialView", posts);
        }


        //post atarken resim eklemek için 
        //bir actiondan diğerine bu session ile veri çekiyorum
        [HttpPost]
        public async Task<IActionResult> UploadImage(IFormFile file)
        {
            if (file != null)
            {
                string imageExtension = Path.GetExtension(file.FileName);//uzantı

                string imageName = Guid.NewGuid() + imageExtension;//unique filename

                HttpContext.Session.SetString("path",imageName);
                string path = Path.Combine(Directory.GetCurrentDirectory(), $"wwwroot/img/{imageName}");
                
                using var stream = new FileStream(path, FileMode.Create);

                await file.CopyToAsync(stream);
            }

            return RedirectToAction("Home","Facebook");
        }

        public IActionResult Like(PostDto post)
        {
            UserDto user = userService.GetUserByName(User.Identity.Name);
            postService.Like(user.Id, post.PostId);
            return Json(true);
        }

    }
}
