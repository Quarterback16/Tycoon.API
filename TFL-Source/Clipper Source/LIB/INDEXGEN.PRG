***************************** ALL RIGHTS RESERVED ***************************
* Program Name: INDEXGEN.PRG
* Author:   SC
* Copyright (c) 1989 by
*-----------------------------------------------------------------------------
* Created: 6/16/1989 at 12:22
* main =
* Called From:
* --- Data Base Files ---   ----- Index Files -----   ----- Other Files ----
*
*
*
*.............................................................................
*
*#   INDEXGEN is a generic Pack & reindex Housekeeping module driven by a
*    Index control file that describes all indexes used.  It is written
*    so that any index changes can be recorded in the Index control file
*    and there will be no need to change this program.
*
*#
*.............................................................................
*  Revision.........: 1.0 Last Revised: 18/3/1990 at 17:59
*  Description......: Initial Creation.
*............................................................................
*---------------------------- ALL RIGHTS RESERVED ----------------------------

#INCLUDE "TFL.H"

FIELD DBFNAME, ALIASNAME, NTX_ORDER, INDEXNAME, INDEXKEY, FILENAME, GROUP

procedure indexGen

parameter mpathname

if pcount() = 0
	mpathname =""
endif

set exact on
standard(2)		&&		Set Standard Funcky colour to Green on Black

private key, old_color, gfactor, num_recs

old_color = setcolor(c_blank)
gfactor   = 10
clear_scn()
close databases	&&  Clear all work areas

head_line( 'HOUSE KEEPING', G_shrtnme + ' ('+ G_version + ')')
@ 21,0 TO 21,80
@ 1,0  TO 1,80    && Single Lines

setcolor( c_desc )
text

          House keeping purges deleted records from files and re-builds
          all associated index files.

endtext

private mopt[4], mmess[4], poption1

mopt[1]  = 'X-exit'
mopt[2]  = 'All'
mopt[3]  = 'Group'
mopt[4]  = 'Individual'

mmess[1] = 'Return to Main Menu'
mmess[2] = 'Pack and reindex All files'
mmess[3] = 'Pack and reindex a particular Group of files'
mmess[4] = 'Pack and reindex an Individual file'

poption1 = 2
do while poption1 <> 0

	poption1 = flatmenu( mopt, mmess, poption1, 22 )

	do case

		case poption1 = 0

		*---stops execution of other cases causing subscript error on Escape

		case mopt [poption1] = 'X-exit'

			poption1 = 0

		case mopt [poption1] = 'All'

			do INDEXALL with mpathname

		case mopt [poption1] = 'Group'

			do INDEXGRP with mpathname

		case mopt [poption1] = 'Individual'

			do INDEXIND with mpathname

	endcase

enddo


close data
set exact off
setcolor(old_color)

return


procedure INDEXALL

	parameter mpathname

	if pcount() = 0
		mpathname =""
	endif

	*---Open Index Control file
	select 0
	if net_use( mpathname + 'INDEXES', .f., 5, 'INDEXES')
 		ntx1 =  mpathname + 'INDEXIND.NTX'
		if .not. file( ntx1 )
			dispmsg(23,'Rebuilding index file ' + ntx1,.F.,1,.t.)
			index on upper(DBFNAME)+NTX_ORDER to ( ntx1 )
		endif
	else
		error_msg( 'Cant open Index control File' )
		retu
	endif
	oldcolr = setcolor()
   set index to &mpathname.indexind

	do while .not. eof()
		*---Open DBF
		mdbfname = upper( DBFNAME)
		mfilename = TRIM(FILENAME)
		openedok = .f.
		do OPENFILE with mpathname, mdbfname, mfilename, openedok
		if openedok
			select INDEXES
			do while upper( DBFNAME) = mdbfname .and. .not. eof()
				mntx_order = NTX_ORDER
				mindexname = INDEXNAME
				mindexkey  = INDEXKEY
				mfilename  = FILENAME
				do PAKINDEX with mpathname, mindexkey, mindexname, mfilename
				select INDEXES
				skip
			enddo
		else
			error_msg( 'Could not open ' + mpathname + mdbfname )
			select INDEXES
			do while upper( DBFNAME) = mdbfname .and. .not. eof()
				select INDEXES
				skip
			enddo
		endif
	enddo

	do CLR_MESS
   close databases

return


procedure CLR_MESS

	if lastkey() <> ESCAPE
		set color to
		@ 23,00
		error_msg( "Packing and Reindexing Complete" )
	endif
	setcolor(oldcolr)

return


procedure PAKINDEX

	*---Assumes an open DBF file with no index set

	parameters mpathname, mindexkey, mindexname, mfilename

	mindexname = mpathname+mindexname

	box( 6, 10, 19, 70, "", 2, 3, 8 )
	print( 08, 15, "Reindexing file     :", 2 )
	print( 09, 15, "Index Number        :", 2 )
	print( 10, 15, "Index Name          :", 2 )
	print( 11, 15, "Index Expression    :", 2 )
	print( 12, 15, "Record #            :", 2 )
	print( 13, 15, "Total Records       :", 2 )
	print( 14, 15, "Percentage complete :", 2 )
	print( 16, 15, "0%---------------------50%--------------------100%", 2 )

	start_time = time()

	select VICTIM

	*--- Prevent CTRL/ALT/DEL while indexing
	if !Gsys_test
		key(.f.)			&&		Funcky  turns keyboard off
	endif

	index on &mindexkey + fuel_gauge() to ( mindexname )

	if !Gsys_test
		key(.t.)			&&		Funcky enable CTL-ALT-DEL
	endif
	***
   ***   Close the index file and then remove
   ***   fuel_gauge() for the key expression by
   ***   doing a low level FWRITE()
	***

   close index
   write_header( mindexname )

	error_msg(" The time taken to re-index " + mfilename +" = " +;
          elaptime( start_time, time() ), .f. )

	scroll( 6, 10, 19, 70, 0 )

	use  && to close victim
return


procedure INDEXGRP

	*---Index a group of files

	parameter mpathname

	if pcount() = 0
		mpathname =""
	endif

	oldcolr = setcolor()

	*---Open Index Control file
   select 0
	if net_use( mpathname + 'INDEXES', .f., 5, 'INDEXES')
		ntx1 = mpathname + 'INDEXGRP.NTX'
		if .not. file( ntx1 )
			dispmsg(23,'Rebuilding index file ' + ntx1,.F.,1,.t.)
			index on upper(GROUP)+upper(DBFNAME) to ( ntx1 )
		endif
		set index to &ntx1
	else
		error_msg( 'Cant open Index control File' )
		retu
	endif

	*---Which group?

	mgroup = upper( POPGROUP())

	dbseek( upper( mgroup) )

	do while upper( GROUP) = mgroup .and. .not. eof()
		mdbfname   = upper( DBFNAME)
		mfilename  = trim( FILENAME)
		openedok = .f.
		do OPENFILE with mpathname, mdbfname, mfilename, openedok
		if openedok
			select INDEXES
			do while upper( DBFNAME) = mdbfname .and. .not. eof()
				mntx_order = NTX_ORDER
				mindexname = INDEXNAME
				mindexkey  = INDEXKEY
				do PAKINDEX with mpathname, mindexkey, mindexname, mfilename
				select INDEXES
				skip
			enddo
		else
			error_msg( 'Could not open ' + mpathname + mdbfname )
			select INDEXES
			do while upper( DBFNAME) = mdbfname .and. .not. eof()
				select INDEXES
				skip
			enddo
		endif
	enddo

	do CLR_MESS
   close databases

return


function POPGROUP

	*---returns a group name

	mgroup = "XXXX"

	grp_cnt = 1
	lastgrp = GROUP
	do while .not. eof()
		skip
		if GROUP <> lastgrp
			if .not. eof()
				grp_cnt = grp_cnt + 1
				lastgrp = GROUP
			endif
		endif
	enddo

*	testmsg( str( grp_cnt,5) + ' groups')

	private grp[grp_cnt], was_like

	go top
	grp_cnt = 1
	lastgrp = GROUP
	do while .not. eof()
		skip
		if GROUP <> lastgrp
			grp[ grp_cnt] = lastgrp
*			testmsg( 'Group ' + str( grp_cnt, 3) + ' is ' + grp[ grp_cnt ] )
			if .not. eof()
				grp_cnt = grp_cnt + 1
				lastgrp = GROUP
			endif
		endif
	enddo

	*---save screen
	save screen to was_like
	set color to &c_help
	*---Box
	boxhead  = '         Select Group         '
	boxdepth = iif( grp_cnt > 10, 10, grp_cnt )
	boxmarg  =  3
	r1       =  6
	c1       = 40

	boxwidth = len(boxhead) + (2*boxmarg) + 1
	@ r1,c1 clear to r1+boxdepth+3,c1+boxwidth
	@ r1,c1 to r1+boxdepth+3,c1+boxwidth double
	@ r1+1,c1+1+boxmarg say boxhead
	@ r1+2,c1 say 'Ç' + replicate('Ä',boxwidth-1) + '¶'
	toprow   = r1+3
	leftcol  = c1 + 1 + boxmarg
	bottrow  = toprow + boxdepth
	rightcol = leftcol + len(boxhead) - 1
*	testmsg( 'Paras are ' + str(toprow,3) + str(leftcol,3) + str(bottrow,3) + str(rightcol,3) )

	choice = achoice( toprow, leftcol, bottrow, rightcol,grp)

	if choice > 0 .and. choice <= grp_cnt
		mgroup  = trim( grp[ choice ])
	endif

	*---restore screen
	restore screen from was_like

return( mgroup)


procedure INDEXIND

	*---Index an individual file
	parameter mpathname

	if pcount() = 0
		mpathname =""
	endif

	oldcolr = setcolor()

	*---Open Index Control file
   select 0
	if net_use( mpathname + 'INDEXES', .f., 5, 'INDEXES')
		ntx1 = mpathname + 'nfl.NTX'
		if .not. file( ntx1 )
			dispmsg(23,'Rebuilding index file ' + ntx1,.F.,1,.t.)
			index on upper( DBFNAME) to ( ntx1 )
		endif
		set index to ( ntx1 )
	else
		error_msg( 'Cant open Index control File' )
		retu
	endif

	*---Which DBF file?

	mdbfname = upper( POPFILE() )

	dispmsg( 23, "Seeking " + mdbfname, .f., -1, .f. )
	select INDEXES
	INDEXES->( dbseek( mdbfname ) )
	//seek mdbfname

	do while upper( DBFNAME) = mdbfname .and. .not. eof()
		mfilename  = trim( FILENAME)
		openedok = .f.
		do OPENFILE with mpathname, mdbfname, mfilename, openedok
		if openedok
			select INDEXES
			do while upper( DBFNAME) = mdbfname .and. .not. eof()
				mntx_order = NTX_ORDER
				mindexname = INDEXNAME
				mindexkey  = INDEXKEY
				do PAKINDEX with mpathname, mindexkey, mindexname, mfilename
				select INDEXES
				skip
			enddo
		else
			error_msg( 'Could not open ' + mpathname + mdbfname )
			select INDEXES
			do while upper( DBFNAME) = mdbfname .and. .not. eof()
				select INDEXES
				skip
			enddo
		endif
	enddo

	do CLR_MESS
   close databases

return


function POPFILE

	*---returns a DBF name
	private was_like

	mdbfname = "XXXX"

	dbf_cnt = 1
	lastdbf = upper( DBFNAME)
	do while .not. eof()
		skip
		if upper( DBFNAME) <> lastdbf
			if .not. eof()
				dbf_cnt = dbf_cnt + 1
				lastdbf = DBFNAME
			endif
		endif
	enddo

	testmsg( str( dbf_cnt,5) + ' dbfnames')

	private dbf[dbf_cnt]
	private filenm[dbf_cnt]

	go top
	dbf_cnt = 1
	lastdbf = upper( DBFNAME)
	lastfn  = FILENAME
	do while .not. eof()
		skip
		if upper( DBFNAME) <> lastdbf
			dbf[ dbf_cnt]    = lastdbf
			filenm[ dbf_cnt] = lastfn
			testmsg( 'DBF ' + str( dbf_cnt, 3) + ' is ' + dbf[ dbf_cnt ] )
			if .not. eof()
				lastdbf = upper( DBFNAME)
				lastfn  = FILENAME
				dbf_cnt = dbf_cnt + 1
			endif
		endif
	enddo

	*---save screen
	save screen to was_like
	oldcolr = setcolor()
	set color to &c_help
	*---Box
	boxhead  = '         Select File          '
	boxdepth = iif( dbf_cnt > 10, 10, dbf_cnt )
	boxmarg  =  3
	r1       =  6
	c1       = 40

	boxwidth = len(boxhead) + (2*boxmarg) + 1
	@ r1,c1 clear to r1+boxdepth+3,c1+boxwidth
	@ r1,c1 to r1+boxdepth+3,c1+boxwidth double
	@ r1+1,c1+1+boxmarg say boxhead
	@ r1+2,c1 say 'Ç' + replicate('Ä',boxwidth-1) + '¶'
	toprow   = r1+3
	leftcol  = c1 + 1 + boxmarg
	bottrow  = toprow + boxdepth - 1   &&    SC 8-jun-89
	rightcol = leftcol + len(boxhead) - 1
	testmsg( 'Paras are ' + str(toprow,3) + str(leftcol,3) + str(bottrow,3) + str(rightcol,3) )

	choice = achoice( toprow, leftcol, bottrow, rightcol, filenm )

	if choice > 0 .and. choice <= dbf_cnt
		mdbfname  = trim( dbf[ choice ])
	endif

	testmsg( 'DBF to reindex is '+ mdbfname )
	*---restore screen
	restore screen from was_like

return( mdbfname)


procedure OPENFILE

	parameters mpathname, mdbfname, mfilename, openedok

	private fname

	fname = mpathname + mdbfname
*	testmsg( 'Looking for ' + fname )
	openedok = .f.

	dispmsg( 23, "Packing " + mfilename, .f., -1, .f. )
	if pack_dbf( mpathname, mdbfname )
	    select VICTIM
		use
		select 0
		if net_use( fname, .t., 5, 'VICTIM' )
			dispmsg( 23, "Re-indexing " + mfilename, .f., -1, .f. )
			openedok = .t.
		endif
	endif

return


function PACK_DBF

	parameters mpathname, mdbfname

	private save_sel, ok, savedir, memoexists, copyspace

	save_sel = select()
	ok = .f.
	savedir = curdir()

	chdir( mpathname )
	*---Check for a memo file?
	if file( mdbfname + '.dbt' )
		memoexists = .t.
	else
		memoexists = .f.
	endif

	*---Check to ensure there is enough disk space
	copyspace = filesize( mdbfname + '.dbf' )
	testmsg( 'File size on ' + mdbfname + '.dbf is ' + str( copyspace ) )
	if memoexists
		copyspace = copyspace + filesize( mdbfname + '.dbf' )
		testmsg( 'Including ' + mdbfname + '.dbt is ' + str( copyspace ) )
	endif

	testmsg( 'Diskspace left ' + str( diskspace() ) )

	if diskspace() > copyspace

		if frename( mdbfname+".dbf", "TEMPPACK.dbf" )
			testmsg( 'DBF renamed TEMPPACK.DBF' )
			if memoexists
				frename( mdbfname + ".dbt", "temppack.dbt" )
				testmsg( 'DBT renamed TEMPPACK.DBT' )
			endif

			select 0
			use TEMPPACK
			copy to ( mpathname + mdbfname )
			use

			ferase( "temppack.dbf" )
			if memoexists
				ferase( "temppack.dbt" )
			endif
			ok = .t.
		else
			error_msg( 'Copy on pack failed' )
		endif

	else
		error_msg( "There is not enough diskspace available" )
	endif			  	 

	chdir( savedir )
	select( save_sel )

return( ok )


function FUEL_GAUGE

	private percentage, ret_val

	if eof()
		*--- Will be EOF() when INDEX ON is first started

		num_recs = 0
		scroll(  8, 37, 14, 69, standard(15) )
		print(   8, 38, mfilename  )
		print(   9, 38, mntx_order )
		print(  10, 38, mindexname )
		print(  11, 38, substr( mindexkey, 1, 30 ) )
		areplicate( 17, 15, chr( 178 ), 50, 3 )

	endif

	if ! eof()

		num_recs   = num_recs + 1
		percentage = ( num_recs/reccount() ) * 100

*		if int( percentage / gfactor ) == ( percentage / gfactor )
			*---Multiples of 10 %

			print( 12, 38, ltrim( transform( num_recs,   "999,999,999" ) ), 15 )
			print( 13, 38, ltrim( transform( reccount(), "999,999,999" ) ), 15 )
			print( 14, 43, ltrim( transform( percentage,      "999.99" ) ), 15 )

			*---Show how far we have got
			areplicate( 17, 15, chr(219), int( percentage/2 ), 110 )

* 			do pause
* 		endif

	endif

	*---Return an innocuous value to the key
	do case
		case type( "&mindexkey" ) = 'N'
			ret_val = 0
		case type( "&mindexkey" ) = 'C'
			ret_val = ""
		case type( "&mindexkey" ) = 'D'
			ret_val = 0
		otherwise
			ret_val = ""
	endcase

return( ret_val )


function WRITE_HEADER

	***
	***   This fn strips out fuel_gauge() from the index expression
	***

	parameter file_name

	private file_handle, bytes_to_write, start_byte, max_length, read_write
	private seek_begin


	start_byte  = 22										 &&		Starting position of expression
	max_length  = 250										 &&		Max length of an index expression
	read_write  = 2										 &&		File opening mode
	seek_begin  = 0										 &&		Beginning of file
	file_name   = file_name+".NTX"


	*--- Open for reading and writing
	file_handle = FOPEN( file_name, read_write)

	*--- Always check the low level opening
	if ferror() <> 0
		*--- return a null string if an error
		retu("")
	endif

	*---Position to the end of the key expression
	fseek( file_handle, start_byte + len( mindexkey ), seek_begin )

	bytes_to_write = max_length - len( mindexkey )

	*---Now write the dummy info
	fwrite( file_handle, replicate( chr(0), bytes_to_write ), bytes_to_write )

	*--- Always clean up
	fclose(file_handle)

return("")



*EOF:  INDEXGEN.PRG
