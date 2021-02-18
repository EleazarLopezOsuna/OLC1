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
%class Analisis_Lexico_Reportes
%cupsym Simbolos_Reportes
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
identificador           = [a-zA-Z]([a-zA-Z]|[0-9|"_"])*
comentarioLinea         = "//".*
comentarioMultiLinea    = "/*"( [^*] | (\*+[^*/]) )*\*+\/

//--------> Estados

%%

/*--------- 3ra Area: Reglas Lexicas ---------*/

//--------> Simbolos

<YYINITIAL> "("                     { return new Symbol(Simbolos_Reportes.parenteA, yycolumn, yyline, yytext()); }
<YYINITIAL> ")"                     { return new Symbol(Simbolos_Reportes.parenteC, yycolumn, yyline, yytext()); }
<YYINITIAL> ";"                     { return new Symbol(Simbolos_Reportes.puntoComa, yycolumn, yyline, yytext()); }
<YYINITIAL> "<"                     { return new Symbol(Simbolos_Reportes.menorQue, yycolumn, yyline, yytext()); }
<YYINITIAL> ">"                     { return new Symbol(Simbolos_Reportes.mayorQue, yycolumn, yyline, yytext()); }
<YYINITIAL> "!"                     { return new Symbol(Simbolos_Reportes.diferente, yycolumn, yyline, yytext()); }
<YYINITIAL> ","                     { return new Symbol(Simbolos_Reportes.coma, yycolumn, yyline, yytext()); }
<YYINITIAL> "="                     { return new Symbol(Simbolos_Reportes.igual, yycolumn, yyline, yytext()); }

//--------> Palabras Reservaas

<YYINITIAL> "archivo"                { return new Symbol(Simbolos_Reportes.tipoArchivo, yycolumn, yyline, yytext()); }
<YYINITIAL> "leerarchivo"            { return new Symbol(Simbolos_Reportes.leerArchivo, yycolumn, yyline, yytext()); }
<YYINITIAL> "numerico"               { return new Symbol(Simbolos_Reportes.tipoNumerico, yycolumn, yyline, yytext()); }
<YYINITIAL> "sumar"                  { return new Symbol(Simbolos_Reportes.sumar, yycolumn, yyline, yytext()); }
<YYINITIAL> "contar"                 { return new Symbol(Simbolos_Reportes.contar, yycolumn, yyline, yytext()); }
<YYINITIAL> "promedio"               { return new Symbol(Simbolos_Reportes.promedio, yycolumn, yyline, yytext()); }
<YYINITIAL> "contarsi"               { return new Symbol(Simbolos_Reportes.contarSi, yycolumn, yyline, yytext()); }
<YYINITIAL> "cadena"                 { return new Symbol(Simbolos_Reportes.tipoCadena, yycolumn, yyline, yytext()); }
<YYINITIAL> "obtenersi"              { return new Symbol(Simbolos_Reportes.obtenerSi, yycolumn, yyline, yytext()); }
<YYINITIAL> "imprimir"               { return new Symbol(Simbolos_Reportes.imprimir, yycolumn, yyline, yytext()); }
<YYINITIAL> "graficar"               { return new Symbol(Simbolos_Reportes.graficar, yycolumn, yyline, yytext()); }

//--------> Expresiones Regulares

<YYINITIAL> {numero}                  { return new Symbol(Simbolos_Reportes.numero, yycolumn, yyline, yytext()); }
<YYINITIAL> {cadena}                  { return new Symbol(Simbolos_Reportes.cadena, yycolumn, yyline, yytext()); }
<YYINITIAL> {comentarioLinea}         { return new Symbol(Simbolos_Reportes.comentarioLinea, yycolumn, yyline, yytext()); }
<YYINITIAL> {comentarioMultiLinea}    { return new Symbol(Simbolos_Reportes.comentarioMultiLinea, yycolumn, yyline, yytext()); }
<YYINITIAL> {identificador}           { return new Symbol(Simbolos_Reportes.ident, yycolumn, yyline, yytext()); }

//--------> Caracteres adicionales
[\t\r\n\f\s\v\xxx\xhh]                { /* Espacios en blanco se ignoran */ }

//--------> Manejo de Error Lexico

.                                   { 
                                        System.out.println("Error Lexico Lexema: " + yytext() + " Fila: " + yyline + " Columna: " + yycolumn);
                                        Errores error = new Errores(yytext(),yyline,yycolumn
                                            ,"Error Lexico, No Especificado en el Lenguaje","Lexico");
                                        ErroresLexicos.insertar(error);
                                    }