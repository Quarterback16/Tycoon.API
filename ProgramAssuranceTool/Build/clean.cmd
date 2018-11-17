@echo off
echo Cleaning %solutionname%

IF EXIST "%buildartifactsdir%" (
	rmdir "%buildartifactsdir%" /s /q
	IF EXIST "%buildartifactsdir%" goto handleerror1orhigher
)
SET %errorlevel%=0
echo "%buildartifactsdir%" deleted

IF EXIST "%testresultdir%" (
	rmdir "%testresultdir%" /s /q
	IF EXIST "%testresultdir%" goto handleerror1orhigher
)
SET %errorlevel%=0
echo "%testresultdir%" deleted

msbuild "%codedir%%solutionname%" /t:Clean /p:Configuration=%configuration% /v:%verbosity% 
if NOT [%errorlevel%] == [0] goto handleerror1orhigher

goto sussess
:handleerror1orhigher
exit /B 1
:sussess
exit /B 0
