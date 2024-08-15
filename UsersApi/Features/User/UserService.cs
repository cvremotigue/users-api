using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using UsersApi.Core.Exceptions;
using UsersApi.Data.Contexts;

namespace UsersApi.Features.User
{
    public class UserService
    {
        private readonly UserDbContext _context;
        private readonly IMapper _mapper;
        private readonly UserValidationService _userValidationService;
        private readonly Serilog.ILogger _logger;
        public UserService(UserDbContext context, IMapper mapper, UserValidationService userValidationService, Serilog.ILogger logger)
        {
            _context = context;
            _mapper = mapper;
            _userValidationService = userValidationService;
            _logger = logger;
        }

        public async Task<Guid> CreateUser(CreateUserRequest request)
        {
            var (isValid, errorMessage) = _userValidationService.IsValid(request);

            if (isValid is false)
            {
                _logger.Error(errorMessage);
                throw new InvalidParameterException(nameof(CreateUser), errorMessage);
            }

            var user = _mapper.Map<Core.Entities.Users>(request);
            await _context.AddAsync(user);
            await _context.SaveChangesAsync();

            return user.Id;
        }

        public async Task<UserDetails> GetUserDetails(Guid id)
        {
            var user = await _context.Users
                .Where(x => x.Id == id)
                .ProjectTo<UserDetails>(_mapper.ConfigurationProvider)
                .SingleOrDefaultAsync();

            if (user is null)
            {
                _logger.Error("Record not found.");
                throw new RecordNotFoundException("User", "Record not found.");
            }

            return user;
        }

        public async Task<List<UserDetails>> GetActiveUsers()
        {
            return await _context.Users
                .Where(x => x.IsDeleted == false)
                .ProjectTo<UserDetails>(_mapper.ConfigurationProvider).ToListAsync();
        }

        public async Task DeleteUser(Guid id)
        {
            var user = await _context.Set<Core.Entities.Users>()
                .SingleOrDefaultAsync(x => x.Id == id);

            if (user is null)
            {
                _logger.Error("Record not found.");
                throw new RecordNotFoundException("User", "Record not found.");
            }

            user.IsDeleted = true;
            user.UpdatedAt = DateTimeOffset.UtcNow;
            _context.Set<Core.Entities.Users>().Update(user);
            await _context.SaveChangesAsync();
        }

        public async Task<UserDetails> EditUser(Guid id, EditUserRequest request)
        {
            var (isValid, errorMessage) = _userValidationService.IsValid(request);

            if (isValid is false)
            {
                _logger.Error(errorMessage);
                throw new InvalidParameterException(nameof(EditUser), errorMessage);
            }

            var user = await _context.Set<Core.Entities.Users>()
                    .SingleOrDefaultAsync(x => x.Id == id);

            if (user is null)
            {
                _logger.Error("Record not found.");
                throw new RecordNotFoundException("User", "Record not found.");
            }

            _mapper.Map(request, user);
            _context.Set<Core.Entities.Users>().Update(user);
            await _context.SaveChangesAsync();

            return _mapper.Map(user, new UserDetails());
        }
    }
}
