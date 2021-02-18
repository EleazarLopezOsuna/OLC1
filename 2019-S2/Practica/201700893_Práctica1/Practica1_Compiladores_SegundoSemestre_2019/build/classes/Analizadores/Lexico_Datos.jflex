/*--------- 1ra Area: Codigo de Usuario ---------*/

//--------> Paquetes e importaciones
package Analizadores;
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
%class Analisis_Lexico_Datos
%cupsym Simbolos_Datos
%cup
%char
%column
%full
%ignorecase
%line
%unicode
%8bit

//--------> Expresiones regulares
numero                  = [\-]?[0-9]+(\.[0-9]+)?
cadena                  = [\"][^\"]*[\"]
comentarioLinea         = "//".*
comentarioMultiLinea    = "/*"( [^*] | (\*+[^*/]) )*\*+\/

//--------> Estados

%%

/*--------- 3ra Area: Reglas Lexicas ---------*/

//--------> Expresiones Regulares

<YYINITIAL> {numero}                  { return new Symbol(Simbolos_Datos.numero, yycolumn, yyline, yytext()); }
<YYINITIAL> {cadena}                  { return new Symbol(Simbolos_Datos.cadena, yycolumn, yyline, yytext()); }
<YYINITIAL> {comentarioLinea}         { return new Symbol(Simbolos_Datos.comentarioLinea, yycolumn, yyline, yytext()); }
<YYINITIAL> {comentarioMultiLinea}    { return new Symbol(Simbolos_Datos.comentarioMultiLinea, yycolumn, yyline, yytext()); }

//--------> Simbolos

<YYINITIAL> "["                     { return new Symbol(Simbolos_Datos.corcheteA, yycolumn, yyline, yytext()); }
<YYINITIAL> "]"                     { return new Symbol(Simbolos_Datos.corcheteC, yycolumn, yyline, yytext()); }
<YYINITIAL> "{"                     { return new Symbol(Simbolos_Datos.llaveA, yycolumn, yyline, yytext()); }
<YYINITIAL> "}"                     { return new Symbol(Simbolos_Datos.llaveC, yycolumn, yyline, yytext()); }
<YYINITIAL> ","                     { return new Symbol(Simbolos_Datos.coma, yycolumn, yyline, yytext()); }
<YYINITIAL> "="                     { return new Symbol(Simbolos_Datos.igual, yycolumn, yyline, yytext()); }

//--------> Palabras Reservaas

<YYINITIAL> "claves"                { return new Symbol(Simbolos_Datos.claves, yycolumn, yyline, yytext()); }
<YYINITIAL> "registros"             { return new Symbol(Simbolos_Datos.registros, yycolumn, yyline, yytext()); }

//--------> Caracteres adicionales
[\t\r\n\f\s\v\xxx\xhh]                { /* Espacios en blanco se ignoran */ }

//--------> Manejo de Error Lexico

.                                   { 
                                        System.out.println("Error Lexico Lexema: " + yytext() + " Fila: " + yyline + " Columna: " + yycolumn);
                                        Errores error = new Errores(yytext(),yyline,yycolumn
                                            ,"Error Lexico, No Especificado en el Lenguaje","Lexico");
                                        ErroresLexicos.insertar(error);
                                    }