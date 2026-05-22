using Coffee.WebSite.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Coffee.WebSite.Pages.Account;

public class LoginModel : PageModel
{
    private readonly IUserService _userService;

    [BindProperty]
    public LoginInput Input { get; set; } = new();

    public LoginModel(IUserService userService) => _userService = userService;

    public void OnGet() { }

    public async Task<IActionResult> OnPostAsync()
    {
        if (Input.Username?.Trim().ToLower() == "admin" && Input.Password == "1234")
        {
            HttpContext.Session.SetString("IsLoggedIn", "true");
            HttpContext.Session.SetString("Username", Input.Username.Trim());

            // Look up real user ID from API
            try
            {
                var response = await _userService.GetAllAsync();
                var user = response.Data?.FirstOrDefault(u =>
                    u.Username.Equals(Input.Username.Trim(), StringComparison.OrdinalIgnoreCase));

                HttpContext.Session.SetString("UserId", user?.Id.ToString() ?? "1");
                HttpContext.Session.SetString("UserFullName", user?.FullName ?? Input.Username);
            }
            catch
            {
                HttpContext.Session.SetString("UserId", "1");
                HttpContext.Session.SetString("UserFullName", Input.Username.Trim());
            }

            return RedirectToPage("/POS/Index");
        }

        ViewData["Error"] = "Usuario o contraseña incorrectos.";
        return Page();
    }
}

public class LoginInput
{
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}
