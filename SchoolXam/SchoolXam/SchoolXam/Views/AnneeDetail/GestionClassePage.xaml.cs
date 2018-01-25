using Prism.Navigation;
using SchoolXam.Models;
using SchoolXam.ViewModels;
using Xamarin.Forms;

namespace SchoolXam.Views
{
	public partial class GestionClassePage : ContentPage, INavigationAware
	{
		public AnneeDetailPageViewModel ViewModel => BindingContext as AnneeDetailPageViewModel;

		public GestionClassePage()
		{
			InitializeComponent();

			lstClasses.ItemSelected += (o, e) =>
			{
				if (e.SelectedItem is Classe)
				{
					var classe = e.SelectedItem as Classe;
					ViewModel.SelectClasse.Execute(classe);
				}
			};
		}

		public void OnNavigatedTo(NavigationParameters parameters)
		{

		}

		public void OnNavigatedFrom(NavigationParameters parameters)
		{
			lstClasses.SelectedItem = null;
		}

		public void OnNavigatingTo(NavigationParameters parameters)
		{

		}
	}
}
