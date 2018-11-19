*****************************************************************************
*  Program Name.....: tyrmatch.prg 
*  Programmer.......: Steve Colonna 
*  System...........:
*  Module...........:
*  Date.............: 17/9/1990 at 18:19
*  Copyright........: (c) 1990 by Knoware 
*  Called From......:
*  Purpose..........:
*
*	  Prints out the results of a Tycoon Round of Matchups
*
*............................................................................
*  Revision.........: 1.0 Last Revised: 17/9/1990 at 18:19
*  Description......: Initial Creation.
*............................................................................
***************************** ALL RIGHTS RESERVED ***************************

#INCLUDE "..\lib\PRINT.H"
#INCLUDE "TYCOON.H"

#define  LINE_LENGTH 		180
#define  STARTING_PLAYERS	Starters( mSeason, mLeagueID )

PROCEDURE TyRMatch

	private printto, workfile, cLeagueName
   private mopt[4], mmess[4], poption1
   clear_scn()

   head_line( "MATCHUPS" )

   ScreenLine(  1 )
   ScreenLine( 21 )

   if !INIT_MU()    && Initialisation routine
      do CLOSE_MU
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

				do GEN_MU
            view_prnt( printto, LINE_LENGTH )

         case mopt[poption1] = 'V'

            view_prnt( printto, LINE_LENGTH )

         case mopt[poption1] = 'P'

				print_prnt( printto )

      endcase
   
   enddo 

   do CLOSE_MU

return


function INIT_MU 

	private mcontinu

	plswait()
   std_rpt_opt( @mopt, @mmess, "Tycoon Matchups" )

   mcontinu = .t.
   mcontinu = iif( mcontinu, opendbf( 'TYSKED', G_ty_path   ), .f. )
   mcontinu = iif( mcontinu, opendbf( 'LINEUP', G_ty_path   ), .f. )
   mcontinu = iif( mcontinu, opendbf( 'LEAGUE', G_ty_path   ), .f. )
   mcontinu = iif( mcontinu, opendbf( 'COMP',   G_ty_path   ), .f. )
   mcontinu = iif( mcontinu, opendbf( 'SCORE',  G_nfl_path  ), .f. )
   mcontinu = iif( mcontinu, opendbf( 'PLAYER', G_nfl_path  ), .f. )
   mcontinu = iif( mcontinu, opendbf( 'STAT',   G_nfl_path  ), .f. )

	@ 23,00

	if ! mcontinu
		error_msg( "Unable to gain access to all the necessary files" )
	else

		select STAT
		set order to 2
	endif

return( mcontinu )


procedure CLOSE_MU

	close databases

return


function VAL_MU

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

		case mvar == "mWeek"

			if .not. TYSKED->( dbSeek( mSeason + mLeagueId, mWeek ) )
				ok = .f.
				error_msg( "Scheduled matchups not found" )
			endif

	endcase
	setcolor( saveclr )

return( ok )


procedure GEN_MU

   private i, repoption, mSeason, mLeagueid, mWeek

	setcolor( c_desc )
	print( 6, 15, "Season :" )
	print( 7, 15, "League :" )
	print( 8, 15, "Week   :" )

	setcolor( c_fields )
	mSeason   = G_season
	mLeagueid = "OL"
	mWeek     = G_week

	vget( 6, 25, "mSeason" ,   "XXXX", .f. )
	vget( 7, 25, "mLeagueid" , "!!",   .f. )
	vget( 8, 25, "mWeek",      "XX",   .f. )

	vread( 1, .f., 'VAL_MU' )

	CLEAR_WINDOW( 6, 15, 8, 79 )
	if abort()
		retu
	else
		G_week   = mWeek
		G_league = mLeagueId
		printto  = MatchPrn()

	endif

	GenMsg()

	PrnMatch( mSeason, mLeagueId, mWeek, printto )

RETURN

FUNCTION MatchPrn
RETURN g_reppath + TEMPORARY_FILE_INDICATOR + "RES" + ;
		 	mWeek + mLeagueId + PRINTFILE_EXT


FUNCTION PrnMatch( mSeason, mLeagueId, mWeek, printto )
	***
	***   Load Data
	***
	private mHt1, mHt2, mHt3, mHt4, mHt5, mHt6, mHt7, mHt8
	private mAt1, mAt2, mAt3, mAt4, mAt5, mAt6, mAt7, mAt8 

	cRound = mSeason + mLeagueid + mweek

	nTeams = TeamsIn( mSeason, mLeagueId )
	*TestMsg( str( nTeams, 3 ) + " teams in " + mLeagueId + " for " + mSeason )

	nGames = int( ( nTeams / 2 ) +.5 )

	private franchise_[ nTeams ]

	fran_cnt = 0

	select COMP
	COMP->( dbseek( mSeason + mLeagueid ) )
	do while .not. eof() .and. SEASON = mseason .and. LEAGUEID = mleagueid
		INC( fran_cnt )
		franchise_[ fran_cnt ] = OWNERID
		skip
	enddo

	TestMsg( "Starters " + str( Starters( mSeason, mLeagueID ) ) )

	private ht1_plyr_[ STARTING_PLAYERS ]
	private at1_plyr_[ STARTING_PLAYERS ]
	private ht2_plyr_[ STARTING_PLAYERS ]
	private at2_plyr_[ STARTING_PLAYERS ]
	private ht3_plyr_[ STARTING_PLAYERS ]
	private at3_plyr_[ STARTING_PLAYERS ]
	private ht4_plyr_[ STARTING_PLAYERS ]
	private at4_plyr_[ STARTING_PLAYERS ]
	private ht5_plyr_[ STARTING_PLAYERS ]
	private at5_plyr_[ STARTING_PLAYERS ]
	private ht6_plyr_[ STARTING_PLAYERS ]
	private at6_plyr_[ STARTING_PLAYERS ]
	private ht7_plyr_[ STARTING_PLAYERS ]
	private at7_plyr_[ STARTING_PLAYERS ]
	private ht8_plyr_[ STARTING_PLAYERS ]
	private at8_plyr_[ STARTING_PLAYERS ]

	private ht1_off_[ STARTING_PLAYERS ]
	private at1_off_[ STARTING_PLAYERS ]
	private ht2_off_[ STARTING_PLAYERS ]
	private at2_off_[ STARTING_PLAYERS ]
	private ht3_off_[ STARTING_PLAYERS ]
	private at3_off_[ STARTING_PLAYERS ]
	private ht4_off_[ STARTING_PLAYERS ]
	private at4_off_[ STARTING_PLAYERS ]
	private ht5_off_[ STARTING_PLAYERS ]
	private at5_off_[ STARTING_PLAYERS ]
	private ht6_off_[ STARTING_PLAYERS ]
	private at6_off_[ STARTING_PLAYERS ]
	private ht7_off_[ STARTING_PLAYERS ]
	private at7_off_[ STARTING_PLAYERS ]
	private ht8_off_[ STARTING_PLAYERS ]
	private at8_off_[ STARTING_PLAYERS ]

	private ht1_def_[ STARTING_PLAYERS ]
	private at1_def_[ STARTING_PLAYERS ]
	private ht2_def_[ STARTING_PLAYERS ]
	private at2_def_[ STARTING_PLAYERS ]
	private ht3_def_[ STARTING_PLAYERS ]
	private at3_def_[ STARTING_PLAYERS ]
	private ht4_def_[ STARTING_PLAYERS ]
	private at4_def_[ STARTING_PLAYERS ]
	private ht5_def_[ STARTING_PLAYERS ]
	private at5_def_[ STARTING_PLAYERS ]
	private ht6_def_[ STARTING_PLAYERS ]
	private at6_def_[ STARTING_PLAYERS ]
	private ht7_def_[ STARTING_PLAYERS ]
	private at7_def_[ STARTING_PLAYERS ]
	private ht8_def_[ STARTING_PLAYERS ]
	private at8_def_[ STARTING_PLAYERS ]

	LoadLineups( mSeason, mLeagueid, mWeek )

	do TALLYPTS

	private    pcol1, pcol2, pcol3, pcol4, pcol5, pcol6
	store 0 to pcol1, pcol2, pcol3, pcol4, pcol5, pcol6

   begin sequence

		*---Using pre printed Stationery

      set print to ( printto )
		set print on

		print_setup( LINE_LENGTH )

		*---Short length	(Landscape)
*		?? p_a3
*		?? chr(27) + chr(38) + "l1O"			&&		HP Laser
		p_linespp = 44

		*---Page mode
		prntpagebuf()

      print_margin( 0, 0, 0 )

      prntcoldec( 1 )

      *---set up the printing columns
		prntcolnxt( @pcol1,  30 )
		prntcolnxt( @pcol2,  30 )
		prntcolnxt( @pcol3,  30 )
		prntcolnxt( @pcol4,  30 )
		prntcolnxt( @pcol5,  30 )
		prntcolnxt( @pcol6,  30 )

		printat( 1, pcol1, mSeason + " - " + cLeagueName + " Week " + mweek )

		PrintTeams()

		for p = 1 to STARTING_PLAYERS
			PrintPlayer( p )
		next

		print_page()
		
	end

   print_release()
   set print to        

	*---Copy output to REPORT directory
	copy file ( printto ) to ( G_reppath + "results.txt" )

	***
	***   Update the TYSKED file with the results
	***
	select TYSKED
	do RLOCKIT with "Tycoon Matchup schedule"
	replace TYSKED->HS1        with TeamScore( "1", .t. )
	replace TYSKED->HS2        with TeamScore( "2", .t. )
	replace TYSKED->HS3        with TeamScore( "3", .t. )
	replace TYSKED->HS4        with TeamScore( "4", .t. )
	replace TYSKED->AS1        with TeamScore( "1", .f. )
	replace TYSKED->AS2        with TeamScore( "2", .f. )
	replace TYSKED->AS3        with TeamScore( "3", .f. )
	replace TYSKED->AS4        with TeamScore( "4", .f. )
	replace TYSKED->THS1       with CombScore( "1", .t. )
	replace TYSKED->THS2       with CombScore( "2", .t. )
	replace TYSKED->THS3       with CombScore( "3", .t. )
	replace TYSKED->THS4       with CombScore( "4", .t. )
	replace TYSKED->TAS1       with CombScore( "1", .f. )
	replace TYSKED->TAS2       with CombScore( "2", .f. )
	replace TYSKED->TAS3       with CombScore( "3", .f. )
	replace TYSKED->TAS4       with CombScore( "4", .f. )
	if nGames > 4
		replace TYSKED->HS5        with TeamScore( "5", .t. )
		replace TYSKED->THS5       with CombScore( "5", .t. )
		replace TYSKED->AS5        with TeamScore( "5", .f. )
		replace TYSKED->TAS5       with CombScore( "5", .f. )
	endif

	if nGames > 5
		replace TYSKED->HS6        with TeamScore( "6", .t. )
		replace TYSKED->AS6        with TeamScore( "6", .f. )
		replace TYSKED->THS6       with CombScore( "6", .t. )
		replace TYSKED->TAS6       with CombScore( "6", .f. )
	endif

	commit
	
	@ 22,00
	clrmenu()
	GenMsgClr()

RETURN nil


function PrintPlayer

	parameters nPlyr

	do case
		case nPlyr = 1
			nRow = 6
		case nPlyr = 2
			nRow = 8
		case nPlyr = 3
			nRow = 9
		case nPlyr = 4
			nRow = 11
		case nPlyr = 5
			nRow = 12
		case nPlyr = 6
			nRow = 14
		case nPlyr = 7
			nRow = 16
		case nPlyr = 8
			nRow = 17
		case nPlyr = 9
			nRow = 18
		case nPlyr = 10
			nRow = 20
		case nPlyr = 11
			nRow = 21
		case nPlyr = 12
			nRow = 22

	endcase

	for g = 1 to nGames
		cG = str(g,1)
		mplayerid = ht&cG._plyr_[nPlyr]

		printat( nRow,  pcol&cG.,    PlayerName( mplayerid ) )
		printat( nRow,  pcol&cG.+20, str( ht&cG._off_[nPlyr], 3 )   )
		printat( nRow,  pcol&cG.+24, str( ht&cG._def_[nPlyr], 3 )   )

		mplayerid = at&cG._plyr_[nPlyr]
		printat( nRow+22,  pcol&cG.,    PlayerName( mplayerid ) )
		printat( nRow+22,  pcol&cG.+20, str( at&cG._off_[nPlyr], 3 )   )
		printat( nRow+22,  pcol&cG.+24, str( at&cG._def_[nPlyr], 3 )   )
	next

RETURN nil


function LoadLineups( mSeason, mLeagueid, mWeek )

	local p, cPlayer

	select TYSKED
	TYSKED->( dbseek( mSeason+mLeagueid+mWeek ) )

	if found()

		mHT1 = HT1
		if empty( mht1 )
			afill( ht1_plyr_, space(8) )
		else
			select LINEUP
			LINEUP->( dbseek( mSeason+mLeagueid+mHT1+mWeek ) )

			if found()
				for p = 1 to Starters( mSeason, mLeagueID )
					cPlayer = "PLAYER" + ltrim( str( p, 2 ) )
					HT1_plyr_[ p ] = &cPlayer
				next

			else
				error_msg( 'Cant find lineup' + mSeason+mLeagueid+mHT1+mWeek )
			endif
		endif

		mHT2 = TYSKED->HT2
		if empty( mht2 )
			afill( ht2_plyr_, space(8) )
		else
			select LINEUP
			LINEUP->( dbseek( mSeason+mLeagueid+mHT2+mWeek ) )

			if found()
				for p = 1 to Starters( mSeason, mLeagueID )
					cPlayer = "PLAYER" + ltrim( str( p, 2 ) )
					HT2_plyr_[ p ] = &cPlayer
				next
			else
				error_msg( 'Cant find lineup' + mSeason+mLeagueid+mHT2+mWeek )
			endif
		endif

		mHT3 = TYSKED->HT3
		if empty( mht3 )
			afill( ht3_plyr_, space(8) )
		else
			select LINEUP
			LINEUP->( dbseek( mSeason+mLeagueid+mHT3+mWeek ) )

			if found()
				for p = 1 to Starters( mSeason, mLeagueID )
					cPlayer = "PLAYER" + ltrim( str( p, 2 ) )
					HT3_plyr_[ p ] = &cPlayer
				next
			else
				error_msg( 'Cant find lineup' + mSeason+mLeagueid+mHT3+mWeek )
			endif
		endif

		mHT4 = TYSKED->HT4
		if empty( mht4 )
			afill( ht4_plyr_, space(8) )
		else
			select LINEUP
			LINEUP->( dbseek( mSeason+mLeagueid+mHT4+mWeek ) )

			if found()
				for p = 1 to Starters( mSeason, mLeagueID )
					cPlayer = "PLAYER" + ltrim( str( p, 2 ) )
					HT4_plyr_[ p ] = &cPlayer
				next
			else
				error_msg( 'Cant find lineup' + mSeason+mLeagueid+mHT4+mWeek )
			endif
		endif

		mHT5 = TYSKED->HT5
		if empty( mHt5 )
			afill( ht5_plyr_, space(8) )
		else

			select LINEUP
			LINEUP->( dbseek( mSeason+mLeagueid+mHT5+mWeek ) )

			if found()
				for p = 1 to Starters( mSeason, mLeagueID )
					cPlayer = "PLAYER" + ltrim( str( p, 2 ) )
					HT5_plyr_[ p ] = &cPlayer
				next
			else
				error_msg( 'Cant find lineup' + mSeason+mLeagueid+mHT5+mWeek )
			endif
		endif

		mHT6 = TYSKED->HT6
		if empty( mHt6 )
			afill( ht6_plyr_, space(8) )
		else

			select LINEUP
			LINEUP->( dbseek( mSeason+mLeagueid+mHT6+mWeek ) )

			if found()
				for p = 1 to Starters( mSeason, mLeagueID )
					cPlayer = "PLAYER" + ltrim( str( p, 2 ) )
					HT6_plyr_[ p ] = &cPlayer
				next
			else
				error_msg( 'Cant find lineup' + mSeason+mLeagueid+mHT6+mWeek )
			endif
		endif

		mAT1 = TYSKED->AT1
		if empty( mat1 )
			afill( at1_plyr_, space(8) )
		else
			select LINEUP
			LINEUP->( dbseek( mSeason+mLeagueid+mAT1+mWeek ) )

			if found()
				for p = 1 to Starters( mSeason, mLeagueID )
					cPlayer = "PLAYER" + ltrim( str( p, 2 ) )
					AT1_plyr_[ p ] = &cPlayer
				next
			else
				error_msg( 'Cant find lineup' + mSeason+mLeagueid+mAT1+mWeek )
			endif
		endif

		mAT2 = TYSKED->AT2
		if empty( mat2 )
			afill( at2_plyr_, space(8) )
		else
			select LINEUP
			LINEUP->( dbseek( mSeason+mLeagueid+mAT2+mWeek ) )

			if found()
				for p = 1 to Starters( mSeason, mLeagueID )
					cPlayer = "PLAYER" + ltrim( str( p, 2 ) )
					AT2_plyr_[ p ] = &cPlayer
				next
			else
				error_msg( 'Cant find lineup' + mSeason+mLeagueid+mAT2+mWeek )
			endif
		endif

		mAT3 = TYSKED->AT3
		if empty( mat3 )
			afill( at3_plyr_, space(8) )
		else
			select LINEUP
			LINEUP->( dbseek( mSeason+mLeagueid+mAT3+mWeek ) )

			if found()
				for p = 1 to Starters( mSeason, mLeagueID )
					cPlayer = "PLAYER" + ltrim( str( p, 2 ) )
					AT3_plyr_[ p ] = &cPlayer
				next
			else
				error_msg( 'Cant find lineup' + mSeason+mLeagueid+mAT3+mWeek )
			endif
		endif

		mAT4 = TYSKED->AT4
		if empty( mat4 )
			afill( at4_plyr_, space(8) )
		else
			select LINEUP
			LINEUP->( dbseek( mSeason+mLeagueid+mAT4+mWeek ) )

			if found()
				for p = 1 to Starters( mSeason, mLeagueID )
					cPlayer = "PLAYER" + ltrim( str( p, 2 ) )
					AT4_plyr_[ p ] = &cPlayer
				next
			else
				error_msg( 'Cant find lineup' + mSeason+mLeagueid+mAT4+mWeek )
			endif
		endif

		mAT5 = TYSKED->AT5
		if empty( mAt5 )
			afill( at5_plyr_, space(8) )
		else

			select LINEUP
			LINEUP->( dbseek( mSeason+mLeagueid+mAT5+mWeek ) )

			if found()
				for p = 1 to Starters( mSeason, mLeagueID )
					cPlayer = "PLAYER" + ltrim( str( p, 2 ) )
					AT5_plyr_[ p ] = &cPlayer
				next
			else
				error_msg( 'Cant find lineup' + mSeason+mLeagueid+mAT5+mWeek )
			endif
		endif

		mAT6 = TYSKED->AT6
		if empty( mat6 )
			afill( at6_plyr_, space(8) )
		else
			select LINEUP
			LINEUP->( dbseek( mSeason+mLeagueid+mAT6+mWeek ) )

			if found()
				for p = 1 to Starters( mSeason, mLeagueID )
					cPlayer = "PLAYER" + ltrim( str( p, 2 ) )
					AT6_plyr_[ p ] = &cPlayer
				next
			else
				error_msg( 'Cant find lineup' + mSeason+mLeagueid+mAT6+mWeek )
			endif
		endif

		mAT7 = TYSKED->AT7
		if empty( mat7 )
			afill( at7_plyr_, space(8) )
		else
			select LINEUP
			LINEUP->( dbseek( mSeason+mLeagueid+mAT7+mWeek ) )

			if found()
				for p = 1 to Starters( mSeason, mLeagueID )
					cPlayer = "PLAYER" + ltrim( str( p, 2 ) )
					AT7_plyr_[ p ] = &cPlayer
				next
			else
				error_msg( 'Cant find lineup' + mSeason+mLeagueid+mAT7+mWeek )
			endif
		endif

		mAT8 = TYSKED->AT8
		if empty( mat8 )
			afill( at8_plyr_, space(8) )
		else
			select LINEUP
			LINEUP->( dbseek( mSeason+mLeagueid+mAT8+mWeek ) )

			if found()
				for p = 1 to Starters( mSeason, mLeagueID )
					cPlayer = "PLAYER" + ltrim( str( p, 2 ) )
					AT8_plyr_[ p ] = &cPlayer
				next
			else
				error_msg( 'Cant find lineup' + mSeason+mLeagueid+mAT8+mWeek )
			endif
		endif

		
	endif

RETURN nil


procedure TALLYPTS

	private cHT, ht, p, cPlyr, cOff, cDef, cOPts

	for ht = 1 to nGames

		cHT = LSTR( ht, 1, 0 )

		for p = 1 to STARTING_PLAYERS

			cPlyr := "ht" + cHT + "_plyr_[ " + ltrim(str( p, 4 ) ) + "]"
			*TestMsg( "cPlyr = " + cPlyr )
			mplayerid = &( cPlyr )

			cScore := ScorePlayer( mLeagueID, mplayerid )  // in h:NFL_LIB

			*TestMsg( "Player " + mPlayerId + "  " + cScore )

			cOff := "ht" + cHT + "_off_[ " + ltrim(str( p, 4 ) ) + "]"
			cDef := "ht" + cHT + "_def_[ " + ltrim(str( p, 4 ) ) + "]"
			cOPts := substr( cScore, 1, 3 ) 
			cOPts := if( cOPts = "0", substr( cOPts, 2, 2 ), cOPts ) 
			&( cOff ) := val( cOPts )
			&( cDef ) := val( substr( cScore, 4, 3 ) )

		next
	next

	for awt = 1 to nGames

		cAwt = LSTR( awt, 1, 0 )

		for p = 1 to STARTING_PLAYERS

			mplayerid = AT&cawt._plyr_[ p ]

			cScore := ScorePlayer( mLeagueID, mplayerid )

			cOPts := substr( cScore, 1, 3 ) 
			cOPts := if( cOPts = "0", substr( cOPts, 2, 2 ), cOPts ) 

			AT&cawt._off_[ p ] = val( cOPts )
			AT&cawt._def_[ p ] = val( substr( cScore, 4, 3 ) )

		next
	next

RETURN 


function PrintTeams

	local mHtCode, mATcode, mAT, mHT, cColumn, cG

	for g = 1 to nGames
		cG  := str( g, 1 )
		mHt := "mHt" + cG
		mAt := "mAt" + cG
		mHtCode := &mHt
		mAtCode := &mAt
		cColumn := "pcol" + cG
		printat( 3,    &cColumn, Franchise( mSeason, mLeagueid, mHtCode )   )
		printat( 3+23, &cColumn, Franchise( mSeason, mLeagueid, mAtCode )   )

		printat( 3,    &cColumn+23, str( TeamScore( cG, .t. ), 4, 0 )   )
		printat( 3+23, &cColumn+23, str( TeamScore( cG, .f. ), 4, 0 )   )
		printat( 4,    &cColumn+23, str( CombScore( cG, .t. ), 4, 0 )   )
		printat( 4+23, &cColumn+23, str( CombScore( cG, .f. ), 4, 0 )   )
	next

RETURN NIL


function Teamscore

	parameters cGame, lHomeAway

	private nTotOff, nOppDef, nScore

	store 0 to nTotOff, nOppDef, nScore

	if lHomeAway
		for p = 1 to STARTING_PLAYERS
			ADDTO( nTotOff, Ht&cGame._off_[ p ] )
			ADDTO( nOppDef, At&cGame._def_[ p ] )
		next
	else
		for p = 1 to STARTING_PLAYERS
			ADDTO( nTotOff, At&cGame._off_[ p ] )
			ADDTO( nOppDef, Ht&cGame._def_[ p ] )
		next
	endif

	nScore = nTotOff - nOppDef
	nScore = iif( nScore < 0, 0, nScore )


return nScore


function CombScore

	parameters cGame, lHomeAway

	private nTotOff, nOppDef, nScore

	store 0 to nTotOff, nTotDef, nScore

	if lHomeAway
		for p = 1 to STARTING_PLAYERS
			ADDTO( nTotOff, Ht&cGame._off_[ p ] )
			ADDTO( nTotDef, Ht&cGame._def_[ p ] )
		next
	else
		for p = 1 to STARTING_PLAYERS
			ADDTO( nTotOff, At&cGame._off_[ p ] )
			ADDTO( nTotDef, At&cGame._def_[ p ] )
		next
	endif

	nScore = nTotOff + nTotDef

return nScore


FUNCTION Starters( mSeason, mLeagueID )

	LOCAL nStarters  := 0

	do case
	case mLeagueID = "OL"
		if mSeason >= "1992"
			nStarters := 12
		else
			nStarters := 11
		endif

	case mLeagueID = "WL"
		nStarters := 7

	otherwise
		nStarters := 11

	endcase

RETURN nStarters



*EOF:  TYRMATCH.PRG

