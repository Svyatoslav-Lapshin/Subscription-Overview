using SubscriptionOverview.Api.DTOs.SubscriptionsDto;
using SubscriptionOverview.Api.Exceptions;
using SubscriptionOverview.Api.Models;
using SubscriptionOverview.Api.Models.Enums;
using SubscriptionOverview.Api.Repositories.CategoryRepositories;
using SubscriptionOverview.Api.Repositories.ProviderRepositories;
using SubscriptionOverview.Api.Repositories.SubscriptionRepositories;


namespace SubscriptionOverview.Api.Services.SubscriptionServices
{
    public class SubscriptionService : ISubscriptionService
    {
        private readonly ISubscriptionRepository _repository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IProviderRepository _providerRepository;

        public SubscriptionService(ISubscriptionRepository repository, 
            ICategoryRepository categoryRepository, 
            IProviderRepository providerRepository)
        {
            _repository = repository;
            _categoryRepository = categoryRepository;
            _providerRepository = providerRepository;
        }

        public async Task<SubscriptionDto> AddAsync(string userId, SubscriptionRequestDto subscriptionDto)
        {

            //End date cannot be before start date.
            if (subscriptionDto.EndDate.HasValue &&
                subscriptionDto.EndDate < subscriptionDto.StartDate)
            {
                throw new AppValidationException("End date cannot be before start date.");
            }

            // Check if the category belongs to the current user.
            var category = await _categoryRepository.GetCategoryByIdAsync(subscriptionDto.CategoryId, userId);

            if (category == null)
            {
                throw new NotFoundException("Category not found.");
            }

            //Check if the provider exists for the given user.
            var provider = await _providerRepository.GetProviderByIdAsync(subscriptionDto.ProviderId, userId);

            if (provider == null)
            {
                throw new NotFoundException("Provider not found.");
            }

            //Create a new subscription entity and map the properties from the DTO.
            var subscription = new Subscription
            {
                Price = subscriptionDto.Price,
                BillingInterval = subscriptionDto.BillingInterval,
                CategoryId = subscriptionDto.CategoryId,
                ProviderId = subscriptionDto.ProviderId,
                StartDate = subscriptionDto.StartDate,
                EndDate = subscriptionDto.EndDate,
                UserId = userId
            };


            await _repository.AddSubscriptionAsync(subscription);

            var result = await _repository.SaveChangesAsync();

            if (!result)
            {
                throw new InvalidOperationException("Failed to save subscription");
            }

            //Reload the subscription to include the related category and provider entities.
            var savedSubscription = await _repository.GetSubscriptionByIdAsync(subscription.Id, userId);

            if (savedSubscription == null)
            {
                throw new InvalidOperationException("Failed to retrieve the saved subscription");
            }

            return MapToSubscriptionDto(savedSubscription);
        }

        public async Task<bool> DeleteAsync(int id, string userId)
        {
            //Check if the subscription exists for the given user.
            var subscription = await _repository.GetSubscriptionByIdAsync(id, userId);

            if (subscription == null)
            {
                throw new NotFoundException($"Subscription with ID {id} not found for the specified user.");
            }

            _repository.DeleteSubscription(subscription);
            var result = await _repository.SaveChangesAsync();

            if (!result)
            {
                throw new InvalidOperationException("Failed to delete subscription.");
            }

            return result;
        }

        public async Task<IEnumerable<SubscriptionDto>> GetAllSubscriptionsAsync(string userId)
        {
            var subscriptions = await _repository.GetAllSubscriptionsAsync(userId);
            return subscriptions.Select(MapToSubscriptionDto);
        }

        public async Task<SubscriptionDto> GetSubscriptionByIdAsync(int id, string userId)
        {
            var subscription = await _repository.GetSubscriptionByIdAsync(id, userId);

            if (subscription == null)
            {
                throw new NotFoundException($"Subscription with ID {id} not found for the specified user.");
            }

            return MapToSubscriptionDto(subscription);
        }

        public async Task<SubscriptionDto> UpdateAsync(int id, string userId, SubscriptionRequestDto subscriptionDto)
        {
            //Check if the subscription exists for the given user.
            var subscription = await _repository.GetSubscriptionByIdAsync(id, userId);

            if (subscription == null)
            {
                throw new NotFoundException($"Subscription with ID {id} not found for the specified user.");
            }

            //End date cannot be before start date.
            if (subscriptionDto.EndDate.HasValue &&
                subscriptionDto.EndDate < subscriptionDto.StartDate)
            {
                throw new AppValidationException("End date cannot be before start date.");
            }

            // Check if the category belongs to the current user.
            var category = await _categoryRepository.GetCategoryByIdAsync(subscriptionDto.CategoryId, userId);

            if (category == null)
            {
                throw new NotFoundException("Category not found.");
            }

            //Check if the provider exists for the given user.
            var provider = await _providerRepository.GetProviderByIdAsync(subscriptionDto.ProviderId, userId);

            if (provider == null)
            {
                throw new NotFoundException("Provider not found.");
            }

            subscription.Price = subscriptionDto.Price;
            subscription.BillingInterval = subscriptionDto.BillingInterval;
            subscription.CategoryId = subscriptionDto.CategoryId;
            subscription.ProviderId = subscriptionDto.ProviderId;
            subscription.StartDate = subscriptionDto.StartDate;
            subscription.EndDate = subscriptionDto.EndDate;

            _repository.UpdateSubscription(subscription);
            var result = await _repository.SaveChangesAsync();

            if (!result)
            {
                throw new InvalidOperationException("Failed to update subscription.");
            }

            //Reload the subscription to include the related category and provider entities.
            var updatedSubscription = await _repository.GetSubscriptionByIdAsync(subscription.Id, userId);

            if (updatedSubscription == null)
            {
                throw new InvalidOperationException("Failed to retrieve the updated subscription.");
            }


            return MapToSubscriptionDto(updatedSubscription);
        }


        public async Task<SubscriptionSummaryDto> GetSummaryAsync(string userId)
        {
            var subscriptions = await _repository.GetAllSubscriptionsAsync(userId);

            var today = DateOnly.FromDateTime(DateTime.Now);
            var activeSubscriptions = subscriptions
                                     .Where(s => s.StartDate <= today &&
                                     (s.EndDate == null || s.EndDate >= today))
                                     .ToList();

            var totalMonthlyCost = activeSubscriptions.Sum(s => CalculateMonthlyEquivalent(s.Price, s.BillingInterval));

            var totalYearlyCost = activeSubscriptions.Sum(s => CalculateYearlyEquivalent(s.Price, s.BillingInterval));

            var summary = new SubscriptionSummaryDto
            {
                TotalMonthlyPayments = totalMonthlyCost,
                TotalYearlyPayments = totalYearlyCost,
                ActiveSubscriptionsCount = activeSubscriptions.Count,
                TotalSubscriptionsCount = subscriptions.Count()
            };

            return summary;


        }


        private static SubscriptionDto MapToSubscriptionDto(Subscription subscription)
        {
            return new SubscriptionDto
            {
                Id = subscription.Id,
                Price = subscription.Price,
                BillingInterval = subscription.BillingInterval,
                CategoryId = subscription.CategoryId,
                MonthlyCost = CalculateMonthlyEquivalent(subscription.Price, subscription.BillingInterval),
                YearlyCost = CalculateYearlyEquivalent(subscription.Price, subscription.BillingInterval),
                CategoryName = subscription.Category.CategoryName,
                ProviderId = subscription.ProviderId,
                ServiceName = subscription.Provider.ServiceName,
                StartDate = subscription.StartDate,
                EndDate = subscription.EndDate,

            };
        }


        private static decimal CalculateMonthlyEquivalent(decimal price, BillingInterval billingInterval)
        {
            var result = billingInterval switch
            {
                BillingInterval.Monthly => price,
                BillingInterval.Quarterly => price / 3,
                BillingInterval.Yearly => price / 12,
                _ => throw new AppValidationException("Invalid billing interval"),
            };

            return Math.Round(result, 2);
        }
        private static decimal CalculateYearlyEquivalent(decimal price, BillingInterval billingInterval)
        {
            var result = billingInterval switch
            {
                BillingInterval.Monthly => price * 12,
                BillingInterval.Quarterly => price * 4,
                BillingInterval.Yearly => price,
                _ => throw new AppValidationException("Invalid billing interval"),
            };

            return Math.Round(result, 2);

        }
    }
} 
