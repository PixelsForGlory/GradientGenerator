msbuild $env:APPVEYOR_BUILD_FOLDER\GradientGenerator.sln /logger:"C:\Program Files\AppVeyor\BuildAgent\Appveyor.MSBuildLogger.dll" /p:Configuration=Release /p:Platform="Any CPU" /p:ReferencePath="$env:DEPENDENCIES_DIR\Unity\Editor\Data\Managed" /v:minimal
if ($LastExitCode -ne 0) { $host.SetShouldExit($LastExitCode) }
