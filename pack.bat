@echo off
rm -rf release
mkdir release
mkdir release\lib
cp Codeaddicts.libArgument.nuspec release
cp codeaddicts.libArgument.dll release\lib
nuget pack release\Codeaddicts.libArgument.nuspec
rm -rf release