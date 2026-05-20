using Coffee.Core.Dto;
using Coffee.WebSite.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Coffee.WebSite.Pages.Order;

[IgnoreAntiforgeryToken(Order = 1001)]
public class ListModel : PageModel
{
    private readonly IOrderService _service;
    public List<OrderDto> Orders { get; set; } = new();

    [BindProperty(SupportsGet = true)]
    public string SearchTerm { get; set; } = string.Empty;

    public ListModel(IOrderService service) => _service = service;

    public async Task<IActionResult> OnGetAsync()
    {
        var response = await _service.GetAllAsync();
        var all = response.Data ?? new List<OrderDto>();

        if (!string.IsNullOrWhiteSpace(SearchTerm))
            all = all.Where(o =>
                o.Status.Contains(SearchTerm, StringComparison.OrdinalIgnoreCase) ||
                o.Id.ToString() == SearchTerm)
                .ToList();

        Orders = all;
        return Page();
    }

    public async Task<JsonResult> OnPostDeleteAsync(int id)
    {
        var response = await _service.DeleteAsync(id);
        return new JsonResult(response.Data);
    }
}
