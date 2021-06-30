using Attendance.Models;
using Attendance.Services;
using MvvmHelpers;
using MvvmHelpers.Commands;
using Org.Json;
using Plugin.Connectivity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Attendance.ViewModels
{
	[QueryProperty(nameof(MyId), nameof(MyId))]
	[QueryProperty(nameof(myfullname), nameof(myfullname))]
	public class AttendanceViewModel : ViewModelBase
	{
		Uri uri = new Uri(string.Format("http://192.168.100.5:8080/android/attendance.php"));
		public ObservableRangeCollection<Employee> EmployeeList { get; set; }
		
		string totalHours;
		public string TotalHours
		{ 
			get => totalHours; 
			set 
			{
				if (value == totalHours)
					return;
				totalHours = value;
				OnPropertyChanged("TotalHours");
			} 
		}
		DateTime _startdate = DateTime.Now;
		public DateTime StartDate
		{
			get
			{
				return _startdate;
			}
			set
			{
				_startdate = value;
				OnPropertyChanged("StartDate");
				OnPropertyChanged(CheckAttendance);
			}
		}

		DateTime _enddate = DateTime.Now;
		public DateTime EndDate
		{
			get
			{
				return _enddate;
			}
			set
			{
				_enddate = value;
				OnPropertyChanged("EndDate");
				OnPropertyChanged(CheckAttendance);
			}
		}

		private async void OnPropertyChanged(Func<Task> checkAttendance)
		{
			var attendance = await AttendanceService.CheckAttendance(MyId, StartDate, EndDate);
			EmployeeList.Clear();
			EmployeeList.AddRange(attendance);

			DateTime startTime = DateTime.Now;
			DateTime endTime = DateTime.Now;
			TimeSpan myTotalHours = startTime - endTime;
			foreach (var item in EmployeeList)
			{
				DateTime in1 = DateTime.Parse(item.In1);
				DateTime out1 = DateTime.Parse(item.Out1);
				DateTime in2 = DateTime.Parse(item.In2);
				DateTime out2 = DateTime.Parse(item.Out2);
				TimeSpan amm = out1.Subtract(in1);
				TimeSpan pmm = out2.Subtract(in2);
				TimeSpan dayHours = amm.Add(pmm);
				myTotalHours = dayHours + myTotalHours;
			}
			TotalHours = myTotalHours.TotalHours.ToString("N0") + ":" + myTotalHours.Minutes.ToString();
		}

		string myid = string.Empty;
		public string MyId
		{
			get => myid;
			set
			{
				if (value == myid)
					return;
				myid = value;
				OnPropertyChanged();
			}
		}
		public string myfullname { get; set; }
		public string mydesignation { get; set; }

		public AttendanceViewModel()
		{
			EmployeeList = new ObservableRangeCollection<Employee>();
			//Task.Run(async () => { await GetProfile(); }).Wait();
		}

		


		async Task CheckAttendance()
		{
			
		}

		//async Task GetProfile()
		//{
		//	var getProfile = await ProfileService.CheckProfile(myid);
		//	myfullname = $"{getProfile.FirstName} {getProfile.LastName}";
		//	//myF = getProfile.FirstName + " " + getProfile.MiddleName + " " + getProfile.LastName;
		//	mydesignation = getProfile.Designation;
		//}

		public async Task<bool> IsReachable()
		{
			bool isReachable = await CrossConnectivity.Current.IsRemoteReachable("http://192.168.100.5", 8080, 5000);
			if (isReachable)
				return true;
			return false;
		}

		public async Task OnLoadNewAttendance()
		{
			try
			{
				HttpClient myClient = new HttpClient();
				if (await IsReachable())
				{
					HttpResponseMessage response = await myClient.GetAsync(uri);
					if (response.IsSuccessStatusCode)
					{
						await AttendanceService.DropTable();
						EmployeeList.Clear();
						string content = await response.Content.ReadAsStringAsync();
						JSONArray ja = new JSONArray(content);
						JSONObject jo;
						for (int i = 0; i < ja.Length(); i++)
						{
							jo = ja.GetJSONObject(i);
							string idno = jo.GetString("idno");
							string s = jo.GetString("datetoday");
							DateTime date = DateTime.Parse(s);
							string in1 = jo.GetString("in1");
							string out1 = jo.GetString("out1");
							string in2 = jo.GetString("in2");
							string out2 = jo.GetString("out2");

							await AttendanceService.AddAttendance(idno, date, in1, out1, in2, out2);
						}
					}
				}
			}
			catch (Exception e)
			{
				e.ToString();
			}
		}
	}
}
