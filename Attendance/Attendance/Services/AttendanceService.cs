using Attendance.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.Services
{
	public static class AttendanceService
	{
		static SQLiteAsyncConnection db;
		static async Task Init()
		{
			if (db != null)
				return;
			// Get an absolute path to the database file
			var databasePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "MyEmployeeData.db");
			db = new SQLiteAsyncConnection(databasePath);
			await db.CreateTableAsync<Employee>();
		}

		public static async Task AddAttendance(string idno, DateTime date, string in1, string out1, string in2, string out2)
		{
			await Init();
			var employee = new Employee { EmployeeId = idno, Date = date, In1 = in1, Out1 = out1, In2 = in2, Out2 = out2 };
			var id = await db.InsertAsync(employee);
		}

		public static async Task RemoveEmployee(int id)
		{
			await Init();
			await db.DeleteAsync<Employee>(id);
		}

		public static async Task<IEnumerable<Employee>> GetAttendance()
		{
			await Init();
			var employee = await db.Table<Employee>().ToListAsync();
			return employee;
		}

		public static async Task<IEnumerable<Employee>> CheckAttendance(string employeeId, DateTime from, DateTime to)
		{
			await Init();
			var myquery = db.Table<Employee>().Where(v => v.EmployeeId == employeeId && v.Date >= from && v.Date <= to);
			var result =  myquery.ToListAsync();
			return await result;
		}

		public static async Task DropTable()
		{
			await Init();
			await db.DropTableAsync<Employee>();
			db = null;
		}

	}
}
