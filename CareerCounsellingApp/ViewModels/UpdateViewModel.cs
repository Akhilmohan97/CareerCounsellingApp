using CareerCounsellingApp.Helpers;
using CareerCounsellingApp.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Velopack;

namespace CareerCounsellingApp.ViewModels;

public partial class UpdateViewModel : ViewModelBase
{
    public UpdateViewModel(UpdateInfo update)
    {
        _updateInfo=update;
        CurrentVersion = VersionHelper.CurrentVersion;

        LatestVersion = update.TargetFullRelease.Version.ToString();
    }
    private readonly UpdateService _service = new();
    private readonly GitHubReleaseService _githubService = new();

    private UpdateInfo? _updateInfo;

    [ObservableProperty]
    private string currentVersion = VersionHelper.CurrentVersion;

    [ObservableProperty]
    private string latestVersion = "";

    [ObservableProperty]
    private string status = "Ready";

    [ObservableProperty]
    private double progress;

    [ObservableProperty]
    private bool updateAvailable;

    [ObservableProperty]
    private bool isDownloading;
    [ObservableProperty]
    private string releaseNotes = string.Empty;
    [RelayCommand]
    public async Task CheckForUpdates()
    {
        Status = "Checking GitHub...";

        var release =
            await _githubService.GetLatestReleaseAsync();

        if (release != null)
        {
            LatestVersion = release.TagName;

            ReleaseNotes = release.Body;

        }

        _updateInfo = await _service.CheckForUpdatesAsync();

        if (_updateInfo == null)
        {
            Status = "You're already using the latest version.";
            return;
        }

        LatestVersion =
            _updateInfo.TargetFullRelease.Version.ToString();

        UpdateAvailable = true;

        Status = "New version available!";
    }

    [RelayCommand]
    public async Task InstallUpdate()
    {
        if (_updateInfo == null)
            return;

        IsDownloading = true;

        await _service.DownloadAndInstallAsync(
            _updateInfo,
            p => Progress = p);

        IsDownloading = false;
    }
}
