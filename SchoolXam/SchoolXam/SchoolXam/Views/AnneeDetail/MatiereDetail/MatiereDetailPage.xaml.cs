using Prism.Navigation;
using Xamarin.Forms;

namespace SchoolXam.Views
{
    public partial class MatiereDetailPage : TabbedPage,INavigatingAware
    {
        public MatiereDetailPage()
        {
            InitializeComponent();
        }

		public void OnNavigatingTo(NavigationParameters parameters)
		{
			foreach (var child in Children)
			{
				(child as INavigatingAware)?.OnNavigatingTo(parameters);
				(child?.BindingContext as INavigatingAware)?.OnNavigatingTo(parameters);
			}
		}
	}
}
