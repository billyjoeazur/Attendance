using Attendance.ViewModels;
using Attendance.Views;
using Xamarin.Forms;

namespace Attendance
{
	public partial class App : Application
	{
		private LoginViewModel login;
		AttendanceViewModel attendance;
		ProfileViewModel profile;
		public App()
		{
			InitializeComponent();
			login = new LoginViewModel();
			attendance = new AttendanceViewModel();
			profile = new ProfileViewModel();
			MainPage = new LoginPage();
			
		}

		protected override async void OnStart()
		{
			await login.OnLoadNewUser();
			await attendance.OnLoadNewAttendance();
			await profile.OnLoadNewProfile();
		}

		protected override void OnSleep()
		{
		}

		protected override void OnResume()
		{
		}
	}
}
