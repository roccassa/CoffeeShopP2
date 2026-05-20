using Coffee.Core.Dto;
using Coffee.WebSite.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;


namespace Coffee.WebSite.Pages.User;

[IgnoreAntiforgeryToken(Order = 1001)]
public class ListModel : PageModel
{
    private readonly IUserService _service;
    public List<UserDto> Users { get; set; } = new();

    [BindProperty(SupportsGet = true)]
    public string SearchTerm { get; set; } = string.Empty;

    public ListModel(IUserService service)
    {
        _service = service;
    } 

    public async Task<IActionResult> OnGetAsync(string searchTerm = "")
    {
        SearchTerm = searchTerm; // Esto ayuda al binding

        var response = await _service.GetAllAsync();
        var allUsers = response?.Data ?? new List<UserDto>();

        if (!string.IsNullOrWhiteSpace(SearchTerm))
        {
            allUsers = allUsers.Where(u =>
                (u.Username?.Contains(SearchTerm, StringComparison.OrdinalIgnoreCase) == true) ||
                (u.FullName?.Contains(SearchTerm, StringComparison.OrdinalIgnoreCase) == true) ||
                (u.Id.ToString() == SearchTerm)
            ).ToList();
        }

        Users = allUsers;
        return Page();
    }
    
    

    public async Task<JsonResult> OnPostDeleteAsync(int id)
    {
        var response = await _service.DeleteAsync(id);
        return new JsonResult(response.Data);
    }
}
