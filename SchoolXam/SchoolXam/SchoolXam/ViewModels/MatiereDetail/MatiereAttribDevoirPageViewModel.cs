using Prism;
using Prism.Navigation;
using Prism.Services;
using SchoolXam.Data;
using SchoolXam.Models;
using System;
using System.Collections.ObjectModel;

namespace SchoolXam.ViewModels
{
	public class MatiereAttribDevoirPageViewModel : MatiereDetailPageViewModel
	{
		public MatiereAttribDevoirPageViewModel(
						INavigationService navigationService,
						IPageDialogService pageDialogService,
						SchoolRepository db)
						: base(navigationService, pageDialogService, db)
		{

		}
	}
}
