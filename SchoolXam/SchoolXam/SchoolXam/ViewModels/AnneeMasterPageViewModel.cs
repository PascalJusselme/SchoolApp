using Prism.Commands;
using Prism.Navigation;
using SchoolXam.Data;
using SchoolXam.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace SchoolXam.ViewModels
{
	public class AnneeMasterPageViewModel : BaseViewModel
	{
		private readonly INavigationService _navigationService;

		#region Annees Properties and Commands

		#region Annees Properties
		private ObservableCollection<AnneeScolaire> _annees;
		public ObservableCollection<AnneeScolaire> Annees
		{
			get { return _annees; }
			set { SetProperty(ref _annees, value); }
		}

		private string _labetLstAnnees;
		public string LabetLstAnnees
		{
			get { return _labetLstAnnees; }
			set { SetProperty(ref _labetLstAnnees, value); }
		}
		#endregion

		#region Annees Commands
		public DelegateCommand<AnneeScolaire> SelectAnneeCommand => new DelegateCommand<AnneeScolaire>(AnneeSelected);
		public DelegateCommand AddAnneeCommand => new DelegateCommand(AddAnnee);
		#endregion

		#endregion

		public AnneeMasterPageViewModel(
					INavigationService navigationService,
					SchoolRepository db)
		: base(db)
		{
			_navigationService = navigationService;
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
			GetAnnees();
		}

		public override void Destroy()
		{

		}
		#endregion

		#region Annees Methods
		private async void AnneeSelected(AnneeScolaire annee)
		{
			var parameter = new NavigationParameters
			{
				{ "Annee", LoadAnnee(annee) }
			};

			await _navigationService.NavigateAsync("AnneeDetailPage", parameter);
		}

		private async void AddAnnee()
		{
			AnneeScolaire an = new AnneeScolaire();

			var parameter = new NavigationParameters
			{
				{ "Annee", LoadAnnee(an) }
			};

			await _navigationService.NavigateAsync("AnneeDetailPage", parameter);
		}

		private void GetAnnees()
		{
			Annees = new ObservableCollection<AnneeScolaire>(_rep.GetAnnees());

			if (Annees.Count != 0)
			{
				LabetLstAnnees = $"Liste des Années Scolaire : ";
			}
			else
			{
				LabetLstAnnees = $"Il n'y a aucune Année Scolaire Enregistrée.";
			}
		}

		private AnneeScolaire LoadAnnee(AnneeScolaire annee)
		{
			if (annee.anneeID == 0)
			{
				annee.anneeLib = string.Empty;
				annee.Classes = new List<Classe>();
				annee.Matieres = new List<Matiere>();
			}
			else
			{
				annee = _rep.GetAnnee(annee);

				annee.Classes = _rep.GetClassesByAnnee(annee);
				annee.Matieres = _rep.GetMatieresByAnnee(annee);

				foreach (Classe classe in annee.Classes)
				{
					Classe cl = new Classe();
					cl = _rep.GetClasse(classe);

					classe.Annee = annee;
					classe.Matieres = new List<Matiere>(cl.Matieres);
					classe.Eleves = new List<Eleve>(cl.Eleves);
					classe.Devoirs = new List<Devoir>(cl.Devoirs);
				}

				foreach (Matiere matiere in annee.Matieres)
				{
					Matiere ma = new Matiere();
					ma = _rep.GetMatiere(matiere);

					matiere.Annee = annee;
					matiere.Classes = new List<Classe>(ma.Classes);
					matiere.Devoirs = new List<Devoir>(ma.Devoirs);
				}
			}

			return annee;
		}
		#endregion
	}
}
