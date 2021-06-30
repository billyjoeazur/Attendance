using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Attendance.Models
{
	[Table("Profile")]
	public class Profile
	{
		public string IdNumber { get; set; }
		public string FirstName { get; set; }
		public string MiddleName { get; set; }
		public string LastName { get; set; }
		public string Designation { get; set; }

	}
}
