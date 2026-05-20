using Coffee.Core.Dto;
using Coffee.WebSite.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Coffee.WebSite.Pages.Category;

public class EditModel : PageModel
{
    private readonly ICategoryService _service;

    [BindProperty]
    public CategoryDto Category { get; set; } = new();
    public string ErrorMessage { get; set; } = string.Empty;

    public EditModel(ICategoryService service)
    {
        _service = service;
    }

    public async Task<IActionResult> OnGet(int? id)
    {
        if (id.HasValue && id.Value > 0)
        {
            var category = await _service.GetByIdAsync(id.Value);
            if (category == null) return NotFound();
            Category = category;
        }
        return Page();
    }

    public async Task<IActionResult> OnPost()
    {
        if (!ModelState.IsValid) return Page();

        if (Category.Id == 0)
        {
            var result = await _service.CreateAsync(Category);
            if (result == null)
            {
                ErrorMessage = "Error creating the category. Please try again.";
                return Page();
            }
        }
        else
        {
            var result = await _service.UpdateAsync(Category);
            if (result == null)
            {
                ErrorMessage = "Error updating the category. Please try again.";
                return Page();
            }
        }

        return RedirectToPage("./List");
    }
}