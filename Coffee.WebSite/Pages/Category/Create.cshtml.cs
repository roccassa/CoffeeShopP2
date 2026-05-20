using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Coffee.Core.Dto;
using Coffee.WebSite.Services.Interfaces;

namespace Coffee.WebSite.Pages.Category;

public class CreateModel : PageModel
{
    private readonly ICategoryService _service;

    [BindProperty]
    public CategoryDto CategoryDto { get; set; } = new();

    public CreateModel(ICategoryService service)
    {
        _service = service;
    }

    public void OnGet() { }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
            return Page();

        await _service.CreateAsync(CategoryDto);
        return RedirectToPage("./List");
    }
}