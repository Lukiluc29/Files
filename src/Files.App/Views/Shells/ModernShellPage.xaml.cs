// Copyright (c) Files Community
// Licensed under the MIT License.

using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media.Animation;
using Microsoft.UI.Xaml.Navigation;
using System.IO;
using System.Windows.Documents;
using Windows.System;

namespace Files.App.Views.Shells
{
	public sealed partial class ModernShellPage : BaseShellPage
	{
		protected override Frame ItemDisplay
			=> ItemDisplayFrame;

		private NavigationInteractionTracker _navigationInteractionTracker;

		private NavigationParams _NavParams;
		public NavigationParams NavParams
		{
			get => _NavParams;
			set
			{
				if (value != _NavParams)
				{
					_NavParams = value;

					if (IsLoaded)
						OnNavigationParamsChanged();
				}
			}
		}

		public ModernShellPage() : base(new CurrentInstanceViewModel())
		{
			InitializeComponent();

			ShellViewModel = new ShellViewModel(InstanceViewModel.FolderSettings);
			ShellViewModel.WorkingDirectoryModified += ViewModel_WorkingDirectoryModified;
			ShellViewModel.ItemLoadStatusChanged += FilesystemViewModel_ItemLoadStatusChanged;
			ShellViewModel.DirectoryInfoUpdated += FilesystemViewModel_DirectoryInfoUpdated;
			ShellViewModel.PageTypeUpdated += FilesystemViewModel_PageTypeUpdated;
			ShellViewModel.OnSelectionRequestedEvent += FilesystemViewModel_OnSelectionRequestedEvent;
			ShellViewModel.GitDirectoryUpdated += FilesystemViewModel_GitDirectoryUpdated;

			ToolbarViewModel.PathControlDisplayText = Strings.Home.GetLocalizedResource();
			ToolbarViewModel.RefreshWidgetsRequested += ModernShellPage_RefreshWidgetsRequested;

			_navigationInteractionTracker = new NavigationInteractionTracker(this, BackIcon, ForwardIcon);
			_navigationInteractionTracker.NavigationRequested += OverscrollNavigationRequested;
		}

		private void ModernShellPage_RefreshWidgetsRequested(object sender, EventArgs e)
		{
			if (ItemDisplayFrame?.Content is HomePage currentPage)
				currentPage.ViewModel.RefreshWidgetList();
		}

		protected override void OnNavigatedTo(NavigationEventArgs eventArgs)
		{
			base.OnNavigatedTo(eventArgs);

			if (eventArgs.Parameter is string navPath)
				NavParams = new NavigationParams { NavPath = navPath };
			else if (eventArgs.Parameter is NavigationParams navParams)
				NavParams = navParams;
		}

		protected override void ShellPage_NavigationRequested(object sender, PathNavigationEventArgs e)
		{
			ItemDisplayFrame.Navigate(InstanceViewModel.FolderSettings.GetLayoutType(e.ItemPath), new NavigationArguments()
			{
				NavPathParam = e.ItemPath,
				AssociatedTabInstance = this
			});
		}

		protected override void OnNavigationParamsChanged()
		{
			if (string.IsNullOrEmpty(NavParams?.NavPath) || NavParams.NavPath == "Home")
			{
				ItemDisplayFrame.Navigate(
					typeof(HomePage),
					new NavigationArguments()
					{
						NavPathParam = NavParams?.NavPath,
						AssociatedTabInstance = this
					}, new SuppressNavigationTransitionInfo());
			}
			else if (NavParams.NavPath == "RecycleBin")
			{
				ItemDisplayFrame.Navigate(
					typeof(RecycleBinPath),
					new NavigationArguments()
					{
						NavPathParam = NavParams?.NavPath,
						AssociatedTabInstance = this
					}, new SuppressNavigationTransitionInfo());
			}
			else if (NavParams.NavPath == "ReleaseNotes")
			{
				ItemDisplayFrame.Navigate(
					typeof(ReleaseNotesPage),
					new NavigationArguments()
					{
						NavPathParam = NavParams?.NavPath,
						AssociatedTabInstance = this
					}, new SuppressNavigationTransitionInfo());
			}
			else if (NavParams.NavPath == "Settings")
			{
				ItemDisplayFrame.Navigate(
					typeof(SettingsPath),
					new NavigationArguments()
					{
						NavPathParam = NavParams?.NavPath,
						AssociatedTabInstance = this
					}, new SuppressNavigationTransitionInfo());
			}
			else if (NavParams.NavPath == "Desktop")
			{
				ItemDisplayFrame.Navigate(
					typeof(DesktopPath),
					new NavigationArguments()
					{
						NavPathParam = NavParams?.NavPath,
						AssociatedTabInstance = this
					}, new SuppressNavigationTransitionInfo());
			}
			else if (NavParams.NavPath == "Downloads")
			{
				ItemDisplayFrame.Navigate(
					typeof(DownloadsPath),
					new NavigationArguments()
					{
						NavPathParam = NavParams?.NavPath,
						AssociatedTabInstance = this
					}, new SuppressNavigationTransitionInfo());
			}
			else if (NavParams.NavPath == "Pictures")
			{
				ItemDisplayFrame.Navigate(
					typeof(PicturesPath),
					new NavigationArguments()
					{
						NavPathParam = NavParams?.NavPath,
						AssociatedTabInstance = this
					}, new SuppressNavigationTransitionInfo());
			}
			else if (NavParams.NavPath == "Music")
			{
				ItemDisplayFrame.Navigate(
					typeof(MusicPath),
					new NavigationArguments()
					{
						NavPathParam = NavParams?.NavPath,
						AssociatedTabInstance = this
					}, new SuppressNavigationTransitionInfo());
			}
			else if (NavParams.NavPath == "Videos")
			{
				ItemDisplayFrame.Navigate(
					typeof(VideosPath),
					new NavigationArguments()
					{
						NavPathParam = NavParams?.NavPath,
						AssociatedTabInstance = this
					}, new SuppressNavigationTransitionInfo());
			}
			else if (NavParams.NavPath == "Documents")
			{
				ItemDisplayFrame.Navigate(
					typeof(DocumentsPath),
					new NavigationArguments()
					{
						NavPathParam = NavParams?.NavPath,
						AssociatedTabInstance = this
					}, new SuppressNavigationTransitionInfo());
			}
			else if (NavParams.NavPath == "Network")
			{
				ItemDisplayFrame.Navigate(
					typeof(NetworkFolderPath),
					new NavigationArguments()
					{
						NavPathParam = NavParams?.NavPath,
						AssociatedTabInstance = this
					}, new SuppressNavigationTransitionInfo());
			}
			else if (NavParams.NavPath == "ThisPC")
			{
				ItemDisplayFrame.Navigate(
					typeof(MyComputerPath),
					new NavigationArguments()
					{
						NavPathParam = NavParams?.NavPath,
						AssociatedTabInstance = this
					}, new SuppressNavigationTransitionInfo());
			}

			else
			{
				var isTagSearch = NavParams.NavPath.StartsWith("tag:");

				ItemDisplayFrame.Navigate(
					InstanceViewModel.FolderSettings.GetLayoutType(NavParams.NavPath),
					new NavigationArguments()
					{
						NavPathParam = NavParams.NavPath,
						SelectItems = !string.IsNullOrWhiteSpace(NavParams?.SelectItem) ? new[] { NavParams.SelectItem } : null,
						IsSearchResultPage = isTagSearch,
						SearchPathParam = isTagSearch ? "Home" : null,
						SearchQuery = isTagSearch ? NavParams.NavPath : null,
						AssociatedTabInstance = this
					});
			}
		}

		protected override async void ViewModel_WorkingDirectoryModified(object sender, WorkingDirectoryModifiedEventArgs e)
		{
			if (string.IsNullOrWhiteSpace(e.Path))
				return;

			if (e.IsLibrary)
				await UpdatePathUIToWorkingDirectoryAsync(null, e.Name);
			else
				await UpdatePathUIToWorkingDirectoryAsync(e.Path);
		}

		private async void ItemDisplayFrame_Navigated(object sender, NavigationEventArgs e)
		{
			ContentPage = await GetContentOrNullAsync();
			if (!ToolbarViewModel.SearchBox.WasQuerySubmitted)
			{
				ToolbarViewModel.SearchBox.Query = string.Empty;
				ToolbarViewModel.IsSearchBoxVisible = false;
			}

			ToolbarViewModel.UpdateAdditionalActions();
			if (ItemDisplayFrame.CurrentSourcePageType == (typeof(DetailsLayoutPage))
				|| ItemDisplayFrame.CurrentSourcePageType == typeof(GridLayoutPage))
			{
				// Reset DataGrid Rows that may be in "cut" command mode
				ContentPage.ResetItemOpacity();
			}

			var parameters = e.Parameter as NavigationArguments;
			var isTagSearch = parameters.NavPathParam is not null && parameters.NavPathParam.StartsWith("tag:");
			TabBarItemParameter = new()
			{
				InitialPageType = typeof(ModernShellPage),
				NavigationParameter = parameters.IsSearchResultPage && !isTagSearch ? parameters.SearchPathParam : parameters.NavPathParam
			};

			if (parameters.IsLayoutSwitch)
				FilesystemViewModel_DirectoryInfoUpdated(sender, EventArgs.Empty);
			_navigationInteractionTracker.CanNavigateBackward = CanNavigateBackward;
			_navigationInteractionTracker.CanNavigateForward = CanNavigateForward;
		}

		private async void KeyboardAccelerator_Invoked(KeyboardAccelerator sender, KeyboardAcceleratorInvokedEventArgs args)
		{
			args.Handled = true;
			var tabInstance =
				CurrentPageType == typeof(DetailsLayoutPage) ||
				CurrentPageType == typeof(GridLayoutPage);

			var ctrl = args.KeyboardAccelerator.Modifiers.HasFlag(VirtualKeyModifiers.Control);
			var shift = args.KeyboardAccelerator.Modifiers.HasFlag(VirtualKeyModifiers.Shift);
			var alt = args.KeyboardAccelerator.Modifiers.HasFlag(VirtualKeyModifiers.Menu);

			switch (c: ctrl, s: shift, a: alt, t: tabInstance, k: args.KeyboardAccelerator.Key)
			{
				// Ctrl + V, Paste
				case (true, false, false, true, VirtualKey.V):
					if (!ToolbarViewModel.IsEditModeEnabled && !ContentPage.IsRenamingItem && !InstanceViewModel.IsPageTypeSearchResults && !ToolbarViewModel.SearchHasFocus)
						await UIFilesystemHelpers.PasteItemAsync(ShellViewModel.WorkingDirectory, this);
					break;
			}
		}

		private void OverscrollNavigationRequested(object? sender, OverscrollNavigationEventArgs e)
		{
			switch (e)
			{
				case OverscrollNavigationEventArgs.Forward:
					Forward_Click();
					break;

				case OverscrollNavigationEventArgs.Back:
					Back_Click();
					break;
			}
		}

		public override void Back_Click()
		{
			ToolbarViewModel.CanGoBack = false;
			if (!ItemDisplayFrame.CanGoBack)
				return;

			base.Back_Click();
		}

		public override void Forward_Click()
		{
			ToolbarViewModel.CanGoForward = false;
			if (!ItemDisplayFrame.CanGoForward)
				return;

			base.Forward_Click();
		}

		public override void Up_Click()
		{
			if (!ToolbarViewModel.CanNavigateToParent)
				return;

			ToolbarViewModel.CanNavigateToParent = false;
			if (string.IsNullOrEmpty(ShellViewModel?.WorkingDirectory))
				return;

			bool isPathRooted = string.Equals(ShellViewModel.WorkingDirectory, PathNormalization.GetPathRoot(ShellViewModel.WorkingDirectory), StringComparison.OrdinalIgnoreCase);
			if (isPathRooted)
			{
				ItemDisplayFrame.Navigate(
					typeof(HomePage),
					new NavigationArguments()
					{
						NavPathParam = "Home",
						AssociatedTabInstance = this
					},
					new SuppressNavigationTransitionInfo());
			}
			else
			{
				string parentDirectoryOfPath = ShellViewModel.WorkingDirectory.TrimEnd('\\', '/');

				var lastSlashIndex = parentDirectoryOfPath.LastIndexOf("\\", StringComparison.Ordinal);
				if (lastSlashIndex == -1)
					lastSlashIndex = parentDirectoryOfPath.LastIndexOf("/", StringComparison.Ordinal);
				if (lastSlashIndex != -1)
					parentDirectoryOfPath = ShellViewModel.WorkingDirectory.Remove(lastSlashIndex);
				if (parentDirectoryOfPath.EndsWith(':'))
					parentDirectoryOfPath += '\\';

				SelectSidebarItemFromPath();
				ItemDisplayFrame.Navigate(
					InstanceViewModel.FolderSettings.GetLayoutType(parentDirectoryOfPath),
					new NavigationArguments()
					{
						NavPathParam = parentDirectoryOfPath,
						AssociatedTabInstance = this
					},
					new SuppressNavigationTransitionInfo());
			}
		}

		public override void Dispose()
		{
			ToolbarViewModel.RefreshWidgetsRequested -= ModernShellPage_RefreshWidgetsRequested;
			_navigationInteractionTracker.NavigationRequested -= OverscrollNavigationRequested;
			_navigationInteractionTracker.Dispose();

			base.Dispose();
		}

		public override void NavigateHome()
		{
			ItemDisplayFrame.Navigate(
				typeof(HomePage),
				new NavigationArguments()
				{
					NavPathParam = "Home",
					AssociatedTabInstance = this
				},
				new SuppressNavigationTransitionInfo());
		}

		public override void NavigateToRecycleBin()
		{
			ItemDisplayFrame.Navigate(
				typeof(RecycleBinPath),
				new NavigationArguments()
				{
					NavPathParam = "RecycleBin",
					AssociatedTabInstance = this
				},
				new SuppressNavigationTransitionInfo());
		}

		public override void NavigateToReleaseNotes()
		{
			ItemDisplayFrame.Navigate(
				typeof(ReleaseNotesPage),
				new NavigationArguments()
				{
					NavPathParam = "ReleaseNotes",
					AssociatedTabInstance = this
				},
				new SuppressNavigationTransitionInfo());
		}

		public override void NavigateToSettings()
		{
			ItemDisplayFrame.Navigate(
				typeof(SettingsPath),
				new NavigationArguments()
				{
					NavPathParam = "Settings",
					AssociatedTabInstance = this
				},
				new SuppressNavigationTransitionInfo());
		}

		public override void NavigateToDesktop()
		{
			ItemDisplayFrame.Navigate(
				typeof(DesktopPath),
				new NavigationArguments()
				{
					NavPathParam = "Desktop",
					AssociatedTabInstance = this
				},
				new SuppressNavigationTransitionInfo());
		}

		public override void NavigateToDownloads()
		{
			ItemDisplayFrame.Navigate(
				typeof(DownloadsPath),
				new NavigationArguments()
				{
					NavPathParam = "Downloads",
					AssociatedTabInstance = this
				},
				new SuppressNavigationTransitionInfo());
		}

		public override void NavigateToPictures()
		{
			ItemDisplayFrame.Navigate(
				typeof(PicturesPath),
				new NavigationArguments()
				{
					NavPathParam = "Pictures",
					AssociatedTabInstance = this
				},
				new SuppressNavigationTransitionInfo());
		}

		public override void NavigateToMusic()
		{
			ItemDisplayFrame.Navigate(
				typeof(MusicPath),
				new NavigationArguments()
				{
					NavPathParam = "Music",
					AssociatedTabInstance = this
				},
				new SuppressNavigationTransitionInfo());
		}

		public override void NavigateToVideos()
		{
			ItemDisplayFrame.Navigate(
				typeof(VideosPath),
				new NavigationArguments()
				{
					NavPathParam = "Videos",
					AssociatedTabInstance = this
				},
				new SuppressNavigationTransitionInfo());
		}

		public override void NavigateToDocuments()
		{
			ItemDisplayFrame.Navigate(
				typeof(DocumentsPath),
				new NavigationArguments()
				{
					NavPathParam = "Documents",
					AssociatedTabInstance = this
				},
				new SuppressNavigationTransitionInfo());
		}

		public override void NavigateToNetwork()
		{
			ItemDisplayFrame.Navigate(
				typeof(NetworkFolderPath),
				new NavigationArguments()
				{
					NavPathParam = "Network",
					AssociatedTabInstance = this
				},
				new SuppressNavigationTransitionInfo());
		}

		public override void NavigateToThisPC()
		{
			ItemDisplayFrame.Navigate(
				typeof(MyComputerPath),
				new NavigationArguments()
				{
					NavPathParam = "ThisPC",
					AssociatedTabInstance = this
				},
				new SuppressNavigationTransitionInfo());
		}

		public override void NavigateToPath(string? navigationPath, Type? sourcePageType, NavigationArguments? navArgs = null)
		{
			ShellViewModel.FilesAndFoldersFilter = null;

			if (sourcePageType is null && !string.IsNullOrEmpty(navigationPath))
				sourcePageType = InstanceViewModel.FolderSettings.GetLayoutType(navigationPath);

			if (navArgs is not null && navArgs.AssociatedTabInstance is not null)
			{
				ItemDisplayFrame.Navigate(
					sourcePageType,
					navArgs,
					new SuppressNavigationTransitionInfo());
			}
			else
			{
				if ((string.IsNullOrEmpty(navigationPath) ||
					string.IsNullOrEmpty(ShellViewModel?.WorkingDirectory) ||
					navigationPath.TrimEnd(Path.DirectorySeparatorChar).Equals(
						ShellViewModel.WorkingDirectory.TrimEnd(Path.DirectorySeparatorChar),
						StringComparison.OrdinalIgnoreCase)) &&
					(TabBarItemParameter?.NavigationParameter is not string navArg ||
					string.IsNullOrEmpty(navArg) ||
					!navArg.StartsWith("tag:"))) // Return if already selected
				{
					if (InstanceViewModel?.FolderSettings is LayoutPreferencesManager fsModel)
						fsModel.IsLayoutModeChanging = false;

					return;
				}

				if (string.IsNullOrEmpty(navigationPath))
					return;

				ItemDisplayFrame.Navigate(
					sourcePageType,
					new NavigationArguments()
					{
						NavPathParam = navigationPath,
						AssociatedTabInstance = this
					},
					new SuppressNavigationTransitionInfo());
			}

			ToolbarViewModel.PathControlDisplayText = ShellViewModel.WorkingDirectory;
		}
	}
}
