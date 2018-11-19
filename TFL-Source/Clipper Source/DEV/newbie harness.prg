*
*  Creates a new NFL player to add to the database
*
*

*
*
parameters cWeek
LOCAL oParser
*SET DATE BRITISH
SET DEFAULT TO e:\tfl\dev
set procedure to NFLLib
SET PROCEDURE TO PlayerPARSER additive
SET PROCEDURE TO NFLGUMBY ADDITIVE
set procedure to NFLTeams additive
set procedure to NFLPlayer additive
set procedure to NFLDataController additive

SET ALTERNATE off
SET ALTERNATE TO 
ERASE ( "newbie" + cWeek + ".txt" )
SET ALTERNATE TO ( "newbie" + cWeek + ".txt" )
SET ALTERNATE on

*debug
clear

oParser = CREATEOBJECT("PlayerParser",cWeek)
oParser.Process()


SET ALTERNATE off
SET ALTERNATE TO
close database
clear all
