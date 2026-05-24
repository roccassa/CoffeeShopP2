using Coffee.Core.Dto;
using Coffee.WebSite.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;

namespace Coffee.WebSite.Pages.Product;

public class UpdateModel : PageModel
{
    private readonly IProductService _service;
    private readonly ICategoryService _categoryService;
    private readonly IProductVariantService _variantService;

    [BindProperty] public ProductDto ProductDto { get; set; } = new();
    [BindProperty] public string VariantsJson { get; set; } = "[]";

    public List<CategoryDto> Categories { get; set; } = new();
    public string ErrorMessage { get; set; } = string.Empty;

    public UpdateModel(IProductService service, ICategoryService categoryService, IProductVariantService variantService)
    {
        _service         = service;
        _categoryService = categoryService;
        _variantService  = variantService;
    }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        var response = await _service.GetByIdAsync(id);
        if (response?.Data == null) return RedirectToPage("./List");
        ProductDto = response.Data;

        var catTask = _categoryService.GetAllAsync();
        var varTask = _variantService.GetAllAsync();
        await Task.WhenAll(catTask, varTask);

        Categories = await catTask;
        // Deduplicate by size to avoid showing/saving duplicates that may have
        // accumulated in the DB. Keep the variant with the lowest Id (oldest,
        // most likely referenced in order history).
        var existing = ((await varTask).Data ?? new())
            .Where(v => v.ProductId == id)
            .GroupBy(v => (v.Size ?? "").Trim().ToLower())
            .Select(g => g.OrderBy(v => v.Id).First())
            .OrderBy(v => v.Price)
            .ToList();

        VariantsJson = JsonSerializer.Serialize(
            existing.Select(v => new { size = v.Size, price = v.Price }));

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

        var response = await _service.UpdateAsync(ProductDto);
        if (response?.Data == null)
        {
            ErrorMessage = "Error al actualizar el producto.";
            return Page();
        }

        // Sync variants: delete old ones, but if deletion fails (FK in DetalleOrden)
        // update the surviving variant instead of creating a duplicate.
        var varRes = await _variantService.GetAllAsync();
        var existingVariants = (varRes.Data ?? new())
            .Where(v => v.ProductId == ProductDto.Id).ToList();

        // Try to delete each existing variant; track those that couldn't be removed
        var notDeleted = new List<ProductVariantDto>();
        foreach (var v in existingVariants)
        {
            var delResult = await _variantService.DeleteAsync(v.Id);
            if (!(delResult?.Data ?? false))
                notDeleted.Add(v);
        }

        // For each variant in the form, update a surviving one or create a new one
        var newVariants = ParseVariants(VariantsJson)
            .Where(v => !string.IsNullOrWhiteSpace(v.Size)).ToList();

        foreach (var nv in newVariants)
        {
            var surviving = notDeleted.FirstOrDefault(r =>
                string.Equals(r.Size?.Trim(), nv.Size.Trim(), StringComparison.OrdinalIgnoreCase));

            if (surviving != null)
            {
                // Reuse existing variant (referenced in order history); just update price
                if (surviving.Price != nv.Price)
                {
                    surviving.Price = nv.Price;
                    await _variantService.UpdateAsync(surviving);
                }
                notDeleted.Remove(surviving);
            }
            else
            {
                await _variantService.CreateAsync(new ProductVariantDto
                {
                    ProductId = ProductDto.Id,
                    Size      = nv.Size.Trim(),
                    Price     = nv.Price
                });
            }
        }

        return RedirectToPage("./List");
    }

    private static List<VariantInput> ParseVariants(string json)
    {
        try { return JsonSerializer.Deserialize<List<VariantInput>>(json ?? "[]",
                  new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new(); }
        catch { return new(); }
    }
}
