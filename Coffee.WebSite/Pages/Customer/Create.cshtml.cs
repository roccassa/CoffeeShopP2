using Coffee.Core.Dto;
using Coffee.WebSite.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Coffee.WebSite.Pages.Customer;

public class CreateModel : PageModel
{
    private readonly ICustomerService _service;

    [BindProperty]
    public CustomerDto CustomerDto { get; set; } = new();
    public string ErrorMessage { get; set; } = string.Empty;

    public CreateModel(ICustomerService service) => _service = service;

    public void OnGet() { }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid) return Page();
        var response = await _service.CreateAsync(CustomerDto);
        if (response?.Data == null)
        {
            ErrorMessage = "Error creating the customer. Please try again.";
            return Page();
        }
        return RedirectToPage("./List");
    }
}