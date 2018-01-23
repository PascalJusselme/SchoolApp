using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using SchoolXam.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SchoolXam.ViewModels
{
	public class BaseViewModel : BindableBase, INavigationAware, IDestructible
	{
		protected readonly ISchoolRepository _rep;

		private string _title;
		public string Title
		{
			get { return _title; }
			set { SetProperty(ref _title, value); }
		}

		private string _subtitle;
		public string SubTitle
		{
			get { return _subtitle; }
			set { SetProperty(ref _subtitle, value); }
		}

		public BaseViewModel(
			SchoolRepository db)
		{
			_rep = db;
		}

		public virtual void OnNavigatedFrom(NavigationParameters parameters) { }

		public virtual void OnNavigatedTo(NavigationParameters parameters) { }

		public virtual void OnNavigatingTo(NavigationParameters parameters) { }

		public virtual void Destroy() { }
	}
}
