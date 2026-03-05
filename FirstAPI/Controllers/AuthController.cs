using AutoMapper;
using FirstAPI.DTO;
using FirstAPI.Models;
using FirstAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FirstAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authServie;
        private readonly IMapper _mapper;

        public AuthController(AuthService authService, IMapper mapper)
        {
            _authServie = authService;
            _mapper = mapper;
        }

        [HttpPost("register")]
        public async Task<APIResponse<User>> RegisterUser(UserRequest request)
        {
            var result = await _authServie.Register(request);
            return result;
        }

        [HttpPost("login")]
        public async Task<APIResponse<dynamic>> Login(UserRequest request)
        {
            var result = await _authServie.Login(request);
            return result;
        }

        [HttpPost("RefreshToken")]
        public async Task<APIResponse<dynamic>> RefreshToken(RefreshTokenReq request)
        {
            var result = await _authServie.RefreshToken(request);
            return result;
        }
    }
}
