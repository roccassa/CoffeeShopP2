using Coffee.Core.Dto;
using Coffee.WebSite.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Coffee.WebSite.Pages.Customer;

[IgnoreAntiforgeryToken(Order = 1001)]
public class ListModel : PageModel
{
    private readonly ICustomerService _service;
    public List<CustomerDto> Customers { get; set; } = new();

    [BindProperty(SupportsGet = true)]
    public string SearchTerm { get; set; } = string.Empty;

    public ListModel(ICustomerService service) => _service = service;

    public async Task<IActionResult> OnGetAsync()
    {
        var response = await _service.GetAllAsync();
        var all = response.Data ?? new List<CustomerDto>();

        if (!string.IsNullOrWhiteSpace(SearchTerm))
            all = all.Where(c =>
                    c.Name.Contains(SearchTerm, StringComparison.OrdinalIgnoreCase) ||
                    (c.Email != null && c.Email.Contains(SearchTerm, StringComparison.OrdinalIgnoreCase)) ||
                    c.Id.ToString() == SearchTerm)
                .ToList();

        Customers = all;
        return Page();
    }

    public async Task<JsonResult> OnPostDeleteAsync(int id)
    {
        var response = await _service.DeleteAsync(id);
        return new JsonResult(response.Data);
    }
}