using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Irony.Parsing;

namespace _OLC1_Proyecto2
{
    class Gramatica:Grammar
    {
        public Gramatica() : base(false)
        {
            #region Expresiones lenguajes
            CommentTerminal Multicoment = new CommentTerminal("ComentarioMultiple", "<-", "->"); // varias lineas
            CommentTerminal SingleComent = new CommentTerminal("SingleComment", ">>", "\n", "\r\n"); // una linea
            base.NonGrammarTerminals.Add(Multicoment);
            base.NonGrammarTerminals.Add(SingleComent);
            // TEMERNIMALES----------------------------------------->
            StringLiteral tipotext = new StringLiteral("Text", "\""); // String comas
            StringLiteral txtchar = new StringLiteral("TChar", "'"); // tipo de dato char

            NumberLiteral tipointeger = new NumberLiteral("Integer"); // Integer 
            RegexBasedTerminal tipoDouble = new RegexBasedTerminal("Double", "[0-9+[.][0-9]+"); // Double
            RegexBasedTerminal thexa = new RegexBasedTerminal("hexa", "^(\\#)[0-9A-F]+$"); // Identificador
            IdentifierTerminal idproc2 = new IdentifierTerminal("idproc2"); // Identificador2
            IdentifierTerminal variable = new IdentifierTerminal("variable"); //  nueva
            IdentifierTerminal variable3 = new IdentifierTerminal("variable3"); //  nueva

            #endregion
            // Palabras en la gramatica                
            #region reservadas
            var tint = ("int");
            var tboolean = ("bool");
            var ttrue = ("true");
            var tfalse = ("false");
            var tdouble = ("double");
            var tchar = ("char");
            var tstring = ("string");
            var tarray = ("array");
            var tclase = ("clase");
            var timportar = ("importar");
            var tpublico = ("publico");
            var tprivado = ("privado");
           
            var toverride = ("override");
          
           
      
            var tsalir = ("salir");
     
            var tvoid = ("void");
          
            #endregion



            RegisterOperators(1, Associativity.Left, "+", "-");
            RegisterOperators(2, Associativity.Left, "*", "/");
            RegisterOperators(3, Associativity.Left, "^");
            RegisterOperators(4, Associativity.Left, "==", "!=", "<", ">", "<=", ">=");
            RegisterOperators(1, Associativity.Left, "||");
            RegisterOperators(1, Associativity.Left, "&&");
            RegisterOperators(1, Associativity.Left, "!");
            RegisterOperators(1, Associativity.Left, "(", ")");







            //-----------------> PRODUCCIONES GRAMATICAS---------------------------
            NonTerminal INICIO = new NonTerminal("INICIO"),
                        VISIBILIDAD = new NonTerminal("VISIBILIDAD"),
                        ID = new NonTerminal("ID"),
                        EXTENDS = new NonTerminal("EXTENDS"),
                        INICIO2 = new NonTerminal("INICIO2"),
                        LISTACLASES = new NonTerminal("LITACLASES"),
                        CUERPO = new NonTerminal("CUERPO"),
                        DECLARACION = new NonTerminal("DECLARACION"),
                        ASIGNACION = new NonTerminal("ASIGNACION"),
                        AUMENTODISMINUCION = new NonTerminal("AUMENTODISMINUCION"),
                        IMPRIMIR = new NonTerminal("IMPRIMIR"),
                        MOSTRAR = new NonTerminal("MOSTRAR"),
                        IF = new NonTerminal("IF"),
                        FOR = new NonTerminal("FOR"),
                        REPEAT = new NonTerminal("REPEAT"),
                        WHILE = new NonTerminal("WHILE"),
                        COMPROBAR = new NonTerminal("COMPROBAR"),
                        DOWHILE = new NonTerminal("DOWHILE"),
                        FIGURE = new NonTerminal("FIGURE"),
                        ADDFIGURE = new NonTerminal("ADDFIGURE"),
                        METODO = new NonTerminal("METODO"),
                        FUNCION = new NonTerminal("FUNCION"),
                        MAIN = new NonTerminal("MAIN"),
                        LLAMADAF = new NonTerminal("LLAMADAF"),
                        CONTINUAR = new NonTerminal("CONTINUAR"),
                        SALIR = new NonTerminal("SALIR"),
                        ASIGNACION2 = new NonTerminal("ASIGNACION2"),
                        DECLARACION2 = new NonTerminal("DECLARACION2"),
                        OBJECTOS = new NonTerminal("OBJETOS"),
                        ARREGLO = new NonTerminal("ARREGLO"),
                        TIPO = new NonTerminal("TIPO"),
                        TIPO2 = new NonTerminal("TIPO2"),
                        ASIGNACION3 = new NonTerminal("ASIGNACION3"),
                        E = new NonTerminal("E"),
                        ARREGLO2 = new NonTerminal("ARREGLO2"),
                        ASIGNARRR = new NonTerminal("ASIGNARR"),
                        ASIGNARR2 = new NonTerminal("ASIGNARR2"),
                        ASIGNARR3 = new NonTerminal("ASIGNARR3"),
                        LISTAARR = new NonTerminal("LISTAAARR"),
                        TIPOVALORES = new NonTerminal("TIPOVALORES"),
                        ELSE = new NonTerminal("ELSE"),
                        CONDICIONFOR = new NonTerminal("CONDICIONFOR"),
                        DATOS = new NonTerminal("DATOS"),
                        POSICION = new NonTerminal("POSICION"),
                        FUNCION2 = new NonTerminal("FUNCION2"),
                        PARAMETROS = new NonTerminal("PARAMETROS"),
                        OVERRIDE = new NonTerminal("OVERRIDE"),
                        DIMENSION = new NonTerminal("DIMENSION"),
                        EXTENDS2 = new NonTerminal("EXTENDS2"),
                        DIMENSION2 = new NonTerminal("DIMENSION2"),
                        CASOS = new NonTerminal("CASOS"),
                        DEFAULT = new NonTerminal("DEFAULT"),
                        CASOS2 = new NonTerminal("CASOS2"),
                        GEOMETRICAS = new NonTerminal("GEOMETRICAS"),
                        COLOR = new NonTerminal("COLOR"),
                        COLOR2 = new NonTerminal("COLOR2"),
                        PARAMETROS2 = new NonTerminal("PARAMETROS2"),
                        VAR2 = new NonTerminal("VAR2"),
                        FROMCLASS = new NonTerminal("FROMCLASS"),
                        CASOSALIR = new NonTerminal("CASOSALIR"),
                        LLAMADARECURSIVA = new NonTerminal("LLAMADARECURSIVA"),
                        PARAMETROSV = new NonTerminal("PARAMETROSV"),
                        PARAMETROSV2 = new NonTerminal("PARAMETROSV2");
            //-------------------------------------- COMIENZO DE LAS REGLAS SINTACTICAS------------------------------------


            INICIO.Rule =  VISIBILIDAD + tclase + ID + EXTENDS+ "{" +INICIO2+ "}" +LISTACLASES;
            
            
            LISTACLASES.Rule = LISTACLASES + VISIBILIDAD + tclase + ID + EXTENDS + "{" +INICIO2+ "}"
            | Empty;
           

            INICIO2.Rule = INICIO2 + CUERPO
                    | CUERPO
                    | Empty;
            CUERPO.Rule = DECLARACION // listo jose
                         | ASIGNACION // LISTO
                         | AUMENTODISMINUCION // LISTO
                         | IMPRIMIR // LISTO
                         | MOSTRAR // LISTO
                         | IF // LISTO
                         | FOR // LISTO
                         | REPEAT// LISTO
                         | WHILE // LISTO
                         | COMPROBAR // LISTO 
                         | DOWHILE // LISTO
                         | FIGURE // LISTO
                         | ADDFIGURE // LISTO
                         | METODO// LISTO
                         | FUNCION // LISTO COMPONER VISIBILIDAD
                         | MAIN // LISTO
                         | LLAMADAF; // LISTO

                        
            ASIGNACION.Rule = VISIBILIDAD+ variable + ASIGNACION2 + ";";
            ASIGNACION2.Rule = "=" + E;

            DECLARACION.Rule = VISIBILIDAD + TIPO + DECLARACION2;
            DECLARACION2.Rule = OBJECTOS + ";"
                               | tarray + ID + ARREGLO + ";";
            DECLARACION.ErrorRule = SyntaxError + ";";

            TIPO.Rule = 
                     ToTerm("int")
                    | ToTerm("bool")
                    | ToTerm("string")
                    | ToTerm("double")
                    | ToTerm("char")
                    | variable;
                    
                    

            VISIBILIDAD.Rule = Empty
                              | tpublico
                              | tprivado;
                              

            OBJECTOS.Rule =   "," + ID + ASIGNACION3 + OBJECTOS
                           | ID + ASIGNACION3;

            ASIGNACION3.Rule = "=" + E
                               | Empty;

            ARREGLO.Rule = "[" + E + "]" + ARREGLO2;
            ARREGLO2.Rule = "[" + E + "]" + ARREGLO2
                           | "=" + ASIGNARRR
                           | Empty;

            ASIGNARRR.Rule = "{" + ASIGNARR2 + "}";
            ASIGNARR2.Rule = ASIGNARR3
                            | LISTAARR;
            ASIGNARR3.Rule = ASIGNARRR
                            | ASIGNARR3 + "," + ASIGNARRR;
            LISTAARR.Rule = LISTAARR + "," + E
                            | E;

            TIPOVALORES.Rule = tipointeger
                              | tipoDouble
                              | tipotext
                              | txtchar
                              | ttrue
                              | tfalse;

            IMPRIMIR.Rule = ToTerm("print") +   "(" + E + ")" + ";";

            MOSTRAR.Rule = ToTerm("show") + "(" + E + "," + E + ")" + ";";

            IF.Rule = ToTerm("if") + "(" + E + ")" + "{" + INICIO2 + "}" + ELSE;

            ELSE.Rule = ToTerm("else") + IF
                        | ToTerm("else") + "{" + INICIO2 + "}"
                        | Empty;
            FOR.Rule = ToTerm("for") + "(" + CONDICIONFOR + E + ";" + AUMENTODISMINUCION + ")" + "{" + INICIO2 + "}";

            REPEAT.Rule = ToTerm("repeat") + "(" + E + ")" + "{" + INICIO2 + "}";

            CONDICIONFOR.Rule = DECLARACION
                               | ASIGNACION;

            AUMENTODISMINUCION.Rule = DATOS + "++"
                                    | DATOS + "--";

            DATOS.Rule = TIPOVALORES
                         | ID + POSICION;

            POSICION.Rule = TIPOVALORES + "[" + E + "]" + POSICION
                           | Empty;

            ID.Rule = idproc2;

            E.Rule = E + "+" + E
                        | E + "-" + E
                        | E + "*" + E
                        | E + "/" + E
                        | E + "^" + E
                        | E + "==" + E
                        | E + "!=" + E
                        | E + "<" + E
                        | E + ">" + E
                        | E + ">=" + E
                        | E + "<=" + E
                        | E + "||" + E
                        | E + "&&" + E
                        | E + "!" + E
                        | "-" + E
                        | "(" + E + ")"
                        | DATOS
                        | ToTerm("new") + variable + "(" + PARAMETROSV + ")"
                        | ID + "." + ID + LLAMADARECURSIVA
                        | FROMCLASS;

            LLAMADARECURSIVA.Rule = "(" + PARAMETROSV + ")"
                                | "." + ID + LLAMADARECURSIVA
                                | Empty;
            PARAMETROSV.Rule = E + PARAMETROSV2
                                | Empty;
            PARAMETROSV2.Rule = "," + E + PARAMETROSV2
                                | Empty;

            FROMCLASS.Rule = ToTerm("print") + ID + "(" + PARAMETROS + ")";
            

            FUNCION.Rule = VISIBILIDAD + variable+ FUNCION2 + "(" + PARAMETROS + ")" + "{" + INICIO2 + ToTerm("return") + E + ";" + "}";
            FUNCION2.Rule = TIPO2 + OVERRIDE
                            | tarray + TIPO + DIMENSION + OVERRIDE;
            TIPO2.Rule = ToTerm("int")
                        | ToTerm("string")
                        | ToTerm("char")
                        | ToTerm("double")
                        | ToTerm("bool");
                
                     

            METODO.Rule = VISIBILIDAD + variable + tvoid +OVERRIDE + "(" + PARAMETROS + ")" + "{" + INICIO2 + "}";

            EXTENDS.Rule =Empty 
                    |ToTerm("importar") +ID + EXTENDS2;
            EXTENDS2.Rule = Empty
                | "," + ID + EXTENDS2;
                            
            OVERRIDE.Rule = Empty
                        | ToTerm("override");

            DIMENSION.Rule = "[" + "]" + DIMENSION2;
            DIMENSION2.Rule = "[" + "]" + DIMENSION2
                            | Empty;

            MAIN.Rule = ToTerm("main") + "(" + ")" + "{" + INICIO2 + "}";

            LLAMADAF.Rule = VISIBILIDAD + variable+ "(" + PARAMETROSV + ")" + ";";

            WHILE.Rule = ToTerm("while") + "(" + E + ")" + "{" + INICIO2 + "}";

            COMPROBAR.Rule = ToTerm("comprobar") + "(" + E + ")" + "{" + CASOS + DEFAULT + "}";

            CASOS.Rule = ToTerm("caso") + E + ":" + INICIO2 + tsalir + ";" + CASOS2;
            CASOS2.Rule = ToTerm("caso") + E + ":" + INICIO2 + tsalir + ";" + CASOS2
                          | Empty;

            DEFAULT.Rule = ToTerm("defecto") + ":" + INICIO2 + CASOSALIR
                           | Empty;
            CASOSALIR.Rule = Empty
                    | ToTerm("salir") + ";";

            DOWHILE.Rule = ToTerm("hacer") + "{" + INICIO2 + "}" + ToTerm("mientras") + "(" + E + ")" + ";";

            FIGURE.Rule = ToTerm("figure") + "(" + E + ")" + ";";

            ADDFIGURE.Rule = ToTerm("addfigure") + "(" + GEOMETRICAS + ")" + ";";

            GEOMETRICAS.Rule = ToTerm("circle") + "(" + COLOR + "," + E + "," + E + "," + E + "," + E + ")"
                               | ToTerm("triangle") + "(" + COLOR + "," + E + "," + E + "," + E + "," + E + "," + E + "," +E+ "," + E  + ")"
                               | ToTerm("square") + "(" + COLOR + "," + E + "," + E + "," + E + "," + E + "," + E +  ")"
                               | ToTerm("line") + "(" + COLOR + "," + E + "," + E + "," + E + "," + E + "," + E + ")";

            COLOR.Rule = "\"" + COLOR2 + "\""
                        | thexa + COLOR;

            COLOR.Rule = Empty
                        | tipotext
                        | ID;
            PARAMETROS.Rule = TIPO + PARAMETROS2
                            | Empty;
            PARAMETROS2.Rule = ID + VAR2
                    | tarray + ID + ARREGLO + VAR2;

            VAR2.Rule = "," + TIPO + PARAMETROS2
                | Empty;

       




            this.Root = INICIO;
            
        }
    }
}
