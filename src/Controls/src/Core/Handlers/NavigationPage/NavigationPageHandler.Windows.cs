﻿using System;
using Microsoft.Maui.Controls.Platform;
using Microsoft.Maui.Handlers;

namespace Microsoft.Maui.Controls.Handlers
{
	// Currently only inheriting because we can't tap into CreateNativeView
	// Once we can wire up into CreateNativeView then all of this code can move into the 
	// Remap structures
	internal partial class NavigationPageHandler : Microsoft.Maui.Handlers.NavigationViewHandler
	{
		public static PropertyMapper<NavigationPage, NavigationPageHandler> NavigationPageMapper =
			new PropertyMapper<NavigationPage, NavigationPageHandler>(NavigationViewHandler.NavigationViewMapper)
			{
				[NavigationPage.HasNavigationBarProperty.PropertyName] = MapHasNavigationBar,
				[NavigationPage.HasBackButtonProperty.PropertyName] = MapHasBackButton,
				[NavigationPage.TitleIconImageSourceProperty.PropertyName] = MapTitleIconImageSource,
				[NavigationPage.TitleViewProperty.PropertyName] = MapTitleView,
				[NavigationPage.IconColorProperty.PropertyName] = MapIconColor,
				[Page.TitleProperty.PropertyName] = MapTitle,
				[NavigationPage.CurrentPageProperty.PropertyName] = MapCurrentPage,
				[NavigationPage.BarBackgroundColorProperty.PropertyName] = MapBarBackground,
				[NavigationPage.BarBackgroundProperty.PropertyName] = MapBarBackground,
				[PlatformConfiguration.WindowsSpecific.Page.ToolbarPlacementProperty.PropertyName] = MapToolbarPlacement,
				[PlatformConfiguration.WindowsSpecific.Page.ToolbarDynamicOverflowEnabledProperty.PropertyName] = MapToolbarDynamicOverflowEnabled,
			};

		// TODO MAUI: break these out into extension methods
		public static void MapBarBackground(NavigationPageHandler handler, NavigationPage view)
		{
			handler._controlsNavigationManager.ToolbarPropertyChanged();
		}

		public static void MapToolbarDynamicOverflowEnabled(NavigationPageHandler handler, NavigationPage view)
		{
			handler._controlsNavigationManager.ToolbarPropertyChanged();
		}

		public static void MapToolbarPlacement(NavigationPageHandler handler, NavigationPage view)
		{
			handler._controlsNavigationManager.ToolbarPropertyChanged();
		}

		public static void MapCurrentPage(NavigationPageHandler handler, NavigationPage view)
		{
			handler._controlsNavigationManager.ToolbarPropertyChanged();
		}

		public static void MapTitle(NavigationPageHandler handler, NavigationPage view)
		{
			handler._controlsNavigationManager.ToolbarPropertyChanged();
		}

		public static void MapIconColor(NavigationPageHandler handler, NavigationPage view)
		{
			handler._controlsNavigationManager.ToolbarPropertyChanged();
		}

		public static void MapTitleView(NavigationPageHandler handler, NavigationPage view)
		{
			handler._controlsNavigationManager.ToolbarPropertyChanged();
		}

		public static void MapTitleIconImageSource(NavigationPageHandler handler, NavigationPage view)
		{
			handler._controlsNavigationManager.ToolbarPropertyChanged();
		}

		public static void MapHasBackButton(NavigationPageHandler handler, NavigationPage view)
		{
			handler._controlsNavigationManager.ToolbarPropertyChanged();
		}

		public static void MapHasNavigationBar(NavigationPageHandler handler, NavigationPage view)
		{
			handler._controlsNavigationManager.ToolbarPropertyChanged();
		}

		ControlsNavigationManager _controlsNavigationManager;
		public NavigationPageHandler() : base(NavigationPageMapper)
		{

		}

		protected override NavigationManager CreateNavigationManager()
			=> _controlsNavigationManager ??= new ControlsNavigationManager(MauiContext!);

		public static void UpdateToolBar(NavigationPageHandler handler, NavigationPage view)
		{
			handler._controlsNavigationManager.ToolbarPropertyChanged();
		}
	}
}
