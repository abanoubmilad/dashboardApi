using DashboardApi.Entities;
using DashboardApi.Entities.Data;
using DashboardApi.Helpers;
using System;
using System.Linq;

namespace DashboardApi.Tests
{
	public static class BulkUtiltiy
	{
		public static DashboardUser CreateUser(DataContext context, DashboardUser user, string password)
		{

			byte[] passwordHash, passwordSalt;
			AuthUtility.CreatePasswordHash(password, out passwordHash, out passwordSalt);

			user.PasswordHash = passwordHash;
			user.PasswordSalt = passwordSalt;
			user.Role = Role.User;
			user.Id = 0;

			context.DashboardUsers.Add(user);
			context.SaveChanges();

			return user;
		}
		public static DashboardUser AddUser(DataContext context)
		{
			var user = CreateUser(context, new DashboardUser
			{
				Id = 0,
				FullName = $"User dummy {AuthUtility.CreateRandomString(10)}",
				Email = $"user_dummy_{ AuthUtility.CreateRandomString(10)}@gmail.com",
				Role = Role.User
			}, "password");
			return user;

		}
		
	}
}
