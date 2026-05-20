using Coffee.Core.Dto;
using Coffee.WebSite.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Coffee.WebSite.Pages.OrderDetail;

public class CreateModel : PageModel
{
    private readonly IOrderDetailService _service;

    [BindProperty]
    public OrderDetailDto OrderDetailDto { get; set; } = new();
    public string ErrorMessage { get; set; } = string.Empty;

    public CreateModel(IOrderDetailService service) => _service = service;

    public void OnGet() { }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid) return Page();
        var response = await _service.CreateAsync(OrderDetailDto);
        if (response?.Data == null)
        {
            ErrorMessage = "Error al crear el detalle. Intente de nuevo.";
            return Page();
        }
        return RedirectToPage("./List");
    }
}
