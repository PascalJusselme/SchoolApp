using Prism;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using SchoolXam.Data;
using SchoolXam.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SchoolXam.ViewModels
{
	public class ClasseModifLibPageViewModel : BaseViewModel,IActiveAware
	{
		private Classe _classe;
		public Classe Classe
		{
			get { return _classe; }
			set { SetProperty(ref _classe, value); }
		}

		public ClasseModifLibPageViewModel(SchoolRepository db) : base(db)
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
			if (parameters != null && parameters.ContainsKey("Classe"))
			{
				Classe = parameters["Classe"] as Classe;
			}
		}

		public override void Destroy()
		{

		}
		#endregion
	}
}
