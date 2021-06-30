using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace Attendance.Models
{
	[Table("Employee")]
	public class Employee
	{
		[PrimaryKey, AutoIncrement]
		public int Id { get; set; }
		public string EmployeeId { get; set; }
		public DateTime Date { get; set; }
		public string In1 { get; set; }
		public string Out1 { get; set; }
		public string In2 { get; set; }
		public string Out2 { get; set; }
	}
}
