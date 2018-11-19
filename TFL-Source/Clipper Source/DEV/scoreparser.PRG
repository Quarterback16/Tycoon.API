***
***   Score processor
***
***   0) set default to e:\tfl\dev
***   1) Point the score directory at the right week
***   2) do NFLHarness
***
***
***   1. Process Date Line
***   2. Process Lineups
***   3. Process Subs
***   4. Set the score
***   5. Process the line scores
***   6. Process Rushing Stats
***   7. Process Passing Stats
***   8. Process Receiving Stats
***   9. Process Interceptions Made
***
#include NFLParser.h

Define Class ScoreParser As Custom

   cDataPath   = "e:\tfl\nfl\"
   FScoreDir   = "e:\tfl\nfl\Gamebooks\Week "
   Fweek       = ""
   lNoScore    = .f.    &&  .t. means dont update the database at all
   FUpdate     = .t.    &&  Set to .t. to update inspite of errors
   FFixRosters = .T.    &&  Use this flag to turn on roster update
   FDumpRoster = .t.    &&  Use this flag to show team rosters
   FDumpBackups = .t.
   FDumpUnreg  = .t.    &&  Use this flag to show unregistered players

   FUpdatesMade = .F.

   FnScores  = 0
   FnLine    = 0
   Dimension FaFiles(1,5)

   NFL = Null

   *---Line type flags
   lLineups     = .F.
   lHomeTeam    = .F.
   lLineScores  = .F.
   lRunner      = .F.
   lPasser      = .F.
   lReceiver    = .F.
   lInterceptor = .F.
   lFumbler     = .F.
   lSacker      = .F.
   lSubs        = .F.
   lKickOffRet  = .F.
   lPuntRet     = .F.
   lFGAnalysis  = .F.
   lFGHome      = .F.
   DoneReceiving = .f.

   lStarted     = .F.
   lFirstLineup = .T.
   nRCount = 0
   nPCount = 0
   nWCount = 0
   nICount = 0
   nFCount = 0
   nSCount = 0
   nKOCount = 0
   nPuntCount = 0

   Games = Null
   Game  = Null

   Errors = Null     &&  Error collection

   TotScoresSaved = 0
   TotStatsSaved  = 0

   nLastNum  = 0    &&  A static to control the substitute processing
   lHomeSub  = .F.

   Procedure Init

      Parameters cWeek

      *? "Initialising Score Parser"

      This.FScoreDir=This.FScoreDir+ cWeek + "\"
      This.Fweek = cWeek
      Set Default To ( This.FScoreDir )
      This.FnScores = Adir( This.FaFiles, '*.txt' )
      ? "There are " + Ltrim( Str( This.FnScores ) )
      ?? " scores for week " + This.Fweek

      This.NFL = Createobject( "NFLTeams" )
      ? "There are " + str( this.NFL.nCount, 4 ) + " NFL Teams"

      ? "Scanning score directory " + this.FScoreDir
      ? "Updating Database in " + This.cDataPath
      ? "Force update is " + Iif( This.FUpdate, "ON", "OFF" )
      ? "Show Rosters is " + Iif( This.FDumpRoster, "ON", "OFF" )
      *--not used
      *? "Show Unregistered is " + iif( this.FDumpUnreg, "ON", "OFF" )
      #Ifdef K_SHOWSCORES
         ? "Show scores is ON"
      #Else
         ? "Show scores is OFF"
      #Endif

      This.Games = Createobject( "Collection" )
      This.Errors = Createobject( "Collection" )

      Return

      ***
      ***   The main thread of control
      ***
   Procedure Process

      Local nErr

      *---turn off the Code page dialog
      Set Cpdialog Off

      *? this.FnScores
      For g = 1 To This.FnScores

         ? Replicate("=", 50)
         ? "Game " + ltrim(str(g))

         This.ProcessGame( This.FaFiles( g, 1 ), ;
            this.FFixRosters, ;
            this.FDumpRoster, ;
            this.ScoreEm() )

      Next

	  If this.FWeek <> "00"
	      cSummaryFileName = "Summary week " +  This.Fweek + "-" + dtos(date()) + "-" + SUBSTR(TIME(),1,2) + SUBSTR(TIME(),4,2)+  ".log"
		  SET ALTERNATE TO ( cSummaryFileName )
		  SET ALTERNATE on
		  If This.ScoreEm()
			 This.FUpdatesMade = .T.
			 ? "  Total " + lstr( This.TotScoresSaved ) + " scores"
			 ? "  Total " + lstr( This.TotStatsSaved  ) + " stats"
		  Endif

		  *---Dump the Error Report
		  nErr = This.ErrorReport()

		  If nErr = 0 .Or. ( This.FUpdate .And. This.FUpdatesMade )
			 *---Force reindexing
			 If .Not. This.lNoScore
				This.RemoveIndexes()
			 Endif
		  Else
			 #Ifdef K_VERBOSE
				? "Errors found so NOT removing indexes"
			 #Endif
		  Endif
		  SET ALTERNATE off
		  SET ALTERNATE TO
	  
	  Endif
	  
      Return


   Procedure ProcessGame

      Parameters cFileName, lFixRosters, lDumpRoster, lScoreEm

      Local nHandle, lThru, cLine
      lThru = .F.
	  
      SET ALTERNATE TO ( cFileName + ".log" )
      SET ALTERNATE on

      *? cFileName
      cFileName =  This.FScoreDir + cFileName
      If .Not. File(cFileName)
         ??  ' File ' + cFileName + ' does not exist'
      Endif
      nHandle = Fopen( cFileName, 0 )

      This.ResetFlags()

      If nHandle <> -1
         *?? "  FileHandle:" + ltrim(str(nHandle))

         This.Game = Createobject("NFLGame")
         This.Game.SetWeek( This.Fweek )

         *--- The main process loop for a game
         Do While .Not. lThru
            cLine = freadline( nHandle )
            If cLine = "XXX" .Or. At( "First Half Summary", cLine ) > 0
               *? "STOP LINE ENCOUNTERED"
               lThru = .T.
            Else
               lThru = This.ProcessLine( cLine, nHandle )
            Endif
         Enddo

         Fclose(nHandle)
         *? "FileHandle:" + ltrim(str(nHandle)) + " closed"
         This.ScoreCheck()
         If lFixRosters
            This.Game.FixRoster(lDumpRoster, this.FDumpBackups )
            If .Not. This.lNoScore
			   If this.FWeek <> "00"
                  This.Game.SaveLineups()
			    EndIf
            Endif
			if this.FDumpUnreg
               This.Game.DumpUnregistered()
			endif
         Endif

         If lScoreEm .and. this.FWeek <> "00"
		    *---Only score if there are NO errors

               nScors = This.Game.StoreScores()
               This.TotScoresSaved = This.TotScoresSaved + nScors
               nStats = This.Game.StoreStats()
               This.TotStatsSaved = This.TotStatsSaved + nStats

		 else
		    ? "Not scoring them..."
         Endif

         This.Game.Report()

         This.Games.Add( This.Game )
      Else
         ? "Unable to open " + cFileName + " handle=" + Str(nHandle)
      Endif
      SET ALTERNATE off
      SET ALTERNATE TO
      Return


   Function ProcessLine

      Parameters cLine, nHandle

      Local cFirst, cTeamCode, lFinish, cSecond
      lFinish = .F.

      *--Get rid of the HTML tags
      *cLine = this.htmlStrip( cLine )
      cLine = " " + Alltrim( cLine ) + " "
      *if this.nKOCount > 0
      *   ? ">>" + cLine
      *endif

      *---What is the First Word?  First word determines type of processing
      cFirst = This.FirstWord( cLine )
      cSecond = This.Word( 2, cLine )
      If .Not. Empty(cFirst)
         *? "CFIRST=" + cFirst
      Endif

      If This.lStarted Then

         *---Set the line type
         Do Case
            Case cFirst = "Lineups"
               *---A) Next bunch of lines contain roster info
			   #Ifdef K_VERBOSE
               ?
               ? cFirst
			   #endif
               This.lLineups = .T.
               
            Case cFirst = "Substitutions"
			   #Ifdef K_VERBOSE			
               ?
               ? cFirst
			   #endif
               This.lLineups = .F.    &&  Turns off lineup processing
               This.lSubs    = .T.    &&  Turns on Substitute processing

            Case cFirst = "Field"
			   #Ifdef K_VERBOSE			
               ?
               ? "Field goals"
			   #endif
               This.lFGAnalysis = .T.
               This.lSubs = .F.    &&  follows the Substitute info

            Case cFirst = "VISITOR:"
               *---We have an away score, single line process

               This.lFGAnalysis = .F.
               This.SetActualScore( cLine, cFirst )
            Case cFirst = "HOME:"
               *---We have an home score, single line process
               This.SetActualScore( cLine, cFirst )

            Case cFirst = "Scoring"
               *---We have the linescores
               This.lLineScores = .T.

            Case cFirst = "Paid"
               *---linescores have finished
               This.lLineScores = .F.
               This.lHomeTeam = .F.

            Case cFirst = "RUSHING"
               *---We have some runners
			   #Ifdef K_VERBOSE
               ? 
               ? cFirst
			   #endif
               This.lRunner = .T.

            Case cFirst = "PASSING"
               *---We have some QBs, stop runners start passers
			   #Ifdef K_VERBOSE			   
               ? 
               ? cFirst
			   #endif
               This.lRunner = .F.
               This.lPasser = .T.

            Case cFirst = "PASS"
               *---We have some recievers
               IF .not. this.DoneReceiving
			      #Ifdef K_VERBOSE			   
                  ? 
                  ? cFirst + " " + cSecond           
				  #endif
                  This.lPasser = .F.
                  This.lReceiver = .T.
               endif

            Case cFirst = "INTERCEPTIONS"
               *---We have some thefts
			   #Ifdef K_VERBOSE			   
               ? 
               ? cFirst        
			   #endif
               This.lReceiver = .F.
               This.lInterceptor = .T.
               this.DoneReceiving = .t.

            Case cFirst = "PUNTING"
               *---Finished intercepts
			   #Ifdef K_VERBOSE			   
               ? 
               ? cFirst        
			   #endif
               This.lReceiver = .F.
               This.lInterceptor = .F.

            Case cFirst = "PUNT" .And. cSecond = "RETURNS"
			   #Ifdef K_VERBOSE			   
               ? 
               ? cFirst + " " + cSecond
			   #endif
               This.lPuntRet = .T.

            Case cFirst = "KICKOFF"
			   #Ifdef K_VERBOSE			
               ? 
               ? cFirst + " " + cSecond
               #endif
               This.lKickOffRet = .T.
               This.lPuntRet = .F.

            Case cFirst = "FUMBLES"
			   #Ifdef K_VERBOSE			
               ? 
               ? cFirst        
				#endif
               This.lKickOffRet = .F.
               This.nFCount = This.nFCount + 1
               If	This.nFCount < 3
                  This.lFumbler = .T.
                  *					this.ProcessFumbles( cLine, cTeamCode )
               Else
                  This.lFumbler = .F.
               Endif

            Case cFirst = "FINAL"  &&  Defensive Statistics
               *---Defenders
               This.lFumbler = .F.
			   #Ifdef K_VERBOSE			   
               ? "DEFENDERS.............."
			   #endif
               This.nSCount = This.nSCount + 1
			   #Ifdef K_VERBOSE			   
               ? cFirst + Str(This.nSCount)
			   #endif
               This.lSacker = .T.
               If This.nSCount = 2 Then
                  This.lHomeTeam = .T.
                  *? "Home Tacklers"
               Else
                  This.lHomeTeam = .F.
                  *? "Away Tacklers"
               Endif

            Case cFirst = "Total"
               *--Next team, two teams Away then Home
               If .Not. This.lHomeTeam
                  This.lHomeTeam = .T.
               Else
                  This.lSacker = .F.
               Endif

            Case cFirst = "First"
               *? "^^^^^^^^^^  Finished  ^^^^^^^^^^"
               This.lSacker = .F.

         Endcase

         *---Process the line type
         If This.lLineups
            This.ProcessLineup( cLine )
         Else
            If This.lFGAnalysis
               This.FGAnalysis( cLine )
            Else
               If This.lLineScores
                  This.ProcessLineScore( cLine )
               Else
                  If This.lSubs
                     This.ProcessSubs( cLine )
                  Else

                     If This.lHomeTeam
                        cTeamCode = This.Game.HomeTeam.cTeamCode
                     Else
                        cTeamCode = This.Game.AwayTeam.cTeamCode
                     Endif
                     If This.lFumbler
                        If This.nFCount = 1 Then
                           This.lHomeTeam = .F.
                           *??	 "Away Fumblers"
                           cTeamCode = This.Game.AwayTeam.cTeamCode
                        Else
                           This.lHomeTeam = .T.
                           *?? " Home Fumblers"
                           cTeamCode = This.Game.HomeTeam.cTeamCode
                        Endif

                        *? "Team code set to " + cTeamCode
                     Endif

                     If This.lRunner
                        This.ProcessRunner( cLine, cTeamCode )
                     Else
                        If This.lPasser
                           This.ProcessPasser( cLine, cTeamCode )
                        Else
                           If This.lReceiver
                              This.ProcessReceiver( cLine, cTeamCode )
                           Else
                              *---Defense
                              If This.lInterceptor
                                 This.ProcessIntercepts( cLine, cTeamCode )
                              Else
                                 If	This.lFumbler
                                    This.ProcessFumbles( cLine, cTeamCode )
                                 Else
                                    If This.lSacker
                                       This.ProcessSacker( cLine, cTeamCode )
                                    Else
                                       If This.lKickOffRet
                                          This.ProcessKO( cLine, cTeamCode )
                                       Else
                                          If This.lPuntRet
                                             This.ProcessPuntRet( cLine, cTeamCode )
                                          Endif

                                       Endif

                                    Endif
                                 Endif
                              Endif
                           Endif
                        Endif
                     Endif
                  Endif

               Endif
            Endif
         Endif

      Else
         *---We havent started yet  THIS is THE key work that kicks us off
         If cFirst = "Date:" Then
            This.lStarted = .T.
            If This.ProcessDate( cLine, nHandle )
               This.OutputLine( cLine )
            Else
               *---Cant do this game
               lFinish = .T.
            Endif
         Endif
      Endif
      Return lFinish


   Function ProcessDate

      Parameters cDateLine, nHandle

      #Ifdef K_VERBOSE
         ? "Processing Date ..."
		 ? "cDateLine=" + cDateLine
      #Endif

      Local nSlash, nEndDate, cDateStr, cHome, cAway, nHome, dGame, lOk
      lOk = .F.
      nSlash = At( "/", cDateLine, 2 )
      If nSlash > 0
         nEndDate = nSlash + 4  &&  end of date
         ? "nEndDate =" + STR(nEndDate)
         nComma = At( ",", cDateLine, 1 )  && start of date
         cDateStr = trim( Substr( cDateLine, nComma+1, nEndDate - nComma ))
         ? "GameDay=" + cDateStr
         *-- Check for date truncation ala 11/11/200
         nDateLen = len( cDateStr )  
         *-- 9
         cLastThree = right( cDateStr, 3 )
         *? "LastThree=" + cLastThree
         
         if cLastThree = "200"
            cDateStr = trim( cDateStr ) + "9"
         else
            cLastTwo = right( cDateStr, 2 )
            if cLastTwo = "00"
               *? cLastTwo
               cDateStr = trim( cDateStr ) + "09"
            endif
         endif
         
         ? "cDateStr=" + cDateStr
         
         dGame = Ctod( cDateStr )
         This.Game.SetDate( dGame )
         
		 *--this old line forces us to look at the NEXT line however game details are on the same line as the Date
		 *cLine = freadline( nHandle )
		 
		 cLine = substr( cDateLine, 30, 256 )
		 
		 ? "  cLine >>" + cLine
         nHome = At( "AT ", UPPER(cLine) )
         ? "nHome =" + STR(nHome)
         cHome = Alltrim(Substr( cLine, nHome+3, 30 ) )
         ? "home=" + cHome
         If This.Game.SetHome( cHome ) Then
            lOk = .T.
         Else
            ? "Home team not discerned"
            This.AddError( "HOM", "Home team not discerned" )
         Endif
		 
		 *--Figure out Away team
         *cAway = Alltrim( Substr( cLine, nEndDate+6, nHome - nEndDate - 6 ) )
		 cLine = Substr( cLine, 1, nHome-2 )
		 cAway = Alltrim(cLine)
         ? "away=" + cAway
         
         if empty( cAway )
            lOk = .f.
            This.AddError( "AWY", "Away team not discerned" )
         Endif

         If lOk .And. This.Game.SetAway( cAway ) Then
            IF this.Fweek <> "00" then
               This.Game.SetGameNo( dGame )
            Endif
         Else
            This.AddError( "AWY", "Away team not discerned" )
            lOk = .F.
         Endif
         *? "Date processed - START"
      Else
         ? "ProcessDate no slash>" + cDateLine
         lOk = .F.
      Endif

      Return lOk


	  ***
	  ***  Lineups are side by side now - 4 players
	  ***  WR 87 M.Muhammad   LE  93 A.Ogunleye   TE  86  B.Manumaleuna  DE  99 I.Olshansky

   Procedure ProcessLineup

      Parameters cLine

      #Ifdef K_VERBOSE
         ? "Processing Lineup ..."
      #Endif

      If .Not. Empty(cLine)
         cLine = Alltrim(cLine) + " "
		 cLine = Strtran(cLine, "Jr.", "")
         #Ifdef K_VERBOSE
            ?
            ? "ProcessLineup: cLine=" + cLine
         #Endif
         cWord = This.FirstWord( " " + cLine )

         If This.IsPosition( cWord )
            This.doFour( cLine )
         Else
		    #Ifdef K_VERBOSE
            ? "not a line up line"
			#Endif
         Endif
      Endif
	  
   Return

   Function doFour
      Parameters cLine
      cOut = ""
      cPos = ""
      cNum = ""
      cName = ""
      p = 1
      For cc = 1 To Len(cLine)

         cChar = Substr( cLine, cc, 1 )
			#Ifdef K_VERBOSE
         *? STR(cc) + "=" + cChar
			#Endif
         If cChar = " " Then
            If Len( cOut ) > 0
               If Empty( cPos )
                  cPos = cOut
						#Ifdef K_VERBOSE
                  *? "cPos set to " + cPos
						#Endif
               Else
                  If Empty( cNum )
                     cNum = cOut
                     *? "cNUm set to " + cNum

                  Else
                     cName = cOut

							#Ifdef K_VERBOSE
                     *? "cName set to " + cName
							#Endif

                     If p > 2
                        lHome = .T.
                     Else
                        lHome = .F.
                     Endif

                     If This.AddPlayer( lHome, cPos + " " + cNum + " " + cName, cPos ) Then
                        *? "Added " + STR(p) + " " + cPos + " " + cName
                     Else
                        ? "doFour:Unknown Player>" + cName + " at " + cPos
                     Endif
                     p = p+1
                     cPos = ""
                     cNum = ""
                     cName = ""
                  Endif
               Endif

            Endif
            cOut = ""
         Else
            cOut = cOut + cChar
         Endif
         *? cOut
      Next cc
	  
      Return .T.

   Procedure AddPlayer

      Parameters lHome, cPlayer, cPlyrPos

      Local cPos, cNum, cName, cRole, lOk, nOffSet
      cPlayer = " " + SingleSpace(cPlayer) + " "
      #Ifdef K_VERBOSE
      ? "AddPlayer: " + IIF( lHome, "Home", "Away" ) + " cPlayer=" + cPlayer
		#Endif
      cPos = This.FirstWord( cPlayer )
      cNum = This.Word( 2, cPlayer )
      cName = This.Word( 3, cPlayer )
      If Right(Alltrim( cName ), 1 ) = "."
         cName = Alltrim( cName ) + This.Word(4, cPlayer )
      Endif
      #Ifdef K_VERBOSE
		? "Pos =" + cPos
      ? "num =" + cNum
      ? "Name=" + cName
		#Endif

		cName = this.Purify( cName )
		cName = FixNames( cName )

      If This.IsPosition( cPos )
         cRole = "S"
         #Ifdef K_VERBOSE
         ?? "  Starter"
			#Endif
      Else
         cRole = "B"
         #Ifdef K_VERBOSE
         ?? "  Backup"
			#Endif
      Endif

      If Empty(Alltrim(cName))
         lOk = .F.
         This.Game.AddUnregistered( cPlayer )
      Else
         lOk = This.Game.AddPlayer( lHome, cPlyrPos, cNum, cName, cRole )
      Endif

   Return lOk

	FUNCTION Purify
	  parameters cStr

     local nLen, cChar, cc, cNewStr
     nLen = 0
     cNewStr = ""
     For cc = 1 To Len( cStr)
         cChar = Substr( cStr, cc, 1 )
		 if cChar = "-"
		 	cNewStr = cNewStr + cChar
		 else
			if	( asc(cChar) > 64 ) or ( asc(cChar) = 46 )
				*--a letter or a full stop (numbers will be filtered out)
				cNewStr = cNewStr + cChar
	        endif
		 endif
	  next cc
      *? "Purify: " + cStr + " to " + cNewStr
   return cNewStr

   Procedure ProcessSubs

      Parameters cLine

      ***
      ***  Substitutes are processed in one big string of numbers
      ***  and names, because there is effectively 2 columns players will be missed.

      ***  We are also looking for the starting Kickers who are always
      ***  subs.
      ***
      Local nWord, cPlayer, lEop, cHomeLine, cAwayLine

      This.nLastNum = 0   &&  Reset the static sub controller
      This.lHomeSub = .F.

      cLine = Alltrim(cLine)
      cLine = Strtran(cLine, "&nbsp;", ",")
      cLine = Alltrim(cLine)
      If Len(cLine) > 0 .And. cLine <> "Substitutions"
         #Ifdef K_VERBOSE
            *? replicate("-", 40)
            *? "ProcessSubs:" + cLine
         #Endif

			*--- Process Away subs
			cAwayLine =  "," + substr(cLine, 1, 72 ) + ","
         #Ifdef K_VERBOSE
            *? "AwayLine:" + cAwayLine
         #Endif
			lEop = .F.
         nWord = 0
         Do While .Not. lEop
            nWord = nWord + 1

            *-- Pull out part, XX NN F.Surname,  delimited by commas
            cPlayer = Strextract( cAwayLine, ",", ",", nWord )

            This.AddSub( cPlayer )

            If Len( Alltrim( cPlayer ) ) = 0
               lEop = .T.
            Endif
         Enddo

			*--- Process Home Subs
			cHomeLine = "," + substr(cLine, 73, 999 )  + ","
         #Ifdef K_VERBOSE
            *? "HomeLine:" + cHomeLine
         #Endif

         lEop = .F.
         nWord = 0
         Do While .Not. lEop
            nWord = nWord + 1

            *-- Pull out part, XX NN F.Surname,  delimited by commas
            cPlayer = Strextract( cHomeLine, ",", ",", nWord )

            This.AddSub( cPlayer )

            If Len(Alltrim(cPlayer)) = 0
               lEop = .T.
            Endif

         Enddo

         #Ifdef K_VERBOSE
            *? replicate("-", 40)
         #Endif

      Endif
      Return


   Procedure AddSub

      Parameters cPlayer

      Local cPos, cNum, cName, cRole, nNum
      cPlayer = Alltrim(cPlayer)

      #Ifdef K_VERBOSE
         *? space(10) + "AddSub: " + cPlayer
      #Endif

      cPos = Strextract( " " + cPlayer, " ", " ", 1 )
      cNum = Strextract( " " + cPlayer, " ", " ", 2 )
      cName = Substr(cPlayer, At(" ",cPlayer,2)+1, 255 )

      If cPos = "K" .And. Len(cPos)=1
         cRole = "S"
         #Ifdef K_VERBOSE
            ?? space(3) + cPos + "=Kicker!"
         #Endif
      Else
         cRole = "B"
      Endif

      cName = Strtran( cName, " ", "" )
      If (.Not. Empty( cName )) .And. ( "." $ cName )

         *? space(14) + "name=" + cName
         *?? "  num=" + cNum + "  pos=" + cPos + "  role=" + cRole

         If This.Game.AddPlayer( .T., cPos, cNum, cName, cRole )
            *?? "  Added to Home bench"
         Else
            If This.Game.AddPlayer( .F., cPos, cNum, cName, cRole )
               *?? "  Added to Away bench"
            Else
               *?? "  Cant add " + cName + " to a Roster"
               This.Game.AddUnregistered( cName )
            Endif
         Endif
      Endif

   Return

   Procedure SetActualScore

      Parameters cLine, cFirst
      ? "---------------------------------------"
      Local nScore
      nScore = Val(Right(Alltrim(cLine),3))
      If cFirst = "HOME:"
         This.Game.SetHomeScore( nScore )
      Else
         This.Game.SetAwayScore( nScore )
      Endif
      ? "---------------------------------------"
      Return



   Function ErrorReport()
      Local nErr
      nErr = 0
      ? "---------------------------------------"
      For Each oGame In This.Games
         oGame.ErrorReport( .F. )
         nErr = nErr + oGame.Errors.Count
         if oGame.Errors.Count = 0
            ? 	
			   ?? oGame.AwayTeam.cFullName 
		      ?? " @ "
		      ?? oGame.HomeTeam.cFullName
		     ?? space(3) + "OK"
         Endif
      Next
      ? "Total Errors:" + Str( nErr, 5 )
      ? "---------------------------------------"
      Return nErr


   Function FixRosters
      *--Determins whether or not to attemp fixing rosters

      Return This.FFixRosters



   Function ScoreEm
      *--Logic to determine whether or not to update the database
      *--to turn off scoring all together set lNoStore to .t.
      Local lStore

      If .Not. This.lNoScore
         If This.Errors.Count = 0
            lStore = .T.
         Else
            If This.FUpdate
               lStore =.T.
            Else
               lStore =.F.
            Endif
         Endif
      Endif

      Return lStore


   Procedure RemoveIndexes
      If this.FWeek <> "00"
      ? "Removing old indexes"
		  Erase ( This.cDataPath + "scorex1.ntx" )
		  Erase ( This.cDataPath + "scorex2.ntx" )
		  Erase ( This.cDataPath + "scorex3.ntx" )
		  Erase ( This.cDataPath + "scorex4.ntx" )
		  Erase ( This.cDataPath + "scorex5.ntx" )
		  Erase ( This.cDataPath + "statx1.ntx" )
		  Erase ( This.cDataPath + "statx2.ntx" )
		  Erase ( This.cDataPath + "statx3.ntx" )
	  Endif
      Return


   Procedure ScoreCheck
      If .Not. This.Game.ScoreCheck()
         This.AddError( "SCO", "Scores dont add up" )
      Endif
      Return


   Function IsPosition
      Parameters cWord
      Local lPos
      lPos = .F.
      Do Case
         Case cWord = "WR"
            lPos = .T.
         Case cWord = "LT"
            lPos = .T.
         Case cWord = "LG"
            lPos = .T.
         Case cWord = "C"
            lPos = .T.
         Case cWord = "RG"
            lPos = .T.
         Case cWord = "RT"
            lPos = .T.
         Case cWord = "TE"
            lPos = .T.
         Case cWord = "QB"
            lPos = .T.
         Case cWord = "RB"
            lPos = .T.
         Case cWord = "HB"
            lPos = .T.
         Case cWord = "FB"
            lPos = .T.
         Case cWord = "FL"
            lPos = .T.
         Case cWord = "DE"
            lPos = .T.
         Case cWord = "DT"
            lPos = .T.
         Case cWord = "LDT"
            lPos = .T.
         Case cWord = "RDT"
            lPos = .T.
         Case cWord = "LDE"
            lPos = .T.
         Case cWord = "RDE"
            lPos = .T.
         Case cWord = "NT"
            lPos = .T.
         Case cWord = "UT"
            lPos = .T.
         Case cWord = "LLB"
            lPos = .T.
         Case cWord = "MLB"
            lPos = .T.
         Case cWord = "RLB"
            lPos = .T.
         Case cWord = "WLB"
            lPos = .T.
         Case cWord = "SLB"
            lPos = .T.
         Case cWord = "LCB"
            lPos = .T.
         Case cWord = "RCB"
            lPos = .T.
         Case cWord = "SS"
            lPos = .T.
         Case cWord = "FS"
            lPos = .T.
         Case cWord = "LE"
            lPos = .T.
         Case cWord = "CB"
            lPos = .T.
         Case cWord = "DB"
            lPos = .T.
         Case cWord = "JACK"
            lPos = .T.
         Case cWord = "SAM"
            lPos = .T.
         Case cWord = "WILL"
            lPos = .T.
         Case cWord = "OLB"
            lPos = .T.
         Case cWord = "ILB"
            lPos = .T.
         Case cWord = "LOLB"
            lPos = .T.
         Case cWord = "ROLB"
            lPos = .T.
         Case cWord = "LILB"
            lPos = .T.
         Case cWord = "RILB"
            lPos = .T.
         Case cWord = "LC"
            lPos = .T.
         Case cWord = "RC"
            lPos = .T.
         Case cWord = "T"
            lPos = .T.
         Case cWord = "H-B"
            lPos = .T.
         Otherwise
            lPos = .F.
      Endcase
      If lPos
         If Len(cWord) > 4
            lPos = .F.
         Else
            *? "Found a Pos >" + cWord
         Endif
      Else
         *? cWord + " is not a position"
      Endif
      Return lPos

      ***
      ***  FG line R.Gould  (27) N.Kaeding   33B
      ***
   Procedure FGAnalysis

      Parameters cLine

      Local cKicker, cFGStr, cTeamCode
      #Ifdef K_VERBOSE
         ?
         *? "Processing FGs ..."
      #Endif

      If This.isPlayer(cLine)
         *? "(player) cLine=" + cLine
         *---Away kicker is first
         cAwayKicker = This.Word( 1, cLine )
         nEndOfAway = At( ".", cLine, 2) - 10
         *? "nEndOfAway = " + STR(nEndOfAway )
         cAwayFGStr = Alltrim( Substr(cLine, Len( cAwayKicker ) + 2, nEndOfAway ) )
         *? "cAwayFGStr = "  + cAwayFGStr
         This.ProcessFGAtts( cAwayKicker , cAwayFGStr , This.Game.AwayTeam.cTeamCode )
         cHomeKickerLine = Alltrim(Substr(cLine, nEndOfAway, 80 ))
         *? "cHomeKickerLine =" + cHomeKickerLine
         cHomeKicker = This.FirstWord( " " + cHomeKickerLine )
         cHomeFGStr = Alltrim(Substr( cHomeKickerLine, Len( cHomeKicker ) + 2 ) )
         *? "cHomeFGStr = "  + cHomeFGStr
         This.ProcessFGAtts( cHomeKicker, cHomeFGStr, This.Game.HomeTeam.cTeamCode )
      Endif

      Return


   Procedure ProcessFGAtts

      Parameters cKicker, cFGStr, cTeamCode

      Local i, lGood, oAtts

      *? "kicker=" + cKicker
      *? "scores=" + cFGStr
      *? "team=" + cTeamCode

      If Len(cFGStr) > 0
         oAtts = This.Attempts( cFGStr )
         For Each cAtt In oAtts
            If Substr( cAtt, 1, 1 ) = "("
               lGood = .T.
            Else
               lGood = .F.
            Endif
            cDist = This.DistFor( cAtt )
            This.Game.AddFGAtt( cKicker, lGood, cDist, cTeamCode, This.lNoScore )
         Next
      Endif


      Return


   Function Attempts

      Parameters cFGStr

      Local cChar, c, cAtt

      cFGStr = cFGStr + " "
      cAtt = ""
      oAtts = Createobject("Collection")

      For c = 1 To Len( cFGStr )
         cChar = Substr( cFGStr, c, 1 )
         If cChar = " " Then
            *--end of word
            oAtts.Add( cAtt )
            cAtt = ""
         Else
            cAtt = cAtt + cChar
         Endif
      Next

      *? Str( oAtts.Count, 1 ) + " attempts"

      Return oAtts


   Function DistFor

      Parameters cAtt
      Local c, cChar, cDist
      cDist = ""

      For c = 1 To Len( cAtt )
         cChar = Substr( cAtt, c, 1 )
         If cChar $ "0123456789" Then
            cDist = cDist + cChar
         Endif
      Next

      Return cDist

   ***
   ***  Bears       2     14:13  R.Gould 27 yd. Field Goal (8-27, 3:54)                                                                      3       0
   ***  Chargers    3       0:45  A.Gates 17 yd. pass from L.Tomlinson (N.Kaeding kick) (4-29, 2:05)                                         3       7
   ***  Chargers    4       9:09  L.Tomlinson 7 yd. run (N.Kaeding kick) (8-56, 4:36)                                                        3      14

   Procedure ProcessLineScore

      Parameters cLine

	  If this.FWeek = "00"
	     Return
	  Endif
	  
      #Ifdef K_VERBOSE
         *?
         *? "Processing Line Scores ..."
      #Endif

      Local cTeam, cQuarter, cTime, cWhen, cScorer, cDist, cScoreType, lDoPAT
      Local cPasser, nWord, cPatWord, nOffSet
      If This.isScore()
         cLine = SingleSpace( cLine )
         cLine = Strtran(cLine,". ",".")
         cLine = Strtran(cLine,"yd.", "yard ")
         cLine = Strtran(cLine,"play by", "from" )
         cLine = Strtran(cLine,"Randle El", "RandleEl" )
         *? "cLine=" + cLine
         cTeam = This.Word(1, cLine)
         *? "cTeam=" + cTeam
         cQuarter = This.Word(2, cLine )
         *? "cQuarter=" + cQuarter
         cTime = This.Word(3, cLine )
         *? "cTime=" + cTime
         cScorer = This.Word(4, cLine )
         *? "cScorer=" + cScorer
         nOffSet = 5
         If Right(cScorer,1)="." Then
            cScorer = cScorer + This.Word(5, cLine )
            nOffSet = nOffSet + 1
			*? "cScorer=" + cScorer
         Endif

         cDist = This.Word(nOffSet, cLine )
         *? "cDist=" + cDist
         nOffSet = nOffSet + 2
         cScoreType = This.Word(nOffSet, cLine)
         cWhen = This.switchWhen( cQuarter + "-" + cTime )
         *? "cWhen=" + cWhen
         If "Safety" $ cLine
            cScoreType = "Safety"
         Endif
         If "punt" $ cLine
            cScoreType = "punt"
         Endif

         *? "ScoreParser.ProcessLineScore:cScoreType=" + cScoreType

         Do Case
            Case cScoreType = "Field"
               *? "Score:>" + cLine
               This.Game.AddFG(cTeam, cWhen, cScorer, cDist )
               lDoPAT = .F.

            Case cScoreType = "run"
               *? "Score:>" + cLine
               This.Game.AddRun(cTeam, cWhen, cScorer, cDist )
               nWord = 9
               lDoPAT = .T.

            Case cScoreType = "interception"
               *? "Score:>" + cLine
               This.Game.AddInterceptTD(cTeam, cWhen, cScorer, cDist )
               nWord = 10
               lDoPAT = .T.

            Case cScoreType = "pass"
               *? "Score:>" + cLine
               cPasser = This.Word(9, cLine )
               *? "Passer=" + cPasser
               This.Game.AddPass(cTeam, cWhen, cScorer, cDist, cPasser )
               nWord = 11
               lDoPAT = .T.

            Case cScoreType = "return"
               *? "Score:>" + cLine
               *---What type of return?
               If "punt" $ cLine
                  cRetType = "punt"
               Else
                  cRetType = This.Word(9, cLine)
               Endif
               ? "ScoreParser.ProcessLineScore:Return Type = " + cRetType
               Do Case
                  Case cRetType = "blocked"
                     This.Game.AddFumbleTD(cTeam, cWhen, cScorer, cDist )
                     nWord = 12
                     If "field goal" $ cLine
                        nWord = 13
                     Endif

                  Case cRetType = "punt"
                     This.Game.AddPuntReturn(cTeam, cWhen, cScorer, cDist )
                     nWord = 11

                  Case cRetType = "field"
                     *--- Return of a FG see SF @ CH Week 10, 2005
                     *Score:> Bears 2 0:00 N.Vasher 108 yard return of field goal (R.Gould kick) 3 7
                     This.Game.AddKickReturn(cTeam, cWhen, cScorer, cDist )
                     nWord = 12

               Endcase

               lDoPAT = .T.

            Case cScoreType = "in"
               *---Fumble recovery in Endzone
               *? "cTeam  =" + cTeam
               *? "cScorer=" + cScorer
               *? "cWhen  =" + cWhen
               *? "cDist  =" + cDist
               This.Game.AddFumbleTD(cTeam, cWhen, cScorer, cDist )
               nWord = 11
               lDoPAT = .T.

            Case cScoreType = "Safety"
				   *---Description = C.Johnson tackled in end zone by J.Mays for a Safety   
               This.Game.AddSafety( cTeam, cWhen )
               lDoPAT = .F.

            Case cScoreType = "punt"
               nWord = 10
               If "blocked punt recovery" $ cLine
                  nWord = nWord + 2
               Endif
               If "return of blocked punt" $ cLine
                  nWord = nWord + 2
               Endif

               ? "ScoreParser.ProcessLineScore: " + cLine
               ? "ScoreParser.ProcessLineScore: " + cScorer

               This.Game.AddPuntReturn(cTeam, cWhen, cScorer, cDist )
               lDoPAT = .T.

            Case cScoreType = "kickoff"
               This.Game.AddKickReturn(cTeam, cWhen, cScorer, cDist )
               nWord = 10
               lDoPAT = .T.

            Case cScoreType = "fumble"
               This.Game.AddFumbleTD(cTeam, cWhen, cScorer, cDist )
               cNextWord = This.Word(nOffSet+1, cLine)
               *? "ScoreParser.ProcessLineScore:next word " + cNextWord
               If cNextWord = "return" .Or. cNextWord = "recovery"
                  nWord = 10
               Else
                  nWord = 12
               Endif
               lDoPAT = .T.

            Otherwise
               *---In this situation we need all the diagnostics we can get
               ? "cLine   =" + cLine
               ? "cWhen   =" + cWhen
               ? "nOffSet =" + Str( nOffSet )
               ? "cTeam   =" + cTeam
               ? "cQuarter=" + cQuarter
               ? "cTime   =" + cTime
               ? "cScorer =" + cScorer
               ? "cDist   =" + cDist
               ? "valid score types are fumble, kickoff, punt, safety, in, return, interception, field, pass and run"
               This.AddError( "TYP-SC", "Unknown Score Type " + cScoreType )
         Endcase

         If lDoPAT
            *? "Doing PAT"
            cPatWord = This.Word(nWord, cLine)
            *? "ScoreParser.ProcessLineScore:Pat word(" + lstr(nWord) + ")=" + cPatWord
            Do Case
               Case cPatWord = "kick)"
                  cScorer = This.Word(nWord-1, cLine)
                  cScorer = Right( cScorer, Len(cScorer)-1)
                  *? "ScoreParser.ProcessLineScore:PAT for " + cScorer
                  This.Game.AddPAT( cTeam, cWhen, cScorer )

               Case cPatWord = "run)"
                  cScorer = This.Word(nWord-1, cLine)
                  cScorer = Right( cScorer, Len(cScorer)-1)
                  *? "PAT run for " + cScorer
                  This.Game.Add2PtConversion( cTeam, cWhen, cScorer, "" )

               Case cPatWord = "pass)"
                  cScorer = This.Word(nWord-1, cLine)
                  cScorer = Right( cScorer, Len(cScorer)-1)
                  cPasser = Substr(cScorer, 1, At("-",cScorer)-1 )
                  cReceiver = Substr(cScorer, At("-",cScorer)+1,;
                     len(cScorer)-At("-",cScorer) )

                  *? "ScoreParser.ProcessLineScore:PAT-pass for " + cPasser + " to " + cReceiver
                  This.Game.Add2PtConversion( cTeam, cWhen, cReceiver,;
                     cPasser  )

               Otherwise
                  nWord = nWord + 1
                  cPatWord = This.Word(nWord, cLine)
                  If cPatWord = "pass)"
                     cScorer = This.Word(nWord-1, cLine)
                     cScorer = Right( cScorer, Len(cScorer)-1)
                     cPasser = Substr(cScorer, 1, At("-",cScorer)-1 )
                     cReceiver = Substr(cScorer, At("-",cScorer)+1,;
                        len(cScorer)-At("-",cScorer) )
                     *? "ScoreParser.ProcessLineScore:PAT-pass for " + cPasser + " to " + cReceiver
                     This.Game.Add2PtConversion( cTeam, cWhen, cReceiver,;
                        cPasser  )

                  Endif

            Endcase
         Endif
      Endif

      Return


   Function switchWhen
      Parameters  mWhen

      *---Format Q-MM:SS
      Local nMinute, nSec, cWhen

      cWhen = mWhen
      If .Not. Empty( mWhen )
         nMinute = Val( Substr( mWhen, 3, 2 ) )
         nSec    =  Val( Substr( mWhen, 6, 2 ) )
         nSec = 60 - nSec
         If nSec = 60
            nSec = 0
            nMinute = 15-nMinute
         Else
            nMinute = 14-nMinute
         Endif

         cWhen = Substr(mWhen, 1, 2 ) + Str( nMinute, 2 ) + ":" + strzero( nSec, 2 )
      Endif

      Return cWhen


   Function isScore
      Local lIsScore
      lIsScore = .F.
      If .Not. Empty( cLine )
         cFirst = This.FirstWord( cLine )
         lIsScore = This.IsTeam( cFirst )
      ENDIF
*      IF lIsScore
*         ? "Score!"
*      endif
      Return lIsScore


   Function IsTeam
      Parameters cTeam
      Local lIs, oTeam
      *? "cTeam=" + cTeam
      nLen = LEN( cTeam )
      lIs = .F.
      For Each oTeam In This.NFL.Teams
         If cTeam = SUBSTR( oTeam.cNickname, 1, nLen )
            lIs = .T.
            Exit
         Endif
      Next
      Return lIs


	***
	***   There  will be 2 runners per line
	***
   Procedure ProcessRunner

      Parameters cLine, cTeamCode

	  If this.FWeek = "00"
	     Return
	  Endif
	  
      #Ifdef K_VERBOSE
		?
      ? "Processing Runners ..."
      #Endif

      Local cRunner, cAtt, cYDr, nOffSet, nEndOfAway, cAwayStr

      nOffSet = 2
      *cLine = SingleSpace( cLine ) &&  we want fixed size now
      *? "Runner:>" + cLine

      cAwayStr = SingleSpace( Substr(cLine, 1, 74 ) )

      #Ifdef K_VERBOSE
      ? "cAwayStr = "  + cAwayStr
      #Endif

      cAwayRunner = This.Word( 1, cAwayStr )

      #Ifdef K_VERBOSE
      ? " cAwayRunner=" + cAwayRunner
      #Endif

      if	( cAwayRunner <> "Total" .and. cAwayRunner <> "RUSHING" )

			if len(alltrim(cAwayRunner)) > 0
				cAtt = This.Word( nOffSet, cAwayStr )
				cYDr = This.Word( nOffSet+1, cAwayStr )

				#Ifdef K_VERBOSE
				? cAwayRunner +" has " + cYDr + " yards on " + cAtt + " carries"
				#Endif

				If .Not. This.Game.AddYDr( This.Game.AwayTeam.cTeamCode, cAwayRunner, cAtt, cYDr ) Then
				    IF .Not. This.Game.AddYDr( This.Game.HomeTeam.cTeamCode, cAwayRunner, cAtt, cYDr ) Then
					   This.AddError( "UNK-RB-AWAY", cAwayRunner  )
					Endif
				Endif
			endif

         cHomeRunnerLine = " " + SingleSpace( Alltrim(Substr(cLine, 75, 80 )))

			#Ifdef K_VERBOSE
         ? "cHomeRunnerLine =" + cHomeRunnerLine
			#Endif

         cHomeRunner = This.FirstWord( cHomeRunnerLine )
			if len(alltrim(cHomeRunner)) > 0
				cAtt = This.Word( nOffSet, cHomeRunnerLine )
				cYDr = This.Word( nOffSet+1, cHomeRunnerLine )

				#Ifdef K_VERBOSE
				? cHomerunner + " has " + cYDr + " yards on " + cAtt + " carries"
				#Endif

				If .Not. This.Game.AddYDr( This.Game.HomeTeam.cTeamCode, cHomeRunner, cAtt, cYDr ) Then
				   IF .Not. This.Game.AddYDr( This.Game.AwayTeam.cTeamCode, cHomeRunner, cAtt, cYDr ) Then
					  This.AddError( "UNK-RB-HOME", cHomeRunner )
				   endif
				Endif
			endif

      	endif

      Return


   Function isPlayer
      Parameters cLine
      *---A player name has a fullstop in it
      Local lisPlayer, cPlayer
      lisPlayer = .F.
      If .Not.Empty(cLine)
         cPlayer = Substr( cLine,1,10 )
         If At(".",cPlayer) > 0
            lisPlayer = .T.
         Endif
      Endif
      Return lisPlayer


   Procedure ProcessPasser

      Parameters cLine, cTeamCode

	  If this.FWeek = "00"
	     Return
	  Endif
	  
      #Ifdef K_VERBOSE
		*?
        *? "Processing Passers ..."
        *? "cLine=" + cLine
      #Endif

      Local cAway, cHome, cAwayStr, cAtt, cYDp, cCom, cInts, nOffSet
      nOffSet = 1
      cAwayStr = ALLTRIM(SingleSpace( Substr(cLine, 1, 74 ) ))
      *? "cAwayStr =>"  + cAwayStr
      cAway = This.FirstWord( " " + cAwayStr )
      *? " cAway=" + cAway
      if	( cAway <> "Total" .and. cAway <> "PASSING" )

			if len(alltrim(cAway)) > 0
				cAtt = This.Word( nOffSet, cAwayStr )
				*? "cAtt = " + cAtt
				cCom = This.Word( nOffSet+1, cAwayStr )
				*? "cCom = " + cCom 				
				cYDp = This.Word( nOffSet+2, cAwayStr )
				*? "cYDp = " + cYDp 				
				cInts = This.Word( nOffSet+6, cAwayStr )
				*? "cInts = " + cInts 				
				? cAway +" has " + cYDp + " yards on " + cCom + " completions"
				If .Not. This.Game.AddPassStats( This.Game.AwayTeam.cTeamCode, cAway, ;
						cAtt, cCom, cYDp, cInts ) Then
				    If .Not. This.Game.AddPassStats( This.Game.HomeTeam.cTeamCode, cAway, ;
						cAtt, cCom, cYDp, cInts ) Then						
					   This.AddError( "UNK-QB", cAway )
					endif
				Endif
			endif

         cHomeLine = SingleSpace( Alltrim(Substr(cLine, 75, 80 )))
         *? "cHomeLine=>" + cHomeLine
         cHome = This.FirstWord( " " + cHomeLine )
			if len(alltrim(cHome)) > 0
				cAtt = This.Word( nOffSet, cHomeLine )
				*? "cAtt = " + cAtt				
				cCom = This.Word( nOffSet+1, cHomeLine )
				*? "cCom = " + cCom 			
				cYDp = This.Word( nOffSet+2, cHomeLine )
				*? "cYDp = " + cYDp 					
				cInts = This.Word( nOffSet+6, cHomeLine )
				*? "cInts = " + cInts 					
				? cHome +" has " + cYDp + " yards on " + cCom + " completions"
				If .Not. This.Game.AddPassStats( This.Game.HomeTeam.cTeamCode, cHome, ;
						cAtt, cCom, cYDp, cInts )
					IF .Not. This.Game.AddPassStats( This.Game.AwayTeam.cTeamCode, cHome, ;
						cAtt, cCom, cYDp, cInts )
					   This.AddError( "UNK-QB", cHome )
					endif
				Endif
			endif
     	endif

      Return



   Procedure ProcessReceiver

      Parameters cLine, cTeamCode

	  If this.FWeek = "00"
	     Return
	  Endif
	  
      #Ifdef K_VERBOSE
		   *?
         *? "Processing Receivers ..."
      #Endif

      Local cPlayer, cRec, cYDc, nOffSet, cAway, cHome, cHomeLine, cAwayStr
      
      nOffSet = 3  &&  added one in 2009 as Targeted is now a stat

      *? "Receiver:>" + cLine

      cAwayStr = SingleSpace( Substr(cLine, 1, 74 ) )
      *? "cAwayStr = "  + cAwayStr
      cAway = This.Word( 1, cAwayStr )
      *? " cAway=" + cAway
      IF ( cAway <> "Total" .and. cAway <> "PASS" )

			if len(alltrim(cAway )) > 0
                cRec = This.Word( nOffSet, cAwayStr )
                cYDc = This.Word( nOffSet+1, cAwayStr )				
				*? cAway +" has " + cYDc + " yards on " + cRec + " catches"
                If .Not. This.Game.AddRecStats( This.Game.AwayTeam.cTeamCode, cAway, cRec, cYDc )
                   If .Not. This.Game.AddRecStats( This.Game.HomeTeam.cTeamCode, cAway, cRec, cYDc )
                      This.AddError( "UNK-PR", cAway )
                   endif
         		endif	
			endif

         cHomeLine = " " + SingleSpace( Alltrim(Substr(cLine, 75, 80 )))
         *? "cHomeLine =" + cHomeLine
         cHome = This.FirstWord( cHomeLine )
			if len(alltrim(cHome)) > 0
                cRec = This.Word( nOffSet, cHomeLine )
                cYDc = This.Word( nOffSet+1, cHomeLine )	
				*? cHome + " has " + cYDc + " yards on " + cRec + " catches"
                If .Not. This.Game.AddRecStats( This.Game.HomeTeam.cTeamCode, cHome , cRec, cYDc )
                   If .Not. This.Game.AddRecStats( This.Game.AwayTeam.cTeamCode, cHome , cRec, cYDc )
                      This.AddError( "UNK-PR", cHome )
                   endif
         		endif	
			endif

      	ENDIF
      	
      
      Return


   Procedure ProcessIntercepts

      Parameters cLine, cTeamCode

	  If this.FWeek = "00"
	     Return
	  Endif
	  
      #Ifdef K_VERBOSE
         *? "Processing Interceptions ..."
      #Endif

      Local cInt, cPlayer, nOffSet
      
      nOffSet = 2

      *? "Interceptions:>" + cLine

      cAwayStr = SingleSpace( Substr(cLine, 1, 74 ) )
      *? "cAwayStr = "  + cAwayStr
      cAway = This.Word( 1, cAwayStr )
      *? " cAway=" + cAway
      IF ( cAway <> "Total" .and. cAway <> "INTERCEPTIONS" )

			if len(alltrim(cAway )) > 0
                cInt = This.Word( nOffSet, cAwayStr )				
				? cAway +" had " + cInt + " interception(s)"
                If .Not. This.Game.AddInts( This.Game.AwayTeam.cTeamCode, cAway, cInt )
                   If .Not. This.Game.AddInts( This.Game.HomeTeam.cTeamCode, cAway, cInt )
                      This.AddError( "UNK-DB", cAway )
                   endif
         		endif	
			endif

         cHomeLine = " " + SingleSpace( Alltrim(Substr(cLine, 75, 80 )))
         *? "cHomeLine =" + cHomeLine
         cHome = This.FirstWord( cHomeLine )
			if len(alltrim(cHome)) > 0
                cInt = This.Word( nOffSet, cHomeLine )				
				? cHome +" had " + cInt + " interception(s)"
                If .Not. This.Game.AddInts( This.Game.HomeTeam.cTeamCode, cHome , cInt )
                   If .Not. This.Game.AddInts( This.Game.AwayTeam.cTeamCode, cHome , cInt )
                      This.AddError( "UNK-DB", cHome )
                   endif
         		endif
			endif

      	ENDIF

      Return

   Procedure ProcessPuntRet

      Parameters cLine, cTeamCode

	  If this.FWeek = "00"
	     Return
	  Endif
	  
      #Ifdef K_VERBOSE
      *?
         *? "Processing Punt returns ..."
      #Endif
      
      LOCAL cHomeLine, cAwayLine
      cAwayLine = SingleSpace( Substr(cLine, 1, 74 ) )      
      cHomeLine = " " + SingleSpace( Alltrim(Substr(cLine, 75, 80 )))
      this.ProcessPuntLine( cAwayLine, This.Game.AwayTeam.cTeamCode  )
      this.ProcessPuntLine( cHomeLine, This.Game.HomeTeam.cTeamCode )
      Return

    PROCEDURE ProcessPuntLine
       PARAMETERS cPuntLine, cTeamCode

	    If this.FWeek = "00"
	      Return
	    Endif
	  
	    *? ">>>puntLine = " + cPuntLine
		 
       LOCAL cPlayer, cAmt1
       cPlayer = This.Word( 1, cPuntLine )
       
		 IF ( cPlayer <> "Returns" .and. ;
				cPlayer <> "PUNT" .and.;          
				cPlayer <> "[OUT" .and.;
				cPlayer <> "Total" .and.;				
				cPlayer <> "[TOUCHBACK]" .and.;
				cPlayer <> "[DOWNED]" ;                              
			 )

			if len( alltrim( cPlayer )) > 0
				 cAmt1 = This.Word( 2, cPuntLine )				
				 *? cPlayer +" had " + cAmt1 + " punt return(s) for " + cTeamCode
				 This.Game.AddPuntReturner( cPlayer, Val( cAmt1 ), cTeamCode )

			ENDIF
		endif
    RETURN
    
   Procedure ProcessKO

      Parameters cLine, cTeamCode

	  If this.FWeek = "00"
	     Return
	  Endif
	  
      #Ifdef K_VERBOSE
      *?
      *? "Processing Kick Off returns ..."
      #Endif     
      
      LOCAL cHomeLine, cAwayLine
      cAwayLine = SingleSpace( Substr(cLine, 1, 74 ) )      
      cHomeLine = " " + SingleSpace( Alltrim(Substr(cLine, 75, 80 )))
      this.ProcessKOLine( cAwayLine, This.Game.AwayTeam.cTeamCode  )
      this.ProcessKOLine( cHomeLine, This.Game.HomeTeam.cTeamCode )
      
      Return


    PROCEDURE ProcessKOLine
       PARAMETERS cKOLine, cTeamCode
	  If this.FWeek = "00"
	     Return
	  Endif	   
       LOCAL cPlayer, cAmt1
       cPlayer = This.Word( 1, cKOLine)
       
          IF ( cPlayer <> "Returns" .and. ;
               cPlayer <> "KICKOFF" ;                              
             )

			if this.IsPlayer( cPlayer )
                cAmt1 = This.Word( 2, cKOLine)				
				? cPlayer +" had " + cAmt1 + " KO return(s)"
                This.Game.AddKOReturner( cPlayer, Val( cAmt1 ), cTeamCode )
			ENDIF
         endif
    RETURN
    
   Procedure ProcessFumbles

      Parameters cLine, cTeamCode
	  
	  If this.FWeek = "00"
	     Return
	  Endif
	  
      If	Len(Trim(cLine)) > 0

         #Ifdef K_VERBOSE
            *? "Processing Fumbles ..." + str( this.nFCount, 2 ) + ">>>"
         #Endif

         Local cFumbles, cRecovered, nFumbles

         cFirst = This.FirstWord( cLine )

         If cFirst = "Total"
            nOffSet = 2
            cLine = SingleSpace( cLine )
            *? "Fumble:>" + cLine + " team:" + cTeamCode
            cFumbles = This.Word( 2, cLine )
            cRecovered = This.Word( 3, cLine )
            nFumbles = Val( cFumbles ) - Val(cRecovered)
            *?? "  " + Str( nFumbles, 2 ) + " FUM"
            *			 if .not. this.Game.AddFumbles( cTeamCode, cInt )
            *				 this.AddError( "UNK-FUM", cPlayer + " for " + cTeamCode )
            *			 endif
         Endif
      Endif

      Return


   Procedure ProcessSacker

      Parameters cLine, cTeamCode

	  If this.FWeek = "00"
	     Return
	  Endif
	  
      #Ifdef K_VERBOSE
         *? "Processing Pass Rushers ..."
      #Endif

      Local cInt, cPlayer, nOffSet
      *? "cline=" + cLine
	  cLine = Strtran(cLine, "Jr.", "")
      If This.isPlayer(cLine)
         nOffSet = 2
         cLine = SingleSpace( cLine )
         cPlayer = This.Word( 1, cLine )
         *? cPlayer + " is a player"
         If Right(cPlayer,1) = "."
            *---Spaced out name!
            nOffSet = nOffSet + 1
            cPlayer = This.Word( 1, cLine ) + This.Word(2, cLine )
         Endif

		 *--These guys have 2 word surnames or a Jr or a III etc which messes uo the sack counts
         If cPlayer = "K.Van"
            nOffSet = nOffSet + 1
         Endif
		 If cPlayer = "V.Hargreaves"
            nOffSet = nOffSet + 1
         Endif

         cSAK = This.Word( nOffSet+3, cLine )
         If Val(cSAK)>0
            #Ifdef K_VERBOSE
               ? "Sacker:>" + cPlayer + " team:" + cTeamCode
               ?? " " + cSAK + " sacks"
            #Endif
            If .Not. This.Game.AddSacks( cTeamCode, cPlayer, cSAK )
               This.AddError( "UNK-DL", cPlayer + " for " + cTeamCode )
            Endif
         Else
            *? "No sacks -" + cSAK
         Endif
      Else
         *? "not a player"
      Endif
      Return


   Function ResetFlags
      This.DoneReceiving = .f.
      This.lLineups     = .F.
      This.lHomeTeam    = .F.
      This.lLineScores  = .F.
      This.lRunner      = .F.
      This.lPasser      = .F.
      This.lReceiver    = .F.
      This.lInterceptor = .F.
      This.lSacker      = .F.

      This.lStarted     = .F.
      This.lFirstLineup = .T.
      This.lKickOffRet  = .F.
      This.lPuntRet     = .F.

      This.nRCount = 0
      This.nPCount = 0
      This.nWCount = 0
      This.nICount = 0
      This.nSCount = 0

      This.nPuntCount = 0
      This.nKOCount = 0

   Endfunc


   Function OutputLine
      Parameters cLine
      If .Not. Empty( cLine ) Then
         *---Pull out the data from the html tags
         This.FnLine = This.FnLine + 1
         *? STR( this.FnLine, 5 ) + ">" + cLine
      Endif
   Endfunc


   Procedure AddError
      Parameters cErrNo, cErrMsg
	  if ( substr( cErrMsg, 1, 4 ) <> "Home" )
         Local oError
         oError = Createobject( "NFLError", cErrNo, cErrMsg )
         oError = This.Game.AddError( oError )
         This.Errors.Add( oError )
	  endif
      Return


   Function HTMLStrip
      Parameters cLine
      Local cOut, cChar, c, lOn
      lOn = .T.
      cOut = ""
      For c = 1 To Len(cLine)
         cChar = Substr( cLine, c, 1 )
         If cChar = "<" Then
            lOn = .F.
         Endif
         If lOn Then
            cOut = cOut + cChar
         Endif
         If cChar = ">" Then
            cOut = cOut + " "
            lOn = .T.
         Endif
      Next
      Return cOut


   Function FirstWord
      Parameters cLine
      Local cWord
      cWord = This.Word( 1, cLine )
      Return cWord


   Function Word
      Parameters nWord, cLine
      Local cWord
      nWordCount = This.CountSpaces( cLine )
      If	nWord > nWordCount
         ? "ERROR: there are only " + Str( nWordCount ) + ;
            " words, cant get word " + Str( nWord )
         cWord = "Error"
      Else
         cWord = Strextract( cLine, " ", " ", nWord )
      Endif
      Return cWord

   Function CountSpaces
      Parameters cLine
      nCount = 0
      For c = 1 To Len( cLine )
         cChar = Substr( cLine, c, 1 )
         If cChar = " " Then
            nCount = nCount + 1
         Endif
      Next
      Return nCount
      
Enddefine

Define Class NFLError As Custom

   ErrorGame = ""
   ErrorType = ""
   ErrorMessage = ""

   Procedure Init
      Parameters cErrType, cErrMsg
      This.ErrorType = cErrType
      This.ErrorMessage = cErrMsg
      Return

   Procedure Report
      ? "   " + This.ErrorGame + "  "
      ??  This.ErrorType + ":" + This.ErrorMessage
      Return

   Procedure SetGame
      Parameters cGame
      This.ErrorGame = cGame
      Return


      
Enddefine

