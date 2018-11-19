*
*  TFL Score Sheet
*
*  Run this script then output the sheet using ssheet.frx
*

cLeague = "OL"
cWeek = "02"
cSeason = "1999"

close data
set safety off
use SSHEET in 1 exclusive
zap

use LINEUP in 2 shared
select LINEUP
index on LEAGUEID+SEASON+WEEK+OWNERID to $LINEUP

use TYSKED in 3 shared
select TYSKED
locate for SEASON=cSeason .and. WEEK=cWeek .and. LEAGUEID=cLeague

*---Game 1
cHomeTeam = TYSKED->HT1
cAwayTeam = TYSKED->AT1

select SSHEET
append blank
replace  WEEK with cWeek
replace HTEAM with cHomeTeam
replace ATEAM with cAwayTeam

replace POS    with 1
select LINEUP
seek cLeague+cSeason+cWeek+cHomeTeam
select SSHEET
replace HP  with LINEUP->PLAYER1

select LINEUP
seek cLeague+cSeason+cWeek+cAwayTeam
select SSHEET
replace AP  with LINEUP->PLAYER1

select SSHEET
append blank
replace  WEEK with cWeek
replace HTEAM with cHomeTeam
replace ATEAM with cAwayTeam

replace POS    with 2
select LINEUP
seek cLeague+cSeason+cWeek+cHomeTeam
select SSHEET
replace HP  with LINEUP->PLAYER2

select LINEUP
seek cLeague+cSeason+cWeek+cAwayTeam
select SSHEET
replace AP  with LINEUP->PLAYER2

select SSHEET
append blank
replace  WEEK with cWeek
replace HTEAM with cHomeTeam
replace ATEAM with cAwayTeam

replace POS    with 3
select LINEUP
seek cLeague+cSeason+cWeek+cHomeTeam
select SSHEET
replace HP  with LINEUP->PLAYER3

select LINEUP
seek cLeague+cSeason+cWeek+cAwayTeam
select SSHEET
replace AP  with LINEUP->PLAYER3

select SSHEET
append blank
replace  WEEK with cWeek
replace HTEAM with cHomeTeam
replace ATEAM with cAwayTeam

replace POS    with 4
select LINEUP
seek cLeague+cSeason+cWeek+cHomeTeam
select SSHEET
replace HP  with LINEUP->PLAYER4

select LINEUP
seek cLeague+cSeason+cWeek+cAwayTeam
select SSHEET
replace AP  with LINEUP->PLAYER4

select SSHEET
append blank
replace  WEEK with cWeek
replace HTEAM with cHomeTeam
replace ATEAM with cAwayTeam

replace POS    with 5
select LINEUP
seek cLeague+cSeason+cWeek+cHomeTeam
select SSHEET
replace HP  with LINEUP->PLAYER5

select LINEUP
seek cLeague+cSeason+cWeek+cAwayTeam
select SSHEET
replace AP  with LINEUP->PLAYER5

select SSHEET
append blank
replace  WEEK with cWeek
replace HTEAM with cHomeTeam
replace ATEAM with cAwayTeam

replace POS    with 6
select LINEUP
seek cLeague+cSeason+cWeek+cHomeTeam
select SSHEET
replace HP  with LINEUP->PLAYER6

select LINEUP
seek cLeague+cSeason+cWeek+cAwayTeam
select SSHEET
replace AP  with LINEUP->PLAYER6

*---Game 2
cHomeTeam = TYSKED->HT2
cAwayTeam = TYSKED->AT2

select SSHEET
append blank
replace  WEEK with cWeek
replace HTEAM with cHomeTeam
replace ATEAM with cAwayTeam

replace POS    with 1
select LINEUP
seek cLeague+cSeason+cWeek+cHomeTeam
select SSHEET
replace HP  with LINEUP->PLAYER1

select LINEUP
seek cLeague+cSeason+cWeek+cAwayTeam
select SSHEET
replace AP  with LINEUP->PLAYER1

select SSHEET
append blank
replace  WEEK with cWeek
replace HTEAM with cHomeTeam
replace ATEAM with cAwayTeam

replace POS    with 2
select LINEUP
seek cLeague+cSeason+cWeek+cHomeTeam
select SSHEET
replace HP  with LINEUP->PLAYER2

select LINEUP
seek cLeague+cSeason+cWeek+cAwayTeam
select SSHEET
replace AP  with LINEUP->PLAYER2

select SSHEET
append blank
replace  WEEK with cWeek
replace HTEAM with cHomeTeam
replace ATEAM with cAwayTeam

replace POS    with 3
select LINEUP
seek cLeague+cSeason+cWeek+cHomeTeam
select SSHEET
replace HP  with LINEUP->PLAYER3

select LINEUP
seek cLeague+cSeason+cWeek+cAwayTeam
select SSHEET
replace AP  with LINEUP->PLAYER3

select SSHEET
append blank
replace  WEEK with cWeek
replace HTEAM with cHomeTeam
replace ATEAM with cAwayTeam

replace POS    with 4
select LINEUP
seek cLeague+cSeason+cWeek+cHomeTeam
select SSHEET
replace HP  with LINEUP->PLAYER4

select LINEUP
seek cLeague+cSeason+cWeek+cAwayTeam
select SSHEET
replace AP  with LINEUP->PLAYER4

select SSHEET
append blank
replace  WEEK with cWeek
replace HTEAM with cHomeTeam
replace ATEAM with cAwayTeam

replace POS    with 5
select LINEUP
seek cLeague+cSeason+cWeek+cHomeTeam
select SSHEET
replace HP  with LINEUP->PLAYER5

select LINEUP
seek cLeague+cSeason+cWeek+cAwayTeam
select SSHEET
replace AP  with LINEUP->PLAYER5

select SSHEET
append blank
replace  WEEK with cWeek
replace HTEAM with cHomeTeam
replace ATEAM with cAwayTeam

replace POS    with 6
select LINEUP
seek cLeague+cSeason+cWeek+cHomeTeam
select SSHEET
replace HP  with LINEUP->PLAYER6

select LINEUP
seek cLeague+cSeason+cWeek+cAwayTeam
select SSHEET
replace AP  with LINEUP->PLAYER6


*---Game 3
cHomeTeam = TYSKED->HT3
cAwayTeam = TYSKED->AT3

select SSHEET
append blank
replace  WEEK with cWeek
replace HTEAM with cHomeTeam
replace ATEAM with cAwayTeam

replace POS    with 1
select LINEUP
seek cLeague+cSeason+cWeek+cHomeTeam
select SSHEET
replace HP  with LINEUP->PLAYER1

select LINEUP
seek cLeague+cSeason+cWeek+cAwayTeam
select SSHEET
replace AP  with LINEUP->PLAYER1

select SSHEET
append blank
replace  WEEK with cWeek
replace HTEAM with cHomeTeam
replace ATEAM with cAwayTeam

replace POS    with 2
select LINEUP
seek cLeague+cSeason+cWeek+cHomeTeam
select SSHEET
replace HP  with LINEUP->PLAYER2

select LINEUP
seek cLeague+cSeason+cWeek+cAwayTeam
select SSHEET
replace AP  with LINEUP->PLAYER2

select SSHEET
append blank
replace  WEEK with cWeek
replace HTEAM with cHomeTeam
replace ATEAM with cAwayTeam

replace POS    with 3
select LINEUP
seek cLeague+cSeason+cWeek+cHomeTeam
select SSHEET
replace HP  with LINEUP->PLAYER3

select LINEUP
seek cLeague+cSeason+cWeek+cAwayTeam
select SSHEET
replace AP  with LINEUP->PLAYER3

select SSHEET
append blank
replace  WEEK with cWeek
replace HTEAM with cHomeTeam
replace ATEAM with cAwayTeam

replace POS    with 4
select LINEUP
seek cLeague+cSeason+cWeek+cHomeTeam
select SSHEET
replace HP  with LINEUP->PLAYER4

select LINEUP
seek cLeague+cSeason+cWeek+cAwayTeam
select SSHEET
replace AP  with LINEUP->PLAYER4

select SSHEET
append blank
replace  WEEK with cWeek
replace HTEAM with cHomeTeam
replace ATEAM with cAwayTeam

replace POS    with 5
select LINEUP
seek cLeague+cSeason+cWeek+cHomeTeam
select SSHEET
replace HP  with LINEUP->PLAYER5

select LINEUP
seek cLeague+cSeason+cWeek+cAwayTeam
select SSHEET
replace AP  with LINEUP->PLAYER5

select SSHEET
append blank
replace  WEEK with cWeek
replace HTEAM with cHomeTeam
replace ATEAM with cAwayTeam

replace POS    with 6
select LINEUP
seek cLeague+cSeason+cWeek+cHomeTeam
select SSHEET
replace HP  with LINEUP->PLAYER6

select LINEUP
seek cLeague+cSeason+cWeek+cAwayTeam
select SSHEET
replace AP  with LINEUP->PLAYER6


*---Game 4
cHomeTeam = TYSKED->HT4
cAwayTeam = TYSKED->AT4

select SSHEET
append blank
replace  WEEK with cWeek
replace HTEAM with cHomeTeam
replace ATEAM with cAwayTeam

replace POS    with 1
select LINEUP
seek cLeague+cSeason+cWeek+cHomeTeam
select SSHEET
replace HP  with LINEUP->PLAYER1

select LINEUP
seek cLeague+cSeason+cWeek+cAwayTeam
select SSHEET
replace AP  with LINEUP->PLAYER1

select SSHEET
append blank
replace  WEEK with cWeek
replace HTEAM with cHomeTeam
replace ATEAM with cAwayTeam

replace POS    with 2
select LINEUP
seek cLeague+cSeason+cWeek+cHomeTeam
select SSHEET
replace HP  with LINEUP->PLAYER2

select LINEUP
seek cLeague+cSeason+cWeek+cAwayTeam
select SSHEET
replace AP  with LINEUP->PLAYER2

select SSHEET
append blank
replace  WEEK with cWeek
replace HTEAM with cHomeTeam
replace ATEAM with cAwayTeam

replace POS    with 3
select LINEUP
seek cLeague+cSeason+cWeek+cHomeTeam
select SSHEET
replace HP  with LINEUP->PLAYER3

select LINEUP
seek cLeague+cSeason+cWeek+cAwayTeam
select SSHEET
replace AP  with LINEUP->PLAYER3

select SSHEET
append blank
replace  WEEK with cWeek
replace HTEAM with cHomeTeam
replace ATEAM with cAwayTeam

replace POS    with 4
select LINEUP
seek cLeague+cSeason+cWeek+cHomeTeam
select SSHEET
replace HP  with LINEUP->PLAYER4

select LINEUP
seek cLeague+cSeason+cWeek+cAwayTeam
select SSHEET
replace AP  with LINEUP->PLAYER4

select SSHEET
append blank
replace  WEEK with cWeek
replace HTEAM with cHomeTeam
replace ATEAM with cAwayTeam

replace POS    with 5
select LINEUP
seek cLeague+cSeason+cWeek+cHomeTeam
select SSHEET
replace HP  with LINEUP->PLAYER5

select LINEUP
seek cLeague+cSeason+cWeek+cAwayTeam
select SSHEET
replace AP  with LINEUP->PLAYER5

select SSHEET
append blank
replace  WEEK with cWeek
replace HTEAM with cHomeTeam
replace ATEAM with cAwayTeam

replace POS    with 6
select LINEUP
seek cLeague+cSeason+cWeek+cHomeTeam
select SSHEET
replace HP  with LINEUP->PLAYER6

select LINEUP
seek cLeague+cSeason+cWeek+cAwayTeam
select SSHEET
replace AP  with LINEUP->PLAYER6


*---Game 5
cHomeTeam = TYSKED->HT5
cAwayTeam = TYSKED->AT5

select SSHEET
append blank
replace  WEEK with cWeek
replace HTEAM with cHomeTeam
replace ATEAM with cAwayTeam

replace POS    with 1
select LINEUP
seek cLeague+cSeason+cWeek+cHomeTeam
select SSHEET
replace HP  with LINEUP->PLAYER1

select LINEUP
seek cLeague+cSeason+cWeek+cAwayTeam
select SSHEET
replace AP  with LINEUP->PLAYER1

select SSHEET
append blank
replace  WEEK with cWeek
replace HTEAM with cHomeTeam
replace ATEAM with cAwayTeam

replace POS    with 2
select LINEUP
seek cLeague+cSeason+cWeek+cHomeTeam
select SSHEET
replace HP  with LINEUP->PLAYER2

select LINEUP
seek cLeague+cSeason+cWeek+cAwayTeam
select SSHEET
replace AP  with LINEUP->PLAYER2

select SSHEET
append blank
replace  WEEK with cWeek
replace HTEAM with cHomeTeam
replace ATEAM with cAwayTeam

replace POS    with 3
select LINEUP
seek cLeague+cSeason+cWeek+cHomeTeam
select SSHEET
replace HP  with LINEUP->PLAYER3

select LINEUP
seek cLeague+cSeason+cWeek+cAwayTeam
select SSHEET
replace AP  with LINEUP->PLAYER3

select SSHEET
append blank
replace  WEEK with cWeek
replace HTEAM with cHomeTeam
replace ATEAM with cAwayTeam

replace POS    with 4
select LINEUP
seek cLeague+cSeason+cWeek+cHomeTeam
select SSHEET
replace HP  with LINEUP->PLAYER4

select LINEUP
seek cLeague+cSeason+cWeek+cAwayTeam
select SSHEET
replace AP  with LINEUP->PLAYER4

select SSHEET
append blank
replace  WEEK with cWeek
replace HTEAM with cHomeTeam
replace ATEAM with cAwayTeam

replace POS    with 5
select LINEUP
seek cLeague+cSeason+cWeek+cHomeTeam
select SSHEET
replace HP  with LINEUP->PLAYER5

select LINEUP
seek cLeague+cSeason+cWeek+cAwayTeam
select SSHEET
replace AP  with LINEUP->PLAYER5

select SSHEET
append blank
replace  WEEK with cWeek
replace HTEAM with cHomeTeam
replace ATEAM with cAwayTeam

replace POS    with 6
select LINEUP
seek cLeague+cSeason+cWeek+cHomeTeam
select SSHEET
replace HP  with LINEUP->PLAYER6

select LINEUP
seek cLeague+cSeason+cWeek+cAwayTeam
select SSHEET
replace AP  with LINEUP->PLAYER6


*---Game 6
cHomeTeam = TYSKED->HT6
cAwayTeam = TYSKED->AT6

select SSHEET
append blank
replace  WEEK with cWeek
replace HTEAM with cHomeTeam
replace ATEAM with cAwayTeam

replace POS    with 1
select LINEUP
seek cLeague+cSeason+cWeek+cHomeTeam
select SSHEET
replace HP  with LINEUP->PLAYER1

select LINEUP
seek cLeague+cSeason+cWeek+cAwayTeam
select SSHEET
replace AP  with LINEUP->PLAYER1

select SSHEET
append blank
replace  WEEK with cWeek
replace HTEAM with cHomeTeam
replace ATEAM with cAwayTeam

replace POS    with 2
select LINEUP
seek cLeague+cSeason+cWeek+cHomeTeam
select SSHEET
replace HP  with LINEUP->PLAYER2

select LINEUP
seek cLeague+cSeason+cWeek+cAwayTeam
select SSHEET
replace AP  with LINEUP->PLAYER2

select SSHEET
append blank
replace  WEEK with cWeek
replace HTEAM with cHomeTeam
replace ATEAM with cAwayTeam

replace POS    with 3
select LINEUP
seek cLeague+cSeason+cWeek+cHomeTeam
select SSHEET
replace HP  with LINEUP->PLAYER3

select LINEUP
seek cLeague+cSeason+cWeek+cAwayTeam
select SSHEET
replace AP  with LINEUP->PLAYER3

select SSHEET
append blank
replace  WEEK with cWeek
replace HTEAM with cHomeTeam
replace ATEAM with cAwayTeam

replace POS    with 4
select LINEUP
seek cLeague+cSeason+cWeek+cHomeTeam
select SSHEET
replace HP  with LINEUP->PLAYER4

select LINEUP
seek cLeague+cSeason+cWeek+cAwayTeam
select SSHEET
replace AP  with LINEUP->PLAYER4

select SSHEET
append blank
replace  WEEK with cWeek
replace HTEAM with cHomeTeam
replace ATEAM with cAwayTeam

replace POS    with 5
select LINEUP
seek cLeague+cSeason+cWeek+cHomeTeam
select SSHEET
replace HP  with LINEUP->PLAYER5

select LINEUP
seek cLeague+cSeason+cWeek+cAwayTeam
select SSHEET
replace AP  with LINEUP->PLAYER5

select SSHEET
append blank
replace  WEEK with cWeek
replace HTEAM with cHomeTeam
replace ATEAM with cAwayTeam

replace POS    with 6
select LINEUP
seek cLeague+cSeason+cWeek+cHomeTeam
select SSHEET
replace HP  with LINEUP->PLAYER6

select LINEUP
seek cLeague+cSeason+cWeek+cAwayTeam
select SSHEET
replace AP  with LINEUP->PLAYER6












