using Coffee.Core.Dto;
using Coffee.WebSite.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Coffee.WebSite.Pages.Order;

public class CreateModel : PageModel
{
    private readonly IOrderService _service;

    [BindProperty]
    public OrderDto OrderDto { get; set; } = new();
    public string ErrorMessage { get; set; } = string.Empty;

    public CreateModel(IOrderService service) => _service = service;

    public void OnGet() { }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid) return Page();
        var response = await _service.CreateAsync(OrderDto);
        if (response?.Data == null)
        {
            ErrorMessage = "Error al crear la orden. Intente de nuevo.";
            return Page();
        }
        return RedirectToPage("./List");
    }
}
