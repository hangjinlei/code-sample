using Auth.Jwt.Models;
using Auth.Jwt.Sample.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection;
using System.Security.Claims;

namespace Auth.Jwt.Sample.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthenticationController : ControllerBase
{
    private readonly IOptions<JwtSettings> options;
    private readonly RoleManager<IdentityRole> roleManager;
    private readonly UserManager<ApplicationUser> userManager;

    public AuthenticationController(IOptions<JwtSettings> options, RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
    {
        this.options = options;
        this.roleManager = roleManager;
        this.userManager = userManager;
    }

    /// <summary>
    /// Sign in both for admin and user
    /// 
    /// 1. Check if user exist
    /// 2. Check if password is correct
    /// 3. Create token (email, subject, jti, iat, roles)
    /// 4. Return Ok
    /// </summary>
    /// <param name="signInModel"></param>
    /// <returns></returns>
    [HttpPost("SignIn")]
    public async Task<IActionResult> SignIn([FromBody] SignInModel signInModel)
    {
        var user = await userManager.FindByEmailAsync(signInModel.Email);

        if (user is { } && await userManager.CheckPasswordAsync(user, signInModel.Password))
        {
            var userRoles = await userManager.GetRolesAsync(user);

            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Email , user.Email!),
                new Claim(JwtRegisteredClaimNames.Sub, options.Value.Subject),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
            };

            foreach (var role in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, role));
            }

            var token = TokenUtil.GetToken(options, authClaims);
            return Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token),
                expiration = token.ValidTo,
                email = user.Email
            });
        }

        return BadRequest();
    }

    /// <summary>
    /// Register for user
    /// 
    /// 1. Ensure role created
    /// 2. Check if user exist
    /// 3. Create user
    /// 4. Add user to role
    /// 5. Return Ok
    /// </summary>
    /// <param name="registerModel"></param>
    /// <returns></returns>
    [HttpPost("Register")]
    public async Task<IActionResult> Register([FromBody] RegisterModel registerModel)
    {
        await EnsureRoleCreated();

        var userExist = await userManager.FindByEmailAsync(registerModel.Email);
        if (userExist is not null)
        {
            return BadRequest();
        }

        var user = new ApplicationUser()
        {
            UserName = registerModel.UserName,
            Email = registerModel.Email
        };

        var createResult = await userManager.CreateAsync(user, registerModel.Password);

        if (createResult.Succeeded)
        {
            await userManager.AddToRoleAsync(user, UserRoles.User);
            return Ok(registerModel);
        }
        else
        {
            return BadRequest(createResult);
        }
    }

    /// <summary>
    /// Register for admin
    /// 
    /// 1. Ensure role created
    /// 2. Check if user exist
    /// 3. Create user
    /// 4. Add user to role
    /// 5. Return Ok
    /// </summary>
    /// <param name="registerModel"></param>
    /// <returns></returns>
    [HttpPost("Admin/Register")]
    public async Task<IActionResult> RegisterAdmin([FromBody] RegisterModel registerModel)
    {
        await EnsureRoleCreated();

        var userExist = await userManager.FindByEmailAsync(registerModel.Email);
        if (userExist is not null)
        {
            return BadRequest();
        }

        var user = new ApplicationUser()
        {
            UserName = registerModel.UserName,
            Email = registerModel.Email
        };

        var createResult = await userManager.CreateAsync(user, registerModel.Password);

        if (createResult.Succeeded)
        {
            await userManager.AddToRoleAsync(user, UserRoles.Admin);
            return Ok(registerModel);
        }
        else
        {
            return BadRequest(createResult);
        }
    }

    private async Task EnsureRoleCreated()
    {
        var fields = typeof(UserRoles)
            .GetFields(BindingFlags.Public | BindingFlags.Static);

        foreach (var role in fields)
        {
            var value = (string)role.GetValue(null)!;
            if (!await roleManager.RoleExistsAsync(value))
            {
                await roleManager.CreateAsync(new(value));
            }
        }
    }
}
