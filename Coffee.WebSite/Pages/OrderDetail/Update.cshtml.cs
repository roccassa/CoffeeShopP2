using Coffee.Core.Dto;
using Coffee.WebSite.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Coffee.WebSite.Pages.OrderDetail;

public class UpdateModel : PageModel
{
    private readonly IOrderDetailService _service;

    [BindProperty]
    public OrderDetailDto OrderDetailDto { get; set; } = new();
    public string ErrorMessage { get; set; } = string.Empty;

    public UpdateModel(IOrderDetailService service) => _service = service;

    public async Task<IActionResult> OnGetAsync(int id)
    {
        var response = await _service.GetByIdAsync(id);
        if (response?.Data == null) return RedirectToPage("./List");
        OrderDetailDto = response.Data;
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid) return Page();
        var response = await _service.UpdateAsync(OrderDetailDto);
        if (response?.Data == null)
        {
            ErrorMessage = "Error al actualizar el detalle. Intente de nuevo.";
            return Page();
        }
        return RedirectToPage("./List");
    }
}
