using Prism.Navigation;
using SchoolXam.Models;
using SchoolXam.ViewModels;
using SchoolXam.ViewModels.ChildPageViewModel;
using Xamarin.Forms;

namespace SchoolXam.Views
{
    public partial class MatiereMasterPage : ContentPage, INavigationAware
	{
		public AnneeDetailPageViewModel ViewModel => BindingContext as AnneeDetailPageViewModel;
		//public MatiereMasterPageViewModel ViewModel => BindingContext as MatiereMasterPageViewModel;

		public MatiereMasterPage()
        {
            InitializeComponent(); 
			
			lstMatieres.ItemSelected += (o, e) =>
			{
				if (e.SelectedItem is Matiere)
				{
					var matiere = e.SelectedItem as Matiere;
					ViewModel.MatiereItemClicked.Execute(matiere);
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
