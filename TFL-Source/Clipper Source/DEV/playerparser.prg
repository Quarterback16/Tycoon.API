***
***   Player processor
***
***   0) set default to e:\tfl\dev
***   1) Point the newbie directory at the right week
***
***
***
#include NFLParser.h

DEFINE CLASS PlayerParser as Custom

   cDataPath   = "e:\tfl\nfl\"
   FNewbieDir   = "e:\tfl\nfl\Newbies\Week "
   FWeek       = ""

   FUpdate     = .f.    &&  Use this flag to update inspite of errors

	FUpdatesMade = .f.

   nHandle  = 0

   FnOthers   = 0
   FnNewbies  = 0
   FnAdds   = 0
   FnTransfers = 0
	FnLine    = 0
	DIMENSION FaFiles(1,5)

   NFL = null
   Gumby = null
   lStarted = .f.
   lCheckTeam = .f.

	PROCEDURE INIT

      parameters cWeek

      this.FNewbieDir=this.FNewbieDir+ cWeek + "\"
      this.FWeek = cWeek
	   set default to ( this.FNewbieDir )
		this.FnNewbies = ADIR( this.FaFiles, '*.htm' )
      ? "There are " + ltrim( str( this.FnNewbies ) )
      ?? " newbies for week " + this.Fweek
      *this.NFL = createObject( "NFLTeams" )
      *? "There are " + str( this.NFL.nCount, 4 ) + " NFL Teams"
      ? "Scanning newbie directory " + this.FNewbieDir
      ? "Updating Database in " + this.cDataPath
      ? "Force update is " + iif( this.FUpdate, "ON", "OFF" )

	RETURN


	PROCEDURE Process
      local nTotErrors
      nTotErrors = 0

      *---turn off the Code page dialog
      SET CPDIALOG OFF

		FOR g = 1 TO this.FnNewbies

         ? replicate("=", 50)
         ? "Gumbie " + ltrim(str(g))

  			this.ProcessGumby( this.FaFiles( g, 1 ) )

		next

      this.RemoveIndexes()

      ?
      ? "Transfers    : " + str( this.FnTransfers, 3 )
      ? "Newbies added: " + str( this.FnAdds     , 3 )
      ? "Others       : " + str( this.FnOthers   , 3 )
      ? "Errors       : " + str( nTotErrors      , 3 )
	RETURN



	PROCEDURE RemoveIndexes
      if this.FnAdds > 0
         ? "Removing old player/serve indexes"
         ERASE ( this.cDataPath + "playerx1.ntx" )
         ERASE ( this.cDataPath + "playerx2.ntx" )
         ERASE ( this.cDataPath + "playerx3.ntx" )
         ERASE ( this.cDataPath + "servex1.ntx" )
         ERASE ( this.cDataPath + "servex2.ntx" )
      endif
      if this.FnTransfers > 0
         ? "Removing old serve indexes"
         ERASE ( this.cDataPath + "servex1.ntx" )
         ERASE ( this.cDataPath + "servex2.ntx" )
      endif
   RETURN


	PROCEDURE ProcessGumby
		PARAMETERS cFileName
		LOCAL lThru, cLine
		lThru = .f.		
		? cFileName
		cFileName =  this.FNewbieDir + cFileName
		IF .not. FILE(cFilename)
			??  ' File ' + cFileName + ' does not exist'
		endif
		nHandle = fopen( cFileName, 0 )

	   if nHandle <> -1
	    	?? "  FileHandle:" + ltrim(str(nHandle))
	      this.lStarted = .f.
         this.lCheckTeam = .f.
	     	this.Gumby = CREATEOBJECT("NFLGumby")

			DO WHILE .not. lThru
				cLine = freadline( nHandle )
				IF ( at( "TOTAL", cLine ) > 0 ) or ( at( "Enterprises", cLine ) > 0 )
					lThru = .t.
				ELSE
					lThru = this.ProcessLine( cLine )
				ENDIF
			ENDDO
			FCLOSE(nHandle)
	    	*? "FileHandle:" + ltrim(str(nHandle)) + " closed"
         this.Gumby.Report()
         if this.Gumby.AddPlayer()
            if this.Gumby.GumbyType = "Newbie"
               this.FnAdds = this.FnAdds + 1
            else
               if this.Gumby.GumbyType = "Newbie"
                  this.FnTransfers = this.FnTransfers + 1
               else
                  this.FnOthers = this.FnOthers + 1
               endif
            endif
         endif

		ELSE
		 	? "Unable to open " + cFileName + " handle=" + STR(nHandle)
		endif
	RETURN
		

	FUNCTION ProcessLine
		PARAMETERS cLine
		LOCAL cFirst, cTeamCode, lFinish, cSecond
      lFinish = .f.

      if this.lStarted
         if this.lCheckTeam
            *if substr( alltrim(cLine), 1, 8 ) = "<td>" + K_SEASON
               this.lStarted = this.ProcTeam( cLine )
            *else
            *   ? "Check Team " + substr( alltrim(cLine), 1, 8 )
            *endif
         else
            *? "Check team off"
         endif
      endif

      cLine = alltrim( this.htmlStrip( cLine ) )
      if this.lStarted .and. .NOT. empty(cLine)
         if this.lCheckTeam
            if substr( cLine, 1, 1 ) = "2"
               *? cLine
            endif
         else
      	   *? cLine
            this.ProcessData( cLine )
         endif
      else
         if substr( cLine, 1, 2 ) = "# "
            this.lStarted = .t.
      	   *? cLine
            this.Gumby.SetPlayerName( cLine )
         endif
      endif

      if this.lStarted
		   *---What is the First Word?
		   cFirst = this.FirstWord( cLine )
         *cSecond = this.word( 2, cLine )
         if .not. empty(cFirst)
            if cFirst = "NFL"
               this.lCheckTeam = .t.  &&  You have hit the 'NFL Experience' label
               ? "Checking for Team"
            else
               ? "cFirst=" + cFirst
            endif
         endif
      endif

	RETURN lFinish


   FUNCTION ProcessData
		PARAMETERS cLine
      private cWord1
      cLine = alltrim( this.HtmlStrip( cLine ) )
      cLine = " " + this.SingleSpace( cLine ) + " "
      *? "DataLine=>" + cLine
      cWord1 = this.WORD( 1, cLine )
      *? cWord1
      do case
      case cWord1 = "Position:"
         this.Gumby.SetPosition( cLine )
      case cWord1 = "College:"
         this.Gumby.SetCollege( cLine )
      case cWord1 = "Height:"
         this.Gumby.SetHeight( cLine )
      case cWord1 = "Weight:"
         this.Gumby.SetWeight( cLine )
      case cWord1 = "Born:"
         this.Gumby.SetDOB( cLine )
      case cWord1 = "NFL"
         this.Gumby.SetRookieYr( cLine )
      endcase

   return .t.


   FUNCTION ProcTeam
		PARAMETERS cLine
      private lStart
      lStart = .f.
		cLine = freadline( nHandle )
      
      *? "TeamLine=" + cLine
      * Pull out the alt tag
      if at( "alt=", cline ) > 0
         cLine = substr( cLine, at( "alt=", cline )+5, len( cLine ) - at( "alt=", cline )-6 )
         ? "alt line " + cLine
         cTeam = ""
         cChar = "X"
         i = 1
         do while cChar <> '"'
            cChar = substr( cLine, i, 1)
            if cChar <> '"'
               cTeam = cTeam + cChar
            endif
            i = i + 1
         enddo

         *? "Team = " + cTeam
         this.Gumby.SetTeam( cTeam )
         lStart = .t.
      else
         *? "no alt tag"
      endif

   RETURN lStart

   
   FUNCTION ProcessTeam
		PARAMETERS cLine
      private cWord1
      cLine = alltrim( this.HtmlStrip( cLine ) )
      ? "TeamLine=" + cLine
      cLine = this.SingleSpace( cLine )
      ? "single spaced =" + cLine
      * Pull out the first and second words
      cWord1 = this.WORD( 1, cLine )
      cTeam = cWord1 + " " + this.WORD( 2, cLine )
      if cWord1 = "San" .or. cWord1 = "New" .or. ;
         cWord1 = "Kansas" .or. cWord1 = "Green" .or. ;
         cWord1 = "Tampa"
         cTeam = cTeam + " " + this.WORD( 3, cLine )
      endif

      ? "Team = " + cTeam
      this.Gumby.SetTeam( cTeam )

   RETURN .f.


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


	FUNCTION HTMLStrip
		PARAMETERS cLine
		LOCAL cOut, cChar, c, lOn
		lOn = .t.
		cOut = ""
		FOR c = 1 TO LEN(cLine)
			cChar = SUBSTR( cLine, c, 1 )
			IF cChar = "<" then
				lOn = .f.
			ENDIF
			IF lOn then
				cOut = cOut + cChar
			endif
			IF cChar = ">" then
			      cOut = cOut + " "
				lOn = .t.
			ENDIF
		next
	RETURN cOut
	

	FUNCTION FirstWord
		PARAMETERS cLine
		LOCAL cWord
		cWord = this.Word( 1, " " + cLine )
	RETURN cWord


	function Word
      parameters nWord, cLine
      local cWord
		cWord = STRextract( cLine, " ", " ", nWord )
   return cWord


ENDDEFINE


