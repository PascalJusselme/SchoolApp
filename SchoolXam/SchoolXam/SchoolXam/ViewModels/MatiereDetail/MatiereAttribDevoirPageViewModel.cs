using Prism;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using SchoolXam.Data;
using SchoolXam.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace SchoolXam.ViewModels
{
	public class MatiereAttribDevoirPageViewModel : BaseViewModel, IActiveAware
	{
		private readonly IPageDialogService _pageDialogService;

		#region Matiere Properties
		private Matiere _matiere;
		public Matiere Matiere
		{
			get { return _matiere; }
			set { SetProperty(ref _matiere, value); }
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

		public MatiereAttribDevoirPageViewModel(
						IPageDialogService pageDialogService,
						SchoolRepository db)
						: base(db)
		{
			_pageDialogService = pageDialogService;
		}

		#region Devoirs Methods		
		private void Refresh_UIMatiere_DevoirPart(Matiere matiere)
		{
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

			if (ListDevoirs.Count != 0)
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
			Refresh_UIMatiere_DevoirPart(Matiere);
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
			if (parameters != null && parameters.ContainsKey("Matiere"))
			{
				Matiere = parameters["Matiere"] as Matiere;
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
