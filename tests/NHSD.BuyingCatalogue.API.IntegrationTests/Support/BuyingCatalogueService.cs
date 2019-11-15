using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using FluentAssertions;
using NHSD.BuyingCatalogue.Testing.Data;
using NHSD.BuyingCatalogue.Testing.Tools;

namespace NHSD.BuyingCatalogue.API.IntegrationTests.Drivers
{
    internal static class BuyingCatalogueService
    {
        private const string WaitServerUrl = "http://localhost:8080/health/dependencies";
        private const string DockerFileCommandLineArgument = "-f docker-compose.yml -f docker-compose.integration.yml";

        private static readonly string SolutionWorkingDirectory = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..\\..\\..\\..\\..\\"));

        private static readonly string TempOutDirectory =
            Path.GetFullPath(Path.Combine(SolutionWorkingDirectory, ".\\out"));
        private static readonly TimeSpan TestTimeout = TimeSpan.FromSeconds(60);

        internal static async Task StartAsync(string serviceConnectionString)
        {
            await PublishAsync();
            await DockerComposeProcess.Create(SolutionWorkingDirectory, $"{DockerFileCommandLineArgument} up -d --build"
                , new KeyValuePair<string, string>("NHSD_BUYINGCATALOGUE_DB", serviceConnectionString)
                , new KeyValuePair<string, string>("NHSD_BUYINGCATALOGUE_DB_PASSWORD", DataConstants.SAPassword)).ExecuteAsync((x) => Debug.WriteLine(x), (x) => Debug.WriteLine(x));
        }

        internal static async Task WaitAsync()
        {
            var started = await HttpClientAwaiter.WaitForGetAsync(WaitServerUrl, TestTimeout);
            if (!started)
            {
                throw new Exception($"Start Buying Catalogue API failed, could not get a successful health status from '{WaitServerUrl}' after trying for '{TestTimeout}'");
            }
        }

        internal static async Task StopAsync()
        {
            await DockerComposeProcess.Create(SolutionWorkingDirectory, $"{DockerFileCommandLineArgument} down -v").ExecuteAsync((x) => Debug.WriteLine(x));
        }

        internal static async Task PublishAsync()
        {
            await Task.Run(() =>
            {
                Directory.CreateDirectory(TempOutDirectory);
                var process = Process.Start(new ProcessStartInfo
                {
                    FileName = "dotnet",
                    Arguments = $"publish {SolutionWorkingDirectory+"\\NHSD.BuyingCatalogue.sln"} --configuration Release --output {TempOutDirectory}"
                });

                process.WaitForExit();
                process.ExitCode.Should().Be(0);
            });
        }

        internal static async Task CleanPublishDirectory()
        {
            await Task.Run( () =>
                Directory.Delete(TempOutDirectory, true));
        }

    }
}
