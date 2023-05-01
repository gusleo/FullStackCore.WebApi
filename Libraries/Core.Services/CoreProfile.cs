using AutoMapper;
using Core.Data.Entities;
using Core.Service.Models;
using Scaffolding.Auth.Entities;
using Scaffolding.Data.Entities;
using Scaffolding.Service.Infrastructure;
using Scaffolding.Service.Model;

namespace Core.Services
{
    public class CoreProfile : Profile
    {
        public CoreProfile() { 
            CreateMap<UserModel, ApplicationUser>().MaxDepth(1).ReverseMap();
            CreateMap<UserDetailModel, UserDetail>().MaxDepth(1).ReverseMap();

            CreateMap(typeof(PaginationSet<>), typeof(PaginationEntity<>)).MaxDepth(1).ReverseMap();
        }
    }
}