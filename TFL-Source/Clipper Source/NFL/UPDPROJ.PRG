***
***  Updates the Player database with Projected Scores
***
*
*		For each Player
*			Determine ID
*        for each TD score
*   			tally scores
*			update fields in PLAYER
*
#INCLUDE "nfl.h"

static aTDp[ 5, 5 ]
static aTDr[ 5, 5 ]
static aOL[  5, 5 ]
static aFG[  5, 5 ]

PROCEDURE UpdProj

	local nScores, nTotTDp := 0, nTotTDr := 0; nTotFG := 0

   *---load Passing array
   aTDp[ 1, 1 ] := 1
   aTDp[ 1, 2 ] := 2
   aTDp[ 1, 3 ] := 2
   aTDp[ 1, 4 ] := 3
   aTDp[ 1, 5 ] := 4

   aTDp[ 2, 1 ] := 0
   aTDp[ 2, 2 ] := 1
   aTDp[ 2, 3 ] := 2
   aTDp[ 2, 4 ] := 2
   aTDp[ 2, 5 ] := 3

   aTDp[ 3, 1 ] := 0
   aTDp[ 3, 2 ] := 0
   aTDp[ 3, 3 ] := 2
   aTDp[ 3, 4 ] := 2
   aTDp[ 3, 5 ] := 2

   aTDp[ 4, 1 ] := 0
   aTDp[ 4, 2 ] := 0
   aTDp[ 4, 3 ] := 1
   aTDp[ 4, 4 ] := 2
   aTDp[ 4, 5 ] := 2

   aTDp[ 5, 1 ] := 0
   aTDp[ 5, 2 ] := 0
   aTDp[ 5, 3 ] := 0
   aTDp[ 5, 4 ] := 1
   aTDp[ 5, 5 ] := 2

   *---load Rushing array
   aTDr[ 1, 1 ] := 1
   aTDr[ 1, 2 ] := 1
   aTDr[ 1, 3 ] := 1
   aTDr[ 1, 4 ] := 2
   aTDr[ 1, 5 ] := 3

   aTDr[ 2, 1 ] := 0
   aTDr[ 2, 2 ] := 1
   aTDr[ 2, 3 ] := 1
   aTDr[ 2, 4 ] := 1
   aTDr[ 2, 5 ] := 2

   aTDr[ 3, 1 ] := 0
   aTDr[ 3, 2 ] := 0
   aTDr[ 3, 3 ] := 1
   aTDr[ 3, 4 ] := 1
   aTDr[ 3, 5 ] := 1

   aTDr[ 4, 1 ] := 0
   aTDr[ 4, 2 ] := 0
   aTDr[ 4, 3 ] := 0
   aTDr[ 4, 4 ] := 1
   aTDr[ 4, 5 ] := 1

   aTDr[ 5, 1 ] := 0
   aTDr[ 5, 2 ] := 0
   aTDr[ 5, 3 ] := 0
   aTDr[ 5, 4 ] := 0
   aTDr[ 5, 5 ] := 1

   *---load Offensive Line array
   aOL[ 1, 1 ] := 0
   aOL[ 1, 2 ] := 0
   aOL[ 1, 3 ] := 1
   aOL[ 1, 4 ] := 1
   aOL[ 1, 5 ] := 2

   aOL[ 2, 1 ] := 0
   aOL[ 2, 2 ] := 0
   aOL[ 2, 3 ] := 0
   aOL[ 2, 4 ] := 1
   aOL[ 2, 5 ] := 1

   aOL[ 3, 1 ] := -1
   aOL[ 3, 2 ] := 0
   aOL[ 3, 3 ] := 0
   aOL[ 3, 4 ] := 0
   aOL[ 3, 5 ] := 1

   aOL[ 4, 1 ] := -1
   aOL[ 4, 2 ] := -1
   aOL[ 4, 3 ] := 0
   aOL[ 4, 4 ] := 0
   aOL[ 4, 5 ] := 0

   aOL[ 5, 1 ] := -2
   aOL[ 5, 2 ] := -1
   aOL[ 5, 3 ] := -1
   aOL[ 5, 4 ] := 0
   aOL[ 5, 5 ] := 0

   *---load FG array
   aFG[ 1, 1 ] := 15
   aFG[ 1, 2 ] := 17
   aFG[ 1, 3 ] := 19
   aFG[ 1, 4 ] := 21
   aFG[ 1, 5 ] := 18

   aFG[ 2, 1 ] := 20
   aFG[ 2, 2 ] := 22
   aFG[ 2, 3 ] := 24
   aFG[ 2, 4 ] := 24
   aFG[ 2, 5 ] := 20

   aFG[ 3, 1 ] := 24
   aFG[ 3, 2 ] := 26
   aFG[ 3, 3 ] := 28
   aFG[ 3, 4 ] := 30
   aFG[ 3, 5 ] := 27

   aFG[ 4, 1 ] := 20
   aFG[ 4, 2 ] := 19
   aFG[ 4, 3 ] := 25
   aFG[ 4, 4 ] := 25
   aFG[ 4, 5 ] := 25

   aFG[ 5, 1 ] := 16
   aFG[ 5, 2 ] := 18
   aFG[ 5, 3 ] := 23
   aFG[ 5, 4 ] := 20
   aFG[ 5, 5 ] := 16

	select SCORE
	nOldOrd := indexord()

	select PLAYER
	go top
	do while .not. PLAYER->( eof() )
      nScores := 0
      *---Quarterbacks
      if Player->CATEGORY == QUARTERBACK_CAT .and. PLAYER->ROLE $ 'SB'
         nScores := ProjectTDp( PLAYER->CURRTEAM, PLAYER->ROLE, PLAYER->POSDESC  )
         nTotTDp += nScores
*         TestMsg( PLAYER->SURNAME + str( nScores , 4 ) )
      endif

      *---Running Backs
      if Player->CATEGORY == RUNNINGBACK_CAT .and. PLAYER->ROLE $ 'SBR'
         *--Rushing TDS
         @ 23,00 say PLAYER->SURNAME
         nScores := ProjectTDr( PLAYER->CURRTEAM, PLAYER->ROLE )
         nTotTDr += nScores
*         TestMsg( PLAYER->SURNAME + str( nScores , 4 ) )
      endif

      *---Pass Recievers
      if Player->CATEGORY == RECEIVER_CAT .and. PLAYER->ROLE $ 'SB'
         *--Receiving TDS
         @ 23,00 say PLAYER->SURNAME
         nScores := ProjectTDp( PLAYER->CURRTEAM, PLAYER->ROLE, PLAYER->POSDESC )
*         TestMsg( trim(PLAYER->FIRSTNAME) + ' ' + PLAYER->SURNAME + str( nScores , 4 ) )
      endif

      *---Place Kickers
      if Player->CATEGORY == KICKER_CAT .and. PLAYER->ROLE $ 'S'
         *--FieldGoals
         @ 23,00 say PLAYER->SURNAME
         nScores := ProjectFG( PLAYER->CURRTEAM, PLAYER->SCORES )
         nTotFG += nScores
*         TestMsg( trim(PLAYER->FIRSTNAME) + ' ' + PLAYER->SURNAME + str( nScores , 4 ) )
      endif

      *---Place Kickers
      if Player->CATEGORY $ DEFENSIVE_CATEGORIES .and. PLAYER->ROLE $ 'S' .and. ;
         PLAYER->CURRTEAM <> '??'
         @ 23,00 say PLAYER->SURNAME
         nScores := ProjectDef( PLAYER->CURRTEAM, PLAYER->SCORES )
*         TestMsg( trim(PLAYER->FIRSTNAME) + ' ' + PLAYER->SURNAME + str( nScores , 4 ) )
      endif

      replace PLAYER->PROJECTED 	with nScores
		PLAYER->( dbskip() )
	enddo

   select SCORE
	set order to nOldOrd
   
	ErrorMsg( "Projections updated! total projected TDp" + str( nTotTDp, 5 ) )
	ErrorMsg( "Total projected TDr" + str( nTotTDr, 5 ) )
	ErrorMsg( "Total projected FG" + str( nTotFG, 5 ) )

   @ 23, 00

RETURN


FUNCTION DomedTeam


  parameters cTeam

  local lDomed := .f.

  if cTeam $ 'SR,NO,AF,DL,MV'
     lDomed := .t.
  endif

RETURN lDomed



FUNCTION ProjectDef

   parameters cTeam, nCareerScores

   local nProjected := 0, cDrafted

   *---Round drafted
   cDrafted := substr( Drafted( PLAYER->PLAYERID ), 1, 3 )
   do case
   case cDrafted = 'D1'
     nProjected := 8
   case cDrafted = 'D2'
     nProjected := 4
   case cDrafted = 'D3'
     nProjected := 2
   case cDrafted = 'D4'
     nProjected := 1
   endcase
   *---First played taken bonus
   if substr( cDrafted, 3, 1 ) = '*'
      nProjected += 2
   endif

   *---Bonus for scooring
   if nCareerScores > 0
      nProjected += 1
   endif

   *---Seasons
   if PLAYER->ROOKIEYR > 0
     nSeasons := CareerSpan()
     if nSeasons < 4
        nProjected += nSeasons
     else
        *---4 or more
        if nSeasons > 8
           nProjected += 6
        else
           nProjected += 4
        endif
     endif
     if nSeasons > 10
        nProjected -= ( nSeasons - 10 )
     endif
   endif

   *---Injury adjustment
   nProjected -= PLAYER->INJURY

RETURN nProjected



FUNCTION ProjectFG

   parameters cTeam, nCareerFG

   local nProjected := 0
   local cTeamPRate, cOpp, cTeamDRate, nThis, cOLRate, cOppPassRush, nOLModifier

   if TEAM->( dbseek( G_Season + cTeam ) )
      cTeamPRate := substr( TEAM->RATE, 1, 1 )
      cTeamRRate := substr( TEAM->RATE, 2, 1 )
   else
      cTeamPRate := ''
      cTeamRRate := ''
      TestMsg( cTeam + ' not found ' + PLAYER->SURNAME )
   endif

   if .not. empty( cTeamPRate )
      nThis := aFG[ asc( cTeamPRate ) - 64, asc( cTeamRRate ) - 64 ]

      nProjected += nThis
   endif

   if nProjected > 0
      *---Apply Domed stadium bonus
      if DomedTeam( cTeam )
         nProjected += 2
      endif
      *---Apply Veteran bonus
      nProjected += int( nCareerFG / 80 )
   endif

RETURN nProjected


FUNCTION ProjectTDp

   parameters cTeam, cRole, cPos

   local nProjected := 0
   local cTeamPRate, cOpp, cTeamDRate, nThis, cOLRate, cOppPassRush, nOLModifier

   if TEAM->( dbseek( G_Season + cTeam ) )
      cTeamPRate := substr( TEAM->RATE, 1, 1 )
      cOLRate    := substr( TEAM->RATE, 3, 1 )
   else
      cTeamPRate := ''
      TestMsg( cTeam + ' not found' )
   endif

   if .not. empty( cTeamPRate )
      SCHED->( dbseek( G_Season ) )
      do while .not. SCHED->( eof() ) .and. SCHED->SEASON = G_Season
         cOpp := ""
         cTeamDRate := ''
         if SCHED->AWAYTEAM = cTeam
            cOpp := SCHED->HOMETEAM
         endif
         if SCHED->HOMETEAM = cTeam
            cOpp := SCHED->AWAYTEAM
         endif
         if .not. empty( cOpp )
            *-- game on
            if TEAM->( dbseek( G_Season + cOpp ) )
               cTeamDRate   := substr( TEAM->RATE, 6, 1 )
               cOppPassRush := substr( TEAM->RATE, 4, 1 )
            else
               TestMsg( cOpp + ' rate not found' )
            endif
         endif
         if .not. empty( cTeamDRate )
            nThis       := aTDp[ asc( cTeamPRate ) - 64, asc( cTeamDRate ) - 64 ]
            nOLModifier := aOL[ asc( cOLRate ) - 64, asc( cOppPassRush )- 64 ]
            nThis += ( nOLModifier / 2 )
            if PLAYER->SURNAME = "Warner"
*               TestMsg( cTeamPRate + ' v ' + cTeamDRate + ' (' + cOpp + ') = ' + str( nThis, 3 ) )
            endif

            nProjected += nThis
         endif

         SCHED->( dbskip() )
      enddo

   endif

   if nProjected > 0
      if cPos = 'QB'
         if cRole = 'S'
            nProjected := int( (nProjected * .9 ) + .5 )
         endif
         if cRole = 'B'
            nProjected := int( (nProjected * .1 ) + .5 )
         endif
      else
         *---Recievers
         if cRole = 'S'
            if cPos = 'WR1'
              nProjected := int( (nProjected * .35 ) + .5 )
            endif
            if cPos = 'WR2'
              nProjected := int( (nProjected * .25 ) + .5 )
            endif
            if cPos = 'TE'
              nProjected := int( (nProjected * .1 ) + .5 )
            endif
         endif
         if cRole = 'B'
            if cPos = 'WR1'
              nProjected := int( (nProjected * .15 ) + .5 )
            endif
            if cPos = 'WR2'
              nProjected := int( (nProjected * .10 ) + .5 )
            endif
            if cPos = 'TE'
              nProjected := int( (nProjected * .05 ) + .5 )
            endif
         endif

      endif

   endif

RETURN nProjected


FUNCTION ProjectTDr

   parameters cTeam, cRole

   local nProjected := 0
   local cTeamRRate, cOpp, cTeamDRate, nThis, cOLRate, cOppPassRush, nOLModifier

   if TEAM->( dbseek( G_Season + cTeam ) )
      cTeamRRate := substr( TEAM->RATE, 2, 1 )
   else
      cTeamRRate := ''
      TestMsg( cTeam + ' not found' )
   endif

   if .not. empty( cTeamRRate )
      SCHED->( dbseek( G_Season ) )
      do while .not. SCHED->( eof() ) .and. SCHED->SEASON = G_Season
         cOpp := ""
         cTeamDRate := ''
         if SCHED->AWAYTEAM = cTeam
            cOpp := SCHED->HOMETEAM
         endif
         if SCHED->HOMETEAM = cTeam
            cOpp := SCHED->AWAYTEAM
         endif
         if .not. empty( cOpp )
            *-- game on
            if TEAM->( dbseek( G_Season + cOpp ) )
               cTeamDRate   := substr( TEAM->RATE, 5, 1 )
            else
               TestMsg( cOpp + ' rate not found' )
            endif
         endif
         if .not. empty( cTeamDRate )
            nThis       := aTDr[ asc( cTeamRRate ) - 64, asc( cTeamDRate ) - 64 ]
            if PLAYER->SURNAME = "Davis"
*               TestMsg( cTeamRRate + ' v ' + cTeamDRate + ' (' + cOpp + ') = ' + str( nThis, 3 ) )
            endif

            nProjected += nThis
         endif

         SCHED->( dbskip() )
      enddo

   endif

   if nProjected > 0
      if cRole = K_STARTER
         nProjected := int( (nProjected * .75 ) + .5 )
      endif
      if cRole = K_BACKUP
         nProjected := int( (nProjected * .20 ) + .5 )
      endif
      if cRole = K_RESERVE
         nProjected := int( (nProjected * .05 ) + .5 )
      endif
   endif

RETURN nProjected


*EOF: UpdProj.PRG