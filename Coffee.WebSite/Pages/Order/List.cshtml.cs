using Coffee.Core.Dto;
using Coffee.WebSite.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Coffee.WebSite.Pages.Order;

[IgnoreAntiforgeryToken(Order = 1001)]
public class ListModel : PageModel
{
    private readonly IOrderService _orderService;
    private readonly IOrderDetailService _detailService;

    public List<OrderDto> Orders { get; set; } = new();

    [BindProperty(SupportsGet = true)]
    public string SearchTerm { get; set; } = string.Empty;

    public ListModel(IOrderService orderService, IOrderDetailService detailService)
    {
        _orderService  = orderService;
        _detailService = detailService;
    }

    public async Task<IActionResult> OnGetAsync()
    {
        if (HttpContext.Session.GetString("IsLoggedIn") != "true")
            return RedirectToPage("/Account/Login");

        var response = await _orderService.GetAllAsync();
        var all = response.Data ?? new();

        if (!string.IsNullOrWhiteSpace(SearchTerm))
            all = all.Where(o =>
                o.Status.Contains(SearchTerm, StringComparison.OrdinalIgnoreCase) ||
                o.Id.ToString() == SearchTerm ||
                (o.UserName ?? "").Contains(SearchTerm, StringComparison.OrdinalIgnoreCase) ||
                (o.PaymentMethodName ?? "").Contains(SearchTerm, StringComparison.OrdinalIgnoreCase))
                .ToList();

        // Sort by ID desc (most recent first)
        Orders = all.OrderByDescending(o => o.Id).ToList();
        return Page();
    }

    // AJAX: return ticket details for a given order
    public async Task<JsonResult> OnGetTicketAsync(int orderId)
    {
        var response = await _detailService.GetAllAsync();
        var details = (response.Data ?? new())
            .Where(d => d.OrderId == orderId)
            .Select(d => new {
                d.Id,
                d.OrderId,
                d.ProductVariantId,
                d.Quantity,
                d.UnitPrice,
                ProductName      = d.ProductName      ?? "—",
                PresentationName = d.PresentationName ?? "—",
                Subtotal = d.Quantity * d.UnitPrice
            })
            .ToList();

        return new JsonResult(details);
    }

    public async Task<JsonResult> OnPostDeleteAsync(int id)
    {
        var response = await _orderService.DeleteAsync(id);
        return new JsonResult(response.Data);
    }
}
