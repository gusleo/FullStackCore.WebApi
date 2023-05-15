using AutoMapper;
using Core.Data.Entities;
using Core.Service.Models;
using Core.Service.Services.Test.Models;
using Scaffolding.Auth.Entities;
using Scaffolding.Data.Entities;
using Scaffolding.Data.Test.Entities;
using Scaffolding.Service.Infrastructure;
using Scaffolding.Service.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Service.Services.Test
{
    public class CoreProfileTest : Profile
    {
        public CoreProfileTest()
        {
            CreateMap<TestModel, TestEntity>().MaxDepth(1).ReverseMap();
            CreateMap<UserModel, ApplicationUser>().MaxDepth(1).ReverseMap();
            CreateMap<UserDetailModel, UserDetail>().MaxDepth(1).ReverseMap();

            CreateMap(typeof(PaginationSet<>), typeof(PaginationEntity<>)).MaxDepth(1).ReverseMap();
        }
    }
}
