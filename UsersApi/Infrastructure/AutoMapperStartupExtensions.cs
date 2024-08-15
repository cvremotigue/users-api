using AutoMapper;
using UsersApi.Core.Entities;
using UsersApi.Features.RunningActivity;
using UsersApi.Features.User;

namespace UsersApi.Infrastructure
{
    public class AutoMapperStartupExtensions : Profile
    {
        public AutoMapperStartupExtensions()
        {
            CreateMap<Users, UserDetails>().ReverseMap();
            CreateMap<Users, CreateUserRequest>().ReverseMap();
            CreateMap<Users, EditUserRequest>().ReverseMap();
            CreateMap<RunningActivities, RunningActivityDetails>().ReverseMap();
            CreateMap<RunningActivities, CreateRunningActivityRequest>().ReverseMap();
            CreateMap<RunningActivities, EditRunningActivityRequest>().ReverseMap();
        }
    }
}
