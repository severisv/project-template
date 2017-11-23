namespace HappyBever
open System
open Microsoft.Extensions.Configuration
open Microsoft.Extensions.Options
open Microsoft.AspNetCore.Http
open Newtonsoft.Json
open System.Security.Claims
open HappyBever
open Tilganger

[<CLIMutableAttribute>]
type ByggDto = {
    id: int
    roles: string[]
}

[<CLIMutableAttribute>]
type MetadataDto =  {
    firstname: string
    lastname: string
    bygg: ByggDto[]
}

type Bygg = {
    id: int
    tilganger: Tilgang list
}

type Metadata =  {
    firstname: string
    lastname: string
    bygg: Bygg list
} with
    member metadata.Name =
        sprintf "%s %s" metadata.firstname metadata.lastname




module User =

    type ClaimsPrincipal with
        member user.Metadata =
            let claim = user.Claims |> Seq.tryFind(fun claim -> claim.Type = "user_metadata")

           
            match claim with
            | Some value ->
                    let dto = value.Value |> JsonConvert.DeserializeObject<MetadataDto>

                    {
                        firstname = dto.firstname
                        lastname = dto.lastname
                        bygg =  (dto.bygg |> Array.map (fun (b: ByggDto) ->
                                                        {
                                                            id = b.id
                                                            tilganger = (b.roles |> mapToTilganger)
                                                        }
                                                    )
                                            |> Array.toList
                                )
                    }
            | None -> { firstname = ""; lastname = ""; bygg = List.empty<Bygg> }

        member user.ByggIds  =
            let metadata = user.Metadata
            metadata.bygg |> List.map(fun bygg -> bygg.id)

        member user.Image =
            let claim = user.Claims |> Seq.tryFind(fun claim -> claim.Type = "picture")

            match claim with
            | Some value -> value.Value
            | None -> ""

        member user.LastModified =
            { UserId = user.Identity.Name; UserName = user.Metadata.Name; TimeStamp = DateTime.Now }

        member user.IsInRoleForBygning(bygningsId: BygningId, [<ParamArray>] roles : Tilgang[])  =

            user.Metadata.bygg
                |> Seq.filter(fun bygg -> bygg.id = bygningsId)
                |> Seq.collect(fun bygg -> bygg.tilganger)
                |> Seq.exists(fun role -> Seq.contains role roles)
