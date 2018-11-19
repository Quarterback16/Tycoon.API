*****************************************************************************
*  Program Name.....: TYRMARG.PRG
*  Programmer.......: Steve Colonna 
*  System...........: TFL
*  Module...........: TYCOON
*  Date.............: 28/12/1996 at 20:18
*  Copyright........: (c) 1996 by Knoware
*  Called From......: TFLMENU
*  Purpose..........:
*
*		To Print Margin stats
*
*............................................................................
*  Revision.........: 1.0 Last Revised: 28/12/1996 at 20:18
*  Description......: Initial Creation.
*............................................................................
***************************** ALL RIGHTS RESERVED ***************************

#INCLUDE "..\LIB\PRINT.H"
#INCLUDE "TYCOON.H"

#define  LINE_LENGTH 		176
#define  K_TITLE           "Game Margins"
#define  K_MAX_WEEK        17

static aStats[ 15, 2 ]

PROCEDURE  TYRMARG

	private printto, workfile, cLeagueName, tixfile
	private mopt[4], mmess[4], poption1

   clear_scn()

   head_line( K_TITLE )

   ScreenLine(  1 )
   ScreenLine( 21 )

   if .not. InitMstat()
      CloseMstat()
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

				GenMstat()
            view_prnt( printto, LINE_LENGTH )

         case mopt[poption1] = 'V'

            view_prnt( printto, LINE_LENGTH )

         case mopt[poption1] = 'P'

				print_prnt( printto )

      endcase
   
   enddo 

   CloseMstat()

RETURN


FUNCTION GenMstat

   local cLastWeek, cFirstWeek, lPlayoffs, cLeagueID, cTitle, cLabel

   *---Get Game Range
   cLeagueID  := G_league
   cFirstWeek := G_season + "01"
   cLastWeek  := G_season + G_week
   lPlayoffs  := .t.
   do GameRange with cFirstWeek, cLastWeek, lPlayoffs, cLeagueID

	GenMsg()

   *---Process the Results
   ProcResults( cFirstWeek, cLastWeek, lPlayoffs, cLeagueID )

   cTitle := K_TITLE + " - " + LeagueName( cLeagueID ) + "  " 
   cTitle += FormatWeek( cFirstWeek ) + " to " + FormatWeek( cLastWeek )
   cLabel := "Margin"
   FreqTable( aStats, cTitle, cLabel )

	GenMsgClr()

return nil


/*
        Name : 
        Does : 
  Parameters : 
     Returns : 
       Notes : 

      Author : LL 
Date Created : 27 Dec 96
Time Created :  9:08 pm
	Last change:  LL   27 Dec 96    9:24 pm
*/
FUNCTION FormatWeek( cWeek )
RETURN substr( cWeek, 1, 4 ) + "-" + substr( cWeek, 5, 2 )


/*
        Name : 
        Does : 
  Parameters : 
     Returns : 
       Notes : 

      Author : LL 
Date Created : 27 Dec 96
Time Created :  9:04 pm
	Last change:  LL   27 Dec 96    9:08 pm
*/
FUNCTION LeagueName( cLeagueID )

   local cName

   if LEAGUE->( dbseek( cLeagueID ) )
      cName := alltrim( LEAGUE->LEAGUE )
   else
      cName := "Unknown League >" + cLeagueID
   endif

RETURN cName


/*
        Name : 
        Does : 
  Parameters : 
     Returns : 
       Notes : 

      Author : LL 
Date Created : 27 Dec 96
Time Created :  7:52 pm
	Last change:  LL   27 Dec 96    9:02 pm
*/
FUNCTION ProcResults( cFirstWeek, cLastWeek, lPlayoffs, cLeagueID )

   local cKey1, cKey2, lCountEm, i
   *---Scan through all the results
   cKey1 := KeyFor( cLeagueID, cFirstWeek )
   cKey2 := KeyFor( cLeagueID, cLastWeek )

   *---Zeroise stats
   for i = 1 to 15
      aStats[ i, 2 ] := 0
   next

	select TYSKED
   seek cKey1
	do while .not. eof()
      if TYSKED->SEASON + TYSKED->LEAGUEID + TYSKED->WEEK > cKey2
         exit
      endif
      *---Do a round
      if lPlayoffs
         *---we want to include playoffs
         lCountEm := .t.
      else
         *---we dont want to include playoffs
         if val( TYSKED->WEEK ) <= RegularGames( TYSKED->SEASON, TYSKED->LEAGUEID )
            lCountEm := .t.
         else
            TestMsg( "Not counting Week " + TYSKED->WEEK )
            lCountEm := .f.
         endif
      endif

      if lCountEm
         GetMarg( TYSKED->HS1, TYSKED->AS1 )
         GetMarg( TYSKED->HS2, TYSKED->AS2 )
         GetMarg( TYSKED->HS3, TYSKED->AS3 )
         GetMarg( TYSKED->HS4, TYSKED->AS4 )
         GetMarg( TYSKED->HS5, TYSKED->AS5 )
         GetMarg( TYSKED->HS6, TYSKED->AS6 )
      endif
      TYSKED->( dbskip() )
   enddo


RETURN NIL


/*
        Name : 
        Does : 
  Parameters : 
     Returns : 
       Notes : 

      Author : LL 
Date Created : 27 Dec 96
Time Created :  8:07 pm
*/
static ;
FUNCTION GetMarg( nHScore, nAScore )

   local nMargin

   if nHScore + nAScore > 0
      nMargin := abs( nHScore - nAScore )
      AddMarg( nMargin )
   endif

RETURN NIL


/*
        Name : 
        Does : 
  Parameters : 
     Returns : 
       Notes : 

      Author : LL 
Date Created : 27 Dec 96
Time Created :  8:10 pm
*/
static ;
FUNCTION AddMarg( nMargin )

   local i, cMargin

   *---Determine Label
   do case
      case nMargin > 40
         cMargin := "40+"
      case nMargin > 30
         cMargin := "30+"
      case nMargin > 20
         cMargin := "20+"
      case nMargin > 10
         cMargin := "10+"
      otherwise
         cMargin := str( nMargin, 3 )
   endcase


   for i = 1 to alen( aStats )
      if aStats[ i, 1 ] == cMargin
         aStats[ i, 2 ]++
         exit
      endif
   next

RETURN NIL

/*
        Name : 
        Does : 
  Parameters : 
     Returns : 
       Notes : 

      Author : LL 
Date Created : 27 Dec 96
Time Created :  8:00 pm
*/
FUNCTION KeyFor( cLeagueid, cWeek )

   local cKeyOut

   cKeyOut := substr( cWeek, 1, 4 ) + cLeagueid + substr( cWeek, 5, 2 )
   
RETURN cKeyOut

/*
        Name : GameRange
        Does : Gets a range of games from the user
  Parameters : 
     Returns : 
       Notes : 

      Author : LL 
Date Created : 27 Dec 96
Time Created :  7:10 pm
	Last change:  LL   27 Dec 96    7:49 pm
*/
PROCEDURE GameRange( cFirstWeek, cLastWeek, lPlayoffs, cLeagueID )

	private cSeason1, cWeek1, cSeason2, cWeek2, mLeagueID, ldoPlayoff

   cSeason1   := substr( cFirstWeek, 1, 4 )
   cWeek1     := substr( cFirstWeek, 5, 2 )
   cSeason2   := substr( cLastWeek,  1, 4 )
   cWeek2     := substr( cLastWeek,  5, 2 )
   mLeagueID  := cLeagueID
   ldoPlayoff := lPlayoffs

	setcolor( c_desc )
	print( 5, 10, "League    |" )
	print( 6, 10, "Week Range:" )
	print( 7, 10, "Playoffs? :" )

	setcolor( c_fields )

	vget( 5, 25, "mLeagueID",  "!!",   .f. )
	vget( 6, 25, "cSeason1",   "XXXX", .f. )
	vget( 6, 30, "cWeek1",     "XX",   .f. )
	vget( 6, 40, "cSeason2",   "XXXX", .f. )
	vget( 6, 45, "cWeek2",     "XX",   .f. )
	vget( 7, 25, "ldoPlayoff", "Y",    .f. )

	vread( 1, .f., 'ValMstat' )

	CLEAR_WINDOW( 4, 5, 19, 79 )
	if abort()
		RETU
	endif
   
	cFirstWeek := cSeason1 + cWeek1
	cLastWeek  := cSeason2 + cWeek2
   cLeagueID  := mLeagueID
   lPlayoffs  := ldoPlayoff

RETURN


FUNCTION ValMstat

	parameters mvar

	local valonly, ok, saveclr, lk

	saveclr = setcolor()
	if lastkey() = UPARROW
		RETU( .t. )
	endif

	ok = .t.
	do case
	case mvar = "mLeagueID"

		if .not. LEAGUE->( dbseek( mLeagueID ) )
			ok = .f.
			error_msg( "League " + mLeagueid + " not found" )
		endif

	case mvar = "cWeek"

		if val( &mvar ) > K_MAX_WEEK
			ok = .f.
			error_msg( &mvar + " - Invalid week" )
		endif

	endcase
	setcolor( saveclr )

RETURN( ok )



FUNCTION FreqTable( aStats, cTitle, cLabel  )

   local nTotal := 0
   local nCumFreq := 0

	printto = g_reppath + TEMPORARY_FILE_INDICATOR + cLabel + PRINTFILE_EXT

	private    pcol1, pcol2, pcol3, pcol4, pcol5, i
	store 0 to pcol1, pcol2, pcol3, pcol4, pcol5, i

   for i = 1 to alen( aStats )
      nTotal += aStats[ i, 2 ]
   next

   begin sequence

      set print to ( printto )
		set print on

		print_setup( LINE_LENGTH, .t. )	//	Continuous

		?? chr(27) +"E"
		p_linespp = 70

      print_margin( 5, 3, 3 )

      prntcoldec( 1 )

      *---set up the printing columns
		prntcolnxt( @pcol1,  len( cLabel ) )
		prntcolnxt( @pcol2,  10 )
		prntcolnxt( @pcol3,  10 )
		prntcolnxt( @pcol4,  10 )
		prntcolnxt( @pcol5,  10 )

		printAt( pcol1, cTitle )
		println()
		println()
		printAt( pcol1, cLabel )
		printAt( pcol2, "      Freq" )
		printAt( pcol3, "    Freq %" )
		printAt( pcol4, "  Cum Freq" )
		printAt( pcol5, "Cum Freq %" )
		println()

		for i := 1 to len( aStats )
			PrintAt( pcol1, aStats[ i, 1 ] )
			PrintAt( pcol2, aStats[ i, 2 ] )
			PrintAt( pcol3, str( Percent( aStats[ i, 2 ], nTotal, .f. ), 10, 1 ) )
         nCumFreq += aStats[ i, 2 ]
			PrintAt( pcol4, nCumFreq )
			PrintAt( pcol5, str( Percent( nCumFreq, nTotal, .f. ), 10, 1 ) )

			println()
		next
		println()

		println()
		println()
      println( 0, "** End of Report **" ) 
      print_page()
		
	end

   print_release()
   set print to        

	@ 22,00
	clrmenu()

RETURN


FUNCTION InitMstat

	private mcontinu, i

	plswait()
   std_rpt_opt( @mopt, @mmess, K_TITLE )

   mcontinu = .t.
   mcontinu = iif( mcontinu, opendbf( 'TYSKED', G_ty_path   ), .f. )
   mcontinu = iif( mcontinu, opendbf( 'LEAGUE', G_ty_path   ), .f. )

   aStats[ 1, 1 ] := "  0"
   aStats[ 2, 1 ] := "  1"
   aStats[ 3, 1 ] := "  2"
   aStats[ 4, 1 ] := "  3"
   aStats[ 5, 1 ] := "  4"
   aStats[ 6, 1 ] := "  5"
   aStats[ 7, 1 ] := "  6"
   aStats[ 8, 1 ] := "  7"
   aStats[ 9, 1 ] := "  8"
   aStats[10, 1 ] := "  9"
   aStats[11, 1 ] := " 10"
   aStats[12, 1 ] := "10+"
   aStats[13, 1 ] := "20+"
   aStats[14, 1 ] := "30+"
   aStats[15, 1 ] := "40+"

   for i = 1 to 15
      aStats[ i, 2 ] := 0
   next

*   TestMsg( "Array has " + str( alen( aStats ), 3 ) + " elements" )

	@ 23,00

	if ! mcontinu
		error_msg( "Unable to gain access to all the necessary files" )
	endif

RETURN( mcontinu )


FUNCTION CloseMstat

	close databases

RETURN( .t. )





*EOF:  TYRMARG.PRG