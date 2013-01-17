<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:cs="http://schemas.microsoft.com/developer/msbuild/2003">
	<xsl:output method="text" version="1.0" encoding="utf-8" indent="no"/>
	
	<xsl:param name="Debug">false</xsl:param>
	<xsl:param name="OutputType">exe</xsl:param>
	<xsl:param name="OutputName"><xsl:value-of select="/cs:Project/cs:PropertyGroup/cs:AssemblyName" />.exe</xsl:param>
	<xsl:param name="OutputDir">.</xsl:param>
	<xsl:param name="ExcludeFiles"></xsl:param>
	<xsl:variable name="ExcludedFilesEx"><xsl:value-of select="$ExcludeFiles" />,</xsl:variable>
	
	<xsl:template match="@*|node()">
		<xsl:apply-templates select="@*|node()"/>
	</xsl:template>
	<xsl:template match="/">/out:"<xsl:value-of select="$OutputDir" />\<xsl:value-of select="$OutputName" />"
/target:<xsl:value-of select="$OutputType" />
/platform:x86<xsl:if test="$Debug = 'true'">
/debug+
/debug:full
/optimize-</xsl:if>
/warn:4
/nowarn:1701,1702,1607
<xsl:apply-templates select=".//cs:ItemGroup//cs:Reference"/>
<xsl:apply-templates select=".//cs:ItemGroup//cs:EmbeddedResource"/>
<xsl:apply-templates select=".//cs:ItemGroup//cs:Compile"/>
	</xsl:template>
	
	<xsl:template match="cs:Reference">/r:"<xsl:value-of select="@Include" />.dll"
</xsl:template>
	<xsl:template match="cs:EmbeddedResource">/res:"..\src\MiniHttp\<xsl:value-of select="@Include" />","<xsl:value-of select="/cs:Project/cs:PropertyGroup/cs:RootNamespace" />.<xsl:value-of select="translate(@Include, '\', '.')" />"
</xsl:template>
	<xsl:template match="cs:Compile">
		<xsl:variable name="exclude">
			<xsl:call-template name="is-excluded">
				<xsl:with-param name="file" select="@Include" />
				<xsl:with-param name="exclude" select="$ExcludedFilesEx" />
			</xsl:call-template>
		</xsl:variable>
		<xsl:if test="$exclude != 'true'">"..\src\MiniHttp\<xsl:value-of select="@Include" />"
</xsl:if>
	</xsl:template>

<xsl:template name="is-excluded">
	<xsl:param name="file" />
	<xsl:param name="exclude" />
	<xsl:variable name="first" select="substring-before($exclude, ',')" />
	<xsl:variable name="remaining" select="substring-after($exclude, ',')" />
	<xsl:if test="$first">
		<xsl:choose>
			<xsl:when test="starts-with($file, $first)">true</xsl:when>
			<xsl:otherwise>
				<xsl:if test="$remaining">
					<xsl:call-template name="is-excluded">
						<xsl:with-param name="file" select="$file" />
						<xsl:with-param name="exclude" select="$remaining" />
					</xsl:call-template>
				</xsl:if>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:if>
</xsl:template>
	
</xsl:stylesheet> 
