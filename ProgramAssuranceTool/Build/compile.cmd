@echo off

echo Compiling %solutionname%
if NOT [%errorlevel%] == [0] goto handleerror1orhigher
msbuild "%codedir%%solutionname%" /t:Build /p:Configuration=%configuration% /v:%verbosity% /p:OutDir=%buildartifactsdir%

if NOT [%errorlevel%] == [0] goto handleerror1orhigher

goto sussess
:handleerror1orhigher
exit /B 1
:sussess
exit /B 0