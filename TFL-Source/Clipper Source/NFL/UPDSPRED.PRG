***
***  Updates the Spread database with results
***
#INCLUDE "nfl.h"

PROCEDURE UpdSpred

   local nSpread, cHomeAway, nOldOrd, nVariance, nTotSpread

   nTotSpread := 0

   select 0
   use SPREAD index SPREADX1 exclusive
   zap

	select SCHED
	nOldOrd := indexord()

	go top
	do while .not. SCHED->( eof() )
      if SCHED->SPREAD <> 0 .and. SCHED->SPREAD <> .5
         nTotSpread++
         nSpread := SCHED->SPREAD

         *---Control the range of spreads
         nSpread := iif( nSpread >  14,  14.9, nSpread )
         nSpread := iif( nSpread < -14, -14.9, nSpread )

         cHomeAway := iif( SCHED->SPREAD > 0, "H", "A" )

         select SPREAD
         seek cHomeAway + str( nSpread, 5, 1 )
         if eof()
            append blank
            replace SPREAD with nSpread
         endif

         replace HOMEAWAY with cHomeAway
         if SCHED->SPREAD > 0
            *---Home favourite
            if ( SCHED->HOMESCORE - SCHED->SPREAD ) > SCHED->AWAYSCORE
               *---Cover
               replace COVERS with COVERS + 1
            else
               if ( SCHED->HOMESCORE - SCHED->SPREAD ) < SCHED->AWAYSCORE
                 *---Fail to Cover
                 replace FAILS with  FAILS + 1
               endif
            endif
         else
            *---Away favourite
            if ( SCHED->AWAYSCORE + SCHED->SPREAD ) > SCHED->HOMESCORE
               *---Cover
               replace COVERS with COVERS + 1
            else
               if ( SCHED->AWAYSCORE + SCHED->SPREAD ) < SCHED->HOMESCORE
                  *---Fail to Cover
                  replace FAILS with  FAILS + 1
               endif
            endif
         endif

      endif


		SCHED->( dbskip() )
	enddo

   *---Process for totals
   nVariance      := 40
   nSamplePercent := .01

   select SPREAD
   go top
   do while .not. eof()
      replace total with (FAILS+COVERS)
      replace RATE with Percent( COVERS, TOTAL )
      if total > ( nSamplePercent * nTotSpread )
         if RATE >= ( 100 - nVariance ) .or. RATE <= nVariance
            replace BAD with .t.
         else
            replace BAD with .f.
         endif
      else
         replace BAD with .f.
      endif

		SPREAD->( dbskip() )
   enddo

   select SCHED
	set order to nOldOrd
   
	ErrorMsg( "Spreads updated!" + str( nTotSpread ) )

   @ 23, 00

RETURN




*EOF: LOADPICK.PRG