using Prism.Mvvm;

namespace SchoolXam.Models
{
    public class SelectableData<T> : BindableBase
    {
		public T Data { get; set; }

		private bool _selected;
		public bool Selected
		{
			get { return _selected; }
			set { SetProperty(ref _selected, value); }
		}
	}
}
