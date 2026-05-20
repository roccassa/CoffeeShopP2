using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
namespace Coffee.WebSite.Pages.Account;

public class LoginModel : PageModel
{
    [BindProperty]
    public LoginInput Input { get; set; } = new();

    public void OnGet()
    {
    }

    public IActionResult OnPost()
    {
        // Usuario y contraseña temporal (cambia después)
        if (Input.Username?.Trim().ToLower() == "admin" && Input.Password == "1234")
        {
            HttpContext.Session.SetString("IsLoggedIn", "true");
            HttpContext.Session.SetString("Username", Input.Username);

            return RedirectToPage("/Category/List");
        }

        ViewData["Error"] = "Usuario o contraseña incorrectos";
        return Page();
    }
}

public class LoginInput
{
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}