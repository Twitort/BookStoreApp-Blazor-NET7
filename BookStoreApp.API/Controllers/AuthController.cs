using AutoMapper;
using BookStoreApp.API.Data;
using BookStoreApp.API.Models.User;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Serilog.Data;
using System.Runtime.InteropServices;
using System.Security.Claims;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using BookStoreApp.API.Static;

namespace BookStoreApp.API.Controllers
{
    [Route("api/[controller]")]
	[ApiController]
	public class AuthController : ControllerBase
	{
		private readonly ILogger<AuthController> logger;
		private readonly IMapper _mapper;
		private readonly UserManager<APIUser> _userManager;
		private readonly IConfiguration _configuration;

		public AuthController(ILogger<AuthController> logger, IMapper mapper, UserManager<APIUser> userManager, IConfiguration configuration)
		{
			this.logger = logger;
			_mapper     = mapper;
			_userManager = userManager;
			_configuration = configuration;
		}

		[HttpPost]
		[Route("register")]
		//public async Task<IActionResult> Login(string returnUrl)
		public async Task<IActionResult> Register(UserDTO userDTO)
		{
			var apiUser = _mapper.Map<APIUser>(userDTO);
			apiUser.UserName = userDTO.Email;
			var result = await _userManager.CreateAsync(apiUser, userDTO.Password);

			logger.LogInformation($"Registration attempt for {userDTO.Email}");

			try
			{
				if (!result.Succeeded)
				{
					foreach (var error in result.Errors)
					{
						ModelState.AddModelError(error.Code, error.Description);
					}

					logger.LogError($"User registraion failure for {userDTO.Email}");

					return BadRequest(ModelState);
				}

				// Assign user's role:
				await _userManager.AddToRoleAsync(apiUser, userDTO.Role);

				logger.LogInformation($"New user registered: {userDTO.Email} with role: {userDTO.Role}");

				return Ok(Accepted());
			}
			catch (Exception ex)
			{
				logger.LogError(ex, $"Exception in {nameof(Register)} during registraion for {userDTO.Email}");
				return Problem($"Exception in {nameof(Register)} during registraion for {userDTO.Email}", statusCode: 500);
			}
		}

		[HttpPost]
		[Route("login")]
		public async Task<ActionResult<AuthResponse>> Login(LoginUserDTO liUserDTO)
		{
			logger.LogInformation($"Login attempt for {liUserDTO.Email}");

			try
			{
				var user = await _userManager.FindByEmailAsync(liUserDTO.Email);
				if (user == null)
				{
					logger.LogInformation($"User not found: {liUserDTO.Email}");
					return Unauthorized(liUserDTO);
				}

				var passwordValid = await _userManager.CheckPasswordAsync(user, liUserDTO.Password);
				if (!passwordValid) 
				{
					logger.LogInformation($"Incorrect password for: {liUserDTO.Email}");
					return Unauthorized(liUserDTO);
				}

				string tokenString = await GenerateToken(user);

				var response = new AuthResponse
				{
					Email  = liUserDTO.Email,
					Token  = tokenString,
					UserID = user.Id
				};

				logger.LogInformation($"Successful login: {liUserDTO.Email}");
				return Ok(Accepted(response));
			}
			catch (Exception ex)
			{
				logger.LogError(ex, $"Exception in {nameof(Login)} during login of {liUserDTO.Email}");
				return Problem($"Exception in {nameof(Login)} during registraion for {liUserDTO.Email}", statusCode: 500);
			}
		}

		private async Task<string> GenerateToken(APIUser user)
		{
			var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:Key"]));
			var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

			var roles = await _userManager.GetRolesAsync(user);
			var roleClaims = roles.Select(q => new Claim(ClaimTypes.Role, q)).ToList();

			var userClaims = await _userManager.GetClaimsAsync(user);

			var claims = new List<Claim>
			{
				new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
				new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
				new Claim(JwtRegisteredClaimNames.Email, user.Email),
				new Claim(CustomClaimTypes.Uid, user.Id),

			}
			.Union(roleClaims)
			.Union(userClaims);

			var token = new JwtSecurityToken(
				issuer: _configuration["JwtSettings:Issuer"],
				audience: _configuration["JwtSettings:Audience"],
				claims: claims,
				expires: DateTime.Now.AddHours(Convert.ToInt32(_configuration["JwtSettings:Duration"])),
				signingCredentials: credentials
				);

			return new JwtSecurityTokenHandler().WriteToken(token);
		}
	}
}
