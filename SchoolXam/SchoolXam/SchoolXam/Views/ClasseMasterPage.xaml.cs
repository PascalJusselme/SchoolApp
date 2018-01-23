using Prism.Navigation;
using SchoolXam.Models;
using SchoolXam.ViewModels;
using SchoolXam.ViewModels.ChildPageViewModel;
using Xamarin.Forms;

namespace SchoolXam.Views
{
	public partial class ClasseMasterPage : ContentPage, INavigationAware
	{
		public AnneeDetailPageViewModel ViewModel => BindingContext as AnneeDetailPageViewModel;

		public ClasseMasterPage()
		{
			InitializeComponent();

			lstClasses.ItemSelected += (o, e) =>
			{
				if (e.SelectedItem is Classe)
				{
					var classe = e.SelectedItem as Classe;
					ViewModel.ClasseItemClicked.Execute(classe);
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
