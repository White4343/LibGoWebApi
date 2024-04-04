using FluentValidation;
using Microsoft.EntityFrameworkCore;
using SendGrid.Helpers.Errors.Model;
using User.API.Data;
using User.API.Data.Entities;
using User.API.Repositories.Interfaces;

namespace User.API.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;
        private readonly ILogger<UserRepository> _logger;

        public UserRepository(AppDbContext context, ILogger<UserRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Users> CreateUserAsync(Users user)
        {
            try
            {
                var userToCreate = await _context.Users.AddAsync(user);

                await _context.SaveChangesAsync();

                return userToCreate.Entity;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                _logger.LogError(e, "An error occurred while creating a user");
                throw;
            }
        }

        public async Task<Users> GetUserByIdAsync(int id)
        {
            try
            {
                return await UserExists(id);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);

                throw;
            }
        }

        public async Task<IEnumerable<Users>> GetUsersAsync()
        {
            try
            {
                var users = await _context.Users.ToListAsync();

                if (users == null || users.Count == 0)
                {
                    throw new NotFoundException("No users found");
                }

                return users;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);

                throw;
            }
        }

        public async Task<Users> UpdateUserAsync(Users user)
        {
            try
            {
                var userToUpdate = await UserExists(user.Id);

                _context.Users.Update(user);

                await _context.SaveChangesAsync();

                return userToUpdate;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);

                throw;
            }
        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            try
            {
                var userToDelete = await UserExists(id);

                _context.Users.Remove(userToDelete);

                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);

                throw;
            }
        }

        private async Task<Users> UserExists(int id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                throw new NotFoundException($"User with id {id} doesn't exist");
            }

            return user;
        }

        private async Task<Users> UserEmailExists(string email)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);

            if (user != null)
            {
                throw new ValidationException("Email is taken!");
            }

            return user;
        }

        private async Task<Users> UserLoginExists(string login)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Login == login);

            if (user != null)
            {
                throw new ValidationException("Login is taken!");
            }

            return user;
        }
    }
}