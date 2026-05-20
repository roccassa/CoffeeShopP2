using Coffee.Core.Dto;
using Coffee.WebSite.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Coffee.WebSite.Pages.User;

public class CreateModel : PageModel
{
    private readonly IUserService _service;

    [BindProperty]
    public UserDto UserDto { get; set; } = new();
    public string ErrorMessage { get; set; } = string.Empty;

    public CreateModel(IUserService service) => _service = service;

    public void OnGet() { }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid) return Page();
        var response = await _service.CreateAsync(UserDto);
        if (response?.Data == null)
        {
            ErrorMessage = "Error al crear el usuario. Intente de nuevo.";
            return Page();
        }
        return RedirectToPage("./List");
    }
}
