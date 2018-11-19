*****************************************************************************
*  Program Name.....: TYRWEEK.PRG
*  Programmer.......: SC 
*  System...........: 
*  Module...........: 
*  Date.............: 10/10/90 @ 13:19:22
*  Compile/link.....: 
*                     
*  Purpose..........:	Display Weekly winners
*
*	Mod:		Pick up ties
*
*.............................................................................

#INCLUDE "..\lib\PRINT.H"
#INCLUDE "TYCOON.H"

#define  LINE_LENGTH 		96

PROCEDURE TyRwEEK

	private printto, workfile, cLeagueName
   private mopt[4], mmess[4], poption1
	private mHt1, mHt2, mHt3, mHt4, mHt5, mHt6
	private mAt1, mAt2, mAt3, mAt4, mAt5, mAt6

   clear_scn()

   head_line( "WEEKLY TOP SCORE" )

   ScreenLine(  1 )
   ScreenLine( 21 )

   if !INIT_WEEK()    && Initialisation routine
      do CLOSE_WEEK
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

				do GEN_WEEK
            view_prnt( printto, LINE_LENGTH )

         case mopt[poption1] = 'V'

            view_prnt( printto, LINE_LENGTH )

         case mopt[poption1] = 'P'

				print_prnt( printto )

      endcase
   
   enddo 

   do CLOSE_WEEK

return


FUNCTION Init_WEEK 

	private mcontinu

	plswait()
   std_rpt_opt( @mopt, @mmess, "Tycoon Weekly Winners" )

   mcontinu = .t.
   mcontinu = iif( mcontinu, opendbf( 'TYSKED', G_ty_path   ), .f. )
   mcontinu = iif( mcontinu, opendbf( 'LINEUP', G_ty_path   ), .f. )
   mcontinu = iif( mcontinu, opendbf( 'LEAGUE', G_ty_path   ), .f. )
   mcontinu = iif( mcontinu, opendbf( 'COMP',   G_ty_path   ), .f. )
   mcontinu = iif( mcontinu, opendbf( 'SCORE',  G_nfl_path  ), .f. )
   mcontinu = iif( mcontinu, opendbf( 'PLAYER', G_nfl_path  ), .f. )
*	mcontinu = iif( mcontinu, opendbf( 'STAT',   G_nfl_path  ), .f. )

	@ 23,00

	if mcontinu
		*---Open temp file
*		workfile = g_ty_path + TEMPORARY_FILE_INDICATOR + "WEEK" 
*		if .not. file( workfile + ".dbf")
*			makefile( workfile + ".dbf", 9, ;
*						"DIVISION   C  1 0", ;
*						"FRANCHISE  C 20 0", ;
*						"WINS       N  2 0", ;
*						"LOSSES     N  2 0", ;
*						"TIES       N  2 0", ;
*						"CLIP       N  5 3", ;
*						"PTSFOR     N  3 0", ;
*						"AGAINST    N  3 0", ;
*						"POINTS     N  4 0"  )
*		endif
*
*		select 0
*		if !net_use( workfile, .t., 5, "WORKFILE" )
*			error_msg( "Work file is in use, try again later")
*			retu ( .f. )
*		endif
*		tixfile = workfile + '.ntx'
*		if .not. file( tixfile)
*			index on DIVISION+descend(str(CLIP,5,3)) to ( tixfile  )
*
*		endif
*		set index to ( tixfile )
	else

		error_msg( "Unable to gain access to all the necessary files" )
	endif

RETURN( mcontinu )


PROCEDURE Close_WEEK

	close databases

RETURN


FUNCTION Val_WEEK

	parameters mvar

	local valonly, ok, saveclr, lk

	saveclr = setcolor()
	if lastkey() = UPARROW
		retu( .t. )
	endif

	ok = .t.
	do case

		case mvar == "mLeagueid"

			select LEAGUE
			LEAGUE->( dbseek( mLeagueid ) )
			if found()
				cLeagueName = LEAGUE
			else
				ok = .f.
				error_msg( "League " + mLeagueid + " not found" )
			endif

	endcase
	setcolor( saveclr )

RETURN( ok )


procedure GEN_WEEK

	private i, repoption, mSeason, mLeagueid, mWeek, cTeamTied

	cTeamTied 	:= ""

	setcolor( c_desc )
	print( 6, 15, "Season :" )
	print( 7, 15, "League :" )
*	print( 8, 15, "Week   :" )

	setcolor( c_fields )
	mSeason   = G_season
	mLeagueid = G_league
	mWeek     = G_week

	vget( 6, 25, "mSeason" ,   "XXXX", .f. )
	vget( 7, 25, "mLeagueid" , "!!",   .f. )
*	vget( 8, 25, "mWeek",      "XX",   .f. )

	vread( 1, .f., 'VAL_WEEK' )

	CLEAR_WINDOW( 6, 15, 8, 79 )
	if abort()
		RETU
	else
		G_league = mLeagueId
		G_Week   = mWeek
		printto  = WeekPrn()

	endif

	GenMsg()

	PrnWeek( mSeason, mLeagueID, printto )

RETURN


FUNCTION WeekPrn
	local cPrintOut
	cPrintOut := g_reppath + TEMPORARY_FILE_INDICATOR + mLeagueId +;
					 	"WIN" + PRINTFILE_EXT

*	Testmsg( "Printing to " + cPrintOut )
RETURN cPrintOut


FUNCTION PrnWeek( mSeason, mLeagueID, printto  )

	private    pcol1, pcol2, pcol3, pcol4, pcol5, pcol6, pcol7, pcol8, nTies
	store 0 to pcol1, pcol2, pcol3, pcol4, pcol5, pcol6, pcol7, pcol8
	nTies 		:= 0

   begin sequence

		*---Using pre printed Stationery

      set print to ( printto )
		set print on

		print_setup( LINE_LENGTH )

		*---Short length	(Landscape)
*		?? p_a3
*		?? chr(27) +"E" + chr(27)+ chr(38) + "I" +chr(27) + "(0U" + chr(27) + "(s0p12h12v0s0b8T" + chr(27) + "&a12L"
		p_linespp = 66

      print_margin( 0, 0, 0 )

      prntcoldec( 1 )

      *---set up the printing columns
		prntcolnxt( @pcol1,  10 )
		prntcolnxt( @pcol2,  25 )
		prntcolnxt( @pcol3,  10 )
		prntcolnxt( @pcol4,  20 )
		prntcolnxt( @pcol5,  20 )
		prntcolnxt( @pcol6,  20 )
		prntcolnxt( @pcol7,  20 )
		prntcolnxt( @pcol8,  20 )

		printat( pcol1,  mSeason )
		printat( pcol2,  "WEEKLY TOP SCORES" )
		println()
		println()

		*---Print Scores
		select TYSKED
		for w = 1 to 14
			*---For each week
			TYSKED->( dbseek( mSeason + mLeagueid + strzero( w, 2 ) ) )

			nTopScore = 0
			*---Who got the high score?
			for s = 1 to 6
				cS = str(s,1)
				if THS&cS > nTopScore
					nTopScore = THS&cS
					cTeam     = HT&cS
				endif
			next
			for s = 1 to 6
				cS = str(s,1)
				if TAS&cS > nTopScore
					nTopScore = TAS&cS
					cTeam     = AT&cS
				endif
			next

			if nTopScore = 0
				exit
			endif

			printat( pcol1,  "Week " + strzero( w, 2 ) )
			printat( pcol2,  Franchise( mSeason, mLeagueId, cTeam ) )
			printat( pcol3,   str( nTopScore, 3 )  )

			for s = 1 to 6
				cS = str(s,1)
				if THS&cS = nTopScore
					cTeamTied = HT&cS
					if cTeamTied <> cTeam
						nTies++
						printat( pcol3-5+(15*nTies),  "+ " + Franchise( mSeason, mLeagueId, cTeamTied ) )
					endif

				endif
			next
			for s = 1 to 6
				cS = str(s,1)
				if TAS&cS = nTopScore
					cTeamTied  := AT&cS
					if cTeamTied <> cTeam
						nTies++
						printat( pcol3-5+(15*nTies), "+ " + Franchise( mSeason, mLeagueId, cTeamTied ) )
					endif
				endif
			next

			println()

		next

		println()

		print_page()
		
	end

   print_release()
   set print to        

	@ 22,00
	clrmenu()
	GenMsgClr()

RETURN



		

*EOF:  TYRWEEK.PRE

