using DotNet_WebApi_Learning.Dtos.Character;
using DotNet_WebApi_Learning.Dtos.Weapon;
using DotNet_WebApi_Learning.Models;
using DotNetWebApi_Learning.Services.WeaponService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNet_WebApi_Learning.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class WeaponController : ControllerBase
    {
        private readonly IWeaponService _weaponService;

        public WeaponController(IWeaponService weaponService)
        {
            _weaponService = weaponService;
        }


        [HttpPost]
        [Route("AddWeapon")]
        public async Task<ActionResult<ServiceResponse<GetCharacterDto>>> AddWeapon(AddWeaponDto request)
        {
            var response = await _weaponService.AddWeapon(request);

            if(response.Success == false)
            {
                return BadRequest(response);
            }

            return Ok(response);        
        }




    }
}
