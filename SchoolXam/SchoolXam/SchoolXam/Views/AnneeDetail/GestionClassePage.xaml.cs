using Prism.Navigation;
using SchoolXam.Models;
using SchoolXam.ViewModels;
using Xamarin.Forms;

namespace SchoolXam.Views
{
	public partial class GestionClassePage : ContentPage, INavigatedAware
	{
		public GestionClassePage()
		{
			InitializeComponent();
		}

		public void OnNavigatedFrom(NavigationParameters parameters)
		{
			lstClasses.SelectedItem = null;
		}

		public void OnNavigatedTo(NavigationParameters parameters)
		{

		}
	}
}
