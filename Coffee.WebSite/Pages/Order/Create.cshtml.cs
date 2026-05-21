using Coffee.Core.Dto;
using Coffee.WebSite.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Coffee.WebSite.Pages.Order;

public class CreateModel : PageModel
{
    private readonly IOrderService _orderService;
    private readonly IUserService _userService;         // Inyectamos servicio de usuarios
    private readonly ICustomerService _customerService; // Inyect
    private readonly IPaymentMethodService _paymentMethodService;
    
    [BindProperty]
    public OrderDto OrderDto { get; set; } = new();

    public string ErrorMessage { get; set; } = string.Empty;

    public CreateModel(IOrderService orderService, IUserService userService, ICustomerService customerService, IPaymentMethodService paymentMethodService)
    {
        _orderService = orderService;
        _userService = userService;
        _customerService = customerService;
        _paymentMethodService = paymentMethodService;
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid) return Page();

        // 1. Validar si el Usuario (Cajero) existe
        var userCheck = await _userService.GetByIdAsync(OrderDto.UserId);
        if (userCheck == null || userCheck.Data == null)
        {
            ErrorMessage = $"⚠️ El ID de Usuario/Cajero ({OrderDto.UserId}) no existe en el sistema.";
            return Page();
        }

        // 2. Validar si el Cliente existe (sólo si ingresaron uno, ya que es opcional)
        if (OrderDto.CustomerId.HasValue && OrderDto.CustomerId.Value > 0)
        {
            var customerCheck = await _customerService.GetByIdAsync(OrderDto.CustomerId.Value);
            if (customerCheck == null || customerCheck.Data == null)
            {
                ErrorMessage = $"⚠️ El ID de Cliente ({OrderDto.CustomerId}) no está registrado.";
                return Page();
            }
        }
        
        var paymentCheck = await _paymentMethodService.GetByIdAsync(OrderDto.PaymentMethodId);
        if (paymentCheck == null || paymentCheck.Data == null)
        {
            ErrorMessage = $"⚠️ El ID del método de pago ({OrderDto.PaymentMethodId}) no existe en el sistema.";
            return Page();
        }

        // Si todo está bien, intentamos crear la orden
        var response = await _orderService.CreateAsync(OrderDto);
        if (response.Success)
        {
            return RedirectToPage("/Order/List");
        }
        
       // var response = await _orderService.CreateAsync(OrderDto);

        if (response == null)
        {
            ErrorMessage = "Response viene null";
            return Page();
        }
        Console.WriteLine(response.Success);
        Console.WriteLine(response.Message);

        ErrorMessage = response.Message;
        return Page();
        
        ErrorMessage = response.Message ?? "Error inesperado al guardar la orden.";
        return Page();
    }
}