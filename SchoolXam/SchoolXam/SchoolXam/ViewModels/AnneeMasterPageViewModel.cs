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
		#endregion
	}
}
