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
        private readonly RunningActivityValidationService _validationService;
        private readonly Serilog.ILogger _logger;

        public RunningActivityService(UserDbContext context, IMapper mapper, RunningActivityValidationService validationService, Serilog.ILogger logger)
        {
            _context = context;
            _mapper = mapper;
            _validationService = validationService;
            _logger = logger;
        }

        public async Task<int> CreateRunningActivity(CreateRunningActivityRequest request)
        {
            var (isValid, errorMessage) = _validationService.IsValid(request);

            if (isValid is false)
            {
                _logger.Error(errorMessage);
                throw new InvalidParameterException(nameof(CreateRunningActivity), errorMessage);
            }

            var activity = _mapper.Map<RunningActivities>(request);
            await _context.AddAsync(activity);
            await _context.SaveChangesAsync();

            return activity.Id;
        }

        public async Task<List<RunningActivityDetails>> GetUsersRunningActivities(Guid userId)
        {
            return await _context.RunningActivities
                 .Where(x => x.UserId == userId)
                 .ProjectTo<RunningActivityDetails>(_mapper.ConfigurationProvider).ToListAsync();
        }

        public async Task DeleteRunningActivity(int id)
        {
            var activity = await _context.Set<RunningActivities>()
                .SingleOrDefaultAsync(x => x.Id == id);

            if (activity is null)
            {
                _logger.Error("Record not found.");
                throw new RecordNotFoundException("RunningActivity", "Record not found.");
            }

            _context.Set<RunningActivities>().Remove(activity);
            await _context.SaveChangesAsync();
        }

        public async Task<RunningActivityDetails> EditRunningActivity(int id, EditRunningActivityRequest request)
        {
            var (isValid, errorMessage) = _validationService.IsValid(request);

            if (isValid is false)
            {
                _logger.Error(errorMessage);
                throw new InvalidParameterException(nameof(CreateRunningActivity), errorMessage);
            }

            var activity = await _context.Set<RunningActivities>()
                .SingleOrDefaultAsync(x => x.Id == id);

            if (activity is null)
            {
                _logger.Error("Record not found.");
                throw new RecordNotFoundException("RunningActivity", "Record not found.");
            }

            _mapper.Map(request, activity);
            _context.Set<RunningActivities>().Update(activity);
            await _context.SaveChangesAsync();

            return _mapper.Map(activity, new RunningActivityDetails());
        }
    }
}
