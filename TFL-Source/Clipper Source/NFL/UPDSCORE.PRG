***
***  Updates the Player database with Score totals
***
*
*		For each Player
*			Determine ID
*        for each TD score
*   			tally scores
*			update fields in PLAYER
*
#INCLUDE "nfl.h"

PROCEDURE UpdScore

	local nScores, nCurScores, t, cScoreType, nOldOrd, cAllScores

	select 0
	OpenDbf( 'SCORE', g_nfl_path, .t. )	
	select 0
	OpenDbf( 'PLAYER', g_nfl_path, .t. )		
	
	select SCORE
	nOldOrd := indexord()

	select PLAYER
	go top
	do while .not. PLAYER->( eof() )
		cPlayerID = PLAYER->PLAYERID
      @ 23,00 say PLAYER->SURNAME
      if Player->CATEGORY == KICKER_CAT
         cAllScores := SCORE_FIELD_GOAL
      else
         cAllScores := DEFENSIVE_SCORES + SCORE_KICK_RETURN + ;
                       SCORE_TD_RUN + SCORE_TD_PASS
      endif
      @ 23, 60 say cAllScores

      nScores := 0
      nCurScores := 0
      for t = 1 to len( cAllScores )
         cScoreType := substr( cAllScores, t, 1 )
         do TallyScores with cScoreType, nScores, nCurScores, cPlayerID
      next
      if PLAYER->CATEGORY  = QUARTERBACK_CAT
         do TallyTDPasses with cPlayerID, nScores, nCurScores
      endif

		replace PLAYER->SCORES 	 	with nScores
		replace PLAYER->CURSCORES  with nCurScores

		PLAYER->( dbskip() )
	enddo

   select SCORE
	set order to nOldOrd
   
	ErrorMsg( "Scores updated!" )

   @ 23, 00

RETURN


PROCEDURE TallyScores

   parameters cScoreType, nScores, nCurScores,  cPlayerID

	select SCORE
   set order to SCORE_BY_TYPE1
   @ 23, 20 say cPlayerID
   @ 23, 30 say cScoreType
   SCORE->( dbseek( cPlayerID + cScoreType ) )
   do while .not. SCORE->( eof() ) .and. ;
                  SCORE->PLAYERID1 = cPlayerID .and. ;
                  SCORE->SCORE     = cScoreType
      nScores++
      if SCORE->SEASON = G_season
         nCurScores++
      endif

      SCORE->( dbskip() )
   enddo

RETURN

Procedure TallyTDPasses

   parameters cPlayerID, nScores, nCurScores

	select SCORE
   set order to SCORE_BY_TYPE2
   SCORE->( dbseek( cPlayerID + SCORE_TD_PASS ) )
   do while .not. SCORE->( eof() ) .and. ;
                  SCORE->PLAYERID2 = cPlayerID .and. ;
                  SCORE->SCORE     = SCORE_TD_PASS
      nScores++
      if SCORE->SEASON = G_season
         nCurScores++
      endif

      SCORE->( dbskip() )
   enddo

RETURN


*EOF: LOADPICK.PRG