using System;
using System.Threading.Tasks;
using System.Threading;
using System.Reflection;
using System.IO.IsolatedStorage;
using System.Collections.Generic;
using Microsoft.Maui.Controls;
using System.Security.Cryptography;
using System.Text;
using FileMode = System.IO.FileMode;
using FileAccess = System.IO.FileAccess;
using FileShare = System.IO.FileShare;
using Stream = System.IO.Stream;
using Microsoft.Maui.Controls.DualScreen.UnitTests;
using Microsoft.Maui.Controls.Internals;

[assembly: Dependency(typeof(MockDeserializer))]
[assembly: Dependency(typeof(MockResourcesProvider))]

namespace Microsoft.Maui.Controls.DualScreen.UnitTests
{
	internal class MockPlatformServices : Internals.IPlatformServices
	{
		Action<Action> invokeOnMainThread;
		Func<Uri, CancellationToken, Task<Stream>> getStreamAsync;
		Func<VisualElement, double, double, SizeRequest> getNativeSizeFunc;
		readonly bool useRealisticLabelMeasure;
		readonly bool _isInvokeRequired;

		public MockPlatformServices(Action<Action> invokeOnMainThread = null,
			Func<Uri, CancellationToken, Task<Stream>> getStreamAsync = null,
			Func<VisualElement, double, double, SizeRequest> getNativeSizeFunc = null,
			bool useRealisticLabelMeasure = false, bool isInvokeRequired = false)
		{
			this.invokeOnMainThread = invokeOnMainThread;
			this.getStreamAsync = getStreamAsync;
			this.getNativeSizeFunc = getNativeSizeFunc;
			this.useRealisticLabelMeasure = useRealisticLabelMeasure;
			_isInvokeRequired = isInvokeRequired;
		}

		public string GetHash(string input) => Internals.Crc64.GetHash(input);
		string IPlatformServices.GetMD5Hash(string input) => GetHash(input);

		static int hex(int v)
		{
			if (v < 10)
				return '0' + v;
			return 'a' + v - 10;
		}

		public double GetNamedSize(NamedSize size, Type targetElement, bool useOldSizes)
		{
			switch (size)
			{
				case NamedSize.Default:
					return 10;
				case NamedSize.Micro:
					return 4;
				case NamedSize.Small:
					return 8;
				case NamedSize.Medium:
					return 12;
				case NamedSize.Large:
					return 16;
				default:
					throw new ArgumentOutOfRangeException(nameof(size));
			}
		}

		public Color GetNamedColor(string name)
		{
			// Not supported on this platform
			return Color.Default;
		}

		public bool IsInvokeRequired
		{
			get { return _isInvokeRequired; }
		}

		public string RuntimePlatform { get; set; }

		public void BeginInvokeOnMainThread(Action action)
		{
			if (invokeOnMainThread == null)
				action();
			else
				invokeOnMainThread(action);
		}

		public Internals.Ticker CreateTicker()
		{
			return new MockTicker();
		}

		public void StartTimer(TimeSpan interval, Func<bool> callback)
		{
			Timer timer = null;
			TimerCallback onTimeout = o => BeginInvokeOnMainThread(() =>
			{
				if (callback())
					return;

				timer.Dispose();
			});
			timer = new Timer(onTimeout, null, interval, interval);
		}

		public Task<Stream> GetStreamAsync(Uri uri, CancellationToken cancellationToken)
		{
			if (getStreamAsync == null)
				throw new NotImplementedException();
			return getStreamAsync(uri, cancellationToken);
		}

		public Assembly[] GetAssemblies()
		{
			return AppDomain.CurrentDomain.GetAssemblies();
		}

		public Internals.IIsolatedStorageFile GetUserStoreForApplication()
		{
			return new MockIsolatedStorageFile(IsolatedStorageFile.GetUserStoreForAssembly());
		}

		public class MockIsolatedStorageFile : Internals.IIsolatedStorageFile
		{
			readonly IsolatedStorageFile isolatedStorageFile;
			public MockIsolatedStorageFile(IsolatedStorageFile isolatedStorageFile)
			{
				this.isolatedStorageFile = isolatedStorageFile;
			}

			public Task<bool> GetDirectoryExistsAsync(string path)
			{
				return Task.FromResult(isolatedStorageFile.DirectoryExists(path));
			}

			public Task CreateDirectoryAsync(string path)
			{
				isolatedStorageFile.CreateDirectory(path);
				return Task.FromResult(true);
			}

			public Task<Stream> OpenFileAsync(string path, FileMode mode, FileAccess access)
			{
				Stream stream = isolatedStorageFile.OpenFile(path, mode, access);
				return Task.FromResult(stream);
			}

			public Task<Stream> OpenFileAsync(string path, FileMode mode, FileAccess access, FileShare share)
			{
				Stream stream = isolatedStorageFile.OpenFile(path, mode, access, share);
				return Task.FromResult(stream);
			}

			public Task<bool> GetFileExistsAsync(string path)
			{
				return Task.FromResult(isolatedStorageFile.FileExists(path));
			}

			public Task<DateTimeOffset> GetLastWriteTimeAsync(string path)
			{
				return Task.FromResult(isolatedStorageFile.GetLastWriteTime(path));
			}
		}

		public SizeRequest GetNativeSize(VisualElement view, double widthConstraint, double heightConstraint)
		{
			if (getNativeSizeFunc != null)
				return getNativeSizeFunc(view, widthConstraint, heightConstraint);
			// EVERYTHING IS 100 x 20

			var label = view as Label;
			if (label != null && useRealisticLabelMeasure)
			{
				var letterSize = new Size(5, 10);
				var w = label.Text.Length * letterSize.Width;
				var h = letterSize.Height;
				if (!double.IsPositiveInfinity(widthConstraint) && w > widthConstraint)
				{
					h = ((int)w / (int)widthConstraint) * letterSize.Height;
					w = widthConstraint - (widthConstraint % letterSize.Width);

				}
				return new SizeRequest(new Size(w, h), new Size(Math.Min(10, w), h));
			}

			return new SizeRequest(new Size(100, 20));
		}

		public OSAppTheme RequestedTheme => OSAppTheme.Unspecified;
	}

	internal class MockDeserializer : Internals.IDeserializer
	{
		public Task<IDictionary<string, object>> DeserializePropertiesAsync()
		{
			return Task.FromResult<IDictionary<string, object>>(new Dictionary<string, object>());
		}

		public Task SerializePropertiesAsync(IDictionary<string, object> properties)
		{
			return Task.FromResult(false);
		}
	}

	internal class MockResourcesProvider : Internals.ISystemResourcesProvider
	{
		public Internals.IResourceDictionary GetSystemResources()
		{
			var dictionary = new ResourceDictionary();
			Style style;
			style = new Style(typeof(Label));
			dictionary[Device.Styles.BodyStyleKey] = style;

			style = new Style(typeof(Label));
			style.Setters.Add(Label.FontSizeProperty, 50);
			dictionary[Device.Styles.TitleStyleKey] = style;

			style = new Style(typeof(Label));
			style.Setters.Add(Label.FontSizeProperty, 40);
			dictionary[Device.Styles.SubtitleStyleKey] = style;

			style = new Style(typeof(Label));
			style.Setters.Add(Label.FontSizeProperty, 30);
			dictionary[Device.Styles.CaptionStyleKey] = style;

			style = new Style(typeof(Label));
			style.Setters.Add(Label.FontSizeProperty, 20);
			dictionary[Device.Styles.ListItemTextStyleKey] = style;

			style = new Style(typeof(Label));
			style.Setters.Add(Label.FontSizeProperty, 10);
			dictionary[Device.Styles.ListItemDetailTextStyleKey] = style;

			return dictionary;
		}
	}

	public class MockApplication : Application
	{
		public MockApplication()
		{
		}
	}

	internal class MockTicker : Internals.Ticker
	{
		bool _enabled;

		protected override void EnableTimer()
		{
			_enabled = true;

			while (_enabled)
			{
				SendSignals(16);
			}
		}

		protected override void DisableTimer()
		{
			_enabled = false;
		}
	}
}