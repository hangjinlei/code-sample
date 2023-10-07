namespace Auth.Jwt.Models;

public class RegisterModel
{
    public string Email { get; set; } = default!;
    public string Password { get; set; } = default!;
    public string UserName { get; set; } = default!;
}
