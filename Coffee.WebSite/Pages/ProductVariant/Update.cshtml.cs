using Coffee.Core.Dto;
using Coffee.WebSite.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Coffee.WebSite.Pages.ProductVariant;

public class UpdateModel : PageModel
{
    private readonly IProductVariantService _service;
    private readonly IProductService _productService;

    [BindProperty]
    public ProductVariantDto ProductVariantDto { get; set; } = new();
    public List<ProductDto> Products { get; set; } = new();
    public string ErrorMessage { get; set; } = string.Empty;

    public UpdateModel(IProductVariantService service, IProductService productService)
    {
        _service = service;
        _productService = productService;
    }

    private async Task LoadProductsAsync()
    {
        var res = await _productService.GetAllAsync();
        Products = (res.Data ?? new()).OrderBy(p => p.Name).ToList();
    }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        var response = await _service.GetByIdAsync(id);
        if (response?.Data == null) return RedirectToPage("./List");
        ProductVariantDto = response.Data;
        await LoadProductsAsync();
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        await LoadProductsAsync();
        if (!ModelState.IsValid) return Page();

        var response = await _service.UpdateAsync(ProductVariantDto);
        if (response?.Data == null)
        {
            ErrorMessage = "Error al actualizar la presentación.";
            return Page();
        }
        return RedirectToPage("./List");
    }
}
