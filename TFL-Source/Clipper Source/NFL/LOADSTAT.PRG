*****************************************************************************
*  Program Name.....: LOADSTAT.PRE
*  Programmer.......: SC
*  System...........: TYCOON
*  Module...........: NFL
*  Date.............: 12/09/91 @ 09:15:39
*  Copyright........: O & O Systems Consultants
*  Called by........: 
*                     
*  Purpose..........: Quick way to add Stats to the Stat file such as Sacks
*
/*
	Structure for database : stat.dbf
	Field  Field Name  Type       Width    Dec
	    1  SEASON      Character      4
	    2  WEEK        Character      2
	    3  GAMENO      Character      1
	    4  TEAMID      Character      2
	    5  PLAYERID    Character      8
	    6  STAT        Character      1
		 7  QTY         Numeric        6      1


	Structure for database : player.dbf
	Field  Field Name  Type       Width    Dec
	    1  PLAYERID    Character      8
	    2  SURNAME     Character     15
	    3  FIRSTNAME   Character     10
	    4  CURRTEAM    Character      2
	    5  HEIGHT_FT   Numeric        1      0
	    6  HEIGHT_IN   Numeric        2      0
	    7  WEIGHT      Numeric        3      0
	    8  FORTY       Numeric        6      2
	    9  COLLEGE     Character     15
	   10  ROOKIEYR    Numeric        4      0
	   11  POSDESC     Character     10
	   12  CATEGORY    Character      1
	   13  DOB         Date           8
	   14  CURRATING   Numeric        4      0
	   15  BIO         Memo          10

	Program will initialise Season, Week, Stat and Categories to Browse

		A tBrowse object will be displayed with:-
			F2 search option
			Update option on Enter
			Xit option

	Update will get any existing stat for the selected player and allow
	user to overwrite the stat.

	Last change:  LL    6 Feb 100    4:25 pm
*/
*...........................................................................

#include "..\lib\tfl.h"

static bMatchKey			//		Code block for matching filter

PROCEDURE LoadStat

	private cSeason, cWeek, cStat, cCats

	*---Screen
	DrawScreen()

	*---Gets
	if GetCrit()
		*---Browse

*		bMatchKey = { || PLAYER->CATEGORY $ trim( cCats ) }
		bMatchKey = { || .t. }

		select PLAYER

		Browse( 6, 6, 13, MaxCol() - 6, cCats )

		G_Season := cSeason
		G_Week   := cWeek

	endif


RETURN


FUNCTION GetCrit

	*---Initialise
	set cursor on
	cSeason := G_season
	cWeek   := G_week
	cStat   := QUARTERBACK_SACKS
	cCats   := "56    "

	print( 03, 05, "Season:",     GREEN_ON_BLACK )
	print( 03, 20, "Week:",       GREEN_ON_BLACK )
	print( 03, 40, "Stat:",       GREEN_ON_BLACK )
	print( 03, 55, "Categories:", GREEN_ON_BLACK )

	setcolor( C_FIELDS )
	vget( 03, 12, 'cSeason', "XXXX",   .f. )
	vget( 03, 26, 'cWeek',   "XX",     .f. )
	vget( 03, 46, 'cStat',   "!",      .t. )
	vget( 03, 70, 'cCats',   "XXXXXX", .f. ) 

	vread( 1, .f., 'VAL_TALL' )

return( lastkey() <> ESCAPE )


FUNCTION DrawScreen

	clear_Scn()

	head_line( 'LOAD STATS' )

   ScreenLine(  1 )


RETURN( nil )


//#include "setcurs.ch"

#define HSEP_CHARACTERS			"мям"
#define CSEP_CHARACTERS			" Ё "

static FUNCTION Browse( nTop, nLeft, nBottom, nRight, cFilter )

	local b, column, cType, n
	local cColorSave := setColor(), nCursSave
	local lMore, nKey

	*---Position at first record
*	GoEnd( "top", left( alltrim( cCats ), 1 ) )

	/* make new browse object */
	b := TBrowseDB( nTop, nLeft, nBottom, nRight )

	/* default heading and column separators */
	b:headSep := HSEP_CHARACTERS
	b:colSep  := CSEP_CHARACTERS

	/* add custom 'skipper' (to handle append mode) */
	b:skipBlock := { |x| SkippIt( x, cFilter ) }

	/* colors */
	b:colorSpec := "N/W, N/BG, B/W, B/BG, B/W, B/BG, R/W, B/R"

	/* add a column for each field in the current workarea */
	for n = 1 to FCount()

		/* make the new column */
		column := TBColumnNew( 	FieldName(n), ;
								FieldWBlock(FieldName(n), Select()) )

		/* evaluate the block once to get the field's data type */
		cType := ValType( Eval(column:block) )

		/* if numeric, use a color block to highlight negative values */
		if ( cType == "N" )
			column:defColor := {5, 6}
			column:colorBlock := {|x| if( x < 0, {7, 8}, {5, 6} )}

		else
			column:defColor := {3, 4}

		end

		b:addColumn(column)
	next

*	/* freeze leftmost column (PLAYERID) */
	b:freeze := 1

*	if .not. empty( cFilter )
*		b:goTopblock    := { |x| GoEnd( "top", cFilter ) }
*		b:goBottomblock := { |x| GoEnd( "bot", cFilter ) }
*	endif

*	nCursSave := SetCursor(0)

	lMore := .t.
	while ( lMore )

		/* don't allow cursor to move into frozen columns */
		if ( b:colPos <= b:freeze )
			b:colPos := b:freeze + 1
		end

		/* stabilize the display */
		while ( !b:stabilize() )
			nKey := InKey()
			if ( nKey != 0 )
				exit 			/* (abort if a key is waiting) */
			end
		end

		if ( b:stable )
			/* display is stable */
	  		if ( b:hitTop .or. b:hitBottom )
	  			Tone(125, 0)
	  		end

	  		/* everything's done; just wait for a key */
	  		nKey := InKey(0)

		end


		/* process key */
		do case
		case ( nKey == K_DOWN )

			b:down()

		case ( nKey == K_UP )

			b:up()

		case ( nKey == K_PGDN )
			b:pageDown()

		case ( nKey == K_PGUP )
			b:pageUp()

		case ( nKey == K_CTRL_PGUP )
 			b:goTop()
*		  	GoEnd( "top", left( alltrim( cCats ), 1 ) )

		case ( nKey == K_CTRL_PGDN )
			b:goBottom()
*			GoEnd( "Bott"  )

		case ( nKey == K_RIGHT )
			b:right()

		case ( nKey == K_LEFT )
			b:left()

		case ( nKey == K_HOME )
			b:home()

		case ( nKey == K_END )
			b:end()

		case ( nKey == K_CTRL_LEFT )
			b:panLeft()

		case ( nKey == K_CTRL_RIGHT )
			b:panRight()

		case ( nKey == K_CTRL_HOME )
			b:panHome()

		case ( nKey == K_CTRL_END )
			b:panEnd()

		case ( nKey == K_ESC )
			lMore := .f.

		case ( nKey == K_F2 )

			FindPlyr( PLAYER->PLAYERID )
			b:refreshAll()

		case ( nKey == K_RETURN )

			*---Edit it
			EditPlyr( PLAYER->PLAYERID )

		otherwise


		end

	end

*	SetCursor(nCursSave)

	SetColor( cColorSave )

RETURN(.t.)


function SkippIt( n, cFilter )

	local i := 0

	if ( LastRec() != 0 )

		if ( n == 0 )
			skip 0

		elseif ( n > 0 .and. Recno() != LastRec()+1 )

			while ( i < n )
				skip 1
				if ( Eof() .or. !eval( bMatchKey ) )

					skip -1

					exit
				end

				i++
			end

		elseif ( n < 0 )
			while ( i > n )
				skip -1
				if ( Bof() )
					exit 

				elseif ! eval( bMatchKey )

					skip
					exit

				end

				i--
			end
		end
	end

RETURN( i )


STATIC FUNCTION FindPlyr

	local cOldColor , nOldRec

	private cPlayer

	cOldColor = setcolor( c_help )
	nOldRec   = PLAYER->( recno() )

	popbox( 18, 20, 23, 60 )

	print( 20, 25, "Player ID :", CYAN_ON_BLUE )

	cPlayer := PLAYER->PLAYERID

	vGet( 20, 40, 'cPlayer', '@K!', .f. )

	vread( 1, .f. )

	if .not. empty( cPlayer )

		if .not. PLAYER->( dbseek( trim( cPlayer ), 1 ) )

			ErrorMsg( cPlayer + " not found key " + PLAYER->( indexkey() ) )
			go nOldRec
		endif

	endif

	unpop()

	setcolor( cOldColor )

RETURN nil


STATIC FUNCTION EditPlyr( cPlayerId )

	local nQty     := 0
	local lFound   := .f.
	local nOldArea := select()
	local cTeamId  := '??'
	local cGameNo  := '?'
	local nOldOrder

	if STAT->( Seeker( cPlayerId + cStat + cSeason + cWeek, 2 ) )
		nQty := STAT->QTY
		lFound := .t.
	else
*		TestMsg( 'No stat for ' + cPlayerId + cSeason + cWeek + cStat +;
*				 ' key ' + STAT->( indexkey() ) ) 
	endif

	setcolor( c_help )
	popbox( 18, 05, 23, 75 )

	print( 20, 10, "Player : " + PlayerName( cPlayerId ), CYAN_ON_BLUE )

	@ 20, 40 get nQty picture '9999.9'
	read

	if lastkey() <> K_ESC
		if lFound		  
			if STAT->( rec_lock( 5 ) )
				replace STAT->QTY with nQty
			endif

		else

			cTeamid := CurrTeam( cPlayerId, GameDate( cWeek, cSeason ) )
			cGameNo := GameCode( cTeamId, cWeek, cSeason )

			if STAT->( add_rec( 5 ) )
				replace STAT->GAMENO  	with cGameNo
				replace STAT->TEAMID  	with cTeamid

				replace STAT->SEASON		with cSeason
				replace STAT->WEEK    	with cWeek
				replace STAT->PLAYERID	with cPlayerId
				replace STAT->STAT    	with cStat
				replace STAT->QTY     	with nQty
			endif
		endif
	endif

	UnPop()

RETURN( select( nOldArea ) )



*EOF:  LOADSTAT.PRE

