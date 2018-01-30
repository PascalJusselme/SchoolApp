using Prism.Navigation;
using SchoolXam.Models;
using SchoolXam.ViewModels;
using Xamarin.Forms;

namespace SchoolXam.Views
{
	public partial class GestionMatierePage : ContentPage, INavigatedAware
	{
		public GestionMatierePage()
		{
			InitializeComponent();
		}

		public void OnNavigatedFrom(NavigationParameters parameters)
		{
			lstMatieres.SelectedItem = null;
		}

		public void OnNavigatedTo(NavigationParameters parameters)
		{

		}
	}
}
