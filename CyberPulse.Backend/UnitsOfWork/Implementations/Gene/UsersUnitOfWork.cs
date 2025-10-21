using CyberPulse.Backend.Repositories.Interfaces.Gene;
using CyberPulse.Backend.UnitsOfWork.Interfaces.Gene;
using CyberPulse.Shared.Entities.Gene;
using CyberPulse.Shared.EntitiesDTO;
using CyberPulse.Shared.EntitiesDTO.Gene;
using CyberPulse.Shared.Enums;
using CyberPulse.Shared.Responses;
using Microsoft.AspNetCore.Identity;

namespace CyberPulse.Backend.UnitsOfWork.Implementations.Gene;

public class UsersUnitOfWork : IUsersUnitOfWork
{
    private readonly IUserRepository _userRepository;

    public UsersUnitOfWork(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<IdentityResult> AddUserASync(User user, string password)=>await _userRepository.AddUserAsync(user, password);

    public async Task AddUserToRoleAsync(User user, string roleName)=>await _userRepository.AddUserToRoleAsync(user,roleName);

    public async Task<IdentityResult> ChangePasswordAsync(User user, string currentPassword, string newPassword)=>await _userRepository.ChangePasswordAsync(user,currentPassword,newPassword);

    public async Task CheckRoleAsync(string roleName)=>await _userRepository.CheckRoleAsync(roleName);

    public async Task<IdentityResult> ConfirmEmailAsync(User user, string token)=>await _userRepository.ConfirmEmailAsync(user,token);

    public async Task<string> GenerateEmailConfirmationTokenAsync(User user)=>await _userRepository.GenerateEmailConfirmationTokenAsync(user);

    public async Task<string> GeneratePasswordResetTokenAsync(User user) => await _userRepository.GeneratePasswordResetTokenAsync(user);

    public async Task<IEnumerable<User>> GetAsync(UserType userType)=>await _userRepository.GetAsync(userType);

    public async Task<IEnumerable<User>> GetAsync(string id)=>await _userRepository.GetAsync(id);

    public async Task<ActionResponse<IEnumerable<User>>> GetAsync(PaginationDTO pagination)=>await _userRepository.GetAsync(pagination);

    public async Task<ActionResponse<int>> GetTotalRecordsAsync(PaginationDTO pagination)=>await _userRepository.GetTotalRecordsAsync(pagination);

    public async Task<User> GetUserAsync(Guid userId) => await _userRepository.GetUserAsync(userId);

    public async Task<User> GetUserAsync(string email)=>await _userRepository.GetUserAsync(email);

    public async Task<ActionResponse<User>> GetUserAsync(string userDocument, UserType userType)=>await _userRepository.GetUserAsync(userDocument, userType);

    public async Task<bool> IsUserInRoleAsync(User user, string roleName)=>await _userRepository.IsUserInRoleAsync(user, roleName);

    public async Task<SignInResult> LoginAsync(LoginDTO model) => await _userRepository.LoginAsync(model);

    public async Task LogoutAsync()=>await _userRepository.LogoutAsync();

    public async Task ResetAccessFailedCountAsync(User user)=>await _userRepository.ResetAccessFailedCountAsync(user);

    public async Task<IdentityResult> ResetPasswordAsync(User user, string token, string password)=>await _userRepository.ResetPasswordAsync(user,token,password);

    public async Task<IdentityResult> UpdateUserAsync(User user)=>await _userRepository.UpdateUserAsync(user);

    public async Task UpdateUserAsync(string userId, UserType userType) => await _userRepository.UpdateUserAsync(userId,userType);
}
