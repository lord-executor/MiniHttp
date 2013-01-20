$scriptPath = Split-Path $MyInvocation.MyCommand.Path
Import-Module (join-path $scriptPath "..\tools\psake.psm1") -force
Invoke-psake -framework '4.0' "$scriptPath\default.ps1" $args[0]
Remove-Module psake
