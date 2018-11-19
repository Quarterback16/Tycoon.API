#include e:\tfl\lib\tfl.h
#include NFLParser.h

DEFINE CLASS NFLGumby as Custom

   NFL = null

   PlayerName = ""
   FirstName = ""
   LastName = ""
   CurrTeam = ""
   GumbyTeam = null
   TeamCode = ""
   Position = ""
   HeightFeet = 6
   HeightInch = 0
   Weight  = 200
   College = ""
   RookieYr = K_SEASON
   Category = "5"
   cDOB = "08/11/1959"
   DOB = date()

   GumbyType = ""

   oPlayer = null

   oDataController = null


	PROCEDURE Init
      this.NFL = createObject("NFLTeams")
      this.oDataController = createobject("NFLDataController", "e:\tfl\nfl\")
	RETURN


   PROCEDURE Report
		SET DATE british
      ? "NFLGumby.Report"
		? "Player      : " + this.PlayerName
      ? "Team        : " + this.CurrTeam
      
      ? "College     : " + this.College
      ? "Rookie Year : " + this.RookieYr
      ? "Pos = " + this.Position
      ? "Height = " + str( this.HeightFeet, 1 ) + "-" + str( this.HeightInch, 2 )
      ? "Weight = " + str( this.Weight, 3 )
      ? "DOB = " + dtoc( this.DOB )
		SET DATE american
   RETURN

   function SetPosition
      parameters cLine
      this.Position = Word( 2, cLine )
      *---work out category
      this.Category = CatFor( this.Position )
   return .t.

   function SetCollege
      parameters cLine
      this.College = alltrim( Word( 2, cLine ) + " " + WORD( 3, cLine ) )
   return .t.

   function SetRookieYr
      parameters cLine
      local cYears
      cYears = Word( 3, cLine )
      if cYears = "Rookie"
         cYears = "1"
      endif

      *? "Years Exp =>" + cYears
      this.RookieYr = str( val( K_SEASON ) - val( cYears ) + 1, 4 )
   return .t.


   function SetHeight
      parameters cLine
      local cHt, cFeet, cIns
      cHt = Word( 2, cLine )
      *? "Height=>" + cHt
      cFeet = substr( cHt, 1, 1 )
      this.HeightFeet = val( cFeet )
      cIns = substr( cHt, 3, 2 )
      this.HeightInch = val( cIns )
   return .t.
   
   function SetWeight
      parameters cLine
      local cWt
      cWt = Word( 2, cLine )
      *? "Weight=>" + cWt
      this.Weight = val( cWt )
   return .t.
   
   function SetDOB
      parameters cLine
      this.cDOB = Word( 2, cLine )
      *? "DOB=>" + this.cDOB
      this.DOB = ctod( this.cDOB )
   return .t.
   

   function SetPlayerName
      parameters cLine

      cLine = cLine + " "
      *? "Player line =>" + cLine
      this.FirstName = Word( 2, cLine )
      *? "FirstName =>" + this.FirstName
      this.LastName = Word( 3, cLine )
      *? "LastName =>" + this.LastName
      this.PlayerName = this.FirstName + " " + this.LastName
      *? "Player name set to  " + this.PlayerName
   return .t.


	FUNCTION AddPlayer

      local lOk
      lOk = .t.
      if .not. empty( this.PlayerName )

         ? "Gumby:AddPlayer "
         ?? this.PlayerName + " : " + this.GumbyTeam.cTeamCode + " " + this.Position

         *--Has the player transfered?

         this.oPlayer = this.GetPlayer( this.GumbyTeam.cTeamCode , this.PlayerName )
         *? "GetPlayer returns id=>" + this.oPlayer.cPlayerID
         if empty( this.oPlayer.cPlayerID ) then
            this.GumbyType = "Newbie"

            ? "   Adding : " + this.PlayerName
            this.oPlayer.cPlayerID = this.oDataController.NextID( ;
                       this.FirstName, this.LastName )
            ? "   ID       : " + this.oPlayer.cPlayerID
            this.oPlayer.cSurname  = this.LastName
            ? "   Surname  : " + this.oPlayer.cSurname
            this.oPlayer.cFirst    = this.FirstName
            ? "   Firstname: " + this.oPlayer.cFirst
            this.oPlayer.cCollege = this.College
            ? "   College  : " + this.oPlayer.cCollege
            this.oPlayer.TeamCode  = this.GumbyTeam.cTeamCode
            ? "   TeamCode : " + this.oPlayer.TeamCode
            this.oPlayer.HeightFt  = this.HeightFeet
            ? "   HeightFt : " + str( this.oPlayer.HeightFt, 1 )
            this.oPlayer.HeightIn  = this.HeightInch
            ? "   HeightIn : " + str( this.oPlayer.HeightIn, 2 )
            this.oPlayer.Weight    = this.Weight
            ? "   Weight   : " + str( this.oPlayer.Weight, 3 )
            this.oPlayer.cRookieYr = this.RookieYr
            ? "   RookieYr : " + this.oPlayer.cRookieYr
            this.oPlayer.cPos      = this.Position
            ? "   Position : " + this.oPlayer.cPos
            this.oPlayer.cCategory = this.Category
            ? "   Category : " + this.oPlayer.cCategory
            this.oPlayer.DOB       = this.DOB
            ? "   DOB      : " + dtoc( this.oPlayer.DOB )

            lOk = this.oDataController.AddPlayer( this.oPlayer )
         else
            if this.oPlayer.TeamCode = this.GumbyTeam.cTeamCode
               *--shes right mate
               this.GumbyType = "Done"
               ? "   Got him, he is at " + this.oPlayer.TeamCode
            else
               this.GumbyType = "Transfer"
               ? "   NFLGumby.AddPlayer:Need to transfer this guy >" + this.oPlayer.cPlayerID
               ? "   Out of " + this.oPlayer.cCollege
               ?? " (gumby college=" + this.College + ")"
               ? "   He was at " + this.oPlayer.TeamCode
               lOk = this.oDataController.TransferPlayer(  ;
                        this.oPlayer.cPlayerID,     ;
                        this.GumbyTeam.cTeamCode,   ;
                        this.oPlayer.TeamCode,      ;
                        date()-7 )
            endif

         endif

      endif

	RETURN lOk


   FUNCTION GetTeam
      parameters  cTeamNick
      ? "NFLGumby.GetTeam for " + cTeamNick
      local oTeam
      oTeam = this.NFL.TeamFor( cTeamNick )
   return oTeam

   FUNCTION SetTeam
      parameters cTeamName
      local cTeamNick
      this.CurrTeam = cTeamName
      cTeamNick = LastWord( cTeamName )
      if cTeamNick = "Louis"
         cTeamNick = "Rams"
      endif

      ? "Team Nick =>" + cTeamNick + " for " + cTeamName
      this.GumbyTeam = this.GetTeam( cTeamNick )
      if isNull(this.GumbyTeam)
         ? "NFLGumby.SetTeam:Failed to determine team"
      else
        ? "NFLGumby.SetTeam:=" + this.GumbyTeam.cTeamCode + " for " + cTeamName
      endif

   return .t.


   FUNCTION GetPlayer
      parameters cTeamCode, cPlayer
      local oPlayer
      *? "cTeamCode=" + cTeamCode
      *? "  Gumby:GetPlayer cPlayer=" + cPlayer
      oPlayer = createObject( "NFLPlayer", "??", this.PlayerName, date() )
   RETURN oPlayer


enddefine

