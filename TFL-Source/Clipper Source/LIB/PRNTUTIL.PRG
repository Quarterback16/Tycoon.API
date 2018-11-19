* global varnames prefixed with pb_ (print buffer)

#include "tfl.h"
#INCLUDE "PRINT.H"

#define left_margin  pb_leftmrg
#define top_margin   pb_topmrg
#define bot_margin   pb_botmrg
#define page_margin  pb_pagemrg
#define printcols    pb_cols
#define header_ok    pb_headok
#define print_buffer pb_buffer
#define printline    pb_line
#define PAGE_THROW   ?? chr(12)


********************
FUNCTION PRINT_SETUP

	parameters linesize, lContinuous

	***
	***	Pass a .t. in lContinous if you dont want page throws
	***
	lContinuous = def_parm( "lContinuous", "L", NO )

	if linesize = 0
		linesize = 250
	endif

	public left_margin, top_margin, printcols, bot_margin,;
		pb_head, header_ok, prow, pcol, print_buffer, page_margin, ;
		printline, printpageno, pb_blanklines
	public pb_cont

	if type( "p_linespp" ) = "U"
		public p_linespp
		p_linespp = 66
	endif

	printcols     = linesize
	left_margin   = 0
	top_margin    = 0
	bot_margin    = 0
	page_margin   = 0
	prow          = 1
	pcol          = 1
	pb_head       = NULL_STRING
	header_ok     = .t.
	printline     = space( linesize )
	print_buffer  = .f.
	printpageno   = 1
	pb_blanklines = PrintBlankOn
	pb_cont       = lContinuous
	set console off

return( "" )


********************
FUNCTION PRNTPAGEBUF

	public printline[p_linespp]

	afill( printline, "" )
	print_buffer = .t.

return( NIL )



**********************
FUNCTION PRINT_RELEASE

	release printline, pb_cont,;
	left_margin, top_margin, ;
	printcols, prow, bot_margin, pb_head, header_ok,;
	prow, pcol, print_buffer, page_margin, printpageno, pc_colnxt, pc_colmarg

	set console on
	set print off

return( NULL_STRING )


*********************
FUNCTION PRINT_MARGIN

	parameters left, top, bot, page

	if pcount() >= 1
		left_margin = if( left >= 0, left, left_margin )
	endif

	if pcount() >= 2
		top_margin = if( top >= 0, top, top_margin )
	endif

	if pcount() >= 3
		bot_margin = if( bot >= 0, bot, bot_margin )
	endif

	if pcount() >= 4
		page_margin = if( page >= 0, page, page_margin )
	endif

return( "" )


*********************
FUNCTION PRINT_HEADER

	parameter udf_name

	pb_head = udf_name
	if !empty( pb_head )
		if !( "(" $ pb_head )
			pb_head = pb_head + "()"
		endif
		private udfret
		header_ok = .f.
		udfret = &pb_head
		header_ok = .t.
	endif

return( "" )


FUNCTION PRINTAT( p1, p2, p3, p4 )

	if print_buffer
*		parameters line, col, output, pictstr

		line    := p1
		col     := p2
		output  := p3
		pictstr := p4

		line = if( line = 0, prow, line )

	else
*		parameters col, output, pictstr

		col     := p1
		output  := p2
		pictstr := p3

	endif
	if type( "pictstr" ) = "U"
		pictstr = ""
	endif
	output = transform( output, pictstr )
	if col <= 0
		if col = 0
			col = pcol
		elseif col = PRINT_CENTER
			col = int( printcols / 2 - len( output ) / 2 ) -left_margin
		elseif col = PRINT_RIGHT
			col = printcols -len( output ) -left_margin +1
		endif
	endif

	if print_buffer
		line = if( line = 0, prow, line )
		prow = line
		line = line + top_margin
		printline[line] = printline[line] + space( printcols - len( printline[line] ) )
		printline[line] = trim( stuff( printline[line], col + left_margin, len( output ), output ) )
	else
		printline = stuff( printline, col + left_margin, len( output ), output )
	endif
	pcol = col + len( output ) 

RETURN( NULL_STRING )



****************
FUNCTION PRINTLN( p1, p2, p3, p4 )

	if !print_buffer
*		parameters col, output, pictstr
		col     := p1
		output  := p2
		pictstr := p3

		if pcount() = 3
			printat( col, output, pictstr )
		elseif pcount() = 2
			printat( col, output )
		endif

		pcol = 1
		if prntNbOk()

			set print on
			? space( page_margin ) + trim( printline )
			set print off

			INC( prow )
			printline = space( printcols )

			* if the print has gone to the bottom margin, new page, call header func
			if prow >= p_linespp - bot_margin .and. header_ok
				print_page( .t. )
			endif

		endif

	else
*		parameters line, col, output, pictstr

		line    := p1
		col     := p2
		output  := p3
		pictstr := p4

		if pcount() = 4
			printat( line, col, output, pictstr )
		elseif pcount() = 3
			printat( line, col, output )
		endif

		pcol = 1
		if prntNbOk()

			INC( prow )
			if top_margin + prow >= p_linespp - bot_margin .and. header_ok
				prow = top_margin
				if !( pb_head == "" )
					header_ok = .f.
					private udfret
					udfret = &pb_head
					header_ok = .t.
				endif
			endif

		endif

	endif

return( NULL_STRING )



*******************
FUNCTION PRINT_PAGE

	parameters headings

	if pcount() < 1
		headings = .f.
	endif

	if print_buffer
		set print on
		for i = 1 to p_linespp
			? space( page_margin ) + trim( printline[i] )
		next
		afill( printline, "" )

		if .not. pb_cont
			PAGE_THROW
		endif

		printpageno = printpageno + 1
		set print off

	else

		if .not. pb_cont
			set print on
			PAGE_THROW
			set print off
		endif

		printpageno = printpageno + 1
		if headings
			set print on
			prow = 1
			do while prow < top_margin
				?
				prow = prow + 1
			enddo
			set print off

			if !( pb_head == "" )
				header_ok = .f.
				priv udfret
				udfret = &pb_head
				header_ok = .t.
			endif
		endif
	endif

return( .t. )


*******************
FUNCTION PRNTCOLDEC

	parameters colmarg

	public pc_colnxt, pc_colmarg
	pc_colnxt = 1
	pc_colmarg = if( pcount() >= 1, colmarg, 2 )

return( 0 )


*******************
FUNCTION PRNTCOLNXT

	parameters colvar, width, plus, leftmarg

	if pcount() = 0
		retu( pc_colnxt )
	endif
	if pcount() < 3
		plus = 0
	endif

	if type( "width " ) = "C"
		width = len( width )
	elseif type( "width" ) = "D"
		width = len( dtoc( width ) )
	endif

	if pc_colnxt = 1
		if pcount() < 4
			leftmarg = 0
		endif
	else
		if pcount() < 4
			leftmarg = pc_colmarg
		endif
	endif

	colvar = pc_colnxt + leftmarg
	pc_colnxt = colvar + width + plus

return( colvar )


*******************
function prntColSet

	parameters newcol

	* set the print column

	priv oldcol
	oldcol = pc_colnxt
	pc_colnxt = newcol

return( oldcol )



********************
function printRemain

	* how many rows remaining on the page
	*-----------------------

	parameters need

	if pcount() = 1
		if p_linespp - bot_margin - prow < need
			print_page( .t. )
		endif
	endif

return( p_linespp - bot_margin - prow )



*********************
function prntNonBlank

	* status of printing of blank println()s
	*    PrintBlankWait - do not print blanks until next non blank is encounted
	*    PrintBlankOff  - do not print blanks
	*    PrintBlankOn   - print blanks
	*-----------------------

	parameters setto

	if pcount() > 1
		pb_blanklines = setto
	endif

return( pb_blanklines )



*****************
function prntNbOk

	*--------------------

	if pb_blanklines != PrintBlankOn
		if empty( printline )
			retu( .f. )
		elseif pb_blanklines = PrintBlankWait
			pb_blanklines = PrintBlankOn
		endif
	endif

return( .t. )


#include "cfg90.h"

function VIEW_PRNT

   parameters printto, length          

   LOCAL modeReset, doReset, maxrows, scrsave

   modeReset = 0
   doReset = .f.

   if pcount() < 2
      length = 200
   endif          

   if !file( printto )
      error_msg( "No report has been generated" )
      retu( .f. )
   endif

   * vwPrnMode( @doReset, @modeReset)

   setcolor( c_help )
   blowup( 1,1, maxrow()-1,78 )
   print( maxrow()-1,4, "PgUp PgDn "+chr(27)+chr(24)+chr(25)+chr(26)+" to move" )
   fbrowse( 2,2, maxrow()-2,77, printto, length )
   shrink()         

   * vwPrnReMode()

return( .t. )


* function vwPrnMode
* 
*    parameters doReset, modeReset
* 
*    scrsave = savescreen( 0,0, maxrow(),79 )
*    doReset = .f.
*    if maxrow() = 24  .and. vidtype() > 1 .and. gsys_test  && normal screen mode, ega/vga card
*       if vidtype() = 2
*          maxrows = 43
*       elseif vidtype() = 3
*          maxrows = 50
*       endif
*       if yes_no( "Use " +str( maxrows, 2 ) +" line mode" )
*          modeReset = getmode()
*          doReset = .t.
*          setmode( maxrows )
*       endif
*    endif
* 
* return( 0 )     
* 
* 
* function vwPrnReMode
* 
*    parameters doReset, modeReset
* 
*    if doReset
*       setmode( modeReset )
*    endif
*    restscreen( 0,0, maxrow(),79, scrsave )
*    if maxrow() > 24
*       setcolor( c_blank )
*       @25,0 clear to maxrow(),79
*    endif
* 
* return( 0 )

function PRINT_PRNT

   parameter printto

   LOCAL handle, aline, i, abort
   abort = .f.

   if !file( printto )
      error_msg( "No report has been generated" )
      retu( .f. )
   endif

   setcolor( c_blank )
   @ 20,0
	setcolor( c_msg )

   atcentre( 20, "Printing... Press ESC to abort" )

   handle = fopen( printto, 2 )
   if handle >= 0              

		begin sequence  && printer error handling

			***
         ***   copy file ( printto ) to ( "LPT" + p_lpt )
         ***
         ***   pump print file out line by line, allows user to escape and
         ***   allows printer errors to be handled cleanly
         ***

         set console off
         printToLpt()
         set print on

         do while !feof( handle )
            if inkey() = ESCAPE
               if !print_cont()
                  abort = .t.
                  exit
               endif
            endif
            aline = freadline( handle )
            ? aline
			enddo

		end   

      fclose( handle )
      set print off
      set console on  

      set print to

   endif

   setcolor( c_blank )
   @ 20,0

return( !abort )


function tmpsetprnt

*    * get_spool (spolnf1)
*    *
*    * a temporary measure until the print driver for hplasers is finished
* 
* 
*    restore from ( g_path + "spool" ) additive         && restores spool info
*    laserset('L', 'C', 6)
*    set console off
*    set print on
*    ?? lpitch16
*    set print off

return( 0 )


*
* example of use for maxlen
* prntColNxt( @etc, maxlen( g_group1shrt, group1->group1 ), 2 )
*

function maxlen

   parameters str1, str2, str3

   priv l1, l2

   l1 = len( str1 )
   l2 = len( str2 )
   if l2 > l1
      l1 = l2
   endif      
   if pcount() >= 3
      l2 = len( str3 )
      if l2 > l1
         return( l2 )
      else
         return( l1 )
      endif
   else
      return( l1 )
   endif


function open_prnt

   parameter printto

   setcolor( c_msg )
	atcentre( 23, "Generating Report, press ESC to abort" )
   print( 21,30, "Record no :" )

   set print to ( printto )
   set console off
   set print on
   ?? p_setup
   set print off

return( 0 )


function close_prnt

   parameter printto

   print_release()
   if type( "p_a4" ) = "C"
      set print on
      set console off
      ??p_a4
      set print off
      set console on
   endif
   set print to         
            
   if abort()
      erase ( printto )
   endif

   setcolor( c_blank )
   @ 23,0 clear to 24,79
   @ 21,0

return( 0 )


*-----------------------
function print2disk

* copy the report to a disk file
*-----------------------

   parameters printto, mfile

   priv mdrv, mdir

   if !file( printto )
      error_msg( "No Report has  been generated" )
      return( 0 )
   endif

   setcolor( c_helpfld )
   blowup( 10,17, 21,62 )
   lineinbox( 19 )                           
   print( 20,19, "Print Report to File" )
   print( 12,19, "File to copy report to :" )
   print( 14,19, "Drive + Directory :" )
   print( 15,20, ":" )
   print( 17,19, mfile )

   mdrv = " "
   mdir = space( 40 )

   vget( 15,19, "mdrv" , "!" , .t. )
   vget( 15,21, "mdir", "@!", .t. )
   vread( 1, .f., "valprn2dsk" )

   shrink()

return( 0 )


*-----------------------
function valprn2dsk

* vread valid for print2disk()
*-----------------------

   parameters mvar

   priv ok
   ok = .t.

   if mvar == "mdir"     

      mdir = alltrim( mdir )
      mdir = mdir + if( right( mdir, 1 ) = "\", "", "\" )

      if file( mdrv +":"+ +mdir +mfile )
         ok = yes_no( "This file already exists, Over write ?" )
      endif

      if ok
         ok = fcopy( printto, mdrv +":"+ +mdir +mfile ) > -1
         error_msg( "Print to file " + if( ok, "OK", "UNsuccessful" ) )
      endif

      mdir = setlen( mdir, 40 )

   endif

return( ok )


*-----------------------
function rptConsole

* report console activity
*----------------------- 

   if inkey() = ESCAPE
      if !print_cont()
         return( .f. )
      endif
   endif                
   reccnt = reccnt + 1
   print( 21,41, str( reccnt ) )

return( .t. )


*----------------------- 
function printToLpt

*----------------------- 

   if type( "p_lpt" ) = "U"
      set print to ( "PRN" )
   elseif p_lpt > "0"
      set print to ( "LPT" + p_lpt )
   elseif gsys_test
      set print to ( "printer.out" )
      testmsg( "Print set to printer.out" )
   endif

return( 0 )



*EOF:  PRNTUTIL.PRG

