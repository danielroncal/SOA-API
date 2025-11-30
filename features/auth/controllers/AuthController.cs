using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SOA.features.auth.dtos;
using SOA.features.auth.dtos.request;
using SOA.features.auth.dtos.response;
using SOA.features.auth.services;
using SOA.features.auth.utils;

namespace SOA.features.auth.controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;
        private readonly UserContextService _userContext;

        public AuthController(AuthService authService, UserContextService userContext)
        {
            _authService = authService;
            _userContext = userContext;
        }

        [HttpPost("Iniciodesesion")]
            public async Task<ResponseAuth> Login([FromBody] LoginUserDto dto)
            {
                return await _authService.LoginAsync(dto); ;
            }

            [HttpPost("Confirmarcuenta")]
            public async Task<ResponseAuth> ConfirmAccount([FromBody] ConfirmAccountDto dto)
            {
                return await _authService.ConfirmAccountAsync(dto); ;
            }

        [HttpPost("Crearcuenta")]
        public async Task<ResponseAuth> RegisterUser([FromBody] CreateUserDto dto)
        {
           return await _authService.CreateUserAsync(dto);
        }

        [Authorize]
        [HttpGet("Listadelacuenta")]
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
