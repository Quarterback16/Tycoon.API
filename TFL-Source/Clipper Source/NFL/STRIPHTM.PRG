****************************************************************************
*
*		Strip HTML code out of a HTML file and create a text file
*
*
*	Start:  21-JAN-96			0935 -
*
*  Mods:	
*
****************************************************************************

#INCLUDE "NFL.H"


static cHtmlDir	:= "c:\download\"		//		Location of HTML files
static nHandle
static cLogFile	:= "LoadHTMLs"				//		Log file
static cSection	:= "TOP"						//		Current Section
static cLineType	:= "NO-OP"						//		Line type
static cGameCode	:= ""							//		TFL game code eg 199501A
static aWords		:= {}							//		Words in the line
static aInts		:= {}							//		Interceptions
static aHTML[ 50 ]

PROCEDURE StripHTM

	local nHTML	:= 0
	local b

	LOCAL GetList[1], oGet

	*---Load up names of HTMLscore files
	TestMsg( "Looking for " + cHtmlDir + "*.htm" )
	nHTML := aDir( cHtmlDir + "*.htm", aHTML )

	if nHTML = 0
		ErrorMsg( "No HTMLs found in " + cHtmlDir )
	else
		erase( cLogFile + ".LOG" )
*		TestMsg( str( nHTML, 4 ) + " HTML scores found" )
		for b = 1 to nHTML
			*---Process HTMLscore
			*---Reset
			DoHTML( cHtmlDir + aHTML[ b ] )
		next
	endif

RETURN


FUNCTION DoHTML( cHTMLFile )

	local nRecords  	:= 0			//		Record count
	local cLine  	   := ""			//		record string
	local cTxtFile   := ""			//		New TXT file
	local lThru			:= .f.

	local i, cErrMsg, cStartTime, getList

	cls()

	*---Open text file
	nHandle := fopen( cHTMLFile )

	if nHandle = -1
		TestMsg( "Could not open " + cHTMLFile )
		RETU .f.
	endif

	print( 03, 32, "Memory    : " + str( memory() ) )
	print( 04, 32, "HTML File  : " + cHTMLFile      )
	print( 05, 32, "Records   : " )
	cStartTime := time()
	cTxtFile := substr( cHtmlFile, 1, at( ".", cHtmlfile ) ) + "txt"
	TestMsg( "Text file " + cTxtFile )

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

		*---------------------------------------------
		*   Process the line
		*---------------------------------------------

		*---Get rid of HTML codes
		cLine		:= HTMLStrip( cLine )		//		Clean line
		print( 10, 05, cLine )
		*---Write the line to the Stripped file
		WriteTxt( cLine, cTxtFile )
	enddo

	print( 16, 20, "Records checked :" + str( nRecords, 7 ) )
	print( 17, 20, "Elapsed Time    :" + elaptime( cStartTime, time() ) )

	fclose( nHandle )

RETURN .t.


FUNCTION WriteTXT

	parameters _msg, cTxtFile

	private log_msg, nTxtHand

	log_msg = _msg + chr(13) + chr(10)

	nTxthand  = fopen( cTxtFile, 2 )

	if nTxtHand = -1
		*---File not found?
		nTxtHand = fcreate( cTxtFile )
	endif

	if nHandle <> -1
		fseek(  nTxtHand, 0, 2 )
		fwrite( nTxtHand, log_msg )
		fclose( nTxtHand )
	endif

RETURN nTxtHand
	





*EOF:  STRIPHTM.PRG

