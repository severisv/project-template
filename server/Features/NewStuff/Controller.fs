namespace HappyBever.Features.Stuff

open Microsoft.AspNetCore.Mvc

open Queries

[<Route("api/newstuff")>]
type NewStuffController () =
    inherit Controller()

    [<Route("")>]
    member this.Get () =

            getStuff ()

            