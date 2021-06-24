using AutoMapper;
using DotNet_WebApi_Learning.Data;
using DotNet_WebApi_Learning.Dtos.Character;
using DotNet_WebApi_Learning.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DotNet_WebApi_Learning.Services.CharacterService
{
    public class CharacterSerice : ICharacterService
    {

        //private static List<Character> characters = new List<Character>()
        //{
        //new Character(),
        //new Character{ Id = 1, Name = "Jackie"}
        //};

        private readonly IMapper _mapper;
        private readonly DataContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        private int GetUserId() => int.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));

        private string GetUserRole() => _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Role);
        public CharacterSerice(IMapper mapper, DataContext context, IHttpContextAccessor httpContextAccessor)
        {
            _mapper = mapper;
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<ServiceResponse<List<GetCharacterDto>>> AddCharacter(AddCharacterDto newCharacter)
        {
            var serviceResponse = new ServiceResponse<List<GetCharacterDto>>();
            //int newId = characters.Max(s => s.Id) + 1;
            //character.Id = newId;
            //characters.Add(character);

            var character = _mapper.Map<Character>(newCharacter);

            character.User = await _context.Users.FirstOrDefaultAsync(s => s.Id == GetUserId());
                
             _context.Characters.Add(character);

            await _context.SaveChangesAsync();

            serviceResponse.Data = await _context.Characters.Where(s=>s.User.Id == GetUserId()).Select(s => _mapper.Map<GetCharacterDto>(s)).ToListAsync();

            return serviceResponse;
        }

        public async Task<ServiceResponse<List<GetCharacterDto>>> GetAllCharacters()
        {
            var serviceResponse = new ServiceResponse<List<GetCharacterDto>>();

            var dbCharacters = 
              GetUserRole().Equals("Admin") ?
              await _context.Characters.Include(c => c.Skills)
                            .Include(c => c.Weapon).ToListAsync()
            : await _context.Characters.Include(c => c.Skills)
                            .Include(c => c.Weapon).Where(s => s.User.Id == GetUserId()).ToListAsync();

            serviceResponse.Data = dbCharacters.Select(s => _mapper.Map<GetCharacterDto>(s)).ToList();
            return serviceResponse; 
        }

        public async Task<ServiceResponse<GetCharacterDto>> GetCharacterById(int id)
        {
            var serviceResponse = new ServiceResponse<GetCharacterDto>();

            var dbCharacter = await _context.Characters.Include(c=>c.Skills)
                            .Include(c=>c.Weapon)
                            .FirstOrDefaultAsync(s => s.Id == id && s.User.Id == GetUserId());

            serviceResponse.Data = _mapper.Map<GetCharacterDto>(dbCharacter);
            return serviceResponse;
        }

        public async Task<ServiceResponse<GetCharacterDto>> UpdateCharacter(UpdateCharacterDto updatedCharacter)
        {
            var serviceResponse = new ServiceResponse<GetCharacterDto>();

            try 
            {
                Character character = await _context.Characters.Include(c => c.User).FirstOrDefaultAsync(s => s.Id == updatedCharacter.Id);

                if (character.User.Id == GetUserId())
                { 
                    character.Name = updatedCharacter.Name;
                    character.HitPoints = updatedCharacter.HitPoints;
                    character.Strength = updatedCharacter.Strength;
                    character.Defence = updatedCharacter.Defence;
                    character.Intelligence = updatedCharacter.Intelligence;
                    character.Class = updatedCharacter.Class;

                    await _context.SaveChangesAsync();

                    serviceResponse.Data = _mapper.Map<GetCharacterDto>(character);
                }
                else
                {
                    serviceResponse.Success = false;
                    serviceResponse.Message = "The character is not found .";
                    return serviceResponse;
                }

            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }
           
            return serviceResponse;
        }

        public async Task<ServiceResponse<List<GetCharacterDto>>> DeleteCharacter(int id)
        {
            var serviceResponse = new ServiceResponse<List<GetCharacterDto>>();
            try
            {
                Character character = await _context.Characters.FirstOrDefaultAsync(s => s.Id == id && s.User.Id == GetUserId());

                if (character != null)
                {
                    _context.Characters.Remove(character);
                    //characters.Remove(character);
                    await _context.SaveChangesAsync();
                    serviceResponse.Data = await _context.Characters.Where(c => c.User.Id == GetUserId()).Select(s => _mapper.Map<GetCharacterDto>(s)).ToListAsync();
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

        public async Task<ServiceResponse<GetCharacterDto>> AddCharacterSkill(AddCharacterSkillDto newCharacterSkill)
        {
            var serviceResponse = new ServiceResponse<GetCharacterDto>();

            try
            {
                var character = await _context.Characters
                                .Include(c => c.Weapon)
                                .Include(c => c.Skills)
                                .FirstOrDefaultAsync(s => s.Id == newCharacterSkill.CharacterId 
                                && s.User.Id == GetUserId());
                
                if (character == null)
                {
                    serviceResponse.Success = false;
                    serviceResponse.Message = "Character not found.";
                    return serviceResponse;
                }

                var skill = await _context.Skills.FirstOrDefaultAsync(s => s.Id == newCharacterSkill.SkillId);

                if (skill == null)
                {
                    serviceResponse.Success = false;
                    serviceResponse.Message = "Skill not found.";
                    return serviceResponse;
                }

                character.Skills.Add(skill);
                await _context.SaveChangesAsync();

                serviceResponse.Data = _mapper.Map<GetCharacterDto>(character);
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

