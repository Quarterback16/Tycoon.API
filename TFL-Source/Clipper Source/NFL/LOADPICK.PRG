***
***	Loads the NFL rookie draft picks into the PLAYER database
***
*				Structure for database : pick.dbf
*				Number of data records : 185
*				Date of last update    : 07/27/93
*				Field  Field Name  Type       Width    Dec
*    				 1  ROUND       Numeric        2      0
*    				 2  SURNAME     Character     15
*    			 	 3  FIRSTNAME   Character     10
*    				 4  TEAM        Character      2
*    			 	 5  COLLEGE     Character     15
*    				 6  POS         Character     10
*    				 7  HEIGHT_FT   Numeric        1      0
*    				 8  HEIGHT_IN   Numeric        2      0
*    				 9  WEIGHT      Numeric        3      0
*				** Total **                   00071
*
*		Determine date of draft
*		For each draft pick
*			Determine ID
*			add record to PLAYER
*			add record to SERVE
*

PROCEDURE LoadPick

	local dDraft, cPlayerID

		*---Select Files to use
	select 0
	OpenDbf( 'PLAYER', g_nfl_path, .t. )	
	select 0
	OpenDbf( 'SCORE', g_nfl_path )	
	select 0
	OpenDbf( 'STAT', g_nfl_path )	
	select 0
	OpenDbf( 'SERVE', g_nfl_path )
	select 0
	OpenDbf( 'SCHED', g_nfl_path )
	select 0
	OpenDbf( 'TEAM', g_nfl_path )	
	select 0
	OpenDbf( 'CATGRY', g_nfl_path )	
	
	select 0
	use PICK alias DRAFT

	dDraft = ctod( "24/4/" + G_season )

	select DRAFT
	go top
	do while .not. DRAFT->( eof() )
	
		cPlayerID = NewID( DRAFT->SURNAME, DRAFT->FIRSTNAME, .t. )
		select PLAYER
		append blank
		replace PLAYER->PLAYERID 	with cPlayerID
		replace PLAYER->SURNAME 	with DRAFT->SURNAME
		replace PLAYER->FIRSTNAME 	with DRAFT->FIRSTNAME
		replace PLAYER->CURRTEAM 	with DRAFT->TEAM
		replace PLAYER->HEIGHT_FT 	with DRAFT->HEIGHT_FT
		replace PLAYER->HEIGHT_IN 	with DRAFT->HEIGHT_IN
		replace PLAYER->WEIGHT	 	with DRAFT->WEIGHT
		*replace PLAYER->FORTY	 	with DRAFT->FORTY
		replace PLAYER->COLLEGE	 	with DRAFT->COLLEGE
		replace PLAYER->ROOKIEYR 	with val( G_season )
		replace PLAYER->POSDESC	 	with DRAFT->POS
		replace PLAYER->CATEGORY 	with Catof( DRAFT->POS )  //  NFL_LIB
		*replace PLAYER->ROLE		 	with iif( DRAFT->MAYSTART, "B", " " )

		select SERVE
		append blank
		replace SERVE->PLAYERID 	with cPlayerID
		replace SERVE->TEAMID	 	with DRAFT->TEAM
		replace SERVE->FROM		 	with dDraft
		replace SERVE->HOW		 	with "D" + ltrim( str( DRAFT->ROUND, 2 ) )

		DRAFT->( dbskip() )
	enddo

	ErrorMsg( "Rookies Loaded!" )

RETURN



*EOF: LOADPICK.PRG