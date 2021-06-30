using System;
using System.Collections.Generic;
using Attendance.Services;
using Attendance.ViewModels;
using Attendance.Views;
using Xamarin.Forms;

namespace Attendance
{
	public partial class AppShell : Xamarin.Forms.Shell
	{
		public AppShell()
		{
			InitializeComponent();
			Shell.SetTabBarIsVisible(this, false);
		}

		protected override void OnAppearing()
		{
			base.OnAppearing();
		}

		private async void OnMenuItemClicked(object sender, EventArgs e)
		{
			await Shell.Current.GoToAsync("//LoginPage");
		}
	}
}
