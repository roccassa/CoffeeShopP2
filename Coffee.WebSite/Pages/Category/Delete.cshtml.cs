using Coffee.Core.Dto;
using Coffee.WebSite.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Coffee.WebSite.Pages.Category;

public class DeleteModel : PageModel
{
    private readonly ICategoryService _service;

    [BindProperty]
    public CategoryDto Category { get; set; } = new();
    public string ErrorMessage { get; set; } = string.Empty;

    public DeleteModel(ICategoryService service)
    {
        _service = service;
    }

    public async Task<IActionResult> OnGet(int id)
    {
        var category = await _service.GetByIdAsync(id);
        if (category == null) return NotFound();
        Category = category;
        return Page();
    }

    public async Task<IActionResult> OnPost()
    {
        var result = await _service.DeleteAsync(Category.Id);
        if (!result)
        {
            ErrorMessage = "Error deleting the category. Please try again.";
            return Page();
        }
        return RedirectToPage("./List");
    }
}