using Coffee.Core.Dto;
using Coffee.WebSite.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Coffee.WebSite.Pages.Order;

public class CreateModel : PageModel
{
    private readonly IOrderService _service;

    public CreateModel(IOrderService service)
    {
        _service = service;
    }

    [BindProperty]
    public OrderDto OrderDto { get; set; } = new();

    public string ErrorMessage { get; set; } = string.Empty;

    public void OnGet()
    {
 
        OrderDto.Status = "Pendiente";
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        try
        {
          
            if (string.IsNullOrWhiteSpace(OrderDto.Status) || OrderDto.Status.Equals("Pending", StringComparison.OrdinalIgnoreCase))
            {
                OrderDto.Status = "Pendiente";
            }

            var response = await _service.CreateAsync(OrderDto);

            if (response == null || response.Data == null)
            {
                ErrorMessage = "El servidor no devolvió datos válidos al intentar crear la orden.";
                return Page();
            }

            return RedirectToPage("./List");
        }
        catch (Exception ex)
        {
            ErrorMessage = $"No se pudo procesar la solicitud. Detalle: {ex.Message}";
            return Page();
        }
    }
}