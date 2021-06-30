using Attendance.Models;
using Attendance.Services;
using MvvmHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Attendance.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class FlyoutView : ContentView
	{
		public FlyoutView()
		{
			InitializeComponent();
			BindingContext = this;
		}
	}
}