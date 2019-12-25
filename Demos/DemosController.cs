using AutoMapper;
using DashboardApi.Dtos;
using DashboardApi.Entities;
using DashboardApi.Helpers;
using DashboardApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Linq;

namespace DashboardApi.Tests
{
	[ApiController]
	[Route("api/[Controller]")]
	public class DemosController : ControllerBase
	{
		private readonly IMapper _mapper;
		private readonly DataContext _context;
		private readonly AppSettings _appSettings;
		private readonly IDashboardService _dashboardService;


		public DemosController(
			IMapper mapper,
			IOptions<AppSettings> appSettings,
			DataContext context,
			IDashboardService dashboardService)
		{
			_mapper = mapper;
			_appSettings = appSettings.Value;
			_context = context;
			_dashboardService = dashboardService;

		}
		private ActionResult<LoginDto> GetAuthorizedUser(DashboardUser user)
		{
			// return basic user info and token without password
			var userLoginDto = _mapper.Map<LoginDto>(user);
			userLoginDto.Token = AuthUtility.GetUserToken(user, _appSettings);
			return Ok(userLoginDto);
		}
		private void ClearAllTablesData()
		{
			_context.DashboardUsers.RemoveRange(_context.DashboardUsers.Where(x => x.Id != DashboardConfig.DashboardUserAdminId));
			_context.DashboardInvitations.RemoveRange(_context.DashboardInvitations);

			_context.SaveChanges();
		}
		/// <summary>
		/// Return dashboard admin
		/// </summary>
		[AllowAnonymous]
		[HttpGet("dashboardAdmin")]
		public ActionResult<LoginDto> dashboardAdmin()
		{
			var admin = _context.DashboardUsers.First(x => x.Id == DashboardConfig.DashboardUserAdminId);
			return GetAuthorizedUser(admin);
		}
		
		/// <summary>
		/// Return newly created dashboard user
		/// </summary>
		[AllowAnonymous]
		[HttpGet("CreateDummyUser")]
		public ActionResult<DashboardUser> CreateDummyUser()
		{
			return Ok(BulkUtiltiy.AddUser(_context));
		}
		/// <summary>
		/// Clear all tables data
		/// </summary>
		[AllowAnonymous]
		[HttpDelete("clearAllData")]
		public IActionResult ClearAllData()
		{
			ClearAllTablesData();
			return Ok();
		}
	}
}
