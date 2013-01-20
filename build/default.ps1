
properties {
	$rootPath = Resolve-Path (Join-Path (Split-Path $psake.build_script_file) "..")
	
	$csc = "C:\Windows\Microsoft.NET\Framework64\v4.0.30319\csc.exe"
	$xslt = Resolve-Path "$rootPath/tools/msxsl.exe"
	$projTransform = Resolve-Path "$rootPath/build/transform.xslt"
	
	$outputDir = Join-Path $rootPath "bin"
	$tmpDir =  Join-Path $outputDir "tmp"
	
	$version = Get-Content "$rootPath/src/MiniHttp/version.txt"
	$commit = git show HEAD --format="%H from %ai" | Select -First 1
}

Task Default -Depends Compile

Task Compile -Depends Clean,Init,CompileFull,CompileMinimal,CompileLibrary {
   "compile"
   write-output $outputDir
   write-output $commit
}

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

Task AssemblyInfo -Depends Init {
	Get-Content "$rootPath/build/AssemblyInfo.Template.cs" | `
	Foreach-Object { $_ -replace "@VERSION", "$version"} | `
	Foreach-Object { $_ -replace "@COMMIT", "$commit"} | `
	Set-Content "$tmpDir\AssemblyInfo.cs"
}

Task Clean {
	if (Test-Path $outputDir) { Remove-Item $outputDir -Recurse -Force }
}

Task Init {
	if (!(Test-Path $outputDir)) { New-Item $outputDir -type directory }
	if (!(Test-Path $tmpDir)) { New-Item $tmpDir -type directory }
}
