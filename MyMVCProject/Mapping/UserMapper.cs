using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyMVCProject.Models;

namespace MyMVCProject.Mapping
{
    public class UserMapper : Profile
    {
        public UserMapper()
        {
            CreateMap<RegisterViewModel, MyUser>();
        }
    }
}
