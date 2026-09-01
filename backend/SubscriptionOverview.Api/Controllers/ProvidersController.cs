using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SubscriptionOverview.Api.DTOs.ProviderDto;
using SubscriptionOverview.Api.Exceptions;
using SubscriptionOverview.Api.Services.ProviderServices;
using System.Security.Claims;

namespace SubscriptionOverview.Api.Controllers
{

    [ApiController]
    [Route("api/providers")]
    [Authorize]
    public class ProvidersController : ControllerBase
    {
        private readonly IProviderService _providerService;

        public ProvidersController(IProviderService providerService)
        {
            _providerService = providerService;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProviderDto>>> GetAllProviders()
        {

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                throw new UnauthorizedException("User not authenticated.");
            }
            var providers = await _providerService.GetAllProvidersAsync(userId);
            return Ok(providers);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProviderDto>> GetProviderById(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                throw new UnauthorizedException("User not authenticated.");
            }

            var provider = await _providerService.GetProviderByIdAsync(id, userId);
            if (provider == null)
            {
                return NotFound();
            }

            return Ok(provider);
        }

        [HttpPost]
        public async Task<ActionResult<ProviderDto>> AddProvider([FromBody] ProviderRequestDto providerDto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                throw new UnauthorizedException("User not authenticated.");
            }

            var provider = await _providerService.CreateProviderAsync(userId, providerDto);
            return CreatedAtAction(nameof(GetProviderById), new { id = provider.Id }, provider);
        }


        [HttpPut("{id}")]
        public async Task<ActionResult<ProviderDto>> UpdateProvider(int id, [FromBody] ProviderRequestDto providerDto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                throw new UnauthorizedException("User not authenticated.");
            }
            var provider = await _providerService.UpdateProviderAsync(id, userId, providerDto);
            return Ok(provider);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProvider(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                throw new UnauthorizedException("User not authenticated.");
            }
            await _providerService.DeleteProviderAsync(id, userId);
            return NoContent();
        }
    }
}
