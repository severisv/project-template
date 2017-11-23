if not EXIST "build\nuget\packages\FAKE\tools\Fake.exe" build\nuget\NuGet.exe "Install" "FAKE" "-OutputDirectory" "build\nuget\packages" "-ExcludeVersion"
call if not defined yarn npm install yarn -g

call build\setdeploycredentials.cmd
"build\nuget\packages\FAKE\tools\Fake.exe" build\build.fsx %*

