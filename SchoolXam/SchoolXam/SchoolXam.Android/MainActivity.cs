using Android.App;
using Android.Content.PM;
using Android.OS;
using Prism.Unity;
using SchoolXam.Data;
using Unity;
using Xamarin.Forms;

namespace SchoolXam.Droid
{
	[Activity(Label = "SchoolXam", Icon = "@drawable/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);

            global::Xamarin.Forms.Forms.Init(this, bundle);
            LoadApplication(new App(new AndroidInitializer()));
        }
    }

    public class AndroidInitializer : IPlatformInitializer
    {
        public void RegisterTypes(IUnityContainer container)
        {
			container.RegisterInstance(new SchoolRepository(DependencyService.Get<FileHelper_Android>().GetConnection()));
		}
	}
}

