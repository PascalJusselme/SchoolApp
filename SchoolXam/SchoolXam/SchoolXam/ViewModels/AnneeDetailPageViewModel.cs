using Prism.Commands;
using Prism.Events;
using Prism.Navigation;
using Prism.Services;
using SchoolXam.Data;
using SchoolXam.Events;
using SchoolXam.Messages;
using SchoolXam.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;

namespace SchoolXam.ViewModels
{
	public class AnneeDetailPageViewModel : BaseViewModel, INavigationAware
	{
		private readonly INavigationService _navigationService;
		private readonly IEventAggregator _eventAggregator;

		#region Annee Propertie and Command
		private AnneeScolaire _annee;
		public AnneeScolaire Annee
		{
			get { return _annee; }
			set { SetProperty(ref _annee, value); }
		}

		public DelegateCommand SaveAnneeCommand { get; set; }
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

		private List<SelectableData<Matiere>> _matiereAttribuable;
		public List<SelectableData<Matiere>> MatiereAttribuable
		{
			get { return _matiereAttribuable; }
			set { SetProperty(ref _matiereAttribuable, value); }
		}

		#region Classe Commands
		public DelegateCommand<string> AddClasseCommand { get; set; }
		public DelegateCommand<Classe> ClasseItemClicked { get; }
		public DelegateCommand ClasseAttribMatiereCommand { get; }
		public DelegateCommand ClasseAttribEleveCommand { get; }
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
		
		#region Matiere Commands
		public DelegateCommand<string> AddMatiereCommand { get; set; }
		public DelegateCommand<Matiere> MatiereItemClicked { get; }
		public DelegateCommand MatiereAttribClasseCommand { get; }
		#endregion

		#endregion

		#region EleveProperties
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
		#endregion

		public AnneeDetailPageViewModel(
			INavigationService navigationService,
			IEventAggregator eventAggregator,
			SchoolRepository db)
			: base(db)
		{
			_navigationService = navigationService;

			_eventAggregator = eventAggregator;

			MatiereAttribuable = new List<SelectableData<Matiere>>();
			ClasseAttribuable = new List<SelectableData<Classe>>();

			SaveAnneeCommand = new DelegateCommand(SaveAnnee).ObservesProperty(() => Annee)
															 .ObservesProperty(() => Classe)
															 .ObservesProperty(() => Matiere);
			#region ClasseCommand
			ClasseItemClicked = new DelegateCommand<Classe>(DoClasseClicked);
			AddClasseCommand = new DelegateCommand<string>(AddClasse);
			ClasseAttribMatiereCommand = new DelegateCommand(AttribClasseMatiere);
			ClasseAttribEleveCommand = new DelegateCommand(AddEleveClasse);
			#endregion

			#region MatiereCommand
			MatiereItemClicked = new DelegateCommand<Matiere>(DoMatiereClicked);
			AddMatiereCommand = new DelegateCommand<string>(AddMatiere);
			MatiereAttribClasseCommand = new DelegateCommand(AttribMatiereClasse);
			#endregion
		}

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

				Annee = GetAnnee(Annee);

				Title = Annee.anneeLib;
			}

			if (parameters != null && parameters.ContainsKey("Classe"))
			{
				Classe = parameters["Classe"] as Classe;

				Classe = GetClasse(Classe);
			}

			if (parameters != null && parameters.ContainsKey("Matiere"))
			{
				Matiere = parameters["Matiere"] as Matiere;

				Matiere = GetMatiere(Matiere);
			}

			if (parameters.Count == 0)
			{
				Annee = GetNewAnnee();

				Title = "Nouvelle Année";
			}
		}
		
		public override void Destroy()
		{

		}

		#region AnneeScolaire
		private AnneeScolaire GetAnnee(AnneeScolaire Annee)
		{
			AnneeScolaire annee = new AnneeScolaire();

			annee = _rep.GetAnnee(Annee);

			annee.Classes = _rep.GetClassesByAnnee(Annee);
			annee.Matieres = _rep.GetMatieresByAnnee(Annee);

			ListClasses = new ObservableCollection<Classe>(annee.Classes);
			ListMatieres = new ObservableCollection<Matiere>(annee.Matieres);

			foreach (Classe classe in annee.Classes)
			{
				Classe cl = new Classe();

				cl = _rep.GetClasse(classe);

				classe.Matieres = new List<Matiere>();

				classe.Eleves = new ObservableCollection<Eleve>(_rep.GetEleveByClasse(classe));

				classe.Matieres = cl.Matieres;

				classe.Annee = annee;
			}

			foreach (Matiere matiere in annee.Matieres)
			{
				Matiere ma = new Matiere();

				ma = _rep.GetMatiere(matiere);

				matiere.Classes = new List<Classe>();

				matiere.Classes = ma.Classes;

				matiere.Annee = annee;
			}

			return annee;
		}

		private AnneeScolaire GetNewAnnee()
		{
			AnneeScolaire annee = new AnneeScolaire();

			annee.Classes = new List<Classe>();
			annee.Matieres = new List<Matiere>();

			ListClasses = new ObservableCollection<Classe>(annee.Classes);
			ListMatieres = new ObservableCollection<Matiere>(annee.Matieres);

			foreach (Classe classe in annee.Classes)
			{
				classe.Eleves = new ObservableCollection<Eleve>();
				classe.Devoirs = new List<Devoir>();
			}

			foreach (Matiere matiere in annee.Matieres)
			{
				matiere.Classes = new List<Classe>();
				matiere.Devoirs = new ObservableCollection<Devoir>();
			}

			return annee;
		}

		private async void SaveAnnee()
		{
			_rep.SaveAnnee(Annee);

			await _navigationService.GoBackToRootAsync();
		}
		#endregion

		#region Classe
		private Classe GetClasse(Classe classe)
		{
			MatiereAttribuable.Clear();

			Annee = classe.Annee;

			classe = Annee.Classes.Find(cl => cl.classeLib == classe.classeLib);

			foreach (Matiere mat in Annee.Matieres)
			{
				MatiereAttribuable.Add(new SelectableData<Matiere>() { Data = mat });
			}

			if (classe.Matieres.Count != 0)
			{
				foreach (Matiere ma in classe.Matieres)
				{
					MatiereAttribuable.Find(d => d.Data.matiereLib == ma.matiereLib)
									  .Selected = true;
				}
			}

			return classe;
		}

		private void AddClasse(string classeLib)
		{
			Classe classe = new Classe
			{
				classeLib = classeLib,
				Annee = Annee,
				Matieres = new List<Matiere>(),
				Eleves = new ObservableCollection<Eleve>()
			};
			
			if (IsValidClasse(classe))
			{
				// Ajout de la Classe a la liste des Classes de l'Annee
				Annee.Classes.Add(classe);
				ListClasses.Add(classe);
			}
		}

		private bool IsValidClasse(Classe classe)
		{
			return (!String.IsNullOrEmpty(classe.classeLib)
						&& !classe.Annee
								  .Classes
								  .Exists(cl => cl.classeLib == classe.classeLib));
		}

		private void AttribClasseMatiere()
		{
			Classe.Matieres.Clear();

			foreach (var data in MatiereAttribuable)
			{
				Matiere matiere = data.Data;

				if (data.Selected)
				{
					Classe.Matieres.Add(data.Data);

					if (!matiere.Classes.Contains(Classe))
					{
						matiere.Classes.Add(Classe);
					}
				}
				else
				{
					if (matiere.Classes.Contains(Classe))
					{
						matiere.Classes.Remove(Classe);
					}
				}

				if (matiere.matiereID != 0)
				{
					_rep.UpdateMatiere(matiere);
				}
			}

			if (Classe.classeID != 0)
			{
				_rep.UpdateClasse(Classe);
			}
		}

		private async void DoClasseClicked(Classe classe)
		{
			var parameter = new NavigationParameters();
			parameter.Add("Classe", classe);
			await _navigationService.NavigateAsync("ClasseDetailPage", parameter);
		}
		#endregion

		#region Matiere
		private Matiere GetMatiere(Matiere matiere)
		{
			ClasseAttribuable.Clear();

			Annee = matiere.Annee;

			matiere = Annee.Matieres.Find(ma => ma.matiereLib == matiere.matiereLib);

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

			return matiere;
		}

		private void AddMatiere(string matiereLib)
		{
			Matiere matiere = new Matiere
			{
				matiereLib = matiereLib,
				Annee = Annee,
				Classes = new List<Classe>()
			};

			if (IsValidMatiere(matiere))
			{
				// Ajout de la Matiere a la liste des Matieres de l'Annee
				Annee.Matieres.Add(matiere);
				ListMatieres.Add(matiere);
			}
		}

		private bool IsValidMatiere(Matiere matiere)
		{
			return (!String.IsNullOrEmpty(matiere.matiereLib)
						&& !matiere.Annee
								   .Matieres
								   .Exists(ma => ma.matiereLib == matiere.matiereLib));
		}

		private void AttribMatiereClasse()
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

				if (classe.classeID != 0)
				{
					_rep.UpdateClasse(classe);
				}
			}

			if (Matiere.matiereID != 0)
			{
				_rep.UpdateMatiere(Matiere);
			}
		}

		private async void DoMatiereClicked(Matiere matiere)
		{
			var parameter = new NavigationParameters();
			parameter.Add("Matiere", matiere);
			await _navigationService.NavigateAsync("MatiereDetailPage", parameter);
		}
		#endregion

		#region Eleve
		private void AddEleveClasse()
		{
			Eleve el = new Eleve
			{
				nomEleve = NomEleve,
				prenomEleve = PrenomEleve,
				Classe = Classe
			};

			//Ajout de l'Eleve a la Classe en cours de Gestion
			Classe.Eleves.Add(el);
		}
		#endregion

		#region Devoir

		#endregion
	}
}
