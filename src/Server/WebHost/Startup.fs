namespace WebHost

open Owin
open Microsoft.Owin
open Microsoft.Owin.Host
open System.Web.Http
open Newtonsoft.Json.Serialization

type Startup() =

    member x.Configuration(app: Owin.IAppBuilder) =
      let cfg = new HttpConfiguration()
      cfg.Routes.MapHttpRoute(
          name = "Default", 
          routeTemplate = "api/{controller}/{id}", 
          defaults = RouteParameter.Optional) 
        |> ignore
      
      cfg.Formatters.JsonFormatter.SerializerSettings.ContractResolver <- CamelCasePropertyNamesContractResolver()

      app
        .UseWebApi(cfg)
        .MapSignalR()
        .Run(fun ctx ->
          ctx.Response.ContentType <- "text/plain"
          ctx.Response.WriteAsync("Web Application Server with F#, Owin, ASP.NET Identity, SignalR...")
      ) |> ignore

type Response = {Name: string; Message: string}    
type RateController() = 
  inherit ApiController() 
  member __.Get(id) = 
    { Name = id; Message = "I am watching you" } 
  

[<assembly: OwinStartup(typeof<Startup>)>]
do ()