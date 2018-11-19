/////////////////////////////////////////////////////////////////////////////
//
//		Clipper 5.01  Print File Handler
//
//


*#   REPORTSETUP( notpapertype ) allows the user to specify LPTn, printer
*    driver, page length in lines, print size, paper width (A4/A3).
*    Loads the print driver sets up a set up string for the options selected.
*    Inappropriate combinations of paper width and print size can be
*    disallowed using the parameter 'notpapertype'.
*
*    reportsetup() sends the printer setup string to the printer
*    reportsetup() should be called inside begin sequence/end to allow for
*    print error breaks
*
*    save the spool settings of the lpt used for spooled output
*    g_spllpt in mc_ctl : what lpt to send spooled output to
*
*    Print que support modes
*    2. - current que id found using netlib, driver for que found in prncfg.dbf
*         ( should work with psprint). Ques are NOT set using netlib.
*    3. - Ques set with netlib, selected by user etc.
*    Current Mode defined by G_PQUEMODE, defined by fncsysin()
*
*    In mode 3 the network specific printer setup routine is called.
*    It is able to do what it will with network ques etc and change
*    the printer driver, lptno.
*    Lines 5 & 6 of the box are available for its input and display.
*    The network printing interface routine
*
*        NetPrint( @prtName, @deviceName )

*    need other functions like ValPrntDevice()
*                              ChsPrntDevice()
*    replacable for the valid printer device names for network installation
*
*#

function REPORTSETUP

   *---set the printer globals dependent on

   parameter notpapertype

	* notpapertype describes the types of paper that will NOT work with
	* the report

   * 1 - A4 paper and normal print
   * 2 - A4 paper and condensed print
   * 3 - A3 paper and normal print
   * 4 - A3 paper and condensed print

	if pcount() < 1
		*---Anything
      notpapertype = ""
   endif

   priv continu, curals, i, choice, curclr
   priv mpaper, mwidth, mlength, mpsize, mdest, mdesttype, mptype, mlptque
   priv mprnserver, mprnque

   continu = .t.

   if type( "g_psv_srv" ) = "U"
      public g_psv_srv, g_psv_que, g_pstart

      *--- the default queue
      set print to ( "LPT1" )
      n_spllpt( 1 )
      n_spool( 1 )
      n_spool( 0 )
      g_pstart = setlen( n_splsrv(), 20 ) + setlen( n_splque(), 20 )

      *--- save the settings of the output LPTx
      set print to ( "LPT"+str( g_spllpt, 1 ) )
      n_spllpt( g_spllpt )
      n_spool( 1 )
      n_spool( 0 )
      g_psv_srv = n_splsrv()
      g_psv_que = n_splque()
*		testmsg( "splsrv:"+g_psv_srv +"--splque:"+g_psv_que +" n_spllpt():" +str( n_spllpt(), 1 ) )
   endif

   curals = alias( 0 )
   curclr = setcolor()
   select 0
   set device to screen

   setcolor( c_helpfld )
   blowup( 3, 18, 15, 56 )
   lineinbox( 13 )
   @ 14, 20 say "Printer Setup."

   for i = 4 to 1 step -1
      if !( str( i, 1 ) $ notpapertype )
         mpaper = str( i, 1 )
      endif
   next

	if type( "p_lpt" ) != "C"
      public p_lpt
		p_lpt = "1"
		set printer to LPT1
   endif

   if type( "p_driver" ) = "C"
      mptype = p_driver
   else
      public p_driver
      mptype = space( 8 )
   endif
   if type( "p_linespp" ) = "N"
      mlength = p_linespp
	else
*		if mpaper $ "12"
*			mlength = 66
*		else
*			mlength = 44
*		endif
		mlength = 66
   endif

   mdesttype = if( g_pquemode = 3, "N", " " )
   mlptque   = ""
   mwidth = if( mpaper $ "12", "A4", "A3" )
   mpsize = if( mpaper $ "13", "N", "C" )

   if g_pquemode = 3
      @4, 20 say "Destination  :   (Network/Local)"

      vget(                     4, 35, "mdesttype", "@K!", .t. )
      vget(                     5, 35, "mlptque"   )
      cvget( "mdesttype = 'L'", 6, 35, "mptype", "@K!" )

      vget_exit( .t., g_getlistkey )
      vread( 1, .f., "repsetval", "", .f. )
      vget_exit( .f., g_getlistkey )

      continu = !abort()
   elseif g_pquemode = 2
      @4, 20 say "Destination  : " + g_psv_srv
      @5, 20 say "Que          : " + g_psv_que

   endif

   @ 7, 20 say "Paper width  :   (A3/A4)"
   @ 8, 20 say "Print size   :   (Normal/Condensed)"
   @ 9, 20 say "Paper length :"
   @ 7, 35 say mwidth
   @ 8, 35 say mpsize
   @ 9, 35 say mlength pict "99"

   do while continu

      @ 11,20 say "Printer ready ?"
      @ 11,38 prompt "Yes"
      @ 11,43 prompt "No"
      @ 11,47 prompt "Options"
      menu to choice

      if choice = 0 .or. choice = 2
         continu = .f.
         exit

      elseif choice = 3
         scroll( 11, 20, 11, 55, 0 )
         if g_pquemode = 3
            vget(  4,35, "mdesttype", "@K!", .t. )
            vget(  5,35, "mlptque"   )
            cvget( "mdesttype = 'L'",      ;
                  6,35, "mptype", "@K!" )
         endif
         vget( 7,35, "mwidth"   , "@!"  )
         vget( 8,35, "mpsize"   , "@!"  )
         vget( 9,35, "mlength"  , "99"  )

         vget_exit( .t., g_getlistkey )
         vread( 1, .f., "repsetval", "", .f. )
         vget_exit( .f., g_getlistkey )
         loop

      endif

      *---setup for local or que
      if g_pquemode = 3
         if mdesttype == "N"
				set print to ( "LPT"+str( g_spllpt, 1 ) )
            n_spllpt( g_spllpt )
            n_splsrv( trim( mprnserver ) )
            n_splque( trim( mprnque ) )
				n_spool( 1 )
            p_lpt = str( g_spllpt, 1 )
         elseif mdesttype == "L"
            * restore queue used by network output
            set print to ( "LPT"+str( g_spllpt, 1 ) )
            n_spllpt( g_spllpt )
            n_splsrv( trim( g_psv_srv ) )
            n_splque( trim( g_psv_que ) )
            n_spool( 1 )

            if mlptque = "0"
               testmsg( "Printer sent to PRINT.OUT" )
               set printer to ( "print.out" )
            else
               set print to ( "LPT" + mlptque )
            endif
            p_lpt = mlptque

         endif

      else
         continu = opendbf( "PRNCFG", g_ad_path )
         if continu
            select PRNCFG
            set order to 1
            dbseek( g_pstart )
            if found()
               mlptque    = PRNCFG->DESC
               mprnserver = PRNCFG->PRNSERVER
               mprnque    = PRNCFG->PRNQUE
               mptype     = PRNCFG->PRTNAME
            else
               continu = .f.
            endif
            closedbf( "PRNCFG" )
         endif
         if !continu
            error_msg( "Driver for current print queue not available !" )
            continu = .t.
            loop
         endif
         set print to ( "LPT"+str( g_spllpt, 1 ) )
         if gsys_test
            if yes_no( "Set print to printer.out", .t. )
               set print to ( "printer.out" )
            endif
         endif

      endif

      *---setup the printer globals

      if !loadprt( mptype )
         error_msg( "Printer table missing." )
         loop
      endif

      p_driver   = mptype
      p_width    = mwidth
      p_charsize = mpsize
      p_linespp  = mlength

      p_setup = ""

      if p_charsize = "N"
         p_setup = p_setup + p_norm
      elseif p_charsize = "C"
         p_setup = p_setup + p_cond
		endif

      if p_width = "A3"
         p_setup = p_setup + p_a3
      elseif p_width = "A4"
         p_setup = p_setup + p_a4
		endif

* 		#IFDEF TEST_MODE
* 		testmsg( 'Pitch string = ' + p_setup )
* 		#ENDIF

		set print on
		set console off
      ?? p_setup
		set console on
      set print off

      exit
   enddo

   shrink()

   setcolor( curclr )
   if !empty( curals )
      select &curals
   endif

return( continu )


function REPSETVAL

   parameter mvar

   priv ok, i
   ok = .t.

   if mvar == "mdesttype"
      if mdesttype = "L"
         if len( mlptque ) != 1
            mlptque = "1"
            @5,35 say space( 20 )
            setcolor( c_help )
            @5,20 say "LPT no       :"
            @6,20 say "Printer type :"
         endif
      elseif mdesttype = "N"
         if len( mlptque ) != 20
            mlptque = space( 20 )
            keyboard chr( 13 )
            setcolor( c_help )
            @5,20 clear to 6,55
            @5,20 say "Print Que    |"
         endif
      else
         ok = .f.
      endif

   elseif mvar == "mlptque"

      if mdesttype = "N"
         * choose a que
         if opendbf( "prncfg", g_ad_path )
            select PRNCFG
            locate for mlptque == PRNCFG->DESC
            if !found() .or. vget_exit = g_getlistkey
               set order to 1
               dbseek( g_pstart )
               if !found()
                  go top
               endif
               blowup( 6,44, 16,67 )
               lineinbox( 14 )
               @15,46 say "Print Queues"
               keybrowse( 7,45,13,66, "rptsetques", "" )
               shrink()
               ok = !abort()
            endif
            if ok
               mlptque    = PRNCFG->DESC
               mprnserver = PRNCFG->PRNSERVER
               mprnque    = PRNCFG->PRNQUE
               mptype     = PRNCFG->PRTNAME
            endif
            closedbf( "prncfg" )
         else
            error_msg( "Print queues not available !" )
            ok = .f.
         endif

      else
         * choose an lpt
         ok = ( mlptque >= "1" .and. mlptque <= "3" ) .or. ( gsys_test .and. mlptque = "0" )
         if mlptque > "0"
            set printer to ( "LPT" + mlptque )
         endif
      endif

   elseif mvar == "mptype"
      if empty( mptype ) .or. !file( g_path + trim( mptype )+".prt" ) .or. vget_exit = g_getlistkey
          i = chooseprt()
          if !empty( i )
             mptype = i
          endif
      endif

   elseif mvar == "mwidth"
      ok = mwidth = "A3" .or. mwidth = "A4"
      if ok
         if mwidth = "A3"
            ok = !( "3" $ notpapertype .and. "4" $ notpapertype )
         elseif mpaper = "A4"
            ok = !( "1" $ notpapertype .and. "2" $ notpapertype )
         endif
         if !ok
            error_msg( mwidth + " type paper will not work with this print out." )
         endif
      endif

   elseif mvar == "mpsize"
      ok = mpsize $ "CN"
      if ok
         if mwidth = "A4"
            if mpsize = "N"
               ok = !( "1" $ notpapertype )
            elseif mpsize = "C"
               ok = !( "2" $ notpapertype )
            endif
         elseif mwidth = "A3"
            if mpsize = "N"
               ok = !( "3" $ notpapertype )
            elseif mpsize = "C"
               ok = !( "4" $ notpapertype )
            endif
         endif
         if !ok
            error_msg( mwidth +" and "+ if( mpsize="N", "Normal", "Condensed" ) + " print not appropriate." )
         endif
      endif

   elseif mvar == "mlength"
      ok = mlength > 10

   endif

   ok = ok .and. !abort()

return( ok )


FUNCTION RPTSETQUES

   parameter mode

   if mode = 5
      return( "" )
   elseif mode = 6
      return( " "+PRNCFG->DESC+" " )
   elseif mode = 3
      return( 0 )
   else
      if lastkey() = 13
         return( 0 )
      endif
   endif

RETURN( 1 )


* global varnames prefixed with pb_ (print buffer)

FUNCTION PRINT_SETUP

	parameters linesize, lContinuous

	***
	***	Pass a .t. in lContinous if you dont want page throws
	***
	lContinuous = def_parm( "lContinuous", "L", .f. )

	if linesize = 0
		linesize = 250
	endif

	public pb_leftmrg, pb_topmrg, pb_cols, pb_botmrg,;
		pb_head, pb_headok, prow, pcol, pb_buffer, pb_pagemrg, ;
		pb_line, printpageno, pb_blanklines
	public pb_cont

	if type( "p_linespp" ) != "N"
		public p_linespp
		p_linespp = 66
	endif

	pb_cols     = linesize
	pb_leftmrg   = 0
	pb_topmrg    = 0
	pb_botmrg    = 0
	pb_pagemrg   = 0
	prow          = 1
	pcol          = 1
	pb_head       = ""
	pb_headok     = .t.
	pb_line     = space( linesize )
	pb_buffer  = .f.
	printpageno   = 1
	pb_blanklines = -3
	pb_cont       = lContinuous
	set console off

return( "" )


FUNCTION PRNTPAGEBUF

	public pb_line[p_linespp]

	afill( pb_line, "" )
	pb_buffer = .t.

return( "" )


FUNCTION PRINT_RELEASE

	release pb_line, pb_cont,;
	pb_leftmrg, pb_topmrg, ;
	pb_cols, prow, pb_botmrg, pb_head, pb_headok,;
	prow, pcol, pb_buffer, pb_pagemrg, printpageno, pc_colnxt, pc_colmarg

	set console on
	set print off

return( "" )


FUNCTION PRINT_MARGIN

	parameters left, top, bot, page

	if pcount() >= 1
		pb_leftmrg = if( left >= 0, left, pb_leftmrg )
	endif

	if pcount() >= 2
		pb_topmrg = if( top >= 0, top, pb_topmrg )
	endif

	if pcount() >= 3
		pb_botmrg = if( bot >= 0, bot, pb_botmrg )
	endif

	if pcount() >= 4
		pb_pagemrg = if( page >= 0, page, pb_pagemrg )
	endif

return( "" )


FUNCTION PRINT_HEADER

	parameter udf_name

	pb_head = udf_name
   pb_headok = .f.
	if !empty( pb_head )
		if !( "(" $ pb_head )
			pb_head = pb_head + "()"
		endif
		private udfret
		pb_headok = .f.
		udfret = &pb_head
		pb_headok = .t.
	endif

return( "" )


FUNCTION PRINTAT

   parameters col, output, pictstr

	if pcount() < 3
		pictstr = ""
   endif

   if pb_buffer
      PrintRow( 0, col, output, pictstr )
   else

	   output = transform( output, pictstr )
	   if col <= 0
		   if col = 0
			   col = pcol
		   elseif col = -1
			   col = int( pb_cols / 2 - len( output ) / 2 ) -pb_leftmrg
		   elseif col = -2
			   col = pb_cols -len( output ) -pb_leftmrg +1
		   endif
	   endif

		pb_line = stuff( pb_line, col + pb_leftmrg, len( output ), output )
	endif
	pcol = col + len( output )

RETURN( "" )


function PrintRow

   parameters line, col, output, pictstr

   line = if( line = 0, prow, line )

	if pcount() < 4
		pictstr = ""
   endif

	output = transform( output, pictstr )
	line = if( line = 0, prow, line )
	if col <= 0
		if col = 0
			col = pcol
		elseif col = -1
			col = int( pb_cols / 2 - len( output ) / 2 ) -pb_leftmrg
		elseif col = -2
			col = pb_cols -len( output ) -pb_leftmrg +1
		endif
	endif

  	prow = line
	line = line + pb_topmrg
	pb_line[line] = pb_line[line] + space( pb_cols - len( pb_line[line] ) )
	pb_line[line] = trim( stuff( pb_line[line], col + pb_leftmrg, len( output ), output ) )

	pcol = col + len( output )

RETURN( "" )


FUNCTION PRINTLN

   parameters col, output, pictstr

	if pcount() = 3
		printat( col, output, pictstr )
	elseif pcount() = 2
		printat( col, output )
	endif

   pcol = 1

	if !pb_buffer

		if prntNbOk()
			set print on
			? space( pb_pagemrg ) + trim( pb_line )
			set print off

			prow = prow + 1
			pb_line = space( pb_cols )

			* if the print has gone to the bottom margin, new page, call header func
			if prow >= p_linespp - pb_botmrg .and. pb_headok
				print_page( !( empty( pb_head ) ) )
			endif

		endif

	else

		if prntNbOk()
			prow = prow + 1
			if pb_topmrg + prow >= p_linespp - pb_botmrg .and. pb_headok
				prow = pb_topmrg
				pb_headok = .f.
				private udfret
				udfret = &pb_head
				pb_headok = .t.
			endif

		endif

	endif

return( "" )


FUNCTION PRINT_PAGE

	parameters headings

	if pcount() < 1
		headings = .f.
	endif

	if pb_buffer
		set print on
		for i = 1 to p_linespp
			? space( pb_pagemrg ) + trim( pb_line[i] )
		next
		afill( pb_line, "" )

		if .not. pb_cont
			?? p_top
		endif

		printpageno = printpageno + 1
      set print off
      prow = 1
		if headings
			do while prow < pb_topmrg
				prow = prow + 1
			enddo

			if pb_headok
				pb_headok = .f.
				priv udfret
				udfret = &pb_head
				pb_headok = .t.
			endif
		endif

	else
		set print on

		if .not. pb_cont
			?? p_top
		endif
		prow = 1

		set print off
		printpageno = printpageno + 1
		if headings
			set print on
			do while prow < pb_topmrg
				?
				prow = prow + 1
			enddo
			set print off

			if pb_headok
				pb_headok = .f.
				priv udfret
				udfret = &pb_head
				pb_headok = .t.
			endif
		endif
	endif

return( .t. )



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


function PrntColSet

	parameters newcol

	* set the print column

	priv oldcol
	oldcol = pc_colnxt
	pc_colnxt = newcol

return( oldcol )


function printRemain

* how many rows remaining on the page
*-----------------------

	parameters need

	if pcount() = 1
		if p_linespp - pb_botmrg - prow < need
			print_page( .t. )
		endif
	endif

return( p_linespp - pb_botmrg - prow )



function PrntNonBlank

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



function prntNbOk

	if pb_blanklines != -3
		if empty( pb_line )
			retu( .f. )
		elseif pb_blanklines = -5
			pb_blanklines = -3
		endif
	endif

return( .t. )


function VIEW_PRNT

   parameters printto, length

   private modeReset, doReset, maxrows, scrsave

   modeReset = 0
   doReset = .f.

   if pcount() < 2
      length = 200
   endif

   if !file( printto )
		error_msg( "No report has been generated" )
*		TestMsg( 'File: ' + printto )
      retu( .f. )
   endif

   vwPrnMode( @doReset, @modeReset)

   setcolor( c_help )
   blowup( 1,1, maxrow()-1,78 )
   print( maxrow()-1,4, "PgUp PgDn "+chr(27)+chr(24)+chr(25)+chr(26)+" to move" )

   ****need to change length here  DEBUG
   *fbrowse( 2,2, maxrow()-2,77, printto, length )
   fbrowse( 5,2, maxrow()-2,77, printto, length )
   shrink()

   vwPrnReMode( doReset, modeReset )

return( .t. )


function vwPrnMode

   parameters doReset, modeReset

   scrsave = savescreen( 0,0, maxrow(),79 )
   doReset = .f.

   *----DEBUG we don't have g_systest - this section can be removed
   if maxrow() = 24  .and. vidtype() > 1 .and. gsys_test  && normal screen mode, ega/vga card
      if vidtype() = 2
         maxrows = 43
      elseif vidtype() = 3
         maxrows = 50
      endif
      if yes_no( "Use " +str( maxrows, 2 ) +" line mode" )
         modeReset = getmode()
         doReset = .t.
         setmode( maxrows )
      endif
   endif

return( 0 )


function vwPrnReMode

   parameters doReset, modeReset

   if doReset
      setmode( modeReset )
   endif
   restscreen( 0,0, maxrow(),79, scrsave )
   if maxrow() > 24
      setcolor( c_blank )
      @25,0 clear to maxrow(),79
   endif

return( 0 )

function PRINT_PRNT

   parameter printto

   private handle, i, abort
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
            if inkey() = 27
               if !print_cont()
                  abort = .t.
                  exit
               endif
            endif
            ? freadline( handle )
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

RETURN( 0 )


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


*---DEBUG  Don't need provide option print to disk
*-----------------------
function print2disk

* copy the report to a disk file
*-----------------------

   parameters printto, mfile

   priv mdrv, mdir

   if !file( printto )
      error_msg( "No Report has  been generated" )
      RETU( 0 )
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

   vget( 15,19, "mdrv", "!" , .t. )
   vget( 15,21, "mdir", "@!", .f. )
   vread( 1, .f., "valprn2dsk" )

   shrink()

RETURN( 0 )


*-----------------------
function valprn2dsk

* vread valid for print2disk()
*-----------------------

   parameters mvar

   priv ok
   ok = .t.

   if mvar == "mdir"

      mdir = alltrim( mdir )
      if !empty( mdir )
         mdir = mdir + if( right( mdir, 1 ) = "\", "", "\" )
      endif

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

   if inkey() = 27
      if !print_cont()
         return( .f. )
      endif
   endif
   reccnt = reccnt + 1
   print( 21,41, str( reccnt ) )

return( .t. )


function printToLpt

*-----------------------

   if type( "p_lpt" ) = "U" .or. if( type( "gPrnSetLpt" )="L", !gPrnSetLpt, .f. )
      set print to   && ( "PRN" )
      testmsg( " Print set to " )
   elseif p_lpt > "0"
      set print to ( "LPT" + p_lpt )
      testmsg( "Print set to " + ( "LPT" + p_lpt ) )
   elseif gsys_test
      set print to ( "printer.out" )
      testmsg( "Print set to printer.out" )
   endif

return( 0 )


*-----------------------
function printIsLpt

* print device name is prn only
*-----------------------

   parameters mset

   public gPrnSetLpt
   gPrnSetLpt = mSet

return( mSet )

function GenMsg

	GenMsgClr()

	setcolor( c_msg )
   atcentre( 20, "Generating... Press ESC to abort" )

return(.t.)

function GenMsgClr

   setcolor( c_blank )
   @ 20,0

return(.t.)

* standard menu contents functions


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

function LOADPVARS

	parameter mptype

	* loads default printer memvars

	public p_linespp, p_setup

	public p_init, p_6lpi, p_boldon, p_boldoff, p_enlgeon, p_enlgeoff, p_ulineon
	public p_ulineoff, p_cmprss, p_uncmpr, p_6lpi, p_8lpi, p_16lpi, p_a4
	public p_a3, p_cpi10, p_cpi12, p_cpi16, p_colzero, p_top, p_nlq
	public p_draft, p_cond, p_norm

	p_init     = ""
	p_boldon   = ""
	p_boldoff  = ""
	p_enlgeon  = ""
	p_enlgeoff = ""
	p_ulineon  = ""
	p_ulineoff = ""
	p_cmprss   = ""
	p_uncmpr   = ""
	p_6lpi     = ""
	p_8lpi     = ""
	p_16lpi    = ""
	p_a4       = ""
	p_a3       = ""
	p_cpi10    = ""
	p_cpi12    = ""
	p_cpi16    = ""
	p_colzero  = ""
	p_top      = ""
	p_nlq      = ""
	p_draft    = ""

	p_cond     = p_cpi16
	p_norm     = p_cpi10

	p_linespp  = 66
	p_setup    = ""

RETURN( "" )


function PRINT_CONT

   *---continue or abort printing

   parameter _reason

   private _saveclr, _ok
   _saveclr = setcolor()
*    #IFDEF USE_DEVICE_TO_SCREEN
      set device to screen
*    #ENDIF

   if pcount() = 1
      error_msg( _reason )
   endif
   _ok = dialog( 21,28, "Continue Printing ?", "Yes", "No", .t. )

*    #IFDEF USE_DEVICE_TO_SCREEN
   if  _ok
	   set device to print
   endif
*    #ENDIF

   setcolor( _saveclr )

return( _ok )


*EOF:		PRINT.PRG
