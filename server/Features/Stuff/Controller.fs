namespace HappyBever.Features.Stuff

open Microsoft.AspNetCore.Mvc

open Queries

[<Route("api/stuff")>]
type StuffController () =
    inherit Controller()

    [<Route("")>]
    member this.Get () =

            getStuff ()

            