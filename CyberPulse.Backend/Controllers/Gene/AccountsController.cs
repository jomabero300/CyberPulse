using CyberPulse.Backend.Data;
using CyberPulse.Backend.Helpers;
using CyberPulse.Backend.Repositories.Interfaces.Gene;
using CyberPulse.Backend.UnitsOfWork.Interfaces.Gene;
using CyberPulse.Shared.Entities.Gene;
using CyberPulse.Shared.EntitiesDTO;
using CyberPulse.Shared.EntitiesDTO.Gene;
using CyberPulse.Shared.Enums;
using CyberPulse.Shared.Responses;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Web;

namespace CyberPulse.Backend.Controllers.Gene;

[ApiController]
[Route("api/[controller]")]
public class AccountsController : ControllerBase
{
    private readonly IUsersUnitOfWork _usersUnitOfWork;
    private readonly IUserRepository _userRepository;
    private readonly IConfiguration _configuration;
    private readonly IWebHostEnvironment _env;

    private readonly IMailHelper _mailHelper;
    private readonly ApplicationDbContext _context;

    public AccountsController(IUsersUnitOfWork usersUnitOfWork, IConfiguration configuration, IMailHelper mailHelper, ApplicationDbContext context, IUserRepository userRepository, IWebHostEnvironment env)
    {
        _usersUnitOfWork = usersUnitOfWork;
        _configuration = configuration;
        _mailHelper = mailHelper;
        _context = context;
        _userRepository = userRepository;
        _env = env;
    }

    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [HttpGet("paginated")]
    public async Task<IActionResult> GetAsync([FromQuery] PaginationDTO pagination)
    {
        var response = await _usersUnitOfWork.GetAsync(pagination);

        if (response.WasSuccess)
        {
            return Ok(response.Result);
        }

        return BadRequest();
    }

    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [HttpGet("totalRecordsPaginated")]
    public async Task<IActionResult> GetPagesAsync([FromQuery] PaginationDTO pagination)
    {
        var action = await _usersUnitOfWork.GetTotalRecordsAsync(pagination);
        if (action.WasSuccess)
        {
            return Ok(action.Result);
        }
        return BadRequest();
    }

    [HttpPost("RecoverPassword")]
    public async Task<IActionResult> RecoverPasswordAsync([FromBody] EmailDTO model)
    {
        var user = await _usersUnitOfWork.GetUserAsync(model.Email);

        if (user == null)
        {
            return NotFound();
        }

        var response = await SendRecoverEmailAsync(user, model.Language);

        if (response.WasSuccess)
        {
            return NoContent();
        }

        return BadRequest(response.Message);
    }

    [HttpPost("ResetPassword")]
    public async Task<IActionResult> ResetPasswordAsync([FromBody] ResetPasswordDTO model)
    {
        var user = await _usersUnitOfWork.GetUserAsync(model.Email);

        if (user == null)
        {
            return NotFound();
        }

        //var result = await _usersUnitOfWork.ResetPasswordAsync(user, HttpUtility.UrlDecode(model.Token), model.NewPassword);
        var result = await _usersUnitOfWork.ResetPasswordAsync(user, model.Token, model.NewPassword);

        if (result.Succeeded)
        {
            return NoContent();
        }

        return BadRequest(result.Errors.FirstOrDefault()!.Description);
    }

    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [HttpPut]
    public async Task<IActionResult> PutAsync(User user)
    {
        try
        {
            var currentUser = await _usersUnitOfWork.GetUserAsync(User.Identity!.Name!);

            if (currentUser == null)
            {
                return NotFound();
            }

            currentUser.PhoneNumber = user.PhoneNumber;
            currentUser.FirstName = user.FirstName;
            currentUser.LastName = user.LastName;
            currentUser.CountryId = user.CountryId;
            if (!string.IsNullOrEmpty(user.Photo))
            {
                var photo = await UploadImageAsync(user.Photo, user.Id);
                currentUser.Photo = photo;
            }

            var result = await _usersUnitOfWork.UpdateUserAsync(currentUser);

            if (result.Succeeded)
            {
                return Ok(BuildToken(currentUser));
            }

            return BadRequest(result.Errors.FirstOrDefault());
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [HttpPut("UserType")]
    public async Task<IActionResult> PutAsync(string userId, UserType userType)
    {
        await _usersUnitOfWork.UpdateUserAsync(userId, userType);
        return BadRequest();
    }

    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [HttpGet]
    public async Task<IActionResult> GetAsync()
    {
        return Ok(await _userRepository.GetUserAsync(User.Identity!.Name!));
    }



    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [HttpGet("LoadUsers/{userType}")]
    public async Task<IActionResult> GetAsync(UserType userType)
    {
        var result = await _usersUnitOfWork.GetAsync(userType);

        return Ok(result);
    }

    [HttpGet("LoadUser/{id}")]
    public async Task<IActionResult> GetAsync(string id)
    {
        var result = await _usersUnitOfWork.GetAsync(id);

        return Ok(result);

    }

    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [HttpGet("Instructor")]
    public async Task<IActionResult> GetAsync(string id, UserType userType)
    {
        var result = await _usersUnitOfWork.GetUserAsync(id, userType);

        if (result.WasSuccess)
        {
            return Ok(result.Result);
        }

        return BadRequest(result.Message);
    }

    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [HttpGet("UserType")]
    public async Task<IActionResult> UserTypeAsync(string id)
    {
        var result = await _usersUnitOfWork.GetUserAsync(new Guid(id));

        if (result != null)
        {
            return Ok(result);
        }

        return BadRequest();
    }

    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [HttpPost("ChangePassword")]
    public async Task<IActionResult> ChangePasswordAsync(ChangePasswordDTO model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var user = await _usersUnitOfWork.GetUserAsync(User.Identity!.Name!);

        if (user == null)
        {
            return NotFound();
        }

        user.Photo = !string.IsNullOrEmpty(user.Photo) ? user.Photo!.Substring(user.Photo.IndexOf("\\Images\\Users\\")) : user.Photo;

        var result = await _usersUnitOfWork.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);

        if (!result.Succeeded)
        {
            return BadRequest(result.Errors.FirstOrDefault()!.Description);
        }

        return NoContent();
    }

    [HttpPost("ResedToken")]
    public async Task<IActionResult> ResedTokenAsync([FromBody] EmailDTO model)
    {
        var user = await _usersUnitOfWork.GetUserAsync(model.Email);
        if (user == null)
        {
            return NotFound();
        }

        var response = await SendConfirmationEmailAsync(user, model.Language);
        if (response.WasSuccess)
        {
            return NoContent();
        }

        return BadRequest(response.Message);
    }

    [HttpPost("CreateUser")]
    public async Task<IActionResult> CreateUser([FromBody] UserDTO model)
    {
        var country = await _context.Countries.FindAsync(model.CountryId);

        if (country == null)
        {
            return BadRequest("ERR004");
        }


        User user = model;
        user.Country = country;
        var result = await _usersUnitOfWork.AddUserASync(user, model.Password);

        if (result.Succeeded)
        {
            await _usersUnitOfWork.AddUserToRoleAsync(user, user.UserType.ToString());

            var response = await SendConfirmationEmailAsync(user, model.Language);

            if (response.WasSuccess)
            {
                return NoContent();
            }

            return BadRequest(response.Message);
        }

        return BadRequest(result.Errors.FirstOrDefault());
    }

    [HttpGet("ConfirmEmail")]
    public async Task<IActionResult> ConfirmEmailAsync(string userId, string token)
    {
        token = token.Replace(" ", "+");

        var user = await _usersUnitOfWork.GetUserAsync(new Guid(userId));

        if (user == null)
        {
            return NotFound();
        }

        if (user.EmailConfirmed == false)
        {
            string url = _configuration["UrlBackend"]!;

            if (!string.IsNullOrWhiteSpace(user.Photo) && user.Photo.Contains(url))
            {
                user.Photo = user.Photo.Substring(url.Length);
            }

            var result = await _usersUnitOfWork.ConfirmEmailAsync(user, token);

            if (!result.Succeeded)
            {
                return BadRequest(result.Errors.FirstOrDefault());
            }

            return NoContent();
        }

        return BadRequest("ERR009");
    }

    [HttpPost("Login")]
    public async Task<IActionResult> LoginAsync([FromBody] LoginDTO model)
    {
        var result = await _usersUnitOfWork.LoginAsync(model);

        if (result.Succeeded)
        {
            var user = await _usersUnitOfWork.GetUserAsync(model.Email);

            if (user != null)
            {
                await _usersUnitOfWork.ResetAccessFailedCountAsync(user);
            }

            return Ok(BuildToken(user!));
        }

        if (result.IsLockedOut)
        {
            return BadRequest("ERR007");
        }

        if (result.IsNotAllowed)
        {
            return BadRequest("ERR008");
        }

        return BadRequest("ERR006");
    }

    [HttpGet("UserTypeUp")]
    public async Task<IActionResult> UserTypeAsync(string id, UserType userType)
    {
        await _usersUnitOfWork.UpdateUserAsync(id, userType);

        return NoContent();
    }

    private async Task<ActionResponse<string>> SendConfirmationEmailAsync(User user, string language)
    {
        var myToken = await _usersUnitOfWork.GenerateEmailConfirmationTokenAsync(user);

        var tokenLink = Url.Action("ConfirmEmail", "accounts", new
        {
            userid = user.Id,
            token = HttpUtility.UrlEncode(myToken)
        }, HttpContext.Request.Scheme, _configuration["Url Frontend"]);

        if (language == "es")
        {
            return await _mailHelper.SendMail(user.FullName, user.Email!, _configuration["Mail:SubjectConfirmationEs"]!, string.Format(_configuration["Mail:BodyConfirmationEs"]!, tokenLink), language);
        }

        return await _mailHelper.SendMail(user.FullName, user.Email!, _configuration["Mail:SubjectConfirmationEn"]!, string.Format(_configuration["Mail:BodyConfirmationEn"]!, tokenLink), language);
    }
    private async Task<ActionResponse<string>> SendRecoverEmailAsync(User user, string language)
    {
        var myToken = await _usersUnitOfWork.GeneratePasswordResetTokenAsync(user);

        var tokenLink = Url.Action("ResetPassword", "accounts", new
        {
            userid = user.Id,
            token = myToken
        }, HttpContext.Request.Scheme, _configuration["Url Frontend"]);

        if (language == "es")
        {
            return await _mailHelper.SendMail(user.FullName, user.Email!, _configuration["Mail:SubjectRecoveryEs"]!, string.Format(_configuration["Mail:BodyRecoveryEs"]!, tokenLink), language);
        }

        return await _mailHelper.SendMail(user.FullName, user.Email!, _configuration["Mail:SubjectRecoveryEn"]!, string.Format(_configuration["Mail:BodyRecoveryEn"]!, tokenLink), language);
    }
    private TokenDTO BuildToken(User user)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name,user.Email!),
            new Claim(ClaimTypes.Role,user.UserType.ToString()),
            new Claim("FrstName",user.DocumentId),
            new Claim("FrstName",user.FirstName),
            new Claim("LastName",user.LastName),
            new Claim("Photo",user.Photo ?? string.Empty),
            new Claim("CountryId",user.Country.Id.ToString()),
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["jwMBELtKey"]!));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var expiration = DateTime.UtcNow.AddDays(30);
        var token = new JwtSecurityToken(
            issuer: null,
            audience: null,
            claims: claims,
            expires: expiration,
            signingCredentials: credentials);
        return new TokenDTO
        {
            Token = new JwtSecurityTokenHandler().WriteToken(token),
            Expiration = expiration,
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
            var user = await _context.Users
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
}