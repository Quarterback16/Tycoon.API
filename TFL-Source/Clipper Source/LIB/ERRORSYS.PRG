***
***	ERRORSYS..............Modified by SC 3-AUG-89
***
***	Clipper error system
***
***	This version will record any error messages to an ascii file called
***   ERROR.LOG.  The aim of this is to facilitate support of production
***	systems.
***

NOTE ALTD()

return


function EXPR_ERROR
	***
	***	expr_error( name, line, info, model, _1, _2, _3)
	***
	***	Error messages:-
	***
	***		Type Mismatch
	***		Subscript range
	***		Zero Divide
	***		Expression error
	***
	***	Note:  This error is unrecoverable
	***

	parameters name, line, info, model, _1, _2, _3

	private m_errmsg, m_errhndl

	set device to screen
	m_errmsg = "Proc " + M->name + " line " + ltrim(str(M->line)) +", " + M->info
	@ 0, 0
	@ 0, 0 say m_errmsg

	do WRITE_LOG with m_errmsg

	altd( 2 )

	close all
	quit

return (.f.)



function UNDEF_ERROR
	***
	***	undef_error(name, line, info, model, _1)
	***
	***
	***	Error Messages:-
	***
	***		Undefined Identifier
	***		Not an Array
	***		Missing EXTERNAL
	***
	***
	***	Note:  This error is unrecoverable
	***

	parameter name, line, info, model, _1

	private m_errmsg, m_errhndl

	set device to screen
	m_errmsg = "Proc " + M->name + " line " + ltrim( str( M->line)) +	", " + M->info + " " + M->_1

	@ 0, 0
	@ 0, 0 say m_errmsg

	do WRITE_LOG with m_errmsg

	altd( 2 )

	close all
	quit

return (.t.)



function MISC_ERROR
	***
	***	misc_error(name, line, info, model)
	***
	***   Miscellaneous Errors
	***
	***	Error messages:-
	***
	***		Type mismatch
	***		RUN error
	***
	***	Note:  This error is unrecoverable on Type Mismatch, but RUN error
	***			can be corrected in the users AUTOEXEC.BAT file by using
	***			DOS SET command.  Eg  SET COMSPEC=C:\COMMAND.COM
	***

	parameters name, line, info, model

	private m_errmsg, m_errhndl
	set device to screen
	m_errmsg = "Proc " + M->name + " line " + LTRIM(STR(M->line)) +", " + M->info
	@ 0, 0
	@ 0, 0 say m_errmsg

	do WRITE_LOG with m_errmsg

	altd( 2 )

	close all
	quit

return (.f.)


function OPEN_ERROR
	***
	***	open_error(name, line, info, model, _1)
	***
	***	Error Messages:-
	***
	***		Open Error
	***
	***
	***	Note:  This routine can be further modified if DOSERROR() = 2 ie
	***			file does not exist.  When this happens, evaluate the M->1
	***			memory variable to determine which file is missing.
	***

	parameters name, line, info, model, _1

	private m_errmsg, m_errhndl

	if neterr() .and. model == "USE"
		retu (.f.)
	endif

	set device to screen
	m_errmsg = "Proc " + M->name + " line " + LTRIM(STR(M->line)) +", " + M->info + " " + M->_1 + " (" + LTRIM(STR(DOSERROR())) + ")"
	@ 0, 0
	@ 0, 0 say m_errmsg
	@ 1, 65 SAY "Retry? (Y/N)"
	m_errmsg = dtoc(date()) + '  ' + time() + '  ' + m_errmsg + ', Select: ' +;
	ltrim( str( select())) + ', Alias: ' + alias() + chr(13) + chr(10)


	inkey(0)

	do while .not. chr(lastkey()) $ "YyNn"
		inkey(0)
	enddo

	if .not. chr(lastkey()) $ "Yy"
		do LOGIT with m_errmsg

		altd( 2 )
		close all
		quit
	endif

	@ 0,0

return (.t.)



function DB_ERROR
	***
	***	db_error(name, line, info)
	***
	***	Error Messages:-
	***
	***		Database required
	***		Lock required
	***		Exclusive required
	***		Field numeric overflow
	***		Index file corrupted
	***
	***
	parameter name, line, info

	private m_errmsg, m_errhndl

	set device to screen

	m_errmsg = "Proc " + M->name + " line " + ltrim(str(M->line)) +", " + M->info
	@ 0, 0
	@ 0, 0 say m_errmsg

	do WRITE_LOG with m_errmsg

	altd( 2 )

	close all
	quit

return (.f.)


function PRINT_ERROR
	***
	***	print_error(name, line)
	***

   parameter name, line        

   do while !isprinter()
      if !print_cont( "Printer not ready." )
         break
         shutdown()
         quit
      endif
   enddo

return (.t.)


procedure WRITE_LOG

	parameter m_errmsg

	m_errmsg = dtoc(date()) + '  ' + time() + '  ' + m_errmsg + ', Select: ' +;
	ltrim( str( select())) + ', Alias: ' + alias() + chr(13) + chr(10)

	do LOGIT with m_errmsg

return


procedure LOGIT

	parameter m_errmsg

	m_errhndl = fopen( "ERROR.LOG", 2)

	if m_errhndl = -1
		*---File not found?
		m_errhndl = fcreate( "ERROR.LOG" )
	endif

	if m_errhndl <> -1
		fseek(  m_errhndl, 0, 2)
		fwrite( m_errhndl, m_errmsg)
		fclose( m_errhndl)
		*---Dump Mem vars
		*save to error
	endif

return


