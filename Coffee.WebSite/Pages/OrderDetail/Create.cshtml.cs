using Coffee.Core.Dto;
using Coffee.WebSite.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Coffee.WebSite.Pages.OrderDetail;

public class CreateModel : PageModel
{
    private readonly IOrderDetailService _orderDetailService;
    private readonly IOrderService _orderService;
    private readonly IProductVariantService _variantService;

    [BindProperty]
    public OrderDetailDto OrderDetailDto { get; set; } = new();
    public List<OrderDto> Orders { get; set; } = new();
    public List<ProductVariantDto> Variants { get; set; } = new();
    public string ErrorMessage { get; set; } = string.Empty;

    public CreateModel(IOrderDetailService orderDetailService, IOrderService orderService, IProductVariantService variantService)
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

    public async Task<IActionResult> OnGetAsync()
    {
        await LoadCatalogAsync();
        OrderDetailDto.Quantity = 1;
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        await LoadCatalogAsync();
        if (!ModelState.IsValid) return Page();

        if (OrderDetailDto.OrderId <= 0)
        { ErrorMessage = "Selecciona una orden válida."; return Page(); }

        if (OrderDetailDto.ProductVariantId <= 0)
        { ErrorMessage = "Selecciona una presentación de producto válida."; return Page(); }

        if (OrderDetailDto.Quantity <= 0)
        { ErrorMessage = "La cantidad debe ser mayor a 0."; return Page(); }

        if (OrderDetailDto.UnitPrice <= 0)
        { ErrorMessage = "El precio unitario debe ser mayor a 0."; return Page(); }

        var response = await _orderDetailService.CreateAsync(OrderDetailDto);
        if (response.Success)
            return RedirectToPage("/OrderDetail/List");

        ErrorMessage = response.Message ?? "Error al registrar el concepto.";
        return Page();
    }
}
