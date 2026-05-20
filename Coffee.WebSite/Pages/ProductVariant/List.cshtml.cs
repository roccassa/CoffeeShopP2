using Coffee.Core.Dto;
using Coffee.WebSite.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Coffee.WebSite.Pages.ProductVariant;

[IgnoreAntiforgeryToken(Order = 1001)]
public class ListModel : PageModel
{
    private readonly IProductVariantService _service;
    public List<ProductVariantDto> ProductVariants { get; set; } = new();

    [BindProperty(SupportsGet = true)]
    public string SearchTerm { get; set; } = string.Empty;

    public ListModel(IProductVariantService service) => _service = service;

    public async Task<IActionResult> OnGetAsync()
    {
        var response = await _service.GetAllAsync();
        var all = response.Data ?? new List<ProductVariantDto>();

        if (!string.IsNullOrWhiteSpace(SearchTerm))
            all = all.Where(v =>
                v.Size.Contains(SearchTerm, StringComparison.OrdinalIgnoreCase) ||
                v.Id.ToString() == SearchTerm)
                .ToList();

        ProductVariants = all;
        return Page();
    }

    public async Task<JsonResult> OnPostDeleteAsync(int id)
    {
        var response = await _service.DeleteAsync(id);
        return new JsonResult(response.Data);
    }
}
