using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dotnet_rpg.Services.CharacterService
{
    public class CharacterService : ICharacterService
    {
        private readonly IMapper mapper;
        private readonly DataContext context;
        private readonly IHttpContextAccessor httpContextAccessor;

        public CharacterService(IMapper mapper, DataContext context, IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
            this.context = context;
            this.mapper = mapper;
        }

        private int GetUserId() => int.Parse(httpContextAccessor.HttpContext!.User
            .FindFirstValue(ClaimTypes.NameIdentifier)!);

        public async Task <ServiceResponse<List<GetCharacterDto>>> AddCharacter(AddCharacterDto newCharacter)
        {
            var serviceResponse = new ServiceResponse<List<GetCharacterDto>>();
            var character = mapper.Map<Character>(newCharacter);
            character.User = await context.Users.FirstOrDefaultAsync(u => u.Id == GetUserId());

            context.Characters.Add(character);
            await context.SaveChangesAsync();

            serviceResponse.Data = 
                await context.Characters
                    .Where(c => c.User!.Id == GetUserId())
                    .Select(c => mapper.Map<GetCharacterDto>(c))
                    .ToListAsync();
            return serviceResponse;
        }

        public async Task<ServiceResponse<List<GetCharacterDto>>> DeleteCharacterById(int id)
        {
            var serviceResponse = new ServiceResponse<List<GetCharacterDto>>();

            try
            {
                var character = await context.Characters
                    .FirstOrDefaultAsync(c => c.Id == id && c.User!.Id == GetUserId());
                if(character is null)
                    throw new Exception($"Character with Id '{id}' not found");

                context.Characters.Remove(character);

                await context.SaveChangesAsync();

                serviceResponse.Data = 
                    await context.Characters
                        .Where (c => c.User!.Id == GetUserId())
                        .Select(c => mapper.Map<GetCharacterDto>(c)).ToListAsync();
            }
            catch (Exception ex) 
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }

            return serviceResponse;
        }

        public async Task <ServiceResponse<List<GetCharacterDto>>> GetAllCharacters()
        {
            var serviceResponse = new ServiceResponse<List<GetCharacterDto>>();
            var dbCharacters = await context.Characters.Where(c => c.User!.Id == GetUserId()).ToListAsync();
            serviceResponse.Data = dbCharacters.Select(c => mapper.Map<GetCharacterDto>(c)).ToList();
            return serviceResponse;
        }

        public async Task <ServiceResponse<GetCharacterDto>> GetCharacterById(int id)
        {
            var serviceResponse = new ServiceResponse<GetCharacterDto>();
            var dbCharacter = await context.Characters
                .FirstOrDefaultAsync(c => c.Id == id && c.User!.Id == GetUserId());
            serviceResponse.Data = mapper.Map<GetCharacterDto>(dbCharacter);
            return serviceResponse;
        }

        public async Task<ServiceResponse<GetCharacterDto>> UpdateCharacter(UpdateCharacterDto updatedCharacter)
        {
            var serviceResponse = new ServiceResponse<GetCharacterDto>();

            try
            {
                var character = 
                    await context.Characters
                        .Include(c => c.User)
                        .FirstOrDefaultAsync(c => c.Id == updatedCharacter.Id);
                if(character is null || character.User!.Id != GetUserId())
                    throw new Exception($"Character with Id '{updatedCharacter.Id}' not found");

                character.Name = updatedCharacter.Name;
                character.HitPoints = updatedCharacter.HitPoints;
                character.Strength = updatedCharacter.Strength;
                character.Defence = updatedCharacter.Defence;
                character.Intelligence = updatedCharacter.Intelligence;
                character.Class = updatedCharacter.Class;

                await context.SaveChangesAsync();
                serviceResponse.Data = mapper.Map<GetCharacterDto>(character);
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