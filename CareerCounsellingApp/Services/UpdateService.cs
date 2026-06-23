using System;
using System.Threading.Tasks;
using Velopack;
using Velopack.Sources;

namespace CareerCounsellingApp.Services;

public class UpdateService
{
    private readonly UpdateManager _updateManager;

    public UpdateService()
    {
        var source = new GithubSource(
            "https://github.com/Akhilmohan97/CareerCounsellingApp",
            null,
            false);

        _updateManager = new UpdateManager(source);
    }

    public async Task<UpdateInfo?> CheckForUpdatesAsync()
    {
        return await _updateManager.CheckForUpdatesAsync();
    }

    public async Task DownloadAndInstallAsync(
        UpdateInfo update,
        Action<int> progress)
    {
        await _updateManager.DownloadUpdatesAsync(
            update,
            progress);

        _updateManager.ApplyUpdatesAndRestart(update);
    }
}