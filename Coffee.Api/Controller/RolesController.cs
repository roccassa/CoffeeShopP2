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

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            if (id <= 0) return BadRequest("ID de rol inválido.");
        
            var role = await _roleRepository.GetByIdAsync(id);
            if (role == null) return NotFound($"No se encontró el rol con ID {id}");
        
            return Ok(role);
        }
        
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Role role)
        {
            if (string.IsNullOrEmpty(role.Name)) return BadRequest("El nombre del rol es obligatorio");
            var result = await _roleRepository.SaveAsync(role);
            return result ? Ok("Rol creado") : BadRequest("Error al crear el rol");
        }
        
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] Role role)
        {
            if (role.Id <= 0) return BadRequest("Se requiere un ID de rol válido para actualizar.");
            if (string.IsNullOrWhiteSpace(role.Name)) return BadRequest("El nombre no puede estar vacío.");

            var result = await _roleRepository.UpdateAsync(role);
            return result ? Ok("Rol actualizado correctamente.") : BadRequest("No se pudo actualizar el rol o el ID no existe.");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0) return BadRequest("ID de rol inválido.");

            try 
            {
                var result = await _roleRepository.DeleteAsync(id);
                return result ? Ok("Rol eliminado.") : NotFound("El rol no existe.");
            }
            catch (Exception)
            {
                // Validación de integridad: Evita que la app truene si el rol está en uso por un usuario
                return BadRequest("No se puede eliminar el rol porque tiene usuarios asociados.");
            }
        }
        
    }
