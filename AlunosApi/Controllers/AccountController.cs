using AlunosApi.Services;
using AlunosApi.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AlunosApi.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase {

        private readonly IConfiguration configuration;
        private readonly IAuthenticate authenticaton;

        public AccountController(IConfiguration configuration, IAuthenticate authenticaton) {
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            this.authenticaton = authenticaton ?? throw new ArgumentNullException(nameof(authenticaton));
        }

        [HttpPost("CreateUser")]
        public async Task<ActionResult<UserToken>> CreateUser([FromBody] RegisterModel model) {
            if (model.Password != model.ConfirmPassword) {
                ModelState.AddModelError("ConfirmPassword", "As Senhas não conferem");
                return BadRequest(ModelState);
            }
            var result = await authenticaton.RegisterUser(model.Email, model.Password);

            if (result) {
                return Ok($"Usuário {model.Email} criado com sucesso");
            }
            ModelState.AddModelError("CreateUser", "Registro inválido");
            return BadRequest(ModelState);
        }
        [HttpPost("LoginUser")]
        public async Task<ActionResult<UserToken>> Login([FromBody] LoginModel userInfo) {
            var result = await authenticaton.Authenticate(userInfo.Email, userInfo.Password);

            if (result) {
                return GenerateToken(userInfo);
            }
            ModelState.AddModelError("LoginUser", "Login inválido");
            return BadRequest(ModelState);
        }

        private ActionResult<UserToken> GenerateToken(LoginModel userInfo) {
            var claims = new[] {
                new Claim("email",userInfo.Email),
                new Claim("meuToken","token"),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("JWT_KEY")?? throw new Exception("jwt não gerada")));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var expiration = DateTime.UtcNow.AddMinutes(30);

            JwtSecurityToken token = new JwtSecurityToken(
                issuer: Environment.GetEnvironmentVariable("JWT_ISSUER") ?? throw new Exception("issuer não gerada"),
                audience: Environment.GetEnvironmentVariable("JWT_AUDIENCE") ?? throw new Exception("audience não gerada"),
                claims: claims,
                expires:expiration,
                signingCredentials: creds
            );

            return new UserToken() {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expiration = expiration
            };

        }
    }
}
