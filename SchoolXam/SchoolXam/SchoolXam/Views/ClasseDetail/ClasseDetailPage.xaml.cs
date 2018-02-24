using SchoolXam.ViewModels;
using Xamarin.Forms;

namespace SchoolXam.Views
{
	public partial class ClasseDetailPage : TabbedPage
	{
        public ClasseDetailPage()
        {
            InitializeComponent();
			NavigationPage.SetHasBackButton(this, false);
		}

		protected override bool OnBackButtonPressed()
		{
			var bindingContext = BindingContext as ClasseDetailPageViewModel;

			Device.BeginInvokeOnMainThread(async () =>
			{
				var result = await bindingContext?.ExitClasse();
			});

			return true;
		}
	}
}
