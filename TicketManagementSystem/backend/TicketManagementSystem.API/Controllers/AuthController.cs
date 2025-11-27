using System.Threading.Tasks;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TicketManagementSystem.API.DTOs;
using Swashbuckle.AspNetCore.Filters;
using TicketManagementSystem.API.Controllers.Examples;
using TicketManagementSystem.API.Services;

namespace TicketManagementSystem.API.Controllers
{
    /// <summary>
    /// Controller for authentication operations
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ILogger<AuthController> _logger;
        private readonly IValidator<LoginDto> _loginValidator;
        private readonly IValidator<RegisterDto> _registerValidator;

        /// <summary>
        /// Constructor for AuthController
        /// </summary>
        /// <param name="authService">Auth service for business logic</param>
        /// <param name="logger">Logger for structured logging</param>
        /// <param name="loginValidator">Validator for login DTO</param>
        /// <param name="registerValidator">Validator for register DTO</param>
        public AuthController(
            IAuthService authService,
            ILogger<AuthController> logger,
            IValidator<LoginDto> loginValidator,
            IValidator<RegisterDto> registerValidator)
        {
            _authService = authService;
            _logger = logger;
            _loginValidator = loginValidator;
            _registerValidator = registerValidator;
        }

        /// <summary>
        /// Authenticate user and return JWT token
        /// </summary>
        /// <param name="dto">Login credentials</param>
        /// <returns>JWT token and user info</returns>
        /// <response code="200">Login successful</response>
        /// <response code="400">Invalid login data</response>
        /// <response code="401">Invalid credentials</response>
        /// <response code="500">Internal server error</response>
        [HttpPost("login")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(LoginResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerRequestExample(typeof(LoginDto), typeof(LoginRequestExample))]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(LoginResponseExample))]
        public async Task<ActionResult<LoginResponseDto>> Login([FromBody] LoginDto dto)
        {
            var validationResult = await _loginValidator.ValidateAsync(dto);
            if (!validationResult.IsValid)
            {
                _logger.LogWarning("Invalid login data: {Errors}", validationResult.Errors);
                return BadRequest(new ProblemDetails
                {
                    Title = "Validation Error",
                    Detail = "One or more fields are invalid",
                    Status = StatusCodes.Status400BadRequest,
                    Extensions = { ["errors"] = validationResult.Errors.Select(e => e.ErrorMessage) }
                });
            }

            try
            {
                var result = await _authService.LoginAsync(dto.Email, dto.Password);
                _logger.LogInformation("User {Email} logged in successfully", dto.Email);
                return Ok(result);
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogWarning(ex, "Failed login attempt for user {Email}", dto.Email);
                return Unauthorized(new ProblemDetails
                {
                    Title = "Invalid Credentials",
                    Detail = "The email or password is incorrect",
                    Status = StatusCodes.Status401Unauthorized
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during login for user {Email}", dto.Email);
                return StatusCode(StatusCodes.Status500InternalServerError, new ProblemDetails
                {
                    Title = "Internal Server Error",
                    Detail = "An error occurred while processing the request",
                    Status = StatusCodes.Status500InternalServerError
                });
            }
        }

        /// <summary>
        /// Register a new user
        /// </summary>
        /// <param name="dto">Registration data</param>
        /// <returns>Registration confirmation</returns>
        /// <response code="201">User registered successfully</response>
        /// <response code="400">Invalid registration data or user already exists</response>
        /// <response code="500">Internal server error</response>
        [HttpPost("register")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(RegisterResponseDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<RegisterResponseDto>> Register([FromBody] RegisterDto dto)
        {
            var validationResult = await _registerValidator.ValidateAsync(dto);
            if (!validationResult.IsValid)
            {
                _logger.LogWarning("Invalid registration data: {Errors}", validationResult.Errors);
                return BadRequest(new ProblemDetails
                {
                    Title = "Validation Error",
                    Detail = "One or more fields are invalid",
                    Status = StatusCodes.Status400BadRequest,
                    Extensions = { ["errors"] = validationResult.Errors.Select(e => e.ErrorMessage) }
                });
            }

            try
            {
                var result = await _authService.RegisterAsync(dto);
                _logger.LogInformation("User {Email} registered successfully", dto.Email);
                return StatusCode(StatusCodes.Status201Created, result);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Registration failed for user {Email}: {Message}", dto.Email, ex.Message);
                return BadRequest(new ProblemDetails
                {
                    Title = "Registration Failed",
                    Detail = ex.Message,
                    Status = StatusCodes.Status400BadRequest
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error during registration for user {Email}: {ExceptionType} - {Message} - {StackTrace}", 
                    dto.Email, ex.GetType().Name, ex.Message, ex.StackTrace);
                return StatusCode(StatusCodes.Status500InternalServerError, new ProblemDetails
                {
                    Title = "Internal Server Error",
                    Detail = "An error occurred while processing the request",
                    Status = StatusCodes.Status500InternalServerError
                });
            }
        }

        /// <summary>
        /// Refresh JWT token
        /// </summary>
        /// <param name="dto">Refresh token data</param>
        /// <returns>New JWT token</returns>
        /// <response code="200">Token refreshed successfully</response>
        /// <response code="400">Invalid refresh token</response>
        /// <response code="500">Internal server error</response>
        [HttpPost("refresh")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(RefreshTokenResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<RefreshTokenResponseDto>> RefreshToken([FromBody] RefreshTokenDto dto)
        {
            if (string.IsNullOrEmpty(dto.RefreshToken))
            {
                return BadRequest(new ProblemDetails
                {
                    Title = "Validation Error",
                    Detail = "Refresh token is required",
                    Status = StatusCodes.Status400BadRequest
                });
            }

            try
            {
                var result = await _authService.RefreshTokenAsync(dto.RefreshToken);
                _logger.LogInformation("Token refreshed successfully");
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error refreshing token");
                return StatusCode(StatusCodes.Status500InternalServerError, new ProblemDetails
                {
                    Title = "Internal Server Error",
                    Detail = "An error occurred while processing the request",
                    Status = StatusCodes.Status500InternalServerError
                });
            }
        }

        /// <summary>
        /// Logout user and invalidate tokens
        /// </summary>
        /// <param name="dto">Logout request with refresh token</param>
        /// <returns>Logout confirmation</returns>
        /// <response code="200">Logout successful</response>
        /// <response code="400">Invalid logout request</response>
        /// <response code="500">Internal server error</response>
        [HttpPost("logout")]
        [Authorize]
        [ProducesResponseType(typeof(LogoutResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<LogoutResponseDto>> Logout([FromBody] LogoutDto dto)
        {
            try
            {
                var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
                var email = User.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value;

                // Invalidate refresh token in the service
                await _authService.LogoutAsync(dto.RefreshToken);

                _logger.LogInformation("User {Email} (ID: {UserId}) logged out successfully", email, userId);

                return Ok(new LogoutResponseDto
                {
                    Message = "Logout successful",
                    LoggedOut = true,
                    Timestamp = DateTime.UtcNow
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during logout");
                return StatusCode(StatusCodes.Status500InternalServerError, new ProblemDetails
                {
                    Title = "Internal Server Error",
                    Detail = "An error occurred while processing the request",
                    Status = StatusCodes.Status500InternalServerError
                });
            }
        }
    }

    /// <summary>
    /// DTO for refresh token request
    /// </summary>
    public class RefreshTokenDto
    {
        /// <summary>
        /// Refresh token
        /// </summary>
        public string RefreshToken { get; set; } = string.Empty;
    }
}