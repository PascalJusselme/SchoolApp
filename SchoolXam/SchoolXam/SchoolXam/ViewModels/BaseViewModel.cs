using Prism.Mvvm;
using Prism.Navigation;
using SchoolXam.Data;

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
