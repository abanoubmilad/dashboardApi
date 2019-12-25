using AutoMapper;
using DashboardApi.Dtos;
using DashboardApi.Entities;
using DashboardApi.ServiceModels;

namespace DashboardApi.Helpers
{
	public class AutoMapperProfile : Profile
	{
		public AutoMapperProfile()
		{

			CreateMap<DashboardUser, UserDto>();
			CreateMap<UserDto, DashboardUser>();
			CreateMap<LoginDto, DashboardUser>();
			CreateMap<DashboardUser, LoginDto>();
			CreateMap<RegisterUser, DashboardUser>();

		}
	}
}