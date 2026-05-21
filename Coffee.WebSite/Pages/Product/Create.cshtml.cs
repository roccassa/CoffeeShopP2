using Coffee.Core.Dto;
using Coffee.WebSite.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Coffee.WebSite.Pages.Product;

public class CreateModel : PageModel
{
    private readonly IProductService _service;
    private readonly ICategoryService _categoryService; // 1. Inyectamos el servicio de categorías

    [BindProperty]
    public ProductDto ProductDto { get; set; } = new();
    public string ErrorMessage { get; set; } = string.Empty;

    public CreateModel(IProductService service, ICategoryService categoryService)
    {
        _service = service;
        _categoryService = categoryService;
    }
    
    public void OnGet() { }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid) return Page();
        // 3. Validar si la Categoría asignada existe en CafeteriaDB
        var categoryCheck = await _categoryService.GetByIdAsync(ProductDto.CategoryId);

        // Si es nulo, significa que el ID no corresponde a ninguna categoría real
        if (categoryCheck == null)
        {
            ErrorMessage = $"⚠️ Error de Integridad: El ID de Categoría ({ProductDto.CategoryId}) no existe en el sistema. Debe asignar una categoría válida.";
            return Page();
        }

        // 4. Si la categoría existe, intentamos registrar el producto comercial
        var response = await _service.CreateAsync(ProductDto);
        
        // Validamos usando la propiedad .Success de tu estructura de respuestas
        if (response.Success)
        {
            return RedirectToPage("/Product/List");
        }
        
        // Mensaje de respaldo si la API falla por otra razón (ej. nombre duplicado)
        ErrorMessage = response?.Message ?? "Error al crear el producto. Intente de nuevo.";
        return Page();
    }
}
