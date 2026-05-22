using Coffee.Core.Dto;
using Coffee.WebSite.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Coffee.WebSite.Pages.OrderDetail;

public class UpdateModel : PageModel
{
    private readonly IOrderDetailService _orderDetailService;
    private readonly IOrderService _orderService;
    private readonly IProductVariantService _variantService;

    [BindProperty]
    public OrderDetailDto OrderDetailDto { get; set; } = new();
    public List<OrderDto> Orders { get; set; } = new();
    public List<ProductVariantDto> Variants { get; set; } = new();
    public string ErrorMessage { get; set; } = string.Empty;

    public UpdateModel(IOrderDetailService orderDetailService, IOrderService orderService, IProductVariantService variantService)
    {
        _orderDetailService = orderDetailService;
        _orderService       = orderService;
        _variantService     = variantService;
    }

    private async Task LoadCatalogAsync()
    {
        var ordRes = await _orderService.GetAllAsync();
        Orders = (ordRes.Data ?? new()).OrderByDescending(o => o.Id).Take(50).ToList();

        var varRes = await _variantService.GetAllAsync();
        Variants = (varRes.Data ?? new()).OrderBy(v => v.ProductName).ThenBy(v => v.Size).ToList();
    }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        var response = await _orderDetailService.GetByIdAsync(id);
        if (response?.Data == null) return RedirectToPage("./List");
        OrderDetailDto = response.Data;
        await LoadCatalogAsync();
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        await LoadCatalogAsync();
        if (!ModelState.IsValid) return Page();

        var response = await _orderDetailService.UpdateAsync(OrderDetailDto);
        if (response?.Data == null)
        {
            ErrorMessage = "Error al actualizar el concepto. Intente de nuevo.";
            return Page();
        }
        return RedirectToPage("./List");
    }
}
