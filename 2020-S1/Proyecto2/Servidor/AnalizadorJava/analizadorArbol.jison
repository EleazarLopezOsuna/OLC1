/* Importaciones */
%{
  function Nodo(nombre, valor, hijos, linea, columna) {
    this.nombre = nombre
    this.valor = valor
    this.hijos = hijos
    this.linea = linea
    this.columna = columna
  }
%}
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
    : l_ins_p EOF { $$ = new Nodo('Raiz', 'Raiz', [$1], @1.first_line, @1.first_column); return $$;}
    ;

l_ins_p
    : l_ins_p ins_p {$1.hijos.push($2); $$ = $1 }
    | ins_p { $$ = new Nodo('Instruccion', 'Instruccion', [$1], @1.first_line, @1.first_column)}
    ;

l_ins_s
    : l_ins_s ins_s {$1.hijos.push($2); $$ = $1 }
    | ins_s { $$ = new Nodo('Instruccion', 'Instruccion', [$1], @1.first_line, @1.first_column)}
    ;

l_ins_t
    : l_ins_t ins_t {$1.hijos.push($2); $$ = $1 }
    | ins_t { $$ = new Nodo('Instruccion', 'Instruccion', [$1], @1.first_line, @1.first_column)}
    ;

ins_p
    : func_crear_clase { $$ = $1 }
    | func_import { $$ = $1 }
    ;

ins_t
    : func_llamar_metodo_funcion { $$ = $1 }
    | func_declarar { $$ = $1 }
    | func_if { $$ = $1 }
    | func_switch { $$ = $1 }
    | func_while { $$ = $1 }
    | func_do { $$ = $1 }
    | func_print { $$ = $1 }
    | func_for { $$ = $1 }
    | func_crear_metodo { $$ = $1 }
    | func_reasignar { $$ = $1 }
    | func_break_continue { $$ = $1 }
    | IDENTIFICADOR '++' ';' { $$ = new Nodo('Incremento', $1, [], @1.first_line, @1.first_column) }
    | IDENTIFICADOR '--' ';' { $$ = new Nodo('Decremento', $1, [], @1.first_line, @1.first_column) }
    | RETURN e ';' { $$ = new Nodo('Return', 'Return', [$2], @1.first_line, @1.first_column) }
    ;

ins_s
    : func_llamar_metodo_funcion { $$ = $1 }
    | func_declarar { $$ = $1 }
    | func_if { $$ = $1 }
    | func_switch { $$ = $1 }
    | func_while { $$ = $1 }
    | func_do  { $$ = $1 }
    | func_print  { $$ = $1 }
    | func_for { $$ = $1 }
    | func_reasignar { $$ = $1 }
    | func_break_continue { $$ = $1 }
    | IDENTIFICADOR '++' ';' { $$ = new Nodo('Incremento', $1, [], @1.first_line, @1.first_column) }
    | IDENTIFICADOR '--' ';' { $$ = new Nodo('Decremento', $1, [], @1.first_line, @1.first_column) }
    | RETURN e ';' { $$ = new Nodo('Return', 'Return', [$2], @1.first_line, @1.first_column) }
    ;

func_llamar_metodo_funcion
    : IDENTIFICADOR '(' lista_parametros ')' ';' { $$ = new Nodo('Llamada', $1, [$3], @1.first_line, @1.first_column) }
    ;
  
lista_parametros
    : lista_parametros ',' e { $1.hijos.push($3); $$ = $1 }
    | e { $$ = new Nodo('Enviar', 'Enviar', [$1], @1.first_line, @1.first_column) }
    ;

func_declarar
    : tipo lista_identificador tipo_declaracion { $$ = new Nodo('Declaracion', 'Declaracion', [$1, $2, $3], @1.first_line, @1.first_column) }
    | tipo IDENTIFICADOR '(' parametros ')' '{' l_ins_s '}' { $$ = new Nodo('Funcion', $2, [$1, $4, $7], @1.first_line, @1.first_column) }
    ;

lista_identificador
    : lista_identificador ',' IDENTIFICADOR { $1.hijos.push($3); $$ = $1}
    | IDENTIFICADOR { $$ = new Nodo('Identificadores', 'Identificadores', [$1], @1.first_line, @1.first_column) }
    ;
  
tipo_declaracion
    :  '=' e ';' { $$ = new Nodo('Valor', $2, [], @1.first_line, @1.first_column) }
    | ';' { $$ = new Nodo('Valor', 'nulo', [], @1.first_line, @1.first_column) }
    ;

parametros
    : parametros ',' t_param {$1.hijos.push($3); $$ = $1 }
    | t_param { $$ = new Nodo('Parametros', 'Si', [$1], @1.first_line, @1.first_column) }
    | { $$ = new Nodo('Parametros', 'No', [], @1.first_line, @1.first_column) }
    ;

t_param
    : tipo IDENTIFICADOR 
    { 
        $$ = new Nodo('Parametro', $1, [new Nodo('Identificador', $2, [], @2.first_line, @2.first_column)], @1.first_line, @1.first_column)
    }
    ;

tipo
    : INT { $$ = 'Int' }
    | DOUBLE { $$ = 'Double'}
    | BOOLEAN { $$ = 'Bolean' }
    | CHAR { $$ = 'Char' }
    | STRING { $$ = 'String' }
    ;

e
    : IDENTIFICADOR '(' lista_parametros ')' { $$ = new Nodo('Valor-Funcion', $1, [$3], @1.first_line, @1.first_column) }
    | IDENTIFICADOR '(' ')' { $$ = new Nodo('Valor-Funcion', $1, [], @1.first_line, @1.first_column) }
    | e '+' e { $$ = new Nodo('Suma', 'Suma', [$1, $3], @1.first_line, @1.first_column) }
    | e '-' e { $$ = new Nodo('Resta', 'Resta', [$1, $3], @1.first_line, @1.first_column) }
    | e '*' e { $$ = new Nodo('Multiplicacion', 'Multiplicacion', [$1, $3], @1.first_line, @1.first_column) }
    | e '/' e { $$ = new Nodo('Division', 'Division', [$1, $3], @1.first_line, @1.first_column) }
    | e '%' e { $$ = new Nodo('Modulo', 'Modulo', [$1, $3], @1.first_line, @1.first_column) }
    | e '^' e { $$ = new Nodo('Potencia', 'Potencia', [$1, $3], @1.first_line, @1.first_column) }
    | '-' e %prec UMINUS  { $$ = new Nodo('Negativo', 'Negativo', [$2], @1.first_line, @1.first_column) }
    | e '++' { $$ = new Nodo('Incremento', 'Incremento', [$1], @1.first_line, @1.first_column) }
    | e '--' { $$ = new Nodo('Decremento', 'Decremento', [$1], @1.first_line, @1.first_column) }
    | e '==' e { $$ = new Nodo('Igual', 'Igual', [$1, $3], @1.first_line, @1.first_column) }
    | e '!=' e { $$ = new Nodo('Diferente', 'Diferente', [$1, $3], @1.first_line, @1.first_column) }
    | e '>' e { $$ = new Nodo('Mayor', 'Mayor', [$1, $3], @1.first_line, @1.first_column) }
    | e '<' e { $$ = new Nodo('Menor', 'Menor', [$1, $3], @1.first_line, @1.first_column) }
    | e '>=' e { $$ = new Nodo('Mayor-Igual', 'Mayor-Igual', [$1, $3], @1.first_line, @1.first_column) }
    | e '<=' e { $$ = new Nodo('Menor-Igual', 'Menor-Igual', [$1, $3], @1.first_line, @1.first_column) }
    | e '&&' e { $$ = new Nodo('And', 'And', [$1, $3], @1.first_line, @1.first_column) }
    | e '||' e { $$ = new Nodo('Or', 'Or', [$1, $3], @1.first_line, @1.first_column) }
    | '!' e { $$ = new Nodo('Negacion', 'Negacion', [$2], @1.first_line, @1.first_column) }
    | '(' e ')' { $$ = new Nodo('Parentesis', 'Parentesis', [$2], @1.first_line, @1.first_column) }
    | dato { $$ = $1 }
    ;

dato
    : ENTERO { $$ = new Nodo('Entero', $1, [], @1.first_line, @1.first_column) }
    | CADENA { $$ = new Nodo('cadena', $1, [], @1.first_line, @1.first_column) }
    | IDENTIFICADOR { $$ = new Nodo('Identificador', $1, [], @1.first_line, @1.first_column) }
    | BOLEANO { $$ = new Nodo('Boleano', $1, [], @1.first_line, @1.first_column) }
    | CARACTER { $$ = new Nodo('Caracter', $1, [], @1.first_line, @1.first_column) }
    | DOBLE { $$ = new Nodo('Doble', $1, [], @1.first_line, @1.first_column) }
    ;

func_if
    : IF '(' e ')' '{' l_ins_s '}' ver_else { $$ = new Nodo('If', 'If', [$3, $6, $8], @1.first_line, @1.first_column) }
    | IF '(' e ')' '{' l_ins_s '}' { $$ = new Nodo('If', 'If', [$3, $6], @1.first_line, @1.first_column) }
    ;

ver_else
    : ELSE func_if { $$ = new Nodo('Else-If', 'Else-If', [$2], @1.first_line, @1.first_column) }
    | ELSE '{' l_ins_s '}' { $$ = new Nodo('Else', 'Else', [$3], @1.first_line, @1.first_column) }
    ;

func_switch
    : SWITCH '(' e ')' '{' lista_cases_default '}'  { $$ = new Nodo('Switch', 'Switch', [$3, $6], @1.first_line, @1.first_column) }
    ;

lista_cases_default
    : lista_cases_default t_case { $1.hijos.push($2); $$ = $1 }
    | t_case  { $$ = new Nodo('Cases', 'Cases', [$1], @1.first_line, @1.first_column) }
    ;

t_case
    : CASE dato ':' l_ins_s  { $$ = new Nodo('Case', 'Case', [$2, $4], @1.first_line, @1.first_column) }
    | DEFAULT ':' l_ins_s  { $$ = new Nodo('Default', 'Default', [$3], @1.first_line, @1.first_column) }
    ;

func_while
    : WHILE '(' e ')' '{' l_ins_s '}'  { $$ = new Nodo('While', 'While', [$3, $6], @1.first_line, @1.first_column) }
    ;

func_do
    : DO '{' l_ins_s '}' WHILE '(' e ')' ';' { $$ = new Nodo('Do', 'Do', [$3, $7], @1.first_line, @1.first_column) }
    ;

func_print
    : SYSTEM '.' OUT '.' PRINTLN '(' e ')' ';' { $$ = new Nodo('Imprimir', 'Imprimir', [$7], @1.first_line, @1.first_column) }
    ;

func_for
    : FOR '(' for_inicio ';' e ';' IDENTIFICADOR '++' ')' '{' ins_s '}'
    { 
        var iden = new Nodo('Incremento', $7, [], @1.first_line, @1.first_column)
        $$ = new Nodo('For', 'For', [$3, $5, iden, $11], @1.first_line, @1.first_column) 
    }
    | FOR '(' for_inicio ';' e ';' IDENTIFICADOR '--' ')' '{' ins_s '}'
    { 
        var iden = new Nodo('Decremento', $7, [], @1.first_line, @1.first_column)
        $$ = new Nodo('For', 'For', [$3, $5, iden, $11], @1.first_line, @1.first_column) 
    }
    ;

for_inicio
    : tipo IDENTIFICADOR '=' e
    { 
        $$ = new Nodo('Identificador-For', $1, [$4], @1.first_line, @1.first_column)
    }
    | IDENTIFICADOR '=' e 
    { 
        $$ = new Nodo('Identificador-For', $1, [$3], @1.first_line, @1.first_column)
    }
    ;

func_crear_clase
    : CLASS IDENTIFICADOR '{' l_ins_t '}' { $$ = new Nodo('Clase', $2, [$4], @1.first_line, @1.first_column) }
    ;

func_crear_metodo
    : VOID tipo_metodo { $$ = $2 }
    ;

tipo_metodo
    : IDENTIFICADOR '(' parametros ')' '{' l_ins_s '}' { $$ = new Nodo('Metodo', $1, [$3, $6], @1.first_line, @1.first_column) }
    | MAIN '(' ')' '{' l_ins_s '}' { $$ = new Nodo('Metodo', 'Main', [$5], @1.first_line, @1.first_column) }
    ;

func_import
    : IMPORT IDENTIFICADOR ';'
    { 
      $$ = new Nodo('Importar', $2, [], @1.first_line, @1.first_column)
    }
    ;

func_reasignar
    : IDENTIFICADOR '=' e ';' 
    { 
      var valor = new Nodo('Valor', $3, [], @1.first_line, @1.first_column)
      var variable = new Nodo('Variable', $1, [], @1.first_line, @1.first_column)
      var asignar = new Nodo('Reasignar', 'Reasignar', [variable, valor], @1.first_line, @1.first_column)
      $$ = asignar
    }
    ;

func_break_continue
    : BREAK ';' { $$ = new Nodo('Break', 'Break', [], @1.first_line, @1.first_column) }
    | CONTINUE ';' { $$ = new Nodo('Continue', 'Continue', [], @1.first_line, @1.first_column) }
    ;