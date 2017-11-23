#r @"nuget/packages/FAKE/tools/FakeLib.dll"
open Fake
open Fake.AppVeyor
open Fake.FileSystemHelper
open Fake.EnvironmentHelper
open System.IO

[<AutoOpen>]
module Settings =
  let getDirectory file = Directory.GetParent(file).FullName
  let currentDir = FileSystemHelper.currentDirectory  
  let appName = EnvironmentHelper.environVar "DEPLOY_ENV_NAME"
  let password = EnvironmentHelper.environVar "DEPLOY_PWD"
  let deployArchitecture = "win7-x64" 
  let deployDir = sprintf "%s%s" currentDir "/.deploy"
  let clientResources = "client"
  let projects = !! "**/*.fsproj" |> Seq.map(getDirectory)
  let entryProject = projects |> Seq.find(fun e -> e.EndsWith "server")
  let testProjects = !! "test/**/*.fsproj" |> Seq.map(getDirectory)


[<AutoOpen>]
module Helpers =

    let shellExec cmdPath args target =
        let result = ExecProcess (
                      fun info ->
                        info.FileName <- cmdPath
                        info.WorkingDirectory <- target
                        info.Arguments <- args
                      ) System.TimeSpan.MaxValue
        if result <> 0 then failwith (sprintf "'%s' failed" cmdPath + " " + args)

    let findOnPath name =
        let executable = tryFindFileOnPath name
        match executable with
            | Some exec -> exec
            | None -> failwith (sprintf "'%s' can't find" name)

    let npm args workingDir =
        let executable = findOnPath "npm.cmd"
        printf "running npm command with node version: "
        shellExec executable "--v" workingDir
        shellExec executable args workingDir

    let yarn args workingDir =
        let executable = findOnPath "yarn.cmd"
        shellExec executable args workingDir

    let dotnet args workingDir =
        let executable = findOnPath "dotnet.exe"
        shellExec executable args workingDir


    let rec execMsdeploy executable args workingDir attempt attempts =
        try
            shellExec executable args workingDir
        with | _ ->
            if attempt > 0 then
                printf "WebDeploy attempt %i: \n" (1+attempts-attempt)
                execMsdeploy executable args workingDir (attempt-1) attempts
            else
                printf "WebDeploy failed after %i attempts \n" attempts
                reraise()

    let msdeploy args workingDir =
        let executable = sprintf "%s\\build\\webdeploy\\msdeploy.exe" currentDir
        execMsdeploy executable args workingDir 3 3


    type DotnetCommands =
        | Restore
        | Build
        | Publish
        | Test

    let Dotnet command target =
        match command with
            | Restore -> (dotnet "restore" target)
            | Build -> (dotnet "build --configuration Release" target)
            | Publish -> (dotnet (sprintf "publish --configuration Release --runtime %s --output %s" deployArchitecture deployDir) target)
            | Test -> (dotnet "test" target)


[<AutoOpen>]
module Targets =
  

  Target "Clean" (fun() ->
    CleanDirs [deployDir; entryProject |> sprintf "%s\\wwwroot"]
  )

  Target "YarnRestore" (fun _ ->
     yarn "install" clientResources
  )

  Target "Webpack" (fun _ ->
     npm "run prod" clientResources
  )

  Target "RestorePackages" (fun _ ->
     projects
     |> Seq.iter (fun proj -> Dotnet Restore proj |> ignore)
  )

  Target "Build" (fun _ ->
     projects
     |> Seq.iter (fun proj -> Dotnet Build proj |> ignore)
  )

  Target "Test" (fun _ ->
     testProjects
     |> Seq.iter (fun proj -> Dotnet Test proj |> ignore)
  )

  Target "Publish" (fun _ ->
        Dotnet Publish entryProject |> ignore
  )

  Target "Deploy" (fun _ ->     
     let args = sprintf "-source:IisApp='%s\.deploy' -dest:IisApp='%s',ComputerName='https://%s.scm.azurewebsites.net/msdeploy.axd',UserName='$%s',Password='%s',IncludeAcls='False',AuthType='Basic' -verb:sync -enableLink:contentLibExtension -enableRule:AppOffline -retryAttempts:2" currentDir appName appName appName password
     msdeploy args "" |> ignore
  )

Target "Default" (ignore)

"Clean"
==> "YarnRestore"
==> "Webpack"
==> "RestorePackages"
==> "Build"
==> "Test"
==> "Publish"
==> "Deploy"
==> "Default"

RunTargetOrDefault "Default"
