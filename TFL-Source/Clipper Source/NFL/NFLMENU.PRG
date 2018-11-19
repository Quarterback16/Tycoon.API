* Program .....: MEMENU
* Author ......: SC
* Purpose......: To produce the draw down menu for NFL module

#INCLUDE "nfl.h"

PROCEDURE NflMenu

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
			store "Players    " to choice1
			store "Update the Player Database" to descrip1
			store "Schedule   " to choice2
			store "Update the NFL Schedules" to descrip2
			store "Box Scores " to choice3
			store "Enter the results of NFL games" to descrip3
			store "Injuries   " to choice4
			store "Enter injuries to players" to descrip4
			store "Stats      " to choice5
			store "Enter individual stats"    to descrip5
			store "update score totals"    to descrip6
			store "Scores     " to choice6
			store "Update projections"    to descrip7
			store "Projections" to choice7
			store "Tally Spreads"    to descrip8
			store "Spreads    " to choice8
			store  8 to maxoptions
         if chr(inkey)=return                        
            continu = .t.
            if continu
   				do case
	      		case val(scroll)=1

						do PLAYER

					case val(scroll)=2

						do SCHED

					case val(scroll)=3

						do BOXSCORE

					case val(scroll)=4

						unavail()

					case val(scroll)=5

						do LoadStat

					case val(scroll)=6

						do UpdScore

					case val(scroll)=7

						do UpdProj

					case val(scroll)=8

						do UpdSpred

               endcase
            endif
			endif

		case mainline=2

			store "NFL Standings     " to choice1
			store "Generate and Print Standings" to descrip1
			store "Player Tally Sheet" to choice2
			store "Generate and Print Stat Tally sheets" to descrip2
			store "Team Stats Sheet" to choice3
			store "Generate and Print Team Stat sheets" to descrip3
			store "NFL Rankings     " to choice4
			store "Generate and Print Rankings" to descrip4
			store "Player Moves     " to choice5
			store "Generate and Print Rankings" to descrip5
			store "Team Defense     " to choice6
			store "Generate and Print Defensive Output" to descrip6
			store  6 to maxoptions
			if chr(inkey)=return
				do case
					case val(scroll)=1
						do NFLstand
					case val(scroll)=2
						do Tally
					case val(scroll)=3
						do NFLrTeam
					case val(scroll)=4
						do NFLrank
					case val(scroll)=5
						do NFLrmove
					case val(scroll)=6
						do NFLrDef
				endcase
			endif

		case mainline=3

			store "Load Week   " to choice1
			store "Load a weeks scores" to descrip1
			store "Housekeeping" to choice2
			store "Pack and Reindex files" to descrip2
			store "Statistics  " to choice3
			store "Generate Statistics" to descrip3
			choice4  := "Archive   "
			descrip4 :=	"Archive statistics off to another directory"
			choice5  := "Load Draft"
			descrip5 :=	"Load the player database with new rookies"

			store  5 to maxoptions
			if chr(inkey)=return
				do case
				case val(scroll)=1
					do LoadWk3
					case val(scroll) = 2
						do INDEXGEN with G_nfl_path
						opendbf( "*", G_nfl_path, .t. )
					case val(scroll) = 3
						close databases
						do STATSON with G_nfl_path
						opendbf( "*", G_nfl_path, .t. )
					case val(scroll) = 4
						do Archive
					case val(scroll) = 5
						do LoadPick
				endcase
			endif

		case mainline=4

			store "Teams    " to choice1
			store "Update the Teams table" to descrip1
			store "HTM strip" to choice2
			store "Update the Honours table" to descrip2
			store  2 to maxoptions
			if chr(inkey)=return
				do case
					case val(scroll)=1
						unavail()
					case val(scroll)=2
						do StripHTM
				endcase
			endif

		case mainline=5

			store "Exit System" to choice1
			store "Return to the previous menu" to descrip1
			store  1 to maxoptions
			if chr(inkey)=return
				do case
               case val(scroll)=1
						retu
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

RETURN



*EOF:  MEMENU.PRG
