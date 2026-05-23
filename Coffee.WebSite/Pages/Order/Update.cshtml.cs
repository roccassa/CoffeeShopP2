using Coffee.Core.Dto;
using Coffee.WebSite.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Coffee.WebSite.Pages.Order;

public class UpdateModel : PageModel
{
    private readonly IOrderService          _service;
    private readonly IUserService           _userService;
    private readonly ICustomerService       _customerService;
    private readonly IPaymentMethodService  _paymentMethodService;

    [BindProperty]
    public OrderDto OrderDto { get; set; } = new();

    public string ErrorMessage { get; set; } = string.Empty;

    public List<UserDto>          Users          { get; set; } = new();
    public List<CustomerDto>      Customers      { get; set; } = new();
    public List<PaymentMethodDto> PaymentMethods { get; set; } = new();

    public UpdateModel(
        IOrderService         service,
        IUserService          userService,
        ICustomerService      customerService,
        IPaymentMethodService paymentMethodService)
    {
        _service              = service;
        _userService          = userService;
        _customerService      = customerService;
        _paymentMethodService = paymentMethodService;
    }

    private async Task LoadListsAsync()
    {
        var users    = await _userService.GetAllAsync();
        var customers = await _customerService.GetAllAsync();
        var payments  = await _paymentMethodService.GetAllAsync();

        Users          = users?.Data          ?? new();
        Customers      = customers?.Data      ?? new();
        PaymentMethods = payments?.Data       ?? new();
    }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        var response = await _service.GetByIdAsync(id);
        if (response?.Data == null) return RedirectToPage("./List");
        OrderDto = response.Data;
        await LoadListsAsync();
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            await LoadListsAsync();
            return Page();
        }

        var response = await _service.UpdateAsync(OrderDto);
        if (response?.Data == null)
        {
            ErrorMessage = "Error al actualizar la orden. Intente de nuevo.";
            await LoadListsAsync();
            return Page();
        }

        return RedirectToPage("./List");
    }
}
