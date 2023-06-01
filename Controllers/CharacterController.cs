using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;

namespace dotnet_rpg.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CharacterController : ControllerBase
    {
        private readonly ICharacterService characterService;

        public CharacterController(ICharacterService characterService)
        {
            this.characterService = characterService;
        }

        [HttpGet("GetAll")]
        public async Task <ActionResult<ServiceResponse<List<GetCharacterDto>>>> Get() 
        {
            return Ok(await characterService.GetAllCharacters());
        }

        [HttpGet("{id}")]
        public async Task <ActionResult<ServiceResponse<GetCharacterDto>>> GetSingleCharacter(int id) 
        {
            return Ok(await characterService.GetCharacterById(id));
        }

        [HttpPost]
        public async Task <ActionResult<ServiceResponse<List<GetCharacterDto>>>> AddCharacter (AddCharacterDto newCharacter)
        {
            return Ok(await characterService.AddCharacter(newCharacter));
        }

        [HttpPut]
        public async Task <ActionResult<ServiceResponse<List<GetCharacterDto>>>> UpdateCharacter (UpdateCharacterDto updatedCharacter)
        {
            var response = await characterService.UpdateCharacter(updatedCharacter);
            if (response.Data is null)
            {
                return NotFound(response);
            }
            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task <ActionResult<ServiceResponse<GetCharacterDto>>> DeleteCharacter(int id) 
        {
            var response = await characterService.DeleteCharacterById(id);
            if (response.Data is null)
            {
                return NotFound(response);
            }
            return Ok(response);
        }
    }
}