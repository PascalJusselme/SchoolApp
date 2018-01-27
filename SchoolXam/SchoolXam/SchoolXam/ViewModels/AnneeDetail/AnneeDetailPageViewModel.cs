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
	public class AnneeDetailPageViewModel : BaseViewModel, INavigationAware
	{
		private readonly INavigationService _navigationService;
		private readonly IPageDialogService _pageDialogService;

		#region Annee Propertie and Command
		private AnneeScolaire _annee;
		public AnneeScolaire Annee
		{
			get { return _annee; }
			set { SetProperty(ref _annee, value); }
		}

		public DelegateCommand SaveAnneeCommand => new DelegateCommand(SaveAnnee);
		#endregion

		#region Classe Properties and Commands
		private ObservableCollection<Classe> _listClasses;
		public ObservableCollection<Classe> ListClasses
		{
			get { return _listClasses; }
			set { SetProperty(ref _listClasses, value); }
		}

		private Classe _classe;
		public Classe Classe
		{
			get { return _classe; }
			set { SetProperty(ref _classe, value); }
		}

		private string _classeLib;
		public string ClasseLib
		{
			get { return _classeLib; }
			set { SetProperty(ref _classeLib, value); }
		}

		private List<SelectableData<Matiere>> _matiereAttribuableToClasse;
		public List<SelectableData<Matiere>> MatiereAttribuableToClasse
		{
			get { return _matiereAttribuableToClasse; }
			set { SetProperty(ref _matiereAttribuableToClasse, value); }
		}

		#region Classe Commands
		public DelegateCommand<string> AddClasseCommand => new DelegateCommand<string>(AddClasse);
		public DelegateCommand<Classe> SelectClasse => new DelegateCommand<Classe>(ClasseSelected);
		public DelegateCommand AttribMatiereToClasseCommand => new DelegateCommand(AttribMatiereToClasse);
		#endregion

		#endregion

		#region Matiere Properties and Commands
		private ObservableCollection<Matiere> _listMatieres;
		public ObservableCollection<Matiere> ListMatieres
		{
			get { return _listMatieres; }
			set { SetProperty(ref _listMatieres, value); }
		}

		private Matiere _matiere;
		public Matiere Matiere
		{
			get { return _matiere; }
			set { SetProperty(ref _matiere, value); }
		}

		private string _matiereLib;
		public string MatiereLib
		{
			get { return _matiereLib; }
			set { SetProperty(ref _matiereLib, value); }
		}

		private List<SelectableData<Classe>> _classeAttribuable;
		public List<SelectableData<Classe>> ClasseAttribuable
		{
			get { return _classeAttribuable; }
			set { SetProperty(ref _classeAttribuable, value); }
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
					ListDevoirs = GetListDevoirs_ClasseMatiere(LoadClasse(_selectClassePicker), Matiere);
				}
				else
				{
					BlocDevoirVisible = false;
				}
			}
		}

		#region Matiere Commands
		public DelegateCommand<string> AddMatiereCommand => new DelegateCommand<string>(AddMatiere);
		public DelegateCommand<Matiere> SelectMatiere => new DelegateCommand<Matiere>(MatiereSelected);
		public DelegateCommand AttribClasseToMatiereCommand => new DelegateCommand(AttribClasseToMatiere);
		#endregion

		#endregion

		#region Eleve Properties and Commands
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

		#region Eleve Commands
		public DelegateCommand AddEleveToClasseCommand => new DelegateCommand(AddEleveToClasse);
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

		public AnneeDetailPageViewModel(
			INavigationService navigationService,
			IPageDialogService pageDialogService,
			SchoolRepository db)
			: base(db)
		{
			ListDevoirs = new ObservableCollection<Devoir>();

			_navigationService = navigationService;

			_pageDialogService = pageDialogService;
		}

		#region PageDialogService Propertie and Method
		public DelegateCommand<string> DisplayAlertCommand => new DelegateCommand<string>(DisplayAlert);

		private async void DisplayAlert(string message)
		{
			await _pageDialogService.DisplayAlertAsync("Alert", message, "Accept", "Cancel");
		}
		#endregion

		#region NavigationService Methods
		public override void OnNavigatedFrom(NavigationParameters parameters)
		{

		}

		public override void OnNavigatedTo(NavigationParameters parameters)
		{

		}

		public override void OnNavigatingTo(NavigationParameters parameters)
		{
			if (parameters != null && parameters.ContainsKey("Annee"))
			{
				Annee = parameters["Annee"] as AnneeScolaire;

				Annee = LoadAnnee(Annee);

				Title = Annee.anneeLib;
			}

			if (parameters != null && parameters.ContainsKey("Classe"))
			{
				Classe = parameters["Classe"] as Classe;

				Classe = GetClasse(Classe);

				Title = Classe.classeLib;
			}

			if (parameters != null && parameters.ContainsKey("Matiere"))
			{
				Matiere = parameters["Matiere"] as Matiere;

				Matiere = GetMatiere(Matiere);

				Title = Matiere.matiereLib;
			}
		}

		public override void Destroy()
		{

		}
		#endregion

		#region AnneeScolaire
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

			ListClasses = new ObservableCollection<Classe>(annee.Classes);
			ListMatieres = new ObservableCollection<Matiere>(annee.Matieres);

			return annee;
		}

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
		#endregion

		#region Classe
		private Classe LoadClasse(Classe classe)
		{
			Classe cl = Annee.Classes.Find(cla => cla.classeLib == classe.classeLib);

			return cl;
		}

		private Classe GetClasse(Classe classe)
		{
			Annee = classe.Annee;

			classe = Annee.Classes.Find(cl => cl.classeLib == classe.classeLib);

			LoadLstMatiereAttribuable(classe);

			ListElevesbyClasse = new ObservableCollection<Eleve>(classe.Eleves);

			AffichageListeEleve(classe);

			return classe;
		}

		private void AddClasse(string classeLib)
		{
			Classe classe = new Classe
			{
				classeLib = classeLib,
				Annee = Annee,
				Matieres = new List<Matiere>(),
				Eleves = new List<Eleve>(),
				Devoirs = new List<Devoir>()
			};

			if (String.IsNullOrEmpty(classe.classeLib) || String.IsNullOrWhiteSpace(classe.classeLib))
			{
				DisplayAlert($"Le Libellé de la Classe ne peut pas être vide.");
			}
			else if (classe.Annee.Classes.Exists(cl => cl.classeLib == classe.classeLib))
			{
				DisplayAlert($"Le Libellé de la Classe existe déjà pour cette Année Scolaire.");
			}
			else
			{
				//Ajout de la Classe a la liste des Classes de l'Annee
				Annee.Classes.Add(classe);
				ListClasses.Add(classe);
				//Reset ClasseLib Entry
				ClasseLib = string.Empty;
			}
		}

		private void AttribMatiereToClasse()
		{
			Classe.Matieres.Clear();

			foreach (var data in MatiereAttribuableToClasse)
			{
				Matiere matiere = data.Data;

				if (data.Selected)
				{
					Classe.Matieres.Add(data.Data);

					if (matiere.Classes.Find(cl => cl.classeLib == Classe.classeLib) == null)
					{
						matiere.Classes.Add(Classe);
					}
				}
				else
				{
					if (matiere.Classes.Find(cl => cl.classeLib == Classe.classeLib) != null)
					{
						matiere.Classes.Remove(Classe);
					}
				}
			}
		}

		private void LoadLstMatiereAttribuable(Classe classe)
		{
			MatiereAttribuableToClasse = new List<SelectableData<Matiere>>();

			foreach (Matiere mat in Annee.Matieres)
			{
				MatiereAttribuableToClasse.Add(new SelectableData<Matiere>() { Data = mat });
			}

			if (classe.Matieres.Count != 0)
			{
				foreach (Matiere ma in classe.Matieres)
				{
					MatiereAttribuableToClasse.Find(d => d.Data.matiereLib == ma.matiereLib)
									  .Selected = true;
				}
			}
		}

		private async void ClasseSelected(Classe classe)
		{
			var parameter = new NavigationParameters
			{
				{ "Classe", classe }
			};

			await _navigationService.NavigateAsync("ClasseDetailPage", parameter);
		}
		#endregion

		#region Matiere
		private Matiere LoadMatiere(Matiere matiere)
		{
			Matiere ma = Annee.Matieres.Find(m => m.matiereLib == matiere.matiereLib);

			return ma;
		}

		private Matiere GetMatiere(Matiere matiere)
		{
			Annee = matiere.Annee;

			matiere = Annee.Matieres.Find(ma => ma.matiereLib == matiere.matiereLib);

			LoadLstClasseAttribuable(matiere);

			ListPickClasseMatiere = new ObservableCollection<Classe>(Matiere.Classes);

			return matiere;
		}

		private void AddMatiere(string matiereLib)
		{
			Matiere matiere = new Matiere
			{
				matiereLib = matiereLib,
				Annee = Annee,
				Classes = new List<Classe>(),
				Devoirs = new List<Devoir>()
			};

			if (String.IsNullOrEmpty(matiere.matiereLib) || String.IsNullOrWhiteSpace(matiere.matiereLib))
			{
				DisplayAlert($"Le Libellé de la Matière ne peut pas être vide.");
			}
			else if (matiere.Annee.Matieres.Exists(ma => ma.matiereLib == matiere.matiereLib))
			{
				DisplayAlert($"Le Libellé de la Matière existe déjà pour cette Année Scolaire.");
			}
			else
			{
				//Ajout de la Matiere a la liste des Matieres de l'Annee
				Annee.Matieres.Add(matiere);
				ListMatieres.Add(matiere);
				//Reset MatiereLib Entry
				MatiereLib = string.Empty;
			}
		}

		private void AttribClasseToMatiere()
		{
			Matiere.Classes.Clear();

			foreach (var data in ClasseAttribuable)
			{
				Classe classe = data.Data;

				if (data.Selected)
				{
					Matiere.Classes.Add(data.Data);

					if (!classe.Matieres.Contains(Matiere))
					{
						classe.Matieres.Add(Matiere);
					}
				}
				else
				{
					if (classe.Matieres.Contains(Matiere))
					{
						classe.Matieres.Remove(Matiere);
					}
				}
			}
		}

		private void LoadLstClasseAttribuable(Matiere matiere)
		{
			ClasseAttribuable = new List<SelectableData<Classe>>();

			foreach (Classe cla in Annee.Classes)
			{
				ClasseAttribuable.Add(new SelectableData<Classe>() { Data = cla });
			}

			if (matiere.Classes.Count != 0)
			{
				foreach (Classe cl in matiere.Classes)
				{
					ClasseAttribuable.Find(d => d.Data.classeLib == cl.classeLib)
									 .Selected = true;
				}
			}
		}

		private async void MatiereSelected(Matiere matiere)
		{
			var parameter = new NavigationParameters
			{
				{ "Matiere", matiere }
			};

			await _navigationService.NavigateAsync("MatiereDetailPage", parameter);
		}
		#endregion

		#region Eleve
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

		#region Devoir
		private void AddDevoirClasseMatiere()
		{
			Devoir devoir = new Devoir
			{
				devoirLib = DevoirLib,
				devoirCoeff = DevoirCoeff,
				devoirNoteMax = DevoirNoteMax,
				Classe = LoadClasse(SelectClassePicker),
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
