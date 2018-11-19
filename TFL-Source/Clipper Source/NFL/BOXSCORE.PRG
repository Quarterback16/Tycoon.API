*****************************************************************************
*  Program Name.....: BOXSCORE.pre 
*  Programmer.......: Steve Colonna 
*  System...........:
*  Module...........:
*  Date.............: 15/4/1990 at 11:10
*  Copyright........: (c) 1990 by Knoware 
*  Called From......:
*  Purpose..........:
*
*............................................................................
*  Revision.........: 1.0 Last Revised: 15/4/1990 at 11:10
*  Description......: Initial Creation.
*............................................................................
***************************** ALL RIGHTS RESERVED ***************************

#INCLUDE "NFL.H"

#define 		C_FIELDS			c_fields
#define 		C_HELP      	c_help  
#define 		C_BLANK     	c_blank
#define 		GAME_SPOT   	 	15
#define 		WEEK_SPOT   	 	40
#define 		SEASON_SPOT   		65 
#define		MAIN_BOX_TOP		8

PROCEDURE BOXSCORE

   private nmess, poption1
	private mopt[9], mmess[9]
	private mSeason, mWeek, mGameno, nGame, cGameKey
	private nAwayScore, nHomeScore, lLoaded

   set century on

   if !INIT_Skor()    && Initialisation routine
      do CLOSE_Skor 
      RETU
   endif

   do DISP_Skor    && Display Screen
   poption1 = 3

   do while poption1 <> 0 
   
      do FLDS_Skor 

      poption1 = flatmenu( mopt, mmess, poption1, 22, .T.)
      poption1 = if( poption1 = 0, 2, poption1)
   
      clrmenu()  

      do case
         case mopt[poption1] = 'P'

				up_skip( -1 )
				CloseTeams()

         case mopt[poption1] = 'X'

            poption1 = 0
				CloseTeams()

         case mopt[poption1] = 'I'

				mSeason = G_season
				do FIND_Skor 
				CloseTeams()

			case mopt[poption1] = 'B'

				 do SRCH_SKOR
				 CloseTeams()

         case mopt[poption1] = 'L'

            do SCORES with mseason, mweek, mgameno

         case mopt[poption1] = 'S'

				do STATSIN with cGameKey

         case mopt[poption1] = 'D'

            if caned_SKOR()
               up_del()
					CloseTeams()
            endif

			case mopt[poption1] = 'T'

				ShowTeamStats( cGameKey )
				inkey(0)

         case mopt[poption1] = 'N'

            up_skip( 1 )
				CloseTeams()

      endcase
   
   enddo 

   do CLOSE_Skor 

RETURN

*===========================================================================*

function INIT_Skor 

   mopt[1]  = 'Previous'  
   mopt[2]  = 'X-exit'    
   mopt[3]  = 'Inquire'      
   mopt[4]  = 'LineScores'       
   mopt[5]  = 'Stats'       
   mopt[6]  = 'Delete'    
   mopt[7]  = 'Team'      
   mopt[8]  = 'Browse'    
   mopt[9]  = 'Next'      

   mmess[1] = 'View Previous Record'                
   mmess[2] = 'Return to Main Menu'                 
   mmess[3] = 'Locate a Record'                 
   mmess[4] = 'Update the Line Score for a Game'           
   mmess[5] = 'Enter Stats for a game'           
   mmess[6] = 'Delete Record Displayed Currently'   
   mmess[7] = 'View Team Stats'                    
   mmess[8] = 'Browse box scores'                  
   mmess[9] = 'View Next Record'                    


   *---Select Files to use
   close data
	cGameKey = G_Season+G_Week+G_Gameno
	select 0
	OpenDbf( 'PLAYER', g_nfl_path )		
	select 0
	OpenDbf( 'SCHED', g_nfl_path )	
	select 0
	OpenDbf( 'SCORE', g_nfl_path )	
	select 0
	OpenDbf( 'SCTYPE', g_nfl_path )		
	select 0
	OpenDbf( 'STAT', g_nfl_path )	
	select 0
	OpenDbf( 'SERVE', g_nfl_path )
	select 0
	OpenDbf( 'STADIUM', g_nfl_path )	
	select 0
	OpenDbf( 'TEAM', g_nfl_path )	
	
	select SCHED
	dbseek( cGameKey )
	if eof()
		set order to 0
		go bottom
		set order to 1
	endif

	mSeason  = SEASON
	mWeek    = WEEK
	mGameno  = GAMENO
	nGame    = asc( mGameno ) - 64
	cGameKey	= SEASON+WEEK+GAMENO
	lLoaded  = .f.

return( .t. )


procedure CLOSE_Skor 

	select SERVE
	set order to SERVE_BY_PLAYERID

   select SCHED
   set order to 1
   
   CloseDbf( 'TEAM' )
   CloseDbf( 'STADIUM' )
   CloseDbf( 'SERVE' )
   CloseDbf( 'STAT' )
   CloseDbf( 'SCORE' )
   CloseDbf( 'SCHED' )
   
RETURN


FUNCTION CloseTeams

	if lLoaded
		lloaded = .f.

		select HOMETEAM
		use

		select AWAYTEAM
		use
	endif

return( .t. )


procedure FIND_Skor 

   private  old_rec        

	select SCHED
	old_rec = recno()

	do CLR_Skor   

	if GetGame( @mSeason, @mWeek, @mGameno )
		mWeek = strzero( val( mWeek ), 2 )
		select SCHED
		dbseek( mSeason + mWeek + mGameno )
		if eof()
			error_msg( "Game not found" )
			go old_rec
		endif
	endif


return

                               

function GetGame

	parameters mSeason, mWeek, mGameno

	setcolor( C_FIELDS )
	vget( 03, GAME_SPOT,   "nGame",   '99',   .t. )
	vget( 03, WEEK_SPOT,   "mWeek",   '99',   .t. )
	vget( 03, SEASON_SPOT, "mSeason", '9999', .t. )

	vread( 1, .f. )
	mGameno = chr( nGame + 64 )

return ( lastkey() <> 27 )


procedure DISP_Skor 

   clear_scn()
   setcolor( c_desc )

   @  3, GAME_SPOT-7   say "Game : "   + str( nGame, 2 )
   @  3, WEEK_SPOT-7   say "Week : "   + mWeek
   @  3, SEASON_SPOT-9 say "Season : " + mSeason

   head_line('BOXSCORES')
   ScreenLine(  1 )
   ScreenLine(  8 )
   ScreenLine( 21 )

RETURN

                                                                               
procedure FLDS_Skor 

	private row

   do CLR_Skor

	select SCHED
	mSeason  = SEASON
	mWeek    = WEEK	
	mGameno  = GAMENO
	G_Season = SEASON
	G_Week   = WEEK	
	G_Gameno = GAMENO
	cGameKey = SEASON+WEEK+GAMENO
	nGame    = asc( GAMENO ) - 64

   setcolor( C_FIELDS )
	@  3,GAME_SPOT   say str( nGame, 2 )
   @  3,WEEK_SPOT   say mWeek
	@  3,SEASON_SPOT say mSeason
	@  5,10 say TeamStr( AWAYTEAM )
	@  5,50 say TeamStr( HOMETEAM )

	showScore( AWAYSCORE, HOMESCORE )

	@  7, 7 say iif( SCHED->SPREAD > 0,;
		 trim( TeamStr( HOMETEAM, .t. ) ) + ' by ' + str(   SCHED->SPREAD, 4, 1 ),;
		 trim( TeamStr( AWAYTEAM, .t. ) ) + ' by ' + str( 0-SCHED->SPREAD, 4, 1 ) )

	@  7,30 say StadiumOf( SCHED->STADIUM )
	@  7,63 say DayAbbr( dow( GAMEDATE ) )
	@  7,67 say GAMEDATE

   setcolor( C_desc )
	@ MAIN_BOX_TOP+1,  5 say "When"
	@ MAIN_BOX_TOP+1, 12 say "Team"
	@ MAIN_BOX_TOP+1, 17 say "Player"
	@ MAIN_BOX_TOP+1, 37 say "How"
	@ MAIN_BOX_TOP+1, 49 say "Dist"
	@ MAIN_BOX_TOP+1, 54 say "Player 2"
   setcolor( C_FIELDS )

	row = 9
	select SCORE
	set relation to SCORE into SCTYPE
	dbseek( cGameKey )
	do while .not. eof() .and. SEASON+WEEK+GAMENO = cGameKey
		INC( row )
		if row = 21
			exit
		endif

		if SCORE $ TOUCHDOWN_SCORES
			setcolor( C_mainh )
		endif

		@ row, 04 say WHEN
		@ row, 13 say TEAM
		@ row, 16 say iif( empty( PLAYERID1), space(19), PlayerName( PLAYERID1 ) )
		@ row, 37 say left( SCTYPE->SCOREDESC, 11 )
		@ row, 50 say iif( empty( DISTANCE ), space( 3), str( DISTANCE, 3 ) )
		@ row, 56 say iif( empty( PLAYERID2), space(19), PlayerName( PLAYERID2 ) )

		if SCORE $ TOUCHDOWN_SCORES
			setcolor( C_fields )
		endif

		skip
	enddo
	set relation to

	select SCHED
	do RLOCKIT with "NFL Schedule"

	TheScore( cGameKey )
	showScore( nAwayScore, nHomeScore )
	WriteScore( nHomeScore, nAwayScore )
							 
RETURN


procedure CLR_Skor

   setcolor( C_BLANK )
   @  3,GAME_SPOT   say space(2)
	@  3,WEEK_SPOT   say space(2)
	@  3,SEASON_SPOT say space(4)
	CLEAR_WINDOW( 4, 0,  7, 79 )
	CLEAR_WINDOW( 9, 0, 20, 79 )

return  && from clr_Skor



function CANED_skor
   ***
   *** types of records that can be edited
   ***

return( up_canedit() )
     

procedure SRCH_SKOR
	***
	***   Calls a routine to display a list of records in a file and
	***   allows for selection of one of those.
	***

	private s_fields[6], s_mess[6]

	select SCHED
	s_fields[1] = "SEASON"
	s_fields[2] = "WEEK"
	s_fields[3] = "TeamStr( AWAYTEAM, .t. )"
	s_fields[4] = "AWAYSCORE"
	s_fields[5] = "TeamStr( HOMETEAM, .t. )"
	s_fields[6] = "HOMESCORE"

	s_mess[1]   = "Season"
	s_mess[2]   = "Week"
	s_mess[3]   = "Away Team"
	s_mess[4]   = "Pts"
	s_mess[5]   = "Home Team"
	s_mess[6]   = "Pts"

	setcolor( C_HELP )
	TBL_SRCH( s_fields, s_mess )

RETURN


FUNCTION StadiumOf( cStadCode )

	local cStadium := ""

	if STADIUM->( dbseek( cStadCode ) )
		cStadium := trim( STADIUM->NAME )
	endif

RETURN cStadium



*EOF:  BOXSCORE.PRE 
