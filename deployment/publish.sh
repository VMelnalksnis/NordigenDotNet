#!/bin/bash
version=$(cat version)
publish_dir="./source/$1/bin/Release"
package_name="$1.$version.nupkg"

dotnet pack \
	./source/"$1"/"$1".csproj \
	--configuration Release \
	-p:AssemblyVersion="$version"."$2" \
	-p:AssemblyFileVersion="$version"."$2" \
	-p:PackageVersion="$version"."$2" \
	-p:InformationalVersion="$version""$3" \
	/warnAsError \
	/nologo

echo "::set-output name=artifact-name::$package_name"
echo "::set-output name=artifact::$publish_dir/$package_name"
