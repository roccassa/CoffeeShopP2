using Coffee.Core.Dto;
using Coffee.WebSite.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Coffee.WebSite.Pages.Category;

public class DeleteModel : PageModel
{
    private readonly ICategoryServices _service;

    [BindProperty]
    public CategoryDto Category { get; set; } = new();
    public string ErrorMessage { get; set; } = string.Empty;

    public DeleteModel(ICategoryServices service)
    {
        _service = service;
    }

    public async Task<IActionResult> OnGet(int id)
    {
        var response = await _service.GetByIdAsync(id);
        if (response?.Data == null) return NotFound();
        Category = response.Data;
        return Page();
    }

    public async Task<IActionResult> OnPost()
    {
        var response = await _service.DeleteAsync(Category.Id);
        if (response == null || !response.Data)
        {
            ErrorMessage = "Error deleting the category. Please try again.";
            return Page();
        }
        return RedirectToPage("./List");
    }
}