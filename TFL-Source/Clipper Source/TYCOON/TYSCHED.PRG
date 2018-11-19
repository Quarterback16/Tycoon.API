*****************************************************************************
*  Program Name.....: tysched.pre 
*  Programmer.......: Steve Colonna 
*  System...........:
*  Module...........:
*  Date.............: 25/9/1990 at 20:06
*  Copyright........: (c) 1990 by Knoware 
*  Called From......:
*  Purpose..........:
*
*............................................................................
*  Revision.........: 1.0 Last Revised: 25/9/1990 at 20:06
*  Description......: Initial Creation.
*............................................................................
***************************** ALL RIGHTS RESERVED ***************************

#INCLUDE "TYCOON.H"

#DEFINE C_FIELDS c_fields
#DEFINE C_HELP   c_help
#DEFINE C_BLANK  c_blank

PROCEDURE TYSCHED

private nmess, poption1, mopt[8], mmess[8]
private mseason, mleagueid, mweek
private mHt1, mHt2, mHt3, mHt4, mHt5, mHt6, mHt7, mHt8
private mAt1, mAt2, mAt3, mAt4, mAt5, mAt6, mAt7, mAt8

if !INIT_SKED()											 && Initialisation routine
	do CLOSE_SKED
	retu
endif

do DISP_SKED												 && Display Screen
poption1 = 3

do while poption1 <> 0

	do CLR_SKED
	do FLDS_SKED

	poption1 = flatmenu (mopt, mmess, poption1, 22, .T.)
	poption1 = if( poption1 = 0, 2, poption1)

	clrmenu()

	* where there's two 1st letters the same, make both strings long enough
	* to be unique

	do case
		case mopt[poption1] = 'P'

			up_skip( -1 )

		case mopt[poption1] = 'X'

			poption1 = 0

		case mopt[poption1] = 'B'

			do SRCH_SKED

		case mopt[poption1] = 'I'

			do FIND_SKED

		case mopt[poption1] = 'A'

			do ADD_SKED

		case mopt[poption1] = 'E'

			do EDIT_SKED

		case mopt[poption1] = 'L'

			mseason   = TYSKED->SEASON
			mleagueid = TYSKED->LEAGUEID
			mOwnerid  = TYSKED->OWNERID
			do LINEUPS with mseason+mleagueid+mOwnerid

		case mopt[poption1] = 'D'

			if caned_SKED()
				up_del()
			endif

		case mopt[poption1] = 'N'

			up_skip( 1 )

		case mopt[poption1] = 'R'

			select TYSKED
			up_list( 'TYSKED', "space( 4 )+ OWNERID +space(3)+ LASTNAME" )

	endcase

enddo

do CLOSE_SKED

return														 && from C:\TFL\TYCOON\TYSKED.PRG

*===========================================================================*

function INIT_SKED
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
	mopt[8]  = 'Next'

	mmess[1] = 'View Previous Record'
	mmess[2] = 'Return to Main Menu'
	mmess[3] = 'Browse Through All Records in the File'
	mmess[4] = 'Locate a Record'
	mmess[5] = 'Add A New Record'
	mmess[6] = 'Amend Details Displayed Currently'
	mmess[7] = 'Delete Record Displayed Currently'
	mmess[8] = 'View Next Record'

	mcontinu = .t.
	*---Select Files to use
	if opendbf( "TYSKED", G_ty_path )
		if opendbf( "LEAGUE", G_ty_path )
			if opendbf( "OWNERS", G_ty_path )
				if opendbf( "COMP", G_ty_path )

					select TYSKED
					set order to 1
					set relation to LEAGUEID into LEAGUE
					go lastrec()
				else
					mcontinu = .f.
				endif
			else
				mcontinu = .f.
			endif
		else
			mcontinu = .f.
		endif
	else
		mcontinu = .f.
	endif

return( mcontinu )


procedure CLOSE_SKED
	***
	***   Closing Down
	***

	close databases

return														 && from CLOSE_SKED


procedure FIND_SKED
	***
	***     Allows search for a particular record
	***
	private dkey, crec

	select TYSKED
	do CLR_SKED
	setcolor( C_FIELDS )

   if empty_dbf()
      error_msg( 'No records found to view')
      RETU
   endif  
   
   crec = recno()
   go lastrec()+1

   do while .t.

      setcolor( c_fields )
		vget(  3,17, 'mseason',    '',    .f. )
		vget(  4,17, 'mleagueid',  '',    .f. )
		vget(  6,17, 'mweek',      '',    .f. )

		vread( 1, .F., "VAL_SKED" )

      if abort() 
         go crec
         RETU
      endif

      dbseek( mSeason+mLeagueId+mWeek )

      if found()
         RETU
      else
         error_msg('Record could not be found')
         go crec
      endif

   enddo


return


procedure ADD_SKED
	***
	***   Allows the addition of a record
	***

	select TYSKED
	do CLR_SKED

	setcolor( C_FIELDS )
	priv crec

	crec = recno()
	go lastrec()
	mseason    = G_season
	mleagueid  = LEAGUEID
	mweek      = WEEK

	do while .t.											 && read loop

		vget(  3,17, 'mseason',    '',    .f. )
		vget(  4,17, 'mleagueid',  '',    .f. )
		vget(  6,17, 'mweek',      '',    .f. )

		vread( 1, .F., "VAL_SKED" )
		if abort()
			go crec
			retu
		endif

		dkey = mseason+mleagueid+mweek

		dbseek( dkey )
		if found()
			error_msg( 'Record already exists ' + dkey )
		elseif !add_rec(5)
			warning( "add_rec" )
		else
			replace SEASON   with mseason
			replace LEAGUEID with mleagueid
			replace WEEK     with mweek
			exit
		endif

	enddo


	GET_SKED( .t. )										 && returns abort()
	commit
	unlock


return														 && from add


procedure EDIT_SKED
	***
	***   Allows amendments of fields displayed on the screen
	***

	select TYSKED
	setcolor( C_FIELDS )
	if !caned_SKED()
		retu
	endif

	if rec_lock(5)
		GET_SKED( .f. )											 && returns abort()
		unlock
		commit
	else
		warning('rec_lock')
	endif

return


function CANED_SKED
	***
	*** types of records that can be edited
	***

return( up_canedit() )


procedure SRCH_SKED
	***
	***   Calls a routine to display a list of records in a file and
	***   allows for selection of one of those.
	***

	private s_fields[3], s_mess[3]

	s_fields[1] = "SEASON"
	s_fields[2] = "LEAGUE->LEAGUE"
	s_fields[3] = "WEEK"

	s_mess[1]   = "Season"
	s_mess[2]   = "League"
	s_mess[3]   = "Week"

	setcolor( C_HELP )
	TBL_SRCH( s_fields, s_mess )

return														 &&   from SRCH_SKED


procedure DISP_SKED
	***
	***   Displays initial screen details
	***

	clear_scn()
	setcolor( c_desc )

	@  3, 8 say "Season :"
	@  4, 8 say "League :"

	@  6, 8 say "Week   :"

	@ 10, 9 say  "1"
	@ 11, 9 say  "2"
	@ 12, 9 say  "3"
	@ 13, 9 say  "4"
	@ 14, 9 say  "5"
	@ 15, 9 say  "6"
	@ 16, 9 say  "7"
	@ 17, 9 say  "8"

	head_line('TYSKED')
	setcolor( c_flat )
	@  1,0 to  1,80										 && Single Lines
	@ 21,0 to 21,80

RETURN


procedure FLDS_SKED
	***
	***   Displays the fields on the screen
	***
	setcolor( C_FIELDS )
	select TYSKED
	if !up_can()
		RETU
	endif

	setcolor( C_FIELDS )
	@  3,17 say SEASON
	@  4,17 say LEAGUE->LEAGUE
	@  6,17 say WEEK

	mseason   = SEASON
	mleagueid = LEAGUEID
	mweek     = WEEK

	@ 10, 11 say iif( empty( HT1 ), space(20), Franchise( SEASON, LEAGUEID, HT1 ) )
	@ 11, 11 say iif( empty( HT2 ), space(20), Franchise( SEASON, LEAGUEID, HT2 )	)
	@ 12, 11 say iif( empty( HT3 ), space(20), Franchise( SEASON, LEAGUEID, HT3 )	)
	@ 13, 11 say iif( empty( HT4 ), space(20), Franchise( SEASON, LEAGUEID, HT4 )	)
	@ 14, 11 say iif( empty( HT5 ), space(20), Franchise( SEASON, LEAGUEID, HT5 )	)
	@ 15, 11 say iif( empty( HT6 ), space(20), Franchise( SEASON, LEAGUEID, HT6 )	)
	@ 16, 11 say iif( empty( HT7 ), space(20), Franchise( SEASON, LEAGUEID, HT7 )	)
	@ 17, 11 say iif( empty( HT8 ), space(20), Franchise( SEASON, LEAGUEID, HT8 )	)

	@ 10, 51 say iif( empty( HT1 ), space(20), Franchise( SEASON, LEAGUEID, AT1 )	)
	@ 11, 51 say iif( empty( HT2 ), space(20), Franchise( SEASON, LEAGUEID, AT2 )	)
	@ 12, 51 say iif( empty( HT3 ), space(20), Franchise( SEASON, LEAGUEID, AT3 )	)
	@ 13, 51 say iif( empty( HT4 ), space(20), Franchise( SEASON, LEAGUEID, AT4 )	)
	@ 14, 51 say iif( empty( HT5 ), space(20), Franchise( SEASON, LEAGUEID, AT5 )	)
	@ 15, 51 say iif( empty( HT6 ), space(20), Franchise( SEASON, LEAGUEID, AT6 )	)
	@ 16, 11 say iif( empty( HT7 ), space(20), Franchise( SEASON, LEAGUEID, AT7 )	)
	@ 17, 11 say iif( empty( HT8 ), space(20), Franchise( SEASON, LEAGUEID, AT8 )	)

	*---Scores
	@ 10, 36 say iif( empty( HT1 ), space(4),  HS1 )
	@ 11, 36 say iif( empty( HT2 ), space(4),  HS2 )
	@ 12, 36 say iif( empty( HT3 ), space(4),  HS3 )
	@ 13, 36 say iif( empty( HT4 ), space(4),  HS4 )
	@ 14, 36 say iif( empty( HT5 ), space(4),  HS5 )
	@ 15, 36 say iif( empty( HT6 ), space(4),  HS6 )
	@ 16, 36 say iif( empty( HT7 ), space(4),  HS7 )
	@ 17, 36 say iif( empty( HT8 ), space(4),  HS8 )

	@ 10, 75 say iif( empty( HT1 ), space(4),  AS1 )
	@ 11, 75 say iif( empty( HT2 ), space(4),  AS2 )
	@ 12, 75 say iif( empty( HT3 ), space(4),  AS3 )
	@ 13, 75 say iif( empty( HT4 ), space(4),  AS4 )
	@ 14, 75 say iif( empty( HT5 ), space(4),  AS5 )
	@ 15, 75 say iif( empty( HT6 ), space(4),  AS6 )
	@ 16, 75 say iif( empty( HT7 ), space(4),  AS7 )
	@ 17, 75 say iif( empty( HT8 ), space(4),  AS8 )

RETURN


procedure CLR_SKED
	***
	***   Clear the fields details from the screen
	***

	setcolor( C_BLANK )
	@  3,17 
	@  4,17 
	@  6,17 

	@ 10,11 say space( 36 )
	@ 11,11 say space( 36 )
	@ 12,11 say space( 36 )
	@ 13,11 say space( 36 )
	@ 14,11 say space( 36 )
	@ 15,11 say space( 36 )
	@ 16,11 say space( 36 )
	@ 17,11 say space( 36 )

	@ 10, 51 say space( 28 )
	@ 11, 51 say space( 28 )
	@ 12, 51 say space( 28 )
	@ 13, 51 say space( 28 )
	@ 14, 51 say space( 28 )
	@ 15, 51 say space( 28 )
	@ 16, 51 say space( 28 )
	@ 17, 51 say space( 28 )

RETURN


function GET_SKED
	***
  	***   Allows entry of details
	***
	parameters adding

	select TYSKED
	setcolor( C_FIELDS )

	store HT1 to mht1
	store AT1 to mat1
	store HT2 to mht2
	store AT2 to mat2
	store HT3 to mht3
	store AT3 to mat3
	store HT4 to mht4
	store AT4 to mat4
	store HT5 to mht5
	store AT5 to mat5
	store HT6 to mht6
	store AT6 to mat6
	store HT7 to mht7
	store AT7 to mat7
	store HT8 to mht8
	store AT8 to mat8


* 	vget(  3,17, 'mseason',    '',    .f. )
* 	vget(  4,17, 'mleagueid',  '',    .f. )
* 	vget(  6,17, 'mweek',   '',    .f. )

	vget( 10,11, 'mHt1', '',    .f. )
	vget( 10,51, 'mAt1', '',    .f. )

	vget( 11,11, 'mHt2', '',    .f. )
	vget( 11,51, 'mAt2', '',    .f. )

	vget( 12,11, 'mHt3', '',    .f. )
	vget( 12,51, 'mAt3', '',    .f. )

	vget( 13,11, 'mHt4', '',    .f. )
	vget( 13,51, 'mAt4', '',    .f. )

	vget( 14,11, 'mHt5', '',    .f. )
	vget( 14,51, 'mAt5', '',    .f. )

	vget( 15,11, 'mHt6', '',    .f. )
	vget( 15,51, 'mAt6', '',    .f. )

	vget( 16,11, 'mHt7', '',    .f. )
	vget( 16,51, 'mAt7', '',    .f. )

	vget( 17,11, 'mHt8', '',    .f. )
	vget( 17,51, 'mAt8', '',    .f. )

	vread( 1, .F., "VAL_SKED" )

	do REPL_SKED

return( .t. )


function VAL_SKED

	parameter mvar

	private ok
	ok = .t.

	do case

		case mvar == 'mleagueid'

			*---Pop up options
	 		ok = poptable2( @mleagueid, "LEAGUE", "LEAGUEID", "LEAGUE","ID",;
	 		                "   League", "C", 1, .t., 30, .f. )

		case mvar = 'mAt' .or. mvar = 'mHt'

			*---Pop up options
			mOwnerid = &mvar
			if .not. empty( mownerid )
*				mownerid = space(5)
				ok = keyedtable( @mownerid, "COMP", mseason+mleagueid,;
					 "SEASON+LEAGUEID", "OWNERID","FRANCHISE",;
	 		                "Owner id", "Franchise", 1 )

				store mownerid to &mvar

			endif

	endcase

return( ok )


procedure REPL_SKED
	***
	***   Replace database fields with variables
	***
	select TYSKED
	replace SEASON    with mseason
	replace LEAGUEID  with mleagueid
	replace WEEK      with mweek
	replace HT1       with mht1
	replace AT1       with mat1
	replace HT2       with mht2
	replace AT2       with mat2
	replace HT3       with mht3
	replace AT3       with mat3
	replace HT4       with mht4
	replace AT4       with mat4
	replace HT5       with mht5
	replace AT5       with mat5
	replace HT6       with mht6
	replace AT6       with mat6
	replace HT7       with mht7
	replace AT7       with mat7
	replace HT8       with mht8
	replace AT8       with mat8

	commit

return														 &&   from get_SKED


*EOF:  TYSCHED.PRE
