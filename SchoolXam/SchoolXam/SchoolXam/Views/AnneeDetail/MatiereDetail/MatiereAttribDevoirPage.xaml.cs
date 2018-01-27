using SchoolXam.ViewModels;
using Xamarin.Forms;

namespace SchoolXam.Views
{
	public partial class MatiereAttribDevoirPage : ContentPage
    {
		public AnneeDetailPageViewModel ViewModel => BindingContext as AnneeDetailPageViewModel;

		public MatiereAttribDevoirPage()
        {
            InitializeComponent();			
		}
    }
}
