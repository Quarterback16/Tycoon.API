#include NFLParser.h

define class NFLDataController as custom

   cDataPath = ""

	PROCEDURE Init
      parameters cDataPath
      this.cDataPath = cDataPath
	   this.cDataPath = "e:\tfl\nfl\"
	RETURN


   FUNCTION GetPlayer

      parameters cTeamcode, cFirst, cSurname, dGame

      local oPlayerData
      local nLenSurname
 	   nLenSurname = this.mylen(alltrim(cSurname))   &&  effectively set exact on
* 	   nLenSurname = len(alltrim(cSurname))   &&  effectively set exact on

		if cSurname = K_TEST_PLAYER
         ? "-----------1234567890"
		   ? "length of >" + cSurname + " is " + str( nLenSurname, 3 )
		endif

		*---Purify first name in case there is a jersey number in there
		cFirst = this.Purify( cFirst )

		nLenFirst = len(trim(cFirst))

		if cSurname = K_TEST_PLAYER
			? "SELECT * from PLAYER, SERVE "
			?? " where substr(PLAYER->SURNAME,1," + alltrim(str(nLenSurname)) + ") = '" + substr(cSurname,1,nLenSurname) + "'"
			?? " .and. SERVE->TEAMID = '" + cTeamCode + "' .and.  CURRTEAM = '" + cTeamCode + "'"
			?? " .and. substr(FIRSTNAME, 1, " + alltrim(str(nLenFirst)) + ") = '" + cFirst + "' .and. .not. deleted() .and. "
			?? " SERVE.PLAYERID = PLAYER.PLAYERID .and. "
			?? "(  SERVE.FROM   <= " + dtoc( dGame ) + " .and. "
			?? "(  empty( SERVE.TO ) .or. SERVE.TO  >= " + dtoc( dGame ) + " ) )"
		endif
		
	   SELECT * from ( this.cDataPath + "PLAYER" ), ;
                    ( this.cDataPath + "SERVE"  ) ;
         where substr(SURNAME,1,nLenSurname) = substr(cSurname,1,nLenSurname) ;
			      .and. SERVE->TEAMID = cTeamCode .and. CURRTEAM = cTeamCode;
				  .and. SURNAME = cSurname;
               .and. substr(FIRSTNAME, 1, nLenFirst) = cFirst  .and. .not. deleted() .and.;
               SERVE.PLAYERID = PLAYER.PLAYERID .and.  ;
               ( SERVE.FROM <= dGame .and. ;
               ( empty(SERVE.TO) .or. SERVE.TO >= dGame ) ) ;
         INTO CURSOR TempCurs

      if _tally = 1
         select TEMPCURS
         go top
         oPlayerData = createObject("NFLPlayerData",      ;
                                    TEMPCURS->PLAYERID_A,   ;
                                    TEMPCURS->SURNAME,   ;
                                    TEMPCURS->FIRSTNAME,   ;
                                    TEMPCURS->CURRTEAM,   ;
                                    TEMPCURS->ROLE,   ;
                                    TEMPCURS->PREVROLE,   ;									
                                    TEMPCURS->HEIGHT_FT,   ;
                                    TEMPCURS->HEIGHT_IN,   ;
                                    TEMPCURS->WEIGHT,   ;
                                    TEMPCURS->FORTY,   ;
                                    TEMPCURS->COLLEGE,   ;
                                    TEMPCURS->ROOKIEYR,   ;
                                    TEMPCURS->POSDESC,   ;
                                    TEMPCURS->CATEGORY,   ;
                                    TEMPCURS->DOB )
      else
         *if cSurname = K_TEST_PLAYER
            ? "GetPlayer: " + cFirst + " " + cSurname + " not found for "
            ?? cTeamCode + space(3) + "tally" + str(_tally,3)
            ?? " on " + dtoc( dGame ) + + str(nLenSurname,3)
         *endif

         oPlayerData = null
         if _Tally > 1
            *if cSurname = K_TEST_PLAYER
				? "SELECT * from PLAYER, SERVE "
				?? " where substr(PLAYER->SURNAME,1," + alltrim(str(nLenSurname)) + ") = '" + substr(cSurname,1,nLenSurname) + "'"
				?? " .and. SERVE->TEAMID = '" + cTeamCode + "' .and.  CURRTEAM = '" + cTeamCode + "'"
				?? " .and. substr(FIRSTNAME, 1, " + alltrim(str(nLenFirst)) + ") = '" + cFirst + "' .and. .not. deleted() .and. "
				?? " SERVE.PLAYERID = PLAYER.PLAYERID .and. "
				?? "(  SERVE.FROM   <= ctod('" + dtoc( dGame ) + "') .and. "
				?? "(  empty( SERVE.TO ) .or. SERVE.TO  >= ctod('" + dtoc( dGame ) + "') ) )"			
               ? "GetPlayer: " + cFirst + " " + cSurname + " too many "
               ?? "(" + lstr(_tally) + ") " + cTeamCode
               ?? " on " + dtoc( dGame )
               select TEMPCURS
               scan
                  ? PLAYERID_A
                  ?? space(2) + CURRTEAM
                  ?? space(2) + TEAMID
                  ?? space(2) + dtoc(FROM)
                  ?? space(2) + dtoc(TO)
               endscan
            *endif
         endif
      endif
      if cSurname = K_TEST_PLAYER
         ? "GetPlayer: " + cFirst + " " + cSurname
         ?? "  Tally=" + str( _Tally, 4 )
      endif

   return oPlayerData

	FUNCTION Purify
	  parameters cStr

     local nLen, cChar, cc, cNewStr
     nLen = 0
     cNewStr = ""
     For cc = 1 To Len( cStr)
         cChar = Substr( cStr, cc, 1 )
			if	asc(cChar) > 64
				*--a letter (numbers will be filtered out)
				cNewStr = cNewStr + cChar
	     endif
	  next cc

   return cNewStr


	FUNCTION mylen
	  parameters cStr
     local nLen, cChar, cc
     nLen = 0
     For cc = 1 To Len( cStr)
         cChar = Substr( cStr, cc, 1 )
			if	asc(cChar) > 64
				nLen = nLen + 1
			   *? str( cc, 3 ) + "  >" + cChar + str( asc(cChar), 4 )
	     endif
	  next cc

   return nLen


   FUNCTION SaveStat
      parameters oStat
      local lOk
      
      if oStat.PlayerID = "????????"
         *---Failed validation
         lOk = .f.
      else
         lOk = .t.
         insert into ( this.cDataPath + "Stat" ) ;
             (SEASON, ;
    		     WEEK, ;
    		     GAMENO, ;
    		     PLAYERID, ;
    		     STAT, ;
    		     QTY, ;
    		     TEAMID ) ;
            values ;
             (oStat.Season,;
              oStat.Week,;
              oStat.GameNo,;
              oStat.PlayerID,;
              oStat.StatType, ;
              oStat.Qty, ;
              oStat.TeamCode )
      endif

   RETURN lOk


   FUNCTION AddPlayer
      parameters oPlayer
      local lOk
      
      if oPlayer.cPlayerID = "????????"
         *---Failed validation
         lOk = .f.
      else
         lOk = .t.
         insert into ( this.cDataPath + "Player" ) ;
             (PLAYERID,  ;
    		     SURNAME,   ;
    		     FIRSTNAME, ;
    		     CURRTEAM,  ;
              ROLE,      ;
    		     INJURY,    ;
    		     Height_FT, ;
    		     Height_IN, ;
    		     Weight,    ;
              College,   ;
              RookieYr,  ;
              POSDESC,   ;
              CATEGORY,  ;
              DOB        ;
    		     ) ;
            values ;
             ( ;
               oPlayer.cPlayerID, ;
               oPlayer.cSurname,  ;
               oPlayer.cFirst,    ;
               oPlayer.TeamCode,  ;
               oPlayer.cRole,     ;
               0,                 ;
               oPlayer.HeightFt,  ;
               oPlayer.HeightIn,  ;
               oPlayer.Weight,    ;
               oPlayer.cCollege,  ;
               val(oPlayer.cRookieYr), ;
               oPlayer.cPos,      ;
               oPlayer.cCategory, ;
               oPlayer.DOB ;
             )

         *---Add a new service record for the new team
         insert into ( this.cDataPath + "SERVE" ) ;
                (;
    		        PLAYERID, ;
    		        TEAMID, ;
    		        FROM, ;
    		        HOW ) ;
               values ;
                (;
                 oPlayer.cPlayerID,;
                 oPlayer.TeamCode, ;
                 ctod("09/01/" + K_SEASON), ;
                 "FA-" )

             
      endif

   RETURN lOk

   
   function ClearGame
      parameters cSeason, cWeek, cGameNo
	  *? "deleting stuff from " + this.cDataPath + " SCORE, STAT & FGATT game:" + cGameNo	  
      delete from ( this.cDataPath + "SCORE" ) ;
         where SEASON = cSeason .and. WEEK = cWeek .and. GAMENO = cGameNo
      delete from ( this.cDataPath + "STAT" ) ;
         where SEASON = cSeason .and. WEEK = cWeek .and. GAMENO = cGameNo
      delete from ( this.cDataPath + "FGATT" ) ;
         where SEASON = cSeason .and. WEEK = cWeek .and. GAME = cGameNo
   endfunc


   function ClearLineups
      parameters oGame
      delete from ( this.cDataPath + "LINEUP" ) ;
         where SEASON = oGame.Season .and. WEEK = oGame.Week .and. GAMECODE = oGame.GameNo
   endfunc

   function ClearRoles
      parameters cTeamCode
	  *--Save old roles for comparison purposes
      update ( this.cDataPath + "PLAYER" ) ;
         set PLAYER.PREVROLE = PLAYER.ROLE ;
         where ( PLAYER.CURRTEAM = cTeamCode ) .and. ;
               .not. ( PLAYER.ROLE $  "I,X,H,R") ;
              .and. .not. deleted()	  
      *--Reset roles except for injuries and suspensions
      update ( this.cDataPath + "PLAYER" ) ;
         set PLAYER.ROLE = " " ;
         where ( PLAYER.CURRTEAM = cTeamCode ) .and. ;
               .not. ( PLAYER.ROLE $  "I,X,H,R") ;
              .and. .not. deleted()
   endfunc

   function ClearReturners
      parameters cTeamCode
	  *--Save old roles for comparison purposes
      update ( this.cDataPath + "PLAYER" ) ;
         set PLAYER.POSDESC = this.StripReturners(PLAYER.POSDESC) ;
         where ( PLAYER.CURRTEAM = cTeamCode )  ;
              .and. .not. deleted()	  
   endfunc   

   function StripReturners
      parameters cPos
	  newPos = StrTran( cPos, ",KR" )
	  newPos = StrTran( newPos, "KR" )
	  newPos = StrTran( newPos, ",PR" )
	  newPos = StrTran( newPos, "PR" )
   return newPos

   
   function TransferPlayer
      parameters cPlayerID, cNewTeam, cOldTeam, dTransfer

      *---Optionally close the last service record
      if cOldTeam <> "??"
         update ( this.cDataPath + "SERVE" ) ;
            set SERVE.TO = dTransfer-1 ;
            where ( SERVE.PLAYERID = cPlayerID ) ;
                 .and. empty(SERVE.TO) .and. .not. deleted()
      endif

      *---Add a new service record for the new team
      insert into ( this.cDataPath + "SERVE" ) ;
             (;
    		     PLAYERID, ;
    		     TEAMID, ;
    		     FROM, ;
    		     HOW ) ;
            values ;
             (;
              cPlayerID,;
              cNewTeam, ;
              dTransfer, ;
              "FA-" )

   RETURN .t.

   
   function UpdateRole
      parameters cPlayerID, cRole, cPos, nShirt
		*
		*  Update the role as per the boxscore, however, boxscore
		*  may say a player is a backup when they are in reality
		*  injured - do not update POS anymore
		*  dont update Running backs either
		local bUpdate
		bUpdate = .t.

		if	cRole = "B"
		   *---Backup
			SELECT * from ( this.cDataPath + "PLAYER" ) ;
				where PLAYERID = cPlayerID .and. .not. deleted() ;
				INTO CURSOR TempCurs
			
			if _tally = 1
				select TEMPCURS
				go top
				if	PLAYER->ROLE = "I"
					*---Leave Injured players be unless they actually start
					bUpdate = .f.
				endif
				if	PLAYER->ROLE = "R"
					*---Leave Reserve players
					bUpdate = .f.
				endif				
			endif
		endif

		if cPos = "PK"
		   *---Dont worry about kickers
		   bUpdate = .f.
		endif
		
		if	bUpdate
			*--- not updating Role anymore - better to do this manually --- set ROLE = cRole, JERSEY = nShirt ;

		    if  Empty( cPos )
				update ( this.cDataPath + "PLAYER" ) ;
					set JERSEY = nShirt ;					
					where PLAYERID = cPlayerID .and. .not. deleted()
				if _Tally > 0
					? "UpdateRole-Updated " + cPlayerID + "("+cRole+") shirt set to " + str( nShirt, 2 )
				else
					*? "UpdateRole-No Update on " + cPlayerID
				endif					
					
			else
				update ( this.cDataPath + "PLAYER" ) ;
					set JERSEY = nShirt ;
					where PLAYERID = cPlayerID .and. .not. deleted()
				if _Tally > 0
					? "UpdateRole-Updated " + cPlayerID + "("+cRole+") pos NOT set to " + cPos + " shirt set to " + str( nShirt, 2 )
				else
					*? "UpdateRole-No Update on " + cPlayerID
				endif					
			endif

		endif

   endfunc


   FUNCTION SaveScore
      parameters oScore
      local lOk
      if oScore.PlayerID1 = "????????" .or. ;
         oScore.PlayerID2 = "????????" then
         lOk = .f.
      else
         insert into ( this.cDataPath + "SCORE" ) ;
             (SEASON, ;
    		     WEEK, ;
    		     GAMENO, ;
    		     PLAYERID1, ;
    		     WHEN, ;
    		     SCORE, ;
    		     DISTANCE, ;
    		     PLAYERID2, ;
    		     TEAM ) ;
            values ;
             (oScore.Season,;
              oScore.Week,;
              oScore.GameNo,;
              oScore.PlayerID1,;
              oScore.When, ;
              oScore.Score, ;
              val(oScore.Distance), ;
              oScore.PlayerID2, ;
              oScore.TeamCode )
         lOk = .t.
      endif

   return lOk

	procedure SaveResult
      parameters cSeason, cWeek, cGameNo, nAwayScore, nHomeScore
      update ( this.cDataPath + "SCHED" ) ;
         set AWAYSCORE = nAwayScore, HOMESCORE = nHomeScore ;
        where SEASON = cSeason .and. WEEK = cWeek .and. GAMENO = cGameNo
   return

   function GetGameNo
      parameters dGame, cHomeCode
      select GAMENO from ( this.cDataPath + "SCHED" ) ;
        where GAMEDATE = dGame .and. HOMETEAM = cHomeCode ;
              .and. .not. deleted();
         INTO CURSOR TempCurs

      if _tally = 1
         select TEMPCURS
         go top
         cGameNo = TEMPCURS->GAMENO
      else
         cGameNo = "?"
         ? "GetGameNo: tally=" + ltrim(str(_Tally)) + " " + dtoc(dGame)
         ?? "  " + cHomeCode
      endif

   return cGameNo

   function AddPlays
      parameters cWeek, cSeason
      local oPlays
      ? "Looking for plays in week " + cWeek + " " + cSeason
      select * ;
         from ( this.cDataPath + "SCHED" ) ;
         where WEEK = cWeek .and. SEASON = cSeason ;
         .and. SPREAD <> 0 ;
         .and. .not. deleted();
         INTO cursor TempCurs

      ? lstr( _Tally ) + " games possible"

      DO case
         *---TODO change this to Segment 1
         case Segment(cWeek) = "1"
            ? "Segment 1"
            if _tally > 0
               oPlays = createObject("Collection")
               select TEMPCURS
               go top
               scan
                  *---The dog is always the play
                  if TEMPCURS.SPREAD > 0
                     *---Away team is the dog
                     oPlay = createObject( "Play", TEMPCURS.AWAYTEAM, ;
                                           TEMPCURS.HOMETEAM, ;
                                           TEMPCURS.SPREAD, "A", cSeason,;
                                           cWeek, TEMPCURS.GAMENO,;
                                           TEMPCURS.GAMELIVE,;
                                           TEMPCURS.GAMEDATE,;
                                           TEMPCURS.GAMEHOUR )
                  else
                     *---Home dog
                     oPlay = createObject( "Play", TEMPCURS.HOMETEAM, ;
                                           TEMPCURS.AWAYTEAM, ;
                                           TEMPCURS.SPREAD, "H", cSeason,;
                                           cWeek, TEMPCURS.GAMENO,;
                                           TEMPCURS.GAMELIVE,;
                                           TEMPCURS.GAMEDATE,;
                                           TEMPCURS.GAMEHOUR )
                  endif
                  oPlays.Add(oPlay)


               endscan
               *? lstr( oPlays.Count ) + " plays found"
            else
               oPlays = null
            endif

         case Segment(cWeek) = "2"
            ? "Segment 2"
            if _tally > 0
               oPlays = createObject("Collection")
               select TEMPCURS
               go top
               scan
                  *---The dog is always the play
                  if TEMPCURS.SPREAD > 0
                     *---Away team is the dog
                     oPlay = createObject( "Play", TEMPCURS.AWAYTEAM, ;
                                           TEMPCURS.HOMETEAM, ;
                                           TEMPCURS.SPREAD, "A", cSeason,;
                                           cWeek, TEMPCURS.GAMENO,;
                                           TEMPCURS.GAMELIVE,;
                                           TEMPCURS.GAMEDATE,;
                                           TEMPCURS.GAMEHOUR )
                  else
                     *---Home dog
                     oPlay = createObject( "Play", TEMPCURS.HOMETEAM, ;
                                           TEMPCURS.AWAYTEAM, ;
                                           TEMPCURS.SPREAD, "H", cSeason,;
                                           cWeek, TEMPCURS.GAMENO,;
                                           TEMPCURS.GAMELIVE,;
                                           TEMPCURS.GAMEDATE,;
                                           TEMPCURS.GAMEHOUR )
                  endif
                  oPlays.Add(oPlay)
                  if abs(TEMPCURS.SPREAD) < 3.5
                     *---add the 3 or less point favourite
                     if TEMPCURS.SPREAD > 0
                        *---Slight home favorite
                        oPlay = createObject( "Play", TEMPCURS.HOMETEAM, ;
                                              TEMPCURS.AWAYTEAM, ;
                                              TEMPCURS.SPREAD, "H", cSeason,;
                                              cWeek, TEMPCURS.GAMENO,;
                                              TEMPCURS.GAMELIVE,;
                                              TEMPCURS.GAMEDATE,;
                                              TEMPCURS.GAMEHOUR )
                     else
                        *---Slight away favourite
                        oPlay = createObject( "Play", TEMPCURS.AWAYTEAM, ;
                                              TEMPCURS.HOMETEAM, ;
                                              TEMPCURS.SPREAD, "A", cSeason,;
                                              cWeek, TEMPCURS.GAMENO,;
                                              TEMPCURS.GAMELIVE,;
                                              TEMPCURS.GAMEDATE,;
                                              TEMPCURS.GAMEHOUR )

                     endif


                     oPlays.Add(oPlay)
                  endif

               endscan
               *? lstr( oPlays.Count ) + " plays found"
            else
               oPlays = null
            endif

      endcase

   return oPlays


   FUNCTION SpreadRangeFor
      parameters cTeam, cSeason
      local nSprRange
      select SRANGE from (this.cDataPath + "TEAM" ) ;
         where SEASON = cSeason .and. TEAMID = cTeam ;
         .and. .not. deleted();
               INTO cursor TempCurs
      if _tally = 1
         nSprRange = TEMPCURS.SRANGE
      else
         nSprRange = 0
      endif      
   return nSprRange


   FUNCTION DivisionFor
      parameters cTeam, cSeason
      local cDiv
      select DIVISION from ( this.cDataPath + "TEAM" ) ;
         where SEASON = cSeason .and. TEAMID = cTeam ;
         .and. .not. deleted();
               INTO cursor DivCurs

      if _tally = 1
         cDiv = DIVCURS.DIVISION
      else
         cDiv = "?"
      endif
      select DIVCURS
      use
   return cDiv


   function GetLastGame
      parameters cTeam
      local oResult
      select * from  ( this.cDataPath + "SCHED" ) ;
         where ( HOMETEAM = cTeam .or. AWAYTEAM = cTeam ) .and. ;
               GAMEDATE < date() .and. ;
               .not. deleted();
               order by gamedate ;
               INTO cursor TempCurs

      if _tally <> 0
         select Tempcurs
         GO BOTTOM
         oResult = createObject( "Result", ;
                                 TEMPCURS.HOMETEAM, TEMPCURS.AWAYTEAM,;
                                 TEMPCURS.HOMESCORE, TEMPCURS.AWAYSCORE, ;
                                 TEMPCURS.GAMEDATE, TEMPCURS.SPREAD,;
                                 TEMPCURS.SEASON )

      else
         ? LSTR(_TALLY) + "  No recent game found for " + cTeam
         oResult = null
      endif

   return oResult


   function GetGamesFor
      parameters cTeam1, dFrom, dTo
      local oResult, oResults
      ?  "GetGamesFor:" + cTeam1
      ?? " from " + dtoc(dFrom)
      ?? " to " + dtoc(dTo)
      oResults = createObject("Collection")
      select * from  ( this.cDataPath + "SCHED" ) ;
         where ( HOMETEAM $ cTeam1 .or. AWAYTEAM $ cTeam1 ) .and. ;
               GAMEDATE >= dFrom .and. GAMEDATE <= dTo .and.;
               .not. deleted();
               INTO cursor TempGames

      select TEMPGAMES
      *? lstr(_tally) + " games found between " + cTeam1 + " and " + cTeam2
      *?? " since " + dtoc(dFrom)
      scan

         if TEMPGAMES.GAMEDATE >= dFrom .and. TEMPGAMES.GAMEDATE < date()
            if TEMPGAMES.HOMESCORE + TEMPGAMES.AWAYSCORE > 0
               oResult = createObject( "Result", ;
                                       TEMPGAMES.HOMETEAM, TEMPGAMES.AWAYTEAM,;
                                       TEMPGAMES.HOMESCORE, TEMPGAMES.AWAYSCORE, ;
                                       TEMPGAMES.GAMEDATE, TEMPGAMES.SPREAD,;
                                       TEMPGAMES.SEASON )
               oResults.Add(oResult)

            endif

         endif

      endscan
   return oResults


   function GetGamesBetween
      parameters cTeam1, cTeam2, dFrom
      local cTeams, oResult, oResults
      cTeams = cTeam1+","+cTeam2
      oResults = createObject("Collection")
      select * from  ( this.cDataPath + "SCHED" ) ;
         where ( HOMETEAM $ cTeams .and. AWAYTEAM $ cTeams ) .and. ;
               GAMEDATE > dFrom .and. ;
               .not. deleted();
               INTO cursor TempCurs

      select TEMPCURS
      *? lstr(_tally) + " games found between " + cTeam1 + " and " + cTeam2
      *?? " since " + dtoc(dFrom)
      scan

         if TEMPCURS.GAMEDATE >= dFrom .and. TEMPCURS.GAMEDATE < date()
            if TEMPCURS.HOMESCORE + TEMPCURS.AWAYSCORE > 0
               oResult = createObject( "Result", ;
                                       TEMPCURS.HOMETEAM, TEMPCURS.AWAYTEAM,;
                                       TEMPCURS.HOMESCORE, TEMPCURS.AWAYSCORE, ;
                                       TEMPCURS.GAMEDATE, TEMPCURS.SPREAD,;
                                       TEMPCURS.SEASON )
               oResults.Add(oResult)

            endif

         endif

      endscan
   return oResults


   function TeamFor
      parameters cTeamCode, cSeason
      local cTeamName

      select TEAMNAME from ( this.cDataPath + "TEAM" ) ;
         where SEASON = cSeason .and. TEAMID = cTeamCode ;
         .and. .not. deleted();
               INTO cursor TempCurs

      if _tally = 1
         cTeamName = TEMPCURS.TEAMNAME
      else
         cTeamName = "???"
      endif

   return cTeamName


   FUNCTION LoadRoster
      parameters cTeamCode
      local oPlayers, oPlayer
      oPlayers = createObject("Collection")
      select * from  ( this.cDataPath + "PLAYER" ) ;
         where CURRTEAM = cTeamCode .and. .not. deleted() ;
         into cursor TempRost

      select TEMPRost
      scan
         oPlayer = createObject("NFLPlayer",)

      endscan
      use
      
   return oPlayers

	
   FUNCTION GetPos

      parameters cPlayer, cTeamCode

		*--- get the players pos desc

      local cSurname, cFirst, nSpot, lOk

      lOk = .f.
      nSpot = at( ".", cPlayer, 1 )
      if nSpot > 1
         cFirst = substr( cPlayer, 1,  nSpot-1 )
         cSurname = substr( cPlayer, nSpot+1, len(cPlayer)-nSpot )
         if .not. empty( cSurname )
            select POSDESC from ( this.cDataPath + "PLAYER" ) ;
               where CURRTEAM  = cTeamCode .and. ;
                     FIRSTNAME = cFirst .and. ;
                     SURNAME   = cSurname .and. .not. deleted() 
            if _Tally > 1
               ? "IncrPos-Updated " + lstr(_tally) + " records for " + ;
                  cFirst + " " + cSurname
            else
               if _tally = 0
                  *  Updating pos for A Rossum KR-AF
                  ? "IncrPos-No update for " + cFirst + " " + cSurname +;
                    " " + cPosInc + "-" + cTeamCode
               else
                  lOk = .t.
               endif
            endif
         endif
		else
		   ? "IncrPos:no . spot in cPlayer-" + cPlayer
      endif

   return lOk

	

   FUNCTION IncrPos

      parameters cPlayer, cTeamCode, cPosInc

		*--- Extends the position description for a player

      local cSurname, cFirst, nSpot, lOk

      lOk = .f.
      nSpot = at( ".", cPlayer, 1 )
      if nSpot > 1
         cFirst = substr( cPlayer, 1,  nSpot-1 )
         cSurname = substr( cPlayer, nSpot+1, len(cPlayer)-nSpot )
         if .not. empty( cSurname )
            update ( this.cDataPath + "PLAYER" ) ;
               set POSDESC = trim(POSDESC) + "," + cPosInc ;
               where CURRTEAM  = cTeamCode .and. ;
                     FIRSTNAME = cFirst .and. ;
                     SURNAME   = cSurname .and. .not. deleted() ;
							.and. not ( cPosInc $ PLAYER.POSDESC )
            if _Tally > 1
               ? "IncrPos-Updated " + lstr(_tally) + " records for " + ;
                  cFirst + " " + cSurname
            else
               if _tally = 0
                  *  Updating pos for A Rossum KR-AF
                  ? "IncrPos-No update for " + cFirst + " " + cSurname +;
                    " " + cPosInc + "-" + cTeamCode
               else
                  lOk = .t.
               endif
            endif
         endif
		else
		   ? "IncrPos:no . spot in cPlayer-" + cPlayer
      endif

   return lOk


   FUNCTION NextID

      parameters cFirst, cSurname

      local cLast4, cFirst2
      cLast4 = substr( cSurname, 1, 4 )
      cFirst2 = substr( cFirst, 1, 2 )

	   SELECT * from ( this.cDataPath + "PLAYER" ) ;
         where substr( SURNAME, 1, 4 ) = cLast4 .and. ;
               substr( FIRSTNAME, 1, 2 ) = cFirst2 ;
               .and. .not. deleted();
         INTO CURSOR TempCurs

      cNextID = cLast4 + cFirst2 + strzero( _tally + 1, 2 )

   return upper( cNextID )


   FUNCTION GetNamedPlayer

      parameters cFirst, cSurname

      local oPlayerData

	   SELECT * from ( this.cDataPath + "PLAYER" ) ;
         where SURNAME = cSurname .and. ;
               FIRSTNAME = cFirst .and. .not. deleted() ;
         INTO CURSOR TempCurs

      if _tally = 1
         select TEMPCURS
         go top
         oPlayerData = createObject("NFLPlayerData",      ;
                                    TEMPCURS->PLAYERID,   ;
                                    TEMPCURS->SURNAME,   ;
                                    TEMPCURS->FIRSTNAME,   ;
                                    TEMPCURS->CURRTEAM,   ;
                                    TEMPCURS->ROLE,   ;
                                    TEMPCURS->PREVROLE,   ;									
                                    TEMPCURS->HEIGHT_FT,   ;
                                    TEMPCURS->HEIGHT_IN,   ;
                                    TEMPCURS->WEIGHT,   ;
                                    TEMPCURS->FORTY,   ;
                                    TEMPCURS->COLLEGE,   ;
                                    TEMPCURS->ROOKIEYR,   ;
                                    TEMPCURS->POSDESC,   ;
                                    TEMPCURS->CATEGORY,   ;
                                    TEMPCURS->DOB )
      else
         ? "GetNamedPlayer: one " + cFirst + " " + cSurname + " not found"
         oPlayerData = null
         if _Tally > 1
            ? "GetNamedPlayer: " + cFirst + " " + cSurname + " found "
            ?? "(" + lstr(_tally) + ") "
            select TEMPCURS
            scan
               ? PLAYERID
               ?? space(2) + CURRTEAM
               *?? space(2) + TEAMID
            endscan
         else
            *---Need to send back an empty oPlayerData
            oPlayerData = createObject("NFLPlayerData",      ;
                                       "????????",;
                                       cSurname,   ;
                                       cFirst,   ;
                                       "??",   ;
                                       " ",   ;
                                       " ",   ;									   
                                       6,   ;
                                       2,   ;
                                       220,   ;
                                       0,   ;
                                       "",   ;
                                       "",   ;
                                       "",   ;
                                       "0",   ;
                                       ctod(" ") )
         endif
      endif

   return oPlayerData



   FUNCTION SaveLineup
      parameters oPlayer, oGame, cTeam
      local lOk
      
      if oPlayer.cPlayerID = "????????"
         *---Failed validation
         lOk = .f.
      else
         lOk = .t.
         insert into ( this.cDataPath + "Lineup" ) ;
             (SEASON, ;
    		     WEEK, ;
    		     GAMECODE, ;
    		     PLAYERID, ;
              SHIRT, ;
              POS,  ;
              START,  ;
    		     TEAMCODE ) ;
            values ;
             (oGame.Season,;
              oGame.Week,;
              oGame.GameNo,;
              oPlayer.cPlayerID, ;
              oPlayer.nJersey, ;
              oPlayer.cPos, ;
              iif( oPlayer.cRole = "S", .t., .f. ), ;
              cTeam )
      endif

   RETURN lOk


   FUNCTION SaveFGAtt
      parameters oFGAtt
      local lOk
      
      if oFGAtt.PlayerID = "????????"
         *---Failed validation
         lOk = .f.
      else
         lOk = .t.
         insert into ( this.cDataPath + "FGATT" ) ;
             (SEASON, ;
    		     WEEK, ;
    		     GAME, ;
    		     PLAYERID, ;
              DISTANCE, ;
              TEAMCODE,  ;
    		     GOOD ) ;
            values ;
             (oFGAtt.Season,;
              oFGAtt.Week,;
              oFGAtt.GameNo,;
              oFGAtt.PlayerID, ;
              oFGAtt.Distance, ;
              oFGAtt.TeamCode, ;
              oFGAtt.Good )
      endif

   RETURN lOk


enddefine


