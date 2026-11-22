using Microsoft.AspNetCore.Mvc;
using RetroVault.Server.Models;
using RetroVault.Server.Services;

namespace RetroVault.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController(CosmosService cosmosService) : ControllerBase
    {
        private readonly CosmosService _cosmosService = cosmosService;

        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] RetroVaultUser user)
        {
            bool success = await _cosmosService.AddUserAsync(user);
            if (!success)
            {
                return BadRequest("Failed to create user.");
            }
            return Created();
        }
    }
}
