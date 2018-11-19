*
*  ERROR Scenarios:
*
*  1) When you get a Scores dont add up error
*
*     1) Turn on debug code in Game.ScoreCheck
*     2) Comment in NFLParser.H-K_SHOWSCORES
*     3) Check for Unknown Score types in ProcessLineScore()
*        3.1)  Check the box score
*
*  2) When you get an unknown player error
*        eg   DC @ NG  UNK-DL:M.Wiley for DC
*
*     1) If he doesnt exist, load his page from nfl.com and
*        include him in this weeks newbie processing
*     2) If he does exist, check his service dates and check his
*         CURRTEAM field
*        2.1)  check for another active player with
*              the same name on the team (tally > 1)
*              GET rid of the Old player OR
*              Change boxscore to add more to Intial
*     3) If dates are correct
*        3.1)  set up K_TEST_PLAYERNAME in NFlParser.H file
*        3.2)  delete *.fxp
*        3.3)  Move boxscores you are not interested in
*        3.4)  re-run harness
*
*  3) When you get an Error 61 Id not found for score type F
*     3.1) Check who did the fumble recovery
*     3.2) Treat as newbie
*
*  4) You may have to doctor the html if you have 2 players
*      with the same name (Init.Surname) in the same team
*
*  5) When you get unable to discern home team:
*     The teams should be on the line after the date 
*     and be centred roughly
*     (not the same line)
* 
*  6) Player not found may happen if you have
*     a) 2 overlapping SERVE records for a player
*        use the K_TEST_PLAYER = "Murray" to 
*        find out (might be deleted but not packed)
*     b) wrong start date for a player so that
*        he misses the game date
* 

parameters cWeek
LOCAL oParser
*SET DATE BRITISH


SET DEFAULT TO e:\tfl\dev\  && Home


set procedure to NFLLib
SET PROCEDURE TO SCOREPARSER additive
SET PROCEDURE TO NFLGame ADDITIVE
set procedure to NFLTeams additive
set procedure to NFLPlayer additive
set procedure to NFLDataController additive
set procedure to FGAttempt additive

SET SAFETY off
SET ALTERNATE off
SET ALTERNATE TO 
ERASE ( "raw" + cWeek + ".txt" )
SET ALTERNATE TO ( "raw" + cWeek + ".txt" )
SET ALTERNATE on

*debug
clear

oParser = CREATEOBJECT("ScoreParser",cWeek)
oParser.Process()


SET ALTERNATE off
SET ALTERNATE TO
close database
clear all

SET DEFAULT TO e:\tfl\dev\  && home 

SET SAFETY on
