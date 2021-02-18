/*--------- 1ra Area: Codigo de Usuario ---------*/

//--------> Paquetes e importaciones
package Analizadores.CSS;
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
numero                  = [0-9]+
colorrgb                = ([0-9][0-9][0-9]|[0-9][0-9]|[0-9])
booleano                = ("true"|"false")
cadena                  = "'"[^"'"]*"'"
comentarioLinea         = "//".*
identificador           = [a-zA-Z]([a-zA-Z]|[0-9|"_"])*
subidentificador        = "."[a-zA-Z]([a-zA-Z]|[0-9|"_"])*
comentarioMultiLinea    = "/*"( [^*] | (\*+[^*/]) )*\*+\/
hexadecimal             = "#"[0-9a-fA-F]{6}

//--------> Estados
%state ESTILO
%%

/*--------- 3ra Area: Reglas Lexicas ---------*/

//--------> Simbolos

<ESTILO> "("                     { return new Symbol(Simbolos.parentesisA, yycolumn, yyline, yytext()); }
<ESTILO> ")"                     { return new Symbol(Simbolos.parentesisC, yycolumn, yyline, yytext()); }
<ESTILO> ";"                     { return new Symbol(Simbolos.puntoComa, yycolumn, yyline, yytext()); }
<ESTILO> ":"                     { return new Symbol(Simbolos.dosPuntos, yycolumn, yyline, yytext()); }
<YYINITIAL> "{"                  { yybegin(ESTILO); return new Symbol(Simbolos.llaveA, yycolumn, yyline, yytext()); }
<ESTILO> "}"                     { yybegin(YYINITIAL); return new Symbol(Simbolos.llaveC, yycolumn, yyline, yytext()); }
<ESTILO> ","                     { return new Symbol(Simbolos.coma, yycolumn, yyline, yytext()); }

//--------> Palabras Reservaas

<ESTILO> "background"            { return new Symbol(Simbolos.background, yycolumn, yyline, yytext()); }
<ESTILO> "border"                { return new Symbol(Simbolos.border, yycolumn, yyline, yytext()); }
<ESTILO> "border-color"          { return new Symbol(Simbolos.borderColor, yycolumn, yyline, yytext()); }
<ESTILO> "border-width"          { return new Symbol(Simbolos.borderWidth, yycolumn, yyline, yytext()); }
<ESTILO> "align"                 { return new Symbol(Simbolos.align, yycolumn, yyline, yytext()); }
<ESTILO> "font"                  { return new Symbol(Simbolos.font, yycolumn, yyline, yytext()); }
<ESTILO> "font-size"             { return new Symbol(Simbolos.fontSize, yycolumn, yyline, yytext()); }
<ESTILO> "font-color"            { return new Symbol(Simbolos.fontColor, yycolumn, yyline, yytext()); }
<ESTILO> "height"                 { return new Symbol(Simbolos.hight, yycolumn, yyline, yytext()); }
<ESTILO> "width"                 { return new Symbol(Simbolos.width, yycolumn, yyline, yytext()); }
<ESTILO> "rgb"                   { return new Symbol(Simbolos.rgb, yycolumn, yyline, yytext()); }
<ESTILO> "red"                   { return new Symbol(Simbolos.red, yycolumn, yyline, yytext()); }
<ESTILO> "pink"                  { return new Symbol(Simbolos.pink, yycolumn, yyline, yytext()); }
<ESTILO> "orange"                { return new Symbol(Simbolos.orange, yycolumn, yyline, yytext()); }
<ESTILO> "yellow"                { return new Symbol(Simbolos.yellow, yycolumn, yyline, yytext()); }
<ESTILO> "purple"                { return new Symbol(Simbolos.purple, yycolumn, yyline, yytext()); }
<ESTILO> "magenta"               { return new Symbol(Simbolos.magenta, yycolumn, yyline, yytext()); }
<ESTILO> "green"                 { return new Symbol(Simbolos.green, yycolumn, yyline, yytext()); }
<ESTILO> "blue"                  { return new Symbol(Simbolos.blue, yycolumn, yyline, yytext()); }
<ESTILO> "brown"                 { return new Symbol(Simbolos.brown, yycolumn, yyline, yytext()); }
<ESTILO> "white"                 { return new Symbol(Simbolos.white, yycolumn, yyline, yytext()); }
<ESTILO> "gray"                  { return new Symbol(Simbolos.gray, yycolumn, yyline, yytext()); }
<ESTILO> "black"                 { return new Symbol(Simbolos.black, yycolumn, yyline, yytext()); }
<ESTILO> "left"                  { return new Symbol(Simbolos.ALIleft, yycolumn, yyline, yytext()); }
<ESTILO> "right"                 { return new Symbol(Simbolos.ALIright, yycolumn, yyline, yytext()); }
<ESTILO> "center"                { return new Symbol(Simbolos.ALIcenter, yycolumn, yyline, yytext()); }

//--------> Expresiones Regulares

<ESTILO> {booleano}                { return new Symbol(Simbolos.booleano, yycolumn, yyline, yytext()); }
<ESTILO> {numero}                  { return new Symbol(Simbolos.numero, yycolumn, yyline, yytext()); }
<ESTILO> {cadena}                  { return new Symbol(Simbolos.cadena, yycolumn, yyline, yytext()); }
<YYINITIAL> {identificador}        { return new Symbol(Simbolos.identificador, yycolumn, yyline, yytext()); }
<YYINITIAL> {subidentificador}     { return new Symbol(Simbolos.subidentificador, yycolumn, yyline, yytext()); }
<ESTILO> {comentarioLinea}         {  }
<ESTILO> {comentarioMultiLinea}    {  }
<YYINITIAL> {comentarioLinea}      {  }
<YYINITIAL> {comentarioMultiLinea} {  }
<ESTILO> {hexadecimal}             { return new Symbol(Simbolos.hexadecimal, yycolumn, yyline, yytext()); }
<ESTILO> {colorrgb}              { return new Symbol(Simbolos.colorrgb, yycolumn, yyline, yytext()); }

//--------> Caracteres adicionales
[\t\r\n\f\s\v\xxx]                { /* Espacios en blanco se ignoran */ }

//--------> Manejo de Error Lexico

.                                   { 
                                        System.out.println("Error Lexico Lexema: " + yytext() + " Fila: " + yyline + " Columna: " + yycolumn);
                                        Errores error = new Errores(yytext(),yyline,yycolumn
                                            ,"Error Lexico, No Especificado en el Lenguaje","Lexico");
                                        ErroresLexicos.insertar(error);
                                    }
