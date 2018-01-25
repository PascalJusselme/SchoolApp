using SchoolXam.ViewModels;
using SchoolXam.Views;
using Prism.Unity;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace SchoolXam
{
	public partial class App : PrismApplication
	{
		/* 
         * The Xamarin Forms XAML Previewer in Visual Studio uses System.Activator.CreateInstance.
         * This imposes a limitation in which the App class must have a default constructor. 
         * App(IPlatformInitializer initializer = null) cannot be handled by the Activator.
         */
		public App() : this(null) { }

		public App(IPlatformInitializer initializer) : base(initializer) { }

		protected override async void OnInitialized()
		{
			InitializeComponent();

			await NavigationService.NavigateAsync("NavigationPage/AnneeMasterPage");
		}

		protected override void RegisterTypes()
		{
			Container.RegisterTypeForNavigation<NavigationPage>();
			
			Container.RegisterTypeForNavigation<AnneeMasterPage>();
			Container.RegisterTypeForNavigation<AnneeDetailPage>();

			Container.RegisterTypeForNavigation<GestionAnneePage>();
			Container.RegisterTypeForNavigation<GestionClassePage>();
			Container.RegisterTypeForNavigation<GestionMatierePage>();

			Container.RegisterTypeForNavigation<ClasseDetailPage>();
			Container.RegisterTypeForNavigation<ClasseAttribMatierePage>();
			Container.RegisterTypeForNavigation<ClasseAttribElevePage>();

			Container.RegisterTypeForNavigation<MatiereDetailPage>();
			Container.RegisterTypeForNavigation<MatiereAttribClassePage>();
			Container.RegisterTypeForNavigation<MatiereAttribDevoirPage>();
		}
	}
}
