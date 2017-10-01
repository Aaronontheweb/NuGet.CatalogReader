using System;
using System.Net;
using System.Threading.Tasks;
using Emgarten.Common;
using Microsoft.Extensions.CommandLineUtils;
using NuGet.Common;
using NuGet.Protocol;
using NuGet.Protocol.Core.Types;
using NuGet.Versioning;

namespace NuGet.CatalogValidator
{
    public class Program
    {
        public static int Main(string[] args)
        {
            var logLevel = LogLevel.Information;

            if (CmdUtils.IsDebugModeEnabled())
            {
                logLevel = LogLevel.Debug;
            }

            var log = new ConsoleLogger(logLevel);

            var task = MainCore(args, log);
            return task.Result;
        }

        public static Task<int> MainCore(string[] args, ILogger log)
        {
            return MainCore(args, httpSource: null, log: log);
        }

        public static Task<int> MainCore(string[] args, HttpSource httpSource, ILogger log)
        {
            CmdUtils.LaunchDebuggerIfSet(ref args, log);

            var app = new CommandLineApplication()
            {
                Name = "NuGet.CatalogValidator",
                FullName = "nuget mirror"
            };
            app.HelpOption(Constants.HelpOption);
            app.VersionOption("--version", (new NuGetVersion(CmdUtils.GetAssemblyVersion())).ToNormalizedString());
            app.Description = "Validate a nuget v3 feed.";

            Configure();

            ValidateCommand.Register(app, httpSource, log);

            app.OnExecute(() =>
            {
                app.ShowHelp();
                return 1;
            });

            var exitCode = 1;

            try
            {
                exitCode = app.Execute(args);
            }
            catch (CommandParsingException ex)
            {
                ex.Command.ShowHelp();
            }
            catch (Exception ex)
            {
                ExceptionUtils.LogException(ex, log);
            }

            return Task.FromResult(exitCode);
        }

        private static void Configure()
        {
            // Set connection limit
            if (!RuntimeEnvironmentHelper.IsMono)
            {
                ServicePointManager.DefaultConnectionLimit = 64;
            }
            else
            {
                // Keep mono limited to a single download to avoid issues.
                ServicePointManager.DefaultConnectionLimit = 1;
            }

            // Limit SSL
            ServicePointManager.SecurityProtocol =
                SecurityProtocolType.Tls |
                SecurityProtocolType.Tls11 |
                SecurityProtocolType.Tls12;

            UserAgent.SetUserAgentString(new UserAgentStringBuilder("NuGet.CatalogValidator"));
        }
    }
}