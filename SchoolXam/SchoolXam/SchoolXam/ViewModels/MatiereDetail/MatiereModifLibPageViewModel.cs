using Prism;
using Prism.Navigation;
using SchoolXam.Data;
using SchoolXam.Models;
using System;

namespace SchoolXam.ViewModels
{
	public class MatiereModifLibPageViewModel : BaseViewModel, IActiveAware
	{
		private Matiere _matiere;
		public Matiere Matiere
		{
			get { return _matiere; }
			set { SetProperty(ref _matiere, value); }
		}

		public MatiereModifLibPageViewModel(SchoolRepository db) : base(db)
		{

		}

		#region IActiveAware Implementation
		public event EventHandler IsActiveChanged;

		protected static bool HasInitialized { get; set; }

		private bool isActive;

		public bool IsActive
		{
			get => isActive;
			set => SetProperty(ref isActive, value, RaiseIsActiveChanged);
		}

		private void RaiseIsActiveChanged()
		{

		}
		#endregion

		#region INavigationService Implementation
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
			}
		}

		public override void Destroy()
		{

		}
		#endregion
	}
}
