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
    public string ErrorMessage { get; set; } = string.Empty;

    public CreateModel(IProductVariantService variantService, IProductService productService)
    {
        _variantService = variantService;
        _productService = productService;
    }
    public void OnGet() { }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid) return Page();

        try
        {
            // 1. Validar si el Producto Padre existe en el catálogo general
            var productCheck = await _productService.GetByIdAsync(ProductVariantDto.ProductId);
            if (productCheck == null)
            {
                ErrorMessage =
                    $"⚠️ Error de Estructura: El ID de Producto ({ProductVariantDto.ProductId}) no existe en el catálogo.";
                return Page();
            }

            // 2. Validación lógica comercial extra: El precio no puede ser negativo
            if (ProductVariantDto.Price < 0)
            {
                ErrorMessage = "⚠️ El precio de venta al público no puede ser un valor negativo.";
                return Page();
            }

            // 3. Intentar registrar la variante física en la API
            // Si el ID de producto no existe y falló la validación previa, la API lanzará el error de MySQL aquí,
            // pero ahora el bloque 'catch' lo va a interceptar de inmediato.
            var response = await _variantService.CreateAsync(ProductVariantDto);

            if (response != null && response.Success)
            {
                return RedirectToPage("./List");
            }

            ErrorMessage = response?.Message ?? "Error inesperado al registrar la nueva presentación comercial.";
            return Page();
        }
        catch (Exception ex)
        {
            // Aquí atrapamos el JsonReaderException provocada por la respuesta no-JSON de la API
            ErrorMessage =
                $"⚠️ Error de Integridad: El ID de Producto ({ProductVariantDto.ProductId}) es inválido o causó un conflicto en la base de datos.";
            return Page();
        }
    }
}
