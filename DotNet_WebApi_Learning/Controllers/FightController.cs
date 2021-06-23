using DotNet_WebApi_Learning.Dtos.Fight;
using DotNet_WebApi_Learning.Models;
using DotNet_WebApi_Learning.Services.FightService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNet_WebApi_Learning.Controllers
{
    //[Authorize]
    [ApiController]
    [Route("[controller]")]
    public class FightController : ControllerBase
    {
        private readonly IFightService _fightService;

        public FightController(IFightService fightService)
        {
            _fightService = fightService;
        }

        [HttpPost]
        [Route("Attack")]
        public async Task<ActionResult<ServiceResponse<AttackResultDto>>> WeaponAttack(WeaponAttackDto weaponAttack)
        {
            var response = await _fightService.WeaponAttack(weaponAttack);

            if (response.Success == false)
            {
                return BadRequest(response);                    
            }
            
            return Ok(response);
        }


        [HttpPost]
        [Route("Skill")]
        public async Task<ActionResult<ServiceResponse<AttackResultDto>>> SkillAttack(SkillAttackDto request)
        {
            var response = await _fightService.SkillAttack(request);

            if (response.Success == false)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        [HttpPost]
        [Route("Fight")]
        public async Task<ActionResult<ServiceResponse<AttackResultDto>>> Fight(FightRequestDto request)
        {
            var response = await _fightService.Fight(request);

            if (response.Success == false)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        [HttpGet]
        [Route("Highscore")]
        public async Task<ActionResult<ServiceResponse<List<HighScoreDto>>>> HighScore() 
        {
            return Ok(await _fightService.GetHighscore());
        }
    }
}
