using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RoleBase_Api.Models;
using RoleBase_Api.Models.DTOs;
using RoleBase_Api.Repository;
using RoleBase_Api.Validations;

namespace RoleBase_Api.Controllers
{
    [Route("api/Register")]
    [ApiController]
    public class RegisterController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public RegisterController(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult GetUsers()
        {
            var users = _userRepository.GetUsers()
                     .Where(u => !string.Equals( u.Role.Name,"Admin",StringComparison.OrdinalIgnoreCase))
                     .ToList();
            var userDTOs = _mapper.Map<List<UserDTO>>(users);
            return Ok(userDTOs);
        }

        [HttpGet("{userId:int}")]
        public IActionResult GetUser(int userId)
        {
            var user = _userRepository.GetUser(userId);
            if (user == null) return NotFound();
            return Ok(_mapper.Map<UserDTO>(user));
        }

        [HttpPost]
        public IActionResult CreateUser([FromBody] RegisterDTO registerDTO)
        {
            if (registerDTO == null) return BadRequest(ModelState);
            if (_userRepository.UserExists( registerDTO.Email, registerDTO.MobileNumber, registerDTO.IdProofNumber))
            {
                ModelState.AddModelError("", "User already exists");
                return StatusCode(422, ModelState);
            }
            var validationMassage=UserValidation.ValidationUser(registerDTO);
            var user = _mapper.Map<Models.User>(registerDTO);
            var users = _userRepository.AddUser(user);
            if (!users) return NotFound("User not created");
            return Ok("User created successfully");
        }

        [HttpPut]
        public IActionResult UpdateUser([FromBody] RegisterDTO registerDTO)
        {
            if (registerDTO == null) return BadRequest(ModelState);
            var validationMassage = UserValidation.ValidationUser(registerDTO);
            var user = _mapper.Map<User>(registerDTO);
            var users = _userRepository.UpdateUser(user);
            if (!users) return NotFound("User not updated");
            return Ok("User updated successfully");
        }

        [HttpDelete("{UserId:int}")]
        public IActionResult UserDelete(int UserId)
        {
            if(!_userRepository.UserExists(UserId))
            {
                return NotFound();
            }
            var user = _userRepository.GetUser(UserId);
            if (user == null) return NotFound();
            var users = _userRepository.DeleteUser(user);
            if (!users) return NotFound("User not deleted");
            return Ok("User deleted successfully");
        }
    }
}
