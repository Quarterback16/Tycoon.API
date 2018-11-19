***
***   Prints the Gordon Plays for the Week
***
***   Output to GordonLine.txt
***
***  Excute like thus
***
***     DO gordonline WITH "01", "2005"
***
parameters cWeek, cSeason

LOCAL oGordon

SET DATE BRITISH
SET DEFAULT TO e:\tfl\dev

*--Classes
set procedure to NFLLib
set procedure to Gordon additive
set procedure to Play additive
set procedure to Result additive
set procedure to NFLDataController additive

SET ALTERNATE off
SET ALTERNATE TO 
ERASE GordonLine.txt
SET ALTERNATE TO GordonLine.txt
SET ALTERNATE on

*debug
clear

_screen.fontname = 'Foxfont'
_screen.fontsize = 10


*---Create Gordon object
oGordon = CREATEOBJECT("Gordon")


*---Where are we
oGordon.SetSeason(cSeason)
oGordon.SetWeek(cWeek)

*---Tell us
oGordon.Report()


SET ALTERNATE off
SET ALTERNATE TO
close database

