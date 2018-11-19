****************************************************************************
*
*  Load a Internet Box scores into the TFL/NFL system
*
*		1)		Load box files into an Array
*		2)		For each box score    doBox()
*					Read a stripped line
*					Identify Section
*					Identify line type
*					Process line
*
*
*	Start:	8-SEP-95			2100 - 2250			1:50
*				9-SEP-95			2030 - 2310			2:40		 4:30
*			  10-SEP-95			1105 - 1200			 :55		 5:25
*			  10-SEP-95			1300 - 1605			3:05		 8:30
*			  10-SEP-95			2015 - 2215			2:00		10:30
*			  11-SEP-95			2055 - 2300			2:05 		12:35
*			  12-SEP-95			2100 - 2125			 :25		13:00
*			  19-SEP-95			2045 - 2145			1:00		14:00
*
*  Mods:	  NOV-96  ESPN changed the format of their box scores, switch
*                  to Mercury Mail's SportsWrap format.
*
*          SEP-98  Infobeat seemed to switch back to old format
*
****************************************************************************

#INCLUDE "NFL.H"

#define		MAX_BOX_SCORES		15

*#define     DEBUGGING

*static cBoxDir	:= "c:\tfl\html\news\"	//		Location of Box scores
static cBoxDir		:= "D:\tfl\nfl\box\"		//		Location of Box scores
static nHandle
static cLogFile	:= "LoadBoxs"				//		Log file
static cSection	:= "TOP"						//		Current Section
static cLineType	:= "NO-OP"					//		Line type
static cGameCode	:= ""							//		TFL game code eg 199501A
static cQuarter   := ""                   //    Which quarter are we in?
static aWords		:= {}							//		Words in the line
static aInts		:= {}							//		Interceptions
static aPlyr		:= {}							//		Player Names
static aBox[ MAX_BOX_SCORES ]
static cWeek
static lSwapped   := .f.
static cAwayAP
static cHomeAP
static cAwayAPname
static cHomeAPname
static cAwayID
static cHomeID
static nAwayScore
static nHomeScore
static cTime
static nHomeSacks	:= 0
static nAwaySacks	:= 0
static nHSackCnt := 0
static nASackCnt := 0
static nHomeInts := 0
static nAwayInts := 0
static nHIntCnt  := 0
static nAIntCnt  := 0
static nRecords  	:= 0			//		Record count
static cAwayName	:= ""
static cHomeName	:= ""
static cTeamID		:= ""
static cStats


FUNCTION LoadWk3

	local nBoxes	:= 0
	local b

	LOCAL GetList[1], oGet

	*---What week are the box scores for?
	cWeek := G_week

	@ 01,00 clear to 24,79
	@ 10, 30 say "Week :"
	SetPos(10, 50)
	GetList[1] = _GET_( cWeek, "cWeek", "@!",,)
	set cursor on
	ReadModal(GetList) 
	set cursor off
	GetList := {}
	G_week  := cWeek
   cAwayID := ''
   cHomeID := ''

	select TEAM
	set order to 4
	select SCHED
	set order to 2

	*---Load up names of boxscore files
*	TestMsg( "Looking for " + cBoxDir + "*.htm" )
	nBoxes := aDir( cBoxDir + "*.htm", aBox )

	if nBoxes = 0
		ErrorMsg( "No boxscores found in " + cBoxDir )
	else
		erase( cLogFile + ".LOG" )
*		TestMsg( str( nBoxes, 4 ) + " box scores found" )
		for b = 1 to nBoxes
			*---Process Boxscore
			*---Reset
			cSection := "TOP"
			cLineType := "NO-OP"

			DoBox( cBoxDir + aBox[ b ] )

		next
	endif

	select SCHED
	set order to 1
	select TEAM
	set order to 1
	select PLAYER
	set order to 1

RETURN nil


FUNCTION DoBox( cBoxFile )

	local cLine  	   := ""			//		record string
	local lThru			:= .f.
   local cLine1      := ""
   local cLine2      := ""

	local i, cErrMsg, cStartTime, getList

	cls()

	*---Open text file
	nHandle := fopen( cBoxFile )

	if nHandle = -1
		TestMsg( "Could not open " + cBoxFile )
		RETU .f.
	endif

   nASackCnt := 0
   nHSackCnt := 0
   nAIntCnt  := 0
   nHIntCnt  := 0

	print( 03, 32, "Memory    : " + str( memory() ) )
	print( 04, 32, "Box File  : " + cBoxFile      )
	print( 05, 32, "Records   : " )
	cStartTime := time()

   lSwapped := .f.

	do while .not. lThru
		*---Read a line
		cLine := freadline( nHandle )
		if feof( nHandle )
			exit
		else
*			cLine :=	alltrim( cLine )
			nRecords++
			print( 10, 00, space( 80 ) )
			print( 11, 00, space( 80 ) )
			print( 05, 70, str( nRecords, 7 ) )
		endif

		if .not. empty( cLine )
			*---------------------------------------------
			*   Process the line
			*---------------------------------------------

         if "QUARTER:" $ upper( cLine )
            *---This is really 2 lines
            cLine1 := substr( cLine, 1, at( ":", cLine ) - 1 )
*            TestMsg( "cLine 1 =" + cLine1 )
            doLine( cLine1, cBoxFile  )
            cLine2 := substr( cLine, at( ":", cLine ) + 1, ;
                               len( cLine ) - at( ":", cLine ) )
*            TestMsg( "cLine 2 =" + cLine2 )
            doLine( cLine2, cBoxFile  )

         else
            lThru := doLine( cLine, cBoxFile  )
         endif
		endif
	enddo

   ***
   ***   Do Integrity Checks
   ***
   CheckSacks()
   CheckInts()

*	WriteLog( cLogFile, "Elapsed Time    :" + elaptime( cStartTime, time() ) )
	print( 16, 20, "Records checked :" + str( nRecords, 7 ) )
	print( 17, 20, "Elapsed Time    :" + elaptime( cStartTime, time() ) )

	fclose( nHandle )

RETURN .t.

static;
FUNCTION CheckSacks

   local lAwayOk := .f., lHomeOk := .f., cMsg

   if nASackCnt <> nAwaySacks
      cMsg := cAwayID + "-Away sacks should be " + str( nAwaySacks, 3 ) + ;
              " I stored " + str( nASackCnt, 3 )

      TestMsg( cMsg )
      Writelog( cLogfile, cMsg )
   else
      lAwayOk = .t.
   endif

   if nHSackCnt <> nHomeSacks
      cMsg := cHomeID + "-Home sacks should be " + str( nHomeSacks, 3 ) + ;
              " I stored " + str( nHSackCnt, 3 )

      TestMsg( cMsg )
      Writelog( cLogfile, cMsg )
   else
      lHomeOk = .t.
   endif

   if lHomeOk .and. lAwayOk
*      TestMsg( "Sacks balance OK!" )
   endif

return nil


static;
FUNCTION CheckInts

   local lAwayOk := .f., lHomeOk := .f.

   if nAIntCnt <> nAwayInts
      cMsg := cAwayID + "-Away ints should be " + str( nAwayInts, 3 ) + ;
              " I stored " + str( nAIntCnt, 3 )

      TestMsg( cMsg )
      Writelog( cLogfile, cMsg )
   else
      lAwayOk = .t.
   endif

   if nHIntCnt <> nHomeInts
      cMsg := cHomeID + "-Home ints should be " + str( nHomeInts, 3 ) + ;
              " I stored " + str( nHIntCnt, 3 )

      TestMsg( cMsg )
      Writelog( cLogfile, cMsg )
   else
      lHomeOk = .t.
   endif

   if lHomeOk .and. lAwayOk
*      TestMsg( "Interceptions balance OK!" )
   endif

return nil


static;
FUNCTION DoLine( cLine, cBoxFile )

   *---Get rid of HTML codes
   local lThru

   lThru    := .f.
   cLine    := ltrim( cLine )
   cLine		:= HTMLStrip( cLine )		//		Clean line
   print( 10, 05, cLine )

   if .not. empty( cLine )
      *---------------------------------------------
	   *   Pre Process the line
	   *---------------------------------------------
      cLine := FixLine( cLine )

   endif

   *---Identify the type of line
   cLineType := LineType( cLine, cSection )

   *---Identify the Section
   cSection	:= IDSection( cLineType, cSection )
*   if cLineType <> "NO-OP"
*      TestMsg( "Section = " + cSection + " Linetype = " + cLineType +;
*               " Line Size = " + str( len( cLine ), 5 ) )
*   endif

   *---Process Linetype
   do case
	case cLineType = "PLAYER STATS"
		*---Do it the old way
		lThru := LoadIndiv()

   case cLineType = "AWAYTEAM"
	   *---Establish Away Score
	   nAwayScore 	:= ScoreOf( cLine )

   case cLineType = "HOMETEAM"
	   *---Establish Home Score
	   nHomeScore 	:= ScoreOf( cLine )

	   *---Establish Game Code, and save scores
	   cGameCode := WhichGame( cWeek, cAwayID, cHomeID, nAwayScore, ;
									   nHomeScore )

	   *---set global
	   G_Gameno := cGameCode

	   if .not. empty( cGameCode )
		   *---Clear scores
		   ClrScores( cGameCode )
		   cMsg	:=	"Game " + cGameCode + "  " + cAwayID + str( nAwayScore, 4 ) +;
			   " @ " + cHomeID + str( nHomeScore, 4 )
		   WriteLog( cLogFile, cMsg + " in " + cBoxFile )
	   endif

   case cLineType = "QUARTER"
	   *---establish current quarter
		cQuarter := WhichQtr( cLine )
*		if cQuarter = "5"
*			DoScore( cLine )
*		endif

   case cLineType = "SCORE"
	   *---Write the Score
	   DoScore( cLine )

   case cLineType = "TEAM STATS"
	   *---Double check scores
	   nHomeScore := nAwayScore := 0
	   cGameKey   := G_Season + cWeek + cGameCode
	   TheScore( cGameKey )
	   if SCHED->HOMESCORE <> nHomeScore
		   TestMsg( SCHED->HOMETEAM + " Home score wrong " + str( SCHED->HOMESCORE,2 ) +;
						   " to " + str( nHomeScore, 2 ) )
		   WriteLog( cLogFile, SCHED->HOMETEAM + " Home score wrong " + str( SCHED->HOMESCORE,2 ) +;
						   " to " + str( nHomeScore, 2 ) )

	   endif
	   if SCHED->AWAYSCORE <> nAwayScore
		   TestMsg( SCHED->AWAYTEAM + " score wrong " + str( SCHED->AWAYSCORE,2 ) +;
						   " to " + str( nAwayScore, 2 ) )
		   WriteLog( cLogFile, SCHED->AWAYTEAM + " score wrong " + str( SCHED->AWAYSCORE,2 ) +;
						   " to " + str( nAwayScore, 2 ) )
	   endif

   case cLineType = "TEAMSACKS"
      *---Set the Sacks
      do SetSacks with cLine

   case cLineType = "TEAMINTS"
      *---Set the Ints
      do SetInts with cLine

   case cLineType = "TRAILER"
	   *---Finish box
	   lThru := .t.
   endcase

*	TestMsg( "Line Type " + cLineType )

RETURN lThru


static ;
FUNCTION GotTeam( cTeam, cTeamID )

	local lTeam := .f.

	if upper( cTeam ) = upper( cHomeAPName )
		cTeamID  := cHomeID
		lTeam		:= .t.
	else
		if upper( cTeam ) = upper( cAwayAPName )
			cTeamID 	:= cAwayID
			lTeam		:= .t.
		endif
	endif

	if lTeam
*		TestMsg( "Got team " + cTeamID )
		cTeam		:= ""
	endif

RETURN lTeam


static ;
FUNCTION GetScoreLine( cLine )

	local cStatLine, nFstop, lEofScore
	*---Get the whole line
   lEofScore := .f.

	cStatLine := alltrim( cLine )
*	TestMsg( "Scoreline is >" + cLine )
   if at( ":", cLine ) > 0
      *---Nothing more to add
      lEofScore := .t.
      cStatLine := cLine
   endif

	do while .not. lEofScore
		cLine := freadline( nHandle )
		if feof( nHandle )
			exit
		else
			*---Strip out HTML
			cLine		:= HtmlStrip( cLine )
			nFstop = at( ":", cLine )
			if nFstop > 0
            *---found a colon
				if len( cLine ) > nFstop + 2
					cLine := substr( cLine, 1, nFstop + 2 )
				endif
            lEofScore := .t.
			endif

			cStatLine += " " + alltrim( cLine )
		endif

		cStatLine = alltrim( cStatLine )
	enddo

*	TestMsg( str( len( cStatLine ), 5 ) + " char Score line " + cStatLine )

RETURN cStatLine


static ;
FUNCTION GetStatLine( cLine, lCheck )

	local cStatLine, nFstop
	*---Get the whole line

*	if lCheck
*		nFstop = at( ". ", cLine )
*		if nFstop > 0
*			if len( cLine ) > nFstop
*				cLine := substr( cLine, 1, nFstop )
*			endif
*		endif
*	endif
	cStatLine := trim( cLine )

	do while right( cStatLine, 1 ) <> "."
		cLine := freadline( nHandle )
		if feof( nHandle )
			exit
		else
			*---Strip out HTML
			cLine		:= HtmlStrip( cLine )
			*---Get rid of anything to the right of the ". "  (not .5 sacks)
			nFstop = at( ". ", cLine )
			if nFstop > 0
				if len( cLine ) > nFstop
               if	.not. lCheck
	   				cLine := substr( cLine, 1, nFstop )
   	   		endif
				endif
			endif

			cStatLine += " " + cLine
		endif

		cStatLine = alltrim( cStatLine )
	enddo

*	TestMsg( "Stat line " + cStatLine )

RETURN cStatLine


FUNCTION LineType( cLine, cSection )

	local cLineType := "NO-OP"
	local cAPcode, cAPname

*  TestMsg( "cSection is " + cSection )
	do case
	case cSection = "TOP" .or. cSection = "AWAY"
		*---Havent hit anything yet
		cPoss := PickTeam( cLine )

		if .not. empty( cPoss )
			*---Look up cPoss as an APNAME

			cAPcode	:= ""
			cAPName  := ""
			cTeam 	:= isaTeam( cPoss, @cAPCode, @cAPName )

			if empty( cTeam )
				*---Not a team
			else
*				TestMsg( cPoss + " is a team" )

				if cSection = "TOP"
					cAwayID 	:= cTeam
					cAwayAP	 	:= cAPcode
					cAwayAPname	:= trim( cAPname )
					cLineType 	:= "AWAYTEAM"
*					TestMsg( "Away team is " + cAwayID + " " + cAwayAPname )
				else
					cHomeID 	:= cTeam
					cHomeAP	 	:= cAPcode
					cHomeAPname	:= trim( cAPname )
					cLineType 	:= "HOMETEAM"
*					TestMsg( "Home team is " + cHomeID + " " + cHomeAPname )
				endif
			endif
		endif

	case cSection = "HOME" .or. cSection = "SCORE"
		*---Now we look for a Quarter, or score lines
		if "QUARTER" $ upper( cLine )  .or. ;
			"OVERTIME" $ upper( cLine ) .or. "OT:" $ upper( cLine )
			cLineType := "QUARTER"
		else
			*---Could be a score
			cScorer := substr( cLine, 1, 3 )
*			TestMsg( "cScorer is " + cScorer )
			if len( cLine ) < 14
            *---End of Scores?
				cLineType := "TEAM"
         else
				if IsaCode( cScorer )
					cLineType := "SCORE"
				else
					if upper( cScorer ) = "TEA" .or. ;
						upper( cScorer ) = "FIR"
						cLineType := "TEAM"
					else
                  if	upper( cScorer ) = "PLAY"
                     TestMsg( "Check the time of the last score!" )
            			cLineType := "PLAYER STATS"
                  else
   						cLineType := "NO-OP"

	      			endif

					endif
				endif
			endif
		endif

	case cSection = "TEAM STATS"
		*---Look for next section
		cLineType := "NO-OP"
		if upper( substr( cLine, 1, 6 ) ) = "PLAYER" .or. ;
         upper( substr( cLine, 1, 6 ) ) = "INDIVI"
			cLineType := "PLAYER STATS"
      else
         if upper( cLine ) = "SACKED"
   			cLineType := "TEAMSACKS"
         else
            if upper( cLine ) = "HAD INTERCEPTED"
      			cLineType := "TEAMINTS"
            endif
         endif
		endif
*		TestMsg( "Line Type is " + cLineType )

	case cSection = "PLAYER STATS"
		*---Look for Individual stats
		cLineType := "NO-OP"
		do case
		case upper( cLine ) = upper( cAwayAPname + " RUSHING" )
			cLineType := "AWAYRUSH"			
		case upper( cLine ) = upper( cHomeAPName + " RUSHING" )
			cLineType := "HOMERUSH"
		case upper( cLine ) = upper( cAwayAPName + " PASSING" )
			cLineType := "AWAYPASS"
		case upper( cLine ) = upper( cHomeAPName + " PASSING" )
			cLineType := "HOMEPASS"
		case upper( cLine ) = upper( cAwayAPName + " RECEIVING" )
			cLineType := "AWAYRECV"
		case upper( cLine ) = upper( cHomeAPName + " RECEIVING" )
			cLineType := "HOMERECV"
		case upper( cLine ) = upper( cAwayAPName + " TACKLES" )
			cLineType := "AWAYSACK"
		case upper( cLine ) = upper( cHomeAPName + " TACKLES" )
			cLineType := "HOMESACK"
		case upper( cLine ) = "INTERCEPTIONS"
			cLineType := "INTERCEPTS"
		case upper( cLine ) = "FUMBLES"
			cLineType := "TRAILER"
		endcase
*		TestMsg( "Line Type is " + cLineType )
	endcase

RETURN cLineType

/*
        Name :   Set Sacks
        Does :   Works out how many sacks there were
  Parameters :   the Team stats Sacked-yrads lost line
     Returns :   nHomeSacks, nAwaySacks
       Notes : 

      Author : LL 
Date Created : 11 Sep 96
Time Created :  9:10 pm
	Last change:  LL    9 Sep 100    3:03 pm
*/
PROCEDURE SetSacks( cLine )
   local nWords
   cLine := strtran( cLine, "-", " " )
   nWords := ParseLine( cLine )        &&  YARDS will get droped off
   nHomeSacks = val( aWords[ 3 ] )
   nAwaySacks = val( aWords[ 5 ] )

   if lSwapped
      nHomeSacks = val( aWords[ 5 ] )
      nAwaySacks = val( aWords[ 3 ] )
   endif

*   TestMsg( "Home sacks = " + str( nHomeSacks, 3 ) )
*   TestMsg( "Away sacks = " + str( nAwaySacks, 3 ) )

RETURN


PROCEDURE SetInts( cLine )
   local nWords
   cLine := strtran( cLine, "-", " " )
   nWords := ParseLine( cLine )        &&  YARDS will get droped off
   nHomeInts = val( aWords[ 3 ] )
   nAwayInts = val( aWords[ 4 ] )

   if lSwapped
      nHomeInts = val( aWords[ 4 ] )
      nAwayInts = val( aWords[ 3 ] )
   endif

*   TestMsg( "Home Ints = " + str( nHomeInts, 3 ) )
*   TestMsg( "Away Ints = " + str( nAwayInts, 3 ) )

RETURN


FUNCTION IDSection( cLineType, cSection )

	do case
	case cLineType = "AWAYTEAM"
		cSection := "AWAY"
	case cLineType = "HOMETEAM"
		cSection := "HOME"
	case cLineType = "QUARTER"
		cSection := "SCORE"
	case cLineType = "SCORE"
		cSection := "SCORE"
	case cLineType = "TEAM"
		cSection := "TEAM STATS"
	case cLineType = "PLAYER STATS"
		cSection := "PLAYER STATS"
	endcase

*	TestMsg( "Section is " + cSection )

RETURN cSection


FUNCTION DoScore( cLine )

	*---dismantle the line to find the line scores
	local lThru := .f., cPlyr1, cPlyr2, nWords, cScoreLine, cRunnrStr, cRecvrStr

	cTeam := substr( cLine, 1, at( "-", cLine ) - 1 )

	cScoreLine := GetScoreLine( cLine )

	cTeamID := iif( cTeam = trim( cAwayAP ), cAwayID, cHomeID )
*	TestMsg( "Score for >" + cTeamId )

	*------------------------------------------------------
	*		Break up the line into an array of words
	*------------------------------------------------------
*	nWords := ParseLine( cScoreLine, "?" )
	nWords := ParseLine( cScoreLine )

	*---The time is the last word
	cTime 	:= TimeOf( cQuarter, aWords[ nWords ] )

*	TestMsg( "aWords[2] " + aWords[2] )    &&  a dash
*	TestMsg( "aWords[3] " + aWords[3] )
*	TestMsg( "nWords =  " + str( len( aWords ), 3 ) )
	do case
	case aWords[2] = "FG"
		*---FG
*		if at( ".", cScoreLine ) > 0
*			cScoreLine := substr( cScoreLine, 1, at( ".", cScoreline ) )
			nWords := ParseLine( cScoreLine )
*		endif

*		TestMsg( "ScoreLine " + cScoreLine )
*		TestMsg( "FG words " + str( nWords, 8 ) )
      if IsANumber( aWords[3] )
			cPlyr1 	:= IDof( "", aWords[4], cTeamID )
   		nDist		:= val( aWords[ 3 ] )
      else
			cPlyr1 	:= IDof( "", aWords[3], cTeamID )
   		nDist		:= val( aWords[ 4 ] )
      endif

		SaveScore( cWeek, cGameCode, cPlyr1, cTime, "3", nDist, "", cTeamID )

	case aWords[4] = "RUN" .or. aWords[5] = "RUN"
		*---TD run
		cRunnrStr := aWords[ 2 ]
*     TestMsg( "Runner is >" + cRunnrStr )
		if WordsIn( cRunnrStr ) = 2
			cPlyr1	:= IDof( aPlyr[1], aPlyr[2], cTeamID )
			nDist		:= val( aWords[ 4 ] )
		else
			*---No first name
			cPlyr1	:= IDof( "", aWords[2], cTeamID )
			nDist		:= val( aWords[ 3 ] )
		endif

		SaveScore( cWeek, cGameCode, cPlyr1, cTime, "R", nDist, "", cTeamID )
		DoPAT( cScoreLine )

	case aWords[4] = "PASS" .or. aWords[5] = "PASS"
		*---TD pass
*     TestMsg( "TD Pass" )
		cRecvrStr := RecvrStr( cScoreLine )
		if WordsIn( cRecvrStr ) = 2
			cPlyr1	:= IDof( aPlyr[1], aPlyr[2], cTeamID )
			nDist		:= val( aWords[ 4 ] )
		else
			*---No first name
			cPlyr1	:= IDof( "", aWords[2], cTeamID )
			nDist		:= val( aWords[ 3 ] )
		endif

		cQBStr := QBStr( cScoreLine )
*     TestMsg( "QB is " + cQBStr + " words " + str( WordsIn( cQBStr ), 2 ) )
		if WordsIn( cQBStr ) >= 2
			cPlyr2	:= IDof( aPlyr[1], aPlyr[2], cTeamID )
		else
			*---No surname
*        TestMsg( "Looking for ID of >" + aPlyr[1] )

			cPlyr2	:= IDof( "", aPlyr[1], cTeamID )
		endif

		SaveScore( cWeek, cGameCode, cPlyr1, cTime, "P", nDist, cPlyr2, cTeamID )
		DoPAT( cScoreLine )

	case "FUMBLE" $ cLine
		*---Player 1 n fumble return
		cPlyr1 	:= IDof( "", aWords[2], cTeamID )
		nDist		:= val( aWords[ 3 ] )
		SaveScore( cWeek, cGameCode, cPlyr1, cTime, "F", nDist, "", cTeamID )
		DoPAT( cScoreLine )

	case "KICKOFF" $ cLine
		*---Kickoff return
		cPlyr1 	:= IDof( "", aWords[2], cTeamID )
		nDist		:= val( aWords[ 3 ] )
		SaveScore( cWeek, cGameCode, cPlyr1, cTime, "K", nDist, "", cTeamID )
		DoPAT( cScoreLine )

	case "SAFETY" $ cLine
		*---Safety
      if aWords[8] = "BY"
         cPlyr1 := IDof( "", aWords[9], cTeamID )
      else
         if aWords[7] = "BY"
            cPlyr1 := IDof( "", aWords[8], cTeamID )
         else
            if aWords[6] = "BY"
               cPlyr1 := IDof( "", aWords[7], cTeamID )

            else
               if aWords[5] = "BY"
                  cPlyr1 := IDof( "", aWords[6], cTeamID )

               else
                  TestMsg( "Safety: " + cScoreLine )
                  cPlyr1 := ""
   	         	Writelog( cLogfile, "UNCLAIMED SAFETY!" )
               endif
            endif
         endif
      endif

		SaveScore( cWeek, cGameCode, cPlyr1, cTime, "S", 0, "", cTeamID )

	case "INTERCEPTION" $ cLine
*		TestMsg( "Interception by " +  aWords[2] )
		cPlyr1 	:= IDof( "", aWords[2], cTeamID )
		nDist		:= val( aWords[ 3 ] )
		SaveScore( cWeek, cGameCode, cPlyr1, cTime, "I", nDist, "", cTeamID )
		DoPAT( cScoreLine )

	case "PUNT" $ cLine
		cPlyr1 	:= IDof( "", aWords[2], cTeamID )
		nDist		:= val( aWords[ 3 ] )
		SaveScore( cWeek, cGameCode, cPlyr1, cTime, "T", nDist, "", cTeamID )
		DoPAT( cScoreLine )

	otherwise
		TestMsg( "Unknown score type! " + cLine )
		WriteLog( cLogFile, "Unknown score type! " + cLine )
	endcase

RETURN lThru


FUNCTION IsANumber( cString )

   local lIsNum := .t.

   for i = 1 to len( cString )
      cChar = substr( cString, i, 1 )
      if .not. isnum( cChar )
         lIsNum := .f.
         exit
      endif
   next

RETURN lIsNum



FUNCTION DoPAT( cLine )

	local cPatLine, nWords, cFirstName, cSurname

	*---Get the PAT part in brackets
	cPatLine := PickPAT( cLine )

	if "FAILED" $ cPatLine .or. "BLOCKED" $ cPatLine .or. "NO KICK" $ cPatLine
		RETU nil
	endif

	nWords := ParseLine( cPatLine )

*	TestMsg( "PAT line >" + cPatLine )

	if "KICK" $ cPatLine
		*---Conversion kick
		if nWords = 2		&&  Boniol kick
			cFirstName  := ""
			cSurname		:= aWords[ 1 ] 
		else
			cFirstName 	:= aWords[ 1 ]
			cSurname		:= aWords[ 2 ] 
		endif

		cPlyr1 	:= IDof( cFirstName, cSurname, cTeamID )
		SaveScore( cWeek, cGameCode, cPlyr1, cTime, "1", 0, "", cTeamID )

	else
		*---Two-Point Conversion
		if "PASS" $ cPatLine
			do case
			case aWords[2] = "PASS"
				*---QB
				cPlyr1 := IDof( "", aWords[1], cTeamID )		&& Receiver
				cPlyr2 := IDof( "", aWords[4], cTeamID )		&&	Passer

			endcase

			SaveScore( cWeek, cGameCode, cPlyr1, cTime, ;
					SCORE_PAT_PASS, 2, cPlyr2, cTeamID )

		else
			if "RUN"	$ cPatLine
  				cPlyr1 	:= IDof( "", aWords[1], cTeamID )
   			SaveScore( cWeek, cGameCode, cPlyr1, cTime, ;
		   				SCORE_PAT_RUN, 2, "", cTeamID )
			endif
		endif
	endif

RETURN nil


FUNCTION TimeOf( cQuarter, cTime )

	local cTimeout

   *---Drop off any full stops
   cTime := strtran( cTime, ".", "" )

	cTime := substr( cTime, 1, len( cTime ) )

	cTimeout := cQuarter + "-" + right( "00" + cTime, 5 )

RETURN cTimeOut


FUNCTION SaveIndiv( cPlayerID, cCat, cAmount, cTeamID )

	if .not. empty( cPlayerID ) .and. val( cAmount ) <> 0
*		WriteLog( cLogFile, cWeek + "," + cGameCode + "," + cPlayerid + " , " + ;
*							cCat + ", " + str( val( cAmount ), 5, 1 ) + " " + cTeamID )

      if	.not. empty( cGameCode )
		   select STAT
		   append blank
		   replace STAT->PLAYERID	with cPlayerID
		   replace STAT->STAT		with cCat
		   replace STAT->QTY			with val( cAmount )
		   replace STAT->SEASON		with G_season
		   replace STAT->WEEK		with cWeek
		   replace STAT->GAMENO		with cGameCode
		   replace STAT->TEAMID		with cTeamID
      endif

	endif

RETURN nil


FUNCTION SaveScore( cWeek, cGameCode, cPlyr1, cTime, cType, nDist, cPlyr2, cTeamID )

*	WriteLog( cLogFile, cWeek + "," + cGameCode + "," + cPlyr1 + " , " + ;
*					"," + cTime + "," + cType +","+ str( nDist,3) +;
*					+ "," + cPlyr2 + "," + cTeamID )

   if .not. empty( cGameCode )
	   select SCORE
	   append blank
	   replace SCORE->SEASON		with G_season
	   replace SCORE->WEEK			with cWeek
	   replace SCORE->GAMENO		with cGameCode
	   replace SCORE->PLAYERID1	with cPlyr1
	   replace SCORE->PLAYERID2	with cPlyr2
	   replace SCORE->WHEN			with cTime
	   replace SCORE->SCORE			with cType
	   replace SCORE->DISTANCE		with nDist
	   replace SCORE->TEAM			with cTeamId
   endif

RETURN nil


FUNCTION ParseLine( cLine, cSkipChar )

	*---put the words into an array
	local cChar, cWord, i, nWords

	if pcount() = 1
		cSkipChar := ","
	endif

	aWords 	:= {}
	cWord 	:= ""
	nWords	:= 0
	cLine		:= alltrim( cLine )

	for i = 1 to len( cLine )
		cChar := substr( cLine, i, 1 )
		if cChar = space(1) .or. cChar = "-"
			*---end of word
         if .not. empty( cWord ) .and. upper( cWord ) <> "YD"
   			nWords++
   			aadd( aWords, cWord )
*    			TestMsg( "Word " + str( nWords, 2 ) + " is " + cWord )
         endif

			cWord := ""
		else
			if .not. ( cChar $ cSkipChar )
				if i <= len( cLine )
					cWord += cChar
				endif
			endif

		endif

	next

	*---last one
	nWords++
	aadd( aWords, cWord )
*	TestMsg( "Word " + str( nWords, 2 ) + " is " + cWord )

RETURN nWords


FUNCTION WRITELOG( _proc, _msg )

	LOCAL log_msg, nHandle, spacing, cLogFile

	spacing = space(2)

*	log_msg = time() + spacing + _msg + chr(13) + chr(10)
	log_msg = spacing + _msg + chr(13) + chr(10)


	cLogFile = _proc + ".LOG"
	nhandle  = fopen( cLogFile, 2)

	if nHandle = -1
		*---File not found?
		nHandle = fcreate( cLogFile )
	endif

	if nHandle <> -1
		fseek(  nHandle, 0, 2)
		fwrite( nHandle, log_msg )
		fclose( nHandle )
	endif

RETURN nil


FUNCTION ScoreOf( cLine )

	local nLen, nSpot, cScore
   cLine := alltrim( strtran( cLine, "FINAL", "" ) )
	nLen := len( cLine )
	nSpot := at( "-", cLine )
   if nSpot > 0
   	cScore := substr( cLine, nSpot+2, nLen - nSpot + 1 )
   else
      cScore := right( cLine, 3 )
   endif

RETURN abs( val( cScore ) )


static ;
FUNCTION PickTeam( cLine )

	local nVal		:=	0
	local nPtr		:=	1
	local cChar		:= ""
	local cTeam		:= ""
   local lAppend  := .t.
   local lAllNumbers := .t.
   local lNumbers := .f.

	***
	***   Got to be the first thing on the line and rest of
   ***   line has to be numbers
	***
*	TestMsg( "Look for a Team name in >" + cLine )

	*---Look for a Team name
	do while nPtr <= len( cLine )
      *---Get next character
		cChar := substr( cLine, nPtr, 1 )
		nVal 	:= asc( cChar )
		if nVal >= 48 .and. nVal <= 57
			*---We've hit a number
			lAppend = .f.
         lNumbers = .t.
		else
         if	lAppend
   			cTeam	+= cChar
         else
            if	cChar = " "
               *---spaces are ok
            else
               lAllNumbers = .f.
            endif
	      endif
		endif
		nPtr++
	enddo

	cTeam := alltrim( cTeam )

	if	lNumbers
	   if	.not. lAllNumbers
         cTeam := "XXXX"
      endif
   else
      cTeam := "XXXX"
   endif

*  TestMsg( "Team picked >" + cTeam )

RETURN cTeam


FUNCTION HTMLStrip( cLine )

	local cStripLine	:= ""
	local lStop			:= .f.
	local i
	local cChar

   *---Get rid of nonblank spaces
   cLine = strtran(cLine,'&nbsp;','')

	*---Read each character
	for i = 1 to len( cLine )
		cChar := substr( cLine, i, 1 )
		if cChar = "<"
			lStop = .t.
		else 
			if cChar = ">"
				lStop = .f.
			else
				*---Not < or >
				if .not. lStop
					cStripLine += cChar
				endif
			endif
		endif
	next

*	TestMsg( "Strip line =" + cStripLine )

RETURN cStripLine


FUNCTION PickPat( cLine )

	local cPatLine	:= ""
	local lStop			:= .t.
	local i
	local cChar

	*---Read each character
	for i = 1 to len( cLine )
		cChar := substr( cLine, i, 1 )
		if cChar = "("
			lStop = .f.
		else 
			if cChar = ")"
				lStop = .t.
			else
				*---Not ( or )
				if .not. lStop
					cPatLine += cChar
				endif
			endif
		endif
	next

RETURN upper( cPatLine )


FUNCTION IsaTeam( cWords, cAPCode, cAPname )

	local cTeam := ""

	*---Checks to see if this is a team
   if len(cWords) < 30 .and. len( cWords ) > 1
	   if TEAM->( dbseek( G_season + cWords ) )
		   cTeam    := TEAM->TEAMID
		   cAPCode	:= TEAM->APCODE
		   cAPName	:= TEAM->APNAME
	   else
         if TEAM->( dbseek( G_season + strtran( cWords, ".", "" ) ) )
		      cTeam    := TEAM->TEAMID
		      cAPCode	:= TEAM->APCODE
		      cAPName	:= TEAM->APNAME
         else
*   		   TestMsg( G_season + cWords + " not found" )
         endif
	   endif
   else 
*      TestMsg( "Too many words" )
      cTeam := ""
   endif

RETURN cTeam


FUNCTION WhichGame( cWeek, cAwayID, cHomeID, nAwayScore, nHomeScore )

	local cGameCode := ""
   local cSwap, nSwap

	if SCHED->( dbseek( G_season + cWeek + cAwayID + cHomeID ) )
		cGameCode 	:= SCHED->GAMENO
*		TestMsg( "Game code = " + cGameCode )
*		TestMsg( "Saving Score " + cAwayID + str( nAwayScore, 4 ) +;
*						" @ " + cHomeID + str( nHomeScore, 4 ) )
		replace SCHED->AWAYSCORE	with nAwayScore
		replace SCHED->HOMESCORE	with nHomeScore
	else
      *---Maybe the teams have been swapped around?
      if SCHED->( dbseek( G_season + cWeek + cHomeID + cAwayID ))
   		cGameCode 	:= SCHED->GAMENO
         lSwapped   := .t.
   		TestMsg( "Swapping - Game code = " + cGameCode )
         cSwap      := cHomeID
         cHomeID    := cAwayID
         cAwayID    := cSwap
         nSwap      := nHomeScore
         nHomeScore := nAwayScore
         nAwayScore := nSwap
   		replace SCHED->AWAYSCORE	with nAwayScore
	   	replace SCHED->HOMESCORE	with nHomeScore
      else
   		TestMsg( cAwayID + " @ " + cHomeID + " Game not scheduled in " +;
               G_season + " Week " + cWeek )

      endif

	endif

RETURN cGameCode


FUNCTION ClrScores( cGameCode )

	*---Wipes out any prior line scores
	local nCount := 0
*	TestMsg( "Wiping " + G_season + " wk " + cWeek + " game " + cGameCode )

	select SCORE
	seek G_season + cWeek + cGameCode
	do while .not. eof() .and. SCORE->SEASON = G_season ;
								.and. SCORE->WEEK   = cWeek	 ;
								.and. SCORE->GAMENO = cGameCode
		delete
		nCount++
		skip
	enddo

*	TestMsg( str( nCount, 7 ) + " scores wiped" )

	select STAT
	seek G_season + cWeek + cGameCode
	do while .not. eof() .and. STAT->SEASON = G_season ;
								.and. STAT->WEEK   = cWeek	 ;
								.and. STAT->GAMENO = cGameCode
		delete
		nCount++

		skip
	enddo

*	TestMsg( str( nCount, 7 ) + " stats wiped" )

RETURN nil


FUNCTION WhichQtr( cLine )

	local nWords, cQuarter := "0"

	nWords := ParseLine( cLine )

	if upper( aWords[ 1 ] ) = "OT" .or. upper( aWords[ 1 ] ) = "OVERTIME"
*     TestMsg( "Overtime >" + aWords[ 1 ] )
		cQuarter := "5"

	else
		if upper( aWords[ 2 ] ) = "QUART"
			do case
			case upper( aWords[ 1 ] ) = "FIRST" .or. ;
				  upper( aWords[ 1 ] ) = "1ST" 
				cQuarter := "1"
			case upper( aWords[ 1 ] ) = "SECOND" .or. ;
				  upper( aWords[ 1 ] ) = "2ND" 
				cQuarter := "2"
			case upper( aWords[ 1 ] ) = "THIRD"	 .or. ;
				  upper( aWords[ 1 ] ) = "3RD" 
				cQuarter := "3"
			case upper( aWords[ 1 ] ) = "FOURTH" .or. ;
				  upper( aWords[ 1 ] ) = "4TH" 
				cQuarter := "4"
			otherwise
				cQuarter := "?"
			endcase
		endif
	endif

*	TestMsg( "We are now in quarter " + cQuarter )

RETURN cQuarter


FUNCTION Isacode( cAPcode )

	local lFound := .f.

	cAPcode := strtran( cAPcode, "-", " " )
	if cAPCode = cAwayAP .or. cAPcode = cHomeAP .or. ;
      strtran( cAPCode, ".", "" ) =  strtran( cAwayAP, ".", "" ) .or. ;
      strtran( cAPCode, ".", "" ) =  strtran( cHomeAP, ".", "" )

		lFound := .t.
	else
*		TestMsg( cApCode + " is not " + cAwayAP + " or " + cHomeAP )
	endif

RETURN lFound


FUNCTION IdOf( cFirstName, cSurName, cTeamIn )

	local cPlayerID := space(8)
	local cTeam

	cFirstName 	:= upper( cFirstName )
	cSurname		:= upper( cSurname   )
   if "." $ cSurname
      cFirstName := substr( cSurname, 1, at( ".", cSurname ) - 1 )
      cSurname   := substr( cSurname,  at( ".", cSurname ) + 1 )
   endif

*  TestMsg( "Finding " + cFirstName + " " + cSurname )

	***
	***	Eg  cName = "STEVE COLONNA"	- given name and team what is the ID?
	***

	select PLAYER
	set order to 2			&&  upper(SURNAME)
	if PLAYER->( dbseek( cSurname ) )
		do while .not. PLAYER->( eof() ) ;
					.and. upper( PLAYER->SURNAME ) = cSurname
							
			if upper( trim( PLAYER->FIRSTNAME ) ) = cFirstName
				if cTeamIn = PLAYER->CURRTEAM
					cPlayerID = PLAYER->PLAYERID
					exit
				endif
			else
*				TestMsg( upper( trim( PLAYER->FIRSTNAME ) ) + "<>" + cFirstName )
			endif

			PLAYER->( dbskip() )
		enddo
	else
		TestMsg( "Surname " + cSurname + " not found" )
	endif

*	TestMsg( "ID for " + cFirstName + " " + cSurname + " is " + cPlayerID )

	if empty( cPlayerID )
		TestMsg( "ID not found for for " + cFirstName + " " + cSurname + ;
					" of " + cTeamIn )
		WriteLog( cLogFile, "Could not find ID for " + cFirstName + " " + cSurname  + " of " + cTeamIn  )
	endif

RETURN cPlayerID


FUNCTION RecvrStr( cScoreLine )

	*---Get the part between the , and the number
	local cPlayer	:= ""
	local lStop			:= .t.
	local i
	local cChar

	*---Read each character
	for i = 1 to len( cScoreLine )
		cChar := substr( cScoreLine, i, 1 )
		if cChar = ","
			lStop = .f.
		else 
			if asc( cChar ) >= 48 .and. asc( cChar ) <= 57 
				lStop = .t.
			else
				if .not. lStop
					cPlayer += cChar
				endif
			endif
		endif
	next

RETURN cPlayer

static ;
FUNCTION WordsIn( cLine )

	*---put the words into an array
	local cChar, cWord, i, nWords

	aPlyr 	:= {}
	cWord 	:= ""
	nWords	:= 0
	cLine		:= alltrim( cLine )

	for i = 1 to len( cLine )
		cChar := substr( cLine, i, 1 )
		if cChar = " "
			*---end of word
			nWords++
			aadd( aPlyr, cWord )
*			TestMsg( "Word " + str( nWords, 2 ) + " is " + cWord )
			cWord := ""
		else
			if i <= len( cLine )
				cWord += cChar
			endif
		endif

	next

	*---last one
	nWords++
	aadd( aPlyr, cWord )
*	TestMsg( "Word " + str( nWords, 2 ) + " is " + cWord )

RETURN nWords


static ;
FUNCTION QBStr( cLine )

	*---get the string between "FROM" and "("
	local cChar, cWord, i, nWords, lStart, cQB

	cQB		:= ""
	aPlyr 	:= {}
	cWord 	:= ""
	nWords	:= 0
	cLine		:= alltrim( cLine )
	lStart	:= .f.

*  TestMsg( "cLine IN>" + cLine )

	for i = 1 to len( cLine )
		cChar := substr( cLine, i, 1 )
		if cChar = "("
			lStart := .f.
         exit
		endif

		if cChar = " "
			*---end of word
			nWords++
			aadd( aPlyr, cWord )
			if cWord = "FROM"
				lStart := .t.
			endif

			cWord := ""
		else
			if i <= len( cLine )
				cWord += cChar
			endif
		endif

		if lStart
			cQB += cChar
*        TestMsg( "cQB >" + cQB )
		endif
	next

	*---last one
	nWords++
	aadd( aPlyr, cWord )

   *---get rid of any commas
	cQB = strtran( cQB, ",", "" )

RETURN cQB


static FUNCTION TheScore( cGameKey )

	local nOldArea

	store 0 to nHomeScore, nAwayScore

	nOldArea = select()

	select SCORE
	dbseek( cGameKey )
	do while .not. eof() .and. SEASON+WEEK+GAMENO = cGameKey

		if SCORE->TEAM = SCHED->HOMETEAM
			ADDTO( nHomeScore, PtsFor( SCORE ) )
		else
			ADDTO( nAwayScore, PtsFor( SCORE ) )
		endif
		skip
	enddo

	select( nOldArea )

RETURN .t.

#define     K_FIXES     18

static FUNCTION FixLine( cLine )

   local nFixes:= K_FIXES

   declare aBadWords[  K_FIXES ]
   declare aGoodWords[ K_FIXES ]

   aBadWords[  1 ] := "DEL GRECO"
   aGoodWords[ 1 ] := "DELGRECO"
   aBadWords[  2 ] := "BILLY JOE"
   aGoodWords[ 2 ] := "BJ"
   aBadWords[  3 ] := "ODONNELL"
   aGoodWords[ 3 ] := "O'DONNELL"
   aBadWords[  4 ] := "JJ STOKES"
   aGoodWords[ 4 ] := "J. STOKES"
   aBadWords[  5 ] := "MICHAEL DEAN PERRY"
   aGoodWords[ 5 ] := "MICHAEL PERRY"
   aBadWords[  6 ] := "VON OELHOFFEN"
   aGoodWords[ 6 ] := "VONOELHOFFEN"
   aBadWords[  7 ] := "CHRIS T JONES"
   aGoodWords[ 7 ] := "CHRIST JONES"
   aBadWords[  8 ] := "VAN DYKE"
   aGoodWords[ 8 ] := "VANDYKE"
   aBadWords[  9 ] := "J.J. BIRDEN"
   aGoodWords[ 9 ] := "JJ BIRDEN"
   aBadWords[ 10 ] := "JOHN HENRY"
   aGoodWords[10 ] := "JOHNHENRY"
   aBadWords[ 11 ] := "D.J."
   aGoodWords[11 ] := "DJ"
   aBadWords[ 12 ] := "O.J."
   aGoodWords[12 ] := "OJ"
   aBadWords[ 13 ] := "O.J. BRIGANCE"
   aGoodWords[13 ] := "OJ BRIGANCE"
   aBadWords[ 14 ] := "LE BEL"
   aGoodWords[14 ] := "LEBEL"
   aBadWords[ 15 ] := "J.J. MCCLESKY"
   aGoodWords[15 ] := "JJ MCCLESKY"
   aBadWords[ 16 ] := "J.B. BROWN"
   aGoodWords[16 ] := "JB BROWN"
   aBadWords[ 17 ] := "VAN PELT"
   aGoodWords[17 ] := "VANPELT"
   aBadWords[ 18 ] := "C.J."
   aGoodWords[18 ] := "CJ"

   cLine = upper( cLine )
   for i = 1 to nFixes
      if len( cLine ) > len( aBadWords[i] )
         if aBadWords[ i ] $ cLine
            cLine := strtran( cLine, aBadWords[ i ], aGoodWords[ i ] )
*            TestMsg( "Line changed to >" + cLine )
         endif
      endif
   next

RETURN cLine


FUNCTION LoadIndiv

	local lThru			:= .f.
	local cLine     	:= ""		//		record string
	local cBigLine   	:= ""		//		record string

	do while .not. lThru
		*---Read a line
		cLine := alltrim( freadline( nHandle ) )
		if feof( nHandle )
			lThru := .t.
			exit
		else
			nRecords++
			print( 10, 05, cLine )
			print( 05, 70, str( nRecords, 7 ) )
		endif

		if cLine = "RUSHING"
			*---make a big string
			do while cLine <> "MISSED FIELD GOAL"
				cBigLine += " " + cLine
				cLine := alltrim( freadline( nHandle ) )
				if feof( nHandle )
					lThru := .t.
					exit
				endif
			enddo

			ParseStats( cBigLine )
			lThru := .t.
			exit
		endif

	enddo

RETURN lThru


FUNCTION ParseStats( cBigLine )

	local cChar, cWord, i, nWords

	cWord 	:= ""
	nWords	:= 0
	cStats	:= ""

	cBigLine := alltrim( cBigLine )

	for i = 1 to len( cBigLine )
		*---For each word in the bigline
		cChar = substr( cBigLine, i, 1 )
      if EndOfWord( cChar, cWord )

			*---end of word
			nWords++
			cWord = alltrim( cWord )
         *---*DEBUGGING* For word by word blow by blow (Hi Monica), uncomment this
         #ifdef DEBUGGING
			TestMsg( "Word " + str( nWords, 4 ) + " is " + cWord )
         #endif
			do case
			case cWord = "RUSHING"
				cStats = "RUSHING"
				cWord = TeamFor( cWord )
*				TestMsg( "Setting stats to " + cStats + " team=" + cWord )

			case cWord = "PASSING"
*				TestMsg( "Setting stats to Passing" )
				cStats = "PASSING"
				cWord = TeamFor( cWord )

		 	case cWord = "RECEIVING"
				cStats = "RECEIVING"
*				TestMsg( "Setting stats to " + cStats + " team=" + cWord )
				cWord = TeamFor( cWord )

		 	case cWord = "TACKLES-ASSISTS-SACKS"
				cStats = "SACKS"
*				TestMsg( "Setting stats to " + cStats )
				cWord = TeamFor( cWord )

		 	case cWord = "INTERCEPTIONS"
				cStats = "INTS"
*				TestMsg( "Setting stats to " + cStats )
				cWord = TeamFor( cWord )
            if cWord = "None"
*              TestMsg( "No Interceptions" )
               cWord := ""
            endif

		 	case "RETURNS" $ cWord
				cStats = "RETS"
*				TestMsg( "Setting stats to " + cStats )

			otherwise
				*---Not a stat type
*				TestMsg( cWord + " is not a stat type" )

			endcase
			EatWord( cWord )

			*---reset word
			cWord := ""

		else
			if .not. ( cChar $ "," )
				if i <> len( cBigLine )
					cWord += cChar
				endif
			endif

		endif

	next

*	TestMsg( "Eat Word " + str( nWords, 2 ) + " is " + cWord )
	EatWord( cWord )

RETURN nil

static;
FUNCTION EndOfWord( cChar, cWord )

   local lEnd := .f., c2ndLastChar

	if cChar = ","
      lEnd := .t.
   else
      if cChar = " "
         if  right( cWord, 1 ) = "."
            *---last char was a full stop
*           TestMsg( "cWord ands in  .>" + cWord )
            c2ndLastChar = left( right( cWord, 2 ), 1 )
            if	 isnum( c2ndLastChar ) .or. c2ndLastChar = ")"
                *---second last char was a ) or number
                lEnd := .t.
             endif
             if upper( right( cWord, 5 ) ) = "NONE."
*               TestMsg( "***Last word was None***" )
                lEnd := .t.
             endif

          endif
       endif
   endif

RETURN lEnd


static;
FUNCTION	TeamFor( cWord )

	local cTeamWord

   *---The part after the dash
	cTeamWord = substr( cWord, at( "-", cWord ) + 1 )
*  TestMsg( "Team word? > " + cTeamWord )

   if cTeamWord = "ASSISTS"
      cTeamWord = substr( cTeamword, 15 )
   endif

	if alltrim( cTeamWord ) = "York"	.or. ;
		alltrim( cTeamWord ) = "New York" .or. ;
		alltrim( cTeamWord ) = "Giants"
		if cHomeAPName = "N.Y. Giants" .or. cAwayAPName = "N.Y. Giants"
			cTeamWord := "N.Y. Giants"
		else
			cTeamWord := "N.Y. Jets"
		endif
	else
		if alltrim( cWord ) = "Jets"
			cTeamWord := "N.Y. Jets"
		endif
	endif

*	TestMsg( "Setting team to " + cTeamWord )

return cTeamWord


FUNCTION EatWord( cWord )

	local cPlayerID 	:= ""
	local nWords		:= 0

   *---Convert 1/2 to .5 (for sacks)
   cWord := strtran( cWord, " 1/2", ".5" )
   cWord := strtran( cWord, "1/2", ".5" )
	cWord := alltrim( cWord  )

	if empty( cWord )
		*---skip it
	else
		*---
		cAwayName := cAwayAPName
		cHomeName := cHomeAPName

		if MatchesAway( cWord )
			cTeamID = cAwayID
         #ifdef DEBUGGING
			TestMsg( "Team ID set to " + cTeamID + " (away)" )
         #endif
		else
			if MatchesHome( cWord )
				cTeamID = cHomeID
            #ifdef DEBUGGING
   			TestMsg( "Team ID set to " + cTeamID + " (home)" )
            #endif
            
			else
				*---Not a team
				*---Its a stat
            if StoredStat( cStats )
				   nWords := GetStat( cWord )

				   do case
				   case cStats = "RUSHING"

					   if nWords < 3
						   WriteLog( cLogFile, "? RUSHING  " + cWord )
					   else
                     if val( aWords[ 2 ] ) > 0
            				cPlayerID := Idof( "", aWords[1], cTeamID )
                        if .not. empty( cPlayerID )
      						   SaveIndiv( cPlayerID, RUSHING_CARRIES,	aWords[ 2 ], cTeamID )
	      					   SaveIndiv( cPlayerID, RUSHING_YARDS, 	aWords[ 3 ], cTeamID )
                        endif
                     endif
					   endif

				   case cStats = "PASSING"

					   if nWords < 5
						   WriteLog( cLogFile, "? PASSING  " + cWord )
					   else
                     if val( aWords[ 3 ] ) > 0
            				cPlayerID := Idof( "", aWords[1], cTeamID )
                        if .not. empty( cPlayerID )
						         SaveIndiv( cPlayerID, PASS_COMPLETIONS,	aWords[ 2 ], cTeamID )
						         SaveIndiv( cPlayerID, PASSING_ATTEMPTS,	aWords[ 3 ], cTeamID )
						         SaveIndiv( cPlayerID, PASSES_INTERCEPTED,	aWords[ 4 ], cTeamID )
						         SaveIndiv( cPlayerID, PASSING_YARDAGE,		aWords[ 5 ], cTeamID )
                        endif
                     endif
					   endif

				   case cStats = "RECEIVING"

					   if nWords < 3
						   WriteLog( cLogFile, "? " + cStats + " " + cWord )
					   else
                     if val( aWords[ 2 ] ) > 0
            				cPlayerID := Idof( "", aWords[1], cTeamID )
                        if .not. empty( cPlayerID )
      						   SaveIndiv( cPlayerID, PASSES_CAUGHT,		aWords[ 2 ], cTeamID )
	      					   SaveIndiv( cPlayerID, RECEPTION_YARDAGE,	aWords[ 3 ], cTeamID )
                        endif
                     endif
					   endif

				   case cStats = "SACKS"

                  if nWords < 4
						   WriteLog( cLogFile, "? " + cStats + " " + cWord )
                  else
                     if val( aWords[ 4 ] ) > 0
               			cPlayerID := Idof( "", aWords[1], cTeamID )
                        if .not. empty( cPlayerID )
         			         SaveIndiv( cPlayerID, QUARTERBACK_SACKS,	aWords[ 4 ], cTeamID )

                           IncrSacks( cTeamID, val( aWords[ 4 ] ) )
*                          TestMsg( "Stored " + aWords[4] + " sax for " + cPlayerID )
                        endif

                     endif
                  endif

				   case cStats = "INTS"

                  if nWords < 2
                     if upper(cWord) <> "NONE"
   						   WriteLog( cLogFile, "? " + cStats + " " + cWord )
                     endif
                  else
                     if val( aWords[ 2 ] ) > 0
            			   cPlayerID := Idof( "", aWords[1], cTeamID )
                        if .not. empty( cPlayerID )
            		         SaveIndiv( cPlayerID, INTERCEPTIONS_MADE,	aWords[ 2 ], cTeamID )
                           IncrInts( cTeamID, val( aWords[ 2 ] ) )
                        endif
                     endif
                  endif

				   otherwise

				   endcase
            endif
			endif
		endif
	endif

RETURN nil


static;
FUNCTION MatchesAway( cWord )
   local lMatches := .f.

   #ifdef DEBUGGING
   TestMsg( "cAwayName=" + cAwayName )
   #endif

   if cWord = cAwayName
      lMatches := .t.
   else
      if cAwayName = "N.Y. Giants" .or. cAwayName = "N.Y. Jets"
         if cWord = "New York"
            lMatches := .t.
         endif
      endif
   endif

RETURN lMatches


static;
FUNCTION MatchesHome( cWord )
   local lMatches := .f.
   if cWord = cHomeName
      lMatches := .t.
   else
      if cHomeName = "N.Y. Giants" .or. cHomeName = "N.Y. Jets"
         if cWord = "New York"
            lMatches := .t.
         endif
      endif
   endif

RETURN lMatches


static;
FUNCTION IncrSacks( cTeamID, nSacks )

   if cTeamID = cHomeID
      nHSackCnt += nSacks
   else
      nASackCnt += nSacks
   endif

RETURN NIL


static;
FUNCTION IncrInts( cTeamID, nInts )

   if cTeamID = cHomeID
      nHIntCnt += nInts
   else
      nAIntCnt += nInts
   endif

RETURN NIL


static;
FUNCTION StoredStat( cStats )

   local lStored := .t.

   if cStats = "RETS"
      lStored = .f.
   endif

return lStored


static;
FUNCTION GetStat( cString )

	*---put the words into an array
	local cChar, cWord, i, nWords, lMinus := .f.

	aWords 	:= {}
	cWord 	:= ""
	nWords	:= 0

	for i = 1 to len( cString )
		cChar = substr( cString, i, 1 )
		if cChar = " " .or. cChar = "-"
			*---end of word
			if cWord = "minus" 
				lMinus = .t.
*				TestMsg( "Eeck  minus yardage" )
			else
				nWords++	 
				if lMinus
					*---Prior word was minus
					cWord := "-" + cWord
*					TestMsg( "Making word negative >" + cWord )
				endif
				aadd( aWords, cWord )
*				TestMsg( "Word " + str( nWords, 2 ) + " is " + cWord )
				lMinus = .f.
			endif

			cWord := ""
		else
			if .not. ( cChar $ ",()" )
				cWord += cChar
			endif

		endif

	next

	*---last one
	nWords++
	if lMinus
		*---Prior word was minus
		cWord := "-" + cWord
*		TestMsg( "Making word negative >" + cWord )
	endif
	aadd( aWords, cWord )
*	TestMsg( "Word " + str( nWords, 2 ) + " is " + cWord )

RETURN nWords


*EOF:  LOADWK3.PRG

