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
    : l_ins_p EOF {return $1}
    ;

l_ins_p
    : l_ins_p ins_p {$1.push($2); $$ = $1}
    | ins_p {$$ = [$1]}
    ;

l_ins_s
    : l_ins_s ins_s {$1.push($2); $$ = $1}
    | ins_s {$$ = [$1]}
    ;

l_ins_t
    : l_ins_t ins_t {$1.push($2); $$ = $1}
    | ins_t {$$ = [$1]}
    ;

ins_p
    : func_crear_clase -> {text: 'clase', children: $1}
    | func_import -> {text: 'importar', children: [{text: $1}]}
    ;

ins_t
    : func_llamar_metodo_funcion -> {text: 'invocacion', children: $1}
    |func_declarar -> {text: 'declarar', children: $1}
    | func_if -> {text: 'if', children: $1}
    | func_switch -> {text: 'switch', children: $1}
    | func_while -> {text: 'while', children: $1}
    | func_do -> {text: 'do', children: $1}
    | func_print -> {text: 'imprimir', children: [{text: $1}]}
    | func_for -> {text: 'for', children: $1}
    | func_crear_metodo -> {text: 'metodo', children: $1}
    | func_reasignar -> {text: 'reasignar', children: $1}
    | func_break_continue {$$ = $1}
    | IDENTIFICADOR '++' ';' -> {text: 'incremento', children: [{text: $1}]}
    | IDENTIFICADOR '--' ';' -> {text: 'decremento', children: [{text: $1}]}
    | RETURN e ';' -> {text: 'return', children: [$2]}
    ;

ins_s
    : func_llamar_metodo_funcion -> {text: 'invocacion', children: $1}
    | func_declarar -> {text: 'declarar', children: $1}
    | func_if -> {text: 'if', children: $1}
    | func_switch -> {text: 'switch', children: $1}
    | func_while -> {text: 'while', children: $1}
    | func_do -> {text: 'do', children: $1}
    | func_print -> {text: 'imprimir', children: [{text: $1}]}
    | func_for -> {text: 'for', children: $1}
    | func_reasignar -> {text: 'reasignar', children: $1}
    | func_break_continue {$$ = $1}
    | IDENTIFICADOR '++' ';' -> {text: 'incremento', children: [{text: $1}]}
    | IDENTIFICADOR '--' ';' -> {text: 'decremento', children: [{text: $1}]}
    | RETURN e ';' -> {text: 'return', children: [$2]}
    ;

func_llamar_metodo_funcion
    : IDENTIFICADOR '(' lista_parametros ')' ';' -> [{text: 'nombre', children: [{text: $1}]}, {text: 'parametros', children: $3}]
    ;
  
lista_parametros
    : lista_parametros ',' e {$1.push({text: $3}); $$ = $1}
    | e -> [{text: $1}]
    ;

func_declarar
    : tipo lista_identificador tipo_declaracion -> [{text: 'tipo', children: [$1]}, {text: 'variables', children: $2}, {text: 'valor', children: [$3]}]
    | tipo IDENTIFICADOR '(' parametros ')' '{' l_ins_s '}' -> [{text: 'funcion', children: [{text: 'nombre', children: [{text: $2}]}, {text: 'parametros', children: $4}, {text: 'instrucciones', children: $7}]}]
    ;

lista_identificador
    : lista_identificador ',' IDENTIFICADOR {$1.push({text: $3}); $$ = $1}
    | IDENTIFICADOR -> [{text: $1}]
    ;
  
tipo_declaracion
    :  '=' e ';' {$$ = $2}
    | ';' -> {text: 'nulo'}
    ;

parametros
    : parametros ',' t_param {$1.push($3); $$ = $1}
    | t_param -> [$1]
    | -> {text: 'nulo'}
    ;

t_param
    : tipo IDENTIFICADOR -> {text: 'parametro', children: [{text: 'tipo', children: [{text: $1}]}, {text: 'nombre', children: [{text: $2}]}]}
    ;

tipo
    : INT {$$ = $1}
    | DOUBLE {$$ = $1}
    | BOOLEAN {$$ = $1}
    | CHAR {$$ = $1}
    | STRING {$$ = $1}
    ;

e
    : IDENTIFICADOR '(' lista_parametros ')' -> {text: 'funcion', children: [{text: 'nombre', children: [{text: $1}]}, {text: 'parametros', children: $3}]}
    | IDENTIFICADOR '(' ')' -> {text: 'funcion', children: [{text: 'nombre', children: [{text: $1}]}]}
    | e '+' e {$$ = $1 + '+' + $3}
    | e '-' e {$$ = $1 + '-' + $3}
    | e '*' e {$$ = $1 + '*' + $3}
    | e '/' e {$$ = $1 + '/' + $3}
    | e '%' e {$$ = $1 + '%' + $3}
    | e '^' e {$$ = $1 + '^' + $3}
    | '-' e %prec UMINUS {$$ = '-' + $2}
    | e '++' {$$ = $1 + '++'}
    | e '--' {$$ = $1 + '--'}
    | e '==' e {$$ = $1 + '==' + $3}
    | e '!=' e {$$ = $1 + '!=' + $3}
    | e '>' e {$$ = $1 + '>' + $3}
    | e '<' e {$$ = $1 + '<' + $3}
    | e '>=' e {$$ = $1 + '>=' + $3}
    | e '<=' e {$$ = $1 + '<=' + $3}
    | e '&&' e {$$ = $1 + '&&' + $3}
    | e '||' e {$$ = $1 + '||' + $3}
    | '!' e {$$ = '!' + $2}
    | '(' e ')' {$$ = '(' + $2 + ')'}
    | dato {$$ = $dato}
    ;

dato
    : ENTERO {$$ = $1}
    | CADENA {$$ = $1.replace(/\"/g, "")}
    | IDENTIFICADOR {$$ = $1}
    | BOLEANO {$$ = $1}
    | CARACTER {$$ = $1.replace(/\'/g, "")}
    | DOBLE {$$ = $1}
    ;

func_if
    : IF '(' e ')' '{' l_ins_s '}' ver_else  -> [{text: 'condicion', children: [{text: $3}]}, {text: 'instrucciones', children: $6}, {text: 'else', children: $8}]
    | IF '(' e ')' '{' l_ins_s '}'  -> [{text: 'condicion', children: [{text: $3}]}, {text: 'instrucciones', children: $6}]
    ;

ver_else
    : ELSE func_if {$$ = $2}
    | ELSE '{' l_ins_s '}' -> [{text: 'instrucciones', children: $3}]
    ;

func_switch
    : SWITCH '(' e ')' '{' lista_cases_default '}' -> [{text: 'dato', children: [{text: $3}]}, {text: 'cases', children: $6}]
    ;

lista_cases_default
    : lista_cases_default t_case {$1.push($2); $$ = $1}
    | t_case -> [$1]
    ;

t_case
    : CASE dato ':' l_ins_s -> {text: 'case', children: [{text: 'nombre', children: [{text: $2}]}, {text: 'instrucciones', children: $4}]}
    | DEFAULT ':' l_ins_s -> {text: 'case', children: [{text: 'default'}, {text: 'instrucciones', children: $3}]}
    ;

func_while
    : WHILE '(' e ')' '{' l_ins_s '}' -> [{text: 'condicion', children: [{text: $3}]}, {text: 'instrucciones', children: $6}]
    ;

func_do
    : DO '{' l_ins_s '}' WHILE '(' e ')' ';' -> [{text: 'instrucciones', children: $3}, {text: 'condicion', children: [{text: $7}]}]
    ;

func_print
    : SYSTEM '.' OUT '.' PRINTLN '(' e ')' ';' {$$ = $7;}
    ;

func_for
    : FOR '(' for_inicio ';' e ';' IDENTIFICADOR '++' ')' '{' ins_s '}' -> [{text: 'iniciacion', children: $3}, {text: 'condicion', children: [{text: $5}]}, {text: 'instrucciones', children: [$11]}, {text: 'update', children: [{text: 'incremento', children: [{text: $7}]}]}]
    | FOR '(' for_inicio ';' e ';' IDENTIFICADOR '--' ')' '{' ins_s '}' -> [{text: 'iniciacion', children: $3}, {text: 'condicion', children: [{text: $5}]}, {text: 'instrucciones', children: [$11]}, {text: 'update', children: [{text: 'decremento', children: [{text: $7}]}]}]
    ;

for_inicio
    : tipo IDENTIFICADOR '=' e -> [{text: 'nombre', children: [{text: $2}]}, {text: 'valor', children: [{text: $4}]}]
    | IDENTIFICADOR '=' e -> [{text: 'nombre', children: [{text: $1}]}, {text: 'valor', children: [{text: $3}]}]
    ;

func_crear_clase
    : CLASS IDENTIFICADOR '{' l_ins_t '}' -> [{text: 'nombre', children: [{text: $2}]}, {text: 'instrucciones', children: $4}]
    ;

func_crear_metodo
    : VOID tipo_metodo {$$ = $2}
    ;

tipo_metodo
    : IDENTIFICADOR '(' parametros ')' '{' l_ins_s '}' -> [{text: 'nombre', children: [{text: $1}]}, {text: 'parametros', children: $3}, {text: 'instrucciones', children: $6}]
    | MAIN '(' ')' '{' l_ins_s '}' -> [{text: 'nombre', children: [{text: $1}]}, {text: 'instrucciones', children: $5}]
    ;

func_import
    : IMPORT IDENTIFICADOR ';' {$$ = $2}
    ;

func_reasignar
    : IDENTIFICADOR '=' e ';' -> [{text: 'variable', children: [{text: $1}]}, {text: 'valor', children: [{text: $3}]}]
    ;

func_break_continue
    : BREAK ';' -> {text: $1}
    | CONTINUE ';' -> {text: $1}
    ;