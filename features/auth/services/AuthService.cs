using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using SOA.features.auth.dtos;
using SOA.features.auth.models;
using SOA.features.auth.repositories;

namespace SOA.features.auth.services
{
    public class AuthService
    {
        private readonly UserRepository _userRepository;
        private readonly IConfiguration _config;

        public AuthService(UserRepository userRepository, IConfiguration config)
        {
            _userRepository = userRepository;
            _config = config;
        }

        // Buscar usuario por email
        public async Task<User?> FindEmailAsync(string email)
        {
            return await _userRepository.FindByEmailAsync(email);
        }

        // Crear usuario
        public async Task<User> CreateUserAsync(CreateUserDto dto)
        {
            var existingUser = await FindEmailAsync(dto.Email);
            if (existingUser != null)
                throw new InvalidOperationException("El correo ya está registrado.");

            var newUser = new User
            {
                Name = dto.Name,
                Lastname = dto.Lastname,
                Email = dto.Email,
                Password = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                Phone = dto.Phone,
                BirthDate = dto.BirthDate,
                Confirmed = true,
            };

            return await _userRepository.CreateAsync(newUser);
        }

        public async Task<string> LoginAsync(LoginUserDto dto)
        {
            var user = await _userRepository.FindByEmailAsync(dto.Email);

            if (user == null)
                throw new KeyNotFoundException("El usuario no existe.");

            if (!BCrypt.Net.BCrypt.Verify(dto.Password, user.Password))
                throw new UnauthorizedAccessException("Email o contraseña incorrectos.");

            return GenerateJwtToken(user);
        }


        public async Task<object?> GetUserInfoAsync(Guid userId)
        {
            var user = await _userRepository.FindByIdAsync(userId);

            if (user == null)
                throw new KeyNotFoundException("Usuario no encontrado.");

            return new
            {
                user.Id,
                user.Name,
                user.Lastname,
                user.Email,
                user.Phone,
                user.BirthDate,
                user.Confirmed
            };
        }

        private string GenerateJwtToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_config["Jwt:Key"]);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Email, user.Email)
                }),
                Expires = DateTime.UtcNow.AddMinutes(double.Parse(_config["Jwt:ExpireMinutes"])),
                Issuer = _config["Jwt:Issuer"],
                Audience = _config["Jwt:Audience"],
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature
                )
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
