*****************************************************************************
*  Program Name.....: TYCOON.PRG
*  Programmer.......: SC
*  System...........: Tycoon Fantasy Leagues Computer system
*  Module...........: TYCOON Module
*  Date.............: 14/4/1990 at 11:23
*  Copyright........: (c) 1990 by Tycoon Gridiron
*  Compile/link.....: 
*                     	TYCLIP
*  Purpose..........:
*
*	  To control the updating of TYCOON information.
*
*............................................................................
*  Revision.........: 1.00 Last Revised: 14/4/1990 at 11:22
*  Description......: Initial Creation.
*.............................................................................
***************************** ALL RIGHTS RESERVED ***************************

#INCLUDE "TYCOON.H"

PROCEDURE TYCOON

parameters G_path		  

private continu

gsys_test = .f.
continu = .t.
clear_scn()

*---Check that we got the path passed
if pcount() < 1
	*---No parameter passed so default
   g_path = '..\'		&&		parent directory
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
*G_version = "v 2.00"	 	&&  14-APR-90
*G_version = "v 2.01"	 	&&  13-SEP-03
G_version = "v 2.03"	 	&&  13-SEP-03
G_username = "STEVE"
G_shrtnme  = "TYKE"
G_modname  = "TYKE"

*---Log Module Useage
do LOG_USE with "IN ", G_username, G_shrtnme, G_version

DD_HEAD( G_sysowner, G_modname, G_version )

*---Open files then Put the menu on screen

do TYMENU

*StoreGweek( G_week )

shutdown()

RETURN




*EOF:  TYCOON.PRE
