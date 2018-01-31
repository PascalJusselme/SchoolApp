using Android.Content;
using Android.Graphics;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(Switch), typeof(SchoolXam.Droid.Renderers.SwitchRenderer_Android))]
namespace SchoolXam.Droid.Renderers
{
	public class SwitchRenderer_Android : SwitchRenderer
	{
		public SwitchRenderer_Android(Context context) : base(context)
		{

		}

		private Android.Graphics.Color redColor = new Android.Graphics.Color(255, 0, 0);
		private Android.Graphics.Color greenColor = new Android.Graphics.Color(32, 156, 68);

		protected override void Dispose(bool disposing)
		{
			Element.Toggled -= OnToggleChange;
			base.Dispose(disposing);
		}

		protected override void OnElementChanged(ElementChangedEventArgs<Switch> e)
		{
			base.OnElementChanged(e);

			if (Control != null)
			{
				if (Element.IsToggled)
				{
					Control.ThumbDrawable.SetColorFilter(greenColor, PorterDuff.Mode.SrcAtop);
					Control.TrackDrawable.SetColorFilter(greenColor, PorterDuff.Mode.SrcAtop);
				}
				else
				{
					Control.ThumbDrawable.SetColorFilter(redColor, PorterDuff.Mode.SrcAtop);
					Control.TrackDrawable.SetColorFilter(redColor, PorterDuff.Mode.SrcAtop);
				}

				Element.Toggled += OnToggleChange;
			}
		}

		private void OnToggleChange(object sender, ToggledEventArgs e)
		{
			if (Control.Checked)
			{
				Control.ThumbDrawable.SetColorFilter(greenColor, PorterDuff.Mode.SrcAtop);
				Control.TrackDrawable.SetColorFilter(greenColor, PorterDuff.Mode.SrcAtop);
			}
			else
			{
				Control.ThumbDrawable.SetColorFilter(redColor, PorterDuff.Mode.SrcAtop);
				Control.TrackDrawable.SetColorFilter(redColor, PorterDuff.Mode.SrcAtop);
			}
		}
	}
}