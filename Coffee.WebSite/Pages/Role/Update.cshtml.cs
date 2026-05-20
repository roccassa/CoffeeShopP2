using Coffee.Core.Dto;
using Coffee.WebSite.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Coffee.WebSite.Pages.Role;

public class UpdateModel : PageModel
{
    private readonly IRoleService _service;

    [BindProperty]
    public RoleDto RoleDto { get; set; } = new();
    public string ErrorMessage { get; set; } = string.Empty;

    public UpdateModel(IRoleService service) => _service = service;

    public async Task<IActionResult> OnGetAsync(int id)
    {
        var response = await _service.GetByIdAsync(id);
        if (response?.Data == null) return RedirectToPage("./List");
        RoleDto = response.Data;
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid) return Page();
        var response = await _service.UpdateAsync(RoleDto);
        if (response?.Data == null)
        {
            ErrorMessage = "Error updating the role. Please try again.";
            return Page();
        }
        return RedirectToPage("./List");
    }
}