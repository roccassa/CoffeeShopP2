using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Coffee.Core.Dto;
using Coffee.WebSite.Services.Interfaces;

namespace Coffee.WebSite.Pages.Customer;

[IgnoreAntiforgeryToken(Order = 1001)]
public class ListModel : PageModel
{
    private readonly ICustomerService _service;
    public List<CustomerDto> Customers { get; set; } = new();
    public string SearchTerm { get; set; } = string.Empty;

    public ListModel(ICustomerService service) => _service = service;

    public async Task<IActionResult> OnGetAsync(string? searchTerm)
    {
        SearchTerm = searchTerm ?? string.Empty;
        var data = await _service.GetAllAsync();

        Customers = string.IsNullOrWhiteSpace(SearchTerm)
            ? data
            : data.Where(c =>
                c.Name.Contains(SearchTerm, StringComparison.OrdinalIgnoreCase) ||
                (c.Email != null && c.Email.Contains(SearchTerm, StringComparison.OrdinalIgnoreCase)) ||
                c.Id.ToString() == SearchTerm).ToList();

        return Page();
    }

    public async Task<JsonResult> OnPostDeleteAsync(int id)
    {
        var result = await _service.DeleteAsync(id);
        return new JsonResult(result);
    }
}