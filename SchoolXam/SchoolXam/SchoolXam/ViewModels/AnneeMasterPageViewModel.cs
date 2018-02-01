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
		private ObservableCollection<AnneeScolaire> _listAnnees;
		public ObservableCollection<AnneeScolaire> ListAnnees
		{
			get { return _listAnnees; }
			set { SetProperty(ref _listAnnees, value); }
		}

		private string _labelListAnnees;
		public string LabelListAnnees
		{
			get { return _labelListAnnees; }
			set { SetProperty(ref _labelListAnnees, value); }
		}
		#endregion

		#region Annees Commands
		public DelegateCommand<AnneeScolaire> SelectAnneeCommand =>
								new DelegateCommand<AnneeScolaire>(Selected_Annee);
		public DelegateCommand AddAnneeCommand =>
								new DelegateCommand(Add_Annee);
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
			Get_ListAnnees();
		}

		public override void Destroy()
		{

		}
		#endregion

		#region Annees Methods
		private async void Selected_Annee(AnneeScolaire annee)
		{
			var parameter = new NavigationParameters
			{
				{ "Annee", Load_Annee(annee) }
			};

			await _navigationService.NavigateAsync("AnneeDetailPage", parameter);
		}

		private async void Add_Annee()
		{
			AnneeScolaire an = new AnneeScolaire();

			var parameter = new NavigationParameters
			{
				{ "Annee", Load_Annee(an) }
			};

			await _navigationService.NavigateAsync("AnneeDetailPage", parameter);
		}

		private void Get_ListAnnees()
		{
			ListAnnees = new ObservableCollection<AnneeScolaire>(_rep.Get_ListAnnees());

			if (ListAnnees.Count != 0)
			{
				LabelListAnnees = $"Liste des Années Scolaire : ";
			}
			else
			{
				LabelListAnnees = $"Il n'y a aucune Année Scolaire Enregistrée.";
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
	}
}
