* Program Name ............. : C:\TFL\TYCOON\OWNERS.PRG
* Author ................... :
* Date Revised ............. : May 16, 1990
* Calling Program .......... :
* General Routine .......... : No
* Report Forms Used ........ : None
* Description of Program ... : This program was intially created by a UI
*                              template.  FIUP.TEM
*
*............................................................................
*  Revision.........: 1.0 Last Revised: 24/9/1990 at 21:21
*  Description......: 
*............................................................................
*  Revision.........: 2.0 Last Revised: 21/05/95 at 21:29:39
*  Description......: Add Other contact (Internet address etc)
*.............................................................................
***************************** ALL RIGHTS RESERVED ***************************

#INCLUDE "TYCOON.H"

#DEFINE C_FIELDS c_fields
#DEFINE C_HELP   c_help
#DEFINE C_BLANK  c_blank

PROCEDURE OWNERS

private nmess, poption1
private mopt[10], mmess[10]

if !INIT_OWNR()											 && Initialisation routine
	do CLOSE_OWNR
	RETU
endif

do DISP_OWNR												 && Display Screen
poption1 = 3

do while poption1 <> 0

	do CLR_OWNR
	do FLDS_OWNR

	poption1 = flatmenu (mopt, mmess, poption1, 22, .T.)
	poption1 = if( poption1 = 0, 2, poption1)

	setcolor( C_BLANK )
	clrmenu()

	* where there's two 1st letters the same, make both strings long enough
	* to be unique

	do case
		case mopt[poption1] = 'P'

			up_skip( -1 )

		case mopt[poption1] = 'X'

			poption1 = 0

		case mopt[poption1] = 'B'

			do SRCH_OWNR

		case mopt[poption1] = 'I'

			do FIND_OWNR

		case mopt[poption1] = 'A'

			do ADD_OWNR

		case mopt[poption1] = 'E'

			do EDIT_OWNR

		case mopt[poption1] = 'V'

			do OWNACCNT

		case mopt[poption1] = 'D'

			if caned_OWNR()
				up_del()
			endif

		case mopt[poption1] = 'N'

			up_skip( 1 )

		case mopt[poption1] = 'L'

			select OWNERS
			up_list( 'OWNERS', "space( 4 )+ OWNERID +space(3)+ LASTNAME" )

	endcase

enddo

do CLOSE_OWNR

return														 && from C:\TFL\TYCOON\OWNERS.PRG

*===========================================================================*

function INIT_OWNR
	***
	***   Initialise all variables, arrays and open files
	***

	mopt[1]  = 'Previous'
	mopt[2]  = 'X-exit'
	mopt[3]  = 'Browse'
	mopt[4]  = 'Inquire'
	mopt[5]  = 'Add'
	mopt[6]  = 'Edit'
	mopt[7]  = 'Delete'
	mopt[8]  = 'List'
	mopt[9]  = 'ViewAccount'
	mopt[10]  = 'Next'

	mmess[1] = 'View Previous Record'
	mmess[2] = 'Return to Main Menu'
	mmess[3] = 'Browse Through All Records in the File'
	mmess[4] = 'Locate a Record'
	mmess[5] = 'Add A New Record'
	mmess[6] = 'Amend Details Displayed Currently'
	mmess[7] = 'Delete Record Displayed Currently'
	mmess[8] = 'List all the Records to the printer'
	mmess[9]  = 'View the owners Account'
	mmess[10] = 'View Next Record'


	*---Select Files to use
	if opendbf( "COMP", G_ty_path )
		if opendbf( "LEAGUE", G_ty_path )
			if opendbf( "OWNERS", G_ty_path )
			if opendbf( "LINEUP", G_ty_path )
			if opendbf( "PLAYER", G_nfl_path )

				select COMP
				set order to 1
				set relation to LEAGUEID into LEAGUE, to OWNERID into OWNERS
				go lastrec()
			endif
			endif
			endif
		endif
	endif

return( .t. )


procedure CLOSE_OWNR
	***
	***   Closing Down
	***

	close databases

RETURN


procedure FIND_OWNR
	***
	***     Allows search for a particular record
	***
	private  dkey, crec

	select OWNERS
	do CLR_OWNR
	setcolor( C_FIELDS )
	up_find( "OWNERID", 5,17 )

RETURN


procedure ADD_OWNR
	***
	***   Allows the addition of a record
	***

	select OWNERS
	do CLR_OWNR

	setcolor( C_FIELDS )
	if up_add( "OWNERID", 5,17 )
		*--- can also pass pict, valid udf, replace flag

		GET_OWNR( .t. )											 && returns abort()
		commit
		unlock
	endif

RETURN


procedure EDIT_OWNR
	***
	***   Allows amendments of fields displayed on the screen
	***

	select OWNERS
	setcolor( C_FIELDS )
	if !caned_OWNR()
		RETU
	endif

	if rec_lock(5)
		GET_OWNR( .f. )											 && returns abort()
		unlock
		commit
	else
		warning('rec_lock')
	endif

RETURN


function CANED_OWNR
	***
	*** types of records that can be edited
	***

RETURN up_canedit()


procedure SRCH_OWNR
	***
	***   Calls a routine to display a list of records in a file and
	***   allows for selection of one of those.
	***

	private s_fields[3], s_mess[3]

	s_fields[1] = "trim( LASTNAME ) + ', ' + FIRSTNAME"
	s_fields[2] = "OWNERID"
	s_fields[3] = "STATUS"

	s_mess[1]   = "Name"
	s_mess[2]   = "Id"
	s_mess[3]   = "Status"

	set order to 2											 &&  Description
	setcolor( C_HELP )
	TBL_SRCH( s_fields, s_mess )
	set order to 1

RETURN


procedure DISP_OWNR
	***
	***   Displays initial screen details
	***

	setcolor( C_BLANK )
	clear
	setcolor( c_desc )

	@  5, 8 say "Owner :"
	@ 11,17 say "(H)                (W)                 Fax"
	@ 12,13 say "eMail"
	@ 13,08 say "Status"
	@ 15,08 say "Fav Team"
*	@ 17,08 say "File"
	head_line('OWNERS')
	setcolor( c_flat )
	@  1,0 to  1,80										 && Single Lines
	@ 21,0 to 21,80

RETURN


procedure FLDS_OWNR
	***
	***   Displays the fields on the screen
	***
	setcolor( C_FIELDS )
	select OWNERS
	if !up_can()
		retu
	endif

	setcolor( C_FIELDS )
	@  5,17 say OWNERID
	@  5,26 say FIRSTNAME
	@  5,37 say LASTNAME
	@  5,53 say SALUTATION
	@  7,17 say ADDR1
	@  8,17 say ADDR2
	@  9,17 say SUBURB
	@  9,35 say STATE
	@  9,47 say POSTCODE
	@ 11,21 say HPHONENO
	@ 11,40 say WPHONENO
	@ 11,60 say FAX
	@ 12,21 say OTHER
	@ 13,17 say STATUS
	@ 15,17 say FAVTEAM
	@ 16,17 say FAVPLAYER

return


procedure CLR_OWNR
	***
	***   Clear the fields details from the screen
	***

	setcolor( C_BLANK )
	@  5,17 say space(5)
	@  5,26 say space(10)
	@  5,37 say space(15)
	@  5,53 say space(4)
	@  7,17 say space(40)
	@  8,17 say space(40)
	@  9,17 say space(15)
	@  9,35 say space(3)
	@  9,47 say space(5)
	@ 11,21 say space(14)
	@ 11,40 say space(14)
	@ 11,60 say space(14)
	@ 12,21 say space(40)
	@ 13,17 say space(3)
	@ 15,17 say space(2)
	@ 16,17 say space(8)
	@ 17,17 say space(8)

RETURN


function GET_OWNR
	***
	***   Allows entry of details
	***
	parameters adding
	private ;
	mownerid, mfirstname, mlastname, msalutatio, maddr1, maddr2, msuburb, ;
	mstate, mpostcode, mhphoneno, mwphoneno, mfax, mstatus, mfavteam, ;
	mfavplayer, mOther

	select OWNERS
	setcolor( C_FIELDS )

	mownerid   = OWNERID
	mfirstname = FIRSTNAME
	mlastname  = LASTNAME
	msalutatio = if( adding, 'Mr  ',   SALUTATION )
	maddr1     = ADDR1
	maddr2     = ADDR2
	msuburb    = SUBURB
	mstate     = if( adding, 'VIC',     STATE )
	mpostcode  = if( adding, '2    ',   POSTCODE )
	mhphoneno  = HPHONENO
	mwphoneno  = WPHONENO
	mfax       = FAX
	mOther     = OTHER
	mstatus    = if( adding, 'P  ', STATUS )
	mfavteam   = FAVTEAM
	mfavplayer = FAVPLAYER

	vget(  5,26, 'mfirstname', '',    .f. )
	vget(  5,37, 'mlastname',  '',    .f. )
	vget(  5,53, 'msalutatio', '',    .f. )
	vget(  7,17, 'maddr1',     '',    .f. )
	vget(  8,17, 'maddr2',     '',    .f. )
	vget(  9,17, 'msuburb',    '',    .f. )
	vget(  9,35, 'mstate',     '!!!', .f. )
	vget(  9,47, 'mpostcode',  '',    .f. )
	vget( 11,21, 'mhphoneno',  '',    .f. )
	vget( 11,40, 'mwphoneno',  '',    .f. )
	vget( 11,60, 'mfax',       '',    .f. )
	vget( 12,21, 'mOther',     '',    .f. )
	vget( 13,17, 'mstatus',    '!!!', .f. )
	vget( 15,17, 'mfavteam',   '!!',  .f. )
	vget( 16,17, 'mfavplayer', '',    .f. )

	vread( 1, .F., "VAL_OWNR" )
	if !abort()
		do REPL_OWNR
	endif

return !abort()


function VAL_OWNR

	parameter mvar
	priv ok
	ok = .t.

	do case

		case mvar == 'mlastname'
*			ok = DESC_CHEK( mlastname, 'LASTNAME' )

	endcase

return( ok )


procedure REPL_OWNR
	***
	***   Replace database fields with variables
	***
	select OWNERS
	replace OWNERID with mownerid
	replace FIRSTNAME with mfirstname
	replace LASTNAME with mlastname
	replace SALUTATION with msalutatio
	replace ADDR1 with maddr1
	replace ADDR2 with maddr2
	replace SUBURB with msuburb
	replace STATE with mstate
	replace POSTCODE with mpostcode
	replace HPHONENO with mhphoneno
	replace WPHONENO with mwphoneno
	replace FAX with mfax
	replace OTHER with mOther
	replace STATUS with mstatus
	replace FAVTEAM with mfavteam
	replace FAVPLAYER with mfavplayer
	commit

RETURN


*EOF:  C:\TFL\TYCOON\OWNERS.PRG
