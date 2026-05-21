using Coffee.Core.Dto;
using Coffee.WebSite.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Coffee.WebSite.Pages.OrderDetail;

public class CreateModel : PageModel
{
    private readonly IOrderDetailService _orderDetailService;
    private readonly IOrderService _orderService;                 // Inyectamos servicio de órdenes
    private readonly IProductVariantService _variantService;
    
    [BindProperty]
    public OrderDetailDto OrderDetailDto { get; set; } = new();
    public string ErrorMessage { get; set; } = string.Empty;

    public CreateModel(IOrderDetailService orderDetailService, IOrderService orderService, IProductVariantService variantService)
    {
        _orderDetailService = orderDetailService;
        _orderService = orderService;
        _variantService = variantService;
    }
    public void OnGet() { }

    public async Task<IActionResult> OnPostAsync()
    {   
        if (!ModelState.IsValid) return Page();
        // 1. Validar si la Orden existe
        var orderCheck = await _orderService.GetByIdAsync(OrderDetailDto.OrderId);
        if (orderCheck == null || orderCheck.Data == null)
        {
            ErrorMessage = $"⚠️ Error de flujo: La Orden con ID #{OrderDetailDto.OrderId} no existe. No se puede añadir un detalle a una orden inexistente.";
            return Page();
        }

        // 2. Validar si la Presentación/Variante de producto existe
        var variantCheck = await _variantService.GetByIdAsync(OrderDetailDto.ProductVariantId);
        if (variantCheck == null || variantCheck.Data == null)
        {
            ErrorMessage = $"⚠️ El ID de Presentación ({OrderDetailDto.ProductVariantId}) no corresponde a ningún producto del menú.";
            return Page();
        }

        // 3. Validar consistencia lógica de la cantidad
        if (OrderDetailDto.Quantity <= 0)
        {
            ErrorMessage = "⚠️ La cantidad del producto debe ser mayor a 0.";
            return Page();
        }

        // Si pasó los filtros de seguridad, mandamos el registro a la API
        var response = await _orderDetailService.CreateAsync(OrderDetailDto);
        if (response.Success)
        {
             return RedirectToPage("/OrderDetail/List");
        }

        ErrorMessage = response.Message ?? "Error al intentar registrar el concepto.";
        return Page();
    }
}
