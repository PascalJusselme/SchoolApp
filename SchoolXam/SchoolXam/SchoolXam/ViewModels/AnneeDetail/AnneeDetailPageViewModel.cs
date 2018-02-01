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
			if (String.IsNullOrEmpty(Annee.anneeLib) || String.IsNullOrWhiteSpace(Annee.anneeLib))
			{
				DisplayAlert($"Le Libellé de l'Année Scolaire ne peut pas être vide.");
			}
			else
			{
				_rep.SaveAnnee(Annee);

				await _navigationService.GoBackToRootAsync();
			}
		}

		private AnneeScolaire Load_Annee(AnneeScolaire annee)
		{
			if (annee.anneeID == 0)
			{
				annee.anneeLib = string.Empty;
				annee.Classes = new List<Classe>();
				annee.Matieres = new List<Matiere>();
			}
			else
			{
				annee = _rep.Get_Annee(annee);

				annee.Classes = _rep.GetClassesByAnnee(annee);
				annee.Matieres = _rep.Get_MatieresByAnnee(annee);

				foreach (Classe classe in annee.Classes)
				{
					Classe cl = _rep.GetClasseWithChildren(classe);

					classe.Annee = annee;
					classe.Matieres = new List<Matiere>(cl.Matieres);
					classe.Eleves = new List<Eleve>(cl.Eleves);
					classe.Devoirs = new List<Devoir>(cl.Devoirs);

					Load_Devoir(classe);
				}

				foreach (Matiere matiere in annee.Matieres)
				{
					Matiere ma = new Matiere();
					ma = _rep.Get_MatiereWithChildren(matiere);

					matiere.Annee = annee;
					matiere.Classes = new List<Classe>(ma.Classes);
					matiere.Devoirs = new List<Devoir>(ma.Devoirs);

					Load_Devoir(matiere);
				}
			}

			return annee;
		}

		private void Load_Devoir(Classe classe)
		{
			foreach (Devoir devoir in classe.Devoirs)
			{
				Devoir dev = _rep.Get_DevoirWithChildren(devoir);

				devoir.Classe = dev.Classe;
				devoir.Matiere = dev.Matiere;
			}
		}

		private void Load_Devoir(Matiere matiere)
		{
			foreach (Devoir devoir in matiere.Devoirs)
			{
				Devoir dev = _rep.Get_DevoirWithChildren(devoir);

				devoir.Classe = dev.Classe;
				devoir.Matiere = dev.Matiere;
			}
		}
		#endregion

		#region IPageDialogService Propertie and Method
		public DelegateCommand<string> DisplayAlertCommand => new DelegateCommand<string>(DisplayAlert);
		public DelegateCommand DisplayActionSheetCommand => new DelegateCommand(DisplayActionSheet);
		public DelegateCommand DisplayActionSheetUsingActionSheetButtonsCommand =>
							new DelegateCommand(DisplayActionSheetUsingActionSheetButtons);

		private async void DisplayAlert(string message)
		{
			await _pageDialogService.DisplayAlertAsync("Alert", message, "Accept", "Cancel");
		}

		private async void DisplayActionSheet()
		{
			await _pageDialogService.DisplayActionSheetAsync("ActionSheet", "Cancel", "Destroy", "Option 1", "Option 2");
		}

		private async void DisplayActionSheetUsingActionSheetButtons()
		{
			//IActionSheetButton option1Action =
			//ActionSheetButton.CreateButton("Option 1", new DelegateCommand(() => { Debug.WriteLine("Option 1"); }));
			//IActionSheetButton option2Action =
			//ActionSheetButton.CreateButton("Option 2", new DelegateCommand(() => { Debug.WriteLine("Option 2"); }));
			//IActionSheetButton cancelAction =
			//ActionSheetButton.CreateCancelButton("Cancel", new DelegateCommand(() => { Debug.WriteLine("Cancel"); }));
			//IActionSheetButton destroyAction =
			//ActionSheetButton.CreateDestroyButton("Destroy", new DelegateCommand(() => { Debug.WriteLine("Destroy"); }));

			//await _pageDialogService.DisplayActionSheetAsync("ActionSheet with ActionSheetButtons", option1Action, option2Action, cancelAction, destroyAction);
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
				StartedAnnee = Load_Annee(Annee);

				Title = Annee.anneeLib;
			}
		}

		public override void Destroy()
		{

		}
		#endregion
	}
}
