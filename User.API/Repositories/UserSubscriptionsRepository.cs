using Microsoft.EntityFrameworkCore;
using SendGrid.Helpers.Errors.Model;
using User.API.Data;
using User.API.Data.Entities;
using User.API.Repositories.Interfaces;

namespace User.API.Repositories
{
    // TODO: Rewrite all UPDATE and EDIT methods to have entity as a parameter and not Id?
    public class UserSubscriptionsRepository : IUserSubscriptionsRepository
    {
        private readonly AppDbContext _context;
        private readonly ILogger<UserSubscriptionsRepository> _logger;

        public UserSubscriptionsRepository(AppDbContext context, ILogger<UserSubscriptionsRepository> logger)
        {
            _context = context;
            _logger = logger;
        }


        public async Task<UserSubscriptions> CreateUserSubscriptionAsync(UserSubscriptions userSubscription)
        {
            try
            {
                var userSubscriptionToCreate = await _context.UserSubscriptions.AddAsync(userSubscription);

                await _context.SaveChangesAsync();

                return userSubscriptionToCreate.Entity;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<UserSubscriptions> GetUserSubscriptionByIdAsync(int id)
        {
            try
            {
                var result = await _context.UserSubscriptions.FirstOrDefaultAsync(x => x.Id == id);

                IsUserSubscriptionNull(result);

                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<UserSubscriptions> GetUserSubscriptionByUserIdBookIdAsync(int userId, int bookId)
        {
            try
            {
                var result =
                    await _context.UserSubscriptions.FirstOrDefaultAsync(x =>
                        x.UserId == userId && x.BookIds.Contains(bookId));

                IsUserSubscriptionNull(result);

                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<UserSubscriptions> GetUserSubscriptionByUserIdSubscriptionIdAsync(int userId, int subscriptionId)
        {
            try
            {
                var result = await _context.UserSubscriptions.FirstOrDefaultAsync(x =>
                                               x.UserId == userId && x.SubscriptionId == subscriptionId);

                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<IEnumerable<UserSubscriptions>> GetUserSubscriptionsByUserIdAsync(int userId)
        {
            try
            {
                var result = await _context.UserSubscriptions.Where(x => x.UserId == userId).ToListAsync();

                IsUserSubscriptionsNullOrEmpty(result);

                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<IEnumerable<UserSubscriptions>> GetUserSubscriptionsByAuthorUserIdAsync(int authorUserId)
        {
            try
            {
                var result = await _context.UserSubscriptions.Where(x => x.AuthorUserId == authorUserId).ToListAsync();

                IsUserSubscriptionsNullOrEmpty(result);

                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<IEnumerable<UserSubscriptions>> GetUserSubscriptionsAsync()
        {
            try
            {
                var result = await _context.UserSubscriptions.ToListAsync();

                IsUserSubscriptionsNullOrEmpty(result);

                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<IEnumerable<UserSubscriptions>> GetUserSubscriptionsByBookIdAsync(int bookId)
        {
            try
            {
                var result = await _context.UserSubscriptions.Where(us => 
                    us.BookIds.Contains(bookId)).ToListAsync();

                IsUserSubscriptionsNullOrEmpty(result);

                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<UserSubscriptions> PatchUserSubscriptionDateEndAsync(int id)
        {
            try
            {
                var userSubscription = await GetUserSubscriptionByIdAsync(id);

                userSubscription.EndDate = DateTime.UtcNow.AddMonths(1);

                if (!userSubscription.IsActive)
                {
                    userSubscription.IsActive = true;
                }

                _context.UserSubscriptions.Update(userSubscription);

                await _context.SaveChangesAsync();

                return userSubscription;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        
        public async Task<UserSubscriptions> PatchUserSubscriptionIsActiveFalseAsync(int id)
        {
            try
            {
                var userSubscription = await GetUserSubscriptionByIdAsync(id);

                userSubscription.IsActive = false;

                _context.UserSubscriptions.Update(userSubscription);

                await _context.SaveChangesAsync();

                return userSubscription;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task PatchUserSubscriptionExpiredAsync()
        {
            try
            {
                var userSubscriptions =
                    await _context.UserSubscriptions.Where(x => x.EndDate < DateTime.UtcNow).ToListAsync();

                foreach (var userSub in userSubscriptions)
                {
                    userSub.IsActive = false;

                    _context.UserSubscriptions.Update(userSub);
                }

                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task DeleteUserSubscriptionByIdAsync(int id)
        {
            try
            {
                var userSubscription = await GetUserSubscriptionByIdAsync(id);

                _context.UserSubscriptions.Remove(userSubscription);

                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        
        private void IsUserSubscriptionNull(UserSubscriptions userSubscription)
        {
            if (userSubscription == null)
            {
                throw new NotFoundException("UserSubscription not found.");
            }
        }

        private void IsUserSubscriptionsNullOrEmpty(List<UserSubscriptions> userSubscription)
        {
            if (userSubscription == null || userSubscription.Count == 0)
            {
                throw new NotFoundException("UserSubscriptions not found.");
            }
        }
    }
}