using AutoMapper;
using UsersApi.Features.User;

namespace UsersApi.Infrastructure
{
    public class AutoMapperStartupExtensions : Profile
    {
        public AutoMapperStartupExtensions()
        {
            CreateMap<Core.Entities.Users, UserDetails>().ReverseMap();
            CreateMap<Core.Entities.Users, CreateUserRequest>().ReverseMap();
            CreateMap<Core.Entities.Users, EditUserRequest>().ReverseMap();
        }
    }
}
