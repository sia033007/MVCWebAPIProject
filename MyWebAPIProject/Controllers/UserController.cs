using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyWebAPIProject.Data;
using MyWebAPIProject.Model;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace MyWebAPIProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly IMapper _mapper;

        public UserController(IUserRepository userRepository, IMapper mapper, ApplicationDbContext applicationDbContext)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _applicationDbContext = applicationDbContext;

        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetAllUsers()
        {
            var users = await _applicationDbContext.Users.ToListAsync();
            return Ok(users);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUserById(int id)
        {
            var userToRetrieve = await _applicationDbContext.Users.FindAsync(id);
            if (userToRetrieve == null)
            {
                return NotFound("Couldn't find such a user");
            }
            return Ok(userToRetrieve);
        }
        [HttpPost("register")]
        public async Task<ActionResult<User>> RegisterUser([FromBody] UserDto user)
        {
            if (await _userRepository.IsUniqueUser(user.UserName))
            {
                return StatusCode(StatusCodes.Status400BadRequest);
            }
            if (user == null)
            {
                ModelState.AddModelError("Custom", "User can not be empty");
                return BadRequest(ModelState);
            }
            if (ModelState.IsValid)
            {
                var userToSave = _mapper.Map<User>(user);
                _userRepository.CreatePasswordHahs(user.Password, out byte[] passwordHash, out byte[] passwordSalt);
                userToSave.PasswordHash = passwordHash;
                userToSave.PasswordSalt = passwordSalt;
                await _userRepository.Register(userToSave);
            }
            return Ok(user);
        }
        [HttpPost("login")]
        public async Task<IActionResult> LoginUser([FromBody] UserDtoLogin user)
        {
            var userToRetrieve = await _applicationDbContext.Users.FirstOrDefaultAsync(u => u.UserName == user.UserName);
            if (userToRetrieve == null)
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }
            if (!_userRepository.VerifyPasswordHash(user.Password, userToRetrieve.PasswordHash, userToRetrieve.PasswordSalt))
            {
                return StatusCode(StatusCodes.Status400BadRequest);
            }
            await _userRepository.Login(userToRetrieve);
            return Ok(user);
        }
    }
}
