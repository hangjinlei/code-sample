namespace Auth.Jwt.Models;

public class SignInModel
{
    public string Email { get; set; } = default!;
    public string Password { get; set; } = default!;
}
