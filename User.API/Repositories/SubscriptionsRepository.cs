using Microsoft.EntityFrameworkCore;
using SendGrid.Helpers.Errors.Model;
using User.API.Data;
using User.API.Data.Entities;
using User.API.Repositories.Interfaces;

namespace User.API.Repositories
{
    public class SubscriptionsRepository : ISubscriptionsRepository
    {
        private readonly AppDbContext _context;
        private readonly ILogger<SubscriptionsRepository> _logger;

        public SubscriptionsRepository(AppDbContext context, ILogger<SubscriptionsRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Subscriptions> CreateSubscriptionAsync(Subscriptions subscription)
        {
            try
            {
                _context.Subscriptions.Add(subscription);

                await _context.SaveChangesAsync();

                _logger.LogInformation($"Subscription with id {subscription.Id} has been created by user {subscription.UserId}.");

                return subscription;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<IEnumerable<Subscriptions>> GetSubscriptionsAsync()
        {
            try
            {
                var subscriptions = await _context.Subscriptions.ToListAsync();

                if (subscriptions == null || subscriptions.Count == 0)
                {
                    throw new NotFoundException("No subscriptions found.");
                }

                return subscriptions;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<Subscriptions> GetSubscriptionByIdAsync(int id)
        {
            try
            {
                var subscription = await _context.Subscriptions.FirstOrDefaultAsync(s => s.Id == id);

                if (subscription == null)
                {
                    throw new NotFoundException($"Subscription with id {id} not found.");
                }

                return subscription;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<Subscriptions> GetSubscriptionByUserIdAsync(int userId)
        {
            try
            {
                var subscription = await _context.Subscriptions.FirstOrDefaultAsync(s => s.UserId == userId);

                if (subscription == null)
                {
                    throw new NotFoundException($"Subscription with user id {userId} not found.");
                }

                return subscription;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<Subscriptions> GetSubscriptionByBookIdAsync(int bookId)
        {
            try
            {
                var subscription = await _context.Subscriptions.FirstOrDefaultAsync(s => s.BookIds.Contains(bookId));

                if (subscription == null)
                {
                    throw new NotFoundException($"Subscription with book id {bookId} not found.");
                }

                return subscription;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<Subscriptions> UpdateSubscriptionAsync(Subscriptions subscription)
        {
            try
            {
                _context.Subscriptions.Update(subscription);

                await _context.SaveChangesAsync();

                _logger.LogInformation(
                    $"Subscription with id {subscription.Id} has been updated by user {subscription.UserId}.");

                return subscription;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<Subscriptions> DeleteSubscriptionAsync(Subscriptions subscription)
        {
            try
            {
                _context.Subscriptions.Remove(subscription);

                await _context.SaveChangesAsync();

                _logger.LogInformation($"Subscription with id {subscription.Id} has been deleted.");

                return subscription;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
