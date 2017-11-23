# Oppsett

## Backend

Installer [.NET Core SDK](https://www.microsoft.com/net/download/core)

Editor: [VS Code](https://code.visualstudio.com/download)

Åpne mappen til prosjektet med VS Code.
F1 -> Install extension -> Ionide-fsharp

Gå inn i backend-mappen: `cd api`

### Laste ned dependencies:
`dotnet restore`


### Kjør appen
Sett miljøvariabel for å få development-mode: `set ASPNETCORE_ENVIRONMENT=Development` (Windows).
Dette kan settes i OS'et sånn at den alltid er der, ellers må det gjøres hver gang man åpner terminalen man kjører fra.
Hvis man ikke gjør det defaulter den til 'Production', sånn at man får feil config og dårligere feilmeldinger.

`dotnet run`

### Kjør med hot reloading:  
`dotnet watch run`

### Debug
Åpne prosjektet i VS Code. Trykk F5.
Obs: Krever C#-extension.
`F1 -> Install extension -> C#`
`Cmd + shift + P -> Debug: download .NET Core Debugger`


## Frontend
Installer [node](https://nodejs.org/en/download/current)

Editor: [VS Code](https://code.visualstudio.com/download)

Installer TSLint-extension:
F1 -> Install extensions -> TSLint

Gå inn i frontend-mappen: `cd client`

### Installere yarn (package manager)
`npm install yarn -g`

### Laste ned avhengigheter (pakker)
`yarn install`

### Starte watch server (kompilerer og server TS/less)
`npm run start`

## Deploy (krever Windows)
Kjør `build.cmd` i rotmappa