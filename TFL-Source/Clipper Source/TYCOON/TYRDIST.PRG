*****************************************************************************
*  Program Name.....: TYRDIST
*  Programmer.......: Steve Colonna 
*  System...........: TFL
*  Module...........: TYCOON
*  Date.............: 17/9/1995 at 18:19
*  Copyright........: (c) 1990 by Knoware 
*  Called From......:
*  Purpose..........:
*
*	  Prints out a Owner distribution list
*
*............................................................................
*  Revision.........: 1.0 Last Revised: 24/9/1990 at 21:21
*  Description......: 
*............................................................................
***************************** ALL RIGHTS RESERVED ***************************

#INCLUDE "..\lib\PRINT.H"
#INCLUDE "TYCOON.H"

#define  LINE_LENGTH 		182
#define  ROSTER_MAXIMUM		30

PROCEDURE TyRDist

	private printto, workfile, cLeagueName
   private mopt[4], mmess[4], poption1

   clear_scn()

   head_line( "DISTRIBUTION LIST" )

   ScreenLine(  1 )
   ScreenLine( 21 )

   if !INIT_Dist()    && Initialisation routine
      do CLOSE_Dist
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

				do GEN_Dist
            view_prnt( printto, LINE_LENGTH )

         case mopt[poption1] = 'V'

            view_prnt( printto, LINE_LENGTH )

         case mopt[poption1] = 'P'

				print_prnt( printto )

      endcase
   
   enddo 

   do CLOSE_Dist

return


function INIT_Dist 

	private mcontinu

	plswait()
   std_rpt_opt( @mopt, @mmess, "Tycoon Matchups" )

   mcontinu = .t.
*  mcontinu = iif( mcontinu, opendbf( 'TYSKED', G_ty_path   ), .f. )
*	mcontinu = iif( mcontinu, opendbf( 'LINEUP', G_ty_path   ), .f. )
   mcontinu = iif( mcontinu, opendbf( 'LEAGUE', G_ty_path   ), .f. )
   mcontinu = iif( mcontinu, opendbf( 'COMP',   G_ty_path   ), .f. )
   mcontinu = iif( mcontinu, opendbf( 'OWNERS', G_ty_path   ), .f. )
*  mcontinu = iif( mcontinu, opendbf( 'SCORE',  G_nfl_path  ), .f. )
*	mcontinu = iif( mcontinu, opendbf( 'PLAYER', G_nfl_path  ), .f. )
*	mcontinu = iif( mcontinu, opendbf( 'SERVE',  G_nfl_path  ), .f. )
*	mcontinu = iif( mcontinu, opendbf( 'TEAM',   G_nfl_path  ), .f. )

	@ 23,00

	if ! mcontinu
		error_msg( "Unable to gain access to all the necessary files" )
	endif

RETURN mcontinu


procedure CLOSE_Dist

	close databases

RETURN


function VAL_Dist

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
			seek mLeagueid
			if found()
				cLeagueName = LEAGUE
			else
				ok = .f.
				error_msg( "League " + mLeagueid + " not found" )
			endif

	endcase
	setcolor( saveclr )

RETURN ok 


procedure GEN_Dist

   private i, repoption, mSeason, mLeagueid

	setcolor( c_desc )
	print( 6, 15, "Season :" )
	print( 7, 15, "League :" )

	setcolor( c_fields )
	mSeason   = G_season
	mLeagueid = G_league

	vget( 6, 25, "mSeason" ,   "XXXX", .f. )
	vget( 7, 25, "mLeagueid" , "!!",   .f. )

	vread( 1, .f., 'VAL_Dist' )

	CLEAR_WINDOW( 6, 15, 8, 79 )
	if abort()
		retu
	else
		G_league = mLeagueId
	endif

	printto = g_reppath + TEMPORARY_FILE_INDICATOR +"DIST_"  + mleagueid +;
					 PRINTFILE_EXT

	GenMsg()
	***
	***   Load Data
	***

	nTeams = TeamsIn( mSeason, mLeagueId )
	nGames = int( ( nTeams / 2 ) + .5 )

	private franchise_[ nTeams, 2 ]
	fran_cnt = 0

	select COMP
	seek mSeason + mLeagueid
	do while .not. eof() .and. SEASON = mseason .and. LEAGUEID = mleagueid
		INC( fran_cnt )
		franchise_[ fran_cnt, 1 ] = COMP->OWNERID
		franchise_[ fran_cnt, 2 ] = OwnerName( COMP->OWNERID )
		skip 
	enddo

	franchise_ := asort( franchise_,,, { |x,y| x[2] < y[2] } )

	private    pcol1, pcol2, pcol3, pcol4, pcol5, pcol6
	store 0 to pcol1, pcol2, pcol3, pcol4, pcol5, pcol6

   begin sequence

		*---Using pre printed Stationery

      set print to ( printto )
		set print on

		print_setup( LINE_LENGTH )

		*---Short length	(Landscape)
*		?? p_a3
*		?? chr(27) + chr(38) + "l1O"			&&		HP Laser
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

		printat( 1, pcol1,  mSeason + " - " + cLeagueName  )
		printat( 1, pcol3, "Distribution List"             )

		DistList()

		print_page()
		
	end

   print_release()
   set print to        

	@ 22,00
	clrmenu()
	GenMsgClr()

RETURN



FUNCTION DistList

	local t, nRow
	printat( 3, pcol1,    "Name" )
	printat( 3, pcol1+ 22, "Address" )
	printat( 3, pcol1+ 75, "Phone H" )
	printat( 3, pcol1+ 90, "Phone W" )
	printat( 3, pcol1+105, "Fax"     )
	printat( 3, pcol1+120, "EMail"   )

	for t = 1 to nTeams
		nRow := ( t * 2 ) + 3
		printat( nRow, pcol1,    OwnerName( franchise_[ t, 1 ] ) )
		printat( nRow, pcol1+ 22, OAddress() )
		printat( nRow, pcol1+ 75, OWNERS->HPHONENO )
		printat( nRow, pcol1+ 90, OWNERS->WPHONENO )
		printat( nRow, pcol1+105, OWNERS->FAX      )
		printat( nRow, pcol1+120, OWNERS->OTHER    )
	next

RETURN NIL


FUNCTION OAddress

	local cAddr := space(50)

	cAddr := trim( OWNERS->ADDR1 ) + ", " + trim( OWNERS->ADDR2 ) + ;
				trim( OWNERS->SUBURB ) + "  " + OWNERS->STATE + ;
				"  " + OWNERS->POSTCODE + cAddr
	cAddr := substr( cAddr, 1, 50 )

RETURN cAddr


FUNCTION OwnerName( mOwnerid )

	local cName := space(20)

	if OWNERS->( dbseek( mOwnerID ) )
		cName := trim( OWNERS->LASTNAME ) + ", " + OWNERS->FIRSTNAME + cName
		cName := substr( cName, 1, 20 )
	endif

RETURN cName
	



*EOF:  TYRDIST.PRG 

