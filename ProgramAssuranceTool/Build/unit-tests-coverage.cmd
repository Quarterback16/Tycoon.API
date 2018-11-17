@echo off
	for %%i in (%buildartifactsdir%%codecoveragefilter%) do vsinstr -coverage  %%i
	if NOT [%errorlevel%] == [0] goto handleerror1orhigher
	vsperfcmd -Start:coverage -Output:%coveragefile%

	for %%i in (%buildartifactsdir%%unittestassemblyfilter%) do (
		echo "Executing tests for %%i"		
		mstest /testcontainer:%%i /usestderr /detail:stderr
	)
	if NOT [%errorlevel%] == [0] goto handleerror2orhigher

	vsperfcmd -shutdown
	IF EXIST "%coveragefile%" (
		echo Opening "%coveragefile%" .........
		START %coveragefile%
		goto sussess	
	)
	SET %errorlevel%=0
	echo Coverage file is not created
	goto sussess

goto sussess
:handleerror2orhigher
vsperfcmd -shutdown
exit /B 1
:handleerror1orhigher
exit /B 1
:sussess
exit /B 0
