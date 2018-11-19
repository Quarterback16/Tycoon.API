DEFINE CLASS Play as Custom

   TeamCode  = ""
   HomeAway  = "A"
   Spread    = 0
   OppCode   = ""
   Season    = "2003"
   Week      = "07"
   GameCode  = " "
   onTV      = .f.
   GameDate  = ctod(" ")
   GameHour  = "1"
   TeamDiv   = " "
   OppDiv    = " "

   Reasons   = null

   oData     = null


   procedure Init
      parameters cTeamCode, cOppCode, nSpread, cHomeAway, ;
                 cSeason, cWeek, cGame, lTV, dGame, cGameHour
      this.TeamCode = cTeamCode
      this.OppCode  = cOppCode
      this.Spread   = nSpread
      this.HomeAway = cHomeAway
      this.Season   = cSeason
      this.Week     = cWeek
      this.GameCode = cGame
      this.onTV     = lTV
      this.GameDate = dGame
      this.GameHour = cGameHour
      this.Reasons  = createObject( "Collection" )
      this.oData    = createobject( "NFLDataController", "h:\tfl\nfl\")
      this.TeamDiv  = this.oData.DivisionFor( cTeamCode, cSeason )
      this.OppDiv   = this.oData.DivisionFor( cOppCode, cSeason )
      *? "Play created on " + cTeamCode
   return


   procedure AddReason
      parameters cReason
      local oReason
      oReason = createObject("Reason",cReason)
      this.Reasons.Add( oReason )
   RETURN


   FUNCTION NonDivisional
      if this.TeamDiv = this.OppDiv
         lNonDiv = .f.
      else
         lNonDiv = .t.
      endif
   return lNonDiv


   FUNCTION PrintLine
      local cLine
      cLine = this.GameOut() + space(2) + this.FormatTeam( this.TeamCode ) +;
              space(2) +;
              iif( this.HomeAway = "H", "Home  ", "Away  " ) + ;
              this.FormatTeam( this.OppCode ) + space(2) + ;
              "(" + str( this.Reasons.Count, 3 ) + ")" + space(2) +;
              this.SpreadOut() + space(2) + this.BetAmount()

   RETURN cLine


   function FormatTeam
      parameters cTeam
      local cTeamOut
      cTeamOut = left( this.oData.TeamFor( cTeam, this.Season ) + space(11), 11 )
   return cTeamOut


   procedure PrintReasons
      local oReason
      for each oReason in this.Reasons
         ? space(16) + oReason.why
      next

   return

   FUNCTION IsPlay
      local lPlay
      if this.Reasons.Count > 0
         lPlay = .t.
      else
         lPlay = .f.
      endif
   return lPlay

   FUNCTION IsDog
      local lDog
      if this.HomeAway = "H"

         if this.Spread < 0
            lDog = .t.
         else
            lDog = .f.
         endif
      else
         if this.Spread > 0
            lDog = .t.
         else
            lDog = .f.
         endif
      endif
   RETURN lDog


   FUNCTION IsHomeDog
      local lDog
      if this.HomeAway = "H"
         if this.Spread < 0
            lDog = .t.
         else
            lDog = .f.
         endif
      endif
   RETURN lDog


   FUNCTION GameOut
      local cGameOut
      *---Sun 1 [tv]
      cGameOut = left( cdow(this.GameDate), 3 ) + " " + this.GameHour + " " +;
                 iif(this.OnTV, "tv  ", "    ")
   RETURN cGameOut


   FUNCTION SpreadOut
      if this.HomeAway = "H"
         if this.Spread > 0
            cWord = "MINUS "
         else
            cWord = " PLUS "
         endif
      else
         if this.Spread > 0
            cWord = " PLUS "
         else
            cWord = "MINUS "
         endif
      endif
   RETURN cWord + str( abs(this.Spread), 4, 1 )


   FUNCTION BetAmount
      *---COuld do stuff here
   RETURN "$" + str( 10, 6, 2 )


enddefine


define class Reason as custom

   why = ""

   procedure Init
      parameters cWhy
      this.why = cWhy
   return

enddefine

