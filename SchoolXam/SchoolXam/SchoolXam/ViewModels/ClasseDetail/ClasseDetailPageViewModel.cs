using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Prism;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using SchoolXam.Data;
using SchoolXam.Models;
using Xamarin.Forms;

namespace SchoolXam.ViewModels
{
	public class ClasseDetailPageViewModel : BaseViewModel
	{
		private readonly INavigationService _navigationService;
		private readonly IPageDialogService _pageDialogService;

		#region Classe Properties and Commands

		#region Classe Properties
		private Classe _classe;
		public Classe Classe
		{
			get { return _classe; }
			set { SetProperty(ref _classe, value); }
		}

		private bool _isSelectedClasse = false;
		public bool IsSelectedClasse
		{
			get { return _isSelectedClasse; }
			set { SetProperty(ref _isSelectedClasse, value); }
		}
		#endregion

		#region Classe Commands
		public DelegateCommand SaveClasseCommand => new DelegateCommand(SaveClasse);
		public DelegateCommand ExitClasseCommand => new DelegateCommand(ExitClasseButton);
		#endregion

		#endregion

		public ClasseDetailPageViewModel(
								INavigationService navigationService,
								IPageDialogService pageDialogService,
								SchoolRepository db) : base(db)
		{
			_navigationService = navigationService;

			_pageDialogService = pageDialogService;
		}

		#region Classe Methods
		private void SaveClasse()
		{
			if (IsValid_Classe())
			{
				AnneeScolaire an = Classe.Annee;

				if (!IsSelectedClasse)
				{
					an.Classes.Add(Classe);
				}

				var param = new NavigationParameters
				{
					{ "Annee", an }
				};

				_navigationService.GoBackAsync(param);
			}
		}

		private bool IsValid_Classe()
		{
			if (Classe.ValidateProperties())
			{
				AnneeScolaire an = Classe.Annee;

				if (an.Classes.Exists(cl => cl.classeLib == Classe.classeLib))
				{
					if (IsSelectedClasse)
					{
						return true;
					}
					else
					{
						_pageDialogService.DisplayAlertAsync("Erreur",
															"Une classe ayant le même libellé existe déjà.",
															"OK");

						return false;
					}
				}
				else
				{
					return true;
				}
			}
			else
			{
				string message = "La classe en cours d'édition n'est pas valide.\n" +
								 "Veuillez vérifier son libellé.";
				_pageDialogService.DisplayAlertAsync("Erreur", message, "OK");

				return false;
			}
		}

		private void DeleteClasse(Classe classe)
		{
			//_rep.DeleteClasse(classe);
		}

		private async void ExitClasseButton()
		{
			await ExitClasse();
		}

		// Method use for HardWare Back Button on Android
		public virtual async Task<bool> ExitClasse()
		{
			string message = $"Voulez-vous vraiment quitter la classe en cours d'édition?";

			var res = await _pageDialogService.DisplayAlertAsync("Alert", message, "Exit without Save", "Save and Exit");

			// Exit without Save
			if (res)
			{
				await _navigationService.GoBackAsync();
			}
			else
			{
				SaveClasse();
			}

			return res;
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
			if (parameters != null && parameters.ContainsKey("Classe"))
			{
				Classe = parameters["Classe"] as Classe;
			}

			if (parameters.ContainsKey("IsSelectedClasse"))
			{
				IsSelectedClasse = (bool)parameters["IsSelectedClasse"];
			}
		}
		#endregion
	}
}
