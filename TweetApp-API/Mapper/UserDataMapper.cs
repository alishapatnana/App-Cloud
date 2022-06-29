using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TweetApp_API.Models;
using TweetApp_API.Models.Dtos.TweetDto;
using TweetApp_API.Models.Dtos.UserDto;

namespace TweetApp_API.Mapper
{
    public class UserDataMapper:Profile
    { 
        public UserDataMapper()
        {
            CreateMap<CreateUserDto, Users>().ReverseMap();
            CreateMap<ViewUserDto, Users>().ReverseMap();
            CreateMap<ViewUserDto, CreateUserDto>().ReverseMap();
            CreateMap<CreateTweetDto, Tweets>();
        }
    }
}
