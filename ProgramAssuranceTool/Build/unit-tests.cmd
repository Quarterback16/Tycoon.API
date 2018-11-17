@echo off

echo Unit testing %solutionname%
mkdir %testresultdir% 
if NOT [%errorlevel%] == [0] goto handleerror1orhigher

for %%i in (%buildartifactsdir%%unittestassemblyfilter%) do (
	echo "Executing tests for %%i"		
	mstest /testcontainer:%%i /usestderr /detail:stderr
)
if NOT [%errorlevel%] == [0] goto handleerror1orhigher

goto sussess
:handleerror1orhigher
exit /B 1
:sussess
exit /B 0