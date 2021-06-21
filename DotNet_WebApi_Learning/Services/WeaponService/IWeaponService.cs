using DotNet_WebApi_Learning.Dtos.Character;
using DotNet_WebApi_Learning.Dtos.Weapon;
using DotNet_WebApi_Learning.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetWebApi_Learning.Services.WeaponService
{
    public interface IWeaponService
    {
        Task<ServiceResponse<GetCharacterDto>> AddWeapon(AddWeaponDto newWeapon);


    }
}
