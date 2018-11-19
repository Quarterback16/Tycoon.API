#include NFLParser.h

define class NFLPlayer as custom

   cPlayerID = ""
   cFirst    = ""
   cSurname  = ""
   TeamCode  = ""
   cName     = ""
   nJersey   = 0
   cRole     = " "
   cPrevRole = " "
   cPos      = ""
   cRookieYr = K_SEASON
   HeightFt  = 6
   HeightIn  = 0
   Weight    = 220
   cCategory = "5"
   DOB       = date()
   cCollege  = "Miami"

   procedure Init
      parameters cTeamCode, cPlayer, dGame
      local oDataController, lFound, oPlayerData, nDot
      oDataController = createobject("NFLDataController", "e:\tfl\nfl\")

      if cTeamCode = "??"
         *---Unknown
         this.cFirst    = Word( 1, " " + cPlayer + " " )
         this.cSurname  = Word( 2, " " + cPlayer + " " )
         *? "NFLPlayer:Looking for " + this.cFirst + " " + this.cSurname
         oPlayerData = oDataController.GetNamedPlayer(  this.cFirst, ;
                                                     this.cSurname ;
                                                      )
      else
         this.TeamCode = cTeamCode
         this.cName = cPlayer
         *---Player comes in as J.Montana or J. Montana
         if cPlayer = K_TEST_PLAYERNAME
            ? "NFLPlayer:Player=" + cPlayer + " , tm="
            ?? cTeamCode + " date " + dtoc( dGame )
         endif

         nDot = at(".", cPlayer )
         if nDot = 0
            nDot = at(" ", cPlayer )
         endif

         this.cFirst = left( cPlayer,nDot-1 )
         if right(this.cFirst,1)="."
            *---Shave it
            this.cFirst = left( cPlayer,len(this.cFirst)-1 )
         endif

         this.cSurname = substr( cPlayer,nDot+1, len(cPlayer)-nDot )
         *---Find a matching player in the database

         if cPlayer = K_TEST_PLAYERNAME
            ? "NFLPlayer:cFirst=" + this.cFirst
            ? "NFLPlayer:cSurname=" + this.cSurname
         endif

         if empty(alltrim(this.cFirst)+alltrim(this.cSurname))
            ? "NFLPlayer:No name " + cPlayer
         else
            oPlayerData = oDataController.GetPlayer( cTeamcode, ;
                                                     this.cFirst, ;
                                                     this.cSurname, ;
                                                     dGame )
         endif
      endif
      oDataController.Destroy()
      if isNUll( oPlayerData )
         this.cPlayerID = "????????"
         #ifdef K_VERBOSE
         ? "NFLPlayer.Init player not found - " + this.cFirst + " " + this.cSurname + " " + cTeamcode
         #endif
      else
         this.cPlayerID = oPlayerData.PlayerID
         this.cFirst    = trim(oPlayerData.FirstName)
         this.cCollege  = oPlayerData.College
         this.TeamCode  = oPlayerData.CurrTeam
         this.cPos      = oPlayerData.PosDesc
         this.cRole     = oPlayerData.Role
		 this.cPrevRole = oPlayerData.PrevRole
         this.cRookieYr = oPlayerData.Rookieyr
         this.HeightFt  = oPlayerData.Height_ft
         this.HeightIn  = oPlayerData.Height_in
         this.Weight    = oPlayerData.Weight
         this.cCategory = oPlayerData.Category
         this.DOB       = oPlayerData.Dob
      endif

   RETURN


   PROCEDURE SetJersey
      parameters nShirt
      this.nJersey = nShirt
   RETURN

   PROCEDURE SetRole
      parameters cRole
      this.cRole = cRole
      *?? "  Role=" + cRole
   RETURN

   PROCEDURE SetPos
      parameters cPos
      this.cPos = cPos
   RETURN

   PROCEDURE SetCollege
      parameters cCollege
      this.cCollege = cCollege
   RETURN
 
   PROCEDURE UpdateRole
      this.cPos = StripNumbers( this.cPos )
      local oDataController
      oDataController = createobject("NFLDataController", "e:\tfl\nfl\")
      oDataController.UpdateRole( this.cPlayerID, this.cRole, this.cPos, this.nJersey )   
   RETURN

   PROCEDURE ReportRole
      cRoleChange = this.RoleChange()
	  if ( .not. empty( cRoleChange ) )
	     ? "RoleChange :" + this.NameOut() + " : " + this.TeamCode + " : " + this.cPos + " : " + cRoleChange
	  endif
   RETURN
   
   FUNCTION NameOut
   RETURN alltrim(this.cFirst) + " " + alltrim(this.cSurname)

   FUNCTION RoleChange
     cChange = ""
	 if this.cCategory = "1" .or. this.cCategory = "2" .or. this.cCategory = "3" .or. this.cCategory = "4"  
		 if ( this.cPrevRole <> "S" ) .and. ( this.cRole = "S" )
			cChange = "Promoted to Starter from " + this.cPrevRole
		 endif
		 if ( this.cRole <> "S" ) .and. ( this.cPrevRole = "S" )
			cChange = "Benched to " + this.cRole
		 endif
	 endif
	 
   RETURN cChange
   
   PROCEDURE PrintOut
      ?  "   " + str(this.nJersey,2)
      ?? " " + this.cFirst
      ?? " " + this.cSurname
      ?? " " + this.cRole
      ?? " " + this.cPos
      *?? " " + this.cPlayerID
  return

enddefine



define class NFLPlayerData as custom

   PlayerID  = ""
   Surname   = ""
   Firstname = ""
   Currteam  = ""
   Role      = ""
   PrevRole  = ""
   Height_ft = ""
   Height_in = ""
   Weight    = ""
   Forty     = ""
   College   = ""
   Rookieyr  = ""
   Posdesc   = ""
   Category  = ""
   Dob       = ctod("")

   procedure Init
      parameters  PlayerID, ;
                  Surname  , ;
                  Firstname, ;
                  Currteam , ;
                  Role     , ;
                  PrevRole , ;				  
                  Height_ft, ;
                  Height_in, ;
                  Weight   , ;
                  Forty    , ;
                  College  , ;
                  Rookieyr , ;
                  Posdesc  , ;
                  Category , ;
                  Dob
      this.PlayerID    =   PlayerID
      this.Surname     =   Surname
      this.Firstname   =   Firstname
      this.Currteam    =   Currteam
      this.Role        =   Role
      this.PrevRole    =   PrevRole	  
      this.Height_ft   =   Height_ft
      this.Height_in   =   Height_in
      this.Weight      =   Weight
      this.Forty       =   Forty
      this.College     =   College
      this.Rookieyr    =   Rookieyr
      this.Posdesc     =   Posdesc
      this.Category    =   Category
      this.Dob         =   Dob

   return

enddefine








