namespace HappyBever

open System
open System.IO
open Microsoft.AspNetCore.Hosting
open Microsoft.AspNetCore.Builder
open Microsoft.AspNetCore.Hosting
open Microsoft.Extensions.Configuration
open Microsoft.Extensions.DependencyInjection
open Microsoft.Extensions.Logging
open Microsoft.Extensions.Options
open HappyBever.Configuration
open HappyBever.Configuration.DependencyRegistration
open HappyBever.Configuration.Auth
open HappyBever.Files
open SlackLogger

module Program =

    [<EntryPoint>]
    let main args =
        let host =
            WebHostBuilder()
                .UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .ConfigureLogging(fun hostingContext logging ->
                                 logging.AddConfiguration(hostingContext.Configuration.GetSection("Logging")) |> ignore
                                 logging.AddConsole() |> ignore
                                 logging.AddDebug() |> ignore
                                 logging.AddSlack(fun options ->
                                    options.NotificationLevel <- LogLevel.None
                                    options.LogLevel <- LogLevel.Warning
                                    options.WebhookUrl <- "https://web.hook.url"
                                 ) |> ignore
                )
                .ConfigureAppConfiguration(fun hostingContext config ->
                                config.AddJsonFile("appsettings.json", optional = false, reloadOnChange = true) |> ignore
                                config.AddJsonFile((sprintf "appsettings.%s.json" (hostingContext.HostingEnvironment.EnvironmentName)), optional = true) |> ignore
                                config.AddEnvironmentVariables() |> ignore
                )
                .ConfigureServices(fun context services ->

                                services.AddOptions() |> ignore
                                services.Configure<Auth0Settings>(context.Configuration.GetSection("Auth0")) |> ignore
                                services.Configure<FileStorageOptions>(context.Configuration.GetSection("FileStorage")) |> ignore
                )
                .UseApplicationInsights()
                .UseAzureAppServices()
                .UseStartup<Startup>()
                .Build()

        host.Run()

        0
