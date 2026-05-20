using Coffee.Core.Dto;
using Coffee.WebSite.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Coffee.WebSite.Pages.Role;

public class ListModel : PageModel
{
    private readonly IRoleService _service;
    public List<RoleDto> Roles { get; set; } = new();

    [BindProperty(SupportsGet = true)]
    public string SearchTerm { get; set; } = string.Empty;

    public ListModel(IRoleService service) => _service = service;

    public async Task<IActionResult> OnGetAsync()
    {
        var response = await _service.GetAllAsync();
        var all = response.Data ?? new List<RoleDto>();

        if (!string.IsNullOrWhiteSpace(SearchTerm))
            all = all.Where(r =>
                    r.Name.Contains(SearchTerm, StringComparison.OrdinalIgnoreCase))
                .ToList();

        Roles = all;
        return Page();
    }
}