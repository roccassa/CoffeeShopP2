using Coffee.Core.Dto;
using Coffee.WebSite.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace Coffee.WebSite.Pages.POS;

[IgnoreAntiforgeryToken(Order = 1001)]
public class IndexModel : PageModel
{
    private readonly ICategoryService _categoryService;
    private readonly IProductService _productService;
    private readonly IProductVariantService _variantService;
    private readonly IOrderService _orderService;
    private readonly IOrderDetailService _orderDetailService;
    private readonly IPaymentMethodService _paymentMethodService;
    private readonly ICustomerService _customerService;
    private readonly IUserService _userService;

    public List<CategoryDto> Categories { get; set; } = new();
    public List<ProductDto> Products { get; set; } = new();
    public List<ProductVariantDto> Variants { get; set; } = new();
    public List<PaymentMethodDto> PaymentMethods { get; set; } = new();
    public List<CustomerDto> Customers { get; set; } = new();
    public List<UserDto> Users { get; set; } = new();
    public string CurrentUsername { get; set; } = string.Empty;
    public int CurrentUserId { get; set; } = 1;

    public IndexModel(
        ICategoryService categoryService,
        IProductService productService,
        IProductVariantService variantService,
        IOrderService orderService,
        IOrderDetailService orderDetailService,
        IPaymentMethodService paymentMethodService,
        ICustomerService customerService,
        IUserService userService)
    {
        _categoryService = categoryService;
        _productService = productService;
        _variantService = variantService;
        _orderService = orderService;
        _orderDetailService = orderDetailService;
        _paymentMethodService = paymentMethodService;
        _customerService = customerService;
        _userService = userService;
    }

    public async Task<IActionResult> OnGetAsync()
    {
        if (HttpContext.Session.GetString("IsLoggedIn") != "true")
            return RedirectToPage("/Account/Login");

        CurrentUsername = HttpContext.Session.GetString("Username") ?? "Empleado";
        var userIdStr = HttpContext.Session.GetString("UserId");
        CurrentUserId = int.TryParse(userIdStr, out var uid) ? uid : 1;

        var catTask      = _categoryService.GetAllAsync();
        var prodTask     = _productService.GetAllAsync();
        var varTask      = _variantService.GetAllAsync();
        var payTask      = _paymentMethodService.GetAllAsync();
        var customerTask = _customerService.GetAllAsync();
        var userTask     = _userService.GetAllAsync();

        await Task.WhenAll(catTask, prodTask, varTask, payTask, customerTask, userTask);

        Categories     = (await catTask) ?? new();
        Products       = ((await prodTask).Data ?? new()).Where(p => p.IsActive).ToList();
        Variants       = (await varTask).Data ?? new();
        PaymentMethods = (await payTask).Data ?? new();
        Customers      = (await customerTask).Data ?? new();
        Users          = (await userTask).Data ?? new();

        return Page();
    }

    public async Task<JsonResult> OnPostCheckoutAsync()
    {
        using var reader = new StreamReader(Request.Body);
        var body = await reader.ReadToEndAsync();
        var req  = JsonConvert.DeserializeObject<CheckoutRequest>(body);

        if (req == null || req.Items.Count == 0)
            return new JsonResult(new { success = false, message = "El ticket está vacío." });

        try
        {
            var sessionUserIdStr = HttpContext.Session.GetString("UserId");
            var sessionUserId = int.TryParse(sessionUserIdStr, out var uid) ? uid : 1;
            var userId = req.UserId > 0 ? req.UserId : sessionUserId;

            var orderDto = new OrderDto
            {
                UserId          = userId,
                CustomerId      = req.CustomerId > 0 ? req.CustomerId : null,
                PaymentMethodId = req.PaymentMethodId,
                Total           = req.Total,
                Status          = "Entregado"
            };

            var orderResponse = await _orderService.CreateAsync(orderDto);
            if (orderResponse?.Data == null)
                return new JsonResult(new { success = false, message = "Error al guardar la orden." });

            var orderId = orderResponse.Data.Id;

            foreach (var item in req.Items)
            {
                await _orderDetailService.CreateAsync(new OrderDetailDto
                {
                    OrderId          = orderId,
                    ProductVariantId = item.VariantId,
                    Quantity         = item.Quantity,
                    UnitPrice        = item.UnitPrice
                });
            }

            return new JsonResult(new { success = true, orderId });
        }
        catch (Exception ex)
        {
            return new JsonResult(new { success = false, message = ex.Message });
        }
    }
}

public class CheckoutRequest
{
    public int PaymentMethodId { get; set; }
    public int CustomerId { get; set; }
    public int UserId { get; set; }
    public decimal Total { get; set; }
    public List<CheckoutItem> Items { get; set; } = new();
}

public class CheckoutItem
{
    public int VariantId { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public string Size { get; set; } = string.Empty;
}
