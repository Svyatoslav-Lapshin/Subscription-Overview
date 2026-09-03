using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SubscriptionOverview.Api.DTOs.SubscriptionsDto;
using SubscriptionOverview.Api.Exceptions;
using SubscriptionOverview.Api.Services.SubscriptionServices;
using System.Security.Claims;

namespace SubscriptionOverview.Api.Controllers
{

    [ApiController]
    [Route("api/subscriptions")]
    [Authorize]

    public class SubscriptionsController:ControllerBase
    {
        private readonly ISubscriptionService _subscriptionService;
        public SubscriptionsController(ISubscriptionService subscriptionService)
        {
            _subscriptionService = subscriptionService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<SubscriptionDto>>> GetAllSubscriptions()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
            {
                throw new UnauthorizedException("User not authenticated.");
            }

            var subscriptions = await _subscriptionService.GetAllSubscriptionsAsync(userId);
            return Ok(subscriptions);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<SubscriptionDto>> GetSubscriptionById(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
            {
                throw new UnauthorizedException("User not authenticated.");
            }
            var subscription = await _subscriptionService.GetSubscriptionByIdAsync(id, userId);
            return Ok(subscription);
        }

        [HttpPost]
        public async Task<ActionResult<SubscriptionDto>> CreateSubscription([FromBody] SubscriptionRequestDto createSubscriptionDto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
            {
                throw new UnauthorizedException("User not authenticated.");
            }

            var subscription = await _subscriptionService.AddAsync(userId,createSubscriptionDto);
            return CreatedAtAction(nameof(GetSubscriptionById), new { id = subscription.Id }, subscription);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<SubscriptionDto>> UpdateSubscription(int id, [FromBody] SubscriptionRequestDto updateSubscriptionDto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
            {
                throw new UnauthorizedException("User not authenticated.");
            }
            var subscription = await _subscriptionService.UpdateAsync(id, userId, updateSubscriptionDto);           
            return Ok(subscription);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteSubscription(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
            {
                throw new UnauthorizedException("User not authenticated.");
            }

            await _subscriptionService.DeleteAsync(id, userId);
            return NoContent();
        }


        [HttpGet("summary")]
        public async Task<ActionResult<SubscriptionSummaryDto>> GetSubscriptionsSummary()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
            {
                throw new UnauthorizedException("User not authenticated.");
            }

            var subscriptions = await _subscriptionService.GetSummaryAsync(userId);
            return Ok(subscriptions);
        }
    }
}
