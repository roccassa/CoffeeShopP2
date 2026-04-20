using Microsoft.AspNetCore.Mvc;
using Coffee.Api.Repositories.Interfaces;
using Coffee.Core.Entities;

namespace Coffee.Api.Controller;


    [ApiController]
    [Route("api/[controller]")]
    public class RolesController : ControllerBase
    {
        private readonly IRoleRepository _roleRepository;

        public RolesController(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _roleRepository.GetAllAsync());

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Role role)
        {
            if (string.IsNullOrEmpty(role.Name)) return BadRequest("El nombre del rol es obligatorio");
            var result = await _roleRepository.SaveAsync(role);
            return result ? Ok("Rol creado") : BadRequest("Error al crear el rol");
        }
        
    }
