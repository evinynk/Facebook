﻿using Domain.Context;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Domain.Entities.Concrete
{
   public class User:IdentityUser<int>
    {
       
        public string Name { get; set; }
        
        public string Surname { get; set; }
        public string Description { get; set; }
        public string ProfilePhoto { get; set; }
        public string BackGroundPhoto { get; set; }
        public DateTime CreatedDate { get; set; }
        public int FriendsCount { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsPublic { get; set; }
        
        public ICollection<Share> Shares { get; set; }
        public ICollection<LikeComment> LikeComments { get; set; }
        public ICollection<LikePost> LikePosts { get; set; }
        public ICollection<Post> Posts { get; set; }
    }
}
