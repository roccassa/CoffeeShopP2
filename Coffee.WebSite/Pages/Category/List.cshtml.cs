using Coffee.Core.Dto;
using Coffee.WebSite.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Coffee.WebSite.Pages.Category;

public class ListModel : PageModel
{
    private readonly ICategoryServices _service;
    public List<CategoryDto> Categories { get; set; } = new();

    [BindProperty(SupportsGet = true)]
    public string SearchTerm { get; set; } = string.Empty;

    public ListModel(ICategoryServices service)
    {
        _service = service;
    }

    public async Task<IActionResult> OnGet()
    {
        var response = await _service.GetAllAsync();
        var all = response.Data ?? new List<CategoryDto>();

        if (!string.IsNullOrWhiteSpace(SearchTerm))
            all = all.Where(c => c.Name.Contains(SearchTerm, StringComparison.OrdinalIgnoreCase)).ToList();

        Categories = all;
        return Page();
    }
}