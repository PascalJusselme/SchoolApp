using Prism.Navigation;
using Prism.Services;
using SchoolXam.Data;
using System;

namespace SchoolXam.ViewModels
{
	public class GestionMatierePageViewModel : AnneeDetailPageViewModel
	{
		public GestionMatierePageViewModel(
						INavigationService navigationService,
						IPageDialogService pageDialogService,
						SchoolRepository db)
						: base(navigationService, pageDialogService, db)
		{

		}
	}
}
