using Coffee.Core.Dto;
using Coffee.WebSite.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Coffee.WebSite.Pages.Category;

public class EditModel : PageModel
{
    private readonly ICategoryServices _service;

    [BindProperty]
    public CategoryDto Category { get; set; } = new();
    public string ErrorMessage { get; set; } = string.Empty;

    public EditModel(ICategoryServices service)
    {
        _service = service;
    }

    public async Task<IActionResult> OnGet(int? id)
    {
        if (id.HasValue && id.Value > 0)
        {
            var response = await _service.GetByIdAsync(id.Value);
            if (response?.Data == null) return NotFound();
            Category = response.Data;
        }
        return Page();
    }

    public async Task<IActionResult> OnPost()
    {
        if (!ModelState.IsValid) return Page();

        if (Category.Id == 0)
        {
            var response = await _service.SaveAsync(Category);
            if (response?.Data == null)
            {
                ErrorMessage = "Error creating the category. Please try again.";
                return Page();
            }
        }
        else
        {
            var response = await _service.UpdateAsync(Category);
            if (response?.Data == null)
            {
                ErrorMessage = "Error updating the category. Please try again.";
                return Page();
            }
        }

        return RedirectToPage("./List");
    }
}