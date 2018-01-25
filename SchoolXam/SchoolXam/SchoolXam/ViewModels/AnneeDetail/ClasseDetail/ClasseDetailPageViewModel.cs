using Prism.Navigation;
using SchoolXam.Data;
using SchoolXam.Models;

namespace SchoolXam.ViewModels
{
	public class ClasseDetailPageViewModel : ChildTabbedPageViewModel
	{
		public ClasseDetailPageViewModel(
					INavigationService navigationService,
					SchoolRepository db)
					: base(navigationService, db)
		{

		}

		public override void OnNavigatedFrom(NavigationParameters parameters)
		{

		}

		public override void OnNavigatedTo(NavigationParameters parameters)
		{
			
		}

		public override void OnNavigatingTo(NavigationParameters parameters)
		{
			if (parameters != null && parameters.ContainsKey("Classe"))
			{
				Classe = parameters["Classe"] as Classe;

				SubTitle = $"Classe : {Classe.classeLib}";
			}
		}

		public override void Destroy()
		{

		}
	}
}
