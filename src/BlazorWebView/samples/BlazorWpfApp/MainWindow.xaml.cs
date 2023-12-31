// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Windows;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.DependencyInjection;
using WebViewAppShared;

namespace BlazorWpfApp
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		private readonly AppState _appState = new();

		public MainWindow()
		{
			var serviceCollection = new ServiceCollection();
			serviceCollection.AddBlazorWebView();
			serviceCollection.AddSingleton<AppState>(_appState);
			Resources.Add("services", serviceCollection.BuildServiceProvider());

			InitializeComponent();

			blazorWebView1.RootComponents.RegisterForJavaScript<MyDynamicComponent>("my-dynamic-root-component");
		}

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			MessageBox.Show(
				owner: this,
				messageBoxText: $"Current counter value is: {_appState.Counter}",
				caption: "Counter");
		}

		private void WebViewAlertButton_Click(object sender, RoutedEventArgs e)
		{
			blazorWebView1.WebView.CoreWebView2.ExecuteScriptAsync("alert('hello from native UI')");
		}
	}

	// Workaround for compiler error "error MC3050: Cannot find the type 'local:Main'"
	// It seems that, although WPF's design-time build can see Razor components, its runtime build cannot.
	public partial class Main { }
}
