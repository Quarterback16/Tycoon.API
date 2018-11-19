FUNCTION TFLteam
   parameters cOwnerID
   select COMP
   locate for OWNERID = cOwnerID .and. SEASON="1998" .and. LEAGUEID="OL"
   select SSHEET
RETURN trim( COMP->FRANCHISE )

