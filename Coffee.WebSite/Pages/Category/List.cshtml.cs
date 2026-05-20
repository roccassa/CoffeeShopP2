using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Coffee.Core.Dto;
using Coffee.WebSite.Services.Interfaces;

namespace Coffee.WebSite.Pages.Category;

[IgnoreAntiforgeryToken(Order = 1001)]
public class ListModel : PageModel  
{
    private readonly ICategoryService _service;
    
    // Propiedades expuestas directamente a la vista HTML
    public List<CategoryDto> Categories { get; set; } 
    public string SearchTerm { get; set; } = string.Empty;

    public ListModel(ICategoryService service)
    {
        Categories = new List<CategoryDto>();
        _service = service;
    }

    // Se ejecuta automáticamente al cargar la página (Petición GET)
    public async Task<IActionResult> OnGetAsync(string? searchTerm)
    {
        SearchTerm = searchTerm ?? string.Empty;

        // Ahora recibe directamente la lista limpia de categorías
        var data = await _service.GetAllAsync();
    
        if (data != null)
        {
            if (!string.IsNullOrWhiteSpace(SearchTerm))
            {
                Categories = data.Where(c =>
                    c.Name.Contains(SearchTerm, StringComparison.OrdinalIgnoreCase) ||
                    (c.Id.ToString() == SearchTerm)
                ).ToList();
            }
            else
            {
                Categories = data;
            }
        }

        return Page();
    }
    
    // Handler especializado para traer los datos de un solo registro vía AJAX
    public async Task<JsonResult> OnGetGetByIdAsync(int id)
    {
        var response = await _service.GetByIdAsync(id);
        return new JsonResult(response);
    }
    
    // Handler especializado para procesar la baja lógica vía Fetch API sin recargar la página
    public async Task<JsonResult> OnPostDeleteAsync(int id)
    {
        var response = await _service.DeleteAsync(id);
        return new JsonResult(response);
    }
}