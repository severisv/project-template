namespace HappyBever

open System
open Newtonsoft.Json
open Newtonsoft.Json.Converters

[<JsonConverter(typedefof<StringEnumConverter>)>]
type Tilgang =
  | AvvikRead = 1
  | AvvikWrite = 2
  | DokumentasjonRead = 3
  | DokumentasjonWrite = 4
  | TegningerRead = 5
  | TegningerWrite = 6
  | ServiceavtalerRead = 7
  | ServiceavtalerWrite = 8



module Tilganger =
    let mapToTilganger roles =
        let (|InvariantEqual|_|) (str:string) arg =
            if String.Compare(str, arg, StringComparison.OrdinalIgnoreCase) = 0
              then Some() else None


        roles
          |> Array.toList
          |> List.collect(fun role ->
              match role with
              | InvariantEqual Roles.Admin ->
                [
                  Tilgang.AvvikRead
                  Tilgang.AvvikWrite
                  Tilgang.DokumentasjonRead
                  Tilgang.DokumentasjonWrite
                  Tilgang.TegningerRead
                  Tilgang.TegningerWrite
                  Tilgang.ServiceavtalerRead
                  Tilgang.ServiceavtalerWrite
                ]
              | InvariantEqual Roles.User ->
                [
                  Tilgang.AvvikRead
                  Tilgang.DokumentasjonRead
                  Tilgang.TegningerRead
                  Tilgang.ServiceavtalerRead
                ]
              |_ -> []
          )
          |> List.distinct
          |> List.sort
