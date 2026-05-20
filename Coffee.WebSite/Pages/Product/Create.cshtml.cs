using Coffee.Core.Dto;
using Coffee.WebSite.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Coffee.WebSite.Pages.Product;

public class CreateModel : PageModel
{
    private readonly IProductService _service;

    [BindProperty]
    public ProductDto ProductDto { get; set; } = new();
    public string ErrorMessage { get; set; } = string.Empty;

    public CreateModel(IProductService service) => _service = service;

    public void OnGet() { }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid) return Page();
        var response = await _service.CreateAsync(ProductDto);
        if (response?.Data == null)
        {
            ErrorMessage = "Error al crear el producto. Intente de nuevo.";
            return Page();
        }
        return RedirectToPage("./List");
    }
}
