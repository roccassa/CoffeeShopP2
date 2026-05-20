using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Coffee.Core.Dto;
using Coffee.WebSite.Services.Interfaces;

namespace Coffee.WebSite.Pages.Role;

public class CreateModel : PageModel
{
    private readonly IRoleService _service;

    [BindProperty]
    public RoleDto RoleDto { get; set; } = new();

    public CreateModel(IRoleService service) => _service = service;

    public void OnGet() { }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid) return Page();
        await _service.CreateAsync(RoleDto);
        return RedirectToPage("./List");
    }
}