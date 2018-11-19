FUNCTION Plyr
   parameters cID
   select PLAYER
   locate for PLAYERID = cID
   select SSHEET
RETURN left( trim( PLAYER->SURNAME ) + ", " + PLAYER->FIRSTNAME +space(23), 23 ) + PLAYER->CURRTEAM

