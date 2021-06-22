using DotNet_WebApi_Learning.Dtos.Skill;
using DotNet_WebApi_Learning.Dtos.Weapon;
using DotNet_WebApi_Learning.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNet_WebApi_Learning.Dtos.Character
{
    public class GetCharacterDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = "Bruce";
        public int HitPoints { get; set; } = 100;
        public int Strength { get; set; } = 10;
        public int Defence { get; set; } = 10;
        public int Intelligence { get; set; } = 10;
        public RpgClass Class { get; set; } = RpgClass.Knight;

        public GetWeaponDto Weapon { get; set; }

        public List<GetSkillDto> Skills { get; set; }

        public int Fights { get; set; }

        public int Victories { get; set; }

        public int Defeats { get; set; }
    }
}
