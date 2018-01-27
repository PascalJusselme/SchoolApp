using Prism;
using Prism.Navigation;
using Prism.Services;
using SchoolXam.Data;
using System;

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
						IPageDialogService pageDialogService,
						SchoolRepository db)
						: base(navigationService, pageDialogService, db)
		{

		}

		protected virtual void RaiseIsActiveChanged()
		{
			IsActiveChanged?.Invoke(this, EventArgs.Empty);
		}
	}
}
