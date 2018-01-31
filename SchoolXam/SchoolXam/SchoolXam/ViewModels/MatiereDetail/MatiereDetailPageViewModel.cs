﻿using Prism;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using SchoolXam.Data;
using SchoolXam.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace SchoolXam.ViewModels
{
	public class MatiereDetailPageViewModel : BaseViewModel, IActiveAware
	{
		private INavigationService _navigationService;
		private IPageDialogService _pageDialogService;

		private AnneeScolaire _annee;
		public AnneeScolaire Annee
		{
			get { return _annee; }
			set { SetProperty(ref _annee, value); }
		}

		#region Matiere Properties and Commands

		#region Matiere Properties
		private Matiere _matiere;
		public Matiere Matiere
		{
			get { return _matiere; }
			set { SetProperty(ref _matiere, value); }
		}

		private List<SelectableData<Classe>> _classeAttribuableToMatiere;
		public List<SelectableData<Classe>> ClasseAttribuableToMatiere
		{
			get { return _classeAttribuableToMatiere; }
			set { SetProperty(ref _classeAttribuableToMatiere, value); }
		}

		private ObservableCollection<Classe> _listPickClasseMatiere;
		public ObservableCollection<Classe> ListPickClasseMatiere
		{
			get { return _listPickClasseMatiere; }
			set { SetProperty(ref _listPickClasseMatiere, value); }
		}

		private Classe _selectClassePicker;
		public Classe SelectClassePicker
		{
			get { return _selectClassePicker; }
			set
			{
				SetProperty(ref _selectClassePicker, value);

				if (_selectClassePicker != null)
				{
					BlocDevoirVisible = true;
					ListDevoirs = GetListDevoirs_ClasseMatiere(LoadClassePicked(_selectClassePicker), Matiere);
				}
				else
				{
					BlocDevoirVisible = false;
				}
			}
		}
		#endregion

		#region Matiere Commands
		public DelegateCommand AttribClasseToMatiereCommand => new DelegateCommand(AttribClasseToMatiere);
		#endregion

		#endregion

		#region Devoir Properties and Commands
		private ObservableCollection<Devoir> _listDevoirs;
		public ObservableCollection<Devoir> ListDevoirs
		{
			get { return _listDevoirs; }
			set { SetProperty(ref _listDevoirs, value); }
		}

		private bool _blocDevoirVisible;
		public bool BlocDevoirVisible
		{
			get { return _blocDevoirVisible; }
			set { SetProperty(ref _blocDevoirVisible, value); }
		}

		private string _devoirLib;
		public string DevoirLib
		{
			get { return _devoirLib; }
			set { SetProperty(ref _devoirLib, value); }
		}

		private double _devoirCoeff;
		public double DevoirCoeff
		{
			get { return _devoirCoeff; }
			set { SetProperty(ref _devoirCoeff, value); }
		}

		private double _devoirNoteMax;
		public double DevoirNoteMax
		{
			get { return _devoirNoteMax; }
			set { SetProperty(ref _devoirNoteMax, value); }
		}

		#region Devoir Commands
		public DelegateCommand AddDevoirCommand => new DelegateCommand(AddDevoirClasseMatiere);
		#endregion

		#endregion

		#region IPageDialogService Propertie and Method
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
			ListDevoirs = new ObservableCollection<Devoir>();
			RefreshLstClasseAttribuable(Matiere);
			ListPickClasseMatiere = new ObservableCollection<Classe>(
							ClasseAttribuableToMatiere.Where(d => d.Selected == true).Select(d => d.Data));
		}

		#endregion

		public MatiereDetailPageViewModel(
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
			if (parameters != null && parameters.ContainsKey("Matiere"))
			{
				Matiere = parameters["Matiere"] as Matiere;

				Annee = Matiere.Annee;

				LoadLstClasseAttribuable(Matiere);
			}
		}

		public override void Destroy()
		{

		}
		#endregion

		#region Matiere Methods
		private void AttribClasseToMatiere()
		{
			// Penser à supprimer les DEVOIRS correspondants 
			// à la DÉSATTRIBUTION d'une CLASSE


			Matiere.Classes.Clear();

			foreach (var data in ClasseAttribuableToMatiere)
			{
				Classe classe = data.Data;

				if (data.Selected)
				{
					Matiere.Classes.Add(data.Data);

					if (!classe.Matieres.Exists(ma => ma.matiereLib == Matiere.matiereLib))
					{
						classe.Matieres.Add(Matiere);
					}
				}
				else
				{
					Matiere.Classes.Remove(data.Data);

					if (classe.Matieres.Exists(ma => ma.matiereLib == Matiere.matiereLib))
					{
						classe.Matieres.Remove(Matiere);
					}
				}

				//Use for ManyToMany RelationShip on Classe_Matiere
				if (classe.classeID != 0 && Matiere.matiereID != 0)
				{
					_rep.UpdateMatiere(Matiere);
					_rep.UpdateClasse(classe);
				}

				// Delete devoirs
				//List<Devoir> lstToDelete =
				//				Matiere.Devoirs
				//					   .FindAll(d => d.Classe.classeLib == classe.classeLib
				//								  && d.Matiere.matiereLib == Matiere.matiereLib);
				//foreach (Devoir devoir in lstToDelete)
				//{

				//}

				//Matiere.Devoirs.Remove(devoir);
				//classe.Devoirs.Remove(devoir);
				//if (devoir.devoirID != 0)
				//{
				//	_rep.DeleteDevoir(devoir);
				//}
			}
		}

		private void LoadLstClasseAttribuable(Matiere matiere)
		{
			//Load ListClasseAttribuable with Attribuate Classe pre-load
			ClasseAttribuableToMatiere = new List<SelectableData<Classe>>();

			foreach (Classe classe in Annee.Classes)
			{
				ClasseAttribuableToMatiere.Add(new SelectableData<Classe>() { Data = classe });
			}
		}

		private void RefreshLstClasseAttribuable(Matiere matiere)
		{
			foreach (Classe classe in Annee.Classes)
			{
				if (classe.Matieres
						  .Exists(m => m.matiereLib == matiere.matiereLib)
					&& matiere.Classes
							  .Exists(c => c.classeLib == classe.classeLib))
				{
					ClasseAttribuableToMatiere.Find(d => d.Data.classeLib == classe.classeLib)
											  .Selected = true;
				}
				else
				{
					ClasseAttribuableToMatiere.Find(d => d.Data.classeLib == classe.classeLib)
												  .Selected = false;
				}
			}
		}
		#endregion

		#region Devoirs Methods
		private void AddDevoirClasseMatiere()
		{
			Devoir devoir = new Devoir
			{
				devoirLib = DevoirLib,
				devoirCoeff = DevoirCoeff,
				devoirNoteMax = DevoirNoteMax,
				Classe = LoadClassePicked(SelectClassePicker),
				Matiere = Matiere
			};

			if (String.IsNullOrEmpty(devoir.devoirLib) || String.IsNullOrWhiteSpace(devoir.devoirLib))
			{
				DisplayAlert($"Le Libellé du Devoir ne peut pas être vide.");
			}
			else if (devoir.Classe.Devoirs.Exists(de => de.devoirLib == devoir.devoirLib))
			{
				DisplayAlert($"Le Libellé du Devoir existe déjà pour la Classe.");
			}
			else if (devoir.Matiere.Devoirs.Exists(dev => dev.devoirLib == devoir.devoirLib))
			{
				DisplayAlert($"Le Libellé du Devoir existe déjà pour la Matière.");
			}
			else
			{
				//Ajout du Devoir aux Listes de Devoir de la Classe et de la Matière sélectionnées
				devoir.Classe.Devoirs.Add(devoir);
				Matiere.Devoirs.Add(devoir);
				ListDevoirs.Add(devoir);

				//Reset Devoir Entries
				DevoirLib = string.Empty;
				DevoirCoeff = 0;
				DevoirNoteMax = 0;
			}

			AffichageListeDevoir();
		}

		private void AffichageListeDevoir()
		{

		}

		private Classe LoadClassePicked(Classe classe)
		{
			classe = Annee.Classes.Find(c => c.classeLib == classe.classeLib);

			return classe;
		}

		private ObservableCollection<Devoir> GetListDevoirs_ClasseMatiere(Classe classe, Matiere matiere)
		{
			List<Devoir> lstDevoir = new List<Devoir>();

			foreach (Devoir devoir in matiere.Devoirs)
			{
				foreach (Devoir dev in classe.Devoirs)
				{
					if (dev.devoirLib == devoir.devoirLib)
					{
						lstDevoir.Add(devoir);
					}
				}
			}

			return new ObservableCollection<Devoir>(lstDevoir);
		}
		#endregion
	}
}