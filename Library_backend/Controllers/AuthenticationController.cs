using Library_backend.Context;
using Library_backend.Services;
using Library_backend.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Library_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly TokenService _tokenService;
        private readonly string _adminCode;
        private readonly ILogger _logger;
        public AuthenticationController(UserManager<ApplicationUser> userManager, TokenService tokenService, IConfiguration configuration, ILogger<AuthenticationController> logger)
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _adminCode = configuration["AdminSettings:SecretAdminCode"];
            _logger = logger;
        }

        // Registering a new user

        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser(RegisterModel registerModel)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid model state for RegisterUser");
                return BadRequest(ModelState);
            }

            var existingUser = await _userManager.FindByNameAsync(registerModel.UserName);
            if (existingUser != null)
            {
                _logger.LogWarning("User with username {UserName} already exists.", registerModel.UserName);
                ModelState.AddModelError("UserName", "User with this username already exists.");
                return Conflict(new { status = "Error", message = "User with this username already exists." });
            }

            var newUser = new ApplicationUser
            {
                UserName = registerModel.UserName,
                Email = registerModel.Email,
                SecurityStamp = System.Guid.NewGuid().ToString(),
                Code = null // No admin code for regular users
            };

            var result = await _userManager.CreateAsync(newUser, registerModel.Password);
            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => e.Description);
                _logger.LogWarning("User registration failed: {Errors}", string.Join(", ", errors));
                return BadRequest(new { status = "Error", message = "User registration failed.", errors });
            }

            return Ok(new { status = "Ok", message = "User registered successfully." });
        }

        // Login as user 
        [HttpPost("loginUser")]
        public async Task<IActionResult> LoginUser(UserLoginModel loginModel)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid model state for LoginUser");
                return BadRequest(ModelState);
            }
            var user = await _userManager.FindByNameAsync(loginModel.username);
            if (user == null || !await _userManager.CheckPasswordAsync(user, loginModel.password))
            {
                _logger.LogWarning("Invalid login attempt for username {UserName}", loginModel.username);
                return Unauthorized(new { status = "Unauthorized", message = "Invalid Credentials" });
            }

            // Generate JWT Token
            var token = await _tokenService.GenerateTokenAsync(user.UserName, "User");

            return Ok(new
            {
                status = "Ok",
                message = "User logged in successfully.",
                userName = user.UserName,
                token
            });
        }

        // Login as admin

        [HttpPost("loginAdmin")]
        public async Task<IActionResult> LoginAdmin(AdminLoginModel loginModel)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid model state for LoginAdmin");
                return BadRequest(ModelState);
            }
            var user = await _userManager.FindByNameAsync(loginModel.username);
            if (user == null || !await _userManager.CheckPasswordAsync(user, loginModel.password))
            {
                _logger.LogWarning("Invalid admin login attempt for username {UserName}", loginModel.username);
                return Unauthorized(new { status = "Unauthorized", message = "Invalid Credentials" });
            }

            // Validate Admin Code
            if (loginModel.code != _adminCode)
            {
                _logger.LogWarning("Invalid admin code attempt for username {UserName}", loginModel.username);
                return Unauthorized(new { status = "Unauthorized", message = "Invalid Admin Code" });
            }
            // Generate JWT Token
            var token = await _tokenService.GenerateTokenAsync(user.UserName, "Admin");
            return Ok(new
            {
                status = "Ok",
                message = "Admin logged in successfully.",
                userName = user.UserName,
                token
            });
        }
    }
}
