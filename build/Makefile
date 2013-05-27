MAKEFILE_PATH := $(shell dirname $(CURDIR)/$(lastword $(MAKEFILE_LIST)))
ROOT_PATH := $(shell readlink -f "$(MAKEFILE_PATH)/..")
PROJ := $(shell readlink -f "$(MAKEFILE_PATH)/../src/MiniHttp/MiniHttp.csproj")
PROJ_PATH := $(shell dirname $(PROJ))
XSLT := xsltproc
TRANSFORM := $(shell readlink -f "$(MAKEFILE_PATH)/transform.xslt")
CSC := mono-csc
OUTPUT_DIR := $(shell readlink -f "$(MAKEFILE_PATH)/../bin")
TMP_DIR := $(shell readlink -m "$(OUTPUT_DIR)/tmp")
TEST_DIR := $(shell readlink -m "$(OUTPUT_DIR)/test")
VERSION := $(shell cat "$(MAKEFILE_PATH)/../src/MiniHttp/version.txt")
COMMIT := $(shell git show HEAD --format="%H from %ai" | head -n 1)
# sed replacements after the XSLT transform
#   replace '/' for csc switches (windows) with '-' for mono csc
#   replace '\' with '/'
PARAM_REPLACE := sed -e 's/^\//-/' -e 's/\\/\//g'

build: clean compile-full compile-mini compile-lib compile-plugins

compile-full: init assembly-info MiniHttp.exe.compile
	$(CSC) -noconfig @"$(TMP_DIR)/MiniHttp.exe.compile" "$(TMP_DIR)/AssemblyInfo.cs"

compile-mini: init assembly-info MiniHttp.Minimal.exe.compile
	$(CSC) -noconfig -define:MINIMAL @"$(TMP_DIR)/MiniHttp.Minimal.exe.compile" "$(TMP_DIR)/AssemblyInfo.cs"

compile-lib: init assembly-info MiniHttp.dll.compile
	$(CSC) -noconfig @"$(TMP_DIR)/MiniHttp.dll.compile" "$(TMP_DIR)/AssemblyInfo.cs"

compile-plugins: init assembly-info-plugins MiniHttp.Plugins.dll.compile
	$(CSC) -noconfig -r:"$(OUTPUT_DIR)/MiniHttp.exe" @"$(TMP_DIR)/MiniHttp.Plugins.dll.compile" "$(TMP_DIR)/AssemblyInfo.Plugins.cs"

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

MiniHttp.UnitTests.dll.compile: init
	$(XSLT) --stringparam ProjectPath "$(ROOT_PATH)/src/MiniHttp.UnitTests"\
		--stringparam OutputDir "$(TEST_DIR)"\
		--stringparam Debug "false"\
		--stringparam OutputName "MiniHttp.UnitTests.dll"\
		--stringparam OutputType "library"\
		 $(TRANSFORM) "$(ROOT_PATH)/src/MiniHttp.UnitTests/MiniHttp.UnitTests.csproj"\
		 | $(PARAM_REPLACE)\
		 > $(TMP_DIR)/MiniHttp.UnitTests.dll.compile

MiniHttp.Plugins.dll.compile: init
	$(XSLT) --stringparam ProjectPath "$(ROOT_PATH)/src/MiniHttp.Plugins"\
		--stringparam OutputDir "$(OUTPUT_DIR)"\
		--stringparam Debug "false"\
		--stringparam OutputName "MiniHttp.Plugins.dll"\
		--stringparam OutputType "library"\
		 $(TRANSFORM) "$(ROOT_PATH)/src/MiniHttp.Plugins/MiniHttp.Plugins.csproj"\
		 | $(PARAM_REPLACE)\
		 > $(TMP_DIR)/MiniHttp.Plugins.dll.compile

assembly-info: init
	cat $(MAKEFILE_PATH)/AssemblyInfo.Template.cs\
		| sed -e 's/@VERSION/$(VERSION)/' -e 's/@COMMIT/$(COMMIT)/'\
		> $(TMP_DIR)/AssemblyInfo.cs

assembly-info-plugins: init
	cat $(MAKEFILE_PATH)/AssemblyInfo.Plugins.Template.cs\
		| sed -e 's/@VERSION/$(VERSION)/' -e 's/@COMMIT/$(COMMIT)/'\
		> $(TMP_DIR)/AssemblyInfo.Plugins.cs

prepare-tests: init compile-full
	cp "$(ROOT_PATH)/lib/nunit.framework.dll" $(TEST_DIR)
	cp "$(ROOT_PATH)/lib/Moq.dll" $(TEST_DIR)
	cp "$(OUTPUT_DIR)/MiniHttp.exe" $(TEST_DIR)

compile-tests: init compile-full prepare-tests MiniHttp.UnitTests.dll.compile
	$(CSC) -noconfig -r:"$(TEST_DIR)/MiniHttp.exe" @"$(TMP_DIR)/MiniHttp.UnitTests.dll.compile"

test: clean init compile-tests

init:
	mkdir $(OUTPUT_DIR)
	mkdir $(TMP_DIR)
	mkdir $(TEST_DIR)

clean:
	rm -rf $(OUTPUT_DIR)
