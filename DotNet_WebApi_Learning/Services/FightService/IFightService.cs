using DotNet_WebApi_Learning.Dtos.Fight;
using DotNet_WebApi_Learning.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNet_WebApi_Learning.Services.FightService
{
    public interface IFightService
    {
        Task<ServiceResponse<AttackResultDto>> WeaponAttack(WeaponAttackDto weaponAttack);
        Task<ServiceResponse<AttackResultDto>> SkillAttack(SkillAttackDto skillAttack); 
        Task<ServiceResponse<FightResultDto>> Fight(FightRequestDto fightRequest);
        Task<ServiceResponse<List<HighScoreDto>>> GetHighscore();

    }
}
