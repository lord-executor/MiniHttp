$proj = "../src/MiniHttp/MiniHttp.csproj"
$xslt = "../tools/msxsl.exe"
$csc = "csc.exe"

&$xslt $proj transform.xslt -o MiniHttp.compile
&$csc /noconfig "@MiniHttp.compile"

&$xslt $proj transform.xslt -o MiniHttp.compile2 OutputName="MiniHttp.dll" OutputType="library" Debug="false" ExcludeFiles="Program.cs,Arguments.cs,NDesk.Options"
&$csc /noconfig "@MiniHttp.compile2"
