****************************************************************************
*
*  Load a Internet Box scores into the TFL/NFL system
*
*		1)		Load box files into an Array
*		2)		For each box score
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
static nHomeSacks
static nAwaySacks

FUNCTION LoadWk2

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
	G_week	:= cWeek

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

	local nRecords  	:= 0			//		Record count
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

*	WriteLog( cLogFile, "Elapsed Time    :" + elaptime( cStartTime, time() ) )
	print( 16, 20, "Records checked :" + str( nRecords, 7 ) )
	print( 17, 20, "Elapsed Time    :" + elaptime( cStartTime, time() ) )

	fclose( nHandle )

RETURN .t.

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
*  TestMsg( "Linetype = " + cLineType )

   *---Identify the Section
   cSection	:= IDSection( cLineType, cSection )
*  TestMsg( "Section = " + cSection )

   do case
   case cLineType = "TEAMSACKS"
      *---Set the Sacks
      do SetSacks with cLine

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
		if cQuarter = "5"
			DoScore( cLine )
		endif

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
		   WriteLog( cStem, SCHED->HOMETEAM + " Home score wrong " + str( SCHED->HOMESCORE,2 ) +;
						   " to " + str( nHomeScore, 2 ) )

	   endif
	   if SCHED->AWAYSCORE <> nAwayScore
		   TestMsg( SCHED->AWAYTEAM + " score wrong " + str( SCHED->AWAYSCORE,2 ) +;
						   " to " + str( nAwayScore, 2 ) )
		   WriteLog( cStem, SCHED->AWAYTEAM + " score wrong " + str( SCHED->AWAYSCORE,2 ) +;
						   " to " + str( nAwayScore, 2 ) )
	   endif

   case cLineType = "AWAYRUSH"
	   Do2Stat( cLine, cAwayID, .t. )

   case cLineType = "HOMERUSH"
	   Do2Stat( cLine, cHomeID, .t. )

   case cLineType = "AWAYPASS"
	   DoPassing( cLine, cAwayID )

   case cLineType = "HOMEPASS"
	   DoPassing( cLine, cHomeID )

   case cLineType = "AWAYRECV"
	   Do2Stat( cLine, cAwayID, .f. )

   case cLineType = "HOMERECV"
	   Do2Stat( cLine, cHomeID, .f. )

   case cLineType = "AWAYSACK"
	   if DoSacks( cLine, cAwayID ) <> nAwaySacks
         TestMsg( cAwayID + "-Away sacks should be " + str( nAwaySacks, 3 ) )
         Writelog( cLogfile, cAwayID + "-Away sacks dont match up!" )
      endif

   case cLineType = "HOMESACK"
	   if DoSacks( cLine, cHomeID ) <> nHomeSacks
         TestMsg( cHomeID + "-Home sacks should be " + str( nHomeSacks, 3 ) )
         Writelog( cLogfile, cHomeID + "-Home sacks dont match up!" )
      endif

   case cLineType = "INTERCEPTS"
	   DoInts( cLine )

   case cLineType = "TRAILER"
	   *---Finish box
	   lThru := .t.
   endcase

*	TestMsg( "Line Type " + cLineType )

RETURN lThru


static ;
FUNCTION DoInts( cLine )

	local cStatLine, nWords, cTeamID, cTeam, cPlayer, lPlayer, nPlyrWord, cWord
	nPlyrWord	:= 0
	cTeamID		:= ""
	cPlayer		:= ""
	lPlayer		:= .f.
	cTeam			:= ""
	cWord			:= ""
	cStatLine 	:= GetStatLine( cLine, .t. )

	*---Convert any minus's
	cStatLine := strtran( cStatLine, "minus ", "-" )
	cStatLine := strtran( cStatLine, "MINUS ", "-" )

	nWords := ParseLine( cStatLine, ",;()" )
*	TestMsg( "nWords=" + str( nWords, 4 ) )
	for i = 2 to nWords
		*---Process word by word
		cTeam += aWords[ i ] + " "

*		TestMsg( "Word " + str( i, 3 ) + " is " + aWords[ i ] )

		if GotTeam( @cTeam, @cTeamID )
			lPlayer := .t.
			nPlyrWord	:= 0
         cPlayer     := ""
		else
			if lPlayer
				cPlayer += aWords[ i ] + " "
				nPlyrWord++
				cWord    := aWords[ i ]
			endif

*			TestMsg( "Player word " + str( nPlyrWord, 2 ) )

			if upper( cWord ) = "YARDS"
				IntStat( cPlayer, cTeamID )
				nPlyrWord	:= 0
				cTeam			:= ""
				cPlayer		:= ""
			endif
		endif

	next

RETURN nil


static ;
FUNCTION IntStat( cPlayer, cTeamID )

	local cPlayerID, nWords

*	TestMsg( "Parsing " + cPlayer + " of team " + cTeamID )

	nWords := ParseInts( cPlayer )

*	TestMsg( "nWords = " + str( nWords, 4 ) )

	if nWords = 6
		*---we have both first and second names
	   if val( aInts[ 3 ] ) > 0
		   cPlayerID = IDof( aInts[1], aInts[2], cTeamID )
		   SaveIndiv( cPlayerID, INTERCEPTIONS_MADE,	aInts[ 3 ], cTeamID )
	   endif
	else
	   if val( aInts[ 2 ] ) > 0
		   cPlayerID = IDof( "", aInts[1], cTeamID )
		   SaveIndiv( cPlayerID, INTERCEPTIONS_MADE,	aInts[ 2 ], cTeamID )
	   endif
	endif

RETURN nil


static ;
FUNCTION ParseInts( cLine, cSkipChar )

	*---put the words into an array
	local cChar, cWord, w, nWords

	if pcount() = 1
		cSkipChar := ","
	endif

	aInts 	:= {}
	cWord 	:= ""
	nWords	:= 0
	cLine		:= alltrim( cLine )

	for w = 1 to len( cLine )
		cChar := substr( cLine, w, 1 )
		if cChar = " "
			*---end of word
			nWords++
			aadd( aInts, cWord )
*			TestMsg( "Word " + str( nWords, 2 ) + " is " + cWord )
			cWord := ""
		else
			if .not. ( cChar $ cSkipChar )
				if w <= len( cLine )
					cWord += cChar
				endif
			endif

		endif

	next

	*---last one
	nWords++
	aadd( aInts, cWord )
*	TestMsg( "Word " + str( nWords, 2 ) + " is " + cWord )

RETURN nWords


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

	TestMsg( "Score line " + cStatLine )

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


FUNCTION DoPassing( cLine, cTeamID )

	local i, cStatLine, nStart, nWords, cStat1, cStat2, cStat3, cStat4
	cStat1 := cStat2 := "0"
	cStatLine := GetStatLine( cLine, .f. )

	*---Convert any minus's
	cStatLine := strtran( cStatLine, "minus ", "-" )
	cStatLine := strtran( cStatLine, "MINUS ", "-" )

	nWords := ParseLine( cStatLine )

	*---Determine where the stats begin
	nStart := iif( upper( right( aWords[2], 4 ) ) = "ING:", 3, 4 )
*   TestMsg( "nWords " + str( nWords, 2 ) )
*   TestMsg( "nstart " + str( nstart, 2 ) )
	for i = nStart to nWords
*		TestMsg( "WORD1 " + str(i,3) + "  " + aWords[i] )
*		TestMsg( "WORD2 " + str(i+1,3) + "  " + aWords[i+1] )
      if asc( substr( aWords[ i+1 ], 1, 1 ) ) >= 47 .and. ;
         asc( substr( aWords[ i+1 ], 1, 1 ) ) <= 58
         *---number
*			TestMsg( "Surname only" )
         cPlayerID = IDof( "", aWords[i], cTeamID )
   		Split4Stat( aWords[ i + 1 ], @cStat1, @cStat2, @cStat3, @cStat4 )
         i = i + 1
      else
         *---Its a surname
*			TestMsg( "Surname and first" )
         cPlayerID = IDof( aWords[i], aWords[i+1], cTeamID )
   		Split4Stat( aWords[ i + 2 ], @cStat1, @cStat2, @cStat3, @cStat4 )
         i = i + 2
      endif

		SaveIndiv( cPlayerID, PASS_COMPLETIONS,	cStat1, cTeamID )
		SaveIndiv( cPlayerID, PASSING_ATTEMPTS,	cStat2, cTeamID )
		SaveIndiv( cPlayerID, PASSING_YARDAGE,	   cStat3, cTeamID )
		SaveIndiv( cPlayerID, PASSES_INTERCEPTED,	cStat4, cTeamID )
	next
	
RETURN nil


static ;
FUNCTION SplitStat( cWord, cStat1, cStat2 )

	local nSpot

	nSpot  := at( "-", cWord ) - 1
	cStat1 := substr( cWord, 1, nSpot )
	cStat2 := substr( cWord, nSpot + 2, len( cWord ) - nSpot - 1 )

*	TestMsg( cWord + " split into " + cStat1 + " and " + cStat2 )

RETURN nil


static ;
FUNCTION Split3Stat( cWord, cStat1, cStat2, cStat3 )

	local nSpot

*	TestMsg( "Spliting " + cWord )
	nSpot  := at( "-", cWord )
	cStat1 := substr( cWord, 1, nSpot - 1 )

	*---Shave off stat1
	cWord  := substr( cWord, nSpot + 1 , len( cWord ) - nSpot )
*	TestMsg( "Shaved " + cWord )

	nSpot  := at( "-", cWord )
	cStat2 := substr( cWord, 1, nSpot - 1 )

	cStat3 := substr( cWord, nSpot + 1, len( cWord ) - nSpot )

*	TestMsg( "Split into "+cStat1+" and "+cStat2 + " and " + cStat3 )

RETURN nil


static ;
FUNCTION Split4Stat( cWord, cStat1, cStat2, cStat3, cStat4 )

	local nSpot

*	TestMsg( "Spliting " + cWord )
	nSpot  := at( "-", cWord )
	cStat1 := substr( cWord, 1, nSpot - 1 )

	*---Shave off stat1
	cWord  := substr( cWord, nSpot + 1 , len( cWord ) - nSpot )
*	TestMsg( "Shaved " + cWord )

	nSpot  := at( "-", cWord )
	cStat2 := substr( cWord, 1, nSpot - 1 )
	cWord  := substr( cWord, nSpot + 1 , len( cWord ) - nSpot )

	nSpot  := at( "-", cWord )
	cStat3 := substr( cWord, 1, nSpot - 1 )

	cStat4 := substr( cWord, nSpot + 1, len( cWord ) - nSpot )

*	TestMsg( "Split into "+cStat1+" and "+cStat2 + " and " + cStat3 ;
*	         + " and " + cStat4 )

RETURN nil


FUNCTION DoSacks( cLine, cTeamID )

	local cStatLine, nWords, cStat1, cStat2, cStat3, i, nStart
   local nSacks := 0
	cStat1 := cStat2 := cStat3 := "0"

	*---Get the whole line
	cStatLine := GetStatLine( cLine, .t. )
	cStatLine := FixLine( cStatLine )

*	Testmsg( "Length of Sack line " + str( len( cStatLine ), 5 ) )
*  WriteLog( cLogFile, cStatLine )

	nWords := ParseLine( cStatLine )

	*---Determine where the stats begin
	nStart := iif( upper( right( aWords[2], 5 ) ) = "SACKS", 4, 5 )
*	TestMsg( "nStart" + str( nStart, 4 ) )
	for i = nStart to nWords
*		TestMsg( "WORD1 " + str(i,3) + "  " + aWords[i] )
*		TestMsg( "WORD2 " + str(i+1,3) + "  " + aWords[i+1] )
      if asc( substr( aWords[ i+1 ], 1, 1 ) ) >= 47 .and. ;
         asc( substr( aWords[ i+1 ], 1, 1 ) ) <= 58
         *---number
   		Split3Stat( aWords[ i + 1 ], @cStat1, @cStat2, @cStat3 )
      else
         *---Its a surname
   		Split3Stat( aWords[ i + 2 ], @cStat1, @cStat2, @cStat3 )
      endif

		if val( cStat3 ) > 0
         if asc( substr( aWords[ i+1 ], 1, 1 ) ) >= 47 .and. ;
            asc( substr( aWords[ i+1 ], 1, 1 ) ) <= 58
            *---number
*			   TestMsg( "Surname only - " + aWords[i] )
            cPlayerID = IDof( "", aWords[i], cTeamID )
         else
            *---Its a surname
*			   TestMsg( "Surname and first - " + aWords[i] + " " + aWords[i+1] )
            cPlayerID = IDof( aWords[i], aWords[i+1], cTeamID )
         endif
*			TestMsg( cStat3 + " sacks for " + cPlayerID )
			SaveIndiv( cPlayerID, QUARTERBACK_SACKS,	cStat3, cTeamID )
         nSacks += val( cStat3 )
		endif
      if asc( substr( aWords[ i+1 ], 1, 1 ) ) >= 47 .and. ;
         asc( substr( aWords[ i+1 ], 1, 1 ) ) <= 58
         *---number
         i = i + 1
      else
         *---Its a surname
         i = i + 2
      endif
	next

*  testMsg( str( nSacks, 4 ) + " Sacks for " + cTeamID )

RETURN nSacks


FUNCTION Do2Stat( cLine, cTeamID, lRush )

	local cStatLine, nSpot, cStat1, cStat2, i, nStart
   local cSurname, cFirstName
	cStat1 := cStat2 := "0"

	*---Get the whole line
	cStatLine := GetStatLine( cLine, .f. )
   cStatLine := FixLine( cStatLine )

	*---Convert any minus's
	cStatLine := strtran( cStatLine, "minus ", "-" )
	cStatLine := strtran( cStatLine, "MINUS ", "-" )

	nWords := ParseLine( cStatLine )

	*---Determine where the stats begin
	if nWords < 2
		TestMsg( "Stat line short>" + cStatLine )
		nStart := 0
	else
      *---Handles Oakland rushing: vs Kansas City rushing:
		nStart := iif( upper( right( aWords[2], 4 ) ) = "ING:", 3, 4 )
*     TestMsg( "Starting at " + str( nStart ) + " right 4 of 2 is " + right( aWords[2], 4 ) )
	endif

	for i = nStart to nWords-2
      if asc( substr( aWords[ i+1 ], 1, 1 ) ) >= 47 .and. ;
         asc( substr( aWords[ i+1 ], 1, 1 ) ) <= 58
         *---numbers
         cPlayerID = IDof( "", aWords[i], cTeamID )
   		SplitStat( aWords[i+1], @cStat1, @cStat2 )
         i = i + 1
      else
         *---Its a surname
         cPlayerID = IDof( aWords[i], aWords[i+1], cTeamID )
   		SplitStat( aWords[i+2], @cStat1, @cStat2 )
         i = i + 2
      endif

		if lRush
			SaveIndiv( cPlayerID, RUSHING_CARRIES,	cStat1, cTeamID )
			SaveIndiv( cPlayerID, RUSHING_YARDS, 	cStat2, cTeamID  )
		else
			SaveIndiv( cPlayerID, PASSES_CAUGHT,		cStat1, cTeamID ) 
			SaveIndiv( cPlayerID, RECEPTION_YARDAGE,	cStat2, cTeamID ) 
		endif
	next

RETURN nil


FUNCTION LineType( cLine, cSection )

	local cLineType := "NO-OP"
	local cAPcode, cAPname

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
*					TestMsg( "Away team is " + cAwayID )
				else
					cHomeID 	:= cTeam
					cHomeAP	 	:= cAPcode
					cHomeAPname	:= trim( cAPname )
					cLineType 	:= "HOMETEAM"
*					TestMsg( "Home team is " + cHomeID )
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
			cScorer := substr( cLine, 1, 4 )
			if .not. empty( cScorer )
				if IsaCode( cScorer )
					cLineType := "SCORE"
				else
					if upper( cScorer ) = "TEAM" .or. ;
						upper( cScorer ) = "FIRS"
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
		if upper( substr( cLine, 1, 6 ) ) = "PLAYER"
			cLineType := "PLAYER STATS"
      else
         if upper( cLine ) = "SACKED"
   			cLineType := "TEAMSACKS"
         endif
		endif

	case cSection = "PLAYER STATS"
		*---Look for Individual stats
		cLineType := "NO-OP"
*		TestMsg( upper( cLine ) )
*		TestMsg( "Compare with " + cHomeAPname + " RUSHING" )
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
	Last change:  LL    7 Sep 98    9:28 pm
*/
PROCEDURE SetSacks( cLine )
   local nWords
   cLine := strtran( cLine, "-", " " )
   nWords := ParseLine( cLine )        &&  YARDS will get droped off
   nHomeSacks = val( aWords[ 4 ] )
   nAwaySacks = val( aWords[ 6 ] )

   if lSwapped
      nHomeSacks = val( aWords[ 6 ] )
      nAwaySacks = val( aWords[ 4 ] )
   endif

*  TestMsg( "Home sacks = " + str( nHomeSacks, 3 ) )
*  TestMsg( "Away sacks = " + str( nAwaySacks, 3 ) )

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

	cTeam := substr( cLine, 1, 4 )
	if cQuarter = "5"
		cTeam := substr( cLine, 5, 3 )
*     TestMsg( "Team is " + cTeam )
		cScoreLine := GetScoreLine( substr( cLine, 5, len( cLine ) - 4 ) )
*     TestMsg( "Scoreline is " + cScoreLine )
*     TestMsg( "Away AP code is " + cAwayAP )
	else
		cScoreLine := GetScoreLine( cLine )
	endif

	cTeamID := iif( cTeam = trim( cAwayAP ), cAwayID, cHomeID )
*	TestMsg( "Score for >" + cTeamId )

	*------------------------------------------------------
	*		Break up the line into an array of words
	*------------------------------------------------------
	nWords := ParseLine( cScoreLine, "?" )

	*---The time is the last word
	cTime 	:= TimeOf( cQuarter, aWords[ nWords ] )

*  if	cQuarter = "5"
*   	TestMsg( "nWOrds = " + str( nWords, 3 ) )
*   	TestMsg( "Time was " + cTime + " from >" + cScoreLine )
*  endif

   if aWords[3] <> "FG" .and. aWords[3] <> "TD"
      *--They forgot to put in the type of score
      TestMsg( "Inserting a TD in front of " + aWords[3] )
      ains( aWords, 3 )
      if nWords > 8
         aWords[ 3 ] = "TD,"
      else
         aWords[ 3 ] = "FG,"
      endif

      *---Fix Score line
      cScoreLine := ""
      for i = 1 to len( aWords )
         cScoreLine += aWords[ i ] + " "
      next
*     TestMsg( "New score line >" + cScoreLine )
   endif

*	TestMsg( "aWords[2] " + aWords[2] )    &&  a dash
*	TestMsg( "aWords[3] " + aWords[3] )
*	TestMsg( "nWords =  " + str( len( aWords ), 3 ) )
	do case
	case aWords[3] = "FG"
		*---FG
		if at( ".", cScoreLine ) > 0
			cScoreLine := substr( cScoreLine, 1, at( ".", cScoreline ) )
			nWords := ParseLine( cScoreLine )
		endif

*		TestMsg( "FG words " + str( nWords, 8 ) )
		if nWords = 7
         *--full name
			cPlyr1 	:= IDof( aWords[4], aWords[5], cTeamID )
			nDist		:= val( aWords[ 6 ] )
		else
			*---6 words (no firstname)
			cPlyr1 	:= IDof( "", aWords[4], cTeamID )
			nDist		:= val( aWords[ 5 ] )
		endif

		SaveScore( cWeek, cGameCode, cPlyr1, cTime, "3", nDist, "", cTeamID )

	case aWords[7] = "RUN" .or. aWords[6] = "RUN"
		*---TD run
		cRunnrStr := RecvrStr( cScoreLine )
*     TestMsg( "Runner is >" + cRunnrStr )
		if WordsIn( cRunnrStr ) = 2
			cPlyr1	:= IDof( aPlyr[1], aPlyr[2], cTeamID )
			nDist		:= val( aWords[ 6 ] )
		else
			*---No first name
			cPlyr1	:= IDof( "", aWords[4], cTeamID )
			nDist		:= val( aWords[ 5 ] )
		endif

		SaveScore( cWeek, cGameCode, cPlyr1, cTime, "R", nDist, "", cTeamID )
		DoPAT( cScoreLine )

	case aWords[7] = "PASS" .or. aWords[6] = "PASS"
		*---TD pass
*     TestMsg( "TD Pass" )
		cRecvrStr := RecvrStr( cScoreLine )
		if WordsIn( cRecvrStr ) = 2
			cPlyr1	:= IDof( aPlyr[1], aPlyr[2], cTeamID )
			nDist		:= val( aWords[ 6 ] )
		else
			*---No first name
			cPlyr1	:= IDof( "", aWords[4], cTeamID )
			nDist		:= val( aWords[ 5 ] )
		endif

		cQBStr := QBStr( cScoreLine )
*     TestMsg( "QB is " + cQBStr + " words " + str( WordsIn( cQBStr ), 2 ) )
		if WordsIn( cQBStr ) >= 2
			cPlyr2	:= IDof( aPlyr[1], aPlyr[2], cTeamID )
		else
			*---No surname
			cPlyr2	:= IDof( "", aPlyr[1], cTeamID )
		endif

		SaveScore( cWeek, cGameCode, cPlyr1, cTime, "P", nDist, cPlyr2, cTeamID )
		DoPAT( cScoreLine )

	case "FUMBLE" $ cLine
		*---Player 1 n fumble return
		cPlyr1 	:= IDof( aWords[4], aWords[5], cTeamID )
		nDist		:= val( aWords[ 6 ] )
		SaveScore( cWeek, cGameCode, cPlyr1, cTime, "F", nDist, "", cTeamID )
		DoPAT( cScoreLine )

	case "KICKOFF" $ cLine
		*---Kickoff return
		cPlyr1 	:= IDof( aWords[4], aWords[5], cTeamID )
		nDist		:= val( aWords[ 6 ] )
		SaveScore( cWeek, cGameCode, cPlyr1, cTime, "K", nDist, "", cTeamID )
		DoPAT( cScoreLine )

	case "SAFETY" $ cLine
		*---Safety
      if aWords[7] = "TACKLED" .and. aWords[8] = "BY"
         cPlyr1 := IDof( aWords[9], aWords[10], cTeamID )
      else
         TestMsg( "Safety words  7 and 8 " + aWords[7] + " " + aWords[8] )
         cPlyr1 := ""
      endif

		SaveScore( cWeek, cGameCode, cPlyr1, cTime, "S", 0, "", cTeamID )
		Writelog( cLogfile, "SAFETY!" )

	case "INTERCEPTION" $ cLine
*		TestMsg( "Interception by " +  aWords[4] + " " + aWords[5] )
		cPlyr1 	:= IDof( aWords[4], aWords[5], cTeamID )
		nDist		:= val( aWords[ 6 ] )
		SaveScore( cWeek, cGameCode, cPlyr1, cTime, "I", nDist, "", cTeamID )
		DoPAT( cScoreLine )

	case "PUNT" $ cLine
		cPlyr1 	:= IDof( aWords[4], aWords[5], cTeamID )
		nDist		:= val( aWords[ 6 ] )
		SaveScore( cWeek, cGameCode, cPlyr1, cTime, "T", nDist, "", cTeamID )
		DoPAT( cScoreLine )

	otherwise
		TestMsg( "Unknown score type! " + cLine )
		WriteLog( cLogFile, "Unknown score type! " + cLine )
	endcase

RETURN lThru


FUNCTION DoPAT( cLine )

	local cPatLine, nWords, cFirstName, cSurname

	*---Get the PAT part in brackets
	cPatLine := PickPAT( cLine )

	if "FAILED" $ cPatLine 
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
				*---QB Surname only
				cPlyr2 := IDof( "", aWords[1], cTeamID )		&&	Passer
				if aWords[5] = "FOR"
					cPlyr1 	:= IDof( "", aWords[4], cTeamID )		&& Receiver
				else
					*---6th word
					cPlyr1 	:= IDof( aWords[4], aWords[5], cTeamID )		&& Receiver
				endif

			case aWords[3] = "PASS"
				cPlyr2 := IDof( aWords[1], aWords[2], cTeamID )		&&	Passer
				if aWords[6] = "FOR"
					cPlyr1 	:= IDof( "", aWords[5], cTeamID )		&& Receiver
				else
					*---7th word
					cPlyr1 	:= IDof( aWords[5], aWords[6], cTeamID )		&& Receiver
				endif
			endcase

			SaveScore( cWeek, cGameCode, cPlyr1, cTime, ;
					SCORE_PAT_PASS, 2, cPlyr2, cTeamID )

		else
			if "RUN"	$ cPatLine
				if aWords[ 3 ] = "RUN"
   				cPlyr1 	:= IDof( aWords[1], aWords[2], cTeamID )
	   			SaveScore( cWeek, cGameCode, cPlyr1, cTime, ;
		   				SCORE_PAT_RUN, 2, "", cTeamID )
				else
					*---Surname only
   				cPlyr1 	:= IDof( "", aWords[1], cTeamID )
	   			SaveScore( cWeek, cGameCode, cPlyr1, cTime, ;
		   				SCORE_PAT_RUN, 2, "", cTeamID )
				endif
			endif
		endif
	endif

RETURN nil


FUNCTION TimeOf( cQuarter, cTime )

	local cTimeout

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
		if cChar = " "
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

	set century off

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
*	TestMsg( "Word1 = " + aWords[1] + " Word 2 " + aWords[2] )
	if upper( aWords[ 1 ] ) = "OT"
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

*  TestMsg( "Finding " + cFirstName + "," + cSurname )

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
*		TestMsg( "Surname " + cSurname + " not found" )
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

	for i = 1 to len( cLine )
		cChar := substr( cLine, i, 1 )
		if cChar = "("
			lStart := .f.
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



*EOF:  LOADWK2.PRG
