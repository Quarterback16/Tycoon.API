*****************************************************************************
*  Program Name.....: NFLSTAND.PRG
*  Programmer.......: SC
*  System...........: Tycoon Fantasy Leagues Computer system
*  Module...........: NFL Module
*  Date.............: 14/4/1990 at 11:23
*  Copyright........: (c) 1990 by Tycoon Gridiron
*  Called by .......: NFLMENU
*  Purpose..........:
*
*	  To generate NFL Standings.
*
*............................................................................
*  Revision.........: 1.00 Last Revised: 14/4/1990 at 11:22
*  Description......: Initial Creation.
*.............................................................................
***************************** ALL RIGHTS RESERVED ***************************

#include "nfl.h"

#define  LINE_LENGTH 		96

PROCEDURE NFLStand

	private printto, workfile, cLeagueName
   private mopt[4], mmess[4], poption1
	private mHt1, mHt2, mHt3, mHt4, mHt5, mHt6
	private mAt1, mAt2, mAt3, mAt4, mAt5, mAt6

   clear_scn()

   head_line( "STANDINGS" )

   ScreenLine(  1 )
   ScreenLine( 21 )

   if !InitStand()    && Initialisation routine
      RETU
   endif

   poption1 = 2

   do while poption1 <> 0 
   
		@ 22,00
      clrmenu()

      poption1 = flatmenu( mopt, mmess, poption1, 22 )
      poption1 = if( poption1 = 0, 1, poption1)

      do case
         case mopt[poption1] = 'X' 

            poption1 = 0

         case mopt[poption1] = 'G'

				GenStand()

         case mopt[poption1] = 'V'

            view_prnt( printto, LINE_LENGTH )

         case mopt[poption1] = 'P'

				print_prnt( printto )

      endcase
   
   enddo 

   CloseDbf( 'TEAM' )
   CloseDbf( 'SCHED' )
   
RETURN


FUNCTION InitStand 

   std_rpt_opt( @mopt, @mmess, "NFL Standings" )
	select 0
	OpenDbf( 'SCHED', g_nfl_path )	
	select 0
	OpenDbf( 'TEAM', g_nfl_path )
	
RETURN( .t. )


FUNCTION ValStand

	parameters mvar

	local valonly, ok, saveclr, lk

	saveclr = setcolor()
	if lastkey() = UPARROW
		RETU( .t. )
	endif

	ok = .t.
	do case

	case mvar == "cSeason"

		ok = TEAM->( dbseek( cSeason ) )

	endcase
	setcolor( saveclr )

RETURN( ok )


FUNCTION GenStand

   private i, repoption, cSeason, cWeekFrom, cWeekTo, lPredict

	setcolor( c_desc )
	print( 6, 15, "Season  :" )
	print( 7, 15, "Week Fr :" )
	print( 8, 15, "Week To :" )
	print( 9, 15, "Predict :" )

	setcolor( c_fields )
	cSeason   = G_season
	cWeekFrom = "01"
	cWeekTo   = G_week
   lPredict  = .f.

	vget( 6, 25, "cSeason" ,   "XXXX", .f. )
	vget( 7, 25, "cWeekFrom" , "XX",   .f. )
	vget( 8, 25, "cWeekTo",    "XX",   .f. )
	vget( 9, 25, "lPredict",   "Y",    .f. )

	vread( 1, .f., 'ValStand' )

	CLEAR_WINDOW( 6, 15, 9, 79 )
	if abort()
		RETU
	else
		G_Week   = cWeekTo
		printto  = g_reppath + TEMPORARY_FILE_INDICATOR + ;
		           "STD" + substr( cSeason, 3, 2 ) + cWeekTo + ;
							  		   PRINTFILE_EXT

	endif

	GenMsg()
	***
	***   Load Data
	***
	cRound = cSeason + cWeekFrom + cWeekTo

	nTeams = NFLTeams( cSeason )
	nGames = int( ( nTeams / 2 ) +.5 )

	private franchise_[ nTeams ], division_[ nTeams ], wins_[ nTeams ]
	private losses_[ nTeams ], ptsfor_[ nTeams ], against_[ nTeams ]
	private ties_[ nTeams ], aTeamId[ nTeams ]

	fran_cnt = 0

	select TEAM
	dbseek( cSeason )
	do while .not. eof() .and. SEASON = cSeason
		INC( fran_cnt )
		franchise_[ fran_cnt ] = trim( CITY ) + " " + trim( TEAMNAME )
		division_[  fran_cnt ] = DIVISION	
		aTeamId[    fran_cnt ] = TEAMID
		wins_[      fran_cnt ] = 0
		losses_[    fran_cnt ] = 0
		ties_[      fran_cnt ] = 0
		ptsfor_[    fran_cnt ] = 0
		against_[   fran_cnt ] = 0
		skip
	enddo

	select SCHED
	for w = val( cWeekFrom ) to val( cWeekTo )
		dbseek( cSeason + strzero( w, 2 ) )

		RecordWins( strzero( w, 2 ), lPredict )

	next

	*---Store totals
	StoreTeams()

	select TEAM
	set order to 2
	private    pcol1, pcol2, pcol3, pcol4, pcol5, pcol6, pcol7, pcol8
	store 0 to pcol1, pcol2, pcol3, pcol4, pcol5, pcol6, pcol7, pcol8

   begin sequence

      set print to ( printto )
		set print on

		print_setup( LINE_LENGTH )

		*?? chr(27) +"E" + chr(27)+ chr(38) + "I" +chr(27) + "(0U" + chr(27) + "(s0p12h12v0s0b8T" + chr(27) + "&a12L"
		p_linespp = 66

      print_margin( 0, 0, 0 )

      prntcoldec( 1 )

      *---set up the printing columns
		prntcolnxt( @pcol1,  25 )
		prntcolnxt( @pcol2,  10 )
		prntcolnxt( @pcol3,   6 )
		prntcolnxt( @pcol4,   6 )
		prntcolnxt( @pcol5,  10 )
		prntcolnxt( @pcol6,   8 )
		prntcolnxt( @pcol7,  10 )
		prntcolnxt( @pcol8,  10 )

		printat( pcol1, cSeason + " - NFL Standings - Week " + cWeekFrom + ;
								" to " + cWeekTo )
		println()
		println()

		printat( pcol1, "Franchise" )
		printat( pcol3, "W"      )
		printat( pcol4, "L"      )
		printat( pcol5, "T"      )
		printat( pcol6, "Clip"   )
		printat( pcol7, "For"    )
		printat( pcol8, "Agin"   )
		println()

		select TEAM
		dbseek( cSeason )
		lastdiv = "X"
		do while .not. TEAM->( eof() ) .and. TEAM->SEASON = cSeason
			if TEAM->DIVISION <> lastdiv
				println()
				lastdiv = TEAM->DIVISION
			endif

			printat( pcol1, trim( TEAM->CITY ) + " "  + TEAM->TEAMNAME )
			printat( pcol2, TEAM->RATE + "-" + RateToPts( TEAM->RATE ) )
			printat( pcol3, TEAM->WINS      )
			printat( pcol4, TEAM->LOSSES    )
			printat( pcol5, TEAM->TIES      )
			printat( pcol6, TEAM->CLIP      )
			printat( pcol7, TEAM->PTSFOR    )
			printat( pcol8, TEAM->AGAINST   )

  			println()

			skip
		enddo

		print_page()
		
	end

   print_release()
   set print to        
   copy file (printto) to ( g_reppath + "CurrStnds" + PRINTFILE_EXT )
	@ 22,00
	clrmenu()
	GenMsgClr()
	select TEAM
	set order to 1

RETURN


FUNCTION RecordWins( cWeek, lPredict )

	LOCAL nHS, nAS
	*---Talley results for the week

	do while .not. SCHED->( eof() ) .and. SCHED->WEEK = cWeek
		*---resolve game
		nHT = ascan( aTeamId, SCHED->HOMETEAM )
		nAT = ascan( aTeamId, SCHED->AWAYTEAM )
		if .not. empty( nHT ) .and. .not. empty( nAT )
			ADDTO( ptsfor_[  nAT ], SCHED->AWAYSCORE )
			ADDTO( ptsfor_[  nHT ], SCHED->HOMESCORE )
			ADDTO( against_[ nAT ], SCHED->HOMESCORE )
			ADDTO( against_[ nHT ], SCHED->AWAYSCORE )

         if lPredict
			   nHS = PredictScore( SCHED->HOMETEAM, 7 )
			   nAs = PredictScore( SCHED->AWAYTEAM, 0 )
            if nHS = nAS
               nHS = nHS + 1
            endif

         else
			   nHS = SCHED->HOMESCORE
			   nAs = SCHED->AWAYSCORE
         endif

			if nHs > nAS
				*---Home Win

				INC( wins_[   nHT ] )
				INC( losses_[ nAT ] )

			elseif nHS = nAS
				*---Tie
				INC( ties_[ nAT ] )
				INC( ties_[ nHT ]	)
			else
				*---Away win
				INC( wins_[   nAT ] )
				INC( losses_[ nHT ] )
			endif
		endif

      if lPredict
         replace SCHED->AWAYSCORE with nAS
         replace SCHED->HOMESCORE with nHS
      endif

		SCHED->( dbskip() )
	enddo

RETURN 


static;
FUNCTION PredictScore

   parameters cTeam, nBonus

   local nScore := 0
	if TEAM->( dbseek( cSeason + cTeam ) )
      nScore := val( RateToPts( TEAM->RATE ) ) + nBonus
   endif

RETURN nScore


STATIC FUNCTION StoreTeams

	local t := 0

	for t = 1 to fran_cnt

		if TEAM->( dbseek( cSeason + aTeamId[ t ] ) )
			do while .not. TEAM->( rec_lock( 5 ) )
				Error_Msg( "StoreTeams : Record lock on TEAM failed" )
			enddo

			replace TEAM->WINS    with wins_[      t ]
			replace TEAM->LOSSES  with losses_[    t ]
			replace TEAM->TIES    with ties_[      t ]
			replace TEAM->CLIP    with Clip( wins_[ t ], losses_[ t ], ties_[ t ] )
			replace TEAM->PTSFOR  with ptsfor_[    t ]
			replace TEAM->AGAINST with against_[   t ]
		endif
	next

RETURN( nil )



*EOF:  NFLSTAND.PRG
