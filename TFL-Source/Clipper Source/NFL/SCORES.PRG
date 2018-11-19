*****************************************************************************
*  Program Name.....: SCORES.prg
*  Programmer.......: Steve Colonna
*  System...........:
*  Module...........:
*  Date.............: 29/4/1990 at 20:01
*  Copyright........: (c) 1990 by Knoware
*  Called From......:
*  Purpose..........: Does the Line Scores
*
*............................................................................
*  Revision.........: 1.0 Last Revised: 29/4/1990 at 20:01
*  Description......: Initial Creation.
*
*                     Win2K  - doesnt like $ in the alias name, changed
*                     TEMPORARY_FILE_INDICATOR from "$" to "z"
*............................................................................
***************************** ALL RIGHTS RESERVED ***************************

#INCLUDE "nfl.h"

#DEFINE		MAIN_BOX_TOP		8
#DEFINE		EDIT_BOX_TOP		14

PROCEDURE SCORES

parameters mSeason, mWeek, mGameno

private start, row, i, old_rec, ok, irow, iprompt
private cHomeQB, cHomeKicker, cAwayQB, cAwayKicker
private cHomeRuner, cAwayRuner, cHomeRecvr, cAwayRecvr
private mscore, mWhen, mPlayerid1, mPlayerId2, mTeam

	store NULL_STRING to cHomeQB, cHomeKicker, cAwayQB, cAwayKicker
	store NULL_STRING to cHomeRuner, cHomeRecvr, cAwayRuner, cAwayRecvr

	cGameKey = mSeason + mWeek + mGameno

   if SCHED->( deleted() .or. eof() )
      error_msg( 'Please select a record.')
		retu
	else
		do RLOCKIT with "NFL Schedule"
   endif  && no record to edit

	select 0
	OpenDbf( 'COMP', g_ty_path )
	
	*select 0
	*OpenDbf( 'SCORE', g_nfl_path )
	*select 0
	*OpenDbf( 'STAT', g_nfl_path )

   setcolor( c_desc )
   head_line( "SCORE")
	@ 24,00

	*---Load Players
	LoadPlayers()

	*---Draw scroll box
   popbox( MAIN_BOX_TOP, 0, 22, 79 )
   lineinbox( MAIN_BOX_TOP+2 )
	@ MAIN_BOX_TOP+1,  5 say "When"
	@ MAIN_BOX_TOP+1, 12 say "Team"
	@ MAIN_BOX_TOP+1, 17 say "Player"
	@ MAIN_BOX_TOP+1, 37 say "How"
	@ MAIN_BOX_TOP+1, 49 say "Dist"
	@ MAIN_BOX_TOP+1, 54 say "Player 2"

   setcolor( c_desc )

	*---Check for preexistence of children
   select SCORE
   dbseek( cGameKey )
   nokids = !found()
   if nokids
      error_msg( "No scores recorded for this Game", .f. )
   endif
   start = 1

   do while .t.

		if !nokids
			***
			***   Whats the score
			***
			ShowScore( nAwayScore, nHomeScore )
			*---Show update options
         setcolor( c_desc )
         @ 23, 0
         @ 23,14 say "INS to Add"
         @ 23,26 say "DEL to Delete"
         @ 23,43 say "ENTER to Edit"
         @ 23,59 say "ESC to Exit"

         setcolor( c_fields )
         keybrowse(  MAIN_BOX_TOP+3, 2, 21, 73, "BRW_SCOR", cGameKey, @start )
         row = row()
		endif

      setcolor( c_desc )

		if lastkey() = INSERT .or. nokids  &&  Add is forced
			***
			***   Add
			***
         if !nokids
            setcolor( c_fields )
				@ row, 2 say BRW_SCOR( 6 )
         endif

         setcolor( c_desc )

         start = 1
         old_rec = recno()
         if !add_rec( 5 )
            warning( "add_rec" )
            loop
			endif

			*---Defaults
			replace SEASON   with mSeason
			replace WEEK     with mWeek
			replace GAMENO   with mGameNo
			replace SCORE    with SCORE_FIELD_GOAL
			replace DISTANCE with 1
         FLD_SERV( 1 )  && flds to mvars


         setcolor( c_help )
         popbox( EDIT_BOX_TOP, 5, 22, 75 )
         @ EDIT_BOX_TOP,6 say " Add another Score "

			if !GET_SCOR( .t. )
				select SCORE
				do RLOCKIT with "Scores file"
            delete
            go old_rec
            if !( SEASON+WEEK+GAMENO == cGameKey  ) .or. deleted()
               dbseek( cGameKey )
					if !found()
						*---User deleted the last one!
                  nokids = .t.
               endif
            endif
         else
				nokids = .f.
				***
				***   Automatic Add PAT
				***

				if mScore <> SCORE_PAT .and. ;
					mScore <> SCORE_FIELD_GOAL .and. ;
					mScore <> SCORE_SAFETY

					select SCORE
					do ADDRECIT with "Scores file"
					replace SEASON    with mseason
					replace WEEK      with mweek
					replace GAMENO    with mgameno
					replace WHEN      with mwhen
					replace SCORE     with SCORE_PAT
					replace TEAM      with mteam
					if mTeam = SCHED->HOMETEAM
						replace PLAYERID1 with cHomeKicker
					else
						replace PLAYERID1 with cAwayKicker
					endif
*					commit
					unlock

				endif

				start = CNT_SCORS( cGameKey )
				if start > 21 - MAIN_BOX_TOP
					start = 21 - MAIN_BOX_TOP
				endif
         endif
         unpop()
         unlock
         if nokids
            exit
         endif

		elseif lastkey() = DEL

         if !rec_lock( 5 )
            warning( "rec_lock" )
            loop
         endif
         setcolor( c_fields )
         setcolor( getcolor( 2 ) )
			if !( SEASON+WEEK+GAMENO == cGameKey )
				testmsg( 'miss match ' + SEASON+WEEK+GAMENO + '<>' + cGameKey )
            start = 1
            loop
         endif
         @ row, 2  say BRW_SCOR( 6 )

			if yes_no( 'Are you sure you wish to delete this record ?' )
				delete
				start = if( start > 1, start-1, 1 )
				skip -1
				if !( SEASON+WEEK+GAMENO == cGameKey  )
					dbseek( cGameKey )
					if !found()
						exit
					endif
				endif
			endif

		elseif lastkey() = ENTER

         setcolor( c_fields )
         if !( SEASON+WEEK+GAMENO == cGameKey )
            loop
         endif
         if !rec_lock( 5 )
            warning( "rec_lock" )
            loop
         endif
         FLD_SCOR( 1 )
			if yes_no( 'Confirm that you want to alter this Score' )
         	setcolor( c_help )
				popbox( EDIT_BOX_TOP, 5, 22, 75 )
	         @ EDIT_BOX_TOP,6 say " Edit this Score line "
				GET_SCOR( .f. )
	         unlock
   	      unpop()
			endif

      elseif lastkey() = ESCAPE
         exit
      endif
   enddo

	unpop()

	setcolor( c_desc )
	@ 23,00

   select COMP
   use

   *---Update team scores
	WriteScore( nHomeScore, nAwayScore )

RETURN


FUNCTION SwitchWhen( mWhen )

   *---Format Q-MM:SS
   local nMinute, nSec, cWhen

   cWhen := mWhen
   if .not. empty( mWhen )
      nMinute := val( substr( mWhen, 3, 2 ) )
 *     tESTMsg( "nMin=" + str(nMinute ) )
      nSec    :=  val( substr( mWhen, 6, 2 ) )
      nSec := 60 - nSec
      if nSec = 60
        nSec := 0
        nMinute := 15-nMinute
      else
        nMinute := 14-nMinute
      endif

      cWhen := substr(mWhen, 1, 2 ) + str( nMinute, 2 ) + ":" + strzero( nSec, 2 )
*      Testmsg( cWhen )
   endif

RETURN cWhen


FUNCTION WriteScore( nHomeScore, nAwayScore )

	*---Update scores
	replace SCHED->HOMESCORE with nHomeScore
	replace SCHED->AWAYSCORE with nAwayScore

RETURN nil


function FLD_SCOR

	parameter direction

	priv retval
	retval = .t.

	select SCORE

	if direction = 1
		*---load
		store SEASON+WEEK+GAMENO to cGameKey
		store WHEN      to mwhen
		store TEAM      to mteam
		store SCORE     to mscore
		store DISTANCE  to mdist
		store PLAYERID1 to mplayerid1
		store PLAYERID2 to mplayerid2

	else
		if rec_lock( 5 )
         *---Switch times around?
         if direction = 2
            mWhen := SwitchWhen( mWhen )
            *TestMsg( "mWhen=" + mWhen + " dir " + str( direction ) )
         endif

			replace WHEN      with mwhen
			replace TEAM      with mteam
			replace SCORE     with mscore
			replace DISTANCE  with mdist
			replace PLAYERID1 with mplayerid1
			replace PLAYERID2 with mplayerid2

		else
			warning( "rec_lock" )
			retval = .f.
		endif
	endif

return( retval )


function BRW_SCOR

	parameters mode

	private _prompt, cPlayer1, cPlayer2, cDist

   SELECT score
	if mode = 5
*		retu( cGameKey )
		retu( SEASON+WEEK+GAMENO )
	elseif mode = 6
		if !( SEASON+WEEK+GAMENO == cGameKey )
			retu( "" )
		endif

		select SCTYPE
		dbseek( SCORE->SCORE )
		select SCORE
		if !empty( PLAYERID1 )
			cPlayer1 = PlayerName( PLAYERID1 )
		else
			cPlayer1 = space(19)
		endif
		if !empty( PLAYERID2 )
			cPlayer2 = PlayerName( PLAYERID2 )
		else
			cPlayer2 = space(19)
		endif
		cDist = if( DISTANCE = 0, '   ', STR( DISTANCE, 3, 0 ) )

		_prompt = "  " + WHEN + "  " + TEAM + "  " + cPlayer1 +;
					 " " + left( SCTYPE->SCOREDESC, 11 ) + space( 1 ) +;
					 cDist + space(2) +;
					 cplayer2 + " "

		retu( _prompt )

	elseif mode = 3
		retu( 0 )
	elseif mode = 0
		if lastkey() = ENTER  .or.;
			lastkey() = INSERT .or.;
			lastkey() = DEL
			retu( 0 )
		endif
	endif

return( 1 )


function GET_SCOR

	parameter add

	private ok, old_rec

   @ EDIT_BOX_TOP+2, 7 say "When         :"
   @ row()+1,        7 say "Team         :"
   @ row()+1,        7 say "How          |"
   @ row()+1,        7 say "Player       :"
   @ row()+1,        7 say "Distance     :"
   @ row()+1,        7 say "Player 2     :"

	select SCORE
	mwhen      = WHEN
	mscore     = SCORE
	mdist      = DISTANCE
	mplayerid1 = PLAYERID1
	mplayerid2 = PLAYERID2
	if add
		mteam  = SCHED->HOMETEAM
	else
		mteam  = TEAM
	endif

	do while .t.

		vget( EDIT_BOX_TOP+2, 25, 'mWhen',      'X-XX:XX' )
		vget( EDIT_BOX_TOP+3, 25, 'mTeam',      '!!'      )
		vget( EDIT_BOX_TOP+4, 25, 'mScore',     '!'       )
		vget( EDIT_BOX_TOP+5, 25, 'mPlayerId1', '@K!'     )
		cvget( "!(mScore$'S12N')", EDIT_BOX_TOP+6, 25, 'mDist',      '999' )
		cvget( "mScore$'P2'", EDIT_BOX_TOP+7, 25, 'mPlayerId2', '@K!'     )

		vget_exit( .t., g_getlistkey )
		vread_cursor = .f.
		vread( 1, .f., "VAL_SCORS" )
		vget_exit( .f., g_getlistkey )

		if abort()
			if !escape_ok()
				loop
			else
				*---Undo it
				ok = .f.
				exit
			endif
		endif

		ok = FLD_SCOR( iif( add, 2, 3 ) )				  && replaces
		exit
	enddo

return( ok .and. !abort() )


function VAL_SCORS

	parameters mvar

	private ok, saveclr, lk

	saveclr = setcolor()
	if lastkey() = UPARROW
		retu( .t. )
	endif

	if vget_exit = g_getlistkey						 && list key pressed last
		if mvar $ "mScore"
			&mvar = oflen( &mvar )
		endif
	endif

	ok = .t.
	do case
		case mvar == "mWhen"
		case mvar == "mPlayerId1"

			if mScore <> SCORE_SAFETY .or. (mScore == SCORE_SAFETY .and. !empty(mPlayerid1) )
				if mTeam = SCHED->HOMETEAM
					mPlayerid1 = popPlayer( .t., mPlayerId1 )
				else
					mPlayerid1 = popPlayer( .f., mPlayerId1  )
				endif
			endif

			if mScore == SCORE_FIELD_GOAL .or. mScore == SCORE_PAT
				if mTeam = SCHED->HOMETEAM
					cHomeKicker = mPlayerId1
				else
					cAwayKicker = mPlayerId1
				endif
			elseif mScore == SCORE_TD_RUN
				if mTeam = SCHED->HOMETEAM
					cHomeRuner = mPlayerId1
				else
					cAwayRuner = mPlayerId1
				endif

			elseif mScore == SCORE_TD_PASS
				if mTeam = SCHED->HOMETEAM
					cHomeRecvr = mPlayerId1
				else
					cAwayRecvr = mPlayerId1
				endif

			endif

		case mvar == "mPlayerId2"

			if mTeam = SCHED->HOMETEAM
				mPlayerid2 = popPlayer( .t., mPlayerId2  )
			else
				mPlayerid2 = popPlayer( .f., mPlayerId2  )
			endif

			if mTeam = SCHED->HOMETEAM
				cHomeQB = mPlayerId2
			else
				cAwayQB = mPlayerId2
			endif

		case mvar == "mTeam"

			if .not. (mTeam $ SCHED->HOMETEAM + SCHED->AWAYTEAM )
				ok = .f.
			endif

		case mvar == "mScore"
			*---Score Type
			mplayerid2 = space(8)
*			if empty( mScore ) .or. lastkey() = g_getlistkey
				ok = poptable2( @mScore, "SCTYPE", "SCORECODE",;
					 "SCOREDESC", "Code", "Desc", "C", 1, .f., 0, .F. )

				if mScore == SCORE_FIELD_GOAL .or. ;
					mScore == SCORE_PAT
					if mTeam = SCHED->HOMETEAM
						mplayerid1 = cHomeKicker
					else
						mplayerid1 = cAwayKicker
					endif
				elseif mScore == SCORE_TD_PASS
					if mTeam = SCHED->HOMETEAM
						mplayerid1 = cHomeRecvr
						mplayerid2 = cHomeQB
					else
						mplayerid1 = cAwayRecvr
						mplayerid2 = cAwayQB
					endif
				endif
				if mScore = SCORE_TD_RUN
					mDistance = 1
					if mTeam = SCHED->HOMETEAM
						mplayerid1 = cHomeRuner
					else
						mplayerid1 = cAwayRuner
					endif
* 				elseif mScore = SCORE_SAFETY
* 					mDistance = 0
* 				elseif mScore = SCORE_FIELD_GOAL
* 					mDistance = 25

				endif

*			endif


      case mvar == 'mDistance'

			ok = if( mdistance < 110 .and. mDistance >= 0, .t., .f. )

	endcase
	setcolor( saveclr )

RETURN ok


function CNT_SCORS

	private old_rec, nCount

	old_rec = recno()
	nCount = 0
	store 0 to nCount, nHomeScore, nAwayScore

	select SCORE
	dbseek( cGameKey )
	do while .not. eof() .and. SEASON+WEEK+GAMENO = cGameKey
		nCount++
		*---Also add score
		if TEAM = SCHED->HOMETEAM
			ADDTO( nHomeScore, PtsFor( SCORE ) )
		else
			ADDTO( nAwayScore, PtsFor( SCORE ) )
		endif
		skip
	enddo

*	testmsg( 'There are ' + ltrim( str( nCount ) ) + ' Team lines' )

	go old_rec

return( nCount )


FUNCTION TheScore( cGameKey )

	local nOldArea

	store 0 to nHomeScore, nAwayScore

	nOldArea = select()

	select SCORE
	dbseek( cGameKey )
	do while .not. eof() .and. SEASON+WEEK+GAMENO = cGameKey

		if SCORE->TEAM = SCHED->HOMETEAM
			ADDTO( nHomeScore, PtsFor( SCORE ) )
		else
			ADDTO( nAwayScore, PtsFor( SCORE ) )
		endif
		skip
	enddo

	select( nOldArea )

RETURN .t.


function ShowScore

	parameters nAwayScore, nHomeScore

	print(  5, 35, str( nAwayScore, 3 ), HIWHITE_ON_BLACK )
	print(  5, 70, str( nHomeScore, 3 ), HIWHITE_ON_BLACK )

return ( .t. )


function PtsFor

	parameters _score

	private _pts, nOldArea

	nOldArea = select()

	select SCTYPE
	dbseek( _score )
	if found()
		_pts = NFLPOINTS
	else
		_pts = 0
	endif

	select( nOldArea )

return( _pts )



function LoadPlayers

	*---Write Home Team
	if lLoaded
		*---Already loaded
		retu(.t.)
	endif

	cHomeFile = "HOMETEAM"
	MakeTemp( cHomeFile )
	OpenTemp( cHomeFile, "HOMETEAM" )

	select SERVE
	set order to SERVE_BY_TEAM

	nHomePlyrs = 0
	dbseek( SCHED->HOMETEAM )
	do while .not. eof() .and. TEAMID = SCHED->HOMETEAM
		if ( SERVE->FROM <= SCHED->GAMEDATE .and. SCHED->GAMEDATE <= SERVE->TO ) .or. ;
			empty( SERVE->TO )
			INC( nHomePlyrs )
			cPlayerCat = PlayerPos( SERVE->PLAYERID )

         if empty(PLAYER->SURNAME)
            TestMsg( "Home Player ID " + SERVE->PLAYERID + " invalid" )
            WriteLog( 'Error', "Bad Serve record " + SERVE->PLAYERID + ;
                               ">" + SCHED->HOMETEAM )

         else

			   select HOMETEAM
			   do ADDRECIT with "Home Team"
			   replace PLAYERID  with SERVE->PLAYERID
			   replace SURNAME   with PLAYER->SURNAME
			   replace FIRSTNAME with PLAYER->FIRSTNAME
			   replace CATEGORY  with cPlayerCat
			   replace FTEAM     with PlaysFor( SERVE->PLAYERID, "G1", G_Season, .t. )

            if PLAYER->ROLE = "S"
			      if cPlayerCat = QUARTERBACK_CAT
				      cHomeQB = SERVE->PLAYERID
			      elseif cPlayerCat = KICKER_CAT
				      cHomeKicker = SERVE->PLAYERID
			      elseif cPlayerCat = RUNNINGBACK_CAT
				      cHomeRuner = SERVE->PLAYERID
			      elseif cPlayerCat = RECEIVER_CAT
				      cHomeRecvr = PLAYERID
			      endif
            endif
         endif
		endif

		select SERVE
		skip
	enddo

*	testmsg( LSTR( nHomePlyrs, 3, 0 ) + " Home players" )

	*---Write Away team
	cAwayFile = "AWAYTEAM"
	MakeTemp( cAwayFile )

	OpenTemp( cAwayFile, "AWAYTEAM" )

	select SERVE
	set order to SERVE_BY_TEAM

	nAwayPlyrs = 0
	dbseek( SCHED->AWAYTEAM )
	do while .not. eof() .and. TEAMID = SCHED->AWAYTEAM
		if SERVE->FROM <= SCHED->GAMEDATE .and. SCHED->GAMEDATE <= SERVE->TO .or. ;
			empty( SERVE->TO )

			INC( nAwayPlyrs )
			cPlayerCat = PlayerPos( PLAYERID )

         if empty( PLAYER->SURNAME )
            TestMsg( "Away Player ID " + SERVE->PLAYERID + " invalid" )
            WriteLog( 'Error', "Bad Serve record " + SERVE->PLAYERID + ;
                               ">" + SCHED->AWAYTEAM )

         else
			   select AWAYTEAM
			   do ADDRECIT with "Away Team"
			   replace PLAYERID  with SERVE->PLAYERID
			   replace SURNAME   with PLAYER->SURNAME
			   replace FIRSTNAME with PLAYER->FIRSTNAME
			   replace CATEGORY  with cPlayerCat
			   replace FTEAM     with PlaysFor( SERVE->PLAYERID, "G1", G_Season, .t. )

            if PLAYER->ROLE = "S"
			      if cPlayerCat = QUARTERBACK_CAT
				      cAwayQB = PLAYERID
			      elseif cPlayerCat = KICKER_CAT
				      cAwayKicker = PLAYERID
			      elseif cPlayerCat = RUNNINGBACK_CAT
				      cAwayRuner = PLAYERID
			      elseif cPlayerCat = RECEIVER_CAT
				      cAwayRecvr = PLAYERID
			      endif
		      endif

         endif

		endif

		select SERVE
		skip
	enddo

*	testmsg( LSTR( nAwayPlyrs, 3, 0 ) + " Away players" )
	lLoaded = .t.

return( .t. )


function OPenTemp

	parameters cFilename, cAlias

	select 0
	do while !net_use( G_nfl_path + cFilename, .t., 5, cAlias )
		error_msg( "Problems opening " + cFileName )
	enddo
	index on SURNAME  to ( G_nfl_path + cFilename )
	index on PLAYERID to ( G_nfl_path + left( cfilename, 4 ) + "X2" )
	set index to ( G_nfl_path + cFilename ), ( G_nfl_path + left( cfilename, 4 ) + "X2" )

return( .t. )


function MakeTemp

	parameters cFilename

 	do MAKEFILE with cfilename, 5, ;
 							'PLAYERID   C  8  ',;
 							'FTEAM      C  2  ',;
 							'SURNAME    C 20 0',;
 							'FIRSTNAME  C 10 0',;
 							'CATEGORY   C	1 0'

return( .t. )



*EOF:  SCORES.PRG


