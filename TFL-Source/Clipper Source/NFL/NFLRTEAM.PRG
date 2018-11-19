*****************************************************************************
*  Program Name.....: NFLRTEAM.PRG 
*  Programmer.......: Steve Colonna 
*  System...........: TFL
*  Module...........: NFL
*  Date.............: 28/9/1990 at 20:18
*  Copyright........: (c) 1990 by Knoware 
*  Called From......: NFLMENU
*  Purpose..........:
*
*		To Print NFL team stats
*
*	Team:  XX XXXXXXXXXXXXXX   Games from  YYYY WW to YYYY WW
*
*	YYYY WW  DD/MM/YY 	vs XXXXXXXXXXXXXX		999 - 999  WLT		(W-L-T)
*	YYYY WW  DD/MM/YY 	vs XXXXXXXXXXXXXX		999 - 999  WLT		(W-L-T)
*	YYYY WW  DD/MM/YY 	vs XXXXXXXXXXXXXX		999 - 999  WLT		(W-L-T)
*	YYYY WW  DD/MM/YY 	vs XXXXXXXXXXXXXX		999 - 999  WLT		(W-L-T)
*															---------			--------
*															999 - 999         (W-L-T)
*
*   new Section:-
*   WK     OP	 SCORE  R  YDr	 TDr	YDp  TDp  SAl	Ial  Ral  Pal	SAK  INT
*   NN	 @TTm	 99-99  W  999  999  999  999	 999  999  999	 999	999  999
*
*	OFFENSE			PASS		RUSH					DEFENSE		    PASS		RUSH
*
*	Plays		  		99999	   99999					Plays		      99999	   99999
*	Play %		 	  100 *    100					Play %		     100      100
*	TD				  	  999		  999					TD				     999		  999
*	FG				  	     		     					FG                      
*
*	Pass Yds	  	   99999								Pass Yds	  	   99999
*	Pass Comp		  999								Pass Comp		  999
*	Comp %			 99.9	* 							Comp %			 99.9
*	Intercepted 	 9999								Intercepts  	 9999
*	Sacks allowed   9999								Sacks           9999
*
*	Rush Yds       99999								Rush Yds       99999
*	Avg carry        9.9	*						   Avg carry        9.9
*
*
*
*			Structure for database : TEAM.dbf
*			Field  Field Name  Type       Width    Dec
*			    1  TEAMID      Character      2
*			    2  TEAMNAME    Character     15
*			    3  CITY        Character     15
*............................................................................
*  Revision.........: 1.0 Last Revised: 28/9/1990 at 20:18
*  Description......: Initial Creation.
*............................................................................
***************************** ALL RIGHTS RESERVED ***************************

#INCLUDE "..\lib\PRINT.H"
#INCLUDE "NFL.H"

#define  LINE_LENGTH 		176

#define	PLAYS					1
#define	RUSHES				2
#define	PASSES				3
#define	YARDS_PASSING		4
#define	YARDS_RUSHING		5
#define	INTERCEPTED  		6
#define	COMPLETIONS  		7
#define	SACKS_ALLOWED		8
#define	PASSING_TDS  		9
#define	RUSHING_TDS  		10
#define	OTHER_TDS	  		11
#define	FIELDGOALS  		12

#define	K_OFFENCE  		1
#define	K_DEFENCE  		2

#define	K_NFLTEAMS 	   	32

#define	K_TEAM_STATS 		 7
#define	K_TEAMSTAT_PO 		 1
#define	K_TEAMSTAT_RO 		 2
#define	K_TEAMSTAT_PP 		 3
#define	K_TEAMSTAT_PR 		 4
#define	K_TEAMSTAT_RD 		 5
#define	K_TEAMSTAT_PTD		 6
#define	K_TEAMSTAT_INT		 7

#define	K_TEST_TEAM       'XX'

static aGameLine := {}
static aTeamLine := {}
static aStat[ 12, 2 ]
static aTStat[ 12, 2 ]
static aWStat[ 12, 2 ]
static nGames  := 0
static nWins   := 0
static nLosses := 0
static nTies   := 0
static nSpreadWins   := 0
static nSpreadLosses := 0
static nSpreadTies   := 0
static lAll    := .f.
static aTm[ K_NFLTEAMS, K_TEAM_STATS ]
static aNFLTeams[ K_NFLTEAMS ]
static aTeamStat[ K_NFLTEAMS, 2 ]

PROCEDURE  NflRteam

	private printto, workfile, cLeagueName, tixfile, nComps, cLeagueId, bRatings
	private mopt[4], mmess[4], poption1, cCat, lAlpha
	private cTeam,   cSeason1, cWeek1,   cSeason2, cWeek2, dRoster, cSchedYr

   clear_scn()

   head_line( "TEAM STATS" )

   ScreenLine(  1 )
   ScreenLine( 21 )

   if .not. InitTstat()
      CloseTstat()
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

				GenTstat()
            view_prnt( printto, LINE_LENGTH )

         case mopt[poption1] = 'V'

            view_prnt( printto, LINE_LENGTH )

         case mopt[poption1] = 'P'

				print_prnt( printto )

      endcase
   
   enddo 

   CloseTstat()

RETURN


FUNCTION GenTstat

   local cLastWeek, nOldStatOrd, nTeamRec

	setcolor( c_desc )
	print(  5, 10, "Team      |" )
	print(  6, 10, "Week Range:" )
	print(  7, 10, "Roster    :" )
	print(  8, 10, "Schedule  :" )
	print(  9, 10, "F League  :" )
	print( 10, 10, "Do Ratings:" )
	print( 12, 15, "Leave Team blank for all teams" )
	print( 13, 15, "Leave Roster date blank to skip roster" )

	setcolor( c_fields )

	vget(  5, 25, "cTeam",      "!!",   .f. )
	vget(  6, 25, "cSeason1",   "XXXX", .f. )
	vget(  6, 30, "cWeek1",     "XX",   .f. )
	vget(  6, 40, "cSeason2",   "XXXX", .f. )
	vget(  6, 45, "cWeek2",     "XX",   .f. )
	vget(  7, 25, "dRoster",    "@D",   .f. )
	vget(  8, 25, "cSchedYr",   "XXXX", .f. )
	vget(  9, 25, "cLeagueId",  "!!",   .f. )
	vget( 10, 25, "bRatings",   "Y",    .f. )

	vread( 1, .f., 'ValTstat' )

	scroll( 4, 5, 19, 79, 0 )
	
	if abort()
		RETU
	endif

	GenMsg()

	cFirstWeek := cSeason1 + cWeek1
	cLastWeek  := cSeason2 + cWeek2

	if empty( cTeam )
		*---Print all teams
      lAll = .t.
		if TEAM->( dbseek( cSeason2 ) )
         nTeamRec = TEAM->( recno() )
			do while .not. TEAM->( eof() ) .and. TEAM->SEASON = cSeason2
            @ 24, 00 say "Doing " + TEAM->TEAMNAME
				PrintTeam( TEAM->TEAMID, cFirstWeek, cLastWeek )
            select TEAM
            go nTeamRec
				TEAM->( dbskip() )
            nTeamRec = TEAM->( recno() )
			enddo
         if bRatings
            StoreRatings()
         endif

		endif

	else
		PrintTeam( cTeam, cFirstWeek, cLastWeek )
	endif

RETURN nil


static FUNCTION PrintTeam( cTeam, cFirstWeek, cLastWeek  )

	printto = g_reppath + TEMPORARY_FILE_INDICATOR + "REP" + cTeam + ;
								 substr( cSeason2, 3, 2 ) + PRINTFILE_EXT

	ZeroStats( .t. )
   aTeamLine := {}

	if .not. SCHED->( dbSeek( cFirstWeek ) )
		ErrorMsg( cFirstWeek + " not found" )
	endif
	do while .not. SCHED->( eof() ) ;
				.and. SCHED->SEASON <= cSeason2 .and. SCHED->WEEK <= cWeek2
		*---For all games in the range
		if SCHED->AWAYTEAM = cTeam .or. SCHED->HOMETEAM = cTeam

			CaptureGame( cTeam, cWeek2 )

         if SCHED->WEEK <= cWeek2
            *---Week is in range
            *---Scan Offense and Defense
			   ScanStats( SCHED->SEASON, SCHED->WEEK, SCHED->GAMENO, cTeam, 1 )
			   if SCHED->AWAYTEAM = cTeam
				   ScanStats( SCHED->SEASON, SCHED->WEEK, SCHED->GAMENO, SCHED->HOMETEAM, 2 )
			   else
				   ScanStats( SCHED->SEASON, SCHED->WEEK, SCHED->GAMENO, SCHED->AWAYTEAM, 2 )
			   endif
            CaptureTeam( cTeam )
      	   ZeroStats( .f. )
         endif

		endif

		@ 24, 10 say str( SCHED->( recno() ), 5 )
		SCHED->( dbSkip() )
	enddo

	private    pcol1, pcol2, pcol3, i
	store 0 to pcol1, pcol2, pcol3, i

   begin sequence

      set print to ( printto )
		set print on

		print_setup( LINE_LENGTH, .t. )	//	Continuous

//		?? chr(27) +"E"
		p_linespp = 70

      print_margin( 3, 3, 3 )

      prntcoldec( 1 )

      *---set up the printing columns
		prntcolnxt( @pcol1,  35  )
		prntcolnxt( @pcol2,  6   )
		prntcolnxt( @pcol3,  140 )

		printAt( pcol1, TeamStr( cTeam ) + space(6) + TEAM->RATE + ;
                                             ' (' + RateToPts( TEAM->RATE ) + ')' )
		println()
		println()

		if	cSchedYr > cSeason2
         *---Print the upcoming schedule
         GetSched( cTeam, cSchedYr )
      endif

      *--Print game results
		for i := 1 to nGames
			PrintAt( pcol1, aGameLine[ i ] )
			println()
		next
		println()

      *--Print Stat Breakdown
		PrintAt( pcol1, "WK OP  SCORE  R  YDr TDr  YDp TDp  SAl  Ial  Ral RTa  Pal PTa  SAK  INT"  )
		println()
		for i := 1 to nGames
			PrintAt( pcol1, aTeamLine[ i ] )
			println()
*         aTeamLine[ i ] = ""
		next
		println()

		PrintStats()
		PrintScorers( cTeam, cFirstWeek, cLastWeek )
		if .not. empty( dRoster )
			*---leave roster date blank for no roster
			PrintRoster( cTeam, cLastWeek )
		endif

		if	lAll .and. cTeam = "WR"
         *---Show Averages
         PrintTStats()
      endif

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


static;
FUNCTION	GetSched( cTeam, cSchedYr )

	if SCHED->( dbSeek( cSchedYr ) )
      *---Clear game array
      nGames    := 0
      aGameLine := {}
   else
		ErrorMsg( cSchedYr + " not found" )
	endif

	do while .not. SCHED->( eof() ) .and. SCHED->SEASON = cSchedYr
		*---For all games in the range
		if SCHED->AWAYTEAM = cTeam .or. SCHED->HOMETEAM = cTeam
			CaptureGame( cTeam, cWeek2 )
		endif

		@ 24, 10 say str( SCHED->( recno() ), 5 )
		SCHED->( dbSkip() )
	enddo

return nil


FUNCTION InitTstat 

	private mcontinu, i, j

   set century on
	cSeason1  := G_Season
	cSeason2  := G_Season
   cSchedyr  := G_season
	cTeam     := "  "
	cWeek1    := "01"
	cWeek2    := G_week
	dRoster	 := date()
   cLeagueID := "YH"
   bRatings  := .f.

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
	
	for i = 1 to 12
		aTStat[ i, 1 ] := 0
		aTStat[ i, 2 ] := 0
	next
	for i = 1 to K_NFLTEAMS
      for j = 1 to K_TEAM_STATS
         aTm[ i, j ] := 0
      next
   next

   i := 0
   select TEAM
   set filter to SEASON = G_season
   go top
   do while .not. eof()
      i = i + 1
      aNFLTeams[ i ] := TEAM->TEAMID
      skip
   enddo

	plswait()
   std_rpt_opt( @mopt, @mmess, "Team Stats" )

	@ 23,00
	
    mcontinu = .t.
	select 0
	OpenDbf( 'COMP', g_ty_path )

   select TEAM
   set order to 1

	if ! mcontinu
		error_msg( "Unable to gain access to all the necessary files" )
	endif

RETURN mcontinu


FUNCTION CloseTstat

*	close databases
   select TEAM
   set filter to
   set century off

	CloseDbf( 'COMP' )
	CloseDbf( 'TEAM' )
	CloseDbf( 'SCHED' )		
	CloseDbf( 'PLAYER' )		

RETURN .t.


FUNCTION ValTstat

	parameters mvar

	local valonly, ok, saveclr, lk

	saveclr = setcolor()
	if lastkey() = UPARROW
		RETU( .t. )
	endif

	ok = .t.
	do case
	case mvar = "cSeason1xxxx"

		if .not. empty( cTeam )
			if .not. TEAM->( dbseek( cSeason1 + cTeam ) )
				ok = .f.
				Error_Msg( "Invalid Team season combo " + cSeason1 + cTeam +;
                       " indexord " + str( indexord(),1 )  )
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


static ;
FUNCTION PrintRoster( cTeam, cLastWeek )

	*---Prints players on the team
	*
	*		QB
	*			XXXXXXXX, XXXXXX
	*		RB
	*		PR
	*		PK
	*		DL
	*		DB
	*
	*			For each category
	*				load an array with players who are on the team roster
	*				Sort alphabetically
	*				print
	*
	local aPlayers, dGame, nOldOrd, nTotPlayers := 0

	*---Roster from When ?
	dGame := dRoster

	aPlayers := {}

	select SERVE
	nOldOrd := indexord()
	set order to SERVE_BY_TEAM

	if SERVE->( dbseek( cTeam ) )
		do while .not. SERVE->( eof() ) .and. SERVE->TEAMID = cTeam
			if ( SERVE->FROM <= dGame .and. dGame <= SERVE->TO ) .or. ;
				( dGame >= SERVE->FROM .and. empty( SERVE->TO ) )
				aadd( aPlayers, { SERVE->PLAYERID, ;
										PlayerName( SERVE->PLAYERID ), ;
										PlayerPos( SERVE->PLAYERID ) } )
            nTotPlayers++
			endif

			SERVE->( dbskip() )
		enddo
	endif

	*---Sort array by Name and Category
	aPlayers := asort( aPlayers,,, { |x,y| x[3]+x[2] < y[3]+y[2] } )

	*---Print roster
	PrintAt( pcol1,  "Roster as of " + dtoc( dGame ) + "  Roster size:" + str( nTotPlayers, 3 ) )
	println()
	println()
	PrintAt( pcol1+3,  "Pos         Name                    Role Age  Yrs     FP   Fteam    Proj" )
	println()

	aeval( aPlayers, { |element| i++, PutPlayer( element[1] ) }  )

	select SERVE
	set order to nOldOrd

RETURN nil


static FUNCTION PutPlayer( cPlayerID )

	local cPlayer, aPlyrCat
	static cLastCat := "0"
	aPlyrCat := {	 "Quarterbacks", 		 ;
						 "Running backs",		 ;
						 "Pass Receivers",  	 ;
						 "Place Kickers",		 ;
						 "Defensive Linemen", ;
						 "Defensive Backs",   ;
						 "Offensive Linemen"	 }

	cPlayer := PlayerName( cPlayerID )

   if .not. empty( cPlayer )
	   if PLAYER->CATEGORY <> cLastCat .and. val( PLAYER->CATEGORY ) > 0 .and. ;
													     val( PLAYER->CATEGORY ) < 8
		   PrintAt( pcol1, 	aPlyrCat[ val( PLAYER->CATEGORY ) ] )
		   cLastCat := PLAYER->CATEGORY
		   println()
	   endif

	   PrintAt( pcol1+4, 	PLAYER->POSDESC )
	   PrintAt( pcol1+15, 	cPlayer )
	   PrintAt( pcol1+40, 	PLAYER->ROLE  )
	   PrintAt( pcol1+45, 	Age( PLAYER->DOB ) )
	   PrintAt( pcol1+50, 	str( nSeasons( PLAYER->ROOKIEYR ), 2 ) )
	   PrintAt( pcol1+55, 	str( PLAYER->FPTS, 4 ) )
	   PrintAt( pcol1+62, 	PlaysFor( PLAYER->PLAYERID, cLeagueID, G_Season ) )
	   PrintAt( pcol1+70, 	str( PLAYER->PROJECTED, 4 ) )
	   PrintAt( pcol1+76, 	LastStat( PLAYER->PLAYERID ) )
	   println()
   endif

RETURN nil


FUNCTION LastStat( cPlayerID )

   local cLastStat, nOldArea, nOldOrder
   cLastStat := ' -NONE- '

	nOldArea  = select()
   nOldRec = recno()
*  TestMsg( 'need Starting ' + cCat + ' for ' + cTeam )

   select STAT
   nOldOrder = indexord()

   set order to 2  &&  	PLAYERID + STAT + SEASON + WEEK + GAMENO
   STAT->( dbseek( cPlayerID) )
   do while .not. eof() .and. STAT->PLAYERID = cPlayerID
      cLastStat := STAT->SEASON + ':' + STAT->WEEK
      select STAT
      skip
   enddo

   select STAT
   set order to nOldOrder
   if cLastStat = ' -NONE- '
      *---Have a look for scores
      select SCORE
      nOldOrder = indexord()
      set order to 2  &&  PLAYERID1
      SCORE->( dbseek( cPlayerID ) )
      do while .not. eof() .and. SCORE->PLAYERID1 = cPlayerID
        cLastStat := SCORE->SEASON + ':' + SCORE->WEEK
        select SCORE
        skip
      enddo

      if cLastStat = ' -NONE- '
         *--Still no dice
         set order to 3  &&  PLAYERID2
         SCORE->( dbseek( cPlayerID ) )
         do while .not. eof() .and. SCORE->PLAYERID2 = cPlayerID
           cLastStat := SCORE->SEASON + ':' + SCORE->WEEK
           select SCORE
           skip
         enddo
      endif
      set order to nOldOrder
   endif

   select( nOldArea )
   go nOldRec


RETURN cLastStat


FUNCTION nSeasons( nRookieYr )

	local nSeasons

	if nRookieYr > 0
		nSeasons := val( G_season ) - nRookieYr
	else
		nSeasons := 0
	endif

RETURN nSeasons


FUNCTION DateOf( cLastWeek, cTeam )

	*---Return the date of the game for a particular team

	local dGame
	dGame := date()

	if SCHED->( dbseek( cLastWeek ) )
		do while .not. SCHED->( eof() ) .and. ;
							cLastWeek = SCHED->SEASON + SCHED->WEEK
			if SCHED->HOMETEAM = cTeam .or. ;
				SCHED->AWAYTEAM = cTeam
				dGame := SCHED->GAMEDATE
				exit
			endif
			SCHED->( dbskip() )
		enddo
	endif

RETURN dGame


STATIC FUNCTION PrintScorers( cTeam, cFirstWeek, cLastWeek )

	***
	***	Add a section to this report which lists scorers, uses SCOREX1
	***
	local aScorers, aScoreCats, i, j
	aScoreCats := { ;
	                	{ "Touchdown Passes",  SCORE_TD_PASS      }, ;
							{ "Touchdown Catches", SCORE_TD_CATCH     }, ;
							{ "Touchdown Runs",    SCORE_TD_RUN       }, ;
							{ "Field Goals",       SCORE_FIELD_GOAL   }, ;
							{ "Sacks",             QUARTERBACK_SACKS  }, ;
							{ "Interceptions",     INTERCEPTIONS_MADE }  ;
					  }

*  TestMsg( "aScoreCats has " + str( len(aScoreCats) ) + " elements" )
	println()
	for i := 1 to len( aScoreCats )
		aScorers  := {}
      if	i < 5
   		LoadScorers( cTeam, cFirstWeek, cLastWeek, aScorers, aScoreCats[ i, 2 ] )
      else
         LoadStats( cTeam, cFirstWeek, cLastWeek, aScorers, aScoreCats[ i, 2 ]  )
      endif

		PrintAt( pcol1, aScoreCats[ i, 1 ] )
		println()
		println()
		for j = 1 to len( aScorers )
			PrintAt( pcol1+3,  aScorers[ j, 1 ] )
			PrintAt( pcol2,    iif( i=5, str( aScorers[ j, 2 ], 8, 1 ), str( aScorers[ j, 2 ], 8 ) ) )
			PrintAt( pcol3+15, PlaysFor( aScorers[ j, 3 ], "YH", G_Season ) )
			PrintAt( pcol3+22, PlaysFor( aScorers[ j, 3 ], "G1", G_Season ) )
			println()
		next
		println()
	next
	println()

RETURN nil


static FUNCTION LoadStats( cTeam, cFirstWeek, cLastWeek, aScorers, cStat )

	*---loads an array with the scorers
	if STAT->( dbseek( cFirstWeek ) )
		do while .not. STAT->( eof() ) .and. ;
			STAT->SEASON + STAT->WEEK + STAT->GAMENO <= cLastWeek

			if STAT->TEAMID = cTeam
				*---Is it the stat we are looking for?
				if STAT->STAT = cStat
					AddPlyr( STAT->PLAYERID, aScorers, STAT->QTY  )
				endif
			endif

			STAT->( dbskip() )
		enddo
	else
*		TestMsg( "No Scores found " + cFirstWeek )
	endif		

	*---Sort the array
	aScorers := asort( aScorers,,, { |x,y| x[2] > y[2] } )

RETURN nil



static FUNCTION LoadScorers( cTeam, cFirstWeek, cLastWeek, aScorers, cStat )

	*---loads an array with the scorers
	if SCORE->( dbseek( cFirstWeek ) )
		do while .not. SCORE->( eof() ) .and. ;
			SCORE->SEASON + SCORE->WEEK + SCORE->GAMENO <= cLastWeek

			if SCORE->TEAM = cTeam
				*---Is it the stat we are looking for?
				if SCORE->SCORE = SCORE_TD_PASS
					if cStat = SCORE_TD_CATCH
						*---TD catch
						AddPlyr( SCORE->PLAYERID1, aScorers, 1  )
					else
						if cStat = SCORE_TD_PASS
							*---TD throw
							AddPlyr( SCORE->PLAYERID2, aScorers, 1  )
						endif
					endif
				else
					if SCORE->SCORE = cStat
						AddPlyr( SCORE->PLAYERID1, aScorers, 1  )
					endif
				endif
			endif

			SCORE->( dbskip() )
		enddo
	else
*		TestMsg( "No Scores found " + cFirstWeek )
	endif		

	*---Sort the array
	aScorers := asort( aScorers,,, { |x,y| x[2] > y[2] } )

RETURN nil


static FUNCTION AddPlyr( cPlayerID, aScorers, nIncrement )

	local cPlayer, p

	cPlayer = PlayerName( cPlayerID )
	*---Have we got this one?
	p = ascan( aScorers, { |x| x[1] = cPlayer } )
	if p = 0
		*---newbie
		aadd( aScorers, { cPlayer, nIncrement, cPlayerID } )
	else
		*---exists
*     TestMsg( "nIncrement =" + str( nIncrement ) )
		aScorers[ p, 2 ] += nIncrement
	endif

RETURN nil


FUNCTION PrintStats

	PrintAt( pcol1, "OFFENSE       PASS      RUSH" )
	PrintAt( pcol3, "DEFENSE       PASS      RUSH" )
	println()
	println()

	PrintAt( pcol1, "Plays     " + ;
		 str( aStat[ PASSES, 1 ], 9 ) + str( aStat[ RUSHES, 1 ], 9 )  )
	PrintAt( pcol3, "Plays     " + ;
		 str( aStat[ PASSES, 2 ], 9 ) + str( aStat[ RUSHES, 2 ], 9 )  )
	println()
	PrintAt( pcol1, "Play %    " + ;
		 str( Percent( aStat[ PASSES, 1 ], aStat[ PLAYS, 1 ], .f. ), 9, 1 ) +;
		 str( Percent( aStat[ RUSHES, 1 ], aStat[ PLAYS, 1 ], .f. ), 9, 1 ) )
	PrintAt( pcol3, "Play %    " + ;
		 str( Percent( aStat[ PASSES, 2 ], aStat[ PLAYS, 2 ], .f. ), 9, 1 ) +;
		 str( Percent( aStat[ RUSHES, 2 ], aStat[ PLAYS, 2 ], .f. ), 9, 1 ) )
	println()
	PrintAt( pcol1, "TDs       " + ;
		 str( aStat[ PASSING_TDS, 1 ], 9 ) + str( aStat[ RUSHING_TDS, 1 ], 9 )  )
	PrintAt( pcol3, "TDs allowd" + ;
		 str( aStat[ PASSING_TDS, 2 ], 9 ) + str( aStat[ RUSHING_TDS, 2 ], 9 )  )
	println()
	PrintAt( pcol1, "FGs       " +  str( aStat[ FIELDGOALS, 1 ], 9 ) )
	PrintAt( pcol3, "FGs allowd" +  str( aStat[ FIELDGOALS, 2 ], 9 ) )
	println()
	println()
	PrintAt( pcol1, "Yardage   " + ;
		 str( aStat[ YARDS_PASSING, 1 ], 9 ) + str( aStat[ YARDS_RUSHING, 1 ], 9 )  )
	PrintAt( pcol3, "Yds allowd" + ;
		 str( aStat[ YARDS_PASSING, 2 ], 9 ) + str( aStat[ YARDS_RUSHING, 2 ], 9 )  )
	println()
	PrintAt( pcol1, "Pass Comp " +  str( aStat[ COMPLETIONS, 1 ], 9 ) + ;
	                                str( ( aStat[ YARDS_PASSING, 1 ] / aStat[ COMPLETIONS, 1 ] ), 9, 1 ) )


	PrintAt( pcol3, "Pass Comp " +  str( aStat[ COMPLETIONS, 2 ], 9 ) )
	println()
	PrintAt( pcol1, "Comp %    " +  str( Percent( aStat[ COMPLETIONS, 1 ], ;
													 			 aStat[ PASSES, 1 ], .f. ), 9, 1 ) )
	PrintAt( pcol3, "Comp % ald" +  str( Percent( aStat[ COMPLETIONS, 2 ], ;
													          aStat[ PASSES, 2 ], .f. ), 9, 1 ) )
	println()
	PrintAt( pcol1, "Interceptd" +  str( aStat[ INTERCEPTED, 1 ], 9 ) )
	PrintAt( pcol3, "Intercepts" +  str( aStat[ INTERCEPTED, 2 ], 9 ) )
	println()
	PrintAt( pcol1, "Sacks Alld" +  str( aStat[ SACKS_ALLOWED, 1 ], 9 ) )
	PrintAt( pcol3, "Sacks made" +  str( aStat[ SACKS_ALLOWED, 2 ], 9 ) )
	println()

RETURN .T.


FUNCTION PrintTStats

   *---Average the figures
   local i
	for i = 1 to 12
		aTStat[ i, 1 ] := ( aTStat[ i, 1 ] / K_NFLTEAMS ) / nGames
		aTStat[ i, 2 ] := ( aTStat[ i, 2 ] / K_NFLTEAMS ) / nGames
	next

	PrintAt( pcol1, "AVERAGE TEAM" )
	println()
	PrintAt( pcol1, "OFFENSE       PASS      RUSH" )
	PrintAt( pcol3, "DEFENSE       PASS      RUSH" )
	println()
	println()

	PrintAt( pcol1, "Plays     " + ;
		 str( aTStat[ PASSES, 1 ], 9 ) + str( aTStat[ RUSHES, 1 ], 9 )  )
	PrintAt( pcol3, "Plays     " + ;
		 str( aTStat[ PASSES, 2 ], 9 ) + str( aTStat[ RUSHES, 2 ], 9 )  )
	println()
	PrintAt( pcol1, "Play %    " + ;
		 str( Percent( aTStat[ PASSES, 1 ], aTStat[ PLAYS, 1 ], .f. ), 9, 1 ) +;
		 str( Percent( aTStat[ RUSHES, 1 ], aTStat[ PLAYS, 1 ], .f. ), 9, 1 ) )
	PrintAt( pcol3, "Play %    " + ;
		 str( Percent( aTStat[ PASSES, 2 ], aTStat[ PLAYS, 2 ], .f. ), 9, 1 ) +;
		 str( Percent( aTStat[ RUSHES, 2 ], aTStat[ PLAYS, 2 ], .f. ), 9, 1 ) )
	println()
	PrintAt( pcol1, "TDs       " + ;
		 str( aTStat[ PASSING_TDS, 1 ], 9 ) + str( aTStat[ RUSHING_TDS, 1 ], 9 )  )
	PrintAt( pcol3, "TDs allowd" + ;
		 str( aTStat[ PASSING_TDS, 2 ], 9 ) + str( aTStat[ RUSHING_TDS, 2 ], 9 )  )
	println()
	PrintAt( pcol1, "FGs       " +  str( aTStat[ FIELDGOALS, 1 ], 9 ) )
	PrintAt( pcol3, "FGs allowd" +  str( aTStat[ FIELDGOALS, 2 ], 9 ) )
	println()
	println()
	PrintAt( pcol1, "Yardage   " + ;
		 str( aTStat[ YARDS_PASSING, 1 ], 9 ) + str( aTStat[ YARDS_RUSHING, 1 ], 9 )  )
	PrintAt( pcol3, "Yds allowd" + ;
		 str( aTStat[ YARDS_PASSING, 2 ], 9 ) + str( aTStat[ YARDS_RUSHING, 2 ], 9 )  )
	println()

	PrintAt( pcol1, "Pass Comp " +  str( aTStat[ COMPLETIONS, 1 ], 9 ) + ;
	                                str( ( aTStat[ YARDS_PASSING, 1 ] / aTStat[ COMPLETIONS, 1 ] ), 9, 1 ) )

	PrintAt( pcol3, "Pass Comp " +  str( aTStat[ COMPLETIONS, 2 ], 9 ) )
	println()


	PrintAt( pcol1, "Comp %    " +  str( Percent( aTStat[ COMPLETIONS, 1 ], ;
													 			 aTStat[ PASSES, 1 ], .f. ), 9, 1 ) )
	PrintAt( pcol3, "Comp % ald" +  str( Percent( aTStat[ COMPLETIONS, 2 ], ;
													          aTStat[ PASSES, 2 ], .f. ), 9, 1 ) )
	println()
	PrintAt( pcol1, "Interceptd" +  str( aTStat[ INTERCEPTED, 1 ], 9 ) )
	PrintAt( pcol3, "Intercepts" +  str( aTStat[ INTERCEPTED, 2 ], 9 ) )
	println()
	PrintAt( pcol1, "Sacks Alld" +  str( aTStat[ SACKS_ALLOWED, 1 ], 9 ) )
	PrintAt( pcol3, "Sacks made" +  str( aTStat[ SACKS_ALLOWED, 2 ], 9 ) )
	println()

return nil


FUNCTION ZeroStats( lAll )

	local i

	if	lAll

	   for i = 1 to 12
		   aStat[  i,  1 ] := 0
		   aStat[  i,  2 ] := 0
		   aWStat[  i, 1 ] := 0
		   aWStat[  i, 2 ] := 0
	   next

	   aGameLine := {}
	   nGames    := 0
	   nWins     := 0
	   nLosses   := 0
	   nTies     := 0
	   nSpreadWins     := 0
	   nSpreadLosses   := 0
	   nSpreadTies     := 0
   else
	   for i = 1 to 12
		   aWStat[  i, 1 ] := 0
		   aWStat[  i, 2 ] := 0
	   next
   endif

RETURN .T.


FUNCTION CaptureGame( cTeam, cWeek2 )

	local cGameLine, cTeamLine

	nGames++
	cWLT       := WltStr( cTeam )
	cSpreadWLT := SpreadWltStr( cTeam )

   cTeamLine := SCHED->WEEK + " "

	if cTeam = SCHED->HOMETEAM
		*---Home Game
		cGameLine = SCHED->SEASON + " " + SCHED->WEEK + "  " +;
									 dtoc( SCHED->GAMEDATE ) + "   vs " + ;
									 left( TeamStr( SCHED->AWAYTEAM ), 22 )

      cTeamLine += "v" + SCHED->AWAYTEAM

      if SCHED->WEEK <= cWeek2
         cGameLine += " " + str( SCHED->HOMESCORE, 3 ) + " - " + ;
									 str( SCHED->AWAYSCORE, 3 ) + "  " + ;
									 cWLT + "  " + RecordStr() + str( SCHED->SPREAD, 6, 1 ) +;
                            "  " + cSpreadWLT + "  " + SpreadRecordStr()
         cTeamLine += " " + str( SCHED->HOMESCORE, 2 ) + "-" +;
                      str( SCHED->AWAYSCORE, 2 ) + "  " + cWLT
      endif


	else
		*---Away Game
		cGameLine = SCHED->SEASON + " " + SCHED->WEEK + "  " +;
									 dtoc( SCHED->GAMEDATE ) + "   at " + ;
									 left( TeamStr( SCHED->HOMETEAM ), 22 )

      cTeamLine += "@" + SCHED->HOMETEAM

      if SCHED->WEEK <= cWeek2
         cGameLine += " " + str( SCHED->AWAYSCORE, 3 ) + " - " + ;
									 str( SCHED->HOMESCORE, 3 ) + "  " + ;
									 cWLT + "  " + RecordStr() + str( SCHED->SPREAD, 6, 1 ) +;
                            "  " + cSpreadWLT + "  " + SpreadRecordStr()
         cTeamLine += " " + str( SCHED->AWAYSCORE, 2 ) + "-" +;
                       str( SCHED->HOMESCORE, 2 ) + "  " + cWLT
      endif
	endif
	aadd( aGameLine, cGameLine )

   *---Build Team Line
*   TestMsg( "Adding a team line >" + cTeamLine )
	aadd( aTeamLine, cTeamLine + "  " )
*   TestMsg( "Length of aTeamLine is " + str( len( aTeamLine ), 3 ) )

	@ 24, 70 say str( nGames, 4 )

RETURN nil


FUNCTION CaptureTeam( cTeam )

	local cTeamLine, cGradeGame

   cTeamLine := str( aWStat[ YARDS_RUSHING, K_OFFENCE ], 3 ) + "  " +;
                str( aWStat[ RUSHING_TDS,   K_OFFENCE ], 2 ) + "  " +;
                str( aWStat[ YARDS_PASSING, K_OFFENCE ], 3 ) + "  " +;
                str( aWStat[ PASSING_TDS,   K_OFFENCE ], 2 ) + "  " +;
                str( aWStat[ SACKS_ALLOWED, K_OFFENCE ], 3 ) + "  " +;
                str( aWStat[ INTERCEPTED,   K_OFFENCE ], 3 ) + "  " +;
                str( aWStat[ YARDS_RUSHING, K_DEFENCE ], 3 ) + "  " +;
                str( aWStat[ RUSHING_TDS,   K_DEFENCE ], 2 ) + "  " +;
                str( aWStat[ YARDS_PASSING, K_DEFENCE ], 3 ) + "  " +;
                str( aWStat[ PASSING_TDS,   K_DEFENCE ], 2 ) + "  " +;
                str( aWStat[ SACKS_ALLOWED, K_DEFENCE ], 3 ) + "  " +;
                str( aWStat[ INTERCEPTED,   K_DEFENCE ], 3 )

*	TestMsg( "Team line " + str( nGames, 3 ) + " is " + cTeamLine )
*  TestMsg( "Length of aTeamLine is " + str( len( aTeamLine ), 3 ) )

   ***
   ***  Grade the game
   ***
   cGameGrade := GradeGame( cTeam,                                ;
                            aWStat[ YARDS_PASSING, K_OFFENCE ],   ;
                            aWStat[ YARDS_RUSHING, K_OFFENCE ],   ;
                            aWStat[ SACKS_ALLOWED, K_OFFENCE ],   ;
                            aWStat[ SACKS_ALLOWED, K_DEFENCE ],   ;
                            aWStat[ YARDS_RUSHING, K_DEFENCE ],   ;
                            aWStat[ YARDS_PASSING, K_DEFENCE ],   ;
                            aWStat[ INTERCEPTED,   K_DEFENCE ]      )


	aTeamLine[ nGames ] += cTeamLine + '  ' + cGameGrade + ' (' + RateToPts( cGameGrade ) + ')'

   *---IncStats
   IncTeamStats( cTeam, K_TEAMSTAT_PO, aWStat[ YARDS_PASSING, K_OFFENCE ] )
   IncTeamStats( cTeam, K_TEAMSTAT_RO, aWStat[ YARDS_RUSHING, K_OFFENCE ] )
   IncTeamStats( cTeam, K_TEAMSTAT_PP, aWStat[ SACKS_ALLOWED, K_OFFENCE ] )
   IncTeamStats( cTeam, K_TEAMSTAT_PR, aWStat[ SACKS_ALLOWED, K_DEFENCE ] )
   IncTeamStats( cTeam, K_TEAMSTAT_RD, aWStat[ YARDS_RUSHING, K_DEFENCE ] )
   IncTeamStats( cTeam, K_TEAMSTAT_PTD, aWStat[ PASSING_TDS, K_DEFENCE ] )
   IncTeamStats( cTeam, K_TEAMSTAT_INT, aWStat[ INTERCEPTED, K_DEFENCE ] )

RETURN nil


static FUNCTION GradeGame( cTeam, nYDp, nYDr, nSAKAllowed, nSAK, nYDrDef, nYDpDef )

  local cGrade, cPO, cRO, cPP, cPR, cRD, cPD

  do case
  case nYDp > 319
     cPO := 'A'
  case nYDp > 219
     cPO := 'B'
  case nYDp > 169
     cPO := 'C'
  case nYDp > 109
     cPO := 'D'
  otherwise
     cPO := 'E'
  endcase

  do case
  case nYDr > 199
     cRO := 'A'
  case nYDr > 149
     cRO := 'B'
  case nYDr > 99
     cRO := 'C'
  case nYDr >  39
     cRO := 'D'
  otherwise
     cRO := 'E'
  endcase

  do case
  case nSAKAllowed = 0
     cPP := 'A'
  case nSAKAllowed = 1
     cPP := 'B'
  case nSAKAllowed < 4
     cPP := 'C'
  case nSAKAllowed < 6
     cPP := 'D'
  otherwise
     cPP := 'E'
  endcase

  do case
  case nSAK = 0
     cPR := 'E'
  case nSAK = 1
     cPR := 'D'
  case nSAK < 4
     cPR := 'C'
  case nSAK < 6
     cPR := 'B'
  otherwise
     cPR := 'A'
  endcase

  do case
  case nYDrDef > 199
     cRD := 'E'
  case nYDrDef > 149
     cRD := 'D'
  case nYDrDef >  99
     cRD := 'C'
  case nYDrDef >  39
     cRD := 'B'
  otherwise
     cRD := 'A'
  endcase

  do case
  case nYDpDef > 319
     cPD := 'E'
  case nYDpDef > 219
     cPD := 'D'
  case nYDpDef > 169
     cPD := 'C'
  case nYDpDef > 119
     cPD := 'B'
  otherwise
     cPD := 'A'
  endcase

  cGrade := cPO + cRO + cPP + cPR + cRD + cPD

  WriteLog( 'Grades', cTeam + ',' + cPO + ',' + cRO + ',' + cPP + ',' + ;
                                    cPR + ',' + cRD + ',' + cPD  )

RETURN cGrade


static FUNCTION ScanStats( cseason, cWeek, cGameno, cTeam, nOffDef )

	*---Stats
	if STAT->( dbseek( cseason + cWeek + cGameno + cTeam ) )
		do while .not. STAT->( eof() ) .and. ;
			STAT->SEASON + STAT->WEEK + STAT->GAMENO + STAT->TEAMID = ;
			cseason + cWeek + cGameno + cTeam

			if STAT->STAT $ OFFENSIVE_PLAYS
				aStat[  PLAYS, nOffDef ] += STAT->QTY
				aTStat[ PLAYS, nOffDef ] += STAT->QTY
				aWStat[ PLAYS, nOffDef ] += STAT->QTY
			endif
			if STAT->STAT = RUSHING_CARRIES
				aStat[  RUSHES, nOffDef ] += STAT->QTY
				aTStat[ RUSHES, nOffDef ] += STAT->QTY
				aWStat[ RUSHES, nOffDef ] += STAT->QTY

			elseif STAT->STAT = PASSING_ATTEMPTS
				aStat[  PASSES, nOffDef ] += STAT->QTY
				aTStat[ PASSES, nOffDef ] += STAT->QTY
				aWStat[ PASSES, nOffDef ] += STAT->QTY

			elseif STAT->STAT = PASSING_YARDAGE
				aStat[  YARDS_PASSING, nOffDef ] += STAT->QTY
				aTStat[ YARDS_PASSING, nOffDef ] += STAT->QTY
				aWStat[ YARDS_PASSING, nOffDef ] += STAT->QTY

			elseif STAT->STAT = RUSHING_YARDS
				aStat[  YARDS_RUSHING, nOffDef ] += STAT->QTY
				aTStat[ YARDS_RUSHING, nOffDef ] += STAT->QTY
				aWStat[ YARDS_RUSHING, nOffDef ] += STAT->QTY

			elseif STAT->STAT = PASSES_INTERCEPTED
            if STAT->QTY > 6
               TestMsg( cTeam + " Week " + cWeek + " Intercepts > 6 "  )
            endif

				aStat[  INTERCEPTED, nOffDef ] += STAT->QTY
				aTStat[ INTERCEPTED, nOffDef ] += STAT->QTY
				aWStat[ INTERCEPTED, nOffDef ] += STAT->QTY

			elseif STAT->STAT = PASS_COMPLETIONS
				aStat[  COMPLETIONS, nOffDef ] += STAT->QTY
				aTStat[ COMPLETIONS, nOffDef ] += STAT->QTY
				aWStat[ COMPLETIONS, nOffDef ] += STAT->QTY

			elseif STAT->STAT = QUARTERBACK_SACKS
				aStat[  SACKS_ALLOWED, iif( nOffDef = K_OFFENCE, 2, 1 ) ] += STAT->QTY
				aTStat[ SACKS_ALLOWED, iif( nOffDef = K_OFFENCE, 2, 1 ) ] += STAT->QTY
				aWStat[ SACKS_ALLOWED, iif( nOffDef = K_OFFENCE, 2, 1 ) ] += STAT->QTY
			endif

			STAT->( dbskip() )
		enddo
	endif	

	*---TDs & FGs
	if SCORE->( dbseek( cSeason + cWeek + cGameno ) )
		do while .not. SCORE->( eof() ) .and. ;
			SCORE->SEASON + SCORE->WEEK + SCORE->GAMENO = ;
			cseason + cWeek + cGameno

			if SCORE->TEAM = cTeam
				if SCORE->SCORE $ TOUCHDOWN_SCORES
					if SCORE->SCORE = SCORE_TD_PASS
						aStat[  PASSING_TDS, nOffDef ] ++
						aTStat[ PASSING_TDS, nOffDef ] ++
						aWStat[ PASSING_TDS, nOffDef ] ++

					elseif SCORE->SCORE = SCORE_TD_RUN
						aStat[  RUSHING_TDS, nOffDef ] ++
						aTStat[ RUSHING_TDS, nOffDef ] ++
						aWStat[ RUSHING_TDS, nOffDef ] ++
					else
						aStat[  OTHER_TDS, nOffDef ] ++
						aTStat[ OTHER_TDS, nOffDef ] ++
						aWStat[ OTHER_TDS, nOffDef ] ++
					endif
				elseif SCORE->SCORE = SCORE_FIELD_GOAL
					aStat[  FIELDGOALS, nOffDef ] ++
					aTStat[ FIELDGOALS, nOffDef ] ++
					aWStat[ FIELDGOALS, nOffDef ] ++

				endif
			endif

			SCORE->( dbskip() )
		enddo
	else
*		TestMsg( "No Scores found " + cSeason + cWeek + cGameno )
	endif		

RETURN nil


static FUNCTION incTeamStat( cTeam, nStat, nQty )

  if cTeam = K_TEST_TEAM
     TestMsg( cTeam + ' for ' + str(nStat,1) + ' had ' + str(nQty,5) )
  endif

  nTeam = ascan( aNFLTeams, cTeam )

  aTm[ nTeam, nStat ] = aTm[ nTeam, nStat ] + nQty

  if cTeam = K_TEST_TEAM
     TestMsg( 'Total now ' + str( aTm[ nTeam, nStat], 5 ) )
  endif

return nil


FUNCTION WLtStr( cTeam )

	LOCAL cStr

	if cTeam = SCHED->HOMETEAM
		if SCHED->HOMESCORE > SCHED->AWAYSCORE
			cStr = "W"
			nWins++
		elseif SCHED->HOMESCORE = SCHED->AWAYSCORE
			cStr = "T"
			nTies++
		else
			cStr = "L"
			nLosses++
		endif
	else
		if SCHED->AWAYSCORE > SCHED->HOMESCORE
			cStr = "W"
			nWins++
		elseif SCHED->AWAYSCORE = SCHED->HOMESCORE
			cStr = "T"
			nTies++
		else
			cStr = "L"
			nLosses++
		endif
	endif

RETURN( cStr )


FUNCTION SpreadWLtStr( cTeam )

	LOCAL cStr

	if cTeam = SCHED->HOMETEAM
		if SCHED->HOMESCORE > ( SCHED->AWAYSCORE + SCHED->Spread )
			cStr = "W"
			nSpreadWins++
		elseif SCHED->HOMESCORE = ( SCHED->AWAYSCORE + SCHED->Spread )
			cStr = "T"
			nSpreadTies++
		else
			cStr = "L"
			nSpreadLosses++
		endif
	else
		if ( SCHED->AWAYSCORE + SCHED->SPREAD ) > SCHED->HOMESCORE
			cStr = "W"
			nSpreadWins++
		elseif ( SCHED->AWAYSCORE + SCHED->SPREAD ) = SCHED->HOMESCORE
			cStr = "T"
			nSpreadTies++
		else
			cStr = "L"
			nSpreadLosses++
		endif
	endif

RETURN( cStr )


FUNCTION RecordStr

RETURN( "(" + str( nWins, 2 ) + "-" + str( nLosses, 2 ) +;
				 iif( nTies > 0, "-" + str( nTies, 2 ), "" ) + ")" )


FUNCTION SpreadRecordStr

RETURN( "(" + str( nSpreadWins, 2 ) + "-" + str( nSpreadLosses, 2 ) +;
				 iif( nSpreadTies > 0, "-" + str( nSpreadTies, 2 ), "" ) + ")" )



static FUNCTION StoreRatings

   local s, i, cRate, cRank

   for s = 1 to 5
      for i = 1 to K_NFLTEAMS
         aTeamStat[ i , 1 ] := aTm[ i, s ]
         aTeamStat[ i , 2 ] := aNFLTeams[ i ]
      next
      if s = K_TEAMSTAT_PP .or. s = K_TEAMSTAT_RD
         *---the less sacks allowed the better
         *---the less Rushing yards allowed the better allowed the better
         aTeamStat := asort( aTeamStat,,, { |x,y| x[1] < y[1] } )
      else
         aTeamStat := asort( aTeamStat,,, { |x,y| x[1] > y[1] } )
      endif

      for i = 1 to K_NFLTEAMS
         select TEAM
         if dbseek( cSeason2 + aTeamStat[ i, 2 ] )
            cRate := TEAM->RATE
            cRank := RankFor( i )
            if s = 1
               cRate := cRank + substr(cRate, 2, 5)
            else
               cRate := substr(cRate, 1, s-1 ) + cRank + substr(cRate, s+1, 6-s)
            endif
            replace TEAM->RATE with cRate
         endif
			*--- write it to the next game too
			*nextWeek = val( cWeek2 ) + 1
         *TestMsg( 'Next week ' + str( nextWeek, 2 ) )	
         *select SCHED
         *if dbseek( cSeason2 + TEAM->TEAMCODE + strzero( nextWeek, 2 ) )
			*   do while ( SCHED->SEASON = cSeason2 .and. SCHED->WEEK = strzero( nextWeek, 2 ) )

			*      if SCHED->AWAYTEAM = TEAM->TEAMCODE
			*         replace AWAYRATE with cRate
			*	   else
			*	      replace HOMERATE with cRate
  			*     SCHED->( dbskip() )
		   *   enddo
			*endif
      next
   next

   *---Finally do Pass Defense
   for i = 1 to K_NFLTEAMS
      if aTm[ i, K_TEAMSTAT_PTD ] = 0
         aTeamStat[ i , 1 ] := 0
      else
         aTeamStat[ i , 1 ] := aTm[ i, K_TEAMSTAT_INT ] / aTm[ i, K_TEAMSTAT_PTD ]
      endif
      aTeamStat[ i , 2 ] := aNFLTeams[ i ]
      if aTeamStat[ i, 2 ] = K_TEST_TEAM
         TestMsg( K_TEST_TEAM + ' had ' + str( aTm[ i, K_TEAMSTAT_INT ], 3 ) + ' picks' )
         TestMsg( 'and given up ' + str( aTm[ i, K_TEAMSTAT_PTD ], 3 ) +  ' TDp' )
      endif
   next
   aTeamStat := asort( aTeamStat,,, { |x,y| x[1] > y[1] } )
   for i = 1 to K_NFLTEAMS
      if  aTeamStat[ i, 2 ] = K_TEST_TEAM
         TestMsg( K_TEST_TEAM + ' is ranked # ' + str(i,2) + ' in Pass Defense' )
         TestMsg( 'Their ratio is ' + str( aTeamStat[ i, 1 ], 6, 2 ) )
      endif

      select TEAM
      if dbseek( cSeason2 + aTeamStat[ i, 2 ] )
         cRate := TEAM->RATE
         cRank := RankFor( i )
         cRate := substr(cRate, 1, 5) + cRank
         replace TEAM->RATE with cRate
      endif
   next

RETURN

STATIC FUNCTION RankFor( i )
local cRank
   do case
   case i <  4
      cRank := "A"
   case i < 10
      cRank := "B"
   case i < 23
      cRank := "C"
   case i < 29
      cRank := "D"
   otherwise
      cRank := "E"
   endcase

RETURN cRank


            
*EOF:  NFLRTEAM.PRE
