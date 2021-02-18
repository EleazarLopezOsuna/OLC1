using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Irony.Ast;
using Irony.Parsing;

namespace Proyecto2_201700893.Analizador
{
    class Gramatica : Grammar
    {
        public Gramatica() : base(caseSensitive: false)
        {
            #region ER
            RegexBasedTerminal entero = new RegexBasedTerminal("entero", "[0-9]+");
            RegexBasedTerminal doble = new RegexBasedTerminal("doble", "[0-9]+(\\.[0-9]+)");
            StringLiteral cadena = new StringLiteral("cadena", "\"", StringOptions.AllowsDoubledQuote);
            RegexBasedTerminal caracter = new RegexBasedTerminal("caracter", "\'.\'");
            RegexBasedTerminal boleano = new RegexBasedTerminal("boleano", "(true|false)");
            IdentifierTerminal id = new IdentifierTerminal("id");
            CommentTerminal comentarioLinea = new CommentTerminal("comentario linea", "//", "\n", "\r\n");
            CommentTerminal comentarioBloque = new CommentTerminal("comentario bloque", "/*", "*/");

            base.NonGrammarTerminals.Add(comentarioLinea);
            base.NonGrammarTerminals.Add(comentarioBloque);
            #endregion

            #region Palabras Reservadas
            this.MarkReservedWords("imprimir");
            this.MarkReservedWords("int");
            this.MarkReservedWords("string");
            #endregion

            #region Terminales
            var parentesisA = "(";
            var parentesisC = ")";
            var corcheteA = "[";
            var corcheteC = "]";
            var llaveA = "{";
            var llaveC = "}";
            var igual = "=";
            var puntoComa = ";";
            var dosPuntos = ":";
            var coma = ",";
            var suma = "+";
            var resta = "-";
            var multiplicacion = "*";
            var division = "/";
            var potencia = "pow";
            var logicoAND = "&&";
            var logicoOR = "||";
            var logicoXOR = "^";
            var logicoNOT = "!";
            var mayorQue = ">";
            var menorQue = "<";
            var igualIgual = "==";
            var desIgual = "<>";
            var mayorIgualQue = ">=";
            var menorIgualQue = "<=";
            var incremento = "++";
            var decremento = "--";
            #endregion

            #region Presedencia
            this.RegisterOperators(1, Associativity.Left, logicoOR);
            this.RegisterOperators(2, Associativity.Left, logicoXOR);
            this.RegisterOperators(3, Associativity.Left, logicoAND);
            this.RegisterOperators(4, Associativity.Neutral, igualIgual, desIgual);
            this.RegisterOperators(5, Associativity.Neutral, mayorQue, mayorIgualQue, menorQue, menorIgualQue);
            this.RegisterOperators(6, Associativity.Left, suma, resta);
            this.RegisterOperators(7, Associativity.Left, multiplicacion, division);
            this.RegisterOperators(8, Associativity.Left, potencia);
            this.RegisterOperators(9, Associativity.Right, logicoNOT);
            this.RegisterOperators(10, Associativity.Neutral, incremento, decremento);
            #endregion

            #region Reservadas
            var variableRes = ToTerm("var");
            var mainRes = ToTerm("main");
            var importRes = ToTerm("importar");
            var logRes = ToTerm("log");
            var alertRes = ToTerm("alert");
            var ifRes = ToTerm("if");
            var graphRes = ToTerm("graph");
            var elseRes = ToTerm("else");
            var switchRes = ToTerm("switch");
            var caseRes = ToTerm("case");
            var whileRes = ToTerm("while");
            var doRes = ToTerm("do");
            var forRes = ToTerm("for");
            var breakRes = ToTerm("break");
            var continueRes = ToTerm("continue");
            var functionRes = ToTerm("function");
            var voidRes = ToTerm("void");
            var returnRes = ToTerm("return");
            var nullRes = ToTerm("null");
            var classRes = ToTerm("class");
            var newRes = ToTerm("new");
            var defaultRes = ToTerm("default");
            #endregion

            #region No Terminales
            NonTerminal INICIO = new NonTerminal("INICIO");
            NonTerminal DATO = new NonTerminal("DATO");
            NonTerminal EXPRESION = new NonTerminal("EXPRESION");
            NonTerminal L_INSTRUCCIONES = new NonTerminal("L_INSTRUCCIONES");
            NonTerminal INSTRUCCION = new NonTerminal("INSTRUCCION");
            NonTerminal L_DECLARACIONES = new NonTerminal("L_DECLARACIONES");
            NonTerminal DECLARACION = new NonTerminal("DECLARACION");
            NonTerminal LINEAL = new NonTerminal("LINEAL");
            NonTerminal ARREGLO = new NonTerminal("ARREGLO");
            NonTerminal ARREGLO2 = new NonTerminal("ARREGLO");
            NonTerminal DIM1 = new NonTerminal("DIM1");
            NonTerminal DIM2 = new NonTerminal("DIM2");
            NonTerminal DIM3 = new NonTerminal("DIM3");
            NonTerminal DIM1_2 = new NonTerminal("DIM1");
            NonTerminal DIM2_2 = new NonTerminal("DIM2");
            NonTerminal DIM3_2 = new NonTerminal("DIM3");
            NonTerminal IMPORT = new NonTerminal("IMPORT");
            NonTerminal MAIN = new NonTerminal("MAIN");
            NonTerminal L_PARAMETROS = new NonTerminal("L_PARAMETROS");
            NonTerminal LOG = new NonTerminal("LOG");
            NonTerminal ALERT = new NonTerminal("ALERT");
            NonTerminal GRAPH = new NonTerminal("GRAPH");
            NonTerminal IF = new NonTerminal("IF");
            NonTerminal ELSE = new NonTerminal("ELSE");
            NonTerminal SWITCH = new NonTerminal("SWITCH");
            NonTerminal CASEDEFAULT = new NonTerminal("CASE");
            NonTerminal CASE = new NonTerminal("CASE");
            NonTerminal WHILE = new NonTerminal("WHILE");
            NonTerminal DOWHILE = new NonTerminal("DOWHILE");
            NonTerminal FOR = new NonTerminal("FOR");
            NonTerminal METODO = new NonTerminal("METODO");
            NonTerminal FUNCION = new NonTerminal("FUNCION");
            NonTerminal CLASE = new NonTerminal("CLASE");
            NonTerminal ELSEIF = new NonTerminal("ELSEIF");
            NonTerminal REASIGNACION = new NonTerminal("REASIGNACION");
            NonTerminal DEFAULT = new NonTerminal("DEFAULT");
            NonTerminal OPCIONESFOR = new NonTerminal("OPCIONESFOR");
            NonTerminal CONTROLFOR = new NonTerminal("CONTROLFOR");
            NonTerminal CONDICIONFOR = new NonTerminal("CONDICIONFOR");
            NonTerminal UPDATEFOR = new NonTerminal("UPDATEFOR");
            NonTerminal PARAMETROS = new NonTerminal("PARAMETROS");
            NonTerminal EOTRO = new NonTerminal("EOTRO");
            NonTerminal L_INSTRUCCIONES_METODO = new NonTerminal("L_INSTRUCCIONES");
            NonTerminal INSTRUCCION_METODO = new NonTerminal("INSTRUCCION");
            NonTerminal L_INSTRUCCIONES_CLASE = new NonTerminal("L_INSTRUCCIONES");
            NonTerminal INSTRUCCION_CLASE = new NonTerminal("INSTRUCCION");
            NonTerminal L_INSTRUCCIONES_FUNCION = new NonTerminal("L_INSTRUCCIONES");
            NonTerminal INSTRUCCION_FUNCION = new NonTerminal("INSTRUCCION");
            NonTerminal RETURN = new NonTerminal("RETURN");
            NonTerminal E_DIM = new NonTerminal("E_DIM");
            NonTerminal E_DIM1 = new NonTerminal("E_DIM1");
            NonTerminal I_DIM = new NonTerminal("I_DIM");
            NonTerminal DIM = new NonTerminal("DIM");
            NonTerminal L_CASE = new NonTerminal("L_CASE");
            NonTerminal GLOBAL = new NonTerminal("GLOBAL");
            NonTerminal INDE = new NonTerminal("EXPRESION");
            #endregion

            #region Gramatica

            #region INICIO, IMPORT, MAIN
            INICIO.Rule = GLOBAL
                ;

            IMPORT.Rule = IMPORT + importRes + parentesisA + cadena + parentesisC + puntoComa
                | importRes + parentesisA + cadena + parentesisC + puntoComa
                ;

            MAIN.Rule = mainRes + parentesisA + parentesisC + llaveA + L_INSTRUCCIONES_METODO + llaveC
                ;

            GLOBAL.Rule = L_INSTRUCCIONES
                ;
            #endregion

            #region L_INSTRUCCIONES, INSTRUCCION (Normales, Metodo, Funcion, Clase)
            L_INSTRUCCIONES.Rule = L_INSTRUCCIONES + INSTRUCCION
                | INSTRUCCION
                ;

            INSTRUCCION.Rule = DECLARACION + puntoComa
                | REASIGNACION + puntoComa
                | EOTRO + puntoComa
                | LOG + puntoComa
                | ALERT + puntoComa
                | GRAPH + puntoComa
                | IF
                | SWITCH
                | WHILE
                | DOWHILE
                | FOR
                | METODO
                | FUNCION
                | CLASE
                | EOTRO + puntoComa
                | breakRes + puntoComa
                | continueRes + puntoComa
                | IMPORT
                | MAIN
                | INDE + puntoComa
                ;

            L_INSTRUCCIONES_METODO.Rule = L_INSTRUCCIONES_METODO + INSTRUCCION_METODO
                | INSTRUCCION_METODO
                ;

            INSTRUCCION_METODO.Rule = DECLARACION + puntoComa
                | REASIGNACION + puntoComa
                | EOTRO + puntoComa
                | LOG + puntoComa
                | ALERT + puntoComa
                | GRAPH + puntoComa
                | IF
                | SWITCH
                | WHILE
                | DOWHILE
                | FOR
                | breakRes + puntoComa
                | continueRes + puntoComa
                | RETURN
                | EOTRO + puntoComa
                | INDE + puntoComa
                ;

            L_INSTRUCCIONES_FUNCION.Rule = L_INSTRUCCIONES_FUNCION + INSTRUCCION_FUNCION
                | INSTRUCCION_FUNCION
                ;

            INSTRUCCION_FUNCION.Rule = DECLARACION + puntoComa
                | REASIGNACION + puntoComa
                | EOTRO + puntoComa
                | LOG + puntoComa
                | ALERT + puntoComa
                | GRAPH + puntoComa
                | IF
                | SWITCH
                | WHILE
                | DOWHILE
                | FOR
                | RETURN
                | EOTRO + puntoComa
                | INDE + puntoComa
                ;

            L_INSTRUCCIONES_CLASE.Rule = L_INSTRUCCIONES_CLASE + INSTRUCCION_CLASE
                | INSTRUCCION_CLASE
                ;

            INSTRUCCION_CLASE.Rule = DECLARACION + puntoComa
                | REASIGNACION + puntoComa
                | EOTRO + puntoComa
                | LOG + puntoComa
                | ALERT + puntoComa
                | GRAPH + puntoComa
                | IF
                | SWITCH
                | WHILE
                | DOWHILE
                | FOR
                | METODO
                | FUNCION
                | EOTRO + puntoComa
                | INDE + puntoComa
                ;
            #endregion

            #region IF, ELSEIF, ELSE
            IF.Rule = ifRes + parentesisA + EXPRESION + parentesisC + llaveA + L_INSTRUCCIONES_METODO + llaveC + ELSEIF + ELSE
                | ifRes + parentesisA + EXPRESION + parentesisC + llaveA + L_INSTRUCCIONES_METODO + llaveC + ELSEIF
                | ifRes + parentesisA + EXPRESION + parentesisC + llaveA + L_INSTRUCCIONES_METODO + llaveC + ELSE
                | ifRes + parentesisA + EXPRESION + parentesisC + llaveA + L_INSTRUCCIONES_METODO + llaveC
                | ifRes + parentesisA + EXPRESION + parentesisC + llaveA + llaveC + ELSEIF + ELSE
                | ifRes + parentesisA + EXPRESION + parentesisC + llaveA + llaveC + ELSEIF
                | ifRes + parentesisA + EXPRESION + parentesisC + llaveA + llaveC + ELSE
                | ifRes + parentesisA + EXPRESION + parentesisC + llaveA + llaveC
                ;

            ELSEIF.Rule = elseRes + IF
                ;

            ELSE.Rule = elseRes + llaveA + L_INSTRUCCIONES_METODO + llaveC
                | elseRes + llaveA + llaveC
                ;
            #endregion

            #region SWITCH, CASEDEFAULT, CASE, DEFAULT
            SWITCH.Rule = switchRes + parentesisA + EXPRESION + parentesisC + llaveA + CASE + llaveC
                | switchRes + parentesisA + EXPRESION + parentesisC + llaveA + llaveC
                ;

            L_CASE.Rule = L_CASE + caseRes + EXPRESION + dosPuntos
                | caseRes + EXPRESION + dosPuntos
                | L_CASE + DEFAULT
                | DEFAULT
                ;

            DEFAULT.Rule = defaultRes + dosPuntos
                ;

            CASE.Rule = CASE + L_CASE + L_INSTRUCCIONES_METODO + breakRes + puntoComa
                | CASE + L_CASE + breakRes + puntoComa
                | L_CASE + L_INSTRUCCIONES_METODO + breakRes + puntoComa
                | L_CASE + breakRes + puntoComa
                ;
            #endregion

            #region WHILE, DOWHILE, FOR, OPCIONESFOR, CONTROLFOR, CONDICIONFOR, UPDATEFOR
            WHILE.Rule = whileRes + parentesisA + EXPRESION + parentesisC + llaveA + L_INSTRUCCIONES_METODO + llaveC
                | whileRes + parentesisA + EXPRESION + parentesisC + llaveA + llaveC
                ;

            DOWHILE.Rule = doRes + llaveA + L_INSTRUCCIONES_METODO + llaveC + whileRes + parentesisA + EXPRESION + parentesisC + puntoComa
                | doRes + llaveA + llaveC + whileRes + parentesisA + EXPRESION + parentesisC + puntoComa
                ;

            FOR.Rule = forRes + parentesisA + OPCIONESFOR + parentesisC + llaveA + L_INSTRUCCIONES_METODO + llaveC
                | forRes + OPCIONESFOR + parentesisA + parentesisC + llaveA + llaveC
                ;

            OPCIONESFOR.Rule = CONTROLFOR + puntoComa + CONDICIONFOR + puntoComa + UPDATEFOR
                ;

            CONTROLFOR.Rule = DECLARACION
                | REASIGNACION
                ;

            CONDICIONFOR.Rule = EXPRESION
                ;

            UPDATEFOR.Rule = EXPRESION
                ;
            #endregion

            #region METODO, FUNCION, CLASE, PARAMETROS, RETURN
            METODO.Rule = functionRes + voidRes + id + parentesisA + PARAMETROS + parentesisC + llaveA + L_INSTRUCCIONES_METODO + llaveC
                | functionRes + voidRes + id + parentesisA + parentesisC + llaveA + L_INSTRUCCIONES_METODO + llaveC
                | functionRes + voidRes + id + parentesisA + PARAMETROS + parentesisC + llaveA + llaveC
                | functionRes + voidRes + id + parentesisA + parentesisC + llaveA + llaveC
                ;

            PARAMETROS.Rule = PARAMETROS + coma + variableRes + id
                | variableRes + id
                ;

            FUNCION.Rule = functionRes + id + parentesisA + PARAMETROS + parentesisC + llaveA + L_INSTRUCCIONES_FUNCION + llaveC
                | functionRes + id + parentesisA + parentesisC + llaveA + L_INSTRUCCIONES_FUNCION + llaveC
                | functionRes + id + parentesisA + PARAMETROS + parentesisC + llaveA + llaveC
                | functionRes + id + parentesisA + parentesisC + llaveA + llaveC
                ;

            RETURN.Rule = returnRes + EXPRESION + puntoComa
                ;

            CLASE.Rule = classRes + id + llaveA + L_INSTRUCCIONES_CLASE + llaveC
                | classRes + id + llaveA + llaveC
                ;
            #endregion

            #region DECLARACION, L_DECLARACIONES, REASIGNACION, LINEAL, ARREGLO, DIM(1,2,3)

            DECLARACION.Rule = variableRes + L_DECLARACIONES
                ;

            L_DECLARACIONES.Rule = L_DECLARACIONES + coma + LINEAL
                | L_DECLARACIONES + coma + ARREGLO
                | LINEAL
                | ARREGLO
                ;

            REASIGNACION.Rule = id + igual + EXPRESION
                | id + "." + id + igual + EXPRESION
                | ARREGLO2 + igual + ARREGLO2
                | ARREGLO2 + igual + EXPRESION
                | id + igual + DIM
                | ARREGLO2 + igual + DIM
                ;

            LINEAL.Rule = id + igual + EXPRESION
                | id
                ;

            ARREGLO.Rule = DIM1
                | DIM2
                | DIM3
                ;

            DIM1.Rule = id + corcheteA + EXPRESION + corcheteC + igual + EXPRESION
                | id + corcheteA + EXPRESION + corcheteC
                | id + corcheteA + EXPRESION + corcheteC + igual + llaveA + I_DIM + llaveC
                /*
                 {
                    0,1,2
                 }
                 */
                ;

            E_DIM.Rule = E_DIM + "," + llaveA + I_DIM + llaveC
                | llaveA + I_DIM + llaveC
                ;

            E_DIM1.Rule = E_DIM1 + "," + llaveA + E_DIM + llaveC
                | llaveA + E_DIM + llaveC
                ;

            I_DIM.Rule = I_DIM + "," + EXPRESION
                | EXPRESION
                ;

            DIM.Rule = llaveA + I_DIM + llaveC
                | llaveA + E_DIM + llaveC
                | llaveA + E_DIM1 + llaveC
                ;

            DIM2.Rule = id + corcheteA + EXPRESION + corcheteC + corcheteA + EXPRESION + corcheteC + igual + EXPRESION
                | id + corcheteA + EXPRESION + corcheteC + corcheteA + EXPRESION + corcheteC
                | id + corcheteA + EXPRESION + corcheteC + corcheteA + EXPRESION + corcheteC + igual + llaveA + E_DIM + llaveC
                /*
                 {
                    {0,1,2},
                    {3,4,5},
                    {6,7,5}
                 }
                 */
                ;

            DIM3.Rule = id + corcheteA + EXPRESION + corcheteC + corcheteA + EXPRESION + corcheteC + corcheteA + EXPRESION + corcheteC + igual + EXPRESION
                | id + corcheteA + EXPRESION + corcheteC + corcheteA + EXPRESION + corcheteC + corcheteA + EXPRESION + corcheteC
                | id + corcheteA + EXPRESION + corcheteC + corcheteA + EXPRESION + corcheteC + corcheteA + EXPRESION + corcheteC + igual + llaveA + E_DIM1 + llaveC
                /*
                 {
                    {
                        {0,1,2},
                        {3,4,5},
                        {6,7,5}
                    },
                    {
                        {0,1,2},
                        {3,4,5},
                        {6,7,5}
                    },
                    {
                        {0,1,2},
                        {3,4,5},
                        {6,7,5}
                    }
                 }
                 */
                ;

            ARREGLO2.Rule = DIM1_2
                | DIM2_2
                | DIM3_2
                ;

            DIM1_2.Rule = id + corcheteA + EXPRESION + corcheteC
                ;

            DIM2_2.Rule = id + corcheteA + EXPRESION + corcheteC + corcheteA + EXPRESION + corcheteC
                ;

            DIM3_2.Rule = id + corcheteA + EXPRESION + corcheteC + corcheteA + EXPRESION + corcheteC + corcheteA + EXPRESION + corcheteC
                ;
            #endregion

            #region EXPRESION, LOG, ALERT, GRAPH, EOTRO, L_PARAMETROS, DATO
            EXPRESION.Rule = EXPRESION + suma + EXPRESION
                | EXPRESION + resta + EXPRESION
                | EXPRESION + multiplicacion + EXPRESION
                | EXPRESION + division + EXPRESION
                | EXPRESION + potencia + EXPRESION
                | EXPRESION + igualIgual + EXPRESION
                | EXPRESION + mayorQue + EXPRESION
                | EXPRESION + mayorIgualQue + EXPRESION
                | EXPRESION + menorQue + EXPRESION
                | EXPRESION + menorIgualQue + EXPRESION
                | resta + EXPRESION
                | parentesisA + EXPRESION + parentesisC
                | EXPRESION + logicoAND + EXPRESION
                | EXPRESION + logicoOR + EXPRESION
                | EXPRESION + logicoXOR + EXPRESION
                | logicoNOT + EXPRESION
                | EXPRESION + desIgual + EXPRESION
                | EXPRESION + incremento
                | EXPRESION + decremento
                | DATO
                | ARREGLO
                | EOTRO
                ;

            INDE.Rule = INDE + incremento
                | INDE + decremento
                | DATO
                ;

            LOG.Rule = logRes + parentesisA + EXPRESION + parentesisC
                ;

            ALERT.Rule = alertRes + parentesisA + EXPRESION + parentesisC
                ;

            GRAPH.Rule = graphRes + parentesisA + EXPRESION + coma + EXPRESION + parentesisC
                ;

            EOTRO.Rule = id + parentesisA + L_PARAMETROS + parentesisC
                | newRes + id + parentesisA + parentesisC
                | id + parentesisA + parentesisC
                | id + "." + id + parentesisA + L_PARAMETROS + parentesisC
                | id + "." + id + parentesisA + parentesisC
                ;

            L_PARAMETROS.Rule = L_PARAMETROS + coma + EXPRESION
                | EXPRESION
                ;

            DATO.Rule = entero
                | doble
                | cadena
                | caracter
                | boleano
                | id
                | id + "." + id
                | nullRes
                ;
            #endregion

            #endregion

            #region Preferencias
            this.Root = INICIO;
            this.MarkPunctuation(corcheteA, corcheteC, igual, coma, puntoComa, parentesisA, parentesisC, ".");
            this.MarkPunctuation("var", "main", "importar", "log", "if", "else", "while", "do", "graph", "alert", "switch", "case");
            this.MarkPunctuation("for", "function", "void", "return", "class", ":");
            this.MarkPunctuation(llaveA, llaveC);
            this.MarkReservedWords("var", "importar", "log", "if", "else", "while", "do", "graph", "alert", "switch", "case");
            this.MarkReservedWords("for", "break", "continue", "function", "void", "return", "null", "class", "new", "pow", "default");
            this.MarkTransient(INSTRUCCION, DECLARACION, INSTRUCCION_CLASE, INSTRUCCION_FUNCION, INSTRUCCION_METODO, DIM);
            #endregion
        }
    }
}
