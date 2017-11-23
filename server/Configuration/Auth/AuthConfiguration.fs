namespace HappyBever.Configuration

open System
open Microsoft.AspNetCore.Builder
open Microsoft.AspNetCore.Hosting
open Microsoft.Extensions.Configuration
open Microsoft.AspNetCore.Authentication
open Microsoft.AspNetCore.Authentication.Cookies
open Microsoft.Extensions.Options
open Microsoft.AspNetCore.Http
open System.Net.Http
open System.Net.Http.Headers
open System.Security.Claims
open Newtonsoft.Json
open Newtonsoft.Json.Linq
open System.Threading.Tasks
open Microsoft.Extensions.DependencyInjection
open Microsoft.AspNetCore.Authentication.OpenIdConnect

module Auth =

    type IServiceCollection with
        member services.AddAuth (auth0Settings : Auth0Settings) =
            services.AddAuthentication(fun o -> 
                                        o.DefaultAuthenticateScheme <- CookieAuthenticationDefaults.AuthenticationScheme;
                                        o.DefaultSignInScheme <- CookieAuthenticationDefaults.AuthenticationScheme;
                                        o.DefaultChallengeScheme <- CookieAuthenticationDefaults.AuthenticationScheme;
                                        )
                                        .AddCookie()
                                        .AddOpenIdConnect("Auth0", fun options ->
                                            // Set the authority to your Auth0 domain
                                            options.Authority <- sprintf "https://%s" auth0Settings.Domain

                                            // Configure the Auth0 Client ID and Client Secret
                                            options.ClientId <- auth0Settings.ClientId
                                            options.ClientSecret <- auth0Settings.ClientSecret

                                            // Set response type to code
                                            options.ResponseType <- "code";

                                            // Configure the scope
                                            options.Scope.Clear();
                                            options.Scope.Add("openid");
                                            options.Scope.Add("profile");
                                            options.Scope.Add("email");

                                            // Set the callback path, so Auth0 will call back to http://localhost:5000/signin-auth0 
                                            // Also ensure that you have added the URL as an Allowed Callback URL in your Auth0 dashboard 
                                            options.CallbackPath <- PathString("/signin-auth0")

                                            // Configure the Claims Issuer to be Auth0
                                            options.ClaimsIssuer <- "Auth0"
                                            
                                            
                                            options.Events <- OpenIdConnectEvents(
                                                // handle the logout redirection 
                                                                OnRedirectToIdentityProviderForSignOut = (fun context ->
                                                                    let mutable logoutUri = sprintf "https://%s/v2/logout?client_id=%s" auth0Settings.Domain auth0Settings.ClientId

                                                                    let mutable postLogoutUri = context.Properties.RedirectUri
                                                                    if not (System.String.IsNullOrEmpty(postLogoutUri)) then                                                
                                                                        if postLogoutUri.StartsWith("/") then
                                                                        
                                                                            // transform to absolute
                                                                            let request = context.Request
                                                                            postLogoutUri <- sprintf "%s://%s%s%s"  request.Scheme (request.Host.ToString()) (request.PathBase.ToString()) postLogoutUri
                                                                        
                                                                        logoutUri <- sprintf "%s&returnTo=%s" logoutUri (Uri.EscapeDataString(postLogoutUri))
                                                                    

                                                                    context.Response.Redirect(logoutUri)
                                                                    context.HandleResponse()

                                                                    Task.CompletedTask
                                                                )                                                                
                                            
                                            )
                                        )                    
