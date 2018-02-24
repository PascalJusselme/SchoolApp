using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using SchoolXam.Data;
using SchoolXam.Models;

namespace SchoolXam.ViewModels
{
	public class MatiereDetailPageViewModel : BaseViewModel
	{
		private readonly INavigationService _navigationService;
		private readonly IPageDialogService _pageDialogService;

		private Matiere _matiere;
		public Matiere Matiere
		{
			get { return _matiere; }
			set { SetProperty(ref _matiere, value); }
		}

		#region Matiere Commands
		public DelegateCommand SaveMatiereCommand => new DelegateCommand(SaveMatiere);
		#endregion

		public MatiereDetailPageViewModel(
								INavigationService navigationService,
								IPageDialogService pageDialogService,
								SchoolRepository db) : base(db)
		{
			_navigationService = navigationService;

			_pageDialogService = pageDialogService;
		}

		private async void SaveMatiere()
		{
			if (Matiere.ValidateProperties())
			{
				AnneeScolaire an = Matiere.Annee;

				an.Matieres.Add(Matiere);

				var param = new NavigationParameters
				{
					{ "Annee", an }
				};

				await _navigationService.NavigateAsync("AnneeDetailPage", param);
			}
			else
			{
				var param = new NavigationParameters
				{
					{ "Matiere", Matiere }
				};

				await _navigationService.NavigateAsync("MatiereDetailPage", param);
			}
		}

		#region INavigationService Implementation
		public override void OnNavigatedFrom(NavigationParameters parameters)
		{

		}

		public override void OnNavigatedTo(NavigationParameters parameters)
		{

		}

		public override void OnNavigatingTo(NavigationParameters parameters)
		{
			if (parameters != null && parameters.ContainsKey("Matiere"))
			{
				Matiere = parameters["Matiere"] as Matiere;
			}
		}
		#endregion
	}
}
