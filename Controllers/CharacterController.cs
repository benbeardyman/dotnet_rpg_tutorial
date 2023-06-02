using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace dotnet_rpg.Controllers
{
    [Authorize]
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
            int userId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)!.Value);
            return Ok(await characterService.GetAllCharacters(userId));
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