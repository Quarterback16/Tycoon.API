*****************************************************************************
*  Program Name.....: TYRBAT
*  Programmer.......: Steve Colonna 
*  System...........:
*  Module...........:
*  Date.............: 17/9/1990 at 18:19
*  Copyright........: (c) 1990 by Knoware 
*  Called From......:
*  Purpose..........:
*
*	  Prints out a batch of reports
*
*............................................................................
*  Revision.........: 1.0 Last Revised: 24/9/1995 at 21:21
*  Description......: Initial Creation.
*............................................................................
***************************** ALL RIGHTS RESERVED ***************************

#INCLUDE "..\LIB\PRINT.H"
#INCLUDE "TYCOON.H"

#define  LINE_LENGTH 		182

*#define  DO_RESULTS
#define  DO_PERFORMANCE
*#define  DO_ROSTERS
#define  DO_STANDINGS
*#define  DO_TEAMS
#define  DO_CAPS
#define  DO_FREEAGENTS
#define  DO_WEEKLY
*#define  DO_HEAD
#define  DO_TOPTEN

PROCEDURE TyRBat

	private printto, workfile, cLeagueName
   private mopt[4], mmess[4], poption1

   clear_scn()

   head_line( "BATCH" )

   ScreenLine(  1 )
   ScreenLine( 21 )

   if !INIT_BAT()    && Initialisation routine
      do CLOSE_BAT
      RETU
   endif

   poption1 = 2

   do while poption1 <> 0 
   
      clrmenu()

      poption1 = flatmenu( mopt, mmess, poption1, 22 )
      poption1 = if( poption1 = 0, 1, poption1)

      do case
         case mopt[poption1] = 'X' 

            poption1 = 0

         case mopt[poption1] = 'G'

				do GEN_BAT


      endcase
   
   enddo 

   do CLOSE_BAT

return


function INIT_BAT 

	private mcontinu

	plswait()
   std_rpt_opt( @mopt, @mmess, "Tycoon Matchups" )

   mcontinu = .t.
	mcontinu = iif( mcontinu, opendbf( 'TYSKED', G_ty_path   ), .f. )
   mcontinu = iif( mcontinu, opendbf( 'LINEUP', G_ty_path   ), .f. )
   mcontinu = iif( mcontinu, opendbf( 'LEAGUE', G_ty_path   ), .f. )
   mcontinu = iif( mcontinu, opendbf( 'COMP',   G_ty_path   ), .f. )
   mcontinu = iif( mcontinu, opendbf( 'OWNERS', G_ty_path   ), .f. )
	mcontinu = iif( mcontinu, opendbf( 'SCORE',  G_nfl_path  ), .f. )
   mcontinu = iif( mcontinu, opendbf( 'SCHED',  G_nfl_path  ), .f. )
   mcontinu = iif( mcontinu, opendbf( 'PLAYER', G_nfl_path  ), .f. )
   mcontinu = iif( mcontinu, opendbf( 'SERVE',  G_nfl_path  ), .f. )
   mcontinu = iif( mcontinu, opendbf( 'TEAM',   G_nfl_path  ), .f. )
	mcontinu = iif( mcontinu, opendbf( 'STAT',   G_nfl_path  ), .f. )

	@ 23,00

	if ! mcontinu
		error_msg( "Unable to gain access to all the necessary files" )
	endif

RETURN mcontinu


procedure CLOSE_BAT

	close databases

RETURN


function VAL_BAT

	parameters mvar

	private valonly, ok, saveclr, lk

	saveclr = setcolor()
	if lastkey() = K_UP
		retu( .t. )
	endif

	ok = .t.
	do case

		case mvar == "mLeagueid"

			select LEAGUE
			LEAGUE->( dbseek( mLeagueid ) )
			if found()
				cLeagueName = LEAGUE
			else
				ok = .f.
				error_msg( "League " + mLeagueid + " not found" )
			endif

	endcase
	setcolor( saveclr )

RETURN ok 


procedure GEN_BAT

   private i, repoption, mSeason, mLeagueid, mWeek
	private lAlpha, lFree, lAnon
	private lShowRole, lShowAll, lProject, tixfile, cWeek
   private cStartWeek

	setcolor( c_desc )
	print( 6, 15, "Season :" )
	print( 7, 15, "League :" )
	print( 8, 15, "Week   :" )

	setcolor( c_fields )
	mSeason   = G_season
	mLeagueid = G_league
	mWeek     = G_week

	vget( 6, 25, "mSeason" ,   "XXXX", .f. )
	vget( 7, 25, "mLeagueid" , "!!",   .f. )
	vget( 8, 25, "mWeek",      "XX",   .f. )

	vread( 1, .f., 'VAL_BAT' )

	CLEAR_WINDOW( 6, 15, 8, 79 )
	if abort()
		retu
	else
		G_week   = mWeek
		G_league = mLeagueId
	endif

	GenMsg()

	WriteLog( replicate( "-", 70 ) )
	WriteLog( "Starting Batch Reports" )

	*---TFL Results
	#IFDEF DO_RESULTS	
	WriteLog( "Printing Results" )
	select STAT
	set order to 2
	printto = MatchPrn()
	Testmsg( "Printing to " + printto )

	PrnMatch( mSeason, mLeagueId, mWeek, printto )

	select STAT
	set order to 1

	#ENDIF

	#IFDEF DO_PERFORMANCE
	*---Performance Reports
	WriteLog( "Performance Reports" )
	cTeam = " "		&&		All teams
   *---Add one to week so you can see next opponent
	mWeek := if( val( mWeek ) = 17, mWeek, right( "0" + alltrim( str( val( mWeek ) + 1 ) ), 2 ) )
	TestMsg( "To week = " + mWeek )
	cStartWk := mWeek
	lAll := .t.
   lAnon := .t.
	PrnPerf( mSeason, mSeason, mLeagueID, mWeek, cStartWk, cTeam, lAll, lAnon )

	#ENDIF

	#IFDEF DO_ROSTERS
	*---TFL Rosters
	WriteLog( "TFL Rosters" )
	printto = RostPrn()
	PrnRost( mSeason, mLeagueId, mWeek, printto, lAnon )
	#ENDIF

	#IFDEF DO_STANDINGS
	*---TFL Standings
	WriteLog( "TFL Standings" )
	printto = StandPrn()
	if OpenStnd()			&&		Workfile
		PrnStand( mSeason, mLeagueId, mWeek, printto )
	endif
	#ENDIF

	#IFDEF DO_TEAMS
	*---TFL Team Records
	WriteLog( "TFL Team Records" )
	if OpenTeam()
		mTeam = " "		&&		All teams
		PrnTeam( mSeason, mLeagueID, mTeam )
	endif
	#ENDIF

	#IFDEF DO_CAPS
	*---TFL Caps
	WriteLog( "TFL Caps" )
	if OpenCaps()
		mTeam = " "		&&		All teams
		PrnCaps( mSeason, mLeagueID, mWeek, mTeam )
	endif
	#ENDIF

	#IFDEF DO_FREEAGENTS
	*---Free Agents
   select WORKFILE
   use
	WriteLog( "Free Agents" )
	if	OpenRank()
		lAlpha 		:= .f.
		lFree  		:= .t.
		lShowRole	:= .f.
		lShowAll		:= .f.
		lProject		:= .f.
		cWeek			:= mWeek
		lTopTen		:= .f.
      cStartWeek  := "01"
		PrnRank( mSeason, mLeagueID, cWeek, " ", lAlpha, lFree, lShowRole, ;
				lShowAll, lProject, lTopTen, cStartWeek, K_PLAYOFF_TEAMS )
	endif
	#ENDIF

	#IFDEF DO_TOPTEN
	*---Top Ten
   select WORKFILE
   use
	WriteLog( "Top Ten" )
	if	OpenRank()
		lAlpha 		:= .f.
		lFree  		:= .f.
		lShowRole	:= .f.
		lShowAll		:= .f.
		lProject		:= .f.
		lTopTen		:= .t.
		cWeek			:= mWeek
      cStartWeek  := "01"
		PrnRank( mSeason, mLeagueID, cWeek, " ", lAlpha, lFree, lShowRole, ;
				lShowAll, lProject, lTopTen, cStartWeek )
	endif
	#ENDIF

	#IFDEF DO_WEEKLY
	*---Weekly High score
	WriteLog( "Weekly High score" )
	printto := WeekPrn()
	PrnWeek( mSeason, mLeagueID, printto )
	#ENDIF

	#IFDEF DO_HEAD
	*---TFL Head to Head
	WriteLog( "TFL Head to Head" )
	printto 		:= HeadPrn()
	cSeasonFr	:=	"1988"
	cSeasonTo	:= mSeason
	lFull			:= .t.
	lAll			:= .f.
	if OpenHead()
		cTeam := " "		&&		All teams
		PrnHead( cSeasonFr, cSeasonTo, mLeagueID, cTeam, lFull, lAll, printto )
	endif
	#ENDIF

	WriteLog( "Batch reports finished" )

RETURN

static ;
FUNCTION WRITELOG( _msg )

	LOCAL log_msg, nHandle, spacing, cLogFile

	spacing = space(2)

	set century off

*	log_msg = time() + spacing + _msg + chr(13) + chr(10)
	log_msg = dtoc( date() ) + " - " + time() + spacing + ;
				 _msg + chr(13) + chr(10)

	cLogFile = "RBAT.LOG"
	nhandle  = fopen( cLogFile, 2)

	if nHandle = -1
		*---File not found?
		nHandle = fcreate( cLogFile )
	endif

	if nHandle <> -1
		fseek(  nHandle, 0, 2)
		fwrite( nHandle, log_msg )
		fclose( nHandle )
	endif

RETURN nil


*EOF:  TYRBAT.PRG