*****************************************************************************
*  Program Name.....: plyrteam.prg 
*  Programmer.......: Steve Colonna 
*  System...........:
*  Module...........:
*  Date.............: 29/4/1990 at 20:01
*  Copyright........: (c) 1990 by Knoware 
*  Called From......:
*  Purpose..........:
*
*............................................................................
*  Revision.........: 1.0 Last Revised: 29/4/1990 at 20:01
*  Description......: Initial Creation.
*............................................................................
***************************** ALL RIGHTS RESERVED ***************************


#INCLUDE "nfl.h"

static dFromOld, dToOld, cHowOld := "          "

PROCEDURE PLYRTEAM

	parameters mplayerid

	local start, row, i, old_rec, ok, irow, iprompt
	private mteamid, dFrom, dTo, cHow

   set century on

   if PLAYER->( deleted() .or. eof() )
      error_msg( 'Please select a record.')
      RETU
   endif

	dFromOld := if( dFromOld = nil, date()-1, dFromOld )
	dToOld   := if( dToOld   = nil, date(),   dToOld   )

   setcolor( c_desc )
   head_line( "TEAM SERVICE")
	@ 24,00

	*---Draw scroll box
   popbox( 4, 0, 22, 79 )
   lineinbox( 6 )
	@ 5,  4 say "Team"
	@ 5, 39 say "From"
	@ 5, 49 say "To"
	@ 5, 60 say "How"

   setcolor( c_desc )

	*---Check for preexistence of children
   select SERVE
   dbseek( mplayerid )
   nokids = !found()
   if nokids
      error_msg( "No Teams are recorded for this Player", .f. )
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
         keybrowse(  7, 2, 21, 77, "BRW_SERV", mplayerid, @start )
         row = row()
		endif

      setcolor( c_desc )

      if lastkey() = INSERT .or. nokids  &&  Add is forced
         if !nokids
            setcolor( c_fields )
				@ row, 2 say BRW_SERV( 6 )
         endif

         setcolor( c_desc )

         start = 1
         old_rec = recno()
         if !add_rec( 5 )
            warning( "add_rec" )
            loop
         endif
         replace PLAYERID with mplayerid

         setcolor( c_help )
         popbox( 16, 5, 22, 75 )
         @ 16,6 say " Add another Team "

			if !GET_SERV( .t. )
*				testmsg( 'Escape hit' )
				select SERVE
				do RLOCKIT with "Team Service file"
            delete
            go old_rec
            if !( PLAYERID == mplayerid  ) .or. deleted()
               dbseek( mplayerid )
					if !found()
						*---User deleted the last one!
                  nokids = .t.
               endif
            endif
         else
				nokids = .f.

				start = CNT_SERVS( mplayerid )		&&		 + 1
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
			if !( PLAYERID == mplayerid )
				testmsg( 'miss match ' + PLAYERID + '<>' + mplayerid )
            start = 1
            loop
         endif
         @ row, 2  say BRW_SERV( 6 )

			if yes_no( 'Are you sure you wish to delete this record ?' )
				delete
				start = if( start > 1, start-1, 1 )
				skip -1
				if !( PLAYERID == mplayerid  )
					dbseek( mplayerid )
					if !found()
						exit
					endif
				endif
			endif

		elseif lastkey() = ENTER

         setcolor( c_fields )
         if !( PLAYERID == mplayerid )
            loop
         endif
         if !rec_lock( 5 )
            warning( "rec_lock" )
            loop
         endif
         FLD_SERV( 1 )
			if yes_no( 'Confirm that you want to alter this Team line' )
         	setcolor( c_help )
				popbox( 15, 5, 22, 75 )
	         @ 15,6 say " Edit this Team line "
				GET_SERV( .f. )
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

RETURN


function FLD_SERV( direction )

	local retval := .t.

	select SERVE

	if direction = 1
		*---load
		store SERVE->PLAYERID to mplayerid
		store TEAMID   to mteamid
		store FROM     to dFrom
		dTo := SERVE->TO
		store HOW      to cHow

	else
		if rec_lock( 5 )
			replace TEAMID    with mteamid
			replace FROM      with dFrom
			replace SERVE->TO with dTo
			replace HOW       with cHow

		else
			warning( "rec_lock" )
			retval = .f.
		endif
	endif

return( retval )


function BRW_SERV

	parameters mode

	private _prompt

	if mode = 5
*		retu( mplayerid )
		retu( PLAYERID )
	elseif mode = 6
		if !( PLAYERID == mplayerid )
			retu( "" )
		endif

		select SERVE
		*---length is
		_prompt = "  " + TeamWas( TEAMID, FROM ) +  ;
					 "  " + dtoc( FROM ) + "  " +;
					 dtoc( TO ) + space(2) + HOW + "  "
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

static ;
FUNCTION TeamWas( cTeam, dFrom )

	local cTeamOut

	if dFrom > ctod( "31/12/1989" )
		cTeamOut := teamstr( cTeam, .f., str( year( dFrom ), 4 ) )
	else
		cTeamOut := teamstr( cTeam, .f., "1990" )
	endif

RETURN cTeamOut


function GET_SERV( add )

	local ok, old_rec

   @ 18, 7 say "Team        |"
   @ 19, 7 say "From        :"
   @ 20, 7 say "To          :"
   @ 21, 7 say "How Aquired :"

	mteamid  := TEAMID
	dFrom    := if( add, iif( empty( dToOld), dFromOld, dToOld), SERVE->FROM )
	dTo      := if( !add .and. empty( SERVE->TO) , dToOld-1, SERVE->TO   )
	cHow     := if( add, cHowOld,   SERVE->HOW  )

	do while .t.

		vget(  18, 25, 'mteamid', '@K!' )
		vget(  19, 25, 'dFrom',   '@D'  )
		vget(  20, 25, 'dTo',     '@D'  )
		vget(  21, 25, 'cHow',    '@K!' )

		vread_cursor = .f.
		vread( 1, .f., "VAL_SERVS" )

		if abort()
			if !escape_ok()
				loop
			else
				*---Undo it
				ok = .f.
				exit
			endif
		else
			dFromOld :=	dFrom
			dToOld   :=	if( !add, dTo + 1, dToOld )
			cHowOld	:= if( add, cHow,     cHowOld )

		endif
		ok = FLD_SERV( 2 )				  && replaces
		exit
	enddo

return( ok .and. !abort() )


function VAL_SERVS

	parameters mvar

	priv ok, saveclr, lk

	saveclr = setcolor()
	if lastkey() = UPARROW
		retu( .t. )
	endif

*	&mvar = if( vget_exit = g_getlistkey, oflen( &mvar ), &mvar )
	ok = .t.
	do case
		case mvar == "dFrom"

			if ! TEAM->( dbseek( G_season + mTeamId ) )
				Error_msg( "Invalid team date combo" )
			endif

	endcase
	setcolor( saveclr )

return( ok )


function CNT_SERVS

	parameter mmplayerid

	private old_rec, _cnt

	old_rec = recno()
	_cnt = 0

	select SERVE
	dbseek( mplayerid )
	do while .not. eof() .and. PLAYERID = mplayerid
		_cnt = _cnt + 1
		skip
	enddo

*	testmsg( 'There are ' + ltrim( str( _cnt ) ) + ' Team lines' )

	go old_rec

return( _cnt )




*EOF:  PLYRTEAM.PRG