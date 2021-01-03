using Common.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Services.Interfaces
{
    public interface IPostService
    {
        List<PostDto> GetPostList(string name);
        bool Post(PostDto postdto);
       
        List<PostDto> GetAllPostList(string name);
        void Like(int userid, int postid);
        
    }
}
