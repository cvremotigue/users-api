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

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateRunningActivity(CreateRunningActivityRequest request)
        {
            try
            {
                var id = await _runningActivityService.CreateRunningActivity(request);
                return Created("", id);
            }

            catch (InvalidParameterException ex)
            {
                ModelState.AddModelError(ex.ModuleName, ex.Message);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(nameof(CreateRunningActivity), ex.Message);
            }

            return BadRequest(ProblemDetailsFactory.CreateValidationProblemDetails(HttpContext, ModelState));
        }

        [HttpGet("{userId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> GetUsersRunningActivities(Guid userId)
        {
            try
            {
                var activity = await _runningActivityService.GetUsersRunningActivities(userId);
                return Ok(activity);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(nameof(GetUsersRunningActivities), ex.Message);
            }

            return BadRequest(ProblemDetailsFactory.CreateValidationProblemDetails(HttpContext, ModelState));
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> DeleteRunningActivity(int id)
        {
            try
            {
                await _runningActivityService.DeleteRunningActivity(id);
                return NoContent();
            }
            catch (RecordNotFoundException ex)
            {
                return NotFound($"{ex.RecordName} : {ex.Message}");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(nameof(DeleteRunningActivity), ex.Message);
            }

            return BadRequest(ProblemDetailsFactory.CreateValidationProblemDetails(HttpContext, ModelState));
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> EditRunningActivity(int id, EditRunningActivityRequest request)
        {
            try
            {
                var activity = await _runningActivityService.EditRunningActivity(id, request);
                return Ok(activity);
            }
            catch (RecordNotFoundException ex)
            {
                return NotFound($"{ex.RecordName} : {ex.Message}");
            }

            catch (InvalidParameterException ex)
            {
                ModelState.AddModelError(ex.ModuleName, ex.Message);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(nameof(EditRunningActivity), ex.Message);
            }

            return BadRequest(ProblemDetailsFactory.CreateValidationProblemDetails(HttpContext, ModelState));
        }
    }
}
