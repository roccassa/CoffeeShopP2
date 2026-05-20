using Coffee.Core.Dto;
using Coffee.WebSite.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Coffee.WebSite.Pages.ProductVariant;

public class UpdateModel : PageModel
{
    private readonly IProductVariantService _service;

    [BindProperty]
    public ProductVariantDto ProductVariantDto { get; set; } = new();
    public string ErrorMessage { get; set; } = string.Empty;

    public UpdateModel(IProductVariantService service) => _service = service;

    public async Task<IActionResult> OnGetAsync(int id)
    {
        var response = await _service.GetByIdAsync(id);
        if (response?.Data == null) return RedirectToPage("./List");
        ProductVariantDto = response.Data;
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid) return Page();
        var response = await _service.UpdateAsync(ProductVariantDto);
        if (response?.Data == null)
        {
            ErrorMessage = "Error al actualizar la presentación. Intente de nuevo.";
            return Page();
        }
        return RedirectToPage("./List");
    }
}
