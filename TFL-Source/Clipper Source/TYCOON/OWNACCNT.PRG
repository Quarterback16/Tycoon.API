*****************************************************************************
*  Program Name.....: OWNACCNT.pre 
*  Programmer.......: SLC 
*  System...........:
*  Module...........:
*  Date.............: 18/7/1990 at 19:59
*  Copyright........: (c) 1990 by Knoware Pty Ltd 
*  Called From......:
*  Purpose..........:
*
*
*
*
*
*............................................................................
*  Revision.........: 1.0 Last Revised: 18/7/1990 at 19:59
*  Description......: Initial Creation.
*............................................................................
***************************** ALL RIGHTS RESERVED ***************************

#INCLUDE "TYCOON.H"

PROCEDURE OWNACCNT

	parameters mownerid

	private start, row, i, old_rec, ok, irow, iprompt
	private mteamid, mfrom, mto, mhow

   if OWNERS->( deleted() .or. eof() )
      error_msg( 'Please select a record.')
		retu	
	else
		mownerid = 	OWNERS->OWNERID
   endif

   setcolor( c_desc )
   head_line( "OWNERS ACCOUNT")
	@ 24,00

	*---Draw scroll box
   popbox( 4, 0, 22, 79 )
   lineinbox( 6 )
	@ 5,  8 say "Date"
	@ 5, 16 say "Desc"
	@ 5, 63 say "Amount"

   setcolor( c_desc )

	*---Check for preexistence of children
   select ACCNT
   seek mownerid
   nokids = !found()
   if nokids
      error_msg( "No Account entries are recorded for this Owner", .f. )
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
         keybrowse(  7, 4, 21, 71, "BRW_ACCNT", mownerid, @start )
         row = row()
		endif

      setcolor( c_desc )

      if lastkey() = INSERT .or. nokids  &&  Add is forced
         if !nokids
            setcolor( c_fields )
				@ row, 4 say BRW_ACCNT( 6 )
         endif

         setcolor( c_desc )

         start = 1
         old_rec = recno()
         if !add_rec( 5 )
            warning( "add_rec" )
            loop
         endif
         replace OWNERID with mownerid
         FLD_ACCNT( 1 )  && flds to mvars

         setcolor( c_help )
         popbox( 16, 5, 22, 75 )
         @ 16,6 say " Add another Account entry "

			if !get_ACCNT( .t. )
				select ACCNT
				do RLOCKIT with "Owners Accounts file"
            delete
            go old_rec
            if !( OwnerID == mownerid  ) .or. deleted()
               seek mownerid
					if !found()
						*---User deleted the last one!
                  nokids = .t.
               endif
            endif
         else
				nokids = .f.

				start = cnt_accnts( mownerid )		&&		 + 1
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
			if !( OwnerID == mownerid )
				testmsg( 'miss match ' + OwnerID + '<>' + mownerid )
            start = 1
            loop
         endif
         @ row, 4  say BRW_ACCNT( 6 )

			if yes_no( 'Are you sure you wish to delete this record ?' )
				delete
				start = if( start > 1, start-1, 1 )
				skip -1
				if !( OwnerID == mownerid  )
					seek mownerid
					if !found()
						exit
					endif
				endif
			endif

		elseif lastkey() = ENTER

         setcolor( c_fields )
         if !( OwnerID == mownerid )
            loop
         endif
         if !rec_lock( 5 )
            warning( "rec_lock" )
            loop
         endif
         FLD_ACCNT( 1 )
			if yes_no( 'Confirm that you want to alter this Account entry' )
         	setcolor( c_help )
				popbox( 16, 5, 22, 75 )
	         @ 16,6 say " Edit this Account entry "
				get_ACCNT( .f. )
	         unlock
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


function FLD_ACCNT

	parameter direction

	priv retval
	retval = .t.

	select ACCNT

	if direction = 1
		*---load
		store OWNERID  to mownerid
		store DATE     to mdate
		store DESC     to mdesc
		store AMOUNT   to mamount

	else
		if rec_lock( 5 )
			replace OWNERID   with mownerid
			replace DATE      with mdate
			replace DESC      with mdesc
			replace AMOUNT    with mamount

		else
			warning( "rec_lock" )
			retval = .f.
		endif
	endif

return( retval )


function BRW_ACCNT

	parameters mode

	private _prompt

	if mode = 5
		retu( mownerid )
	elseif mode = 6
		if !( OwnerID == mownerid )
			retu( "" )
		endif

		select ACCNT
		*---length is
		_prompt = "  " + dtoc( DATE ) + "  " + DESC + "  " +;
							 str( AMOUNT, 6, 2 ) + "   "
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


function GET_ACCNT

	parameter add

	private ok

   @ 18, 7 say "Date     :"
   @ 19, 7 say "Desc     :"
   @ 20, 7 say "Amount   :"

	if add
		mdate = date()
	else
		mdate = DATE
	endif
	mdesc     = DESC
	mamount   = AMOUNT

	do while .t.

		vget(  18, 25, 'mdate',   '@D'     )
		vget(  19, 25, 'mdesc',   '@X'     )
		vget(  20, 25, 'mamount', '999.99' )

		if !vread_test()
			testmsg( "vread error" )
			shutdown()
		endif

		vread_cursor = .f.
		vread( 1, .f., "VAL_ACCNTS" )

		if abort()
			if !escape_ok()
				loop
			else
				*---Undo it
				ok = .f.
				exit
			endif
		endif
		ok = FLD_ACCNT( 2 )				  && replaces
		exit
	enddo

return( ok .and. !abort() )


function VAL_ACCNTS

	parameters mvar

	priv ok, saveclr, lk

	saveclr = setcolor()
	if lastkey() = UPARROW
		retu( .t. )
	endif

*	&mvar = if( vget_exit = g_getlistkey, oflen( &mvar ), &mvar )
	ok = .t.
	do case
		case mvar == "mamount"
			
			if mamount > 100
				ok = yes_no( "Are you sure?" )
			endif

	endcase
	setcolor( saveclr )

return( ok )


function cnt_accnts

	parameter mmownerid

	private old_rec, _cnt

	old_rec = recno()
	_cnt = 0

	select ACCNT
	seek mownerid
	do while .not. eof() .and. OwnerID = mownerid
		_cnt = _cnt + 1
		skip
	enddo

*	testmsg( 'There are ' + ltrim( str( _cnt ) ) + ' Team lines' )

	go old_rec

return( _cnt )




*EOF:  OWNACCNT.PRE 

