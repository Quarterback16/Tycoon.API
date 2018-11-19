*****************************************************************************
*  Program Name.....: NFLRANK.PRG
*  Programmer.......: SC
*  System...........: Tycoon Fantasy Leagues Computer system
*  Module...........: NFL Module
*  Date.............: 14/4/1990 at 11:23
*  Copyright........: (c) 1990 by Tycoon Gridiron
*  Called by .......: NFLMENU
*  Purpose..........:
*
*	  To generate NFL Team Rankings.
*
*		Uses routines in SCHED.PRG
*
*
*............................................................................
*  Revision.........: 1.00 Last Revised: 14/4/1990 at 11:22
*  Description......: Initial Creation.
*.............................................................................
***************************** ALL RIGHTS RESERVED ***************************

#include "..\lib\print.h"
#include "nfl.h"

#define  LINE_LENGTH 		96

PROCEDURE NFLRank

	private printto, workfile, cLeagueName
   private mopt[4], mmess[4], poption1

   clear_scn()

   head_line( "RANKINGS" )

   ScreenLine(  1 )
   ScreenLine( 21 )

   if !InitRank()    && Initialisation routine
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

				GenRank()

         case mopt[poption1] = 'V'

            view_prnt( printto, LINE_LENGTH )

         case mopt[poption1] = 'P'

				print_prnt( printto )

      endcase
   
   enddo 

RETURN


FUNCTION InitRank 

   	select 0
	OpenDbf( 'PLAYER', g_nfl_path, .t. )	
	select 0
	OpenDbf( 'SCHED', g_nfl_path, .t. )
	select 0
	OpenDbf( 'TEAM', g_nfl_path, .t. )	
	select 0
	OpenDbf( 'STAT', g_nfl_path, .t. )
	select 0
	OpenDbf( 'SCORE', g_nfl_path, .t. )
	select 0
	OpenDbf( 'SERVE', g_nfl_path, .t. )
	
   std_rpt_opt( @mopt, @mmess, "NFL Rankings" )

RETURN( .t. )


FUNCTION ValRank

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


FUNCTION GenRank

   private i, repoption, cSeason, cWeekFrom, cWeekTo, nAvgScore

	setcolor( c_desc )
	print( 6, 15, "Season  :" )
	print( 7, 15, "Week Fr :" )
	print( 8, 15, "Week To :" )

	setcolor( c_fields )
	cSeason   = G_season
	cWeekFrom = "01"
	cWeekTo   = G_week

	vget( 6, 25, "cSeason" ,   "XXXX", .f. )
	vget( 7, 25, "cWeekFrom" , "XX",   .f. )
	vget( 8, 25, "cWeekTo",    "XX",   .f. )

	vread( 1, .f., 'ValRank' )

	scroll( 6, 15, 8, 79, 0 )
	if abort()
		RETU
	else
		G_Week   = cWeekTo
		printto  = g_reppath + TEMPORARY_FILE_INDICATOR + "R" + cSeason + ;
							  		   cWeekTo + PRINTFILE_EXT
		testmsg( printto )
	endif

	GenMsg()

	ClearRanks( cSeason )

	nAvgScore := AvgScore( cSeason, cWeekFrom, cWeekTo )

	select SCHED
	for w = val( cWeekFrom ) to val( cWeekTo )
		dbseek( cSeason + strzero( w, 2 ) )

		RecordRanks( strzero( w, 2 ), cSeason, nAvgScore )

	next

	select TEAM
	set order to 3
	private    pcol1, pcol2, pcol3, pcol4, pcol5, pcol6, pcol7, pcol8
	store 0 to pcol1, pcol2, pcol3, pcol4, pcol5, pcol6, pcol7, pcol8

   begin sequence

      set print to ( printto )
		set print on

		print_setup( LINE_LENGTH )

		*?? chr(27) +"E" + chr(27)+ chr(38) + "I" +chr(27) + "(0U" + chr(27) + "(s0p12h12v0s0b8T" + chr(27) + "&a12L"
		p_linespp = 999

      print_margin( 0, 0, 0 )

      prntcoldec( 1 )

      *---set up the printing columns
		prntcolnxt( @pcol1,  25 )
		prntcolnxt( @pcol2,  10 )
		prntcolnxt( @pcol3,  10 )
		prntcolnxt( @pcol4,  10 )
		prntcolnxt( @pcol5,  10 )

		printat( pcol1, cSeason + " - NFL Rankings - Week " + cWeekFrom + ;
								" to " + cWeekTo )
		println()
		println()

		printat( pcol1, "Franchise"  )
		printat( pcol2, "Offense"    )
		printat( pcol3, "Defense"    )
		printat( pcol4, "     Total" )
		printat( pcol5, "Ratings"    )
		println()

		select TEAM
		dbseek( cSeason )
		lastdiv = "X"
		do while .not. TEAM->( eof() ) .and. TEAM->SEASON = cSeason

			printat( pcol1, trim( TEAM->CITY ) + " "  + TEAM->TEAMNAME )
			printat( pcol2, TEAM->OFFENSE    )
			printat( pcol3, TEAM->DEFENSE   )
			printat( pcol4, TEAM->OFFENSE - TEAM->DEFENSE )
			printat( pcol5, TEAM->RATE + "-" + RateToPts( TEAM->RATE ) )

  			println()

			skip
		enddo

		print_page()
		
	end

   print_release()
   set print to        

	@ 22,00
	clrmenu()
	GenMsgClr()
	select TEAM
	set order to 1

RETURN


FUNCTION RecordRanks( cWeek, mSeason, nAvgScore )

	*---Talley results for the week
	SCHED->( dbseek( mSeason + cWeek ) )
	do while .not. SCHED->( eof() ) .and. SCHED->WEEK = cWeek

		RateGame( nAvgScore, mSeason, val(G_week) - val(cWeek) )

		SCHED->( dbskip() )
	enddo

RETURN



*EOF:  NFLRank.PRG