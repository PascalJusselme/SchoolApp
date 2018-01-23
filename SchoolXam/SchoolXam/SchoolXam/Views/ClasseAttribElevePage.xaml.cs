using SchoolXam.ViewModels;
using Xamarin.Forms;

namespace SchoolXam.Views
{
    public partial class ClasseAttribElevePage : ContentPage
    {
		public AnneeDetailPageViewModel ViewModel => BindingContext as AnneeDetailPageViewModel;

		public ClasseAttribElevePage()
        {
            InitializeComponent();
        }
    }
}
