#!/bin/sh
export ASPNETCORE_URLS="http://0.0.0.0:${PORT:-10000}"
exec dotnet ProjetoOS.dll
