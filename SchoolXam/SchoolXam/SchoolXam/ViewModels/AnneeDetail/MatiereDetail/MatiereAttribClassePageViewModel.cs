using Prism.Navigation;
using Prism.Services;
using SchoolXam.Data;
using SchoolXam.Models;
using System;

namespace SchoolXam.ViewModels
{
	public class MatiereAttribClassePageViewModel : ChildTabbedPageViewModel
	{
		public MatiereAttribClassePageViewModel(
						INavigationService navigationService,
						IPageDialogService pageDialogService,
						SchoolRepository db)
						: base(navigationService, pageDialogService, db)
		{
			IsActiveChanged += HandleIsActiveTrue;
			//IsActiveChanged += HandleIsActiveFalse;

		}

		// Use if there's some code to be executed when the tab is not 
		// the active
		//private void HandleIsActiveFalse(object sender, EventArgs e)
		//{
		//    if (IsActive == true) return;
		//}

		// Use if there's some code to be executed when the tab is the active tab
		private void HandleIsActiveTrue(object sender, EventArgs e)
		{
			if (IsActive == false) { return; }
		}

		public override void Destroy()
		{
			IsActiveChanged -= HandleIsActiveTrue;
			//IsActiveChanged -= HandleIsActiveFalse;
			
			//foreach (Classe classe in Matiere.Classes)
			//{
			//	if (classe.Matieres.Find(ma => ma.matiereLib == Matiere.matiereLib) == null)
			//	{
			//		classe.Matieres.Add(Matiere);
			//	}
			//}
		}

		// Use the INavigationAware methods (OnNavigatedTo, OnNavigedFrom, OnNavigatingTo)
		// if you want to execute some code when the page is charged using NavigationService.
		public override void OnNavigatedTo(NavigationParameters parameters)
		{

		}
	}
}
