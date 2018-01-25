using Prism.Navigation;
using SchoolXam.Models;
using SchoolXam.ViewModels;
using Xamarin.Forms;

namespace SchoolXam.Views
{
	public partial class GestionMatierePage : ContentPage, INavigationAware
	{
		public AnneeDetailPageViewModel ViewModel => BindingContext as AnneeDetailPageViewModel;

		public GestionMatierePage()
        {
            InitializeComponent(); 
			
			lstMatieres.ItemSelected += (o, e) =>
			{
				if (e.SelectedItem is Matiere)
				{
					var matiere = e.SelectedItem as Matiere;
					ViewModel.SelectMatiere.Execute(matiere);
				}
			};
		}

		public void OnNavigatedTo(NavigationParameters parameters)
		{

		}

		public void OnNavigatedFrom(NavigationParameters parameters)
		{
			lstMatieres.SelectedItem = null;
		}

		public void OnNavigatingTo(NavigationParameters parameters)
		{

		}
	}
}
