using AutoMapper;
using FluentValidation;
using Newtonsoft.Json.Linq;
using SendGrid.Helpers.Errors.Model;
using User.API.Data.Entities;
using User.API.Models.Dtos;
using User.API.Models.Requests;
using User.API.Models.Responses;
using User.API.Repositories.Interfaces;
using User.API.Services.Interfaces;

namespace User.API.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly ILogger<UserService> _logger;
        private readonly IMapper _mapper;
        private readonly IValidator<Users> _validator;
        private readonly IValidator<UserEmailDto> _emailValidator;
        private readonly IValidator<string> _passwordValidator;
        private readonly IValidator<UserPatchDto> _userPatchValidator;

        public UserService(IUserRepository userRepository, ILogger<UserService> logger, IMapper mapper,
            IValidator<Users> validator, 
            IValidator<UserEmailDto> emailValidator, 
            IValidator<string> passwordValidator, 
            IValidator<UserPatchDto> userPatchValidator)
        {
            _userRepository = userRepository;
            _logger = logger;
            _mapper = mapper;
            _validator = validator;
            _emailValidator = emailValidator;
            _passwordValidator = passwordValidator;
            _userPatchValidator = userPatchValidator;
        }

        public async Task<UserDto> CreateUserAsync(CreateUserRequest user)
        {
            try
            {
                var userEntity = _mapper.Map<Users>(user);

                var validationResult = await _validator.ValidateAsync(userEntity);

                if (!validationResult.IsValid)
                {
                    throw new ValidationException(validationResult.Errors);
                }

                userEntity.PasswordHash = HashPassword(userEntity.PasswordHash);

                userEntity.Role = "User";

                userEntity.RegisterDate = DateTime.UtcNow;

                userEntity.LockoutEnabled = false;

                userEntity.LockoutEnd = null;

                userEntity.TwoFactorEnabled = false;

                userEntity.PhoneNumberConfirmed = false;

                userEntity.EmailConfirmed = false;

                userEntity.SecurityStamp = Guid.NewGuid().ToString();

                userEntity.ConcurrencyStamp = Guid.NewGuid().ToString();

                userEntity.NormalizedEmail = userEntity.Email.ToUpper();

                userEntity.NormalizedUserName = userEntity.UserName.ToUpper();

                userEntity.PhoneNumber = null;

                userEntity.AccessFailedCount = 0;

                var createdUser = await _userRepository.CreateUserAsync(userEntity);

                var result = _mapper.Map<UserDto>(createdUser);

                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<UserDto> GetUserByIdAsync(int id)
        {
            try
            {
                var user = await _userRepository.GetUserByIdAsync(id);

                var resultUser = _mapper.Map<UserDto>(user);

                return resultUser;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);

                throw;
            }
        }

        public async Task<GetUserPrivatePageResponse> GetUserPrivatePageByIdAsync(int id, int tokenUserId)
        {
            try
            {
                await IsRequestUser(id, tokenUserId);

                var user = await _userRepository.GetUserByIdAsync(id);

                var resultUser = _mapper.Map<GetUserPrivatePageResponse>(user);

                return resultUser;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);

                throw;
            }
        }

        public async Task<IEnumerable<UserDto>> GetUsersAsync()
        {
            try
            {
                var users = await _userRepository.GetUsersAsync();

                var resultUsers = _mapper.Map<IEnumerable<UserDto>>(users);

                return resultUsers;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);

                throw;
            }
        }

        public async Task<IEnumerable<UserDto>> GetUsersByNicknameAsync(string nickname)
        {
            try
            {
                var users = await GetUsersAsync();

                var resultUsers = users.Where(u => u.Nickname.Contains(nickname));

                if (resultUsers == null || resultUsers.Count() == 0)
                {
                    throw new NotFoundException("No users found with this nickname");
                }

                return resultUsers;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<Users> UpdateUserAsync(Users user, int tokenUserId)
        {
            try
            {
                await IsRequestUser(user.Id, tokenUserId);

                var validationResult = await _validator.ValidateAsync(user);

                if (!validationResult.IsValid)
                {
                    throw new ValidationException(validationResult.Errors);
                }

                user.PasswordHash = HashPassword(user.PasswordHash);

                return await _userRepository.UpdateUserAsync(user);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<Users> PatchUserAsync(PatchUserRequest request, int tokenUserId)
        {
            try
            {
                var user = await IsRequestUser(request.Id, tokenUserId);

                FluentValidation.Results.ValidationResult? validationResult;
                
                UserPatchDto userToPatch;

                if (request.FieldToPatch == "Email")
                {
                    var userEmailToPatch = new UserEmailDto { Email = request.ValueToPatch};

                    validationResult = await _emailValidator.ValidateAsync(userEmailToPatch);
                }
                else if (request.FieldToPatch == "Nickname")
                {
                    userToPatch = new UserPatchDto { Nickname = request.ValueToPatch };

                    validationResult = await _userPatchValidator.ValidateAsync(userToPatch);
                }
                else if (request.FieldToPatch == "Description")
                {
                    userToPatch = new UserPatchDto { Description = request.ValueToPatch };

                    validationResult = await _userPatchValidator.ValidateAsync(userToPatch);
                }
                else if (request.FieldToPatch == "PhotoUrl")
                {
                    userToPatch = new UserPatchDto { PhotoUrl = request.ValueToPatch };

                    validationResult = await _userPatchValidator.ValidateAsync(userToPatch);
                }
                else
                {
                    throw new ValidationException("Invalid field to patch");
                }

                if (!validationResult.IsValid)
                {
                    throw new ValidationException(validationResult.Errors);
                }

                return await _userRepository.PatchUserAsync(request.Id, request.FieldToPatch, request.ValueToPatch);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<Users> PatchUserPasswordAsync(PatchUserPasswordRequest request, int tokenUserId)
        {
            try
            {
                var user = await IsRequestUser(request.Id, tokenUserId);

                var requestPassword = HashPassword(request.OldPassword);

                VerifyPassword(requestPassword, user.PasswordHash);

                if (request.NewPassword != request.NewPasswordRepeat)
                {
                    throw new ValidationException("New passwords do not match");
                }

                var validationResult = await _passwordValidator.ValidateAsync(request.NewPassword);

                if (!validationResult.IsValid)
                {
                    throw new ValidationException(validationResult.Errors);
                }

                return await _userRepository.PatchUserPasswordAsync(request);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<bool> DeleteUserAsync(int id, int tokenUserId)
        {
            try
            {
                await IsRequestUser(id, tokenUserId);

                return await _userRepository.DeleteUserAsync(id);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        
        private async Task<Users> IsRequestUser(int id, int tokenUserId)
        {
            try
            {
                var user = await _userRepository.GetUserByIdAsync(id);

                if (user.Id != tokenUserId)
                {
                    throw new UnauthorizedAccessException("You are not authorized to perform this action");
                }

                return user;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        private string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.EnhancedHashPassword(password);
        }

        private void VerifyPassword(string password, string passwordHash)
        {
            if (!BCrypt.Net.BCrypt.EnhancedVerify(password, passwordHash))
            {
                throw new ValidationException("Password is incorrect");
            }
        }
    }
}