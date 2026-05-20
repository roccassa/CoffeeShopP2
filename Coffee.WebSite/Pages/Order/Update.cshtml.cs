using Coffee.Core.Dto;
using Coffee.WebSite.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Coffee.WebSite.Pages.Order;

public class UpdateModel : PageModel
{
    private readonly IOrderService _service;

    [BindProperty]
    public OrderDto OrderDto { get; set; } = new();
    public string ErrorMessage { get; set; } = string.Empty;

    public UpdateModel(IOrderService service) => _service = service;

    public async Task<IActionResult> OnGetAsync(int id)
    {
        var response = await _service.GetByIdAsync(id);
        if (response?.Data == null) return RedirectToPage("./List");
        OrderDto = response.Data;
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid) return Page();
        var response = await _service.UpdateAsync(OrderDto);
        if (response?.Data == null)
        {
            ErrorMessage = "Error al actualizar la orden. Intente de nuevo.";
            return Page();
        }
        return RedirectToPage("./List");
    }
}
