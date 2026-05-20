using Coffee.Core.Dto;
using Coffee.WebSite.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Coffee.WebSite.Pages.Role;

[IgnoreAntiforgeryToken(Order = 1001)]
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
                    r.Name.Contains(SearchTerm, StringComparison.OrdinalIgnoreCase) ||
                    r.Id.ToString() == SearchTerm)
                .ToList();

        Roles = all;
        return Page();
    }

    public async Task<JsonResult> OnPostDeleteAsync(int id)
    {
        var response = await _service.DeleteAsync(id);
        return new JsonResult(response.Data);
    }
}