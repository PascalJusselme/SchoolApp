using Prism.Navigation;
using SchoolXam.Data;
using System;

namespace SchoolXam.ViewModels
{
	public class ClasseAttribElevePageViewModel : ChildTabbedPageViewModel
	{
		public ClasseAttribElevePageViewModel(
					INavigationService navigationService,
					SchoolRepository db)
			: base(navigationService, db)
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
			if (IsActive == false) return;
		}

		public override void Destroy()
		{
			IsActiveChanged -= HandleIsActiveTrue;
			//IsActiveChanged -= HandleIsActiveFalse;
		}

		// Use the INavigationAware methods (OnNavigatedTo, OnNavigedFrom, OnNavigatingTo)
		// if you want to execute some code when the page is charged using NavigationService.
		public override void OnNavigatedTo(NavigationParameters parameters)
		{

		}
	}
}
