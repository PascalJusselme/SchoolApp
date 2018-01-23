using SchoolXam.ViewModels;
using Xamarin.Forms;

namespace SchoolXam.Views
{
    public partial class MatiereAttribClassePage : ContentPage
    {
		public AnneeDetailPageViewModel ViewModel => BindingContext as AnneeDetailPageViewModel;

		public MatiereAttribClassePage()
        {
            InitializeComponent();

			lstClasses.ItemTapped += (object sender, ItemTappedEventArgs e) => 
			{
				// don't do anything if we just de-selected the row
				if (e.Item == null) return;

				// do something with e.SelectedItem
				((ListView)sender).SelectedItem = null; // de-select the row
			};
		}
    }
}
