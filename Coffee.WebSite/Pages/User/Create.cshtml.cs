using Coffee.Core.Dto;
using Coffee.WebSite.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Coffee.WebSite.Pages.User;

public class CreateModel : PageModel
{
    private readonly IUserService _userService;
    private readonly IRoleService _roleService; // 1. Inyectamos el servicio de roles

    [BindProperty]
    public UserDto UserDto { get; set; } = new();
    // 2. Propiedad para exponer la lista real de roles a la vista
    public List<RoleDto> RolesList { get; set; } = new();
    public string ErrorMessage { get; set; } = string.Empty;

    public CreateModel(IUserService userService, IRoleService roleService)
    {
        _userService = userService;
        _roleService = roleService;
    }
    
    //Método auxiliar para no duplicar código de carga
    private async Task LoadRolesAsync()
    {
        var response = await _roleService.GetAllAsync();
        RolesList = response.Data ?? new List<RoleDto>();
    }
    public async Task<IActionResult> OnGetAsync()
    {
        await LoadRolesAsync(); // Cargamos los roles al abrir la página
        return Page();
    }
    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            await LoadRolesAsync(); // Recargamos si el modelo es inválido
            return Page();
        }

        // Validación extra: Que realmente hayan seleccionado un rol válido
        if (UserDto.RoleId <= 0)
        {
            ErrorMessage = "⚠️ Por favor, selecciona un rol válido para el colaborador.";
            await LoadRolesAsync();
            return Page();
        }

        var response = await _userService.CreateAsync(UserDto);
        if (response.Success)
        {
            return RedirectToPage("./List");
        }

        ErrorMessage = response.Message ?? "Error al intentar guardar el usuario.";
        await LoadRolesAsync(); // Recargamos si la API de usuarios falló
        return Page();
    }
}
