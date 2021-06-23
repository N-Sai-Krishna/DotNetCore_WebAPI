using AutoMapper;
using DotNet_WebApi_Learning.Data;
using DotNet_WebApi_Learning.Dtos.Fight;
using DotNet_WebApi_Learning.Models;
using DotNet_WebApi_Learning.Services.CharacterService;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNet_WebApi_Learning.Services.FightService
{
    public class FightService : IFightService
    {
        private readonly DataContext _context;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly ICharacterService _characterService;
        private readonly IMapper _mapper;

        public FightService(DataContext context, IHttpContextAccessor contextAccessor
            , ICharacterService characterService, IMapper mapper)
        {
            _context = context;
            _contextAccessor = contextAccessor;
            _characterService = characterService;
            _mapper = mapper;
        }

        public async Task<ServiceResponse<FightResultDto>> Fight(FightRequestDto fightRequest)
        {
            var response = new ServiceResponse<FightResultDto>()
            {
                Data = new FightResultDto()
            };

            try
            {
                var characters = await _context.Characters
                            .Include(c => c.Weapon)
                            .Include(c => c.Skills)
                            .Where(c => fightRequest.CharacterIds.Contains(c.Id)).ToListAsync();
                bool defeated = false;
                while (!defeated)
                {
                    foreach (var attacker in characters)
                    {
                        var opponents = characters.Where(s => s.Id != attacker.Id).ToList();
                        var opponent = opponents[new Random().Next(opponents.Count)];

                        int damage = 0;

                        string attackUsed = string.Empty;

                        bool useWeapon = new Random().Next(2) == 0;

                        if (useWeapon)
                        {
                            attackUsed = attacker.Weapon.Name;
                            damage = DoWeaponAttack(attacker, opponent); 
                        }
                        else 
                        {
                            var skill = attacker.Skills[new Random().Next(attacker.Skills.Count)];
                            damage = DoSkillAttack(attacker, opponent, skill);
                        }

                        response.Data.Log.Add($"{attacker.Name} attacks {opponent.Name} using {attackUsed} with {(damage >=0 ? damage : 0)} damage.");

                        if (opponent.HitPoints <= 0)
                        {
                            defeated = true;
                            attacker.Victories++;
                            opponent.Defeats++;
                            response.Data.Log.Add($"{opponent.Name} has been defeated");
                            response.Data.Log.Add($"{attacker.Name} wins with {attacker.HitPoints} HP left.");
                            break;
                        }
                    }
                }

                characters.ForEach(s =>
                {
                    s.Fights++;
                    s.HitPoints = 100;
                });

                await _context.SaveChangesAsync(); 
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public async Task<ServiceResponse<AttackResultDto>> SkillAttack(SkillAttackDto skillAttack)
        {
            var response = new ServiceResponse<AttackResultDto>();

            try
            {
                var attacker = await _context.Characters.Include(c => c.Skills)
                                   .FirstOrDefaultAsync(c => c.Id == skillAttack.AttackerId);


                var opponent = await _context.Characters
                                .FirstOrDefaultAsync(c => c.Id == skillAttack.OpponentId);

                var skill = attacker.Skills.FirstOrDefault(s => s.Id == skillAttack.SkillId);

                if (skill == null)
                {
                    response.Success = false;
                    response.Message = "The Character doesn't have the skill";
                    return response;
                }

                int damage = DoSkillAttack(attacker, opponent, skill);

                if (opponent.HitPoints <= 0)
                {
                    response.Message = $"{opponent.Name} has been defeated";
                }

                await _context.SaveChangesAsync();

                response.Data = new AttackResultDto()
                {
                    Attacker = attacker.Name,
                    AttackerHP = attacker.HitPoints,
                    Damage = damage,
                    Opponent = opponent.Name,
                    OpponentHP = opponent.HitPoints
                };
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }
            
            return response;
        }

        private static int DoSkillAttack(Character attacker, Character opponent, Skill skill)
        {
            int damage = skill.Damage + (new Random().Next(attacker.Intelligence));

            damage -= new Random().Next(opponent.Defence);

            if (damage > 0)
            {
                opponent.HitPoints -= damage;
            }

            return damage;
        }

        public async Task<ServiceResponse<AttackResultDto>> WeaponAttack(WeaponAttackDto weaponAttack)
        {
            var response = new ServiceResponse<AttackResultDto>();

            try
            {
                var attacker = await _context.Characters.Include(c => c.Weapon)
                                .FirstOrDefaultAsync(c => c.Id == weaponAttack.AttackerId);


                var opponent = await _context.Characters
                                .FirstOrDefaultAsync(c => c.Id == weaponAttack.OpponentId);
                int damage = DoWeaponAttack(attacker, opponent);

                if (opponent.HitPoints <= 0)
                {
                    response.Message = $"{opponent.Name} has been defeated";
                }

                await _context.SaveChangesAsync();

                response.Data = new AttackResultDto()
                {
                    Attacker = attacker.Name,
                    AttackerHP = attacker.HitPoints,
                    Damage = damage,
                    Opponent = opponent.Name,
                    OpponentHP = opponent.HitPoints
                };
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }
            return response;
        }

        private static int DoWeaponAttack(Character attacker, Character opponent)
        {
            int damage = attacker.Weapon.Damage + (new Random().Next(attacker.Strength));

            damage -= new Random().Next(opponent.Defence);

            if (damage > 0)
            {
                opponent.HitPoints -= damage;
            }

            return damage;
        }

        public async Task<ServiceResponse<List<HighScoreDto>>> GetHighscore()
        {
            var response = new ServiceResponse<List<HighScoreDto>>();
            
            try 
            {
                var characters = await _context.Characters.Where(c => c.Fights > 0)
                                   .OrderByDescending(c => c.Victories)
                                   .ThenBy(c => c.Defeats)
                                   .ToListAsync();


                response.Data = characters.Select(s => _mapper.Map<HighScoreDto>(s)).ToList();
            }
            catch(Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }
    }
}
 