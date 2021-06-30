using Attendance.Services;
using MvvmHelpers.Commands;
using Org.Json;
using Plugin.Connectivity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Drawing;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using Attendance.Models;
using MvvmHelpers;

namespace Attendance.ViewModels
{
	public class ProfileViewModel : ViewModelBase
	{
		Uri uri = new Uri(string.Format("http://192.168.100.5:8080/android/profile.php"));
		public AsyncCommand LoadNewProfileCommand { get; }

		
		public ProfileViewModel()
		{
			LoadNewProfileCommand = new AsyncCommand(OnLoadNewProfile);
			
		}

		public async Task<bool> IsReachable()
		{
			bool isReachable = await CrossConnectivity.Current.IsRemoteReachable("http://192.168.100.5", 8080, 5000);
			if (isReachable)
				return true;
			return false;
		}

		public async Task OnLoadNewProfile()
		{
			try
			{
				HttpClient myClient = new HttpClient();
				if (await IsReachable())
				{
					HttpResponseMessage response = await myClient.GetAsync(uri);
					if (response.IsSuccessStatusCode)
					{
						await ProfileService.DropTable();
						string content = await response.Content.ReadAsStringAsync();
						var Items = JsonConvert.DeserializeObject<ObservableCollection<Profile>>(content);
						foreach (var item in Items)
						{
							await ProfileService.AddProfile(item.IdNumber, item.FirstName, item.MiddleName, item.LastName, item.Designation);
						}
					}
				}
			}
			catch (Exception e)
			{
				await Application.Current.MainPage.DisplayAlert("Awit!", e.ToString(), "Awts gege");
			}
		}
	}
}
