using SubscriptionOverview.Api.DTOs.ProviderDto;
using SubscriptionOverview.Api.Exceptions;
using SubscriptionOverview.Api.Models;
using SubscriptionOverview.Api.Repositories.ProviderRepositories;

namespace SubscriptionOverview.Api.Services.ProviderServices
{
    public class ProviderService : IProviderService
    {

        private readonly IProviderRepository _providerRepository;
        public ProviderService(IProviderRepository providerRepository)
        {
            _providerRepository = providerRepository;
        }
        public async Task<ProviderDto> CreateProviderAsync(string userId, ProviderRequestDto providerRequest)
        {
            var existingProvider = await _providerRepository.ExistsByNameAsync(providerRequest.ServiceName, userId);
            if (existingProvider)
            {
                throw new ConflictException("A provider with the specified name already exists.");
            }
            var provider = new Provider
            {
                ServiceName = providerRequest.ServiceName,
                UserId = userId,
                IsCustom = true
            };
            await _providerRepository.AddProviderAsync(provider);

            var result = await _providerRepository.SaveChangesAsync();

            if (!result)
            {
                throw new InvalidOperationException("Failed to save provider");
            }

            return MapToProviderDto(provider);
        }

        public async Task<bool> DeleteProviderAsync(int id, string userId)
        {
            var provider = await _providerRepository.GetCustomProviderByIdAsync(id, userId);

            if (provider == null)
            {
                throw new NotFoundException("Provider not found.");
            }

           _providerRepository.DeleteProvider(provider);

            var result = await _providerRepository.SaveChangesAsync();

            if (!result)
            {
                throw new InvalidOperationException("Failed to delete provider");
            }
            return result;
        }

        public async Task<IEnumerable<ProviderDto>> GetAllProvidersAsync(string userId)
        {
            var providers = await _providerRepository.GetAllProvidersAsync(userId);
            var providerDtos = providers.Select(MapToProviderDto);
            return providerDtos;

        }

        public async Task<ProviderDto> GetProviderByIdAsync(int id, string userId)
        {
            var provider = await _providerRepository.GetProviderByIdAsync(id, userId);

            if (provider == null)
            {
                throw new NotFoundException("Provider not found.");
            }
            return MapToProviderDto(provider);
        }

        public async Task<ProviderDto> UpdateProviderAsync(int id, string userId, ProviderRequestDto providerRequest)
        {
           var provider = await _providerRepository.GetCustomProviderByIdAsync(id, userId);
            if (provider == null)
            {
                throw new NotFoundException("Provider not found.");
            }

            if (string.Equals(provider.ServiceName, providerRequest.ServiceName, StringComparison.OrdinalIgnoreCase))
            {
                return MapToProviderDto(provider);
            }

            var existingProvider = await _providerRepository.ExistsByNameAsync(providerRequest.ServiceName, userId);

            if (existingProvider)
            {
                throw new ConflictException("A provider with the specified name already exists.");
            }

            provider.ServiceName = providerRequest.ServiceName;
            _providerRepository.UpdateProvider(provider);
            var result = await _providerRepository.SaveChangesAsync();
            if (!result)
            {
                throw new InvalidOperationException("Failed to update provider");
            }
            return MapToProviderDto(provider);
        }


        private static ProviderDto MapToProviderDto(Provider provider)
        {
            return new ProviderDto
            {
                Id = provider.Id,
                ServiceName = provider.ServiceName,
                IsCustom = provider.IsCustom
            };
        }
    }
}
