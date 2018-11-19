*****************************************************************************
*  Program Name.....: TYRROST
*  Programmer.......: Steve Colonna 
*  System...........:
*  Module...........:
*  Date.............: 17/9/1990 at 18:19
*  Copyright........: (c) 1990 by Knoware 
*  Called From......:
*  Purpose..........:
*
*	  Prints out the rosters
*
*............................................................................
*  Revision.........: 1.0 Last Revised: 24/9/1990 at 21:21
*  Description......: Initial Creation.
*............................................................................
***************************** ALL RIGHTS RESERVED ***************************

#INCLUDE "..\lib\PRINT.H"
#INCLUDE "TYCOON.H"

#define  LINE_LENGTH 		182
#define  ROSTER_MAXIMUM		30

PROCEDURE TyRrost

	private printto, workfile, cLeagueName
   private mopt[4], mmess[4], poption1

   clear_scn()

   head_line( "ROSTERS" )

   ScreenLine(  1 )
   ScreenLine( 21 )

   if !INIT_ROST()    && Initialisation routine
      do CLOSE_ROST
      RETU
   endif

   poption1 = 2

   do while poption1 <> 0 
   
      clrmenu()

      poption1 = flatmenu( mopt, mmess, poption1, 22 )
      poption1 = if( poption1 = 0, 1, poption1)

      do case
         case mopt[poption1] = 'X' 

            poption1 = 0

         case mopt[poption1] = 'G'

				do GEN_ROST
            view_prnt( printto, LINE_LENGTH )

         case mopt[poption1] = 'V'

            view_prnt( printto, LINE_LENGTH )

         case mopt[poption1] = 'P'

				print_prnt( printto )

      endcase
   
   enddo 

   do CLOSE_ROST

return


function INIT_ROST 

	private mcontinu

	plswait()
   std_rpt_opt( @mopt, @mmess, "Tycoon Matchups" )

   mcontinu = .t.
*  mcontinu = iif( mcontinu, opendbf( 'TYSKED', G_ty_path   ), .f. )
   mcontinu = iif( mcontinu, opendbf( 'LINEUP', G_ty_path   ), .f. )
   mcontinu = iif( mcontinu, opendbf( 'LEAGUE', G_ty_path   ), .f. )
   mcontinu = iif( mcontinu, opendbf( 'COMP',   G_ty_path   ), .f. )
   mcontinu = iif( mcontinu, opendbf( 'OWNERS', G_ty_path   ), .f. )
*  mcontinu = iif( mcontinu, opendbf( 'SCORE',  G_nfl_path  ), .f. )
   mcontinu = iif( mcontinu, opendbf( 'PLAYER', G_nfl_path  ), .f. )
   mcontinu = iif( mcontinu, opendbf( 'SERVE',  G_nfl_path  ), .f. )
   mcontinu = iif( mcontinu, opendbf( 'TEAM',   G_nfl_path  ), .f. )

	@ 23,00

	if ! mcontinu
		error_msg( "Unable to gain access to all the necessary files" )
	endif

RETURN mcontinu


procedure CLOSE_ROST

	close databases

RETURN


function VAL_ROST

	parameters mvar

	private valonly, ok, saveclr, lk

	saveclr = setcolor()
	if lastkey() = K_UP
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

RETURN ok 


procedure GEN_ROST

   private i, repoption, mSeason, mLeagueid, mWeek, lAnon

	setcolor( c_desc )
	print( 6, 15, "Season :" )
	print( 7, 15, "League :" )
	print( 8, 15, "Week   :" )
	print( 9, 15, "Anon   :" )

	setcolor( c_fields )
	mSeason   = G_season
	mLeagueid = G_league
	mWeek     = G_week
   lAnon     = .t.

	vget( 6, 25, "mSeason" ,   "XXXX", .f. )
	vget( 7, 25, "mLeagueid" , "!!",   .f. )
	vget( 8, 25, "mWeek",      "XX",   .f. )
	vget( 9, 25, "lAnon",      "Y",    .f. )

	vread( 1, .f., 'VAL_ROST' )

	CLEAR_WINDOW( 6, 15, 9, 79 )
	if abort()
		retu
	else
		G_week   = mWeek
		G_league = mLeagueId
	endif

	printto = RostPrn() 

	GenMsg()

	PrnRost( mSeason, mLeagueId, mWeek, printto, lAnon )

RETURN


FUNCTION RostPrn
RETURN g_reppath + TEMPORARY_FILE_INDICATOR +"ROST_"  + mleagueid +;
			 PRINTFILE_EXT

FUNCTION PrnRost( mSeason, mLeagueId, mWeek, printto, lAnon  )
	***
	***   Load Data
	***
	cRound = mSeason + mLeagueid + mweek

	nTeams = TeamsIn( mSeason, mLeagueId )
	nGames = int( ( nTeams / 2 ) + .5 )

	private franchise_[ nTeams ]
	private PlyrA_[ nTeams ]
	private PlyrB_[ nTeams ]
	private PlyrC_[ nTeams ]
	private PlyrD_[ nTeams ]
	private PlyrE_[ nTeams ]
	private PlyrF_[ nTeams ]
	private PlyrG_[ nTeams ]
	private PlyrH_[ nTeams ]
	private PlyrI_[ nTeams ]
	private PlyrJ_[ nTeams ]
	private PlyrK_[ nTeams ]
	private PlyrL_[ nTeams ]
	private PlyrM_[ nTeams ]
	private PlyrN_[ nTeams ]
	private PlyrO_[ nTeams ]
	private PlyrP_[ nTeams ]
	private PlyrQ_[ nTeams ]
	private PlyrR_[ nTeams ]
	private PlyrS_[ nTeams ]
	private PlyrT_[ nTeams ]
	private PlyrU_[ nTeams ]
	private PlyrV_[ nTeams ]
	private PlyrW_[ nTeams ]
	private PlyrX_[ nTeams ]
	private PlyrY_[ nTeams ]
	private PlyrZ_[ nTeams ]
	private IR1_[ nTeams ]
	private IR2_[ nTeams ]
	private IR3_[ nTeams ]
	private IR4_[ nTeams ]

	fran_cnt = 0

	select COMP
	COMP->( dbseek( mSeason + mLeagueid ) )
	do while .not. eof() .and. SEASON = mseason .and. LEAGUEID = mleagueid
		INC( fran_cnt )
		franchise_[ fran_cnt ] = OWNERID
		plyrA_[ fran_cnt ]     = PLAYERA
		plyrB_[ fran_cnt ]     = PLAYERB
		plyrC_[ fran_cnt ]     = PLAYERC
		plyrD_[ fran_cnt ]     = PLAYERD
		plyrE_[ fran_cnt ]     = PLAYERE
		plyrF_[ fran_cnt ]     = PLAYERF
		plyrG_[ fran_cnt ]     = PLAYERG
		plyrH_[ fran_cnt ]     = PLAYERH
		plyrI_[ fran_cnt ]     = PLAYERI
		plyrJ_[ fran_cnt ]     = PLAYERJ
		plyrK_[ fran_cnt ]     = PLAYERK
		plyrL_[ fran_cnt ]     = PLAYERL
		plyrM_[ fran_cnt ]     = PLAYERM
		plyrN_[ fran_cnt ]     = PLAYERN
		plyrO_[ fran_cnt ]     = PLAYERO
		plyrP_[ fran_cnt ]     = PLAYERP
		plyrQ_[ fran_cnt ]     = PLAYERQ
		plyrR_[ fran_cnt ]     = PLAYERR
		plyrS_[ fran_cnt ]     = PLAYERS
		plyrT_[ fran_cnt ]     = PLAYERT
		plyrU_[ fran_cnt ]     = PLAYERU
		plyrV_[ fran_cnt ]     = PLAYERV
		plyrW_[ fran_cnt ]     = PLAYERW
		plyrX_[ fran_cnt ]     = PLAYERX
		plyrY_[ fran_cnt ]     = PLAYERY
		plyrZ_[ fran_cnt ]     = PLAYERZ
		IR1_[ fran_cnt ]       = IR1
		IR2_[ fran_cnt ]       = IR2
		IR3_[ fran_cnt ]       = IR3
		IR4_[ fran_cnt ]       = IR4
		skip 
	enddo

	private    pcol1, pcol2, pcol3, pcol4, pcol5, pcol6
	store 0 to pcol1, pcol2, pcol3, pcol4, pcol5, pcol6

   begin sequence

		*---Using pre printed Stationery

      set print to ( printto )
		set print on

		print_setup( LINE_LENGTH )

		*---Short length	(Landscape)
*		?? p_a3
		?? chr(27) + chr(38) + "l1O"			&&		HP Laser
		p_linespp = 78

		*---Page mode
		prntpagebuf()

      print_margin( 0, 0, 0 )

      prntcoldec( 1 )

      *---set up the printing columns
		prntcolnxt( @pcol1,  30 )
		prntcolnxt( @pcol2,  30 )
		prntcolnxt( @pcol3,  30 )
		prntcolnxt( @pcol4,  30 )
		prntcolnxt( @pcol5,  30 )
		prntcolnxt( @pcol6,  30 )

		printat( 1, pcol1,  mSeason + " - " + cLeagueName + " Week " + mweek )
		printat( 1, pcol3, "ROSTERS"                                        )

		TeamNames()	

      if .not. lAnon
   		OwnerPhone()
      endif

		ListPlayer()

		print_page()
		
	end

   print_release()
   set print to        

	@ 22,00
	clrmenu()
	GenMsgClr()

RETURN nil


static FUNCTION ListPlayer

	private rs, nRow, cChar, cCol, cPlyr, cColumn

	For rs = 1 to ROSTER_MAXIMUM
		cChar = chr( 64 + rs )
		For t = 1 to nTeams

			nRow = iif( t < 7, 6+rs, ROSTER_MAXIMUM + 11 + rs )
			cCol = iif( t < 7, LSTR( t, 2, 0 ), LSTR( t-6, 2, 0 ) )

			cPlyr := iif( rs > 26, "IR" + str( rs-26, 1 ), "plyr" + cChar ) + ;
						"_[ " + ltrim( str( t, 2 ) ) + " ]"

			mplayerid = &cPlyr
			cColumn := "pcol" + cCol
			printat( nRow,  &cColumn,;
			 iif( IsStarter( mPlayerid, mWeek, franchise_[ t ] ),;
		 		 upper(  PlayerName( mplayerid ) ), PlayerName( mplayerid ) ) )

			if .not. empty( mPlayerid )
				printat( nRow,  &cColumn+20, TeamInit( mSeason, CurrTeam( mplayerid, date() ) ) )
				printat( nRow,  &cColumn+24, PlayerCat( PLAYER->CATEGORY ) )
				printat( nRow,  &cColumn+26, str( PLAYER->CURRATING, 2 )   )
			endif

		next
	next

RETURN NIL




FUNCTION TeamNames

	for t = 1 to nTeams
		nRow = iif( t < 7, 3, ROSTER_MAXIMUM + 8 )
		cCol = iif( t < 7, LSTR( t, 2, 0 ), LSTR( t-6, 2, 0 ) )

		printat( nRow, pcol&cCol, upper(Franchise( mSeason, mLeagueid, franchise_[t] )) )
	next

RETURN NIL



FUNCTION OwnerPhone

	for t = 1 to nTeams
		nRow = iif( t < 7, 4, ROSTER_MAXIMUM + 9 )
		cCol = iif( t < 7, LSTR( t, 2, 0 ), LSTR( t-6, 2, 0 ) )

		printat( nRow, pcol&cCol,   Owner( franchise_[t] ) )
		printat( nRow+1, pcol&cCol, OWNERS->HPHONENO       )
	next

RETURN NIL



*EOF:  TYRROST.PRE