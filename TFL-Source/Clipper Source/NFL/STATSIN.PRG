*****************************************************************************
*  Program Name.....: statsin.pre 
*  Programmer.......: Steve Colonna 
*  System...........:
*  Module...........:
*  Date.............: 20/8/1990 at 19:56
*  Copyright........: (c) 1990 by Knoware 
*  Called From......:
*  Purpose..........:
*
*	 To record individual game Stats
*
*............................................................................
*  Revision.........: 1.0 Last Revised: 20/8/1990 at 19:56
*  Description......: Initial Creation.
*............................................................................
***************************** ALL RIGHTS RESERVED ***************************

#INCLUDE "nfl.h"

PROCEDURE StatsIn

parameters cGameKey

   private nmess, poption1, mopt[ 5], mmess[ 5]

	if INIT_STATS()    && Initialisation routine
		poption1 = 1
	else
      RETU
   endif

   do while poption1 <> 0 

		ShowTeamStats( cGameKey )

      poption1 = flatmenu (mopt, mmess, poption1, 22, .T.)
      poption1 = if( poption1 = 0, 1, poption1)
   
      clrmenu()  

      do case

         case mopt[poption1] = 'X'

				CLEAR_WINDOW( 22, 0, 24, 79 )
            poption1 = 0

         case mopt[poption1] = 'P'

				lHomeTeam = HomeOrAway()
				PassingStats( lHomeTeam )

			case mopt[poption1] = 'R'

				lHomeTeam = HomeOrAway()
				RushingStats( lHomeTeam )

         case mopt[poption1] = 'C'

				lHomeTeam = HomeOrAway()
				CatchingStats( lHomeTeam )
            
         case mopt[poption1] = 'D'

				lHomeTeam = HomeOrAway()
				DefenceStats( lHomeTeam )
            
      endcase
   
   enddo 

	select WORKFILE
	use

   select COMP
   use
   
	select STAT
	set order to 1

return


function INIT_STATS
   ***
   ***   Initialise all variables, arrays and open files
   ***

   mopt[1]  = 'X-exit'    
   mopt[2]  = 'Rushing'
   mopt[3]  = 'Passing'
   mopt[4]  = 'Catching'       
   mopt[5]  = 'Defense'       

   mmess[1] = 'Return to Player Menu'           
   mmess[2] = 'Update Rushing Stats'
   mmess[3] = 'Update Passing Stats'
   mmess[4] = 'Update Receiving Stats'
   mmess[5] = 'Update Defensive Stats'

	select 0
	OpenDbf( 'COMP', g_ty_path )

	LoadPlayers()			&&	in scores.prg
	*---Make a temporary workfile
	workfile = g_nfl_path + TEMPORARY_FILE_INDICATOR + "STAT"
	if .not. file( workfile + ".dbf")
		makefile( workfile + ".dbf", 7, ;
						"PLAYERID   C  8 0", ;
						"FTEAM      C  3 0", ;
						"NAME       C 19 0", ;
						"STAT1      N  5 1", ;
						"STAT2      N  5 1", ;
						"STAT3      N  5 1", ;
						"STAT4      N  5 1"  )
	endif


	select 0
	if net_use( workfile, .t., 2, "WORKFILE" )
		tixfile = workfile + '.ntx'
		if .not. file( tixfile )
			index on NAME to ( tixfile  )
		else
			set index to ( tixfile )
		endif
	else
		error_msg( "Work file is in use, try again later")
		retu ( .f. )
	endif

	select STAT
	set order to 2

	CLEAR_WINDOW( 22, 0, 24, 79 )

return .t.


function DefenceStats

	parameters lHomeTeam

	*---Load team to temp file with any current stats
	if lHomeTeam
		cworkfile = "HOMETEAM"
	else
		cworkfile = "AWAYTEAM"
	endif

	select WORKFILE
	zap

	select &cWorkFile
	go top

	do while .not. eof()

		if CATEGORY $ DEFENSIVE_CATEGORIES

			cPlayerId = PLAYERID
			sack_cnt    = 0
			int_cnt    = 0

			select STAT
			dbseek( cPlayerId + QUARTERBACK_SACKS + cGameKey )
			if found()
				ADDTO( sack_cnt, QTY )
				do RLOCKIT with "Stat file"
				delete
			endif
			dbseek( cPlayerId + INTERCEPTIONS_MADE + cGameKey )
			if found()
				ADDTO( int_cnt, QTY )
				do RLOCKIT with "Stat file"
				delete
			endif

			select WORKFILE
			do ADDRECIT with "Workfile"
			replace PLAYERID with cPlayerId
			replace NAME     with PlayerName( cPlayerId )
			replace STAT1    with sack_cnt
			replace STAT2    with int_cnt

		endif

		select &cWorkFile
		skip
	enddo

	select TEAM
	if lHomeTeam
		dbseek( SCHED->SEASON + SCHED->HOMETEAM )
	else
		dbseek( SCHED->SEASON + SCHED->AWAYTEAM )
	endif

	*---dbedit it makeing mods including on the fly player adds
	select WORKFILE
	go top
	private mfields[ 3 ], mcols[ 3 ]

	mfields[ 1 ] = "NAME"
	mfields[ 2 ] = "str(STAT1,4,1)"
	mfields[ 3 ] = "str(STAT2,4,0)"
	mcols[   1 ] = "Player"
	mcols[   2 ] = "Sacks"
	mcols[   3 ] = "Ints"

	setcolor( C_help )
	popbox( 7,33,20,78 )
	lineinbox( 18 )
	inCentre( 33, 78, TEAM->TEAMNAME, 7 )

	dbedit(8,34,17,77,mfields,'UpdateDef','@X',mcols,"Ä"," ")

	unpop()

	SaveStats( DEFENSIVE_STATS )

return ( NIL )


function RushingStats

	parameters lHomeTeam

	*---Load team to temp file with any current stats
	if lHomeTeam
		cworkfile = "HOMETEAM"
	else
		cworkfile = "AWAYTEAM"
	endif

	select WORKFILE
	zap

	select &cWorkFile
	go top

	do while .not. eof()

		if CATEGORY $ RUSHING_CATEGORIES

			cPlayerId = PLAYERID
			rc_cnt    = 0
			ry_cnt    = 0

			select STAT
			dbseek( cPlayerId + RUSHING_CARRIES + cGameKey )
			if found()
				ADDTO( rc_cnt, QTY )
				do RLOCKIT with "Stat file"
				delete
			endif
			dbseek( cPlayerId + RUSHING_YARDS + cGameKey )
			if found()
				ADDTO( ry_cnt, QTY )
				do RLOCKIT with "Stat file"
				delete
			endif

			select WORKFILE
			do ADDRECIT with "Workfile"
			replace PLAYERID with cPlayerId
			replace NAME     with PlayerName( cPlayerId )
			replace FTEAM    with PlaysFor( cPlayerID, "G1", G_Season, .t. )
			replace STAT1    with rc_cnt
			replace STAT2    with ry_cnt

		endif

		select &cWorkFile
		skip
	enddo

	select TEAM
	if lHomeTeam
		dbseek( SCHED->SEASON + SCHED->HOMETEAM )
	else
		dbseek( SCHED->SEASON + SCHED->AWAYTEAM )
	endif

	*---dbedit it makeing mods including on the fly player adds
	select WORKFILE
	go top
	private mfields[ 5 ], mcols[ 5 ]

	mfields[ 1 ] = "NAME"
	mfields[ 2 ] = "FTEAM"
	mfields[ 3 ] = "str(STAT1,4,0)"
	mfields[ 4 ] = "str(STAT2,4,0)"
	mfields[ 5 ] = "AvgPerCarry()"
	mcols[   1 ] = "Player"
	mcols[   2 ] = "G1"
	mcols[   3 ] = "Carries"
	mcols[   4 ] = "Yards"
	mcols[   5 ] = " Avg"

	setcolor( C_help )
	popbox( 7,31,20,78 )
	lineinbox( 18 )
	inCentre( 31, 78, TEAM->TEAMNAME, 7 )

	dbedit(8,33,17,77,mfields,'UpdateRY','@X',mcols,"Ä"," ")

	unpop()

	SaveStats( RUSHING_STATS )

return ( NIL )


FUNCTION CatchingStats( lHomeTeam )

	*---Load team to temp file with any current stats
	if lHomeTeam
		cworkfile = "HOMETEAM"
	else
		cworkfile = "AWAYTEAM"
	endif

	select WORKFILE
	zap

	select &cWorkFile
	go top

	do while .not. eof()

		if CATEGORY $ RECEIVING_CATEGORIES

			cPlayerId := PLAYERID
			rc_cnt    := 0
			ry_cnt    := 0

			select STAT
			dbseek( cPlayerId + PASSES_CAUGHT + cGameKey )
			if found()
				ADDTO( rc_cnt, QTY )
				do RLOCKIT with "Stat file"
				delete
			endif
			dbseek( cPlayerId + RECEPTION_YARDAGE + cGameKey )
			if found()
				ADDTO( ry_cnt, QTY )
				do RLOCKIT with "Stat file"
				delete
			endif

         *testmsg( "playerid=" + cPlayerID )
			select WORKFILE
			do ADDRECIT with "Workfile"
			replace PLAYERID with cPlayerId
			replace NAME     with PlayerName( cPlayerId )
			replace FTEAM    with PlaysFor( cPlayerID, "G1", G_Season, .t. )
			replace STAT1    with rc_cnt
			replace STAT2    with ry_cnt

		endif

		select &cWorkFile
		skip
	enddo

	select TEAM
	if lHomeTeam
		dbseek( SCHED->SEASON + SCHED->HOMETEAM )
	else
		dbseek( SCHED->SEASON + SCHED->AWAYTEAM )
	endif

	*---dbedit it makeing mods including on the fly player adds
	select WORKFILE
	go top
	private mfields[ 5 ], mcols[ 5 ]

	mfields[ 1 ] = "NAME"
	mfields[ 2 ] = "FTEAM"
	mfields[ 3 ] = "str(STAT1,4,0)"
	mfields[ 4 ] = "str(STAT2,4,0)"
	mfields[ 5 ] = "AvgPerCarry()"
	mcols[   1 ] = "Player"
	mcols[   2 ] = "G1"
	mcols[   3 ] = "Catches"
	mcols[   4 ] = "Yards"
	mcols[   5 ] = " Avg"

	setcolor( C_help )
	popbox( 7,33,20,78 )
	lineinbox( 18 )
	inCentre( 33, 78, TEAM->TEAMNAME, 7 )

	dbedit(8,35,17,77,mfields,'UpdatePY','@X',mcols,"Ä"," ")

	unpop()

	SaveStats( RECEPTION_STATS )

return ( NIL )


function PassingStats

	parameters lHomeTeam

	*---Load team to temp file with any current stats
	if lHomeTeam
		cworkfile = "HOMETEAM"
	else
		cworkfile = "AWAYTEAM"
	endif

	select WORKFILE
	zap

	select &cWorkFile
	go top

	do while .not. eof()

		if CATEGORY $ PASSING_CATEGORIES

			cPlayerId = PLAYERID

			com_cnt    = 0
			att_cnt    = 0
			pyd_cnt    = 0
			int_cnt    = 0

			select STAT
			dbseek( cPlayerId + PASS_COMPLETIONS + cGameKey )
			if found()
				ADDTO( com_cnt, QTY )
				do RLOCKIT with "Stat file"
				delete
			endif
			dbseek( cPlayerId + PASSING_YARDAGE + cGameKey )
			if found()
				ADDTO( pyd_cnt, QTY )
				do RLOCKIT with "Stat file"
				delete
			endif
			dbseek( cPlayerId + PASSING_ATTEMPTS + cGameKey )
			if found()
				ADDTO( att_cnt, QTY )
				do RLOCKIT with "Stat file"
				delete
			endif
			dbseek( cPlayerId + PASSES_INTERCEPTED + cGameKey )
			if found()
				ADDTO( int_cnt, QTY )
				do RLOCKIT with "Stat file"
				delete
			endif

			select WORKFILE
			do ADDRECIT with "Workfile"
			replace PLAYERID with cPlayerId
			replace NAME     with PlayerName( cPlayerId )
			replace FTEAM    with PlaysFor( cPlayerID, "G1", G_Season, .t. )
			replace STAT1    with com_cnt
			replace STAT2    with att_cnt
			replace STAT3    with pyd_cnt
			replace STAT4    with int_cnt

		endif

		select &cWorkFile
		skip
	enddo

	select TEAM
	if lHomeTeam
		dbseek( SCHED->SEASON + SCHED->HOMETEAM )
	else
		dbseek( SCHED->SEASON + SCHED->AWAYTEAM )
	endif

	*---dbedit it makeing mods including on the fly player adds
	select WORKFILE
	go top
	private mfields[ 7], mcols[ 7 ]

	mfields[ 1 ] = "NAME"
	mfields[ 2 ] = "FTEAM"
	mfields[ 3 ] = "str(STAT1,4,0)"
	mfields[ 4 ] = "str(STAT2,4,0)"
	mfields[ 5 ] = "str(STAT3,5,0)"
	mfields[ 6 ] = "str(STAT4,4,0)"
	mfields[ 7 ] = "ComplPercent()"
	mcols[   1 ] = "Player"
	mcols[   2 ] = "G1"
	mcols[   3 ] = "Comp"
	mcols[   4 ] = "Att"
	mcols[   5 ] = "Yds"
	mcols[   6 ] = "Int"
	mcols[   7 ] = "Com %"

	setcolor( C_help )
	popbox( 7,24,20,78 )
	lineinbox( 18 )
	inCentre( 24, 78, TEAM->TEAMNAME, 7 )

	dbedit(8,26,17,77,mfields,'UpdatePass','@X',mcols,"Ä"," ")

	unpop()

	SaveStats( PASSING_STATS )

return ( NIL )



function AvgPerCarry

	private cRetVal

	cRetVal = "    "

	if STAT1 > 1
		cRetVal = str( (STAT2 / STAT1), 4, 1 )
	endif

return ( cRetVal )


function ComplPercent

	private cRetVal

	cRetVal = "     "

	if STAT2 > 1
		cRetVal = str( (STAT1 / STAT2) * 100, 5, 1 )
	endif

return ( cRetVal )


function SaveStats

	parameters cStatType

	select WORKFILE
	go top
	do while .not. eof()

		do case
			case cStatType = RUSHING_STATS

				if WORKFILE->STAT1 <> 0
					UpdateStat( RUSHING_CARRIES, "STAT1" )
				endif

				if WORKFILE->STAT2 <> 0
					UpdateStat( RUSHING_YARDS, "STAT2" )
				endif

			case cStatType = PASSING_STATS

				if WORKFILE->STAT1 <> 0
					UpdateStat( PASS_COMPLETIONS, "STAT1" )
				endif

				if WORKFILE->STAT2 <> 0
					UpdateStat( PASSING_ATTEMPTS, "STAT2" )
				endif

				if WORKFILE->STAT3 <> 0
					UpdateStat( PASSING_YARDAGE, "STAT3" )
				endif

				if WORKFILE->STAT4 <> 0
					UpdateStat( PASSES_INTERCEPTED, "STAT4" )
				endif

			case cStatType = RECEPTION_STATS

				if WORKFILE->STAT1 <> 0
					UpdateStat( PASSES_CAUGHT, "STAT1" )
				endif

				if WORKFILE->STAT2 <> 0
					UpdateStat( RECEPTION_YARDAGE, "STAT2" )
				endif

			case cStatType = DEFENSIVE_STATS

				if WORKFILE->STAT1 <> 0
					UpdateStat( QUARTERBACK_SACKS, "STAT1" )
				endif

				if WORKFILE->STAT2 <> 0
					UpdateStat( INTERCEPTIONS_MADE, "STAT2" )
				endif


		endcase

		select WORKFILE
		skip
	enddo

return( NIL )


function UpdateStat

	parameters cStatType, _field

	select STAT
	STAT->( dbseek( WORKFILE->PLAYERID + cStatType + cGameKey ) )
	if found()
		if WORKFILE->&_field <> QTY
			do RLOCKIT with "Stat file"
			replace QTY with WORKFILE->&_field
		endif
	else
		do ADDRECIT with "Stat file"
		replace TEAMID   with if( lHomeTeam, SCHED->HOMETEAM, SCHED->AWAYTEAM )
		replace PLAYERID with WORKFILE->PLAYERID
		replace QTY      with WORKFILE->&_field
		replace SEASON   with mSeason
		replace WEEK     with mWeek
		replace GAMENO   with mGameNo
		replace STAT     with cStatType
	endif

return( NIL )


function UpdateDef

	parameters mode, fld_ptr

	private ret_mode

	do case
		case mode < 3
			ret_mode = 1									 && continue
		case mode = 3
			ret_mode = 0									 && exit
		case lastkey() = ESCAPE

			ret_mode = 0

		case lastkey() = ENTER

			*---Edit the stat
			mRow = row()
			nStat1 = STAT1
			nStat2 = STAT2
			vget( mrow, 62, 'nStat1', "9.9" )
			vget( mrow, 68, 'nStat2', "999" )
			vread( 1, .f. )
			if .not. abort()
				do Rlockit with "Defensive file"
				replace STAT1 with nStat1
				replace STAT2 with nStat2
			endif

			ret_mode = 2

		case lastkey() = INSERT

			*---Add a player on the fly
			cPlayerID = ""
			mteam = iif( lHomeTeam, SCHED->HOMETEAM, SCHED->AWAYTEAM )
			pushgets()
			do ADDONFLY with cPlayerID, mteam, LINEMAN_CAT
			popgets()

			***
			***   Also add to the Temp Stat file
			***
			select WORKFILE
			do ADDRECIT with "Workfile"
			replace PLAYERID with cPlayerId
			replace NAME     with PlayerName( cPlayerId )

			ret_mode = 2

		case lastkey() > 64 .and. lastkey() < 123
			*---Alpha key
			char = upper( chr( lastkey() ) )
			set softseek on
			dbseek( char )
			set softseek off
			ret_mode = 2									 && continue and redraw
		otherwise
			ret_mode = 1									 && continue
	endcase

	*---Draw Total line
	private nTotStat1, nTotStat2
	nTotStat1    = totalStat( "STAT1" )
	nTotStat2    =	totalStat( "STAT2" )
	@ 19, 41 say 'TOTAL DEFENSE'
	@ 19, 61 say str( nTotStat1, 4, 1 )
	@ 19, 67 say str( nTotStat2, 4, 0 )

return(ret_mode)


function UpdateRY

	parameters mode, fld_ptr

	private ret_mode

	do case
		case mode < 3
			ret_mode = 1									 && continue
		case mode = 3
			ret_mode = 0									 && exit
		case lastkey() = ESCAPE

			ret_mode = 0

		case lastkey() = ENTER

			*---Edit the stat
			mRow = row()
			nStat1 = STAT1
			nStat2 = STAT2
			vget( mrow, 60, 'nStat1', "99" )
			vget( mrow, 67, 'nStat2', "999" )
			vread( 1, .f. )
			if .not. abort()
				do Rlockit with "Rushing file"
				replace STAT1 with nStat1
				replace STAT2 with nStat2
			endif

			ret_mode = 2

		case lastkey() = INSERT

			*---Add a player on the fly
			cPlayerID = ""
			mteam = iif( lHomeTeam, SCHED->HOMETEAM, SCHED->AWAYTEAM )
			pushgets()
			do ADDONFLY with cPlayerID, mteam, RUNNINGBACK_CAT
			popgets()

			***
			***   Also add to the Temp Stat file
			***
			select WORKFILE
			do ADDRECIT with "Workfile"
			replace PLAYERID with cPlayerId
			replace NAME     with PlayerName( cPlayerId )

			ret_mode = 2

		case lastkey() > 64 .and. lastkey() < 123
			*---Alpha key
			char = upper( chr( lastkey() ) )
			set softseek on
			dbseek( char )
			set softseek off
			ret_mode = 2									 && continue and redraw
		otherwise
			ret_mode = 1									 && continue
	endcase

	*---Draw Total line
	totalLine('TOTAL RUSHING')

return(ret_mode)


function UpdatePY

	parameters mode, fld_ptr

	private ret_mode

	do case
		case mode < 3
			ret_mode = 1									 && continue
		case mode = 3
			ret_mode = 0									 && exit
		case lastkey() = ESCAPE

			ret_mode = 0

		case lastkey() = ENTER

			*---Edit the stat
			mRow = row()
			nStat1 = STAT1
			nStat2 = STAT2
			vget( mrow, 61, 'nStat1', "99" )
			vget( mrow, 68, 'nStat2', "999" )
			vread( 1, .f. )
			if .not. abort()
				do Rlockit with "Catching file"
				replace STAT1 with nStat1
				replace STAT2 with nStat2
			endif

			ret_mode = 2

		case lastkey() = INSERT

			*---Add a player on the fly
			cPlayerID = ""
			mteam = iif( lHomeTeam, SCHED->HOMETEAM, SCHED->AWAYTEAM )
			pushgets()
			do ADDONFLY with cPlayerID, mteam, RECEIVER_CAT
			popgets()

			***
			***   Also add to the Temp Stat file
			***
			select WORKFILE
			do ADDRECIT with "Workfile"
			replace PLAYERID with cPlayerId
			replace NAME     with PlayerName( cPlayerId )

			ret_mode = 2

		case lastkey() > 64 .and. lastkey() < 123
			*---Alpha key
			char = upper( chr( lastkey() ) )
			set softseek on
			dbseek( char )
			set softseek off
			ret_mode = 2									 && continue and redraw
		otherwise
			ret_mode = 1									 && continue
	endcase

	totalLine( 'TOTAL RECEIVING' )

return(ret_mode)


function TotalLine

	parameters cTotName
	*---Draw Total line
	private nTeamCarries, nTeamYards

	nTeamCarries = totalStat( "STAT1" )
	nTeamYards   =	totalStat( "STAT2" )
	@ 19, 39 say cTotName
	@ 19, 59 say str( nTeamCarries, 4, 0 )
	@ 19, 67 say str( nTeamYards  , 4, 0 )
	if nTeamCarries > 0
		@ 19, 72 say str( ( nTeamYards  /  nTeamCarries ) , 5, 1 )
	endif

return( .t. )


function UpdatePass

	parameters mode, fld_ptr

	private ret_mode

	do case
		case mode < 3
			ret_mode = 1									 && continue
		case mode = 3
			ret_mode = 0									 && exit
		case lastkey() = ESCAPE

			ret_mode = 0

		case lastkey() = ENTER

			*---Edit the stat
			mRow = row()
			nStat1 = STAT1
			nStat2 = STAT2
			nStat3 = STAT3
			nStat4 = STAT4
			vget( mrow, 53, 'nStat1', "99" )
			vget( mrow, 58, 'nStat2', "99" )
			vget( mrow, 63, 'nStat3', "999" )
			vget( mrow, 70, 'nStat4', "9" )
			vread( 1, .f. )
			if .not. abort()
				do Rlockit with "Rushing file"
				replace STAT1 with nStat1
				replace STAT2 with nStat2
				replace STAT3 with nStat3
				replace STAT4 with nStat4
			endif

			ret_mode = 2

		case lastkey() = INSERT

			*---Add a player on the fly
			cPlayerID = ""
			mteam = iif( lHomeTeam, SCHED->HOMETEAM, SCHED->AWAYTEAM )
			pushgets()
			do ADDONFLY with cPlayerID, mteam, QUARTERBACK_CAT
			popgets()

			***
			***   Also add to the Temp Stat file
			***
			select WORKFILE
			do ADDRECIT with "Workfile"
			replace PLAYERID with cPlayerId
			replace NAME     with PlayerName( cPlayerId )

			ret_mode = 2

		case lastkey() > 64 .and. lastkey() < 123
			*---Alpha key
			char = upper( chr( lastkey() ) )
			set softseek on
			dbseek( char )
			set softseek off
			ret_mode = 2									 && continue and redraw
		otherwise
			ret_mode = 1									 && continue
	endcase

	*---Draw Total line
	private nTotStat1, nTotStat2, nTotStat3, nTotStat4
	nTotStat1    = totalStat( "STAT1" )
	nTotStat2    =	totalStat( "STAT2" )
	nTotStat3    = totalStat( "STAT3" )
	nTotStat4    =	totalStat( "STAT4" )
	@ 19, 30 say 'TOTAL PASSING'
	@ 19, 51 say str( nTotStat1, 4, 0 )
	@ 19, 56 say str( nTotStat2, 4, 0 )
	@ 19, 62 say str( nTotStat3, 4, 0 )
	@ 19, 67 say str( nTotStat4, 4, 0 )
	if nTotStat2 > 0
		@ 19, 72 say str( ( nTotStat1 / nTotStat2 ) * 100, 5, 1 )
	endif

return(ret_mode)



function TotalStat

	parameters cStatField

	private oldrec, nTotal

	nTotal = 0

	select WORKFILE
	oldrec = recno()

*	sum ( cStatField ) to nTotal
	go top
	do while .not. eof()
		ADDTO( nTotal, &cStatField )
		skip
	enddo

	go oldrec

return( nTotal )


function HomeOrAway

	private cTeam

	if dialog( 19,25, "Home Team or Away Team", "Away", "Home", .f. )

		cTeam = SCHED->AWAYTEAM
		lHomeTeam = .f.

	else

		cTeam = SCHED->HOMETEAM
		lHomeTeam = .t.

	endif


return( lHomeTeam )


#define 	ANCHOR_LINE		10
#define 	AWAY_START 		10
#define 	HOME_START 		50

function ShowTeamStats

	parameters cGameKey

	*---Initialise counters
	private aTotYards[2], aRushYards[2], aPassYards[2]
	private aTotPlays[2], aRushPlays[2], aPassPlays[2]
	private aSacks[2],    aIntercepts[2], nTeamFlag, nOldOrder

	*---Clear Screen
	setcolor( c_blank )
	CLEAR_WINDOW( 9, 0, 20, 79 )

	*---Descriptors
	setcolor( c_desc )
	StatDesc( AWAY_START )
	StatDesc( HOME_START )

	afill( aSacks,      0 )
	afill( aIntercepts, 0 )
	afill( aTotYards,   0 )
	afill( aRushYards,  0 )
	afill( aPassYards,  0 )
	afill( aTotPlays,   0 )
	afill( aRushPlays,  0 )
	afill( aPassPlays,  0 )

	*---Process data
	select STAT
	nOldOrder = indexord()
	set order to 1
	STAT->( dbseek( cGameKey ) )
	do while .not. eof() .and. SEASON+WEEK+GAMENO = cGameKey

		nTeamFlag = iif( STAT->TEAMID = SCHED->AWAYTEAM, 1, 2 )

		do case
			case STAT = RUSHING_CARRIES

				ADDTO(  aTotPlays[ nTeamFlag ], QTY )
				ADDTO( aRushPlays[ nTeamFlag ], QTY )

			case STAT = RUSHING_YARDS

				ADDTO(  aTotYards[ nTeamFlag ], QTY )
				ADDTO( aRushYards[ nTeamFlag ], QTY )

			case STAT = PASSING_ATTEMPTS

				ADDTO(  aTotPlays[ nTeamFlag ], QTY )
				ADDTO( aPassPlays[ nTeamFlag ], QTY )

			case STAT = RECEPTION_YARDAGE

				ADDTO(  aTotYards[ nTeamFlag ], QTY )
				ADDTO( aPassYards[ nTeamFlag ], QTY )

			case STAT = QUARTERBACK_SACKS

				ADDTO(  aSacks[ nTeamFlag ], QTY )

			case STAT = INTERCEPTIONS_MADE

				ADDTO(  aIntercepts[ nTeamFlag ], QTY )

		endcase

		select STAT
		skip
	enddo
	set order to nOldOrder

#define 	AWAY_COLUMN			25
#define 	AWAY_PCT_COLUMN 	31
#define 	HOME_COLUMN			60
#define 	HOME_PCT_COLUMN 	66

	*---Display counters
	setcolor( c_fields )
	print( ANCHOR_LINE,   AWAY_COLUMN, str(  aTotYards[1], 4 ) )
	print( ANCHOR_LINE+1, AWAY_COLUMN, str( aRushYards[1], 4 ) )
	print( ANCHOR_LINE+2, AWAY_COLUMN, str( aPassYards[1], 4 ) )

	print( ANCHOR_LINE+4, AWAY_COLUMN, str(  aTotPlays[1], 4 ) )
	print( ANCHOR_LINE+5, AWAY_COLUMN, str( aRushPlays[1], 4 ) )
	print( ANCHOR_LINE+6, AWAY_COLUMN, str( aPassPlays[1], 4 ) )

	print( ANCHOR_LINE+8, AWAY_COLUMN, str(      aSacks[1], 4 ) )
	print( ANCHOR_LINE+9, AWAY_COLUMN, str( aIntercepts[1], 4 ) )

	print( ANCHOR_LINE,   HOME_COLUMN, str(  aTotYards[2], 4 ) )
	print( ANCHOR_LINE+1, HOME_COLUMN, str( aRushYards[2], 4 ) )
	print( ANCHOR_LINE+2, HOME_COLUMN, str( aPassYards[2], 4 ) )

	print( ANCHOR_LINE+4, HOME_COLUMN, str(  aTotPlays[2], 4 ) )
	print( ANCHOR_LINE+5, HOME_COLUMN, str( aRushPlays[2], 4 ) )
	print( ANCHOR_LINE+6, HOME_COLUMN, str( aPassPlays[2], 4 ) )

	print( ANCHOR_LINE+8, HOME_COLUMN, str(      aSacks[2], 4 ) )
	print( ANCHOR_LINE+9, HOME_COLUMN, str( aIntercepts[2], 4 ) )

	nPercent = percent( aRushYards[1], aTotYards[1] )
	print( ANCHOR_LINE+1, AWAY_PCT_COLUMN, str( nPercent, 4 ) )
	nPercent = percent( aPassYards[1], aTotYards[1] )
	print( ANCHOR_LINE+2, AWAY_PCT_COLUMN, str( nPercent, 4 ) )
	nPercent = percent( aTotPlays[1], ( aTotPlays[1] + aTotPlays[2] ) )
	print( ANCHOR_LINE+4, AWAY_PCT_COLUMN, str( nPercent, 4 ) )
	nPercent = percent( aRushPlays[1], aTotPlays[1] )
	print( ANCHOR_LINE+5, AWAY_PCT_COLUMN, str( nPercent, 4 ) )
	nPercent = percent( aPassPlays[1], aTotPlays[1] )
	print( ANCHOR_LINE+6, AWAY_PCT_COLUMN, str( nPercent, 4 ) )

	nPercent = percent( aRushYards[2], aTotYards[2] )
	print( ANCHOR_LINE+1, HOME_PCT_COLUMN, str( nPercent, 4 ) )
	nPercent = percent( aPassYards[2], aTotYards[2] )
	print( ANCHOR_LINE+2, HOME_PCT_COLUMN, str( nPercent, 4 ) )
	nPercent = percent( aTotPlays[2], ( aTotPlays[1] + aTotPlays[2] ) )
	print( ANCHOR_LINE+4, HOME_PCT_COLUMN, str( nPercent, 4 ) )
	nPercent = percent( aRushPlays[2], aTotPlays[2] )
	print( ANCHOR_LINE+5, HOME_PCT_COLUMN, str( nPercent, 4 ) )
	nPercent = percent( aPassPlays[2], aTotPlays[2] )
	print( ANCHOR_LINE+6, HOME_PCT_COLUMN, str( nPercent, 4 ) )

return( NIL )


function StatDesc

	parameters nCol

	print( ANCHOR_LINE,   nCol, 'Yards' )
	print( ANCHOR_LINE+1, nCol, 'Rushing' )
	print( ANCHOR_LINE+2, nCol, 'Passing' )

	print( ANCHOR_LINE+4, nCol, 'Plays' )
	print( ANCHOR_LINE+5, nCol, 'Rushing' )
	print( ANCHOR_LINE+6, nCol, 'Passing' )

	print( ANCHOR_LINE+8, nCol, 'Sacks by'  )
	print( ANCHOR_LINE+9, nCol, 'Intercepts' )

return( NIL )


*EOF:  STATSIN.PRE