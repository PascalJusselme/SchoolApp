﻿using Prism;
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
	public class GestionMatierePageViewModel : BaseViewModel, IActiveAware
	{
		private readonly INavigationService _navigationService;
		private readonly IPageDialogService _pageDialogService;

		#region Annee Properties
		private AnneeScolaire _annee;
		public AnneeScolaire Annee
		{
			get { return _annee; }
			set { SetProperty(ref _annee, value); }
		}
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
		public DelegateCommand AddMatiereCommand => new DelegateCommand(AddMatiere);
		public DelegateCommand<Matiere> SelectMatiereCommand => new DelegateCommand<Matiere>(SelectMatiere);
		#endregion

		#endregion

		public GestionMatierePageViewModel(
						INavigationService navigationService,
						IPageDialogService pageDialogService,
						SchoolRepository db)
						: base(db)
		{
			_navigationService = navigationService;

			_pageDialogService = pageDialogService;
		}

		#region Matiere Methods
		private async void AddMatiere()
		{
			Matiere matiere = new Matiere(Annee);

			var parameter = new NavigationParameters
			{
				{ "Matiere", matiere }
			};

			await _navigationService.NavigateAsync("MatiereDetailPage", parameter);
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
			ListMatieres = new ObservableCollection<Matiere>(Annee.Matieres);
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
			if (parameters != null && parameters.ContainsKey("Annee"))
			{
				Annee = parameters["Annee"] as AnneeScolaire;
			}
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
