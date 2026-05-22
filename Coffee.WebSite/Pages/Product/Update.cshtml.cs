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
        var existing = ((await varTask).Data ?? new())
            .Where(v => v.ProductId == id)
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

        // Replace all variants for this product
        var varRes = await _variantService.GetAllAsync();
        var toDelete = (varRes.Data ?? new()).Where(v => v.ProductId == ProductDto.Id).ToList();
        foreach (var v in toDelete)
            await _variantService.DeleteAsync(v.Id);

        foreach (var v in ParseVariants(VariantsJson).Where(v => !string.IsNullOrWhiteSpace(v.Size)))
        {
            await _variantService.CreateAsync(new ProductVariantDto
            {
                ProductId = ProductDto.Id,
                Size      = v.Size.Trim(),
                Price     = v.Price
            });
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
