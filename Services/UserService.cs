using DashboardApi.Entities;
using DashboardApi.Helpers;
using DashboardApi.ServiceModels;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DashboardApi.Services
{
	public interface IUserService
	{
		DashboardUser Authenticate(string email, string password);
		IEnumerable<DashboardUser> GetAll();
		DashboardUser GetById(int id);
		DashboardUser Create(DashboardUser user, string password);
		bool Update(int id, UpdateUser userUpdate);
		bool Delete(int id);
	}

	public class UserService : IUserService
	{
		private DataContext _context;
		private readonly AppSettings _appSettings;

		public UserService(DataContext context, IOptions<AppSettings> appSettings)
		{
			_context = context;
			_appSettings = appSettings.Value;

		}

		public DashboardUser Authenticate(string email, string password)
		{
			if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
				return null;

			var user = _context.DashboardUsers.SingleOrDefault(x => x.Email == email);

			if (user == null)
				return null;
			if (!AuthUtility.VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
				return null;

			return user;
		}

		public IEnumerable<DashboardUser> GetAll()
		{
			return _context.DashboardUsers;
		}

		public DashboardUser GetById(int id)
		{
			return _context.DashboardUsers.Find(id);
		}

		public DashboardUser Create(DashboardUser user, string password)
		{
			// validation
			if (string.IsNullOrWhiteSpace(password))
				throw new AppException("Password is required");

			if (_context.DashboardUsers.FirstOrDefault(x => x.Email == user.Email) != null)
				throw new AppException("Email " + user.Email + " is already taken");

			byte[] passwordHash, passwordSalt;
			AuthUtility.CreatePasswordHash(password, out passwordHash, out passwordSalt);

			user.PasswordHash = passwordHash;
			user.PasswordSalt = passwordSalt;
			user.Role = Role.User;
			user.Id = 0;

			_context.DashboardUsers.Add(user);
			var invitaiton = _context.DashboardInvitations.Where(x => x.Email == user.Email).FirstOrDefault();
			if (invitaiton != null)
				invitaiton.Status = InvitationStatus.Accepted;
			_context.SaveChanges();

			return user;
		}

		public bool Update(int id, UpdateUser userUpdate)
		{
			var user = _context.DashboardUsers.Find(id);

			if (user == null)
				throw new AppException("User not found");

			if (!String.IsNullOrWhiteSpace(userUpdate.Email) && user.Email != userUpdate.Email)
			{
				// check if new email is already taken
				if (_context.DashboardUsers.FirstOrDefault(x => x.Email == userUpdate.Email) != null)
					throw new AppException("Email " + userUpdate.Email + " is already taken");
				user.Email = userUpdate.Email;
			}

			// update user properties
			user.FullName = userUpdate.FullName;

			// update password if it was entered
			if (!string.IsNullOrWhiteSpace(userUpdate.Password))
			{
				byte[] passwordHash, passwordSalt;
				AuthUtility.CreatePasswordHash(userUpdate.Password, out passwordHash, out passwordSalt);

				user.PasswordHash = passwordHash;
				user.PasswordSalt = passwordSalt;
			}

			return _context.SaveChanges() != 0;
		}

		public bool Delete(int id)
		{
			var user = _context.DashboardUsers.Find(id);
			if (user != null)
			{
				_context.DashboardUsers.Remove(user);
				return _context.SaveChanges() != 0;
			}
			return false;
		}


	}
}