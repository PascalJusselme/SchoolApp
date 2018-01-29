using Prism.Navigation;
using Prism.Services;
using SchoolXam.Data;
using System;

namespace SchoolXam.ViewModels
{
	public class GestionAnneePageViewModel : AnneeDetailPageViewModel
	{
		public GestionAnneePageViewModel(
						INavigationService navigationService,
						IPageDialogService pageDialogService,
						SchoolRepository db)
						: base(navigationService, pageDialogService, db)
		{

		}
	}
}
