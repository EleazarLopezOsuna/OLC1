
/*--------- 1ra Area: Codigo de Usuario ---------*/

//--------> Paquetes e importaciones
package Analizadores.HTML;
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
cadena                  = [\"][^\"]*[\"]
comentarioMultiLinea    = "<!--"[^>]*"-->"
texto                   = [^<]*
otros                   = [\t|\r|\n|\f|\s|\v|\xxx|\xhh]*

//--------> Estados
%state TEXTO
%%

/*--------- 3ra Area: Reglas Lexicas ---------*/

//--------> Simbolos
<YYINITIAL> {comentarioMultiLinea}  { /**/ }
<YYINITIAL> "<"                     { return new Symbol(Simbolos.abrir, yycolumn, yyline, yytext()); }
<YYINITIAL> ">"                     { yybegin(TEXTO); return new Symbol(Simbolos.cerrar, yycolumn, yyline, yytext()); }
<YYINITIAL> "="                     { return new Symbol(Simbolos.igual, yycolumn, yyline, yytext()); }

//--------> Palabras Reservaas

<YYINITIAL> "html"                { return new Symbol(Simbolos.inicioHtml, yycolumn, yyline, yytext()); }
<YYINITIAL> "/html"               { return new Symbol(Simbolos.finHtml, yycolumn, yyline, yytext()); }
<YYINITIAL> "head"                { return new Symbol(Simbolos.inicioHead, yycolumn, yyline, yytext()); }
<YYINITIAL> "/head"               { return new Symbol(Simbolos.finHead, yycolumn, yyline, yytext()); }
<YYINITIAL> "title"               { return new Symbol(Simbolos.inicioTitle, yycolumn, yyline, yytext()); }
<YYINITIAL> "/title"              { return new Symbol(Simbolos.finTitle, yycolumn, yyline, yytext()); }
<YYINITIAL> "body"                { return new Symbol(Simbolos.inicioBody, yycolumn, yyline, yytext()); }
<YYINITIAL> "/body"               { return new Symbol(Simbolos.finBody, yycolumn, yyline, yytext()); }
<YYINITIAL> "noufe"               { return new Symbol(Simbolos.inicioNoufe, yycolumn, yyline, yytext()); }
<YYINITIAL> "/noufe"              { return new Symbol(Simbolos.finNoufe, yycolumn, yyline, yytext()); }
<YYINITIAL> "div"                  { return new Symbol(Simbolos.inicioDiv, yycolumn, yyline, yytext()); }
<YYINITIAL> "/div"                { return new Symbol(Simbolos.finDiv, yycolumn, yyline, yytext()); }
<YYINITIAL> "id"                    { return new Symbol(Simbolos.id, yycolumn, yyline, yytext()); }


<TEXTO> {comentarioMultiLinea}      { /**/ }
<TEXTO> {otros}                     { /**/ }
<TEXTO> "<"                         { yybegin(YYINITIAL); return new Symbol(Simbolos.abrir, yycolumn, yyline, yytext()); }
<TEXTO> {texto}                     { return new Symbol(Simbolos.texto, yycolumn, yyline, yytext()); }

//--------> Expresiones Regulares

<YYINITIAL> {cadena}                { return new Symbol(Simbolos.cadena, yycolumn, yyline, yytext()); }


//--------> Caracteres adicionales
[\t\r\n\f\s\v\xxx\xhh]              { /* Espacios en blanco se ignoran */ }

//--------> Manejo de Error Lexico

.                                   { 
                                        System.out.println("ERROR LEXICO: " + yytext() + " LINEA: " + yyline + " COLUMNA: " + yycolumn);
                                    }