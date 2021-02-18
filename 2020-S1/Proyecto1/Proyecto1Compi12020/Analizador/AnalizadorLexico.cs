using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Proyecto1Compi12020.Modelos;
using System.Text.RegularExpressions;
using System.Xml;

namespace Proyecto1Compi12020.Analizador
{
    class AnalizadorLexico
    {
        public Lista listaTokens { get; set; }
        public Lista listaErrores { get; set; }

        private Regex identificador = new Regex("^(([a-zA-Z][a-zA-Z0-9_]*)|_[a-zA-Z][a-zA-Z0-9_]*)$");
        private Regex rango = new Regex("^.{1}~.{1}$");
        private Regex lista = new Regex("^.{1}(,.{1})*$");
        private Regex validos = new Regex("^[\x20-\x7D]$");
        public Dictionary<string, List<char>> conjuntos;
        public Dictionary<string, Lista> expresionesRegulares;
        public Dictionary<string, ArbolExpresiones> arboles;
        public List<string[]> evaluaciones;

        public AnalizadorLexico()
        {
            listaTokens = new Lista();
            listaErrores = new Lista();
            conjuntos = new Dictionary<String, List<char>>();
            expresionesRegulares = new Dictionary<String, Lista>();
            arboles = new Dictionary<string, ArbolExpresiones>();
            evaluaciones = new List<string[]>();
        }

        public void AnalizarCodigo(String codigo)
        {
            StringReader code = new StringReader(codigo);
            String line = null;
            int contadorAuxiliar = 0;

            //Parsing Variables
            String lexeme = null;
            String id = null;
            int column = 0;
            int row = 0;

            //Control Variables
            bool multiLineComment = false;

            /* state = 0 -> normal
             * state = 1 -> conjunto
             * state = 2 -> expresion
             * state = 3 -> evaluacion
             */
            int state = 0;
            Lista tmpToken = new Lista();
            while (true)
            {
                line = code.ReadLine();
                if (line != null)
                {
                    for (column = 0; column < line.Length; column++)
                    {
                        switch (state)
                        {
                            #region Area de Conjunto
                            case 1:
                                switch (line[column])
                                {
                                    case ';':
                                        if (!lexeme.Equals(""))
                                        {
                                            if (identificador.IsMatch(lexeme))
                                            {
                                                id = lexeme;
                                                listaTokens.AddNode(new Node(new Token("Identificador", lexeme, column, row)));
                                            }
                                            else
                                            {
                                                if (rango.IsMatch(lexeme))
                                                {
                                                    listaTokens.AddNode(new Node(new Token("Rango", lexeme, column, row)));
                                                }
                                                if (lista.IsMatch(lexeme))
                                                {
                                                    listaTokens.AddNode(new Node(new Token("Lista", lexeme, column, row)));
                                                }
                                                conjuntos.Add(id, generarConjunto(lexeme));
                                                id = "";
                                            }
                                            lexeme = "";
                                        }
                                        state = 0;
                                        break;
                                    case ':':
                                        listaTokens.AddNode(new Node(new Token("Dos Puntos", ":", column, row)));
                                        break;
                                    case ' ':
                                        if (!lexeme.Equals(""))
                                        {
                                            if (identificador.IsMatch(lexeme))
                                            {
                                                id = lexeme;
                                                listaTokens.AddNode(new Node(new Token("Identificador", lexeme, column, row)));
                                            }
                                            else
                                            {
                                                if (rango.IsMatch(lexeme))
                                                {
                                                    listaTokens.AddNode(new Node(new Token("Rango", lexeme, column, row)));
                                                }
                                                if (lista.IsMatch(lexeme))
                                                {
                                                    listaTokens.AddNode(new Node(new Token("Lista", lexeme, column, row)));
                                                }
                                                conjuntos.Add(id, generarConjunto(lexeme));
                                                id = "";
                                            }
                                            lexeme = "";
                                        }
                                        break;
                                    case '-':
                                        if (!lexeme.Equals(""))
                                        {
                                            if (identificador.IsMatch(lexeme))
                                            {
                                                id = lexeme;
                                                listaTokens.AddNode(new Node(new Token("Identificador", lexeme, column, row)));
                                            }
                                            lexeme = "";
                                        }
                                        listaTokens.AddNode(new Node(new Token("Guion", "-", column, row)));
                                        break;
                                    case '>':
                                        if (contadorAuxiliar > 0)
                                        {
                                            lexeme += ">";
                                        }
                                        else
                                        {
                                            listaTokens.AddNode(new Node(new Token("Mayor Que", ">", column, row)));
                                        }
                                        contadorAuxiliar++;
                                        break;
                                    case '~':
                                        lexeme += "~";
                                        break;
                                    default:
                                        if (validos.IsMatch(Char.ToString(line[column])))
                                        {
                                            lexeme += line[column];
                                        }
                                        else
                                        {
                                            listaErrores.AddNode(new Node(new Error(Char.ToString(line[column]), column, row)));
                                            lexeme = "";
                                        }
                                        break;
                                }
                                break;
                            #endregion Area de Conjunto

                            #region Area de Expresion
                            case 2:
                                switch (line[column])
                                {
                                    #region Simbolos de Expresiones Regulares
                                    case '.':
                                        listaTokens.AddNode(new Node(new Token("Concatenacion", ".", column, row)));
                                        tmpToken.AddNode(new Node(new Token("Concatenacion", ".", column, row)));
                                        break;
                                    case '?':
                                        listaTokens.AddNode(new Node(new Token("Llave 1 o 0", "?", column, row)));
                                        tmpToken.AddNode(new Node(new Token("Llave 1 o 0", "?", column, row)));
                                        break;
                                    case '|':
                                        listaTokens.AddNode(new Node(new Token("Disyuncion", "|", column, row)));
                                        tmpToken.AddNode(new Node(new Token("Disyuncion", "|", column, row)));
                                        break;
                                    case '+':
                                        listaTokens.AddNode(new Node(new Token("Llave Positiva", "+", column, row)));
                                        tmpToken.AddNode(new Node(new Token("Llave Positiva", "+", column, row)));
                                        break;
                                    case '*':
                                        listaTokens.AddNode(new Node(new Token("Llave Kleene", "*", column, row)));
                                        tmpToken.AddNode(new Node(new Token("Llave Kleene", "*", column, row)));
                                        break;
                                    #endregion Simbolos de Expresiones Regulares

                                    #region Caracteres especiales
                                    case '\\':
                                        column++;
                                        switch (line[column])
                                        {
                                            case 'n':
                                                //Caracter especial: nueva linea
                                                listaTokens.AddNode(new Node(new Token("Salto de Linea", "\\\\n", column, row)));
                                                tmpToken.AddNode(new Node(new Token("Salto de Linea", "\\\\n", column, row)));
                                                break;
                                            case '\'':
                                                //Caracter especial: comilla simple
                                                listaTokens.AddNode(new Node(new Token("Comilla Simple", "\\\\'", column, row)));
                                                tmpToken.AddNode(new Node(new Token("Comilla Simple", "\\\\'", column, row)));
                                                break;
                                            case '\"':
                                                //Caracter especial: comilla doble
                                                listaTokens.AddNode(new Node(new Token("Comilla Doble", "\\\\\"", column, row)));
                                                tmpToken.AddNode(new Node(new Token("Comilla Doble", "\\\\\"", column, row)));
                                                break;
                                            case 't':
                                                //Caracter especial: tabulacion
                                                listaTokens.AddNode(new Node(new Token("Tabulacion", "\\\\t", column, row)));
                                                tmpToken.AddNode(new Node(new Token("Tabulacion", "\\\\t", column, row)));
                                                break;
                                        }
                                        break;
                                    case '[':
                                        //Caracter especial: todo menos salto de linea
                                        listaTokens.AddNode(new Node(new Token("Todo", "[:todo:]", column, row)));
                                        tmpToken.AddNode(new Node(new Token("Todo", "[:todo:]", column, row)));
                                        column += 7;
                                        break;
                                    #endregion Caracteres especiales

                                    #region Cadenas, caracteres y variables
                                    case '\"':
                                        column++;
                                        while (line[column] != '\"')
                                        {
                                            lexeme += line[column];
                                            column++;
                                        }
                                        listaTokens.AddNode(new Node(new Token("Cadena", lexeme, column, row)));
                                        tmpToken.AddNode(new Node(new Token("Cadena", lexeme, column, row)));
                                        lexeme = "";
                                        break;
                                    case '\'':
                                        column++;
                                        if(line[column] != '\'')
                                        {
                                            lexeme = Char.ToString(line[column]);
                                        }
                                        else
                                        {
                                            lexeme = "";
                                        }
                                        listaTokens.AddNode(new Node(new Token("Caracter", lexeme, column, row)));
                                        tmpToken.AddNode(new Node(new Token("Caracter", lexeme, column, row)));
                                        lexeme = "";
                                        break;
                                    case '{':
                                        column++;
                                        while (line[column] != '}')
                                        {
                                            lexeme += line[column];
                                            column++;
                                        }
                                        listaTokens.AddNode(new Node(new Token("Variable", lexeme, column, row)));
                                        tmpToken.AddNode(new Node(new Token("Variable", lexeme, column, row)));
                                        lexeme = "";
                                        break;
                                    #endregion Cadenas, caracteres y variables

                                    #region Otros y errores
                                    case ' ':
                                        break;
                                    case ';':
                                        expresionesRegulares.Add(id, tmpToken);
                                        arboles.Add(id, new ArbolExpresiones(tmpToken));
                                        state = 0;
                                        break;
                                    default:
                                        listaErrores.AddNode(new Node(new Error(Char.ToString(line[column]), column, row)));
                                        break;
                                    #endregion Otros y errores
                                }
                                break;
                            #endregion Area de Expresion

                            #region Area de Evaluacion
                            case 3:
                                switch (line[column])
                                {
                                    case '\"':
                                        column++;
                                        while (line[column] != '\"')
                                        {
                                            lexeme += line[column];
                                            column++;
                                        }
                                        string[] temp = new string[2] {id, lexeme};
                                        evaluaciones.Add(temp);
                                        listaTokens.AddNode(new Node(new Token("Cadena", lexeme, column, row)));
                                        lexeme = "";
                                        break;
                                    case ' ':
                                        break;
                                    case ';':
                                        state = 0;
                                        break;
                                }
                                break;
                            #endregion Area de Evaluacion
                        }

                        //Comprobar el estado para hacer el siguiente analisis
                        if (state != 0) continue;

                        #region Comentario Multi Linea
                        //Comprobar si es un comentario multi linea para ignorar los caracteres
                        if (multiLineComment)
                        {
                            switch (line[column])
                            {
                                case '!':
                                    if (line[column + 1] == '>')
                                    {
                                        column++;
                                        multiLineComment = false;
                                    }
                                    break;
                            }
                        }
                        #endregion Comentario Multi Linea

                        #region Area aparte
                        else
                        {
                            switch (line[column])
                            {
                                case '/':
                                    if (line[column + 1] == '/')
                                    {
                                        column = line.Length;
                                    }
                                    break;
                                case '<':
                                    if (line[column + 1] == '!')
                                    {
                                        column++;
                                        multiLineComment = true;
                                    }
                                    break;
                                case '!':
                                    if (line[column + 1] == '>')
                                    {
                                        column++;
                                        multiLineComment = false;
                                    }
                                    break;
                                case ' ':
                                    if (!lexeme.Equals(""))
                                    {
                                        if (lexeme.Equals("CONJ"))
                                        {
                                            listaTokens.AddNode(new Node(new Token("Conjunto", lexeme, column, row)));
                                            state = 1;
                                        }
                                        else if (identificador.IsMatch(lexeme))
                                        {
                                            id = lexeme;
                                            listaTokens.AddNode(new Node(new Token("Identificador", lexeme, column, row)));
                                        }
                                        lexeme = "";
                                    }
                                    break;
                                case '-':
                                    listaTokens.AddNode(new Node(new Token("Gioun", "-", column, row)));
                                    column++;
                                    listaTokens.AddNode(new Node(new Token("Mayor Que", ">", column, row)));
                                    tmpToken = new Lista();
                                    state = 2;
                                    break;
                                case ':':
                                    if (lexeme.Equals("CONJ"))
                                    {
                                        listaTokens.AddNode(new Node(new Token("Conjunto", lexeme, column, row)));
                                        lexeme = "";
                                        contadorAuxiliar = 0;
                                        state = 1;
                                    }
                                    else if (identificador.IsMatch(lexeme))
                                    {
                                        id = lexeme;
                                        listaTokens.AddNode(new Node(new Token("Identificador", lexeme, column, row)));
                                        lexeme = "";
                                        state = 3;
                                    }
                                    listaTokens.AddNode(new Node(new Token("Dos Puntos", ":", column, row)));
                                    break;
                                case ';':
                                    listaTokens.AddNode(new Node(new Token("Punto Coma", ";", column, row)));
                                    break;
                                default:
                                    if (validos.IsMatch(Char.ToString(line[column])))
                                    {
                                        lexeme += line[column];
                                    }
                                    else
                                    {
                                        listaErrores.AddNode(new Node(new Error(Char.ToString(line[column]), column, row)));
                                        lexeme = "";
                                    }
                                    break;
                            }
                        }
                        #endregion Area aparte
                    }
                }
                else
                {
                    break;
                }
                row++;
            }
        }

        private List<char> generarConjunto(string lexeme)
        {
            List<char> retorno = new List<char>();

            #region Rango
            if (lexeme.Contains("~"))
            {
                int inicio = Convert.ToInt32(lexeme[0]);
                int final = Convert.ToInt32(lexeme[2]);

                #region 0-9 A-Z a-z
                if ((48 <= inicio && inicio <= 57) || (65 <= inicio && inicio <= 90) || (97 <= inicio && inicio <= 122))
                {
                    for (int i = inicio; i <= final; i++)
                    {
                        retorno.Add(Convert.ToChar(i));
                    }
                }
                #endregion 0-9 A-Z a-z

                #region Simbolos
                else
                {
                    for (int i = inicio; i <= final; i++)
                    {
                        if ((48 <= i && i <= 57) || (65 <= i && i <= 90) || (97 <= i && i <= 122)) { }
                        else retorno.Add(Convert.ToChar(i));
                    }
                }
                #endregion Simbolos
            }
            #endregion

            #region Lista
            else
            {
                int posicion = 0;
                while(posicion <= lexeme.Length)
                {
                    retorno.Add(lexeme[posicion]);
                    posicion += 2;
                }
            }
            #endregion Lista

            return retorno;
        }

        public void generarXMLTokens()
        {
            XmlDocument doc = new XmlDocument();
            XmlDeclaration xmlDeclaration = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
            XmlElement root = doc.DocumentElement;
            doc.InsertBefore(xmlDeclaration, root);
            XmlElement element1 = doc.CreateElement(string.Empty, "ListaTokens", string.Empty);
            doc.AppendChild(element1);

            XmlElement element2;
            XmlElement element3;
            XmlElement element4;
            XmlElement element5;
            XmlElement element6;

            XmlText nombre;
            XmlText valor;
            XmlText fila;
            XmlText columna;
            Node tmp = listaTokens.Head;
            while (tmp != null)
            {
                element2 = doc.CreateElement(string.Empty, "Token", string.Empty);
                element1.AppendChild(element2);

                element3 = doc.CreateElement(string.Empty, "Nombre", string.Empty);
                nombre = doc.CreateTextNode(tmp.Token.Name);
                element3.AppendChild(nombre);
                element2.AppendChild(element3);

                element4 = doc.CreateElement(string.Empty, "Valor", string.Empty);
                valor = doc.CreateTextNode(tmp.Token.Value.Replace("▀", "\n"));
                element4.AppendChild(valor);
                element2.AppendChild(element4);

                element5 = doc.CreateElement(string.Empty, "Fila", string.Empty);
                fila = doc.CreateTextNode(tmp.Token.Row + "");
                element5.AppendChild(fila);
                element2.AppendChild(element5);

                element6 = doc.CreateElement(string.Empty, "Columna", string.Empty);
                columna = doc.CreateTextNode(tmp.Token.Column + "");
                element6.AppendChild(columna);
                element2.AppendChild(element6);

                tmp = tmp.Next;
            }

            doc.Save("C:\\Users\\USER\\Desktop\\PruebasProyecto1\\XML\\Tokens.xml");
        }

        public void generarXMLErrores()
        {
            XmlDocument doc = new XmlDocument();
            XmlDeclaration xmlDeclaration = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
            XmlElement root = doc.DocumentElement;
            doc.InsertBefore(xmlDeclaration, root);
            XmlElement element1 = doc.CreateElement(string.Empty, "ListaErrores", string.Empty);
            doc.AppendChild(element1);

            XmlElement element2;
            XmlElement element4;
            XmlElement element5;
            XmlElement element6;

            XmlText valor;
            XmlText fila;
            XmlText columna;
            Node tmp = listaErrores.Head;
            while (tmp != null)
            {
                element2 = doc.CreateElement(string.Empty, "Error", string.Empty);
                element1.AppendChild(element2);

                element4 = doc.CreateElement(string.Empty, "Valor", string.Empty);
                valor = doc.CreateTextNode(tmp.Error.Value.Replace("▀", "\n"));
                element4.AppendChild(valor);
                element2.AppendChild(element4);

                element5 = doc.CreateElement(string.Empty, "Fila", string.Empty);
                fila = doc.CreateTextNode(tmp.Error.Row + "");
                element5.AppendChild(fila);
                element2.AppendChild(element5);

                element6 = doc.CreateElement(string.Empty, "Columna", string.Empty);
                columna = doc.CreateTextNode(tmp.Error.Column + "");
                element6.AppendChild(columna);
                element2.AppendChild(element6);

                tmp = tmp.Next;
            }

            doc.Save("C:\\Users\\USER\\Desktop\\PruebasProyecto1\\XML\\Errores.xml");
        }
    }
}
