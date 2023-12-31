using System.Collections.Generic;

namespace Microsoft.Maui.Handlers.Benchmarks
{
	class ApplicationStub : IApplication
	{
		readonly List<IWindow> _windows = new List<IWindow>();

		public IElementHandler Handler { get; set; }

		public IElement Parent { get; set; }

		public IReadOnlyList<IWindow> Windows => _windows.AsReadOnly();

		public IWindow CreateWindow(IActivationState state)
		{
			_windows.Add(new WindowStub());

			return _windows[0];
		}

		public void ThemeChanged() { }
	}
}