MAKEFILE_PATH := $(shell dirname $(CURDIR)/$(lastword $(MAKEFILE_LIST)))
PROJ := $(shell readlink -f "$(MAKEFILE_PATH)/../src/MiniHttp/MiniHttp.csproj")
PROJ_PATH := $(shell dirname $(PROJ))
XSLT := xsltproc
TRANSFORM := $(shell readlink -f "$(MAKEFILE_PATH)/transform.xslt")
CSC := mono-csc
OUTPUT_DIR := $(shell readlink -f "$(MAKEFILE_PATH)/../bin")
TMP_DIR := $(shell readlink -f "$(OUTPUT_DIR)/tmp")
VERSION := $(shell cat "$(MAKEFILE_PATH)/../src/MiniHttp/version.txt")

build: clean compile-full

compile-full: init

init:
	echo $(MAKEFILE_PATH)

clean:
	echo "cleaning"
	rm -r $(OUTPUT_DIR)
