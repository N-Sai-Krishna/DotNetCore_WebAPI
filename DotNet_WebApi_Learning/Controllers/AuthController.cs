using DotNet_WebApi_Learning.Data;
using DotNet_WebApi_Learning.Dtos.User;
using DotNet_WebApi_Learning.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNet_WebApi_Learning.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _authRepository;

        public AuthController(IAuthRepository authRepository)
        {
            _authRepository = authRepository;
        }

        [HttpPost("Register")]
        public async Task<ActionResult<ServiceResponse<int>>> Register(UserRegisterDto request)
        {
            var response =  await _authRepository.Register(new User { Username = request.Username} , request.Password);

            if (response.Success == false)
            {
                return BadRequest(response);
            }
            
            return Ok(response);
        }

        [HttpPost("Login")]
        public async Task<ActionResult<ServiceResponse<string>>> Login(UserLoginDto request)
        {
            var response = await _authRepository.Login(request.Username, request.Password);

            if (response.Success == false)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

    }
}
