using Prism;
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

				if (SelectClassePicker != null)
				{
					BlocDevoirVisible = true;
					AffichageListeDevoir();
				}
				else
				{
					BlocDevoirVisible = false;
				}
			}
		}

		private bool _isVisible_CountAnneeClasse;
		public bool IsVisible_CountAnneeClasse
		{
			get { return _isVisible_CountAnneeClasse; }
			set { SetProperty(ref _isVisible_CountAnneeClasse, value); }
		}

		private string _lbl_IsVisible_lstClasseAttribuable;
		public string Lbl_IsVisible_lstClasseAttribuable
		{
			get { return _lbl_IsVisible_lstClasseAttribuable; }
			set { SetProperty(ref _lbl_IsVisible_lstClasseAttribuable, value); }
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

		private bool _isVisible_ListDevoirs;
		public bool IsVisible_ListDevoirs
		{
			get { return _isVisible_ListDevoirs; }
			set { SetProperty(ref _isVisible_ListDevoirs, value); }
		}

		private string _lbl_IsVisible_ListDevoirs;
		public string Lbl_IsVisible_ListDevoirs
		{
			get { return _lbl_IsVisible_ListDevoirs; }
			set { SetProperty(ref _lbl_IsVisible_ListDevoirs, value); }
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
		public DelegateCommand AddDevoirCommand => new DelegateCommand(Add_DevoirClasseMatiere);
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
			//RefreshLstClasseAttribuable(Matiere);
			//ListPickClasseMatiere = new ObservableCollection<Classe>(
			//				ClasseAttribuableToMatiere
			//				.Where(d => d.Selected == true)
			//				.Select(d => d.Data));

			Load_Matiere(Matiere);

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

				//ListDevoirs = new ObservableCollection<Devoir>();

				//Refresh_UIMatiereDetail(Matiere);

				//LoadLstClasseAttribuable(Matiere);
			}
		}

		public override void Destroy()
		{

		}
		#endregion

		#region Matiere Methods
		private void Load_Matiere(Matiere matiere)
		{
			Refresh_UIMatiere_ClasseAttribPart(matiere);

			LoadLstClasseAttribuable(matiere);

			Refresh_UIMatiere_DevoirPart(matiere);
		}

		private void Refresh_UIMatiere_ClasseAttribPart(Matiere matiere)
		{
			if (matiere.Annee.Classes.Count != 0)
			{
				Lbl_IsVisible_lstClasseAttribuable = $"Libellé des Classes Attribuables à la Matière";
				IsVisible_CountAnneeClasse = true;
			}
			else
			{
				Lbl_IsVisible_lstClasseAttribuable = $"Il n'y a pas de Classes dans l'Année en cours.";
				IsVisible_CountAnneeClasse = false;
			}


		}

		private void AttribClasseToMatiere()
		{
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
					// Implémenter PageDialog pour SIGNALER la SUPPRESSION
					// des DEVOIRS liés à la DÉSATTRIBUTION en cours

					Matiere.Classes.Remove(data.Data);

					if (classe.Matieres.Exists(ma => ma.matiereLib == Matiere.matiereLib))
					{
						classe.Matieres.Remove(Matiere);

						Delete_Devoir_OnDesattribClasseMatiere(classe, Matiere);
					}
				}
			}
		}

		private void LoadLstClasseAttribuable(Matiere matiere)
		{
			//Load ListClasseAttribuable with Attribuate Classe pre-load
			ClasseAttribuableToMatiere = new List<SelectableData<Classe>>();

			foreach (Classe classe in matiere.Annee.Classes)
			{
				ClasseAttribuableToMatiere.Add(new SelectableData<Classe>() { Data = classe });
			}

			RefreshLstClasseAttribuable(matiere);
		}

		private void RefreshLstClasseAttribuable(Matiere matiere)
		{
			foreach (Classe classe in matiere.Annee.Classes)
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

		private void Delete_Devoir_OnDesattribClasseMatiere(Classe classe, Matiere matiere)
		{
			Devoir dev = matiere.Devoirs.Find(d => d.Matiere.matiereLib == matiere.matiereLib);
			Devoir devoir = classe.Devoirs.Find(d => d.Classe.classeLib == classe.classeLib);
			if (dev != null && devoir != null)
			{
				classe.Devoirs.Remove(devoir);
				matiere.Devoirs.Remove(devoir);

				if (devoir.devoirID != 0)
				{
					_rep.Delete_Devoir(dev);
				}
			}
		}
		#endregion

		#region Devoirs Methods		
		private void Refresh_UIMatiere_DevoirPart(Matiere matiere)
		{
			//ListPickClasseMatiere = new ObservableCollection<Classe>(
			//				ClasseAttribuableToMatiere
			//				.Where(d => d.Selected == true)
			//				.Select(d => d.Data));

			ListPickClasseMatiere = new ObservableCollection<Classe>(Matiere.Classes);

			ListDevoirs = new ObservableCollection<Devoir>();
		}

		private void Add_DevoirClasseMatiere()
		{

			// IMPOSSIBLE DE RENTRER DES CHIFFRES A VIRGULES
			// LES POINTS MARCHENT. VOIR A AMELIORER

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
			else if (!Double.TryParse(DevoirCoeff.ToString(), out double devoirCoeff)
					|| DevoirCoeff <= 0)
			{
				DisplayAlert($"Le Coefficient du Devoir n'est pas valide.");
			}
			else if (!Double.TryParse(DevoirNoteMax.ToString(), out double devoirNoteMax)
					|| DevoirNoteMax <= 0)
			{
				DisplayAlert($"La Note Max du Devoir n'est pas valide.");
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
			ListDevoirs = GetListDevoirs_ClasseMatiere(
								LoadClassePicked(SelectClassePicker),
								Matiere);

			if (ListDevoirs.Count() != 0)
			{
				Lbl_IsVisible_ListDevoirs = $"Liste des Devoirs pour la classe : " +
											$"{SelectClassePicker.classeLib} et " +
											$"la matière : {Matiere.matiereLib}";
				IsVisible_ListDevoirs = true;
			}
			else
			{
				Lbl_IsVisible_ListDevoirs = $"Il n'y a pas de Devoirs pour la classe : " +
											$"{SelectClassePicker.classeLib} et " +
											$"la matière : {Matiere.matiereLib}";
				IsVisible_ListDevoirs = false;
			}
		}

		/// <summary>
		/// Method for Load Classe in Annee.Classes
		/// corresponding to SelectClassePicker 
		/// </summary>
		/// <param name="classe"></param>
		/// <returns>Classe</returns>
		private Classe LoadClassePicked(Classe classe)
		{
			if (classe != null)
			{
				classe = Matiere.Annee.Classes.Find(c => c.classeLib == classe.classeLib);
			}

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
