using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Navigation;
using SchoolXam.Data;
using SchoolXam.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SchoolXam.ViewModels
{
	public class MatiereDetailPageViewModel : ChildTabbedPageViewModel
	{
		public Matiere mat { get; private set; }

		public MatiereDetailPageViewModel(
					INavigationService navigationService,
					SchoolRepository db)
					: base(navigationService, db)
		{

		}

		public override void OnNavigatedFrom(NavigationParameters parameters)
		{

		}

		public override void OnNavigatedTo(NavigationParameters parameters)
		{
			if (parameters != null && parameters.ContainsKey("Matiere"))
			{
				mat = parameters["Matiere"] as Matiere;

				SubTitle = mat.matiereLib;
			}
		}

		public override void OnNavigatingTo(NavigationParameters parameters)
		{

		}

		public override void Destroy()
		{

		}
	}
}
