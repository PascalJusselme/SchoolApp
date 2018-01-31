﻿using Prism.Commands;
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

		#region Classe Properties and Commands

		#region Classe Properties
		private ObservableCollection<Classe> _listClasses;
		public ObservableCollection<Classe> ListClasses
		{
			get { return _listClasses; }
			set { SetProperty(ref _listClasses, value); }
		}

		private string _classeLib;
		public string ClasseLib
		{
			get { return _classeLib; }
			set { SetProperty(ref _classeLib, value); }
		}
		#endregion

		#region Classe Commands
		public DelegateCommand<string> AddClasseCommand => new DelegateCommand<string>(AddClasse);
		public DelegateCommand<Classe> SelectClasseCommand => new DelegateCommand<Classe>(SelectClasse);
		#endregion

		#endregion

		#region Matiere Properties and Commands

		#region Matiere Properties
		private ObservableCollection<Matiere> _listMatieres;
		public ObservableCollection<Matiere> ListMatieres
		{
			get { return _listMatieres; }
			set { SetProperty(ref _listMatieres, value); }
		}

		private string _matiereLib;
		public string MatiereLib
		{
			get { return _matiereLib; }
			set { SetProperty(ref _matiereLib, value); }
		}
		#endregion

		#region Matiere Commands
		public DelegateCommand<string> AddMatiereCommand => new DelegateCommand<string>(AddMatiere);
		public DelegateCommand<Matiere> SelectMatiereCommand => new DelegateCommand<Matiere>(SelectMatiere);
		#endregion

		#endregion

		#region IPageDialogService Propertie and Method
		public DelegateCommand<string> DisplayAlertCommand => new DelegateCommand<string>(DisplayAlert);

		private async void DisplayAlert(string message)
		{
			await _pageDialogService.DisplayAlertAsync("Alert", message, "Accept", "Cancel");
		}
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

		#region INavigationService Implementation
		public override void OnNavigatedFrom(NavigationParameters parameters)
		{
			if (parameters != null && parameters.ContainsKey("Annee"))
			{
				StartedAnnee = parameters["Annee"] as AnneeScolaire;
			}
		}

		public override void OnNavigatedTo(NavigationParameters parameters)
		{
			
		}

		public override void OnNavigatingTo(NavigationParameters parameters)
		{
			if (parameters != null && parameters.ContainsKey("Annee"))
			{
				Annee = parameters["Annee"] as AnneeScolaire;

				ListClasses = new ObservableCollection<Classe>(Annee.Classes);
				ListMatieres = new ObservableCollection<Matiere>(Annee.Matieres);

				Title = Annee.anneeLib;
			}
		}

		public override void Destroy()
		{
			if (StartedAnnee != null)
			{
				_rep.SaveAnnee(StartedAnnee);
			}
		}
		#endregion

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
		#endregion

		#region Classe Methods
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

		private async void SelectClasse(Classe classe)
		{
			var parameter = new NavigationParameters
			{
				{ "Classe", classe }
			};

			await _navigationService.NavigateAsync("ClasseDetailPage", parameter);
		}
		#endregion

		#region Matiere Methods
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

		private async void SelectMatiere(Matiere matiere)
		{
			var parameter = new NavigationParameters
			{
				{ "Matiere", matiere }
			};

			await _navigationService.NavigateAsync("MatiereDetailPage", parameter);
		}
		#endregion
	}
}
