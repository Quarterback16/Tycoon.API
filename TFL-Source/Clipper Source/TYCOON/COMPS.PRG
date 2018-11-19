*****************************************************************************
*  Program Name.....: comps.pre 
*  Programmer.......: Steve Colonna 
*  System...........:
*  Module...........:
*  Date.............: 5/9/1990 at 21:21
*  Copyright........: (c) 1990 by Knoware 
*  Called From......:
*  Purpose..........:
*
*
* 		Sets up a competition file.
*
*
*............................................................................
*  Revision.........: 1.0 Last Revised: 5/9/1990 at 21:21
*  Description......: Initial Creation.
*............................................................................
***************************** ALL RIGHTS RESERVED ***************************

#INCLUDE "TYCOON.H"

#DEFINE C_FIELDS c_fields
#DEFINE C_HELP   c_help
#DEFINE C_BLANK  c_blank

PROCEDURE COMPS

private nmess, poption1, mopt[10], mmess[10]
private mseason, mleagueid, mownerid, mfranchise, mFrancode, mDivision
private mplayera, mplayerb, mplayerc, mplayerd, mplayere, mplayerf, mplayerg
private mplayerh, mplayeri, mplayerj, mplayerk, mplayerl, mplayerm, mplayern
private mplayero, mplayerp, mplayerq, mplayerr, mplayers, mplayert
private mplayeru, mplayerv, mplayerw, mplayerx, mplayery, mplayerz
private cIR1, cIR2, cIR3, cIR4, mfield

if !INIT_COMP()											 && Initialisation routine
	do CLOSE_COMP
	RETU
endif

do DISP_COMP												 && Display Screen
poption1 = 3

do while poption1 <> 0

	do CLR_COMP
	do FLDS_COMP

	poption1 = flatmenu (mopt, mmess, poption1, 22, .T.)
	poption1 = if( poption1 = 0, 2, poption1)

	clrmenu()

	* where there's two 1st letters the same, make both strings long enough
	* to be unique

	do case
		case mopt[poption1] = 'P'

			up_skip( -1 )

		case mopt[poption1] = 'X'

			poption1 = 0

		case mopt[poption1] = 'B'

			do SRCH_COMP

		case mopt[poption1] = 'I'

			do FIND_COMP

		case mopt[poption1] = 'A'

			do ADD_COMP

		case mopt[poption1] = 'E'

			do EDIT_COMP

		case mopt[poption1] = 'L'

			mseason   = COMP->SEASON
			mleagueid = COMP->LEAGUEID
			mOwnerid  = COMP->OWNERID
			do LINEUPS with mseason+mleagueid+mOwnerid

		case mopt[poption1] = 'D'

			if caned_COMP()
				up_del()
			endif

		case mopt[poption1] = 'N'

			up_skip( 1 )

		case mopt[poption1] = 'R'

			select COMP
			up_list( 'COMP', "space( 4 )+ OWNERID +space(3)+ LASTNAME" )

	endcase

enddo

do CLOSE_COMP

RETURN

*===========================================================================*

function INIT_COMP
	***
	***   Initialise all variables, arrays and open files
	***

	mopt[1]  = 'Previous'
	mopt[2]  = 'X-exit'
	mopt[3]  = 'Browse'
	mopt[4]  = 'Inquire'
	mopt[5]  = 'Add'
	mopt[6]  = 'Edit'
	mopt[7]  = 'Delete'
	mopt[8]  = 'Report'
	mopt[9]  = 'Lineups'
	mopt[10]  = 'Next'

	mmess[1] = 'View Previous Record'
	mmess[2] = 'Return to Main Menu'
	mmess[3] = 'Browse Through All Records in the File'
	mmess[4] = 'Locate a Record'
	mmess[5] = 'Add A New Record'
	mmess[6] = 'Amend Details Displayed Currently'
	mmess[7] = 'Delete Record Displayed Currently'
	mmess[8] = 'List all the Records to the printer'
	mmess[9]  = 'Update Lineups'
	mmess[10] = 'View Next Record'


	*---Select Files to use
	if opendbf( "COMP", G_ty_path )
		if opendbf( "LEAGUE", G_ty_path )
			if opendbf( "OWNERS", G_ty_path )
			if opendbf( "LINEUP", G_ty_path )
			if opendbf( "PLAYER", G_nfl_path )

				select COMP
				set order to 1
				set relation to LEAGUEID into LEAGUE, to OWNERID into OWNERS
            if	empty( G_team )
   				go lastrec()
            else
               COMP->( dbseek( G_team ) )
      		endif

			endif
			endif
			endif
		endif
	endif

RETURN .t.


procedure CLOSE_COMP
	***
	***   Closing Down
	***

	close databases

return														 && from CLOSE_COMP


procedure FIND_COMP
	***
	***     Allows search for a particular record
	***
	private dkey, crec

	select COMP
	do CLR_COMP
	setcolor( C_FIELDS )
	up_find( "SEASON", 3, 17 )

RETURN


procedure ADD_COMP
	***
	***   Allows the addition of a record
	***

	select COMP
	do CLR_COMP

	setcolor( C_FIELDS )
	priv crec

	crec = recno()
	go lastrec()+1
	mseason    = G_season
	mleagueid  = G_League
	mownerid   = OWNERID

	do while .t.											 && read loop

		vget(  3,17, 'mseason',    '',    .f. )
		vget(  4,17, 'mleagueid',  '',    .f. )
		vget(  6,17, 'mownerid',   '',    .f. )

		vread( 1, .F., "VAL_COMP" )
		if abort()
			go crec
			retu
		endif

		dkey = mseason+mleagueid+mownerid

		dbseek( dkey )
		if found()
			error_msg( 'Record already exists ' + dkey )
		elseif !add_rec(5)
			warning( "add_rec" )
		else
			replace SEASON   with mseason
			replace LEAGUEID with mleagueid
			replace OWNERID  with mownerid
			exit
		endif

	enddo


	GET_COMP( .t. )										 && returns abort()
	commit
	unlock

RETURN


procedure EDIT_COMP
	***
	***   Allows amendments of fields displayed on the screen
	***

	select COMP
	setcolor( C_FIELDS )
	if !caned_COMP()
		retu
	endif

	if rec_lock(5)
		GET_COMP( .f. )											 && returns abort()
		unlock
		commit
	else
		warning('rec_lock')
	endif

RETURN


function CANED_COMP
	***
	*** types of records that can be edited
	***

RETURN up_canedit()


procedure SRCH_COMP
	***
	***   Calls a routine to display a list of records in a file and
	***   allows for selection of one of those.
	***

	private s_fields[4], s_mess[4]

	s_fields[1] = "SEASON"
	s_fields[2] = "COMP->FRANCHISE"
	s_fields[3] = "trim( OWNERS->FIRSTNAME ) + ' ' + OWNERS->LASTNAME"
	s_fields[4] = "LEAGUE->LEAGUE"

	s_mess[1]   = "Season"
	s_mess[2]   = "Franchise"
	s_mess[3]   = "Owner"
	s_mess[4]   = "League"

	setcolor( C_HELP )
	TBL_SRCH( s_fields, s_mess )

RETURN


procedure DISP_COMP
	***
	***   Displays initial screen details
	***

	clear_scn()
	setcolor( c_desc )

	@  3, 8 say "Season :"
	@  4, 8 say "League :"

	@  6, 8 say "Owner  :"
	@  8, 8 say "Team   :"

	@ 10, 4 say  "1"
	@ 11, 4 say  "2"
	@ 12, 4 say  "3"
	@ 13, 4 say  "4"
	@ 14, 4 say  "5"
	@ 15, 4 say  "6"
	@ 16, 4 say  "7"
	@ 17, 4 say  "8"
	@ 18, 4 say  "9"
	@ 19, 3 say "10" 

	@ 10, 30 say "11"
	@ 11, 30 say "12"
	@ 12, 30 say "13"
	@ 13, 30 say "14"
	@ 14, 30 say "15"
	@ 15, 30 say "16"
	@ 16, 30 say "17"
	@ 17, 30 say "18"
	@ 18, 30 say "19"
	@ 19, 30 say "20" 

	@ 10, 56 say "R1"
	@ 11, 56 say "R2"
	@ 12, 56 say "R3"
	@ 13, 56 say "R4"
	@ 14, 56 say "R5"
	@ 15, 56 say "R6"
	@ 16, 56 say "I1"
	@ 17, 56 say "I2"
	@ 18, 56 say "I3"
	@ 19, 56 say "I4" 

	head_line('COMP')
	ScreenLine(  1 )
	ScreenLine( 21 )

RETURN


procedure FLDS_COMP
	***
	***   Displays the fields on the screen
	***
   local nRosterSize
   local nTeamExp

	setcolor( C_FIELDS )
	select COMP
	if !up_can()
		RETU
	endif

	mSeason   = COMP->SEASON
	G_season  = mSeason
	mLeagueid = COMP->LEAGUEID
	G_league  = mLeagueId
   G_team    = G_season + mLeagueID + COMP->OWNERID

	setcolor( C_FIELDS )
	@  3,17 say SEASON
	@  3,37 say FRANCHISE
	@  4,17 say LEAGUE->LEAGUE
	@  6,17 say trim( OWNERS->FIRSTNAME ) + " " + OWNERS->LASTNAME
	@  8,17 say FRANCHISE
	@  8,40 say FRANCODE
	@  8,45 say DIVISION
   nTeamExp := TeamExp()
   @  8,60 say str( nTeamExp, 5 )
   nRosterSize := RosterSize()
   @  8,67 say str( nRosterSize, 5 )

	@ 10,  6 say iif( empty( PLAYERA ), space(20), iif( IsStarter( PLAYERA, G_week, OWNERS->OWNERID ), upper( PlayerName( PLAYERA )), PlayerName( PLAYERA ) ) + ' ' + PLAYER->CURRTEAM +  str(PLAYER->SCORES,3) + PLAYER->ROLE)
	@ 11,  6 say iif( empty( PLAYERB ), space(20), iif( IsStarter( PLAYERB, G_week, OWNERS->OWNERID ), upper( PlayerName( PLAYERB )), PlayerName( PLAYERB ) ) + ' ' + PLAYER->CURRTEAM +  str(PLAYER->SCORES,3) + PLAYER->ROLE)
	@ 12,  6 say iif( empty( PLAYERC ), space(20), iif( IsStarter( PLAYERC, G_week, OWNERS->OWNERID ), upper( PlayerName( PLAYERC )), PlayerName( PLAYERC ) ) + ' ' + PLAYER->CURRTEAM +  str(PLAYER->SCORES,3) + PLAYER->ROLE)
	@ 13,  6 say iif( empty( PLAYERD ), space(20), iif( IsStarter( PLAYERD, G_week, OWNERS->OWNERID ), upper( PlayerName( PLAYERD )), PlayerName( PLAYERD ) ) + ' ' + PLAYER->CURRTEAM +  str(PLAYER->SCORES,3) + PLAYER->ROLE)
	@ 14,  6 say iif( empty( PLAYERE ), space(20), iif( IsStarter( PLAYERE, G_week, OWNERS->OWNERID ), upper( PlayerName( PLAYERE )), PlayerName( PLAYERE ) ) + ' ' + PLAYER->CURRTEAM +  str(PLAYER->SCORES,3) + PLAYER->ROLE)
	@ 15,  6 say iif( empty( PLAYERF ), space(20), iif( IsStarter( PLAYERF, G_week, OWNERS->OWNERID ), upper( PlayerName( PLAYERF )), PlayerName( PLAYERF ) ) + ' ' + PLAYER->CURRTEAM +  str(PLAYER->SCORES,3) + PLAYER->ROLE)
	@ 16,  6 say iif( empty( PLAYERG ), space(20), iif( IsStarter( PLAYERG, G_week, OWNERS->OWNERID ), upper( PlayerName( PLAYERG )), PlayerName( PLAYERG ) ) + ' ' + PLAYER->CURRTEAM +  str(PLAYER->SCORES,3) + PLAYER->ROLE)
	@ 17,  6 say iif( empty( PLAYERH ), space(20), iif( IsStarter( PLAYERH, G_week, OWNERS->OWNERID ), upper( PlayerName( PLAYERH )), PlayerName( PLAYERH ) ) + ' ' + PLAYER->CURRTEAM +  str(PLAYER->SCORES,3) + PLAYER->ROLE)
	@ 18,  6 say iif( empty( PLAYERI ), space(20), iif( IsStarter( PLAYERI, G_week, OWNERS->OWNERID ), upper( PlayerName( PLAYERI )), PlayerName( PLAYERI ) ) + ' ' + PLAYER->CURRTEAM +  str(PLAYER->SCORES,3) + PLAYER->ROLE)
	@ 19,  6 say iif( empty( PLAYERJ ), space(20), iif( IsStarter( PLAYERJ, G_week, OWNERS->OWNERID ), upper( PlayerName( PLAYERJ )), PlayerName( PLAYERJ ) ) + ' ' + PLAYER->CURRTEAM +  str(PLAYER->SCORES,3) + PLAYER->ROLE)

	@ 10, 33 say iif( empty( PLAYERK ), space(20), iif( IsStarter( PLAYERK, G_week, OWNERS->OWNERID ), upper( PlayerName( PLAYERK )), PlayerName( PLAYERK ) ) + ' ' + PLAYER->CURRTEAM +  str(PLAYER->SCORES,3) + PLAYER->ROLE)
	@ 11, 33 say iif( empty( PLAYERL ), space(20), iif( IsStarter( PLAYERL, G_week, OWNERS->OWNERID ), upper( PlayerName( PLAYERL )), PlayerName( PLAYERL ) ) + ' ' + PLAYER->CURRTEAM +  str(PLAYER->SCORES,3) + PLAYER->ROLE)
	@ 12, 33 say iif( empty( PLAYERM ), space(20), iif( IsStarter( PLAYERM, G_week, OWNERS->OWNERID ), upper( PlayerName( PLAYERM )), PlayerName( PLAYERM ) ) + ' ' + PLAYER->CURRTEAM +  str(PLAYER->SCORES,3) + PLAYER->ROLE)
	@ 13, 33 say iif( empty( PLAYERN ), space(20), iif( IsStarter( PLAYERN, G_week, OWNERS->OWNERID ), upper( PlayerName( PLAYERN )), PlayerName( PLAYERN ) ) + ' ' + PLAYER->CURRTEAM +  str(PLAYER->SCORES,3) + PLAYER->ROLE)
	@ 14, 33 say iif( empty( PLAYERO ), space(20), iif( IsStarter( PLAYERO, G_week, OWNERS->OWNERID ), upper( PlayerName( PLAYERO )), PlayerName( PLAYERO ) ) + ' ' + PLAYER->CURRTEAM +  str(PLAYER->SCORES,3) + PLAYER->ROLE)
	@ 15, 33 say iif( empty( PLAYERP ), space(20), iif( IsStarter( PLAYERP, G_week, OWNERS->OWNERID ), upper( PlayerName( PLAYERP )), PlayerName( PLAYERP ) ) + ' ' + PLAYER->CURRTEAM +  str(PLAYER->SCORES,3) + PLAYER->ROLE)
	@ 16, 33 say iif( empty( PLAYERQ ), space(20), iif( IsStarter( PLAYERQ, G_week, OWNERS->OWNERID ), upper( PlayerName( PLAYERQ )), PlayerName( PLAYERQ ) ) + ' ' + PLAYER->CURRTEAM +  str(PLAYER->SCORES,3) + PLAYER->ROLE)
	@ 17, 33 say iif( empty( PLAYERR ), space(20), iif( IsStarter( PLAYERR, G_week, OWNERS->OWNERID ), upper( PlayerName( PLAYERR )), PlayerName( PLAYERR ) ) + ' ' + PLAYER->CURRTEAM +  str(PLAYER->SCORES,3) + PLAYER->ROLE)
	@ 18, 33 say iif( empty( PLAYERS ), space(20), iif( IsStarter( PLAYERS, G_week, OWNERS->OWNERID ), upper( PlayerName( PLAYERS )), PlayerName( PLAYERS ) ) + ' ' + PLAYER->CURRTEAM +  str(PLAYER->SCORES,3) + PLAYER->ROLE)
	@ 19, 33 say iif( empty( PLAYERT ), space(20), iif( IsStarter( PLAYERT, G_week, OWNERS->OWNERID ), upper( PlayerName( PLAYERT )), PlayerName( PLAYERT ) ) + ' ' + PLAYER->CURRTEAM +  str(PLAYER->SCORES,3) + PLAYER->ROLE)

	@ 10, 59 say iif( empty( PLAYERU ), space(20), iif( IsStarter( PLAYERU, G_week, OWNERS->OWNERID ), upper( PlayerName( PLAYERU )), PlayerName( PLAYERU ) ) + ' ' + PLAYER->CURRTEAM +  str(PLAYER->SCORES,3) + PLAYER->ROLE)
	@ 11, 59 say iif( empty( PLAYERV ), space(20), iif( IsStarter( PLAYERV, G_week, OWNERS->OWNERID ), upper( PlayerName( PLAYERV )), PlayerName( PLAYERV ) ) + ' ' + PLAYER->CURRTEAM +  str(PLAYER->SCORES,3) + PLAYER->ROLE)
	@ 12, 59 say iif( empty( PLAYERW ), space(20), iif( IsStarter( PLAYERW, G_week, OWNERS->OWNERID ), upper( PlayerName( PLAYERW )), PlayerName( PLAYERW ) ) + ' ' + PLAYER->CURRTEAM +  str(PLAYER->SCORES,3) + PLAYER->ROLE)
	@ 13, 59 say iif( empty( PLAYERX ), space(20), iif( IsStarter( PLAYERX, G_week, OWNERS->OWNERID ), upper( PlayerName( PLAYERX )), PlayerName( PLAYERX ) ) + ' ' + PLAYER->CURRTEAM +  str(PLAYER->SCORES,3) + PLAYER->ROLE)
	@ 14, 59 say iif( empty( PLAYERY ), space(20), iif( IsStarter( PLAYERY, G_week, OWNERS->OWNERID ), upper( PlayerName( PLAYERY )), PlayerName( PLAYERY ) ) + ' ' + PLAYER->CURRTEAM +  str(PLAYER->SCORES,3) + PLAYER->ROLE)
	@ 15, 59 say iif( empty( PLAYERZ ), space(20), iif( IsStarter( PLAYERZ, G_week, OWNERS->OWNERID ), upper( PlayerName( PLAYERZ )), PlayerName( PLAYERZ ) ) + ' ' + PLAYER->CURRTEAM +  str(PLAYER->SCORES,3) + PLAYER->ROLE)
	@ 16, 59 say iif( empty( IR1     ), space(20), iif( IsStarter( IR1    , G_week, OWNERS->OWNERID ), upper( PlayerName( IR1     )), PlayerName( IR1     ) ) + ' ' + PLAYER->CURRTEAM +  str(PLAYER->SCORES,3) + PLAYER->ROLE)
	@ 17, 59 say iif( empty( IR2     ), space(20), iif( IsStarter( IR2    , G_week, OWNERS->OWNERID ), upper( PlayerName( IR2     )), PlayerName( IR2     ) ) + ' ' + PLAYER->CURRTEAM +  str(PLAYER->SCORES,3) + PLAYER->ROLE)
	@ 18, 59 say iif( empty( IR3     ), space(20), iif( IsStarter( IR3    , G_week, OWNERS->OWNERID ), upper( PlayerName( IR3     )), PlayerName( IR3     ) ) + ' ' + PLAYER->CURRTEAM +  str(PLAYER->SCORES,3) + PLAYER->ROLE)
	@ 19, 59 say iif( empty( IR4     ), space(20), iif( IsStarter( IR4    , G_week, OWNERS->OWNERID ), upper( PlayerName( IR4     )), PlayerName( IR4     ) ) + ' ' + PLAYER->CURRTEAM +  str(PLAYER->SCORES,3) + PLAYER->ROLE)

RETURN


FUNCTION TeamExp

  local nExpPoints := 0

  *--For each rostered player accumulate points
  nExpPoints += EPointsFor( COMP->PLAYERA )
  nExpPoints += EPointsFor( COMP->PLAYERB )
  nExpPoints += EPointsFor( COMP->PLAYERC )
  nExpPoints += EPointsFor( COMP->PLAYERD )
  nExpPoints += EPointsFor( COMP->PLAYERE )
  nExpPoints += EPointsFor( COMP->PLAYERF )
  nExpPoints += EPointsFor( COMP->PLAYERG )
  nExpPoints += EPointsFor( COMP->PLAYERH )
  nExpPoints += EPointsFor( COMP->PLAYERI )
  nExpPoints += EPointsFor( COMP->PLAYERJ )
  nExpPoints += EPointsFor( COMP->PLAYERK )
  nExpPoints += EPointsFor( COMP->PLAYERL )
  nExpPoints += EPointsFor( COMP->PLAYERM )
  nExpPoints += EPointsFor( COMP->PLAYERN )
  nExpPoints += EPointsFor( COMP->PLAYERO )
  nExpPoints += EPointsFor( COMP->PLAYERP )
  nExpPoints += EPointsFor( COMP->PLAYERQ )
  nExpPoints += EPointsFor( COMP->PLAYERR )
  nExpPoints += EPointsFor( COMP->PLAYERS )
  nExpPoints += EPointsFor( COMP->PLAYERT )
  nExpPoints += EPointsFor( COMP->PLAYERU )
  nExpPoints += EPointsFor( COMP->PLAYERV )
  nExpPoints += EPointsFor( COMP->PLAYERW )
  nExpPoints += EPointsFor( COMP->PLAYERX )
  nExpPoints += EPointsFor( COMP->PLAYERY )
  nExpPoints += EPointsFor( COMP->PLAYERZ )

return nExpPoints


FUNCTION EPointsFor( cPlayerID )

  local nEPoints := 0

  if .not. empty( cPlayerID )
     if PLAYER->( dbseek( cPlayerID ) )
        if ( val( PLAYER->CATEGORY ) < 5 ) .and. ( PLAYER->CURRTEAM <> '??' ) .and. ;
           ( PLAYER->ROLE = 'S' )
           if PLAYER->CATEGORY = "4"
             nEPoints += int( PLAYER->SCORES / 4 )
           else
             nEPoints += PLAYER->SCORES
           endif

        endif
     endif
  endif

return nEPoints


function RosterSize
   local nActive := 0
   nActive += IsActive( COMP->PLAYERA )
   nActive += IsActive( COMP->PLAYERB )
   nActive += IsActive( COMP->PLAYERC )
   nActive += IsActive( COMP->PLAYERD )
   nActive += IsActive( COMP->PLAYERE )
   nActive += IsActive( COMP->PLAYERF )
   nActive += IsActive( COMP->PLAYERG )
   nActive += IsActive( COMP->PLAYERH )
   nActive += IsActive( COMP->PLAYERI )
   nActive += IsActive( COMP->PLAYERJ )
   nActive += IsActive( COMP->PLAYERK )
   nActive += IsActive( COMP->PLAYERL )
   nActive += IsActive( COMP->PLAYERM )
   nActive += IsActive( COMP->PLAYERN )
   nActive += IsActive( COMP->PLAYERO )
   nActive += IsActive( COMP->PLAYERP )
   nActive += IsActive( COMP->PLAYERQ )
   nActive += IsActive( COMP->PLAYERR )
   nActive += IsActive( COMP->PLAYERS )
   nActive += IsActive( COMP->PLAYERT )
   nActive += IsActive( COMP->PLAYERU )
   nActive += IsActive( COMP->PLAYERV )
   nActive += IsActive( COMP->PLAYERW )
   nActive += IsActive( COMP->PLAYERX )
   nActive += IsActive( COMP->PLAYERY )
   nActive += IsActive( COMP->PLAYERZ )
return nActive

function IsActive( cPlayerID )
  local nAlive := 0

  if .not. empty( cPlayerID )
     if PLAYER->( dbseek( cPlayerID ) )
        if ( val( PLAYER->CATEGORY ) < 5 ) .and. ( PLAYER->CURRTEAM <> '??' ) .and. ;
           ( PLAYER->ROLE = 'S' )
           nAlive := 1
        endif
     endif
  endif
return nAlive

procedure CLR_COMP
	***
	***   Clear the fields details from the screen
	***

	setcolor( C_BLANK )
	@  3,17 
	@  4,17 
	@  6,17 
	@  8,17

	@ 10,6 say space( 26 )
	@ 11,6 say space( 26 )
	@ 12,6 say space( 26 )
	@ 13,6 say space( 26 )
	@ 14,6 say space( 26 )
	@ 15,6 say space( 26 )
	@ 16,6 say space( 26 )
	@ 17,6 say space( 26 )
	@ 18,6 say space( 26 )
	@ 19,6 say space( 26 )

	@ 10, 33 say space( 26 )
	@ 11, 33 say space( 26 )
	@ 12, 33 say space( 26 )
	@ 13, 33 say space( 26 )
	@ 14, 33 say space( 26 )
	@ 15, 33 say space( 26 )
	@ 16, 33 say space( 26 )
	@ 17, 33 say space( 26 )
	@ 18, 33 say space( 26 )
	@ 19, 33 say space( 26 )

	@ 10, 59 say space( 26 )
	@ 11, 59 say space( 26 )
	@ 12, 59 say space( 26 )
	@ 13, 59 say space( 26 )
	@ 14, 59 say space( 26 )
	@ 15, 59 say space( 26 )
	@ 16, 59 say space( 26 )
	@ 17, 59 say space( 26 )
	@ 18, 59 say space( 26 )
	@ 19, 59 say space( 26 )

RETURN


function GET_COMP
	***
	***   Allows entry of details
	***
	parameters adding

	select COMP
	setcolor( C_FIELDS )

	mseason    = SEASON
	mleagueid  = LEAGUEID
	mownerid   = OWNERID
	mfranchise = FRANCHISE
	mfrancode  = FRANCODE
	mDivision  = DIVISION
	store PLAYERA to mplayera
	store PLAYERB to mplayerb
	store PLAYERC to mplayerc
	store PLAYERD to mplayerd
	store PLAYERE to mplayere
	store PLAYERF to mplayerf
	store PLAYERG to mplayerg
	store PLAYERH to mplayerh
	store PLAYERI to mplayeri
	store PLAYERJ to mplayerj
	store PLAYERK to mplayerk
	store PLAYERL to mplayerl
	store PLAYERM to mplayerm
	store PLAYERN to mplayern
	store PLAYERO to mplayero
	store PLAYERP to mplayerp
	store PLAYERQ to mplayerq
	store PLAYERR to mplayerr
	store PLAYERS to mplayers
	store PLAYERT to mplayert
	store PLAYERU to mplayeru
	store PLAYERV to mplayerv
	store PLAYERW to mplayerw
	store PLAYERX to mplayerx
	store PLAYERY to mplayery
	store PLAYERZ to mplayerz
	store IR1     to cIR1
	store IR2     to cIR2
	store IR3     to cIR3
	store IR4     to cIR4

* 	vget(  3,17, 'mseason',    '',    .f. )
* 	vget(  4,17, 'mleagueid',  '',    .f. )
* 	vget(  6,17, 'mownerid',   '',    .f. )

	vget(  8,17, 'mfranchise', '',    .f. )
	vget(  8,40, 'mfrancode',  '',    .f. )
	vget(  8,45, 'mDivision',  '',    .f. )

	*---Roster

	vget( 10,6, 'mplayera', '@!',    .f. )
	vget( 11,6, 'mplayerb', '@!',    .f. )
	vget( 12,6, 'mplayerc', '@!',    .f. )
	vget( 13,6, 'mplayerd', '@!',    .f. )
	vget( 14,6, 'mplayere', '@!',    .f. )
	vget( 15,6, 'mplayerf', '@!',    .f. )
	vget( 16,6, 'mplayerg', '@!',    .f. )
	vget( 17,6, 'mplayerh', '@!',    .f. )
	vget( 18,6, 'mplayeri', '@!',    .f. )
	vget( 19,6, 'mplayerj', '@!',    .f. )

	vget( 10,33, 'mplayerk', '@!',    .f. )
	vget( 11,33, 'mplayerl', '@!',    .f. )
	vget( 12,33, 'mplayerm', '@!',    .f. )
	vget( 13,33, 'mplayern', '@!',    .f. )
	vget( 14,33, 'mplayero', '@!',    .f. )
	vget( 15,33, 'mplayerp', '@!',    .f. )
	vget( 16,33, 'mplayerq', '@!',    .f. )
	vget( 17,33, 'mplayerr', '@!',    .f. )
	vget( 18,33, 'mplayers', '@!',    .f. )
	vget( 19,33, 'mplayert', '@!',    .f. )

	vget( 10,59, 'mplayeru', '@!',    .f. )
	vget( 11,59, 'mplayerv', '@!',    .f. )
	vget( 12,59, 'mplayerw', '@!',    .f. )
	vget( 13,59, 'mplayerx', '@!',    .f. )
	vget( 14,59, 'mplayery', '@!',    .f. )
	vget( 15,59, 'mplayerz', '@!',    .f. )
	vget( 16,59, 'cIR1',     '@!',    .f. )
	vget( 17,59, 'cIR2',     '@!',    .f. )
	vget( 18,59, 'cIR3',     '@!',    .f. )
	vget( 19,59, 'cIR4',     '@!',    .f. )

	vread( 1, .F., "VAL_COMP" )
	if !abort()
		do REPL_COMP
	endif

RETURN !abort()


function VAL_COMP

	parameter mvar

	private ok
	ok = .t.

	do case

		case mvar == 'mleagueid'

			*---Pop up options
	 		ok = poptable2( @mleagueid, "LEAGUE", "LEAGUEID", "LEAGUE","ID",;
	 		                "   League", "C", 1, .t., 30, .f. )

		case mvar == 'mownerid'

			*---Pop up options
			ok = poptable2( @mownerid, "OWNERS", "OWNERID",;
					 "trim(FIRSTNAME)+' '+LASTNAME","Owner ID",;
	 		                "Owner", "C", 1, .t., 30, .f. )

		case mvar = 'mplayer'

			if .not. empty( &mvar )

				if len( trim( &mvar ) ) = 6
					&mvar = trim( &mvar ) + "01"
				endif

				mfield = "COMP->PLAYER" + right( mvar, 1 )

				select PLAYER
				PLAYER->( dbseek( &mvar ) )
				if eof()
					ok = .f.
					error_msg( "Invalid Code" )
				else
					nRow := row() ; nCol := col()
					if &mvar <> &mfield
						*---check he's not elsewhere
						UsedCheck( mvar, mLeagueId, mSeason, mOwnerId )
					endif
					@ nrow, ncol say PlayerName( PLAYER->PLAYERID ) + ' ' + PLAYER->CURRTEAM +  str(PLAYER->SCORES,3) + PLAYER->ROLE
				endif
			endif

			vread_redraw = .f.

		case mvar = 'cIR'

			if .not. empty( &mvar )

				if len( trim( &mvar ) ) = 6
					&mvar = trim( &mvar ) + "01"
				endif

				mfield = "COMP->IR" + right( mvar, 1 )

				select PLAYER
				PLAYER->( dbseek( &mvar ) )
				if eof()
					ok = .f.
					error_msg( "Invalid Code" )
				else
					nRow := row() ; nCol := col()
					if &mvar <> &mfield
						*---check he's not elsewhere
						UsedCheck( mvar, mLeagueId, mSeason, mOwnerId )
					endif
					@ nrow, ncol say padr( trim( PLAYER->SURNAME ) + ", " + PLAYER->FIRSTNAME, 20 )
				endif
			endif

			vread_redraw = .f.

	endcase

RETURN ok


procedure REPL_COMP
	***
	***   Replace database fields with variables
	***
	select COMP
	replace SEASON    with mseason
	replace LEAGUEID  with mleagueid
	replace OWNERID   with mownerid
	replace FRANCHISE with mfranchise
	replace FRANCODE  with mfrancode 
	replace DIVISION  with mDivision
	replace PLAYERA   with mplayera
	replace PLAYERB   with mplayerb
	replace PLAYERC   with mplayerc
	replace PLAYERD   with mplayerd
	replace PLAYERE   with mplayere
	replace PLAYERF   with mplayerf
	replace PLAYERG   with mplayerg
	replace PLAYERH   with mplayerh
	replace PLAYERI   with mplayeri
	replace PLAYERJ   with mplayerj
	replace PLAYERK   with mplayerk
	replace PLAYERL   with mplayerl
	replace PLAYERM   with mplayerm
	replace PLAYERN   with mplayern
	replace PLAYERO   with mplayero
	replace PLAYERP   with mplayerp
	replace PLAYERQ   with mplayerq
	replace PLAYERR   with mplayerr
	replace PLAYERS   with mplayers
	replace PLAYERT   with mplayert
	replace PLAYERU   with mplayeru
	replace PLAYERV   with mplayerv
	replace PLAYERW   with mplayerw
	replace PLAYERX   with mplayerx
	replace PLAYERY   with mplayery
	replace PLAYERZ   with mplayerz
	replace IR1       with cIR1
	replace IR2       with cIR2
	replace IR3       with cIR3
	replace IR4       with cIR4

	SortRoster()

	commit

RETURN


FUNCTION UsedCheck( cMemVar, cLeague, cSeason, cOwner )

	local nOldRec

	nOldRec := COMP->( recno() )
	if COMP->( dbSeek( cSeason + cLeague ) )
		do while .not. COMP->( eof() ) .and. COMP->SEASON   = cSeason;
												 .and. COMP->LEAGUEID = cLeague
			
			if CheckRost( &cMemVar )
				ErrorMsg( 'Player used by ' + trim( COMP->FRANCHISE ) )
				exit
			endif
			COMP->( dbSkip( 1 ) )
		enddo
	endif

	
RETURN( COMP->( dbGoto( nOldrec ) ) )


FUNCTION CheckRost( cPlayerId )

	local lWarn := .f.

	do case
	case COMP->PLAYERA = cPlayerId
		lWarn := .t.
	case COMP->PLAYERB = cPlayerId
		lWarn := .t.
	case COMP->PLAYERC = cPlayerId
		lWarn := .t.
	case COMP->PLAYERD = cPlayerId
		lWarn := .t.
	case COMP->PLAYERE = cPlayerId
		lWarn := .t.
	case COMP->PLAYERF = cPlayerId
		lWarn := .t.
	case COMP->PLAYERG = cPlayerId
		lWarn := .t.
	case COMP->PLAYERH = cPlayerId
		lWarn := .t.
	case COMP->PLAYERI = cPlayerId
		lWarn := .t.
	case COMP->PLAYERJ = cPlayerId
		lWarn := .t.
	case COMP->PLAYERK = cPlayerId
		lWarn := .t.
	case COMP->PLAYERL = cPlayerId
		lWarn := .t.
	case COMP->PLAYERM = cPlayerId
		lWarn := .t.
	case COMP->PLAYERN = cPlayerId
		lWarn := .t.
	case COMP->PLAYERO = cPlayerId
		lWarn := .t.
	case COMP->PLAYERP = cPlayerId
		lWarn := .t.
	case COMP->PLAYERQ = cPlayerId
		lWarn := .t.
	case COMP->PLAYERR = cPlayerId
		lWarn := .t.
	case COMP->PLAYERS = cPlayerId
		lWarn := .t.
	case COMP->PLAYERT = cPlayerId
		lWarn := .t.
	case COMP->PLAYERU = cPlayerId
		lWarn := .t.
	case COMP->PLAYERV = cPlayerId
		lWarn := .t.
	case COMP->PLAYERW = cPlayerId
		lWarn := .t.
	case COMP->PLAYERX = cPlayerId
		lWarn := .t.
	case COMP->PLAYERY = cPlayerId
		lWarn := .t.
	case COMP->PLAYERZ = cPlayerId
		lWarn := .t.
	case COMP->IR1     = cPlayerId
		lWarn := .t.
	case COMP->IR2     = cPlayerId
		lWarn := .t.
	case COMP->IR3     = cPlayerId
		lWarn := .t.
	case COMP->IR4     = cPlayerId
		lWarn := .t.

	endcase

RETURN lWarn


STATIC FUNCTION SortRoster

	local aRoster[ 20, 2 ], mField, i

	*---Load rostr
	for i = 1 to 20
		mField := "COMP->PLAYER" + chr( 64 + i )
		aRoster[ i, 1 ] = &mField

		if empty( aRoster[ i, 1 ] )
			aRoster[ i, 2 ] := 9
		else
			aRoster[ i, 2 ] := val( PlayerPos( aRoster[ i, 1 ] ) )
		endif

	next

	aRoster := asort( aRoster,,, { |x,y| x[2] < y[2] } )

	for i = 1 to 20
		mField := "COMP->PLAYER" + chr( 64 + i )
		replace &mField with aRoster[ i, 1 ]
	next

RETURN nil


FUNCTION IsStarter( cPlayerId, cWeek, cTeam )

	local lIsStarter := .f.

	if LINEUP->( Seeker( mSeason + mLeagueId + cTeam + cWeek ) )

		if LINEUP->PLAYER1 = cPlayerId
			lIsStarter := .t.
		elseif LINEUP->PLAYER2 = cPlayerId
			lIsStarter := .t.
		elseif LINEUP->PLAYER3 = cPlayerId
			lIsStarter := .t.
		elseif LINEUP->PLAYER4 = cPlayerId
			lIsStarter := .t.
		elseif LINEUP->PLAYER5 = cPlayerId
			lIsStarter := .t.
		elseif LINEUP->PLAYER6 = cPlayerId
			lIsStarter := .t.
		elseif LINEUP->PLAYER7 = cPlayerId
			lIsStarter := .t.
		elseif LINEUP->PLAYER8 = cPlayerId
			lIsStarter := .t.
		elseif LINEUP->PLAYER9 = cPlayerId
			lIsStarter := .t.
		elseif LINEUP->PLAYER10= cPlayerId
			lIsStarter := .t.
		elseif LINEUP->PLAYER11= cPlayerId
			lIsStarter := .t.
		elseif LINEUP->PLAYER12= cPlayerId
			lIsStarter := .t.
		endif
	endif

RETURN lIsStarter

FUNCTION Owner( mOwnerid )

	private oldarea, ret_val

	oldarea = select()

	select OWNERS
	OWNERS->( dbseek( mOwnerid ) )

	if found()
		ret_val = trim( OWNERS->FIRSTNAME ) + " " + OWNERS->LASTNAME

	else
		ret_val = space(20)

	endif

	select( oldarea )

RETURN ret_val
	


*EOF:  COMPS.PRE

