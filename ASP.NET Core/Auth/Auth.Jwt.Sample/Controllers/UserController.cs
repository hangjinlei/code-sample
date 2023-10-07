using Auth.Jwt.Sample.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Auth.Jwt.Sample.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly ApplicationDbContext context;

    public UserController(ApplicationDbContext context)
    {
        this.context = context;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        return Ok(await context.Users.ToListAsync());
    }
}
