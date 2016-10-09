properties {
	$pwd = Split-Path $psake.build_script_file	
	$build_directory  = "$pwd\output\condep-dsl-operations-contrib"
	$configuration = "Release"
	$releaseNotes = ""
	$nunitPath = "$pwd\..\src\packages\NUnit.ConsoleRunner.3.4.1\tools"
	$nuget = "$pwd\..\tools\nuget.exe"
}
 
include .\..\tools\psake_ext.ps1

Framework '4.6x64'

function GetNugetAssemblyVersion($assemblyPath) {
    
    if(Test-Path Env:\APPVEYOR_BUILD_VERSION)
    {
        #When building on appveyor, set correct beta number.
        $appVeyorBuildVersion = $env:APPVEYOR_BUILD_VERSION
        
        $version = $appVeyorBuildVersion.Split('-') | Select-Object -First 1
        $betaNumber = $appVeyorBuildVersion.Split('-') | Select-Object -Last 1 | % {$_.replace("beta","")}

        switch ($betaNumber.length) 
        { 
            1 {$betaNumber = $betaNumber.Insert(0, '0').Insert(0, '0').Insert(0, '0').Insert(0, '0')} 
            2 {$betaNumber = $betaNumber.Insert(0, '0').Insert(0, '0').Insert(0, '0')} 
            3 {$betaNumber = $betaNumber.Insert(0, '0').Insert(0, '0')}
            4 {$betaNumber = $betaNumber.Insert(0, '0')}                
            default {$betaNumber = $betaNumber}
        }

        return "$version-beta$betaNumber"
    }
    else
    {
        $versionInfo = Get-Item $assemblyPath | % versioninfo
        return "$($versionInfo.FileVersion)"
    }
}

task default -depends Build-All, Test-All, Pack-All

task Build-All -depends Clean, RestoreNugetPackages, Build
task Test-All -depends Test
task Pack-All -depends Create-BuildSpec-ConDep-Dsl-Operations-Contrib, Pack-ConDep-Dsl-Operations-Contrib

task RestoreNugetPackages {
	Exec { & $nuget restore "$pwd\..\src\condep-dsl-operations-contrib.sln" }
}

task Build {
	Exec { msbuild "$pwd\..\src\condep-dsl-operations-contrib.sln" /t:Build /p:Configuration=$configuration /p:OutDir=$build_directory /p:GenerateProjectSpecificOutputFolder=true}
}

task Test {
}

task Clean {
	Write-Host "Cleaning Build output.."  -ForegroundColor Green
	Remove-Item $build_directory -Force -Recurse -ErrorAction SilentlyContinue
}

task Create-BuildSpec-ConDep-Dsl-Operations-Contrib {
    Write-Host "Creating nuget spec file.."  -ForegroundColor Green
	Generate-Nuspec-File `
		-file "$build_directory\condep.dsl.operations.contrib.$(GetNugetAssemblyVersion $build_directory\ConDep.Dsl.Operations.Contrib\ConDep.Dsl.Operations.Contrib.dll).nuspec" `
		-version $(GetNugetAssemblyVersion $build_directory\ConDep.Dsl.Operations.Contrib\ConDep.Dsl.Operations.Contrib.dll) `
		-id "ConDep.Dsl.Operations.Contrib" `
		-title "ConDep.Dsl.Operations.Contrib" `
		-licenseUrl "http://www.con-dep.net/license/" `
		-projectUrl "http://www.con-dep.net/" `
		-description "Note: This package is for extending the ConDep DSL with operations from contributors." `
		-iconUrl "https://raw.github.com/condep/ConDep/master/images/ConDepNugetLogo.png" `
		-releaseNotes "$releaseNotes" `
		-tags "Continuous Deployment Delivery Infrastructure WebDeploy Deploy msdeploy IIS automation powershell remote aws azure" `
		-dependencies @(
			@{ Name="ConDep.Dsl"; Version="[4.0.1,5)"},
			@{ Name="ConDep.Dsl.Operations"; Version="[3.2.3,4)"},
			@{ Name="Newtonsoft.Json"; Version="[6.0.6,7)"},
			@{ Name="SlowCheetah.Tasks.Unofficial"; Version="[1.0.0]"}
		) `
		-files @(
			@{ Path="ConDep.Dsl.Operations.Contrib\ConDep.Dsl.Operations.Contrib.dll"; Target="lib/net40"}
		)
}

task Pack-ConDep-Dsl-Operations-Contrib {
    Write-Host "Creating nuget package.."  -ForegroundColor Green
	Exec { & $nuget pack "$build_directory\condep.dsl.operations.contrib.$(GetNugetAssemblyVersion $build_directory\ConDep.Dsl.Operations.Contrib\ConDep.Dsl.Operations.Contrib.dll).nuspec" -OutputDirectory "$build_directory" }
}