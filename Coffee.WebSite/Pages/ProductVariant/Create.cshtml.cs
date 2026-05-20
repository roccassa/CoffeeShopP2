using Coffee.Core.Dto;
using Coffee.WebSite.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Coffee.WebSite.Pages.ProductVariant;

public class CreateModel : PageModel
{
    private readonly IProductVariantService _service;

    [BindProperty]
    public ProductVariantDto ProductVariantDto { get; set; } = new();
    public string ErrorMessage { get; set; } = string.Empty;

    public CreateModel(IProductVariantService service) => _service = service;

    public void OnGet() { }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid) return Page();
        var response = await _service.CreateAsync(ProductVariantDto);
        if (response?.Data == null)
        {
            ErrorMessage = "Error al crear la presentación. Intente de nuevo.";
            return Page();
        }
        return RedirectToPage("./List");
    }
}
