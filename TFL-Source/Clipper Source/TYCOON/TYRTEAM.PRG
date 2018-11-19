*****************************************************************************
*  Program Name.....: tyrteam.pre 
*  Programmer.......: Steve Colonna 
*  System...........:
*  Module...........:
*  Date.............: 1/1/1991 at 20:36
*  Copyright........: (c) 1991 by Knoware 
*  Called From......:
*  Purpose..........:
*
* 		To display and print a Tycoon Team record for inclusion in final
*		season summary report in Newsletter.
*
*
*............................................................................
*  Revision.........: 1.0 Last Revised: 1/1/1991 at 20:36
*  Description......: Initial Creation.
*............................................................................
***************************** ALL RIGHTS RESERVED ***************************

#INCLUDE "..\lib\PRINT.H"
#INCLUDE "TYCOON.H"

#define  LINE_LENGTH 		80

PROCEDURE TYRTEAM

	private printto, workfile, cLeagueName
   private mopt[4], mmess[4], poption1
	private mHt1, mHt2, mHt3, mHt4, mHt5, mHt6
	private mAt1, mAt2, mAt3, mAt4, mAt5, mAt6

   clear_scn()

   head_line( "TEAM REPORT" )

   ScreenLine(  1 )
   ScreenLine( 21 )

   if !INIT_TEAM()    && Initialisation routine
      do CLOSE_TEAM
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

				do GEN_TEAM
            view_prnt( printto, LINE_LENGTH )

         case mopt[poption1] = 'V'

            view_prnt( printto, LINE_LENGTH )

         case mopt[poption1] = 'P'

				print_prnt( printto )

      endcase
   
   enddo 

   do CLOSE_TEAM

return


FUNCTION Init_team 

	private mcontinu

	plswait()
   std_rpt_opt( @mopt, @mmess, "Tycoon Team report" )

   mcontinu = .t.
   mcontinu = iif( mcontinu, opendbf( 'TYSKED', G_ty_path   ), .f. )
*	mcontinu = iif( mcontinu, opendbf( 'LINEUP', G_ty_path   ), .f. )
	mcontinu = iif( mcontinu, opendbf( 'LEAGUE', G_ty_path   ), .f. )
	mcontinu = iif( mcontinu, opendbf( 'COMP',   G_ty_path   ), .f. )
*	mcontinu = iif( mcontinu, opendbf( 'SCORE',  G_nfl_path  ), .f. )
*	mcontinu = iif( mcontinu, opendbf( 'PLAYER', G_nfl_path  ), .f. )
*	mcontinu = iif( mcontinu, opendbf( 'STAT',   G_nfl_path  ), .f. )

	@ 23,00

	if mcontinu
		*---Open temp file
		OpenTeam()
	else
		error_msg( "Unable to gain access to all the necessary files" )
	endif

RETURN mcontinu


FUNCTION OpenTeam

	workfile = g_ty_path + TEMPORARY_FILE_INDICATOR + "TEAM" 
	if .not. file( workfile + ".dbf")
		makefile( workfile + ".dbf", 8, ;
					"ROUND      C  2 0", ;
					"HOMEAWAY   C  1 0", ;
					"OPPONENT   C 20 0", ;
					"PTSFOR     N  3 0", ;
					"PTSAGIN    N  3 0", ;
					"WINS       N  2 0", ;
					"LOSSES     N  2 0", ;
					"TIES       N  2 0"  )
	endif

	select 0
	if !net_use( workfile, .t., 5, "WORKFILE" )
		error_msg( "Work file is in use, try again later")
		retu ( .f. )
	endif

RETURN .t.


procedure CLOSE_TEAM

	close databases

RETURN


function VAL_TEAM

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
			dbseek( mLeagueid )
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
				printto = g_reppath + TEMPORARY_FILE_INDICATOR + mTeam +;
								 PRINTFILE_EXT


			endif
	endcase
	setcolor( saveclr )

return( ok )


PROCEDURE Gen_team

   private i, repoption, mSeason, mLeagueid, mTeam

	select WORKFILE
	zap

	setcolor( c_desc )
	print( 6, 15, "Season :" )
	print( 7, 15, "League :" )
	print( 8, 15, "Team   :" )

	setcolor( c_fields )
	mSeason   = G_season
	mLeagueid = G_league
	mTeam     = space(5)

	vget( 6, 25, "mSeason" ,   "XXXX", .f. )
	vget( 7, 25, "mLeagueid" , "!!",   .f. )
	vget( 8, 25, "mTeam",      "XXXXX",   .f. )

	vread( 1, .f., 'VAL_TEAM' )

	CLEAR_WINDOW( 6, 15, 8, 79 )
	if abort()
		RETU
	endif

	GenMsg()

	PrnTeam( mSeason, mLeagueID, mTeam )

RETURN


FUNCTION PrnTeam( mSeason, mLeagueID, mTeam )

	if empty( mTeam )
		*---Do them all
		select COMP
		dbseek( mSeason + mLeagueid )
		do while .not. eof() .and. SEASON = mSeason .and. LEAGUEID = mLeagueid

			@ 24,70 say OWNERID

			TeamRep( OWNERID )

			select COMP
			skip

		enddo

	else
		TeamRep( mTeam )
	endif

   print_release()
   set print to        

	@ 22,00
	clrmenu()
	GenMsgClr()

RETURN	


FUNCTION TeamRep

	parameters mTeam

	***
	***   Load Data
	***
	private cHomeAway, cRound, cOpponent, nPtsFor, nPtsAgin, nWins, nLosses
	private nByes, nTies

	store 0 to nWins, nLosses, nTies, nByes
	select WORKFILE
	zap

	select TYSKED
	dbseek( mSeason + mLeagueid )
	do while .not. eof() .and. SEASON = mseason .and. LEAGUEID = mleagueid
		*---For each week of the season

		if TeamPlayed( mTeam )
			AddResult()
		else
			testmsg( mTeam + " did not play in week " + TYSKED->WEEK )
		endif

		if val( TYSKED->WEEK ) = RegularGames( mSeason, mLeagueid )
			store 0 to nWins, nLosses, nTies
		endif

		select TYSKED
		skip
	enddo

	private    pcol1, pcol2, pcol3, pcol4, pcol5, pcol6
	store 0 to pcol1, pcol2, pcol3, pcol4, pcol5, pcol6

	private    nTotFor, nTotAgin, nPOFor, nPOAgin, nPOGames, nPlayed
	store 0 to nTotFor, nTotAgin, nPOFor, nPOAgin, nPOGames, nPlayed

   begin sequence

		printto = g_reppath + TEMPORARY_FILE_INDICATOR + ;
					 substr( mSeason, 3, 2 ) + mTeam + PRINTFILE_EXT
	   set print to ( printto )
		set print on

		print_setup( LINE_LENGTH )

		*---Short length	(Landscape)
*		?? p_a3
*		?? chr(27) +"E" + chr(27)+ chr(38) + "I" +chr(27) + "(0U" + chr(27) + "(s0p12h12v0s0b8T" + chr(27) + "&a12L"
		p_linespp = 66

      print_margin( 6, 6, 6 )

      prntcoldec( 1 )

      *---set up the printing columns
		prntcolnxt( @pcol1,   5 )
		prntcolnxt( @pcol2,   5 )
		prntcolnxt( @pcol3,  23 )
		prntcolnxt( @pcol4,  15 )
		prntcolnxt( @pcol5,  15 )
		prntcolnxt( @pcol6,  10 )

		println()
		printat( pcol1, mSeason + " - " + Franchise( mSeason, mLeagueId, mTeam ) )
		println()
		println()

		select WORKFILE
		go top
		do while .not. eof()

			printat( pcol1, WORKFILE->ROUND     )
			printat( pcol2, HOMEAWAY  )
			printat( pcol3, OPPONENT  )
			if OPPONENT <> "Bye"
				printat( pcol4, str( PTSFOR, 3 ) + " - " + str( PTSAGIN, 3 ) )
				printat( pcol5, WLTRecord( WINS, LOSSES, TIES ) )
				cResult := MatchResult( mSeason, mLeagueId, ;
						iif( HOMEAWAY = "H", .t., .f. ), PTSFOR, PTSAGIN )
				printat( pcol6, cResult )
				ADDTO( nTotFor,  PTSFOR  )
				ADDTO( nTotAgin, PTSAGIN )
				if cResult <> "-"
					nPlayed++
				endif
			else
				INC( nByes )
			endif
			println()

			if val( WORKFILE->ROUND ) = RegularGames( mSeason, mLeagueid )

				PrintRecord( nTotFor, nTotAgin, nPlayed )

			elseif val( WORKFILE->ROUND ) > RegularGames( mSeason, mLeagueid )

				ADDTO( nPOFor,  PTSFOR  )
				ADDTO( nPOAgin, PTSAGIN )
				INC( nPOGames )
			endif

			skip
		enddo

		skip -1
		if val( WORKFILE->ROUND ) > RegularGames( mSeason, mLeagueid )
			PrintRecord( nPOFor, nPOAgin, nPOGames )
		endif

		print_page()
		
	end

RETURN( NIL )


FUNCTION AddResult

	select WORKFILE
	do ADDRECIT with "Work File"
	replace WORKFILE->ROUND     with cRound
	replace WORKFILE->HOMEAWAY  with cHomeAway
	replace WORKFILE->OPPONENT  with cOpponent
	replace WORKFILE->PTSFOR    with nPtsFor
	replace WORKFILE->PTSAGIN   with nPtsAgin
	replace WORKFILE->WINS      with nWins
	replace WORKFILE->LOSSES    with nLosses
	replace WORKFILE->TIES      with nTies

RETURN nil


FUNCTION TeamPlayed

	parameters mTeam

	private cSub, i, lPlayed, cResult, cHomeTeam, cAwayTeam

	lPlayed = .f.
	for i = 1 to 6
		cSub = str( i, 1 )
		cHomeTeam := "TYSKED->HT" + cSub
		cAwayTeam := "TYSKED->AT" + cSub

		if &cHomeTeam = mTeam
			lPlayed = .t.
			cHomeAway = "H"
			cRound    = TYSKED->WEEK
			if empty( &cAwayTeam )
				cOpponent = "Bye"
			else
				cOpponent = Franchise( mSeason, mLeagueId, &cAwayTeam )
			endif

			nPtsFor   = TYSKED->HS&cSub.
			nPtsAgin  = TYSKED->AS&cSub.

			if cOpponent <> "Bye"
				cResult = MatchResult( mSeason, mLeagueId,;
						 iif( cHomeaway = "H", .t., .f. ), nPTSFOR, nPTSAGIN )
				if cResult = "W"
					INC( nWins )
				elseif cResult = "L"
					INC( nLosses )
				elseif cResult = "T"
					INC( nTies )
				endif
			endif
			exit

		else

			if &cAwayTeam = mTeam
				lPlayed = .t.
				cHomeAway = "A"
				cRound    = TYSKED->WEEK
				if empty( &cHomeTeam )
					cOpponent = "Bye"
				else
					cOpponent = Franchise( mSeason, mLeagueId, &cHomeTeam )
				endif

				nPtsFor   = TYSKED->AS&cSub.
				nPtsAgin  = TYSKED->HS&cSub.
				if cOpponent <> "Bye"
					cResult = MatchResult( mSeason, mLeagueId, ;
							iif( cHomeaway = "H", .t., .f. ), nPTSFOR, nPTSAGIN )

					if cResult = "W"
						INC( nWins )
					elseif cResult = "L"
						INC( nLosses )
					elseif cResult = "T"
						INC( nTies )
					endif
				endif
				exit

			endif
		endif
	next

RETURN lPlayed


FUNCTION MatchResult

	parameters mSeason, mLeagueId, lHome, nPtsfor, nPtsagin 

	local cResult

	if nPtsfor + nPtsagin = 0
		*---No result
		cResult := "-"
	else
		do case
			case mSeason = "1990"

				if lHome
					if nPtsfor >= nPtsAgin
						cResult = "W"
					else
						cResult = "L"
					endif
				else
					if nPtsfor > nPtsAgin
						cResult = "W"
					else
						cResult = "L"
					endif
				endif

			otherwise

				if nPtsfor > nPtsAgin
					cResult = "W"
				elseif nPtsFor = nPtsAgin
					cResult = "T"
				else
					cResult = "L"
				endif

		endcase
	endif
RETURN cResult


function WLTRecord

	parameters nWin, nLoss, nTie

	private cRecord

	cRecord = "( " + str( nWin, 2 ) + " - " + str( nLoss,  2 )

	if FULL( nTie )

		cRecord = cRecord + " - " + str( nTie, 2 ) + " )"

	else
		cRecord = cRecord  + " )"

	endif

return cRecord


FUNCTION RegularGames

	parameters mSeason, mLeagueid 

	private nGames

*	do case
*	case mSeason = "1990" .or. mSeason = "1991"

		do case
		case mLeagueid = "OL"

			nGames = 14

		case mLeagueid = "CL"

			nGames = 15

		case mLeagueid = "ME"

			nGames = 14

		endcase

*	endcase

RETURN nGames


FUNCTION PrintRecord

	parameters nFor, nAgin, nGames

	private cTotString

	println()

	if nGames <= RegularGames( mSeason, mLeagueid ) - nByes
		cTotString = "Regular season"
	else
		cTotString = "Playoffs"
	endif

	printat( pcol3, cTotString  )
	printat( pcol4, str( nFor, 3 ) + " - " + str( nAgin, 3 ) )
	printat( pcol5, "Avg: " + str( nFor / nGames, 5, 1 ) )
	println()
	println()

RETURN nil



*EOF:  TYRTEAM.PRE

