@echo off
call "C:\Program Files (x86)\Microsoft Visual Studio 11.0\Common7\Tools\VsDevCmd.bat"
SET %errorlevel%=0
SET currentdir=%~dp0
SET "solutionname=Build ProgramAssuranceTool.sln"
SET "codedir=%currentdir%..\"
SET "configuration=Debug"
SET "verbosity=m"
SET "buildartifactsdir=%currentdir%BuildArtifacts\"
SET "testresultdir=%currentdir%TestResults\"
SET "unittestassemblyfilter=*.Tests.dll"
SET "codecoveragefilter=ProgramAssuranceTool.dll"
SET "coveragefile=ProgramAssuranceTool.coverage"


call clean.cmd
if NOT [%errorlevel%] == [0] goto handleerror1orhigher
call compile.cmd 
if NOT [%errorlevel%] == [0] goto handleerror1orhigher
call unit-tests.cmd
if NOT [%errorlevel%] == [0] goto handleerror2orhigher


goto sussess
:handleerror2orhigher
echo Unit tests failed
:handleerror1orhigher
echo 'error'
goto end
:sussess
echo success
:end
pause