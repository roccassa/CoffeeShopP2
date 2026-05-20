using Coffee.Core.Dto;
using Coffee.WebSite.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Coffee.WebSite.Pages.PaymentMethod;

public class UpdateModel : PageModel
{
    private readonly IPaymentMethodService _service;

    [BindProperty]
    public PaymentMethodDto PaymentMethodDto { get; set; } = new();
    public string ErrorMessage { get; set; } = string.Empty;

    public UpdateModel(IPaymentMethodService service) => _service = service;

    public async Task<IActionResult> OnGetAsync(int id)
    {
        var response = await _service.GetByIdAsync(id);
        if (response?.Data == null) return RedirectToPage("./List");
        PaymentMethodDto = response.Data;
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid) return Page();
        var response = await _service.UpdateAsync(PaymentMethodDto);
        if (response?.Data == null)
        {
            ErrorMessage = "Error al actualizar el método de pago. Intente de nuevo.";
            return Page();
        }
        return RedirectToPage("./List");
    }
}
