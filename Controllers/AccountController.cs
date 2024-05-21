//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.IdentityModel.Tokens;
//using QuadrifoglioAPI.DTOs;
//using QuadrifoglioAPI.Models;
//using System.IdentityModel.Tokens.Jwt;
//using System.Security.Claims;
//using System.Text;

//namespace QuadrifoglioAPI.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class AccountController : ControllerBase
//    {
//        private readonly UserManager<ApplicationUser> _userManager;
//        private readonly SignInManager<ApplicationUser> _signInManager;
//        private readonly IConfiguration _configuration;

//        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IConfiguration configuration)
//        {
//            _userManager = userManager;
//            _signInManager = signInManager;
//            _configuration = configuration;
//        }

//        [HttpPost("login")]
//        public async Task<IActionResult> Login([FromBody] LoginModel model)
//        {
//            var result = await _signInManager.PasswordSignInAsync(model.Username, model.Password, model.RememberMe,/* lockoutOnFailure:*/ false);
//            if (result.Succeeded)
//            {
//                var user = await _userManager.FindByNameAsync(model.Username);
//                var token = GenerateJwtToken(user);
//                return Ok(new { token });
//            }
//            return Unauthorized();
//        }

//        [HttpPost("register")]
//        public async Task<IActionResult> Register([FromBody] RegisterModel model)
//        {
//            var user = new ApplicationUser { UserName = model.Username, Email = model.Email, Address = "" };
//            var result = await _userManager.CreateAsync(user, model.Password);
//            if (result.Succeeded)
//            {
//                // Optionally, sign the user in
//                await _signInManager.SignInAsync(user, isPersistent: false);
//                return Ok();
//            }
//            return BadRequest(result.Errors);
//        }

//        private string GenerateJwtToken(ApplicationUser user)
//        {   
//            var claims = new[]
//            {
//            new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
//            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
//        };

//            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
//            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

//            var token = new JwtSecurityToken(
//                issuer: _configuration["Jwt:Issuer"],
//                audience: _configuration["Jwt:Audience"],
//                claims: claims,
//                expires: DateTime.Now.AddMinutes(30),
//                signingCredentials: creds);

//            return new JwtSecurityTokenHandler().WriteToken(token);
//        }
//    }
//}
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using QuadrifoglioAPI.DTOs;
using QuadrifoglioAPI.Models;
using System.Threading.Tasks;

namespace QuadrifoglioAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Username, model.Password, model.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    return Ok(new { Message = "Login successful." });
                }
                return Unauthorized(new { Message = "Invalid login attempt." });
            }
            return BadRequest(ModelState);
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Ok(new { Message = "Logout successful." });
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = model.Username, Email = model.Email };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    // Assign the default role (customer) to the user
                    await _userManager.AddToRoleAsync(user, "customer");

                    return Ok(new { Message = "User registered successfully." });
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return BadRequest(ModelState);
        }

        [HttpGet("getroles")]
        public async Task<IActionResult> GetRoles(string username)
        {
            var user = await _userManager.FindByNameAsync(username);
            if (user == null)
            {
                return NotFound();
            }

            var roles = await _userManager.GetRolesAsync(user);
            return Ok(roles);
        }

        [Authorize(Roles = "admin")]
        [HttpPost("assign-role")]
        public async Task<IActionResult> AssignRole([FromBody] AssignRoleModel model)
        {
            var user = await _userManager.FindByNameAsync(model.Username);
            if (user == null)
            {
                return NotFound(new { Message = "User not found." });
            }

            if (!await _roleManager.RoleExistsAsync(model.Role))
            {
                return BadRequest(new { Message = "Role does not exist." });
            }

            var result = await _userManager.AddToRoleAsync(user, model.Role);
            if (result.Succeeded)
            {
                return Ok(new { Message = "Role assigned successfully." });
            }
            return BadRequest(result.Errors);
        }

        [Authorize(Roles = "admin")]
        [HttpPost("remove-role")]
        public async Task<IActionResult> RemoveRole([FromBody] AssignRoleModel model)
        {
            var user = await _userManager.FindByNameAsync(model.Username);
            if (user == null)
            {
                return NotFound(new { Message = "User not found." });
            }

            var result = await _userManager.RemoveFromRoleAsync(user, model.Role);
            if (result.Succeeded)
            {
                return Ok(new { Message = "Role removed successfully." });
            }
            return BadRequest(result.Errors);
        }
    }

}
