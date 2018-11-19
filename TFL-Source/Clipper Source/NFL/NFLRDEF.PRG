*****************************************************************************
*  Program Name.....: NFLRDEF.PRG
*  Programmer.......: Steve Colonna 
*  System...........: TFL
*  Module...........: TYCOON
*  Date.............:  6/8/1997 at 21:18
*  Copyright........: (c) 1997 by Knoware
*  Called From......: TYMENU
*  Purpose..........:
*
*		To Print NFL defensive team stats
*
*     Team			  Pts	  TDr	  TDp	  Sak	  TDs	  Saf	  ShO		<7	  <14   <18
*     XXXXXXXXXX   9999	  999   999   999   999   999   999   999   999	  999
*
*
*			Structure for database : TEAM.dbf
*			Field  Field Name  Type       Width    Dec
*			    1  TEAMID      Character      2
*			    2  TEAMNAME    Character     15
*			    3  CITY        Character     15
*............................................................................
*  Revision.........: 1.0 Last Revised: 8/8/1997 at 21:18
*  Description......: Initial Creation.
*............................................................................
***************************** ALL RIGHTS RESERVED ***************************

#INCLUDE "..\lib\PRINT.H"
#INCLUDE "NFL.H"

*---Constants

#define  LINE_LENGTH 		176

static nSacks   := 0
static nTDs     := 0
static nSafetys := 0

PROCEDURE  NflRDef

	private printto, workfile, cLeagueName, tixfile, nComps, mLeagueId
	private mopt[4], mmess[4], poption1, cCat, lAlpha
	private cTeam,   cSeason1, cWeek1,   cSeason2, cWeek2, dRoster

   clear_scn()

   head_line( "TEAM DEFENSIVE STATS" )

   ScreenLine(  1 )
   ScreenLine( 21 )

   if .not. InitTDef()
      CloseTDef()
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

				GenTdef()
            view_prnt( printto, LINE_LENGTH )

         case mopt[poption1] = 'V'

            view_prnt( printto, LINE_LENGTH )

         case mopt[poption1] = 'P'

				print_prnt( printto )

      endcase
   
   enddo 

   CloseTdef()

RETURN


FUNCTION GenTdef

   local cLastWeek, nOldStatOrd

	setcolor( c_desc )
	print( 5, 10, "Team      |" )
	print( 6, 10, "Week Range:" )
	print( 10, 15, "Leave Team blank for all teams" )

	setcolor( c_fields )

	vget( 5, 25, "cTeam",      "!!",   .f. )
	vget( 6, 25, "cSeason1",   "XXXX", .f. )
	vget( 6, 30, "cWeek1",     "XX",   .f. )
	vget( 6, 40, "cSeason2",   "XXXX", .f. )
	vget( 6, 45, "cWeek2",     "XX",   .f. )

	vread( 1, .f., 'ValTdef' )

	CLEAR_WINDOW( 4, 5, 19, 79 )
	if abort()
		RETU
	endif

	GenMsg()

	cFirstWeek := cSeason1 + cWeek1
	cLastWeek  := cSeason2 + cWeek2

	select WORKFILE
	zap

	if empty( cTeam )
		*---Print all teams
		if TEAM->( dbseek( cSeason2 ) )
			do while .not. TEAM->( eof() ) .and. TEAM->SEASON = cSeason2
				ProcessTeam( TEAM->TEAMID, cFirstWeek, cLastWeek )
				TEAM->( dbskip() )
			enddo
		endif

	else
		ProcessTeam( cTeam, cFirstWeek, cLastWeek )
	endif

RETURN nil


static ;
FUNCTION ProcessTeam( cTeam, cFirstWeek, cLastWeek  )

   local nAgin

	printto = g_reppath + TEMPORARY_FILE_INDICATOR + "DEF" + cTeam + ;
								 substr( cSeason2, 3, 2 ) + PRINTFILE_EXT

	ZeroStats()

	if .not. SCHED->( dbSeek( cFirstWeek ) )
		ErrorMsg( cFirstWeek + " not found" )
	endif
	do while .not. SCHED->( eof() ) ;
				.and. SCHED->SEASON <= cSeason2 .and. SCHED->WEEK <= cWeek2
		*---For all games in the range
		if SCHED->AWAYTEAM = cTeam .or. SCHED->HOMETEAM = cTeam
			nAgin  = CaptureGame( cTeam )
			ScanStats( SCHED->SEASON, SCHED->WEEK, SCHED->GAMENO, cTeam )

         *---Store the data
			WriteDef( cTeam, nAgin, SCHED->SEASON, SCHED->WEEK )
		endif

		@ 24, 10 say str( SCHED->( recno() ), 5 )
		SCHED->( dbSkip() )
	enddo

	PrintTeam( cTeam )

return .t.


static;
FUNCTION	PrintTeam( cTeam )

	private    pcol1, pcol2, pcol3, pcol4, pcol5, pcol6, i
	store 0 to pcol1, pcol2, pcol3, pcol4, pcol5, pcol6, i

   begin sequence

      set print to ( printto )
		set print on

		print_setup( LINE_LENGTH, .t. )	//	Continuous

		?? chr(27) +"E"
		p_linespp = 70

      print_margin( 5, 3, 3 )

      prntcoldec( 1 )

      *---set up the printing columns
		prntcolnxt( @pcol1,  10 )
		prntcolnxt( @pcol2,  10 )
		prntcolnxt( @pcol3,  10 )
		prntcolnxt( @pcol4,  10 )
		prntcolnxt( @pcol5,  10 )
		prntcolnxt( @pcol6,  10 )

		PrintStats()

		println()
		println()
      println( 0, "** End of Report **" ) 
      print_page()
		
	end

   print_release()
   set print to        

	@ 22,00
	clrmenu()
	GenMsgClr()

RETURN


static FUNCTION InitTdef

	private mcontinu

	cSeason1  := G_Season
	cSeason2  := G_Season
	cTeam     := "  "
	cWeek1    := "01"
	cWeek2    := G_week

	plswait()
   std_rpt_opt( @mopt, @mmess, "Team Defence Stats" )

   mcontinu = .t.

	@ 23,00

	if mcontinu
		*---Open temp file
		mcontinu = OpenDef()
	endif

	if ! mcontinu
		error_msg( "Unable to gain access to all the necessary files" )
	endif

RETURN mcontinu


FUNCTION OpenDef

	workfile = g_ty_path + TEMPORARY_FILE_INDICATOR + "DEF"
	if .not. file( workfile + ".dbf")
		makefile( workfile + ".dbf",  7, ;
						"TEAMID     C  2 0", ;
						"SACKS      N  3 0", ;
						"TDS        N  3 0", ;
						"SAFETYS    N  3 0", ;
						"AGIN       N  3 0", ;
						"SEASON     C  4 0", ;
						"WEEK       C  2 0"  )
	endif

	select 0
	if !net_use( workfile, .t., 5, "WORKFILE" )
		error_msg( "Work file is in use, try again later")
		retu ( .f. )
   else
*     TestMsg( workfile + " created" )
	endif
	tixfile = workfile + '.ntx'
	if .not. file( tixfile)
		index on TEAMID+SEASON+WEEK to ( tixfile  )

	endif
	set index to ( tixfile )

	select PLAYER
	set order to 3

	select STAT
	set order to 2

RETURN .t.


FUNCTION WriteDef( cTeam, nAgin, cSeason, cWeek )

	select WORKFILE
	append blank
	replace WORKFILE->TEAMID		with cTeam
	replace WORKFILE->SACKS 		with nSacks
	replace WORKFILE->TDS   		with nTDs
	replace WORKFILE->SAFETYS	  	with nSafetys
	replace WORKFILE->AGIN			with nAgin
	replace WORKFILE->SEASON 		with cSeason
	replace WORKFILE->WEEK   		with cWeek

RETURN nil


FUNCTION CloseTdef

*	close databases

RETURN( .t. )


FUNCTION ValTdef

	parameters mvar

	local valonly, ok, saveclr, lk

	saveclr = setcolor()
	if lastkey() = UPARROW
		RETU( .t. )
	endif

	ok = .t.
	do case
	case mvar = "cSeason1"

		if .not. empty( cTeam )
			if .not. TEAM->( dbseek( cSeason1 + cTeam ) )
				ok = .f.
				Error_Msg( "Invalid Team season combo" )
			endif
		endif

	case mvar = "cWeek2"
		*---Set Roster date
		if .not. empty( cTeam )
			dRoster := Dateof( cSeason2 + cWeek2, cTeam )
		endif

	endcase
	setcolor( saveclr )

RETURN( ok )


static FUNCTION PrintStats( cTeam )

   local cLastTeam

	cLastTeam = "XX"

	select WORKFILE
   go top

	do while .not. WORKFILE->( eof() )
		if WORKFILE->TEAMID = cTeam .or. empty( cTeam )
			if WORKFILE->TEAMID <> cLastTeam
            *---Totals
				println()
				printAt( pcol1, TeamStr( WORKFILE->TEAMID ) )
				println()
            *---Heading
	         PrintAt( pcol1, "When"  )
	         PrintAt( pcol2, "F Pts" )
	         PrintAt( pcol3, "Against"  )
	         PrintAt( pcol4, "Sacks" )
	         PrintAt( pcol5, "TDs"   )
	         PrintAt( pcol6, "Safetys"  )
	         println()
	         println()
				cLastTeam = WORKFILE->TEAMID
			endif

			PrintAt( pcol1, WORKFILE->SEASON + ":" + WORKFILE->WEEK )
			PrintAt( pcol2, DefFpts( WORKFILE->AGIN,     ;
			                         WORKFILE->SACKS,		;
											 WORKFILE->TDS,		;
											 WORKFILE->SAFETYS ) )
			PrintAt( pcol3, WORKFILE->AGIN    )
			PrintAt( pcol4, WORKFILE->SACKS   )
			PrintAt( pcol5, WORKFILE->TDS     )
			PrintAt( pcol6, WORKFILE->SAFETYS )

			println()

		endif
		WORKFILE->( dbskip() )
	enddo
   

return nil


/*
        Name :	 DefFPts
        Does :	 Calculates the Fantasy points (ESPN method) for a defense
  Parameters :  Points given up, sacks made, TDs scored, Safetys
     Returns :  nPoints
       Notes :

      Author : LL
Date Created : 14 Oct 97
Time Created :  9:58 pm
	Last change:  LL   14 Oct 97   10:34 pm
*/
static FUNCTION DefFPts( nAgin, nSacks, nTDS, nSafetys )

   local nPoints := 0

	*---Points given up
   do case
      case nAgin < 1
         *---Shutout
         nPoints := 10
      case nAgin < 7
         nPoints := 7
      case nAgin < 14
         nPoints := 4
      case nAgin < 18
         nPoints := 1
   endcase

   *---Add any sacks made
   nPoints += nSacks

   *---Add TDS
   nPoints += ( nTDs * 3 )

   *---Add Safetys
   nPoints += ( nSafetys * 2 )

RETURN str( nPoints, 5 )


static FUNCTION ZeroStats

   nSacks   := 0
   nTDs     := 0
   nSafetys := 0

RETURN .T.


static ;
FUNCTION CaptureGame( cTeam )

	local nAgin

	if cTeam = SCHED->HOMETEAM
		*---Home Game
      nAgin = SCHED->AWAYSCORE
	else
		*---Away Game
      nAgin = SCHED->HOMESCORE
	endif

RETURN nAgin


static FUNCTION ScanStats( cseason, cWeek, cGameno, cTeam )

	*---Stats
	if STAT->( dbseek( cseason + cWeek + cGameno + cTeam ) )
		do while .not. STAT->( eof() ) .and. ;
			STAT->SEASON + STAT->WEEK + STAT->GAMENO + STAT->TEAMID = ;
			cseason + cWeek + cGameno + cTeam

			if STAT->STAT = QUARTERBACK_SACKS
				nSacks += STAT->QTY
			endif

			STAT->( dbskip() )
		enddo
	endif	

	*---TDs
	if SCORE->( dbseek( cSeason + cWeek + cGameno ) )
		do while .not. SCORE->( eof() ) .and. ;
			SCORE->SEASON + SCORE->WEEK + SCORE->GAMENO = ;
			cseason + cWeek + cGameno

			if SCORE->TEAM = cTeam
				if SCORE->SCORE $ DEFENSIVE_SCORES
					nTDs++
				elseif SCORE->SCORE = SCORE_SAFETY
					nSafetys++
				endif
			endif

			SCORE->( dbskip() )
		enddo
	else
		TestMsg( "No Scores found " + cSeason + cWeek + cGameno )
	endif		

RETURN nil



*EOF:  NFLRDEF.PRE
