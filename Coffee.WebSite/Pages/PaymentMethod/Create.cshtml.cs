using Coffee.Core.Dto;
using Coffee.WebSite.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Coffee.WebSite.Pages.PaymentMethod;

public class CreateModel : PageModel
{
    private readonly IPaymentMethodService _service;

    [BindProperty]
    public PaymentMethodDto PaymentMethodDto { get; set; } = new();
    public string ErrorMessage { get; set; } = string.Empty;

    public CreateModel(IPaymentMethodService service) => _service = service;

    public void OnGet() { }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid) return Page();
        var response = await _service.CreateAsync(PaymentMethodDto);
        if (response?.Data == null)
        {
            ErrorMessage = "Error al crear el método de pago. Intente de nuevo.";
            return Page();
        }
        return RedirectToPage("./List");
    }
}
