namespace HappyBever.Configuration

open Microsoft.Extensions.DependencyInjection
open HappyBever.Files

module DependencyRegistration =

    type IServiceCollection with
        member services.RegisterDepdendencies () =
 
            // IO
            services.AddScoped<FileStorage, FileStorage>() |> ignore
        
            // Return
            services


