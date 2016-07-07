param($installPath, $toolsPath, $package, $project)

$baseDir = (Get-Item $project.FullName).DirectoryName
Remove-Item "$baseDir\Assets\Plugins\PixelsForGlory\GradientGenerator\GradientGenerator.dll*"
Remove-Item "$baseDir\Assets\Plugins\PixelsForGlory\GradientGenerator\GradientGenerator.pdb*"
