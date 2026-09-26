using System.ComponentModel.DataAnnotations;

namespace SubscriptionOverview.Api.DTOs.ProviderDto
{
    public class ProviderRequestDto
    {

        [Required(ErrorMessage = "Service name is required")]
        [MaxLength(100, ErrorMessage = "Service name cannot exceed 100 characters")]
        public string ServiceName { get; set; } = string.Empty;

    }
}
