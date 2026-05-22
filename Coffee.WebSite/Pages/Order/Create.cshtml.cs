using Coffee.Core.Dto;
using Coffee.WebSite.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Coffee.WebSite.Pages.Order;

public class CreateModel : PageModel
{
    private readonly IOrderService _orderService;
    private readonly IProductService _productService;
    private readonly IProductVariantService _variantService;
    private readonly IUserService _userService;
    private readonly IPaymentMethodService _paymentService;

    [BindProperty]
    public OrderDto OrderDto { get; set; } = new();

    public List<ProductDto> ProductsList { get; set; } = new();
    public List<ProductVariantDto> VariantsList { get; set; } = new();
    public List<UserDto> UsersList { get; set; } = new();
    public List<PaymentMethodDto> PaymentsList { get; set; } = new();

    [BindProperty]
    public string CartJson { get; set; } = "[]";

    public string ErrorMessage { get; set; } = string.Empty;

    public CreateModel(
        IOrderService orderService, 
        IProductService productService, 
        IProductVariantService variantService,
        IUserService userService,
        IPaymentMethodService paymentService)
    {
<<<<<<< Updated upstream
        _orderService = orderService;
        _productService = productService;
        _variantService = variantService;
        _userService = userService;
        _paymentService = paymentService;
    }

    public async Task<IActionResult> OnGetAsync()
    {
        await LoadCatalogosAsync();
        // Ajustado a tu propiedad real en inglés
        OrderDto.Status = "Pendiente"; 
        return Page();
    }

    private async Task LoadCatalogosAsync()
    {
        // 1. Consumir las respuestas de la API
        var pRes = await _productService.GetAllAsync();
        ProductsList = pRes.Data ?? new();

        var vRes = await _variantService.GetAllAsync();
        var variantesOriginales = vRes.Data ?? new();

        var uRes = await _userService.GetAllAsync();
        UsersList = uRes.Data ?? new();

        var payRes = await _paymentService.GetAllAsync();
        PaymentsList = payRes.Data ?? new();

        // 2. CORRECCIÓN CLAVE: Inyectar el nombre del producto directamente en la variante
        // Si tu ProductVariantDto no tiene un campo para el nombre, usaremos un truco en el bucle
        VariantsList = variantesOriginales;
=======
  
        OrderDto.Status = "Pendiente";
>>>>>>> Stashed changes
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            await LoadCatalogosAsync();
            return Page();
        }

        try
        {
            // Enviamos la orden directo a la API (la fecha la controlará tu repositorio o API interna)
            var responseOrder = await _orderService.CreateAsync(OrderDto);

            if (responseOrder == null || !responseOrder.Success)
            {
                ErrorMessage = responseOrder?.Message ?? "Error al registrar la orden base.";
                await LoadCatalogosAsync();
                return Page();
            }

            return RedirectToPage("./List");
        }
        catch (Exception ex)
        {
            ErrorMessage = $"⚠️ Excepción en el flujo POS: {ex.Message}";
            await LoadCatalogosAsync();
            return Page();
        }
    }
}