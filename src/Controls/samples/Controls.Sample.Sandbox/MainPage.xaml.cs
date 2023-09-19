using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;

namespace Maui.Controls.Sample
{
	public partial class MainPage : ContentPage
	{
		public MainPage()
		{
			InitializeComponent();
		}

		void Button_Clicked(System.Object sender, System.EventArgs e)
		{
			this.HelloWorld.HorizontalOptions = (this.HelloWorld.HorizontalOptions.Alignment == LayoutAlignment.Start)
				? LayoutOptions.End
				: LayoutOptions.Start;
		}
	}
}