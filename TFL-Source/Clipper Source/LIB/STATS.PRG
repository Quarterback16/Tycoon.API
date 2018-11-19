*#  STATSON will display a screen full of Statistical information.
*
*#



procedure STATSON

	parameters mpathname

	private dbf_cnt, lastdbf, old_color, openedok, ntx1, pre_open, file_cnt
	private post_open, exclusiv

	clear_scn()		 
	head_line( "STATISTICS" )
	@ 01,00 to 01, 79

	standard(2)		&&		Green on Black						 
	
	pre_open = val( open_cnt() )
	if opendbf( '*', mpathname, .f. )
		post_open = val( open_cnt() )
		print( 02, 01, "Directory :" )
		print( 02, 13, mpathname, 15 )
		file_cnt = post_open - pre_open
		print( 03, 01, "Files     :" )
		print( 03, 12, str( file_cnt, 3 ), 15 )
	else
		retu
	endif

	private ntx_cnt, i

	@ 23,00
	print( 22, 00, "Memory      :" )
	print( 22, 14, str( memory(0) ) + "K", 15 )
	print( 23, 00, "System type :" )
	print( 23, 14, sysid( systype() ), 15 )
	print( 23, 60, "Disk space  :" )
	print( 23, 74, str( ( diskspace()/1024 ),5,0) + "K", 15 )

	select 0
	if net_use( mpathname + 'INDEXES', .f., 5, 'INDEXCTL')
		ntx1 =  mpathname + 'INDEXIND.NTX'
		if .not. file( ntx1 )
			dispmsg(23,'Rebuilding index file ' + ntx1,.F.,1,.t.)
			index on upper(DBFNAME)+NTX_ORDER to ( ntx1 )
		endif
	else
		error_msg( 'Cant open Index control File' )
		retu
	endif
	set index to ( mpathname + "indexind" )
	ntx_cnt = reccount()
	*---Count DBFs
	dbf_cnt = 0
	lastdbf = "SPQR"
	go top
	do while .not. eof()
		mdbfname = DBFNAME
		if mdbfname <> lastdbf
			lastdbf = mdbfname
			dbf_cnt = dbf_cnt + 1
		endif
		skip
	enddo
	print( 04, 01, "DBFs      :" )
	print( 04, 12, str( dbf_cnt, 3 ), 15 )
	print( 05, 01, "NTXs      :" )
	print( 05, 12, str( ntx_cnt, 3 ), 15 )

	private filestat_[ dbf_cnt ]
	private dbfname_[ dbf_cnt ]
	afill( filestat_, "" )
	afill( dbfname_, "" )

	*---load it
	dbf_cnt = 0
	select INDEXCTL
	go top
	do while .not. eof()
		*---Open DBF
		select INDEXCTL
		mdbfname  = upper( DBFNAME )
		mfilename = FILENAME
		malias    = ALIASNAME
		if empty( malias )
			malias = mdbfname
		endif

		dbf_cnt             = dbf_cnt + 1
		dbfname_[ dbf_cnt ] = mpathname + trim( mdbfname ) + ".DBF"

		openedok = .f.
		exclusiv = .f.
		do OPENIT with mpathname, mdbfname, mfilename, malias, openedok
		if openedok
			*---Add it to the array
			filestat_[ dbf_cnt ] = mfilename + str( reccount(), 6 )
		else
			error_msg( 'Could not open ' + mfilename )
			filestat_[ dbf_cnt ] = mfilename + '??????'
		endif
		*---Get next DBF
		select INDEXCTL
		do while .not. eof() .and. DBFNAME = mdbfname
			skip
		enddo
   enddo			 

	close databases

	for i = 1 to dbf_cnt
		filestat_[ i ] = filestat_[ i ] + space(3) + ;
		                 dtoc( f_lupdate( dbfname_[ i ] ) )

		filestat_[ i ] = filestat_[ i ] + space(3) + ;
									  str( filesize( dbfname_[ i ] ) )
   next

	old_color = setcolor()
	setcolor( C_help )
	popbox( 7, 0, 20, 79 )
	header = " FILENAME" + space(20) + "RECORDS" + space( 4 ) + "UPDATED" + ;
				space(8) + "SIZE"  + space( 12 )
	print( 8, 1, header, 31 )
	lineinbox( 9 )

	#IFDEF NETLIB_AVAILABLE
	n_settime( 300, chr(27), 1, "showtime" )
	#ENDIF

	achoice( 10, 1, 19, 78, filestat_ )

	#IFDEF NETLIB_AVAILABLE
	n_settime( 0 )
	#ENDIF

   setcolor( old_color )

RETURN 


function F_LUPDATE

	parameters _filespec

	private fname_[1], fsize_[1], fdate_[1]

	adir( _filespec, fname_, fsize_, fdate_ )

return( fdate_[1] )


function FILESIZE

	parameters _filespec

	private fname_[1], fsize_[1], fdate_[1]

	adir( _filespec, fname_, fsize_, fdate_ )

return( fsize_[1] )

#IFDEF NETLIB_AVAILABLE
procedure SHOWTIME

	parameters seconds

	mattr = n_saveattr()
	print( 02, 70, time() )
	n_restattr( mattr )

return 
#ENDIF


function SYSID

	parameters sysidbyte

	private ret_val

	do case
		case sysidbyte = 255
			ret_val = "IBM PC           "
		case sysidbyte = 254
			ret_val = "IBM PC XT        "
		case sysidbyte = 253
			ret_val = "IBM PCjr         "
		case sysidbyte = 252
			ret_val = "IBM PC AT        "
		case sysidbyte = 251
			ret_val = "IBM PC XT        "
		case sysidbyte = 250
			ret_val = "IBM PS/2 model 30"
		case sysidbyte = 249
			ret_val = "IBM PC convrtable"
		case sysidbyte = 248
			ret_val = "IBM PS/2 model 80"
		otherwise
			ret_val = "Clone ? " + str( sysidbyte, 3 ) + space(6)
	endcase

	ret_val = ret_val + "  CPU= " + str( cputype(), 5 )

return( ret_val )




*EOF: STATS.PRG

