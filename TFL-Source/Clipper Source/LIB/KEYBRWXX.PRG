* parameter testkey
* if pcount() = 0
*    testkey = "b"
* endif
* set deleted on
* setcolor( "w/n, n/w" )
* testcode()
*
* a browse of a file only on the key expression, dbedit style with out the need
* for use of the filter, an index of the key is required.
* expects that the file is currently selected, with the correct index set

* if the param "dkeyexp" is not supplied the udf is called for the key.
* if flds is not supplied the udf is called for the prompt of fields.
* this method runs faster !!

* udfmodes    5 - generate data key for current record
*             6 - generate prompt for current record, empty if != dkey
*             0 - key exception
*             3 - data file empty for specified key

* udf returns for mode  5   - the group key on the current record
*                       6   - empty str if record is not in group
*                             else the prompt for this record
*                             note - prompts must be the same length as the window width
*                       0,3 - 0 quit
*                             1 continue
*                             2 redraw screen and continue
*
*
* NOTE this requires an object file called KEYBRWXC.OBJ with should be placed
*                                          ^^^^^^^^^^^^
*      in the library with KEYBRWXX.OBJ


******************
FUNCTION KEYBROWSE

	parameters _t, _l, _b, _r, _udf, _dkey, _start

	priv _dkeymode, _fldmode, _fclr, _bclr, _saveclr, _winlen, _udfcall, ;
	_item, _i, _j, __getall, _mode, _reccnt, _scrpage, _cols, _recpos, ;
	_vector, _newprompt, _ik, _blankpage, _saverec, _udfret, _flds, _dkeyexp, ;
	_startrec

	* _recpos:  -1 top of page, 1 bot of page, else 0

	if pcount() < 7
		_start = 1
	endif
	_saverec = recno()
	_winlen = _b-_t+1
	private _prompts[_winlen], _recnos[_winlen]

	_saveclr = setcolor()
	_fclr = getcolor( 1 )
	_bclr = getcolor( 2 )
	_udfcall = _udf + "(_mode)"

	_dkeymode = if( type( "_dkeyexp" ) = "U", 1, 2 )
	_fldmode  = if( type( "_flds" )    = "U", 1, 2 )

	_dkeyexp = if( _dkeymode=1, _udf+"(5)", _dkeyexp )
	if _fldmode = 1
		_flds = _udf+"(6)"
	endif

	_cols = _r-_l+1
	_item = 0
	setcolor( _fclr )
	_i = savescreen( _t, _l, _b, _r )
	@_t,_l clear to _t+1,_l+1
	_blankpage = savescreen( _t, _l, _t+1, _l+1 )
	*restscreen( _t, _l, _b, _r, _i )
	_blankpage = replicate( substr( _blankpage, 1, 2 ), _winlen * _cols * 2 )

	_recpos = 1
	_reccnt = 0
	_getall = .t.
	_vector = 0

	if !empty( _dkeyexp )
*		testmsg( '_dkeyexp = ' + _dkeyexp )
		if !keybrw_wid()
			retu( 0 )
		endif
	endif
	_item = _start
	_startrec = recno()
	if _item > 1
		keybrw_brc( .f. )
	endif
	if !keybrw_dat()
		retu( 0 )
	endif

	if _item > 1
		_i = ascan( _recnos, _startrec, 1, _reccnt )
		if _i > 0
			_item = max( min( _i, _reccnt ), 0 )
		endif
	endif

	do while .t.

		restscreen( _t, _l, _b, _r, _scrpage )
		setcolor( _bclr )
		@_t+_item-1,_l say _prompts[_item]

		_ik = inkey( 0 )
		if _ik = 0
			loop
		endif

		if _ik = 24											  && down

			if _item < _reccnt
				_item = _item + 1
				loop
			else
				_getall = .f.
				_vector = 1
			endif

		elseif _ik = 5									  && up

			if _item > 1
				_item = _item - 1
				loop
			else
				_getall = .f.
				_vector = -1
			endif

		elseif _ik = 3									  && pgdn
			_getall = .t.
			_vector = 1

		elseif _ik = 18									  && pgup
			_getall = .t.
			_vector = -1

      elseif _ik = 27
         keybrw_rec()
			exit

		elseif _ik = 6									  && end
			_item = _reccnt
			loop

		elseif _ik = 1									  && home
			_item = 1
			loop

		else
			if keybrw_rec()
				_mode = 0
				setcolor( _saveclr )
				@row(),_l say ""
				_udfret = &_udfcall
				if _udfret = 0
					exit
				endif
				if _udfret != 2 .and. indexord() > 0

					* try to move the file to a record with the index beginning
					* with the key pressed
					if _ik > 47 
						_i = chr( _ik )
						if (_i >= "0" .and. _i <= "9") .or. (_i >= "A" .and. _i <= "Z" );
							 .or. (_i >= "a" .and. _i <= "z" )
							 set softseek on
							 dbseek( _dkey + _i )
							 if !found()
								 if (_i >= "A" .and. _i <= "Z" ) .or. (_i >= "a" .and. _i <= "z" )
									 _i = if( isupper( _i ), lower( _i ), upper( _i ) )
									 dbseek( _dkey + _i )
								 endif
							 endif
							 set softseek off
							 if !found()
								 if !( _dkey == &_dkeyexp )
									 skip -1
								 endif
							 endif
							 _i = ascan( _recnos, recno(), 1, _reccnt )
							 if _i = 0
								 _udfret = 2
								 _item = 1
							 else
								 _item = _i
							 endif

						endif
					endif

					if _udfret != 2
						go _recnos[_item]
						_recpos = 0
						if keybrw_rec()
							 loop
						endif
					endif

				endif
			endif
			_getall = .t.
			_vector = 0
			_recpos = 0

		endif

		if !keybrw_dat()
			exit
		endif

	enddo
	_start = _item
	setcolor( _saveclr )
return( 0 )


*******************
FUNCTION KEYBRW_DAT

	* gets data for the _prompts
	* go top or bot of page
	* if rec does not match _dkey
	*   find a new start pos from recs on page
	*   if none match
	*     start from top of _dkey group
	*     if no data in dkey group return .f.
	* _vector -1 up file, from top
	*         1 down file, from bot
	*         0 down file, from current pos

	if _vector = 1
		if _recpos != 1
			if _reccnt > 0
				go _recnos[_reccnt]
			endif
		endif
	elseif _vector = -1
		if _recpos != -1
			go _recnos[1]
		endif

	endif
	_recpos = _vector

	do while .t.
		if !( &_dkeyexp == _dkey ) .or. deleted()
			* start from somewhere else on the page
			private _i, _j
			if !_getall
				go _recnos[1]
				_vector = 0
			endif
			_j = if( _vector = 0, 1, -(_vector) )
			_i = if( _vector = 1, _reccnt, 1 )
			do while _i > 0 .and. _i <= _reccnt
				go _recnos[_i]
				if &_dkeyexp == _dkey .and. !deleted()
					exit
				endif
				_i = _i + _j
			enddo
         if !( &_dkeyexp == _dkey ) .or. deleted()
            if indexord() > 0
               dbseek( _dkey )					  && start page from scratch
            else
               go top
            endif
				if !keybrw_nod()
					retu( .f. )
				endif
			endif
			_reccnt = 0
			_getall = .t.
			_vector = 0
		endif

		if _getall

			if _vector = -1
				_recpos = -1
				_i = _winlen
				do while _i > 0
					skip -1
					_newprompt = &_flds
					if empty( _newprompt ) .or. bof()
						if !bof()
							 _recpos = 0
						endif
						exit
					endif
					ains( _prompts, 1 )
					ains( _recnos, 1 )
					_prompts[1] = _newprompt
					_recnos[1] = recno()
					if _reccnt < _winlen
						_reccnt = _reccnt + 1
					endif
					_i = _i - 1
				enddo

			else
				_recpos = 1
				if _vector = 0
					_newprompt = &_flds
					_prompts[1] = _newprompt
					_recnos[1] = recno()
					_i = 2
					_reccnt = 1
				else
					_i = 1
				endif
				do while _i <= _winlen
					skip
					_newprompt = &_flds
					if empty( _newprompt )
						_recpos = 0
						exit
					endif
					if _reccnt = _winlen
						adel( _prompts, 1 )
						adel( _recnos, 1 )
					else
						_reccnt = _reccnt + 1
					endif
					_prompts[_reccnt] = _newprompt
					_recnos[_reccnt] = recno()
					_i = _i + 1
				enddo
			endif
			if _item > _reccnt
				_item = _reccnt
			endif

		else
			skip _vector
			_newprompt = &_flds
			if !empty( _newprompt ) .and. !bof()
				_recpos = _vector
				if _vector = 1
					if _reccnt = _winlen
						adel( _prompts, 1 )
						adel( _recnos, 1 )
					else
						_reccnt = _reccnt + 1
					endif
					_prompts[_reccnt] = _newprompt
					_recnos[_reccnt] = recno()
					if ascan( _recnos, recno(), 2, _reccnt ) > 0
						* the new record is already on the page, redraw
						_vector = 0
						_getall = .t.
						go _recnos[1]
						loop
					endif
				else
					ains( _prompts, 1 )
					ains( _recnos, 1 )
					_prompts[1] = _newprompt
					_recnos[1] = recno()
					if _reccnt < _winlen
						_reccnt = _reccnt + 1
					endif
					if ascan( _recnos, recno(), 1, _reccnt-1 ) > 0
						* the new record is already on the page, redraw
						_vector = 0
						_getall = .t.
						go _recnos[1]
						loop
					endif
				endif
			else
				tone( 800, .5 )
				tone( 600, .5 )
				_vector = 0
				_getall = .t.
				go _recnos[1]
				loop
			endif

		endif
		exit
	enddo

	_scrpage = _blankpage
	keybrw_pgc( @_scrpage, _prompts, _reccnt, _cols )

return( .t. )


*******************
FUNCTION KEYBRW_REC

	* try to move file to current prompt
	_recpos = 0
	go _recnos[_item]
	_newprompt = &_flds
	if !empty( _newprompt )
		_prompts[_item] = _newprompt
		_scrpage = _blankpage
		keybrw_pgc( @_scrpage, _prompts, _reccnt, _cols )
		restscreen( _t, _l, _b, _r, _scrpage )
		setcolor( _bclr )
		@_t+_item-1,_l say _prompts[_item]
		retu( .t. )
	else
		retu( .f. )
	endif


*******************
FUNCTION KEYBRW_BRC

	* try to back record to top of page

	_recpos = 0
	_i = _item
	do while _i > 1
		skip -1
		if !( &_dkeyexp == _dkey ) .or. bof()
			_item = 1
			_recpos = -1
			exit
		endif
		_i = _i - 1
	enddo

return( 0 )


*******************
FUNCTION KEYBRW_NOD
	*---no data in file
	do while empty( &_dkeyexp ) .or. deleted()
		_mode = 3
		if( &_udfcall ) = 0
	   	retu( .f. )
      endif
      if indexord() > 0
         dbseek( _dkey )
      else
         go top
      endif
   enddo
   if !keybrw_wid()
	   retu( .f. )
   endif

return( .t. )


function KEYBRW_WID
	*---width of window matches the width of the prompt
	_newprompt = &_flds
	if len( _newprompt ) != _cols
		_msg = "KEYBROWSE ERROR - window width = " + ltrim( str( _cols ) ) + " & "
		_msg = _msg + "prompt length width = " + ltrim( str( len( _newprompt ) ) )
		testmsg( _msg )
*		testmsg( 'New prompt  = ' + _newprompt )
		retu( .f. )
	endif

return( .t. )


*EOF:  KEYBRWXX.PRG





