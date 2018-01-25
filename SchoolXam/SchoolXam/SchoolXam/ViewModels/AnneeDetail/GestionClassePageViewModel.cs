﻿using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;
using SchoolXam.Data;
using SchoolXam.Events;
using SchoolXam.Messages;
using SchoolXam.Models;
using SchoolXam.ViewModels.ChildPageViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Xamarin.Forms;

namespace SchoolXam.ViewModels.ChildPageViewModel
{
	public class GestionClassePageViewModel : ChildTabbedPageViewModel
	{
		public GestionClassePageViewModel(
					INavigationService navigationService,
					SchoolRepository db)
			: base(navigationService, db)
		{

			IsActiveChanged += HandleIsActiveTrue;
			//IsActiveChanged += HandleIsActiveFalse;

		}

		// Use if there's some code to be executed when the tab is not 
		// the active
		//private void HandleIsActiveFalse(object sender, EventArgs e)
		//{
		//    if (IsActive == true) return;
		//}

		// Use if there's some code to be executed when the tab is the active tab
		private void HandleIsActiveTrue(object sender, EventArgs e)
		{
			if (IsActive == false) return;
		}

		public override void Destroy()
		{
			IsActiveChanged -= HandleIsActiveTrue;
			//IsActiveChanged -= HandleIsActiveFalse;
		}

		// Use the INavigationAware methods (OnNavigatedTo, OnNavigedFrom, OnNavigatingTo)
		// if you want to execute some code when the page is charged using NavigationService.
		public override void OnNavigatedTo(NavigationParameters parameters)
		{

		}
	}
}