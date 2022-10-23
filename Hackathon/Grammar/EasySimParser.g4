parser grammar EasySimParser;

options { 
		  tokenVocab = EasySimLexer ;
		  language  = "CSharp";
		}

parse : ( instructions )* EOF ;

instructions : ( instruction )*				# instruction
			 | ( execute_together )*		# instructionExecuteTogether
			 ;

instruction : SET_ 'I' ('X' NAME BOOLEAN_ | 'W' NAME NUMBER_ )						# set
			| ASSERT_ 'Q' ('X'  NAME BOOLEAN_ | 'W'  NAME NUMEBR_ )	(interval)?     # assert
			;

execute_together :  '(' ( instruction )+ ')' 	 # executeTogether
				 ;

interval : FOR_ NUMBER MS_			# intervalTime
		 | FOR_ NUMBER CYCLES_		# intervalCycles
		 ;

