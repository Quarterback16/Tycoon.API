*****************************************************************************
*  Program Name.....: ARCHIVE.PRG
*  Programmer.......: SC
*  System...........: Tycoon Fantasy Leagues Computer system
*  Module...........: NFL Module
*  Date.............: 14/4/1990 at 11:23
*  Copyright........: (c) 1990 by Tycoon Gridiron
*  Purpose..........:
*
*	  To move a seasons data to a different directory for archival
*
*............................................................................


PROCEDURE Archive

	local cArchYear, cArchDir
	
	*---Enter year to Archive
	cArchYear := GetAlph( "Enter year to archive", 4 )

	cArchDir  := G_nfl_path + cArchYear + "\"

	TestMsg( 'Archiving to ' + cArchDir )

	*---Sched & SCHED & Stat
	select SCHED
	copy to ( cArchDir + "SCHED" ) for SCHED->SEASON == cArchYear
	delete for SCHED->SEASON == cArchYear

	select SCORE
	copy to ( cArchDir + "SCORE" ) for SCORE->SEASON == cArchYear
	delete for SCORE->SEASON == cArchYear

	select STAT
	copy to ( cArchDir + "STAT" ) for STAT->SEASON == cArchYear
	delete for STAT->SEASON == cArchYear

RETURN


*EOF:  ARCHIVE.PRG
