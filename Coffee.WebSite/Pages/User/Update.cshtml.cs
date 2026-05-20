using Coffee.Core.Dto;
using Coffee.WebSite.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Coffee.WebSite.Pages.User;

public class UpdateModel : PageModel
{
    private readonly IUserService _service;

    [BindProperty]
    public UserDto UserDto { get; set; } = new();
    public string ErrorMessage { get; set; } = string.Empty;

    public UpdateModel(IUserService service) => _service = service;

    public async Task<IActionResult> OnGetAsync(int id)
    {
        var response = await _service.GetByIdAsync(id);
        if (response?.Data == null) return RedirectToPage("./List");
        UserDto = response.Data;
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid) return Page();
        var response = await _service.UpdateAsync(UserDto);
        if (response?.Data == null)
        {
            ErrorMessage = "Error al actualizar el usuario. Intente de nuevo.";
            return Page();
        }
        return RedirectToPage("./List");
    }
}
