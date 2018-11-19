#include ..\lib\tfl.h
#include NFLParser.h

DEFINE CLASS NFLGame as Custom

	dGame  = null
	NFLGame  = null

	HomeTeam = null         &&  set to the teamcode
	AwayTeam = null

	HomePoints = 0
	AwayPoints = 0

   ActualHomeScore = 0
   ActualAwayScore = 0

   Scores = null
   Stats  = null

   HomePlayers = null
   AwayPlayers = null

   UnregisteredPlayers = null

   HomeKOReturner = ""
   MaxHomeRets = 0
   AwayKOReturner = ""
   MaxAwayRets = 0

   HomePuntReturner = ""
   MaxHomePuntRets = 0
   AwayPuntReturner = ""
   MaxAwayPuntRets = 0

   Season = K_SEASON
   Week   = "01"
   GameNo = "A"

   NFL = null

   oDataController = null

   Errors = null

	PROCEDURE Init
      this.Scores = createObject("Collection")
      this.Stats = createObject("Collection")
      this.NFL = createObject("NFLTeams")
      this.oDataController = createobject("NFLDataController", "e:\tfl\nfl\")
      this.Errors = createObject( "Collection" )
      this.AwayPlayers = CREATEOBJECT("Collection")
      this.HomePlayers = CREATEOBJECT("Collection")
      this.UnregisteredPlayers = createObject("Collection")
      ? "NFLGame Initialised"
	RETURN
	
	Procedure SetDate
		PARAMETERS dGameIn
		this.dGame = dGameIn
	RETURN

	Procedure SetHomeScore
		PARAMETERS nScore
		this.ActualHomeScore = nScore
		? "Home Score : " + STR(nScore,3)
	RETURN

	Procedure SetAwayScore
		PARAMETERS nScore
		this.ActualAwayScore = nScore
		? "Away Score : " + STR(nScore,3)		
	RETURN
   

   function SetGameNo
      parameters dGame
      local cGameNo
      This.GameNo = this.oDataController.GetGameNo( dGame, this.HomeTeam.cTeamCode)
      lOk = iif(this.GameNo="?", .f., .t.)
      if .not. lOk
	     ? "Couldnt find game on " + dtoc(dGame) + " for " + this.HomeTeam.cTeamCode
         this.AddErrorDetail( "22", "Couldnt find game on " + dtoc(dGame) + " for " + this.HomeTeam.cTeamCode )
      endif
   return lOk

   function SetWeek
      parameters cWeek
      This.Week = cWeek
   return .t.
   
	
	FUNCTION SetHome
		PARAMETERS cHome
      local cValue, lOk
      ? "cHome=" + cHome
      cValue = this.NFL.TeamForFull(cHome)
      if .not. isNull( cValue )
   		this.HomeTeam = cValue
         * "Home Team set to " + this.HomeTeam.cTeamCode
         lOk = .t.
      else
         lOk = .f.
      endif
	RETURN lOk


	FUNCTION SetAway
		PARAMETERS cAway
      *? "cAway=" + cAway
      local cValue, lOk
      cValue = this.NFL.TeamForFull(cAway)
      if .not. isNull( cValue )
		   this.AwayTeam = cValue
         *? "Away Team set to " + this.AwayTeam.cTeamCode
         lOk = .t.
      else
         lOk = .f.
      endif
	RETURN lOk


   PROCEDURE AddErrorDetail
      parameters cErrNo, cErrMsg
      local oError
      oError = createObject( "NFLError", cErrNo, cErrMsg )
      oError = this.AddError( oError )
   return


   FUNCTION AddError
      parameters oError
      oError.SetGame( this.AwayTeam.cTeamCode + " @ " + ;
                      this.HomeTeam.cTeamCode )
      this.Errors.Add( oError )
   return oError


   FUNCTION ScoreCheck

      local lOk
      #ifdef K_SHOWSCORES
      ? "AWAY:"
      ?? str(this.ActualAwayScore,4)
      ?? "-"
      ?? str(this.AwayPoints,4)
      ? "HOME:"
      ?? str(this.ActualHomeScore,4)
      ?? "-"
      ?? str(this.HomePoints,4)
      #endif
      if this.ActualAwayScore = this.AwayPoints .and.;
         this.ActualHomeScore = this.HomePoints
         lOk = .t.
      else
         lOk = .f.
      endif
   RETURN lOk


   FUNCTION ErrorReport( lShowTotal )
   
      if this.Week <> "00"
	  
		  if lShowTotal
			 ? " Game Errors:" + str( this.Errors.Count )
		  endif

		  if this.Errors.Count > 0
			 *---Error Report
			 for each oError in this.Errors
				oError.Report
			 next
		  endif
	  
	  endif
	  
   return this.Errors.Count


   PROCEDURE Report
   
      if this.Week <> "00"
	  
			SET DATE british
			? DTOC( this.dGame ) + space(3)
			?? this.AwayTeam.cFullName + + str( this.AwayPoints, 5)
		  ?? " @ "
		  ?? this.HomeTeam.cFullName + str( this.HomePoints, 5)
		  ?? space(3) + this.GameNo
		  
		  #ifdef K_SHOWSCORES
		  for each oScore in this.Scores
			 ?  oScore.TeamCode + "  "
			 ?? oScore.When + "  " + oScore.Score + " "
			 ?? oScore.Distance + "  "
			 ?? oScore.PlayerID1
			 ?? " (" + str(this.PointsFor(oScore.Score),5) + ")"
		  next
		  *? "HomePlayers " + str(this.HomePlayers.count)
		  #endif
		  
		  this.ErrorReport(.t.)
			SET DATE american
		
	 endif	
   RETURN


	FUNCTION StoreScores

		local nScore
      nScore = 0

      #ifdef K_VERBOSE
      ? "Storing Scores..."
      #endif

      ? "NFLGame.StoreScores:Clearing Game " + this.GameNo
      this.oDataController.ClearGame( this.Season, this.Week, this.GameNo )

      *---Store Scores
      ? "There are " + ltrim(str(this.Scores.Count)) + " scores"
      
      for each oScore in this.Scores
      
         if this.oDataController.SaveScore( oScore ) then
            nScore = nScore + 1
         ELSE
            ? "failed to store score " + oScore.Score
         endif
      next
      ? "  Saved " + ltrim(str(nScore)) + " scores"
	RETURN nScore


	FUNCTION StoreStats

      #ifdef K_VERBOSE
      ? "Storing Stats..."
      #endif

		local nStat
      nStat = 0

      *---Store Stats
      for each oStat in this.Stats
         if this.oDataController.SaveStat( oStat ) then
            nStat = nStat + 1
         endif
      next
      ? "  Saved " + ltrim(str(nStat)) + " stats"
      *---Update Result
      ? "Updating Result " + this.GameNo + str( this.AwayPoints, 4)
      ?? " @ " + str( this.HomePoints, 4)
      this.oDataController.SaveResult( this.Season, this.Week, this.GameNo,;
                                       this.AwayPoints, this.HomePoints )
      
	RETURN nStat


	PROCEDURE FixRoster

      parameters lShow, lShowBackups
      local oPlayer

      #ifdef K_VERBOSE
      ? "Fixing Rosters..."
      if lShow
         this.DumpPlayerList( "Home", this.HomePlayers, ;
                                      this.HomeTeam.cFullName, lShowBackups )
      endif
      ? "NFLGame.FixRoster:Clearing Returners for " + this.HomeTeam.cFullName
	  #endif
	  
      this.HomeTeam.ClearReturners()

	  #ifdef K_VERBOSE	  
	  
      if lShow
         this.DumpPlayerList( "Away", this.AwayPlayers, ;
                                      this.AwayTeam.cFullName, lShowBackups )
      endif

      ? "NFLGame.FixRoster: Clearing Returners for " + this.AwayTeam.cFullName
	  #endif

      this.AwayTeam.ClearReturners()

	  if this.Week = "00"
	     ? "NFLGame.FixRoster: Skipping kick returners in pre-season"
	  else
		  *---Do something with the Kick Returners
		  this.StoreReturner( this.HomeTeam.cFullName, ;
							  this.HomePuntReturner  , ;
							  this.HomeTeam.cTeamCode, ;
							  "PR", "17" )

		  this.StoreReturner( this.AwayTeam.cFullName, ;
							  this.AwayPuntReturner  , ;
							  this.AwayTeam.cTeamCode, ;
							  "PR", "18" )

		  this.StoreReturner( this.HomeTeam.cFullName, ;
							  this.HomeKOReturner  , ;
							  this.HomeTeam.cTeamCode, ;
							  "KR", "19" )

		  this.StoreReturner( this.AwayTeam.cFullName, ;
							  this.AwayKOReturner  , ;
							  this.AwayTeam.cTeamCode, ;
							  "KR", "20" )
	   endif
   RETURN



   PROCEDURE SaveLineups
      #ifdef K_VERBOSE
      *? "Saving Lineups..."
      #endif

      this.ClearLineups()
      this.SaveLineup( this.HomePlayers, this.HomeTeam.cTeamCode )
      this.SaveLineup( this.AwayPlayers, this.AwayTeam.cTeamCode )

   RETURN

   procedure SaveLineup
      parameters PlayerList, cTeam
      local oDataController
      oDataController = createobject("NFLDataController", "e:\tfl\nfl\")
      FOR each oPlayer in PlayerList
         *---Save
         oDataController.SaveLineup( oPlayer, this, cTeam  )
      NEXT
   return


   procedure ClearLineups
      local oDataController
      oDataController = createobject("NFLDataController", "e:\tfl\nfl\")
      oDataController.ClearLineups( this  )
   return


   PROCEDURE StoreReturner

      parameters cTeam, cReturner, cTeamCode, cType, cErrNum
      #ifdef K_VERBOSE
      ? "Storing Returner..." + cType
      #endif
      if empty( cReturner )
         ? "Unknown " + cType + " for " + cTeam
         *--- not an error
      else
         ? "Setting " + cType + " for " + cTeam + "=" + cReturner

            if this.oDataController.IncrPos( cReturner, cTeamCode, cType )
               ??   "  Set"
            ELSE
               *TODO:  Put this back in as an error
               *this.AddErrorDetail( cErrNum, "Couldnt find " + cType + ;
                            + "  " + cReturner + " for " + cTeam )
            endif

      endif
   return
   
   procedure AddPuntReturner

      parameters cPlayer, nRets, cTeamCode

      *? "NFLGame.AddPuntReturner: " + cPlayer + " had " + str(nRets,3) + " for " + cTeamCode
      if cTeamCode = this.HomeTeam.cTeamCode
         if nRets >= this.MaxHomePuntRets
            this.HomePuntReturner = cPlayer
            this.MaxHomePuntRets = nRets
         endif
      else
         if nRets >= this.MaxAwayPuntRets
            this.AwayPuntReturner = cPlayer
            this.MaxAwayPuntRets = nRets
         endif
      endif
   return


   procedure AddKOReturner

      parameters cPlayer, nRets, cTeamCode

      *? "AddKOReturner: " + cPlayer + " had " + str(nRets,3) + " for " + cTeamCode
      if cTeamCode = this.HomeTeam.cTeamCode
         if nRets > this.MaxHomeRets
            this.HomeKOReturner = cPlayer
            this.MaxHomeRets = nRets
         endif
      else
         if nRets > this.MaxAwayRets
            this.AwayKOReturner = cPlayer
            this.MaxAwayRets = nRets
         endif
      endif
   return


   procedure DumpPlayerList
      parameters cWhich, PlayerList, cTeam, lShowBackups
      ? cWhich + " Players: (" + str(PlayerList.Count) + ")"
      ? cWhich + " Players: "
      ?? "  " + cTeam
      FOR each oPlayer in PlayerList
	    
	    if lShowBackups
           oPlayer.PrintOut
		else
		   if oPlayer.cRole = "S"
		        oPlayer.PrintOut
		   Endif
		Endif
      NEXT
   return

   procedure DumpUnregistered
      ? "------------------"
      ? " Unregistered Players"
      ? "------------------"
      FOR each cPlayer in this.UnregisteredPlayers
        ? cPlayer
      NEXT
      ? "------------------"
   return

	FUNCTION AddPlayer

		PARAMETERS lHome, cPos, cNum, cName, cRole

      local oPlayer, cTeamCode, lOk

		cName = FixNames( cName )  

      if .not. empty(Alltrim(cName))

         cTeamCode = iif( lHome, this.HomeTeam.cTeamCode, ;
                                 this.AwayTeam.cTeamCode )

         #Ifdef K_VERBOSE
         ? "Game:AddPlayer " + cName + " : " + cTeamCode + " " + cPos
         ?? "  " + cRole + "  " + cNum + iif(lHome," home"," away")
		   #Endif

         oPlayer = this.GetPlayer( cTeamCode, cName )
         if oPlayer.cPlayerID = "????????" .or. isNull(oPlayer) then
            *---Could be a new or moved player
            lOk = .f.

          #Ifdef K_VERBOSE
          ?? "   not got"
		  #Endif

         else
            *---Current player
            oPlayer.SetJersey( this.FixNumber(cNum) )
            oPlayer.SetRole( cRole )
            oPlayer.SetPos( cPos )
            if lHome then
               this.HomePlayers.Add(oPlayer)
            else
               this.AwayPlayers.Add(oPlayer)
            endif
            lOk = .t.
         endif

      endif


	RETURN lOk

	FUNCTION FixNumber( cNum )
	  if LEN(cNum) > 2
	      cNum = LEFT(cNum,2)
	  endif
	  fixedNum = val(cNum)
	RETURN fixedNum

   PROCEDURE AddUnregistered
      parameters cPlayer
      this.UnregisteredPlayers.Add( FixNames( cPlayer ) )
   RETURN


   PROCEDURE AddFG
      parameters cTeam, cWhen, cScorer, cDist
      local oTeam, oPlayer1, cTeamCode, cKickerID
      oTeam = this.GetTeam( cTeam )
      cTeamCode = oTeam.cTeamCode
      *? "TeamCode=" + cTeamCode
      oPlayer1 = this.GetPlayer( cTeamCode, cScorer )
	  *? "cScorer=" + cScorer
      cKickerID = oPlayer1.cPlayerID
      *? "KickerID=" + cKickerID
      this.AddScore( cTeamCode, SCORE_FIELD_GOAL, cWhen, cDist, cKickerID, "" )
      oPlayer1.Destroy()
      oTeam.Destroy()
   RETURN

   PROCEDURE AddFGAtt
      parameters cKicker, lGood, cDist, cTeamCode, lNoSave
      *? "NFLGame.AddFGAtt: " + cKicker + iif(lGood, " is good", " misses" ) + " from " + cDist
      oPlayer = this.GetPlayer( cTeamCode, cKicker )
      oFGAtt = createObject( "FGAttempt", ;
                              this.Season, this.Week, this.GameNo, ;
                              cTeamCode, lGood, cDist, oPlayer.cPlayerID )

      if .not. lNoSave
         oFGAtt.Save()
      endif

   RETURN


   PROCEDURE AddPAT
      parameters cTeam, cWhen, cScorer
      local oTeam, oPlayer1, cTeamCode, cKickerID
      oTeam = this.GetTeam( cTeam )
      cTeamCode = oTeam.cTeamCode
      *? "NFLGame.AddPAT:TeamCode=" + cTeamCode + " scorer " + cScorer
      oPlayer1 = this.GetPlayer( cTeamCode, cScorer )
      cKickerID = oPlayer1.cPlayerID
      *? "NFLGame.AddPAT:KickerID=" + cKickerID
      if cKickerID = "????????"
         ? "NFLGame.AddPAT: Could not find kicker " + cScorer + " for " + cTeam + " at " + cWhen
      else
         this.AddScore( cTeamCode, SCORE_PAT , cWhen, "", cKickerID, "" )
      endif

      oPlayer1.Destroy()
      oTeam.Destroy()
   RETURN


   PROCEDURE Add2PtConversion
      parameters cTeam, cWhen, cScorer1, cScorer2
      local oTeam, oPlayer1, oPlayer2, cTeamCode, cKickerID
      oTeam = this.GetTeam( cTeam )
      cTeamCode = oTeam.cTeamCode
      *? "TeamCode=" + cTeamCode
      oPlayer1 = this.GetPlayer( cTeamCode, cScorer1 )
      if cScorer2 <> ""
         *---Pass for a PAT
         oPlayer2 = this.GetPlayer( cTeamCode, cScorer2 )
         cReceiverID = oPlayer1.cPlayerID
         cPasserID = oPlayer2.cPlayerID
         this.AddScore( cTeamCode, SCORE_PAT_PASS , cWhen, "", ;
                        cReceiverID, cPasserID )
         oPlayer2.Destroy()
      else
         *---Run for two points
         cRunnerID = oPlayer1.cPlayerID
         this.AddScore( cTeamCode, SCORE_PAT_RUN , cWhen, "", cRunnerID, "" )
      endif

      oPlayer1.Destroy()
      oTeam.Destroy()
   RETURN


   PROCEDURE AddPass
      parameters cTeam, cWhen, cScorer, cDist, cPasser
      local oTeam, oPlayer1, oPlayer2, cTeamCode, cReceiverID, cPasserID
      oTeam = this.GetTeam( cTeam )
      cTeamCode = oTeam.cTeamCode
      *? "TeamCode=" + cTeamCode
      oPlayer1 = this.GetPlayer( cTeamCode, cScorer )
      oPlayer2 = this.GetPlayer( cTeamCode, cPasser )
      cReceiverID = oPlayer1.cPlayerID
      cPasserID = oPlayer2.cPlayerID
      *? "passerID=" + cPasserID
      *? "ReceiverID=" + cReceiverID
      this.AddScore( cTeamCode, SCORE_TD_PASS , cWhen, cDist, cReceiverID, cPasserID )
      oPlayer1.Destroy()
      oPlayer2.Destroy()
      oTeam.Destroy()
   RETURN

   
   PROCEDURE AddRun
      parameters cTeam, cWhen, cScorer, cDist
      local oTeam, oPlayer1, cTeamCode, cRunnerID
      oTeam = this.GetTeam( cTeam )
      cTeamCode = oTeam.cTeamCode
      *? "TeamCode=" + cTeamCode
      oPlayer1 = this.GetPlayer( cTeamCode, cScorer )
      cRunnerID = oPlayer1.cPlayerID
      *? "RunnerID=" + cRunnerID
      this.AddScore( cTeamCode, SCORE_TD_RUN , cWhen, cDist, cRunnerID, "" )
      oPlayer1.Destroy()
      oTeam.Destroy()
   RETURN

   
   PROCEDURE AddFumbleTD
      parameters cTeam, cWhen, cScorer, cDist
      local oTeam, oPlayer1, cTeamCode, cScorerID
      oTeam = this.GetTeam( cTeam )
      cTeamCode = oTeam.cTeamCode
      *? "NFLGame.AddFumbleTD:TeamCode=" + cTeamCode
      oPlayer1 = this.GetPlayer( cTeamCode, cScorer )
      cScorerID = oPlayer1.cPlayerID
      *? "NFLGame.AddFumbleTD:cScorerID=" + cScorerID
      this.AddScore( cTeamCode, SCORE_FUMBLE_RETURN , cWhen, cDist, cScorerID, "" )
      oPlayer1.Destroy()
      oTeam.Destroy()
   RETURN


   PROCEDURE AddKickReturn
      parameters cTeam, cWhen, cScorer, cDist
      local oTeam, oPlayer1, cTeamCode, cScorerID
      oTeam = this.GetTeam( cTeam )
      cTeamCode = oTeam.cTeamCode
      *? "TeamCode=" + cTeamCode
      oPlayer1 = this.GetPlayer( cTeamCode, cScorer )
      cScorerID = oPlayer1.cPlayerID
      *? "NFLGame.AddKickReturn:cScorerID=" + cScorerID
      this.AddScore( cTeamCode, SCORE_KICK_RETURN, cWhen, cDist, cScorerID, "" )
      oPlayer1.Destroy()
      oTeam.Destroy()
   RETURN

   PROCEDURE AddPuntReturn
      parameters cTeam, cWhen, cScorer, cDist
      local oTeam, oPlayer1, cTeamCode, cScorerID
      oTeam = this.GetTeam( cTeam )
      cTeamCode = oTeam.cTeamCode
      *? "TeamCode=" + cTeamCode
      oPlayer1 = this.GetPlayer( cTeamCode, cScorer )
      cScorerID = oPlayer1.cPlayerID
      ? "NFLGame.AddPuntReturn:cScorerID=" + cScorerID
      this.AddScore( cTeamCode, SCORE_PUNT_RETURN, cWhen, cDist, cScorerID, "" )
      oPlayer1.Destroy()
      oTeam.Destroy()
   RETURN
	

   PROCEDURE AddSafety
      parameters cTeam, cWhen
      oTeam = this.GetTeam( cTeam )
      cTeamCode = oTeam.cTeamCode
      *? "TeamCode=" + cTeamCode
      this.AddScore( cTeamCode, SCORE_SAFETY, cWhen, "0", "", "" )
      oTeam.Destroy()
   RETURN

   
   PROCEDURE AddInterceptTD
      parameters cTeam, cWhen, cScorer, cDist
      local oTeam, oPlayer1, cTeamCode, cInterceptorID
      oTeam = this.GetTeam( cTeam )
      cTeamCode = oTeam.cTeamCode
      *? "TeamCode=" + cTeamCode
      oPlayer1 = this.GetPlayer( cTeamCode, cScorer )
      cInterceptorID = oPlayer1.cPlayerID
      *? "cInterceptorID=" + cInterceptorID
      this.AddScore( cTeamCode, "I" , cWhen, cDist, cInterceptorID, "" )
      oPlayer1.Destroy()
      oTeam.Destroy()
   RETURN

   
   FUNCTION GetTeam
      parameters  cTeam
      local oTeam
      oTeam = this.NFL.TeamFor( cTeam )
   return oTeam


   FUNCTION GetPlayer

      parameters cTeamCode, cPlayer

      local oPlayer

      #Ifdef K_VERBOSE
		*if	cPlayer = K_TEST_PLAYER
			? "cTeamCode=" + cTeamCode
			? "NFLGame.GetPlayer cPlayer=" + cPlayer
		*endif
	   #Endif

	   cPlayer = FixNames( cPlayer )

       oPlayer = createObject( "NFLPlayer", cTeamCode, cPlayer, this.dGame )
		
   RETURN oPlayer


   FUNCTION AddScore
      parameters cTeamCode, cScoreType, cWhen, cDist, cPlayerID1, cPlayerID2
      local oScore

      if cPlayerID1 =  "????????"
         this.AddErrorDetail( "61", "Id not found for score type " + cScoreType + " at " + cTeamCode )
      endif
      if cPlayerID2 = "????????"
         this.AddErrorDetail( "62", "Id not found for score type " + cScoreType + " at " + cTeamCode )
      endif

      oScore = CreateObject("NFLScore", ;
               this.Season, this.Week, this.GameNo, ;
               cTeamCode, cScoreType, cWhen, cDist, cPlayerID1, cPlayerID2)
      this.Scores.Add(oScore)
      *? "AddScore:Score for " + oScore.PlayerID1 + " added"
      oScore.destroy()
      this.AddPoints( cScoreType, cTeamCode )
   endfunc


   function AddYDr
      parameters cTeamCode, cRunner, cAtt, cYDr
      local oPlayer, lOk
      oPlayer = this.GetPlayer( cTeamCode, cRunner )
      if isNull( oPlayer )
         lOk = .f.
      else
         if oPlayer.cPlayerID = "????????"
            lOk = .f.
         else
            this.AddStat( cTeamCode, RUSHING_CARRIES , val(cAtt), oPlayer.cPlayerID )
            this.AddStat( cTeamCode, RUSHING_YARDS , val(cYDr), oPlayer.cPlayerID )
            lOk = .t.
         endif
      endif
   return lOk


   function AddPassStats
      parameters cTeamCode, cPlayer, cAtt, cCom, cYDp, cInt
      local oPlayer, lOk
      oPlayer = this.GetPlayer( cTeamCode, cPlayer )
      if isNull( oPlayer )
         lOk = .f.
      else
         if oPlayer.cPlayerID = "????????"
            lOk = .f.
         else
            this.AddStat( cTeamCode, PASSING_ATTEMPTS, val(cAtt), oPlayer.cPlayerID )
            this.AddStat( cTeamCode, PASS_COMPLETIONS, val(cCom), oPlayer.cPlayerID )
            this.AddStat( cTeamCode, PASSING_YARDAGE, val(cYDp), oPlayer.cPlayerID )
            this.AddStat( cTeamCode, PASSES_INTERCEPTED, val(cInt), oPlayer.cPlayerID )
            lOk = .t.
         endif
      endif
   return lOk
   

   function AddRecStats
      parameters cTeamCode, cPlayer, cRec, cYDc
      local oPlayer, lOk
      oPlayer = this.GetPlayer( cTeamCode, cPlayer )
      if isNull( oPlayer )
         lOk = .f.
      else
         if oPlayer.cPlayerID = "????????"
            lOk = .f.
         else
            this.AddStat( cTeamCode, PASSES_CAUGHT, val(cRec), oPlayer.cPlayerID )
            this.AddStat( cTeamCode, RECEPTION_YARDAGE, val(cYDc), oPlayer.cPlayerID )
            lOk = .t.
         endif
      endif
   return lOk


   function AddInts
      parameters cTeamCode, cPlayer, cInt
      local oPlayer, lOk
      oPlayer = this.GetPlayer( cTeamCode, cPlayer )
      if isNull( oPlayer )
         lOk = .f.
      else
         if oPlayer.cPlayerID = "????????"
            lOk = .f.
         else
            this.AddStat( cTeamCode, INTERCEPTIONS_MADE, val(cInt), oPlayer.cPlayerID )
            lOk = .t.
         endif
      endif
   return lOk


   function AddSacks
      parameters cTeamCode, cPlayer, cSak
      local oPlayer, lOk
      oPlayer = this.GetPlayer( cTeamCode, cPlayer )
      if isNull( oPlayer ) then
         ? "Did not find " + cPlayer + " of " + cTeamCode
         lOk = .f.
      ELSE
         IF oPlayer.cPlayerID = "????????" then
         	lOk = .f.
         else
         	this.AddStat( cTeamCode, QUARTERBACK_SACKS, val(cSak), oPlayer.cPlayerID )
         	lOk = .t.
         endif
      endif
   return lOk


   FUNCTION AddStat
      parameters cTeamCode, cStatType, nStat, cPlayerID
      local oStat
      if nStat <> 0
         oStat = CreateObject("NFLStat", ;
                  this.Season, this.Week, this.GameNo, ;
                  cTeamCode, cStatType, nStat, cPlayerID )
         this.Stats.Add(oStat)
         *? ltrim(str(nStat)) + " Stats for " + oStat.PlayerID + " added"
         oStat.destroy()
      endif
   endfunc


   FUNCTION PointsFor
      parameters cScore
      local nPoints
      do case
         case cScore $ TOUCHDOWN_SCORES
            nPoints = 6
         case cScore = SCORE_FIELD_GOAL
            nPoints = 3
         case cScore = SCORE_PAT
            nPoints = 1
         case cScore = SCORE_PAT_RUN
            nPoints = 2
         case cScore = SCORE_PAT_PASS
            nPoints = 2
         case cScore = SCORE_SAFETY
            *? "NFLGame.PointsFor: Safety"
            nPoints = 2
         otherwise
            ? "NFLGame.PointsFor: Unknown Score Type " + cScore
            nPoints = 0
      endcase
   RETURN nPoints


   PROCEDURE AddPoints
      parameters cScore, cTeamCode
      local nPoints
      nPoints = this.PointsFor( cScore )
      if cTeamCode = this.AwayTeam.cTeamCode
         this.AwayPoints = this.AwayPoints + nPoints
      else
         *? "Adding points to Home team " + str( nPoints, 2 )
         this.HomePoints = this.HomePoints + nPoints
      endif
   return
		
enddefine


define class NFLScore as custom

   Season     = ""
   Week       = ""
   GameNo     = ""
   PlayerID1  = ""
   When       = ""
   Score      = ""
   Distance   = ""
   PlayerID2  = ""
   TeamCode   = ""

   procedure Init
      parameters cSeason, cWeek, cGameNo, ;
                 cTeamCode, cScoreType, cWhen, cDist, cPlayerID1, cPlayerID2

      this.Season     = cSeason
      this.Week       = cWeek
      this.GameNo     = cGameNo
      this.PlayerID1  = cPlayerID1
      this.When       = cWhen
      this.Score      = cScoreType
      this.Distance   = cDist
      this.PlayerID2  = cPlayerID2
      this.TeamCode   = cTeamCode
   return


enddefine


define class NFLStat as custom

   Season     = ""
   Week       = ""
   GameNo     = ""
   PlayerID   = ""
   StatType   = ""
   Qty        = 0
   TeamCode   = ""

   procedure Init
      parameters cSeason, cWeek, cGameNo, ;
                 cTeamCode, cStatType, nQty, cPlayerID

      this.Season     = cSeason
      this.Week       = cWeek
      this.GameNo     = cGameNo
      this.PlayerID   = cPlayerID
      this.StatType   = cStatType
      this.Qty        = nQty
      this.TeamCode   = cTeamCode
   return


enddefine
