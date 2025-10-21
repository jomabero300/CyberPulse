﻿using CyberPulse.Backend.Data;
using CyberPulse.Backend.Helpers;
using CyberPulse.Backend.Repositories.Interfaces.Gene;
using CyberPulse.Shared.Entities.Gene;
using CyberPulse.Shared.EntitiesDTO;
using CyberPulse.Shared.EntitiesDTO.Gene;
using CyberPulse.Shared.Enums;
using CyberPulse.Shared.Responses;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CyberPulse.Backend.Repositories.Implementations.Gene;

public class UserRepository : IUserRepository
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly SignInManager<User> _signInManager;
    private readonly IConfiguration _configuration;
    private readonly IWebHostEnvironment _env;

    public UserRepository(ApplicationDbContext context, UserManager<User> userManager, RoleManager<IdentityRole> roleManager, SignInManager<User> signInManager, IWebHostEnvironment env, IConfiguration configuration)
    {
        _context = context;
        _userManager = userManager;
        _roleManager = roleManager;
        _signInManager = signInManager;
        _env = env;
        _configuration = configuration;
    }

    public async Task<IdentityResult> AddUserAsync(User user, string password)
    {
        if (!string.IsNullOrWhiteSpace(user.Photo))
        {
            user.Photo = await UploadImageAsync(user.Photo, "");
        }

        return await _userManager.CreateAsync(user, password);
    }

    public async Task AddUserToRoleAsync(User user, string roleName)
    {
        await _userManager.AddToRoleAsync(user, roleName);
    }

    public async Task<IdentityResult> ChangePasswordAsync(User user, string currentPassword, string newPassword)
    {
        return await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);
    }

    public async Task CheckRoleAsync(string roleName)
    {
        var rolExiste = await _roleManager.RoleExistsAsync(roleName);

        if (!rolExiste)
        {
            await _roleManager.CreateAsync(new IdentityRole { Name = roleName });
        }
    }

    public async Task<IdentityResult> ConfirmEmailAsync(User user, string token)
    {
        return await _userManager.ConfirmEmailAsync(user, token);
    }

    public async Task<string> GenerateEmailConfirmationTokenAsync(User user)
    {
        return await _userManager.GenerateEmailConfirmationTokenAsync(user);
    }

    public async Task<string> GeneratePasswordResetTokenAsync(User user)
    {
        return await _userManager.GeneratePasswordResetTokenAsync(user);
    }

    public async Task<IEnumerable<User>> GetAsync(UserType userType)
    {
        var resul = await _context.Users.AsNoTracking().Where(x => x.UserType == userType).OrderBy(x => x.FirstName).ToListAsync();

        return resul;
    }

    public async Task<User> GetUserAsync(Guid userId)
    {
        var user = await _context.Users
                                .Include(x => x.Country)
                                .FirstOrDefaultAsync(x => x.Id == userId.ToString());
        if (user != null && !string.IsNullOrWhiteSpace(user!.Photo))
        {
            user.Photo = $"{_configuration["UrlBackend"]}/{user.Photo}";
        }

        return user!;
    }

    public async Task<User> GetUserAsync(string email)
    {
        var user = await _context.Users.AsNoTracking()
            .Include(c => c.Country)
            .FirstOrDefaultAsync(x => x.Email == email);

        if (user != null && !string.IsNullOrWhiteSpace(user!.Photo))
        {
            user.Photo = $"{_configuration["UrlBackend"]}/{user.Photo}";
        }

        return user!;
    }

    public async Task<bool> IsUserInRoleAsync(User user, string roleName)
    {
        return await _userManager.IsInRoleAsync(user, roleName);
    }

    public async Task<SignInResult> LoginAsync(LoginDTO model)
    {
        return await _signInManager.PasswordSignInAsync(model.Email, model.Password, false, true);
    }

    public async Task LogoutAsync()
    {
        await _signInManager.SignOutAsync();
    }

    public async Task<IdentityResult> ResetPasswordAsync(User user, string token, string password)
    {
        return await _userManager.ResetPasswordAsync(user, token, password);
    }

    public async Task<IdentityResult> UpdateUserAsync(User user)
    {
        if (!string.IsNullOrWhiteSpace(user.Photo) && !user.Photo.Contains("\\Images\\Users\\"))
        {
            user.Photo = await UploadImageAsync(user.Photo, user.Id);
        }

        return await _userManager.UpdateAsync(user);
    }

    public async Task<ActionResponse<User>> GetUserAsync(string userDocument, UserType userType)
    {
        var entity = userDocument.Length < 15 ?
            await _context.Users.AsNoTracking().Where(x => x.DocumentId == userDocument && x.UserType == userType).FirstOrDefaultAsync() :
            await _context.Users.AsNoTracking().Where(x => x.Id == userDocument && x.UserType == userType).FirstOrDefaultAsync();

        if (entity == null)
        {
            return new ActionResponse<User>
            {
                WasSuccess = false,
                Message = "ERR001"
            };
        }

        return new ActionResponse<User>
        {
            WasSuccess = true,
            Result = entity
        };
    }

    private async Task<string?> UploadImageAsync(string image, string id)
    {
        string webRootPath = _env.WebRootPath;

        string directoryFolder = "\\Images\\Users\\";

        string DirectoryPath = $"{webRootPath}{directoryFolder}";

        var imageBase64 = Convert.FromBase64String(image!);

        string pathImage = $"{Guid.NewGuid()}.jpg";

        if (!Directory.Exists(DirectoryPath))
        {
            Directory.CreateDirectory(DirectoryPath);
        }

        var path = $"{DirectoryPath}{pathImage}";

        await System.IO.File.WriteAllBytesAsync(path, imageBase64);

        if (!string.IsNullOrWhiteSpace(id))
        {
            var user = await _context.Users.AsNoTracking()
                                         .Where(x => x.Id == id)
                                         .Select(p => p.Photo)
                                         .FirstOrDefaultAsync();
            if (!string.IsNullOrWhiteSpace(user) && user != null)
            {
                System.IO.File.Delete($"{webRootPath}{user}");
            }
        }

        return $"{directoryFolder}{pathImage}";
    }

    public async Task<IEnumerable<User>> GetAsync(string id)
    {
        var resul = await _context.Users.AsNoTracking().Where(x => x.Id == id).OrderBy(x => x.FirstName).ToListAsync();

        return resul;

    }
    public async Task<ActionResponse<IEnumerable<User>>> GetAsync(PaginationDTO pagination)
    {
        var queryable = pagination.UserType!=null?  
                                                _context.Users
                                                        .AsNoTracking()
                                                        .Include(x => x.Country)
                                                        .Where(x=>x.UserType==pagination.UserType)
                                                        .AsQueryable():
                                                _context.Users
                                                        .AsNoTracking()
                                                        .Include(x => x.Country)
                                                        .AsQueryable();

        if (!string.IsNullOrWhiteSpace(pagination.Filter))
        {
            queryable = queryable.Where(x => x.FirstName.ToLower().Contains(pagination.Filter.ToLower()) ||
                                             x.LastName.ToLower().Contains(pagination.Filter.ToLower()));
        }

        return new ActionResponse<IEnumerable<User>>
        {
            WasSuccess = true,
            Result = await queryable
                .OrderBy(x => x.FirstName)
                .ThenBy(x => x.LastName)
                .Select(x=>new User
                    {
                        Id = x.Id,
                        FirstName = x.FirstName,
                        LastName = x.LastName,
                        Photo = !string.IsNullOrWhiteSpace(x.Photo) ? $"{_configuration["UrlBackend"]}/{x.Photo}": $"{_configuration["UrlBackend"]}/Images/NoImage.png",
                        UserType=x.UserType,
                        NormalizedUserName=x.NormalizedUserName,
                        Email = x.Email,
                        NormalizedEmail=x.NormalizedEmail,
                        EmailConfirmed=x.EmailConfirmed,
                        PasswordHash=x.PasswordHash,
                        SecurityStamp=x.SecurityStamp,
                        ConcurrencyStamp=x.ConcurrencyStamp,
                        PhoneNumber=x.PhoneNumber,
                        PhoneNumberConfirmed=x.PhoneNumberConfirmed,
                        TwoFactorEnabled=x.TwoFactorEnabled,
                        LockoutEnd=x.LockoutEnd,
                        LockoutEnabled=x.LockoutEnabled,
                        AccessFailedCount=x.AccessFailedCount,
                        Country = x.Country,
                        CountryId = x.CountryId,
                        DocumentId = x.DocumentId,                    
                    })
                .Paginate(pagination)
                .ToListAsync()
        };
    }

    public async Task<ActionResponse<int>> GetTotalRecordsAsync(PaginationDTO pagination)
    {
        var queryable = _context.Users.AsNoTracking().AsQueryable();

        if (!string.IsNullOrWhiteSpace(pagination.Filter))
        {
            queryable = queryable.Where(x => x.FirstName.ToLower().Contains(pagination.Filter.ToLower()) ||
                                             x.LastName.ToLower().Contains(pagination.Filter.ToLower()));
        }

        double count = await queryable.CountAsync();
        return new ActionResponse<int>
        {
            WasSuccess = true,
            Result = (int)count
        };
    }
    public async Task UpdateUserAsync(string userId, UserType userType)
    {
        var entity = await _context.Users.Where(x => x.Id == userId).FirstOrDefaultAsync();

        if (entity == null) return;

        _context.Database.ExecuteSql($"UPDATE Admi.AspNetUsers SET UserType={userType} WHERE Id={userId}");
    }

    public async Task ResetAccessFailedCountAsync(User user)
    {
        await _userManager.ResetAccessFailedCountAsync(user);

    }
}
