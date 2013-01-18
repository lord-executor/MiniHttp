
$scriptPath = Split-Path $MyInvocation.MyCommand.Path
$proj = Resolve-Path "$scriptPath/../src/MiniHttp/MiniHttp.csproj"
$projPath = Split-Path $proj
$xslt = Resolve-Path "$scriptPath/../tools/msxsl.exe"
$transform = Resolve-Path "$scriptPath/transform.xslt"
$csc = "C:\Windows\Microsoft.NET\Framework64\v4.0.30319\csc.exe"
$outputDir = Resolve-Path "$scriptPath/../bin"
$tmpDir = "$outputDir\tmp"
$version = Get-Content "$scriptPath/../src/MiniHttp/version.txt"
$commit = git show HEAD --format="%H from %ai" | Select -First 1
if (!(Test-Path $outputDir))
{
	New-Item $outputDir -type directory
}
Remove-Item "$outputDir\*" -Recurse -Force
New-Item $tmpDir -type directory

Get-Content "$scriptPath/AssemblyInfo.Template.cs" | `
Foreach-Object { $_ -replace "@VERSION", "$version"} | `
Foreach-Object { $_ -replace "@COMMIT", "$commit"} | `
Set-Content "$tmpDir\AssemblyInfo.cs"

&$xslt $proj $transform -o "$tmpDir\MiniHttp.exe.compile" ProjectPath="$projPath" OutputDir="$outputDir" Debug="false"
&$csc /noconfig "@$tmpDir\MiniHttp.exe.compile" "$tmpDir\AssemblyInfo.cs"

&$xslt $proj $transform -o "$tmpDir\MiniHttp.dll.compile" ProjectPath="$projPath" OutputName="MiniHttp.dll" OutputDir="$outputDir" OutputType="library" Debug="false" ExcludeFiles="Program.cs,Arguments.cs,NDesk.Options"
&$csc /noconfig "@$tmpDir\MiniHttp.dll.compile" "$tmpDir\AssemblyInfo.cs"

&$xslt $proj $transform -o "$tmpDir\MiniHttp.Minimal.exe.compile" ProjectPath="$projPath" OutputName="MiniHttp.Minimal.exe" OutputDir="$outputDir" Debug="false" ExcludeFiles="Processors\"
&$csc /noconfig /define:MINIMAL "@$tmpDir\MiniHttp.Minimal.exe.compile" "$tmpDir\AssemblyInfo.cs"
