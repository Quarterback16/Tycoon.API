*****************************************************************************
*  Program Name.....: TFL_LIB.PRE 
*  Programmer.......: Steve Colonna 
*  System...........:
*  Module...........:
*  Date.............: 14/4/1990 at 16:07
*  Copyright........: (c) 1990 by Knoware 
*  Called From......:
*  Purpose..........:
*
*			To store Global functions.
*
*............................................................................
*  Revision.........: 1.0 Last Revised: 15/4/1990 at 11:30
*  Description......: Initial Creation.
*............................................................................
***************************** ALL RIGHTS RESERVED ***************************

#INCLUDE "TFL.H"

field SYSNAME, SYSOWNER, DRIVE, NFL_PATH, TY_PATH, GETLISTKEY
field SEASON, LEAGUE, WEEK


function GenMsg

	GenMsgClr()

	setcolor( c_msg )
   atcentre( 20, "Generating... Press ESC to abort" )

return(.t.)

function GenMsgClr

   setcolor( c_blank )
   @ 20,0

return(.t.)

*#   KEYEDTABLE( var,
*                table,
*                keyval, 	  holds the key of the group to browse on
*                keyexp,	  generates the group key of the current record
*                code,
*                desc, 
*                codetitle,
*                desctitle,
*                show_desc,
*                poptable ) expL - popup the table on an incorrect var
*
*	  See also poptable
*    Eg. 
*		ok = keyedtable( @munit_div, 'ORGUNT', 'D', 'UNIT_TYPE', 'UNIT_ID',;
*                      'UNIT_NAME', 'Code', 'Division', 0, !valonly )
*
* ASHLEY
* 23-10-89
*#



*******************
FUNCTION KEYEDTABLE

parameters var, table, keyval, keyexp, code, desc, codetitle, desctitle, show_desc, poptable


* popup a table browse for a table grouped by a key

* keyval holds the key of the group to browse on
* keyexp generates the group key of the current record
* expects the correct index selected
* index key of the from, group [+ identifier within group]
* only works with character keys
* poptable - expL - popup the table on an incorrect var
* extention of poptable() in lu_tabl.prg

priv ok, curals, len, drow, dcol, clr

poptable  = def_parm( "poptable",  "L", .t. )
show_desc = def_parm( "show_desc", "N",  0  )

drow = row()
dcol = col()
clr = setcolor()
curals = alias( 0 )
select &table
ok = !empty( var )
if ok
   dbseek( keyval + var )
	ok = found()
endif

if !ok .and. poptable
	dbseek( keyval )
*	if empty( var ) .or. !found()
      len = len( &code ) + 3 + len( &desc ) + 2
      setcolor( c_help )
      blowup( 10,3, 20,3+len+1 )
      lineinbox( 12 )
      @11,5 say desctitle
      @11,3+len-len( codetitle )-1 say codetitle
      @20,6 say "Enter select"
      @20,20 say "Esc exit"
      keybrowse( 13,4, 19, 3+len, 'brwkeytabl', keyval )
      shrink()          
      setcolor( clr )
      ok = !abort()   
      if ok 
         var = &code
      endif
*	endif
endif
setcolor( clr )
if ok
   if show_desc = 1
      @drow, dcol+len( &code )+3 say trim( &desc )
   elseif show_desc = 2
		@drow, dcol say trim( &desc )
	elseif show_desc = 3
      setcolor( getcolor( 5 ) )
		@drow, dcol say &code
   endif
endif
setcolor( clr )
return( ok )


*******************
FUNCTION BRWKEYTABL

parameter mode

* keybrowse udf for keyedtable()

if mode = 5
   return( &keyexp )
elseif mode = 6
   if !( keyval == &keyexp )
      return( "" )
   endif
   return( " "+ &desc +space( 3 )+ &code +" " )
elseif mode = 0
   if lastkey() = 13
      return( 0 )
   endif
elseif mode = 3
   return( 0 )
endif

return( 1 )



function STD_RPT_OPT

   parameters mopt, mmess, _report

   mopt[1]  = 'X-exit'
   mopt[2]  = "Generate"
   mopt[3]  = "View"
   mopt[4]  = "Print"

   mmess[1] = 'Return to Main Menu'
   mmess[2] = "Generate the " + _report
   mmess[3] = "View the generated report on screen"
   mmess[4] = "Print the generated report on the printer"

return( "" )




function ScreenLine

	parameters nLine

	@ nline, 0 to nline, 79

return( .t. )


****************
FUNCTION FBROWSE

   parameters t, l, b, r, file, rightmarg

   if pcount() < 6
      rightmarg = 256
   endif

   priv handle, linecnt, winlen, winwid, horiz
   priv i, j, line, filepos, savepos

   if !file( file )
      return( .f. )
   endif

   handle = fopen( file, 2 )                        && open file in read/write mode
   if ferror() != 0
      return( .f. )
   endif

   fseek( handle, 0, 0 )

   winlen = b - t + 1
   winwid = r - l + 1
   horiz = 0
   filepos = 1       && -1 top 1 bottom

   rightmarg = rightmarg - winwid

   fbrw_page( 1 )

   do while .t.

      ik = inkey( 0 )

      if ik = 3
         fbrw_page( 1 )

      elseif ik = 18
         fbrw_page( -1 )

      elseif ik = 24
         fbrw_line( 1 )

      elseif ik = 5
         fbrw_line( -1 )

      elseif ik = 19
         horiz = horiz - int( winwid / 3 )
         if horiz < 1
            horiz = 1
         endif
         fbrw_page( 0 )

      elseif ik = 4
         if horiz < rightmarg
            horiz = horiz + int( winwid / 3 )
         endif
         fbrw_page( 0 )

      elseif ik = 1
         horiz = 1
         fseek( handle, 0, 0 )
         filepos = -1
         fbrw_page( 0 )

      elseif ik = 6
         fseek( handle, 0, 2 )   && seek end of file
         filepos = 1
         fbrw_page( 0 )

      elseif ik = 27
         exit

      endif

   enddo

   fclose( handle )

return( .t. )


function fbrw_page

   parameters movedirec

   * move whole page
   * position the file at the start position for drawing a page

   if movedirec = 1                         && move foward
      if filepos = -1
         fbrw_advance()
      endif

   else
      *---move file to top of page
      if filepos = 1
         fbrw_rewind()
      endif
      if movedirec = -1
         fbrw_rewind()
      endif

   endif

   @t,l clear to b,r
   filepos = 1
   savepos = fseek( handle,0, 1 ) && ftell
   for i = t to b
      print( i, l, substr( freadline( handle ), horiz, winwid ), -1, winwid )
   next
   if feof( handle )
      fseek( handle, savepos, 0 )
      filepos = -1
   endif


return( "" )


function fbrw_line

   parameters movedirec

   * scroll one line

   if movedirec = 1
      if !feof( handle )                       && move foward
         if filepos = -1
            fbrw_advance()
         endif
         if filepos = 1
            scroll( t,l, b,r, 1 )
            print( b, l, substr( freadline( handle ), horiz, winwid ), -1, winwid )
         endif
      endif
   else
      *---move file to top of page
      if filepos = 1
         fbrw_rewind()
      endif
      filepos = -1
      if !fbof( handle )
         frewind( handle )
         scroll( t,l, b,r, -1 )
         print( t, l, substr( freadline( handle ), horiz, winwid ), -1, winwid )
         frewind( handle )
      endif

   endif

return( "" )


function fbrw_advance

   filepos = 1
   savepos = fseek( handle,0, 1 ) && ftell
   for i = 1 to winlen
      fadvance( handle )
   next
   if feof( handle )
      fseek( handle, savepos, 0 )
      filepos = -1
   endif

return( "" )


function fbrw_rewind

   if !feof( handle )
      frewind( handle )
   endif
   for i = 2 to winlen
      frewind( handle )
   next
   filepos = -1

return( "" )


function SoftSeek

	parameters cKey

	set softseek on
	dbseek( cKey )
	set softseek off

return( .t. )

function DayAbbr

	parameters nDay

	do case
		case nDay = 1
			cAbbr = 'Sun'
		case nDay = 2
			cAbbr = 'Mon'
		case nDay = 3
			cAbbr = 'Tue'
		case nDay = 4
			cAbbr = 'Wed'
		case nDay = 5
			cAbbr = 'Thu'
		case nDay = 6
			cAbbr = 'Fri'
		case nDay = 7
			cAbbr = 'Sat'
	endcase

return( cAbbr )

function inCentre

	parameters nLeft, nRight, cMessage, nRow

	LOCAL nRoom, nStartCol

	cMessage = trim( cMessage )

	nRoom = nRight - nLeft + 1
	if nRoom < len( cMessage )
		error_msg( "Cant fit" )
	else
		nStartCol = ( nRight - nLeft ) - int( len( cMessage ) / 2 )
		@ nRow, nStartCol say cMessage
	endif

return( NIL )


*#   UNAVAIL()  displays a message that the option is currently unavailable.
*    To be used as a stub in main menu programs.
*
*#

function UNAVAIL

   private message        && Message to display
   private old_color

   old_color = setcolor()
   message = "This option is currently unavailable"
   @ 24,00
   error_msg("  "+message+"  ",.f.,3,.t.)
   setcolor(old_color)

return(.t.)



function CLEAR_SCN

* 	if gsys_test
* 		vblindshut()
* 	else
		clear
*	endif

return(.t.)


*#   HEAD_LINE( ttext, progname ) displays on line 0 a standard heading in
*    cyan on black.										
*
*#

function HEAD_LINE

   ***
   ***   Displays prog name, todays date and ttext on top of screen
   ***

   parameters ttext, progname
	*---ttext is function name heading to be displayed
	if pcount() = 1
		progname = G_shrtnme+" ("+G_version+")"
	endif

	private printat, old_color

   old_color = setcolor()
   if type('c_flat') = 'U'        && undefined type, doesnt exist
      set color to
   else
      setcolor( c_flat )
	endif
	@ 0, 0
   @ 0, 0 say progname

   printat = (80 - len( alltrim( ttext)))/2
   if printat < len(progname) + 2
      @ 0,len( progname) + 2 say upper( alltrim(ttext))
   else
      @ 0,printat say upper(alltrim (ttext))
   endif
   @ 0,70 say dtoc( date() )
   setcolor( old_color )

return(.t.)   &&   from head_line


function SHUTDOWN

	parameters _logit

	_logit = def_parm( '_logit', 'L', .t. )

* 	if gsys_test
* 		*---Memory left
* 		@ 00,00 say 'Memory left : ' + ltrim(str(memory(0))) + 'K'
* 	endif        

*    if type( "g_psv_srv" ) <> "U" .and. g_pquemode = 3 && restore the spool settings 
*       set print to ( "LPT"+str( g_spllpt, 1 ) )
*       n_spllpt( g_spllpt )
*       n_splsrv( trim( g_psv_srv ) )
*       n_splque( trim( g_psv_que ) )
*       n_spool( 1 )
*    endif
* 
* 	#IFDEF USE_CFTS_INDEXES
* 	cfts_clsall()
* 	#ENDIF

	close data
	if _logit
		do LOG_USE with "OUT", G_username, G_shrtnme, G_version
	endif
	*blow()
	set cursor on
	quit

*#   LOG_USE is used to record the entry an exit of modules by users.
* 
*    Eg.   *---Log Module Useage
*			  do LOGUSE with "IN ", G_username, procname(), G_version
*
*#


procedure LOG_USE

	parameters _logtype, _user, _module, _ver

	private log_msg, m_hndl, spacing

	spacing = space(3)

	set century off

	log_msg = spacing + dtoc(date()) + spacing + time() + spacing + _logtype ;
				 + spacing + left( _user+space(8), 8 )	+ spacing + ;
				 left( _module+space(10), 10 ) + spacing + _ver + chr(13) + chr(10)

*	testmsg( log_msg )

	m_hndl = fopen( "USAGE.LOG", 2)

	if m_hndl = -1
		*---File not found?
		m_hndl = fcreate( "USAGE.LOG" )
	else
*		Disp_msg( "USAGE.LOG found"  )
	endif

	if m_hndl <> -1
		fseek(  m_hndl, 0, 2)
		fwrite( m_hndl, log_msg)
		fclose( m_hndl)
	endif

return


procedure LOG

	parameters _msg

	private log_msg, m_hndl, spacing

	spacing = space(3)

	set century off

	log_msg = spacing + dtoc(date()) + spacing + time() + spacing + ;
				 _msg + chr(13) + chr(10)

*	testmsg( log_msg )

	m_hndl = fopen( "USAGE.LOG", 2)

	if m_hndl = -1
		*---File not found?
		m_hndl = fcreate( "USAGE.LOG" )
	else
*		Disp_msg( "USAGE.LOG found"  )
	endif

	if m_hndl <> -1
		fseek(  m_hndl, 0, 2)
		fwrite( m_hndl, log_msg)
		fclose( m_hndl)
	endif

return


function SET_ENV

	set bell       off
	set century    on
	set confirm    off
	set console    off
	set cursor     off
	set date       british
	set deleted    on
	set delimiters off
	set device     to screen
	set escape     on
	set exact      off
	set exclusive  off
	set fixed      off
	set function 5 to chr(3)
	set function 6 to chr(25)
	set intensity  on
	set margin     to
	set print      off
	set procedure  to
	set scoreboard off
	set softseek   off
	set talk       off
	set typeahead  to 32
	set unique     off
	set wrap       off

return(.t.)


*******************
FUNCTION PRINT_CONT

   * continue or abort printing

   parameter _reason

   priv _saveclr, _ok
   _saveclr = setcolor()
   set device to screen

   error_msg( _reason )
   _ok = dialog( 21,28, "Continue Printing ?", "Yes", "No", .t. )

   set device to print
   setcolor( _saveclr )         

   return( _ok )


*#   ADDRECIT loops until it adds and locks a record.
*      Parameter : filename C  used in error message
*
*#

procedure ADDRECIT

	parameter filename   &&  string

	private locked

	locked = .f.
	do while .not. locked
		if add_rec(5)
			locked = .t.
*			testmsg( filename + ' record ' + ltrim(str(recno())) + ' locked' )
		else
			error_msg( 'Having trouble adding ' + filename + ' record' )
		endif
	enddo

return


*
*   POPUP BOXES SYSTEM
*
* box coordinates are only passed to the box drawing functions
* these coordinates are then stored and used by the box remove functions

* blowup_init() - must be run before using any of the functions below
* blowup( top, left, bottom, right ) - draw a box,( expanding )
* lineinbox( row )  - draw a line across the box 
* linedownbox( col ) - draw a line down the box
* popbox( top, left, bottom, right ) - draw a box
* setboxes( frametype, shadow_on_off ) - frames: "S" for singles, "D" for doubles
*                                      - use of shadows switch
* shrink() - remove the last box, ( shrinking )
* unpop() - romove the last box


*#   CLRMENU( func)  is the general function used with UI generated
*    basic file update screens to clear the menu area of the sceen.
*    The menu area of the sceen is defined as lines 23 and 24.
*
*#


function CLRMENU

   ***
   ***   Clears the Menu from the Bottom of the Screen
	***

   setcolor( C_BLANK )
	@ 24,00

return(.t.)


* Function .......... : tbl_srch
* Author ............ : PC
* System ............ : NCC
* Date .............. : 26 - May - 1989
* Purpose ........... : To put a list of table contents on the screen

*#   TBL_SRCH( mfields, mcols [,mline,lsearch] ) where mfields is an array of 
*    field names and mcols is an array of column headers, uses dbedit to pop 
*    up a window of records that can be used to reposition the record pointer.
*	  Most common use is with the "search" option on a UI generated file
*    update screen.
*    Optional parameter mline specifies which line to begin the box at.
*    Optional parameter lsearch specifies whether or not to include first
*    letter search on the file.  Defaults to .T. if not passed.
*
*    Optional parameters filtfield and filtval are used to simulate the
*    effects of a filter on dbedit.
*
*#

function TBL_SRCH

   parameters mfields, mcols, mline, lsearch, _mess

	mline   = def_parm( "mline",   "N", 9 )
	lsearch = def_parm( "lsearch", "L", .t. )
	_mess   = def_parm( "_mess",   "L", .t. )

*	testmsg( 'lsearch is ' + iif( lsearch, 'ON', 'OFF' ) )

   private old_color, old_screen, old_rec

   old_color  = setcolor(c_help)
   old_screen = savescreen(mline,01,20,78)
	old_rec    = recno()
	chosen     = .t.

*	go top

* Enter to Select        to Move Up         to Move Down        ESC to Exit

	@ mline,01,20,78 box 'ÉÍ»º¼ÍÈº '
	if _mess
		@ 20,03 say " Enter to Select "
	endif
   @ 20,24 say " "+chr(24)+" to Move Up "
   @ 20,44 say " "+chr(25)+" to Move Down "
	@ 20,64 say " ESC to Exit " 
	if empty_dbf()
		@ mline+4, 05 say 'File is empty!'
		chosen = .f.
		inkey(0)
	else
		if lsearch
			dbedit(mline+1,02,19,77,mfields,'srch_func','@X',mcols,"Ä"," ")
		else
			dbedit(mline+1,02,19,77,mfields,'nosrch_func','@X',mcols,"Ä"," ")
		endif
	endif
   setcolor(old_color)
   restscreen(mline,01,20,78,old_screen)

return( chosen)


function SRCH_FUNC

   parameters mode, fld_ptr

   private ret_mode

   do case        
      case mode < 3
         ret_mode = 1            && continue
      case mode = 3
         ret_mode = 0            && exit
      case lastkey() = 27
         go old_rec
         ret_mode = 0
      case lastkey() = 13
			ret_mode = 0	  
		case lastkey() = 287  &&  Alt-S
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
* 		case lastkey() = 286   &&  Alt-A
* 			testmsg( 'Alt A pressed' )
* 			if type( 'doall') = 'L'
* 				doall = .t.
* 			endif
* 			ret_mode = 0	  
		case lastkey() > 64 .and. lastkey() < 123
			*---Alpha key
			char = upper( chr( lastkey() ) )
*			testmsg( 'Char = ' + char )
			set softseek on
			dbseek( char )
			set softseek off
			ret_mode = 2            && continue and redraw
		case lastkey() > 47 .and. lastkey() < 58
			*---Numeric key
			char = upper( chr( lastkey() ) )
*			testmsg( 'Char = ' + char )
			set softseek on
			dbseek( char )
			set softseek off
			ret_mode = 2            && continue and redraw
      otherwise
         ret_mode = 1            && continue
   endcase

return(ret_mode)


function NOSRCH_FUNC

   parameters mode, fld_ptr

   private ret_mode

	do case
		case mode = 0   &&   Idle
			ret_mode = 1

      case mode < 3
			ret_mode = 1            && continue

      case mode = 3
			ret_mode = 0            && exit

      case lastkey() = 27
         go old_rec
			ret_mode = 0
      case lastkey() = 13
			ret_mode = 0	  
      otherwise
         ret_mode = 1            && continue
   endcase

return(ret_mode)



* Procedure.......... : edit_memo
* Author ............ : PC
* System ............ : NCC
* Date .............. : 14 - Jun - 1989
* Update ............ : 14 - Jun - 1989
* Purpose ........... : To display/edit a memo field

*#   EDIT_MEMO displays and edits a memo field on the screen from line 8 to
*    line 20.  Parameters passed are the memo field name and the title to
*    put on the box around the memo field.  A third optional parameter to
*    allow/prevent the memo being edited - defaults to allow editing.
*#

PROCEDURE edit_memo

	parameters mfield, mheader, medit, top, left, bott, right, no_wipe

	medit   = def_parm( "medit",   "L", .T.)
	top     = def_parm( "top"  ,   "N", 10 )
	left    = def_parm( "left" ,   "N",  2 )
	bott    = def_parm( "bott" ,   "N", 18 )
	right   = def_parm( "right",   "N", 77 )
	no_wipe = def_parm( "no_wipe", "L", .f.)
* 	testmsg( 'Top = ' + str(top) )
* 	testmsg( 'left = ' + str(left) )
* 	testmsg( 'bott = ' + str(bott) )
* 	testmsg( 'right = ' + str(right) )
   set scoreboard off

   private oldcolor, winbuf
   private mstring

	if !no_wipe
		winbuf = savescreen( top-2, left-2, bott+2, right+2 )
	endif
   oldcolor = setcolor()

	*mstring = memotran(&mfield.," "," ")
	mstring = &mfield

   if rec_lock(5) 

      setcolor(c_help)
		@ top-2, left-2, bott+2, right+2 box 'ÉÍ»º¼ÍÈº '
		if !empty( mheader )
			@ top-2, left say " "+mheader+" "
		endif
		if !no_wipe
			@ bott+2, left+2   say " End - Exit "
			@ bott+2, right-17 say " ESC - Abandon "
		endif

		mstring = memoedit( mstring, top, left, bott, right, medit, 'MEMO_FUNC')

		if lastkey() <> 27
*        replace &mField with HARDCR( mstring )
         replace &mField with mstring
      endif
		unlock
		commit
   else
      warning('rec_lock')
   endif

	if !no_wipe
   	inkey(0)
		restscreen( top-2, left-2, bott+2, right+2, winbuf)
	endif
	set scoreboard on
   setcolor(oldcolor)

return


function MEMO_FUNC

	parameters mmode, mline, mcol

	private ret_mode

	ret_mode = 0

	if lastkey() = 27 .and. mmode = 2

		if dialog( 22, ctr( "Do you wish to undo these changes ?",80)-1,;
			"Do you wish to undo these changes ?","Yes","No",.f.)

			keyboard chr(27)
			ret_mode = 0

		else

			ret_mode = 32

		endif

	elseif lastkey() = 27  .and. mmode <> 3

		keyboard chr(27)

	elseif (lastkey() = 23 .or. lastkey() = 6) .and. mmode <> 3
		*---Ctl-End or End

		keyboard chr(23)

	endif

return(ret_mode)


* Program .....: DDhead
* Author ......: PC
* System ......: NCC
* Date  .......: 25 - May - 1989
* Update ......: 25 - May - 1989
* Purpose......: To produce the heading for the draw down menu 

function dd_head( cSysowner, cSysname, version )

	local old_color := setcolor(c_mainh)

	@ 03,ctr( cSysowner, 80 ) say cSysowner
	@ 06,ctr( cSysname + " " + version, 80 )  say cSysname + " " + version

	setcolor(old_color)

return(.T.)

*#   RLOCKIT loops until it locks the current record.
*      Parameter : filename C  used in error message
*
*#

procedure RLOCKIT

	parameter filename   &&  string

	private locked

	locked = .f.
	do while .not. locked
		if rec_lock(5)
			locked = .t.
*			testmsg( filename + ' record ' + ltrim(str(recno())) + ' locked' )
		else
			error_msg( 'Having trouble locking ' + filename + ' record' )
		endif
	enddo

return


* Function .......... : Error_msg
* Author ............ : PC
* System ............ : NCC
* Date .............. : 26 - May - 1989
* Update ............ : 26 - May - 1989

*#   ERROR_MSG( message ) displays an error message on line 23 for 5 secs.
*
*#


function WHOOPS

  parameter msg

  dispmsg(23,msg,.f.,3,.t.)

return(.t.)



***
***   GETAPLH
***
***   EG.   set cursor on
***         outfile = GETALPH('Enter Program Name',8) + '.PRG'
***

*#   GETALPH(prompt, memvarsize) prompts the user to enter a character
*    string of a certain length.
*
*#

function GETALPH

	parameters prompt, memvarsize, _t, def_val

	_t      = def_parm( "_t"      , "N", 17 )
	def_val = def_parm( "def_val" , "C", space( memvarsize) )

   private _l , _b, _r, memvar, old_colo, width

	prompt = alltrim( prompt )

	if len(prompt) > memvarsize
		width = len( prompt )
		_l = ctr( prompt , 80) - 2
	else
		width = len( def_val )
		_l = ctr( def_val, 80)	- 2
	endif
	_b = _t + 3
	_r = _l + width + 4
	memvar = def_val

	old_colo = setcolor()
   setcolor( c_dialog )

	popbox( _t, _l, _b, _r )

   @ _t+1, ctr( prompt, 80 ) say prompt

	@ _t+2, _l+2 get memvar picture '@K'
	set cursor on
   read
	set cursor off

   if lastkey() = 27
      memvar = space(memvarsize)
   endif

	unpop()

	setcolor( old_colo )

return(memvar)


******************

*#   DEF_PARM() handles setting of default parameters.
*
*     Syntax: Def_Parm(<ExpC1>,<ExpC2>,<Exp>) 
*
* Parameters: ExpC1 - Name of var to be checked
*
*             ExpC2 - 'Type' of var to be checked
*
*             Exp   - default value if typecheck fails
*
*     Return: The contents of the variable being checked if
*             it passes the type-check test, otherwise the
*             default value passed as the third parameter.
* 
*   Example:
*
* 	         medit   = def_parm( "medit",   "L", .T.)
* 	         top     = def_parm( "top"  ,   "N", 10 )
* 	         left    = def_parm( "left" ,   "N",  2 )
* 	         bott    = def_parm( "bott" ,   "N", 18 )
* 	         right   = def_parm( "right",   "N", 77 )
* 
*#

Function DEF_PARM

   parameters pname, ptype, pval

return(iif(type(pname)<>ptype,pval,&pname.))





*#   EMPTY_DBF() returns a logical true if the currently selected file
*    has no records.
*
*#

*#   STD_HEAD ouputs the standard System report heading given the report
*    title.
*
*#


procedure STD_HEAD

   parameters rtitle, pageno, pitch

	private width

	width = pitch * 7

	pageno = pageno + 1

* 	if .not. Glaser
* 		?
* 		?
* 	endif
   ? date_123( Date() ) + iif( pitch=12, space(18), space(11)) + tcent( G_sysowner,30)
   ?? iif( pitch=12, space(20), space(11)) + 'Page' + str( pageno,4)

   ? iif( pitch=12,space(27),space(20)) + tcent( G_sysname,30)

   ? '  ' + substr( time(),1,5) + iif( pitch=12, space(20), space(13)) + tcent( rtitle,30)
	? replicate( '-', width )
	?

return

*#   STD_FOOT ouputs the standard System report footer.
*
*#

procedure STD_FOOT

   parameters progname

   ?
   ?
	? iif( pitch=12, space(24), space(19)) + '*** End of Report ***' +;
	  iif( pitch=12, space(20), space(13)) + ;
     iif( gsys_test,'[' + trim(progname) + ']','')
   

return

function TCENT

   parameters text, maxsize

   p2 = trim(text)
   p1 = space((maxsize-len(p2))/2)
*   if gsys_test
*      p1 = replicate('X',(maxsize-len(p2))/2)
*   endif
   p3 = p1 + p2 + space( maxsize-len(p1)-len(p2) )
*   if gsys_test
*      p3 = p1 + p2 + replicate('X', maxsize-len(p1)-len(p2) )
*   endif


return(p3)


FUNCTION POPTABLE2

	* changes from poptable()
	* the row, col, params have gone and the writes occur at row(), col()
	* the show_desc param is a number, 0 - do not show desc
	*                                  1 - show desc 3 spaces after code ( as in poptable() )
	*                                  2 - show desc overwrites the code
	*
	*    Parameters:
	*       mval   -  a variable containing the value being looked up
	*                 (passed by reference as it may change)
	*       talias -  The alias name of the table (must be opened)
	*       code_fld - A string identifing the field name of the code field
	*       desc_fld - A string identifing the field name of the description
	*                  field
	*       code_headr - A string with the table heading for the code
	*       desc_headr - A string with the table heading for the description
	*       code_type  - "N" if the value is a number (otherwise its a string)
	*       show_desc -  0 - do not show description
	*							1 - show description 3 spaces after
	*							2 - overwrite the code
	*       poptable  - Whether or not to pop the table or just to validate mval.
	*                   Defaults to Yes.
	*		  show_len  - How much of the description to display
	*		  _filt     - Whether to filter out Cancelled records def. = .t.
	*
	*  Eg.
	* 		ok = poptable2( @mcntryad3, "CNTRY", "COUNTRY", "NAME","Country",;
	* 		                "   Name", "C", 0, .t., 30, .f. )
	*

	parameters mval, talias, code_fld, desc_fld, code_headr, desc_headr, ;
              code_type, show_desc, poptable, show_len, _filt

   priv sc_row, sc_col, sav_col, _show
   sc_row = row()
   sc_col = col()

   show_desc = def_parm( "show_desc", "N", 1 )
	poptable  = def_parm( "poptable",  "L", .t. )
   show_len  = def_parm( "show_len",  "N", 0 )
   _filt     = def_parm( "_filt",     "L", .t. )


	if gsys_test .and. !_filt
		* testmsg( 'No Canc flag filter' )
	endif

	private ok, old_alias, old_record

	old_alias =  str(select(),2)

	ok = .t.
   select &talias

   dbseek( trim( mval ) )
   if poptable
      old_record = recno()

		do LU_TABLE with;
		   mval, talias, code_fld, desc_fld, code_headr, desc_headr, _filt

      if lastkey() = 27
         go old_record
      endif   
   	dbseek( mval )
	endif

	if eof()
		private mvalstr
		do case
		case code_type = 'N'
			mvalstr = ltrim( str( mval ) )
		otherwise
			mvalstr = trim( mval )
		endcase
		error_msg( 'Invalid Table value ' + mvalstr + '!' )
      ok = .f.
   else
      ok = .t.
	endif

	*---Put it on the screen

   sav_col = setcolor( getcolor( 5 ) )  && the unselected color
   @ sc_row, sc_col say mval
   setcolor( sav_col )
	_show = &desc_fld
	if show_len > 0
		_show = left( _show + space( show_len ), show_len )
*		testmsg( 'Truncating to ' + str( show_len, 2 ) + ' chars' )
	else
		_show = trim( _show )
	endif
*	testmsg( 'Show = ' + _show )
	if show_desc = 1  
      *---show description 3 spaces after code
		if type( code_fld ) = "N"
			@ sc_row, sc_col+lennum( &code_fld )+3 say _show
		else
			@ sc_row, sc_col+len( &code_fld )+3 say _show
      endif        
   elseif show_desc = 2                     
      *---show description at same place as code
   	@ sc_row, sc_col say _show
   endif

	select &old_alias

return( ok )


procedure LU_TABLE

	parameters mval, talias, code_fld, desc_fld, code_headr, desc_headr, _filt

   _filt = def_parm( "_filt", "L", .t. )

	private lu_fields[2], lu_cols[2], old_area

*	set key -1 to
	old_area = str( select(), 2)

	select &talias
	if _filt
		set filter to .not. CANC_FLAG
		go top  
	endif

	lu_fields[1] = code_fld
	lu_fields[2] = desc_fld
	lu_cols[1]   = code_headr
	lu_cols[2]   = desc_headr

	mval = lookitup( lu_fields, lu_cols )

	if _filt
		set filter to
	endif

*	set key -1 to LU_TABLE

	select &old_area

return


*#   LOOKITUP( fieldname array, column heading array ) uses DBEDIT to return
*    a valid code to the user from a look up table DBF.	 The code returned
*    is the value of the first field.
*					  
*    Eg.
*			  *---Initialization
* 			  private supp_fields[2], supp_cols[2]
* 
* 			  supp_fields[1] = "SUPPCODE"
* 			  supp_fields[2] = "SUPPLIER"
* 
* 			  supp_cols[1]   = "Supplier Code"
* 			  supp_cols[2]   = " Supplier Name"
* 			  *---Find it
* 				msuppcode = LOOKITUP( supp_fields, supp_cols )
* 
*#

function LOOKITUP

	parameters mfields, mcols, top, left, bott, right

	top   = def_parm( "top",   "N", 12)
	left  = def_parm( "left",  "N", 01)
	bott  = def_parm( "bott",  "N", 20)
	right = def_parm( "right", "N", 78)

   private old_color, old_screen, old_rec, ret_val

   old_color = setcolor(c_help)
   old_screen = savescreen( top, left, bott, right)
   old_rec = recno()

   @  top, left, bott, right box 'ÉÍ»º¼ÍÈº '
   @  bott, left+2   say " Enter to Select "
	@  bott, left+21  say " Alt-S to Search "
	@  bott, right-19 say " ESC No Change " 

   DBEDIT( top+1, left+1, bott-1, right-1, mfields, 'SRCH_FUNC', '@X', mcols, "Ä", " ")

   setcolor(old_color)
   restscreen( top, left, bott, right, old_screen)

   ret_val = mfields[1]

return(&ret_val)


*#   ABORT( func)  is the genral abort function used with UI generated
*    basic file update screens.  It is used to allow users to abort the
*    adding or editing of record, by prompting the user with a dialog box.
*
*#

function ESCAPE_OK
   ***
   ***    Returns .T. if ESCAPE was pressed and YES is answered 
   ***    to the undo question, AND sets lastkey() to 27
   ***
	private rcode, mess, pos

   mess = 'Undo Changes ?'
   pos =  40 - (len(mess) + 6)/2

   rcode = .f.
   if lastkey() = 27 
      rcode = dialog( 22, pos, mess, 'Yes', 'No ', NO )
      KEYPRESS( 27 )
      inkey()
   endif

return(rcode) && from abort

*#   MAKEFILE will create a DBF file given the correct parameters of up to
*    9 fields.
*       para1  file_dbf : Name of the DBF you want created
*       para2  no_parm  : The number of fields
*       para3-9         : The details describing the field
*       strings         12345678901234567  of 17 chars
*                       XXXXXXXXXX X NN N	  
*    eg.
* 	do MAKEFILE with tempfile, 5, ;		 
* 							'HOTELCODE  C  2  ',;
* 							'ROOMS      N  3 0',;
* 							'DATE_IN    D  8 0',;
* 							'DATE_OUT   D	8 0',;
* 							'ROOMRATE   N  4 0'
* 
* 
*#

procedure MAKEFILE

	parameters file_dbf, no_parm, f1, f2, f3, f4, f5, f6, f7, f8, f9, f10, f11

	private c, cnt, fld, mcontinu, tmpstru

	tmpstru = TEMPORARY_FILE_INDICATOR + "TEMP"

	select 0
	create &tmpstru.

	mcontinu = .f.
	do while .not. mcontinu
		if net_use( tmpstru, .t., 5 )
			mcontinu = .t.
		endif
	enddo

	c = 1
	do while c <= no_parm
		cnt = str(c,1)
		append blank
		fld = f&cnt
		replace FIELD_NAME with substr( fld,  1, 10 )
		replace FIELD_TYPE with substr( fld, 12,  1 )
		replace FIELD_LEN  with val( substr( fld, 14,  2 ) )
		replace FIELD_DEC  with val( substr( fld, 17,  1 ) )
		c = c + 1
	enddo


	use
	create &file_dbf. from &tmpstru 
	use
*	if .not. gsys_test
		erase( tmpstru )
*	endif

return  



*EOF:  TFL_LIB.PRE 


