* Program .....: TYMENU
* Author ......: SC
* Purpose......: To produce the draw down menu for NFL module

#INCLUDE "TYCOON.H"

PROCEDURE TyMenu

private continu

clear_scn()

DD_HEAD( G_sysowner, G_modname, G_version )

up         = chr(5)
right      = chr(4)+chr(9)
left       = chr(19)+chr(15)
down       = chr(24)+chr(32)
return     = chr(13)
done_one   = .f.
firstleter = "URMTX"
main1      = "Update"
main2      = "Reports"
main3      = "Miscellaneous"
main4      = "Tables"
main5      = "X-exit"
maxmain    = 5
inkey      = 0
mainline   = 1
scroll     = "1"

*---Draw the top line of the menu.....
topline = 9
setcolor( C_mainop )
@ topline,0 say replic("Ä",80)
store "1" to col
do while val(col)<= maxmain
	@ topline+1,2+((val(col)-1)*68/(maxmain-1)) say main&col
	store ltrim(str(val(col)+1,2)) to col
enddo
@ topline+2,0 say replic("Ä",80)

do while .t.

	if gsys_test
		*---Memory left
		@ 00,00 say 'Memory left : ' + ltrim(str(memory(0))) + 'K'
	endif
	do case
		case mainline=1
			store "Owners      " to choice1
			store "Update the Owners Database" to descrip1
			store "Comps       " to choice2
			store "Update the Competitions File" to descrip2
			store "Results     " to choice3
			store "View the results of TFL games" to descrip3
			store "Transactions" to choice4
			store "Enter Tycoon transactions" to descrip4
			store "Schedule    " to choice5
			store "Enter Tycoon schedules" to descrip5
			store  5 to maxoptions
         if chr(inkey)=return                        
            continu = .t.
            if continu
   				do case
	      		case val(scroll)=1

						do OWNERS

					case val(scroll)=2

						do COMPS

					case val(scroll)=3

						do TYRMATCH

					case val(scroll)=4
						unavail()
					case val(scroll)=5
						
						do TYSCHED

               endcase
            endif
			endif

		case mainline=2

			store "Standings  " to choice1
			store "Generate and Print the Standings for a league" to descrip1
			store "Rankings   " to choice2
			store "Generate and Print the Tycoon Rankings for players" to descrip2
			store "Rosters    " to choice3
			store "Generate and Print the Tycoon Rosters" to descrip3
			store "Record     " to choice4
			store "Generate and Print a Team Record" to descrip4
			store "Caps       " to choice5
			store "Generate and Print a Teams' Appearance Record" to descrip5
			store "Weeklies   " to choice6
			store "Generate and Print the Weekly Winners" to descrip6
			store "Performance" to choice7
			store "Generate and Print the Performance Reports" to descrip7
			store "Exp Output " to choice8
			store "Generate and Print the Expected Output Reports" to descrip8
			store "Head 2 Head" to choice9
			store "Generate and Print the Head to Head Reports" to descrip9
			store  9 to maxoptions
			if chr(inkey)=return
				do case
					case val(scroll)=1
						do TYRSTAND
					case val(scroll)=2
						do TYRRANK
					case val(scroll)=3
						do TYRROST
					case val(scroll)=4
						do TYRTEAM
					case val(scroll)=5
						do TYRCAPS
					case val(scroll)=6
						do TYRWEEK
					case val(scroll)=7
						do TYRPERF
					case val(scroll)=8
						do TYREXP
					case val(scroll)=9
						do TYRHEAD
				endcase
			endif

		case mainline=3

			store "Housekeeping" to choice1
			store "Pack and Reindex files" to descrip1
			store "Statistics  " to choice2
			store "Show file statistics" to descrip2
			store "Margins     " to choice3
			store "Margin of victory report" to descrip3
			store "Owner List  " to choice4
			store "Print Distribution List" to descrip4
			store "BatchReports" to choice5
			store "Update Weekly Reports" to descrip5
			store  5 to maxoptions
			if chr(inkey)=return
				do case
					case val(scroll)=1
						do INDEXGEN with G_ty_path
						opendbf( '*', G_ty_path )
					case val(scroll)=2
						do STATSON with G_ty_path
						opendbf( '*', G_ty_path )
					case val(scroll)=3
						do TYRMARG.PRG
					case val(scroll)=4
						do TYRDIST.PRG
					case val(scroll)=5
						do TYRBAT.PRG
				endcase
			endif

		case mainline=4

			store "Leagues" to choice1
			store "Update the Leagues table" to descrip1
			store "Honours" to choice2
			store "Update the Honours table" to descrip2
			store  2 to maxoptions
			if chr(inkey)=return
				do case
					case val(scroll)=1
						unavail()
					case val(scroll)=2
						unavail()
				endcase
			endif

		case mainline=5

			store "Exit System" to choice1
			store "Return to the previous menu" to descrip1
			store  1 to maxoptions
			if chr(inkey)=return
				do case
               case val(scroll)=1
						RETU
				endcase
			endif
	endcase

	if done_one
		done_one=.f.
		clear_scn()
		DD_HEAD( G_sysowner, G_modname, G_version )
		store 0 to inkey
		*---Draw the top line of the menu.....
		store "1" to col
		setcolor( C_mainop )
		do while val(col)<= maxmain
			@ topline,0 say replic("Ä",80)
			@ topline+1,2+((val(col)-1)*68/ (maxmain-1)) say main&col
			@ topline+2,0 say replic("Ä",80)
			store ltrim(str(val(col)+1,2)) to col
		enddo
		loop
	endif

	if type('mstr')='U'
		store "1" to mstr
	endif

	@ topline+1,2+((val(mstr)-1)*68/ (maxmain-1)) say main&mstr

	store str(mainline,1) to mstr
	setcolor( C_mainsl )
	@ topline+1,2+((val(mstr)-1)*68/ (maxmain-1)) say main&mstr
	setcolor( C_mainop )

	* draw the box that the menu will appear in.
	@ topline+3,0 clear
	store 2+((mainline-1)*68/ (maxmain-1)) to column
	store min(column,column+(79-(column+len(choice1)+ 3))) to across
	@ topline+3,across say 'Ú'+repli('Ä',len(choice1)+2)+'¿'
	store 1 to box
	do while box<=maxoptions
		@ topline+3+box,across say '³'+space(len(choice1)+2)+'³'
		store box+1 to box
	enddo
	@ topline+3+box,across say 'À'+repli('Ä',len(choice1)+2)+'Ù'

	store val(scroll) to currentline
	* write the choices into the window
	store 1 to scrollnum
	do while scrollnum<=maxoptions
		store ltrim(str(scrollnum,2)) to scroll
		@ topline+3+scrollnum,across+2 say choice&scroll
		store scrollnum+1 to scrollnum
	enddo

	inkey = 0

	do while .not. upper(chr(inkey))$return+right+left+firstleter
		*---display current line in reverse video
		setcolor( C_mainsl )
		store ltrim(str(currentline,2)) to scroll
		@ topline+3+currentline,across+2 say choice&scroll
		setcolor( C_mainop )

		* write the long comment on the bottom of the screen
		store substr(space(40-(len(trim(descrip&scroll))/2))+descrip&scroll+space(40),1,80) to descript
		@ 24,0 say descript

		store 0 to inkey
		do while .not. upper(chr(inkey))$return+up+down+left+right+firstleter
			store inkey() to inkey
			if inkey = K_ESC .and. gsys_test
				RETURN
			endif
		enddo
		* if an option was selected, jump to top of loop and set "done_one" to true!
		if chr(inkey)=return
			done_one=.t.
			loop
		endif
		*if cursor key=up or down move up and down the menu
		if chr(inkey)$up+down
			do case
				case chr(inkey)$up
					if .not. currentline=1
						store -1 to change
					else
						store maxoptions-1 to change
					endif
				case chr(inkey)$down
					if .not. currentline=maxoptions
						store 1 to change
					else
						store -maxoptions+1 to change
					endif

			endcase

			* rewrite the current line without the highlight....
			@ topline+3+currentline,across+2 say choice&scroll
			* update the current line
			currentline=currentline+change
			store ltrim(str(currentline,2)) to scroll
		endif
		* if cursor key is left or right or a letter of an option change menu!
		do case
			case chr(inkey)$left
				store "1" to scroll
				if mainline=1
					mainline=maxmain
				else
					mainline=mainline-1
				endif
			case chr(inkey)$right
				store "1" to scroll
				if mainline=maxmain
					mainline=1
				else
					mainline=mainline+1
				endif
			case upper(chr(inkey))$firstleter
				store "1" to scroll
				mainline=at(upper(chr(inkey)),firstleter)
		endcase

   enddo

enddo

return



*EOF:  MEMENU.PRG

