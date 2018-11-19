DEFINE CLASS Result as Custom
   HomeTeam = ""
   AwayTeam = ""
   HomeScore = 0
   AwayScore = 0
   HighScore = 0
   LowScore  = 0
   MatchDate = ctod(" ")
   Winner    = ""
   Loser     = ""
   winMargin = 0
   Spread    = 0
   HomeDiv   = ""
   AwayDiv   = ""
   oData     = null

   procedure Init
      parameters cHome, cAway, nHome, nAway, dGame, nSpread, cSeason
      this.HomeTeam   = cHome
      this.AwayTeam   = cAway
      this.HomeScore  = nHome
      this.AwayScore  = nAway
      this.MatchDate  = dGame
      this.Spread     = nSpread
      if nHome > nAway
         this.Winner = cHome
         this.Loser  = cAway
         this.HighScore = nHome
         this.LowScore = nAway
         this.winMargin = nHome - nAway
      else
         this.Winner = cAway
         this.Loser  = cHome
         this.HighScore = nAway
         this.LowScore = nHome
         this.winMargin = nAway - nHome
      endif
      this.oData    = createobject( "NFLDataController", "h:\tfl\nfl\")      
      this.HomeDiv  = this.oData.DivisionFor( cHome, cSeason )
      this.AwayDiv   = this.oData.DivisionFor( cAway, cSeason )
   return


   function Won
      parameters cTeam
      local lWon
      if cTeam = this.Winner
         lWon = .t.
      else
         lWon = .f.
      endif
   return lWon


   function Lost
      parameters cTeam
      local lLost
      if cTeam = this.Loser
         lLost = .t.
      else
         lLost = .f.
      endif
   return lLost


   function Rout
      local lRout
      if this.winMargin >= 21
         lRout = .t.
      else
         lRout = .f.
      endif
   return lRout


   function Score
      local cScore
      cScore = this.Winner + " " + str(this.HighScore,2) + " d " + ;
               this.Loser + " " + str(this.LowScore,2)
   return cScore

   function WasHomeFavourite
      parameters cTeam
      local lFav
      if cTeam = this.HomeTeam
         if this.Spread > 0
            lFav = .t.
         else
            lFav = .f.
         endif
      else
         lFav = .f.
      endif

   return lFav


   FUNCTION NonDivisional
      if this.HomeDiv = this.AwayDiv
         lNonDiv = .f.
      else
         lNonDiv = .t.
      endif
   return lNonDiv

enddefine

