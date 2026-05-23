using Coffee.WebSite.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Coffee.WebSite.Pages.Account;

public class LoginModel : PageModel
{
    private readonly IAuthService _authService;

    [BindProperty]
    public LoginInput Input { get; set; } = new();

    public LoginModel(IAuthService authService) => _authService = authService;

    public void OnGet() { }

    public async Task<IActionResult> OnPostAsync()
    {
        if (string.IsNullOrWhiteSpace(Input.Username) || string.IsNullOrWhiteSpace(Input.Password))
        {
            ViewData["Error"] = "Ingresa usuario y contraseña.";
            return Page();
        }

        var response = await _authService.LoginAsync(Input.Username.Trim(), Input.Password);

        if (!response.Success || response.Data == null)
        {
            ViewData["Error"] = response.Message ?? "Usuario o contraseña incorrectos.";
            return Page();
        }

        var user = response.Data;
        HttpContext.Session.SetString("IsLoggedIn",   "true");
        HttpContext.Session.SetString("Username",     user.Username);
        HttpContext.Session.SetString("UserFullName", user.FullName);
        HttpContext.Session.SetString("UserId",       user.Id.ToString());
        HttpContext.Session.SetString("RoleId",       user.RoleId.ToString());
        HttpContext.Session.SetString("RoleName",     user.RoleName ?? "");

        return RedirectToPage("/POS/Index");
    }
}

public class LoginInput
{
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}
