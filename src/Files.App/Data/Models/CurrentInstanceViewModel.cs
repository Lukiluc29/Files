// Copyright (c) Files Community
// Licensed under the MIT License.

namespace Files.App.Data.Models
{
	public sealed partial class CurrentInstanceViewModel : ObservableObject
	{
		// TODO:
		//  In the future, we should consolidate these public variables into
		//  a single enum property providing simplified customization of the
		//  values being manipulated inside the setter blocks

		public LayoutPreferencesManager FolderSettings { get; }

		public CurrentInstanceViewModel()
		{
			FolderSettings = new LayoutPreferencesManager();
		}

		public CurrentInstanceViewModel(FolderLayoutModes rootLayoutMode)
		{
			FolderSettings = new LayoutPreferencesManager(rootLayoutMode);
		}

		private bool isPageTypeSearchResults = false;
		public bool IsPageTypeSearchResults
		{
			get => isPageTypeSearchResults;
			set
			{
				SetProperty(ref isPageTypeSearchResults, value);
				OnPropertyChanged(nameof(CanCreateFileInPage));
				OnPropertyChanged(nameof(CanCopyPathInPage));
			}
		}

		private string currentSearchQuery;
		public string CurrentSearchQuery
		{
			get => currentSearchQuery;
			set => SetProperty(ref currentSearchQuery, value);
		}

		private bool isPageTypeNotHome = false;
		public bool IsPageTypeNotHome
		{
			get => isPageTypeNotHome;
			set
			{
				SetProperty(ref isPageTypeNotHome, value);
				OnPropertyChanged(nameof(CanCreateFileInPage));
				OnPropertyChanged(nameof(CanCopyPathInPage));
			}
		}

		private bool isPageTypeReleaseNotes = false;
		public bool IsPageTypeReleaseNotes
		{
			get => isPageTypeReleaseNotes;
			set
			{
				SetProperty(ref isPageTypeReleaseNotes, value);
				OnPropertyChanged(nameof(CanCreateFileInPage));
				OnPropertyChanged(nameof(CanCopyPathInPage));
			}
		}

		private bool isPageTypeSettings = false;
		public bool IsPageTypeSettings
		{
			get => isPageTypeSettings;
			set
			{
				SetProperty(ref isPageTypeSettings, value);
				OnPropertyChanged(nameof(CanCreateFileInPage));
				OnPropertyChanged(nameof(CanCopyPathInPage));
			}
		}

		private bool isPageTypeDesktop = false;
		public bool IsPageTypeDesktop
		{
			get => isPageTypeDesktop;
			set
			{
				SetProperty(ref isPageTypeDesktop, value);
				OnPropertyChanged(nameof(CanCreateFileInPage));
				OnPropertyChanged(nameof(CanCopyPathInPage));
			}
		}

		private bool isPageTypeDownloads = false;
		public bool IsPageTypeDownloads
		{
			get => isPageTypeDownloads;
			set
			{
				SetProperty(ref isPageTypeDownloads, value);
				OnPropertyChanged(nameof(CanCreateFileInPage));
				OnPropertyChanged(nameof(CanCopyPathInPage));
			}
		}

		private bool isPageTypePictures = false;
		public bool IsPageTypePictures
		{
			get => isPageTypePictures;
			set
			{
				SetProperty(ref isPageTypePictures, value);
				OnPropertyChanged(nameof(CanCreateFileInPage));
				OnPropertyChanged(nameof(CanCopyPathInPage));
			}
		}

		private bool isPageTypeMusic = false;
		public bool IsPageTypeMusic
		{
			get => isPageTypeMusic;
			set
			{
				SetProperty(ref isPageTypeMusic, value);
				OnPropertyChanged(nameof(CanCreateFileInPage));
				OnPropertyChanged(nameof(CanCopyPathInPage));
			}
		}

		private bool isPageTypeVideos = false;
		public bool IsPageTypeVideos
		{
			get => isPageTypeVideos;
			set
			{
				SetProperty(ref isPageTypeVideos, value);
				OnPropertyChanged(nameof(CanCreateFileInPage));
				OnPropertyChanged(nameof(CanCopyPathInPage));
			}
		}

		private bool isPageTypeDocuments = false;
		public bool IsPageTypeDocuments
		{
			get => isPageTypeDocuments;
			set
			{
				SetProperty(ref isPageTypeDocuments, value);
				OnPropertyChanged(nameof(CanCreateFileInPage));
				OnPropertyChanged(nameof(CanCopyPathInPage));
			}
		}

		private bool isPageTypeNetwork = false;
		public bool IsPageTypeNetwork
		{
			get => isPageTypeNetwork;
			set
			{
				SetProperty(ref isPageTypeNetwork, value);
				OnPropertyChanged(nameof(CanCreateFileInPage));
				OnPropertyChanged(nameof(CanCopyPathInPage));
			}
		}

		private bool isPageTypeThisPC = false;
		public bool IsPageTypeThisPC
		{
			get => isPageTypeThisPC;
			set
			{
				SetProperty(ref isPageTypeThisPC, value);
				OnPropertyChanged(nameof(CanCreateFileInPage));
				OnPropertyChanged(nameof(CanCopyPathInPage));
			}
		}

		private bool isPageTypeMtpDevice = false;
		public bool IsPageTypeMtpDevice
		{
			get => isPageTypeMtpDevice;
			set
			{
				SetProperty(ref isPageTypeMtpDevice, value);
				OnPropertyChanged(nameof(CanCreateFileInPage));
				OnPropertyChanged(nameof(CanCopyPathInPage));
			}
		}

		private bool isPageTypeRecycleBin = false;
		public bool IsPageTypeRecycleBin
		{
			get => isPageTypeRecycleBin;
			set
			{
				SetProperty(ref isPageTypeRecycleBin, value);
				OnPropertyChanged(nameof(CanCreateFileInPage));
				OnPropertyChanged(nameof(CanCopyPathInPage));
				OnPropertyChanged(nameof(CanTagFilesInPage));
			}
		}

		private bool isPageTypeFtp = false;
		public bool IsPageTypeFtp
		{
			get => isPageTypeFtp;
			set
			{
				SetProperty(ref isPageTypeFtp, value);
				OnPropertyChanged(nameof(CanCreateFileInPage));
				OnPropertyChanged(nameof(CanTagFilesInPage));
			}
		}

		private bool isPageTypeCloudDrive = false;
		public bool IsPageTypeCloudDrive
		{
			get => isPageTypeCloudDrive;
			set
			{
				SetProperty(ref isPageTypeCloudDrive, value);
			}
		}

		private bool isPageTypeZipFolder = false;
		public bool IsPageTypeZipFolder
		{
			get => isPageTypeZipFolder;
			set
			{
				SetProperty(ref isPageTypeZipFolder, value);
				OnPropertyChanged(nameof(CanCreateFileInPage));
				OnPropertyChanged(nameof(CanTagFilesInPage));
			}
		}

		private bool isPageTypeLibrary = false;
		public bool IsPageTypeLibrary
		{
			get => isPageTypeLibrary;
			set
			{
				SetProperty(ref isPageTypeLibrary, value);
			}
		}

		public bool CanCopyPathInPage
		{
			get => !isPageTypeMtpDevice && !isPageTypeRecycleBin && isPageTypeNotHome && !isPageTypeSearchResults && !isPageTypeFtp && !isPageTypeZipFolder && !IsPageTypeReleaseNotes && !IsPageTypeSettings && !isPageTypeDesktop && !isPageTypeDownloads && !isPageTypePictures && !isPageTypeMusic && !isPageTypeVideos && !isPageTypeDocuments && !isPageTypeNetwork && !isPageTypeThisPC;
		}

		public bool CanCreateFileInPage
		{
			get => !isPageTypeMtpDevice && !isPageTypeRecycleBin && isPageTypeNotHome && !isPageTypeSearchResults && !isPageTypeFtp && !isPageTypeZipFolder && !IsPageTypeReleaseNotes && !IsPageTypeSettings && !isPageTypeDesktop && !isPageTypeDownloads && !isPageTypePictures && !isPageTypeMusic && !isPageTypeVideos && !isPageTypeDocuments && !isPageTypeNetwork && !isPageTypeThisPC;

		}

		public bool CanTagFilesInPage
		{
			get => !isPageTypeRecycleBin && !isPageTypeFtp && !isPageTypeZipFolder;
		}

		private bool isGitRepository;
		public bool IsGitRepository
		{
			get => isGitRepository;
			set
			{
				SetProperty(ref isGitRepository, value);
			}
		}

		private string? gitRepositoryPath;
		public string? GitRepositoryPath
		{
			get => gitRepositoryPath;
			set
			{
				SetProperty(ref gitRepositoryPath, value);
			}
		}

		private string gitBranchName = string.Empty;
		public string GitBranchName
		{
			get => gitBranchName;
			set => SetProperty(ref gitBranchName, value);
		}
	}
}
