*****************************************************************************
*  Program Name.....: NFLRMOVE.PRG 
*  Programmer.......: Steve Colonna 
*  System...........: TFL
*  Module...........: NFL
*  Date.............: 17/7/1993 at 20:18
*  Copyright........: (c) 1990 by Knoware 
*  Called From......: NFLMENU
*  Purpose..........:
*
*		To Print NFL Player Movements by Team
*
*	Team:  XX XXXXXXXXXXXXXX   Player movements from  DD/MM/YY  to DD/MM/YY
* 
*		NAME			DATE			TO			FROM			COMMENT
*
*
*			Structure for database : serve.dbf
*			Number of data records : 2008
*			Date of last update    : 07/17/93
*			Field  Field Name  Type       Width    Dec
*    			1  PLAYERID    Character      8
*    			2  TEAMID      Character      2
*    			3  FROM        Date           8
*    			4  TO          Date           8
*    			5  HOW         Character     15
*			** Total **                   00042
*
*		Indexes
*					SERVE	1	PLAYERID + dtos( FROM )
*							2	TEAM + PLAYERID
*
*
*............................................................................
*  Revision.........: 1.0 Last Revised: 28/9/1990 at 20:18
*  Description......: Initial Creation.
*............................................................................
***************************** ALL RIGHTS RESERVED ***************************

#INCLUDE "..\lib\PRINT.H"
#INCLUDE "NFL.H"

#define  LINE_LENGTH 		176


PROCEDURE  NflRMOVE

	private printto, workfile, cLeagueName, tixfile, nComps, mLeagueId
	private mopt[4], mmess[4], poption1, cCat, lAlpha
	private cTeam,   dFrom, dTo, cLeague, cSeason

   clear_scn()

   head_line( "PLAYER MOVEMENTS" )

   ScreenLine(  1 )
   ScreenLine( 21 )

   if .not. InitPmove()
      ClosePmove()
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

				GenPmove()
            view_prnt( printto, LINE_LENGTH )

         case mopt[poption1] = 'V'

            view_prnt( printto, LINE_LENGTH )

         case mopt[poption1] = 'P'

				print_prnt( printto )

      endcase
   
   enddo 

   ClosePmove()

RETURN


FUNCTION InitPmove 

	private mcontinu

	cTeam    = "  "
	dFrom		= ctod( "01/01/2000" )
	dTo		= date()
	cLeague  = "OL"
	cSeason  = G_season
	plswait()
   std_rpt_opt( @mopt, @mmess, "Player Movements" )

	*---Select Files to use
	select 0
	mcontinu = OpenDbf( 'COMP', g_ty_path )

   set century on

	@ 23,00

	if mcontinu
		*---Open temp file
		workfile = g_nfl_path + TEMPORARY_FILE_INDICATOR + "MOVE" 
		if .not. file( workfile + ".dbf")
			makefile( workfile + ".dbf", 9, ;
						"PLAYERID   C  8 0", ;
						"NAME       C 19 0", ;
						"ROLE       C  1 0", ;
						"TOTEAM     C  2 0", ;
						"FROMTEAM   C  2 0", ;
						"POS        C  2 0", ;
						"MOVED      D  8 0", ;
						"MOVETYPE   C  1 0", ;
						"COMMENT    C 30 0"  )
		endif

		select 0
		if !net_use( workfile, .t., 5, "WORKFILE" )
			error_msg( "Work file is in use, try again later")
			retu ( .f. )
		endif
		tixfile = workfile + '.ntx'

	endif

	if ! mcontinu
		error_msg( "Unable to gain access to all the necessary files" )
	endif

RETURN( mcontinu )


FUNCTION ClosePmove

*	close databases
	select WORKFILE
	use		  

	select COMP
	use

   set century off

RETURN .t.


FUNCTION ValPmove

	parameters mvar

	local valonly, ok, saveclr, lk

	saveclr = setcolor()
	if lastkey() = UPARROW
		RETU( .t. )
	endif

	ok = .t.
	do case
	case mvar = "dFrom"
*		dTo = dFrom + 30
	endcase
	setcolor( saveclr )

RETURN( ok )


FUNCTION GenPmove

   local cLastWeek, nOldStatOrd

	setcolor( c_desc )
	print( 5, 10, "Team      |" )
	print( 6, 10, "Date Range:" )
	print( 7, 10, "League    :" )
	print( 8, 10, "Season    :" )

	setcolor( c_fields )

	vget( 5, 25, "cTeam",   "!!", .f. )
	vget( 6, 25, "dFrom",   "@D", .f. )
	vget( 6, 40, "dTo",     "@D", .f. )
	vget( 7, 25, "cLeague", "!!", .f. )
	vget( 8, 25, "cSeason", "!!!!", .f. )

	vread( 1, .f., 'ValPmove' )

	CLEAR_WINDOW( 4, 5, 19, 79 )
	if abort()
		RETU
	else
		printto = g_reppath + TEMPORARY_FILE_INDICATOR + "MOVE" + cTeam + ;
									 PRINTFILE_EXT

	endif

	GenMsg()
	***
	***   Load Data
	***
	select WORKFILE
	zap
	index on TOTEAM+MOVETYPE+NAME to ( tixfile  )

	select SERVE
	go top
	do while .not. SERVE->( eof() )
		*---For each service record
		if SERVE->FROM >= dFrom .and. SERVE->FROM <= dTo
			WriteStart( SERVE->PLAYERID, SERVE->FROM, SERVE->TEAMID, SERVE->HOW )
		endif
		SERVE->( dbskip() )
	enddo

	*---retirements and cuts
	select SERVE
	go top
	do while .not. SERVE->( eof() )
		*---For each service record

		if SERVE->TO >= dFrom .and. SERVE->TO <= dTo
			if LastTeam( SERVE->PLAYERID, SERVE->FROM )
				*---Was last team
				WriteMove( "L", SERVE->PLAYERID, SERVE->TEAMID, SERVE->TEAMID, ;
								 SERVE->TO, StopReason( SERVE->HOW ) )
			endif
		endif
		SERVE->( dbskip() )
	enddo

	select WORKFILE
	go top

	private    pcol1, pcol2, pcol3, pcol4, pcol5, pcol6,i
	store 0 to pcol1, pcol2, pcol3, pcol4, pcol5, pcol6,i

   begin sequence

      set print to ( printto )
		set print on

		print_setup( LINE_LENGTH, .t. )

		?? chr(27) +"E"
		p_linespp = 66

      print_margin( 5, 3, 3 )

      prntcoldec( 1 )

      *---set up the printing columns
		prntcolnxt( @pcol1,  20  )
		prntcolnxt( @pcol2,  6   )
		prntcolnxt( @pcol3,  10  )
		prntcolnxt( @pcol4,  10  )
		prntcolnxt( @pcol5,  10  )
		prntcolnxt( @pcol6,  40  )

		println()
		printAt( pcol1, "PLAYER MOVEMENTS REPORT" )
		printAt( pcol3, dtoc( dFrom ) )
		printAt( pcol4, dtoc( dTo ) )
		println()
		println()

		cLastTeam = "XX"

		do while .not. WORKFILE->( eof() )
			if WORKFILE->TOTEAM = cTeam .or. empty( cTeam )
				if WORKFILE->TOTEAM <> cLastTeam
					println()
					printAt( pcol1, TeamStr( WORKFILE->TOTEAM ) )
					println()
					cLastTeam = WORKFILE->TOTEAM
					cLastType = "X"
				endif

				if WORKFILE->MOVETYPE <> cLastType
					println()
					printAt( pcol1, iif( WORKFILE->MOVETYPE = "G", "GAINS", "LOSSES" ) )
					println()
					cLastType = WORKFILE->MOVETYPE 
				endif

				PrintAt( pcol1, WORKFILE->NAME )
				PrintAt( pcol2, WORKFILE->POS + ' ' + WORKFILE->ROLE )
				PrintAt( pcol3, dtoc( WORKFILE->MOVED ) )
				PrintAt( pcol4, FromStr() )
				PrintAt( pcol6, WORKFILE->COMMENT  )

				println()

			endif
			WORKFILE->( dbskip() )
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


FUNCTION LastTeam( cPlayerid, dFrom )

	private nOldRec, lLast
	lLast = .t.

	select SERVE
	nOldRec = recno()
	SERVE->( dbskip() )
	if SERVE->PLAYERID = cPlayerID
		lLast = .f.
	endif
	
	go nOldRec

RETURN lLast


FUNCTION StopReason( cHow )

	*---Determines the reason for stoping from the HOW comment
	*---Looks for a "."
	private cReason

	if at( ".", cHow ) = 0
		cReason = "Cut"
	else
		cHow = substr( cHow, at( ".", cHow ), len( cHow ) - at( ".", cHow ) )
		do case
		case cHow = ".RET"
			cReason = "Retired"
		case cHow = ".DIED"
			cReason = "Died"
		case cHow = ".INJ"
			cReason = "Severe Injury"
		otherwise
			cReason = cHow
		endcase
	endif

RETURN cReason


FUNCTION FromStr

	private cFromStr

	if WORKFILE->FROMTEAM = WORKFILE->TOTEAM
		*---Cut or retired
		cFromStr = space( 8 )
	else
		if WORKFILE->MOVETYPE = "G"
			cFromStr = "from the " + TeamStr( WORKFILE->FromTEAM, .t. )
		else
			cFromStr = "to the " + TeamStr( WORKFILE->FROMTEAM, .t. )
		endif
      if empty( substr( cFromStr, 10, 31 ) )
         cFromStr = ''
      endif

	endif

RETURN cFromStr


FUNCTION WriteStart

	parameters cPlayerID, dStart, cTeamID, cComment

	cToTeam 		= cTeamID
	dMove			= dStart
	cFromTeam	= PriorTeam( cPlayerID, dMove )

	WriteMove( "G", cPlayerID, cToTeam, cFromTeam, dMove, cComment )
	WriteMove( "L", cPlayerID, cFromTeam, cToTeam, dMove, cComment )

RETURN nil



FUNCTION PriorTeam

	parameters cPlayerID, dMove

	private nOldRec, cLastTeam
	cLastTeam = space(2)

	select SERVE	
	nOldRec = recno()

	set order to 1
	dbseek( cPlayerID )
	do while .not. SERVE->( eof() ) .and. SERVE->PLAYERID = cPlayerID
		if SERVE->FROM = dMove
			exit
		else
			cLastTeam = SERVE->TEAMID
		endif
		SERVE->( dbskip() )
	enddo

	select SERVE
	goto nOldRec

RETURN cLastTeam



FUNCTION WriteMove( cMoveType, cPlayerID, cToTeam, cFromTeam, dMove, cComment )

	local lWrite

	if empty( cLeague )
		lWrite = .t.
	else
		if PlaysFor( cPlayerid, cLeague, cSeason ) <> "  "
			lWrite = .t.
		else
			lWrite = .f.
		endif
	endif

	if lWrite

		select WORKFILE
		append blank
		replace WORKFILE->PLAYERID		with cPlayerId
		replace WORKFILE->TOTEAM		with cToTeam
		replace WORKFILE->FROMTEAM		with cFromTeam
		replace WORKFILE->MOVETYPE	  	with cMoveType
		replace WORKFILE->MOVED			with dMove
		replace WORKFILE->NAME			with PlayerName( cPlayerId )
		replace WORKFILE->POS			with PLAYER->POSDESC
		replace WORKFILE->ROLE		   with PLAYER->ROLE
		replace WORKFILE->COMMENT		with cComment
	endif

RETURN nil


*EOF:  NFLRMOVE.PRE