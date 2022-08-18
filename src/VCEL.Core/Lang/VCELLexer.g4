lexer grammar VCELLexer;

LET: 'let';
MATCH: 'match';
OTHERWISE: 'otherwise';

OPEN_BRACE: '{';
CLOSE_BRACE: '}';
LPAREN: '(';
RPAREN: ')';
OPEN_SQ: '[';
CLOSE_SQ: ']';
COMMA: ',';
QUEST: '?';
COLON: ':';

NOT: '!';
AND: 'and'|'&&';
OR: 'or'|'||';
GT: '>';
GTE: '>=';
LT: '<';
LTE: '<=';
EQ: '==';
NEQ: '!=';
IN: 'in';
MATCHES: 'matches'|'~';
BETWEEN: 'between';
ASSIGN: '=';

PLUS: '+';
MINUS: '-';
MULTIPLY: '*';
DIVIDE: '/';
POW: '^';

SPREAD: '...';

DOT: '.';

TRUE: 'true';
FALSE: 'false';
NULL: 'null';

WS: [ \t\r\n]+ -> skip;
LEGACY_DATETIME: ('T(System.DateTime)'|'T(DateTime)');
LEGACY_MATH: 'T(System.Math)';
STRING_LITERAL: '\'' (APOS | ~'\'')* '\'';
TIME_LITERAL: ('-'? DIGIT+ '.')? TIME;
LONG_LITERAL: (DIGIT)+ LONG_SUFFIX;
INT_LITERAL: (DIGIT)+;
FLOAT_LITERAL: NUMBER ('f'|'F');
DATE_LITERAL: '@' TDG TDG '-' TDG '-' TDG ('T' TIME_LITERAL (TZ)?)?;
fragment TZ: 'Z' | (('+'|'-') TDG (':'?) TDG?);
fragment TIME_SPECIFIER: 'T';
fragment TDG: DIGIT DIGIT;
fragment TIME: TDG ':' TDG ':' TDG (DOT TDG DIGIT)?;

HASH: '#';
BAR: '|';
DOLLAR: '$';

fragment APOS: '\'' '\'';
fragment LONG_SUFFIX: ( 'L' | 'l');
fragment HEX_DIGIT: [0-9A-Fa-f];

fragment DIGIT: [0-9];
NUMBER: DIGIT+ ([.] DIGIT+)? | '.' DIGIT+;
ID: ('a' ..'z' | 'A' ..'Z' | '_' ) (
		'a' ..'z'
		| 'A' ..'Z'
		| '_'
		| '0' ..'9'
	)*;