using Prism.Unity;
using SchoolXam.Data;
using Unity;
using Xamarin.Forms;

namespace SchoolXam.UWP
{
	public sealed partial class MainPage
    {
        public MainPage()
        {
            this.InitializeComponent();

			LoadApplication(new SchoolXam.App(new UwpInitializer()));
        }
    }

    public class UwpInitializer : IPlatformInitializer
    {
        public void RegisterTypes(IUnityContainer container)
		{
			container.RegisterInstance(new SchoolRepository(DependencyService.Get<FileHelper_UWP>().GetConnection()));
		}
	}
}
