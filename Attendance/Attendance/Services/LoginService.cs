using Attendance.Models;
using Attendance.Views;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Attendance.Services
{
    public static class LoginService
    {
		static SQLiteAsyncConnection db;
		static async Task Init()
		{
			if (db != null)
				return;
			// Get an absolute path to the database file
			var databasePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "MyUserData.db");
			db = new SQLiteAsyncConnection(databasePath);
			await db.CreateTableAsync<User>();
		}

		public static async Task AddUser(string employeeId, string password)
		{
			await Init();
			var user = new User { Id = employeeId, Password = password };
			var id = await db.InsertAsync(user);
		}

		public static async Task RemoveUser(int id)
		{
			await Init();
			await db.DeleteAsync<User>(id);
		}

		public static async Task<IEnumerable<User>> GetUser()
		{
			await Init();
			var user = await db.Table<User>().ToListAsync();
			return user;
		}

		static SQLiteConnection localDB;
		public static IEnumerable<User> LoadUser()
		{
			// Get an absolute path to the database file
			var databasePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "MyUserData.db");
			localDB = new SQLiteConnection(databasePath);
			var user = localDB.Table<User>().ToList();
			return user;
		}

		public static async Task<bool> Login(string employeeId, string password)
		{
			await Init();
			var myquery = db.Table<User>().Where(v => v.Id == employeeId && v.Password == password);
			if (await myquery.CountAsync() > 0)
				return true;
			return false;
		}

		public static async Task DropTable()
		{
			await Init();
			await db.DropTableAsync<User>();
			db = null;
		}
	}
}
