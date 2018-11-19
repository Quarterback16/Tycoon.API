*****************************************************************************
*  Program Name.....: nfl_lib.pre
*  Programmer.......: Steve Colonna
*  System...........: TFL
*  Module...........: Project Library
*  Date.............: 29/4/1990 at 20:28
*  Copyright........: (c) 1990 by Knoware
*  Purpose..........: To hold common routines for the TFL project
*
*............................................................................
*  Revision.........: 1.0 Last Revised: 29/4/1990 at 20:28
*  Description......: Initial Creation.
*............................................................................
***************************** ALL RIGHTS RESERVED ***************************

field SYSNAME, SYSOWNER, DRIVE, NFL_PATH, TY_PATH, GETLISTKEY
field SEASON, LEAGUE, WEEK, PLAYER, GAME, REPPATH, LASTTEAM

#INCLUDE "TFL.H"

function SYS_INFO

	parameters sys_path

	local full_name, i, j, opened_ok

	external DESCEND

	vread_init( 36 )										 && vread 36 gets
	vread_cursor = .f.
	blowup_init()											 && boxes
	setboxes( "S", .t. )									 && double frames, no shadows on boxes

	opened_ok = .t.
	full_name = sys_path+"TFL_CTL"

	public G_cursor
	public G_sysname
	public G_shrtnme
	public G_version
	public G_sysowner
	public G_printing
	public G_nfl_path
	public G_ty_path
	public G_reppath
	public G_module
	public G_getlistkey
	public g_explode
	public g_station
	public g_season
	public g_league
	public g_week
	public G_Gameno
	public G_player
    public G_Team
	public Gsys_test

	*---Get System test status
	if upper(trim(gete("SYSTEM"))) = "TEST"
		Gsys_test = .t.
	else
		*---Disable ALT-C
		Gsys_test = .f.
	endif

	select 0
	if .not. net_use( full_name, .f., 5, "CONTROL" )
		error_msg( "Unable to open the system control file " + full_name )
		close databases
		opened_ok = .f.
	else
	    TestMsg( "Opened control file " + full_name )
	endif

	if opened_ok
		G_cursor   = .f.
		G_sysname  = alltrim( SYSNAME  )
		G_sysowner = alltrim( SYSOWNER )
		G_drive    = alltrim( DRIVE    )
		G_nfl_path = alltrim( NFL_PATH )
		G_ty_path  = alltrim( TY_PATH  )
		G_reppath  = alltrim( REPPATH  )
		G_getlistkey = GETLISTKEY
		G_season     = SEASON
		G_league     = LEAGUE
		G_week       = WEEK
		G_GameNo     = GAME
		G_player     = PLAYER
        G_Team       = CONTROL->LASTTEAM
		g_explode    = .t.

	endif


RETURN( opened_ok)


FUNCTION StoreGweek

	parameters G_week
	select 0
	if .not. net_use( full_name, .f., 5, "CONTROL" )
		error_msg( "Unable to open the system control file " + full_name )
		close databases
		opened_ok = .f.
	else
	    TestMsg( "Opened control file " + full_name )
	endif
	if opened_ok
   select CONTROL

		if CONTROL->WEEK <> G_week
         TestMsg('Setting week to ' + G_week )
			do RLOCKIT with "Control file"
			replace CONTROL->WEEK   with G_week
      else
         TestMsg('Leaving week at ' + G_week )
		endif
		if CONTROL->PLAYER <> G_player
			do RLOCKIT with "Control file"
			replace CONTROL->PLAYER with G_player
		endif
		if CONTROL->GAME <> G_GameNo
			do RLOCKIT with "Control file"
			replace CONTROL->GAME with G_GameNo
		endif
		if CONTROL->LASTTEAM <> G_team
			do RLOCKIT with "Control file"
			replace CONTROL->LASTTEAM with G_Team
		endif
	endif


RETURN NIL


function PlayerCat

	parameter nCat, nChars

	nChars = def_parm( "nChars", "N", 1 )

	do case
	case nCat = "1"
		cCat = if( nChars = 1, "Q", "QB" )
	case nCat = "2"
		cCat = if( nChars = 1, "R", "RB" )
	case nCat = "3"
		cCat = if( nChars = 1, "P", "PR" )
	case nCat = "4"
		cCat = if( nChars = 1, "K", "K " )
	case nCat = "5"
		cCat = if( nChars = 1, "L", "DL" )
	case nCat = "6"
		cCat = if( nChars = 1, "S", "DB" )
	otherwise
		cCat = if( nChars = 1, "?", "??" )
	endcase

RETURN cCat


function CatName

	parameter nCatcode

	do case
	case nCatCode = "1"
		cCatName = "Quarterback"
	case nCatCode = "2"
		cCatName = "Running Back"
	case nCatCode = "3"
		cCatName = "Pass Receiver"
	case nCatCode = "4"
		cCatName = "Kicker"
	case nCatCode = "5"
		cCatName = "Defensive Lineman"
	case nCatCode = "6"
		cCatName = "Defensive Back"
	endcase

RETURN cCatName


FUNCTION ScorePlayer( mLeagueID, mPlayerid )

	LOCAL nTotOff, nTotDef, cRetString

	***
	***	Determine Fantasy score given Player ID, mSeason, mWeek
	***	and mLeagueID
	***

	if empty( mPlayerid )
		retu( "000000" )
	endif

	cTestPlayer = "XXXX01"
	if mPlayerID = cTestPlayer
		TestMsg( "Test player " + cTestPlayer + " League " + mLeagueID )
	endif

	nTotOff = 0
	nTotDef = 0

	select STAT

	*---Tally Sacks
	dbseek( mPlayerid + QUARTERBACK_SACKS + mSeason + mWeek )
	if found()
		ADDTO( nTotDef, STAT->QTY * PointsFor( mLeagueID, QUARTERBACK_SACKS ) )
		if mPlayerId = cTestPlayer
			TestMsg( 'after sacks def = ' + str( nTotDef, 3 ) )
		endif
	endif

	*---Tally Intercepts
	dbseek( mPlayerid + INTERCEPTIONS_MADE + mSeason + mWeek )
	if found()
		ADDTO( nTotDef, STAT->QTY * PointsFor( mLeagueID, INTERCEPTIONS_MADE ) )
		if mPlayerId = cTestPlayer
			TestMsg( 'after intercepts def = ' + str( nTotDef, 3 ) )
		endif
	endif

	if ( mSeason > "1990" .and. mLeagueId = "CL" ) .or. mLeagueID $ "WL,ES"
		*---Tally Interceptions thrown
		if STAT->( dbseek( mPlayerId + PASSES_INTERCEPTED + mSeason + mWeek ))
			ADDTO( nTotOff, STAT->QTY * PointsFor( mLeagueID, PASSES_INTERCEPTED ) )
		endif

		*---Bonus for rushing yds
		if STAT->( dbseek( mPlayerId + RUSHING_YARDS + mSeason + mWeek ))
			ADDTO( nTotOff, PointsFor( mLeagueID, RUSHING_YARDS, STAT->QTY ) )
			if mPlayerId = cTestPlayer
				TestMsg( 'after Rushing Bonus off = ' + str( nTotOff, 3 ) )
			endif

		endif

		*---Bonus for receiving yds
		if STAT->( dbseek( mPlayerId + RECEPTION_YARDAGE + mSeason + mWeek ))
			ADDTO( nTotOff, PointsFor( mLeagueID, RECEPTION_YARDAGE, STAT->QTY ) )
			if mPlayerId = cTestPlayer
				TestMsg( 'after Receiving Bonus off = ' + str( nTotOff, 3 ) )
			endif
		endif

		*---Bonus for passing yds
		if STAT->( dbseek( mPlayerId + PASSING_YARDAGE + mSeason + mWeek ))
			ADDTO( nTotOff, PointsFor( mLeagueID, PASSING_YARDAGE, STAT->QTY ) )
			if mPlayerId = cTestPlayer
				TestMsg( 'player had ' + str( STAT->QTY, 3 ) + " YDp " + PASSING_YARDAGE )
				TestMsg( 'after Passing Bonus off = ' + str( nTotOff, 3 ) )
			endif
		endif
	endif

  if mLeagueID = "GS"
		*---Bonus for receptions
		if STAT->( dbseek( mPlayerId + PASSES_CAUGHT + mSeason + mWeek ))
			ADDTO( nTotOff, PointsFor( mLeagueID, "GSP", STAT->QTY ) )
			if mPlayerId = cTestPlayer
				TestMsg( 'after GS Reception Bonus off = ' + str( nTotOff, 3 ) )
			endif
		endif
	endif

	*---Tally Scores
	select SCORE
	set order to 2
	dbseek( mPlayerid + mSeason + mweek )
	do while .not. eof() .and. SCORE->PLAYERID1 = mPlayerid ;
								.and. SCORE->SEASON    = mSeason   ;
								.and. SCORE->WEEK      = mWeek

		ADDTO( nTotOff, PointsFor( mLeagueID, SCORE->SCORE, SCORE->DISTANCE	) )

		if mPlayerId = cTestPlayer
			TestMsg( 'after ' + SCORE->SCORE + ' off = ' + str( nTotOff, 3 ) )
		endif

		select SCORE
		skip
	enddo

	*---Tally Touchdown Passes
	select SCORE
	set order to 3
	dbseek( mPlayerid + mSeason + mweek )
	do while .not. eof() .and. SCORE->PLAYERID2 = mPlayerid ;
								.and. SCORE->SEASON    = mSeason   ;
								.and. SCORE->WEEK      = mWeek

		ADDTO( nTotOff, PointsFor( mLeagueID, SCORE->SCORE, SCORE->DISTANCE	) )

		if mPlayerId = cTestPlayer
			TestMsg( 'after TD passes thrown off = ' + str( nTotOff, 3 ) )
		endif

		select SCORE
		skip
	enddo

	cRetString = right( "000" + LSTR( nTotOff, 3, 0 ), 3 ) +;
					 right( "000" + LSTR( nTotDef, 3, 0 ), 3 )

	if mPlayerId = cTestPlayer
		TestMsg( mWeek + " = " + cRetString )
	endif

RETURN cRetString


FUNCTION PointsFor( mLeagueID, cScoreType, nDistance )

	LOCAL nPoints := 0

	if pcount() = 2
		nDistance = 0
	endif

*	TestMsg( "Score Type is " + cScoreType )
	do case
		case cScoreType == QUARTERBACK_SACKS
			nPoints = iif( mLeagueID = "OL", 2, 0 )

		case cScoreType == INTERCEPTIONS_MADE
			nPoints := iif( mSeason >= "1992" .and. mLeagueID = "OL", 4, 6 )
			nPoints := iif( mLeagueID = "GS", 0, nPoints )

		case cScoreType $ TOUCHDOWN_SCORES

			nPoints = 6 + iif( mLeagueID $ "WL,GS,ES", 0, iif( nDistance > 49, 6, 0 ) )
         if	cScoreType = SCORE_TD_PASS .and. mLeagueID = "ES"
            nPoints = 4
      	endif

		case cScoreType == SCORE_FIELD_GOAL
			nPoints = 3 + iif( mLeagueID = "GS", 0, iif( nDistance > 49, 3, 0 ) )
         if	mLeagueID = "ES"
            do case
				case nDistance > 49
               nPoints = 5
            case nDistance > 39
               nPoints = 4
      		endcase
      	endif

		case cScoreType == SCORE_PAT
			nPoints = 1
		case cScoreType == SCORE_PAT_PASS
			nPoints = iif( mLeagueID $ "OL,ES", 2, 0 )

		case cScoreType == SCORE_PAT_RUN
			nPoints = iif( mLeagueID $ "OL,ES", 2, 0 )

		case cScoreType == SCORE_SAFETY .and. mLeagueID = "OL"
			nPoints = iif( mLeagueID = "OL", 2, 0 )

		case cScoreType == PASSES_INTERCEPTED
			nPoints = iif( mLeagueID = "ES", -2, 0 )
			nPoints = iif( mLeagueID = "WL", -3, 0 )

		case cScoreType == PASSING_YARDAGE
			nPoints := if( nDistance >= 250, iif( mLeagueID = "WL", 7, 0 ), 0 )
			nPoints := iif( mLeagueID = "GS", int( nDistance / 100 ) * 6, nPoints )
			nPoints := iif( mLeagueID = "ES", int( nDistance / 50 ) * 1, 0 )
*        TestMsg( "Passing yardage " + str( nDistance ) + " points " + str( nPoints ) )

		case cScoreType == "GSP"
			nPoints := iif( mLeagueID = "GS", int( nDistance / 5 ) * 6, 0 )

		case cScoreType == RECEPTION_YARDAGE
			nPoints = if( nDistance > 99, iif( mLeagueID = "WL", 1, 3 ), 0 )
			nPoints = if( nDistance > 149, iif( mLeagueID = "WL", 7, 3 ), 0 )
			nPoints := iif( mLeagueID = "GS", int( nDistance / 80 ) * 6, 0 )
			nPoints := iif( mLeagueID = "ES", int( nDistance / 20 ) * 1, 0 )

		case cScoreType == RUSHING_YARDS
			nPoints = if( nDistance > 99, iif( mLeagueID = "WL", 1, 6 ), 0 )
			nPoints = if( nDistance > 149, iif( mLeagueID = "WL", 7, 3 ), 0 )
*			nPoints := iif( mLeagueID = "GS", int( nDistance / 80 ) * 6, nPoints )
			nPoints := iif( mLeagueID = "GS", int( nDistance / 80 ) * 6, 0 )
			nPoints := iif( mLeagueID = "ES", int( nDistance / 20 ) * 1, 0 )

	endcase

RETURN nPoints



FUNCTION TotalPts

	parameters mPlayerid, mSeason, mWeek, mLeagueID

	LOCAL cScore, nPoints, cOff

	cScore  = ScorePlayer( mLeagueID, mPlayerId )
	nPoints = 0

	cOff = substr( cScore, 1, 3 )
	if cOff = "0"
		cOff = substr( cOff, 2, 2 )
	endif

	ADDTO( nPoints, val( cOff ) )
	ADDTO( nPoints, val( substr( cScore, 4, 3 ) ) )

*	tESTmsg( 'F pts = ' + str( nPoints, 4, 0 ) )

RETURN nPoints


FUNCTION PlaysFor

	parameters mPlayerid, cComp, mSeason, lShort

	LOCAL nOldArea, cFranCode, lGotHim

    if type('lShort') = "U"
      lShort := .f.
    else
      lShort := .t.
    endif

	nOldArea  = select()
	lGotHim   = .f.
	cFranCode = "  "

    TestMsg( "Looking for " +  mSeason + cComp + " " + mPlayerID )
	select COMP

	dbseek( mSeason + cComp )
	do while .not. eof() .and. COMP->SEASON = mSeason .and.;
										COMP->LEAGUEID = cComp .and. lGotHim = .f.

		if mPlayerid = COMP->PLAYERA
			lGotHim = .t.
		elseif mPlayerid = COMP->PLAYERB
			lGotHim = .t.
		elseif mPlayerid = COMP->PLAYERC
			lGotHim = .t.
		elseif mPlayerid = COMP->PLAYERD
			lGotHim = .t.
		elseif mPlayerid = COMP->PLAYERE
			lGotHim = .t.
		elseif mPlayerid = COMP->PLAYERF
			lGotHim = .t.
		elseif mPlayerid = COMP->PLAYERG
			lGotHim = .t.
		elseif mPlayerid = COMP->PLAYERH
			lGotHim = .t.
		elseif mPlayerid = COMP->PLAYERI
			lGotHim = .t.
		elseif mPlayerid = COMP->PLAYERJ
			lGotHim = .t.
		elseif mPlayerid = COMP->PLAYERK
			lGotHim = .t.
		elseif mPlayerid = COMP->PLAYERL
			lGotHim = .t.
		elseif mPlayerid = COMP->PLAYERM
			lGotHim = .t.
		elseif mPlayerid = COMP->PLAYERN
			lGotHim = .t.
		elseif mPlayerid = COMP->PLAYERO
			lGotHim = .t.
		elseif mPlayerid = COMP->PLAYERP
			lGotHim = .t.
		elseif mPlayerid = COMP->PLAYERQ
			lGotHim = .t.
		elseif mPlayerid = COMP->PLAYERR
			lGotHim = .t.
		elseif mPlayerid = COMP->PLAYERS
			lGotHim = .t.
		elseif mPlayerid = COMP->PLAYERT
			lGotHim = .t.
		elseif mPlayerid = COMP->PLAYERU
			lGotHim = .t.
		elseif mPlayerid = COMP->PLAYERV
			lGotHim = .t.
		elseif mPlayerid = COMP->PLAYERW
			lGotHim = .t.
		elseif mPlayerid = COMP->PLAYERX
			lGotHim = .t.
		elseif mPlayerid = COMP->PLAYERY
			lGotHim = .t.
		elseif mPlayerid = COMP->PLAYERZ
			lGotHim = .t.
		elseif mPlayerid = COMP->IR1
			lGotHim = .t.
		elseif mPlayerid = COMP->IR2
			lGotHim = .t.
		elseif mPlayerid = COMP->IR3
			lGotHim = .t.
		elseif mPlayerid = COMP->IR4
			lGotHim = .t.
		endif


		if lGotHim
		   if mPlayerId = "JACKST02"
		      TestMsg( COMP->FRANCODE + " has got im " + mPlayerID )
		   endif
         if lShort
    			cFranCode = COMP->FRANCODE
         else
    			cFranCode = cComp + "." + COMP->FRANCODE
         endif
		else
		   if lShort
    			cFranCode = "--"
         else
    			cFranCode = cComp + ".--"
          endif
		endif

		skip
	enddo
		   if mPlayerId = "JACKST02"
		      TestMsg( " fran code is  " + cFranCode )
		   endif

	select( nOldArea )

RETURN cFranCode


function NoOfComps

	parameters mSeason

	LOCAL nComps, lastcomp, nOldArea

	nOldArea = select()

	nComps  = 0
	select COMP
	dbseek( mSeason )
	lastcomp = "XX"
	do while .not. eof() .and. COMP->SEASON = mSeason
		if COMP->LEAGUEID <> lastcomp
			INC( nComps )
			lastcomp         = COMP->LEAGUEID
		endif
		skip
	enddo

	select( nOldArea )

RETURN nComps


FUNCTION TeamsIn

	parameters mSeason, mLeagueid

	LOCAL nOldArea, fran_cnt

	fran_cnt = 0

	nOldArea = select()
	select COMP

	dbseek( mSeason + mLeagueid )
	do while .not. eof() .and. COMP->SEASON = mseason ;
								.and. COMP->LEAGUEID = mleagueid
		INC( fran_cnt )
		skip
	enddo

	select( nOldArea )

RETURN fran_cnt


FUNCTION TeamStr

	parameters mteamid, lDoShort, cSeason

	lDoShort = def_parm( 'lDoShort', 'L', NO       )
	cSeason  = def_parm( 'cSeason',  'C', G_Season )

	private oldarea, ret_val

	oldarea = select()

	select TEAM
	dbseek( cSeason + mteamid )

	if found()
		if lDoShort
			ret_val = TEAM->TEAMNAME
		else
			ret_val = left( trim( TEAM->CITY ) + ' ' +;
								 trim( TEAM->TEAMNAME ) + space(31), 31)
		endif

	else
		if lDoShort
			ret_val = replicate( ' ', len( TEAM->TEAMNAME ) )
		else
			ret_val = replicate( ' ', 31 )
		endif

	endif

	select( oldarea )

RETURN( ret_val )


FUNCTION Franchise

	parameters mSeason, mLeagueid, mOwnerid

	private oldarea, ret_val

	oldarea = select()

	select COMP
	dbseek( mSeason+mLeagueid+mOwnerid )

	if found()
		ret_val = COMP->FRANCHISE

	else
		ret_val = space(20)

	endif

	select( oldarea )

return ( ret_val )


FUNCTION Drafted( cPlayerId )

	LOCAL ret_val
	ret_val := ''

	if SERVE->( dbseek( cPlayerID ) )
		ret_val = SERVE->HOW
	endif

RETURN ret_val


FUNCTION CurrTeam( mplayerid, _date )

	LOCAL oldarea, ret_val, nOldRec

	*---Gives back the team code for a player at a particular date

	oldarea = select()
	ret_val = '??'

	select SERVE
	nOldRec = recno()
	dbseek( mplayerid	 )
	do while .not. eof() .and. SERVE->PLAYERID = mplayerid

		if empty( SERVE->TO )
			ret_val = SERVE->TEAMID
			exit

		elseif SERVE->FROM >= _date .and. SERVE->TO <= _date

			ret_val = SERVE->TEAMID
			exit

		endif

		skip
	enddo

	select SERVE
	go nOldRec

	select( oldarea )

RETURN ret_val


FUNCTION TeamInit( cSeason, cTeamID )

	LOCAL cInits

	*---Gives back the team initials code for a team ID

	cInits = '??'

	if TEAM->( dbseek( cSeason + cTeamID ) )
		cInits := TEAM->INITS
	endif


RETURN cInits


FUNCTION Opponent( cPlayerId, cWeek, cSeason )

	*---Returns the opponent that the player was facing in a particular week

	local cOpponent := space( 12 )
	local cHisTeam  := space(  2 )

	if SCHED->( dbSeek( cSeason + cWeek ) )
		cHisTeam := CurrTeam( cPlayerId, SCHED->GAMEDATE )
		do while SCHED->WEEK = cWeek .and. .not. SCHED->( eof() )
			if SCHED->AWAYTEAM == cHisTeam
				cOpponent := "@ " + left( TeamStr( SCHED->HOMETEAM, .t. ), 10 ) +;
								str( SCHED->AWAYSCORE, 5 ) + str( SCHED->HOMESCORE, 4 )
				exit
			elseif SCHED->HOMETEAM == cHisTeam
				cOpponent := "v " + left( TeamStr( SCHED->AWAYTEAM, .t. ), 10 ) +;
						str( SCHED->HOMESCORE, 5 ) + str( SCHED->AWAYSCORE, 4 )
				exit
			endif
			( SCHED->( dbSkip() ) )
		enddo
	endif

RETURN cOpponent


FUNCTION Opp( cPlayerId, cWeek, cSeason )

	*---Returns the opponent that the player was facing in a particular week
	*---Clients:	TYRPERF.PRG

	local cOpponent := " bye "
	local cHisTeam  := space(  2 )

	if SCHED->( dbSeek( cSeason + cWeek ) )
		cHisTeam := CurrTeam( cPlayerId, SCHED->GAMEDATE )
		do while SCHED->WEEK = cWeek .and. .not. SCHED->( eof() )
			if SCHED->AWAYTEAM == cHisTeam
				cOpponent := "@ " + SCHED->HOMETEAM + DOGind( SCHED->GAMEDATE )
				exit
			elseif SCHED->HOMETEAM == cHisTeam
				cOpponent := "v " + SCHED->AWAYTEAM + DOGind( SCHED->GAMEDATE )
				exit
			endif
			( SCHED->( dbSkip() ) )
		enddo
	endif

RETURN cOpponent


FUNCTION DOGInd( dGame )

	*---Day of the week indicator
	local cInd
	cInd := " "
	do case
	case dow( dGame ) = 2
		*---MNF
		cInd := "m"
	case dow( dGame ) = 4
		cInd := "w"
	case dow( dGame ) = 5
		cInd := "t"
	case dow( dGame ) = 6
		cInd := "f"
	case dow( dGame ) = 7
		cInd := "s"
	endcase

RETURN cInd


FUNCTION GameCode( cTeamId, cWeek, cSeason )

	*---Returns the Game Code {A-N} that the team was scheduled in

	local cOpponent := space( 12 )
	local cGameCode := space(  1 )

	if SCHED->( dbSeek( cSeason + cWeek ) )
		do while SCHED->WEEK = cWeek .and. .not. SCHED->( eof() )
			if SCHED->AWAYTEAM == cTeamId
				cOpponent := "@ " + left( TeamStr( SCHED->HOMETEAM, .t. ), 10 ) +;
							str( SCHED->AWAYSCORE, 5 ) + str( SCHED->HOMESCORE, 4 )
*				TestMsg( cOpponent )
				cGameCode := SCHED->GAMENO
				exit
			elseif SCHED->HOMETEAM == cTeamId
				cOpponent := "v " + left( TeamStr( SCHED->AWAYTEAM, .t. ), 10 ) +;
							str( SCHED->HOMESCORE, 5 ) + str( SCHED->AWAYSCORE, 4 )

*				TestMsg( cOpponent )
				cGameCode := SCHED->GAMENO
				exit
			endif
			( SCHED->( dbSkip() ) )
		enddo
	endif

RETURN( cGameCode )


FUNCTION GameDate( cWeek, cSeason )

	*---Returns the first date an NFL game was scheduled for a round

	local dGame := ctod( " " )

	if SCHED->( dbSeek( cSeason + cWeek ) )
		dGame := SCHED->GAMEDATE
	endif

RETURN( dGame )


FUNCTION PlayerName

	parameters mplayerid, lDoLong, lDoRole		&&  Could pass length of ret str?

	LOCAL oldarea, ret_val

	lDoLong = def_parm( 'lDoLong', 'L', .f. )
	lDoRole = def_parm( 'lDoRole', 'L', .f. )

	oldarea = select()

	select PLAYER
   if PLAYER->PLAYERID <> mPlayerID
  	   dbseek( mplayerid )
   endif

	if PLAYER->PLAYERID = mPlayerID
		if lDoLong
			ret_val = trim( PLAYER->FIRSTNAME ) + ' ' + trim( PLAYER->SURNAME )
		else
			ret_val = trim( PLAYER->SURNAME ) + ', ' + trim( PLAYER->FIRSTNAME ) + space(19)
			if lDoRole
				ret_val = left( ret_val, 17 ) + PLAYER->ROLE
			else
				ret_val = left( ret_val, 18 )
			endif
		endif
		if PLAYER->ROOKIEYR = val( G_season )
			ret_val = ret_val + "*"
		else
			ret_val = ret_val + " "
		endif

	else
		ret_val = space( 18 )

	endif

	select( oldarea )

RETURN( ret_val )


FUNCTION PlayerPos( mplayerid )

	local oldarea, ret_val

	oldarea = select()

	select PLAYER
	dbseek( mplayerid )

	if found()
		ret_val = PLAYER->CATEGORY

	else
		ret_val = '?'

	endif

	select( oldarea )

RETURN ret_val


FUNCTION Clip

	parameters nWins,  nLosses, nTies

	LOCAL nClip

	nClip = ( ( nWins * 2 ) + nTies ) / ( ( nWins + nLosses + nTies ) * 2 )

*	TestMsg( "Clip is " + str( nClip, 5, 3 ) )

RETURN( nClip )


FUNCTION NFLTeams( cSeason )

	LOCAL nTeams := 0

	if TEAM->( dbseek( cSeason ) )
		do while .not. TEAM->( eof() ) .and. TEAM->SEASON = cSeason
			nTeams++
			TEAM->( dbskip() )
		enddo
	endif

RETURN nTeams


FUNCTION CatOf( cPos )

	*---Returns the TFL category dpending on the Position description

	local nCat

	do case
	case cPos = "QB"
		nCat := 1
	case cPos = "FB"
		nCat := 2
	case cPos = "RB"
		nCat := 2
	case cPos = "HB"
		nCat := 2
	case cPos = "WR"
		nCat := 3
	case cPos = "TE"
		nCat := 3
	case cPos = "PR"
		nCat := 3
	case cPos = "K"
		nCat := 4
	case cPos = "PK"
		nCat := 4
	case cPos = "LB"
		nCat := 5
	case cPos = "ILB"
		nCat := 5
	case cPos = "OLB"
		nCat := 5
	case cPos = "MLB"
		nCat := 5
	case cPos = "NT"
		nCat := 5
	case cPos = "DL"
		nCat := 5
	case cPos = "DT"
		nCat := 5
	case cPos = "DE"
		nCat := 5
	case cPos = "DB"
		nCat := 6
	case cPos = "CB"
		nCat := 6
	case cPos = "S"
		nCat := 6
	case cPos = "FS"
		nCat := 6
	case cPos = "SS"
		nCat := 6
	case cPos = "C "
		nCat := 7
	case cPos = "OG"
		nCat := 7
	case cPos = "G "
		nCat := 7
	case cPos = "OT"
		nCat := 7
	case cPos = "OL"
		nCat := 7
	case cPos = "P"
		nCat := 1

	otherwise
		ErrorMsg( "Don know a category for " + cPos )
		nCat := 0
	endcase

RETURN str( nCat, 1 )


FUNCTION RoleOf( cRole )

	local cOut := ""
	do case
	case cRole = "S"
		cOut = "Starter"
	case cRole = "B"
		cOut = "Backup"
	case cRole = "R"
		cOut = "Reserve"
	case cRole = "D"
		cOut = "Deep Rsrve"
	case cRole = "I"
		cOut = "Injured"
	case cRole = "X"
		cOut = "Suspended"
	case cRole = "H"
		cOut = "Holdout"
	endcase

RETURN cOut



*EOF:  NFL_LIB.PRE
