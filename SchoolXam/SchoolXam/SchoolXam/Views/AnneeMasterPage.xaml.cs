using Prism.Navigation;
using SchoolXam.Models;
using SchoolXam.ViewModels;
using Xamarin.Forms;

namespace SchoolXam.Views
{
    public partial class AnneeMasterPage : ContentPage,INavigationAware
	{
		public AnneeMasterPageViewModel AnneeMasterPageViewModel => BindingContext as AnneeMasterPageViewModel;

		public AnneeMasterPage()
		{
			InitializeComponent();

			lstAnnees.ItemSelected += (o, e) =>
			{
				if (e.SelectedItem is AnneeScolaire)
				{
					var annee = e.SelectedItem as AnneeScolaire;
					AnneeMasterPageViewModel.SelectAnnee.Execute(annee);
				}
			};
		}

		public void OnNavigatedFrom(NavigationParameters parameters)
		{
			lstAnnees.SelectedItem = null;
		}

		public void OnNavigatedTo(NavigationParameters parameters)
		{
			
		}

		public void OnNavigatingTo(NavigationParameters parameters)
		{
			
		}
	}
}
