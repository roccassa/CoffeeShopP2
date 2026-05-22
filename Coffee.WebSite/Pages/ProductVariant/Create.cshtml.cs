using Coffee.Core.Dto;
using Coffee.WebSite.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Coffee.WebSite.Pages.ProductVariant;

public class CreateModel : PageModel
{
    private readonly IProductVariantService _variantService;
    private readonly IProductService _productService;

    [BindProperty]
    public ProductVariantDto ProductVariantDto { get; set; } = new();
    public List<ProductDto> Products { get; set; } = new();
    public string ErrorMessage { get; set; } = string.Empty;

    public CreateModel(IProductVariantService variantService, IProductService productService)
    {
        _variantService = variantService;
        _productService = productService;
    }

    [BindProperty(SupportsGet = true)]
    public int ProductId { get; set; }

    public async Task<IActionResult> OnGetAsync()
    {
        await LoadProductsAsync();
        if (ProductId > 0)
            ProductVariantDto.ProductId = ProductId;
        return Page();
    }

    private async Task LoadProductsAsync()
    {
        var res = await _productService.GetAllAsync();
        Products = (res.Data ?? new()).Where(p => p.IsActive).OrderBy(p => p.Name).ToList();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        await LoadProductsAsync();
        if (!ModelState.IsValid) return Page();

        if (ProductVariantDto.ProductId <= 0)
        {
            ErrorMessage = "Selecciona un producto válido.";
            return Page();
        }

        if (ProductVariantDto.Price < 0)
        {
            ErrorMessage = "El precio no puede ser negativo.";
            return Page();
        }

        try
        {
            var response = await _variantService.CreateAsync(ProductVariantDto);
            if (response != null && response.Success)
                return RedirectToPage("./List");

            ErrorMessage = response?.Message ?? "Error al registrar la presentación.";
            return Page();
        }
        catch
        {
            ErrorMessage = "Error de integridad: verifica que el producto seleccionado exista en la base de datos.";
            return Page();
        }
    }
}
