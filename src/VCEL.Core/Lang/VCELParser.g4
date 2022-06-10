parser grammar VCELParser;

options {
	tokenVocab = VCELLexer;
}

expression
	: letexpr EOF
	| expr EOF
	;

letexpr
	: LET assignExpr (COMMA assignExpr)* IN expr
	;

assignExpr
	: ID ASSIGN expr
	;

expr
	: guardExpr #Guard
	| <assoc=right>expr '?' expr ':' expr #Ternary
	| booleanExpr #BoolExpr
	;

guardClause: BAR test=booleanExpr ASSIGN assign=arithExpr;

guardExpr
	: MATCH (matcher=ID)? (varName=ID)? 
		guardClause+
		(BAR otherwiseExpr otherwise=arithExpr)?
	;

otherwiseExpr: OTHERWISE;

booleanExpr
	: '!' booleanExpr #Not
	| booleanExpr AND booleanExpr #And
	| booleanExpr OR booleanExpr #Or
	| equalityExpr #Equality
	;

equalityExpr
	: equalityExpr EQ equalityExpr #Eq
	| equalityExpr NEQ equalityExpr #Neq
	| equalityExpr GT equalityExpr #GT
	| equalityExpr GTE equalityExpr #GTE
	| equalityExpr LT equalityExpr #LT
	| equalityExpr LTE equalityExpr #LTE
	| booleanOpExpr #booleanOp
	;

booleanOpExpr
	: left=booleanOpExpr IN right=arithExpr #InOp
	| booleanOpExpr MATCHES stringLiteral #Matches
	| booleanOpExpr MATCHES arithExpr #Matches
	| left=booleanOpExpr BETWEEN betweenArgs #Between
	| arithExpr #Arith
	;

arithExpr
	: MINUS arithExpr #UnaryMinus
	| '(' expr ')' #Paren
	| arithExpr DOT var #MemberExpr
	| arithExpr DOT functionExpr #MemberExpr
	| arithExpr DOT arithExpr #MemberExpr
	| <assoc=right>arithExpr POW arithExpr # Pow
	| arithExpr op=(MULTIPLY | DIVIDE) arithExpr # MultDiv
	| arithExpr op=(PLUS | MINUS) arithExpr # PlusMinus
	| functionExpr # FuncExpr
	| term #ExprListTerm
	| setLiteral #ExprSetLiteral
	| list #ExprList
	| legacyNode DOT var #LegacyNodeExpr
	| legacyNode DOT functionExpr #LegacyNodeExpr
	;

listItem
	: arithExpr      
	| SPREAD arithExpr
	;

betweenArgs
	: (OPEN_BRACE lower=arithExpr COMMA upper=arithExpr CLOSE_BRACE)
	;

objExpr
	: '(' expr ')'
	| var
	;

argList: '(' expr? (COMMA expr)* ')';

legacyNode
	: LEGACY_MATH
	| LEGACY_DATETIME
	;

functionExpr: ID '(' expr? (COMMA expr)* ')';

term
	: literal
	| var;

var
	: property
	| variable;

property: ID;
variable: HASH ID;

setLiteral: (OPEN_BRACE literal? (COMMA literal)* CLOSE_BRACE);

list: (OPEN_SQ listItem? (COMMA listItem)* CLOSE_SQ);


dateLiteral
	: timeLiteral
	| dateTimeLiteral
	;

literal
	: integerLiteral
	| longLiteral
	| stringLiteral
	| doubleLiteral
	| floatLiteral
	| boolLiteral
	| dateLiteral
	| nullLiteral;

boolLiteral: TRUE | FALSE;
integerLiteral: INT_LITERAL;
longLiteral: LONG_LITERAL;
floatLiteral: FLOAT_LITERAL;
doubleLiteral: NUMBER;
stringLiteral: STRING_LITERAL;
timeLiteral: TIME_LITERAL;
dateTimeLiteral: DATE_LITERAL;
nullLiteral: NULL;