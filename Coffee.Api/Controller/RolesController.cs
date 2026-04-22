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
            if (id <= 0) return BadRequest("Invalid role ID.");
        
            var role = await _roleRepository.GetByIdAsync(id);
            if (role == null) return NotFound($"The role with ID was not found. {id}");
        
            return Ok(role);
        }
        
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Role role)
        {
            if (string.IsNullOrEmpty(role.Name)) return BadRequest("The role name is required");
            var result = await _roleRepository.SaveAsync(role);
            return result ? Ok("Rol created") : BadRequest("Error creating role");
        }
        
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] Role role)
        {
            if (role.Id <= 0) return BadRequest("A valid role ID is required to update.");
            if (string.IsNullOrWhiteSpace(role.Name)) return BadRequest("The name cannot be empty..");

            var result = await _roleRepository.UpdateAsync(role);
            return result ? Ok("Role updated successfully.") : BadRequest("The role could not be updated or the ID does not exist..");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0) return BadRequest("Invalid rol ID");

            try 
            {
                var result = await _roleRepository.DeleteAsync(id);
                return result ? Ok("Rol deleted.") : NotFound("Rol doesn't exist.");
            }
            catch (Exception)
            {
                // Validación de integridad: Evita que la app truene si el rol está en uso por un usuario
                return BadRequest("The role cannot be deleted because it has associated users..");
            }
        }
        
    }
