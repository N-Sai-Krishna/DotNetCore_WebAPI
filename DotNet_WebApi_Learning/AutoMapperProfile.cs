using AutoMapper;
using DotNet_WebApi_Learning.Dtos.Character;
using DotNet_WebApi_Learning.Dtos.Fight;
using DotNet_WebApi_Learning.Dtos.Skill;
using DotNet_WebApi_Learning.Dtos.Weapon;
using DotNet_WebApi_Learning.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNet_WebApi_Learning
{
    public class AutoMapperProfile: Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Character, GetCharacterDto>();
            CreateMap<AddCharacterDto, Character>();
            CreateMap<AddWeaponDto, Weapon>();
            CreateMap<Weapon,GetWeaponDto>();
            CreateMap<Skill, GetSkillDto>();
            CreateMap<Character, HighScoreDto>();
        }
    }
}
