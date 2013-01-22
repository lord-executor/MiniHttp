MAKEFILE_PATH := $(shell dirname $(CURDIR)/$(lastword $(MAKEFILE_LIST)))
PROJ := $(shell readlink -f "$(MAKEFILE_PATH)/../src/MiniHttp/MiniHttp.csproj")
PROJ_PATH := $(shell dirname $(PROJ))
XSLT := xsltproc
TRANSFORM := $(shell readlink -f "$(MAKEFILE_PATH)/transform.xslt")
CSC := mono-csc
OUTPUT_DIR := $(shell readlink -f "$(MAKEFILE_PATH)/../bin")
TMP_DIR := $(shell readlink -f "$(OUTPUT_DIR)/tmp")
VERSION := $(shell cat "$(MAKEFILE_PATH)/../src/MiniHttp/version.txt")
COMMIT := $(shell git show HEAD --format="%H from %ai" | head -n 1)
PARAM_REPLACE := sed -e 's/^\//-/' -e 's/\\/\//g'

build: clean compile-full compile-mini compile-lib

compile-full: init assembly-info MiniHttp.exe.compile
	$(CSC) -noconfig @"$(TMP_DIR)/MiniHttp.exe.compile" "$(TMP_DIR)/AssemblyInfo.cs"

compile-mini: init assembly-info MiniHttp.Minimal.exe.compile
	$(CSC) -noconfig -define:MINIMAL @"$(TMP_DIR)/MiniHttp.Minimal.exe.compile" "$(TMP_DIR)/AssemblyInfo.cs"

compile-lib: init assembly-info MiniHttp.dll.compile
	$(CSC) -noconfig @"$(TMP_DIR)/MiniHttp.dll.compile" "$(TMP_DIR)/AssemblyInfo.cs"

MiniHttp.exe.compile: init
	$(XSLT) --stringparam ProjectPath "$(PROJ_PATH)"\
		--stringparam OutputDir "$(OUTPUT_DIR)"\
		--stringparam Debug "false"\
		 $(TRANSFORM) $(PROJ)\
		 | $(PARAM_REPLACE)\
		 > $(TMP_DIR)/MiniHttp.exe.compile

MiniHttp.Minimal.exe.compile: init
	$(XSLT) --stringparam ProjectPath "$(PROJ_PATH)"\
		--stringparam OutputDir "$(OUTPUT_DIR)"\
		--stringparam Debug "false"\
		--stringparam OutputName "MiniHttp.Minimal.exe"\
		--stringparam ExcludeFiles "Processors/"\
		 $(TRANSFORM) $(PROJ)\
		 | $(PARAM_REPLACE)\
		 > $(TMP_DIR)/MiniHttp.Minimal.exe.compile

MiniHttp.dll.compile: init
	$(XSLT) --stringparam ProjectPath "$(PROJ_PATH)"\
		--stringparam OutputDir "$(OUTPUT_DIR)"\
		--stringparam Debug "false"\
		--stringparam OutputName "MiniHttp.dll"\
		--stringparam OutputType "library"\
		--stringparam ExcludeFiles "Program.cs,Arguments.cs,NDesk.Options"\
		 $(TRANSFORM) $(PROJ)\
		 | $(PARAM_REPLACE)\
		 > $(TMP_DIR)/MiniHttp.dll.compile

assembly-info: init
	cat $(MAKEFILE_PATH)/AssemblyInfo.Template.cs\
		| sed -e 's/@VERSION/$(VERSION)/' -e 's/@COMMIT/$(COMMIT)/'\
		> $(TMP_DIR)/AssemblyInfo.cs

init:
	mkdir $(OUTPUT_DIR)
	mkdir $(TMP_DIR)

clean:
	rm -rf $(OUTPUT_DIR)
