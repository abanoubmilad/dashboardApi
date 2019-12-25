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
using System.Linq;

namespace DashboardApi.Controllers
{
	[Authorize]
	[ApiController]
	[Route("api/[controller]")]
	public class DashboardController : ControllerBase
	{
		private IMapper _mapper;
		private IAuthService _authService;
		private IDashboardService _dashboardService;
		private readonly AppSettings _appSettings;

		public DashboardController(
			IMapper mapper,
			IAuthService authService,
		IDashboardService dashboardService,
		IOptions<AppSettings> appSettings)
		{
			_mapper = mapper;
			_authService = authService;
			_dashboardService = dashboardService;
			_appSettings = appSettings.Value;
		}
		
		
		/// <summary>
		/// Send dashboard invitaiton email
		/// </summary>
		[Authorize(Roles = Role.Admin)]
		[HttpPost("invitation")]
		public ActionResult<bool>
			Invitation([FromBody] InviteDashbaord inviteDashbaord)
		{
			try
			{
				return Ok(_dashboardService.SendDashboardInvitaitonEmail(inviteDashbaord.Email));
			}
			catch (AppException ex)
			{
				return BadRequest(new
				{
					message = ex.Message
				});
			}
		}

		/// <summary>
		/// Get dashboard invitatons
		/// </summary>
		[Authorize(Roles = Role.Admin)]
		[HttpGet("invitation")]
		public ActionResult<IList<DashboardInvitationDto>> GeInvitations()
		{
			return Ok(_dashboardService.GetDashboardInvitaitonEmails());
		}
	}
}
