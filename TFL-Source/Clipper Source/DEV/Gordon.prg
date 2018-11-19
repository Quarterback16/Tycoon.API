***
***  Worker class
***

DEFINE CLASS Gordon as Custom

   Season = "2009"
   Week   = "01"

   Plays  = null
   oDataController = null

   procedure Init
      this.oDataController = createobject("NFLDataController", "e:\tfl\nfl\")
      this.Plays = createObject("Collection")
   return

   procedure SetSeason
      parameters cSeason
      this.Season = cSeason
   RETURN


   procedure SetWeek
      parameters cWeek
      this.Week = cWeek
   return


   procedure Report
      ? "Gordon Line for " + this.Season + " week " + this.Week
      ?

      this.LoadPlays

      if isnull(this.Plays)
         ?
         ? "   NO PLAYS"
         ?
      else

         for each oPlay In this.Plays
            *---Look for emotional factors
            *---Segment 2 pointer 1 p72
            this.CheckForRevenge(oPlay)

            *---Segment 2 pointer 2 p72
            this.CheckForHumiliation(oPlay)

            *---Segment 2 pointer 3 p73
            this.CheckHomeDogs(oPlay)

            *---Segment 1 pointer 11 p70
            *---Segment 2 pointer 5 p73
            this.CheckSandwich(oPlay)

            *---Segment 3 pointer 1 p77
            this.CheckSpreadRange(oPlay)

            *---Segment 3 pointer 2 p77
            this.CheckUndefeatedHomeDog(oPlay)

            if oPlay.IsPlay
               ? oPlay.PrintLine()
               oPlay.PrintReasons()
               ?
            endif
         next
      endif

   return


   PROCEDURE CheckUndefeatedHomeDog
      parameters oPlay
      local oResults, oResult, lPlay, cReason, nDiff
      lPlay = .f.
      cReason = "Undefeated Home Dog"
      ? "Checking " + cReason + ":" + oPlay.TeamCode
      if oPlay.IsHomeDog
         ? "    home dog " + lstr( oPlay.Spread ) + "  "
         oResults = this.oDataController.GetGamesFor( ;
                           oPlay.TeamCode, ;
                           ctod("01/06/"+oPlay.Season), ;
                           oPlay.GameDate  )
         lPlay = .t.
         for each oResult in oResults
            if oResult.HomeTeam = oPlay.TeamCode
               if oResult.Lost( oPlay.TeamCode )
                  ? "    lost to " + oResult.AwayTeam + ;
                     ", " + oResult.Score + " " + ;
                     dtoc( oResult.MatchDate )
                  lPlay = .f.
                  exit
               endif
            endif
         next
         if lPlay
            oPlay.AddReason(cReason)
         endif
      else
         ? "    not home dog"
      endif
   RETURN
   

   PROCEDURE CheckSpreadRange

      parameters oPlay
      local lPlay, cReason, nDiff
      local nFavSpreadRange, nDogSpreadRange
      lPlay = .f.
      cReason = "Large Spread Range"

      ? "Checking for " + cReason
      
      if oPlay.IsDog
         nDogSpreadRange = this.oDataController.SpreadRangeFor( ;
                              oPlay.TeamCode, oPlay.Season )
         nFavSpreadRange = this.oDataController.SpreadRangeFor( ;
                              oPlay.OppCode , oPlay.Season )
         if oPlay.Week < "09"
            nRange = 10
         else
            nRange = 20
         endif
         nDiff = nFavSpreadRange - ( nDogSpreadRange + nRange )
         *? nDiff
         if nDiff >= 0
            lPlay = .t.
            cReason = cReason + ":" + oPlay.TeamCode + ;
                      " spread range "  + lstr(nDogSpreadRange) +;
                      "  " + oPlay.OppCode + ;
                      " spread range "  + lstr(nFavSpreadRange)
         endif
      endif

      if lPlay
         oPlay.AddReason(cReason)
      endif

   return


   PROCEDURE CheckSandwich

      parameters oPlay
      local oResults, oResult, lPlay, cReason, nPast, nUpcoming
      local d2WksAgo
      lPlay = .f.
      cReason = "Sandwich"

      ? "Checking for " + cReason
      
      if oPlay.NonDivisional()
         *? "Non Divisional game"
         d2WksAgo = ctod(dtoc( oPlay.GameDate))-16
         oResults = this.oDataController.GetGamesFor( ;
                                 oPlay.TeamCode, ;
                                 d2WksAgo,;
                                 oPlay.GameDate )
         *? lstr( oResults.Count ) + " results got"
         if oResults.Count < 2
            lPlay = .f.
         else
            lPlay = .t.
            nPast = 0
            for each oResult in oResults
               *? "   " + oResult.Score
               if oResult.NonDivisional
                  *?? "-non div"
                  lPlay = .f.
               else
                  *?? " " + oResult.HomeDiv
                  nPast = nPast + 1
               endif
            next
         endif

         if lPlay
            *? " last 2 games were divisional"
            nUpcoming = 0
            oResults = this.oDataController.GetGamesFor( ;
                                    oPlay.TeamCode, ;
                                    oPlay.GameDate+16,;
                                    oPlay.GameDate )
            for each oResult in oResults
               *? oResult.Score
               if oResult.NonDivisional
                  lPlay = .f.
                  *?? " -non div"
               else
                  nUpcoming = nUpcoming + 1
                  *?? " " + oResult.HomeDiv
               endif
            next
            if lPlay
               if nUpcoming = 2
                  *?? " next 2 games are divisional
               else
                  lPlay = .f.
               endif
            endif
         else
            *?? " non div games in last 2"
         endif

      else
         *? ":Divisional game"
      endif

      if lPlay
         oPlay.AddReason(cReason)
      endif

   return


   PROCEDURE CheckHomeDogs

      parameters oPlay
      local oResults, oResult, lHomeDog, cReason
      lHomeDog = .f.
      cReason = "HomeDog"

      if oPlay.HomeAway = "H" .and. oPlay.Spread < 0
         *---Home Dog play, but must have lost as Home Fav last game
         oResult = this.oDataController.GetLastGame( oPlay.TeamCode )
         if .not. isNull(oResult)
            if oResult.Lost( oPlay.TeamCode )
               if oResult.WasHomeFavourite(oPlay.TeamCode)
                  lHomeDog = .t.
                  cReason = cReason + " - " + oResult.Score() + "  " + ;
                            dtoc(oResult.MatchDate)
               endif
            endif
         else
            ? "Last result not found"
         endif
      endif

      if lHomeDog
         oPlay.AddReason(cReason)
      endif

   return


   PROCEDURE CheckForHumiliation

      parameters oPlay
      local oResults, oResult, lHumiliation, cReason
      lHumiliation = .f.
      cReason = "Humiliation"
      oResult = this.oDataController.GetLastGame( oPlay.TeamCode )
      cReason = cReason + " - " + oResult.Score() + "  " + ;
                dtoc(oResult.MatchDate)

      ? "Checking for " + cReason

      if oResult.Lost( oPlay.TeamCode )
         if oResult.Rout()
            lHumiliation = .t.
         endif
      endif

      if lHumiliation
         oPlay.AddReason(cReason)
      endif

   return


   PROCEDURE CheckForRevenge

      parameters oPlay
      local oResults, oResult, lRevenge, cReason, dSearch
      lRevenge = .f.
      cReason = "Revenge Motive"

      ? "Checking for " + cReason

      dSearch = (date()-365)
      oResults = this.oDataController.GetGamesBetween( oPlay.TeamCode,;
                                                       oPlay.OppCode,;
                                                       dSearch )

      ? "   " + lstr(oResults.Count) + " previous games"
                                                             
      for each oResult in oResults
         if oResult.Won( oPlay.OppCode )
            lRevenge = .t.
            cReason = cReason + " - " + oResult.Score() + "  " + ;
                      dtoc(oResult.MatchDate)
            ? "   " + cReason
            exit
         endif
      next

      if lRevenge
         oPlay.AddReason(cReason)
      endif

   RETURN


   PROCEDURE LoadPlays

      local oPlays

      oPlays = this.oDataController.AddPlays( this.Week, this.Season )

      for each oPlay in oPlays
         this.Plays.Add(oPlay)
      next

      *? lstr(this.Plays.Count) + " plays loaded"

   RETURN



enddefine

