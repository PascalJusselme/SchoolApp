using Prism.Navigation;
using Prism.Services;
using SchoolXam.Data;
using SchoolXam.Models;
using System;

namespace SchoolXam.ViewModels
{
	public class MatiereAttribClassePageViewModel : MatiereDetailPageViewModel
	{
		public MatiereAttribClassePageViewModel(
						INavigationService navigationService,
						IPageDialogService pageDialogService,
						SchoolRepository db)
						: base(navigationService, pageDialogService, db)
		{

		}
	}
}
