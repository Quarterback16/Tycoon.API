*****************************************************************************
*  Program Name.....: tyrrank.pre
*  Programmer.......: Steve Colonna
*  System...........:
*  Date.............: 28/9/1990 at 20:18
*  Copyright........: (c) 1990 by Knoware
*  Called From......:
*  Purpose..........:
*
*		To list NFL players by ranking
*
*		Uses TotalPts( cPlayerID, cSeason, nWeek ) from h:nfl_lib
*
*............................................................................
*  Revision.........: 1.0 Last Revised: 28/9/1990 at 20:18
*  Description......: Initial Creation.
*............................................................................
*  Revision.........: 1.1 Last Revised: 21/08/93 at 14:24:35
*  Description......: Store fantasy points in file
*.............................................................................
*  Revision.........: 1.2 Last Revised: 23/08/94 at 20:36:14
*  Description......: Allow ranking by projections
*.............................................................................
*  Revision.........: 1.3 Last Revised: 06/10/94 at 20:45:21
*  Description......: Improved Free Agent logic
*.............................................................................
*  Revision.........: 1.4 Last Revised: 30/09/95 at 21:08:34
*  Description......: Top 10
*.............................................................................
*  Revision.........: 1.5 Last Revised: 19/01/98 at 19:35:46
*  Description......: Added a from week parameter.
*.............................................................................
***************************** ALL RIGHTS RESERVED ***************************

#INCLUDE "..\LIB\PRINT.H"
#INCLUDE "TYCOON.H"

#define  LINE_LENGTH 				96
#define  LAST_WEEK_OF_SEASON 		"17"

#define  K_DEBUG_PLAYER          "XXXXXX01"

PROCEDURE  TyRrank

	private printto, workfile, cLeagueName, tixfile, nComps, mLeagueId, cTeamFilt
	private mopt[4], mmess[4], poption1, mSeason, mCat, cWeek, lAlpha, lFree
	private lShowRole, lShowAll, lProject, lTopTen, cStartWeek

	clear_scn()

	head_line( "RANKINGS" )

	ScreenLine(  1 )
	ScreenLine( 21 )

	if !INIT_RANK()										 && Initialisation routine
		do CLOSE_RANK
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

			do GEN_RANK
			view_prnt( printto, LINE_LENGTH )

		case mopt[poption1] = 'V'

			view_prnt( printto, LINE_LENGTH )

		case mopt[poption1] = 'P'

			print_prnt( printto )

		endcase

	enddo

	do CLOSE_RANK

RETURN


FUNCTION Init_rank

	private mcontinu

	mLeagueId  = "GS"
	mSeason    = G_Season
	mCat       = "1"
	cWeek      = G_week
    cStartWeek = "01"
	lAlpha     = .f.
	lFree      = .t.
	lShowRole  = .t.
	lShowAll   = .f.
	lProject   = .f.
	lTopTen	  = .f.
    cTeamFilt  = K_PLAYOFF_TEAMS

	plswait()
	std_rpt_opt( @mopt, @mmess, "Tycoon Rankings" )

	mcontinu = .t.
	mcontinu = iif( mcontinu, opendbf( 'TYSKED', G_ty_path   ), .f. )
	mcontinu = iif( mcontinu, opendbf( 'LINEUP', G_ty_path   ), .f. )
	mcontinu = iif( mcontinu, opendbf( 'LEAGUE', G_ty_path   ), .f. )
	mcontinu = iif( mcontinu, opendbf( 'COMP',   G_ty_path   ), .f. )
	mcontinu = iif( mcontinu, opendbf( 'SCORE',  G_nfl_path  ), .f. )
	mcontinu = iif( mcontinu, opendbf( 'PLAYER', G_nfl_path  ), .f. )
	mcontinu = iif( mcontinu, opendbf( 'STAT',   G_nfl_path  ), .f. )
	mcontinu = iif( mcontinu, opendbf( 'SERVE',  G_nfl_path  ), .f. )

	if mcontinu
		*---Open temp file
		mcontinu = OpenRank()
	endif

	@ 23,00

	if ! mcontinu
		error_msg( "Unable to gain access to all the necessary files" )
	endif

RETURN mcontinu 


FUNCTION OpenRank

	workfile = g_ty_path + TEMPORARY_FILE_INDICATOR + "RANK"
	if .not. file( workfile + ".dbf")
		makefile( workfile + ".dbf", 7, ;
						"PLAYERID   C  8 0", ;
						"NAME       C 23 0", ;
						"POSDESC    C 10 0", ;
						"ROLE       C 10 0", ;
						"PLAYSFOR   C 20 0", ;
						"PARVALUE   N  2 0", ;
						"POINTS     N  4 0"  )
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

	select STAT
	set order to 2

RETURN .t.

procedure CLOSE_RANK

	close databases

RETURN


function VAL_RANK

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
			LEAGUE->( dbseek ( mLeagueid ) )
			if found()
				cLeagueName = LEAGUE
			else
				ok = .f.
				error_msg( "League " + mLeagueid + " not found" )
			endif

	endcase
	setcolor( saveclr )

RETURN ok


procedure GEN_RANK

   private i, repoption

	setcolor( c_desc )
	print( 5, 15, "League      :" )
	print( 6, 15, "Season      :" )
	print( 7, 15, "from Week   :" )
	print( 8, 15, "to Week     :" )
	print( 9, 15, "Category(all)" )
	print(10, 15, "Alpha Y/N   :" )
	print(11, 15, "Free Agents :" )
	print(12, 15, "Show Roles  :" )
	print(13, 15, "Show All    :" )
	print(14, 15, "Projections :" )
	print(15, 15, "Top Ten     :" )
	print(16, 15, "Team Filter :" )

	setcolor( c_fields )

	vget( 5, 29, "mLeagueId" , "!!",   .f. )
	vget( 6, 29, "mSeason" ,   "XXXX", .f. )
	vget( 7, 29, "cStartWeek", "XX",   .f. )
	vget( 8, 29, "cWeek",      "XX",   .f. )
	vget( 9, 29, "mCat",       "X",    .f. )
	vget(10, 29, "lAlpha",     "Y",    .f. )
	vget(11, 29, "lFree",      "Y",    .f. )
	vget(12, 29, "lShowRole",  "Y",    .f. )
	vget(13, 29, "lShowAll",   "Y",    .f. )
	vget(14, 29, "lProject",   "Y",    .f. )
	vget(15, 29, "lTopTen",    "Y",    .f. )
	vget(16, 29, "cTeamFilt",  "XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX",    .f. )

	vread( 1, .f., 'VAL_RANK' )

*	CLEAR_WINDOW( 5, 15, 15, 79 )
	if abort()
		RETU
	else 
		if cWeek > G_week
			G_week = cWeek
			if G_week >= LAST_WEEK_OF_SEASON
				Error_Msg( "Storing Ranks on player file" )
			endif
		endif
	endif

	GenMsg()

	PrnRank( mSeason, mLeagueID, cWeek, mCat, lAlpha, lFree, lShowRole, ;
				lShowAll, lProject, lTopTen, cStartWeek, cTeamFilt )

RETURN


FUNCTION PrnRank( mSeason, mLeagueID, cWeek, mCat, lAlpha, lFree, lShowRole, ;
				lShowAll, lProject, lTopTen, cStartWeek, cTeamFilt )

	nComps  = NoOfComps( mSeason )
	private cComp_[ nComps ]
	nComps  = 0

	if lTopTen
*		TestMsg( "Top Ten!" )
	endif

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
		*---Rank all the players
		for nCat = 1 to 6
			mCat = str( nCat, 1 )
			RankCat( mCat, cTeamFilt )
			PrintCat( mCat, lTopTen )
		next

	else
		RankCat( mCat, cTeamFilt )
		PrintCat( mCat, lTopTen )
	endif

	@ 22,00
*	CloseDbf( "WORKFILE" )
	clrmenu()
	GenMsgClr()

RETURN .t.


function HEAD_RANK

   printat( PRINT_CENTER, g_sysowner )
   printat( PRINT_RIGHT, "Date: " +dtoc( date() ) )
   printat( PRINT_RIGHT, "Page: " +str( printpageno, 8 ) )
   println()

	printat( PRINT_CENTER, CatName( mCat ) + "   <PLAYER RANKINGS>   " + ;
					"From Week " + cStartWeek + " to " + cWeek + ;
					 iif( lTopTen, "  TOP 10", "  " ) )
   println()
	println()			 

	printat( pcol1,   "ID"        )
	printat( pcol2,   "Player"    )
	printat( pcol6,   "Points"    )
	printat( pcol8,   cComp_[1]   )
	if nComps > 1
   	printat( pcol8+4, cComp_[2]   )
* 	   printat( pcol8+8, cComp_[3]   )
	endif
	println()			 
	println()			 

RETURN "" 


FUNCTION UnitValue

	parameters nPoints

	LOCAL nParValue

	nParValue = int( ( nPoints / 10 ) + 1 )

RETURN nParValue


FUNCTION RankCat

	parameters mCat, cTeamFilt

	local nPoints

	***
	***   Load Data
	***
	select WORKFILE
	zap
	if lAlpha
		index on NAME to ( tixfile  )
	else
		index on descend( str( POINTS, 4, 0 ) ) to ( tixfile  )
	endif

	select PLAYER
	PLAYER->( dbseek( mCat ) )
	do while .not. eof() .and. CATEGORY = mCat

		mPlayerId = PLAYERID
		nPoints   = 0
		cName     = left( trim( SURNAME ) + ', ' + trim( FIRSTNAME ) + ;
							space(18), 18 ) + ;
							iif( PLAYER->ROOKIEYR = val( G_season ), "*", " " ) +;
							'  ' + CurrTeam( mPlayerID, date() )

		@ 24, 70 say mPlayerId
      if	mPlayerId = K_DEBUG_PLAYER
         TestMsg( "Doing " + cName )
      endif

		if lProject
			*---Use projections
			nPoints := PLAYER->PROJECTED
		else
			for w = val( cStartWeek ) to val( cWeek )

				mWeek = strzero( w, 2 )

				ADDTO( nPoints, TotalPts( mPlayerid, mSeason, mWeek, mLeagueID ) )

			next
		endif

      if	mPlayerId = K_DEBUG_PLAYER
         TestMsg( cName + " has " + str( nPoints, 4 ) + " points" )
      endif

		if !empty( nPoints ) .or. lShowAll
			cPlaysFor = ""

			for c = 1 to nComps
				*---Remove this condition if you only want to see who has the
				*---player in the specified league
*				if cComp_[ c ] = mLeagueId
					cPlaysFor += PlaysFor( mPlayerid, cComp_[ c ], mSeason ) + "  "
*				endif
			next

			if ( .not. lFree ) .or. ( lFree .and. FreeAgent( mLeagueID, mPlayerID, mSeason ) )
				if TeamOk( PLAYER->CURRTEAM, cTeamFilt )
					select WORKFILE
					do ADDRECIT with "Work File"
					replace WORKFILE->PLAYERID with mPlayerId
					replace WORKFILE->NAME     with cName
					replace WORKFILE->POSDESC  with PLAYER->POSDESC
					if lShowRole
						replace WORKFILE->ROLE     with RoleOf( PLAYER->ROLE )
					endif

					replace WORKFILE->PLAYSFOR with cPlaysFor
					replace WORKFILE->POINTS   with nPoints

					if cWeek = LAST_WEEK_OF_SEASON
						*---Store par value against player
						select PLAYER
						do RLOCKIT with "Player file"
						replace PLAYER->CURRATING 	with UnitValue( WORKFILE->POINTS )
						replace PLAYER->FPTS			with WORKFILE->POINTS

					endif
					replace WORKFILE->PARVALUE with PLAYER->CURRATING
				endif
			endif
		endif

		select PLAYER
		skip
	enddo

RETURN NIL


FUNCTION TeamOk
   parameters cTeam, cTeamFilt
   local lOk
   lOk := .t.
   if cTeam = "??"
      lOk := .f.
   else
		if	empty(cTeamFilt)
			lOk = .t.
		else
			if len( trim( cTeamFilt ) ) > 0
				if cTeam $ cTeamFilt
					lOk := .t.
				else
					lOk := .f.
				endif
			endif
      endif
   endif

return lOk


FUNCTION PrintCat

	parameters mCat, lTopTen

	local nTotalPts := 0, nTotPlyrs := 0, nLastPoints := 0

	private    pcol1, pcol2, pcol3, pcol4, pcol5, pcol6, pcol7, pcol8, pcol9, pcol10
	store 0 to pcol1, pcol2, pcol3, pcol4, pcol5, pcol6, pcol7, pcol8, pcol9, pcol10

	printto = g_reppath + TEMPORARY_FILE_INDICATOR + "RK" + mCat +;
						mLeagueID + iif( lTopTen, "10", "" ) + PRINTFILE_EXT

   begin sequence

      set print to ( printto )
		set print on

		print_setup( LINE_LENGTH, .t. )

		*---Short length	(Landscape)
		?? chr(27) +"E" + chr(27)+ chr(38)+ "I" +chr(27) + "(0U" + chr(27) + "(s0p12h12v0s0b8T" + chr(27) + "&a12L"
		p_linespp = 66

      print_margin( 0, 3, 3 )

      prntcoldec( 1 )

      *---set up the printing columns
		prntcolnxt( @pcol1,  11 + 3 )
		prntcolnxt( @pcol2,  19 + 3 )
		prntcolnxt( @pcol3,   2 + 3 )
		prntcolnxt( @pcol4,   6 + 3 )
		prntcolnxt( @pcol5,   2 + 3 )
		prntcolnxt( @pcol6,   2 + 3 )
		prntcolnxt( @pcol7,   3 + 1 )
		prntcolnxt( @pcol8,   3 + 3 )
		prntcolnxt( @pcol9,   3 + 3 )
		prntcolnxt( @pcol10,  3 + 3 )

      print_header( "HEAD_RANK" )
		select WORKFILE
		go top
      do while !eof() 

			print( 24, 70, str( recno(), 6 ) )

         if inkey() = K_ESC
            if !print_cont()
               exit
            endif
         endif

			nTotPlyrs++
			if lTopTen
				if nTotPlyrs > 9
					if WORKFILE->POINTS < nLastPoints
						exit
					else
						nLastPoints := WORKFILE->POINTS
					endif
				endif
			endif

			printat( pcol1,   str( nTotPlyrs, 3 ) + "  " + WORKFILE->PLAYERID )
			printat( pcol2,   WORKFILE->NAME + " " + WORKFILE->POSDESC )
			if lShowRole
				printat( pcol4+2,   WORKFILE->ROLE       )
         else
				printat( pcol4+2,   WORKFILE->PARVALUE   )
			endif
			printat( pcol6,   WORKFILE->POINTS     )
			printat( pcol8,   WORKFILE->PLAYSFOR   )
			println()

			nTotalPts += WORKFILE->POINTS

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


FUNCTION FreeAgent( cLeague, mPlayerID, mSeason )

	*---Determines if a player is a free agent
   local cYr, lFreeAgent, cPlaysFor
   lFreeAgent = .t.

*  cYr = str( val( mSeason ) + 1, 4 )			&&  use this for doing THE PAGE
   cYr = mSeason

   cPlaysFor = PlaysFor( mPlayerid, cLeague, cYr )

   if	mPlayerid = K_DEBUG_PLAYER
      TestMsg(	mPlayerid + " plays for " + cPlaysFor + " in league " + cLeague + " season " + mSeason )
   endif

   if empty( cPlaysFor )
      lFreeAgent = .t.
   else
      lFreeAgent = .f.
   endif


RETURN lFreeAgent



*EOF:  TYRRANK.PRE
