
properties {
	$rootPath = Resolve-Path (Join-Path (Split-Path $psake.build_script_file) "..")
	
	$csc = "C:\Windows\Microsoft.NET\Framework64\v4.0.30319\csc.exe"
	$xslt = Resolve-Path "$rootPath/tools/msxsl.exe"
	$nunit = Resolve-Path "$rootPath/tools/nunit/nunit-console-x86.exe"
	$projTransform = Resolve-Path "$rootPath/build/transform.xslt"
	
	$outputDir = Join-Path $rootPath "bin"
	$tmpDir =  Join-Path $outputDir "tmp"
	$testDir = Join-Path $outputDir "test"
}

Task Default -Depends Compile,Test

Task Compile -Depends Clean,Init,CompileFull,CompileMinimal,CompileLibrary,CompilePlugins

Task CompileFull -Depends Init,AssemblyInfo {
	$proj = Join-Path $rootPath "src/MiniHttp/MiniHttp.csproj"
	$projPath = Split-Path $proj
	$outName = "MiniHttp.exe"
	$paramsFile = Join-Path $tmpDir "$outName.compile"
	
	Exec { &$xslt $proj $projTransform -o "$paramsFile" ProjectPath="$projPath" OutputName="$outName" OutputDir="$outputDir" Debug="false" }
	Exec { &$csc /noconfig "@$paramsFile" "$tmpDir\AssemblyInfo.cs" }
}

Task CompileMinimal -Depends Init,AssemblyInfo {
	$proj = Join-Path $rootPath "src/MiniHttp/MiniHttp.csproj"
	$projPath = Split-Path $proj
	$outName = "MiniHttp.Minimal.exe"
	$paramsFile = Join-Path $tmpDir "$outName.compile"
	
	Exec { &$xslt $proj $projTransform -o "$paramsFile" ProjectPath="$projPath" OutputName="$outName" OutputDir="$outputDir" Debug="false" ExcludeFiles="Processors\" }
	Exec { &$csc /noconfig /define:MINIMAL "@$paramsFile" "$tmpDir\AssemblyInfo.cs" }
}

Task CompileLibrary -Depends Init,AssemblyInfo {
	$proj = Join-Path $rootPath "src/MiniHttp/MiniHttp.csproj"
	$projPath = Split-Path $proj
	$outName = "MiniHttp.dll"
	$paramsFile = Join-Path $tmpDir "$outName.compile"
	
	Exec { &$xslt $proj $projTransform -o "$paramsFile" ProjectPath="$projPath" OutputName="$outName" OutputDir="$outputDir" Debug="false" OutputType="library" ExcludeFiles="Program.cs,Arguments.cs,NDesk.Options" }
	Exec { &$csc /noconfig "@$paramsFile" "$tmpDir\AssemblyInfo.cs" }
}

Task CompilePlugins -Depends Init,AssemblyInfo {
	$proj = Join-Path $rootPath "src/MiniHttp.Plugins/MiniHttp.Plugins.csproj"
	$projPath = Split-Path $proj
	$outName = "MiniHttp.Plugins.dll"
	$paramsFile = Join-Path $tmpDir "$outName.compile"
	
	Exec { &$xslt $proj $projTransform -o "$paramsFile" ProjectPath="$projPath" OutputName="$outName" OutputDir="$outputDir" Debug="false" OutputType="library" }
	Exec { &$csc /noconfig /r:"$outputDir/MiniHttp.exe" "@$paramsFile" "$tmpDir\AssemblyInfo.cs" }
}

Task AssemblyInfo -Depends Init {
	$version = Get-Content "$rootPath/src/MiniHttp/version.txt"
	$commit = git show HEAD --format="%H from %ai" | Select -First 1
	
	Get-Content "$rootPath/build/AssemblyInfo.Template.cs" | `
	Foreach-Object { $_ -replace "@VERSION", "$version"} | `
	Foreach-Object { $_ -replace "@COMMIT", "$commit"} | `
	Set-Content "$tmpDir\AssemblyInfo.cs"
}

Task CompileTests -Depends Init,CompileFull,PrepareTests {
	$proj = Join-Path $rootPath "src/MiniHttp.UnitTests/MiniHttp.UnitTests.csproj"
	$projPath = Split-Path $proj
	$outName = "MiniHttp.UnitTests.dll"
	$paramsFile = Join-Path $tmpDir "$outName.compile"
	
	Exec { &$xslt $proj $projTransform -o "$paramsFile" ProjectPath="$projPath" OutputName="$outName" OutputDir="$testDir" Debug="false" OutputType="library" }
	Exec { &$csc /noconfig /r:"$testDir/MiniHttp.exe" "@$paramsFile" }
}

Task PrepareTests -Depends Init,CompileFull {
	Copy-Item (Join-Path $rootPath "lib/nunit.framework.dll") $testDir
	Copy-Item (Join-Path $rootPath "lib/Moq.dll") $testDir
	Copy-Item (Join-Path $outputDir "MiniHttp.exe") $testDir
}

Task Test -Depends PrepareTests,CompileTests {
	Exec { &$nunit /noresult /exclude="disposal" /framework="4.0" "$testDir/MiniHttp.UnitTests.dll" }
}

Task Clean {
	if (Test-Path $outputDir) { Remove-Item $outputDir -Recurse -Force }
}

Task Init {
	if (!(Test-Path $outputDir)) { New-Item $outputDir -type directory }
	if (!(Test-Path $tmpDir)) { New-Item $tmpDir -type directory }
	if (!(Test-Path $testDir)) { New-Item $testDir -type directory }
}
