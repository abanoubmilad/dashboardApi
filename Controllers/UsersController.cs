using AutoMapper;
using DashboardApi.Dtos;
using DashboardApi.Entities;
using DashboardApi.Helpers;
using DashboardApi.ServiceModels;
using DashboardApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Collections.Generic;

namespace DashboardApi.Controllers
{
	[Authorize]
	[ApiController]
	[Route("api/[controller]")]
	public class UsersController : ControllerBase
	{
		private IUserService _userService;
		private IMapper _mapper;
		private IAuthService _authService;
		private readonly AppSettings _appSettings;

		public UsersController(
			IUserService userService,
			IMapper mapper,
			IAuthService authService,
			IOptions<AppSettings> appSettings)
		{
			_userService = userService;
			_mapper = mapper;
			_authService = authService;
			_appSettings = appSettings.Value;
		}


		private ActionResult<LoginDto> GetAuthorizedUser(DashboardUser user)
		{
			// return basic user info and token without password
			var userLoginDto = _mapper.Map<LoginDto>(user);
			userLoginDto.Token = AuthUtility.GetUserToken(user, _appSettings);
			return Ok(userLoginDto);
		}
		/// <summary>
		/// Login
		/// </summary>
		[AllowAnonymous]
		[HttpPost("login")]
		public ActionResult<LoginDto> Login([FromBody]LoginUser userLogin)
		{
			var user = _userService.Authenticate(userLogin.Email, userLogin.Password);

			if (user == null)
				return BadRequest(new { message = "Email or password is incorrect" });

			return GetAuthorizedUser(user);
		}

		/// <summary>
		/// Create a new user
		/// </summary>
		[AllowAnonymous]
		[HttpPost("register")]
		public ActionResult<LoginDto> Register([FromBody]RegisterUser userRegister)
		{
			try
			{
				_authService.CheckDashbaordInvitation(userRegister.Email, userRegister.ConfirmationCode);

				var user = _userService.Create(_mapper.Map<DashboardUser>(userRegister), userRegister.Password);
				if (user == null)
					return BadRequest(new { message = "User could not be created" });

				return GetAuthorizedUser(user);
			}
			catch (AppException ex)
			{
				return BadRequest(new { message = ex.Message });
			}
		}
		/// <summary>
		/// Get all users
		/// </summary>
		[Authorize(Roles = Role.Admin)]
		[HttpGet]
		public ActionResult<IList<UserDto>> GetAll()
		{
			var users = _userService.GetAll();
			var userDtos = _mapper.Map<IList<UserDto>>(users);
			return Ok(userDtos);
		}
		/// <summary>
		/// Get currently logged in user
		/// </summary>
		[HttpGet("profile")]
		public ActionResult<UserDto> GetById()
		{
			var id = AuthUtility.GetCurrentUserId(User);
			try
			{
				_authService.CheckDashboardReadAccessToUser(AuthUtility.GetCurrentUserId(User), id);
			}
			catch (AppException ex)
			{
				return BadRequest(new { message = ex.Message });
			}
			return Ok(_mapper.Map<UserDto>(_userService.GetById(id)));
		}
		/// <summary>
		/// Get user
		/// </summary>
		[HttpGet("{id}")]
		public ActionResult<UserDto> GetById(int id)
		{
			try
			{
				_authService.CheckDashboardReadAccessToUser(AuthUtility.GetCurrentUserId(User), id);
			}
			catch (AppException ex)
			{
				return BadRequest(new { message = ex.Message });
			}
			return Ok(_mapper.Map<UserDto>(_userService.GetById(id)));
		}
		/// <summary>
		/// Update user
		/// </summary>
		[HttpPut("{id}")]
		public ActionResult<bool> Update(int id, [FromBody]UpdateUser userUpdate)
		{
			try
			{
				_authService.CheckDashboardWriteAccessToUser(AuthUtility.GetCurrentUserId(User), id);
				return Ok(_userService.Update(id, userUpdate));
			}
			catch (AppException ex)
			{
				return BadRequest(new { message = ex.Message });
			}
		}
		/// <summary>
		/// Delete user 
		/// </summary>
		[Authorize(Roles = Role.Admin)]
		[HttpDelete("{id}")]
		public ActionResult<bool> Delete(int id)
		{
			try
			{
				_authService.CheckDashboardWriteAccessToUser(AuthUtility.GetCurrentUserId(User), id);
			}
			catch (AppException ex)
			{
				return BadRequest(new { message = ex.Message });
			}
			return Ok(_userService.Delete(id));
		}
		/// <summary>
		/// Logout
		/// </summary>
		[HttpDelete("logout")]
		public ActionResult Logout()
		{
			return Ok();
		}
	}
}
