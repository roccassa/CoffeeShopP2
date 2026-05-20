using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Coffee.Core.Dto;
using Coffee.WebSite.Services.Interfaces;

namespace Coffee.WebSite.Pages.Customer;

public class UpdateModel : PageModel
{
    private readonly ICustomerService _service;

    [BindProperty]
    public CustomerDto CustomerDto { get; set; } = new();

    public UpdateModel(ICustomerService service) => _service = service;

    public async Task<IActionResult> OnGetAsync(int id)
    {
        CustomerDto = await _service.GetByIdAsync(id);
        if (CustomerDto == null) return RedirectToPage("./List");
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid) return Page();
        await _service.UpdateAsync(CustomerDto);
        return RedirectToPage("./List");
    }
}