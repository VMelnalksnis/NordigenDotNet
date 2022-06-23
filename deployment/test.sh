#!/bin/bash

dotnet restore || exit

dotnet build \
--configuration Release \
--no-restore \
/nologo \
/clp:NoSummary ||
	exit

dotnet test \
-p:CollectCoverage=true \
-p:CoverletOutput=TestResults/ \
-p:CoverletOutputFormat=opencover \
-p:BuildInParallel=false \
-m:1 \
--configuration Release \
--no-build ||
	exit
