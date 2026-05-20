using Coffee.Core.Dto;
using Coffee.WebSite.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Coffee.WebSite.Pages.OrderDetail;

[IgnoreAntiforgeryToken(Order = 1001)]
public class ListModel : PageModel
{
    private readonly IOrderDetailService _service;
    public List<OrderDetailDto> OrderDetails { get; set; } = new();

    [BindProperty(SupportsGet = true)]
    public string SearchTerm { get; set; } = string.Empty;

    public ListModel(IOrderDetailService service) => _service = service;

    public async Task<IActionResult> OnGetAsync()
    {
        var response = await _service.GetAllAsync();
        var all = response.Data ?? new List<OrderDetailDto>();

        if (!string.IsNullOrWhiteSpace(SearchTerm))
            all = all.Where(d =>
                d.Id.ToString() == SearchTerm ||
                d.OrderId.ToString() == SearchTerm)
                .ToList();

        OrderDetails = all;
        return Page();
    }

    public async Task<JsonResult> OnPostDeleteAsync(int id)
    {
        var response = await _service.DeleteAsync(id);
        return new JsonResult(response.Data);
    }
}
