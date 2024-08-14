using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UsersApi.Core.Exceptions;

namespace UsersApi.Features.RunningActivity
{
    [Route("[controller]")]
    [ApiController]
    public class RunningActivityController : ControllerBase
    {
        private readonly RunningActivityService _runningActivityService;

        public RunningActivityController(RunningActivityService runningActivityService)
        {
            _runningActivityService = runningActivityService;
        }

    }
}
