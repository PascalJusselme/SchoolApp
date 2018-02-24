using SchoolXam.ViewModels;
using Xamarin.Forms;

namespace SchoolXam.Views
{
	public partial class AnneeDetailPage : TabbedPage
	{
		public AnneeDetailPage()
		{
			InitializeComponent();
			NavigationPage.SetHasBackButton(this, false);
		}

		protected override bool OnBackButtonPressed()
		{
			var bindingContext = BindingContext as AnneeDetailPageViewModel;

			Device.BeginInvokeOnMainThread(async () =>
			{
				var result = await bindingContext?.ExitAnnee();
			});

			return true;
		}
	}
}
