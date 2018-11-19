LOCAL stripFile, cBigStr, nSize

CLEAR

stripFile = GETFILE("txt")
? stripFile

nHandle = FOPEN(stripFile, 12)

IF nHandle < 0
	WAIT "Cannot open"
ELSE
 	nSize = FSEEK(nHandle, 0, 2 )
 	? "Input file has " + STR( nSize ) + " bytes"
 	=FSEEK(nHandle, 0, 0 )
      cBigStr = FREAD( nHandle, nSize )
	=FCLOSE(nHandle)
ENDIF

cNewStr = TransformFile( cBigStr )

? "Output file has " + STR( LEN(cNewStr) ) + " bytes"
*---Write it back out
nNewHandle = FCREATE("GS4_Wk21.txt")
IF nNewHandle < 0 
	WAIT "Error creating file"
ELSE
	=FWRITE(nNewHandle, cNewStr )
	=FCLOSE(nNewHandle)
endif


function Transformfile
	LPARAMETERS cStr
	LOCAL loRE
	cOut = ""
	loRE = CREATEOBJECT( "vbscript.regexp")
	loRE.global = .t.
	loRE.ignorecase = .t.
	loRE.pattern = '<.{1}>'   &&  pattern to match
	cOut = loRE.replace( cStr, '' )
	loRE.pattern = '<..{1}>'  
	cOut = loRE.replace( cOut, '' )
	loRE.pattern = '<...{1}>'  
	cOut = loRE.replace( cOut, '' )
	loRE.pattern = '<....{1}>'  
	cOut = loRE.replace( cOut, '' )
	loRE.pattern = '<.....{1}>'  
	cOut = loRE.replace( cOut, '' )
	loRE.pattern = '<......{1}>'  
	cOut = loRE.replace( cOut, '' )
	loRE.pattern = '<.......{1}>'  
	cOut = loRE.replace( cOut, '' )
	loRE.pattern = '<........{1}>'  
	cOut = loRE.replace( cOut, '' )
	loRE.pattern = '<.........{1}>'  
	cOut = loRE.replace( cOut, '' )
	loRE.pattern = '<..........{1}>'  
	cOut = loRE.replace( cOut, '' )
	loRE.pattern = '<...........{1}>'  
	cOut = loRE.replace( cOut, '' )
	loRE.pattern = '<............{1}>'  
	cOut = loRE.replace( cOut, '' )
	
RETURN cOut
	