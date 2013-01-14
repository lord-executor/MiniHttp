<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:cs="http://schemas.microsoft.com/developer/msbuild/2003">
	<xsl:output method="text" version="1.0" encoding="utf-8" indent="no"/>
	
	<xsl:variable name="header">
  <tr>
  <th>Element</th>
  <th>Description</th>
  </tr>
</xsl:variable>
	
	<xsl:template match="@*|node()">
		<xsl:apply-templates select="@*|node()"/>
	</xsl:template>
	<xsl:template match="/">/out:"MiniHttp.exe"
/target:exe
/platform:x86
/debug
<xsl:apply-templates select=".//cs:ItemGroup//cs:EmbeddedResource"/>
<xsl:apply-templates select=".//cs:ItemGroup//cs:Compile"/>
	</xsl:template>
	
	<xsl:template match="cs:Compile">"..\src\MiniHttp\<xsl:value-of select="@Include" />"
</xsl:template>
	<xsl:template match="cs:ItemGroup//cs:EmbeddedResource">/res:"..\src\MiniHttp\<xsl:value-of select="@Include" />","<xsl:value-of select="/cs:Project/cs:PropertyGroup/cs:RootNamespace" />.<xsl:value-of select="translate(@Include, '\', '.')" />"
</xsl:template>
</xsl:stylesheet> 
