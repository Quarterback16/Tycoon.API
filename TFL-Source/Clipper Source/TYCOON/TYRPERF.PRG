*****************************************************************************
*  Program Name.....: tyrperf
*  Programmer.......: Steve Colonna 
*  System...........:
*  Module...........:
*  Date.............: 17/9/1990 at 18:19
*  Copyright........: (c) 1990 by Knoware 
*  Called From......:
*  Purpose..........:
*
*	  Prints out the rosters
*
*............................................................................
*  Revision.........: 1.0 Last Revised: 24/9/1990 at 21:21
*  Description......: Initial Creation.
*............................................................................
***************************** ALL RIGHTS RESERVED ***************************

#INCLUDE "..\lib\PRINT.H"
#INCLUDE "TYCOON.H"

#define  LINE_LENGTH 		130
#define  ROSTER_MAXIMUM		26
#define  LINES_ON_PAGE		45

PROCEDURE tyrperf

	private workfile, cLeagueName, printto
   private mopt[4], mmess[4], poption1

   clear_scn()

   head_line( "PERFORMANCE REPORT" )

   ScreenLine(  1 )
   ScreenLine( 21 )

   if !INIT_perf()    && Initialisation routine
      do CLOSE_perf
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

				do GEN_perf
				if lastkey() <> 27
					view_prnt( printto, LINE_LENGTH )
				endif

         case mopt[poption1] = 'V'

            view_prnt( printto, LINE_LENGTH )

         case mopt[poption1] = 'P'

				print_prnt( printto )

      endcase
   
   enddo 

   do CLOSE_perf

return


function INIT_perf 

	private mcontinu

	plswait()
   std_rpt_opt( @mopt, @mmess, "Tycoon Matchups" )

   mcontinu = .t.
*  mcontinu = iif( mcontinu, opendbf( 'TYSKED', G_ty_path   ), .f. )
   mcontinu = iif( mcontinu, opendbf( 'LINEUP', G_ty_path   ), .f. )
   mcontinu = iif( mcontinu, opendbf( 'LEAGUE', G_ty_path   ), .f. )
   mcontinu = iif( mcontinu, opendbf( 'COMP',   G_ty_path   ), .f. )
   mcontinu = iif( mcontinu, opendbf( 'OWNERS', G_ty_path   ), .f. )
   mcontinu = iif( mcontinu, opendbf( 'SCHED',  G_nfl_path  ), .f. )
   mcontinu = iif( mcontinu, opendbf( 'SCORE',  G_nfl_path  ), .f. )
   mcontinu = iif( mcontinu, opendbf( 'STAT',   G_nfl_path  ), .f. )
   mcontinu = iif( mcontinu, opendbf( 'TEAM',   G_nfl_path  ), .f. )
   mcontinu = iif( mcontinu, opendbf( 'PLAYER', G_nfl_path  ), .f. )
   mcontinu = iif( mcontinu, opendbf( 'SERVE',  G_nfl_path  ), .f. )

	@ 23,00

	select STAT
	set order to 3

	if ! mcontinu
		error_msg( "Unable to gain access to all the necessary files" )
	endif

RETURN mcontinu


procedure CLOSE_perf

	close databases

RETURN


FUNCTION Val_Perf

	parameter mvar

	private lOk
	lOk = .t.

	do case
		case mvar == 'mLeagueid'

			*---Pop up options
	 		lOk = poptable2( @mleagueid, "LEAGUE", "LEAGUEID", "LEAGUE","ID",;
	 		                "   League", "C", 1, .t., 30, .f. )
			cLeagueName = LEAGUE->LEAGUE

		case mvar = 'cTeam'

			*---Pop up options
			if .not. empty( cTeam )
				lOk = keyedtable( @cTeam, "COMP", mseason+mleagueid,;
					 "SEASON+LEAGUEID", "OWNERID","FRANCHISE",;
	 		                "Owner id", "Franchise", 1 )

			endif

	endcase

RETURN lOk


procedure GEN_perf

   private i, repoption, mSeason, mLeagueid, mWeek, lAll, cStartWk, cTeam
	private cRosterYr

	*-------------------------------------------------------------------
	*	By using a different year in Roster you can see how your current
	*  roster went last year
	*		eg  Roster = "1994", Season = "1993"
	*------------------------------------------------------------------
	setcolor( c_desc )
	print(  5, 15, "Roster :" )
	print(  6, 15, "Season :" )
	print(  7, 15, "League :" )
	print(  8, 15, "Week   :" )
	print(  9, 15, "Team   :" )
	print( 10, 15, "all wks:" )
	print( 11, 15, "Anon   :" )

	setcolor( c_fields )
	cRosterYr		:= G_season
	mSeason   	:= G_season
	mLeagueid 	:= G_league
	mWeek     	:= G_week
	cTeam			:= space(5)
	lAll			:= .t.
   lAnon       := .t.

	vget(  5, 25, "cRosterYr" ,   "XXXX", .f. )
	vget(  6, 25, "mSeason" ,   "XXXX", .f. )
	vget(  7, 25, "mLeagueid" , "!!",   .f. )
	vget(  8, 25, "mWeek",      "XX",   .f. )
	vget(  9, 25, "cTeam",      "!!!!!",.f. )
	vget( 10, 25, "lAll",       "Y",   .f. )
	vget( 11, 25, "lAnon",       "Y",   .f. )

	vread( 1, .f., 'VAL_perf' )

	CLEAR_WINDOW( 5, 15, 12, 79 )
	if abort()
		RETU
	else
		G_week   := mWeek
		G_league := mLeagueId
		cStartWk := mWeek
	endif

	GenMsg()

	PrnPerf( cRosterYr, mSeason, mLeagueID, mWeek, cStartWk, cTeam, lAll, lAnon )

RETURN


FUNCTION PrnPerf( cRosterYr, mSeason, mLeagueID, mWeek, cStartWk, cTeam, lAll, lAnon )

	***
	***   Load Data
	***
	cRound = mSeason + mLeagueid + mweek

	nTeams = TeamsIn( cRosterYr, mLeagueId )
	nGames = int( ( nTeams / 2 ) + .5 )
	if .not. empty( cTeam )
		nTeams := 1
	endif

	private franchise_[ nTeams ]
	private PlyrA_[ nTeams ]
	private PlyrB_[ nTeams ]
	private PlyrC_[ nTeams ]
	private PlyrD_[ nTeams ]
	private PlyrE_[ nTeams ]
	private PlyrF_[ nTeams ]
	private PlyrG_[ nTeams ]
	private PlyrH_[ nTeams ]
	private PlyrI_[ nTeams ]
	private PlyrJ_[ nTeams ]
	private PlyrK_[ nTeams ]
	private PlyrL_[ nTeams ]
	private PlyrM_[ nTeams ]
	private PlyrN_[ nTeams ]
	private PlyrO_[ nTeams ]
	private PlyrP_[ nTeams ]
	private PlyrQ_[ nTeams ]
	private PlyrR_[ nTeams ]
	private PlyrS_[ nTeams ]
	private PlyrT_[ nTeams ]
	private PlyrU_[ nTeams ]
	private PlyrV_[ nTeams ]
	private PlyrW_[ nTeams ]
	private PlyrX_[ nTeams ]
	private PlyrY_[ nTeams ]
	private PlyrZ_[ nTeams ]

	fran_cnt = 0

	select COMP
	COMP->( dbseek( cRosterYr + mLeagueid ) )
	do while .not. eof() .and. SEASON = cRosterYr .and. LEAGUEID = mleagueid
		if COMP->OWNERID = trim( cTeam )
			INC( fran_cnt )
			franchise_[ fran_cnt ] = OWNERID
			plyrA_[ fran_cnt ]     = PLAYERA
			plyrB_[ fran_cnt ]     = PLAYERB
			plyrC_[ fran_cnt ]     = PLAYERC
			plyrD_[ fran_cnt ]     = PLAYERD
			plyrE_[ fran_cnt ]     = PLAYERE
			plyrF_[ fran_cnt ]     = PLAYERF
			plyrG_[ fran_cnt ]     = PLAYERG
			plyrH_[ fran_cnt ]     = PLAYERH
			plyrI_[ fran_cnt ]     = PLAYERI
			plyrJ_[ fran_cnt ]     = PLAYERJ
			plyrK_[ fran_cnt ]     = PLAYERK
			plyrL_[ fran_cnt ]     = PLAYERL
			plyrM_[ fran_cnt ]     = PLAYERM
			plyrN_[ fran_cnt ]     = PLAYERN
			plyrO_[ fran_cnt ]     = PLAYERO
			plyrP_[ fran_cnt ]     = PLAYERP
			plyrQ_[ fran_cnt ]     = PLAYERQ
			plyrR_[ fran_cnt ]     = PLAYERR
			plyrS_[ fran_cnt ]     = PLAYERS
			plyrT_[ fran_cnt ]     = PLAYERT
			plyrU_[ fran_cnt ]     = PLAYERU
			plyrV_[ fran_cnt ]     = PLAYERV
			plyrW_[ fran_cnt ]     = PLAYERW
			plyrX_[ fran_cnt ]     = PLAYERX
			plyrY_[ fran_cnt ]     = PLAYERY
			plyrZ_[ fran_cnt ]     = PLAYERZ
		endif
		skip 
	enddo

	private    pcol1, pcol2, pcol3, pcol4, pcol5, pcol6
	store 0 to pcol1, pcol2, pcol3, pcol4, pcol5, pcol6

   begin sequence

		*---Using pre printed Stationery

		print_setup( LINE_LENGTH, .t. )

*		?? p_a3
*		?? chr(27) + chr(38) + "l1O"			&&		HP Laser

		p_linespp = ( val( iif( lAll, "17", mWeek ) ) * LINES_ON_PAGE ) + 10

		*---Page mode
		prntpagebuf()

      print_margin( 0, 0, 0 )

      prntcoldec( 1 )

      *---set up the printing columns
		prntcolnxt( @pcol1,  05 )
		prntcolnxt( @pcol2,  25 )
		prntcolnxt( @pcol3,  30 )
		prntcolnxt( @pcol4,  30 )
		prntcolnxt( @pcol5,  30 )
		prntcolnxt( @pcol6,  30 )

		ListPlayer()

	end

   print_release()
   set print to        

	@ 22,00
	clrmenu()
	GenMsgClr()

RETURN


static FUNCTION ListPlayer

	private rs, cChar, cCol, cPlyr, cPlayer, cLastCat, nRow, cTitle

	For t = 1 to nTeams

		cLastCat := "X"
		cTitle	:= upper( Franchise( mSeason, mLeagueid, franchise_[t] ) )
      if .not. lAnon
	   	cTitle	+= "   Owner : " + Owner( franchise_[t] )
   		cTitle	+= "   ph : " + OWNERS->HPHONENO
      endif

		printto = g_reppath + alltrim( COMP->FILENAME ) + PRINTFILE_EXT

      set print to ( printto )
		set print on

		printat( 1, pcol1,  mSeason + " - " + cLeagueName + " Week " + mweek )
		printat( 1, pcol3, "PERFORMANCE REPORT"                              )

		printat( 3, pcol1, cTitle )

		nRow := 5
		For rs = 1 to ROSTER_MAXIMUM
			cChar = chr( 64 + rs )

			nRow++
			cPlyr 		:= "plyr" + cChar + "_[ " + ltrim( str( t, 2 ) ) + " ]"
			mplayerid 	:= &cPlyr
			cPlayer 		:= PlayerName( mplayerid )
			if IsStarter( mPlayerid, mWeek, franchise_[ t ] )
				cPlayer 	:= upper( cPlayer )
			endif

			if .not. empty( mPlayerid )

				if PLAYER->CATEGORY <> cLastCat
					nRow++
					HeaderFor( PLAYER->CATEGORY )
					cLastCat := PLAYER->CATEGORY
				endif

				PrintPlyr( mPlayerid )

			endif

		next

		*---One Page per team
		print_page()
		
	   set print to        
*		*---Copy output to REPORT directory
*		copy file ( printto ) to ( REPORTDIR + alltrim( COMP->FILENAME ) + ".txt" )

	next

RETURN NIL


static FUNCTION PrintPlyr( cPlayerID )

	local w, cWeek, cStatWeek
	cWeek := cStartWk

	printat( nRow,  pcol2, cPlayer )
	printat( nRow,  pcol2+20, CurrTeam( cPlayerid, date() ) )

	if lAll
		for w = val( cWeek ) to 1 step -1
			cStatWeek := strzero( w, 2 )	
*			TestMsg( "Week " + cStatWeek )
			PrintStat( cStatWeek, cPlayerID, PLAYER->CATEGORY )
			nRow++
		next
	else
		PrintStat( mWeek, cPlayerID, PLAYER->CATEGORY )
	endif

RETURN nil


static FUNCTION HeaderFor( cCat )

	if cCat $ "123"
		printat( nRow,  pcol3, "WK   Opp  Com  Att  Yds  Int  sTDp lTDp  TDp  Car  YDr sTDr lTDr  TDr  Rec  Yds sTDc lTDc  TDc  FP" )
	endif

	do case
	case cCat = "1"
		nRow++
		printat( nRow,  pcol1, "QB" )
	case cCat = "2"
		nRow++
		printat( nRow,  pcol1, "RB" ) 
	case cCat = "3"
		nRow++
		printat( nRow,  pcol1, "PR" ) 
	case cCat = "4"
		printat( nRow,  pcol3, "WK   Opp   PAT  sFG  lFG  FG   FP" )
		nRow++
		printat( nRow,  pcol1, "PK" ) 
	case cCat = "5"
		printat( nRow,  pcol3, "WK   Opp   SAK  INT  SAF  sTD  lTD  TDs  FP" )
		nRow++
		printat( nRow,  pcol1, "DL" ) 
	case cCat = "6"
		printat( nRow,  pcol3, "WK   Opp   SAK  INT  SAF  sTD  lTD  TDs  FP" )
		nRow++
		printat( nRow,  pcol1, "DB" ) 
	endcase

RETURN nil


static FUNCTION PrintStat( cWeek, cPlayerID, cCat )

	local cStatStr := ""

	cStatStr := cWeek + " " + Opp( cPlayerId, cWeek, mSeason ) 

	select STAT
	set order to 3

	do case
	case cCat $ "123"
		cStatStr += OffScore( cWeek, cPlayerID )

	case cCat = "4"
		cStatStr += KickScore( cWeek, cPlayerID )

	case cCat $ "56"
		cStatStr += DefScore( cWeek, cPlayerID )

	endcase

	printat( nRow,  pcol3, cStatStr ) 

RETURN nil


static FUNCTION DefScore( cWeek, cPlayerID )

	local cStr := ""

	cStr := WackOn( cStr, QUARTERBACK_SACKS, cWeek, cPlayerID )
	cStr := WackOn( cStr, INTERCEPTIONS_MADE, cWeek, cPlayerID )
	cStr := PrimScores( cStr, SCORE_SAFETY, cWeek, cPlayerID, .f. )
	cStr := DefScores( cStr, cWeek, cPlayerID )
	select STAT
	set order to 2
	cStr := FPTS( cStr, cWeek, cPlayerID )
	select STAT
	set order to 3

RETURN cStr


static FUNCTION DefScores( cStr, cWeek, cPlayerID, lSplit )

	local cKey, nQty := nShort := nLong := 0

	if pcount() = 3
		lSplit := .t.
	endif

	select SCORE
	set order to 2
	cKey := cPlayerId + mSeason + cWeek + SCORE_FUMBLE_RETURN
	SCORE->( dbseek( cKey ) )
	do while .not. eof() .and. SCORE->PLAYERID1 = cPlayerId .and.;
										SCORE->SEASON    = mSeason   .and.;
										SCORE->WEEK      = cWeek     .and.;
										SCORE->SCORE     = SCORE_FUMBLE_RETURN
		if SCORE->DISTANCE >= 50
			nLong++
		else
			nShort++
		endif

		nQty++
		skip
	enddo

	cKey := cPlayerId + mSeason + cWeek + SCORE_INTERCEPT_RETURN
	dbseek( cKey )
	do while .not. eof() .and. SCORE->PLAYERID1 = cPlayerId .and.;
										SCORE->SEASON    = mSeason   .and.;
										SCORE->WEEK      = cWeek     .and.;
										SCORE->SCORE     = SCORE_INTERCEPT_RETURN
		if SCORE->DISTANCE >= 50
			nLong++
		else
			nShort++
		endif

		nQty++
		skip
	enddo

	if lSplit
		if nShort > 0
			cStr += str( nShort, 5 )
		else
			cStr += "    -"
		endif
		if nLong > 0
			cStr += str( nLong, 5 )
		else
			cStr += "    -"
		endif
	endif
	if nQty > 0
		cStr += str( nQty, 5 )
	else
		cStr += "    -"
	endif

RETURN cStr


static FUNCTION KickScore( cWeek, cPlayerID )

	local cStr := ""

	cStr := PrimScores( cStr, SCORE_PAT, cWeek, cPlayerID, .f. )
	cStr := PrimScores( cStr, SCORE_FIELD_GOAL, cWeek, cPlayerID )
	cStr := FPTS( cStr, cWeek, cPlayerID )

RETURN cStr


static FUNCTION OffScore( cWeek, cPlayerID )

	local cStr := ""

	cStr := WackOn( cStr, PASS_COMPLETIONS, cWeek, cPlayerID )
	cStr := WackOn( cStr, PASSING_ATTEMPTS, cWeek, cPlayerID )
	cStr := WackOn( cStr, PASSING_YARDAGE, cWeek, cPlayerID )
	cStr := WackOn( cStr, PASSES_INTERCEPTED, cWeek, cPlayerID )
	cStr := SecScores( cStr, SCORE_TD_PASS, cWeek, cPlayerID )
	cStr := WackOn( cStr, RUSHING_CARRIES, cWeek, cPlayerID )
	cStr := WackOn( cStr, RUSHING_YARDS, cWeek, cPlayerID )
	cStr := PrimScores( cStr, SCORE_TD_RUN, cWeek, cPlayerID )
	cStr := WackOn( cStr, PASSES_CAUGHT, cWeek, cPlayerID )
	cStr := WackOn( cStr, RECEPTION_YARDAGE, cWeek, cPlayerID )
	cStr := PrimScores( cStr, SCORE_TD_PASS, cWeek, cPlayerID )
	cStr := FPTS( cStr, cWeek, cPlayerID )

RETURN cStr


static FUNCTION FPTS( cStr, cWeek, cPlayerID )

	local cFPts, nPts
	private mWeek 
	mWeek := cWeek

	cFPts := ScorePlayer( mLeagueID, cPlayerID )
	nPts  := val( substr( cFpts, 1, 3 ) ) + val( substr( cFpts, 4, 3 ) )

	if nPts > 0
		cStr += str( nPts, 5 )
	else
		cStr += "    -"
	endif

RETURN cStr


static FUNCTION PrimScores( cStr, cScore, cWeek, cPlayerID, lSplit )

	local cKey, nQty := nShort := nLong := 0

	if pcount() = 4
		lSplit := .t.
	endif

	select SCORE
	set order to 2
	cKey := cPlayerId + mSeason + cWeek + cScore
	SCORE->( dbseek( cKey ) )
	do while .not. eof() .and. SCORE->PLAYERID1 = cPlayerId .and.;
										SCORE->SEASON    = mSeason   .and.;
										SCORE->WEEK      = cWeek     .and.;
										SCORE->SCORE     = cScore
		if SCORE->DISTANCE >= 50
			nLong++
		else
			nShort++
		endif

		nQty++
		skip
	enddo

	if lSplit
		if nShort > 0
			cStr += str( nShort, 5 )
		else
			cStr += "    -"
		endif
		if nLong > 0
			cStr += str( nLong, 5 )
		else
			cStr += "    -"
		endif
	endif
	if nQty > 0
		cStr += str( nQty, 5 )
	else
		cStr += "    -"
	endif

RETURN cStr


static FUNCTION SecScores( cStr, cScore, cWeek, cPlayerID )

	local cKey, nQty := nShort := nLong := 0

	select SCORE
	set order to 3
	cKey := cPlayerId + mSeason + cWeek + cScore
	SCORE->( dbseek( cKey ) )
	do while .not. eof() .and. SCORE->PLAYERID2 = cPlayerId .and.;
										SCORE->SEASON    = mSeason   .and.;
										SCORE->WEEK      = cWeek     .and.;
										SCORE->SCORE     = cScore
		if SCORE->DISTANCE >= 50
			nLong++
		else
			nShort++
		endif

		nQty++
		skip
	enddo

	if nShort > 0
		cStr += str( nShort, 5 )
	else
		cStr += "    -"
	endif
	if nLong > 0
		cStr += str( nLong, 5 )
	else
		cStr += "    -"
	endif
	if nQty > 0
		cStr += str( nQty, 5 )
	else
		cStr += "    -"
	endif

RETURN cStr


static FUNCTION WackOn( cStr, cStat, cWeek, cPlayerID )

	local cKey
	cKey := cStat + mSeason + cWeek + cPlayerID
	if STAT->( dbseek( cKey ) )
		cStr += str( STAT->QTY, 5 )
	else
		cStr += "    -"
	endif

RETURN cStr


*EOF:  tyrperf.PRE