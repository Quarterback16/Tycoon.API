*****************************************************************************
*  Program Name.....: tyrexp.pre
*  Programmer.......: Steve Colonna
*  System...........: TFL
*  Module...........: TYCOON
*  Date.............: 28/9/1993 at 20:18
*  Copyright........: (c) 1993 by Knoware
*  Called From......: TYMENU
*  Purpose..........:
*
*		To list NFL players by expected points
*
*............................................................................
*  Revision.........: 1.0 Last Revised: 28/9/1990 at 20:18
*  Description......: Initial Creation.
*............................................................................
*  Revision.........: 1.1 Last Revised: 21/08/93 at 14:24:35
*  Description......: Store fantasy points in file
*.............................................................................
***************************** ALL RIGHTS RESERVED ***************************

#INCLUDE "..\lib\PRINT.H"
#INCLUDE "TYCOON.H"

#define  LINE_LENGTH 		96

PROCEDURE  tyrexp

	private printto, workfile, cLeagueName, tixfile, nComps, mLeagueId
	private mopt[4], mmess[4], poption1, mSeason, mCat, cWeek, lAlpha, lFree
	private lShowRole, lShowAll

	clear_scn()

	head_line( "EXPECTED OUTPUT" )

	ScreenLine(  1 )
	ScreenLine( 21 )

	if !INIT_EXP()									 && Initialisation routine
		do CLOSE_EXP
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

			do GEN_EXP
			view_prnt( printto, LINE_LENGTH )

		case mopt[poption1] = 'V'

			view_prnt( printto, LINE_LENGTH )

		case mopt[poption1] = 'P'

			print_prnt( printto )

		endcase

	enddo

	do CLOSE_EXP

RETURN


FUNCTION Init_EXP

	private mcontinu

	mLeagueId = G_League
	mSeason   = G_Season
	mCat      = "1"
	cWeek     = G_week
	lAlpha    = .f.
	lFree     = .f.
	lShowRole = .t.
	lShowAll  = .f.

	plswait()
	std_rpt_opt( @mopt, @mmess, "Expected Output" )

	mcontinu = .t.
	mcontinu = iif( mcontinu, opendbf( 'TYSKED', G_ty_path   ), .f. )
	mcontinu = iif( mcontinu, opendbf( 'LINEUP', G_ty_path   ), .f. )
	mcontinu = iif( mcontinu, opendbf( 'LEAGUE', G_ty_path   ), .f. )
	mcontinu = iif( mcontinu, opendbf( 'COMP',   G_ty_path   ), .f. )
	mcontinu = iif( mcontinu, opendbf( 'SCORE',  G_nfl_path  ), .f. )
	mcontinu = iif( mcontinu, opendbf( 'PLAYER', G_nfl_path  ), .f. )
	mcontinu = iif( mcontinu, opendbf( 'STAT',   G_nfl_path  ), .f. )
	mcontinu = iif( mcontinu, opendbf( 'TEAM',   G_nfl_path  ), .f. )
	mcontinu = iif( mcontinu, opendbf( 'SERVE',  G_nfl_path  ), .f. )
	mcontinu = iif( mcontinu, opendbf( 'SCHED',  G_nfl_path  ), .f. )

	if mcontinu
		*---Open temp file
		workfile = g_ty_path + TEMPORARY_FILE_INDICATOR + "EXP"
		if .not. file( workfile + ".dbf")
			makefile( workfile + ".dbf", 6, ;
							"PLAYERID   C  8 0", ;
							"NAME       C 23 0", ;
							"POSDESC    C 10 0", ;
							"ROLE       C 10 0", ;
							"PLAYSFOR   C 20 0", ;
							"POINTS     N  5 1"  )
		endif

		select 0
		if !net_use( workfile, .t., 5, "WORKFILE" )
			error_msg( "Work file is in use, try again later")
			retu ( .f. )
		endif
		tixfile = workfile + '.ntx'
		if .not. file( tixfile)
			index on NAME to ( tixfile  )

		endif
		set index to ( tixfile )

		select PLAYER
		set order to 3

	endif

	@ 23,00

	if ! mcontinu
		error_msg( "Unable to gain access to all the necessary files" )
	endif

RETURN mcontinu


procedure CLOSE_EXP

	close databases

RETURN


function VAL_EXP

	parameters mvar

	private valonly, ok, saveclr, lk

	saveclr = setcolor()
	if lastkey() = UPARROW
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

RETURN( ok )


procedure GEN_EXP

   private i, repoption

	setcolor( c_desc )
	print( 5, 15, "League      :" )
	print( 6, 15, "Season      :" )
	print( 7, 15, "Week        :" )
	print( 8, 15, "Category(all)" )
	print( 9, 15, "Alpha Y/N   :" )
	print(10, 15, "Free Agents :" )
	print(11, 15, "Show Roles  :" )
	print(12, 15, "Show All    :" )

	setcolor( c_fields )

	vget( 5, 29, "mLeagueId" , "!!",   .f. )
	vget( 6, 29, "mSeason" ,   "XXXX", .f. )
	vget( 7, 29, "cWeek",      "XX",   .f. )
	vget( 8, 29, "mCat",       "X",    .f. )
	vget( 9, 29, "lAlpha",     "Y",    .f. )
	vget(10, 29, "lFree",      "Y",    .f. )
	vget(11, 29, "lShowRole",  "Y",    .f. )
	vget(12, 29, "lShowAll",   "Y",    .f. )

	vread( 1, .f., 'VAL_EXP' )

	CLEAR_WINDOW( 5, 15, 12, 79 )
	if abort()
		RETU
	else 
		if cWeek > G_week
			G_week = cWeek
		endif
	endif

	GenMsg()

	nComps  = NoOfComps( mSeason )
	private cComp_[ nComps ]
	nComps  = 0
	select COMP
    COMP->( dbseek( mSeason ) )
	lastcomp = "XX"
	do while .not. eof() .and. SEASON = mSeason
		if LEAGUEID <> lastcomp
			INC( nComps )
			cComp_[ nComps ] = LEAGUEID
			lastcomp         = LEAGUEID
		endif
		skip
	enddo

	if empty( mCat )
		*---EXP all the players
		for nCat = 1 to 6
			mCat = str( nCat, 1 )
			EXPCat( mCat )
			PrintCat( mCat )
		next

	else
		EXPCat( mCat )
		PrintCat( mCat )
	endif

	@ 22,00
	clrmenu()
	GenMsgClr()

RETURN


function HEAD_EXP

   printat( PRINT_CENTER, g_sysowner )
   printat( PRINT_RIGHT, "Date: " +dtoc( date() ) )
   printat( PRINT_RIGHT, "Page: " +str( printpageno, 8 ) )
   println()

	printat( PRINT_CENTER, CatName( mCat ) + "   <PLAYER EXPECTED OUTPUT>   Week " + cWeek )
   println()
	println()			 

	printat( pcol1,   "ID"        )
	printat( pcol2,   "Player"    )
	printat( pcol6,   "Points"    )
	printat( pcol8,   cComp_[1]   )
	printat( pcol8+4, cComp_[2]   )
	if nComps > 2
		printat( pcol8+8, cComp_[3]   )
	endif
	println()			 
	println()			 

RETURN "" 


FUNCTION EXPCat

	parameters mCat

	private nExpPts

	***
	***   Load Data
	***
	select WORKFILE
	zap
	if lAlpha
		index on NAME to ( tixfile  )
	else
		index on descend( str( POINTS, 5, 1 ) ) to ( tixfile  )
	endif

	select PLAYER
	PLAYER->( dbseek( mCat ) )
	do while .not. eof() .and. CATEGORY = mCat

		mPlayerId := PLAYERID
		nExpPts   := 0
		cName     := left( trim( SURNAME ) + ', ' + trim( FIRSTNAME ) + ;
							space(18), 18 ) + ;
							iif( PLAYER->ROOKIEYR = val( G_season ), "*", " " ) +;
							'  ' + CurrTeam( mPlayerID, date() )

		@ 24, 70 say mPlayerId

		if PLAYER->CURRTEAM <> "??" .and. ;
			(.not. PLAYER->ROLE $ " RD" .or. mCat $ "456" ) .and. ;
			.not. PLAYER->ROLE = "I"
			nFPts := 0
			*---Work out what he should get
			nExpPts := PredictPts( mPlayerId, PLAYER->ROLE, mCat, mSeason, ;
											cWeek, mLeagueID )

   		@ 24, 60 say str( nExpPts, 6 )

			if !empty( nExpPts ) .or. lShowAll
				cPlaysFor = ""

				for c = 1 to nComps
					*---Remove this condition if you only want to see who has the
					*---player in the specified league
	*				if cComp_[ c ] = mLeagueId
						cPlaysFor += PlaysFor( mPlayerid, cComp_[ c ], mSeason ) + "  "
	*				endif
				next

				if ( .not. lFree ) .or. ( lFree .and. empty( cPlaysFor ) )
					select WORKFILE
					do ADDRECIT with "Work File"
					replace WORKFILE->PLAYERID with mPlayerId
					replace WORKFILE->NAME     with cName
					replace WORKFILE->POSDESC  with PLAYER->POSDESC
					if lShowRole
						replace WORKFILE->ROLE     with RoleOf( PLAYER->ROLE )
					endif

					replace WORKFILE->PLAYSFOR with cPlaysFor
					replace WORKFILE->POINTS   with nExpPts

				endif
			endif
		endif

		select PLAYER
		skip
	enddo

RETURN NIL

static ;
FUNCTION PredictPts( cPlayerId, cRole, cCat, cSeason, cWeek, cLeagueID )

	local nExpPts  := nOffAvg  := nDefAvg   := nFPts     := 0
	local nOffStat := nDefStat := nOffGames := nDefGames := 0
	local cOppStr
	local lAtHome := .f.

	if SCHED->( dbSeek( cSeason + cWeek ) )
		cHisTeam := CurrTeam( cPlayerId, SCHED->GAMEDATE )
		cOppStr  := Opp( cPlayerId, cWeek, cSeason )
		cOppTeam := right( trim( cOppStr ), 2 )
		lAtHome  := iif( left( cOppStr, 1 ) = "v", .t., .f. ) 
      @ 24,00 say cHisTeam + cOppStr

		*---We have the matchup

		do case
		case cCat = QUARTERBACK_CAT
			*---Number of TD passes his team scored
			nOffStat := PickStat( SCORE_TD_PASS, cSeason, cWeek, cHisTeam, .t. )
			*---Number of TD passes his opponent allowed
			nDefStat := PickStat( SCORE_TD_PASS, cSeason, cWeek, cOppTeam, .f. )

		case cCat = RUNNINGBACK_CAT
			nOffStat := PickStat( SCORE_TD_RUN, cSeason, cWeek, cHisTeam, .t. )
			nDefStat := PickStat( SCORE_TD_RUN, cSeason, cWeek, cOppTeam, .f. )
		case cCat = RECEIVER_CAT
			*---Number of TD passes his team scored
			nOffStat := PickStat( SCORE_TD_PASS, cSeason, cWeek, cHisTeam, .t. )
			*---Number of TD passes his opponent allowed
			nDefStat := PickStat( SCORE_TD_PASS, cSeason, cWeek, cOppTeam, .f. )
		case cCat = KICKER_CAT
			nOffStat := PickStat( SCORE_FIELD_GOAL, cSeason, cWeek, cHisTeam, .t. )
			nDefStat := PickStat( SCORE_FIELD_GOAL, cSeason, cWeek, cOppTeam, .f. )
		case cCat = LINEMAN_CAT
			nOffStat := PickStat( QUARTERBACK_SACKS, cSeason, cWeek, cHisTeam, .t. )
			nDefStat := PickStat( QUARTERBACK_SACKS, cSeason, cWeek, cOppTeam, .f. )
		case cCat = DEFENSIVEBACK_CAT
			nOffStat := PickStat( INTERCEPTIONS_MADE, cSeason, cWeek, cHisTeam, .t. )
			nDefStat := PickStat( INTERCEPTIONS_MADE, cSeason, cWeek, cOppTeam, .f. )
		endcase

*      TestMsg( 'nOffStat' + str( nOffStat, 5 ) )
*      TestMsg( 'nDefStat' + str( nDefStat, 5 ) )

		nOffGames := GamesPlayed( cSeason, cHisTeam, cWeek )
		nDefGames := GamesPlayed( cSeason, cOppTeam, cWeek )

*      TestMsg( 'nOffGames' + str( nOffGames, 5 ) )
*      TestMsg( 'nDefGames' + str( nDefGames, 5 ) )

		nOffAvg := ( nOffStat / nOffGames )
		nDefAvg := ( nDefStat / nDefGames )
	else
		ErrorMsg( "No schedule for " + cSeason + " Week " + cWeek )
	endif

	do case
	case cCat = QUARTERBACK_CAT
		nFPts := PointsFor( cLeagueID, SCORE_TD_PASS, 25 )
*      TestMsg( 'Points For TDpass=' + str( nFPTs, 5 ) )
		if cRole = "B"
			nFpts := int( nFpts / 3 )
		endif
	case cCat = RUNNINGBACK_CAT
		nFPts := PointsFor( cLeagueID, SCORE_TD_RUN, 25 )
		if cRole = "B"
			nFpts := int( nFpts / 2 )
		endif
		if "FB" $ PLAYER->POSDESC
			nFpts := int( nFpts * .67 )
		elseif "HB" $ PLAYER->POSDESC
			nFpts := int( nFpts * .5 )
		endif

	case cCat = RECEIVER_CAT
		nFPts := PointsFor( cLeagueID, SCORE_TD_PASS, 25 ) / 2
		if cRole = "B"
			nFpts := int( nFpts / 2 )
		endif
		if "TE" $ PLAYER->POSDESC
			nFpts := int( nFpts * .8 )
		elseif "HB" $ PLAYER->POSDESC
			nFpts := int( nFpts * .65 )
		endif

	case cCat = KICKER_CAT
		nFPts := PointsFor( cLeagueID, SCORE_FIELD_GOAL, 25 )

	case cCat = LINEMAN_CAT
		nFPts := PointsFor( cLeagueID, QUARTERBACK_SACKS, 25 )
		if cRole = "B"
			nFpts := int( nFpts / 2 )
		endif
		if "DE" $ PLAYER->POSDESC .or. "OLB" $ PLAYER->POSDESC
			nFpts := int( nFpts * 1.4 )
		elseif "NT" $ PLAYER->POSDESC
			nFpts := int( nFpts * .8 )
		endif

	case cCat = DEFENSIVEBACK_CAT
		nFPts := PointsFor( cLeagueID, INTERCEPTIONS_MADE, 25 )
		if cRole = "B"
			nFpts := int( nFpts / 2 )
		endif
		if "FS" $ PLAYER->POSDESC
			nFpts := int( nFpts * 1.2 )
		elseif PLAYER->POSDESC = "SS"
			nFpts := int( nFpts * .8 )
		elseif PLAYER->POSDESC = "S"
			nFpts := int( nFpts * .9 )
		endif
	endcase

	*---Adjust for home field advantage
	if lAtHome
		nFpts = nFPts * 1.2
	else
		nFpts = nFPts * 0.8
	endif

	nExpPts := ( ( nOffAvg + nDefAvg ) / 2 ) * nFPts

*   TestMsg( 'Expected Points ' + str( nExpPts, 4 ) )

RETURN nExpPts


FUNCTION GamesPlayed( cSeason, cTeam, cWeek )

	*---Determines how many games a team has played up to a certain week

	local nGames := 0, cRankSeason, cRankWeek
   if cWeek < "04"
      cRankSeason := str( val( cSeason ) - 1, 4 )
      cRankWeek   := "17"
   else
      cRankWeek   := cWeek
      cRankSeason := cSeason
   endif

	if .not. SCHED->( Seek( cRankSeason ) )
		ErrorMsg( "Season not found " + cRankSeason )
	endif
	
	do while .not. SCHED->( eof() ) ;
				.and. SCHED->SEASON = cRankSeason .and. SCHED->WEEK <= cRankWeek
		*---For all games played 

		if SCHED->AWAYTEAM = cTeam .or. SCHED->HOMETEAM = cTeam .and. ;
			( SCHED->AWAYSCORE + SCHED->HOMESCORE ) > 0
			nGames++
		endif

		@ 24, 10 say str( SCHED->( recno() ), 5 )
		SCHED->( dbSkip() )
	enddo

RETURN nGames


FUNCTION PickStat( cStat, cSeason, cWeek, cTeam, lFor )

	local nStat := nRec := 0

	local cRankSeason, cRankWeek
   if cWeek < "04"
      cRankSeason := str( val( cSeason ) - 1, 4 )
      cRankWeek   := "17"
   else
      cRankWeek   := cWeek
      cRankSeason := cSeason
   endif


	if .not. SCHED->( Seek( cRankSeason ) )
		ErrorMsg( "Season not found " + cRankSeason )
	endif
	
	do while .not. SCHED->( eof() ) ;
				.and. SCHED->SEASON = cRankSeason .and. SCHED->WEEK < cRankWeek
		*---For all games played to date prior to the week
		nRec := SCHED->( recno() )
		if SCHED->AWAYTEAM = cTeam .or. SCHED->HOMETEAM = cTeam
			if SCHED->AWAYTEAM = cTeam
				*---They are the away score
				if lFor
					nStat += GetStat( cStat, SCHED->SEASON, SCHED->WEEK, ;
											SCHED->GAMENO, SCHED->AWAYTEAM )
				else
					nStat += GetStat( cStat, SCHED->SEASON, SCHED->WEEK, ;
											SCHED->GAMENO, SCHED->HOMETEAM )
				endif
			else
				*---They are the home team
				if lFor
					nStat += GetStat( cStat, SCHED->SEASON, SCHED->WEEK, ;
											SCHED->GAMENO, SCHED->HOMETEAM )
				else
					nStat += GetStat( cStat, SCHED->SEASON, SCHED->WEEK, ;
											SCHED->GAMENO, SCHED->AWAYTEAM )
				endif
			endif

		endif

		@ 24, 10 say str( SCHED->( recno() ), 5 )
		select SCHED
		go nRec
		SCHED->( dbSkip() )
	enddo

RETURN nStat


FUNCTION GetStat( cStat, cseason, cWeek, cGameno, cTeam )

	local nStat := 0

	*---Stats
	if cStat $ DEFENSIVE_STATS
		if STAT->( dbseek( cseason + cWeek + cGameno + cTeam ) )
			do while .not. STAT->( eof() ) .and. ;
				STAT->SEASON + STAT->WEEK + STAT->GAMENO + STAT->TEAMID = ;
				cseason + cWeek + cGameno + cTeam

				if STAT->STAT = QUARTERBACK_SACKS .and. cStat = QUARTERBACK_SACKS 
					nStat += STAT->QTY
				endif

				if STAT->STAT = INTERCEPTIONS_MADE .and. cStat = INTERCEPTIONS_MADE
					nStat += STAT->QTY
				endif

				STAT->( dbskip() )
			enddo
		else
*			TestMsg( "No Stats for " + cseason + cWeek + cGameno + cTeam  )
		endif	
	else
		*---TDs & FGs
		if SCORE->( dbseek( cSeason + cWeek + cGameno ) )
			do while .not. SCORE->( eof() ) .and. ;
				SCORE->SEASON + SCORE->WEEK + SCORE->GAMENO = ;
				cseason + cWeek + cGameno

				if SCORE->TEAM = cTeam
				 	if SCORE->SCORE = SCORE_TD_PASS .and. cStat = SCORE_TD_PASS
						nStat++
					endif
					if SCORE->SCORE = SCORE_TD_RUN .and. cStat = SCORE_TD_RUN 
						nStat++
					endif
					if SCORE->SCORE = SCORE_FIELD_GOAL .and. cStat = SCORE_FIELD_GOAL
						nStat++
					endif
				endif

				SCORE->( dbskip() )
			enddo
		else
*			TestMsg( "No Scores found " + cSeason + cWeek + cGameno )
		endif		
	endif		

RETURN nStat


static FUNCTION PrintCat

	parameters mCat

	local nTotalPts := 0, nTotPlyrs := 0

	private    pcol1, pcol2, pcol3, pcol4, pcol5, pcol6, pcol7, pcol8, pcol9, pcol10
	store 0 to pcol1, pcol2, pcol3, pcol4, pcol5, pcol6, pcol7, pcol8, pcol9, pcol10

	printto = g_reppath + TEMPORARY_FILE_INDICATOR + "EP" + mCat +;
						mLeagueId + cWeek + PRINTFILE_EXT

   begin sequence

      set print to ( printto )
		set print on

		print_setup( LINE_LENGTH )

		*---Short length	(Landscape)
		*?? chr(27) +"E" + chr(27)+ chr(38)+ "I" +chr(27) + "(0U" + chr(27) + "(s0p12h12v0s0b8T" + chr(27) + "&a12L"
		p_linespp = 66

      print_margin( 0, 3, 3 )

      prntcoldec( 1 )

      *---set up the printing columns
		prntcolnxt( @pcol1,   8 + 3 )
		prntcolnxt( @pcol2,  19 + 3 )
		prntcolnxt( @pcol3,   2 + 3 )
		prntcolnxt( @pcol4,   6 + 3 )
		prntcolnxt( @pcol5,   2 + 3 )
		prntcolnxt( @pcol6,   3 + 3 )
		prntcolnxt( @pcol7,   3 + 3 )
		prntcolnxt( @pcol8,   3 + 3 )
		prntcolnxt( @pcol9,   3 + 3 )
		prntcolnxt( @pcol10,  3 + 3 )

      print_header( "HEAD_EXP" )
		select WORKFILE
		go top
      do while !eof() 

			print( 24, 70, str( recno(), 6 ) )

         if inkey() = 27
            if !print_cont()
               exit
            endif
         endif

			printat( pcol1,   WORKFILE->PLAYERID   )
			printat( pcol2,   WORKFILE->NAME + " " + WORKFILE->POSDESC )
			if lShowRole
				printat( pcol4+2,   WORKFILE->ROLE       )
			endif
			printat( pcol6,   WORKFILE->POINTS     )
			printat( pcol8,   WORKFILE->PLAYSFOR   )
			println()

			nTotalPts += WORKFILE->POINTS
			nTotPlyrs++

         skip

      enddo          

		println() 
		printat( pcol5,   str( nTotPlyrs, 4 )  )
		printat( pcol6,   str( nTotalPts, 4 )  )
		printat( pcol7,   str( nTotalPts / nTotPlyrs, 5, 1 )  )
		println()
		println()
		println()
      println( 0, "** End of Report **" ) 
      print_page()
		
	end

   print_release()
   set print to        

RETURN nil



*EOF:  TYREXP.PRE

