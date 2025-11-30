
using SOA.features.auth.dtos;
using SOA.features.auth.dtos.request;
using SOA.features.auth.dtos.response;
using SOA.features.auth.models;
using SOA.features.auth.repositories;

namespace SOA.features.auth.services
{
    public class AuthService(
            UserRepository userRepository,
            JwtService jwtService,
            CookieService cookieService,
            TokenService tokenService,
            EmailService emailService,
            TokenRepository tokenRepository
        )
    {
        private readonly UserRepository _userRepository = userRepository;
        private readonly JwtService _jwtService = jwtService;
        private readonly CookieService _cookieService = cookieService;
        private readonly TokenService _tokenService = tokenService;
        private readonly EmailService _emailService = emailService;
        private readonly TokenRepository _tokenRepository = tokenRepository;

        public async Task<User?> FindEmailAsync(string email)
        {
            return await _userRepository.FindByEmailAsync(email);
        }

        public async Task<ResponseAuth> CreateUserAsync(CreateUserDto dto)
        {
            var userDB = await FindEmailAsync(dto.Email);
            if (userDB != null) throw new InvalidOperationException("El correo ya está registrado.");

            var newUser = GenerateInstance.user(dto);

            var created = await _userRepository.CreateAsync(newUser);

            var token = _tokenService.get();

            var tokenInstance = GenerateInstance.token(token, created.Id);
            await _tokenRepository.UpsertAsync(token, tokenInstance);

            await _emailService.SendEmailAsync(
                created.Email, "Confirma tu cuenta - SOA App", _emailService.GetConfirmationEmailBody(token), token
            );
            var userInfo = GenerateInstance.userResponseInfo(created);

            return new ResponseAuth
            {
                Message = "Cuenta creada exitosamente, revisa tu correo para confirmar tu cuenta",
 
                Info = userInfo
            };
        }

        public async Task<ResponseAuth> LoginAsync(LoginUserDto dto)
        {
            var user = await FindEmailAsync(dto.Email);
            if (user == null) throw new KeyNotFoundException("El usuario no existe.");

            if (!user.Confirmed) throw new UnauthorizedAccessException("Por favor, confirma tu cuenta antes de iniciar sesión.");

            bool match = BCrypt.Net.BCrypt.Verify(dto.Password, user.Password);
            if (!match) throw new UnauthorizedAccessException("Email o contraseña incorrectos.");

            var token = _jwtService.GenerateJwtToken(user);
            _cookieService.SetTokenCookie(token, double.Parse("999999999"));

            var userInfo = GenerateInstance.userResponseInfo(user);
            return new ResponseAuth
            {
                Message = "Inicio de sesión exitoso",
                Token = token,
                Info = userInfo
            };
        }

        public async Task<ResponseAuth> ConfirmAccountAsync(ConfirmAccountDto dto)
        {
            var userDB = await FindEmailAsync(dto.Email);
            if (userDB is null) throw new Exception("El usuario no existe");

            var tokenDb = await _tokenRepository.FindByUserIdAsync(userDB.Id);
            if (tokenDb is null)
                throw new Exception("No existe un token asignado a este usuario");

            if (tokenDb.TokenValue.ToString() != dto.Token)
                throw new Exception("Token inválido");

            if (tokenDb.ExpiresAt < DateTime.UtcNow)
            {
                await _tokenRepository.DeleteByIdAsync(tokenDb.Id);
                throw new Exception("El token ha expirado, solicite uno nuevo");
            }

            userDB.Confirmed = true;
            await _userRepository.UpdateAsync(userDB);
            await _tokenRepository.DeleteByIdAsync(tokenDb.Id);

            return new ResponseAuth
            {
                Message = "Cuenta confirmada correctamente",
                Info = GenerateInstance.userResponseInfo(userDB)
            };
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
                user.Confirmed
            };
        }

    }
}
