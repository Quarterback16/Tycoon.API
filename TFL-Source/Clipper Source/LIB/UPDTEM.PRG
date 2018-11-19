* ashley 

*#INCLUDE L:FI.H

FUNCTION UP_DEL

   local deletedok
   deletedok = .f.

   if !up_can()
      RETU( .f. )  && no point in deleting a deleted or eof
   endif

   if yes_no( 'Are you sure you wish to delete this record ?' )
      if rec_lock(5)
         delete
         deletedok = .t.
         skip   && to reset the pointer
         if eof() 
            skip -1
         endif
         error_msg( 'Record deleted',.F.)
      else
         warning('rec_lock')
      endif
   endif
   unlock
   commit                        

return( deletedok )


FUNCTION UP_ADD

   parameters fld, row,col, vpict, vreadvalid, dkey, replaceit

   if if( pcount() < 4, .t., empty( vpict ) )
      vpict = if( type( fld ) = "C", "@K!", "" )
   endif
   if pcount() < 5 
      vreadvalid = ""
   endif                                          
   replaceit = if( pcount() < 7, .t., replaceit )

   priv crec

   crec = recno()       
   go lastrec()+1
   dkey = &fld 
   do while .t.    && read loop
   
      vget( row,col, "dkey", vpict, .t., vreadvalid )
      vread()
      if abort() 
         go crec
         return( .f. )
*<-------
      endif
   
      seek dkey
      if found()   
         error_msg( 'Record already exists')
      elseif !add_rec(5)   
         warning( "add_rec" )
      else
         exit                 
   *<----successful read + add
      endif
   
   enddo

   if replaceit
      replace &fld with dkey   
   endif

return( .t. )


FUNCTION UP_ASKADD

   error_msg( "No records found" )
   return( yes_no( 'Do you want to add a record ?' ) )


FUNCTION UP_CANEDIT

   if eof() .or. deleted()   && do not edit these types of records
      error_msg( 'Please select a record to edit')
      return( .f. )
   endif  

return( .t. )    


FUNCTION UP_CAN
return( !eof() .and. !deleted() )


FUNCTION UP_FIND

   parameter fld, row,col, vpict, seekfunc

   if if( pcount() < 4, .t., empty( vpict ) )
      vpict = if( type( fld ) = "C", "@K!", "" )
   endif                         
   if pcount() < 5
      seekfunc = ""        
   else
      seekfunc = seekfunc + "(getdkey)"
   endif

   priv getdkey                

   if empty_dbf()
      error_msg( 'No records found to view')
      return( .f. )
   endif  
   
   crec = recno()
   go lastrec()+1
   getdkey = &fld

   do while .t.

      setcolor( c_fields )
      vget( row,col, "getdkey", vpict, .t. )
      vread()

      if abort() 
         go crec
         return( .f. )
      endif

      if empty( seekfunc )
         getdkey = if( type("getdkey") = 'C', trim(getdkey), getdkey )
         dbseek( getdkey )
      else
         dbseek( &seekfunc )
      endif

      if found()
         return( .t. )
      else
         error_msg('Record could not be found')
         go crec
      endif

   enddo


FUNCTION UP_SKIP

   parameter vector

   skip vector
   if bof() .or. eof()
      error_msg( "No more records to view." )
      if eof()
         go bottom     && thses two gotos correctly set eof and bof when the
      elseif bof()     && file is really empty, ie lastrec() = 0, skips do not
         go top
      endif
   endif

return( 0 )


FUNCTION UP_LIST

   parameters heading, linevars

   private old_rec

   if empty_dbf()
      error_msg( 'There is no data to print' )
      return( 0 )
   endif

   old_rec = recno()
   go top

   private pagelen, footerlen, line, pageno, sp, pitch, abort_prt
   private recds, i

   rtitle     = heading + ' TABLE LISTING'
   footerlen  = 6
   pagelen    = 66 - footerlen
   line       = 254
   pageno     = 0
   sp         = 2
   pitch      = 10
   abort_prt  = .f.
   *---Counts
   recds      = 0

   set margin to    &&   pitch
   set print on
   set console off

   if gsys_test
      set printer to ( "PRINTER.OUT" )
      testmsg( 'Output redirected to PRINTER.OUT' )
   endif

   ? p_init
   do case
      case pitch = 10
         ?? p_cpi10
      case pitch = 12
         ?? p_cpi12
      case pitch = 16
         ?? p_cpi16
   endcase

   *---Process file
   dispmsg( 23, 'Hit ESC to stop printing', .f., -1, .f.)

   do while .not. eof() .and. .not. end_print()
      
      @ 24,75 say str( recno(), 4) 

      if line >= pagelen
         if line <> 254
            eject
         endif
         *---Do Heading
         do STD_HEAD with rtitle, pageno, pitch
         line = 10
      endif

      *----PrintLine
      for i = 1 to sp
         ?
         line = line + 1
      next
      ?? &linevars

      *---Incr Counters
      recds = recds + 1

      skip
   enddo
   *---Summary
   ?
   ?  'Total Records = ' + str( recds,10 ) + '     ' 
   line = line + 2
   
   for i = 1 to pagelen - line
      ?
   next
   * do STD_FOOT with procname(), pitch

   set margin to
   set print off
   set console on
 
   go old_rec     
   setcolor( c_blank )
   @23,0

   return( 0 )
