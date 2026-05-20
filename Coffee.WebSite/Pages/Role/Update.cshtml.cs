using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Coffee.Core.Dto;
using Coffee.WebSite.Services.Interfaces;

namespace Coffee.WebSite.Pages.Role;

public class UpdateModel : PageModel
{
    private readonly IRoleService _service;

    [BindProperty]
    public RoleDto RoleDto { get; set; } = new();

    public UpdateModel(IRoleService service) => _service = service;

    public async Task<IActionResult> OnGetAsync(int id)
    {
        RoleDto = await _service.GetByIdAsync(id);
        if (RoleDto == null) return RedirectToPage("./List");
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid) return Page();
        await _service.UpdateAsync(RoleDto);
        return RedirectToPage("./List");
    }
}