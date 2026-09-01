using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RoleBase_Api.Data;
using RoleBase_Api.Models;
using RoleBase_Api.Models.DTOs;
using RoleBase_Api.Repository;

namespace RoleBase_Api.Controllers
{
    [Route("api/Role")]
    [ApiController]
    public class RoleController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IRoleRepository _roleRepository;
        private readonly IMapper _mapper;
        public RoleController(ApplicationDbContext context, IRoleRepository roleRepository, IMapper mapper)
        {
            _context = context;
            _roleRepository = roleRepository;
            _mapper = mapper;
        }

        //[Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult GetRoles()
        {
            var roles = _roleRepository.GetRoles().Where(r=>r.Name !="Admin").ToList();
            var rolesDTO = _mapper.Map<List<RoleDTO>>(roles);
            return Ok(rolesDTO);
        }

        /// <summary>
        /// Diagnostic endpoint to check database connection
        /// </summary>
        [HttpGet("diagnostic")]
        public IActionResult GetDiagnostic()
        {
            try
            {
                var allRoles = _roleRepository.GetRoles();
                return Ok(new 
                { 
                    status = "Connected",
                    totalRoles = allRoles.Count,
                    roles = allRoles.Select(r => new { r.Id, r.Name }).ToList()
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { status = "Error", message = ex.Message });
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("{roleId:int}")]
        public IActionResult GetRole(int roleId)
        {
            var role = _roleRepository.GetRole(roleId);
            if (role == null) return NotFound();
            return Ok(_mapper.Map<RoleDTO>(role));
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult CreateRole([FromBody] RoleDTO roleDTO)
        {
            if (roleDTO == null) return BadRequest(ModelState);
            if (_roleRepository.RoleExists(roleDTO.Name))
            {
                ModelState.AddModelError("", "Role already exists");

                return StatusCode(422, ModelState);
            }
            var roleData = _mapper.Map<Role>(roleDTO);
            var roles = _roleRepository.AddRole(roleData);
            if (!roles) return NotFound("Role not created");
            return Ok("Role  created successfully");
        }

        [Authorize(Roles = "Admin")]
        [HttpPut]
        public IActionResult UpdateRole([FromBody] RoleDTO roleDTO)
        {
            if (roleDTO == null) return BadRequest(ModelState);
            var roleData = _mapper.Map<Role>(roleDTO);
            var roles = _roleRepository.UpdateRole(roleData);
            if (!roles) return NotFound("Role not updated");
            return Ok("Role  updated successfully");
        }


        [Authorize(Roles = "Admin")]
        [HttpDelete("{roleId:int}")]
        public IActionResult RoleDelete(int roleId)
        {
            if (!_roleRepository.RoleExists(roleId))
            {
                return NotFound();
            }
            var role = _roleRepository.GetRole(roleId);
            if (role == null) return NotFound();
            var roles = _roleRepository.DeleteRole(role);
            if (!roles) return NotFound("Role not deleted");
            return Ok("Role  deleted successfully");
        }
    }
}
