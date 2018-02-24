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
	public class GestionClassePageViewModel : BaseViewModel, IActiveAware
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

		#region Classe Properties and Commands

		#region Classe Properties
		private ObservableCollection<Classe> _listClasses;
		public ObservableCollection<Classe> ListClasses
		{
			get { return _listClasses; }
			set { SetProperty(ref _listClasses, value); }
		}
		#endregion

		#region Classe Commands
		public DelegateCommand AddClasseCommand => new DelegateCommand(AddClasse);
		public DelegateCommand<Classe> SelectClasseCommand => new DelegateCommand<Classe>(SelectClasse);
		#endregion

		#endregion

		public GestionClassePageViewModel(
						INavigationService navigationService,
						IPageDialogService pageDialogService,
						SchoolRepository db)
						: base(db)
		{
			_navigationService = navigationService;

			_pageDialogService = pageDialogService;
		}

		#region Classe Methods
		private async void AddClasse()
		{
			Classe classe = new Classe(Annee);

			var parameter = new NavigationParameters();
			parameter.Add("Classe", classe);

			await _navigationService.NavigateAsync("ClasseDetailPage", parameter);			
		}

		private async void SelectClasse(Classe classe)
		{
			var parameter = new NavigationParameters();
			parameter.Add("Classe", classe);
			parameter.Add("IsSelectedClasse", true);

			await _navigationService.NavigateAsync("ClasseDetailPage", parameter);
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
			if (IsActive)
			{
				ListClasses = new ObservableCollection<Classe>(Annee.Classes);
			}
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
		public DelegateCommand DisplayActionSheetCommand => new DelegateCommand(DisplayActionSheet);
		public DelegateCommand DisplayActionSheetUsingActionSheetButtonsCommand =>
							new DelegateCommand(DisplayActionSheetUsingActionSheetButtons);

		private async void DisplayAlert(string message)
		{
			await _pageDialogService.DisplayAlertAsync("Alert", message, "Accept", "Cancel");
		}

		private async void DisplayActionSheet()
		{
			await _pageDialogService.DisplayActionSheetAsync("ActionSheet", "Cancel", "Destroy", "Option 1", "Option 2");
		}

		private void DisplayActionSheetUsingActionSheetButtons()
		{
			//IActionSheetButton option1Action =
			//ActionSheetButton.CreateButton("Option 1", new DelegateCommand(() => { Debug.WriteLine("Option 1"); }));
			//IActionSheetButton option2Action =
			//ActionSheetButton.CreateButton("Option 2", new DelegateCommand(() => { Debug.WriteLine("Option 2"); }));
			//IActionSheetButton cancelAction =
			//ActionSheetButton.CreateCancelButton("Cancel", new DelegateCommand(() => { Debug.WriteLine("Cancel"); }));
			//IActionSheetButton destroyAction =
			//ActionSheetButton.CreateDestroyButton("Destroy", new DelegateCommand(() => { Debug.WriteLine("Destroy"); }));

			//await _pageDialogService.DisplayActionSheetAsync("ActionSheet with ActionSheetButtons", option1Action, option2Action, cancelAction, destroyAction);
		}
		#endregion
	}
}
