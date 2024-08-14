using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using UsersApi.Core.Entities;
using UsersApi.Core.Exceptions;
using UsersApi.Data.Contexts;

namespace UsersApi.Features.RunningActivity
{
    public class RunningActivityService
    {
        private readonly UserDbContext _context;
        private readonly IMapper _mapper;

        public RunningActivityService(UserDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

    }
}
