using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dotnet_rpg.Services.WeaponService;

namespace dotnet_rpg.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class WeaponController : ControllerBase
    {
        private readonly IWeaponService weaponService;

        public WeaponController(IWeaponService weaponService)
        {
            this.weaponService = weaponService;
        }

        [HttpPost]
        public async Task<ActionResult<ServiceResponse<GetCharacterDto>>> AddWeapon(AddWeaponDto newWeapon)
        {
            return Ok(await weaponService.AddWeapon(newWeapon));
        }
    }
}