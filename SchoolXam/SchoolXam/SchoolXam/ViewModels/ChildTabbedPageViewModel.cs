using Prism;
using Prism.Commands;
using Prism.Events;
using Prism.Navigation;
using SchoolXam.Data;
using SchoolXam.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace SchoolXam.ViewModels
{
	public class ChildTabbedPageViewModel : AnneeDetailPageViewModel, IActiveAware
	{
		protected static bool HasInitialized { get; set; }

		private bool isActive;
		public bool IsActive
		{
			get => isActive;
			set => SetProperty(ref isActive, value, RaiseIsActiveChanged);
		}

		public event EventHandler IsActiveChanged;

		public ChildTabbedPageViewModel(
					INavigationService navigationService,
					SchoolRepository db)
					:base(navigationService,db)
		{

		}

		protected virtual void RaiseIsActiveChanged()
		{
			IsActiveChanged?.Invoke(this, EventArgs.Empty);
		}
	}
}
