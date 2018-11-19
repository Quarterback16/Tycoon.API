****************************************************************************
*
*  Load an AP stats capture file into the TFL/NFL system
*
*
*	Oddities:-		Change Del Greco to DelGreco
*						Ensure no spurious characters
*						Convert fumble recover in the end zone to 0 fumble return
*
*	During the playoffs a team will appear more than once in the schedule
*	NewGame() needs to be re-written
*
*  Mods:	Aug-94	2 point Conversions!
*
****************************************************************************

static cStem
static nHandle		:= 0
static nOther		:= 0
static nRecords	:= 0
static nGames		:= 0
static cWeek		:= ""
static cGameCode	:= ""
static cTeamID		:= ""
static cHomeID		:= ""
static cAwayID		:= ""
static cHomeName	:= ""
static cAwayName	:= ""
static nAwayScore	:= 0
static nHomeScore	:= 0
static cQuarter	:= 0
static aWords
static cStats

#INCLUDE "NFL.H"

FUNCTION LoadWeek

	*---Get the file name
	LOCAL GetList[1], cSTAfile := "WEEK", oGet
	cSTAfile += G_week
	@ 01,00 clear to 24,79
	@ 10, 30 say "Stat File :"
	SetPos(10, 50)
	GetList[1] = _GET_(cSTAfile, "cSTAfile", "@!",,)
	set cursor on
	ReadModal(GetList) 
	set cursor off
	GetList := {}

	select TEAM
	set order to 4
	select SCHED
	set order to 2

	cStem 	:= cStaFile
	cWeek    := substr( cStem, 5, 2 )
	G_week   := cWeek
	cStaFile := ".\BOX\" + G_season + "\" + cStaFile + ".CAP"

	*---Get rid of old log file
	if file( cStem + ".log" )
		erase( cStem + ".log" )
	endif

	LoadWk( cSTAFile )

	select SCHED
	set order to 1
	select TEAM
	set order to 1
	select PLAYER
	set order to 1

RETURN nil


FUNCTION LoadWk( cSTAFile )

	local lCorrect  := .t.		//		Return value
	local lThru     := .f.
	local nRecords  := 0			//		Record count
	local cLine     := ""		//		record string
	local i, cErrMsg, cStartTime, getList

	cls()

	*---Open text file
	nHandle := fopen( cSTAFile )

	if nHandle = -1
		TestMsg( "Could not open " + cSTAFile )
		RETU .f.
	endif

	print( 03, 52, "Memory    : " + str( memory() ) )
	print( 04, 52, "Week File : " + cStem      )
	print( 05, 52, "Records : " )
	cStartTime := time()

	do while .not. lThru
		*---Read a line
		cLine := freadline( nHandle )
		if feof( nHandle )
			exit
		else
			cLine :=	alltrim( cLine )
			nRecords++
			print( 10, 05, cLine )
			print( 05, 70, str( nRecords, 7 ) )
		endif

 		if .not. empty( cLine )
			*---Do something with the line
			if NewGame( cLine )
				*---We have the start of a game
				nGames++
			else
*				TestMsg( "No game" )
			endif
		endif
	enddo

	WriteLog( cStem, "Elapsed Time    :" + elaptime( cStartTime, time() ) )
	WriteLog( cStem, "Box Scores      :" + str( nGames,   7 ) )
	print( 16, 20, "Box Scores      :" + str( nGames,   7 ) )
	print( 16, 20, "Records checked :" + str( nRecords, 7 ) )
	print( 17, 20, "Elapsed Time    :" + elaptime( cStartTime, time() ) )

	wait

	fclose( cSTAFile )

RETURN lCorrect


FUNCTION NewGame( cLine )

	local lGameOn 	:= .f.
	local cAwayTeam		:= ""
	local cHomeTeam		:= ""

	cAwayTeam := PickTeam( cLine )

	if TEAM->( dbseek( G_season + cAwayTeam ) )
		lGameOn := .t.
		cAPaway   	:= trim( TEAM->APCODE )
		cAwayName 	:= trim( TEAM->APNAME )
		cAwayID		:= TEAM->TEAMID
*		TestMsg( "Away team is " + cAwayName )
		nAwayScore 	:= ScoreOf( cLine )

		*---Who is the Home team?
		cLine 		:= freadline( nHandle )
		nHomeScore 	:= ScoreOf( cLine )
		cHomeTeam	:= PickTeam( cLine )

		if TEAM->( dbseek( G_season + cHomeTeam ) )

			cAPhome   	:= trim( TEAM->APCODE )
			cHomeName 	:= trim( TEAM->APNAME )
			cHomeID		:= TEAM->TEAMID
*			TestMsg( "Home team is " + cHomeName )

			*---Now which game is it?
			if SCHED->( dbseek( G_season + cWeek + cAwayID + cHomeID ) )
				cGameCode 	:= SCHED->GAMENO
			else
				TestMsg( cAwayID + " @ " + cHomeID + " Game not scheduled " )
			endif

			select TEAM
			set order to 4

			if SCHED->( dbseek( G_season + cWeek + cAwayID ) )
	*			TestMsg( "Saving Score " + cAwayID + str( nAwayScore, 4 ) +;
	*					" @ " + cHomeID + str( nHomeScore, 4 ) )
				replace SCHED->AWAYSCORE	with nAwayScore
				replace SCHED->HOMESCORE	with nHomeScore
			endif

			cMsg	:=	"Game " + cGameCode + "  " + cAwayID + str( nAwayScore, 4 ) +;
						" @ " + cHomeID + str( nHomeScore, 4 )
			WriteLog( cStem, cMsg )

			ClrScores( cGameCode )

			LoadGame()
		else

			TestMsg( cHomeTeam + " (home) not in the NFL" )
		endif
	else
		TestMsg( cAwayTeam + " (away) not in the NFL" )
		lGameOn := .f.
	endif

RETURN lGameOn


FUNCTION ClrScores( cGameCode )

	*---Wipes out any prior line scores
*	TestMsg( "Wiping " + G_season + " wk " + cWeek + " game " + cGameCode )
	local nCount := 0

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
		if .NOT. ( STAT->STAT $ DEFENSIVE_STATS )
			delete
			nCount++
		endif

		skip
	enddo

*	TestMsg( str( nCount, 7 ) + " stats wiped" )

RETURN nil


FUNCTION LoadGame()

	*---Load an individual game

	local lThru			:= .f.
	local cLine     	:= ""		//		record string
	local lFirstTime 	:= .t.
	private cGameKey

	do while .not. lThru
		*---Read a line
		cLine := freadline( nHandle )
		if feof( nHandle )
			lThru := .t.
			exit
		else
			nRecords++
			print( 10, 05, cLine )
			print( 05, 70, str( nRecords, 7 ) )
		endif

		cLine := alltrim( cLine )
		if cLine = "MISSED FIELD GOAL"
			*---End of Game
			lThru := .t.
		else

			if .not. empty( cLine )

				*---We have a score
				lThru := LoadScore( cLine )

			endif
		endif

		if lThru
			*---Double check scores
			nHomeScore := nAwayScore := 0
			cGameKey   := G_Season + cWeek + cGameCode
			TheScore( cGameKey )		&&		in Scores.prg
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
		endif

	enddo

RETURN lThru


static FUNCTION TheScore( cGameKey )

	local nOldArea

	store 0 to nHomeScore, nAwayScore

	nOldArea = select()

	select SCORE
	seek cGameKey
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


FUNCTION LoadScore( cLine )

	*---dismantle the line to find the line scores
	local lThru := .f., cPlyr1, cPlyr2, nWords

	if cLine = "A--"
		lThru := LoadIndiv()
	else
		if "Quarter" $ cLine .or. upper( cLine ) = "OVERTIME"
			*---Determine quarter
			do case
			case cLine = "First"
				cQuarter = "1"
			case cLine = "Second"
				cQuarter = "2"
			case cLine = "Third"
				cQuarter = "3"
			case cLine = "Fourth"
				cQuarter = "4"
			otherwise
				cQuarter = "5"
			endcase
			*		TestMsg("Quarter " + cQuarter)
		else
			*---A score line
			if ScoreLine( cLine )
				cTeam = substr( cLine, 1, at( "-", cLine )-1 )
				*		TestMsg( "A score for >" + cTeam )
				cTeamID = iif( cTeam == cAPaway, cAwayID, cHomeID )
				*		TestMsg( "Aka >" + cTeamId )
				*------------------------------------------------------
				*		Break up the line into an array of words
				*------------------------------------------------------
				nWords := ParseLine( substr( cLine, at( "-", cLine )+2, ;
						len( cLine ) - ( len( cTeam ) + 2 ) ) )

				cTime 	:= TimeOf( cQuarter, aWords[ nWords ] )

				do case
				case aWords[3] = "run"
					*---TD run
					cPlyr1 	:= IDof( aWords[1], cTeamID, .t. )
					nDist		:= val( aWords[ 2 ] )
					SaveScore( cWeek, cGameCode, cPlyr1, cTime, "R", nDist, "", cTeamID )
					if aWords[ nWords-1 ] = "kick"
						*---Conversion kick
						cPlyr1 	:= IDof( aWords[nWords-2], cTeamID, .f. )
						SaveScore( cWeek, cGameCode, cPlyr1, cTime, "1", 0, "", cTeamID )
					else
						if aWords[ nWords-3 ] = "pass"
							*---2 pt conversion
							cPlyr1 	:= IDof( aWords[nWords-4], cTeamID, .t. )
							cPlyr2 	:= IDof( aWords[nWords-1], cTeamID, .t. )
							SaveScore( cWeek, cGameCode, cPlyr1, cTime, ;
									SCORE_PAT_PASS, 2, cPlyr2, cTeamID )

						else
							if aWords[ nWords-1 ] = "run"
								cPlyr1 	:= IDof( aWords[nWords-2], cTeamID, .f. )
								SaveScore( cWeek, cGameCode, cPlyr1, cTime, ;
										SCORE_PAT_RUN, 2, "", cTeamID )
							endif
						endif
					endif

				case "FG" $ cLine
					*---FG
					cPlyr1 	:= IDof( aWords[2], cTeamID, .t. )
					nDist		:= val( aWords[ 3 ] )
					SaveScore( cWeek, cGameCode, cPlyr1, cTime, "3", nDist, "", cTeamID )

				case aWords[3] = "pass"
					*---TD pass
					nDist		:= val( aWords[ 2 ] )
					cPlyr1 	:= IDof( aWords[1], cTeamID, .t. )
					cPlyr2 	:= IDof( aWords[5], cTeamID, .t. )
					SaveScore( cWeek, cGameCode, cPlyr1, cTime, "P", nDist, cPlyr2, cTeamID )
					if aWords[ nWords-1 ] = "kick"
						*---Conversion kick
						cPlyr1 	:= IDof( aWords[nWords-2], cTeamID, .f. )
						SaveScore( cWeek, cGameCode, cPlyr1, cTime, "1", 0, "", cTeamID )
					else
						if aWords[ nWords-3 ] = "pass"
							*---2 pt conversion
							cPlyr1 	:= IDof( aWords[nWords-4], cTeamID, .t. )
							cPlyr2 	:= IDof( aWords[nWords-1], cTeamID, .t. )
							SaveScore( cWeek, cGameCode, cPlyr1, cTime, ;
									SCORE_PAT_PASS, 2, cPlyr2, cTeamID )

						else
							if aWords[ nWords-1 ] = "run"
								cPlyr1 	:= IDof( aWords[nWords-2], cTeamID, .f. )
								SaveScore( cWeek, cGameCode, cPlyr1, cTime, ;
										SCORE_PAT_RUN, 2, "", cTeamID )
							endif
						endif
					endif

				case "fumble" $ cLine
					*---Player 1 n fumble return
					nDist		:= val( aWords[ 2 ] )
					cPlyr1 	:= IDof( aWords[1], cTeamID, .f. )
					SaveScore( cWeek, cGameCode, cPlyr1, cTime, "F", nDist, "", cTeamID )
					if aWords[ nWords-1 ] = "kick"
						*---Conversion kick
						cPlyr1 	:= IDof( aWords[nWords-2], cTeamID, .f. )
						SaveScore( cWeek, cGameCode, cPlyr1, cTime, "1", 0, "", cTeamID )
					else
						if aWords[ nWords-3 ] = "pass"
							*---2 pt conversion
							cPlyr1 	:= IDof( aWords[nWords-4], cTeamID, .t. )
							cPlyr2 	:= IDof( aWords[nWords-1], cTeamID, .t. )
							SaveScore( cWeek, cGameCode, cPlyr1, cTime, ;
									SCORE_PAT_PASS, 2, cPlyr2, cTeamID )

						else
							if aWords[ nWords-1 ] = "run"
								cPlyr1 	:= IDof( aWords[nWords-2], cTeamID, .f. )
								SaveScore( cWeek, cGameCode, cPlyr1, cTime, ;
										SCORE_PAT_RUN, 2, "", cTeamID )
							endif
						endif
					endif

				case "kickoff" $ cLine
					*---Kickoff return
					nDist		:= val( aWords[ 2 ] )
					cPlyr1 	:= IDof( aWords[1], cTeamID, .f. )
					SaveScore( cWeek, cGameCode, cPlyr1, cTime, "K", nDist, "", cTeamID )
					if aWords[ nWords-1 ] = "kick"
						*---Conversion kick
						cPlyr1 	:= IDof( aWords[nWords-2], cTeamID, .f. )
						SaveScore( cWeek, cGameCode, cPlyr1, cTime, "1", 0, "", cTeamID )
					else
						if aWords[ nWords-3 ] = "pass"
							*---2 pt conversion
							cPlyr1 	:= IDof( aWords[nWords-4], cTeamID, .t. )
							cPlyr2 	:= IDof( aWords[nWords-1], cTeamID, .t. )
							SaveScore( cWeek, cGameCode, cPlyr1, cTime, ;
									SCORE_PAT_PASS, 2, cPlyr2, cTeamID )

						else
							if aWords[ nWords-1 ] = "run"
								cPlyr1 	:= IDof( aWords[nWords-2], cTeamID, .f. )
								SaveScore( cWeek, cGameCode, cPlyr1, cTime, ;
										SCORE_PAT_RUN, 2, "", cTeamID )
							endif
						endif
					endif

				case "Safety" $ cLine
					*---Safety
					SaveScore( cWeek, cGameCode, "", cTime, "S", 0, "", cTeamID )

				case "interception" $ cLine
					nDist		:= val( aWords[ 2 ] )
					cPlyr1 	:= IDof( aWords[1], cTeamID, .f. )
					SaveScore( cWeek, cGameCode, cPlyr1, cTime, "I", nDist, "", cTeamID )
					if aWords[ nWords-1 ] = "kick"
						*---Conversion kick
						cPlyr1 	:= IDof( aWords[nWords-2], cTeamID, .f. )
						SaveScore( cWeek, cGameCode, cPlyr1, cTime, "1", 0, "", cTeamID )
					else
						if aWords[ nWords-3 ] = "pass"
							*---2 pt conversion
							cPlyr1 	:= IDof( aWords[nWords-4], cTeamID, .t. )
							cPlyr2 	:= IDof( aWords[nWords-1], cTeamID, .t. )
							SaveScore( cWeek, cGameCode, cPlyr1, cTime, ;
									SCORE_PAT_PASS, 2, cPlyr2, cTeamID )

						else
							if aWords[ nWords-1 ] = "run"
								cPlyr1 	:= IDof( aWords[nWords-2], cTeamID, .f. )
								SaveScore( cWeek, cGameCode, cPlyr1, cTime, ;
										SCORE_PAT_RUN, 2, "", cTeamID )
							endif
						endif
					endif

				case "punt" $ cLine
					nDist		:= val( aWords[ 2 ] )
					cPlyr1 	:= IDof( aWords[1], cTeamID, .f. )
					SaveScore( cWeek, cGameCode, cPlyr1, cTime, "T", nDist, "", cTeamID )
					if aWords[ nWords-1 ] = "kick"
						*---Conversion kick
						cPlyr1 	:= IDof( aWords[nWords-2], cTeamID, .f. )
						SaveScore( cWeek, cGameCode, cPlyr1, cTime, "1", 0, "", cTeamID )
					else
						if aWords[ nWords-3 ] = "pass"
							*---2 pt conversion
							cPlyr1 	:= IDof( aWords[nWords-4], cTeamID, .t. )
							cPlyr2 	:= IDof( aWords[nWords-1], cTeamID, .t. )
							SaveScore( cWeek, cGameCode, cPlyr1, cTime, ;
									SCORE_PAT_PASS, 2, cPlyr2, cTeamID )

						else
							if aWords[ nWords-1 ] = "run"
								cPlyr1 	:= IDof( aWords[nWords-2], cTeamID, .f. )
								SaveScore( cWeek, cGameCode, cPlyr1, cTime, ;
										SCORE_PAT_RUN, 2, "", cTeamID )
							endif
						endif
					endif

				otherwise
					TestMsg( "Unknown score type! " + cLine )
				endcase
			endif

		endif
	endif

RETURN lThru


FUNCTION TimeOf( cQuarter, cTime )

	local cTimeout

	cTimeout := cQuarter + "-" + right( "00" + cTime, 5 )

RETURN cTimeOut


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
		if cChar = "," .or. ( cChar = " " .and. right( cWord, 1 ) = "." ) ;
							.or. ( cChar = "-" .and. right( cWord, 1 ) = "-" )

			*---end of word
			nWords++
			if cWord = "RUSHING"
				cStats = "RUSHING"
			else
				if cWord = "PASSING"
					cStats = "PASSING"
				else
					if cWord = "RECEIVING"
						cStats = "RECEIVING"
					else
						*---Not a stat type
*						TestMsg( "Eat Word " + str( nWords, 2 ) + " is " + cWord )
						if alltrim( cWord ) == "New" .or. alltrim( cWord ) == "Los"
							*---Teams skip first part
						else
							if alltrim( cWord ) = "Angeles" .or. ;
								alltrim( cWord ) = "Los Angeles"
								*---An LA team
								if cHomeName = "L.A. Raiders" .or. ;
									cAwayName = "L.A. Raiders"
									cWord := "L.A. Raiders"
								else
									cWord := "L.A. Rams"
								endif
							else
								*---A NY team
								if alltrim( cWord ) = "York"	.or. ;
									alltrim( cWord ) = "New York" .or. ;
									alltrim( cWord ) = "Giants"
									if cHomeName = "N.Y. Giants" .or. ;
										cAwayName = "N.Y. Giants"
										cWord := "N.Y. Giants"
*										TestMsg( "Eat word Converted to N.Y. Giants" )
									else
*										TestMsg( "Eat word Converted to N.Y. Jets" )
										cWord := "N.Y. Jets"
									endif
								else
									if alltrim( cWord ) = "Jets"
*										TestMsg( "Eat word Converted to N.Y. Jets" )
										cWord := "N.Y. Jets"
									endif
								endif
							endif

							EatWord( cWord )
						endif

					endif
				endif
			endif

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


FUNCTION EatWord( cWord )

	local cPlayerID 	:= ""
	local nWords		:= 0

	cWord := alltrim( cWord  )

	if empty( cWord )
		*---skip it
	else

		if cAwayName $ cWord
			cTeamID = cAwayID
		else
*			TestMsg( cAwayName + " not in " + cWord )
			if cHomeName $ cWord
				cTeamID = cHomeID
			else
				*---Not a team
*				TestMsg( cHomeName + " not in " + cWord )
				nWords := GetStat( cWord )

				cPlayerID := Idof( aWords[1], cTeamID, .t. )
				*---Uncomment next line for debugging
*				WriteLog( cStem, aWords[1] + "  " + cPlayerID )
				do case
				case cStats = "RUSHING"

					if nWords < 3
						WriteLog( cStem, "? RUSHING  " + cPlayerID )
					else
						SaveIndiv( cPlayerID, RUSHING_CARRIES,	aWords[ 2 ] )
						SaveIndiv( cPlayerID, RUSHING_YARDS, 	aWords[ 3 ] )
					endif

				case cStats = "PASSING"

					if nWords < 5
						WriteLog( cStem, "? PASSING  " + cPlayerID )
					else
						SaveIndiv( cPlayerID, PASS_COMPLETIONS,	aWords[ 2 ] )
						SaveIndiv( cPlayerID, PASSING_ATTEMPTS,	aWords[ 3 ] )
						SaveIndiv( cPlayerID, PASSES_INTERCEPTED,	aWords[ 4 ] )
						SaveIndiv( cPlayerID, PASSING_YARDAGE,		aWords[ 5 ] )
					endif

				case cStats = "RECEIVING"

					if nWords < 3
						WriteLog( cStem, "? RECEIVING  " + cPlayerID )
					else
						SaveIndiv( cPlayerID, PASSES_CAUGHT,		aWords[ 2 ] ) 
						SaveIndiv( cPlayerID, RECEPTION_YARDAGE,	aWords[ 3 ] ) 
					endif
				endcase
			endif
		endif
	endif

RETURN nil


FUNCTION SaveIndiv( cPlayerID, cCat, cAmount )

	if .not. empty( cPlayerID )
*		WriteLog( cStem, cWeek + "," + cGameCode + "," + cPlayerid + " , " + ;
*							cCat + ", " + str( val( cAmount ), 5, 1 ) + " " + cTeamID )

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

RETURN nil


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


FUNCTION ScoreLine( cLine )

	local lScore := .t.

	if at( "-", cLine ) = 0
		lScore := .f.
	else

		if cLine = "A--"
			*---Attendance line
			lScore := .f.
		else
			if cLine = "---"
				lScore := .f.
			endif
		endif
	endif

RETURN lScore


FUNCTION SaveScore( cWeek, cGameCode, cPlyr1, cTime, cType, nDist, cPlyr2, cTeamID )

*	WriteLog( cStem, cWeek + "," + cGameCode + "," + cPlyr1 + " , " + ;
*					"," + cTime + "," + cType +","+ str( nDist,3) +;
*					+ "," + cPlyr2 + "," + cTeamID )

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

RETURN nil


FUNCTION ParseLine( cLine )

	*---put the words into an array
	local cChar, cWord, i, nWords

	aWords 	:= {}
	cWord 	:= ""
	nWords		:= 0

	for i = 1 to len( cLine )
		cChar = substr( cLine, i, 1 )
		if cChar = " "
			*---end of word
			nWords++
			aadd( aWords, cWord )
*			TestMsg( "Word " + str( nWords, 2 ) + " is " + cWord )
			cWord := ""
		else
			if .not. ( cChar $ ",()" )
				if i <> len( cLine )
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


FUNCTION IdOf( cName, cTeamIn, lOff )

	local cPlayerID := space(8)
	local cSurname, cInits, nFullStop, cTeam

	***
	***	Eg  cName = "S.Colonna"	- given name and team what is the ID?
	***
*	if cName = "Hill" .and. cTeamIn = "SL"
*		TestMsg( "HILL" )
*	endif

	*---Surname?
	nFullStop 	:= at( ".", cName )			&&		eg 2
	if nFullStop = 0
		cSurName := cName
		cInits	:= ""
	else
		cInits	:= substr( cName, 1, nFullstop - 1 )				&&		eg S
		cSurName := right( cName, len( cName ) - nFullStop )		&&	Colonna
	endif

*	if cName = "K."
*		TestMsg( "cName   = " + cName )
*		TestMsg( "Surname = " + cSurname )
*		TestMsg( "Inits   = " + cInits   )
*		TestMsg( "Team    = " + cTeamIn  )
*	endif

	select PLAYER
	set order to 2
	if PLAYER->( dbseek( cSurname ) )
		do while .not. PLAYER->( eof() ) ;
					.and. PLAYER->SURNAME = cSurname
							
			if trim( PLAYER->FIRSTNAME ) = cInits
				if cTeamIn = PLAYER->CURRTEAM
					if lOff .and. empty( cInits )
						if PLAYER->CATEGORY $ "1234"
							cPlayerID = PLAYER->PLAYERID
						endif
					else
						cPlayerID = PLAYER->PLAYERID
						exit
					endif
				endif
			else
*				TestMsg( "inits dont match" )
			endif

			PLAYER->( dbskip() )
		enddo
	endif

*	TestMsg( "ID for " + cName + " is " + cPlayerID )

	if empty( cPlayerID )
		TestMsg( "ID not found for for " + cName )
		WriteLog( cStem, "Could not find ID for " + cName + " of " + cTeamIn  )
	endif

RETURN cPlayerID


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
	nLen := len( cLine )
	nSpot := at( "-", cLine )

	cScore := substr( cLine, nSpot+2, nLen - nSpot + 1 )

RETURN abs( val( cScore ) )


static ;
FUNCTION PickTeam( cLine )

	local nVal		:=	0
	local nPtr		:=	1
	local cChar		:= ""
	local cTeam		:= ""

	*---Look for a Team name
	do while nPtr <= len( cLine )
		cChar := substr( cLine, nPtr, 1 )
		nVal 	:= asc( cChar )
		if nVal >= 48 .and. nVal <= 57
			*---We've hit a number
			exit
		else
			cTeam	+= cChar
			nPtr++
		endif
	enddo

	cTeam := trim( cTeam )

RETURN cTeam



*EOF:  LOADWEEK.PRG
