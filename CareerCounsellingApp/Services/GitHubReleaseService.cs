using CareerCounsellingApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace CareerCounsellingApp.Services
{
    public class GitHubReleaseService
    {
        private readonly HttpClient httpClient;
        public GitHubReleaseService()
        {
            httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("CareerCounsellingApp");
        }
        public async Task<GitHubRelease?> GetLatestReleaseAsync()
        {
            var url = "https://github.com/Akhilmohan97/CareerCounsellingApp/releases/latest";
            var json=await httpClient.GetStringAsync(url);
            return JsonSerializer.Deserialize<GitHubRelease>(json);
        }
    }
}
