*---Function Library

function FixNames( cName )

   *--- strip out any numbers
   cName = StripNumbers( cName )
	*---  "special" players
	if	cName = "C.Ochocinco"
		cName = "C.Johnson"
   endif
   * Handle bizare 2 part names
   if cName = "K.von" .or. cName = "K.Von"
      cName = "K.VonOelHoffen"
      *? "  Game: " + cPlayer
   endif
   if cName = "K.Vanden"
      cName = "K.Vanden Bosch"
   endif
   if cName = "A.Randle"
      cName = "A.RandleEl"
   endif
   if cName = "Kv.Smith"
      cName = "Kev.Smith"
   endif
   if cName = "Jz.Rodgers"
      cName = "J.Rodgers"
   endif   
   *--- Works for Alex smith but fails for HT A.Smith
*   if cName = "A.Smith"
*      cName = "Alex.Smith"
*   endif
   

return cName


function StrZero

   parameters nNumber, nZeros

   local cOut

   cOut = replicate("0", nZeros )

   cOut = right( cOut + ltrim(str(nNumber)), nZeros )

return cOut

function StripNumbers
   parameters cName
   local cOut, i, cChar
   cOut = ""
   for i = 1 to len(cName)
      cChar = substr( cName, i, 1 )
      if cChar $ "0123456789"
	     *--- skip numbers
      else
         cOut = cOut + cChar
      endif
   next
return cOut


function SingleSpace
   parameters cLine
   local cOut, i, cChar
   cOut = ""
   cLastChar = "X"
   for i = 1 to len(cLine)
      cChar = substr( cLine, i, 1 )
      if cLastChar = " " .and. cChar = " "
      else
         cOut = cOut + cChar
      endif
      cLastChar = cChar
   next
return cOut

function Word
   parameters nWord, cLine
   local cWord
	cWord = STRextract( cLine, " ", " ", nWord )
return cWord

function LastWord
   parameters cPhrase
   local cLast, i, cChar
   cLast = ""
   cLastChar = "X"
   for i = 1 to len(cPhrase)
      cChar = substr( cPhrase, i, 1 )
      if cChar = " "
         cLast = ""
      else
         cLast = cLast + cChar
      endif
      cLastChar = cChar
   next
return cLast


FUNCTION Freadline
	PARAMETERS nHandle
	LOCAL cLine, lEOL
	cLine = ""
	lEOL = .f.
	DO WHILE .not. lEOL
		cChar = FREAD( nHandle, 1 )
		IF ASC(cChar) = 0 THEN
			cLine = "XXX"
			lEOL = .t.
		else
			IF asc(cChar)  = 10
				lEOL = .t.
			ELSE
				cLine = cLine + cChar
			ENDIF
		endif
	enddo
RETURN cLine

FUNCTION LStr
   parameters nValue
RETURN alltrim(str(nValue))
   

   function Segment
      parameters cWeek
      do case
         case cWeek < "05"
            cSegment = "1"
         case cWeek < "09"
            cSegment = "2"
         case cWeek < "18"
            cSegment = "2"
         otherwise
            cSegment = "P"
      endcase
   return cSegment

FUNCTION CatFor
   parameters cPos

   local cCat
   cCat = "5"
   cPos = alltrim( cPos )
   do case
   case cPos = "T"
      cCat = "7"
   case cPos = "G"
      cCat = "7"
   case cPos = "C"
      cCat = "7"
   case "RB" $ cPos
      cCat = "2"
   case "HB" $ cPos
      cCat = "2"
   case "FB" $ cPos
      cCat = "2"
   case "QB" $ cPos
      cCat = "1"
   case cPos = "P"
      cCat = "1"
   case "WR" $ cPos
      cCat = "3"
   case "TE" $ cPos
      cCat = "2"
   case "PK" $ cPos
      cCat = "4"
   case "CB" $ cPos
      cCat = "6"
   case "DB" $ cPos
      cCat = "6"
   case "FS" $ cPos
      cCat = "6"
   case "SS" $ cPos
      cCat = "6"
   case "LG" $ cPos
      cCat = "7"
   case "LT" $ cPos
      cCat = "7"
   case "C" $ cPos
      cCat = "7"
   case "RG" $ cPos
      cCat = "7"
   case "RT" $ cPos
      cCat = "7"
   case "ROT" $ cPos
      cCat = "7"
   case "LOT" $ cPos
      cCat = "7"
   otherwise
      cCat = "?:" + cPos
   endcase

return cCat

