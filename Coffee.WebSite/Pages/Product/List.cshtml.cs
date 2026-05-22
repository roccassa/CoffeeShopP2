using Coffee.Core.Dto;
using Coffee.WebSite.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Coffee.WebSite.Pages.Product;

[IgnoreAntiforgeryToken(Order = 1001)]
public class ListModel : PageModel
{
    private readonly IProductService _service;
    private readonly IProductVariantService _variantService;

    public List<ProductDto> Products { get; set; } = new();
    public Dictionary<int, List<ProductVariantDto>> VariantsByProduct { get; set; } = new();

    [BindProperty(SupportsGet = true)]
    public string SearchTerm { get; set; } = string.Empty;

    public ListModel(IProductService service, IProductVariantService variantService)
    {
        _service        = service;
        _variantService = variantService;
    }

    public async Task<IActionResult> OnGetAsync()
    {
        var prodTask    = _service.GetAllAsync();
        var variantTask = _variantService.GetAllAsync();
        await Task.WhenAll(prodTask, variantTask);

        var all = (await prodTask).Data ?? new();

        if (!string.IsNullOrWhiteSpace(SearchTerm))
            all = all.Where(p =>
                p.Name.Contains(SearchTerm, StringComparison.OrdinalIgnoreCase) ||
                p.Id.ToString() == SearchTerm)
                .ToList();

        Products = all;

        VariantsByProduct = ((await variantTask).Data ?? new())
            .GroupBy(v => v.ProductId)
            .ToDictionary(g => g.Key, g => g.OrderBy(v => v.Price).ToList());

        return Page();
    }

    public async Task<JsonResult> OnPostDeleteAsync(int id)
    {
        try
        {
            // Primero eliminar todas las presentaciones del producto (FK constraint)
            var varRes = await _variantService.GetAllAsync();
            var variants = (varRes.Data ?? new()).Where(v => v.ProductId == id).ToList();
            foreach (var v in variants)
                await _variantService.DeleteAsync(v.Id);

            // Luego eliminar el producto
            var response = await _service.DeleteAsync(id);
            return new JsonResult(new { success = response.Data });
        }
        catch (Exception ex)
        {
            return new JsonResult(new { success = false, message = ex.Message });
        }
    }
}
