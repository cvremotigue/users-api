using Microsoft.AspNetCore.Mvc;
using UsersApi.Core.Exceptions;

namespace UsersApi.Features.User
{
    [Route("[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;

        public UserController(UserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> CreateUser(CreateUserRequest request)
        {
            try
            {
                var id = await _userService.CreateUser(request);
                return Created("", id);
            }
            catch (InvalidParameterException ex)
            {
                ModelState.AddModelError(ex.ModuleName, ex.Message);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(nameof(CreateUser), ex.Message);
            }

            return BadRequest(ProblemDetailsFactory.CreateValidationProblemDetails(HttpContext, ModelState));
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> GetUser(Guid id)
        {
            try
            {
                var user = await _userService.GetUserDetails(id);
                return Ok(user);
            }
            catch (RecordNotFoundException ex)
            {
                return NotFound($"{ex.RecordName} : {ex.Message}");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(nameof(GetUser), ex.Message);
            }

            return BadRequest(ProblemDetailsFactory.CreateValidationProblemDetails(HttpContext, ModelState));
        }

        [HttpGet("list")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> GetActiveUsers()
        {
            try
            {
                var users = await _userService.GetActiveUsers();
                return Ok(users);
            }
            catch (Exception ex) 
            {
                ModelState.AddModelError(nameof(GetActiveUsers), ex.Message);
            }

            return BadRequest(ProblemDetailsFactory.CreateValidationProblemDetails(HttpContext, ModelState));
           
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            try
            {
                await _userService.DeleteUser(id);
                return NoContent();
            }
            catch (RecordNotFoundException ex)
            {
                return NotFound($"{ex.RecordName} : {ex.Message}");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(nameof(GetActiveUsers), ex.Message);
            }

            return BadRequest(ProblemDetailsFactory.CreateValidationProblemDetails(HttpContext, ModelState));
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> EditUser(Guid id, EditUserRequest request)
        {
            try
            {
                var user = await _userService.EditUser(id, request);
                return Ok(user);
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
                ModelState.AddModelError(nameof(EditUser), ex.Message);
            }

            return BadRequest(ProblemDetailsFactory.CreateValidationProblemDetails(HttpContext, ModelState));
        }
    }
}
