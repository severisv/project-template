namespace HappyBever.Features.App
open Microsoft.AspNetCore.Mvc
open HappyBever

type AppController () =
    inherit Controller()

    // Matcher alle url'er som ikke begynner på /api
    [<Route("{*url:regex(^(?!api).*$)}")>]
    [<Route("")>]
    member this.Index () =
          
        this.View("~/wwwroot/Index.cshtml")


    [<Route("Error")>]
     member this.Error () =
        "Det oppstod en feil"

  