using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using SchoolXam.Data;
using SchoolXam.Models;
using System.Threading.Tasks;

namespace SchoolXam.ViewModels
{
	public class AnneeDetailPageViewModel : BaseViewModel
	{
		private readonly INavigationService _navigationService;
		private readonly IPageDialogService _pageDialogService;

		#region Annee Properties and Commands

		#region Annee Properties
		private AnneeScolaire _annee;
		public AnneeScolaire Annee
		{
			get { return _annee; }
			set { SetProperty(ref _annee, value); }
		}

		private bool _isNewAnnee;
		public bool IsNewAnnee
		{
			get { return _isNewAnnee; }
			set { SetProperty(ref _isNewAnnee, value); }
		}
		#endregion

		#region Annee Commands
		public DelegateCommand SaveAnneeCommand => new DelegateCommand(SaveAnnee);
		public DelegateCommand ExitAnneeCommand => new DelegateCommand(ExitAnneeButton);
		#endregion

		#endregion

		public AnneeDetailPageViewModel(
			INavigationService navigationService,
			IPageDialogService pageDialogService,
			SchoolRepository db)
			: base(db)
		{
			_navigationService = navigationService;

			_pageDialogService = pageDialogService;
		}

		#region AnneeScolaire Methods
		private void SaveAnnee()
		{
			if (IsValid_Annee())
			{
				_rep.SaveAnnee(Annee);

				_navigationService.GoBackToRootAsync();
			}
		}

		private bool IsValid_Annee()
		{
			if (Annee.ValidateProperties())
			{
				if (!_rep.Get_ListAnnees().Exists(an => an.anneeLib == Annee.anneeLib
													  && an.anneeID != Annee.anneeID))
				{
					return true;
				}
				else
				{
					_pageDialogService.DisplayAlertAsync("Erreur",
														 "Une année ayant le même libellé existe déjà.",
														 "OK");

					return false;
				}
			}
			else
			{
				_pageDialogService.DisplayAlertAsync("Erreur", "L'année en cours d'édition n'est pas valide.\n" +
													 "Veuillez vérifier son libellé.", "OK");

				return false;
			}
		}

		private void DeleteAnnee(AnneeScolaire annee)
		{

		}

		private async void ExitAnneeButton()
		{
			await ExitAnnee();
		}

		// Method use for HardWare Back Button on Android
		public virtual async Task<bool> ExitAnnee()
		{
			string message = $"Voulez-vous vraiment quitter l'année en cours d'édition?";

			var res = await _pageDialogService.DisplayAlertAsync("Alert", message, "Exit without Save", "Save and Exit");

			// Exit without Save
			if (res)
			{
				await _navigationService.GoBackToRootAsync();
			}
			else
			{
				SaveAnnee();
			}

			return res;
		}
		#endregion

		#region INavigationService Implementation
		public override void OnNavigatedFrom(NavigationParameters parameters)
		{

		}

		public override void OnNavigatedTo(NavigationParameters parameters)
		{

		}

		public override void OnNavigatingTo(NavigationParameters parameters)
		{
			if (parameters.ContainsKey("Annee"))
			{
				Annee = parameters["Annee"] as AnneeScolaire;
			}
		}
		#endregion
	}
}
