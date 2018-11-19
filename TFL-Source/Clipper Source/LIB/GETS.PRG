* GETS.PRG 
* ASHLEY
* REPLACE CLIPPER "@ GET" 

*   Improvement on clipper's @ get, read
* - remaining valid clauses not bypassed by the pgup or pgdn keys.
* - up arrow key bypasses the current valid clause
* - a message line for the current get/prompt
* - prompts mixed with gets
* - gets/prompts executed condtionally on input of previuos gets

* vget         - get a variable
* dget         - display a prompt and call a function for the action
* cvget        - if a condition is satisfied get a variable
* cdget        - if a condition is satisfied display a prompt and call a function for the action
* vread        - execute the gets + prompts specified 
* vread_key    - is a key defined by vread
* vread_init   - initialise the global variables required by these functions 
* vread_mess   - current get/prompt message parameters
* vread_test   - test the macros used for the get functions

* These functions are used in the same way as clipper's @ get, read format
* but include several enhancements. 

* Note - optional parameters, if included, these must be of the correct type

*  vget( row, col, varname, pict_str, not_empty, valid_clause, message )
*  cvget( condition, row, col, varname, pict_str, not_empty, valid_clause, message )
*  dget( row, col, varname, pict_str, not_empty, update_fnc, message )
*  cdget( condition, row, col, varname, pict_str, not_empty, update_fnc, message )
*                  
*  The mandatory parameters are condition, row, col, varname.
*  The remaining parameters are optional.
*
*     - Get/prompt a variable ( usage is the same as @row,col get.... )  
*                                    
*     condition    - exp C - string must contain a exp L macro
*     row, col     - exp N - screen row and column for the get
*     varname      - exp C - the name of the variable to display/get/prompt ( only stored by vget(), not used until vread() )
*     pict_str     - exp C - the picture string
*     not_empty    - exp L - replaces: @x,y get mvar valid !empty( mvar )
*                            should be .f. if using popup tables
*     valid_clause - exp C - the valid_clause, must contain a exp L macro or be left empty for no validation clause 
*     update_fnc   - exp C - must contain the name of a function complete with the brackets eg. "xfunc()"
*     message      - exp C - a message, the string may contain the chars of the message or a macro,
*                            if the 1st char of the string is "&" then remains of the string are used as a macro,
*                            else the string is just written straight to the screen
*     clear_len    - exp N - the len to clear before getting a variable, default is no clear

*      
*       - DISPLAY prompt amongst gets

*
*  vread( start, disp_mess, valid_fnc, update_fnc, upvalid )
*
*        start     - N -the get to start on, can pass by ref to pickup the last 
*                       get from which the read was exited
*        disp_mess - L -display messages flag
*        valid_fnc - C -the name of the valid function to be run for each get
*        update_fnc- C -the name of a function to run when a dget is selected,
*                       if no function was specifed in the dget
*        upvalid   - L -weather or not to run validations on exit from a get with
*                       the up arrow, defaults to global vread_upvalid

* 
*  read_init( max_prompts )
* 
*  vget - var get, like a normal get
* 
*  dget - var not got, prompted and update fnc called
*       - if update_fnc empty prompted and update fnc in read() called with varname
*       - call if update_fnc specified in dget is as is
*       - call if update_fnc specified in read is
*                 update_fnc( varname, @var, userchosen )

*  cdget( condition, row, col, varname, pict_str, not_empty, update_func, message )
*       condition on which to use this prompt
* 
*  messages beginning with & are macroed

*  publics available for programmers use
*     vget_exit the key that exited the last get
*     vread_exit set to .t. within a valid will cause vread to terminate
*     vread_redraw, .t. before valid, draw variable after valid flag
*     vread_dget, .t. if the dget var was gotten by vread
*     vread_cursor, state cursor of cursor after a vread
*     vread_upvalid, run validation clause on up arrow, default .t. for run valid

* compile with clipper's -v option to avoid field name clashes

* testing code

* clear
* set scoreboard off
* set console off
* vread_init( 20 )
* vread_mess( 24,0, 80, "n/bg" )
* v1 = "hi there"
* v2 = "bob       "
* v3 = 0
* v4 = 0
* v5 = space( 10 )
* v6 = 5
* vget( 1,0, "v1", "@!", .t., "", "Greetings." )
* vget( 2,0, "v2", "", .f., "", "Name." )
* vget( 3,0, "v3", "999", .f., "v3 > 0 .and. v3 < 200", "Must be between 3 and 200." )
* dget( 5,0, "v5", "", .f., "v5fnc()", "&'alfred or sloth is '+str(v3,3)+' years old'" )
* vget( 7,0, "v4", "9999", .f., "v4 > v3 .and. v4 < 4000", "&'Must be between '+str(v3,3)+' and 4000.'" )
* cvget( "v5='alfred'", 8,0, "v6", "", .t., "", "Alfred's beers." )
* set color to n/w, n/bg
* set print to vreader1.txt
* if !vread_test()
*    vread( 1, .t. )
* endif               
* 
* * errors in macros
* vget( 1,"0", "vr", 6, .t., "", "Greetings." )
* vget( 2,0, "v2", "", .f., "", "Name." )
* vget( 3,0, "v3", "999", .f., "v3 > 0 .and. v3 < 200", "Must be between 3 and 200." )
* dget( 5,0, "v5", "", .f., "v5fnc", "&'alfred or sloth is '+str(v3,3)+' years old'" )
* vget( 7,0, "v4", "9999", .f., "v4 > v3 .and. v4 < 4000", "&'Must be between '+str(v3,3)+ and 4000.'" )
* cvget( "v5='alfred'", 8,0, "v6", "", .t., "", "Alfred's beers." )
* set color to n/w, n/bg
* set print to vreader2.txt
* if !vread_test()
*    vread( 1, .t. )
* endif
* 
* set color to
* set cursor on
* quit
* 
* 
* **************
* FUNCTION V5FNC
* 
* priv choice
* if vread_force
*    return( if( empty( v5 ), 1, 0 ) )
* endif
* do while .t.
*    if empty( v5 ) .or. lastkey() = -1
*       save screen to scr
*       set color to w/n, n/bg
*       @11,11 prompt " Alfred "        
*       @12,11 prompt " Sloth  "        
*       menu to choice
*       rest screen from scr           
*       if choice = 1                  
*          v5 = "alfred"               
*       elseif choice = 2              
*          v5 = "sloth"   
*       endif                  
*       v5 = setlen( v5, 10 )
*    endif
*    vread_say()
*    if lastkey() = 27
*       exit
*    endif
*    if vread_key( inkey( 0 ) )
*       exit
*    endif
* enddo
* return( lastkey() )
* 

*******************
FUNCTION VREAD_INIT

   parameter maxprompts

   if maxprompts > 0
      public gget_row[maxprompts], gget_col[maxprompts], gget_var[maxprompts], ;
             gget_pict[maxprompts], gget_empty[maxprompts], gget_valid[maxprompts],  ;
             gget_mess[maxprompts], gget_ptype[maxprompts], gget_cnt, gget_mrow, gget_mcol, ;
             gget_mlen, gget_mclr, gget_rdcnt, gget_rd1st[20], gget_cond[maxprompts], ;
             gget_exkey, vget_exit, vread_exit, gget_clear[maxprompts], gget_emerr, ;
             vread_cursor, vread_upvalid
      gget_cnt = 0
      gget_rdcnt = 0
      gget_rd1st[1] = 1
      store 0 to gget_mrow , gget_mcol, gget_mlen
      gget_mclr = ""                                       
      vread_exit = 0
      gget_emerr = ""
      vread_cursor = .t.                
      vread_upvalid = .t. 
   else
      release gget_row, gget_col, gget_var, gget_pict, gget_empty, gget_valid, ;
              gget_mess, gget_ptype, gget_cnt, gget_mrow, gget_mcol, gget_mclr, ;
              gget_rdcnt, gget_cond, gget_exkey, vread_exit, gget_clear, gget_emerr, ;
              vread_cursor, vread_upvalid
   endif

return( 0 )


*************
FUNCTION VGET

	parameters row, col, var, pict_str, not_empty, valid_clause, message, clrlen

	gget_cnt = gget_cnt + 1
	gget_row[gget_cnt] = row
	gget_col[gget_cnt] = col
	gget_var[gget_cnt] = var
	gget_pict[gget_cnt] = if( pcount() >= 4, pict_str, "" )
	gget_empty[gget_cnt] = if( pcount() >= 5, not_empty, .f. )
	if pcount() >= 6
		gget_valid[gget_cnt] = if( !empty( valid_clause ), valid_clause, ".t." )
	else
		gget_valid[gget_cnt] = ".t."
	endif
	gget_mess[gget_cnt] = if( pcount() >= 7, message, "" )
	gget_clear[gget_cnt] = if( pcount() >= 8, clrlen, 0 )
	gget_ptype[gget_cnt] = "V"

return( 0 )


**************
FUNCTION CVGET

parameters condition, row, col, var, pict_str, not_empty, valid_clause, message;
         , clrlen


gget_cnt = gget_cnt + 1
gget_cond[gget_cnt] = condition
gget_row[gget_cnt] = row
gget_col[gget_cnt] = col
gget_var[gget_cnt] = var
gget_pict[gget_cnt] = if( pcount() >= 5, pict_str, "" )
gget_empty[gget_cnt] = if( pcount() >= 6, not_empty, .f. )
if pcount() >= 7
   gget_valid[gget_cnt] = if( !empty( valid_clause ), valid_clause, ".t." )
else
   gget_valid[gget_cnt] = ".t."
endif
gget_mess[gget_cnt] = if( pcount() >= 8, message, "" )
gget_clear[gget_cnt] = if( pcount() >= 9, clrlen, 0 )
gget_ptype[gget_cnt] = "CV"

return( 0 )


*************
FUNCTION DGET

parameters row, col, var, pict_str, update_fnc, message, clrlen

gget_cnt = gget_cnt + 1
gget_row[gget_cnt] = row
gget_col[gget_cnt] = col
gget_var[gget_cnt] = var
gget_pict[gget_cnt] = if( pcount() >= 4, pict_str, "" )
gget_empty[gget_cnt] = .f. 
gget_valid[gget_cnt] = if( pcount() >= 5, update_fnc, "" )
gget_mess[gget_cnt] = if( pcount() = 6, message, "" )
gget_clear[gget_cnt] = if( pcount() >= 7, clrlen, 0 )
gget_ptype[gget_cnt] = "D"

return( 0 )


**************
FUNCTION CDGET

parameters condition, row, col, var, pict_str, update_fnc, message, clrlen

gget_cnt = gget_cnt + 1                              
gget_cond[gget_cnt] = condition
gget_row[gget_cnt] = row
gget_col[gget_cnt] = col
gget_var[gget_cnt] = var
gget_pict[gget_cnt] = if( pcount() >= 5, pict_str, "" )
gget_empty[gget_cnt] = .f.
gget_valid[gget_cnt] = if( pcount() >= 6, update_fnc, "" )
gget_mess[gget_cnt] = if( pcount() = 7, message, "" )
gget_clear[gget_cnt] = if( pcount() >= 8, clrlen, 0 )
gget_ptype[gget_cnt] = "CD"

return( 0 )


*******************
FUNCTION VREAD_MESS

   parameters row, col, len, color

   gget_mrow = row
   gget_mcol = col
   gget_mlen = len
   gget_mclr = color

return( 0 )



**************
FUNCTION VREAD

parameters _start, _disp_mess, _valid_fnc, _update_fnc, _upvalid

priv _i, _j, _lk, _v, _vld, _item, _moveditem, _fclr, _bclr, _eclr, _topitem, _invalid, ;
     vread_force, _c, _clearclr, _saveclr, vread_redraw, vread_dget, _t, _force_start, ;
     _forced

if pcount() < 1
   _start = 1
endif
if pcount() < 2
   _disp_mess = .f.
endif
if pcount() < 3
   _valid_fnc = ""
endif
if pcount() < 4
   _update_fnc = ""
endif
if pcount() < 5
   _upvalid = vread_upvalid
endif

_saveclr = setcolor()
_fclr = getcolor( 1 )
_bclr = getcolor( 2 )
_eclr = getcolor( 5 )
if !vread_cursor
   set cursor off
endif
gget_rdcnt = gget_rdcnt + 1
gget_rd1st[gget_rdcnt+1] = gget_cnt + 1
_topitem = gget_rd1st[gget_rdcnt]

* save the background color of undisplayed prompts
if ascan( gget_ptype, "C", _topitem, gget_cnt ) > 0 
   _i = ascan( gget_ptype, "C", _topitem, gget_cnt )
   *_clearclr = savescreen( gget_row[_i], gget_col[_i], gget_row[_i]+1, gget_col[_i]+1 )
   _clearclr = restofstr( _fclr, at( "/", _fclr )+1 ) && background of _fclr
   _clearclr = _clearclr +"/"+ _clearclr
endif

* display the prompts
setcolor( _eclr )
for _i = 1 to gget_cnt
   if "C" $ gget_ptype[_i] 
      _c = gget_cond[_i]
      if !( &_c )
         loop
      endif
   endif
   _v = gget_var[_i]
   if gget_clear[_i] > 0                      
      setcolor( _fclr )
      @gget_row[_i], gget_col[_i] say space( gget_clear[_i] )
      setcolor( _eclr )               
      if type( _v ) = "C"
         @gget_row[_i], gget_col[_i] say substr( &_v, 1, gget_clear[_i] ) pict gget_pict[_i]
      else
         @gget_row[_i], gget_col[_i] say &_v pict gget_pict[_i]
      endif
   else
      @ gget_row[_i], gget_col[_i] say &_v pict gget_pict[_i]
   endif
next

_item = _topitem + _start -1
do while _item > _topitem .and. "C" $ gget_ptype[_item]
   _item = _item - 1
enddo

_lk = 0
vread_force = .f.
_forced = .f.
vread_exit = .f.
vread_redraw = .t.

_moveditem = .t.  && just entering an item for the 1st time
do while .t.
   if vread_exit
      exit
   endif

   * display the prompts now with satisfied conditions
   if ascan( gget_ptype, "C", _item, gget_cnt ) > 0
      for _i = _item to gget_cnt  
         if "C" $ gget_ptype[_i]
            _c = gget_cond[_i]
            _v = gget_var[_i]
            if &_c
               setcolor( _eclr )
               if gget_clear[_i] > 0 
                  if type( _v ) = "C"
                     @gget_row[_i], gget_col[_i] say substr( &_v, 1, gget_clear[_i] ) pict gget_pict[_i]
                  else
                     @gget_row[_i], gget_col[_i] say &_v pict gget_pict[_i]
                  endif
               else
                  @gget_row[_i], gget_col[_i] say &_v pict gget_pict[_i]
               endif
            else
               setcolor( _clearclr )
               if gget_clear[_i] > 0 
                  @gget_row[_i], gget_col[_i] say space( gget_clear[_i] )
               else
                  @gget_row[_i], gget_col[_i] say &_v pict gget_pict[_i]
               endif
            endif
         endif
      next
   endif

   *---test to include a conditional prompt and move to next prompt
   if "C" $ gget_ptype[_item]
      if _lk != 5
         for _i = _item to gget_cnt
            _c = if( "C" $ gget_ptype[_i], gget_cond[_i], ".t." )
            if &_c 
               exit
            endif
         next
      else
         for _i = _item to 1 step -1 
            _c = if( "C" $ gget_ptype[_i], gget_cond[_i], ".t." )
            if &_c
               exit
            endif
         next
      endif  
      if _i > gget_cnt 
         exit
      elseif _i >= 1 
         _item = _i
      endif
   endif

   if _disp_mess .and. _moveditem
      setcolor( gget_mclr )
      @ gget_mrow, gget_mcol clear to gget_mrow, gget_mcol+gget_mlen-1
      if substr( gget_mess[_item], 1, 1 ) == "&"
         _i = substr( gget_mess[_item], 2, len( gget_mess[_item] ) - 1 )
         @ gget_mrow, gget_mcol say &_i
      else
         @ gget_mrow, gget_mcol say gget_mess[_item]
      endif
   endif

   *---clear the area in of the get
   setcolor( _fclr )
   @ gget_row[_item], gget_col[_item] say space( gget_clear[_item] )

   _moveditem = .f.
   _v = gget_var[_item]
   if "V" $ gget_ptype[_item]
      * call the read function if no 
      _vld = if( !empty( _valid_fnc ) .and. (gget_valid[_item]==".t."), ;
                 _valid_fnc + "( gget_var[_item], gget_pict[_item] )", gget_valid[_item] )
      do while .t.         

         if !_forced
            do while .t.
               setcolor( _saveclr )
               @ gget_row[_item], gget_col[_item] get &_v pict gget_pict[_item]
               readexit( .t. )
               gget_exkey = 0 && exit key pressed flag
               set cursor on
               read     
               if !vread_cursor
                  set cursor off
               endif
               readexit( .f. )                                                
               _lk = lastkey()
               if !( _item = 1 .and. _lk = 5 ) && up
                  exit
               endif
            enddo
            vget_exit = _lk  && vreads lastkey
            if _lk = 13
               if gget_exkey != 0
                  vget_exit = gget_exkey 
               endif
            endif
            if _lk = 27  && escape
               exit
            endif
         endif
         _moveditem = .t.
         setcolor( _eclr )
         if gget_clear[_item] > 0 
            if type( _v ) = "C"
               @gget_row[_item], gget_col[_item] say substr( &_v, 1, gget_clear[_item] ) pict gget_pict[_item]
            else
               @gget_row[_item], gget_col[_item] say &_v pict gget_pict[_item]
            endif
         else
            @gget_row[_item], gget_col[_item] say &_v pict gget_pict[_item]
         endif
         vread_redraw = .t.
         setcolor ( _saveclr )
         @gget_row[_item], gget_col[_item] say ""

         * validate
         if !( _lk = 5 .and. !_upvalid ) && lastkey = 5 no valid when _upvalid = .t.
            if if( gget_empty[_item], empty( &_v ), .f. )
               if _disp_mess
                  setcolor( gget_mclr )
                  @gget_mrow, gget_mcol say "Must not be empty !"
                  inkey( 5 )
               else
                  if empty( gget_emerr )
                     error_msg( "This field must not be empty !" )
                  elseif type( "gget_emerr" ) = "C"
                     _t = &gget_emerr
                  endif
               endif
               _forced = .f.
               if _lk != 5  && not empty check
                  loop
               endif
            elseif !( &_vld )   
               _forced = .f.
               if _lk != 5   && valid clause, don't loop for up key
                  loop
               endif
            endif
         endif
         exit
      enddo

   elseif "D" $ gget_ptype[_item] 
      vread_dget = .f.
      do while .t.             
         setcolor( _bclr )
         @gget_row[_item], gget_col[_item] say &_v pict gget_pict[_item]
         setcolor( _saveclr )
         @gget_row[_item], gget_col[_item] say ""
         if !empty( gget_valid[_item] )
            _vld = gget_valid[_item]
         else
            _vld = if( !empty( _update_fnc ), _update_fnc, _valid_fnc ) + "( gget_var[_item], gget_pict[_item] )"
         endif
         vread_redraw = .t.
         _i = &_vld  && returns 0-get var, else lastkey = i
         if _i = 0        
            do while .t.
               setcolor( _saveclr )
               @gget_row[_item], gget_col[_item] get &_v pict gget_pict[_item]
               readexit( .t. )
               set cursor on
               gget_exkey = 0
               read
               if !vread_cursor
                  set cursor off
               endif
               readexit( .f. )
               vread_dget = .t.
               setcolor( _eclr )
               @gget_row[_item], gget_col[_item] say &_v pict gget_pict[_item]
               _lk = lastkey()
               if !( _item = 1 .and. _lk = 5 ) && up
                  exit
               endif
            enddo
            vget_exit = _lk  && vreads lastkey
            if _lk = 13
               if gget_exkey != 0
                  vget_exit = gget_exkey 
               endif
            endif
            if _lk = 27  && escape
               exit
            endif
         else
            _lk = _i
            vget_exit = _lk  && vreads lastkey
            exit
         endif  
      enddo

   endif

   if vread_redraw 
      setcolor( _eclr )
      if gget_clear[_item] > 0 
         if type( _v ) = "C"
            @gget_row[_item], gget_col[_item] say substr( &_v, 1, gget_clear[_item] ) pict gget_pict[_item]
         else
            @gget_row[_item], gget_col[_item] say &_v pict gget_pict[_item]
         endif
      else
         @gget_row[_item], gget_col[_item] say &_v pict gget_pict[_item]
      endif
   endif

   if vread_exit
      exit
   endif

   if _lk = 5  && up
      if _item > _topitem
         _item = _item - 1
      endif

   elseif _lk = 3 .or. _lk = 18 .or. _lk = 23 && pgdn, pgup, ^W
      * validate all remaining gets
      if !_forced
         _force_start = _item
      endif
      _forced = .t.
      if _item < gget_cnt
         _item = _item + 1
      else
         exit
      endif
      
   elseif _lk = 27  && escape
      exit

   else                        &&  _lk = 24 .or. _lk = 13  down, enter
      if _item < gget_cnt
         _item = _item + 1
      elseif _lk = 13
         exit
      endif

   endif

enddo

if _disp_mess
   setcolor( gget_mclr )
   @gget_mrow, gget_mcol clear to gget_mrow, gget_mcol+gget_mlen-1
endif
setcolor( _saveclr )
gget_rdcnt = gget_rdcnt - 1
gget_cnt = 0              
_start = if( _forced, _force_start, _item )

return( 0 )


*********************
FUNCTION VREAD_SETKEY

   parameters set_clear, keyno, udfname

   * on keypress within get, the get is exited, and the procedure or function is called
   * lastkey() will then return the key set
   * the udf is then able to return the next key for vread

return( 0 )


******************
FUNCTION VGET_EXIT

   parameters set_clear, keyno

   * allows keys to be set to exit gets within vread()
   * lastkey() will then return the key set
   if set_clear
		set key keyno to vreadexprc  
   else
      set key keyno to
   endif

return( 0 )


********************
PROCEDURE VREADEXPRC

	testmsg( 'F2 pressed' )
   gget_exkey = lastkey()
   keyboard chr( 13 )

return


********************
FUNCTION VREAD_ISKEY

parameter lk                         
* is key deifned by vread ?
return( lk = 5 .or. lk = 24 .or. lk = 27 .or. lk = 13 .or. lk = 18 .or. lk = 3 )  
* up, down, escape, enter, pgup, pgdn

******************
FUNCTION VREAD_SAY
priv v
setcolor( _bclr )
v = gget_var[_item]
@gget_row[_item], gget_col[_item] say &v pict gget_pict[_item]
return( 0 )


*******************
FUNCTION VREAD_TEST

* test the programmers macroed code
priv _i, _maxerrors, _errorcnt, _errorfnd, _v, _topitem

_maxerrors = 60
_errorcnt = 0
_errorfnd = .f.

private errors[_maxerrors]
gget_rdcnt = gget_rdcnt + 1
gget_rd1st[gget_rdcnt+1] = gget_cnt + 1
_topitem = gget_rd1st[gget_rdcnt]
for _i = _topitem to gget_cnt
   vread_error( type( "gget_row[_i]" ) == "N", "row" )
   vread_error( type( "gget_col[_i]" ) == "N", "col" )
   if !vread_error( !( type( "gget_var[_i]" ) == "U" ), "var " + str( _i, 3 ) )
      _v = gget_var[_i]
      vread_error( !( type( _v ) == "U" ), "var" )
   endif
   vread_error( type( "gget_pict[_i]" ) == "C", "pict" )
   vread_error( type( "gget_empty[_i]" ) == "L", "not empty" )
   if !vread_error( type( "gget_valid[_i]" ) == "C", "valid" )
      _v = gget_valid[_i]
      if "V" $ gget_ptype[_i] 
         vread_error( type( _v ) == "L" .or. type( _v ) == "UI", "valid" )
      elseif "D" $ gget_ptype[_i]                       
         if !empty( _v ) 
            vread_error( type( _v+"()" ) == "UI", "valid" )
         endif
      endif
   endif               
   if !vread_error( type( "gget_mess[_i]" ) == "C", "message" )
      if substr( gget_mess[_i], 1, 1 ) == "&"
         _v = substr( gget_mess[_i], 2, len( gget_mess[_i] ) - 1 )
         vread_error( !( type( _v ) == "U" ), "message macro" )
      endif
   endif
next            
gget_rdcnt = gget_rdcnt - 1

return( !vread_error( .f., "" ) )


********************
FUNCTION VREAD_ERROR

parameters ok, errormess

if ok
   return( .f. )
endif

priv j, mess
* _i from vread_test is accessed
if ( _errorcnt = _maxerrors .or. empty( errormess ) ) .and. _errorfnd
   * print the errors
   set print on                                
   for j = 1 to _errorcnt
      ?errors[j]
   next       
   ?
   eject
   set print off
   _errorcnt = 0
endif             
if !empty( errormess )
   _errorfnd = .t.
   _errorcnt = _errorcnt + 1
   mess = setlen( if( type( "gget_var[_i]" ) != "U", gget_var[_i], "Unkwown" ), 20 )
   errors[_errorcnt] = mess + errormess
endif

return( if( empty( errormess ), _errorfnd, .t. ) )



function PUSHGETS

	private maxprompts

	maxprompts = 26

   public sget_row[maxprompts], sget_col[maxprompts], sget_var[maxprompts], ;
          sget_pict[maxprompts], sget_empty[maxprompts], sget_valid[maxprompts],  ;
          sget_mess[maxprompts], sget_ptype[maxprompts], sget_cnt, sget_mrow, sget_mcol, ;
          sget_mlen, sget_mclr, sget_rdcnt, sget_rd1st[20], sget_cond[maxprompts], ;
          sget_exkey, svget_exit, svread_exit, sget_clear[maxprompts], sget_emerr, ;
          svread_cursor, svread_upvalid

	acopy( gget_row, sget_row )
	acopy( gget_col, sget_col )
	acopy( gget_var, sget_var )
	acopy( gget_pict, sget_pict )
	acopy( gget_empty, sget_empty )
	acopy( gget_valid, sget_valid )
	acopy( gget_mess,  sget_mess  )
	acopy( gget_ptype, sget_ptype )
	acopy( gget_cond,  sget_cond  )
	acopy( gget_clear, sget_clear )

	sget_cnt      = gget_cnt
   sget_rdcnt    = gget_rdcnt
   sget_rd1st[1] = gget_rd1st[1]
	sget_mrow     = gget_mrow 
	sget_mcol     = gget_mcol
	sget_mlen     = gget_mlen
   sget_mclr     = gget_mclr
   svread_exit   = vread_exit
   sget_emerr    = gget_emerr
   svread_cursor = vread_cursor
   svread_upvalid = vread_upvalid

	gget_cnt = 0
	gget_rdcnt = 0
	gget_rd1st[1] = 1
	store 0 to gget_mrow , gget_mcol, gget_mlen
	gget_mclr = ""
	vread_exit = 0
	gget_emerr = ""
	vread_cursor = .t.
	vread_upvalid = .t.

return ( .t. )


function POPGETS

	acopy( sget_row, gget_row )
	acopy( sget_col, gget_col )
	acopy( sget_var, gget_var )
	acopy( sget_pict, gget_pict )
	acopy( sget_empty, gget_empty )
	acopy( sget_valid, gget_valid )
	acopy( sget_mess,  gget_mess  )
	acopy( sget_ptype, gget_ptype )
	acopy( sget_cond,  gget_cond  )
	acopy( sget_clear, gget_clear )

	gget_cnt      = sget_cnt
   gget_rdcnt    = sget_rdcnt
   gget_rd1st[1] = sget_rd1st[1]
	gget_mrow     = sget_mrow 
	gget_mcol     = sget_mcol
	gget_mlen     = sget_mlen
   gget_mclr     = sget_mclr
   gget_emerr    = sget_emerr
   vread_exit    = svread_exit
   vread_cursor  = svread_cursor
   vread_upvalid = svread_upvalid

	release sget_row, sget_col, sget_var, sget_pict, sget_empty, sget_valid, ;
   	     sget_mess, sget_ptype, sget_cnt, sget_mrow, sget_mcol, sget_mclr, ;
      	  sget_rdcnt, sget_cond, sget_exkey, svread_exit, sget_clear, sget_emerr, ;
	        svread_cursor, svread_upvalid

return ( .t. )


      

