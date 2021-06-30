using Attendance.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.Services
{
	public static class ProfileService
	{
		static SQLiteAsyncConnection db;
		static async Task Init()
		{
			if (db != null)
				return;
			// Get an absolute path to the database file
			var databasePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "MyProfileData.db");
			db = new SQLiteAsyncConnection(databasePath);
			await db.CreateTableAsync<Profile>();
		}

		public static async Task AddProfile(string employeeId, string firstname, string middlename, string lastname, string designation)
		{
			await Init();
			var profile = new Profile { IdNumber = employeeId, FirstName = firstname, MiddleName = middlename, LastName = lastname, Designation = designation };
			var id = await db.InsertAsync(profile);
		}

		public static async Task DropTable()
		{
			await Init();
			await db.DropTableAsync<Profile>();
			db = null;
			await Init();
		}

		public static async Task<Profile> CheckProfile(string employeeId)
		{
			await Init();
			var myquery = db.Table<Profile>().Where(v => v.IdNumber == employeeId);
			var result = myquery.FirstAsync();
			return await result;
			
		}
	}
}
