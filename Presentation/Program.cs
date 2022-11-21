using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Application;
using Core.Application;
using Core.Domain;
using Infrastructure.Shared;
using Infrastructure.Shared.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;

namespace Tretton37
{

    class Program
    {
        static async Task Main(string[] args)
        {
            var builder = GetConfigurationBuilder();

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(builder.Build())
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .CreateLogger();

            Log.Logger.Information("******");
            Log.Logger.Information("** The application has started **");
            Log.Logger.Information("******");

            var config = builder.Build();
            var startDirForFolderCreation = config.GetValue<string>("StartDirForFolderCreation") + @"/TrettonHTMLPages";

            var host = Host.CreateDefaultBuilder()
                .ConfigureServices((context, services) =>
                {
                    // DI Registrations
                    services.AddTransient<ITreeTraversalService, TreeTraversalService>();
                    services.AddTransient<IHTTPClientService, HTTPClientService>();
                    services.AddTransient<IIOService>(x => 
                    {
                        var logger = x.GetRequiredService<ILogger<IOService>>();
                        var dataService = new IOService(logger, startDirForFolderCreation);
                        return dataService;
                    });
                })
                .UseSerilog()
                .Build();

            var traversalService = host.Services.GetService<ITreeTraversalService>();
            var ioService = host.Services.GetService<IIOService>();

            var baseUrl = config.GetValue<string>("BaseUrl");

            var usedUrls = new List<string>();
            var treeNode = new TreeNode(baseUrl, baseUrl, usedUrls);

            await ioService.ClearDirectory();

            await Task.Run( async () =>
            {
                await traversalService.TraverseAsync(treeNode);
            });
            

            Log.Logger.Information("******");
            Log.Logger.Information($"*** Number of nodes that has been processed: {traversalService.GetNodes().Count} ***");
            Log.Logger.Information("");
            Log.Logger.Information("** The application has Ended **");
            Log.Logger.Information("******");


        }
        static IConfigurationBuilder GetConfigurationBuilder()
        {
            var builder = new ConfigurationBuilder();

            builder.SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPENTCORE_ENVIRONMENT") ?? Environments.Production}.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();

            return builder;
        }
    }
}
