lexer grammar EasySimLexer;

options { caseInsensitive = false; }


/* KEYWORDS */

ASSERT_ : 'assert' ;
SET_ : 'set' ;
FOR_ : 'for' ;
BOOLEAN_ : 'false' | 'true' ;
MS_ : 'ms' ;
CYCLES_ : 'cycles' ;

NAME : [A-Za-z0-9_]+ ;
NUMBER : [0-9] | [1-9][0-9]* ;