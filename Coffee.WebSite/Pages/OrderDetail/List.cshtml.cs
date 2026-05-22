using Coffee.Core.Dto;
using Coffee.WebSite.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Coffee.WebSite.Pages.OrderDetail;

[IgnoreAntiforgeryToken(Order = 1001)]
public class ListModel : PageModel
{
    private readonly IOrderDetailService _detailService;
    private readonly IOrderService _orderService;

    public List<OrderDetailDto> OrderDetails { get; set; } = new();
    public OrderDto? CurrentOrder { get; set; }

    [BindProperty(SupportsGet = true)]
    public string SearchTerm { get; set; } = string.Empty;

    [BindProperty(SupportsGet = true)]
    public int OrderId { get; set; }

    public ListModel(IOrderDetailService detailService, IOrderService orderService)
    {
        _detailService = detailService;
        _orderService  = orderService;
    }

    public async Task<IActionResult> OnGetAsync()
    {
        if (HttpContext.Session.GetString("IsLoggedIn") != "true")
            return RedirectToPage("/Account/Login");

        var detailResponse = await _detailService.GetAllAsync();
        var all = detailResponse.Data ?? new();

        if (OrderId > 0)
        {
            all = all.Where(d => d.OrderId == OrderId).ToList();
            try
            {
                var orderResp = await _orderService.GetByIdAsync(OrderId);
                CurrentOrder = orderResp.Data;
            }
            catch { /* order not found – stay null */ }
        }
        else if (!string.IsNullOrWhiteSpace(SearchTerm))
        {
            all = all.Where(d =>
                d.Id.ToString()      == SearchTerm ||
                d.OrderId.ToString() == SearchTerm ||
                (d.PresentationName ?? "").Contains(SearchTerm, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        OrderDetails = all;
        return Page();
    }

    public async Task<JsonResult> OnPostDeleteAsync(int id)
    {
        var response = await _detailService.DeleteAsync(id);
        return new JsonResult(response.Data);
    }
}
