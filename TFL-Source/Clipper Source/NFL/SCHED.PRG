*****************************************************************************
*  Program Name.....: Sched.pre 
*  Programmer.......: Steve Colonna 
*  System...........:
*  Module...........:
*  Date.............: 15/4/1990 at 11:10
*  Copyright........: (c) 1990 by Knoware
*  Called From......:
*  Purpose..........: To record the NFL schedule
*
*............................................................................
*  Revision.........: 1.0 Last Revised: 15/4/1990 at 11:10
*  Description......: Initial Creation.
*............................................................................
*  Revision.........: 2.0 Last Revised: 15/9/1999 at 11:10
*  Description......: Spruced up Tipping report adding Starting QB and RB.
*                     Potential Enhancement - Previous results
*                     DD/MM/YYYY - AT 99 v HT 99   XXXXXXXXXXXXXXXXXXXXXXXXX
*............................................................................
***************************** ALL RIGHTS RESERVED ***************************

#INCLUDE "nfl.h"
#include "..\lib\print.h"

#define  LINE_LENGTH 		96

#define	FUDGE_FACTOR				1.5
#define	HOMEFIELD_ADVANTAGE		3

#define C_FIELDS c_fields
#define C_HELP   c_help  
#define C_BLANK  c_blank

PROCEDURE SCHED

   private nmess, poption1
	private mopt[7], mmess[7]
	private  gamedate_[ MAX_GAMES_PER_WEEK ]
	private  gamehour_[ MAX_GAMES_PER_WEEK ]
	private  gamelive_[ MAX_GAMES_PER_WEEK ]
	private  awayteam_[ MAX_GAMES_PER_WEEK ]
	private  hometeam_[ MAX_GAMES_PER_WEEK ]
	private awayscore_[ MAX_GAMES_PER_WEEK ]
	private homescore_[ MAX_GAMES_PER_WEEK ]
	private    spread_[ MAX_GAMES_PER_WEEK ]
	private mSeason, mWeek, nGames, g, lAdding

   set century on

   if !INIT_SKED()    && Initialisation routine
      do CLOSE_SKED 
      RETU
   endif

   do DISP_SKED    && Display Screen
   poption1 = 3

   do while poption1 <> 0 
   
      do FLDS_SKED 

      poption1 = flatmenu (mopt, mmess, poption1, 22, .T.)
      poption1 = if( poption1 = 0, 2, poption1)
   
      clrmenu()  

      do case
         case mopt[poption1] = 'P'

				do while SEASON = mSeason .and. WEEK = mWeek
					skip -1
					if bof()
						error_msg( 'Top of file reached' )
						exit
					endif
				enddo

         case mopt[poption1] = 'X'

            poption1 = 0

         case mopt[poption1] = 'I'

            do FIND_SKED 

         case mopt[poption1] = 'U'

            do UpdateSked

         case mopt[poption1] = 'D'

				LoadRound( mSeason, mWeek, nGames, .t. )
				up_skip( -1 )

         case mopt[poption1] = 'T'

				TipRound( mSeason, mWeek )

         case mopt[poption1] = 'N'

				do while SEASON = mSeason .and. WEEK = mWeek
					skip
					if eof()
						error_msg( 'End of file reached' )
						go bottom
						exit
					endif
				enddo

      endcase
   
   enddo 

   do CLOSE_SKED 

RETURN

*===========================================================================*

function INIT_SKED 
   ***
   ***   Initialise all variables, arrays and open files
   ***

   mopt[1]  = 'Previous'  
   mopt[2]  = 'X-exit'    
   mopt[3]  = 'Inquire'      
   mopt[4]  = 'Update'       
   mopt[5]  = 'Delete'    
   mopt[6]  = 'Tip'      
   mopt[7]  = 'Next'      

   mmess[1] = 'View Previous Record'                
   mmess[2] = 'Return to Main Menu'                 
   mmess[3] = 'Locate a Record'                 
   mmess[4] = 'Update a round of matches'           
   mmess[5] = 'Delete Record Displayed Currently'   
   mmess[6] = 'Tip this Round'                    
   mmess[7] = 'View Next Record'                    

   *---Select Files to use
	select 0
	OpenDbf( 'PLAYER', g_nfl_path )	
	select 0
	OpenDbf( 'TOTTIP', g_nfl_path )
	select 0
	OpenDbf( 'TEAM', g_nfl_path, .t. )	   
	select 0
	OpenDbf( 'STAT', g_nfl_path )	 	
   select 0
   OpenDbf( 'SCHED', g_nfl_path )   
   select SCHED
   set order to 1
*   testmsg( G_Season + G_week )
   SCHED->( dbseek( G_Season + G_week ) )
*	go lastrec()    && start at last record entered
	mSeason = SEASON
	mWeek   = WEEK
	nGames  = 0

RETURN .T.


procedure CLOSE_SKED 
   ***
   ***   Closing Down 
   ***
   CloseDbf( 'PLAYER' )
   CloseDbf( 'TOTTIP' )
   CloseDbf( 'TEAM' )
   CloseDbf( 'STAT' )   
   CloseDbf( 'SCHED' )
*   select SCHED
*   set order to 1

RETURN


procedure FIND_SKED 
   ***
   ***     Allows search for a particular record
   ***
   LOCAL  old_rec        

	select SCHED
	old_rec = recno()

	do CLR_SKED   

	if GetKey( @mSeason, @mWeek )
		mWeek = strzero( val( mWeek ), 2 )
		select SCHED
		dbseek( mSeason + mWeek )
		if eof()
			error_msg( "Round not found" )
			go old_rec
      else
         G_week = mWeek
		endif
	endif

RETURN

                               
PROCEDURE UpdateSked
   ***
   ***   Allows the Update of a round
   ***

   select SCHED
   do CLR_SKED

	if GetKey( @mSeason, @mWeek )
		*---Load it into memory from disk

		LoadRound( mSeason, mWeek, nGames, .t. )

		*---Get Changes
		GetSked( @nGames, lAdding )

		SaveRound( mSeason, mWeek, nGames )

	endif

RETURN  


FUNCTION GetKey

	parameters mSeason, mWeek

	setcolor( C_FIELDS )
	vget( 03, 15, "mWeek",   '99',   .t. )
	vget( 03, 57, "mSeason", '9999', .t. )

	vread( 1, .f. )


RETURN( lastkey() <> 27 )


procedure DISP_SKED 
   ***
   ***   Displays initial screen details
   ***

   setcolor( C_BLANK )
   clear_scn()
   setcolor( c_desc )

   @  3, 8 say "Week : "   + mWeek
   @  3,48 say "Season : " + mSeason
   @  4,11 say "Date"
   @  4,29 say "Away"
   @  4,42 say "Home"
   @  4,56 say "Spread"
   @  4,66 say "Score"

   head_line('NFL Schedule')
   ScreenLine(  1 )
   ScreenLine( 21 )

RETURN

                                                                               
procedure FLDS_SKED 
   ***
   ***   Displays a round
   ***
   do CLR_SKED

	select SCHED
	mSeason = SEASON
	mWeek   = WEEK
   setcolor( C_FIELDS )
   @  3,15 say mWeek
	@  3,57 say mSeason

	nGames = LoadRound( mSeason, mWeek, nGames, .f. )

	for g = 1 to nGames
		@ 4+g,  2 say str( g, 2 )
		@ 4+g, 07 say gamedate_[ g ]
		@ 4+g, 19 say DayAbbr( dow( gamedate_[ g ] ) )
		@ 4+g, 23 say gamehour_[ g ]
		@ 4+g, 26 say iif( gamelive_[ g ], "tv", "  " )
		@ 4+g, 29 say TeamStr( awayteam_[ g ], .t. )
		@ 4+g, 40 say '@'
		@ 4+g, 42 say TeamStr( hometeam_[ g ], .t. )
		@ 4+g, 57 say SpreadOut( spread_[ g ] )
		@ 4+g, 64 say iif( date()-1 >= gamedate_[ g ], str( awayscore_[ g ], 3 ), "   " )
		@ 4+g, 68 say iif( date()-1 >= gamedate_[ g ], str( homescore_[ g ], 3 ), "   " )
	next

RETURN


FUNCTION SpreadOut( nSpread )

   local cOut

   if nSpread = 0
      cOut = "  OTB"
   else
      if nSpread = 0.5
         cOut = "  PKM"
      else
         cOut := str( nSpread, 5, 1 )
      endif
   endif

RETURN cOut


FUNCTION NextWeekDate

  local dOut
  select SCHED
  go bott
  dOut := SCHED->GAMEDATE + 6

return dOut


FUNCTION LoadRound

	parameters mSeason, mWeek, nGames, lDelete

	nGames  = 0
	afill( gamedate_, Date() )
	afill( gamehour_, " "  )
	afill( gamelive_, .f.  )
	afill( awayteam_, "  " )
	afill( hometeam_, "  " )
	afill( spread_,   0    )
	afill( awayscore_, 0   )
	afill( homescore_, 0   )

	select SCHED
	dbseek( mSeason + mWeek )
	if eof()
		Error_Msg( 'Adding new round' )
		lAdding = .t.
	else
		lAdding = .f.
	endif

	do while .not. eof() .and. SEASON = mSeason .and. WEEK = mWeek
		nGames = nGames + 1
		gamedate_[  nGames ] = GAMEDATE
		gamehour_[  nGames ] = GAMEHOUR
      gamelive_[  nGames ] = GAMELIVE
		awayteam_[  nGames ] = AWAYTEAM
		hometeam_[  nGames ] = HOMETEAM
		awayscore_[ nGames ] = AWAYSCORE
		homescore_[ nGames ] = HOMESCORE
		spread_[    nGames ] = SPREAD
		if lDelete
			do RLOCKIT with "Schedule"
			delete
		endif

		skip
	enddo
	skip -1

RETURN( nGames )


FUNCTION SaveRound

	parameters mSeason, mWeek, nGames

	*testmsg( 'Saving - No of Games ' + LSTR( nGames, 3, 0 ) )
	select SCHED
	for g = 1 to nGames
		do ADDRECIT with "Schedule"
		replace SEASON    with mseason
		replace WEEK      with mweek
		replace GAMENO    with chr( 64 + g )
		replace GAMEDATE  with gamedate_[  g ]
		replace GAMEHOUR  with gamehour_[  g ]
		replace GAMELIVE  with gamelive_[  g ]
		replace AWAYTEAM  with awayteam_[  g ]
		replace HOMETEAM  with hometeam_[  g ]
		replace SPREAD    with spread_[    g ]
		if .not. lAdding
			replace AWAYSCORE with awayscore_[ g ]
			replace HOMESCORE with homescore_[ g ]
		endif
		commit 
	next

RETURN( nGames )



procedure CLR_SKED
   ***
   ***   Clear the fields details from the screen
   ***

   setcolor( C_BLANK )
   @  3,15 say space(2)
	@  3,57 say space(4)

	scroll( 5, 0, MAX_GAMES_PER_WEEK+4, 79, 0 )

RETURN


FUNCTION GetSked

	parameter nGames, lAdding

	private mgamedate, mawayteam, mhometeam, dLastDate, nSpread, mGameHour, mLastHour

	select SCHED

	dLastDate = NextWeekDate()
   mLastHour = "1"
	setcolor( C_FIELDS )

	for g = 1 to MAX_GAMES_PER_WEEK

		mgamedate = if( lAdding, dLastDate, gamedate_[ g ] )
		mgamedate = if( mGameDate = date(), dLastDate, mGameDate )
      mGameHour = if( GameHour_[ g ] = " ", mLastHour,  gamehour_[ g ] )
		mawayteam =	if( lAdding,  space(2),  awayteam_[ g ] )
		mhometeam =	if( lAdding,  space(2),  hometeam_[ g ] )
		nSpread   = if( lAdding,         0,  spread_[   g ] )

		@ 4+g,  2 say str( g, 2 )
		@ 4+g, 40 say '@'

		vget( 4+g,  7, 'mgamedate',  '@D',    .t. )
		vget( 4+g, 20, 'mgamehour',  '!',     .t. )
		vget( 4+g, 29, 'mawayteam',  '!!',    .t. )
		vget( 4+g, 42, 'mhometeam',  '!!',    .t. )

*      if dLastDate - date() < 14
  		   vget( 4+g, 57, 'nSpread',    '999.9', .f. )
*      endif

		vread( 1, .F., "VAL_SKED" )
		if abort()
			if lAdding
				nGames = g - 1
			endif

			exit
		else
			if g > nGames
				nGames = g
			endif
			gamedate_[ g ] = mgamedate
         gamehour_[ g ] = mGameHour
			awayteam_[ g ] = mawayteam
			hometeam_[ g ] = mhometeam
			spread_[ g ]   = nSpread
			dLastDate      = mgamedate
         mLastHour      = mGameHour
		endif
	next

RETURN( !abort() )


FUNCTION Val_sked 

   parameter mvar
   LOCAL ok := .t.

   do case

      case mvar == 'mawayteam'

			ok = TEAM->( dbseek( mSeason + mAwayTeam ) )

      case mvar == 'mhometeam'

			ok = TEAM->( dbseek( mSeason + mHomeTeam ) )

   endcase

RETURN( ok )              


FUNCTION	TipRound( mSeason, mWeek )

	*---Predict the scores for the round
	local nOldRec, nAvgScore, cRankWeek, nWeeksAgo, cRankSeason

	*---Cant use this weeks scores to help predict, unless its week 18
*	TestMsg( "mWeek = " +  mWeek )
	cRankWeek := right( "00" + alltrim( str( val( mWeek ) - iif( mWeek="18", 0, 1), 2)), 2 )
*	TestMsg( "CRankWeek = " +  cRankWeek )

	*---Rank teams to the TEAM DBF
	ClearRanks( mSeason )

   if cRankWeek < "03"
      cRankSeason := str( val( mSeason ) - 1, 4 )
      cRankWeek := "17"    &&  Dont use round 18 as it is all the playoffs (gives SB winners multiple points)
   	TestMsg( "Using season " +  cRankSeason )
   else
      cRankSeason := mSeason
   endif
	TestMsg( "CRankWeek = " +  cRankWeek )

	*---Determine the Average Score
	nAvgScore := AvgScore( cRankSeason, "01", cRankWeek )

	*---Rate the teams, updates Offense and Defense ratings
	select SCHED
	nOldRec = SCHED->( recno() )
	SCHED->( dbseek( cRankSeason ) )
	do while .not. eof() .and. SCHED->SEASON = cRankSeason

		if SCHED->WEEK > cRankWeek
			exit
		else
			nWeeksAgo = val( cRankWeek ) - val( SCHED->WEEK )
		endif

		if SCHED->HOMESCORE + SCHED->AWAYSCORE  > 0
			RateGame( nAvgScore, cRankSeason, nWeeksAgo )
		endif

		SCHED->( dbSKip() )
	enddo

	*---Print the report
	PrintTips( mSeason, mWeek, nAvgScore, cRankSeason )

	select SCHED
	SCHED->( dbSeek( mSeason + mWeek ) )

RETURN .t.


FUNCTION AvgScore( cSeason, cWeek1, cWeek2 )

	local nAvgScore := 0, nTotGames := 0, nTotPoints := 0
   local cAvgSeason

	select SCHED
	nOldRec = SCHED->( recno() )
	SCHED->( dbseek( cSeason + cWeek1 ) )
	do while .not. eof() .and. SCHED->SEASON = cSeason

		if SCHED->WEEK > cWeek2
			exit
		endif

		if SCHED->GAMEDATE < date()
			if ( SCHED->AWAYSCORE + SCHED->HOMESCORE ) > 0
				nTotGames++
				nTotPoints += ( SCHED->AWAYSCORE + SCHED->HOMESCORE )
			endif

		endif
		SCHED->( dbSKip() )
	enddo
	nAvgScore := ( nTotPoints / 2 ) / nTotGames
	TestMsg( "Average Score was " + str( nAvgScore, 5, 1 ) )

RETURN nAvgScore


FUNCTION PrintTips( cSeason, cWeek, nAvgScore, cRankSeason )

	local nCorrect := 0, nGames := 0, nATS := 0, nSpreads := 0, nSpec := 0, nLocks := 0
   local nDogWins := 0, nDogLosses := 0

   local cPlay, cHome, cAway, cResult

	private printto

	printto  = g_reppath + TEMPORARY_FILE_INDICATOR + "TIPS" + cWeek + ;
									PRINTFILE_EXT

	private    pcol1, pcol2, pcol3, pcol4, pcol5, pcol6, pcol7, pcol8, pcol9, pcolL
	store 0 to pcol1, pcol2, pcol3, pcol4, pcol5, pcol6, pcol7, pcol8, pcol9, pcolL

   begin sequence

      set print to ( printto )
		set print on

		print_setup( LINE_LENGTH )

		*?? chr(27) +"E" + chr(27)+ chr(38) + "I" +chr(27) + "(0U" + chr(27) + "(s0p12h12v0s0b8T" + chr(27) + "&a12L"
		p_linespp = 9999

      print_margin( 0, 0, 0 )

      prntcoldec( 1 )

      *---set up the printing columns
		prntcolnxt( @pcol1,  11 )
		prntcolnxt( @pcol2,  10 )
		prntcolnxt( @pcol3,   5 )
		prntcolnxt( @pcol4,   2 )
		prntcolnxt( @pcol5,  10 )
		prntcolnxt( @pcol6,   5 )
		prntcolnxt( @pcol7,   2 )
		prntcolnxt( @pcol8,   9 )
		prntcolnxt( @pcolL,   6 )		
		prntcolnxt( @pcol9,  10 )

		printat( pcol1, cSeason + " - NFL Tips - Round " + cWeek )
		println()
		println()

		printat( pcol2, "Away Team" )
		printat( pcol5, "Home Team" )
		printat( pcol8+3, "Spread"  )
		printat( pcol9, "CPU Tip"   )
		println()
		println()

		select SCHED
		dbseek( cSeason + cWeek )
		do while .not. SCHED->( eof() ) .and. SCHED->SEASON = cSeason .and. ;
														  SCHED->WEEK   = cWeek

			nAwayScore := TipScore( SCHED->AWAYTEAM, SCHED->HOMETEAM, nAvgScore, cRankSeason )
			nHomeScore := TipScore( SCHED->HOMETEAM, SCHED->AWAYTEAM, nAvgScore, cRankSeason )
			nHomeScore += HOMEFIELD_ADVANTAGE
			if nAwayScore = nHomeScore
				nHomeScore++
			endif
			nSpread    := nHomeScore - nAwayScore

			TEAM->( dbseek( cSeason + SCHED->AWAYTEAM ) )
         cAway := TEAM->TEAMNAME

*        TestMsg( 'Game Rating ' + str( GameRating( SCHED->SEASON, SCHED->HOMETEAM, SCHED->AWAYTEAM ), 3 ) )

         *--Scoreline
			printat( pcol1, Timing( SCHED->GAMEDATE, SCHED->GAMEHOUR, SCHED->GAMELIVE ) + ;
			         str( GameRating( SCHED->SEASON, SCHED->HOMETEAM, SCHED->AWAYTEAM ), 3 ) )

			printat( pcol2, trim( cAway )      )
			printat( pcol3, '['+str( nAwayScore, 3 )+']' )
			printat( pcol4, "@"                         )
			TEAM->( dbseek( cSeason + SCHED->HOMETEAM ) )
         cHome := TEAM->TEAMNAME
			printat( pcol5, trim( cHome )      )
			printat( pcol6, '['+str( nHomeScore, 3 )+']' )
			printat( pcol7, "="                         )
			printat( pcol8, str( nSpread, 3 )  + ;
                        iif( isSpec( nSpread, SCHED->SPREAD ), "*", " " ) + ;
                        iif( isSpread(), str( SCHED->SPREAD, 5, 1 ) + ;
                           BadNo( SCHED->SPREAD ), iif( SCHED->SPREAD = .5, "  PKM", "  OTB"  )) )

			printat( pcolL, str( SCHED->TOTAL, 5, 1 ) )
			
         cPlay := space(10)
         if IsSpread()
            if nSpread > SCHED->SPREAD
               cPlay := trim( cHome )
            endif
            if nSpread < SCHED->SPREAD
               cPlay := trim( cAway )
            endif
         endif
			printat( pcol9, cPlay + ;
             iif( .not. empty(cPlay), " (" + ltrim( str( abs( nSpread - SCHED->SPREAD), 4, 1 )) + ")", "" ) )

			if SCHED->GAMEDATE < date()-1
				printat( pcol9+10, "(" + str( SCHED->AWAYSCORE, 3 ) + "-" + ;
									 		   str( SCHED->HOMESCORE, 3 ) + ")"  )
            *---Tally Accuracy
				if SCHED->HOMESCORE > SCHED->aWAYSCORE
					*---Home win
					if nHomeScore > nAwayScore
						nCorrect++
					endif
				else
					*---Away win
					if nAwayScore > nHomeScore
						nCorrect++
					endif
				endif

            *---Is there a spread? if <> 0 or .5
            if isSpread()
               *---A valid Spread
               if nSpread <> SCHED->SPREAD
                 nSpreads++
               endif

               if isSpec( nSpread, SCHED->SPREAD )
                  nSpec++
               endif

               cResult := BeatSpread( SCHED->SPREAD, nSpread, SCHED->HOMESCORE, SCHED->AWAYSCORE )
               do case
                  case cResult = "W"
                     *---beat the spread
                     nATS++
                     if isSpec( nSpread, SCHED->SPREAD )

                        nLocks++
                     endif
                  case cResult = "P"
                     *---Push
                     nSpread--
               endcase
               if SCHED->SPREAD < 0
                  *---Home team was underdog
                  if ( SCHED->AWAYSCORE + SCHED->SPREAD ) < SCHED->HOMESCORE
                     *---Home Dog Won!
                     nDogWins++
                  endif
                  if ( SCHED->AWAYSCORE + SCHED->SPREAD ) > SCHED->HOMESCORE
                     *---Home Dog Lost!
                     nDogLosses++
                  endif
               endif

            endif

				nGames++
				
         else
            *---Game has not been played yet thus current
     			println()

            *--QB Line
			   printat( pcol2, PlyrInfo( SCHED->AWAYTEAM, QUARTERBACK_CAT  ) )
   			printat( pcol5, PlyrInfo( SCHED->HOMETEAM, QUARTERBACK_CAT  ) )
			   printat( pcolL, str( nAwayScore + nHomeScore, 5, 1 ) )						
   			printat( pcol9, RatingInfo( SCHED->AWAYTEAM, SCHED->HOMETEAM ) )
 			   println()

            *--RB Line
			   printat( pcol2, PlyrInfo( SCHED->AWAYTEAM, RUNNINGBACK_CAT  ) )
   			printat( pcol5, PlyrInfo( SCHED->HOMETEAM, RUNNINGBACK_CAT  ) )
				
				if ( SCHED->TOTAL > 0 )
					if ( ( nAwayScore + nHomeScore ) > SCHED->TOTAL )
						printat( pcolL, " OVER" )
					else
						if ( ( nAwayScore + nHomeScore ) < SCHED->TOTAL ) 
							printat( pcolL, "UNDER" )
						endif
					endif
				endif
   			printat( pcol9, MisMatches( SCHED->AWAYTEAM, SCHED->HOMETEAM ) )			
 			   println()

			endif

  			println()
         select SCHED
*        TestMsg( 'Sched rec ' + str( SCHED->( recno() ), 8 ) )
			skip
		enddo

		if nCorrect > 0
         *--Update Total Tips
         UpdateTips( SCHED->SEASON, cWeek, ;
                     nCorrect, nGames-nCorrect, ;
                     nATS, nSpreads - nATS, ;
                     nLocks, nSpec - nLocks, nDogWins, nDogLosses )

         *---Straight up
  			println()
			printat( pcol2, "Correct = " + str( nCorrect, 3 ) + "-" + ;
													 str( nGames-nCorrect  , 2 ) )
			printat( pcol5, str( percent( nCorrect, nGames ), 6, 1 ) + "%" )
         printat( pcol6, TipTotal( SCHED->SEASON, cWeek, 'SU' ) )
         printat( pcol9, TipTotal( '', '', 'SU' ) )
  			println()

         *---Against the Spread
			printat( pcol2, "ATS     = " + str( nATS    , 3 ) + "-" + ;
													 str( nSpreads - nATS, 2 ) )
			printat( pcol5, str( percent( nATS, nSpreads ), 6, 1 ) + "%" )
         printat( pcol6, TipTotal( SCHED->SEASON, cWeek, 'ATS' ) )
         printat( pcol9, TipTotal( '', '', 'ATS' ) )
  			println()

         *---Locks
			printat( pcol2, "Locks   = " + str( nLocks  , 3 ) + "-" + ;
													 str( nSpec - nLocks, 2 ) )
			printat( pcol5, str( percent( nLocks, nSpec ), 6, 1 ) + "%" )
         printat( pcol6, TipTotal( SCHED->SEASON, cWeek, 'Locks' ) )
         printat( pcol9, TipTotal( '', '', 'Locks' ) )
  			println()

         *---Home Dogs
			printat( pcol2, "HomeDogs= " + str( nDogWins  , 3 ) + "-" + ;
													 str( nDogLosses, 2 ) )
			printat( pcol5, str( percent( nDogWins, (nDogWins + nDogLosses )), 6, 1 ) + "%" )
         printat( pcol6, TipTotal( SCHED->SEASON, cWeek, 'HomeDogs' ) )
         printat( pcol9, TipTotal( '', '', 'HomeDogs' ) )
  			println()
  			println()

		endif

		print_page()
		
	end

   print_release()
   set print to        

   view_prnt( printto, LINE_LENGTH )

RETURN .t.


FUNCTION BadNo( nSpread )

  local cBadNo := " "

  ***
  ***  Bad numbers are numbers that show a statistical trend of 60% or more
  ***
  ***  eg Home favourites by 9.5 point fail to cover > 60% of the time
  ***
  ***  f means the favourite fails to cover
  ***  c means the favourite covers
  ***
  do case
  case nSpread = -2
    cBadNo := 'f'
  case nSpread = -3.5
    cBadNo := 'f'
  case nSpread = -4
    cBadNo := 'c'
  case nSpread = -7
    cBadNo := 'c'
  case nSpread = 5
    cBadNo := 'f'
  case nSpread = 5.5
    cBadNo := 'f'
  case nSpread = 6.5
    cBadNo := 'f'
  case nSpread = 9.5
    cBadNo := 'f'
  case nSpread = 10
    cBadNo := 'f'
  case nSpread = 14
    cBadNo := 'f'
  case nSpread > 14
    cBadNo := 'f'
  endcase

return cBadNo


function TipTotal

   parameters cSeason, cWeek, cTip

   local nTotalW := 0
   local nTotalL := 0

	if TOTTIP->( dbseek( cSeason ) )
      do while TOTTIP->SEASON = cSeason .and. TOTTIP->WEEK <= cWeek ;
                                        .and. .not. TOTTIP->( eof() )
         do case
         case cTip = "SU"
            nTotalW += TOTTIP->SU_W
            nTotalL += TOTTIP->SU_L
         case cTip = "ATS"
            nTotalW += TOTTIP->ATS_W
            nTotalL += TOTTIP->ATS_L
         case cTip = "Locks"
            nTotalW += TOTTIP->LK_W
            nTotalL += TOTTIP->LK_L
         case cTip = "HomeDogs"
            nTotalW += TOTTIP->DOG_W
            nTotalL += TOTTIP->DOG_L
         endcase
         TOTTIP->( dbskip() )
      enddo
   endif

return str( nTotalW, 4 ) + "-" + str( nTotalL, 4 ) + "  " + str( percent( nTotalW, nTotalW + nTotalL ), 5, 1 ) + "%"


static FUNCTION MisMatches( cAwayTeam, cHomeTeam )

   local cOut, cAwayRate, cHomeRate, nMissMatches

	nOldArea  = select()
   nOldRec = recno()

   select TEAM
   nOldOrder := indexord()
   cOut      := ''

   set order to 1
   if TEAM->( dbseek(G_Season+cAwayTeam) )
      cAwayRate := TEAM->RATE
   endif
   if TEAM->( dbseek(G_Season+cHomeTeam) )
      cHomeRate := TEAM->RATE
   endif

   *---Compare Passing Off with Pass Defense
   cAwayPO := substr( cAwayRate, 1, 1 )
   cHomePD := substr( cHomeRate, 6, 1 )
   if abs( asc(cAwayPO) - asc( cHomePD ) ) > 2
      if asc(cAwayPO) < asc( cHomePD )
         cOut := 'passOffA;'
      else
         cOut := 'passDefH;'
      endif
   else
      *--Home wins
      if abs( asc(substr(cHomeRate, 1, 1)) - asc( substr(cAwayRate, 6, 1 ) ) ) > 2
         if asc(substr(cHomeRate, 1, 1)) < asc( substr(cAwayRate, 6, 1 ) )
            cOut := 'passOffH;'
         else
            cOut := 'passDefA;'
         endif
      endif
   endif

   *---Compare Rushing Off with Run Defense
   cAwayRO := substr( cAwayRate, 2, 1 )
   cHomeRD := substr( cHomeRate, 5, 1 )
   cHomeRO := substr( cHomeRate, 2, 1 )
   cAwayRD := substr( cAwayRate, 5, 1 )
   if abs( asc(cAwayRO) - asc( cHomeRD ) ) > 2
      if asc(cAwayRO) < asc( cHomeRD )
         cOut += 'runOffA;'
      else
         cOut += 'runDefH;'
      endif
   else
      if abs( asc( cHomeRO ) - asc( cAwayRD ) ) > 2
         if asc(cHomeRO) < asc( cAwayRD )
            cOut += 'runOffH;'
         else
            cOut += 'runDefA;'
         endif
      endif
   endif

   *---Compare Pass Rush against Pass Protection
   cAwayPR := substr( cAwayRate, 4, 1 )
   cHomePP := substr( cHomeRate, 3, 1 )
   cHomePR := substr( cHomeRate, 4, 1 )
   cAwayPP := substr( cAwayRate, 3, 1 )
   if abs( asc(cAwayPR) - asc( cHomePP ) ) > 2
      if asc(cAwayPR) < asc( cHomePP )
         cOut += 'blitzA;'
      else
         cOut += 'protectH;'
      endif
   else
      if abs( asc( cHomePR ) - asc( cAwayPP ) ) > 2
         if asc(cHomePR) < asc( cAwayPP )
            cOut += 'blitzH;'
         else
            cOut += 'protectA;'
         endif
      endif
   endif

   cOut := substr( cOut, 1, len(cOut)-1 )

   select TEAM
   set order to nOldOrder
   select( nOldArea )
   go nOldRec

RETURN cOut


static FUNCTION RatingInfo( cAwayTeam, cHomeTeam )

local cOut

	nOldArea  = select()
   nOldRec = recno()

   select TEAM
   nOldOrder := indexord()
   cOut      := ''

   set order to 1
   if TEAM->( dbseek(G_Season+cAwayTeam) )
      cOut := TEAM->RATE + '-' + RateToPts( TEAM->RATE ) + ' @ '
   endif
   if TEAM->( dbseek(G_Season+cHomeTeam) )
      cOut := cOut + TEAM->RATE + '-' + RateToPts( TEAM->RATE )
   endif

   select TEAM
   set order to nOldOrder
   select( nOldArea )
   go nOldRec

RETURN cOut


FUNCTION RateToPts( cRate )

  local cPoints, nPoints
  nPoints := 0
  nPoints := nPoints + RPointsFor( substr( cRate, 1, 1 ) )  && Passing Off
  nPoints := nPoints + RPointsFor( substr( cRate, 2, 1 ) )
  nPoints := nPoints + RPointsFor( substr( cRate, 3, 1 ) )
  nPoints := nPoints + RPointsFor( substr( cRate, 4, 1 ) )
  nPoints := nPoints + RPointsFor( substr( cRate, 5, 1 ) )
  nPoints := nPoints + RPointsFor( substr( cRate, 6, 1 ) )

  cPoints := str( nPoints, 2 )

return cPoints


FUNCTION RPointsFor( cRateCode )

  local nRPoints

  nRPoints := 0
  do case
  case cRateCode = "A"
     nRPoints := 10
  case cRateCode = "B"
     nRPoints :=  7
  case cRateCode = "C"
     nRPoints :=  5
  case cRateCode = "D"
     nRPoints :=  3
  endcase

RETURN nRPoints


FUNCTION PlyrInfo( cTeam, cCat )

   local cOut, nOldArea, nOldOrder, cPlyrID, nGames

	nOldArea  = select()
   nOldRec = recno()
   TestMsg( 'need Starting ' + cCat + ' for ' + cTeam )

   select PLAYER
   nOldOrder = indexord()
   cOut = 'unknown'

   set order to 3  &&  CATEGORY + PLAYERID
   if .not. PLAYER->( dbseek(cCat) )
      TestMsg( 'No Cat ' + cCat )
   endif
   do while .not. eof() .and. PLAYER->CATEGORY = cCat
      if PLAYER->CURRTEAM = cTeam .and. PLAYER->ROLE = K_STARTER
			TestMsg( cCat + '=' + PLAYER->PLAYERID + ' ' + PLAYER->CURRTEAM + '-' + cTeam )
			if	at( "FB", PLAYER->POSDESC ) > 0
				TestMsg( "Skipping FB " + substr( PLAYER->FIRSTNAME, 1, 1 ) + trim( PLAYER->SURNAME ) )
			else
				cOut = substr( PLAYER->FIRSTNAME, 1, 1 ) + ;
                   trim( PLAYER->SURNAME ) + ;
                   iif( str(PLAYER->ROOKIEYR,4) = SCHED->SEASON, "*", "" )
				cPlyrID = PLAYER->PLAYERID
				exit
			endif
      endif
      select PLAYER
      skip
   enddo

   if .not. empty(cPlyrID)
      do case
      case cCat = QUARTERBACK_CAT
         nGames = CountGames( cPlyrID, PASSING_ATTEMPTS, 15 )
      case cCat = RUNNINGBACK_CAT
         nGames = CountGames( cPlyrID, RUSHING_CARRIES, 10 )
      endcase

      cOut = cOut + str( nGames, 4)
   endif

   select PLAYER
   set order to nOldOrder
   select( nOldArea )
   go nOldRec

RETURN cOut


FUNCTION CountGames( cPlayerID, cStat, nLimit )

   local nGames, nOldStatOrd, nOldArea

   nOldArea = select()
   nGames = 0
   select STAT
   nOldStatOrd = indexord()
   set order to 2
   STAT->( dbseek( cPlayerID + cStat ) )
   do while .not. eof() .and. STAT->STAT = cStat ;
                        .and. STAT->PLAYERID = cPlayerID

      if STAT->QTY > nLimit
         nGames++
      endif

      skip
   enddo

   set order to nOldStatOrd
   select( nOldArea )

RETURN nGames

  
  
FUNCTION Timing( dGame, cHour, lLive )

   local cOut

   do case
   case dow(dGame) = 1
      cOut = "Su"
   case dow(dGame) = 2
      cOut = "Mo"
   case dow(dGame) = 3
      cOut = "Tu"
   case dow(dGame) = 4
      cOut = "We"
   case dow(dGame) = 5
      cOut = "Th"
   case dow(dGame) = 6
      cOut = "Fr"
   case dow(dGame) = 7
      cOut = "Sa"
   endcase

   cOut += ' ' + cHour + ' ' + iif(lLive, 'tv', '  ')

RETURN cOut


FUNCTION UpdateTips( cSeason, cWeek, nSUwins, nSUlosses, nATSwins, nATSlosses, ;
                     nLockWins, nLockLosses, nDogWins, nDogLosses )

   TestMsg( 'Updating Season ' + cSeason + ' Week ' + cWeek )

	if TOTTIP->( dbseek( cSeason + cWeek ) )
      *---Record exists
		replace TOTTIP->SU_W    with nSUwins
		replace TOTTIP->SU_L    with nSUlosses
		replace TOTTIP->ATS_W   with nATSwins
		replace TOTTIP->ATS_L   with nATSlosses
		replace TOTTIP->LK_W    with nLockwins
		replace TOTTIP->LK_L    with nLockLosses
		replace TOTTIP->DOG_W   with nDogWins
		replace TOTTIP->DOG_L   with nDogLosses
      commit

   else
      *---New Record
      select TOTTIP
		do ADDRECIT with "Tipping Records"
		replace SEASON  with cSeason
		replace WEEK    with cWeek
		replace SU_W    with nSUwins
		replace SU_L    with nSUlosses
		replace ATS_W   with nATSwins
		replace ATS_L   with nATSlosses
		replace LK_W    with nLockwins
		replace LK_L    with nLockLosses
		replace DOG_W   with nDogWins
		replace DOG_L   with nDogLosses
      commit
   endif

RETURN .t.

   
FUNCTION BeatSpread( nSpread, nCompSpread, nHome, nAway )

   local cResult := "P", lHome := .t.

   if nSpread = nCompSpread
      cResult := "P"
   else

      if nSpread > 0
         *---Home favourite
         if nCompSpread > nSpread
            lHome := .t.
         else
            lHome := .f.
         endif

      else
         *---Away favourite
         if nCompSpread < nSpread
            lHome := .f.
         else
            lHome := .t.
         endif

      endif

      if lHome
         *---playing home team
         if nHome - nSpread > nAway
            cResult := "W"
         	*TestMsg( "home pick - Beats spread" )

         else
         	*TestMsg( "home pick - looses spread" )
            cResult := "L"
         endif
      else
         *---playing away team
         if nAway + nSpread > nHome
         	*TestMsg( "away pick - Beats spread" )
            cResult := "W"
         else
            cResult := "L"
         	*TestMsg( "away pick - looses spread" )
         endif
      endif
   endif

return cResult


FUNCTION IsSpec( nCompSpread, nSpread )

   local lSpec := .f.

   if nSpread <> 0
      if abs( nCompSpread - nSpread ) > 9.5
         lSpec := .t.
      endif
   endif

return lSpec


FUNCTION IsSpread

   local lOnBoard := .f.

   if SCHED->SPREAD <> 0 .and. SCHED->SPREAD <> .5
      lOnBoard := .t.
   endif

return lOnBoard

   
FUNCTION ClearRanks( mSeason )

	TEAM->( dbseek( mSeason ) )
	do while .not. TEAM->( eof() ) .and. TEAM->SEASON = mSeason
		replace TEAM->OFFENSE with 0
		replace TEAM->DEFENSE with 0
		TEAM->( dbskip() )
	enddo

RETURN .t.


FUNCTION	AdjustRank( mSeason, cTeam, nOff, nDef )

	if TEAM->( dbseek( mSeason + cTeam ) )
		replace TEAM->OFFENSE with ( TEAM->OFFENSE + nOff )
		replace TEAM->DEFENSE with ( TEAM->DEFENSE + nDef )
	endif

RETURN .t.


FUNCTION	GameRating( mSeason, cHomeTeam, cAwayTeam )
   local nRating
   nRating := GetWins( mSeason, cHomeTeam ) + GetWins( mSeason, cAwayTeam )
RETURN nRating


FUNCTION	GetWins( mSeason, cTeam )

   local nWins
   nWins := -1
	if TEAM->( dbseek( mSeason + cTeam ) )
		nWins := TEAM->WINS
	endif

RETURN nWins


FUNCTION TipScore( cTeam, cOpponent, nAvgScore, cRankSeason )

	*---Given a Team and an Opponent tip the Score for that team

	local nTeamOff := 0, nOppDef := 0, nTip := 0

	if TEAM->( dbseek( cRankSeason + cTeam ) )
		nTeamOff := TEAM->OFFENSE
	endif
	if TEAM->( dbseek( cRankSeason + cOpponent ) )
		nOppDef := TEAM->DEFENSE
	endif
*  TestMsg( mseason + ":" + cTeam + "-Off" + str( nTeamOff, 4 ) + " vs Def" + + str( nOppDef, 4 ) )

	nTip := nAvgScore + ( ( nTeamOff + nOppDef ) / 2 )
	nTip := int( nTip + .5 )

	nTip := PickAScore( nTip )

RETURN nTip


FUNCTION PickAScore( nTip )

	local nClosest
*	TestMsg( "nTip=" + str( nTip, 5, 1 ) )
	do case 
	case nTip = 1
		nClosest := 0
	case nTip = 2
		nClosest := 3
	case nTip = 4
		nClosest := 3
	case nTip = 5
		nClosest := 6
	case nTip = 8
		nClosest := 7
	case nTip = 11
		nClosest := 10
	case nTip = 15
		nClosest := 14
	case nTip = 16
		nClosest := 17
	case nTip = 18
		nClosest := 17
	case nTip = 19
		nClosest := 20
	case nTip = 22
		nClosest := 21
	case nTip = 25
		nClosest := 24
	case nTip = 26
		nClosest := 28
	case nTip = 29
		nClosest := 28
	case nTip = 32
		nClosest := 31
	case nTip = 34
		nClosest := 35
	case nTip = 36
		nClosest := 35
	case nTip = 37
		nClosest := 38
	case nTip = 39
		nClosest := 38
	otherwise
		nClosest := nTip
	endcase

*	TestMsg( "Closest" + str( nClosest, 5, 1 ) )

RETURN nClosest


FUNCTION RateGame( nAvgScore, mSeason, nWeeksAgo )

	LOCAL nHomeTip, nAwayTip, nHomeOff, nHomeDef

	nHomeTip := TipScore( SCHED->HOMETEAM, SCHED->AWAYTEAM, nAvgScore, mSeason  )
	nAwayTip := TipScore( SCHED->AWAYTEAM, SCHED->HOMETEAM, nAvgScore, mSeason  )
	nHomeOff := ( Max35( SCHED->HOMESCORE ) - nHomeTip ) / min( FUDGE_FACTOR + nWeeksAgo, 6 )
	nHomeDef := ( Max35( SCHED->AWAYSCORE ) - nAwayTip )/ min( FUDGE_FACTOR + nWeeksAgo, 6 )
	AdjustRank( mSeason, SCHED->HOMETEAM, nHomeOff, nHomeDef )
	nAwayOff := ( Max35( SCHED->AWAYSCORE ) - nAwayTip ) / min( FUDGE_FACTOR + nWeeksAgo, 6 )
	nAwayDef := ( Max35( SCHED->HOMESCORE ) - nHomeTip ) / min( FUDGE_FACTOR + nWeeksAgo, 6 )
	AdjustRank( mSeason, SCHED->AWAYTEAM, nAwayOff, nAwayDef )

RETURN .t.


FUNCTION Max35( nScore )
   if	nScore > 35
		nScore = 35
   endif
RETURN nScore



*EOF:  SCHED.PRE