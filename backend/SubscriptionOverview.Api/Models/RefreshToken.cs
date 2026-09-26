using SubscriptionOverview.Api.Models.Identity;
using System.ComponentModel.DataAnnotations;

namespace SubscriptionOverview.Api.Models
{
    public class RefreshToken
    {
        public int Id { get; set; }
        public string UserId { get; set; } = string.Empty;
        public ApplicationUser User { get; set; } = null!;
        [MaxLength(256)]
        public string TokenHash { get; set; } = string.Empty;
        public DateTime ExpiresAt { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? RevokedAt { get; set; }
        [MaxLength(256)]
        public string? ReplacedByTokenHash { get; set; }


    }
}
