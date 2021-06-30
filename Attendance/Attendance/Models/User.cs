using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace Attendance.Models
{
	[Table("User")]
	public class User
    {
		//[PrimaryKey, AutoIncrement]
		//public int Id { get; set; }
		public string Id { get; set; }
		public string Password { get; set; }
		//public string UserLevel { get; set; }
	}
}
