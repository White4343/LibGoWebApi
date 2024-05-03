using FluentValidation;
using Microsoft.EntityFrameworkCore;
using SendGrid.Helpers.Errors.Model;
using User.API.Data;
using User.API.Data.Entities;
using User.API.Models.Requests;
using User.API.Repositories.Interfaces;

namespace User.API.Repositories
{
    public class UserRepository : IUserRepository, IUserValidationRepository
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

        public async Task<Users> PatchUserAsync(int id, string field, string value)
        {
            try
            {
                var userToPatch = await UserExists(id);

                if (field == "Email")
                {
                    userToPatch.Email = value;
                }
                else if (field == "Nickname")
                {
                    userToPatch.Nickname = value;
                }
                else if (field == "Description")
                {
                    userToPatch.Description = value;
                }
                else if (field == "PhotoUrl")
                {
                    userToPatch.PhotoUrl = value;
                }
                else
                {
                    throw new ValidationException("Invalid field to patch");
                }

                _context.Users.Update(userToPatch);

                await _context.SaveChangesAsync();

                return userToPatch;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);

                throw;
            }
        }

        public async Task<Users> PatchUserPasswordAsync(PatchUserPasswordRequest request)
        {
            try
            {
                var userToPatch = await UserExists(request.Id);

                userToPatch.PasswordHash = request.NewPassword;

                _context.Users.Update(userToPatch);

                await _context.SaveChangesAsync();

                return userToPatch;
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
        
        public async Task UserEmailExists(string email)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);

            if (user != null)
            {
                throw new ValidationException("Email is taken!");
            }
        }

        public async Task UserLoginExists(string login)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == login);

            if (user != null)
            {
                throw new ValidationException("Login is taken!");
            }
        }
    }
}