using Auth.Jwt.Sample.Data;
using Auth.Jwt.Sample.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Auth.Jwt.Sample.Controllers;

[ApiController]
[Authorize(Roles = UserRoles.Admin)]
[Route("api/[controller]")]
public class RoleController : ControllerBase
{
    private readonly ApplicationDbContext context;

    public RoleController(ApplicationDbContext context)
    {
        this.context = context;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        return Ok(await context.Roles.ToListAsync());
    }
}
