
namespace HappyBever.Features.App
open System
open System.Threading.Tasks
open Microsoft.AspNetCore.Mvc
open Microsoft.AspNetCore.Http.Authentication
open Microsoft.AspNetCore.Authorization
open Microsoft.AspNetCore.Authentication.Cookies
open Microsoft.Extensions.Options
open HappyBever.Configuration
open Microsoft.AspNetCore.Authentication


[<Route("account")>]
type AccountController (settings: IOptions<Auth0Settings>) =
    inherit Controller()


    [<Route("login")>]
    member this.Login (returnUrl : string) =
        this.HttpContext.ChallengeAsync("Auth0", AuthenticationProperties(RedirectUri = 
                                                    match returnUrl with
                                                        | null -> "/"
                                                        | _ -> returnUrl
                                            ))      


    [<Authorize>]
    [<Route("logout")>]
    member this.Logout() =

        // Construct the post-logout URL (i.e. where we'll tell Auth0 to redirect after logging the user out)
        let request = this.HttpContext.Request
        let postLogoutUri = request.Scheme + "://" + request.Host.ToString() + request.PathBase

        this.HttpContext.SignOutAsync("Auth0", AuthenticationProperties(            
                                                    RedirectUri = postLogoutUri
            )
        ) |> ignore
        this.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme) |> ignore



    [<Route("user")>]
    member this.User() =
        this.HttpContext.User.Claims |> Seq.map(fun claim -> sprintf "%s - %s" claim.Type claim.Value) |> String.concat("\n")