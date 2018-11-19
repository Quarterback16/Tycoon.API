*****************************************************************************
*  Program Name.....: TALLY.PRG 
*  Programmer.......: Steve Colonna 
*  System...........:
*  Module...........: NFL
*  Date.............: 28/9/1990 at 20:18
*  Copyright........: (c) 1990 by Knoware 
*  Called From......:
*  Purpose..........:
*
*		To list NFL players by ranking
*
*
*............................................................................
*  Revision.........: 1.0 Last Revised: 28/9/1990 at 20:18
*  Description......: Initial Creation.
*............................................................................
***************************** ALL RIGHTS RESERVED ***************************

#INCLUDE "..\LIB\PRINT.H"
#INCLUDE "NFL.H"

#define  LINE_LENGTH 		176

PROCEDURE  Tally

	private printto, workfile, cLeagueName, tixfile, nComps, mLeagueId
	private mopt[4], mmess[4], poption1, cCat, lAlpha
	private cStat,   cSeason1, cWeek1,   cSeason2, cWeek2, cTeam

   clear_scn()

   head_line( "TALLY SHEET" )

   ScreenLine(  1 )
   ScreenLine( 21 )

   if !INIT_TALL()
      do CLOSE_TALL
      RETU
   endif

   poption1 = 2

   do while poption1 <> 0 
   
		@ 22,00
      clrmenu()

      poption1 = flatmenu( mopt, mmess, poption1, 22 )
      poption1 = if( poption1 = 0, 1, poption1)

      do case
         case mopt[poption1] = 'X' 

            poption1 = 0

         case mopt[poption1] = 'G'

				do GEN_TALL

         case mopt[poption1] = 'V'

			   TestMsg( "Looking for " + printto )
            view_prnt( printto, LINE_LENGTH )

         case mopt[poption1] = 'P'

				print_prnt( printto )

      endcase
   
   enddo 

   do CLOSE_TALL

RETURN


function INIT_TALL 

	private mcontinu

	cSeason1  = G_Season
	cSeason2  = G_Season
	cCat      = " "
	cStat     = " "
	cWeek1    = G_week
	cWeek2    = G_week
	lAlpha    = .f.
   cTeam     = "  "

	plswait()
   std_rpt_opt( @mopt, @mmess, "Tally Sheet" )

	select 0
	OpenDbf( 'PLAYER', g_nfl_path )	
	select 0
	OpenDbf( 'STTYPE', g_nfl_path )	
	select 0
	OpenDbf( 'STAT', g_nfl_path )	
	select 0
	OpenDbf( 'TEAM', g_nfl_path )	
	
   mcontinu = .t.
   mcontinu = iif( mcontinu, opendbf( 'COMP',   G_ty_path   ), .f. )


	if mcontinu
		*---Open temp file
		workfile = g_nfl_path + TEMPORARY_FILE_INDICATOR + "TALL" 
		if .not. file( workfile + ".dbf")
			makefile( workfile + ".dbf", 6, ;
						"PLAYERID   C  8 0", ;
						"NAME       C 22 0", ;
						"PLAYSFOR   C 20 0", ;
						"POS        C  6 0", ;
						"ROLE       C  1 0", ;
						"TALLY      N  6 1"  )
		endif

		select 0
		if !net_use( workfile, .t., 5, "WORKFILE" )
			error_msg( "Work file is in use, try again later")
			retu ( .f. )
      else
			TestMsg( "Workfile is " + workfile )
		endif
		tixfile = workfile + '.ntx'

	endif

	@ 23,00

	if ! mcontinu
		error_msg( "Unable to gain access to all the necessary files" )
	endif

RETURN( mcontinu )


PROCEDURE Close_tall

	CloseDbf( "COMP" )
    CloseDbf( 'TEAM' )
   CloseDbf( 'STAT' )
   CloseDbf( 'STTYPE' )
   CloseDbf( 'PLAYER' )

   select WORKFILE
	use

RETURN


FUNCTION VAL_TALL

	parameters mvar

	private valonly, ok, saveclr, lk

	saveclr = setcolor()
	if lastkey() = UPARROW
		RETU( .t. )
	endif

	ok = .t.
	do case

		case mvar == "cStat"

		   if	cStat = " "
				ok = .t.
		   else

				select STTYPE
				ok = poptable2( @cStat, "STTYPE", "STTYPE->STATCODE",;
			    "STTYPE->STATDESC",;
				    "Stat Code", "   Stat Name", "C", 1, .t., 20, .f. )
			endif

			vread_redraw = .f.

	endcase
	setcolor( saveclr )

RETURN( ok )


PROCEDURE Gen_Tall

   local cLastWeek, nOldStatOrd

	setcolor( c_desc )
	print( 5, 10, "Stat      |" )
	print( 6, 10, "Week Range:" )
	print( 7, 10, "Team      :" )
	print( 8, 10, "Category  :" )
	print( 9, 10, "Alpha Y/N :" )

	setcolor( c_fields )

	vget( 5, 25, "cStat",      "!",    .f. )
	vget( 6, 25, "cSeason1",   "XXXX", .f. )
	vget( 6, 30, "cWeek1",     "XX",   .f. )
	vget( 6, 40, "cSeason2",   "XXXX", .f. )
	vget( 6, 45, "cWeek2",     "XX",   .f. )
	vget( 7, 25, "cTeam",      "!!",   .f. )
	vget( 8, 25, "cCat",       "X",    .f. )
	vget( 9, 25, "lAlpha",     "Y",    .f. )

	vread( 1, .f., 'VAL_TALL' )

	CLEAR_WINDOW( 4, 5, 19, 79 )
	if abort()
		RETU
	endif

	if	cStat = " "
 	   do DoTally with "R", cSeason1, cWeek1, cSeason2, cWeek2, cTeam, cCat, lAlpha
 	   do DoTally with "Y", cSeason1, cWeek1, cSeason2, cWeek2, cTeam, cCat, lAlpha
 	   do DoTally with "S", cSeason1, cWeek1, cSeason2, cWeek2, cTeam, cCat, lAlpha
 	   do DoTally with "P", cSeason1, cWeek1, cSeason2, cWeek2, cTeam, cCat, lAlpha
 	   do DoTally with "G", cSeason1, cWeek1, cSeason2, cWeek2, cTeam, cCat, lAlpha
 	   do DoTally with "M", cSeason1, cWeek1, cSeason2, cWeek2, cTeam, cCat, lAlpha
 	   do DoTally with "Q", cSeason1, cWeek1, cSeason2, cWeek2, cTeam, cCat, lAlpha

   else
 	   do DoTally with cStat, cSeason1, cWeek1, cSeason2, cWeek2, cTeam, cCat, lAlpha
   endif

RETURN

PROCEDURE DoTally

	parameters mStat, cSeason1, cWeek1, cSeason2, cWeek2, cTeam, cCat, lAlpha

	GenMsg()

	printto = g_reppath + TEMPORARY_FILE_INDICATOR + "T" + mStat + ;
			substr(cSeason2, 3, 2) + strzero( val( cWeek2 ), 2 )

   if	cWeek1 = "01" .and. cWeek2 <> "01"
		printto = printto + "y"
   endif
	printto = printto + + PRINTFILE_EXT

	***
	***   Load Data
	***
	select WORKFILE
	zap
	index on PLAYERID to ( tixfile  )

	nComps  = NoOfComps( G_Season )
	private cComp_[ nComps ]
	nComps  = 0
	select COMP
	dbseek( G_Season )
	lastcomp = "XX"
	do while .not. eof() .and. SEASON = G_Season
		if LEAGUEID <> lastcomp
			INC( nComps )
			cComp_[ nComps ] = LEAGUEID
			lastcomp         = LEAGUEID
		endif
		skip
	enddo

	TestMsg( "There are " + str( nComps, 1 ) + " comps" )

	nOldStatOrd := STAT->( SetOrder ( 3 ) )

	cFirstWeek := mStat + cSeason1 + cWeek1
	cLastWeek  := mStat + cSeason2 + cWeek2

	if STAT->( Seeker( cFirstWeek ) )

		do while STAT->STAT + STAT->SEASON + STAT->WEEK <= cLastWeek

			cPlayerId = STAT->PLAYERID

			@ 24, 70 say cPlayerId

			SaveStat( cPlayerId )

			STAT->( dbSkip( 1 ) )
		enddo
	else
		TestMsg( 'First week ' + cFirstWeek + ' not found' )
	endif

	private    pcol1, pcol2, pcol3
	store 0 to pcol1, pcol2, pcol3
    private cPlayerOut
	cPlayerOut := ""
   begin sequence

      set print to ( printto )
	   set print on

	   print_setup( LINE_LENGTH )

		*---Short length	(Landscape)
		*?? chr(27) +"E" + chr(27)+ chr(38)+ "I" +chr(27) + "(0U" + chr(27) + "(s0p12h12v0s0b8T" + chr(27) + "&a12L"
		p_linespp = 9999999

      print_margin( 5, 3, 3 )

      prntcoldec( 1 )

      *---set up the printing columns
		prntcolnxt( @pcol1,  60  )
		prntcolnxt( @pcol2,  6   )
		prntcolnxt( @pcol3,  140 )

      print_header( "HEAD_TALL" )
		select WORKFILE
		if lAlpha
			index on NAME to ( tixfile  )
		else
			index on descend( str( TALLY, 6, 1 ) ) to ( tixfile  )
		endif
		go top
      do while !eof() 

		 print( 24, 70, str( recno(), 6 ) )

         if inkey() = 27
            if !print_cont()
               exit
            endif
         endif
         cPlayerOut = WORKFILE->NAME + " " + WORKFILE->POS + " " +;
			             WORKFILE->ROLE + "  " + WORKFILE->PLAYSFOR

			if mStat = QUARTERBACK_SACKS
				cPlayerOut = cPlayerOut +   str( WORKFILE->TALLY, 6, 1 )
			else
				cPlayerOut = cPlayerOut +   str( WORKFILE->TALLY, 6, 0 )
			endif

			*TestMsg( cPlayerOut + " len=" + str( len(cPlayerOut), 3, 0 ) )

		   printat( pcol1, cPlayerOut )

			PrintDet( WORKFILE->PLAYERID )

			println()

         skip

      enddo          

		println() 
		println()
      println( 0, "** End of Report **" ) 
      print_page()
		
	end

   print_release()
   set print to        

	@ 22,00
	clrmenu()
	GenMsgClr()

RETURN


FUNCTION SaveStat( cPlayerId )

   local cPlayerName, cFor

   cPlayerName := PlayerName( cPlayerId )
   if empty( cTeam ) .or. PLAYER->CURRTEAM = cTeam
	   if WORKFILE->( Seeker( cPlayerId ) )
		   *---Player already spotted
		   if WORKFILE->( rec_lock( 5 ) )
			   *---Update It
			   replace WORKFILE->TALLY with WORKFILE->TALLY + STAT->QTY
		   endif

	   else
		    *---Add it
		    cPlaysFor = ""

           *---PLAYER file should be positioned
           if cTeam = PLAYER->CURRTEAM
           endif

		    for c = 1 to nComps
			    cFor = PlaysFor( cPlayerId, cComp_[ c ], G_Season, .t. ) + "  "
             if empty(cFor)
                cFor = "    "
				 endif

			    ADDTO( cPlaysFor, cFor )
             *TestMsg( G_season + " Comp " + str(c,1) + " is " + cComp_[ c ] + " " + cPlayerId + " plays for " + cFor )
 	       next

		    if WORKFILE->( add_rec( 5 ) )
			   replace WORKFILE->PLAYERID with cPlayerId
			   replace WORKFILE->NAME     with cPlayerName + " " + PLAYER->CURRTEAM
			   replace WORKFILE->PLAYSFOR with cPlaysFor
			   replace WORKFILE->ROLE     with PLAYER->ROLE
			   replace WORKFILE->TALLY    with STAT->QTY
			   replace WORKFILE->POS      with PLAYER->POSDESC
		    endif

      endif
   endif

RETURN .t.


FUNCTION PrintDet( cPlayerId )

	local cDetail := ""
	local cRound  := cSeason1 + cWeek1
	local nStat   := 0

	do while val( cRound ) <= val( cSeason2 + cWeek2 )

		nStat := 0
		if STAT->( seeker( mStat + cRound + cPlayerId ) )
			nStat := STAT->QTY
		endif

		cDetail += iif( cStat = QUARTERBACK_SACKS,;
							 str( nStat, 4, 1 ), str( nStat, 4, 0 ) )
		cRound := IncrRound( cRound )

	enddo

RETURN( printAt( pcol3, cDetail ) )


FUNCTION TallHead

	local cDetail := ""
	local cRound  := cSeason1 + cWeek1

	do while val( cRound ) <= val( cSeason2 + cWeek2 )

		cDetail += "  " + substr( cRound, 5, 2 )

		cRound := IncrRound( cRound )
	enddo

RETURN( printAt( pcol3, cDetail ) )


FUNCTION IncrRound( cRound )

	local cWeek := substr( cRound, 5, 2 )

	cWeek := strzero( ( val( cWeek ) + 1 ), 2 )

	if cWeek > "17"
		cRound := str( ( val( substr( cRound, 1, 4 ) ) + 1 ), 4 ) + "01"
	else
		cRound := substr( cRound, 1, 4 ) + cWeek
	endif

*	TestMsg( "Next round is " + cRound )

RETURN( cRound )


function HEAD_TALL()

   *printat( PRINT_CENTER, g_sysowner )
   printat( pcol1, "Date: " +dtoc( date() ) )
   *printat( PRINT_RIGHT, "Page: " +str( printpageno, 8 ) )
   println()

	STTYPE->( dbseek( mStat ) )
	printat( pcol1, trim( STTYPE->STATDESC ) + ;
	         "   <TALLY SHEET>   From " + cSeason1 + " Week " + cWeek1 +;
				" to " + cSeason2 + " Week " + cWeek2 )
   println()
	println()			 

	printat( pcol1,   "Player"    )
	printat( pcol2,   "Total"    )
	TallHead()
	println()			 
	println()			 

RETURN( "" )



*EOF:  TYRRANK.PRE