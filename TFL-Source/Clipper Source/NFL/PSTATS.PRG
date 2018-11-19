*****************************************************************************
*  Program Name.....: pstats.pre 
*  Programmer.......: Steve Colonna 
*  System...........:
*  Module...........:
*  Date.............: 3/10/1990 at 15:55
*  Copyright........: (c) 1990 by Knoware 
*  Called From......:
*  Purpose..........:
*
*............................................................................
*  Revision.........: 1.0 Last Revised: 3/10/1990 at 15:55
*  Description......: Initial Creation.
*............................................................................
***************************** ALL RIGHTS RESERVED ***************************

#INCLUDE "nfl.h"

PROCEDURE PStats

	*---Which season
	mSeason   = getalph( 'Which season', 4, 17, G_season )
	mLeagueId = getalph( 'Which league', 4, 17, G_league )

	*---Make a temporary workfile
	workfile = g_nfl_path + TEMPORARY_FILE_INDICATOR + "PSTAT"
	if .not. file( workfile + ".dbf")
		makefile( workfile + ".dbf", 7, ;
						"WEEK       C 24 0", ;
						"TDS        N  2 0", ;
						"PTS        N  3 0", ;
						"STAT1      N  6 1", ;
						"STAT2      N  6 1", ;
						"STAT3      N  6 1", ;
						"STAT4      N  6 1"  )
	endif

	select 0
	if net_use( workfile, .t., 5, "WORKFILE" )
		zap
	else
		error_msg( "Work file is in use, try again later")
		retu
	endif

	private mfields[ 8 ], mcols[ 8 ]
	private nTotStat1, nTotStat2, nTotStat3, nTotStat4, nTotPts      

	select STAT
	set order to 2

	mPlayerid = PLAYER->PLAYERID

	do case
		case PLAYER->CATEGORY == QUARTERBACK_CAT

			for w = 1 to iif( mSeason = "1993", 18, 17 )

				mWeek = strzero( w, 2 )
*				TestMsg( 'Week ' + mWeek )
				cGameKey = mSeason + mWeek
				store 0 to nStat1, nStat2, nStat3, nStat4
				nPts  = TotalPts( mPlayerid, mSeason, mWeek, mLeagueID )

				select STAT
				dbseek( mPlayerId + PASS_COMPLETIONS + cGameKey )
				if found()
					ADDTO( nStat1, QTY )
				endif
				dbseek( mPlayerId + PASSING_YARDAGE + cGameKey )
				if found()
					ADDTO( nStat3, QTY )
				endif
				dbseek( mPlayerId + PASSING_ATTEMPTS + cGameKey )
				if found()
					ADDTO( nStat2, QTY )
				endif
				dbseek( mPlayerId + PASSES_INTERCEPTED + cGameKey )
				if found()
					ADDTO( nStat4, QTY )
				endif

				select WORKFILE
				do ADDRECIT with "Workfile"
				replace WEEK  with mWeek + ' ' +;
										 Opponent( mPlayerId, mWeek, mSeason )
				replace STAT1 with nStat1
				replace STAT2 with nStat2
				replace STAT3 with nStat3
				replace STAT4 with nStat4
*				replace TDS	  with nTds
				replace PTS	  with nPts
			next

*			private mfields[ 7 ], mcols[ 7 ]
			asize( mFields, 7 )
			asize( mCols,   7 )

			mfields[ 1 ] = "WEEK"
			mfields[ 2 ] = "str(STAT1,4,0)"
			mfields[ 3 ] = "str(STAT2,4,0)"
			mfields[ 4 ] = "str(STAT3,5,0)"
			mfields[ 5 ] = "str(STAT4,4,0)"
			mfields[ 6 ] = "ComplPercent()"
			mfields[ 7 ] = "PTS"
			mcols[   1 ] = "Week"
			mcols[   2 ] = "Comp"
			mcols[   3 ] = "Att"
			mcols[   4 ] = "Yds"
			mcols[   5 ] = "Int"
			mcols[   6 ] = "Com %"
			mcols[   7 ] = "Pts"
			setcolor( C_help )
			popbox( 7, 20, 20, 78 )
			lineinbox( 18 )
			inCentre( 20, 78, trim(PLAYER->FIRSTNAME) + " " + trim( PLAYER->SURNAME ) + " " + mSeason, 7 )

			*---Draw Total line
			nTotStat1    = totalStat( "STAT1" )
			nTotStat2    =	totalStat( "STAT2" )
			nTotStat3    = totalStat( "STAT3" )
			nTotStat4    =	totalStat( "STAT4" )
			nTotPts      =	totalStat( "PTS" )
			@ 19, 30 say 'TOTALS' + space( len( WORKFILE->WEEK ) - 6 ) 
			@ 19, 47 say str( nTotStat1, 4, 0 )
			@ 19, 52 say str( nTotStat2, 4, 0 )
			@ 19, 58 say str( nTotStat3, 4, 0 )
			@ 19, 63 say str( nTotStat4, 4, 0 )
			if nTotStat2 > 0
				@ 19, 68 say str( ( nTotStat1 / nTotStat2 ) * 100, 5, 1 )
			endif
			@ 19, 73 say str( nTotPts, 4, 0 )

			go top
			dbedit( 8, 21, 17, 77,mfields,'','@X',mcols,"Ä"," ")

			unpop()

		case PLAYER->CATEGORY == RUNNINGBACK_CAT .or.;
			  PLAYER->CATEGORY == RECEIVER_CAT

			for w = 1 to iif( mSeason = "1993", 18, 17 )

				mWeek = strzero( w, 2 )
				cGameKey = mSeason + mWeek
				store 0 to nStat1, nStat2, nStat3, nStat4
				nPts  = TotalPts( mPlayerid, mSeason, mWeek, mLeagueID )

				select STAT
				dbseek( mPlayerId + RUSHING_CARRIES + cGameKey )
				if found()
					ADDTO( nStat1, QTY )
				endif
				dbseek( mPlayerId + RUSHING_YARDS + cGameKey )
				if found()
					ADDTO( nStat2, QTY )
				endif
				dbseek( mPlayerId + PASSES_CAUGHT + cGameKey )
				if found()
					ADDTO( nStat3, QTY )
				endif
				dbseek( mPlayerId + RECEPTION_YARDAGE + cGameKey )
				if found()
					ADDTO( nStat4, QTY )
				endif

				select WORKFILE
				do ADDRECIT with "Workfile"
				replace WEEK  with mWeek + ' ' +;
										 Opponent( mPlayerId, mWeek, mSeason )
				replace STAT1 with nStat1
				replace STAT2 with nStat2
				replace STAT3 with nStat3
				replace STAT4 with nStat4
*				replace TDS	  with nTds
				replace PTS	  with nPts
			next

*			private mfields[ 8 ], mcols[ 8 ]
			mfields[ 1 ] = "WEEK"
			mfields[ 2 ] = "str(STAT1,4,0)"
			mfields[ 3 ] = "str(STAT2,4,0)"
			mfields[ 4 ] = "AvgPerCarry()"
			mfields[ 5 ] = "str(STAT3,5,0)"
			mfields[ 6 ] = "str(STAT4,4,0)"
			mfields[ 7 ] = "AvgPerCatch()"
			mfields[ 8 ] = "PTS"
			mcols[   1 ] = "Week"
			mcols[   2 ] = "Runs"
			mcols[   3 ] = "Yds"
			mcols[   4 ] = "Avg"
			mcols[   5 ] = "Rec"
			mcols[   6 ] = "Yds"
			mcols[   7 ] = "Avg"
			mcols[   8 ] = "Pts"
			setcolor( C_help )
			popbox( 7,18,20,78 )
			lineinbox( 18 )
			inCentre( 19, 78, trim(PLAYER->FIRSTNAME) + " " + trim( PLAYER->SURNAME ) + " " + mSeason, 7 )

			*---Draw Total line
			nTotStat1    = totalStat( "STAT1" )
			nTotStat2    =	totalStat( "STAT2" )
			nTotStat3    = totalStat( "STAT3" )
			nTotStat4    =	totalStat( "STAT4" )
			nTotPts      =	totalStat( "PTS" )
			@ 19, 30 say 'TOTALS'
			@ 19, 44 say str( nTotStat1, 4, 0 )
			@ 19, 49 say str( nTotStat2, 4, 0 )
			if nTotStat1 > 0
				@ 19, 54 say str( ( nTotStat2 / nTotStat1 ) , 4, 1 )
			endif
			@ 19, 60 say str( nTotStat3, 4, 0 )
			@ 19, 65 say str( nTotStat4, 4, 0 )
			if nTotStat3 > 0
				@ 19, 70 say str( ( nTotStat4 / nTotStat3 ) , 4, 1 )
			endif
			@ 19, 74 say str( nTotPts, 4, 0 )

			go top
			dbedit(8,19,17,77,mfields,'','@X',mcols,"Ä"," ")

			unpop()

		case PLAYER->CATEGORY $ DEFENSIVE_CATEGORIES

			for w = 1 to iif( mSeason = "1993", 18, 17 )

				mWeek = strzero( w, 2 )
				cGameKey = mSeason + mWeek
				store 0 to nStat1, nStat2, nStat3, nStat4
				nPts  = TotalPts( mPlayerid, mSeason, mWeek, mLeagueID )

				select STAT
				dbseek( mPlayerId + QUARTERBACK_SACKS + cGameKey )
				if found()
					ADDTO( nStat1, QTY )
				endif
				dbseek( mPlayerId + INTERCEPTIONS_MADE + cGameKey )
				if found()
					ADDTO( nStat2, QTY )
				endif

				select WORKFILE
				do ADDRECIT with "Workfile"
				replace WEEK  with mWeek + ' ' +;
										 Opponent( mPlayerId, mWeek, mSeason )
				replace STAT1 with nStat1
				replace STAT2 with nStat2
				replace STAT3 with nStat3
				replace STAT4 with nStat4
*				replace TDS	  with nTds
				replace PTS	  with nPts
			next

*			private mfields[ 4 ], mcols[ 4 ]
			asize( mFields, 4 )
			asize( mCols,   4 )
			mfields[ 1 ] = "WEEK"
			mfields[ 2 ] = "str(STAT1,4,1)"
			mfields[ 3 ] = "str(STAT2,4,0)"
			mfields[ 4 ] = "PTS"
			mcols[   1 ] = "Week"
			mcols[   2 ] = "  Sacks  "
			mcols[   3 ] = "  Ints  "
			mcols[   4 ] = " Pts  "
			setcolor( C_help )
			popbox( 7,26,20,78 )
			lineinbox( 18 )
			inCentre( 26, 78, trim(PLAYER->FIRSTNAME) + " " + trim( PLAYER->SURNAME ) + " " + mSeason, 7 )

			*---Draw Total line
			nTotStat1    = totalStat( "STAT1" )
			nTotStat2    =	totalStat( "STAT2" )
			nTotPts      =	totalStat( "PTS" )
			@ 19, 30 say 'TOTALS'
			@ 19, 52 say str( nTotStat1, 4, 0 )
			@ 19, 62 say str( nTotStat2, 4, 0 )
			@ 19, 70 say str( nTotPts, 4, 0 )

			go top
			dbedit(8,27,17,77,mfields,'','@X',mcols,"Ä"," ")

			unpop()


		case PLAYER->CATEGORY == KICKER_CAT

			for w = 1 to iif( mSeason = "1993", 18, 17 )

				mWeek = strzero( w, 2 )
				cGameKey = mSeason + mWeek
				store 0 to nStat1, nStat2, nStat3, nStat4
				nPts  = TotalPts( mPlayerid, mSeason, mWeek, mLeagueID )

				select SCORE
				set order to 2
				dbseek( mPlayerId + cGameKey + SCORE_FIELD_GOAL )
				do while .not. eof() .and. SCORE->PLAYERID1 = mPlayerId .and.;
													SCORE->SEASON    = mSeason   .and.;
													SCORE->WEEK      = mWeek     .and.;
													SCORE->SCORE     = SCORE_FIELD_GOAL
					INC( nStat1 )
					skip
				enddo

				dbseek( mPlayerId + cGameKey + SCORE_PAT )
				do while .not. eof() .and. SCORE->PLAYERID1 = mPlayerId .and.;
													SCORE->SEASON    = mSeason   .and.;
													SCORE->WEEK      = mWeek     .and.;
													SCORE->SCORE     = SCORE_PAT
					INC( nStat2 )
					skip
				enddo

				select WORKFILE
				do ADDRECIT with "Workfile"
				replace WEEK  with mWeek + ' ' +;
										 Opponent( mPlayerId, mWeek, mSeason )
				replace STAT1 with nStat1
				replace STAT2 with nStat2
				replace STAT3 with nStat3
				replace STAT4 with nStat4
*				replace TDS	  with nTds
				replace PTS	  with nPts
			next

*			private mfields[ 4 ], mcols[ 4 ]
			asize( mFields, 4 )
			asize( mCols,   4 )
			mfields[ 1 ] = "WEEK"
			mfields[ 2 ] = "str(STAT1,4,0)"
			mfields[ 3 ] = "str(STAT2,4,0)"
			mfields[ 4 ] = "PTS"
			mcols[   1 ] = "Week"
			mcols[   2 ] = "   FGs   "
			mcols[   3 ] = "  PATs  "
			mcols[   4 ] = " Pts  "
			setcolor( C_help )
			popbox( 7,26,20,78 )
			lineinbox( 18 )
			inCentre( 26, 78, trim(PLAYER->FIRSTNAME) + " " + trim( PLAYER->SURNAME ) + " " + mSeason, 7 )

			*---Draw Total line
			nTotStat1    = totalStat( "STAT1" )
			nTotStat2    =	totalStat( "STAT2" )
			nTotPts      =	totalStat( "PTS" )
			@ 19, 30 say 'TOTALS'
			@ 19, 52 say str( nTotStat1, 4, 0 )
			@ 19, 62 say str( nTotStat2, 4, 0 )
			@ 19, 70 say str( nTotPts, 4, 0 )

			go top
			dbedit(8,27,17,77,mfields,'','@X',mcols,"Ä"," ")

			unpop()

	endcase


	select WORKFILE
	use

RETURN 


function AvgPerCatch

	LOCAL cRetVal

	cRetVal = "    "

	if STAT3 > 1
		cRetVal = str( (STAT4 / STAT3), 4, 1 )
	endif

return ( cRetVal )



*EOF:  PSTATS.PRE