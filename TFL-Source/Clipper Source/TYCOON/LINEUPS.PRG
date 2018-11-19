*****************************************************************************
*  Program Name.....: lineups.pre 
*  Programmer.......: Steve Colonna 
*  System...........:
*  Module...........:
*  Date.............: 10/9/1990 at 20:08
*  Copyright........: (c) 1990 by Knoware 
*  Called From......:
*  Purpose..........:
*
*............................................................................
*  Revision.........: 1.0 Last Revised: 10/9/1990 at 20:08
*  Description......: Initial Creation.
*............................................................................
*  Revision.........: 2 Last Revised: 30/08/92 at 13:12:28
*  Description......: 1992 lINEUPS 12 starters
*.............................................................................
*  Revision.........: 3 Last Revised: 05/09/93 at 20:30:07
*  Description......: Garbage bug fix
*.............................................................................
***************************** ALL RIGHTS RESERVED ***************************

#INCLUDE "TYCOON.H"

#DEFINE   KEY_EXPRESSION   SEASON+LEAGUEID+OWNERID
#DEFINE   BOX_START_ROW    7
#DEFINE   BOX_START_COL    5
#DEFINE   BOX_END_ROW      23
#DEFINE   BOX_END_COL      75
#DEFINE   MANY_ALIAS       LINEUP
#DEFINE   MANY_ENTITY      "LINEUP"

PROCEDURE LINEUPS

	parameters cKey

	private start, row, i, old_rec, ok, irow, iprompt
	private roster_[ 20 ], player_[ 20 ]

   if COMP->( deleted() .or. eof() )
      error_msg( 'Please select a record.')
      retu
   endif  && no record to edit

   setcolor( c_desc )
   head_line( MANY_ENTITY + "S" )
	@ 24,00

	*---Draw scroll box
   popbox( 4, 0, 22, 79 )
   lineinbox( 6 )
	@ 5, 39 say "Player"

   setcolor( c_desc )

	*---Load Roster for validation
	LoadRoster()

	*---Check for preexistence of children
   select MANY_ALIAS
   seek cKey
   nokids = !found()
   if nokids
      error_msg( "No Line ups are recorded for this Franchise", .f. )
   endif
   start = 1

   do while .t.

		if !nokids
			*---Show update options
         setcolor( c_desc )
         @ 23, 0
         @ 23,14 say "INS to Add"
         @ 23,26 say "DEL to Delete"
         @ 23,43 say "ENTER to Edit"
         @ 23,59 say "ESC to Exit"

         setcolor( c_fields )
         keybrowse(  7, 2, 21, 73, "BRW_LU", cKey, @start )
         row = row()
		endif

      setcolor( c_desc )

      if lastkey() = INSERT .or. nokids  &&  Add is forced
         if !nokids
            setcolor( c_fields )
				@ row, 2 say BRW_LU( 6 )
         endif

         setcolor( c_desc )

         start = 1
         old_rec = recno()
         if !add_rec( 5 )
            warning( "add_rec" )
            loop
			endif

			*---Write key
			replace OWNERID  with mOwnerId
			replace LEAGUEID with mLeagueId
			replace SEASON   with mSeason

			FLD_LU( 1 )  && flds to mvars

         setcolor( c_help )
         popbox( BOX_START_ROW, BOX_START_COL, BOX_END_ROW, BOX_END_COL )
         @ BOX_START_ROW,BOX_START_COL + 3 say " Add another " + MANY_ENTITY 

			if !GET_LU( .t. )
				testmsg( 'Escape hit' )
				select MANY_ALIAS
				do RLOCKIT with MANY_ENTITY + " file"
            delete
            go old_rec
            if !( KEY_EXPRESSION == cKey ) .or. deleted()
               seek cKey
					if !found()
						*---User deleted the last one!
                  nokids = .t.
               endif
            endif
         else
				nokids = .f.

				start = CNT_LUS( cKey )		&&		 + 1
				if start > 15
					start = 15
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
			if !( KEY_EXPRESSION == cKey )
				testmsg( 'miss match ' + KEY_EXPRESSION + '<>' + cKey )
            start = 1
            loop
         endif
         @ row, 2  say BRW_LU( 6 )

			if yes_no( 'Are you sure you wish to delete this record ?' )
				delete
				start = if( start > 1, start-1, 1 )
				skip -1
				if !( KEY_EXPRESSION == cKey )
					seek cKey
					if !found()
						exit
					endif
				endif
			endif

		elseif lastkey() = ENTER

         setcolor( c_fields )
         if !( KEY_EXPRESSION == cKey )
            loop
         endif
         if !rec_lock( 5 )
            warning( "rec_lock" )
            loop
         endif
         FLD_LU( 1 )
			if yes_no( 'Confirm that you want to alter this ' + MANY_ENTITY )
         	setcolor( c_help )
	         popbox( BOX_START_ROW, BOX_START_COL, BOX_END_ROW, BOX_END_COL )
   	      @ BOX_START_ROW,BOX_START_COL + 3 say " Edit this " + MANY_ENTITY
				GET_LU( .f. )
	         unlock

				if yes_no( "Carry", YES )
					CarryEm()
				endif

   	      unpop()
			endif

      elseif lastkey() = K_ESC
         exit
      endif
   enddo

	unpop()

	setcolor( c_desc )
	@ 23,00

return


function CarryEm

	private i

	store SEASON   to mseason
	store LEAGUEID to mleagueid
	store OWNERID  to mownerid
	store WEEK     to mweek
	store PLAYER1  to mplayer1
	store PLAYER2  to mplayer2
	store PLAYER3  to mplayer3
	store PLAYER4  to mplayer4
	store PLAYER5  to mplayer5
	store PLAYER6  to mplayer6
	store PLAYER7  to mplayer7
	store PLAYER8  to mplayer8
	store PLAYER9  to mplayer9
	store PLAYER10 to mplayer10
	store PLAYER11 to mplayer11
	store PLAYER12 to mplayer12

	for i = val( mweek )+1 to 17
		seek mseason+mleagueid+mownerid+right( "0" + ltrim( str( i, 2 ) ), 2 )
		if eof()
			do ADDRECIT with "Lineup file"
		else
			do RLOCKIT with "Lineup file"
		endif

		replace SEASON   with mseason
		replace LEAGUEID with mleagueid
		replace OWNERID  with mownerid
		replace WEEK     with right( "0" + ltrim( str( i, 2 ) ), 2 )
		replace PLAYER1  with mplayer1
		replace PLAYER2  with mplayer2
		replace PLAYER3  with mplayer3
		replace PLAYER4  with mplayer4
		replace PLAYER5  with mplayer5
		replace PLAYER6  with mplayer6
		replace PLAYER7  with mplayer7
		replace PLAYER8  with mplayer8
		replace PLAYER9  with mplayer9
		replace PLAYER10 with mplayer10
		replace PLAYER11 with mplayer11
		replace PLAYER12 with mplayer12
	next

return ( .t. )


function FLD_LU

	parameter direction

	private retval
	retval = .t.

	select MANY_ALIAS

	if direction = 1
		*---load

		store WEEK     to mweek
		store PLAYER1  to mplayer1
		store PLAYER2  to mplayer2
		store PLAYER3  to mplayer3
		store PLAYER4  to mplayer4
		store PLAYER5  to mplayer5
		store PLAYER6  to mplayer6
		store PLAYER7  to mplayer7
		store PLAYER8  to mplayer8
		store PLAYER9  to mplayer9
		store PLAYER10 to mplayer10
		store PLAYER11 to mplayer11
		store PLAYER12 to mplayer12

	else
		if rec_lock( 5 )
			replace WEEK     with mweek
			replace PLAYER1  with mplayer1
			replace PLAYER2  with mplayer2
			replace PLAYER3  with mplayer3
			replace PLAYER4  with mplayer4
			replace PLAYER5  with mplayer5
			replace PLAYER6  with mplayer6
			replace PLAYER7  with mplayer7
			replace PLAYER8  with mplayer8
			replace PLAYER9  with mplayer9
			replace PLAYER10 with mplayer10
			replace PLAYER11 with mplayer11
			replace PLAYER12 with mplayer12

		else
			warning( "rec_lock" )
			retval = .f.
		endif
	endif

return( retval )


function BRW_LU

	parameters mode

	private _prompt

	if mode = 5
		retu( KEY_EXPRESSION )
*		retu( cKey )		causes garbage bug
	elseif mode = 6
		select MANY_ALIAS
		if !( KEY_EXPRESSION == cKey )
			retu( NULL_STRING )
		endif

		select MANY_ALIAS
		*---length is
		_prompt = "  " + WEEK + "   " + PlayerName( PLAYER1 ) + space(46)
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


function GET_LU

	parameter add

	private ok, old_rec

	if G_season >= "1992"
		@ 09, 7 say "Week   :"
		@ 10, 7 say "QB     :"
		@ 11, 7 say "R1     :"
		@ 12, 7 say "R2     :"
		@ 13, 7 say "P1     :"
		@ 14, 7 say "P2     :"
		@ 15, 7 say "K      :"
		@ 16, 7 say "L1     :"
		@ 17, 7 say "L2     :"
		@ 18, 7 say "L3     :"
		@ 19, 7 say "S1     :"
		@ 20, 7 say "S2     :"
		@ 21, 7 say "S3     :"
	else
		@ 10, 7 say "Week   :"
		@ 11, 7 say "QB     :"
		@ 12, 7 say "QB2    :"
		@ 13, 7 say "R1     :"
		@ 14, 7 say "R2     :"
		@ 15, 7 say "P1     :"
		@ 16, 7 say "P2     :"
		@ 17, 7 say "K      :"
		@ 18, 7 say "L1     :"
		@ 19, 7 say "L2     :"
		@ 20, 7 say "S1     :"
		@ 21, 7 say "S2     :"
	endif

	mweek		 = WEEK    
	mplayer1	 = PLAYER1 
	mplayer2	 = PLAYER2 
	mplayer3	 = PLAYER3 
	mplayer4	 = PLAYER4 
	mplayer5	 = PLAYER5 
	mplayer6	 = PLAYER6 
	mplayer7	 = PLAYER7 
	mplayer8	 = PLAYER8 
	mplayer9	 = PLAYER9 
	mplayer10 = PLAYER10
	mplayer11 = PLAYER11
	mplayer12 = PLAYER12

	do while .t.

		if G_season >= "1992"
			vget(  09, 25, 'mweek',     '@K!' )
			vget(  10, 25, 'mplayer1',  '@K!' )
			vget(  11, 25, 'mplayer2',  '@K!' )
			vget(  12, 25, 'mplayer3',  '@K!' )
			vget(  13, 25, 'mplayer4',  '@K!' )
			vget(  14, 25, 'mplayer5',  '@K!' )
			vget(  15, 25, 'mplayer6',  '@K!' )
			vget(  16, 25, 'mplayer7',  '@K!' )
			vget(  17, 25, 'mplayer8',  '@K!' )
			vget(  18, 25, 'mplayer9',  '@K!' )
			vget(  19, 25, 'mplayer10', '@K!' )
			vget(  20, 25, 'mplayer11', '@K!' )
			vget(  21, 25, 'mplayer12', '@K!' )
		else
			vget(  10, 25, 'mweek',     '@K!' )
			vget(  11, 25, 'mplayer1',  '@K!' )
			vget(  12, 25, 'mplayer2',  '@K!' )
			vget(  13, 25, 'mplayer3',  '@K!' )
			vget(  14, 25, 'mplayer4',  '@K!' )
			vget(  15, 25, 'mplayer5',  '@K!' )
			vget(  16, 25, 'mplayer6',  '@K!' )
			vget(  17, 25, 'mplayer7',  '@K!' )
			vget(  18, 25, 'mplayer8',  '@K!' )
			vget(  19, 25, 'mplayer9',  '@K!' )
			vget(  20, 25, 'mplayer10', '@K!' )
			vget(  21, 25, 'mplayer11', '@K!' )
		endif

		if !vread_test()
			testmsg( "vread error" )
			shutdown()
		endif

		TURN_ON_POPUP_KEY
		vread_cursor = .f.
		vread( 1, .f., "VAL_LUS" )
		TURN_OFF_POPUP_KEY

		if abort()
			if !escape_ok()
				loop
			else
				*---Undo it
				ok = .f.
				exit
			endif
		endif
		ok = FLD_LU( 2 )				  && replaces
		exit
	enddo

return( ok .and. !abort() )


function VAL_LUS

	parameters mvar

	private ok, saveclr, lk

	saveclr = setcolor()
	if lastkey() = UPARROW
		retu( .t. )
	endif

* 	if mvar = "mplayer"
* 		&mvar = oflen( &mvar )
* 	endif
	ok = .t.
	do case
		case mvar = "mplayer"

			if ! empty( &mvar )
				nRow = row()

				if len( trim( &mvar ) ) = 6
					&mvar = trim( &mvar ) + "01"
				endif

				nPlayer =  ascan( roster_, &mvar )

				if !empty( nPlayer )
					&mvar = roster_[ nPlayer ]

					select PLAYER
					seek &mvar
					if eof()
						ok = .f.
						error_msg( "Invalid Code" )
					else
						@ nRow, 37 say Player_[ nPlayer ]
					endif

				else
					ok = .f.
					error_msg( "Player not on roster" )
				endif

			endif

	endcase
	setcolor( saveclr )

return( ok )


function CNT_LUS

	parameter cKey

	private old_rec, _cnt

	old_rec = recno()
	_cnt = 0

	select MANY_ALIAS
	seek cKey
	do while .not. eof() .and. KEY_EXPRESSION = cKey
		INC( _cnt )
		skip
	enddo

	go old_rec

return( _cnt )


function LoadRoster

	private i

	roster_[  1 ] = COMP->PLAYERA
	roster_[  2 ] = COMP->PLAYERB
	roster_[  3 ] = COMP->PLAYERC
	roster_[  4 ] = COMP->PLAYERD
	roster_[  5 ] = COMP->PLAYERE
	roster_[  6 ] = COMP->PLAYERF
	roster_[  7 ] = COMP->PLAYERG
	roster_[  8 ] = COMP->PLAYERH
	roster_[  9 ] = COMP->PLAYERI
	roster_[ 10 ] = COMP->PLAYERJ
	roster_[ 11 ] = COMP->PLAYERK
	roster_[ 12 ] = COMP->PLAYERL
	roster_[ 13 ] = COMP->PLAYERM
	roster_[ 14 ] = COMP->PLAYERN
	roster_[ 15 ] = COMP->PLAYERO
	roster_[ 16 ] = COMP->PLAYERP
	roster_[ 17 ] = COMP->PLAYERQ
	roster_[ 18 ] = COMP->PLAYERR
	roster_[ 19 ] = COMP->PLAYERS
	roster_[ 20 ] = COMP->PLAYERT

	for i = 1 to 20
		player_[ i ] = PlayerName( roster_[ i ] )
	next


return( NIL )

*EOF:  LINEUPS.PRE

