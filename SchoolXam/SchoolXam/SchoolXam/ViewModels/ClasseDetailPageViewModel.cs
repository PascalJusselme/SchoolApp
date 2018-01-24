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
	public class ClasseDetailPageViewModel : ChildTabbedPageViewModel
	{
		public Classe cla { get; private set; }

		public ClasseDetailPageViewModel(
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
			if (parameters != null && parameters.ContainsKey("Classe"))
			{
				cla = parameters["Classe"] as Classe;

				SubTitle = cla.classeLib;
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
