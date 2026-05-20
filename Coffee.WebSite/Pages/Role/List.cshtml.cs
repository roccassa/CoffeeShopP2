using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Coffee.Core.Dto;
using Coffee.WebSite.Services.Interfaces;

namespace Coffee.WebSite.Pages.Role;

[IgnoreAntiforgeryToken(Order = 1001)]
public class ListModel : PageModel
{
    private readonly IRoleService _service;
    public List<RoleDto> Roles { get; set; } = new();
    public string SearchTerm { get; set; } = string.Empty;

    public ListModel(IRoleService service) => _service = service;

    public async Task<IActionResult> OnGetAsync(string? searchTerm)
    {
        SearchTerm = searchTerm ?? string.Empty;
        var data = await _service.GetAllAsync();

        Roles = string.IsNullOrWhiteSpace(SearchTerm)
            ? data
            : data.Where(r =>
                r.Name.Contains(SearchTerm, StringComparison.OrdinalIgnoreCase) ||
                r.Id.ToString() == SearchTerm).ToList();

        return Page();
    }

    public async Task<JsonResult> OnPostDeleteAsync(int id)
    {
        var result = await _service.DeleteAsync(id);
        return new JsonResult(result);
    }
}