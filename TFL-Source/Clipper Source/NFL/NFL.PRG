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
*............................................................................
*  Revision.........: 1.00 Last Revised: 14/4/1990 at 11:22
*  Description......: Initial Creation.
*.............................................................................
*  Revision.........: 1.01 Last Revised: 04/01/92 at 12:59:34
*  Description......: Added Standings Program
*.............................................................................
***************************** ALL RIGHTS RESERVED ***************************

#INCLUDE "NFL.H"

PROCEDURE NFL

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
stdcolor( '\PUBLIC\' )

set_env()
set epoch to 2000

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
*G_version = "v 1.00"	&&  14-APR-90
*G_version = "v 1.01"	&&   4-JAN-92
*G_version = "v 1.02"	&&  16-SEP-93
*G_version = "v 1.03"	&&  24-OCT-93
*G_version = "v 1.04"	&&  14-SEP-94
*G_version = "v 1.05"	&&   6-FEB-2000
*G_version = "v 2.00"	 	&&   5-DEC-2005
*G_version = "v 2.01"	 	&&   6-DEC-2005  fixed Standings file name
*G_version = "v 2.09"	 	&&  12-MAY-2015
G_version = "v 2.10"	 	&&  17-SEP-2015  -- VICTIM bug in TFL.LIB, dbseek bug, LIB is not being updated!!

G_username = "STEVE"
G_shrtnme  = "NFL"
G_modname  = "NFL"

*---Log Module Useage
do LOG_USE with "IN ", G_username, G_shrtnme, G_version

DD_HEAD( G_sysowner, G_modname, G_version )

*---Open files then Put the menu on screen

*TestMsg('Opening all in ' + G_nfl_path )
do LOG with "Opening all in " + G_nfl_path 

///
///  Vesuvius for some unknown reason would not open a DBF in this fashion
///
///  moving all open and closes back to the individual code modules
///  alows Vesuvius to be the one true source
///
*if opendbf( "PLAYER",  G_nfl_path, .t. )
*if opendbf( "TEAM",    G_nfl_path, .t. )
*if opendbf( "CATGRY",  G_nfl_path, .t. )
*if opendbf( "SERVE",   G_nfl_path, .t. )
*if opendbf( "SCHED",   G_nfl_path, .t. )
*if opendbf( "STAT",    G_nfl_path, .t. )
*if opendbf( "STTYPE",  G_nfl_path, .t. )
*if opendbf( "SCTYPE",  G_nfl_path, .t. )
*if opendbf( "SCORE",   G_nfl_path, .t. )
*if opendbf( "STADIUM", G_nfl_path, .t. )
*if opendbf( "PICK",   G_nfl_path, .t. )
*if opendbf( "TOTTIP", G_nfl_path, .t. )
**if opendbf( "SEASON", G_nfl_path, .t. )
*   if opendbf( '*', G_nfl_path, .t. )

      do NFLMENU

	   StoreGWeek( g_week )
*   endif
*endif
*endif
*endif
*endif
*endif
*endif
*endif
*endif
*endif
*endif
*endif
*endif
*endif

shutdown()

RETURN


*EOF:  NFL.PRE