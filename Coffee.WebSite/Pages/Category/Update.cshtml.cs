using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Coffee.Core.Dto;
using Coffee.WebSite.Services.Interfaces;

namespace Coffee.WebSite.Pages.Category;

public class UpdateModel : PageModel
{
    private readonly ICategoryService _service;

    [BindProperty]
    public CategoryDto CategoryDto { get; set; } = new();

    public UpdateModel(ICategoryService service)
    {
        _service = service;
    }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        CategoryDto = await _service.GetByIdAsync(id);
        if (CategoryDto == null)
            return RedirectToPage("./List");

        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
            return Page();

        await _service.UpdateAsync(CategoryDto);
        return RedirectToPage("./List");
    }
}