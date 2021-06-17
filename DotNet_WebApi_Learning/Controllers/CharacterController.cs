using DotNet_WebApi_Learning.Dtos.Character;
using DotNet_WebApi_Learning.Models;
using DotNet_WebApi_Learning.Services.CharacterService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DotNet_WebApi_Learning.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class CharacterController : ControllerBase
    {
        ////private static Character knight = new Character();
        //private static List<Character> characters = new List<Character>()
        //{
        //new Character(),
        //new Character{ Id = 1, Name = "Jackie"}
        //};

        private ICharacterService _characterService;

        public CharacterController(ICharacterService characterService)
        {
            _characterService = characterService;
        }

        //[AllowAnonymous]
        [HttpGet]
        [Route("GetAll")]
        public async Task<ActionResult<ServiceResponse<List<Character>>>> Get()
        {
            //since we use Controllerbase we can get the  user name from the claims which we injected as part of jwt token creation
            int id = int.Parse(this.User.Claims.FirstOrDefault(s => s.Type == ClaimTypes.NameIdentifier).Value);
           //so with the help of this id now we can et all the characters related to that particular user
            return Ok(await _characterService.GetAllCharacters(id));  
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ServiceResponse<Character>>> GetSingle(int id)
        {
            return Ok(await _characterService.GetCharacterById(id));
        }

        [HttpPost]
        public async Task<ActionResult<ServiceResponse<List<Character>>>> AddCharacter(AddCharacterDto newCharacter)
        {
            return Ok(await _characterService.AddCharacter(newCharacter));
        }

        [HttpPut]
        public async Task<ActionResult<ServiceResponse<GetCharacterDto>>> UpdateCharacter(UpdateCharacterDto updatedCharacter)
        {
            var response = await _characterService.UpdateCharacter(updatedCharacter);
            if(response.Data == null)
            {
                return NotFound(response);
            }
            
            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ServiceResponse<List<GetCharacterDto>>>> DeleteCharacter(int id)
        {
            var response = await _characterService.DeleteCharacter(id);

            if (response.Success == false)
            {
                return NotFound(response);
            }
            return Ok(response);
        }


    }
}
