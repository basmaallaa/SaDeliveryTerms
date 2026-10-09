using DeliveryTermsBL.IServices.IAuthService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DeliveryTermsSL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class AuthController : ControllerBase
    {
        private readonly IJwtTokenService _tokens;
        private readonly IConfiguration _config;
        public AuthController(IJwtTokenService jwtTokenService, IConfiguration config)
        {
            _tokens = jwtTokenService;
            _config = config;
        }

        public record LoginRequest(string Username, string Password);

        [HttpPost("login")]
        public IActionResult Login(LoginRequest req)
        {
            if(req.Username != _config["DevLogin:Username"] || req.Password != _config["DevLogin:Password"])
            {
                return Unauthorized();
            }

            return Ok(new { token = _tokens.CreateToken(1, 1, req.Username) });
        }
    }
}
