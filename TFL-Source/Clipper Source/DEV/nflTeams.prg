define class NFLTeams as custom

   Teams = null
   nCount = 0

   PROCEDURE INIT
      this.Teams = createObject("Collection")
      *---AFC
      *------AFC North
      this.AddTeam("BR","Baltimore","Ravens")
      this.AddTeam("CI","Cincinnati","Bengals")
      this.AddTeam("CL","Cleveland","Browns")
      this.AddTeam("PS","Pittsburgh","Steelers")
      
      *------AFC South
      this.AddTeam("HT","Houston","Texans")
      this.AddTeam("IC","Indianapolis","Colts")
      this.AddTeam("JJ","Jacksonville","Jaguars")
      this.AddTeam("TT","Tennessee","Titans")

      *------AFC East
      this.AddTeam("MD","Miami","Dolphins")
      this.AddTeam("BB","Buffalo","Bills")
      this.AddTeam("NE","New England","Patriots")
      this.AddTeam("NJ","New York","Jets")

      *------AFC West
      this.AddTeam("LC","Los Angeles","Chargers")
      this.AddTeam("KC","Kansas City","Chiefs")
      this.AddTeam("DB","Denver","Broncos")
      this.AddTeam("OR","Oakland","Raiders")

      *---NFC
      *------NFC North
      this.AddTeam("CH","Chicago","Bears")
      this.AddTeam("DL","Detroit","Lions")
      this.AddTeam("GB","Green Bay","Packers")
      this.AddTeam("MV","Minnesota","Vikings")
      
      *------NFC South
      this.AddTeam("AF","Atlanta","Falcons")
      this.AddTeam("TB","Tampa Bay","Buccaneers")
      this.AddTeam("NO","New Orleans","Saints")
      this.AddTeam("CP","Carolina","Panthers")

      *------NFC East
      this.AddTeam("DC","Dallas","Cowboys")
      this.AddTeam("NG","New York","Giants")
      this.AddTeam("PE","Philadelphia","Eagles")
      this.AddTeam("WR","Washington","Redskins")

      *------NFC West
      this.AddTeam("LR","Los Angeles","Rams")
      this.AddTeam("SF","San Francisco","49ers")
      this.AddTeam("SS","Seattle","Seahawks")
      this.AddTeam("AC","Arizona","Cardinals")
      
   RETURN


   PROCEDURE AddTeam
      parameters cCode, cCity, cNick
      local oTeam
      oTeam = createObject("NFLTeam",cCode,cCity,cNick)
      this.teams.Add( oTeam )
      this.nCount = this.nCount + 1
      *? "added " + cCity + " " + cNick + ":" + oTeam.cTeamCode
      oTeam.destroy()
   RETURN


   FUNCTION TeamFor
      parameters cNickname
      local oTeam, theTeam
      for each oTeam in this.Teams
         if oTeam.cNickname = cNickname then
            theTeam = oTeam
            exit
         endif
      next
   return theTeam


   FUNCTION TeamForFull
      parameters cFullname
      local oTeam, theTeam
      theTeam = null
      for each oTeam in this.Teams
         *? oTeam.cFullName
         if alltrim(oTeam.cFullName) = alltrim(cFullname) then
            theTeam = oTeam
            exit
         endif
      next
      if isNull(theTeam)
         ? cFullname + " not found"
      endif

   return theTeam


enddefine


define class NFLTeam as custom

   cTeamCode = ""
   cCity     = ""
   cNickname = ""
   cFullName = ""
   aliases = null

   PROCEDURE INIT

      parameters cTeamCode, cCity, cNick

      this.cTeamCode = cTeamCode
      this.cCity = cCity
      this.cNickName = cNick
      this.cFullName = cCity + " " + cNick
      this.aliases = createObject("Collection")
      this.aliases.Add( cNick )

   RETURN


   PROCEDURE ClearRoles
      local oDataController
      oDataController = createobject("NFLDataController", "k:\tfl\nfl\")
      oDataController.ClearRoles( this.cTeamCode )
      #ifdef K_VERBOSE	  
      ? "NFLTeam:ClearRoles cleared " + lstr(_tally) + " roles for " + this.cTeamCode
	  #endif
   RETURN
   
   PROCEDURE ClearReturners
      local oDataController
      oDataController = createobject("NFLDataController", "k:\tfl\nfl\")
      oDataController.ClearReturners( this.cTeamCode )
      #ifdef K_VERBOSE	  
      ? "NFLTeam:ClearReturners cleared " + lstr(_tally) + " positions for " + this.cTeamCode
	  #endif
   RETURN   


enddefine

