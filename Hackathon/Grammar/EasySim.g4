grammar EasySim;

parse : instructions* EOF ;

instructions : instruction*				# instructionsExecuteAlone
			 | execute_together*		# instructionsExecuteTogether
			 ;

instruction : SET_ INPUT_ ( XTYPE_ NAME BOOLEAN_ | WTYPE_ NAME NUMBER )							# set
			| ASSERT_ OUTPUT_ (XTYPE_  NAME BOOLEAN_ | WTYPE_  NAME NUMBER )	(interval)?     # assert
			;

execute_together :  OPENPAR_ instruction+ CLOSEPAR_	 # executeTogether
				 ;

interval : FOR_ NUMBER MS_			# intervalTime
		 | FOR_ NUMBER CYCLES_		# intervalCycles
		 ;

ASSERT_ : 'assert' ;
SET_ : 'set' ;
FOR_ : 'for' ;
BOOLEAN_ : 'false' | 'true' ;
MS_ : 'ms' ;
CYCLES_ : 'cycles' ;
OPENPAR_ : '(' ;
CLOSEPAR_ : ')' ;
INPUT_ : 'I' ;
OUTPUT_ : 'Q' ;
XTYPE_ : 'X' ;
WTYPE_ : 'W' ;

NAME : [A-Za-z0-9_]+ ;
NUMBER : [0-9] | [1-9][0-9]* ;