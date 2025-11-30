using SOA.features.auth.models;

namespace SOA.features.auth.dtos.response
{
    public class ResponseAuth
    {
        public string Message { get; set; } = null!;
        public string Token { get; set; } = null!;

        public UserResponseDto Info { get; set; } = null!;
    }

}
