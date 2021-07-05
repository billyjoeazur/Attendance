using Attendance.Models;
using Attendance.Services;
using Attendance.Views;
using MvvmHelpers;
using MvvmHelpers.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Plugin.Connectivity;
using System.Net.Http;
using Org.Json;
using Newtonsoft.Json;
using System.Collections.ObjectModel;

namespace Attendance.ViewModels
{
	public class LoginViewModel : ViewModelBase 
	{
		Uri uri = new Uri(string.Format("http://192.168.100.5:8080/android/user.php"));

		public string UserName { get; set; }
		public string Password { get; set; }
		public AsyncCommand LoadNewUserCommand { get; }
		public AsyncCommand LoginCommand { get; }

		public LoginViewModel()
		{
			LoadNewUserCommand = new AsyncCommand(OnLoadNewUser);
			LoginCommand = new AsyncCommand(Login);
		}

		public async Task<bool> IsReachable()
		{
			bool isReachable = await CrossConnectivity.Current.IsRemoteReachable("http://192.168.100.5", 8080, 5000);
			if (isReachable)
				return true;
			return false;
		}

		private async Task Login()
		{
			try
			{
				bool result = await LoginService.Login(UserName, LoginService.HashPassword(Password));
				if (result)
				{
					Application.Current.MainPage = new AppShell();
					var getProfile = await ProfileService.CheckProfile(UserName);
					var fullname = $"{getProfile.FirstName} {getProfile.LastName}";
					var route = $"//{nameof(AttendancePage)}?MyId={UserName}&myfullname={fullname}";
					DependencyService.Get<IToast>()?.MakeToast("Login");
					await Shell.Current.GoToAsync(route);
				}
				else
				{
					await Application.Current.MainPage.DisplayAlert("Login", "Wrong employee id or password!", "OK");
				}
			}
			catch (Exception e)
			{
				await Application.Current.MainPage.DisplayAlert("Awit!", e.ToString(), "Awts gege");
			}
		}

		public async Task OnLoadNewUser()
		{
			try
			{
				HttpClient myClient = new HttpClient();
				if (await IsReachable())
				{
					DependencyService.Get<IToast>()?.MakeToast("Connected...");
					HttpResponseMessage response = await myClient.GetAsync(uri);
					if (response.IsSuccessStatusCode)
					{
						await LoginService.DropTable();
						string content = await response.Content.ReadAsStringAsync();
						var Items = JsonConvert.DeserializeObject<ObservableCollection<User>>(content);
						foreach (var item in Items)
						{
							await LoginService.AddUser(item.Id,item.Password);
						}
					}
				}
				else
				{
					DependencyService.Get<IToast>()?.MakeToast("Not Connected");
				}
			}
			catch (Exception e)
			{
				await Application.Current.MainPage.DisplayAlert("Awit!", e.ToString(), "Awts gege");
			}
		}
	}
}
