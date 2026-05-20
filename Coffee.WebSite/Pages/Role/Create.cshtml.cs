using Coffee.Core.Dto;
using Coffee.WebSite.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Coffee.WebSite.Pages.Role;

public class CreateModel : PageModel
{
    private readonly IRoleService _service;

    [BindProperty]
    public RoleDto RoleDto { get; set; } = new();
    public string ErrorMessage { get; set; } = string.Empty;

    public CreateModel(IRoleService service) => _service = service;

    public void OnGet() { }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid) return Page();
        var response = await _service.CreateAsync(RoleDto);
        if (response?.Data == null)
        {
            ErrorMessage = "Error creating the role. Please try again.";
            return Page();
        }
        return RedirectToPage("./List");
    }
}