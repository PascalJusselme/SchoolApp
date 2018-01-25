﻿using Prism.Navigation;
using SchoolXam.Data;
using SchoolXam.Models;

namespace SchoolXam.ViewModels
{
	public class MatiereDetailPageViewModel : AnneeDetailPageViewModel
	{
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
			
		}

		public override void OnNavigatingTo(NavigationParameters parameters)
		{
			if (parameters != null && parameters.ContainsKey("Matiere"))
			{
				Matiere = parameters["Matiere"] as Matiere;

				SubTitle = $"Matiere : {Matiere.matiereLib}";
			}
		}

		public override void Destroy()
		{

		}
	}
}
