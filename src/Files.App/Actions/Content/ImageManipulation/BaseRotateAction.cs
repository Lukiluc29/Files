// Copyright (c) Files Community
// Licensed under the MIT License.

using Files.Shared.Helpers;
using Windows.Graphics.Imaging;

namespace Files.App.Actions
{
	internal abstract class BaseRotateAction : ObservableObject, IAction
	{
		protected readonly IContentPageContext context;

		private readonly InfoPaneViewModel _infoPaneViewModel;

		public abstract string Label { get; }

		public abstract string Description { get; }

		public abstract RichGlyph Glyph { get; }

		protected abstract BitmapRotation Rotation { get; }

		public bool IsExecutable =>
			context.ShellPage is not null &&
			context.ShellPage.SlimContentPage is not null &&
			context.PageType != ContentPageTypes.ZipFolder &&
			context.PageType != ContentPageTypes.Home &&
			context.PageType != ContentPageTypes.RecycleBin &&
			context.PageType != ContentPageTypes.ReleaseNotes &&
			context.PageType != ContentPageTypes.Settings &&
			context.PageType != ContentPageTypes.Desktop &&
			context.PageType != ContentPageTypes.Downloads &&
			context.PageType != ContentPageTypes.Pictures &&
			context.PageType != ContentPageTypes.Music &&
			context.PageType != ContentPageTypes.Videos &&
			context.PageType != ContentPageTypes.Documents &&
			context.PageType != ContentPageTypes.Network &&
			context.PageType != ContentPageTypes.ThisPC &&
			context.HasSelection &&
			context.SelectedItems.All(x => FileExtensionHelpers.IsCompatibleToSetAsWindowsWallpaper(x.FileExtension));

		public BaseRotateAction()
		{
			context = Ioc.Default.GetRequiredService<IContentPageContext>();
			_infoPaneViewModel = Ioc.Default.GetRequiredService<InfoPaneViewModel>();

			context.PropertyChanged += Context_PropertyChanged;
		}

		public async Task ExecuteAsync(object? parameter = null)
		{
			await Task.WhenAll(context.SelectedItems.Select(image => BitmapHelper.RotateAsync(PathNormalization.NormalizePath(image.ItemPath), Rotation)));

			context.ShellPage?.SlimContentPage?.ItemManipulationModel?.RefreshItemsThumbnail();

			await _infoPaneViewModel.UpdateSelectedItemPreviewAsync();
		}

		private void Context_PropertyChanged(object? sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName is nameof(IContentPageContext.SelectedItems))
				OnPropertyChanged(nameof(IsExecutable));
		}
	}
}