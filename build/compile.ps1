
$proj = "../src/MiniHttp/MiniHttp.csproj"
$xslt = "../tools/msxsl.exe"
$csc = "C:\Windows\Microsoft.NET\Framework64\v4.0.30319\csc.exe"
$outputDir = "..\bin"
$tmpDir = "$outputDir\tmp"
$version = Get-Content "../src/MiniHttp/version.txt"
$commit = &git show HEAD --format="%H from %ai"

if (!(Test-Path $outputDir))
{
	New-Item $outputDir -type directory
}
Remove-Item "$outputDir\*" -Recurse -Force
New-Item $tmpDir -type directory

Get-Content "AssemblyInfo.Template.cs" | `
Foreach-Object { $_ -replace "@VERSION", "$version"} | `
Foreach-Object { $_ -replace "@COMMIT", "$commit"} | `
Set-Content "$tmpDir\AssemblyInfo.cs"

&$xslt $proj transform.xslt -o "$tmpDir\MiniHttp.exe.compile" OutputDir="$outputDir" Debug="false"
&$csc /noconfig "@$tmpDir\MiniHttp.exe.compile" "$tmpDir\AssemblyInfo.cs"

&$xslt $proj transform.xslt -o "$tmpDir\MiniHttp.dll.compile" OutputName="MiniHttp.dll" OutputDir="$outputDir" OutputType="library" Debug="false" ExcludeFiles="Program.cs,Arguments.cs,NDesk.Options"
&$csc /noconfig "@$tmpDir\MiniHttp.dll.compile" "$tmpDir\AssemblyInfo.cs"

&$xslt $proj transform.xslt -o "$tmpDir\MiniHttp.Minimal.exe.compile" OutputName="MiniHttp.Minimal.exe" OutputDir="$outputDir" Debug="false" ExcludeFiles="Processors\"
&$csc /noconfig /define:MINIMAL "@$tmpDir\MiniHttp.Minimal.exe.compile" "$tmpDir\AssemblyInfo.cs"
