using AutoMapper;
using DotNet_WebApi_Learning.Data;
using DotNet_WebApi_Learning.Dtos.Character;
using DotNet_WebApi_Learning.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public CharacterSerice(IMapper mapper, DataContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<ServiceResponse<List<GetCharacterDto>>> AddCharacter(AddCharacterDto newCharacter)
        {
            var serviceResponse = new ServiceResponse<List<GetCharacterDto>>();
            //int newId = characters.Max(s => s.Id) + 1;
            var character = _mapper.Map<Character>(newCharacter);

            //character.Id = newId;
            //characters.Add(character);
             _context.Characters.Add(character);

            await _context.SaveChangesAsync();

            serviceResponse.Data = await _context.Characters.Select(s => _mapper.Map<GetCharacterDto>(s)).ToListAsync();

            return serviceResponse;
        }

        public async Task<ServiceResponse<List<GetCharacterDto>>> GetAllCharacters(int Id)
        {
            var serviceResponse = new ServiceResponse<List<GetCharacterDto>>();

            var dbCharacters = await _context.Characters.Where(s => s.User.Id == Id).ToListAsync();

            serviceResponse.Data = dbCharacters.Select(s => _mapper.Map<GetCharacterDto>(s)).ToList();
            return serviceResponse; 
        }

        public async Task<ServiceResponse<GetCharacterDto>> GetCharacterById(int id)
        {
            var serviceResponse = new ServiceResponse<GetCharacterDto>();

            var dbCharacter = await _context.Characters.FirstOrDefaultAsync(s => s.Id == id);

            serviceResponse.Data = _mapper.Map<GetCharacterDto>(dbCharacter);
            return serviceResponse;
        }

        public async Task<ServiceResponse<GetCharacterDto>> UpdateCharacter(UpdateCharacterDto updatedCharacter)
        {
            var serviceResponse = new ServiceResponse<GetCharacterDto>();

            try 
            {
                Character character = await _context.Characters.FirstOrDefaultAsync(s => s.Id == updatedCharacter.Id);

                character.Name = updatedCharacter.Name;
                character.HitPoints = updatedCharacter.HitPoints;
                character.Strength = updatedCharacter.Strength;
                character.Defence = updatedCharacter.Defence;
                character.Intelligence = updatedCharacter.Intelligence;
                character.Class = updatedCharacter.Class;

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

        public async Task<ServiceResponse<List<GetCharacterDto>>> DeleteCharacter(int id)
        {
            var serviceResponse = new ServiceResponse<List<GetCharacterDto>>();
            try
            {
                Character character = await _context.Characters.FirstAsync(s => s.Id == id);

                _context.Characters.Remove(character);
                //characters.Remove(character);

                await _context.SaveChangesAsync();

                serviceResponse.Data = await _context.Characters.Select(s => _mapper.Map<GetCharacterDto>(s)).ToListAsync();
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

