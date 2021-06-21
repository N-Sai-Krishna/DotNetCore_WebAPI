using AutoMapper;
using DotNet_WebApi_Learning.Data;
using DotNet_WebApi_Learning.Dtos.Character;
using DotNet_WebApi_Learning.Dtos.Weapon;
using DotNet_WebApi_Learning.Models;
using DotNetWebApi_Learning.Services.WeaponService;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DotNet_WebApi_Learning.Services.WeaponService
{
    public class WeaponService : IWeaponService
    {
        private readonly DataContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;

        public WeaponService(DataContext context, IHttpContextAccessor httpContextAccessor, IMapper mapper)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
        }

        private int GetUserId() => int.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));

        public async Task<ServiceResponse<GetCharacterDto>> AddWeapon(AddWeaponDto newWeapon)
        {
            var serviceResponse = new ServiceResponse<GetCharacterDto>();

            try
            { 
                var character = await _context.Characters.FirstOrDefaultAsync(s => s.Id == newWeapon.CharacterId && s.User.Id == GetUserId());

                if (character != null)
                {
                    //_context.Weapons.Add(_mapper.Map<Weapon>(newWeapon));

                    var weapon = new Weapon
                    {
                        Name = newWeapon.Name,
                        Damage = newWeapon.Damage,
                        Character = character
                    };
 
                    _context.Weapons.Add(weapon);

                    await _context.SaveChangesAsync();

                    var response = _mapper.Map<GetCharacterDto>(character);
                    response.Weapon = _mapper.Map<GetWeaponDto>(weapon);

                    serviceResponse.Data = response;
                }
                else
                {
                    serviceResponse.Success = false;
                    serviceResponse.Message = "The Character is not found";
                }

            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }

          

            return serviceResponse;

        }
    }
}
