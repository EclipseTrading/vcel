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

guardExpr
	: MATCH (matcher=ID)? (varName=ID)?
	     (BAR booleanExpr ASSIGN arithExpr)+ 
		 (BAR otherwiseExpr arithExpr)?
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
	: booleanOpExpr IN expr #InOp 
	| booleanOpExpr MATCHES expr #Matches
	| booleanOpExpr BETWEEN betweenArgs #Between
	| arithExpr #Arith
	;

arithExpr
	: MINUS arithExpr #UnaryMinus
	| '(' expr ')' #Paren
	| arithExpr DOT expr #MemberExpr
	| arithExpr MULTIPLY arithExpr # Mult
	| arithExpr DIVIDE arithExpr # Div	
	| arithExpr PLUS arithExpr # Plus
	| arithExpr MINUS arithExpr # Minus
	| arithExpr POW arithExpr # Pow
	| functionExpr # FuncExpr
	| listExpr #List
	| term #ExprListTerm
	| legacyNode #LegacyNodeExpr
	;


betweenArgs
	: (OPEN_BRACE expr COMMA expr CLOSE_BRACE)
	;

objExpr
	: '(' expr ')'
	| var
	;

argList: '(' expr? (COMMA expr)* ')';

listExpr: (OPEN_BRACE expr? (COMMA expr)* CLOSE_BRACE);

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