#include NFLParser.h

define class FGAttempt as custom

   Season     = ""
   Week       = ""
   GameNo     = ""
   PlayerID   = ""
   Distance   = 0
   Good       = .f.
   TeamCode   = ""
   oDataController = null

   procedure Init
      parameters cSeason, cWeek, cGameNo, ;
                 cTeamCode, lGood, cDist, cPlayerID

      this.Season     = cSeason
      this.Week       = cWeek
      this.GameNo     = cGameNo
      this.PlayerID   = cPlayerID
      this.Distance   = val(cDist)
      this.Good       = lGood
      this.TeamCode   = cTeamCode

      this.oDataController = createobject("NFLDataController", "f:\tfl\nfl\")
      
   return

   function Save
      local lSaved
      lSaved = this.oDataController.SaveFGAtt( this )
   return lSaved

enddefine



