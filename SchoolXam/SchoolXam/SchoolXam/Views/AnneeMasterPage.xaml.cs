using Prism.Navigation;
using Xamarin.Forms;

namespace SchoolXam.Views
{
	public partial class AnneeMasterPage : ContentPage, INavigatedAware
	{
		public AnneeMasterPage()
		{
			InitializeComponent();
		}

		public void OnNavigatedFrom(NavigationParameters parameters)
		{
			lstAnnees.SelectedItem = null;
		}

		public void OnNavigatedTo(NavigationParameters parameters)
		{

		}
	}
}
