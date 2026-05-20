using Coffee.Core.Dto;
using Coffee.WebSite.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Coffee.WebSite.Pages.Customer;

public class UpdateModel : PageModel
{
    private readonly ICustomerService _service;

    [BindProperty]
    public CustomerDto CustomerDto { get; set; } = new();
    public string ErrorMessage { get; set; } = string.Empty;

    public UpdateModel(ICustomerService service) => _service = service;

    public async Task<IActionResult> OnGetAsync(int id)
    {
        var response = await _service.GetByIdAsync(id);
        if (response?.Data == null) return RedirectToPage("./List");
        CustomerDto = response.Data;
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid) return Page();
        var response = await _service.UpdateAsync(CustomerDto);
        if (response?.Data == null)
        {
            ErrorMessage = "Error updating the customer. Please try again.";
            return Page();
        }
        return RedirectToPage("./List");
    }
}