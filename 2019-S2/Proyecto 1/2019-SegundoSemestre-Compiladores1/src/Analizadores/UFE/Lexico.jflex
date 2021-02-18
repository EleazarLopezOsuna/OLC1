
/*--------- 1ra Area: Codigo de Usuario ---------*/

//--------> Paquetes e importaciones
package Analizadores.UFE;
import java_cup.runtime.*;
import Estructuras.Cola;
import Modelos.Errores;

/*--------- 2da Area: Opciones y Declaraciones ---------*/

%%
%{
    //--------> Codigo de Usuario en sintaxis Java
    public Cola ErroresLexicos = new Cola();

%}

//--------> Directivas
%public
%class Analisis_Lexico
%cupsym Simbolos
%cup
%char
%column
%full
%ignorecase
%line
%unicode
%8bit

//--------> Expresiones regulares
entero                  = [0-9]+
doble                   = [0-9]+(\.[0-9]+)
cadena                  = [\"][^\"]*[\"]
booleano                = ("true"|"false")
caracter                = "'"(.)"'"
identificador           = [a-zA-Z]([a-zA-Z]|[0-9|"_"])*
comentarioLinea         = "//".*
comentarioMultiLinea    = "/*"( [^*] | (\*+[^*/]) )*\*+\/
hexadecimal             = "#"[0-9a-fA-F]{6}
texto                   = ([^("{" | "<")] |\t|\r|\n|\f|\s|\v)*
otros                   = [\t|\r|\n|\f|\s]*

//--------> Estados
%state TEXTO, ETIQUETA
%%

/*--------- 3ra Area: Reglas Lexicas ---------*/

//--------> Simbolos

<YYINITIAL> "*"                      { return new Symbol(Simbolos.multiplicacion, yycolumn, yyline, yytext()); }
<YYINITIAL> "/"                      { return new Symbol(Simbolos.division, yycolumn, yyline, yytext()); }
<YYINITIAL> "-"                      { return new Symbol(Simbolos.resta, yycolumn, yyline, yytext()); }
<YYINITIAL> "+"                      { return new Symbol(Simbolos.suma, yycolumn, yyline, yytext()); }
<YYINITIAL> "pow"                    { return new Symbol(Simbolos.potencia, yycolumn, yyline, yytext()); }
<YYINITIAL> "="                      { return new Symbol(Simbolos.igual, yycolumn, yyline, yytext()); }


<YYINITIAL> "=="                      { return new Symbol(Simbolos.igualigual, yycolumn, yyline, yytext()); }
<YYINITIAL> "!"                      { return new Symbol(Simbolos.not, yycolumn, yyline, yytext()); }
<YYINITIAL> "!="                      { return new Symbol(Simbolos.diferente, yycolumn, yyline, yytext()); }
<YYINITIAL> "<="                      { return new Symbol(Simbolos.menorIgual, yycolumn, yyline, yytext()); }
<YYINITIAL> ">="                      { return new Symbol(Simbolos.mayorIgual, yycolumn, yyline, yytext()); }
<YYINITIAL> "<"                      { return new Symbol(Simbolos.menorQue, yycolumn, yyline, yytext()); }
<YYINITIAL> ">"                      { return new Symbol(Simbolos.mayorQue, yycolumn, yyline, yytext()); }

<YYINITIAL> "&&"                     { return new Symbol(Simbolos.logicoAND, yycolumn, yyline, yytext()); }
<YYINITIAL> "OR"                     { return new Symbol(Simbolos.logicoOR, yycolumn, yyline, yytext()); }
<YYINITIAL> "^"                      { return new Symbol(Simbolos.logicoXOR, yycolumn, yyline, yytext()); }

<YYINITIAL> "("                      { return new Symbol(Simbolos.parentesisA, yycolumn, yyline, yytext()); }
<YYINITIAL> ")"                      { return new Symbol(Simbolos.parentesisC, yycolumn, yyline, yytext()); }
<YYINITIAL> "{"                      { return new Symbol(Simbolos.llaveA, yycolumn, yyline, yytext()); }
<YYINITIAL> "}"                      { return new Symbol(Simbolos.llaveC, yycolumn, yyline, yytext()); }
<YYINITIAL> "["                      { return new Symbol(Simbolos.corcheteA, yycolumn, yyline, yytext()); }
<YYINITIAL> "]"                      { return new Symbol(Simbolos.corcheteC, yycolumn, yyline, yytext()); }

<YYINITIAL> ";"                      { return new Symbol(Simbolos.puntoComa, yycolumn, yyline, yytext()); }
<YYINITIAL> ","                      { return new Symbol(Simbolos.coma, yycolumn, yyline, yytext()); }

//--------> Palabras Reservaas

<YYINITIAL> "var"                    { return new Symbol(Simbolos.var, yycolumn, yyline, yytext()); }
<YYINITIAL> "component"              { return new Symbol(Simbolos.component, yycolumn, yyline, yytext()); }
<YYINITIAL> "id"                     { return new Symbol(Simbolos.id, yycolumn, yyline, yytext()); }
<YYINITIAL> "x"                      { return new Symbol(Simbolos.posicionX, yycolumn, yyline, yytext()); }
<YYINITIAL> "y"                      { return new Symbol(Simbolos.posicionY, yycolumn, yyline, yytext()); }
<YYINITIAL> "height"                 { return new Symbol(Simbolos.height, yycolumn, yyline, yytext()); }
<YYINITIAL> "width"                  { return new Symbol(Simbolos.width, yycolumn, yyline, yytext()); }
<YYINITIAL> "color"                  { return new Symbol(Simbolos.color, yycolumn, yyline, yytext()); }
<YYINITIAL> "border"                 { return new Symbol(Simbolos.border, yycolumn, yyline, yytext()); }
<YYINITIAL> "classname"              { return new Symbol(Simbolos.className, yycolumn, yyline, yytext()); }
<YYINITIAL> "render"                 { return new Symbol(Simbolos.render, yycolumn, yyline, yytext()); }
<YYINITIAL> "return"                 { return new Symbol(Simbolos.retorno, yycolumn, yyline, yytext()); }
<YYINITIAL> "panel"                  { return new Symbol(Simbolos.panel, yycolumn, yyline, yytext()); }
<YYINITIAL> "text"                   { yybegin(ETIQUETA); return new Symbol(Simbolos.textt, yycolumn, yyline, yytext()); }
<YYINITIAL> "textfield"              { yybegin(ETIQUETA); return new Symbol(Simbolos.textField, yycolumn, yyline, yytext()); }
<YYINITIAL> "button"                 { yybegin(ETIQUETA); return new Symbol(Simbolos.button, yycolumn, yyline, yytext()); }
<YYINITIAL> "/text"                   { return new Symbol(Simbolos.ftext, yycolumn, yyline, yytext()); }
<YYINITIAL> "/textfield"              { return new Symbol(Simbolos.ftextField, yycolumn, yyline, yytext()); }
<YYINITIAL> "/button"                 { return new Symbol(Simbolos.fbutton, yycolumn, yyline, yytext()); }
<YYINITIAL> "list"                   { return new Symbol(Simbolos.list, yycolumn, yyline, yytext()); }
<YYINITIAL> "elements"                { return new Symbol(Simbolos.element, yycolumn, yyline, yytext()); }
<YYINITIAL> "item"                   { yybegin(ETIQUETA); return new Symbol(Simbolos.items, yycolumn, yyline, yytext()); }
<YYINITIAL> "default"                { return new Symbol(Simbolos.defecto, yycolumn, yyline, yytext()); }
<YYINITIAL> "spinner"                { return new Symbol(Simbolos.spinner, yycolumn, yyline, yytext()); }
<YYINITIAL> "/item"                   { return new Symbol(Simbolos.fitem, yycolumn, yyline, yytext()); }
<YYINITIAL> "/default"                { return new Symbol(Simbolos.fdefecto, yycolumn, yyline, yytext()); }
<YYINITIAL> "/spinner"                { return new Symbol(Simbolos.fspinner, yycolumn, yyline, yytext()); }
<YYINITIAL> "min"                    { return new Symbol(Simbolos.min, yycolumn, yyline, yytext()); }
<YYINITIAL> "max"                    { return new Symbol(Simbolos.max, yycolumn, yyline, yytext()); }
<YYINITIAL> "image"                  { return new Symbol(Simbolos.image, yycolumn, yyline, yytext()); }
<YYINITIAL> "src"                    { return new Symbol(Simbolos.fuente, yycolumn, yyline, yytext()); }
<YYINITIAL> "import"                 { return new Symbol(Simbolos.importar, yycolumn, yyline, yytext()); }
<YYINITIAL> "from"                   { return new Symbol(Simbolos.from, yycolumn, yyline, yytext()); }
<YYINITIAL> "si"                     { return new Symbol(Simbolos.si, yycolumn, yyline, yytext()); }
<YYINITIAL> "sino"                   { return new Symbol(Simbolos.sino, yycolumn, yyline, yytext()); }
<YYINITIAL> "repetir"                { return new Symbol(Simbolos.repetir, yycolumn, yyline, yytext()); }
<YYINITIAL> "imprimir"               { return new Symbol(Simbolos.imprimir, yycolumn, yyline, yytext()); }
<YYINITIAL> "mientras"               { return new Symbol(Simbolos.mientras, yycolumn, yyline, yytext()); }
<YYINITIAL> {otros}                  { /* Espacios en blanco se ignoran */ }

<ETIQUETA> {otros}                  { /* Espacios en blanco se ignoran */ }
<ETIQUETA> "="                      { return new Symbol(Simbolos.igual, yycolumn, yyline, yytext()); }
<ETIQUETA> {entero}                 { return new Symbol(Simbolos.entero, yycolumn, yyline, yytext()); }
<ETIQUETA> {doble}                  { return new Symbol(Simbolos.doble, yycolumn, yyline, yytext()); }
<ETIQUETA> {cadena}                 { return new Symbol(Simbolos.cadena, yycolumn, yyline, yytext()); }
<ETIQUETA> {booleano}               { return new Symbol(Simbolos.booleano, yycolumn, yyline, yytext()); }
<ETIQUETA> {caracter}               { return new Symbol(Simbolos.caracter, yycolumn, yyline, yytext()); }
<ETIQUETA> {hexadecimal}            { return new Symbol(Simbolos.hexadecimal, yycolumn, yyline, yytext()); }
<ETIQUETA> "id"                     { return new Symbol(Simbolos.id, yycolumn, yyline, yytext()); }
<ETIQUETA> "("                      { return new Symbol(Simbolos.parentesisA, yycolumn, yyline, yytext()); }
<ETIQUETA> ")"                      { return new Symbol(Simbolos.parentesisC, yycolumn, yyline, yytext()); }
<ETIQUETA> "["                      { return new Symbol(Simbolos.corcheteA, yycolumn, yyline, yytext()); }
<ETIQUETA> "]"                      { return new Symbol(Simbolos.corcheteC, yycolumn, yyline, yytext()); }
<ETIQUETA> "{"                      { return new Symbol(Simbolos.llaveA, yycolumn, yyline, yytext()); }
<ETIQUETA> "}"                      { return new Symbol(Simbolos.llaveC, yycolumn, yyline, yytext()); }
<ETIQUETA> "x"                      { return new Symbol(Simbolos.posicionX, yycolumn, yyline, yytext()); }
<ETIQUETA> "y"                      { return new Symbol(Simbolos.posicionY, yycolumn, yyline, yytext()); }
<ETIQUETA> "height"                 { return new Symbol(Simbolos.height, yycolumn, yyline, yytext()); }
<ETIQUETA> "width"                  { return new Symbol(Simbolos.width, yycolumn, yyline, yytext()); }
<ETIQUETA> "color"                  { return new Symbol(Simbolos.color, yycolumn, yyline, yytext()); }
<ETIQUETA> "border"                 { return new Symbol(Simbolos.border, yycolumn, yyline, yytext()); }
<ETIQUETA> "classname"              { return new Symbol(Simbolos.className, yycolumn, yyline, yytext()); }
<ETIQUETA> "onclick"                { return new Symbol(Simbolos.onClick, yycolumn, yyline, yytext()); }
<ETIQUETA> ">"                      { yybegin(TEXTO); return new Symbol(Simbolos.mayorQue, yycolumn, yyline, yytext()); }
<ETIQUETA> {identificador}          { return new Symbol(Simbolos.identificador, yycolumn, yyline, yytext()); }
<TEXTO> {otros}                  { /* Espacios en blanco se ignoran */ }
<TEXTO> "{"                      { yybegin(YYINITIAL); return new Symbol(Simbolos.llaveA, yycolumn, yyline, yytext()); }
<TEXTO> "<"                      { yybegin(YYINITIAL); return new Symbol(Simbolos.menorQue, yycolumn, yyline, yytext()); }
<TEXTO> {texto}                  { yybegin(YYINITIAL); return new Symbol(Simbolos.texto, yycolumn, yyline, yytext()); }


//--------> Expresiones Regulares

<YYINITIAL> {entero}                 { return new Symbol(Simbolos.entero, yycolumn, yyline, yytext()); }
<YYINITIAL> {doble}                  { return new Symbol(Simbolos.doble, yycolumn, yyline, yytext()); }
<YYINITIAL> {cadena}                 { return new Symbol(Simbolos.cadena, yycolumn, yyline, yytext()); }
<YYINITIAL> {booleano}               { return new Symbol(Simbolos.booleano, yycolumn, yyline, yytext()); }
<YYINITIAL> {caracter}               { return new Symbol(Simbolos.caracter, yycolumn, yyline, yytext()); }
<YYINITIAL> {comentarioLinea}        {  }
<YYINITIAL> {comentarioMultiLinea}   {  }
<YYINITIAL> {identificador}          { return new Symbol(Simbolos.identificador, yycolumn, yyline, yytext()); }
<YYINITIAL> {hexadecimal}            { return new Symbol(Simbolos.hexadecimal, yycolumn, yyline, yytext()); }

//--------> Caracteres adicionales
[\t|\r|\n|\f|\s]*              { /* Espacios en blanco se ignoran */ }

//--------> Manejo de Error Lexico

.                                   { 
                                        System.out.println("Error Lexico Lexema: " + yytext() + " Fila: " + yyline + " Columna: " + yycolumn);
                                        Errores error = new Errores(yytext(),yyline,yycolumn
                                            ,"Error Lexico, No Especificado en el Lenguaje","Lexico");
                                        ErroresLexicos.insertar(error);
                                    }