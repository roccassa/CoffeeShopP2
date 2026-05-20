using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Coffee.Core.Dto;
using Coffee.WebSite.Services.Interfaces;

namespace Coffee.WebSite.Pages.Customer;

public class CreateModel : PageModel
{
    private readonly ICustomerService _service;

    [BindProperty]
    public CustomerDto CustomerDto { get; set; } = new();

    public CreateModel(ICustomerService service) => _service = service;

    public void OnGet() { }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid) return Page();
        await _service.CreateAsync(CustomerDto);
        return RedirectToPage("./List");
    }
}