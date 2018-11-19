*****************************************************************************
*  Program Name.....: TYRHEAD.PRG
*  Programmer.......: SC 
*  System...........: TFL
*  Module...........: TYCOON
*  Date.............: 28/12/93 @ 13:19:22
*  Compile/link.....: 
*                     
*  Purpose..........:	Display hEAD TO hEAD
*
*
*.............................................................................

#INCLUDE "..\lib\PRINT.H"
#INCLUDE "TYCOON.H"

#define  LINE_LENGTH 		96

PROCEDURE TyRHEAD

	private printto, workfile, cLeagueName, lFull
   private mopt[4], mmess[4], poption1
	private mHt1, mHt2, mHt3, mHt4, mHt5, mHt6
	private mAt1, mAt2, mAt3, mAt4, mAt5, mAt6

   clear_scn()

   head_line( "HEAD TO HEAD" )

   ScreenLine(  1 )
   ScreenLine( 21 )

   if !INIT_HEAD()    && Initialisation routine
      do CLOSE_HEAD
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

				do GEN_HEAD
            view_prnt( printto, LINE_LENGTH )

         case mopt[poption1] = 'V'

            view_prnt( printto, LINE_LENGTH )

         case mopt[poption1] = 'P'

				print_prnt( printto )

      endcase
   
   enddo 

   do CLOSE_HEAD

return


FUNCTION Init_HEAD 

	private mcontinu

	plswait()
   std_rpt_opt( @mopt, @mmess, "Tycoon HEAD 2 HEAD" )

   mcontinu = .t.
   mcontinu = iif( mcontinu, opendbf( 'TYSKED', G_ty_path   ), .f. )
   mcontinu = iif( mcontinu, opendbf( 'LINEUP', G_ty_path   ), .f. )
   mcontinu = iif( mcontinu, opendbf( 'LEAGUE', G_ty_path   ), .f. )
   mcontinu = iif( mcontinu, opendbf( 'OWNERS', G_ty_path   ), .f. )
   mcontinu = iif( mcontinu, opendbf( 'COMP',   G_ty_path   ), .f. )
*  mcontinu = iif( mcontinu, opendbf( 'SCORE',  G_nfl_path  ), .f. )
*  mcontinu = iif( mcontinu, opendbf( 'PLAYER', G_nfl_path  ), .f. )
*	mcontinu = iif( mcontinu, opendbf( 'STAT',   G_nfl_path  ), .f. )

	@ 23,00

	if mcontinu
		*---Open temp file
		OpenHead()
	else
		error_msg( "Unable to gain access to all the necessary files" )
	endif

RETURN mcontinu

FUNCTION OpenHead

	local workfile

	workfile = g_ty_path + TEMPORARY_FILE_INDICATOR + "HEAD" 
	if .not. file( workfile + ".dbf")
		makefile( workfile + ".dbf", 8, ;
					"OWNER      C  5 0", ;
					"OPPONENT   C  5 0", ;
					"WINS       N  3 0", ;
					"LOSSES     N  3 0", ;
					"TIES       N  3 0", ;
					"PTSFOR     N  4 0", ;
					"AGAINST    N  4 0", ;
					"POINTS     N  5 0"  )
	endif

	select 0
	if .not. net_use( workfile, .t., 5, "WORKFILE" )
		error_msg( "Work file is in use, try again later")
		retu ( .f. )
	endif
	tixfile = workfile + '.ntx'
	if .not. file( tixfile)
		index on OWNER+OPPONENT to ( tixfile  )
	endif
	tixfile2 = workfile + '2.ntx'
	index on OWNER +  str( 99999 - ( WINS + LOSSES + TIES ), 5 ) to ( tixfile2  )

	set index to ( tixfile ), ( tixfile2 )

	select WORKFILE
	zap
	set order to 1

RETURN .t.

PROCEDURE Close_HEAD

	close databases

RETURN


FUNCTION Val_HEAD

	parameters mvar

	local valonly, ok, saveclr, lk

	saveclr = setcolor()
	if lastkey() = UPARROW
		retu( .t. )
	endif

	ok = .t.
	do case

		case mvar == "mLeagueid"

			select LEAGUE
			seek mLeagueid
			if found()
				cLeagueName = LEAGUE
			else
				ok = .f.
				error_msg( "League " + mLeagueid + " not found" )
			endif

		case mvar = 'cTeam'

			*---Pop up options
			if .not. empty( cTeam )
				lOk = keyedtable( @cTeam, "COMP", g_season+mleagueid,;
					 "SEASON+LEAGUEID", "OWNERID","FRANCHISE",;
	 		                "Owner id", "Franchise", 1 )

			endif

	endcase
	setcolor( saveclr )

RETURN( ok )


procedure GEN_HEAD

	local nWins, nLosses, nTies, nPtsFor, nAgainst, nPoints
	private i, repoption, cSeasonFr, mLeagueid, cSeasonTo, cTeam

	setcolor( c_desc )
	print( 6, 15, "Season :        to" )
	print( 7, 15, "League :" )
	print( 8, 15, "Owner  :" )
	print( 9, 15, "Full   :" )

	setcolor( c_fields )
	cSeasonFr := "1988"
	cSeasonTo := G_season
	mLeagueid := G_league
	cTeam		 := space(5)
	lFull 	 := .t.

	vget( 6, 25, "cSeasonFr" ,   "XXXX", .f. )
	vget( 6, 35, "cSeasonTo" ,   "XXXX", .f. )
	vget( 7, 25, "mLeagueid" , "!!",   .f. )
	vget( 8, 25, "cTeam",      "XXXXX",.f. )
	vget( 9, 25, "lFull",      "Y",.f. )

	vread( 1, .f., 'VAL_HEAD' )

	CLEAR_WINDOW( 6, 15, 9, 79 )
	if abort()
		RETU
	else
		G_league = mLeagueId
		printto  = HeadPrn()

	endif

	GenMsg()

	PrnHead( cSeasonFr, cSeasonTo, mLeagueID, cTeam, lFull, .t., printto )

RETURN

FUNCTION HeadPrn
RETURN g_reppath + TEMPORARY_FILE_INDICATOR + mLeagueId +;
						"HEAD" + PRINTFILE_EXT

FUNCTION PrnHead( cSeasonFr, cSeasonTo, mLeagueID, cTeam, lFull, lAll, printto )

	*---Generate data
	select TYSKED
	for nYear = val( cSeasonFr ) to val( cSeasonTo )
		seek str( nyear, 4 ) + mLeagueId
		do while .not. TYSKED->( eof() ) .and. ;
							TYSKED->SEASON = str( nYear, 4 ) .and. ;
							TYSKED->LEAGUEID = mLeagueId
			if TYSKED->HS1 + TYSKED->HS2 + TYSKED->HS3 + TYSKED->HS4 + ;
				TYSKED->AS1 + TYSKED->AS2 + TYSKED->AS3 + TYSKED->AS4 > 0
				do LoadRound
			endif

			select TYSKED
			skip
		enddo
	next

	private    pcol1, pcol2, pcol3, pcol4, pcol5, pcol6, pcol7, pcol8, pcol9
	store 0 to pcol1, pcol2, pcol3, pcol4, pcol5, pcol6, pcol7, pcol8, pcol9

   begin sequence

      set print to ( printto )
		set print on

		print_setup( LINE_LENGTH )

		*---Short length	(Landscape)
*		?? p_a3
*		?? chr(27) +"E" + chr(27)+ chr(38) + "I" +chr(27) + "(0U" + chr(27) + "(s0p12h12v0s0b8T" + chr(27) + "&a12L"
		p_linespp = 66

      print_margin( 0, 0, 0 )

      prntcoldec( 1 )

      *---set up the printing columns
		prntcolnxt( @pcol1,  18 )
		prntcolnxt( @pcol2,   7 )
		prntcolnxt( @pcol3,   6 )
		prntcolnxt( @pcol4,   6 )
		prntcolnxt( @pcol5,   7 )
		prntcolnxt( @pcol6,   7 )
		prntcolnxt( @pcol7,   6 )
		prntcolnxt( @pcol8,   6 )
		prntcolnxt( @pcol9,   6 )

		select WORKFILE
		set order to 2
		go top
		cLastTeam = "XXXX"
		do while .not. WORKFILE->( eof() )

			if WORKFILE->OWNER <> cLastTeam

				if CurrTeam( WORKFILE->OWNER ) .or. lAll
					*---Can print
					if lFull
						PrintTit()
					endif
				endif

				cLastTeam = WORKFILE->OWNER
				store 0 to nWins, nLosses, nTies, nPtsFor, nAgainst, nPoints

			endif

			if CurrTeam( WORKFILE->OWNER ) .or. lAll
				if lFull
					printat( pcol1, LastTeam( WORKFILE->OPPONENT )  )
					printat( pcol2, str( WORKFILE->WINS + WORKFILE->LOSSES + WORKFILE->TIES, 4  ) )
					printat( pcol3, str( WORKFILE->WINS, 4 ) )
					printat( pcol4, str( WORKFILE->LOSSES, 4 ) )
					printat( pcol5, str( WORKFILE->TIES, 4 ) )
					printat( pcol6, str( Clip( WORKFILE->WINS, WORKFILE->LOSSES, ;
									WORKFILE->TIES ), 5, 3 ) )
					printat( pcol7, str( WORKFILE->PTSFOR, 4 ) )
					printat( pcol8, str( WORKFILE->AGAINST, 4 ) )
					printat( pcol9, str( WORKFILE->POINTS, 4 ) )
					println()
				endif

				nWins += WORKFILE->WINS
				nLosses += WORKFILE->LOSSES
				nTies += WORKFILE->TIES
				nPtsFor += WORKFILE->PTSFOR
				nAgainst += WORKFILE->AGAINST
				nPoints += WORKFILE->POINTS

			endif
			select WORKFILE
			skip

			if WORKFILE->OWNER <> cLastTeam
				if CurrTeam( cLastTeam ) .or. lAll
					*---Print totals
					if lFull
						println()
					endif
					printat( pcol1, LastTeam( cLastTeam ) )
					printat( pcol2, str( nWINS + nLOSSES + nTIES, 4  ) )
					printat( pcol3, str( nWINS, 4 ) )
					printat( pcol4, str( nLOSSES, 4 ) )
					printat( pcol5, str( nTIES, 4 ) )
					printat( pcol6, str( Clip( nWINS, nLOSSES, nTIES ), 5, 3 ) )
					printat( pcol7, str( nPTSFOR, 4 ) )
					printat( pcol8, str( nAGAINST, 4 ) )
					printat( pcol9, str( nPOINTS, 4 ) )
					println()

					if lFull
						Streaks( cLastTeam )
					endif

				endif
			endif
		enddo

		print_page()

	end

	print_release()
	set print to

	@ 22,00
	clrmenu()
	GenMsgClr()

RETURN

static ;
FUNCTION PrintTit
	println()
	printat( pcol1,  cSeasonFr + " to " + cSeasonTo )
	printat( pcol3,  "HEAD TO HEAD" )
	println()
	println()

	if	OWNERS->( dbseek( WORKFILE->OWNER ) )
		printat( pcol1,  "Owner: " + trim( OWNERS->FIRSTNAME ) + " " + OWNERS->LASTNAME  )
		printat( pcol3, LastTeam( WORKFILE->OWNER ) )
		println()
		println()
	endif
	printat( pcol1, "OPPONENT" )
	printat( pcol2, "GAMES" )
	printat( pcol3, "WINS" )
	printat( pcol4, "LOSSES" )
	printat( pcol5, "TIES" )
	printat( pcol6, "CLIP" )
	printat( pcol7, "FOR" )
	printat( pcol8, "AGIN")
	printat( pcol9, "FPS")
	println()

RETURN nil

STATIC ;
FUNCTION Streaks( cOwnerID )

	private nYear, nHighScore, cHighOpp, cHighYear, cHighWeek, nWins, nLosses
	private cWinEnd, cLossEnd, nWinStreak, nLossStreak
	nWinStreak := nLossStreak := nWins := nLosses := nHighScore := 0
	store "" to cHighOpp, cHighYear, cHighWeek, cWinEnd, cLossEnd

	select TYSKED
	for nYear = val( cSeasonFr ) to val( cSeasonTo )
		seek str( nyear, 4 ) + mLeagueId
		do while .not. TYSKED->( eof() ) .and. ;
							TYSKED->SEASON = str( nYear, 4 ) .and. ;
							TYSKED->LEAGUEID = mLeagueId

			for s = 1 to 6
				cS = str(s,1)
				if HT&cS = cOwnerID
					*---Home game
					if THS&cS > nHighScore
						nHighScore := THS&cS
						cHighOpp   := AT&cs
						cHighYear  := TYSKED->SEASON
						cHighWeek  := TYSKED->WEEK + "  " + ;
												str( HS&cS, 3 ) + " -" + ;
												str( AS&cS, 3 )
					endif

					if HS&cS > AS&cS
						*---Home win
						do BreakLoss

						nLosses			:= 0
						nWins++
					else
						if HS&cS = AS&cS
							*---Tie
							do BreakLoss
							do BreakWin
							nWins			:= 0
							nLosses		:= 0
						else
							*---Home loss
							do BreakWin

							nWins			:= 0
							nLosses++

						endif
					endif
				else
					if AT&cS = cOwnerID
						*---Away game
						if TAS&cS > nHighScore
							nHighScore := TAS&cS
							cHighOpp   := HT&cs
							cHighYear  := TYSKED->SEASON
							cHighWeek  := TYSKED->WEEK + "  " + ;
												str( AS&cS, 3 ) + " -" + ;
												str( HS&cS, 3 )
						endif
						if AS&cS > HS&cS
							*---Away win
							do BreakLoss

							nLosses			:= 0
							nWins++
						else
							if HS&cS = AS&cS
								*---Tie
								do BreakLoss
								do BreakWin
								nWins			:= 0
								nLosses		:= 0
							else
								*---Away loss
								do BreakWin
								nWins			:= 0
								nLosses++
							endif
						endif

					endif

				endif
			next

			select TYSKED
			skip
		enddo
	next

	do BreakLoss
	do BreakWin

	println()
	printat( pcol2, "High score  " + str( nHighScore, 3 ) + " vs " +;
						 trim( LastTeam( cHighOpp ) ) + "  " + cHighYear + ;
						 " week " + cHighWeek )
	println()
	printat( pcol2, "longest winning streak  " + str( nWinStreak, 3 ) + ;
						 " ended " + cWinEnd )
	println()
	printat( pcol2, "longest loosing streak  " + str( nLossStreak, 3 ) + ;
						 " ended " + cLossEnd )
	println()
	println()
	
RETURN nil

STATIC ;
PROCEDURE  BreakLoss

	if nLosses > nLossStreak
		*---End of lossing streak
		nLossStreak 	:= nLosses
		cLossEnd			:= TYSKED->SEASON + " week " + TYSKED->WEEK
	endif

RETURN


STATIC ;
PROCEDURE  BreakWin

	if nWins > nWinStreak
		*---end of win streak
		nWinStreak 	:= nWins
		cWinend    	:= TYSKED->SEASON + " week " + TYSKED->WEEK
	endif

RETURN

STATIC ;
FUNCTION LastTeam( cOwnerID )

	local cTeam, i, nYear

	for nYear = val( cSeasonTo ) to val( cSeasonFr ) step -1
		cTeam =	Franchise( str(nYear,4), mLeagueId, cOwnerID )
		if .not. empty( cTeam )
			if G_season = str( nYear, 4 )
				*---Currently active
				cTeam = upper( cTeam )
			endif
			exit
		endif
	next

RETURN cTeam


STATIC ;
FUNCTION CurrTeam( cOwnerID )

	local cTeam, i, nYear, lCurr
	lCurr := .f.

	for nYear = val( cSeasonTo ) to val( cSeasonFr ) step -1
		cTeam =	Franchise( str(nYear,4), mLeagueId, cOwnerID )
		if .not. empty( cTeam )
			if G_season = str( nYear, 4 )
				*---Currently active
				lCurr := .t.
			endif
			exit
		endif
	next

RETURN lCurr


STATIC ;
PROCEDURE LoadRound

	local s

	for s = 1 to 6
		cS = str(s,1)
		if .not. empty( HT&cS )
			if HS&cS > AS&cS
				WinFor( HT&cS, AT&cS, HS&cS, AS&cS, THS&cS )
				LossFor( AT&cS, HT&cS, AS&cS, HS&cS, TAS&cS )
			else
				if HS&cS = AS&cS
					TieFor( HT&cS, AT&cS, HS&cS, AS&cS, THS&cS )
					TieFor( AT&cS, HT&cS, AS&cS, HS&cS, TAS&cS )
				else
					WinFor( AT&cS, HT&cS, AS&cS, HS&cS, TAS&cS )
					LossFor( HT&cS, AT&cS, HS&cS, AS&cS, THS&cS )
				endif
			endif
		endif
	next

RETURN


STATIC ;
FUNCTION WinFor( cWinner, cLooser, nWinScore, nLossScore, nPoints )

	if cWinner = trim( cTeam )
		select WORKFILE
		if .not. dbseek( cWinner + cLooser )
			append blank
			replace WORKFILE->OWNER		with cWinner
			replace WORKFILE->OPPONENT	with cLooser
		endif

		replace WORKFILE->WINS		with WORKFILE->WINS + 1
		replace WORKFILE->PTSFOR	with WORKFILE->PTSFOR + nWinScore
		replace WORKFILE->AGAINST	with WORKFILE->AGAINST + nLossScore
		replace WORKFILE->POINTS	with WORKFILE->POINTS + nPoints

	endif

	select TYSKED

RETURN nil
		
STATIC ;
FUNCTION LossFor( cLooser, cWinner, nLossScore, nWinScore, nPoints )

	if cLooser = trim( cTeam )
		select WORKFILE
		if .not. dbseek( cLooser + cWinner )
			append blank
			replace WORKFILE->OWNER		with cLooser
			replace WORKFILE->OPPONENT	with cWinner
		endif

		replace WORKFILE->LOSSES	with WORKFILE->LOSSES + 1
		replace WORKFILE->PTSFOR	with WORKFILE->PTSFOR + nLossScore
		replace WORKFILE->AGAINST	with WORKFILE->AGAINST + nWinScore
		replace WORKFILE->POINTS	with WORKFILE->POINTS + nPoints
	endif

	select TYSKED

RETURN nil


STATIC ;
FUNCTION TieFor( cWinner, cLooser, nWinScore, nLossScore, nPoints )

	if cWinner = trim( cTeam )
		select WORKFILE
		if .not. dbseek( cWinner + cLooser )
			append blank
			replace WORKFILE->OWNER		with cWinner
			replace WORKFILE->OPPONENT	with cLooser
		endif

		replace WORKFILE->TIES		with WORKFILE->TIES + 1
		replace WORKFILE->PTSFOR	with WORKFILE->PTSFOR + nWinScore
		replace WORKFILE->AGAINST	with WORKFILE->AGAINST + nLossScore
		replace WORKFILE->POINTS	with WORKFILE->POINTS + nPoints

	endif

	select TYSKED

RETURN nil


*EOF:  TYRHEAD.PRE

