using Coffee.Core.Dto;
using Coffee.WebSite.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;

namespace Coffee.WebSite.Pages.Product;

public class CreateModel : PageModel
{
    private readonly IProductService _service;
    private readonly ICategoryService _categoryService;
    private readonly IProductVariantService _variantService;

    [BindProperty] public ProductDto ProductDto { get; set; } = new();
    [BindProperty] public string VariantsJson { get; set; } = "[]";

    public List<CategoryDto> Categories { get; set; } = new();
    public string ErrorMessage { get; set; } = string.Empty;

    public CreateModel(IProductService service, ICategoryService categoryService, IProductVariantService variantService)
    {
        _service        = service;
        _categoryService = categoryService;
        _variantService = variantService;
    }

    public async Task<IActionResult> OnGetAsync()
    {
        Categories = await _categoryService.GetAllAsync();
        ProductDto.IsActive = true;
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        Categories = await _categoryService.GetAllAsync();
        if (!ModelState.IsValid) return Page();

        if (ProductDto.CategoryId <= 0)
        {
            ErrorMessage = "Selecciona una categoría válida.";
            return Page();
        }

        // ── Crear producto ──────────────────────────────────────
        string rawJson = "(sin respuesta)";
        try
        {
            var response = await _service.CreateAsync(ProductDto);
            if (!response.Success)
            {
                ErrorMessage = $"Error al crear el producto: {response?.Message ?? "sin detalle"}.";
                return Page();
            }

            var newProductId = response.Data?.Id ?? 0;
            if (newProductId <= 0)
            {
                ErrorMessage = "Producto guardado pero la API devolvió Id=0. Revisa que la API esté corriendo y que la BD tenga la columna imagen_url.";
                return Page();
            }

            // ── Crear presentaciones ────────────────────────────
            var variants = ParseVariants(VariantsJson)
                .Where(v => !string.IsNullOrWhiteSpace(v.Size))
                .ToList();

            var variantErrors = new List<string>();
            foreach (var v in variants)
            {
                try
                {
                    var varRes = await _variantService.CreateAsync(new ProductVariantDto
                    {
                        ProductId = newProductId,
                        Size      = v.Size.Trim(),
                        Price     = v.Price
                    });
                    if (!varRes.Success)
                        variantErrors.Add($"'{v.Size}': {varRes.Message}");
                }
                catch (Exception ex)
                {
                    variantErrors.Add($"'{v.Size}': {ex.Message}");
                }
            }

            if (variantErrors.Any())
            {
                ErrorMessage = "Producto creado pero hubo errores en las presentaciones: " +
                               string.Join(" | ", variantErrors);
                return Page();
            }
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Error de conexión con la API: {ex.Message}";
            return Page();
        }

        return RedirectToPage("/Product/List");
    }

    private static List<VariantInput> ParseVariants(string json)
    {
        try { return JsonSerializer.Deserialize<List<VariantInput>>(json ?? "[]",
                  new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new(); }
        catch { return new(); }
    }
}
