using Prism.Navigation;
using Prism.Services;
using SchoolXam.Data;
using SchoolXam.Models;
using System;

namespace SchoolXam.ViewModels
{
	public class ClasseAttribElevePageViewModel : ClasseDetailPageViewModel
	{
		public ClasseAttribElevePageViewModel(
						INavigationService navigationService,
						IPageDialogService pageDialogService,
						SchoolRepository db)
						: base(navigationService, pageDialogService, db)
		{

		}
	}
}
