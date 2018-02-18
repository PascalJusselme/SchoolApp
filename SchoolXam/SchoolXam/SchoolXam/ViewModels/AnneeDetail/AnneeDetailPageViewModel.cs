using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using SchoolXam.Data;
using SchoolXam.Models;
using System;
using System.Collections.Generic;

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

		private AnneeScolaire _startedAnnee;
		public AnneeScolaire StartedAnnee
		{
			get { return _startedAnnee; }
			set { SetProperty(ref _startedAnnee, value); }
		}
		#endregion

		#region Annee Commands
		public DelegateCommand SaveAnneeCommand => new DelegateCommand(SaveAnnee);
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
		private async void SaveAnnee()
		{
			//if (Annee.IsSubmitEnabled)
			//{
			//	_rep.SaveAnnee(Annee);

			//	await _navigationService.GoBackToRootAsync();
			//}
			//else
			//{
			//	var param = new NavigationParameters
			//	{
			//		{ "Annee", Annee }
			//	};

			//	await _navigationService.NavigateAsync("AnneeDetailPage", param);
			//}

			if (Annee.ValidateProperties())
			{
				_rep.SaveAnnee(Annee);

				await _navigationService.GoBackToRootAsync();
			}
			else
			{
				var param = new NavigationParameters
				{
					{ "Annee", Annee }
				};

				await _navigationService.NavigateAsync("AnneeDetailPage", param);
			}
		}
		#endregion

		#region INavigationService Implementation
		public override void OnNavigatedFrom(NavigationParameters parameters)
		{
			// Implémenter PageDialog pour demander si on veut RETOURNER SUR LA LISTE DES ANNEES
			// SANS SAUVEGARDER le(s) changement(s) apportés à l'ANNÉE en cours de modif
			// TROUVER LE MOYEN DE FAIRE CELA SANS TROP DE CHARGEMENT

			//_rep.SaveAnnee(StartedAnnee);
		}

		public override void OnNavigatedTo(NavigationParameters parameters)
		{
			
		}

		public override void OnNavigatingTo(NavigationParameters parameters)
		{
			if (parameters != null && parameters.ContainsKey("Annee"))
			{
				Annee = parameters["Annee"] as AnneeScolaire;

				// Sauvegarde de l'Année à son état à la selection
				// pour ecraser les modifs si on veut pas sauvegarder
				// StartedAnnee = Load_Annee(Annee);
			}
		}

		public override void Destroy()
		{

		}
		#endregion

		#region IPageDialogService Propertie and Method
		public DelegateCommand<string> DisplayAlertCommand => new DelegateCommand<string>(DisplayAlert);

		private async void DisplayAlert(string message)
		{
			await _pageDialogService.DisplayAlertAsync("Alert", message, "Accept", "Cancel");
		}
		#endregion
	}
}
