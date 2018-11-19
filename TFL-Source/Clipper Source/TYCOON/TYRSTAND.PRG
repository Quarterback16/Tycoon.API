*****************************************************************************
*  Program Name.....: TYRSTAND.PRE
*  Programmer.......: SC 
*  System...........: 
*  Module...........: 
*  Date.............: 10/10/90 @ 13:19:22
*  Compile/link.....: 
*                     
*  Purpose..........:	Display standings
*
*.............................................................................

#INCLUDE "..\lib\PRINT.H"
#INCLUDE "TYCOON.H"

#define  LINE_LENGTH 		96

PROCEDURE TyRStand

	private printto, STAND, cLeagueName
   private mopt[4], mmess[4], poption1
	private mHt1, mHt2, mHt3, mHt4, mHt5, mHt6
	private mAt1, mAt2, mAt3, mAt4, mAt5, mAt6

   clear_scn()

   head_line( "STANDINGS" )

   ScreenLine(  1 )
   ScreenLine( 21 )

   if !INIT_STAND()    && Initialisation routine
      do CLOSE_STAND
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

				do GEN_STAND
            view_prnt( printto, LINE_LENGTH )

         case mopt[poption1] = 'V'

            view_prnt( printto, LINE_LENGTH )

         case mopt[poption1] = 'P'

				print_prnt( printto )

      endcase
   
   enddo 

   do CLOSE_STAND

return


FUNCTION Init_stand 

	private mcontinu

	plswait()
   std_rpt_opt( @mopt, @mmess, "Tycoon Standings" )

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
		OpenStnd()
	else

		error_msg( "Unable to gain access to all the necessary files" )
	endif

RETURN mcontinu


FUNCTION OpenStnd

	STAND = g_ty_path + TEMPORARY_FILE_INDICATOR + "STAND"
	if .not. file( STAND + ".dbf")
		makefile( STAND + ".dbf", 9, ;
		"DIVISION   C  1 0", ;
		"FRANCHISE  C 20 0", ;
		"WINS       N  2 0", ;
		"LOSSES     N  2 0", ;
		"TIES       N  2 0", ;
		"CLIP       N  5 3", ;
		"PTSFOR     N  3 0", ;
		"AGAINST    N  3 0", ;
		"POINTS     N  4 0"  )
	endif

	select 0
	if !net_use( STAND, .t., 5, "STAND" )
		error_msg( "Work file is in use, try again later")
		retu ( .f. )
	endif
	tixfile = STAND + '.ntx'
	if .not. file( tixfile)
		index on DIVISION+descend(str(CLIP,5,3)) to ( tixfile  )

	endif
	set index to ( tixfile )

	select STAND
	zap

RETURN .t.


PROCEDURE Close_stand

	close databases

RETURN


FUNCTION Val_Stand

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


procedure GEN_STAND

   private i, repoption, mSeason, mLeagueid, mWeek

	setcolor( c_desc )
	print( 6, 15, "Season :" )
	print( 7, 15, "League :" )
	print( 8, 15, "Week   :" )

	setcolor( c_fields )
	mSeason   = G_season
	mLeagueid = G_league
	mWeek     = G_week

	vget( 6, 25, "mSeason" ,   "XXXX", .f. )
	vget( 7, 25, "mLeagueid" , "!!",   .f. )
	vget( 8, 25, "mWeek",      "XX",   .f. )

	vread( 1, .f., 'VAL_STAND' )

	CLEAR_WINDOW( 6, 15, 8, 79 )
	if abort()
		RETU
	else
		G_league = mLeagueId
		G_Week   = mWeek
*		printto  = g_reppath + TEMPORARY_FILE_INDICATOR + mLeagueId +;
*						mWeek + "STD" + PRINTFILE_EXT
		printto = StandPrn()
	endif

	GenMsg()

	PrnStand( mSeason, mLeagueid, mweek, printto )

RETURN

FUNCTION StandPrn
RETURN g_reppath + TEMPORARY_FILE_INDICATOR + mLeagueId +;
		 	"STD" + PRINTFILE_EXT

FUNCTION PrnStand( mSeason, mLeagueid, mweek, printto )
	***
	***   Load Data
	***
	cRound = mSeason + mLeagueid + mweek

	nTeams = TeamsIn( mSeason, mLeagueId )
	nGames = int( ( nTeams / 2 ) +.5 )

	private franchise_[ nTeams ], division_[ nTeams ], wins_[ nTeams ]
	private losses_[ nTeams ], ptsfor_[ nTeams ], against_[ nTeams ]
	private totalpts_[ nTeams ], owner_[ nTeams ], ties_[ nTeams ]

	fran_cnt = 0

	select STAND
   zap

	select COMP
	COMP->( dbseek( mSeason + mLeagueid ) )
	do while .not. eof() .and. SEASON = mseason .and. LEAGUEID = mleagueid
		INC( fran_cnt )
		franchise_[ fran_cnt ] = FRANCHISE
		division_[  fran_cnt ] = DIVISION
		owner_[     fran_cnt ] = OWNERID
		wins_[      fran_cnt ] = 0
		losses_[    fran_cnt ] = 0
		ties_[      fran_cnt ] = 0
		ptsfor_[    fran_cnt ] = 0
		against_[   fran_cnt ] = 0
		totalpts_[  fran_cnt ] = 0
		skip
	enddo

	select TYSKED
	for w = 1 to val( mWeek )
		TYSKED->( dbseek( mSeason + mLeagueid + strzero( w, 2 ) ) )

		if TYSKED->HS1 + TYSKED->AS1 + TYSKED->HS2 + TYSKED->AS2 > 0 
			do RECORDWINS
		endif

	next

	for t = 1 to fran_cnt

		select STAND
		do ADDRECIT with "Work File"
		replace DIVISION  with division_[  t ]
		replace FRANCHISE with franchise_[ t ]
		replace WINS      with wins_[      t ]
		replace LOSSES    with losses_[    t ]
		replace TIES      with ties_[      t ]
		replace CLIP      with Clip( wins_[ t ], losses_[ t ], ties_[ t ] )
		replace PTSFOR    with ptsfor_[       t ]
		replace AGAINST   with against_[   t ]
		replace POINTS    with totalpts_[  t ]

	next

	private    pcol1, pcol2, pcol3, pcol4, pcol5, pcol6, pcol7, pcol8
	store 0 to pcol1, pcol2, pcol3, pcol4, pcol5, pcol6, pcol7, pcol8, pcol9

   begin sequence

		*---Using pre printed Stationery

      set print to ( printto )
		set print on

		print_setup( LINE_LENGTH )

		*---Short length	(Landscape)
*		?? p_a3
		?? chr(27) +"E" + chr(27)+ chr(38) + "I" +chr(27) + "(0U" + chr(27) + "(s0p12h12v0s0b8T" + chr(27) + "&a12L"
		p_linespp = 66

      print_margin( 0, 0, 0 )

      prntcoldec( 1 )

      *---set up the printing columns
		prntcolnxt( @pcol1,  25 )
		prntcolnxt( @pcol2,   6 )
		prntcolnxt( @pcol3,   6 )
		prntcolnxt( @pcol4,   6 )
		prntcolnxt( @pcol5,  10 )
		prntcolnxt( @pcol6,   8 )
		prntcolnxt( @pcol7,  10 )
		prntcolnxt( @pcol8,  10 )
		prntcolnxt( @pcol9,  8  )

		*---Print Scores
		select TYSKED
		for w = 1 to val( mWeek )
			TYSKED->( dbseek( mSeason + mLeagueid + strzero( w, 2 ) ) )

			if TYSKED->HS1 + TYSKED->AS1 + TYSKED->HS2 + TYSKED->AS2 > 0 
				printat( pcol1,  "Week " + strzero( w, 2 ) )
				println()

				do PRINTSCORES
			endif
		next

		println()
		printat( pcol1, mSeason + " - " + cLeagueName + " Standings as of Week " + mweek )
		println()
		println()

		select STAND
		go top
		lastdiv = "X"
		do while .not. eof()
			if DIVISION <> lastdiv
				println()
				printat( pcol1, DivName( DIVISION )  )
				printat( pcol2, " W"     )
				printat( pcol3, " L"     )
				printat( pcol4, " T"     )
				printat( pcol5, "Clip"   )
				printat( pcol6, "For"    )
				printat( pcol7, "Agin"   )
				printat( pcol8, "F Pts"  )
				printat( pcol9, "  Avg"  )
				println()
				lastdiv = DIVISION
			endif

			printat( pcol1, FRANCHISE )
			printat( pcol2, WINS      )
			printat( pcol3, LOSSES    )
			printat( pcol4, TIES      )
			printat( pcol5, CLIP      )
			printat( pcol6, PTSFOR    )
			printat( pcol7, AGAINST   )
			printat( pcol8, POINTS    )
			printat( pcol9, str( POINTS / (WINS+LOSSES+TIES), 5, 1 ) )
			println()

			skip
		enddo

		print_page()
		
	end

   print_release()
   set print to        

	@ 22,00
	clrmenu()
	GenMsgClr()

RETURN nil


PROCEDURE Recordwins

	LOCAL nHS, nAS

	for g = 1 to nGames
		*---resolve game
		cG = str(g,1)
		nHT = ascan( owner_, HT&cG. )
		nAT = ascan( owner_, AT&cG. )
		if .not. empty( nHT ) .and. .not. empty( nAT )
			ADDTO( ptsfor_[  nAT ], AS&cG. )
			ADDTO( ptsfor_[  nHT ], HS&cG. )
			ADDTO( against_[ nAT ], HS&cG. )
			ADDTO( against_[ nHT ], AS&cG. )

			nHS = HS&cG
			nAs = AS&cG
			if nHs > nAS
				*---Home Win

				INC( wins_[   nHT ] )
				INC( losses_[ nAT ] )

			elseif nHS = nAS
				*---Tie
				INC( ties_[ nAT ] )
				INC( ties_[ nHT ]	)
			else
				*---Away win
				INC( wins_[   nAT ] )
				INC( losses_[ nHT ] )
			endif
		endif

		if .not. empty( nAT )
			ADDTO( totalpts_[ nAT ], TAS&cG. )
		endif

		if .not. empty( nHT )
			ADDTO( totalpts_[ nHT ], THS&cG. )
		endif


	next

RETURN 


procedure PRINTSCORES

	for g = 1 to nGames
		*---resolve game
		cG = str(g,1)
		nHT = ascan( owner_, HT&cG. )
		nAT = ascan( owner_, AT&cG. )

		if .not. empty(nHt) .and. .not. empty( nAT )
			printat( pcol1,    franchise_[ nAT ] )
			printat( pcol1+20, AS&cG.            )
			printat( pcol1+25, " @ " )
			printat( pcol1+28, franchise_[ nHT ] )
			printat( pcol1+48, HS&cG.            )
			println()
		endif

	next

return 


FUNCTION Clip

	parameters nWins,  nLosses, nTies

	LOCAL nClip

	nClip = ( ( nWins * 2 ) + nTies ) / ( ( nWins + nLosses + nTies ) * 2 )

*	TestMsg( "Clip is " + str( nClip, 5, 3 ) )

RETURN( nClip )


FUNCTION DivName( cDiv )

	local cName := ""

	do case
	case cDiv = "C"
		cName = "CENTRAL DIVISION"
	case cDiv = "N"
		cName = "NORTHERN DIVISION"
	case cDiv = "S"
		cName = "SOUTHERN DIVISION"
	endcase

RETURN cName



*EOF:  TYRSTAND.PRE

