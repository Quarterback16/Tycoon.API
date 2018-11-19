#INCLUDE "TFL.H"

*#   OPENDBF( what, pathname[, exclusiv]) will open files according to the 
*    defined DBFs and NTXs in the Index Control File  (INDEXES.DBF)
*    Parameter:  what  - either a DBFNAME or "*"
*                pathname - dir  Note all files must reside in the same 
*					  				 directory
*                exclusiv - whether to open files exclusively or not
*										default is .f.
*
*    files opened successfully ( by alias )are recorded and 
*    if reuested again are not opened. 
*    the index control file can remain between calls to opendbf
*
*#

field DBFNAME, NTX_ORDER, FILENAME, ALIASNAME, INDEXTYPE, INDEXKEY
field INDEXNAME

function OPENDBF

	parameter what, mpathname, exclusiv, _indexno

    _indexno = def_parm( '_indexno', 'C', '*' )

    do LOG with "Opening " + what + "path: " + mpathname   	
	
	what = upper( what )

	if pcount() = 1
		mpathname = ""
        exclusiv  = .f.
	elseif pcount() = 2 
		exclusiv  = .f.
    endif								

   if type( "g_filemode" ) = "U"
      public g_filemode, g_noclose, g_openpath
      g_filemode = .f.            
      g_noclose = .f.                         
      g_openpath = ""
   endif

	private ok, ntx1, oldcolr, single, openedok
    private mdbfname, mfilename, malias, sub, i, ntxs[5], openitx[10], itxcnt
    itxcnt = 0

	ok = .t.
	oldcolr = setcolor()
	if what == '*'
		plswait()
		single = .f.
	else
      single = .t.
      if g_filemode
         i = ascan( g_openfile, setlen( what, 8 ) )
         if i > 0
            g_closfile[i] = .f.
            return( .t. )
         endif
      endif
    endif

   *---Open Index Control file
   if !( g_noclose .and. g_openpath == mpathname )  && index file may already be open
      if empty( g_openpath )
         select 0
      else
         select INDEXES
      endif
	   if net_use( mpathname + 'INDEXES', .f., 5, 'INDEXES')
 		   ntx1 =  mpathname + 'INDEXIND.NTX'
   		if .not. file( ntx1 )
	   		dispmsg(23,'Rebuilding index file ' + ntx1,.F.,1,.t.)
		   	index on upper(DBFNAME)+NTX_ORDER to ( ntx1 )
   		endif
	   else
		   error_msg( 'Cant open Index control File' )
   		retu(.f.)
      endif
      set index to ( mpathname + "indexind" )
   endif

   if single   
      select INDEXES
      if .not. INDEXES->( dbseek( setlen( what, 8 ) ) )
         ok = .f.
      endif
	endif

	do while .not. eof() .and. ok
		*---Open DBF
		select INDEXES
		mdbfname  = upper( DBFNAME)
		mfilename = TRIM(FILENAME)
		malias    = ALIASNAME
		if empty( malias )
			malias = mdbfname
        endif

      if if( g_filemode, ascan( g_openfile, setlen( mdbfname, 8 ) ) = 0, .t. )

         openedok = .f.
   		 do OPENIT with mpathname, mdbfname, mfilename, malias, openedok
			if openedok
				if _indexno == "*"
					*---Count the indexes
					ntx_cnt = 0
					select INDEXES
               do while upper( DBFNAME) = mdbfname .and. .not. eof() .and. ok

                  if if( type( "INDEXES->INDEXTYPE" ) = "C", INDEXES->INDEXTYPE == " ", .t. )

							mindexname = trim( INDEXNAME )
							ntx_cnt = ntx_cnt + 1
							ntxs[ntx_cnt] = mpathname + mindexname
							if !file( ntxs[ntx_cnt] + ".ntx" )
								mindexkey  = trim( INDEXKEY )
								do CHKINDEX with mpathname, mindexkey, mindexname, ;
													  mfilename, malias
                    endif

                  elseif INDEXES->INDEXTYPE == "T"
                     *---identify ctfs indexes for opening later
                     itxcnt = itxcnt + 1
                     openitx[itxcnt] = recno()

                  endif

						select INDEXES
						skip
					enddo

					select &malias
					do NTX_SET with ntx_cnt
				else
					*---Only open one index
					select INDEXES
					dbseek( mdbfname + _indexno )
					if found() 
*						testmsg( 'Setting index ' + _indexno + ' on only' )
						mindexname = trim( INDEXNAME )
						ntx_cnt = 1
						ntxs[ ntx_cnt ] = mpathname + mindexname
						if !file( ntxs[ntx_cnt] + ".ntx" )
							mindexkey  = trim( INDEXKEY )
							do CHKINDEX with mpathname, mindexkey, mindexname, ;
												  mfilename, malias
						endif
						select &malias
						set index to ( ntxs[1] )
					else
						testmsg( 'Bad index no' )
						ok = .f.
					endif
				endif
   		else
	   		if Gsys_test
		   		   error_msg( 'L Could not open ' + mpathname + mdbfname )
			   else
				   error_msg( 'Could not open ' + mfilename )
   			endif
	   		ok = .f.	 &&  to break out of loop
		   	*---Dont go any further
   		endif
         select INDEXES
      else
         * file is already open, goto the next file
   		select INDEXES
         skip
         locate rest for !( DBFNAME == mdbfname )
      endif

      if g_filemode
         i = ascan( g_openfile, setlen( mdbfname, 8 ) )
         if i > 0
            g_closfile[i] = .f.
         endif
      endif

      if single
         exit   && for execution speed

 		   * select INDEXES
 		   * go bottom
 		   * skip
      endif
 	  select INDEXES

   enddo			 

   * open the ctfs indexes found

   #IFDEF USE_CFTS_INDEXES
   i = 1
   do while i <= itxcnt
      select INDEXES
      go openitx[i]        
		malias = if( empty( ALIASNAME ), DBFNAME, ALIASNAME )
      select &malias
      ok = open_fts( mpathname+INDEXES->INDEXNAME, ;
                 INDEXES->INDEXKEY, exclusiv, ;
                 20, .f., 1 ) .and. ok
      i = i + 1                       
   enddo
   #ENDIF

   if !g_noclose
   	  select INDEXES
      use  
      g_openpath = ""
   else
      g_openpath = mpathname
   endif

return( ok )


procedure OPENIT

	parameters mpathname, mdbfname, mfilename, malias, openedok

    private fname

	fname = mpathname + mdbfname
	exclusiv = if( type("exclusiv")="U", .f., exclusiv )

	select 0
	if Gsys_test
	   dispmsg( 23, "Attempting to open " + fname, .f.,2,.f.)
	endif
    if net_use( fname, exclusiv, 5, malias )
       openedok = .t.

      if g_filemode
         * file opened ok, record file as open
         ains( g_openfile, 1 )
         ains( g_closfile, 1 )
         g_openfile[1] = setlen( mdbfname, 8 )
         g_closfile[1] = .f.
      endif

	else
	   openedok = .f.
   endif

return


procedure CHKINDEX

	*---Assumes an open DBF file with no index set

	parameters mpathname, mindexkey, mindexname, mfilename, malias

	mindexname = mpathname+mindexname+'.NTX'
	if .not. file( mindexname)
		*---Its not there so rebuild it
		msg = "Indexing " + trim(mfilename) + " to " + mindexname 
		msg = iif( gsys_test, msg + " Alias:" + malias, msg )
		dispmsg( 23, msg, .f.,2,.f.)
		select &malias
		index on &mindexkey to &mindexname
	endif

return


procedure NTX_SET

	parameter ntx_cnt

	do case
		case ntx_cnt = 1
			set index to (ntxs[1])
		case ntx_cnt = 2
			set index to (ntxs[1]), (ntxs[2])
		case ntx_cnt = 3
			set index to (ntxs[1]), (ntxs[2]), (ntxs[3])
		case ntx_cnt = 4
			set index to (ntxs[1]), (ntxs[2]), (ntxs[3]), (ntxs[4])
		case ntx_cnt = 5
			set index to (ntxs[1]), (ntxs[2]), (ntxs[3]), (ntxs[4]), (ntxs[5])
		otherwise
			error_msg( 'Only 5 indexes can be opened!' )
			shutdown()
	endcase

RETURN


function CLOSEDBF

   parameter what, mpathname

   private i

	what = upper( what )

	if pcount() = 1
		mpathname = ""
	endif

	private ok, ntx1, oldcolr, single, openedok
	private mdbfname, mfilename, malias, sub

	ok = .t.
   oldcolr = setcolor()		 
   single = !( what == '*' )
   if single
      if g_filemode
         i = ascan( g_openfile, setlen( what, 8 ) )
         if i != 0
            select &what

            #IFDEF USE_CFTS_INDEXES
            fts_close()
            #ENDIF

            use            
            adel( g_openfile, i )
            adel( g_closfile, i )
         endif                   
      else
         select &what

         #IFDEF USE_CFTS_INDEXES
         fts_close()
         #ENDIF

         use            

      endif          
      retu( ok )      
   endif

	*---Open Index Control file
	select 0
	if net_use( mpathname + 'INDEXES', .f., 5, 'INDEXES')
 		ntx1 =  mpathname + 'INDEXIND.NTX'
		if .not. file( ntx1 )
			dispmsg(23,'Rebuilding index file ' + ntx1,.F.,1,.t.)
			index on upper(DBFNAME)+NTX_ORDER to &ntx1
		endif
	else
		error_msg( 'Cant open Index control File' )
		retu(.f.)
	endif
   set index to &mpathname.indexind

	if single
		dbseek( what )
	else
		go top
	endif
	
	do while .not. eof() .and. ok
		*---Close DBF
		select INDEXES
		mdbfname  = upper( DBFNAME)
		mfilename = trim( FILENAME)
		malias    = ALIASNAME
		if empty( malias)
			malias = mdbfname
		endif		

      if g_filemode
         i = ascan( g_openfile, setlen( mdbfname, 8 ) )
         if i != 0
      		select &malias
*				testmsg( 'Closing ' + malias )
            use            
            adel( g_openfile, i )
            adel( g_closfile, i )
         endif                   
      else
      	select &malias
         testmsg( 'Closing ' + malias )

         #IFDEF USE_CFTS_INDEXES
         fts_close()
         #ENDIF

			use
			commit 
      endif

		select INDEXES
		skip
		do while .not. eof() .and. DBFNAME = mdbfname
			skip
		enddo
	enddo			 

	select INDEXES
	use

return( ok )


function FILEMODE       

   parameter filemode

   public g_openfile[60], g_closfile[60], g_filemode, g_openpath, g_noclose
   g_filemode = filemode
   g_noclose  = .f.  && the trigger for opendbf not to close the index control file
   g_openpath = ""  

return( 0 )


function DBFSET

   afill( g_closfile, .t. )                              
   g_noclose  = .t.  && trigger opendbf not to close the index control file
   g_openpath = ""  

return( 0 )


function CLOSEREST

   priv i, als

   do while .t.
      i = ascan( g_closfile, .t. )
      if i = 0 
         exit
      endif  
      if type( "g_openfile[i]" ) != "C"
         exit
      endif     

      als = g_openfile[i]
      select &als
      use
      adel( g_openfile, i )
      adel( g_closfile, i )
      
   enddo

   if g_noclose
      if !empty( g_openpath )
         select INDEXES
         use         
      endif
   endif
   g_noclose = .f.
   g_openpath = ""

   return( 0 )


function KEEPOPEN

   parameter file

   priv i

   i = ascan( g_openfile, setlen( file, 8 ) )
   if i > 0
      g_closfile[i] = .f.
   endif
return( i > 0 )


*****************
function OPEN_CNT

   *---counts the no of files open   Note: return value is a string!

   private als, i, ni, cnt, curals, dbfs[100], di

   if type( "g_mostopen" ) = "U"
      public g_mostopen
      g_mostopen = 0
   endif

   curals = alias( 0 )
   select 0

   di = 0
	cnt = 0
	*---Load dbs array with Alias names open
   for i = 1 to 255
      if !empty( alias( i ) )
         als = alias( i )    
         if ascan( dbfs, als ) > 0
            return( "ERROR - FILE ALREADY OPEN: " + als )
         endif
         di = di + 1
         dbfs[i] = als
         select &als
         cnt = cnt + 1
         ni = 1
         do while !empty( indexkey( ni ) )
            cnt = cnt + 1
            ni = ni + 1
         enddo
      endif
   next

   if !empty( curals )
      select &curals
   endif

   if cnt > g_mostopen
      g_mostopen = cnt
   endif

return( alltrim( str( cnt ) ) )



*EOF:  OPENPRC.PRG
