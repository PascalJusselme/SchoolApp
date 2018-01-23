using SchoolXam.ViewModels;
using Xamarin.Forms;

namespace SchoolXam.Views
{
    public partial class ClasseAttribMatierePage : ContentPage
    {
		public AnneeDetailPageViewModel ViewModel => BindingContext as AnneeDetailPageViewModel;

		public ClasseAttribMatierePage()
        {
            InitializeComponent();
        }
    }
}
