﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Prism;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using SchoolXam.Data;
using SchoolXam.Models;

namespace SchoolXam.ViewModels
{
	public class ClasseDetailPageViewModel : BaseViewModel, IActiveAware
	{
		private INavigationService _navigationService;
		private IPageDialogService _pageDialogService;

		#region Classe Properties and Commands

		#region Classe Properties
		private Classe _classe;
		public Classe Classe
		{
			get { return _classe; }
			set { SetProperty(ref _classe, value); }
		}

		private List<SelectableData<Matiere>> _matiereAttribuableToClasse;
		public List<SelectableData<Matiere>> MatiereAttribuableToClasse
		{
			get { return _matiereAttribuableToClasse; }
			set { SetProperty(ref _matiereAttribuableToClasse, value); }
		}

		private bool _isVisible_CountAnneeMatiere;
		public bool IsVisible_CountAnneeMatiere
		{
			get { return _isVisible_CountAnneeMatiere; }
			set { SetProperty(ref _isVisible_CountAnneeMatiere, value); }
		}

		private string _lbl_IsVisible_lstMatiereAttribuable;
		public string Lbl_IsVisible_lstMatiereAttribuable
		{
			get { return _lbl_IsVisible_lstMatiereAttribuable; }
			set { SetProperty(ref _lbl_IsVisible_lstMatiereAttribuable, value); }
		}
		#endregion

		#region Classe Commands
		public DelegateCommand AttribMatiereToClasseCommand => new DelegateCommand(AttribMatiereToClasse);
		#endregion

		#endregion

		#region Eleve Properties Commands

		#region Eleve Properties
		private ObservableCollection<Eleve> _listElevesbyClasse;
		public ObservableCollection<Eleve> ListElevesbyClasse
		{
			get { return _listElevesbyClasse; }
			set { SetProperty(ref _listElevesbyClasse, value); }
		}

		private string _nomEleve;
		public string NomEleve
		{
			get { return _nomEleve; }
			set { SetProperty(ref _nomEleve, value); }
		}

		private string _prenomEleve;
		public string PrenomEleve
		{
			get { return _prenomEleve; }
			set { SetProperty(ref _prenomEleve, value); }
		}

		private string _labelLstEleve;
		public string LabelLstEleve
		{
			get { return _labelLstEleve; }
			set { SetProperty(ref _labelLstEleve, value); }
		}
		#endregion

		#region Eleve Commands
		public DelegateCommand AddEleveToClasseCommand => new DelegateCommand(AddEleveToClasse);
		#endregion

		#endregion

		#region IPageDialogService Command and Method
		public DelegateCommand<string> DisplayAlertCommand => new DelegateCommand<string>(DisplayAlert);

		private async void DisplayAlert(string message)
		{
			await _pageDialogService.DisplayAlertAsync("Alert", message, "Accept", "Cancel");
		}
		#endregion

		#region IActiveAware Implementation
		public event EventHandler IsActiveChanged;

		protected static bool HasInitialized { get; set; }

		private bool isActive;
		public bool IsActive
		{
			get => isActive;
			set => SetProperty(ref isActive, value, RaiseIsActiveChanged);
		}

		private void RaiseIsActiveChanged()
		{
			ListElevesbyClasse = new ObservableCollection<Eleve>(Classe.Eleves);
			AffichageListeEleve(Classe);
			RefreshLstMatiereAttribuable(Classe);
		}
		#endregion

		public ClasseDetailPageViewModel(
						INavigationService navigationService,
						IPageDialogService pageDialogService,
						SchoolRepository db)
						: base(db)
		{
			_navigationService = navigationService;

			_pageDialogService = pageDialogService;
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
			if (parameters != null && parameters.ContainsKey("Classe"))
			{
				Classe = parameters["Classe"] as Classe;

				Refresh_UIClasseDetail(Classe);

				LoadLstMatiereAttribuable(Classe);
			}
		}

		public override void Destroy()
		{

		}
		#endregion

		#region Classe Methods
		private void AttribMatiereToClasse()
		{
			Classe.Matieres.Clear();

			foreach (var data in MatiereAttribuableToClasse)
			{
				Matiere matiere = data.Data;

				if (data.Selected
						&& !Classe.Matieres
								  .Exists(m => m.matiereLib == matiere.matiereLib))
				{
					Classe.Matieres.Add(matiere);

					if (!matiere.Classes.Exists(cl => cl.classeLib == Classe.classeLib))
					{
						matiere.Classes.Add(Classe);
					}
				}
				else
				{

					// Implémenter PageDialog pour SIGNALER la SUPPRESSION
					// des DEVOIRS liés à la DÉSATTRIBUTION en cours


					Classe.Matieres.Remove(matiere);

					if (matiere.Classes.Exists(cl => cl.classeLib == Classe.classeLib))
					{
						matiere.Classes.Remove(Classe);
						Delete_Devoir_OnDesattribClasseMatiere(Classe, matiere);
					}
				}
			}
		}

		private void LoadLstMatiereAttribuable(Classe classe)
		{
			//Load ListMatiereAttribuable with Attribuate Matiere pre-load
			MatiereAttribuableToClasse = new List<SelectableData<Matiere>>();

			foreach (Matiere matiere in classe.Annee.Matieres)
			{
				MatiereAttribuableToClasse.Add(new SelectableData<Matiere>() { Data = matiere });
			}
		}

		public void RefreshLstMatiereAttribuable(Classe classe)
		{
			foreach (Matiere matiere in classe.Annee.Matieres)
			{
				if (matiere.Classes
						   .Exists(c => c.classeLib == classe.classeLib)
					&& classe.Matieres
							 .Exists(m => m.matiereLib == matiere.matiereLib))
				{
					MatiereAttribuableToClasse.Find(d => d.Data.matiereLib == matiere.matiereLib)
											  .Selected = true;
				}
				else
				{
					MatiereAttribuableToClasse.Find(d => d.Data.matiereLib == matiere.matiereLib)
												  .Selected = false;
				}
			}
		}

		private void Refresh_UIClasseDetail(Classe classe)
		{
			if (classe.Annee.Matieres.Count != 0)
			{
				Lbl_IsVisible_lstMatiereAttribuable = $"Libellé des Matières Attribuables à la Classe";
				IsVisible_CountAnneeMatiere = true;
			}
			else
			{
				Lbl_IsVisible_lstMatiereAttribuable = $"Il n'y a pas de Matières dans l'Année en cours.";
				IsVisible_CountAnneeMatiere = false;
			}
		}
		#endregion

		#region Eleve Methods
		private void AddEleveToClasse()
		{
			Eleve eleve = new Eleve
			{
				nomEleve = NomEleve,
				prenomEleve = PrenomEleve,
				Classe = Classe
			};

			if (String.IsNullOrEmpty(eleve.nomEleve) || String.IsNullOrWhiteSpace(eleve.prenomEleve))
			{
				DisplayAlert($"Le Nom ou le Prénom de l'Élève ne peuvent pas être vide.");
			}
			else if (eleve.Classe.Eleves.Exists(el => el.nomEleve == eleve.nomEleve
													&& el.prenomEleve == eleve.prenomEleve))
			{
				DisplayAlert($"L'Élève existe déjà pour cette Classe.");
			}
			else
			{
				//Ajout de l'Élève à la liste des Élèves de la Classe
				Classe.Eleves.Add(eleve);
				ListElevesbyClasse.Add(eleve);

				//Reset Eleve Entries
				NomEleve = string.Empty;
				PrenomEleve = string.Empty;
			}

			AffichageListeEleve(Classe);
		}

		private void AffichageListeEleve(Classe classe)
		{
			if (classe.Eleves.Count != 0)
			{
				LabelLstEleve = $"Liste des Élèves de la Classe : {classe.classeLib}";
			}
			else
			{
				LabelLstEleve = $"Il n'y a aucun Élève dans la Classe : {classe.classeLib}";
			}
		}
		#endregion

		#region Devoir Methods
		private void Delete_Devoir_OnDesattribClasseMatiere(Classe classe, Matiere matiere)
		{
			Devoir dev = matiere.Devoirs.Find(d => d.Matiere.matiereLib == matiere.matiereLib);
			Devoir devoir = classe.Devoirs.Find(d => d.Classe.classeLib == classe.classeLib);
			if (dev != null && devoir != null)
			{
				classe.Devoirs.Remove(dev);
				matiere.Devoirs.Remove(dev);

				if (dev.devoirID != 0)
				{
					_rep.Delete_Devoir(dev);
				}
			}
		}
		#endregion
	}
}
