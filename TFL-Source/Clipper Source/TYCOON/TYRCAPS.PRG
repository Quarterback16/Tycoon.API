*****************************************************************************
*  Program Name.....: TYRCAPS.pre 
*  Programmer.......: Steve Colonna 
*  System...........:
*  Module...........:
*  Date.............: 1/1/1991 at 20:36
*  Copyright........: (c) 1991 by Knoware 
*  Called From......:
*  Purpose..........:
*
* 		To display and print a Tycoon Team Appearance record.
*
*............................................................................
*  Revision.........: 1.0 Last Revised: 1/1/1991 at 20:36
*  Description......: Initial Creation.
*............................................................................
***************************** ALL RIGHTS RESERVED ***************************

#INCLUDE "..\lib\PRINT.H"
#INCLUDE "TYCOON.H"

#define  LINE_LENGTH 		80

PROCEDURE TyRcaps

	private cLastWeek
	private printto, workfile, cLeagueName
   private mopt[4], mmess[4], poption1

   clear_scn()

   head_line( "APPEARANCE REPORT" )

   ScreenLine(  1 )
   ScreenLine( 21 )

   if !INIT_CAPS()    && Initialisation routine
      do CLOSE_CAPS
      retu
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

				do GEN_CAPS
            view_prnt( printto, LINE_LENGTH )

         case mopt[poption1] = 'V'

            view_prnt( printto, LINE_LENGTH )

         case mopt[poption1] = 'P'

				print_prnt( printto )

      endcase
   
   enddo 

   do CLOSE_CAPS

return


function INIT_CAPS 

	private mcontinu, tixfile, tixfile2

	plswait()
   std_rpt_opt( @mopt, @mmess, "Tycoon Team Appearances report" )

   mcontinu = .t.
   mcontinu = iif( mcontinu, opendbf( 'TYSKED', G_ty_path   ), .f. )
	mcontinu = iif( mcontinu, opendbf( 'LINEUP', G_ty_path   ), .f. )
	mcontinu = iif( mcontinu, opendbf( 'LEAGUE', G_ty_path   ), .f. )
	mcontinu = iif( mcontinu, opendbf( 'COMP',   G_ty_path   ), .f. )
	mcontinu = iif( mcontinu, opendbf( 'SCORE',  G_nfl_path  ), .f. )
	mcontinu = iif( mcontinu, opendbf( 'PLAYER', G_nfl_path  ), .f. )
	mcontinu = iif( mcontinu, opendbf( 'STAT',   G_nfl_path  ), .f. )

	@ 23,00

	if mcontinu

		OpenCaps()
	else

		error_msg( "Unable to gain access to all the necessary files" )
	endif

return( mcontinu )


FUNCTION OpenCaps

	select STAT
	set order to 2

	*---Open temp file
	workfile = g_ty_path + TEMPORARY_FILE_INDICATOR + "CAPS" 
	if .not. file( workfile + ".dbf")
		makefile( workfile + ".dbf", 5, ;
					"CATEGORY   C  1 0", ;
					"PLAYERID   C  8 0", ;
					"CAPS       N  2 0", ;
					"OFFPTS     N  3 0", ;
					"DEFPTS     N  3 0"  )
	endif

	select 0
	if !net_use( workfile, .t., 5, "WORKFILE" )
		error_msg( "Work file is in use, try again later")
		retu ( .f. )
	endif
	tixfile  = workfile + 'x1.ntx'
	tixfile2 = workfile + 'x2.ntx'
	if .not. file( tixfile )
		index on PLAYERID                     to ( tixfile  )
	endif
	if .not. file( tixfile2 )
		index on CATEGORY + descend( str( CAPS, 2 ) ) to ( tixfile2 )
	endif
	set index to ( tixfile ), ( tixfile2 )

RETURN .t.


procedure CLOSE_CAPS

	close databases

RETURN


function VAL_CAPS

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

		case mvar == "mTeam"

			mTeam = &mvar
			if .not. empty( mTeam )
*				mTeam = space(5)
				ok = keyedtable( @mTeam, "COMP", mseason+mleagueid,;
					 "SEASON+LEAGUEID", "OWNERID","FRANCHISE",;
	 		                "Owner id", "Franchise", 1 )

				store mTeam to &mvar

			endif
	endcase
	setcolor( saveclr )

RETURN( ok )


PROCEDURE Gen_caps

   private i, repoption, mSeason, mLeagueid, mTeam

	select WORKFILE
	zap

	setcolor( c_desc )
	print( 6, 15, "Season :" )
	print( 7, 15, "League :" )
	print( 8, 15, "Team   :" )
	print( 9, 15, "To Wk  :" )

	setcolor( c_fields )
	mSeason   = G_season
	mLeagueid = G_league
	mTeam     = space(5)
	cLastWeek = G_week

	vget( 6, 25, "mSeason" ,   "NNNN",  .f. )
	vget( 7, 25, "mLeagueid" , "!!",    .f. )
	vget( 8, 25, "mTeam",      "XXXXX", .f. )
	vget( 9, 25, "cLastWeek",  "NN",    .f. )

	vread( 1, .f., 'VAL_CAPS' )

	CLEAR_WINDOW( 6, 15, 9, 79 )
	if abort()
		retu
	else
		G_season = mSeason
		G_league = mLeagueid
		G_week   = cLastWeek
	endif

	GenMsg()

	PrnCaps( mSeason, mLeagueID, cLastWeek, mTeam )

RETURN


FUNCTION PrnCaps( mSeason, mLeagueID, cLastWeek, mTeam )

	if empty( mTeam )
		*---Do them all
		select COMP
		dbseek( mSeason + mLeagueid )
		do while .not. eof() .and. SEASON = mSeason .and. LEAGUEID = mLeagueid

			@ 24,70 say OWNERID

			TeamApps( OWNERID, cLastWeek )

			select COMP
			skip

		enddo

	else
		TeamApps( mTeam, cLastWeek )
	endif

   print_release()
   set print to        

	@ 22,00
	clrmenu()
	GenMsgClr()

RETURN nil


FUNCTION TeamApps

	parameters mTeam, cLastWeek

 	***
	***   Load Data
	***
	store 0 to nWins, nLosses, nTies
	select WORKFILE
	zap

	select TYSKED
	dbseek( mSeason + mLeagueid )
	do while .not. eof() .and. SEASON = mseason .and. LEAGUEID = mleagueid
		*---For each week of the season
		mWeek = WEEK
		if TYSKED->HS1 + TYSKED->AS1 + TYSKED->HS2 + TYSKED->AS2 > 0 
			if TeamPlayed( mTeam )
				AwardCaps( mSeason, mLeagueid, mTeam, mWeek )
			else
				testmsg( mTeam + " did not play in week " + TYSKED->WEEK )
			endif
		endif

		select TYSKED
		skip

		if TYSKED->WEEK > cLastWeek
			exit
		endif

	enddo

	private    pcol1, pcol2, pcol3, pcol4, pcol5
	store 0 to pcol1, pcol2, pcol3, pcol4, pcol5

	private    nTotFor, nTotAgin, nPosOff, nPosDef
	store 0 to nTotFor, nTotAgin, nPosOff, nPosDef

   begin sequence

		printto = g_reppath + TEMPORARY_FILE_INDICATOR + mLeagueId + ;
					mTeam + PRINTFILE_EXT

		set print to ( printto )
		set print on

		print_setup( LINE_LENGTH )

		*---Short length	(Landscape)
*		?? p_a3
*		?? chr(27) +"E" + chr(27)+ "&I" +chr(27) + "(0U" + chr(27) + "(s0p12h12v0s0b8T" + chr(27) + "&a12L"
		p_linespp = 66

      print_margin( 6, 6, 6 )

      prntcoldec( 1 )

      *---set up the printing columns
		prntcolnxt( @pcol1,   5 )
		prntcolnxt( @pcol2,   5 )
		prntcolnxt( @pcol3,  25 )
		prntcolnxt( @pcol4,  15 )
		prntcolnxt( @pcol5,  15 )

		println()
		printat( pcol1, mSeason + " - " + Franchise( mSeason, mLeagueId, mTeam ) +;
								" Player Appearances"  )
		println()

		select WORKFILE
		set order to 2
		go top
		cLastCat = "X"
		do while .not. eof()

			if CATEGORY <> cLastCat
				if cLastCat <> "X"
					PosTotals()
					store 0 to nPosOff, nPosDef
				endif
				println()
				printat( pcol1, PlayerCat( CATEGORY, 2 ) )
				cLastCat = CATEGORY
			endif
			printat( pcol2,        str( CAPS,      2  ) )
			printat( pcol3, PlayerName( PLAYERID, .t. ) )
			printat( pcol4, str( OFFPTS, 3 ) + " - " + str( DEFPTS, 3 ) )
			println()

			ADDTO( nTotFor,  OFFPTS )
			ADDTO( nTotAgin, DEFPTS )
			ADDTO( nPosOff,  OFFPTS )
			ADDTO( nPosDef,  DEFPTS )

			skip
		enddo

 		PosTotals()
		println()
		printat( pcol5, str( nTotFor, 3 ) + " - " + str( nTotAgin, 3 ) )
		println()

		print_page()
		
		select WORKFILE
		set order to 1
	end

RETURN( NIL )


function AwardCaps

	parameters mSeason, mLeagueid, mTeam, mWeek

	private i

	select LINEUP
	dbseek( mSeason + mLeagueid + mTeam + mWeek )

	if found()

		for i = 1 to 12

			cSuf = ltrim( str( i, 2 ) )
			mPlayerId = PLAYER&cSuf.

			if FULL( mPlayerId )
				AddCap( mPlayerId )
			endif
		next
	endif

RETURN NIL


function AddCap

	parameters mPlayerId

	private nOffPts, nDefPts

	cScore = ScorePlayer( mLeagueID, mplayerid )

	nOffPts = val( substr( cScore, 1, 3 ) )
	nDefPts = val( substr( cScore, 4, 3 ) )

	select WORKFILE
	dbseek( mPlayerId )
	if eof()
		do ADDRECIT with "Work File"
		replace PLAYERID with mPlayerid
		replace CATEGORY with PlayerPos( mPlayerid )
		replace CAPS     with 1
		replace OFFPTS   with nOffPts
		replace DEFPTS   with nDefPts
	else
		replace CAPS     with CAPS + 1
		replace OFFPTS   with OFFPTS + nOffPts
		replace DEFPTS   with DEFPTS + nDefPts
	endif

	select LINEUP

return( NIL )


FUNCTION PosTotals

	printat( pcol5, str( nPosOff, 3 ) + " - " + str( nPosDef, 3 ) )
	println()

RETURN( NIL )



*EOF:  TYRCAPS.pre

