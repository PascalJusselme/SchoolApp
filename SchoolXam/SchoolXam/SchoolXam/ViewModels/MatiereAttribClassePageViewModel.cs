﻿using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Navigation;
using SchoolXam.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SchoolXam.ViewModels
{
	public class MatiereAttribClassePageViewModel : ChildTabbedPageViewModel
	{
        public MatiereAttribClassePageViewModel(
					INavigationService navigationService,
					IEventAggregator eventAggregator,
					SchoolRepository db)
			: base(navigationService, eventAggregator, db)
		{

        }
	}
}
