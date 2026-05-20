using Coffee.Core.Dto;
using Coffee.WebSite.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Coffee.WebSite.Pages.PaymentMethod;

[IgnoreAntiforgeryToken(Order = 1001)]
public class ListModel : PageModel
{
    private readonly IPaymentMethodService _service;
    public List<PaymentMethodDto> PaymentMethods { get; set; } = new();

    [BindProperty(SupportsGet = true)]
    public string SearchTerm { get; set; } = string.Empty;

    public ListModel(IPaymentMethodService service) => _service = service;

    public async Task<IActionResult> OnGetAsync()
    {
        var response = await _service.GetAllAsync();
        var all = response.Data ?? new List<PaymentMethodDto>();

        if (!string.IsNullOrWhiteSpace(SearchTerm))
            all = all.Where(m =>
                m.Name.Contains(SearchTerm, StringComparison.OrdinalIgnoreCase) ||
                m.Id.ToString() == SearchTerm)
                .ToList();

        PaymentMethods = all;
        return Page();
    }

    public async Task<JsonResult> OnPostDeleteAsync(int id)
    {
        var response = await _service.DeleteAsync(id);
        return new JsonResult(response.Data);
    }
}
