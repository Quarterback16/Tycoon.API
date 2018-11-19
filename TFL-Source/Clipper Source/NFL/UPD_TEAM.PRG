*****************************************************************************
*  Program Name.....: NFL.PRG
*  Programmer.......: SC
*  System...........: Tycoon Fantasy Leagues Computer system
*  Module...........: NFL Module
*  Date.............: 14/4/1990 at 11:23
*  Copyright........: (c) 1990 by Tycoon Gridiron
*  Compile/link.....: 
*                     	NFLCLIP
*  Purpose..........:
*
*	  To control the updating of NFL information.
*
*
*
*............................................................................
*  Revision.........: 1.00 Last Revised: 14/4/1990 at 11:22
*  Description......: Initial Creation.
*.............................................................................
***************************** ALL RIGHTS RESERVED ***************************

#INCLUDE "NFL.H"

parameters G_path		  

private continu

gsys_test = .f.
continu = .t.
clear_scn()

*---Check that we got the path passed
if pcount() < 1
	*---No parameter passed so default directory
   g_path = '\TFL\'		&&		default directory
endif			  
stdcolor( G_path )

set_env()

*--------------------------- Initialise Publics ---------------------------*
***   
***   A standard system initialisation function in used to set up the module
***   from the System Control file.
***   
if .not. sys_info( G_path )
	error_msg("Initialisation Failure, Aborting System")
	shutdown(.f.)
endif

*---Semi publics
G_version = "v 1.00"	 	&&  14-APR-90
G_username = "STEVE"
G_shrtnme  = "NFL"
G_modname  = "NFL"

*---Log Module Useage
do LOG_USE with "IN ", G_username, G_shrtnme, G_version

DD_HEAD( G_sysowner, G_modname, G_version )

*---Open files then Put the menu on screen

if opendbf( '*', G_nfl_path )

	select PLAYER
	go top
	do while .not. eof()
		do RLOCKIT with "Player file"
		replace CURRTEAM with currteam( PLAYER->PLAYERID, date() )
		commit 
		skip
	enddo


endif

shutdown()

return


*EOF:  NFL.PRE

