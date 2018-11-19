*  Program Name.....: popplayr.pre
*  Programmer.......: Steve Colonna
*  System...........:
*  Module...........:
*  Date.............: 10/8/1990 at 20:01
*  Copyright........: (c) 1990 by Knoware
*  Called From......:
*  Purpose..........:
*
*	 To allow the Choice of a player and also to add on the fly.
*
*
*
*............................................................................
*  Revision.........: 1.0 Last Revised: 10/8/1990 at 20:01
*  Description......: Initial Creation.
*............................................................................
***************************** ALL RIGHTS RESERVED ***************************

#INCLUDE "nfl.h"


function PopPlayer

	parameters lHomeTeam, mplayerid

	private old_color, old_rec, mfields[ 3 ], mcols[ 3 ]

	if lHomeTeam
		cworkfile = "HOMETEAM"
	else
		cworkfile = "AWAYTEAM"
	endif

	select &cWorkFile

	if .not. empty( trim( mplayerid ) )
		set order to 2
		softseek( mplayerid )
		set order to 1
		if eof()
			go top
		endif
	else
		go top
	endif

	old_color  = setcolor(c_help)
	old_rec    = recno()

	mfields[ 1 ] = "trim( SURNAME )+', '+FIRSTNAME"
	mfields[ 2 ] = "PLAYERID"
	mfields[ 3 ] = "FTEAM"
	mcols[   1 ] = "Player"
	mcols[   2 ] = "ID"
	mcols[   3 ] = "G1"

	popbox( 8,41,20,78 )
	if empty_dbf()
		@ 12, 45 say 'File is empty!'
		inkey(0)
		mplayerid = NULL_STRING
	else
		dbedit(8+1,42,19,77,mfields,'onthefly','@X',mcols,"Ä"," ")
		mplayerid = PLAYERID
	endif
	setcolor(old_color)
	unpop()

return( mplayerid )


function OnTheFly

	parameters mode, fld_ptr

	private ret_mode

	do case
		case mode < 3
			ret_mode = 1									 && continue
		case mode = 3
			ret_mode = 0									 && exit
		case lastkey() = 27  &&  K_ESC

			go old_rec
			ret_mode = 0

		case lastkey() = 13

			ret_mode = 0

		case lastkey() = 22  &&  K_INS

			mplayerid = NULL_STRING
			pushgets()
			do ADDONFLY with mplayerid, mteam, LINEMAN_CAT
			popgets()

			ret_mode = 2

		case lastkey() = 287								 &&  Alt-S

			testmsg( 'Alt S pressed' )
			_oldrec = recno()
			thekey = mfields[1]
			_skey = oflen( &thekey )
			_len = len( _skey )
			_skey = getalph( 'Enter search code', _len )
			dbseek( trim( _skey ) )
			if eof()
				error_msg( 'Not Found!' )
				go _oldrec
			endif
			ret_mode = 2
		case lastkey() > 64 .and. lastkey() < 123
			*---Alpha key
			char = upper( chr( lastkey() ) )
			*			testmsg( 'Char = ' + char )
			set softseek on
			dbseek( char )
			set softseek off
			ret_mode = 2									 && continue and redraw
		case lastkey() > 47 .and. lastkey() < 58
			*---Numeric key
			char = upper( chr( lastkey() ) )
			*			testmsg( 'Char = ' + char )
			set softseek on
			dbseek( char )
			set softseek off
			ret_mode = 2									 && continue and redraw
		otherwise
			ret_mode = 1									 && continue
	endcase

return(ret_mode)
/*
	Structure for database : player.dbf
	Field  Field Name  Type       Width    Dec
	    1  PLAYERID    Character      8
	    2  SURNAME     Character     15
	    3  FIRSTNAME   Character     10
	    4  CURRTEAM    Character      2
	    5  HEIGHT_FT   Numeric        1      0
	    6  HEIGHT_IN   Numeric        2      0
	    7  WEIGHT      Numeric        3      0
	    8  FORTY       Numeric        6      2
	    9  COLLEGE     Character     15
	   10  ROOKIEYR    Numeric        4      0
	   11  POSDESC     Character     10
	   12  CATEGORY    Character      1
	   13  DOB         Date           8
	   14  CURRATING   Numeric        4      0
	   15  BIO         Memo          10

*/

procedure ADDONFLY

	parameters mplayerid, mteam, mcategory

	private msurname, mfirstname, nRookieYr, cHow, nForty

	mplayerid  = space(8)
	msurname   = space(20)
	mfirstname = space(10)
	mdob       = ctod( "  /  /  " )
	mweight    = 0
	mheight_ft = 6
	mheight_in = 0
	mcollege   = space( 15 )
	mposdesc   = space( 10 )
	nRookieYr  = val( G_season )
	nForty     = 0
	cHow       = space(15)
	adding     = .t.
	*---Popup a box

	popbox( 10, 10, 23, 70 )

	@ 10, 15 say "Add a player on the fly"
	@ 12, 20 say "Surname  : "
	@ 13, 20 say "First    : "
	@ 14, 20 say "Category : "
	@ 15, 20 say "Position : "
	@ 16, 20 say "Height Ft: "
	@ 17, 20 say "Height In: "
	@ 18, 20 say "Weight   : "
	@ 19, 20 say "DOB      : "
	@ 20, 20 say "Rookie Yr: "
	@ 21, 20 say "College  : "
	@ 22, 20 say "Speed    : "

	*---get info
	vget( 12,30, 'msurname',   '',       .f. )
	vget( 13,30, 'mfirstname', '',       .f. )
	vget( 14,30, 'mcategory',  '',       .f. )
	vget( 15,30, 'mposdesc',   '',       .f. )
	vget( 16,30, 'mheight_ft', '9',      .f. )
	vget( 17,30, 'mheight_in', '99',     .f. )
	vget( 18,30, 'mweight',    '999',    .f. )
	vget( 19,30, 'mdob',       '',       .f. )
	vget( 20,30, 'nRookieYr',  '9999',   .f. )
	vget( 20,40, 'cHow',       '@!',     .f. )
	vget( 21,30, 'mcollege',   '',       .f. )
	vget( 22,30, 'nForty',     '999.99', .f. )

	vread( 1, .F., "VAL_PLYR" )

	if !abort()
		*---Save to PLAYER
		select PLAYER
		do ADDRECIT with "Player File"

		mPlayerId := NewId( mSurname, mFirstname, .t. )

		replace PLAYERID  with mplayerid
		replace SURNAME   with msurname
		replace FIRSTNAME with mfirstname
		replace CATEGORY  with mcategory
*		replace CURRTEAM  with CurrTeam( mPlayerid, date() )
		replace CURRTEAM  with mTeam
		replace POSDESC   with mposdesc 
		replace HEIGHT_FT with mheight_ft
		replace HEIGHT_IN with mHeight_in
		replace COLLEGE   with mcollege
		replace DOB       with mdob
		replace WEIGHT    with mweight
		replace FORTY     with nForty 
		replace ROOKIEYR  with nRookieYr

		*---Save to team
		select &cWorkFile
		do ADDRECIT with "Team File"
		replace PLAYERID  with mplayerid
		replace SURNAME   with msurname
		replace FIRSTNAME with mfirstname
		replace CATEGORY  with mcategory

		*---Save to SERVE
		select SERVE
		do ADDRECIT with "Service File"
		replace PLAYERID  with mplayerid
		replace TEAMID    with mteam
		replace FROM      with ctod( "01/09/" + substr( dtoc(SCHED->GAMEDATE), 7, 2) )
		replace HOW       with cHow

		if mcategory = KICKER_CAT
			if lHomeTeam
				cHomeKicker = mplayerid
			else
				cAwayKicker = mplayerid
			endif
		elseif mcategory = QUARTERBACK_CAT
			if lHomeTeam
				cHomeQB = mplayerid
			else
				cAwayQB = mplayerid
			endif
		endif

	endif

	unpop()

	select &cWorkFile

return


*EOF:  POPPLAYR.PRE

