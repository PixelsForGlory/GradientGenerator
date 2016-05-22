# Run unit test
Copy-Item $env:APPVEYOR_BUILD_FOLDER\GradientGenerator\bin\Release\GradientGenerator.* -Destination $env:APPVEYOR_BUILD_FOLDER\GradientGeneratorTest\bin\Release\ -Force
vstest.console $env:APPVEYOR_BUILD_FOLDER\GradientGeneratorTest\bin\Release\GradientGeneratorTest.dll /Settings:$env:APPVEYOR_BUILD_FOLDER\GradientGeneratorTest\test.runsettings /logger:Appveyor
