using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Navigation;
using SchoolXam.Data;
using SchoolXam.Events;
using SchoolXam.Messages;
using SchoolXam.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Xamarin.Forms;

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

		public DelegateCommand<AnneeScolaire> AnneeClicked { get; set; }
		public DelegateCommand AddAnneeCommand { get; set; }

		public AnneeMasterPageViewModel(
					INavigationService navigationService,
					SchoolRepository db)
		: base(db)
		{
			_navigationService = navigationService;

			AnneeClicked = new DelegateCommand<AnneeScolaire>(DoAnneeClicked);
			AddAnneeCommand = new DelegateCommand(AddAnnee);
		}

		private async void DoAnneeClicked(AnneeScolaire annee)
		{
			var parameter = new NavigationParameters();
			parameter.Add("Annee", annee);
			await _navigationService.NavigateAsync("AnneeDetailPage", parameter);
		}

		private void GetAnnees()
		{
			Annees = new ObservableCollection<AnneeScolaire>(_rep.GetAnnees());
		}

		private async void AddAnnee()
		{
			await _navigationService.NavigateAsync("AnneeDetailPage");
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
