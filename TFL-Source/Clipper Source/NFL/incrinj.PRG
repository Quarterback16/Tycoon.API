***
***  Pre-season tasks
***
select 1
use PLAYER
GO top
scan
	if ROLE = "I"
	  replace INJURY with INJURY + 1
	  replace ROLE with " "
	endif
ENDSCAN
USE
