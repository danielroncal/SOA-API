using SOA.features.auth.services;
using System.Security.Claims;

namespace SOA.features.auth.utils
{
    public class UserContextService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly AuthService _authService;

        public UserContextService(IHttpContextAccessor httpContextAccessor, AuthService authService)
        {
            _httpContextAccessor = httpContextAccessor;
            _authService = authService;
        }

        public async Task<object> GetAsyncUser()
        {
            try
            {
                var userIdClaim = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);

                if (string.IsNullOrEmpty(userIdClaim))
                {
                    return new
                    {
                        Success = false,
                        StatusCode = 401,
                        Message = "Token inválido o expirado."
                    };
                }

                var userId = Guid.Parse(userIdClaim);
                var userInfo = await _authService.GetUserInfoAsync(userId);

                return new
                {
                    Success = true,
                    StatusCode = 200,
                    Data = userInfo
                };
            }
            catch (KeyNotFoundException ex)
            {
                return new
                {
                    Success = false,
                    StatusCode = 404,
                    Message = ex.Message
                };
            }
            catch (Exception)
            {
                return new
                {
                    Success = false,
                    StatusCode = 500,
                    Message = "Error interno del servidor."
                };
            }
        }

        public Guid? GetUserId()
        {
            var userId = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            return string.IsNullOrEmpty(userId) ? null : Guid.Parse(userId);
        }

        public string? GetUserEmail()
        {
            return _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Email);
        }
    }
}
