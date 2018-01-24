using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Navigation;
using SchoolXam.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SchoolXam.ViewModels
{
	public class MatiereAttribDevoirPageViewModel : ChildTabbedPageViewModel
	{
        public MatiereAttribDevoirPageViewModel(
					INavigationService navigationService,
					SchoolRepository db)
			: base(navigationService, db)
		{

        }
	}
}
