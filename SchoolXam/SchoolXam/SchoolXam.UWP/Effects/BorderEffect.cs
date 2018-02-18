﻿using SchoolXam.UWP.Effects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml.Media;
using Xamarin.Forms;
using Xamarin.Forms.Platform.UWP;

[assembly: ResolutionGroupName("SchoolXam")]
[assembly: ExportEffect(typeof(BorderEffect), "BorderEffect")]
namespace SchoolXam.UWP.Effects
{
	public class BorderEffect : PlatformEffect
	{
		Brush _previousBrush;
		Brush _previousBorderBrush;
		Brush _previousFocusBrush;
		FormsTextBox _control;

		protected override void OnAttached()
		{
			_control = Control as FormsTextBox;
			if (_control != null)
			{
				_previousBrush = _control.Background;
				_previousFocusBrush = _control.BackgroundFocusBrush;
				_previousBorderBrush = _control.BorderBrush;
				_control.Background = new SolidColorBrush(Colors.Red);
				_control.BackgroundFocusBrush = new SolidColorBrush(Colors.Red);
				_control.BorderBrush = new SolidColorBrush(Colors.Red);
			}
		}

		protected override void OnDetached()
		{
			if (_control != null)
			{
				_control.Background = _previousBrush;
				_control.BackgroundFocusBrush = _previousFocusBrush;
				_control.BorderBrush = _previousBorderBrush;
			}
		}
	}
}
