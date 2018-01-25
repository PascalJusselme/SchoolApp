using Prism.Commands;
using Prism.Navigation;
using SchoolXam.Data;
using SchoolXam.Models;
using System;
using System.Collections.ObjectModel;

namespace SchoolXam.ViewModels
{
	public class AnneeMasterPageViewModel : BaseViewModel
	{
		private readonly INavigationService _navigationService;

		private ObservableCollection<AnneeScolaire> _annees;
		public ObservableCollection<AnneeScolaire> Annees
		{
			get { return _annees; }
			set { SetProperty(ref _annees, value); }
		}

		private DelegateCommand<AnneeScolaire> _selectAnnee;
		public DelegateCommand<AnneeScolaire> SelectAnnee => _selectAnnee ?? (_selectAnnee = new DelegateCommand<AnneeScolaire>(AnneeSelected));
		
		private DelegateCommand _addAnneeCommand;
		public DelegateCommand AddAnneeCommand => _addAnneeCommand ?? (_addAnneeCommand = new DelegateCommand(AddAnnee));

		private string _labetLstAnnees;
		public string LabetLstAnnees
		{
			get { return _labetLstAnnees; }
			set { SetProperty(ref _labetLstAnnees, value); }
		}

		public AnneeMasterPageViewModel(
					INavigationService navigationService,
					SchoolRepository db)
		: base(db)
		{
			_navigationService = navigationService;
		}

		private async void AnneeSelected(AnneeScolaire annee)
		{
			var parameter = new NavigationParameters
			{
				{ "Annee", annee }
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

		private async void AddAnnee()
		{
			var parameter = new NavigationParameters
			{
				{ "Annee", new AnneeScolaire() }
			};

			await _navigationService.NavigateAsync("AnneeDetailPage", parameter);
		}

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

	}
}
