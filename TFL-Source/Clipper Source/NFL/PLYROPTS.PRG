*****************************************************************************
*  Program Name.....: plyropts.pre 
*  Programmer.......: Steve Colonna 
*  System...........:
*  Module...........:
*  Date.............: 29/4/1990 at 19:53
*  Copyright........: (c) 1990 by Knoware 
*  Called From......:
*  Purpose..........:
*
*............................................................................
*  Revision.........: 1.0 Last Revised: 29/4/1990 at 19:53
*  Description......: Initial Creation.
*............................................................................
***************************** ALL RIGHTS RESERVED ***************************

#INCLUDE "nfl.h"

PROCEDURE PLYROPTS

   private nmess, poption1
   private mopt[ 6], mmess[ 6]

	if INIT_OPTS()    && Initialisation routine
		poption1 = 1
	else
      retu
   endif

   do while poption1 <> 0 
   
      poption1 = flatmenu (mopt, mmess, poption1, 22, .T.)
      poption1 = if( poption1 = 0, 1, poption1)
   
      setcolor( C_BLANK )
      clrmenu()  

      do case

         case mopt[poption1] = 'X'

				CLEAR_WINDOW( 22, 0, 24, 79 )
            poption1 = 0

         case mopt[poption1] = 'T'

				do PLYRTEAM with mplayerid

				*---May have changed jerseys
				select PLAYER
				do RLOCKIT with "Player file"
				replace CURRTEAM with CurrTeam( PLAYER->PLAYERID, date() )
*				TestMsg( "Team = " + PLAYER->CURRTEAM )
				if PLAYER->ROOKIEYR = 0
					replace ROOKIEYR with FirstYr( PLAYER->PLAYERID )
				endif
				unlock

			case mopt[poption1] = 'H'

				unavail()

         case mopt[poption1] = 'I'

				unavail()
            
         case mopt[poption1] = 'B'

				set cursor on
				do Edit_memo with "PLAYER->BIO", "Comments", .t., 11, 37
				set cursor off
            
         case mopt[poption1] = 'S'

				do PSTATS
            
      endcase
   
   enddo 

RETURN

FUNCTION FirstYr( cPlayerID )

	local nYear := 0

	if SERVE->( dbseek( cPlayerID ) )
		nYear := year( SERVE->FROM )
	endif

RETURN nYear


function INIT_OPTS
   ***
   ***   Initialise all variables, arrays and open files
   ***

   mopt[1]  = 'X-exit'    
   mopt[2]  = 'Teams'      
   mopt[3]  = 'Honours'      
   mopt[4]  = 'Injuries'       
   mopt[5]  = 'Biography'       
   mopt[6]  = 'Stats'       

   mmess[1] = 'Return to Player Menu'           
   mmess[2] = 'Show Teams this player has played for'
   mmess[3] = 'Show Honours this player has received'
   mmess[4] = 'Show injuries sustained by this player'
   mmess[5] = 'Show this players pen picture'
   mmess[6] = 'Show this players Stats'

	CLEAR_WINDOW( 22, 0, 24, 79 )

return( .t. )



*EOF:  PLYROPTS.PRE