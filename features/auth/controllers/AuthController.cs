using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SOA.features.auth.dtos;
using SOA.features.auth.services;
using SOA.features.auth.utils;

namespace SOA.features.auth.controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;
        private readonly IConfiguration _config;
        private readonly UserContextService _userContext;

        public AuthController(AuthService authService, IConfiguration config, UserContextService userContext)
        {
            _authService = authService;
            _config = config;
            _userContext = userContext;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginUserDto dto)
        {
            try
            {
                var jwt = await _authService.LoginAsync(dto);

                Response.Cookies.Append("TOKEN", jwt, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = Request.IsHttps,
                    SameSite = SameSiteMode.Strict,
                    Expires = DateTime.UtcNow.AddMinutes(double.Parse(_config["Jwt:ExpireMinutes"]))
                });

                return Ok(new { Message = "Login correcto" });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { Message = ex.Message });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { Message = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, new { Message = "Error interno del servidor." });
            }
        }

        [HttpPost("create-account")]
        public async Task<IActionResult> RegisterUser([FromBody] CreateUserDto dto)
        {
            try
            {
                var created = await _authService.CreateUserAsync(dto);
                return Ok(new { Message = "Usuario creado correctamente", User = created });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { Message = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, new { Message = "Error interno del servidor." });
            }
        }

        [Authorize]
        [HttpGet("info-account")]
        public async Task<IActionResult> InfoAccount()
        {
            var result = await _userContext.GetAsyncUser();

            bool success = (bool)result.GetType().GetProperty("Success")?.GetValue(result)!;
            int statusCode = (int)result.GetType().GetProperty("StatusCode")?.GetValue(result)!;

            if (!success)
                return StatusCode(statusCode, result);

            return Ok(result);
        }
    }
}
