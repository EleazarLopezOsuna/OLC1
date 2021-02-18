/* Importaciones */

/* Analisis Lexico */
%lex
%%

/* AREA DE IGNORAR */
(\s|\t|\r)                         /**/
\/\/.*                                /**/
\/\*[^\*]*\*\/                        /**/

/* AREA DE PALABRAS RESERVADAS */
"class"                               return 'CLASS'
"import"                              return 'IMPORT'
"int"                                 return 'INT'
"double"                              return 'DOUBLE'
"boolean"                             return 'BOOLEAN'
"char"                                return 'CHAR'
"String"                              return 'STRING'
"if"                                  return 'IF'
"else"                                return 'ELSE'
"switch"                              return 'SWITCH'
"case"                                return 'CASE'
"default"                             return 'DEFAULT'
"break"                               return 'BREAK'
"continue"                            return 'CONTINUE'
"while"                               return 'WHILE'
"do"                                  return 'DO'
"for"                                 return 'FOR'
"while"                               return 'WHILE'
"void"                                return 'VOID'
"main"                                return 'MAIN'
"System"                              return 'SYSTEM'
"out"                                 return 'OUT'
"println"                             return 'PRINTLN'
"return"                              return 'RETURN'

/* AREA DE EXPRESIONES REGULARES */
[0-9]+("."[0-9]+)                     return 'DOBLE'
[a-zA-Z_][a-zA-Z0-9_]*                return 'IDENTIFICADOR'
[0-9]+                                return 'ENTERO'
"false"|"true"                        return 'BOLEANO'
\"[^\"]*\"                            return 'CADENA'
\'[^\'']\'                            return 'CARACTER'

/* AREA DE SIMBOLOS */
"++"                                  return '++'
"+"                                   return '+'
"--"                                  return '--'
"-"                                   return '-'
"*"                                   return '*'
"/"                                   return '/'
"^"                                   return '^'
"%"                                   return '%'
"=="                                  return '=='
"="                                   return '='
">="                                  return '>='
">"                                   return '>'
"<="                                  return '<='
"<"                                   return '<'
"&&"                                  return '&&'
"||"                                  return '||'
"!="                                  return '!='
"!"                                   return '!'
"{"                                   return '{'
"}"                                   return '}'
";"                                   return ';'
"("                                   return '('
")"                                   return ')'
":"                                   return ':'
"."                                   return '.'
","                                   return ','

/* AREA DE FIN DE CADENA */
<<EOF>>               return 'EOF'

/* AREA DE ERRORES */
.                     return 'INVALID'

/lex

/* PRECEDENCIA */
%left '||'
%left '&&'
%nonassoc '==' '!='
%nonassoc '<' '<=' '>' '>='
%left '+' '-'
%left '*' '/' '%'
%left '^'
%right '!'
%right '++' '--'
%left UMINUS

%start inicio

%% 
/* GRAMATICA */

inicio
    : l_ins_p EOF { return $1 }
    ;

l_ins_p
    : l_ins_p ins_p { $$ = $1 + "" + $2 }
    | ins_p {$$ = $1}
    ;

l_ins_s
    : l_ins_s ins_s { $$ = $1 + "" + $2 }
    | ins_s {$$ = $1}
    ;

l_ins_t
    : l_ins_t ins_t { $$ = $1 + "" + $2 }
    | ins_t {$$ = $1}
    ;

ins_p
    : func_crear_clase {$$ = $1}
    | func_import {$$ = $1}
    | error {$$ = "E-S-" + yytext + "-" + @1.first_line + "-" + @1.first_column + "\n"}
    ;

ins_t
    : func_llamar_metodo_funcion {$$ = $1}
    |func_declarar {$$ = $1}
    | func_if {$$ = $1}
    | func_switch {$$ = $1}
    | func_while {$$ = $1}
    | func_do {$$ = $1}
    | func_print {$$ = $1}
    | func_for {$$ = $1}
    | func_crear_metodo {$$ = $1}
    | func_reasignar {$$ = $1}
    | func_break_continue  {$$ = $1}
    | IDENTIFICADOR '++' ';' {$$ = ""}
    | IDENTIFICADOR '--' ';' {$$ = ""}
    | RETURN e ';' {$$ = ""}
    | error {$$ = "E-S-" + yytext + "-" + @1.first_line + "-" + @1.first_column + "\n";}
    ;

ins_s
    : func_llamar_metodo_funcion {$$ = $1}
    |func_declarar {$$ = $1}
    | func_if  {$$ = $1}
    | func_switch  {$$ = $1}
    | func_while  {$$ = $1}
    | func_do  {$$ = $1}
    | func_print  {$$ = $1}
    | func_for {$$ = $1}
    | func_reasignar {$$ = $1}
    | func_break_continue  {$$ = $1}
    | IDENTIFICADOR '++' ';' {$$ = ""}
    | IDENTIFICADOR '--' ';' {$$ = ""}
    | RETURN e ';' {$$ = ""}
    | error {$$ = "E-S-" + yytext + "-" + @1.first_line + "-" + @1.first_column + "\n";}
    ;

func_llamar_metodo_funcion
    : IDENTIFICADOR '(' lista_parametros ')' ';' {$$ = $3}
    ;
  
lista_parametros
    : lista_parametros ',' e {$$ = $1 + "" + $3}
    | e {$$ = $1}
    | error {$$ = "E-S-" + yytext + "-" + @1.first_line + "-" + @1.first_column + "\n";}
    ;

func_declarar
    : tipo lista_identificador tipo_declaracion  {$$ = $1 + "" + $2 + $3}
    | tipo IDENTIFICADOR '(' parametros ')' '{' l_ins_s '}'  {$$ = $1 + "" + $4}
    ;

lista_identificador
    : lista_identificador ',' IDENTIFICADOR  {$$ = $1}
    | IDENTIFICADOR  {$$ = ""}
    | error {$$ = "E-S-" + yytext + "-" + @1.first_line + "-" + @1.first_column + "\n";}
    ;
  
tipo_declaracion
    :  '=' e ';'  {$$ = $2}
    | ';' { $$ = "" }
    | error {$$ = "E-S-" + yytext + "-" + @1.first_line + "-" + @1.first_column + "\n";}
    ;

parametros
    : parametros ',' t_param  {$$ = $1 + "" + $3}
    | t_param  {$$ = $1}
    | error {$$ = "E-S-" + yytext + "-" + @1.first_line + "-" + @1.first_column + "\n";}
    | { $$ = "" }
    ;

t_param
    : tipo IDENTIFICADOR  {$$ = $1}
    ;

tipo
    : INT  {$$ = ""}
    | DOUBLE  {$$ = ""}
    | BOOLEAN  {$$ = ""}
    | CHAR {$$ = ""}
    | STRING {$$ = ""}
    ;

e
    : IDENTIFICADOR '(' lista_parametros ')'  {$$ = $3}
    | IDENTIFICADOR '(' ')' {$$ = ""}
    | e '+' e {$$ = $1 + "" + $3}
    | e '-' e {$$ = $1 + "" + $3}
    | e '*' e {$$ = $1 + "" + $3}
    | e '/' e {$$ = $1 + "" + $3}
    | e '%' e {$$ = $1 + "" + $3}
    | e '^' e {$$ = $1 + "" + $3}
    | '-' e %prec UMINUS {$$ = $2}
    | e '++' {$$ = $1}
    | e '--' {$$ = $1}
    | e '==' e {$$ = $1 + "" + $3}
    | e '!=' e {$$ = $1 + "" + $3}
    | e '>' e {$$ = $1 + "" + $3}
    | e '<' e {$$ = $1 + "" + $3}
    | e '>=' e {$$ = $1 + "" + $3}
    | e '<=' e {$$ = $1 + "" + $3}
    | e '&&' e {$$ = $1 + "" + $3}
    | e '||' e {$$ = $1 + "" + $3}
    | '!' e {$$ = $2}
    | '(' e ')' {$$ = $2}
    | dato {$$ = $1}
    ;

dato
    : ENTERO {$$ = ""}
    | CADENA {$$ = ""}
    | IDENTIFICADOR {$$ = ""}
    | BOLEANO {$$ = ""}
    | CARACTER {$$ = ""}
    | DOBLE {$$ = ""}
    ;

func_if
    : IF '(' e ')' '{' l_ins_s '}' ver_else {$$ = $3 + $6 + $8}
    | IF '(' e ')' '{' l_ins_s '}' {$$ = $3 + $6}
    ;

ver_else
    : ELSE func_if  {$$ = $2}
    | ELSE '{' l_ins_s '}'  {$$ = $3}
    ;

func_switch
    : SWITCH '(' e ')' '{' lista_cases_default '}' {$$ = $3 + $6}
    ;

lista_cases_default
    : lista_cases_default t_case {$$ = $1 + "" + $2}
    | t_case {$$ = $1}
    | error {$$ = "E-S-" + yytext + "-" + @1.first_line + "-" + @1.first_column + "\n";}
    ;

t_case
    : CASE dato ':' l_ins_s {$$ = $2 + $4}
    | DEFAULT ':' l_ins_s {$$ = $3}
    ;

func_while
    : WHILE '(' e ')' '{' l_ins_s '}' {$$ = $3 + $6}
    ;

func_do
    : DO '{' l_ins_s '}' WHILE '(' e ')' ';' {$$ = $3 + $7}
    ;

func_print
    : SYSTEM '.' OUT '.' PRINTLN '(' e ')' ';' {$$ = $7}
    ;

func_for
    : FOR '(' for_inicio ';' e ';' IDENTIFICADOR '++' ')' '{' ins_s '}' {$$ = $3 + $5 + $11}
    | FOR '(' for_inicio ';' e ';' IDENTIFICADOR '--' ')' '{' ins_s '}' {$$ = $3 + $5 + $11}
    ;

for_inicio
    : tipo IDENTIFICADOR '=' e { $$ = $1 + "" + $4 }
    | IDENTIFICADOR '=' e { $$ = $3 }
    | error {$$ = "E-S-" + yytext + "-" + @1.first_line + "-" + @1.first_column + "\n";}
    ;

func_crear_clase
    : CLASS IDENTIFICADOR '{' l_ins_t '}' { $$ = $4 }
    ;

func_crear_metodo
    : VOID tipo_metodo { $$ = $2 }
    ;

tipo_metodo
    : IDENTIFICADOR '(' parametros ')' '{' l_ins_s '}' { $$ = $6 }
    | MAIN '(' ')' '{' l_ins_s '}' { $$ = $5 }
    | error {$$ = "E-S-" + yytext + "-" + @1.first_line + "-" + @1.first_column + "\n";}
    ;

func_import
    : IMPORT IDENTIFICADOR ';' { $$ = "" }
    ;

func_reasignar
    : IDENTIFICADOR '=' e ';' { $$ = $3 }
    ;

func_break_continue
    : BREAK ';' { $$ = "" }
    | CONTINUE ';' { $$ = "" }
    ;