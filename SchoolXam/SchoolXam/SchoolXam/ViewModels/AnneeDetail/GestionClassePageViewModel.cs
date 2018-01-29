using Prism.Navigation;
using Prism.Services;
using SchoolXam.Data;
using System;

namespace SchoolXam.ViewModels
{
	public class GestionClassePageViewModel : AnneeDetailPageViewModel
	{
		public GestionClassePageViewModel(
						INavigationService navigationService,
						IPageDialogService pageDialogService,
						SchoolRepository db)
						: base(navigationService, pageDialogService, db)
		{
		
		}
	}
}
