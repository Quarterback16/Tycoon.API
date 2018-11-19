*****************************************************************************
*  Program Name.....: player.pre 
*  Programmer.......: Steve Colonna 
*  System...........:
*  Module...........:
*  Date.............: 15/4/1990 at 11:10
*  Copyright........: (c) 1990 by Knoware 
*  Called From......:
*  Purpose..........:
*
*............................................................................
*  Revision.........: 2.0 Last Revised: 18/7/2000 at 11:10
*  Description......: Added Injury rating.
*............................................................................
*  Revision.........: 1.0 Last Revised: 15/4/1990 at 11:10
*  Description......: Initial Creation.
*............................................................................
***************************** ALL RIGHTS RESERVED ***************************

#INCLUDE "nfl.h"

#DEFINE C_FIELDS c_fields
#DEFINE C_HELP   c_help  
#DEFINE C_BLANK  c_blank

PROCEDURE PLAYER

   private nmess, poption1, mopt[10], mmess[10], mplayerid

   if !INIT_PLYR( mOpt, mMess )    && Initialisation routine
      do CLOSE_PLYR 
      RETU
   endif

   do DISP_PLYR    && Display Screen
   poption1 = 3

   do while poption1 <> 0 
   
      do CLR_PLYR
      do FLDS_PLYR 

      poption1 = flatmenu (mopt, mmess, poption1, 22, .T.)
      poption1 = if( poption1 = 0, 2, poption1)
   
      clrmenu()  

      do case
         case mopt[poption1] = 'P'

            up_skip( -1 )

         case mopt[poption1] = 'X'

            poption1 = 0

         case mopt[poption1] = 'B'

            do SRCH_PLYR

         case mopt[poption1] = 'I'

            do FIND_PLYR 

         case mopt[poption1] = 'A'

            do ADD_PLYR  

         case mopt[poption1] = 'E'

            do EDIT_PLYR 

         case mopt[poption1] = 'D'

            if caned_PLYR()
               up_del()
            endif

         case mopt[poption1] = 'N'

            up_skip( 1 )

         case mopt[poption1] = 'O'

            do PLYROPTS

         case mopt[poption1] = 'L'

            select PLAYER
            up_list( 'NFL PLAYERS', "space( 4 )+ PLAYERID +space(3)+ SURNAME" )

      endcase
   
   enddo 

   do CLOSE_PLYR 

return  && from C:\NFL\PLAYER.PRG

*===========================================================================*

function INIT_PLYR 
   ***
   ***   Initialise all variables, arrays and open files
   ***
	parameters mopt, mmess

   mopt[1]  = 'Previous'  
   mopt[2]  = 'X-exit'    
   mopt[3]  = 'Browse'      
   mopt[4]  = 'Inquire'      
   mopt[5]  = 'Add'       
   mopt[6]  = 'Edit'      
   mopt[7]  = 'Delete'    
   mopt[8]  = 'List'      
   mopt[9]  = 'Options'      
   mopt[10] = 'Next'      

   mmess[1] = 'View Previous Record'                
   mmess[2] = 'Return to Main Menu'                 
   mmess[3] = 'Browse Through All Records in the File'                    
   mmess[4] = 'Locate a Record'                 
   mmess[5] = 'Add A New Record'                    
   mmess[6] = 'Amend Details Displayed Currently'   
   mmess[7] = 'Delete Record Displayed Currently'   
   mmess[8] = 'List all the Records to the printer' 
   mmess[9] = 'Further options'                    
   mmess[10]= 'View Next Record'                    

   
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
	OpenDbf( 'COMP', g_ty_path )

	if empty( G_player )
		select PLAYER
		set order to 1
		go lastrec()	 	 && start at last record entered
	else
		( PLAYER->( dbSeek( G_player ) ) )
	endif

   set century on

RETURN .t.


procedure CLOSE_PLYR 
   ***
   ***   Closing Down 
   ***

   	CloseDbf( 'CATGRY' )
	CloseDbf( 'COMP' )
	CloseDbf( 'PLAYER' )
	CloseDbf( 'SCORE' )
	CloseDbf( 'STAT' )
	CloseDbf( 'SERVE' )
	CloseDbf( 'TEAM' )	
	
    set century off

RETURN


procedure FIND_PLYR 
   ***
   ***     Allows search for a particular record
   ***
   private  dkey, crec        

   select PLAYER
   do CLR_PLYR   
   setcolor( C_FIELDS )
   dkey = space(8)
   up_find( "PLAYERID", 4,16 ) 

RETURN

                               
procedure ADD_PLYR 
   ***
   ***   Allows the addition of a record
   ***

   select PLAYER
   do CLR_PLYR

	setcolor( C_FIELDS )
	if add_rec(5)
		*---Defaults
		replace CATEGORY  with "6"
		replace CURRATING with 1
		replace HEIGHT_FT with 6
		replace ROOKIEYR  with val( G_season )

		if !GET_PLYR( .t. )  && returns abort()
			delete
		endif

		unlock
	endif


RETURN  


procedure EDIT_PLYR 
   ***
   ***   Allows amendments of fields displayed on the screen
   ***

   select PLAYER
   setcolor( C_FIELDS )
   if !caned_PLYR()
      RETU
   endif

   if rec_lock(5)
      GET_PLYR( .f. )  && returns abort()
      unlock
      commit
   else
      warning('rec_lock')
   endif

RETURN


function CANED_PLYR
   ***
   *** types of records that can be edited
   ***

return( up_canedit() )
     

procedure SRCH_PLYR 
   ***
   ***   Calls a routine to display a list of records in a file and
   ***   allows for selection of one of those.
   ***

   private s_fields[3], s_mess[3]

   s_fields[1] = "trim(SURNAME)+', '+FIRSTNAME"
   s_fields[2] = "POSDESC"
   s_fields[3] = "PLAYERID"

   s_mess[1]   = "Name"
   s_mess[1]   = "Pos"
   s_mess[2]   = "Code"

   set order to 2   &&  Description
   setcolor( C_HELP )
   TBL_SRCH( s_fields, s_mess )
   set order to 1

return   &&   from SRCH_PLYR
     
          
procedure DISP_PLYR 
   ***
   ***   Displays initial screen details
   ***

   setcolor( C_BLANK )
   clear_scn()
   setcolor( c_desc )

   @  4, 4 say "Player Id :"
   @  6, 4 say "Name      :"
   @  8, 4 say "Position  :"
   @ 10, 4 say "Rating    :"
   @ 11, 4 say "Projected :"
   @ 10,25 say "Role      :"
   @ 12, 4 say "DOB       :"
   @ 13, 4 say "Height    :"
   @ 14, 4 say "Weight    :"
   @ 15, 4 say "Speed     :"
	@ 16, 4 say "College   :"
	@ 17, 4 say "Rookie Yr :"
	@ 19, 4 say "Scores    :"

   head_line('NFL PLAYERS')

   ScreenLine(  1 )
   ScreenLine( 21 )

RETURN

                                                                               
procedure FLDS_PLYR 
   ***
   ***   Displays the fields on the screen
   ***
   setcolor( C_FIELDS )
   select PLAYER
   if !up_can()
      RETU
   endif

	setcolor( C_FIELDS )
	mplayerid := PLAYER->PLAYERID
	G_player  := PLAYER->PLAYERID
   @  4,16 say PLAYER->PLAYERID
   @  4,45 say PlaysFor( PLAYER->PLAYERID, "G1", G_Season )
   @  4,54 say PlaysFor( PLAYER->PLAYERID, "TN", G_Season )
   @  4,63 say PlaysFor( PLAYER->PLAYERID, "YH", G_Season )
   @  4,72 say PlaysFor( PLAYER->PLAYERID, "RR", G_Season )
	@  6,16 say trim( PLAYER->SURNAME ) + ' ' + PLAYER->FIRSTNAME
	@  7,45 say teamstr( currteam( mplayerid, date() ) )
   @  8,16 say PLAYER->CATEGORY
   @  8,37 say PLAYER->POSDESC
   @ 10,37 say RoleOf( PLAYER->ROLE )
   @ 10,16 say PLAYER->CURRATING
   @ 10,29 say "ir"+str(PLAYER->INJURY+iif(PLAYER->ROLE='I', 1, 0 ), 2)
   @ 11,16 say PLAYER->PROJECTED
	@ 12,16 say PLAYER->DOB
	@ 12,29 say age( PLAYER->DOB )
   @ 13,16 say PLAYER->HEIGHT_FT
   @ 13,18 say PLAYER->HEIGHT_IN
   @ 14,16 say PLAYER->WEIGHT
   @ 15,16 say PLAYER->FORTY
   @ 16,16 say PLAYER->COLLEGE
	@ 17,16 say PLAYER->ROOKIEYR
	@ 17,22 say substr( Drafted( PLAYER->PLAYERID ), 1, 3 )
	@ 17,26 say nYears()
	@ 19,16 say str( PLAYER->CURSCORES, 3 ) + ' - ' + str( PLAYER->SCORES, 3 ) + AvgScores()
	@ 12,40 say memoline( PLAYER->BIO, 39, 1, 3 )
	@ 13,40 say memoline( PLAYER->BIO, 39, 2, 3 )
	@ 14,40 say memoline( PLAYER->BIO, 39, 3, 3 )
	@ 15,40 say memoline( PLAYER->BIO, 39, 4, 3 )
	@ 16,40 say memoline( PLAYER->BIO, 39, 5, 3 )
	@ 17,40 say memoline( PLAYER->BIO, 39, 6, 3 )
	@ 18,40 say memoline( PLAYER->BIO, 39, 7, 3 )
	@ 19,40 say memoline( PLAYER->BIO, 39, 8, 3 )

RETURN

function nYears
   local nYrs, cYrsOut

   if PLAYER->ROOKIEYR = 0
      cYrsOut := space(4)
   else
      nYrs := CareerSpan()
      cYrsOut := '(' + ltrim( str( nYrs, 2 ) ) + ')'
   endif
return cYrsOut


function AvgScores

   local nYrs, cAvgOut

   if PLAYER->ROOKIEYR == val( G_Season )
      cAvgOut := space(6)
   else
      if PLAYER->ROOKIEYR = 0
         cAvgOut := space(6)
      else
         nYrs := CareerSpan()
         if PLAYER->SCORES > 0
            cAvgOut := str( PLAYER->SCORES / nYrs, 6, 1 )
         else
            cAvgOut := '   0.0'
         endif
      endif
   endif

return cAvgOut


function CareerSpan

  local nSpan

  if PLAYER->CURRTEAM == "??"
     *--Assume retired
     nLastYear := val( G_Season )
     SERVE->( dbseek( PLAYER->PLAYERID ) )
     do while .not. SERVE->(eof()) .and. SERVE->PLAYERID == PLAYER->PLAYERID
        nLastYear := year( SERVE->to )
        SERVE->( dbskip() )
     enddo
     nSpan := nLastYear - PLAYER->ROOKIEYR
  else
     nSpan := val( G_Season ) - PLAYER->ROOKIEYR
  endif

Return nSpan


procedure CLR_PLYR
   ***
   ***   Clear the fields details from the screen
   ***

   setcolor( C_BLANK )
   @  4,16
   @  6,16 
   @  7,45
   @  8,16
   @ 10,16
   @ 11,16
   @ 12,16
   @ 13,16
   @ 14,16
   @ 15,16
   @ 16,16
   @ 17,16
   @ 18,16
   @ 19,16

RETURN


function GET_PLYR
	***
	***   Allows entry of details
	***

	parameter adding

	local lFlag

	private msurname, mfirstname, mcategory, mposdesc, mcurrating, oplayerid
	private mdob, mheight_ft, mheight_in, mweight, mforty, mcollege, nRookieYr
	private cRole, mProjected

	select PLAYER
	setcolor( C_FIELDS )

	mplayerid  = PLAYERID
	oplayerid  = PLAYERID
	msurname   = SURNAME
	mfirstname = FIRSTNAME
	mcategory  = CATEGORY
	mposdesc   = POSDESC
	cRole		  = ROLE
	mcurrating = CURRATING
	mProjected = PROJECTED
	mdob       = DOB
	mheight_ft = HEIGHT_FT
	mheight_in = HEIGHT_IN
	mweight    = WEIGHT
	mforty     = FORTY
	mcollege   = COLLEGE
	nRookieYr  = ROOKIEYR

	vget(  6,16, 'msurname',   '', .f. )
	vget(  6,37, 'mfirstname', '', .f. )
	vget(  8,16, 'mcategory',  '', .f. )
	vget(  8,37, 'mposdesc',   '!!!!!!!!!!!!!!!', .f. )
	vget( 10,16, 'mcurrating', '', .f. )
	vget( 10,37, 'cRole',     '!', .f. )
	vget( 11,16, 'mProjected', '', .f. )
	vget( 12,16, 'mdob',       '', .f. )
	vget( 13,16, 'mheight_ft', '', .f. )
	vget( 13,18, 'mheight_in', '', .f. )
	vget( 14,16, 'mweight',    '', .f. )
	vget( 15,16, 'mforty',     '', .f. )
	vget( 16,16, 'mcollege',   '', .f. )
	vget( 17,16, 'nRookieYr',  '9999', .f. )

	vread( 1, .F., "VAL_PLYR" )
	if !abort()
		if substr( mPlayerId, 1, 6 ) <> substr( oPlayerID, 1, 6 )
			*---Key changed
			TestMsg( "Key changed" )
			lFlag := .t.
		else
			lFlag := adding
		endif
		
		mPlayerId = NewId( mSurname, mFirstname, adding )
		if .not. adding
			*---Check change of ID
			if substr( mPlayerId, 1, 6 ) <> substr( oPlayerID, 1, 6 )
				*---change all occurences (referential integrity)
				Testmsg( 'New Id ' + mPlayerId + ' old Id ' + oPlayerId )
				ChangeID( 'SERVE', 1, 'PLAYERID',  oPlayerId, mPlayerId )
				ChangeID( 'STAT',  2, 'PLAYERID',  oPlayerId, mPlayerId )
				ChangeID( 'SCORE', 2, 'PLAYERID1', oPlayerId, mPlayerId )
				ChangeID( 'SCORE', 3, 'PLAYERID2', oPlayerId, mPlayerId )
				ChangeTyke( oPlayerId, mPlayerId )
			else
				mPlayerId = oPlayerID
			endif

		endif

		do REPL_PLYR
	endif

return( !abort() )


FUNCTION ChangeID( cAlias, nNtxOrder, cFldName, cOldID, cNewId )

	local nOldArea, nOldNtx

	nOldArea = select()

	select &cAlias
	nOldNtx = indexord()
	set order to nNtxOrder

	do while .t.
		dbseek( cOldId )
		if eof()
			exit
		else
			do RLOCKIT with 'Child file' 
			replace &cFldName with cNewId
		endif
	enddo

	set order to nOldNtx

	select( nOldArea )

RETURN nil


FUNCTION ChangeTyke( cOldID, cNewId )

	local i, cPrfx

*	select 0
*	if opendbf( 'COMP', G_ty_path )
		select 0
		if OpenDbf( 'LINEUP', G_ty_path )
			select COMP
			do while .not. eof()
				for i = 65 to 84
					cPrfx = "PLAYER" + chr( i )
					if COMP->&cPrfx = cOldId
						do RLOCKIT with "Competition file"
						replace COMP->&cPrfx with cNewId
					endif
				next
				skip
			enddo

			select LINEUP
			do while .not. eof()
				for i = 1 to 11
					cPrfx = "PLAYER" + ltrim( str( i, 2 ) )
					if LINEUP->&cPrfx = cOldId
						do RLOCKIT with "Line Up file"
						replace LINEUP->&cPrfx with cNewId
					endif
				next
				skip
			enddo

		endif
		CloseDbf( "LINEUP" )
*		CloseDbf( "COMP" )
*	else
*		CloseDbf( "COMP" )
*	endif

RETURN nil


function VAL_PLYR 

   parameter mvar
   priv ok
   ok = .t.

   do case

      case mvar == 'mcategory'

			ok = poptable2( @mcategory, "CATGRY", "CATEGORY", "CATNAME",;
								"Category", "   Description","C", 1, .f., 35, .f. )
	 		if adding
				mposdesc = CATGRY->DEF_DESC
			endif

		case mVar == 'nRookieYr'

			if nRookieYr < 20 .and. .not. empty( nRookieYr )
				nRookieYr = val( G_season ) - nRookieYr + 1
			endif

   endcase

return( ok )              


PROCEDURE Repl_Plyr 
   ***
   ***   Replace database fields with variables
   ***
   select PLAYER
   replace PLAYERID  with mplayerid
   replace SURNAME   with msurname
   replace FIRSTNAME with mfirstname
   replace CATEGORY  with mcategory
   replace POSDESC   with mposdesc
   replace ROLE      with cRole
   replace CURRATING with mcurrating
   replace PROJECTED with mProjected
	replace CURRTEAM  with CurrTeam( mPlayerId, date() )
*	TestMsg( "CURRTEAM set to " + PLAYER->CURRTEAM )
   replace DOB       with mdob
   replace HEIGHT_FT with mheight_ft
   replace HEIGHT_IN with mheight_in
   replace WEIGHT    with mweight
   replace FORTY     with mforty
	replace COLLEGE   with mcollege
	replace ROOKIEYR  with nRookieYr
	commit 

RETURN


FUNCTION Newid( cSurname, cFirstname, lAdding )

	***
	***	Works out the Players ID  SSSSII00
	***

	local oldrec, cnt, cPlayerId, nOldOrder

	cnt := if( lAdding, 1, 0 )

	cPlayerid = upper( left( cSurname, 4 ) + left( cFirstname, 2 ))

	select PLAYER
	oldrec = recno()
	nOldOrder = indexord()
	set order to 1
	dbseek( cPlayerid )

	do while .not. eof() .and.;
			cPlayerid = upper( left( PLAYER->SURNAME, 4 ) +;
								    left( PLAYER->FIRSTNAME, 2 ) )
		cnt++
		skip
	enddo
	cnt := if( cnt = 0, 1, cnt )

	go oldrec
	set order to nOldOrder
	cPlayerid += strzero( cnt, 2 )

RETURN cPlayerId


function AGE

	parameters dobin

	private _age

	_age = int( ( date() - dobin ) / 365 )

return( str( _age, 2 ) )




*EOF:  PLAYER.PRE 

