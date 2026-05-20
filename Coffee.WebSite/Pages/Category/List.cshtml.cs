using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Coffee.Core.Dto;
using System.Collections.Generic;           // ← Agregar
using System.Linq;
using Microsoft.AspNetCore.Http;
using Coffee.Core.Http;
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
    
    [BindProperty]
    public InputModel Input { get; set; }

    // 2. Definición de la estructura de Input
    public class InputModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
    
    public IActionResult OnPost()
    {
        if (Input.Username?.Trim().ToLower() == "admin" && Input.Password == "1234")
        {
            HttpContext.Session.SetString("IsLoggedIn", "true");
            HttpContext.Session.SetString("Username", Input.Username);

            // Ruta correcta según tu estructura
            return RedirectToPage("/Category/List");
        }

        ViewData["Error"] = "Usuario o contraseña incorrectos";
        return Page();
    }
    
// Se ejecuta automáticamente al cargar la página (Petición GET)
    public async Task<IActionResult> OnGetAsync(string? searchTerm)
    {
        // Protección con Session
        if (HttpContext.Session.GetString("IsLoggedIn") != "true")
        {
            return RedirectToPage("/Account/Login");
        }

        SearchTerm = searchTerm ?? string.Empty;

        // "response" ahora contiene directamente el List<CategoryDto>
        var response = await _service.GetAllAsync();
    
        if (response != null)
        {
            // Filtramos directamente sobre la variable response sin usar .Data
            Categories = string.IsNullOrWhiteSpace(SearchTerm) 
                ? response 
                : response.Where(c => 
                    c.Name.Contains(SearchTerm, StringComparison.OrdinalIgnoreCase) || 
                    c.Id.ToString() == SearchTerm).ToList();
        }

        return Page();
    }
    
    // Handler especializado para traer los datos de un solo registro vía AJAX
    public async Task<JsonResult> OnGetGetByIdAsync(int id)
    {
        if (HttpContext.Session.GetString("IsLoggedIn") != "true")
        {
            return new JsonResult(new { success = false, message = "No autorizado" });
        }
        
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