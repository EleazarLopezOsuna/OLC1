using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Irony.Parsing;
using Proyecto2_201700893.Modelos;

namespace Proyecto2_201700893.Analizador
{
    class Semantico
    {
        public ArrayList Consola;
        public ArrayList Errores;
        public bool parar;
        private bool continuar;
        private Entorno entornoGlobal;
        private Expresion retorno;
        private NodoSintactico rootArbol = null;
        private int contadorNodos = 0;
        private ArrayList MetodosClasesFunciones;
        private Dictionary<String, Clase> Clases;
        private String path = "C://Users//USER//Desktop//Pruebas Proyecto 2 - Compi 1//";

        public void IniciarAnalisisSemantico(NodoSintactico root)
        {
            entornoGlobal = new Entorno(null);
            Consola = new ArrayList();
            Errores = new ArrayList();
            Clases = new Dictionary<String, Clase>();
            MetodosClasesFunciones = new ArrayList();
            reconocerImports(root, entornoGlobal, "2_0_1_7_0_0_8_9_3");
            reconocerMetodosFuncionesClases(root, entornoGlobal, 0, "2_0_1_7_0_0_8_9_3");
            recorrer(root, entornoGlobal, "2_0_1_7_0_0_8_9_3");
            parar = false;
            continuar = false;
            retorno = null;
        }

        private void reconocerImports(NodoSintactico root, Entorno ent, String archivo)
        {
            switch (root.getNombre())
            {
                case "INICIO":
                    foreach (NodoSintactico hijo in root.getHijos())
                    {
                        reconocerImports(hijo, ent, archivo);
                    }
                    break;
                case "GLOBAL":
                    foreach (NodoSintactico hijo in root.getHijos())
                    {
                        reconocerImports(hijo, ent, archivo);
                    }
                    break;
                case "IMPORT":
                    if (root.getHijos().Count == 0)
                    {
                        if (File.Exists(path + root.getValor().ToString()))
                        {
                            String texto = File.ReadAllText(path + root.getValor().ToString());
                            ParseTree resultadoAnalisis = Sintactico.Analizar(texto);
                            if (resultadoAnalisis != null)
                            {
                                if (resultadoAnalisis.ParserMessages.Count == 0)
                                {
                                    Entorno nuevo = new Entorno(null);
                                    recorrerArbol(rootArbol, resultadoAnalisis.Root);
                                    reconocerImports(rootArbol, nuevo, root.getValor().ToString());
                                    reconocerMetodosFuncionesClases(rootArbol, nuevo, 1, root.getValor().ToString());
                                    recorrerSinMain(rootArbol, nuevo, root.getValor().ToString());
                                }
                                else
                                {
                                    foreach (Irony.LogMessage error in resultadoAnalisis.ParserMessages)
                                    {
                                        Error reportarError;
                                        if (error.Message.Contains("Invalid character"))
                                        {
                                            String errorx = error.Message.Replace("Invalid character: ", "");
                                            errorx = errorx.Remove(errorx.Length - 1);
                                            reportarError = new Error(root.getValor().ToString(), "0", "0", "Lexico", errorx);
                                        }
                                        else
                                        {
                                            String errorx = error.Message.Replace("Syntax error, expected: ", "");
                                            reportarError = new Error(root.getValor().ToString(), "0", "0", "Sintactico", errorx);
                                        }
                                        Errores.Add(reportarError);
                                    }
                                }
                            }
                        }
                        else
                        {
                            Error reportarError = new Error(archivo, "0", "0", "Semantico", "El archivo " + root.getValor().ToString() + " no existe");
                            Errores.Add(reportarError);
                        }
                    }
                    else
                    {
                        foreach (NodoSintactico hijo in root.getHijos())
                        {
                            if (File.Exists(path + hijo.getValor().ToString()))
                            {
                                String texto = File.ReadAllText(path + hijo.getValor().ToString());
                                ParseTree resultadoAnalisis = Sintactico.Analizar(texto);
                                if (resultadoAnalisis != null)
                                {
                                    if (resultadoAnalisis.ParserMessages.Count == 0)
                                    {
                                        Entorno nuevo = new Entorno(null);
                                        recorrerArbol(rootArbol, resultadoAnalisis.Root);
                                        reconocerImports(rootArbol, nuevo, root.getValor().ToString());
                                        reconocerMetodosFuncionesClases(rootArbol, nuevo, 1, root.getValor().ToString());
                                        recorrerSinMain(rootArbol, nuevo, root.getValor().ToString());
                                    }
                                    else
                                    {
                                        foreach (Irony.LogMessage error in resultadoAnalisis.ParserMessages)
                                        {
                                            Error reportarError;
                                            if (error.Message.Contains("Invalid character"))
                                            {
                                                String errorx = error.Message.Replace("Invalid character: ", "");
                                                errorx = errorx.Remove(errorx.Length - 1);
                                                reportarError = new Error(root.getValor().ToString(), "0", "0", "Lexico", errorx);
                                            }
                                            else
                                            {
                                                String errorx = error.Message.Replace("Syntax error, expected: ", "");
                                                reportarError = new Error(root.getValor().ToString(), "0", "0", "Sintactico", errorx);
                                            }
                                            Errores.Add(reportarError);
                                        }
                                    }
                                }
                            }
                            else
                            {
                                Error reportarError = new Error(archivo, "0", "0", "Semantico", "El archivo " + root.getValor().ToString() + " no existe");
                                Errores.Add(reportarError);
                            }
                        }
                    }
                    break;
                case "INSTRUCCION":
                    foreach (NodoSintactico hijo in root.getHijos())
                    {
                        reconocerImports(hijo, ent, archivo);
                    }
                    break;
            }
        }

        private void recorrerSinMain(NodoSintactico root, Entorno ent, String archivo)
        {
            if (!parar && !continuar)
            {

                {
                    Expresion resultado;
                    Entorno nuevo;
                    ArrayList listaHijos;
                    NodoSintactico auxiliar;
                    NodoSintactico auxiliar2;
                    switch (root.getNombre())
                    {
                        case "INICIO":
                            foreach (NodoSintactico hijo in root.getHijos())
                            {
                                recorrerSinMain(hijo, ent, archivo);
                            }
                            break;
                        case "GLOBAL":
                            foreach (NodoSintactico hijo in root.getHijos())
                            {
                                recorrerSinMain(hijo, ent, archivo);
                            }
                            break;
                        case "INSTRUCCION":
                            foreach (NodoSintactico hijo in root.getHijos())
                            {
                                recorrerSinMain(hijo, ent, archivo);
                            }
                            break;
                        case "BREAK":
                            parar = true;
                            break;
                        case "CONTINUE":
                            continuar = true;
                            break;
                        case "ALERT":
                            listaHijos = root.getHijos();
                            resultado = resolverExpresion((NodoSintactico)listaHijos[0], ent, archivo);
                            if (resultado.tipo != Simbolo.EnumTipo.error)
                            {
                                MessageBox.Show(resultado.valor.ToString());
                            }
                            else
                            {
                                Error reportarError = new Error(archivo, "0", "0", "Semantico", resultado.valor.ToString());
                                Errores.Add(reportarError);
                            }
                            break;
                        case "GRAPH":
                            EjecutarGrafica(root, ent, archivo);
                            break;
                        case "LOG":
                            listaHijos = root.getHijos();
                            resultado = resolverExpresion((NodoSintactico)listaHijos[0], ent, archivo);
                            if (resultado.tipo != Simbolo.EnumTipo.error)
                            {
                                Consola.Add(resultado.valor.ToString());
                            }
                            else
                            {
                                Error reportarError = new Error(archivo, "0", "0", "Semantico", resultado.valor.ToString());
                                Errores.Add(reportarError);
                            }
                            break;
                        case "DECLARACION":
                            listaHijos = root.getHijos();
                            auxiliar = (NodoSintactico)listaHijos[0];
                            switch (auxiliar.getNombre())
                            {
                                case "DIM0":
                                    ejecutarDeclaracion(auxiliar, ent, 0, archivo);
                                    break;
                                case "DIM1":
                                    ejecutarDeclaracion(auxiliar, ent, 1, archivo);
                                    break;
                                case "DIM2":
                                    ejecutarDeclaracion(auxiliar, ent, 2, archivo);
                                    break;
                                case "DIM3":
                                    ejecutarDeclaracion(auxiliar, ent, 3, archivo);
                                    break;
                            }
                            break;
                        case "RETURN":
                            listaHijos = root.getHijos();
                            retorno = resolverExpresion((NodoSintactico)listaHijos[0], ent, archivo);
                            break;
                        case "REASIGNACION":
                            listaHijos = root.getHijos();
                            auxiliar = (NodoSintactico)listaHijos[0];
                            auxiliar2 = (NodoSintactico)listaHijos[1];
                            ejecutarReasignacion(auxiliar, auxiliar2, ent, archivo);
                            break;
                        case "WHILE":
                            nuevo = new Entorno(ent);
                            EjecutarMientras(root, nuevo, archivo);
                            parar = false;
                            continuar = false;
                            break;
                        case "DOWHILE":
                            nuevo = new Entorno(ent);
                            EjecutarDoWhile(root, nuevo, archivo);
                            parar = false;
                            continuar = false;
                            break;
                        case "FOR":
                            nuevo = new Entorno(ent);
                            EjecutarFor(root, nuevo, archivo);
                            parar = false;
                            continuar = false;
                            break;
                        case "SWITCH":
                            nuevo = new Entorno(ent);
                            EjecutarSwitch(root, nuevo, archivo);
                            break;
                        case "EOTRO":
                            listaHijos = root.getHijos();
                            if (listaHijos.Count == 3)
                            {
                                NodoSintactico auxi = (NodoSintactico)listaHijos[0];
                                NodoSintactico nuevoRoot = new NodoSintactico(root.getNombre(), root.getNumNodo());
                                for (int i = 1; i < root.getHijos().Count; i++)
                                {
                                    nuevoRoot.addHijo((NodoSintactico)listaHijos[i]);
                                }
                                nuevoRoot.setValor(root.getValor());
                                EjecutarLlamadoMetodo(nuevoRoot, ent, entornoGlobal, archivo, auxi.getValor().ToString());
                            }
                            else
                            {
                                EjecutarLlamadoMetodo(root, ent, entornoGlobal, archivo, "");
                            }
                            break;
                        case "IF":
                            nuevo = new Entorno(ent);
                            listaHijos = root.getHijos();
                            resultado = resolverExpresion((NodoSintactico)listaHijos[0], ent, archivo);
                            if (resultado.tipo != Simbolo.EnumTipo.error)
                            {
                                if (resultado.tipo == Simbolo.EnumTipo.boleano)
                                {
                                    //Tiene Bloque de condiciones y bloque (SI y NO)
                                    if (listaHijos.Count == 3)
                                    {
                                        //El resultado es true
                                        if (resultado.valor.ToString().ToLower().Equals("true"))
                                        {
                                            recorrerSinMain((NodoSintactico)listaHijos[1], nuevo, archivo);
                                        }
                                        //El resultado es false
                                        else if (resultado.valor.ToString().ToLower().Equals("false"))
                                        {
                                            recorrerSinMain((NodoSintactico)listaHijos[2], nuevo, archivo);
                                        }
                                    }
                                    //Tiene Bloque de condiciones y bloque (SI o NO)
                                    else if (listaHijos.Count == 2)
                                    {
                                        if (((NodoSintactico)listaHijos[1]).getNombre().Equals("ELSE"))
                                        {
                                            //El resultado es false
                                            if (resultado.valor.ToString().ToLower().Equals("false"))
                                            {
                                                recorrerSinMain((NodoSintactico)listaHijos[1], nuevo, archivo);
                                            }
                                        }
                                        else
                                        {
                                            //El resultado es true
                                            if (resultado.valor.ToString().ToLower().Equals("true"))
                                            {
                                                recorrerSinMain((NodoSintactico)listaHijos[1], nuevo, archivo);
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    Error reportarError = new Error(archivo, "0", "0", "Semantico", "Se esperaba tipo boleano, tipo " + resultado.tipo + " encontrado");
                                    Errores.Add(reportarError);
                                }
                            }
                            else
                            {
                                Error reportarError = new Error(archivo, "0", "0", "Semantico", resultado.valor.ToString());
                                Errores.Add(reportarError);
                            }
                            break;
                        case "ELSE":
                            nuevo = new Entorno(ent);
                            foreach (NodoSintactico hijo in root.getHijos())
                            {
                                recorrerSinMain(hijo, nuevo, archivo);
                            }
                            break;
                    }
                }
            }
        }

        private void reconocerMetodosFuncionesClases(NodoSintactico root, Entorno ent, int tipo, String archivo)
        {
            String nombre;
            Simbolo sim;
            Object[] datos;
            int numeroParametros;
            ArrayList listaHijos = root.getHijos();
            Entorno entornoParaAlmacenarVariables;
            Entorno entornoParaAlmacenarMetodosFunciones;
            switch (root.getNombre())
            {
                case "INICIO":
                    foreach (NodoSintactico hijo in root.getHijos())
                    {
                        reconocerMetodosFuncionesClases(hijo, ent, tipo, archivo);
                    }
                    break;
                case "GLOBAL":
                    foreach (NodoSintactico hijo in root.getHijos())
                    {
                        reconocerMetodosFuncionesClases(hijo, ent, tipo, archivo);
                    }
                    break;
                case "MAIN":
                    sim = ent.buscar("main", 0, 0);
                    if (sim == null)
                    {
                        sim = new Simbolo(Simbolo.EnumTipo.metodo, root);
                        ent.insertar("main", sim, 0, 0);
                    }
                    else
                    {
                        Error reportarError = new Error(archivo, "0", "0", "Semantico", "Main duplicado");
                        Errores.Add(reportarError);
                    }
                    break;
                case "FUNCION":
                    nombre = ((NodoSintactico)listaHijos[0]).getValor().ToString();
                    sim = ent.buscar(nombre, 0, 0);
                    if (sim == null)
                    {
                        if (root.getHijos().Count == 3)
                        {
                            numeroParametros = ((NodoSintactico)listaHijos[1]).getHijos().Count;
                            datos = new Object[2] { numeroParametros, root };

                            sim = new Simbolo(Simbolo.EnumTipo.funcion, datos);
                            ent.insertar(nombre, sim, 0, 0);
                            Object[] valores = new Object[] { nombre, ent };
                            MetodosClasesFunciones.Add(valores);
                        }
                        else if (root.getHijos().Count == 2)
                        {
                            if (((NodoSintactico)listaHijos[1]).getNombre().Equals("PARAMETROS"))
                            {
                                numeroParametros = ((NodoSintactico)listaHijos[1]).getHijos().Count;
                                datos = new Object[2] { numeroParametros, null };

                                sim = new Simbolo(Simbolo.EnumTipo.funcion, datos);
                                ent.insertar(nombre, sim, 0, 0);
                                Object[] valores = new Object[] { nombre, ent };
                                MetodosClasesFunciones.Add(valores);
                            }
                            else
                            {
                                datos = new Object[2] { 0, root };

                                sim = new Simbolo(Simbolo.EnumTipo.funcion, datos);
                                ent.insertar(nombre, sim, 0, 0);
                                Object[] valores = new Object[] { nombre, ent };
                                MetodosClasesFunciones.Add(valores);
                            }
                        }
                    }
                    else
                    {
                        Error reportarError = new Error(archivo, "0", "0", "Semantico", "Funcion " + nombre + " duplicada");
                        Errores.Add(reportarError);
                    }
                    break;
                case "CLASE":
                    nombre = ((NodoSintactico)listaHijos[0]).getValor().ToString();
                    sim = ent.buscar(nombre, 0, 0);
                    if (sim == null)
                    {
                        if (root.getHijos().Count == 2)
                        {
                            entornoParaAlmacenarVariables = new Entorno(null);
                            entornoParaAlmacenarMetodosFunciones = new Entorno(null);
                            reconocerMetodosFuncionesClases((NodoSintactico)listaHijos[1], entornoParaAlmacenarMetodosFunciones, 0, archivo);
                            //reconocerMetodosFuncionesClases((NodoSintactico)listaHijos[1], entornoParaAlmacenarMetodosFunciones, 1, archivo);
                            recorrer((NodoSintactico)listaHijos[1], entornoParaAlmacenarVariables, archivo);

                            /*foreach (String llave in entornoParaAlmacenarMetodosFunciones.tabla.Keys)
                            {
                                entornoParaAlmacenarMetodosFunciones.tabla.TryGetValue(llave, out Simbolo valor);
                                if (valor != null)
                                {
                                    Object[] objeto = (Object[])valor.valor;
                                    MessageBox.Show(llave + "   ");
                                }
                            }*/

                            entornoParaAlmacenarVariables.anterior = entornoGlobal;
                            entornoParaAlmacenarMetodosFunciones.anterior = entornoParaAlmacenarVariables;

                            Clase clase = new Clase(entornoParaAlmacenarVariables, entornoParaAlmacenarMetodosFunciones);
                            if (!Clases.ContainsKey(nombre))
                            {
                                Clases.Add(nombre, clase);
                            }
                            else
                            {
                                Error reportarError = new Error(archivo, "0", "0", "Semantico", "Clase " + nombre + " duplicada");
                                Errores.Add(reportarError);
                            }
                        }
                    }
                    else
                    {
                        Error reportarError = new Error(archivo, "0", "0", "Semantico", "Clase " + nombre + " duplicada");
                        Errores.Add(reportarError);
                    }
                    break;
                case "METODO":
                    nombre = ((NodoSintactico)listaHijos[0]).getValor().ToString();
                    sim = ent.buscar(nombre, 0, 0);
                    if (sim == null)
                    {
                        if (root.getHijos().Count == 3)
                        {
                            numeroParametros = ((NodoSintactico)listaHijos[1]).getHijos().Count;
                            datos = new Object[2] { numeroParametros, root };

                            sim = new Simbolo(Simbolo.EnumTipo.metodo, datos);
                            ent.insertar(nombre, sim, 0, 0);
                            Object[] valores = new Object[] { nombre, ent };
                            MetodosClasesFunciones.Add(valores);
                        }
                        else if (root.getHijos().Count == 2)
                        {
                            if (((NodoSintactico)listaHijos[1]).getNombre().Equals("PARAMETROS"))
                            {
                                numeroParametros = ((NodoSintactico)listaHijos[1]).getHijos().Count;
                                datos = new Object[2] { numeroParametros, null };

                                sim = new Simbolo(Simbolo.EnumTipo.metodo, datos);
                                ent.insertar(nombre, sim, 0, 0);
                                Object[] valores = new Object[] { nombre, ent };
                                MetodosClasesFunciones.Add(valores);
                            }
                            else
                            {
                                datos = new Object[2] { 0, root };

                                sim = new Simbolo(Simbolo.EnumTipo.metodo, datos);
                                ent.insertar(nombre, sim, 0, 0);
                                Object[] valores = new Object[] { nombre, ent };
                                MetodosClasesFunciones.Add(valores);
                            }
                        }
                    }
                    else
                    {
                        Error reportarError = new Error(archivo, "0", "0", "Semantico", "Metodo " + nombre + " duplicado");
                        Errores.Add(reportarError);
                    }
                    break;
                case "INSTRUCCION":
                    foreach (NodoSintactico hijo in root.getHijos())
                    {
                        reconocerMetodosFuncionesClases(hijo, ent, tipo, archivo);
                    }
                    break;
            }
        }

        private void EjecutarGrafica(NodoSintactico root, Entorno ent, String archivo)
        {
            ArrayList listaHijos = root.getHijos();
            NodoSintactico nombre = (NodoSintactico)listaHijos[0];
            NodoSintactico contenido = (NodoSintactico)listaHijos[1];
            Expresion eNombre = resolverExpresion(nombre, ent, archivo);
            Expresion eContenido = resolverExpresion(contenido, ent, archivo);
            switch (eNombre.tipo)
            {
                case Simbolo.EnumTipo.error:
                    Error reportarError = new Error(archivo, "0", "0", "Semantico", "" + eNombre.valor);
                    Errores.Add(reportarError);
                    break;
                case Simbolo.EnumTipo.cadena:
                    switch (eContenido.tipo)
                    {
                        case Simbolo.EnumTipo.error:
                            reportarError = new Error(archivo, "0", "0", "Semantico", "" + eContenido.valor);
                            Errores.Add(reportarError);
                            break;
                        case Simbolo.EnumTipo.cadena:
                            //Creamos el archivo de texto donde estara el codigo de graphViz
                            String nombreTexto = Path.GetFileNameWithoutExtension(eNombre.valor.ToString());
                            String extension = Path.GetExtension(eNombre.valor.ToString());

                            using (FileStream fs = File.Create(nombreTexto + ".txt"))
                            {
                                Byte[] info = new UTF8Encoding(true).GetBytes(eContenido.valor.ToString());
                                // Add some information to the file.
                                fs.Write(info, 0, info.Length);
                            }

                            //Llamamos al CMD y pasamos como parametros los comandos que solicita dot
                            Process process = new Process();
                            ProcessStartInfo startInfo = new ProcessStartInfo();
                            startInfo.WindowStyle = ProcessWindowStyle.Hidden;
                            startInfo.FileName = "cmd.exe";
                            String mostrar = "/C dot -T" + extension.Replace(".", "") + " " + nombreTexto + ".txt -o " + nombreTexto + extension;
                            startInfo.Arguments = mostrar;
                            process.StartInfo = startInfo;
                            process.Start();

                            //Abrimos la imagen
                            Thread.Sleep(1000);
                            Process.Start(Environment.CurrentDirectory + "/" + nombreTexto + extension);
                            break;
                        default:
                            reportarError = new Error(archivo, "0", "0", "Semantico", "Se esperaba tipo cadena, tipo " + eContenido.tipo + " encontrado");
                            Errores.Add(reportarError);
                            break;
                    }
                    break;
                default:
                    reportarError = new Error(archivo, "0", "0", "Semantico", "Se esperaba tipo cadena, tipo " + eNombre.tipo + " encontrado");
                    Errores.Add(reportarError);
                    break;
            }
        }

        private void recorrer(NodoSintactico root, Entorno ent, String archivo)
        {
            if (!parar && !continuar)
            {

                {
                    Expresion resultado;
                    Entorno nuevo;
                    ArrayList listaHijos;
                    NodoSintactico auxiliar;
                    NodoSintactico auxiliar2;
                    switch (root.getNombre())
                    {
                        case "INICIO":
                            foreach (NodoSintactico hijo in root.getHijos())
                            {
                                recorrer(hijo, ent, archivo);
                            }
                            break;
                        case "GLOBAL":
                            foreach (NodoSintactico hijo in root.getHijos())
                            {
                                recorrer(hijo, ent, archivo);
                            }
                            break;
                        case "MAIN":
                            nuevo = new Entorno(ent);
                            foreach (NodoSintactico hijo in root.getHijos())
                            {
                                recorrer(hijo, nuevo, archivo);
                            }
                            break;
                        case "INSTRUCCION":
                            foreach (NodoSintactico hijo in root.getHijos())
                            {
                                recorrer(hijo, ent, archivo);
                            }
                            break;
                        case "INCREMENTO":
                        case "DECREMENTO":
                            resolverExpresion(root, ent, archivo);
                            break;
                        case "BREAK":
                            parar = true;
                            break;
                        case "CONTINUE":
                            continuar = true;
                            break;
                        case "ALERT":
                            listaHijos = root.getHijos();
                            resultado = resolverExpresion((NodoSintactico)listaHijos[0], ent, archivo);
                            if (resultado.tipo != Simbolo.EnumTipo.error)
                            {
                                MessageBox.Show(resultado.valor.ToString());
                            }
                            else
                            {
                                Error reportarError = new Error(archivo, "0", "0", "Semantico", resultado.valor.ToString());
                                Errores.Add(reportarError);
                            }
                            break;
                        case "GRAPH":
                            EjecutarGrafica(root, ent, archivo);
                            break;
                        case "LOG":
                            listaHijos = root.getHijos();
                            resultado = resolverExpresion((NodoSintactico)listaHijos[0], ent, archivo);
                            if (resultado.tipo != Simbolo.EnumTipo.error)
                            {
                                Consola.Add(resultado.valor.ToString());
                            }
                            else
                            {
                                Error reportarError = new Error(archivo, "0", "0", "Semantico", resultado.valor.ToString());
                                Errores.Add(reportarError);
                            }
                            break;
                        case "DECLARACION":
                            listaHijos = root.getHijos();
                            auxiliar = (NodoSintactico)listaHijos[0];
                            switch (auxiliar.getNombre())
                            {
                                case "DIM0":
                                    ejecutarDeclaracion(auxiliar, ent, 0, archivo);
                                    break;
                                case "DIM1":
                                    ejecutarDeclaracion(auxiliar, ent, 1, archivo);
                                    break;
                                case "DIM2":
                                    ejecutarDeclaracion(auxiliar, ent, 2, archivo);
                                    break;
                                case "DIM3":
                                    ejecutarDeclaracion(auxiliar, ent, 3, archivo);
                                    break;
                            }
                            break;
                        case "RETURN":
                            listaHijos = root.getHijos();
                            retorno = resolverExpresion((NodoSintactico)listaHijos[0], ent, archivo);
                            break;
                        case "REASIGNACION":
                            listaHijos = root.getHijos();
                            if (listaHijos.Count != 3)
                            {
                                auxiliar = (NodoSintactico)listaHijos[0];
                                auxiliar2 = (NodoSintactico)listaHijos[1];
                                ejecutarReasignacion(auxiliar, auxiliar2, ent, archivo);
                            }
                            else
                            {
                                String nombreDelObjeto = ((NodoSintactico)listaHijos[0]).getValor().ToString();
                                String nombreDeLaVariable = ((NodoSintactico)listaHijos[1]).getValor().ToString();
                                resultado = resolverExpresion((NodoSintactico)listaHijos[2], ent, archivo);

                                Simbolo sim = ent.buscar(nombreDelObjeto, 0, 0);
                                if (sim != null)
                                {
                                    Clase cl = (Clase)sim.valor;
                                    Entorno entVariables = cl.contenedorDeVariables;

                                    Simbolo var = entVariables.buscar(nombreDeLaVariable, 0, 0);

                                    if (var != null)
                                    {
                                        if (resultado.tipo == Simbolo.EnumTipo.clase)
                                        {
                                            Clase clase = (Clase)resultado.valor;
                                            entVariables.modificar(nombreDeLaVariable, new Simbolo(Simbolo.EnumTipo.clase, clase));
                                        }
                                        else
                                        {
                                            entVariables.modificar(nombreDeLaVariable, new Simbolo(resultado.tipo, resultado.valor));
                                        }
                                    }
                                    else
                                    {
                                        Error reportarError = new Error(archivo, "0", "0", "Semantico", "La variable: " + nombreDeLaVariable + " no existe");
                                        Errores.Add(reportarError);
                                    }
                                }
                                else
                                {
                                    Error reportarError = new Error(archivo, "0", "0", "Semantico", "El Objeto: " + nombreDelObjeto + " no existe");
                                    Errores.Add(reportarError);
                                }
                            }
                            break;
                        case "WHILE":
                            nuevo = new Entorno(ent);
                            EjecutarMientras(root, nuevo, archivo);
                            parar = false;
                            continuar = false;
                            break;
                        case "DOWHILE":
                            nuevo = new Entorno(ent);
                            EjecutarDoWhile(root, nuevo, archivo);
                            parar = false;
                            continuar = false;
                            break;
                        case "FOR":
                            nuevo = new Entorno(ent);
                            EjecutarFor(root, nuevo, archivo);
                            parar = false;
                            continuar = false;
                            break;
                        case "SWITCH":
                            nuevo = new Entorno(ent);
                            EjecutarSwitch(root, nuevo, archivo);
                            break;
                        case "EOTRO":
                            listaHijos = root.getHijos();
                            if (listaHijos.Count == 3)
                            {
                                NodoSintactico auxi = (NodoSintactico)listaHijos[0];
                                NodoSintactico nuevoRoot = new NodoSintactico(root.getNombre(), root.getNumNodo());
                                for (int i = 1; i < root.getHijos().Count; i++)
                                {
                                    nuevoRoot.addHijo((NodoSintactico)listaHijos[i]);
                                }
                                nuevoRoot.setValor(root.getValor());
                                EjecutarLlamadoMetodo(nuevoRoot, ent, entornoGlobal, archivo, auxi.getValor().ToString());
                            }
                            else
                            {
                                EjecutarLlamadoMetodo(root, ent, entornoGlobal, archivo, "");
                            }
                            break;
                        case "IF":
                            nuevo = new Entorno(ent);
                            listaHijos = root.getHijos();
                            resultado = resolverExpresion((NodoSintactico)listaHijos[0], ent, archivo);
                            if (resultado.tipo != Simbolo.EnumTipo.error)
                            {
                                if (resultado.tipo == Simbolo.EnumTipo.boleano)
                                {
                                    //Tiene Bloque de condiciones y bloque (SI y NO)
                                    if (listaHijos.Count == 3)
                                    {
                                        //El resultado es true
                                        if (resultado.valor.ToString().ToLower().Equals("true"))
                                        {
                                            recorrer((NodoSintactico)listaHijos[1], nuevo, archivo);
                                        }
                                        //El resultado es false
                                        else if (resultado.valor.ToString().ToLower().Equals("false"))
                                        {
                                            recorrer((NodoSintactico)listaHijos[2], nuevo, archivo);
                                        }
                                    }
                                    //Tiene Bloque de condiciones y bloque (SI o NO)
                                    else if (listaHijos.Count == 2)
                                    {
                                        if (((NodoSintactico)listaHijos[1]).getNombre().Equals("ELSE"))
                                        {
                                            //El resultado es false
                                            if (resultado.valor.ToString().ToLower().Equals("false"))
                                            {
                                                recorrer((NodoSintactico)listaHijos[1], nuevo, archivo);
                                            }
                                        }
                                        else
                                        {
                                            //El resultado es true
                                            if (resultado.valor.ToString().ToLower().Equals("true"))
                                            {
                                                recorrer((NodoSintactico)listaHijos[1], nuevo, archivo);
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    Error reportarError = new Error(archivo, "0", "0", "Semantico", "Se esperaba tipo boleano, tipo" + resultado.tipo + " encontrado");
                                    Errores.Add(reportarError);
                                }
                            }
                            else
                            {
                                Error reportarError = new Error(archivo, "0", "0", "Semantico", resultado.valor.ToString());
                                Errores.Add(reportarError);
                            }
                            break;
                        case "ELSE":
                            nuevo = new Entorno(ent);
                            foreach (NodoSintactico hijo in root.getHijos())
                            {
                                recorrer(hijo, nuevo, archivo);
                            }
                            break;
                    }
                }
            }
        }

        private Expresion resolverEotro(NodoSintactico root, Entorno ent, String archivo, String nombreClase)
        {
            Error reportarError = null;
            ArrayList listaHijos = root.getHijos();
            ArrayList listaAuxiliar;
            NodoSintactico auxiliar = (NodoSintactico)listaHijos[0];
            Expresion resultado = resolverExpresion(auxiliar, entornoGlobal, archivo);
            bool cambiarEntorno = false;
            Entorno cambioEnto = null;

            if (resultado.tipo == Simbolo.EnumTipo.error)
            {
                Simbolo cl = ent.buscar(nombreClase, 0, 0);
                if (cl != null)
                {
                    try
                    {
                        Clase clase = (Clase)cl.valor;
                        Entorno entornoClase = clase.contenedorDeFuncionesYMetodos;
                        resultado = resolverExpresion(auxiliar, entornoClase, archivo);
                        if (resultado.tipo != Simbolo.EnumTipo.error)
                        {
                            cambiarEntorno = true;
                            cambioEnto = entornoClase;
                        }
                    }
                    catch (InvalidCastException)
                    {

                    }

                    /*foreach (String llave in clase.contenedorDeFuncionesYMetodos.tabla.Keys)
                    {
                        MessageBox.Show("FUNCIONES Y METODOS: " + llave);
                    }
                    foreach (String llave in clase.contenedorDeVariables.tabla.Keys)
                    {
                        clase.contenedorDeVariables.tabla.TryGetValue(llave, out Simbolo sim);
                        MessageBox.Show("VARIABLES: " + llave + " VALOR: " + sim.valor);
                    }*/

                }
            }

            if (resultado.tipo == Simbolo.EnumTipo.error)
            {
                foreach (Object[] dat in MetodosClasesFunciones)
                {
                    resultado = resolverExpresion(auxiliar, (Entorno)dat[1], archivo);
                    if (resultado.tipo != Simbolo.EnumTipo.error)
                    {
                        /*cambiarEntorno = true;
                        cambioEnto = (Entorno)dat[1];*/
                        break;
                    }
                }
            }
            Object[] datos;
            int numeroParametros;
            switch (resultado.tipo)
            {
                case Simbolo.EnumTipo.funcion:
                    datos = (Object[])resultado.valor;
                    numeroParametros = int.Parse(datos[0].ToString());
                    if (numeroParametros > 0)
                    {
                        if (listaHijos.Count == 2)
                        {
                            if (((NodoSintactico)listaHijos[1]).getHijos().Count == numeroParametros)
                            {
                                if (datos[1] != null)
                                {
                                    Entorno nuevo = new Entorno(entornoGlobal);
                                    listaAuxiliar = ((NodoSintactico)datos[1]).getHijos();
                                    if (EjecutarParametros((NodoSintactico)listaAuxiliar[1], (NodoSintactico)listaHijos[1], nuevo, ent, archivo))
                                    {
                                        listaHijos = ((NodoSintactico)datos[1]).getHijos();
                                        NodoSintactico instrucciones = (NodoSintactico)listaHijos[2];
                                        recorrer(instrucciones, nuevo, archivo);
                                    }
                                }
                            }
                            else
                            {
                                reportarError = new Error(archivo, "0", "0", "Semantico", "Se esperaban numeroParametros parametros, " + ((NodoSintactico)listaHijos[1]).getHijos().Count + " encontrados");
                                Errores.Add(reportarError);
                            }
                        }
                        else
                        {
                            reportarError = new Error(archivo, "0", "0", "Semantico", "Se esperaban " + numeroParametros + " parametros");
                            Errores.Add(reportarError);
                        }
                    }
                    else
                    {
                        if (listaHijos.Count == 1)
                        {
                            if (datos[1] != null)
                            {
                                //EjecutarParametros((NodoSintactico)datos[1], (NodoSintactico)listaHijos[1], ent, archivo);
                                listaHijos = ((NodoSintactico)datos[1]).getHijos();
                                NodoSintactico instrucciones = (NodoSintactico)listaHijos[1];
                                if (cambiarEntorno)
                                {
                                    recorrer(instrucciones, cambioEnto, archivo);
                                }
                                else
                                {
                                    recorrer(instrucciones, ent, archivo);
                                }
                            }
                        }
                        else
                        {
                            reportarError = new Error(archivo, "0", "0", "Semantico", "Se esperaban " + numeroParametros + " parametros");
                            Errores.Add(reportarError);
                        }
                    }
                    if (retorno == null)
                    {
                        return new Expresion(Simbolo.EnumTipo.error, "No se encontro ningun valor de retorno");
                    }
                    return retorno;
                case Simbolo.EnumTipo.clase:
                    break;
                case Simbolo.EnumTipo.error:
                    return resultado;
                default:
                    return new Expresion(Simbolo.EnumTipo.error, "Se esperaba funcion o clase, " + resultado.tipo + " encontrado");
            }
            return new Expresion(Simbolo.EnumTipo.error, "");
        }

        private bool EjecutarParametros(NodoSintactico esperados, NodoSintactico obtenidos, Entorno destino, Entorno fuente, String archivo)
        {
            ArrayList listaEsperados = esperados.getHijos();
            ArrayList listaObtenidos = obtenidos.getHijos();
            bool error = false;
            for (int i = 0; i < listaObtenidos.Count; i++)
            {
                Expresion resultado = resolverExpresion((NodoSintactico)listaObtenidos[i], fuente, archivo);
                if (resultado.tipo == Simbolo.EnumTipo.error)
                {
                    error = true;
                    break;
                }
            }

            if (!error)
            {
                for (int i = 0; i < listaObtenidos.Count; i++)
                {
                    Expresion resultado = resolverExpresion((NodoSintactico)listaObtenidos[i], fuente, archivo);
                    Simbolo nuevaVariable = new Simbolo(resultado.tipo, resultado.valor);
                    if (!destino.insertar(((NodoSintactico)listaEsperados[i]).getValor().ToString(), nuevaVariable, 0, 0))
                    {
                        error = true;
                        break;
                    }
                }
                if (!error)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        private void EjecutarLlamadoMetodo(NodoSintactico root, Entorno ent, Entorno global, String archivo, String nombreVariable)
        {
            Error reportarError = null;
            ArrayList listaHijos = root.getHijos();
            ArrayList listaAuxiliar;
            String nombre = ((NodoSintactico)listaHijos[0]).getValor().ToString();
            Simbolo sim = global.buscar(nombre, 0, 0);
            bool buscarAfuera = false;
            Entorno xx = null;
            bool esClase = false;
            Expresion resultado;

            if (sim == null)
            {
                if (nombreVariable != "")
                {
                    if (ent.tabla.ContainsKey(nombreVariable))
                    {
                        ent.tabla.TryGetValue(nombreVariable, out Simbolo s);
                        Clase cl = (Clase)s.valor;
                        Entorno entornoClase = cl.contenedorDeFuncionesYMetodos;
                        sim = entornoClase.buscar(nombre, 0, 0);
                        if (sim != null)
                        {
                            xx = entornoClase;
                            esClase = true;
                        }
                    }
                }
            }

            //Comprobar si el dato esta en otro archivo
            if (sim == null)
            {
                foreach (Object[] dat in MetodosClasesFunciones)
                {
                    if (dat[0].ToString().Equals(nombre))
                    {
                        sim = ((Entorno)dat[1]).buscar(nombre, 0, 0);
                        xx = (Entorno)dat[1];
                        buscarAfuera = true;
                        break;
                    }
                }
            }


            Object[] datos;
            if (sim != null)
            {
                //Comprobar si lo encontrado es una clase
                if (sim.tipo == Simbolo.EnumTipo.clase)
                {
                    try
                    {
                        Clase clase = (Clase)sim.valor;
                        Entorno entornoClase = clase.contenedorDeFuncionesYMetodos;
                        resultado = resolverExpresion((NodoSintactico)listaHijos[1], entornoClase, archivo);
                        String nombreMeFu = ((NodoSintactico)listaHijos[1]).getValor().ToString();
                        if (resultado.tipo == Simbolo.EnumTipo.error)
                        {
                            foreach (Object[] dat in MetodosClasesFunciones)
                            {
                                if (dat[0].ToString().Equals(nombreMeFu))
                                {
                                    sim = ((Entorno)dat[1]).buscar(nombreMeFu, 0, 0);
                                    xx = (Entorno)dat[1];
                                    buscarAfuera = true;
                                    break;
                                }
                            }
                        }
                        else
                        {
                            esClase = true;
                            xx = entornoClase;
                            sim = new Simbolo(resultado.tipo, resultado.valor);
                        }
                    }
                    catch (InvalidCastException)
                    {

                    }
                }
                if (sim.tipo == Simbolo.EnumTipo.metodo)
                {
                    datos = (Object[])sim.valor;
                    int numeroParametros = int.Parse(datos[0].ToString());
                    if (numeroParametros > 0)
                    {
                        if (listaHijos.Count == 2)
                        {
                            if (((NodoSintactico)listaHijos[1]).getHijos().Count == numeroParametros)
                            {
                                if (datos[1] != null)
                                {
                                    Entorno nuevo = new Entorno(global);
                                    if (buscarAfuera)
                                    {
                                        nuevo = new Entorno(xx);
                                    }
                                    if (esClase)
                                    {
                                        nuevo = new Entorno(xx);
                                    }
                                    listaAuxiliar = ((NodoSintactico)datos[1]).getHijos();
                                    if (EjecutarParametros((NodoSintactico)listaAuxiliar[1], (NodoSintactico)listaHijos[1], nuevo, ent, archivo))
                                    {
                                        listaHijos = ((NodoSintactico)datos[1]).getHijos();
                                        NodoSintactico instrucciones = (NodoSintactico)listaHijos[2];
                                        recorrer(instrucciones, nuevo, archivo);
                                    }
                                }
                            }
                            else
                            {
                                reportarError = new Error(archivo, "0", "0", "Semantico", "Se esperaban numeroParametros parametros, " + ((NodoSintactico)listaHijos[1]).getHijos().Count + " encontrados");
                                Errores.Add(reportarError);
                            }
                        }
                        else
                        {
                            reportarError = new Error(archivo, "0", "0", "Semantico", "Se esperaban " + numeroParametros + " parametros");
                            Errores.Add(reportarError);
                        }
                    }
                    else
                    {
                        if (listaHijos.Count == 2 && ((NodoSintactico)listaHijos[1]).getNombre().Equals("PARAMETROS"))
                        {
                            reportarError = new Error(archivo, "0", "0", "Semantico", "Se esperaban " + numeroParametros + " parametros");
                            Errores.Add(reportarError);
                        }
                        else
                        {
                            if (datos[1] != null)
                            {
                                //EjecutarParametros((NodoSintactico)datos[1], (NodoSintactico)listaHijos[1], ent, archivo);
                                listaHijos = ((NodoSintactico)datos[1]).getHijos();
                                NodoSintactico instrucciones = (NodoSintactico)listaHijos[1];
                                if (buscarAfuera || esClase)
                                {
                                    recorrer(instrucciones, xx, archivo);
                                }
                                else
                                {
                                    recorrer(instrucciones, ent, archivo);
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                reportarError = new Error(archivo, "0", "0", "Semantico", "Metodo " + nombre + " no encontrado");
                Errores.Add(reportarError);
            }
        }

        private Dictionary<String, NodoSintactico> x(NodoSintactico root, Entorno ent, Simbolo.EnumTipo tipo, Dictionary<String, NodoSintactico> caso, String archivo)
        {
            Dictionary<String, NodoSintactico> aux = new Dictionary<String, NodoSintactico>();
            ArrayList listaHijos = root.getHijos();
            NodoSintactico auxiliar;
            ArrayList listaAuxiliar;
            Expresion resultado;
            Error reportarError = null;
            if (listaHijos.Count == 4)
            {
                auxiliar = (NodoSintactico)listaHijos[1];
                listaAuxiliar = auxiliar.getHijos();

                foreach (NodoSintactico hijo in listaAuxiliar)
                {
                    if (hijo.getNombre().Equals("DEFAULT"))
                    {
                        if (caso.ContainsKey("2997765060101"))
                        {
                            reportarError = new Error(archivo, "0", "0", "Semantico", "default duplicado");
                            Errores.Add(reportarError);
                            return null;
                        }
                        caso.Add("2997765060101", (NodoSintactico)listaHijos[2]);
                    }
                    else
                    {
                        resultado = resolverExpresion(hijo, ent, archivo);
                        if (resultado.tipo == tipo)
                        {
                            if (caso.ContainsKey(resultado.valor.ToString()))
                            {
                                reportarError = new Error(archivo, "0", "0", "Semantico", "Case " + resultado.valor + " duplicado");
                                Errores.Add(reportarError);
                                return null;
                            }
                            caso.Add(resultado.valor.ToString(), (NodoSintactico)listaHijos[2]);
                        }
                        else
                        {
                            reportarError = new Error(archivo, "0", "0", "Semantico", "Los tipos " + tipo + " y " + resultado.tipo + " no coinciden");
                            Errores.Add(reportarError);
                            return null;
                        }
                    }
                }
                //Hay mas cases
                aux = x((NodoSintactico)listaHijos[0], ent, tipo, caso, archivo);
                if (aux == null)
                {
                    return null;
                }
            }
            else
            {
                //Ya no hay mas cases
                auxiliar = (NodoSintactico)listaHijos[0];
                listaAuxiliar = auxiliar.getHijos();

                foreach (NodoSintactico hijo in listaAuxiliar)
                {
                    if (hijo.getNombre().Equals("DEFAULT"))
                    {
                        if (caso.ContainsKey("2997765060101"))
                        {
                            reportarError = new Error(archivo, "0", "0", "Semantico", "default duplicado");
                            Errores.Add(reportarError);
                            return null;
                        }
                        caso.Add("2997765060101", (NodoSintactico)listaHijos[1]);
                    }
                    else
                    {
                        resultado = resolverExpresion(hijo, ent, archivo);
                        if (resultado.tipo == tipo)
                        {
                            if (caso.ContainsKey(resultado.valor.ToString()))
                            {
                                reportarError = new Error(archivo, "0", "0", "Semantico", "Case " + resultado.valor + " duplicado");
                                Errores.Add(reportarError);
                                return null;
                            }
                            caso.Add(resultado.valor.ToString(), (NodoSintactico)listaHijos[1]);
                        }
                        else
                        {
                            reportarError = new Error(archivo, "0", "0", "Semantico", "Los tipos " + tipo + " y " + resultado.tipo + " no coinciden");
                            Errores.Add(reportarError);
                            return null;
                        }
                    }
                }
            }
            return caso;
        }

        private void EjecutarSwitch(NodoSintactico root, Entorno ent, String archivo)
        {
            Dictionary<String, NodoSintactico> caso = new Dictionary<String, NodoSintactico>();
            ArrayList listaHijos = root.getHijos();
            NodoSintactico auxiliar;
            Expresion resultado;
            Error reportarError = null;
            if (listaHijos.Count > 1)
            {
                resultado = resolverExpresion((NodoSintactico)listaHijos[0], ent, archivo);
                auxiliar = (NodoSintactico)listaHijos[1];
                if (resultado.tipo != Simbolo.EnumTipo.error)
                {
                    caso = x(auxiliar, ent, resultado.tipo, caso, archivo);
                    if (caso != null)
                    {
                        if (caso.ContainsKey(resultado.valor.ToString()))
                        {
                            caso.TryGetValue(resultado.valor.ToString(), out NodoSintactico nodo);
                            recorrer(nodo, ent, archivo);
                        }
                        else if (caso.ContainsKey("2997765060101"))
                        {
                            caso.TryGetValue("2997765060101", out NodoSintactico nodo);
                            recorrer(nodo, ent, archivo);
                        }
                        else
                        {
                            reportarError = new Error(archivo, "0", "0", "Semantico", "No existe case para aplicar");
                            Errores.Add(reportarError);
                        }
                    }
                }
                else
                {
                    reportarError = new Error(archivo, "0", "0", "Semantico", resultado.valor.ToString());
                    Errores.Add(reportarError);
                }
            }
        }

        private void EjecutarFor(NodoSintactico root, Entorno ent, String archivo)
        {
            ArrayList listaHijos = root.getHijos();
            ArrayList opcionesFor = ((NodoSintactico)listaHijos[0]).getHijos();
            ArrayList auxiliar;
            Error reportarError = null;
            //Ejecutamos la parte de control
            auxiliar = ((NodoSintactico)opcionesFor[0]).getHijos();
            if (auxiliar.Count == 1)
            {
                recorrer((NodoSintactico)opcionesFor[0], ent, archivo);
                //Obtenemos la condicion
                Expresion resultado = resolverExpresion((NodoSintactico)opcionesFor[1], ent, archivo);
                switch (resultado.tipo)
                {
                    case Simbolo.EnumTipo.boleano:
                        bool comprobador = bool.Parse(resultado.valor.ToString());
                        while (comprobador)
                        {
                            continuar = false;
                            parar = false;
                            Entorno nuevo = new Entorno(ent);
                            if (listaHijos.Count == 2)
                            {
                                recorrer((NodoSintactico)listaHijos[1], nuevo, archivo);
                            }

                            //Se ejecuta el update
                            resolverExpresion((NodoSintactico)opcionesFor[2], ent, archivo);

                            //Se comprueba la condicion
                            resultado = resolverExpresion((NodoSintactico)opcionesFor[1], ent, archivo);
                            if (resultado.valor.ToString().ToLower().Equals("true"))
                            {
                                comprobador = true;
                            }
                            else if (resultado.valor.ToString().ToLower().Equals("false"))
                            {
                                comprobador = false;
                            }
                            else
                            {
                                comprobador = false;
                            }
                            if (parar)
                            {
                                break;
                            }
                        }
                        break;
                    case Simbolo.EnumTipo.error:
                        reportarError = new Error(archivo, "0", "0", "Semantico", resultado.valor.ToString());
                        Errores.Add(reportarError);
                        break;
                    default:
                        reportarError = new Error(archivo, "0", "0", "Semantico", "Se esperaba tipo boleano, tipo" + resultado.tipo + " encontrado");
                        Errores.Add(reportarError);
                        break;
                }
            }
            else
            {
                reportarError = new Error(archivo, "0", "0", "Sintactico", "Se esperaba declaracion de una sola variable");
                Errores.Add(reportarError);
            }
        }

        private void EjecutarDoWhile(NodoSintactico root, Entorno ent, String archivo)
        {
            ArrayList listaHijos = root.getHijos();
            Expresion resultado = resolverExpresion((NodoSintactico)listaHijos[1], ent, archivo);
            Error reportarError = null;
            switch (resultado.tipo)
            {
                case Simbolo.EnumTipo.boleano:
                    bool comprobador;
                    do
                    {
                        continuar = false;
                        parar = false;

                        Entorno nuevo = new Entorno(ent);
                        if (listaHijos.Count == 2)
                        {
                            recorrer((NodoSintactico)listaHijos[1], nuevo, archivo);
                        }

                        resultado = resolverExpresion((NodoSintactico)listaHijos[1], ent, archivo);
                        if (resultado.valor.ToString().ToLower().Equals("true"))
                        {
                            comprobador = true;
                        }
                        else if (resultado.valor.ToString().ToLower().Equals("false"))
                        {
                            comprobador = false;
                        }
                        else
                        {
                            comprobador = false;
                        }
                        if (parar)
                        {
                            break;
                        }
                    } while (comprobador);
                    break;
                case Simbolo.EnumTipo.error:
                    reportarError = new Error(archivo, "0", "0", "Semantico", resultado.valor.ToString());
                    Errores.Add(reportarError);
                    break;
                default:
                    reportarError = new Error(archivo, "0", "0", "Semantico", "Se esperaba tipo boleano, tipo" + resultado.tipo + " encontrado");
                    Errores.Add(reportarError);
                    break;
            }
        }

        private void EjecutarMientras(NodoSintactico root, Entorno ent, String archivo)
        {
            ArrayList listaHijos = root.getHijos();
            Expresion resultado = resolverExpresion((NodoSintactico)listaHijos[0], ent, archivo);
            Error reportarError = null;
            switch (resultado.tipo)
            {
                case Simbolo.EnumTipo.boleano:
                    bool comprobador = bool.Parse(resultado.valor.ToString());
                    while (comprobador)
                    {
                        continuar = false;
                        parar = false;
                        Entorno nuevo = new Entorno(ent);
                        if (listaHijos.Count == 2)
                        {
                            recorrer((NodoSintactico)listaHijos[1], nuevo, archivo);
                        }
                        resultado = resolverExpresion((NodoSintactico)listaHijos[0], ent, archivo);
                        if (resultado.valor.ToString().ToLower().Equals("true"))
                        {
                            comprobador = true;
                        }
                        else if (resultado.valor.ToString().ToLower().Equals("false"))
                        {
                            comprobador = false;
                        }
                        else
                        {
                            comprobador = false;
                        }
                        if (parar)
                        {
                            break;
                        }
                    }
                    break;
                case Simbolo.EnumTipo.error:
                    reportarError = new Error(archivo, "0", "0", "Semantico", resultado.valor.ToString());
                    Errores.Add(reportarError);
                    break;
                default:
                    reportarError = new Error(archivo, "0", "0", "Semantico", "Se esperaba tipo boleano, tipo" + resultado.tipo + " encontrado");
                    Errores.Add(reportarError);
                    break;
            }
        }

        private void ejecutarReasignacion(NodoSintactico variable, NodoSintactico nuevo, Entorno ent, String archivo)
        {
            ArrayList listaHijos1 = variable.getHijos();
            ArrayList listaHijos2 = nuevo.getHijos();
            ArrayList listaHijos3;
            ArrayList listaHijos4;
            Simbolo[] simbolos = new Simbolo[10];
            Expresion[] datos = new Expresion[10];
            Simbolo[] resultados = new Simbolo[10];
            Expresion[] expresiones = new Expresion[10];
            Expresion[] index = new Expresion[10];
            NodoSintactico[] auxiliar = new NodoSintactico[10];
            Simbolo[] variableDim1;
            Simbolo[] nuevoDim1;
            Simbolo[,] variableDim2;
            Simbolo[,] nuevoDim2;
            Simbolo[,,] variableDim3;
            Simbolo[,,] nuevoDim3;
            bool sinError = true;
            Error reportarError = null;
            switch (variable.getNombre())
            {
                #region DIM1
                case "DIM1":
                    switch (nuevo.getNombre())
                    {
                        #region Arreglos
                        case "DIM1":
                        case "DIM2":
                        case "DIM3":
                            //Tomamos los nodos donde se encuentran los nombres de las variables
                            auxiliar[0] = (NodoSintactico)listaHijos1[0];
                            auxiliar[1] = (NodoSintactico)listaHijos2[0];
                            //Buscamos las variables para verificar el tipo de dato que son
                            simbolos[0] = ent.buscar(auxiliar[0].getValor().ToString(), 0, 0);
                            simbolos[1] = ent.buscar(auxiliar[1].getValor().ToString(), 0, 0);
                            if (simbolos[0] != null)
                            {
                                if (simbolos[1] != null)
                                {
                                    if (simbolos[0].tipo == Simbolo.EnumTipo.dim1)
                                    {
                                        switch (simbolos[1].tipo)
                                        {
                                            case Simbolo.EnumTipo.dim1:
                                                //Tomamos los nodos donde se encuentran los indices a modificar
                                                auxiliar[2] = (NodoSintactico)listaHijos1[1];
                                                auxiliar[3] = (NodoSintactico)listaHijos2[1];
                                                //Obtenemos los indices
                                                index[0] = resolverExpresion(auxiliar[2], ent, archivo);
                                                index[1] = resolverExpresion(auxiliar[3], ent, archivo);
                                                if (index[0].tipo != Simbolo.EnumTipo.error)
                                                {
                                                    if (index[0].tipo == Simbolo.EnumTipo.entero)
                                                    {
                                                        if (index[1].tipo != Simbolo.EnumTipo.error)
                                                        {
                                                            if (index[1].tipo == Simbolo.EnumTipo.entero)
                                                            {
                                                                //Operar reasignacion
                                                                variableDim1 = (Simbolo[])simbolos[0].valor;
                                                                nuevoDim1 = (Simbolo[])simbolos[1].valor;
                                                                if (variableDim1.GetLength(0) == nuevoDim1.GetLength(0))
                                                                {
                                                                    try
                                                                    {
                                                                        variableDim1[int.Parse(index[0].valor.ToString())] = nuevoDim1[int.Parse(index[1].valor.ToString())];
                                                                        resultados[0] = new Simbolo(Simbolo.EnumTipo.dim1, variableDim1);
                                                                        ent.modificar(auxiliar[0].getValor().ToString(), resultados[0]).ToString();
                                                                    }
                                                                    catch (IndexOutOfRangeException)
                                                                    {
                                                                        reportarError = new Error(archivo, "0", "0", "Semantico", "Index fuera del limite");
                                                                        Errores.Add(reportarError);
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    reportarError = new Error(archivo, "0", "0", "Semantico", "Los tamaños no coinciden");
                                                                    Errores.Add(reportarError);
                                                                }
                                                            }
                                                            else
                                                            {
                                                                reportarError = new Error(archivo, "0", "0", "Semantico", "Se esperaba tipo entero, tipo" + index[1].tipo + " encontrado");
                                                                Errores.Add(reportarError);
                                                            }
                                                        }
                                                        else
                                                        {
                                                            reportarError = new Error(archivo, "0", "0", "Semantico", "" + index[1].valor);
                                                            Errores.Add(reportarError);
                                                        }
                                                    }
                                                    else
                                                    {
                                                        reportarError = new Error(archivo, "0", "0", "Semantico", "Se esperaba tipo entero, tipo" + index[0].tipo + " encontrado");
                                                        Errores.Add(reportarError);
                                                    }
                                                }
                                                else
                                                {
                                                    reportarError = new Error(archivo, "0", "0", "Semantico", "" + index[0].valor);
                                                    Errores.Add(reportarError);
                                                }
                                                break;
                                            case Simbolo.EnumTipo.dim2:
                                                switch (listaHijos2.Count)
                                                {
                                                    case 3:
                                                        //Tomamos los nodos donde se encuentran los indices a modificar
                                                        auxiliar[2] = (NodoSintactico)listaHijos1[1];
                                                        auxiliar[3] = (NodoSintactico)listaHijos2[1];
                                                        auxiliar[4] = (NodoSintactico)listaHijos2[2];
                                                        //Obtenemos los indices
                                                        index[0] = resolverExpresion(auxiliar[2], ent, archivo);
                                                        index[1] = resolverExpresion(auxiliar[3], ent, archivo);
                                                        index[2] = resolverExpresion(auxiliar[4], ent, archivo);
                                                        if (index[0].tipo != Simbolo.EnumTipo.error)
                                                        {
                                                            if (index[0].tipo == Simbolo.EnumTipo.entero)
                                                            {
                                                                if (index[1].tipo != Simbolo.EnumTipo.error)
                                                                {
                                                                    if (index[1].tipo == Simbolo.EnumTipo.entero)
                                                                    {
                                                                        if (index[2].tipo != Simbolo.EnumTipo.error)
                                                                        {
                                                                            if (index[2].tipo == Simbolo.EnumTipo.entero)
                                                                            {
                                                                                //Operar reasignacion
                                                                                variableDim1 = (Simbolo[])simbolos[0].valor;
                                                                                nuevoDim2 = (Simbolo[,])simbolos[1].valor;
                                                                                if (variableDim1.GetLength(0) == nuevoDim2.GetLength(1))
                                                                                {
                                                                                    try
                                                                                    {
                                                                                        variableDim1[int.Parse(index[0].valor.ToString())] = nuevoDim2[int.Parse(index[1].valor.ToString()), int.Parse(index[2].valor.ToString())];
                                                                                        resultados[0] = new Simbolo(Simbolo.EnumTipo.dim1, variableDim1);
                                                                                        ent.modificar(auxiliar[0].getValor().ToString(), resultados[0]).ToString();
                                                                                    }
                                                                                    catch (IndexOutOfRangeException)
                                                                                    {
                                                                                        reportarError = new Error(archivo, "0", "0", "Semantico", "Index fuera del limite");
                                                                                        Errores.Add(reportarError);
                                                                                    }
                                                                                }
                                                                                else
                                                                                {
                                                                                    reportarError = new Error(archivo, "0", "0", "Semantico", "Los tamaños no coinciden");
                                                                                    Errores.Add(reportarError);
                                                                                }
                                                                            }
                                                                            else
                                                                            {
                                                                                reportarError = new Error(archivo, "0", "0", "Semantico", "Se esperaba tipo entero, tipo" + index[2].tipo + " encontrado");
                                                                                Errores.Add(reportarError);
                                                                            }
                                                                        }
                                                                        else
                                                                        {
                                                                            reportarError = new Error(archivo, "0", "0", "Semantico", "" + index[2].valor);
                                                                            Errores.Add(reportarError);
                                                                        }
                                                                    }
                                                                    else
                                                                    {
                                                                        reportarError = new Error(archivo, "0", "0", "Semantico", "Se esperaba tipo entero, tipo" + index[1].tipo + " encontrado");
                                                                        Errores.Add(reportarError);
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    reportarError = new Error(archivo, "0", "0", "Semantico", "" + index[1].valor);
                                                                    Errores.Add(reportarError);
                                                                }
                                                            }
                                                            else
                                                            {
                                                                reportarError = new Error(archivo, "0", "0", "Semantico", "Se esperaba tipo entero, tipo" + index[0].tipo + " encontrado");
                                                                Errores.Add(reportarError);
                                                            }
                                                        }
                                                        else
                                                        {
                                                            reportarError = new Error(archivo, "0", "0", "Semantico", "" + index[0].valor);
                                                            Errores.Add(reportarError);
                                                        }
                                                        break;
                                                    case 2:
                                                        reportarError = new Error(archivo, "0", "0", "Semantico", "Se esperaba dato, arreglo encontrado");
                                                        Errores.Add(reportarError);
                                                        break;
                                                }
                                                break;
                                            case Simbolo.EnumTipo.dim3:
                                                switch (listaHijos2.Count)
                                                {
                                                    case 4:
                                                        //Tomamos los nodos donde se encuentran los indices a modificar
                                                        auxiliar[2] = (NodoSintactico)listaHijos1[1];
                                                        auxiliar[3] = (NodoSintactico)listaHijos2[1];
                                                        auxiliar[4] = (NodoSintactico)listaHijos2[2];
                                                        auxiliar[5] = (NodoSintactico)listaHijos2[3];
                                                        //Obtenemos los indices
                                                        index[0] = resolverExpresion(auxiliar[2], ent, archivo);
                                                        index[1] = resolverExpresion(auxiliar[3], ent, archivo);
                                                        index[2] = resolverExpresion(auxiliar[4], ent, archivo);
                                                        index[3] = resolverExpresion(auxiliar[5], ent, archivo);
                                                        if (index[0].tipo != Simbolo.EnumTipo.error)
                                                        {
                                                            if (index[0].tipo == Simbolo.EnumTipo.entero)
                                                            {
                                                                if (index[1].tipo != Simbolo.EnumTipo.error)
                                                                {
                                                                    if (index[1].tipo == Simbolo.EnumTipo.entero)
                                                                    {
                                                                        if (index[2].tipo != Simbolo.EnumTipo.error)
                                                                        {
                                                                            if (index[2].tipo == Simbolo.EnumTipo.entero)
                                                                            {
                                                                                if (index[3].tipo != Simbolo.EnumTipo.error)
                                                                                {
                                                                                    if (index[3].tipo == Simbolo.EnumTipo.entero)
                                                                                    {
                                                                                        //Operar reasignacion
                                                                                        variableDim1 = (Simbolo[])simbolos[0].valor;
                                                                                        nuevoDim3 = (Simbolo[,,])simbolos[1].valor;
                                                                                        if (variableDim1.GetLength(0) == nuevoDim3.GetLength(2))
                                                                                        {
                                                                                            try
                                                                                            {
                                                                                                variableDim1[int.Parse(index[0].valor.ToString())] = nuevoDim3[int.Parse(index[1].valor.ToString()), int.Parse(index[2].valor.ToString()), int.Parse(index[3].valor.ToString())];
                                                                                                resultados[0] = new Simbolo(Simbolo.EnumTipo.dim1, variableDim1);
                                                                                                ent.modificar(auxiliar[0].getValor().ToString(), resultados[0]).ToString();
                                                                                            }
                                                                                            catch (IndexOutOfRangeException)
                                                                                            {
                                                                                                reportarError = new Error(archivo, "0", "0", "Semantico", "Index fuera del limite");
                                                                                                Errores.Add(reportarError);
                                                                                            }
                                                                                        }
                                                                                        else
                                                                                        {
                                                                                            reportarError = new Error(archivo, "0", "0", "Semantico", "Los tamaños no coinciden");
                                                                                            Errores.Add(reportarError);
                                                                                        }
                                                                                    }
                                                                                    else
                                                                                    {
                                                                                        reportarError = new Error(archivo, "0", "0", "Semantico", "" + index[3].valor);
                                                                                        Errores.Add(reportarError);
                                                                                    }
                                                                                }
                                                                                else
                                                                                {
                                                                                    reportarError = new Error(archivo, "0", "0", "Semantico", "Se esperaba tipo entero, tipo" + index[3].tipo + " encontrado");
                                                                                    Errores.Add(reportarError);
                                                                                }
                                                                            }
                                                                            else
                                                                            {
                                                                                reportarError = new Error(archivo, "0", "0", "Semantico", "Se esperaba tipo entero, tipo" + index[2].tipo + " encontrado");
                                                                                Errores.Add(reportarError);
                                                                            }
                                                                        }
                                                                        else
                                                                        {
                                                                            reportarError = new Error(archivo, "0", "0", "Semantico", "" + index[2].valor);
                                                                            Errores.Add(reportarError);
                                                                        }
                                                                    }
                                                                    else
                                                                    {
                                                                        reportarError = new Error(archivo, "0", "0", "Semantico", "Se esperaba tipo entero, tipo" + index[1].tipo + " encontrado");
                                                                        Errores.Add(reportarError);
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    reportarError = new Error(archivo, "0", "0", "Semantico", "" + index[1].valor);
                                                                    Errores.Add(reportarError);
                                                                }
                                                            }
                                                            else
                                                            {
                                                                reportarError = new Error(archivo, "0", "0", "Semantico", "Se esperaba tipo entero, tipo" + index[0].tipo + " encontrado");
                                                                Errores.Add(reportarError);
                                                            }
                                                        }
                                                        else
                                                        {
                                                            reportarError = new Error(archivo, "0", "0", "Semantico", "" + index[0].valor);
                                                            Errores.Add(reportarError);
                                                        }
                                                        break;
                                                    case 3:
                                                        reportarError = new Error(archivo, "0", "0", "Semantico", "Se esperaba dato, arreglo encontrado");
                                                        Errores.Add(reportarError);
                                                        break;
                                                    case 2:
                                                        reportarError = new Error(archivo, "0", "0", "Semantico", "Se esperaba dato, arreglo encontrado");
                                                        Errores.Add(reportarError);
                                                        break;
                                                }
                                                break;
                                        }
                                    }
                                    else if (simbolos[0].tipo == Simbolo.EnumTipo.dim2)
                                    {
                                        switch (simbolos[1].tipo)
                                        {
                                            case Simbolo.EnumTipo.dim1:
                                                if (listaHijos2.Count == 2)
                                                {
                                                    reportarError = new Error(archivo, "0", "0", "Semantico", "Se esperaba arreglo de 1 dimension, dato puntual encontrado");
                                                    Errores.Add(reportarError);
                                                }
                                                break;
                                            case Simbolo.EnumTipo.dim2:
                                                switch (listaHijos2.Count)
                                                {
                                                    case 2:
                                                        //Tomamos los nodos donde se encuentran los indices a modificar
                                                        auxiliar[2] = (NodoSintactico)listaHijos1[1];
                                                        auxiliar[3] = (NodoSintactico)listaHijos2[1];
                                                        //Obtenemos los indices
                                                        index[0] = resolverExpresion(auxiliar[2], ent, archivo);
                                                        index[1] = resolverExpresion(auxiliar[3], ent, archivo);
                                                        if (index[0].tipo != Simbolo.EnumTipo.error)
                                                        {
                                                            if (index[0].tipo == Simbolo.EnumTipo.entero)
                                                            {
                                                                if (index[1].tipo != Simbolo.EnumTipo.error)
                                                                {
                                                                    if (index[1].tipo == Simbolo.EnumTipo.entero)
                                                                    {
                                                                        //Operar reasignacion
                                                                        variableDim2 = (Simbolo[,])simbolos[0].valor;
                                                                        nuevoDim2 = (Simbolo[,])simbolos[1].valor;
                                                                        if (variableDim2.GetLength(1) == nuevoDim2.GetLength(1))
                                                                        {
                                                                            try
                                                                            {
                                                                                for (int i = 0; i < nuevoDim2.GetLength(1); i++)
                                                                                {
                                                                                    variableDim2[int.Parse(index[0].valor.ToString()), i] = nuevoDim2[int.Parse(index[1].valor.ToString()), i];
                                                                                    resultados[0] = new Simbolo(Simbolo.EnumTipo.dim2, variableDim2);
                                                                                    ent.modificar(auxiliar[0].getValor().ToString(), resultados[0]).ToString();
                                                                                }
                                                                            }
                                                                            catch (IndexOutOfRangeException)
                                                                            {
                                                                                reportarError = new Error(archivo, "0", "0", "Semantico", "Index fuera del limite");
                                                                                Errores.Add(reportarError);
                                                                            }
                                                                        }
                                                                        else
                                                                        {
                                                                            reportarError = new Error(archivo, "0", "0", "Semantico", "Los tamaños no coinciden");
                                                                            Errores.Add(reportarError);
                                                                        }
                                                                    }
                                                                    else
                                                                    {
                                                                        reportarError = new Error(archivo, "0", "0", "Semantico", "Se esperaba tipo entero, tipo" + index[1].tipo + " encontrado");
                                                                        Errores.Add(reportarError);
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    reportarError = new Error(archivo, "0", "0", "Semantico", "" + index[1].valor);
                                                                    Errores.Add(reportarError);
                                                                }
                                                            }
                                                            else
                                                            {
                                                                reportarError = new Error(archivo, "0", "0", "Semantico", "Se esperaba tipo entero, tipo" + index[0].tipo + " encontrado");
                                                                Errores.Add(reportarError);
                                                            }
                                                        }
                                                        else
                                                        {
                                                            reportarError = new Error(archivo, "0", "0", "Semantico", "" + index[0].valor);
                                                            Errores.Add(reportarError);
                                                        }
                                                        break;
                                                    case 3:
                                                        reportarError = new Error(archivo, "0", "0", "Semantico", "Se esperaba arreglo de 1 dimension, dato puntual encontrado");
                                                        Errores.Add(reportarError);
                                                        break;
                                                }
                                                break;
                                            case Simbolo.EnumTipo.dim3:
                                                switch (listaHijos2.Count)
                                                {
                                                    case 3:
                                                        //Tomamos los nodos donde se encuentran los indices a modificar
                                                        auxiliar[2] = (NodoSintactico)listaHijos1[1];
                                                        auxiliar[3] = (NodoSintactico)listaHijos2[1];
                                                        auxiliar[4] = (NodoSintactico)listaHijos2[2];
                                                        //Obtenemos los indices
                                                        index[0] = resolverExpresion(auxiliar[2], ent, archivo);
                                                        index[1] = resolverExpresion(auxiliar[3], ent, archivo);
                                                        index[2] = resolverExpresion(auxiliar[4], ent, archivo);
                                                        if (index[0].tipo != Simbolo.EnumTipo.error)
                                                        {
                                                            if (index[0].tipo == Simbolo.EnumTipo.entero)
                                                            {
                                                                if (index[1].tipo != Simbolo.EnumTipo.error)
                                                                {
                                                                    if (index[1].tipo == Simbolo.EnumTipo.entero)
                                                                    {
                                                                        if (index[2].tipo != Simbolo.EnumTipo.error)
                                                                        {
                                                                            if (index[2].tipo == Simbolo.EnumTipo.entero)
                                                                            {
                                                                                //Operar reasignacion
                                                                                variableDim2 = (Simbolo[,])simbolos[0].valor;
                                                                                nuevoDim3 = (Simbolo[,,])simbolos[1].valor;
                                                                                if (variableDim2.GetLength(1) == nuevoDim3.GetLength(2))
                                                                                {
                                                                                    try
                                                                                    {
                                                                                        for (int i = 0; i < nuevoDim3.GetLength(2); i++)
                                                                                        {
                                                                                            variableDim2[int.Parse(index[0].valor.ToString()), i] = nuevoDim3[int.Parse(index[1].valor.ToString()), int.Parse(index[2].valor.ToString()), i];
                                                                                            resultados[0] = new Simbolo(Simbolo.EnumTipo.dim2, variableDim2);
                                                                                            ent.modificar(auxiliar[0].getValor().ToString(), resultados[0]).ToString();
                                                                                        }
                                                                                    }
                                                                                    catch (IndexOutOfRangeException)
                                                                                    {
                                                                                        reportarError = new Error(archivo, "0", "0", "Semantico", "Index fuera del limite");
                                                                                        Errores.Add(reportarError);
                                                                                    }
                                                                                }
                                                                                else
                                                                                {
                                                                                    reportarError = new Error(archivo, "0", "0", "Semantico", "Los tamaños no coinciden");
                                                                                    Errores.Add(reportarError);
                                                                                }
                                                                            }
                                                                            else
                                                                            {
                                                                                reportarError = new Error(archivo, "0", "0", "Semantico", "Se esperaba tipo entero, tipo" + index[2].tipo + " encontrado");
                                                                                Errores.Add(reportarError);
                                                                            }
                                                                        }
                                                                        else
                                                                        {
                                                                            reportarError = new Error(archivo, "0", "0", "Semantico", "" + index[2].valor);
                                                                            Errores.Add(reportarError);
                                                                        }
                                                                    }
                                                                    else
                                                                    {
                                                                        reportarError = new Error(archivo, "0", "0", "Semantico", "Se esperaba tipo entero, tipo" + index[1].tipo + " encontrado");
                                                                        Errores.Add(reportarError);
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    reportarError = new Error(archivo, "0", "0", "Semantico", "" + index[1].valor);
                                                                    Errores.Add(reportarError);
                                                                }
                                                            }
                                                            else
                                                            {
                                                                reportarError = new Error(archivo, "0", "0", "Semantico", "Se esperaba tipo entero, tipo" + index[0].tipo + " encontrado");
                                                                Errores.Add(reportarError);
                                                            }
                                                        }
                                                        else
                                                        {
                                                            reportarError = new Error(archivo, "0", "0", "Semantico", "" + index[0].valor);
                                                            Errores.Add(reportarError);
                                                        }
                                                        break;
                                                    case 4:
                                                        reportarError = new Error(archivo, "0", "0", "Semantico", "Se esperaba arreglo de 1 dimension, dato puntual encontrado");
                                                        Errores.Add(reportarError);
                                                        break;
                                                    case 2:
                                                        reportarError = new Error(archivo, "0", "0", "Semantico", "Se esperaba arreglo de 1 dimension, arreglo de 2 dimensiones encontrado");
                                                        Errores.Add(reportarError);
                                                        break;
                                                }
                                                break;
                                        }
                                    }
                                    else if (simbolos[0].tipo == Simbolo.EnumTipo.dim3)
                                    {
                                        switch (simbolos[1].tipo)
                                        {
                                            case Simbolo.EnumTipo.dim1:
                                                if (listaHijos2.Count == 2)
                                                {
                                                    reportarError = new Error(archivo, "0", "0", "Semantico", "Se esperaba arreglo de 2 dimension, dato puntual encontrado");
                                                    Errores.Add(reportarError);
                                                }
                                                break;
                                            case Simbolo.EnumTipo.dim2:
                                                if (listaHijos2.Count == 3)
                                                {
                                                    reportarError = new Error(archivo, "0", "0", "Semantico", "Se esperaba arreglo de 2 dimension, dato puntual encontrado");
                                                    Errores.Add(reportarError);
                                                }
                                                else if (listaHijos2.Count == 2)
                                                {
                                                    reportarError = new Error(archivo, "0", "0", "Semantico", "Se esperaba arreglo de 1 dimension, arreglo de 1 dimension encontrado");
                                                    Errores.Add(reportarError);
                                                }
                                                break;
                                            case Simbolo.EnumTipo.dim3:
                                                switch (listaHijos2.Count)
                                                {
                                                    case 2:
                                                        //Tomamos los nodos donde se encuentran los indices a modificar
                                                        auxiliar[2] = (NodoSintactico)listaHijos1[1];
                                                        auxiliar[3] = (NodoSintactico)listaHijos2[1];
                                                        //Obtenemos los indices
                                                        index[0] = resolverExpresion(auxiliar[2], ent, archivo);
                                                        index[1] = resolverExpresion(auxiliar[3], ent, archivo);
                                                        if (index[0].tipo != Simbolo.EnumTipo.error)
                                                        {
                                                            if (index[0].tipo == Simbolo.EnumTipo.entero)
                                                            {
                                                                if (index[1].tipo != Simbolo.EnumTipo.error)
                                                                {
                                                                    if (index[1].tipo == Simbolo.EnumTipo.entero)
                                                                    {
                                                                        //Operar reasignacion
                                                                        variableDim3 = (Simbolo[,,])simbolos[0].valor;
                                                                        nuevoDim3 = (Simbolo[,,])simbolos[1].valor;
                                                                        if (variableDim3.GetLength(1) == nuevoDim3.GetLength(1) && variableDim3.GetLength(2) == nuevoDim3.GetLength(2))
                                                                        {
                                                                            try
                                                                            {
                                                                                for (int i = 0; i < nuevoDim3.GetLength(1); i++)
                                                                                {
                                                                                    for (int j = 0; i < nuevoDim3.GetLength(2); i++)
                                                                                    {
                                                                                        variableDim3[int.Parse(index[0].valor.ToString()), i, j] = nuevoDim3[int.Parse(index[1].valor.ToString()), i, j];
                                                                                        resultados[0] = new Simbolo(Simbolo.EnumTipo.dim3, variableDim3);
                                                                                        ent.modificar(auxiliar[0].getValor().ToString(), resultados[0]).ToString();
                                                                                    }
                                                                                }
                                                                            }
                                                                            catch (IndexOutOfRangeException)
                                                                            {
                                                                                reportarError = new Error(archivo, "0", "0", "Semantico", "Index fuera del limite");
                                                                                Errores.Add(reportarError);
                                                                            }
                                                                        }
                                                                        else
                                                                        {
                                                                            reportarError = new Error(archivo, "0", "0", "Semantico", "Los tamaños no coinciden");
                                                                            Errores.Add(reportarError);
                                                                        }
                                                                    }
                                                                    else
                                                                    {
                                                                        reportarError = new Error(archivo, "0", "0", "Semantico", "Se esperaba tipo entero, tipo" + index[1].tipo + " encontrado");
                                                                        Errores.Add(reportarError);
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    reportarError = new Error(archivo, "0", "0", "Semantico", "" + index[1].valor);
                                                                    Errores.Add(reportarError);
                                                                }
                                                            }
                                                            else
                                                            {
                                                                reportarError = new Error(archivo, "0", "0", "Semantico", "Se esperaba tipo entero, tipo" + index[0].tipo + " encontrado");
                                                                Errores.Add(reportarError);
                                                            }
                                                        }
                                                        else
                                                        {
                                                            reportarError = new Error(archivo, "0", "0", "Semantico", "" + index[0].valor);
                                                            Errores.Add(reportarError);
                                                        }
                                                        break;
                                                    case 3:
                                                        reportarError = new Error(archivo, "0", "0", "Semantico", "Se esperaba arreglo de 1 dimension, arreglo de 1 dimension encontrado");
                                                        Errores.Add(reportarError);
                                                        break;
                                                    case 4:
                                                        reportarError = new Error(archivo, "0", "0", "Semantico", "Se esperaba arreglo de 2 dimension, dato puntual encontrado");
                                                        Errores.Add(reportarError);
                                                        break;
                                                }
                                                break;
                                        }
                                    }
                                }
                                else
                                {
                                    reportarError = new Error(archivo, "0", "0", "Semantico", "La variable " + auxiliar[1].getValor().ToString() + " ya existe");
                                    Errores.Add(reportarError);
                                }
                            }
                            else
                            {
                                reportarError = new Error(archivo, "0", "0", "Semantico", "La variable " + auxiliar[0].getValor().ToString() + " ya existe");
                                Errores.Add(reportarError);
                            }
                            break;
                        #endregion

                        #region Objeto o funcion
                        case "EOTRO":
                            //Tomamos los nodos donde se encuentran los nombres de las variables
                            auxiliar[0] = (NodoSintactico)listaHijos1[0];
                            //Buscamos las variables para verificar el tipo de dato que son
                            simbolos[0] = ent.buscar(auxiliar[0].getValor().ToString(), 0, 0);
                            if (simbolos[0] != null)
                            {
                                switch (simbolos[0].tipo)
                                {
                                    #region arregloDim1[indice] = dato puntual 
                                    case Simbolo.EnumTipo.dim1:
                                        auxiliar[2] = (NodoSintactico)listaHijos1[1];
                                        //Obtenemos los indices
                                        index[0] = resolverExpresion(auxiliar[2], ent, archivo);
                                        if (index[0].tipo != Simbolo.EnumTipo.error)
                                        {
                                            if (index[0].tipo == Simbolo.EnumTipo.entero)
                                            {
                                                //Operar reasignacion
                                                variableDim1 = (Simbolo[])simbolos[0].valor;
                                                expresiones[0] = resolverExpresion(nuevo, ent, archivo);
                                                switch (expresiones[0].tipo)
                                                {
                                                    case Simbolo.EnumTipo.dim1:
                                                    case Simbolo.EnumTipo.dim2:
                                                    case Simbolo.EnumTipo.dim3:
                                                        reportarError = new Error(archivo, "0", "0", "Semantico", "Se esperaba dato, arreglo encontrado");
                                                        Errores.Add(reportarError);
                                                        break;
                                                    case Simbolo.EnumTipo.error:
                                                        reportarError = new Error(archivo, "0", "0", "Semantico", "" + expresiones[0].valor);
                                                        Errores.Add(reportarError);
                                                        break;
                                                    default:
                                                        try
                                                        {
                                                            resultados[0] = new Simbolo(expresiones[0].tipo, expresiones[0].valor);
                                                            variableDim1[int.Parse(index[0].valor.ToString())] = resultados[0];
                                                            resultados[1] = new Simbolo(Simbolo.EnumTipo.dim1, variableDim1);
                                                            ent.modificar(auxiliar[0].getValor().ToString(), resultados[1]).ToString();
                                                        }
                                                        catch (IndexOutOfRangeException)
                                                        {
                                                            reportarError = new Error(archivo, "0", "0", "Semantico", "Index fuera del limite");
                                                            Errores.Add(reportarError);
                                                        }
                                                        break;
                                                }
                                            }
                                            else
                                            {
                                                reportarError = new Error(archivo, "0", "0", "Semantico", "Se esperaba tipo entero, tipo" + index[0].tipo + " encontrado");
                                                Errores.Add(reportarError);
                                            }
                                        }
                                        else
                                        {
                                            reportarError = new Error(archivo, "0", "0", "Semantico", "" + index[0].valor);
                                            Errores.Add(reportarError);
                                        }
                                        break;
                                    #endregion

                                    #region arregloDim2[indice] = arregloDim1, dato puntual
                                    case Simbolo.EnumTipo.dim2:
                                        auxiliar[2] = (NodoSintactico)listaHijos1[1];
                                        //Obtenemos los indices
                                        index[0] = resolverExpresion(auxiliar[2], ent, archivo);
                                        if (index[0].tipo != Simbolo.EnumTipo.error)
                                        {
                                            if (index[0].tipo == Simbolo.EnumTipo.entero)
                                            {
                                                //Operar reasignacion
                                                variableDim2 = (Simbolo[,])simbolos[0].valor;
                                                expresiones[0] = resolverExpresion(nuevo, ent, archivo);
                                                if (expresiones[0].tipo != Simbolo.EnumTipo.error)
                                                {
                                                    try
                                                    {
                                                        resultados[0] = new Simbolo(expresiones[0].tipo, expresiones[0].valor);
                                                        switch (resultados[0].tipo)
                                                        {
                                                            case Simbolo.EnumTipo.dim2:
                                                            case Simbolo.EnumTipo.dim3:
                                                                reportarError = new Error(archivo, "0", "0", "Semantico", "Se esperaba arreglo de 1 dimension o dato puntual, arreglo de " + resultados[0].tipo + " encontrado");
                                                                Errores.Add(reportarError);
                                                                break;
                                                            case Simbolo.EnumTipo.error:
                                                                reportarError = new Error(archivo, "0", "0", "Semantico", "" + expresiones[0].valor);
                                                                Errores.Add(reportarError);
                                                                break;
                                                            default:
                                                                nuevoDim1 = (Simbolo[])resultados[0].valor;
                                                                for (int i = 0; i < variableDim2.GetLength(1); i++)
                                                                {
                                                                    variableDim2[int.Parse(index[0].valor.ToString()), i] = nuevoDim1[i];
                                                                    resultados[1] = new Simbolo(Simbolo.EnumTipo.dim2, variableDim2);
                                                                }
                                                                ent.modificar(auxiliar[0].getValor().ToString(), resultados[1]).ToString();
                                                                break;
                                                        }
                                                    }
                                                    catch (IndexOutOfRangeException)
                                                    {
                                                        reportarError = new Error(archivo, "0", "0", "Semantico", "Index fuera del limite");
                                                        Errores.Add(reportarError);
                                                    }
                                                }
                                                else
                                                {
                                                    reportarError = new Error(archivo, "0", "0", "Semantico", "" + expresiones[0].valor);
                                                    Errores.Add(reportarError);
                                                }
                                            }
                                            else
                                            {
                                                reportarError = new Error(archivo, "0", "0", "Semantico", "Se esperaba tipo entero, tipo" + index[0].tipo + " encontrado");
                                                Errores.Add(reportarError);
                                            }
                                        }
                                        else
                                        {
                                            reportarError = new Error(archivo, "0", "0", "Semantico", "" + index[0].valor);
                                            Errores.Add(reportarError);
                                        }
                                        break;
                                    #endregion

                                    #region arregloDim3[indice] = arregloDim2
                                    case Simbolo.EnumTipo.dim3:
                                        auxiliar[2] = (NodoSintactico)listaHijos1[1];
                                        //Obtenemos los indices
                                        index[0] = resolverExpresion(auxiliar[2], ent, archivo);
                                        if (index[0].tipo != Simbolo.EnumTipo.error)
                                        {
                                            if (index[0].tipo == Simbolo.EnumTipo.entero)
                                            {
                                                //Operar reasignacion
                                                variableDim3 = (Simbolo[,,])simbolos[0].valor;
                                                expresiones[0] = resolverExpresion(nuevo, ent, archivo);
                                                if (expresiones[0].tipo != Simbolo.EnumTipo.error)
                                                {
                                                    try
                                                    {
                                                        resultados[0] = new Simbolo(expresiones[0].tipo, expresiones[0].valor);
                                                        switch (resultados[0].tipo)
                                                        {
                                                            case Simbolo.EnumTipo.dim1:
                                                            case Simbolo.EnumTipo.dim3:
                                                                reportarError = new Error(archivo, "0", "0", "Semantico", "Se esperaba arreglo de 1 dimension, arreglo de " + resultados[0].tipo + " encontrado");
                                                                Errores.Add(reportarError);
                                                                break;
                                                            case Simbolo.EnumTipo.error:
                                                                reportarError = new Error(archivo, "0", "0", "Semantico", "" + resultados[0].valor);
                                                                Errores.Add(reportarError);
                                                                break;
                                                            case Simbolo.EnumTipo.dim2:
                                                                nuevoDim2 = (Simbolo[,])resultados[0].valor;
                                                                for (int i = 0; i < variableDim3.GetLength(1); i++)
                                                                {
                                                                    for (int j = 0; j < variableDim3.GetLength(2); j++)
                                                                    {
                                                                        variableDim3[int.Parse(index[0].valor.ToString()), i, j] = nuevoDim2[i, j];
                                                                        resultados[1] = new Simbolo(Simbolo.EnumTipo.dim3, variableDim3);
                                                                    }
                                                                }
                                                                ent.modificar(auxiliar[0].getValor().ToString(), resultados[1]).ToString();
                                                                break;
                                                            default:
                                                                for (int i = 0; i < variableDim3.GetLength(1); i++)
                                                                {
                                                                    for (int j = 0; j < variableDim3.GetLength(2); j++)
                                                                    {
                                                                        variableDim3[int.Parse(index[0].valor.ToString()), i, j] = resultados[0];
                                                                        resultados[1] = new Simbolo(Simbolo.EnumTipo.dim3, variableDim3);
                                                                    }
                                                                }
                                                                ent.modificar(auxiliar[0].getValor().ToString(), resultados[1]).ToString();
                                                                break;
                                                        }
                                                    }
                                                    catch (IndexOutOfRangeException)
                                                    {
                                                        reportarError = new Error(archivo, "0", "0", "Semantico", "Index fuera del limite");
                                                        Errores.Add(reportarError);
                                                    }
                                                }
                                                else
                                                {
                                                    reportarError = new Error(archivo, "0", "0", "Semantico", "" + expresiones[0].valor);
                                                    Errores.Add(reportarError);
                                                }
                                            }
                                            else
                                            {
                                                reportarError = new Error(archivo, "0", "0", "Semantico", "Se esperaba tipo entero, tipo" + index[0].tipo + " encontrado");
                                                Errores.Add(reportarError);
                                            }
                                        }
                                        else
                                        {
                                            reportarError = new Error(archivo, "0", "0", "Semantico", "" + index[0].valor);
                                            Errores.Add(reportarError);
                                        }
                                        break;
                                        #endregion
                                }
                            }
                            else
                            {
                                reportarError = new Error(archivo, "0", "0", "Semantico", "La variable " + auxiliar[0].getValor().ToString() + " ya existe");
                                Errores.Add(reportarError);
                            }
                            break;
                        #endregion

                        #region Conjuntos de datos D1, D2, D3
                        case "CONJUNTO_D1":
                        case "CONJUNTO_D2":
                        case "CONJUNTO_D3":
                            //Tomamos los nodos donde se encuentran los nombres de las variables
                            auxiliar[0] = (NodoSintactico)listaHijos1[0];
                            //Buscamos las variables para verificar el tipo de dato que son
                            simbolos[0] = ent.buscar(auxiliar[0].getValor().ToString(), 0, 0);
                            if (simbolos[0] != null)
                            {
                                switch (nuevo.getNombre())
                                {
                                    //dim1 y conjunto D1
                                    case "CONJUNTO_D1":
                                        if (simbolos[0].tipo == Simbolo.EnumTipo.dim1)
                                        {
                                            reportarError = new Error(archivo, "0", "0", "Semantico", "Se esperaba dato, arreglo encontrado");
                                            Errores.Add(reportarError);
                                        }
                                        else if (simbolos[0].tipo == Simbolo.EnumTipo.dim2)
                                        {
                                            //Tomamos los nodos donde se encuentran los indices a modificar
                                            auxiliar[1] = (NodoSintactico)listaHijos1[1];
                                            //Obtenemos los indices
                                            index[0] = resolverExpresion(auxiliar[1], ent, archivo);
                                            if (index[0].tipo != Simbolo.EnumTipo.error)
                                            {
                                                if (index[0].tipo == Simbolo.EnumTipo.entero)
                                                {
                                                    variableDim2 = (Simbolo[,])simbolos[0].valor;
                                                    if (variableDim2.GetLength(1) == listaHijos2.Count)
                                                    {
                                                        for (int i = 0; i < listaHijos2.Count; i++)
                                                        {
                                                            auxiliar[2] = (NodoSintactico)listaHijos2[i];
                                                            expresiones[0] = resolverExpresion(auxiliar[2], ent, archivo);
                                                            if (expresiones[0].tipo != Simbolo.EnumTipo.error)
                                                            {
                                                                variableDim2[int.Parse(index[0].valor.ToString()), i] = new Simbolo(expresiones[0].tipo, expresiones[0].valor);
                                                            }
                                                            else
                                                            {
                                                                reportarError = new Error(archivo, "0", "0", "Warning", "" + expresiones[0].valor + " el espacio de memoria tomo el valor de nulo");
                                                                Errores.Add(reportarError);
                                                                variableDim2[int.Parse(index[0].valor.ToString()), i] = new Simbolo(Simbolo.EnumTipo.nulo, 0);
                                                            }
                                                        }
                                                        ent.modificar(auxiliar[0].getValor().ToString(), new Simbolo(Simbolo.EnumTipo.dim2, variableDim2));
                                                    }
                                                    else
                                                    {
                                                        reportarError = new Error(archivo, "0", "0", "Semantico", "Los tamaños no coinciden");
                                                        Errores.Add(reportarError);
                                                    }
                                                }
                                                else
                                                {
                                                    reportarError = new Error(archivo, "0", "0", "Semantico", "Se esperaba tipo entero, tipo" + index[0].tipo + " encontrado");
                                                    Errores.Add(reportarError);
                                                }
                                            }
                                            else
                                            {
                                                reportarError = new Error(archivo, "0", "0", "Semantico", "" + index[0].valor);
                                                Errores.Add(reportarError);
                                            }
                                        }
                                        else if (simbolos[0].tipo == Simbolo.EnumTipo.dim3)
                                        {
                                            reportarError = new Error(archivo, "0", "0", "Semantico", "Se esperaba arreglo de 2 dimensiones, conjunto de datos de 1 dimension encontrado");
                                            Errores.Add(reportarError);
                                        }
                                        break;
                                    case "CONJUNTO_D2":
                                        if (simbolos[0].tipo == Simbolo.EnumTipo.dim1)
                                        {
                                            reportarError = new Error(archivo, "0", "0", "Semantico", "Se esperaba dato, arreglo encontrado");
                                            Errores.Add(reportarError);
                                        }
                                        else if (simbolos[0].tipo == Simbolo.EnumTipo.dim2)
                                        {
                                            reportarError = new Error(archivo, "0", "0", "Semantico", "Se esperaba arreglo de 1 dimension, conjunto de datos de 2 dimension encontrado");
                                            Errores.Add(reportarError);
                                        }
                                        else if (simbolos[0].tipo == Simbolo.EnumTipo.dim3)
                                        {
                                            //Tomamos los nodos donde se encuentran los indices a modificar
                                            auxiliar[1] = (NodoSintactico)listaHijos1[1];
                                            //Obtenemos los indices
                                            index[0] = resolverExpresion(auxiliar[1], ent, archivo);
                                            if (index[0].tipo != Simbolo.EnumTipo.error)
                                            {
                                                if (index[0].tipo == Simbolo.EnumTipo.entero)
                                                {
                                                    variableDim3 = (Simbolo[,,])simbolos[0].valor;
                                                    if (variableDim3.GetLength(1) == listaHijos2.Count)
                                                    {
                                                        sinError = true;
                                                        for (int i = 0; i < listaHijos2.Count; i++)
                                                        {
                                                            sinError = true;
                                                            auxiliar[2] = (NodoSintactico)listaHijos2[i];
                                                            listaHijos3 = auxiliar[2].getHijos();
                                                            if (variableDim3.GetLength(2) == listaHijos3.Count)
                                                            {
                                                                for (int j = 0; j < listaHijos3.Count; j++)
                                                                {
                                                                    auxiliar[3] = (NodoSintactico)listaHijos3[j];
                                                                    expresiones[0] = resolverExpresion(auxiliar[3], ent, archivo);
                                                                    if (expresiones[0].tipo != Simbolo.EnumTipo.error)
                                                                    {
                                                                        variableDim3[int.Parse(index[0].valor.ToString()), i, j] = new Simbolo(expresiones[0].tipo, expresiones[0].valor);
                                                                    }
                                                                    else
                                                                    {
                                                                        reportarError = new Error(archivo, "0", "0", "Warning", "" + expresiones[0].valor + " el espacio de memoria tomo el valor de nulo");
                                                                        Errores.Add(reportarError);
                                                                        variableDim3[int.Parse(index[0].valor.ToString()), i, j] = new Simbolo(Simbolo.EnumTipo.nulo, 0);
                                                                    }
                                                                }
                                                            }
                                                            else
                                                            {
                                                                sinError = false;
                                                                break;
                                                            }
                                                        }
                                                    }
                                                    else
                                                    {
                                                        sinError = false;
                                                    }
                                                    if (sinError)
                                                    {
                                                        ent.modificar(auxiliar[0].getValor().ToString(), new Simbolo(Simbolo.EnumTipo.dim3, variableDim3));
                                                    }
                                                    else
                                                    {
                                                        reportarError = new Error(archivo, "0", "0", "Semantico", "Los tamaños no coinciden");
                                                        Errores.Add(reportarError);
                                                    }
                                                }
                                                else
                                                {
                                                    reportarError = new Error(archivo, "0", "0", "Semantico", "Se esperaba tipo entero, tipo" + index[0].tipo + " encontrado");
                                                    Errores.Add(reportarError);
                                                }
                                            }
                                            else
                                            {
                                                reportarError = new Error(archivo, "0", "0", "Semantico", "" + index[0].valor);
                                                Errores.Add(reportarError);
                                            }
                                        }
                                        break;
                                    case "CONJUNTO_D3":
                                        if (simbolos[0].tipo == Simbolo.EnumTipo.dim1)
                                        {
                                            reportarError = new Error(archivo, "0", "0", "Semantico", "Se esperaba dato, arreglo encontrado");
                                            Errores.Add(reportarError);
                                        }
                                        else if (simbolos[0].tipo == Simbolo.EnumTipo.dim2)
                                        {
                                            reportarError = new Error(archivo, "0", "0", "Semantico", "Se esperaba conjunto de datos de 1 dimension, conjunto de datos de 2 dimensiones encontrado");
                                            Errores.Add(reportarError);
                                        }
                                        else if (simbolos[0].tipo == Simbolo.EnumTipo.dim3)
                                        {
                                            reportarError = new Error(archivo, "0", "0", "Semantico", "Se esperaba conjunto de datos de 2 dimensiones, conjunto de datos de 3 dimensiones encontrado");
                                            Errores.Add(reportarError);
                                        }
                                        break;
                                }
                            }
                            else
                            {
                                reportarError = new Error(archivo, "0", "0", "Semantico", "La variable " + auxiliar[0].getValor().ToString() + " ya existe");
                                Errores.Add(reportarError);
                            }
                            break;
                        #endregion

                        #region Datos puntuales: entero, doble, caracter, cadena, boleano, nulo
                        default:
                            //Tomamos los nodos donde se encuentran los nombres de las variables
                            auxiliar[0] = (NodoSintactico)listaHijos1[0];
                            //Buscamos las variables para verificar el tipo de dato que son
                            simbolos[0] = ent.buscar(auxiliar[0].getValor().ToString(), 0, 0);
                            if (simbolos[0] != null)
                            {
                                switch (simbolos[0].tipo)
                                {
                                    #region arregloDim1[indice] = dato puntual 
                                    case Simbolo.EnumTipo.dim1:
                                        if (!nuevo.getNombre().Equals("ID"))
                                        {
                                            auxiliar[2] = (NodoSintactico)listaHijos1[1];
                                            //Obtenemos los indices
                                            index[0] = resolverExpresion(auxiliar[2], ent, archivo);
                                            if (index[0].tipo != Simbolo.EnumTipo.error)
                                            {
                                                if (index[0].tipo == Simbolo.EnumTipo.entero)
                                                {
                                                    //Operar reasignacion
                                                    variableDim1 = (Simbolo[])simbolos[0].valor;
                                                    expresiones[0] = resolverExpresion(nuevo, ent, archivo);
                                                    if (expresiones[0].tipo != Simbolo.EnumTipo.error)
                                                    {
                                                        try
                                                        {
                                                            resultados[0] = new Simbolo(expresiones[0].tipo, expresiones[0].valor);
                                                            variableDim1[int.Parse(index[0].valor.ToString())] = resultados[0];
                                                            resultados[1] = new Simbolo(Simbolo.EnumTipo.dim1, variableDim1);
                                                            ent.modificar(auxiliar[0].getValor().ToString(), resultados[1]).ToString();
                                                        }
                                                        catch (IndexOutOfRangeException)
                                                        {
                                                            reportarError = new Error(archivo, "0", "0", "Semantico", "Index fuera del limite");
                                                            Errores.Add(reportarError);
                                                        }
                                                    }
                                                    else
                                                    {
                                                        reportarError = new Error(archivo, "0", "0", "Semantico", "" + expresiones[0].valor);
                                                        Errores.Add(reportarError);
                                                    }
                                                }
                                                else
                                                {
                                                    reportarError = new Error(archivo, "0", "0", "Semantico", "Se esperaba tipo entero, tipo" + index[0].tipo + " encontrado");
                                                    Errores.Add(reportarError);
                                                }
                                            }
                                            else
                                            {
                                                reportarError = new Error(archivo, "0", "0", "Semantico", "" + index[0].valor);
                                                Errores.Add(reportarError);
                                            }
                                        }
                                        else
                                        {
                                            //Buscamos las variables para verificar el tipo de dato que son
                                            simbolos[1] = ent.buscar(nuevo.getValor().ToString(), 0, 0);
                                            if (simbolos[1] != null)
                                            {
                                                if (simbolos[1].tipo == Simbolo.EnumTipo.dim1 || simbolos[1].tipo == Simbolo.EnumTipo.dim2 || simbolos[1].tipo == Simbolo.EnumTipo.dim3)
                                                {
                                                    reportarError = new Error(archivo, "0", "0", "Semantico", "Se esperaba dato, arreglo encontrado");
                                                    Errores.Add(reportarError);
                                                }
                                                else
                                                {
                                                    auxiliar[2] = (NodoSintactico)listaHijos1[1];
                                                    //Obtenemos los indices
                                                    index[0] = resolverExpresion(auxiliar[2], ent, archivo);
                                                    if (index[0].tipo != Simbolo.EnumTipo.error)
                                                    {
                                                        if (index[0].tipo == Simbolo.EnumTipo.entero)
                                                        {
                                                            //Operar reasignacion
                                                            variableDim1 = (Simbolo[])simbolos[0].valor;
                                                            expresiones[0] = resolverExpresion(nuevo, ent, archivo);
                                                            if (expresiones[0].tipo != Simbolo.EnumTipo.error)
                                                            {
                                                                try
                                                                {
                                                                    resultados[0] = new Simbolo(expresiones[0].tipo, expresiones[0].valor);
                                                                    variableDim1[int.Parse(index[0].valor.ToString())] = resultados[0];
                                                                    resultados[1] = new Simbolo(Simbolo.EnumTipo.dim1, variableDim1);
                                                                    ent.modificar(auxiliar[0].getValor().ToString(), resultados[1]).ToString();
                                                                }
                                                                catch (IndexOutOfRangeException)
                                                                {
                                                                    reportarError = new Error(archivo, "0", "0", "Semantico", "Index fuera del limite");
                                                                    Errores.Add(reportarError);
                                                                }
                                                            }
                                                            else
                                                            {
                                                                reportarError = new Error(archivo, "0", "0", "Semantico", "" + expresiones[0].valor);
                                                                Errores.Add(reportarError);
                                                            }
                                                        }
                                                        else
                                                        {
                                                            reportarError = new Error(archivo, "0", "0", "Semantico", "Se esperaba tipo entero, tipo" + index[0].tipo + " encontrado");
                                                            Errores.Add(reportarError);
                                                        }
                                                    }
                                                    else
                                                    {
                                                        reportarError = new Error(archivo, "0", "0", "Semantico", "" + index[0].valor);
                                                        Errores.Add(reportarError);
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                reportarError = new Error(archivo, "0", "0", "Semantico", "La variable " + nuevo.getValor().ToString() + " ya existe");
                                                Errores.Add(reportarError);
                                            }
                                        }
                                        break;
                                    #endregion

                                    #region arregloDim2[indice] = arregloDim1, dato puntual
                                    case Simbolo.EnumTipo.dim2:
                                        if (!nuevo.getNombre().Equals("ID"))
                                        {
                                            auxiliar[2] = (NodoSintactico)listaHijos1[1];
                                            //Obtenemos los indices
                                            index[0] = resolverExpresion(auxiliar[2], ent, archivo);
                                            if (index[0].tipo != Simbolo.EnumTipo.error)
                                            {
                                                if (index[0].tipo == Simbolo.EnumTipo.entero)
                                                {
                                                    //Operar reasignacion
                                                    variableDim2 = (Simbolo[,])simbolos[0].valor;
                                                    expresiones[0] = resolverExpresion(nuevo, ent, archivo);
                                                    if (expresiones[0].tipo != Simbolo.EnumTipo.error)
                                                    {
                                                        try
                                                        {
                                                            resultados[0] = new Simbolo(expresiones[0].tipo, expresiones[0].valor);
                                                            for (int i = 0; i < variableDim2.GetLength(1); i++)
                                                            {
                                                                variableDim2[int.Parse(index[0].valor.ToString()), i] = resultados[0];
                                                                resultados[1] = new Simbolo(Simbolo.EnumTipo.dim2, variableDim2);
                                                            }
                                                            ent.modificar(auxiliar[0].getValor().ToString(), resultados[1]).ToString();
                                                        }
                                                        catch (IndexOutOfRangeException)
                                                        {
                                                            reportarError = new Error(archivo, "0", "0", "Semantico", "Index fuera del limite");
                                                            Errores.Add(reportarError);
                                                        }
                                                    }
                                                    else
                                                    {
                                                        reportarError = new Error(archivo, "0", "0", "Semantico", "" + expresiones[0].valor);
                                                        Errores.Add(reportarError);
                                                    }
                                                }
                                                else
                                                {
                                                    reportarError = new Error(archivo, "0", "0", "Semantico", "Se esperaba tipo entero, tipo" + index[0].tipo + " encontrado");
                                                    Errores.Add(reportarError);
                                                }
                                            }
                                            else
                                            {
                                                reportarError = new Error(archivo, "0", "0", "Semantico", "" + index[0].valor);
                                                Errores.Add(reportarError);
                                            }
                                        }
                                        else
                                        {
                                            //Buscamos las variables para verificar el tipo de dato que son
                                            simbolos[1] = ent.buscar(nuevo.getValor().ToString(), 0, 0);
                                            if (simbolos[1] != null)
                                            {
                                                switch (simbolos[1].tipo)
                                                {
                                                    case Simbolo.EnumTipo.dim1:
                                                        variableDim2 = (Simbolo[,])simbolos[0].valor;
                                                        nuevoDim1 = (Simbolo[])simbolos[1].valor;

                                                        auxiliar[2] = (NodoSintactico)listaHijos1[1];
                                                        index[0] = resolverExpresion(auxiliar[2], ent, archivo);
                                                        if (index[0].tipo != Simbolo.EnumTipo.error)
                                                        {
                                                            if (index[0].tipo == Simbolo.EnumTipo.entero)
                                                            {
                                                                if (variableDim2.GetLength(1) == nuevoDim1.GetLength(0))
                                                                {
                                                                    for (int i = 0; i < nuevoDim1.GetLength(0); i++)
                                                                    {
                                                                        variableDim2[int.Parse(index[0].valor.ToString()), i] = nuevoDim1[i];
                                                                    }
                                                                    ent.modificar(auxiliar[0].getValor().ToString(), new Simbolo(Simbolo.EnumTipo.dim2, variableDim2));
                                                                }
                                                                else
                                                                {
                                                                    reportarError = new Error(archivo, "0", "0", "Semantico", "Los tamaños no coinciden");
                                                                    Errores.Add(reportarError);
                                                                }
                                                            }
                                                            else
                                                            {
                                                                reportarError = new Error(archivo, "0", "0", "Semantico", "Se esperaba tipo entero, tipo " + index[0].tipo + " encontrado");
                                                                Errores.Add(reportarError);
                                                            }
                                                        }
                                                        else
                                                        {
                                                            reportarError = new Error(archivo, "0", "0", "Semantico", "" + index[0].valor);
                                                            Errores.Add(reportarError);
                                                        }
                                                        break;
                                                    case Simbolo.EnumTipo.dim2:
                                                        reportarError = new Error(archivo, "0", "0", "Semantico", "Se esperaba arreglo de 1 dimension, arreglo de 2 dimensiones encontrado");
                                                        Errores.Add(reportarError);
                                                        break;
                                                    case Simbolo.EnumTipo.dim3:
                                                        reportarError = new Error(archivo, "0", "0", "Semantico", "Se esperaba arreglo de 1 dimension, arreglo de 2 dimensiones encontrado");
                                                        Errores.Add(reportarError);
                                                        break;
                                                    default:
                                                        auxiliar[2] = (NodoSintactico)listaHijos1[1];
                                                        //Obtenemos los indices
                                                        index[0] = resolverExpresion(auxiliar[2], ent, archivo);
                                                        if (index[0].tipo != Simbolo.EnumTipo.error)
                                                        {
                                                            if (index[0].tipo == Simbolo.EnumTipo.entero)
                                                            {
                                                                //Operar reasignacion
                                                                variableDim2 = (Simbolo[,])simbolos[0].valor;
                                                                expresiones[0] = resolverExpresion(nuevo, ent, archivo);
                                                                if (expresiones[0].tipo != Simbolo.EnumTipo.error)
                                                                {
                                                                    try
                                                                    {
                                                                        resultados[0] = new Simbolo(expresiones[0].tipo, expresiones[0].valor);
                                                                        for (int i = 0; i < variableDim2.GetLength(1); i++)
                                                                        {
                                                                            variableDim2[int.Parse(index[0].valor.ToString()), i] = resultados[0];
                                                                            resultados[1] = new Simbolo(Simbolo.EnumTipo.dim2, variableDim2);
                                                                        }
                                                                        ent.modificar(auxiliar[0].getValor().ToString(), resultados[1]).ToString();
                                                                    }
                                                                    catch (IndexOutOfRangeException)
                                                                    {
                                                                        reportarError = new Error(archivo, "0", "0", "Semantico", "Index fuera del limite");
                                                                        Errores.Add(reportarError);
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    reportarError = new Error(archivo, "0", "0", "Semantico", "" + expresiones[0].valor);
                                                                    Errores.Add(reportarError);
                                                                }
                                                            }
                                                            else
                                                            {
                                                                reportarError = new Error(archivo, "0", "0", "Semantico", "Se esperaba tipo entero, tipo" + index[0].tipo + " encontrado");
                                                                Errores.Add(reportarError);
                                                            }
                                                        }
                                                        else
                                                        {
                                                            reportarError = new Error(archivo, "0", "0", "Semantico", "" + index[0].valor);
                                                            Errores.Add(reportarError);
                                                        }
                                                        break;
                                                }
                                            }
                                            else
                                            {
                                                reportarError = new Error(archivo, "0", "0", "Semantico", "La variable " + nuevo.getValor().ToString() + " ya existe");
                                                Errores.Add(reportarError);
                                            }
                                        }
                                        break;
                                    #endregion

                                    #region arregloDim3[indice] = arregloDim2
                                    case Simbolo.EnumTipo.dim3:
                                        if (!nuevo.getNombre().Equals("ID"))
                                        {
                                            auxiliar[2] = (NodoSintactico)listaHijos1[1];
                                            //Obtenemos los indices
                                            index[0] = resolverExpresion(auxiliar[2], ent, archivo);
                                            if (index[0].tipo != Simbolo.EnumTipo.error)
                                            {
                                                if (index[0].tipo == Simbolo.EnumTipo.entero)
                                                {
                                                    //Operar reasignacion
                                                    variableDim3 = (Simbolo[,,])simbolos[0].valor;
                                                    expresiones[0] = resolverExpresion(nuevo, ent, archivo);
                                                    if (expresiones[0].tipo != Simbolo.EnumTipo.error)
                                                    {
                                                        try
                                                        {
                                                            resultados[0] = new Simbolo(expresiones[0].tipo, expresiones[0].valor);
                                                            for (int i = 0; i < variableDim3.GetLength(1); i++)
                                                            {
                                                                for (int j = 0; j < variableDim3.GetLength(2); j++)
                                                                {
                                                                    variableDim3[int.Parse(index[0].valor.ToString()), i, j] = resultados[0];
                                                                    resultados[1] = new Simbolo(Simbolo.EnumTipo.dim3, variableDim3);
                                                                }
                                                            }
                                                            ent.modificar(auxiliar[0].getValor().ToString(), resultados[1]).ToString();
                                                        }
                                                        catch (IndexOutOfRangeException)
                                                        {
                                                            reportarError = new Error(archivo, "0", "0", "Semantico", "Index fuera del limite");
                                                            Errores.Add(reportarError);
                                                        }
                                                    }
                                                    else
                                                    {
                                                        reportarError = new Error(archivo, "0", "0", "Semantico", "" + expresiones[0].valor);
                                                        Errores.Add(reportarError);
                                                    }
                                                }
                                                else
                                                {
                                                    reportarError = new Error(archivo, "0", "0", "Semantico", "Se esperaba tipo entero, tipo" + index[0].tipo + " encontrado");
                                                    Errores.Add(reportarError);
                                                }
                                            }
                                            else
                                            {
                                                reportarError = new Error(archivo, "0", "0", "Semantico", "" + index[0].valor);
                                                Errores.Add(reportarError);
                                            }
                                        }
                                        else
                                        {
                                            simbolos[1] = ent.buscar(nuevo.getValor().ToString(), 0, 0);
                                            if (simbolos[1] != null)
                                            {
                                                switch (simbolos[1].tipo)
                                                {
                                                    case Simbolo.EnumTipo.dim2:
                                                        variableDim3 = (Simbolo[,,])simbolos[0].valor;
                                                        nuevoDim2 = (Simbolo[,])simbolos[1].valor;

                                                        auxiliar[2] = (NodoSintactico)listaHijos1[1];
                                                        index[0] = resolverExpresion(auxiliar[2], ent, archivo);
                                                        if (index[0].tipo != Simbolo.EnumTipo.error)
                                                        {
                                                            if (index[0].tipo == Simbolo.EnumTipo.entero)
                                                            {
                                                                if (variableDim3.GetLength(1) == nuevoDim2.GetLength(0) && variableDim3.GetLength(2) == nuevoDim2.GetLength(1))
                                                                {
                                                                    for (int i = 0; i < nuevoDim2.GetLength(0); i++)
                                                                    {
                                                                        for (int j = 0; j < nuevoDim2.GetLength(1); j++)
                                                                        {
                                                                            variableDim3[int.Parse(index[0].valor.ToString()), i, j] = nuevoDim2[i, j];
                                                                        }
                                                                    }
                                                                    ent.modificar(auxiliar[0].getValor().ToString(), new Simbolo(Simbolo.EnumTipo.dim3, variableDim3));
                                                                }
                                                                else
                                                                {
                                                                    reportarError = new Error(archivo, "0", "0", "Semantico", "Los tamaños no coinciden");
                                                                    Errores.Add(reportarError);
                                                                }
                                                            }
                                                            else
                                                            {
                                                                reportarError = new Error(archivo, "0", "0", "Semantico", "Se esperaba tipo entero, tipo " + index[0].tipo + " encontrado");
                                                                Errores.Add(reportarError);
                                                            }
                                                        }
                                                        else
                                                        {
                                                            reportarError = new Error(archivo, "0", "0", "Semantico", "" + index[0].valor);
                                                            Errores.Add(reportarError);
                                                        }
                                                        break;
                                                    case Simbolo.EnumTipo.dim1:
                                                        reportarError = new Error(archivo, "0", "0", "Semantico", "Se esperaba arreglo de 1 dimension, arreglo de 1 dimension encontrado");
                                                        Errores.Add(reportarError);
                                                        break;
                                                    case Simbolo.EnumTipo.dim3:
                                                        reportarError = new Error(archivo, "0", "0", "Semantico", "Se esperaba arreglo de 1 dimension, arreglo de 3 dimensiones encontrado");
                                                        Errores.Add(reportarError);
                                                        break;
                                                    default:
                                                        auxiliar[2] = (NodoSintactico)listaHijos1[1];
                                                        //Obtenemos los indices
                                                        index[0] = resolverExpresion(auxiliar[2], ent, archivo);
                                                        if (index[0].tipo != Simbolo.EnumTipo.error)
                                                        {
                                                            if (index[0].tipo == Simbolo.EnumTipo.entero)
                                                            {
                                                                //Operar reasignacion
                                                                variableDim3 = (Simbolo[,,])simbolos[0].valor;
                                                                expresiones[0] = resolverExpresion(nuevo, ent, archivo);
                                                                if (expresiones[0].tipo != Simbolo.EnumTipo.error)
                                                                {
                                                                    try
                                                                    {
                                                                        resultados[0] = new Simbolo(expresiones[0].tipo, expresiones[0].valor);
                                                                        for (int i = 0; i < variableDim3.GetLength(1); i++)
                                                                        {
                                                                            for (int j = 0; j < variableDim3.GetLength(2); j++)
                                                                            {
                                                                                variableDim3[int.Parse(index[0].valor.ToString()), i, j] = resultados[0];
                                                                                resultados[1] = new Simbolo(Simbolo.EnumTipo.dim3, variableDim3);
                                                                            }
                                                                        }
                                                                        ent.modificar(auxiliar[0].getValor().ToString(), resultados[1]).ToString();
                                                                    }
                                                                    catch (IndexOutOfRangeException)
                                                                    {
                                                                        reportarError = new Error(archivo, "0", "0", "Semantico", "Index fuera del limite");
                                                                        Errores.Add(reportarError);
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    reportarError = new Error(archivo, "0", "0", "Semantico", "" + expresiones[0].valor);
                                                                    Errores.Add(reportarError);
                                                                }
                                                            }
                                                            else
                                                            {
                                                                reportarError = new Error(archivo, "0", "0", "Semantico", "Se esperaba tipo entero, tipo" + index[0].tipo + " encontrado");
                                                                Errores.Add(reportarError);
                                                            }
                                                        }
                                                        else
                                                        {
                                                            reportarError = new Error(archivo, "0", "0", "Semantico", "" + index[0].valor);
                                                            Errores.Add(reportarError);
                                                        }
                                                        break;
                                                }
                                            }
                                            else
                                            {
                                                reportarError = new Error(archivo, "0", "0", "Semantico", "La variable " + nuevo.getValor().ToString() + " ya existe");
                                                Errores.Add(reportarError);
                                            }
                                        }
                                        break;
                                        #endregion
                                }
                            }
                            else
                            {
                                reportarError = new Error(archivo, "0", "0", "Semantico", "La variable " + auxiliar[0].getValor().ToString() + " ya existe");
                                Errores.Add(reportarError);
                            }
                            break;
                            #endregion
                    }
                    break;
                #endregion

                #region DIM2
                case "DIM2":
                    switch (nuevo.getNombre())
                    {
                        #region Arreglos
                        case "DIM1":
                        case "DIM2":
                        case "DIM3":
                            //Tomamos los nodos donde se encuentran los nombres de las variables
                            auxiliar[0] = (NodoSintactico)listaHijos1[0];
                            auxiliar[1] = (NodoSintactico)listaHijos2[0];
                            //Buscamos las variables para verificar el tipo de dato que son
                            simbolos[0] = ent.buscar(auxiliar[0].getValor().ToString(), 0, 0);
                            simbolos[1] = ent.buscar(auxiliar[1].getValor().ToString(), 0, 0);
                            if (simbolos[0] != null)
                            {
                                if (simbolos[1] != null)
                                {
                                    if (simbolos[0].tipo == Simbolo.EnumTipo.dim1)
                                    {
                                        reportarError = new Error(archivo, "0", "0", "Semantico", "El arreglo no puede tener mas de 1 dimension");
                                        Errores.Add(reportarError);
                                    }
                                    else if (simbolos[0].tipo == Simbolo.EnumTipo.dim2)
                                    {
                                        switch (simbolos[1].tipo)
                                        {
                                            case Simbolo.EnumTipo.dim1:
                                                if (listaHijos2.Count == 2)
                                                {
                                                    //Tomamos los nodos donde se encuentran los indices a modificar
                                                    auxiliar[2] = (NodoSintactico)listaHijos1[1];
                                                    auxiliar[3] = (NodoSintactico)listaHijos1[2];
                                                    auxiliar[4] = (NodoSintactico)listaHijos2[1];
                                                    //Obtenemos los indices
                                                    index[0] = resolverExpresion(auxiliar[2], ent, archivo);
                                                    index[1] = resolverExpresion(auxiliar[3], ent, archivo);
                                                    index[2] = resolverExpresion(auxiliar[4], ent, archivo);
                                                    if (index[0].tipo != Simbolo.EnumTipo.error)
                                                    {
                                                        if (index[0].tipo == Simbolo.EnumTipo.entero)
                                                        {
                                                            if (index[1].tipo != Simbolo.EnumTipo.error)
                                                            {
                                                                if (index[1].tipo == Simbolo.EnumTipo.entero)
                                                                {
                                                                    if (index[2].tipo != Simbolo.EnumTipo.error)
                                                                    {
                                                                        if (index[2].tipo == Simbolo.EnumTipo.entero)
                                                                        {
                                                                            //Operar reasignacion
                                                                            variableDim2 = (Simbolo[,])simbolos[0].valor;
                                                                            nuevoDim1 = (Simbolo[])simbolos[1].valor;
                                                                            if (variableDim2.GetLength(1) == nuevoDim1.Length)
                                                                            {
                                                                                try
                                                                                {
                                                                                    variableDim2[int.Parse(index[0].valor.ToString()), int.Parse(index[1].valor.ToString())] = nuevoDim1[int.Parse(index[2].valor.ToString())];
                                                                                    resultados[0] = new Simbolo(Simbolo.EnumTipo.dim2, variableDim2);
                                                                                    ent.modificar(auxiliar[0].getValor().ToString(), resultados[0]).ToString();
                                                                                }
                                                                                catch (IndexOutOfRangeException)
                                                                                {
                                                                                    reportarError = new Error(archivo, "0", "0", "Semantico", "Index fuera del limite");
                                                                                    Errores.Add(reportarError);
                                                                                }
                                                                            }
                                                                            else
                                                                            {
                                                                                reportarError = new Error(archivo, "0", "0", "Semantico", "Los tamaños no coinciden");
                                                                                Errores.Add(reportarError);
                                                                            }
                                                                        }
                                                                        else
                                                                        {
                                                                            reportarError = new Error(archivo, "0", "0", "Semantico", "Se esperaba tipo entero, tipo" + index[2].tipo + " encontrado");
                                                                            Errores.Add(reportarError);
                                                                        }
                                                                    }
                                                                    else
                                                                    {
                                                                        reportarError = new Error(archivo, "0", "0", "Semantico", "" + index[2].valor);
                                                                        Errores.Add(reportarError);
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    reportarError = new Error(archivo, "0", "0", "Semantico", "Se esperaba tipo entero, tipo" + index[1].tipo + " encontrado");
                                                                    Errores.Add(reportarError);
                                                                }
                                                            }
                                                            else
                                                            {
                                                                reportarError = new Error(archivo, "0", "0", "Semantico", "" + index[1].valor);
                                                                Errores.Add(reportarError);
                                                            }
                                                        }
                                                        else
                                                        {
                                                            reportarError = new Error(archivo, "0", "0", "Semantico", "Se esperaba tipo entero, tipo" + index[0].tipo + " encontrado");
                                                            Errores.Add(reportarError);
                                                        }
                                                    }
                                                    else
                                                    {
                                                        reportarError = new Error(archivo, "0", "0", "Semantico", "" + index[0].valor);
                                                        Errores.Add(reportarError);
                                                    }
                                                }
                                                else
                                                {
                                                    reportarError = new Error(archivo, "0", "0", "Semantico", "El arreglo no puede tener mas de 1 dimension");
                                                    Errores.Add(reportarError);
                                                }
                                                break;
                                            case Simbolo.EnumTipo.dim2:
                                                switch (listaHijos2.Count)
                                                {
                                                    case 3:
                                                        //Tomamos los nodos donde se encuentran los indices a modificar
                                                        auxiliar[2] = (NodoSintactico)listaHijos1[1];
                                                        auxiliar[3] = (NodoSintactico)listaHijos1[2];
                                                        auxiliar[4] = (NodoSintactico)listaHijos2[1];
                                                        auxiliar[5] = (NodoSintactico)listaHijos2[2];
                                                        //Obtenemos los indices
                                                        index[0] = resolverExpresion(auxiliar[2], ent, archivo);
                                                        index[1] = resolverExpresion(auxiliar[3], ent, archivo);
                                                        index[2] = resolverExpresion(auxiliar[4], ent, archivo);
                                                        index[3] = resolverExpresion(auxiliar[5], ent, archivo);
                                                        if (index[0].tipo != Simbolo.EnumTipo.error)
                                                        {
                                                            if (index[0].tipo == Simbolo.EnumTipo.entero)
                                                            {
                                                                if (index[1].tipo != Simbolo.EnumTipo.error)
                                                                {
                                                                    if (index[1].tipo == Simbolo.EnumTipo.entero)
                                                                    {
                                                                        if (index[2].tipo != Simbolo.EnumTipo.error)
                                                                        {
                                                                            if (index[2].tipo == Simbolo.EnumTipo.entero)
                                                                            {
                                                                                if (index[3].tipo != Simbolo.EnumTipo.error)
                                                                                {
                                                                                    if (index[3].tipo == Simbolo.EnumTipo.entero)
                                                                                    {
                                                                                        //Operar reasignacion
                                                                                        variableDim2 = (Simbolo[,])simbolos[0].valor;
                                                                                        nuevoDim2 = (Simbolo[,])simbolos[1].valor;
                                                                                        if (variableDim2.GetLength(1) == nuevoDim2.GetLength(1))
                                                                                        {
                                                                                            try
                                                                                            {
                                                                                                variableDim2[int.Parse(index[0].valor.ToString()), int.Parse(index[1].valor.ToString())] = nuevoDim2[int.Parse(index[2].valor.ToString()), int.Parse(index[3].valor.ToString())];
                                                                                                resultados[0] = new Simbolo(Simbolo.EnumTipo.dim2, variableDim2);
                                                                                                ent.modificar(auxiliar[0].getValor().ToString(), resultados[0]).ToString();
                                                                                            }
                                                                                            catch (IndexOutOfRangeException)
                                                                                            {
                                                                                                reportarError = new Error(archivo, "0", "0", "Semantico", "Index fuera del limite");
                                                                                                Errores.Add(reportarError);
                                                                                            }
                                                                                        }
                                                                                        else
                                                                                        {
                                                                                            reportarError = new Error(archivo, "0", "0", "Semantico", "Los tamaños no coinciden");
                                                                                            Errores.Add(reportarError);
                                                                                        }
                                                                                    }
                                                                                    else
                                                                                    {
                                                                                        reportarError = new Error(archivo, "0", "0", "Semantico", "Se esperaba tipo entero, tipo" + index[3].tipo + " encontrado");
                                                                                        Errores.Add(reportarError);
                                                                                    }
                                                                                }
                                                                                else
                                                                                {
                                                                                    reportarError = new Error(archivo, "0", "0", "Semantico", "" + index[3].valor);
                                                                                    Errores.Add(reportarError);
                                                                                }
                                                                            }
                                                                            else
                                                                            {
                                                                                reportarError = new Error(archivo, "0", "0", "Semantico", "Se esperaba tipo entero, tipo" + index[2].tipo + " encontrado");
                                                                                Errores.Add(reportarError);
                                                                            }
                                                                        }
                                                                        else
                                                                        {
                                                                            reportarError = new Error(archivo, "0", "0", "Semantico", "" + index[2].valor);
                                                                            Errores.Add(reportarError);
                                                                        }
                                                                    }
                                                                    else
                                                                    {
                                                                        reportarError = new Error(archivo, "0", "0", "Semantico", "Se esperaba tipo entero, tipo" + index[1].tipo + " encontrado");
                                                                        Errores.Add(reportarError);
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    reportarError = new Error(archivo, "0", "0", "Semantico", "" + index[1].valor);
                                                                    Errores.Add(reportarError);
                                                                }
                                                            }
                                                            else
                                                            {
                                                                reportarError = new Error(archivo, "0", "0", "Semantico", "Se esperaba tipo entero, tipo" + index[0].tipo + " encontrado");
                                                                Errores.Add(reportarError);
                                                            }
                                                        }
                                                        else
                                                        {
                                                            reportarError = new Error(archivo, "0", "0", "Semantico", "" + index[0].valor);
                                                            Errores.Add(reportarError);
                                                        }
                                                        break;
                                                    case 2:
                                                        reportarError = new Error(archivo, "0", "0", "Semantico", "Se esperaba dato, arreglo encontrado");
                                                        Errores.Add(reportarError);
                                                        break;
                                                    case 4:
                                                        reportarError = new Error(archivo, "0", "0", "Semantico", "El arreglo no puede tener mas de 2 dimensiones");
                                                        Errores.Add(reportarError);
                                                        break;
                                                }
                                                break;
                                            case Simbolo.EnumTipo.dim3:
                                                switch (listaHijos2.Count)
                                                {
                                                    case 4:
                                                        //Tomamos los nodos donde se encuentran los indices a modificar
                                                        auxiliar[2] = (NodoSintactico)listaHijos1[1];
                                                        auxiliar[3] = (NodoSintactico)listaHijos1[2];
                                                        auxiliar[4] = (NodoSintactico)listaHijos2[1];
                                                        auxiliar[5] = (NodoSintactico)listaHijos2[2];
                                                        auxiliar[6] = (NodoSintactico)listaHijos2[3];
                                                        //Obtenemos los indices
                                                        index[0] = resolverExpresion(auxiliar[2], ent, archivo);
                                                        index[1] = resolverExpresion(auxiliar[3], ent, archivo);
                                                        index[2] = resolverExpresion(auxiliar[4], ent, archivo);
                                                        index[3] = resolverExpresion(auxiliar[5], ent, archivo);
                                                        index[4] = resolverExpresion(auxiliar[6], ent, archivo);
                                                        if (index[0].tipo != Simbolo.EnumTipo.error)
                                                        {
                                                            if (index[0].tipo == Simbolo.EnumTipo.entero)
                                                            {
                                                                if (index[1].tipo != Simbolo.EnumTipo.error)
                                                                {
                                                                    if (index[1].tipo == Simbolo.EnumTipo.entero)
                                                                    {
                                                                        if (index[2].tipo != Simbolo.EnumTipo.error)
                                                                        {
                                                                            if (index[2].tipo == Simbolo.EnumTipo.entero)
                                                                            {
                                                                                if (index[3].tipo != Simbolo.EnumTipo.error)
                                                                                {
                                                                                    if (index[3].tipo == Simbolo.EnumTipo.entero)
                                                                                    {
                                                                                        if (index[4].tipo != Simbolo.EnumTipo.error)
                                                                                        {
                                                                                            if (index[4].tipo == Simbolo.EnumTipo.entero)
                                                                                            {
                                                                                                //Operar reasignacion
                                                                                                variableDim2 = (Simbolo[,])simbolos[0].valor;
                                                                                                nuevoDim3 = (Simbolo[,,])simbolos[1].valor;
                                                                                                if (variableDim2.GetLength(1) == nuevoDim3.GetLength(2))
                                                                                                {
                                                                                                    try
                                                                                                    {
                                                                                                        variableDim2[int.Parse(index[0].valor.ToString()), int.Parse(index[1].valor.ToString())] = nuevoDim3[int.Parse(index[2].valor.ToString()), int.Parse(index[3].valor.ToString()), int.Parse(index[4].valor.ToString())];
                                                                                                        resultados[0] = new Simbolo(Simbolo.EnumTipo.dim2, variableDim2);
                                                                                                        ent.modificar(auxiliar[0].getValor().ToString(), resultados[0]).ToString();
                                                                                                    }
                                                                                                    catch (IndexOutOfRangeException)
                                                                                                    {
                                                                                                        reportarError = new Error(archivo, "0", "0", "Semantico", "Index fuera del limite");
                                                                                                        Errores.Add(reportarError);
                                                                                                    }
                                                                                                }
                                                                                                else
                                                                                                {
                                                                                                    reportarError = new Error(archivo, "0", "0", "Semantico", "Los tamaños no coinciden");
                                                                                                    Errores.Add(reportarError);
                                                                                                }
                                                                                            }
                                                                                            else
                                                                                            {
                                                                                                reportarError = new Error(archivo, "0", "0", "Semantico", "Se esperaba tipo entero, tipo" + index[4].tipo + " encontrado");
                                                                                                Errores.Add(reportarError);
                                                                                            }
                                                                                        }
                                                                                        else
                                                                                        {
                                                                                            reportarError = new Error(archivo, "0", "0", "Semantico", "" + index[4].valor);
                                                                                            Errores.Add(reportarError);
                                                                                        }
                                                                                    }
                                                                                    else
                                                                                    {
                                                                                        reportarError = new Error(archivo, "0", "0", "Semantico", "Se esperaba tipo entero, tipo" + index[3].tipo + " encontrado");
                                                                                        Errores.Add(reportarError);
                                                                                    }
                                                                                }
                                                                                else
                                                                                {
                                                                                    reportarError = new Error(archivo, "0", "0", "Semantico", "" + index[3].valor);
                                                                                    Errores.Add(reportarError);
                                                                                }
                                                                            }
                                                                            else
                                                                            {
                                                                                reportarError = new Error(archivo, "0", "0", "Semantico", "Se esperaba tipo entero, tipo" + index[2].tipo + " encontrado");
                                                                                Errores.Add(reportarError);
                                                                            }
                                                                        }
                                                                        else
                                                                        {
                                                                            reportarError = new Error(archivo, "0", "0", "Semantico", "" + index[2].valor);
                                                                            Errores.Add(reportarError);
                                                                        }
                                                                    }
                                                                    else
                                                                    {
                                                                        reportarError = new Error(archivo, "0", "0", "Semantico", "Se esperaba tipo entero, tipo" + index[1].tipo + " encontrado");
                                                                        Errores.Add(reportarError);
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    reportarError = new Error(archivo, "0", "0", "Semantico", "" + index[1].valor);
                                                                    Errores.Add(reportarError);
                                                                }
                                                            }
                                                            else
                                                            {
                                                                reportarError = new Error(archivo, "0", "0", "Semantico", "Se esperaba tipo entero, tipo" + index[0].tipo + " encontrado");
                                                                Errores.Add(reportarError);
                                                            }
                                                        }
                                                        else
                                                        {
                                                            reportarError = new Error(archivo, "0", "0", "Semantico", "" + index[0].valor);
                                                            Errores.Add(reportarError);
                                                        }
                                                        break;
                                                    case 3:
                                                        reportarError = new Error(archivo, "0", "0", "Semantico", "Se esperaba dato, arreglo encontrado");
                                                        Errores.Add(reportarError);
                                                        break;
                                                    case 2:
                                                        reportarError = new Error(archivo, "0", "0", "Semantico", "Se esperaba dato, arreglo encontrado");
                                                        Errores.Add(reportarError);
                                                        break;
                                                }
                                                break;
                                        }
                                    }
                                    else if (simbolos[0].tipo == Simbolo.EnumTipo.dim3)
                                    {
                                        switch (simbolos[1].tipo)
                                        {
                                            case Simbolo.EnumTipo.dim1:
                                                if (listaHijos2.Count == 2)
                                                {
                                                    reportarError = new Error(archivo, "0", "0", "Semantico", "Se esperaba arreglo de 2 dimension, dato puntual encontrado");
                                                    Errores.Add(reportarError);
                                                }
                                                break;
                                            case Simbolo.EnumTipo.dim2:
                                                if (listaHijos2.Count == 3)
                                                {
                                                    reportarError = new Error(archivo, "0", "0", "Semantico", "Se esperaba arreglo de 2 dimension, dato puntual encontrado");
                                                    Errores.Add(reportarError);
                                                }
                                                else if (listaHijos2.Count == 2)
                                                {
                                                    reportarError = new Error(archivo, "0", "0", "Semantico", "Se esperaba arreglo de 1 dimension, arreglo de 1 dimension encontrado");
                                                    Errores.Add(reportarError);
                                                }
                                                break;
                                            case Simbolo.EnumTipo.dim3:
                                                switch (listaHijos2.Count)
                                                {
                                                    case 2:
                                                        //Tomamos los nodos donde se encuentran los indices a modificar
                                                        auxiliar[2] = (NodoSintactico)listaHijos1[1];
                                                        auxiliar[3] = (NodoSintactico)listaHijos2[1];
                                                        //Obtenemos los indices
                                                        index[0] = resolverExpresion(auxiliar[2], ent, archivo);
                                                        index[1] = resolverExpresion(auxiliar[3], ent, archivo);
                                                        if (index[0].tipo != Simbolo.EnumTipo.error)
                                                        {
                                                            if (index[0].tipo == Simbolo.EnumTipo.entero)
                                                            {
                                                                if (index[1].tipo != Simbolo.EnumTipo.error)
                                                                {
                                                                    if (index[1].tipo == Simbolo.EnumTipo.entero)
                                                                    {
                                                                        //Operar reasignacion
                                                                        variableDim3 = (Simbolo[,,])simbolos[0].valor;
                                                                        nuevoDim3 = (Simbolo[,,])simbolos[1].valor;
                                                                        if (variableDim3.GetLength(1) == nuevoDim3.GetLength(1) && variableDim3.GetLength(2) == nuevoDim3.GetLength(2))
                                                                        {
                                                                            try
                                                                            {
                                                                                for (int i = 0; i < nuevoDim3.GetLength(1); i++)
                                                                                {
                                                                                    for (int j = 0; i < nuevoDim3.GetLength(2); i++)
                                                                                    {
                                                                                        variableDim3[int.Parse(index[0].valor.ToString()), i, j] = nuevoDim3[int.Parse(index[1].valor.ToString()), i, j];
                                                                                        resultados[0] = new Simbolo(Simbolo.EnumTipo.dim3, variableDim3);
                                                                                        ent.modificar(auxiliar[0].getValor().ToString(), resultados[0]).ToString();
                                                                                    }
                                                                                }
                                                                            }
                                                                            catch (IndexOutOfRangeException)
                                                                            {
                                                                                reportarError = new Error(archivo, "0", "0", "Semantico", "Index fuera del limite");
                                                                                Errores.Add(reportarError);
                                                                            }
                                                                        }
                                                                        else
                                                                        {
                                                                            reportarError = new Error(archivo, "0", "0", "Semantico", "Los tamaños no coinciden");
                                                                            Errores.Add(reportarError);
                                                                        }
                                                                    }
                                                                    else
                                                                    {
                                                                        reportarError = new Error(archivo, "0", "0", "Semantico", "Se esperaba tipo entero, tipo" + index[1].tipo + " encontrado");
                                                                        Errores.Add(reportarError);
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    reportarError = new Error(archivo, "0", "0", "Semantico", "" + index[1].valor);
                                                                    Errores.Add(reportarError);
                                                                }
                                                            }
                                                            else
                                                            {
                                                                reportarError = new Error(archivo, "0", "0", "Semantico", "Se esperaba tipo entero, tipo" + index[0].tipo + " encontrado");
                                                                Errores.Add(reportarError);
                                                            }
                                                        }
                                                        else
                                                        {
                                                            reportarError = new Error(archivo, "0", "0", "Semantico", "" + index[0].valor);
                                                            Errores.Add(reportarError);
                                                        }
                                                        break;
                                                    case 3:
                                                        reportarError = new Error(archivo, "0", "0", "Semantico", "Se esperaba arreglo de 1 dimension, arreglo de 1 dimension encontrado");
                                                        Errores.Add(reportarError);
                                                        break;
                                                    case 4:
                                                        reportarError = new Error(archivo, "0", "0", "Semantico", "Se esperaba arreglo de 2 dimension, dato puntual encontrado");
                                                        Errores.Add(reportarError);
                                                        break;
                                                }
                                                break;
                                        }
                                    }
                                }
                                else
                                {
                                    reportarError = new Error(archivo, "0", "0", "Semantico", "La variable " + auxiliar[1].getValor().ToString() + " ya existe");
                                    Errores.Add(reportarError);
                                }
                            }
                            else
                            {
                                reportarError = new Error(archivo, "0", "0", "Semantico", "La variable " + auxiliar[0].getValor().ToString() + " ya existe");
                                Errores.Add(reportarError);
                            }
                            break;
                        #endregion

                        #region Objeto o funcion
                        case "EOTRO":
                            //Tomamos los nodos donde se encuentran los nombres de las variables
                            auxiliar[0] = (NodoSintactico)listaHijos1[0];
                            //Buscamos las variables para verificar el tipo de dato que son
                            simbolos[0] = ent.buscar(auxiliar[0].getValor().ToString(), 0, 0);
                            if (simbolos[0] != null)
                            {
                                switch (simbolos[0].tipo)
                                {
                                    #region arregloDim1[indice][indice]  #ERROR#
                                    case Simbolo.EnumTipo.dim1:
                                        reportarError = new Error(archivo, "0", "0", "Semantico", "El arreglo no puede tener mas de 1 dimension");
                                        Errores.Add(reportarError);
                                        break;
                                    #endregion

                                    #region arregloDim2[indice][indice] = dim1, dim2, dim3 #ERROR#
                                    case Simbolo.EnumTipo.dim2:
                                        simbolos[1] = ent.buscar(nuevo.getValor().ToString(), 0, 0);
                                        if (simbolos[1] != null)
                                        {
                                            switch (simbolos[1].tipo)
                                            {
                                                case Simbolo.EnumTipo.dim1:
                                                    reportarError = new Error(archivo, "0", "0", "Semantico", "Se esperaba dato, arreglo encontrado");
                                                    Errores.Add(reportarError);
                                                    break;
                                                case Simbolo.EnumTipo.dim2:
                                                    reportarError = new Error(archivo, "0", "0", "Semantico", "Se esperaba dato, arreglo encontrado");
                                                    Errores.Add(reportarError);
                                                    break;
                                                case Simbolo.EnumTipo.dim3:
                                                    reportarError = new Error(archivo, "0", "0", "Semantico", "Se esperaba dato, arreglo encontrado");
                                                    Errores.Add(reportarError);
                                                    break;
                                                default:
                                                    auxiliar[2] = (NodoSintactico)listaHijos1[1];
                                                    auxiliar[3] = (NodoSintactico)listaHijos1[2];
                                                    //Obtenemos los indices
                                                    index[0] = resolverExpresion(auxiliar[2], ent, archivo);
                                                    index[1] = resolverExpresion(auxiliar[3], ent, archivo);
                                                    if (index[0].tipo != Simbolo.EnumTipo.error)
                                                    {
                                                        if (index[0].tipo == Simbolo.EnumTipo.entero)
                                                        {
                                                            if (index[1].tipo != Simbolo.EnumTipo.error)
                                                            {
                                                                if (index[1].tipo == Simbolo.EnumTipo.entero)
                                                                {
                                                                    //Operar reasignacion
                                                                    variableDim2 = (Simbolo[,])simbolos[0].valor;
                                                                    expresiones[0] = resolverExpresion(nuevo, ent, archivo);
                                                                    if (expresiones[0].tipo != Simbolo.EnumTipo.error)
                                                                    {
                                                                        try
                                                                        {
                                                                            resultados[0] = new Simbolo(expresiones[0].tipo, expresiones[0].valor);
                                                                            variableDim2[int.Parse(index[0].valor.ToString()), int.Parse(index[1].valor.ToString())] = resultados[0];
                                                                            resultados[1] = new Simbolo(Simbolo.EnumTipo.dim2, variableDim2);
                                                                            ent.modificar(auxiliar[0].getValor().ToString(), resultados[1]).ToString();
                                                                        }
                                                                        catch (IndexOutOfRangeException)
                                                                        {
                                                                            reportarError = new Error(archivo, "0", "0", "Semantico", "Index fuera del limite");
                                                                            Errores.Add(reportarError);
                                                                        }
                                                                    }
                                                                    else
                                                                    {
                                                                        reportarError = new Error(archivo, "0", "0", "Semantico", "" + expresiones[0].valor);
                                                                        Errores.Add(reportarError);
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    reportarError = new Error(archivo, "0", "0", "Semantico", "Se esperaba tipo entero, tipo" + index[1].tipo + " encontrado");
                                                                    Errores.Add(reportarError);
                                                                }
                                                            }
                                                            else
                                                            {
                                                                reportarError = new Error(archivo, "0", "0", "Semantico", "" + index[1].valor);
                                                                Errores.Add(reportarError);
                                                            }
                                                        }
                                                        else
                                                        {
                                                            reportarError = new Error(archivo, "0", "0", "Semantico", "Se esperaba tipo entero, tipo" + index[0].tipo + " encontrado");
                                                            Errores.Add(reportarError);
                                                        }
                                                    }
                                                    else
                                                    {
                                                        reportarError = new Error(archivo, "0", "0", "Semantico", "" + index[0].valor);
                                                        Errores.Add(reportarError);
                                                    }
                                                    break;
                                            }
                                        }
                                        else
                                        {
                                            reportarError = new Error(archivo, "0", "0", "Semantico", "La variable " + nuevo.getValor().ToString() + " ya existe");
                                            Errores.Add(reportarError);
                                        }
                                        break;
                                    #endregion

                                    #region arregloDim3[indice][indice] = dim1, dim2 #ERROR# dim3
                                    case Simbolo.EnumTipo.dim3:
                                        auxiliar[2] = (NodoSintactico)listaHijos1[1];
                                        auxiliar[3] = (NodoSintactico)listaHijos1[2];
                                        //Obtenemos los indices
                                        index[0] = resolverExpresion(auxiliar[2], ent, archivo);
                                        index[1] = resolverExpresion(auxiliar[3], ent, archivo);
                                        if (index[0].tipo != Simbolo.EnumTipo.error)
                                        {
                                            if (index[0].tipo == Simbolo.EnumTipo.entero)
                                            {
                                                if (index[1].tipo != Simbolo.EnumTipo.error)
                                                {
                                                    if (index[1].tipo == Simbolo.EnumTipo.entero)
                                                    {
                                                        //Operar reasignacion
                                                        variableDim3 = (Simbolo[,,])simbolos[0].valor;
                                                        expresiones[0] = resolverExpresion(nuevo, ent, archivo);
                                                        if (expresiones[0].tipo != Simbolo.EnumTipo.error)
                                                        {
                                                            try
                                                            {
                                                                resultados[0] = new Simbolo(expresiones[0].tipo, expresiones[0].valor);
                                                                switch (resultados[0].tipo)
                                                                {
                                                                    case Simbolo.EnumTipo.dim2:
                                                                    case Simbolo.EnumTipo.dim3:
                                                                        reportarError = new Error(archivo, "0", "0", "Semantico", "Se esperaba arreglo de 1 dimension, arreglo de " + resultados[0].tipo + " encontrado");
                                                                        Errores.Add(reportarError);
                                                                        break;
                                                                    case Simbolo.EnumTipo.dim1:
                                                                        nuevoDim1 = (Simbolo[])resultados[0].valor;
                                                                        for (int i = 0; i < variableDim3.GetLength(1); i++)
                                                                        {
                                                                            variableDim3[int.Parse(index[0].valor.ToString()), int.Parse(index[1].valor.ToString()), i] = nuevoDim1[i];
                                                                        }
                                                                        resultados[1] = new Simbolo(Simbolo.EnumTipo.dim3, variableDim3);
                                                                        ent.modificar(auxiliar[0].getValor().ToString(), resultados[1]).ToString();
                                                                        break;
                                                                    case Simbolo.EnumTipo.error:
                                                                        reportarError = new Error(archivo, "0", "0", "Semantico", resultados[0].valor + "");
                                                                        Errores.Add(reportarError);
                                                                        break;
                                                                    default:
                                                                        for (int i = 0; i < variableDim3.GetLength(1); i++)
                                                                        {
                                                                            variableDim3[int.Parse(index[0].valor.ToString()), int.Parse(index[1].valor.ToString()), i] = resultados[0];
                                                                        }
                                                                        resultados[1] = new Simbolo(Simbolo.EnumTipo.dim3, variableDim3);
                                                                        ent.modificar(auxiliar[0].getValor().ToString(), resultados[1]).ToString();
                                                                        break;
                                                                }
                                                            }
                                                            catch (IndexOutOfRangeException)
                                                            {
                                                                reportarError = new Error(archivo, "0", "0", "Semantico", "Index fuera del limite");
                                                                Errores.Add(reportarError);
                                                            }
                                                        }
                                                        else
                                                        {
                                                            reportarError = new Error(archivo, "0", "0", "Semantico", "" + expresiones[0].valor);
                                                            Errores.Add(reportarError);
                                                        }
                                                    }
                                                    else
                                                    {
                                                        reportarError = new Error(archivo, "0", "0", "Semantico", "Se esperaba tipo entero, tipo" + index[1].tipo + " encontrado");
                                                        Errores.Add(reportarError);
                                                    }
                                                }
                                                else
                                                {
                                                    reportarError = new Error(archivo, "0", "0", "Semantico", "" + index[1].valor);
                                                    Errores.Add(reportarError);
                                                }
                                            }
                                            else
                                            {
                                                reportarError = new Error(archivo, "0", "0", "Semantico", "Se esperaba tipo entero, tipo" + index[0].tipo + " encontrado");
                                                Errores.Add(reportarError);
                                            }
                                        }
                                        else
                                        {
                                            reportarError = new Error(archivo, "0", "0", "Semantico", "" + index[0].valor);
                                            Errores.Add(reportarError);
                                        }
                                        break;
                                        #endregion
                                }
                            }
                            else
                            {
                                reportarError = new Error(archivo, "0", "0", "Semantico", "La variable " + auxiliar[0].getValor().ToString() + " ya existe");
                                Errores.Add(reportarError);
                            }
                            break;
                        #endregion

                        #region Conjuntos de datos D1, D2, D3
                        case "CONJUNTO_D1":
                        case "CONJUNTO_D2":
                        case "CONJUNTO_D3":
                            //Tomamos los nodos donde se encuentran los nombres de las variables
                            auxiliar[0] = (NodoSintactico)listaHijos1[0];
                            //Buscamos las variables para verificar el tipo de dato que son
                            simbolos[0] = ent.buscar(auxiliar[0].getValor().ToString(), 0, 0);
                            if (simbolos[0] != null)
                            {
                                switch (nuevo.getNombre())
                                {
                                    //dim1 y conjunto D1
                                    case "CONJUNTO_D1":
                                        if (simbolos[0].tipo == Simbolo.EnumTipo.dim2)
                                        {
                                            reportarError = new Error(archivo, "0", "0", "Semantico", "Se esperaba dato, arreglo encontrado");
                                            Errores.Add(reportarError);
                                        }
                                        else if (simbolos[0].tipo == Simbolo.EnumTipo.dim3)
                                        {
                                            //Tomamos los nodos donde se encuentran los indices a modificar
                                            auxiliar[1] = (NodoSintactico)listaHijos1[1];
                                            auxiliar[2] = (NodoSintactico)listaHijos1[2];
                                            //Obtenemos los indices
                                            index[0] = resolverExpresion(auxiliar[1], ent, archivo);
                                            index[1] = resolverExpresion(auxiliar[2], ent, archivo);
                                            if (index[0].tipo != Simbolo.EnumTipo.error)
                                            {
                                                if (index[0].tipo == Simbolo.EnumTipo.entero)
                                                {
                                                    if (index[1].tipo != Simbolo.EnumTipo.error)
                                                    {
                                                        if (index[1].tipo == Simbolo.EnumTipo.entero)
                                                        {
                                                            variableDim3 = (Simbolo[,,])simbolos[0].valor;
                                                            if (variableDim3.GetLength(2) == listaHijos2.Count)
                                                            {
                                                                for (int i = 0; i < listaHijos2.Count; i++)
                                                                {
                                                                    auxiliar[2] = (NodoSintactico)listaHijos2[i];
                                                                    expresiones[0] = resolverExpresion(auxiliar[2], ent, archivo);
                                                                    if (expresiones[0].tipo != Simbolo.EnumTipo.error)
                                                                    {
                                                                        variableDim3[int.Parse(index[0].valor.ToString()), int.Parse(index[1].valor.ToString()), i] = new Simbolo(expresiones[0].tipo, expresiones[0].valor);
                                                                    }
                                                                    else
                                                                    {
                                                                        reportarError = new Error(archivo, "0", "0", "Warning", "" + expresiones[0].valor + " el espacio de memoria tomo el valor nulo");
                                                                        Errores.Add(reportarError);
                                                                        variableDim3[int.Parse(index[0].valor.ToString()), int.Parse(index[1].valor.ToString()), i] = new Simbolo(Simbolo.EnumTipo.nulo, 0);
                                                                    }
                                                                }
                                                                ent.modificar(auxiliar[0].getValor().ToString(), new Simbolo(Simbolo.EnumTipo.dim3, variableDim3));
                                                            }
                                                            else
                                                            {
                                                                reportarError = new Error(archivo, "0", "0", "Semantico", "Los tamaños no coinciden");
                                                                Errores.Add(reportarError);
                                                            }
                                                        }
                                                        else
                                                        {
                                                            reportarError = new Error(archivo, "0", "0", "Semantico", "Se esperaba tipo entero, tipo" + index[1].tipo + " encontrado");
                                                            Errores.Add(reportarError);
                                                        }
                                                    }
                                                    else
                                                    {
                                                        reportarError = new Error(archivo, "0", "0", "Semantico", "" + index[1].valor);
                                                        Errores.Add(reportarError);
                                                    }
                                                }
                                                else
                                                {
                                                    reportarError = new Error(archivo, "0", "0", "Semantico", "Se esperaba tipo entero, tipo" + index[0].tipo + " encontrado");
                                                    Errores.Add(reportarError);
                                                }
                                            }
                                            else
                                            {
                                                reportarError = new Error(archivo, "0", "0", "Semantico", "" + index[0].valor);
                                                Errores.Add(reportarError);
                                            }
                                        }
                                        break;
                                    case "CONJUNTO_D2":
                                        if (simbolos[0].tipo == Simbolo.EnumTipo.dim2)
                                        {
                                            reportarError = new Error(archivo, "0", "0", "Semantico", "Se esperaba dato, arreglo encontrado");
                                            Errores.Add(reportarError);
                                        }
                                        else if (simbolos[0].tipo == Simbolo.EnumTipo.dim3)
                                        {
                                            reportarError = new Error(archivo, "0", "0", "Semantico", "Se esperaba conjunto de datos de 1 dimension, conjunto de datos de 2 dimensiones encontrado");
                                            Errores.Add(reportarError);
                                        }
                                        break;
                                    case "CONJUNTO_D3":
                                        if (simbolos[0].tipo == Simbolo.EnumTipo.dim2)
                                        {
                                            reportarError = new Error(archivo, "0", "0", "Semantico", "Se esperaba dato, arreglo encontrado");
                                            Errores.Add(reportarError);
                                        }
                                        else if (simbolos[0].tipo == Simbolo.EnumTipo.dim3)
                                        {
                                            reportarError = new Error(archivo, "0", "0", "Semantico", "Se esperaba conjunto de datos de 1 dimension, conjunto de datos de 2 dimensiones encontrado");
                                            Errores.Add(reportarError);
                                        }
                                        break;
                                }
                            }
                            else
                            {
                                reportarError = new Error(archivo, "0", "0", "Semantico", "La variable " + auxiliar[0].getValor().ToString() + " ya existe");
                                Errores.Add(reportarError);
                            }
                            break;
                        #endregion

                        #region Datos puntuales: entero, doble, caracter, cadena, boleano, nulo
                        default:
                            //Tomamos los nodos donde se encuentran los nombres de las variables
                            auxiliar[0] = (NodoSintactico)listaHijos1[0];
                            //Buscamos las variables para verificar el tipo de dato que son
                            simbolos[0] = ent.buscar(auxiliar[0].getValor().ToString(), 0, 0);
                            if (simbolos[0] != null)
                            {
                                switch (simbolos[0].tipo)
                                {
                                    #region arregloDim1[indice][indice]  #ERROR#
                                    case Simbolo.EnumTipo.dim1:
                                        reportarError = new Error(archivo, "0", "0", "Semantico", "El arreglo no puede tener mas de 1 dimension");
                                        Errores.Add(reportarError);
                                        break;
                                    #endregion

                                    #region arregloDim2[indice][indice] = dim1, dim2, dim3 #ERROR#
                                    case Simbolo.EnumTipo.dim2:
                                        simbolos[1] = ent.buscar(nuevo.getValor().ToString(), 0, 0);
                                        if (simbolos[1] != null)
                                        {
                                            switch (simbolos[1].tipo)
                                            {
                                                case Simbolo.EnumTipo.dim1:
                                                    reportarError = new Error(archivo, "0", "0", "Semantico", "Se esperaba dato, arreglo encontrado");
                                                    Errores.Add(reportarError);
                                                    break;
                                                case Simbolo.EnumTipo.dim2:
                                                    reportarError = new Error(archivo, "0", "0", "Semantico", "Se esperaba dato, arreglo encontrado");
                                                    Errores.Add(reportarError);
                                                    break;
                                                case Simbolo.EnumTipo.dim3:
                                                    reportarError = new Error(archivo, "0", "0", "Semantico", "Se esperaba dato, arreglo encontrado");
                                                    Errores.Add(reportarError);
                                                    break;
                                                default:
                                                    auxiliar[2] = (NodoSintactico)listaHijos1[1];
                                                    auxiliar[3] = (NodoSintactico)listaHijos1[2];
                                                    //Obtenemos los indices
                                                    index[0] = resolverExpresion(auxiliar[2], ent, archivo);
                                                    index[1] = resolverExpresion(auxiliar[3], ent, archivo);
                                                    if (index[0].tipo != Simbolo.EnumTipo.error)
                                                    {
                                                        if (index[0].tipo == Simbolo.EnumTipo.entero)
                                                        {
                                                            if (index[1].tipo != Simbolo.EnumTipo.error)
                                                            {
                                                                if (index[1].tipo == Simbolo.EnumTipo.entero)
                                                                {
                                                                    //Operar reasignacion
                                                                    variableDim2 = (Simbolo[,])simbolos[0].valor;
                                                                    expresiones[0] = resolverExpresion(nuevo, ent, archivo);
                                                                    if (expresiones[0].tipo != Simbolo.EnumTipo.error)
                                                                    {
                                                                        try
                                                                        {
                                                                            resultados[0] = new Simbolo(expresiones[0].tipo, expresiones[0].valor);
                                                                            variableDim2[int.Parse(index[0].valor.ToString()), int.Parse(index[1].valor.ToString())] = resultados[0];
                                                                            resultados[1] = new Simbolo(Simbolo.EnumTipo.dim2, variableDim2);
                                                                            ent.modificar(auxiliar[0].getValor().ToString(), resultados[1]).ToString();
                                                                        }
                                                                        catch (IndexOutOfRangeException)
                                                                        {
                                                                            reportarError = new Error(archivo, "0", "0", "Semantico", "Index fuera del limite");
                                                                            Errores.Add(reportarError);
                                                                        }
                                                                    }
                                                                    else
                                                                    {
                                                                        reportarError = new Error(archivo, "0", "0", "Semantico", "" + expresiones[0].valor);
                                                                        Errores.Add(reportarError);
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    reportarError = new Error(archivo, "0", "0", "Semantico", "Se esperaba tipo entero, tipo" + index[1].tipo + " encontrado");
                                                                    Errores.Add(reportarError);
                                                                }
                                                            }
                                                            else
                                                            {
                                                                reportarError = new Error(archivo, "0", "0", "Semantico", "" + index[1].valor);
                                                                Errores.Add(reportarError);
                                                            }
                                                        }
                                                        else
                                                        {
                                                            reportarError = new Error(archivo, "0", "0", "Semantico", "Se esperaba tipo entero, tipo" + index[0].tipo + " encontrado");
                                                            Errores.Add(reportarError);
                                                        }
                                                    }
                                                    else
                                                    {
                                                        reportarError = new Error(archivo, "0", "0", "Semantico", "" + index[0].valor);
                                                        Errores.Add(reportarError);
                                                    }
                                                    break;
                                            }
                                        }
                                        else
                                        {
                                            reportarError = new Error(archivo, "0", "0", "Semantico", "La variable " + nuevo.getValor().ToString() + " ya existe");
                                            Errores.Add(reportarError);
                                        }
                                        break;
                                    #endregion

                                    #region arregloDim3[indice][indice] = dim1, dim2 #ERROR# dim3
                                    case Simbolo.EnumTipo.dim3:
                                        if (!nuevo.getNombre().Equals("ID"))
                                        {
                                            auxiliar[2] = (NodoSintactico)listaHijos1[1];
                                            auxiliar[3] = (NodoSintactico)listaHijos1[2];
                                            //Obtenemos los indices
                                            index[0] = resolverExpresion(auxiliar[2], ent, archivo);
                                            index[1] = resolverExpresion(auxiliar[3], ent, archivo);
                                            if (index[0].tipo != Simbolo.EnumTipo.error)
                                            {
                                                if (index[0].tipo == Simbolo.EnumTipo.entero)
                                                {
                                                    if (index[1].tipo != Simbolo.EnumTipo.error)
                                                    {
                                                        if (index[1].tipo == Simbolo.EnumTipo.entero)
                                                        {
                                                            //Operar reasignacion
                                                            variableDim3 = (Simbolo[,,])simbolos[0].valor;
                                                            expresiones[0] = resolverExpresion(nuevo, ent, archivo);
                                                            if (expresiones[0].tipo != Simbolo.EnumTipo.error)
                                                            {
                                                                try
                                                                {
                                                                    resultados[0] = new Simbolo(expresiones[0].tipo, expresiones[0].valor);
                                                                    for (int i = 0; i < variableDim3.GetLength(1); i++)
                                                                    {
                                                                        variableDim3[int.Parse(index[0].valor.ToString()), int.Parse(index[1].valor.ToString()), i] = resultados[0];
                                                                        resultados[1] = new Simbolo(Simbolo.EnumTipo.dim3, variableDim3);
                                                                    }
                                                                    ent.modificar(auxiliar[0].getValor().ToString(), resultados[1]).ToString();
                                                                }
                                                                catch (IndexOutOfRangeException)
                                                                {
                                                                    reportarError = new Error(archivo, "0", "0", "Semantico", "Index fuera del limite");
                                                                    Errores.Add(reportarError);
                                                                }
                                                            }
                                                            else
                                                            {
                                                                reportarError = new Error(archivo, "0", "0", "Semantico", "" + expresiones[0].valor);
                                                                Errores.Add(reportarError);
                                                            }
                                                        }
                                                        else
                                                        {
                                                            reportarError = new Error(archivo, "0", "0", "Semantico", "Se esperaba tipo entero, tipo" + index[1].tipo + " encontrado");
                                                            Errores.Add(reportarError);
                                                        }
                                                    }
                                                    else
                                                    {
                                                        reportarError = new Error(archivo, "0", "0", "Semantico", "" + index[1].valor);
                                                        Errores.Add(reportarError);
                                                    }
                                                }
                                                else
                                                {
                                                    reportarError = new Error(archivo, "0", "0", "Semantico", "Se esperaba tipo entero, tipo" + index[0].tipo + " encontrado");
                                                    Errores.Add(reportarError);
                                                }
                                            }
                                            else
                                            {
                                                reportarError = new Error(archivo, "0", "0", "Semantico", "" + index[0].valor);
                                                Errores.Add(reportarError);
                                            }
                                        }
                                        else
                                        {
                                            simbolos[1] = ent.buscar(nuevo.getValor().ToString(), 0, 0);
                                            if (simbolos[1] != null)
                                            {
                                                switch (simbolos[1].tipo)
                                                {
                                                    case Simbolo.EnumTipo.dim1:
                                                        auxiliar[1] = (NodoSintactico)listaHijos1[1];
                                                        auxiliar[2] = (NodoSintactico)listaHijos1[2];
                                                        //Obtenemos los indices
                                                        index[0] = resolverExpresion(auxiliar[1], ent, archivo);
                                                        index[1] = resolverExpresion(auxiliar[2], ent, archivo);
                                                        if (index[0].tipo != Simbolo.EnumTipo.error)
                                                        {
                                                            if (index[0].tipo == Simbolo.EnumTipo.entero)
                                                            {
                                                                if (index[1].tipo != Simbolo.EnumTipo.error)
                                                                {
                                                                    if (index[1].tipo == Simbolo.EnumTipo.entero)
                                                                    {
                                                                        //Operar reasignacion
                                                                        variableDim3 = (Simbolo[,,])simbolos[0].valor;
                                                                        nuevoDim1 = (Simbolo[])simbolos[1].valor;
                                                                        if (variableDim3.GetLength(2) == nuevoDim1.Length)
                                                                        {
                                                                            for (int i = 0; i < nuevoDim1.Length; i++)
                                                                            {
                                                                                variableDim3[int.Parse(index[0].valor.ToString()), int.Parse(index[1].valor.ToString()), i] = nuevoDim1[i];
                                                                                resultados[0] = new Simbolo(Simbolo.EnumTipo.dim3, variableDim3);
                                                                                ent.modificar(auxiliar[0].getValor().ToString(), resultados[0]).ToString();
                                                                            }
                                                                        }
                                                                        else
                                                                        {
                                                                            reportarError = new Error(archivo, "0", "0", "Semantico", "Los tamaños no coinciden");
                                                                            Errores.Add(reportarError);
                                                                        }
                                                                    }
                                                                    else
                                                                    {
                                                                        reportarError = new Error(archivo, "0", "0", "Semantico", "Se esperaba tipo entero, tipo" + index[1].tipo + " encontrado");
                                                                        Errores.Add(reportarError);
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    reportarError = new Error(archivo, "0", "0", "Semantico", "" + index[1].valor);
                                                                    Errores.Add(reportarError);
                                                                }
                                                            }
                                                            else
                                                            {
                                                                reportarError = new Error(archivo, "0", "0", "Semantico", "Se esperaba tipo entero, tipo" + index[0].tipo + " encontrado");
                                                                Errores.Add(reportarError);
                                                            }
                                                        }
                                                        else
                                                        {
                                                            reportarError = new Error(archivo, "0", "0", "Semantico", "" + index[0].valor);
                                                            Errores.Add(reportarError);
                                                        }
                                                        break;
                                                    case Simbolo.EnumTipo.dim2:
                                                        reportarError = new Error(archivo, "0", "0", "Semantico", "Se esperaba arreglo de 1 dimension, arreglo de 2 dimensiones encontrado");
                                                        Errores.Add(reportarError);
                                                        break;
                                                    case Simbolo.EnumTipo.dim3:
                                                        reportarError = new Error(archivo, "0", "0", "Semantico", "Se esperaba arreglo de 1 dimension, arreglo de 3 dimensiones encontrado");
                                                        Errores.Add(reportarError);
                                                        break;
                                                    default:
                                                        auxiliar[2] = (NodoSintactico)listaHijos1[1];
                                                        auxiliar[3] = (NodoSintactico)listaHijos1[2];
                                                        //Obtenemos los indices
                                                        index[0] = resolverExpresion(auxiliar[2], ent, archivo);
                                                        index[1] = resolverExpresion(auxiliar[3], ent, archivo);
                                                        if (index[0].tipo != Simbolo.EnumTipo.error)
                                                        {
                                                            if (index[0].tipo == Simbolo.EnumTipo.entero)
                                                            {
                                                                if (index[1].tipo != Simbolo.EnumTipo.error)
                                                                {
                                                                    if (index[1].tipo == Simbolo.EnumTipo.entero)
                                                                    {
                                                                        //Operar reasignacion
                                                                        variableDim3 = (Simbolo[,,])simbolos[0].valor;
                                                                        expresiones[0] = resolverExpresion(nuevo, ent, archivo);
                                                                        if (expresiones[0].tipo != Simbolo.EnumTipo.error)
                                                                        {
                                                                            try
                                                                            {
                                                                                resultados[0] = new Simbolo(expresiones[0].tipo, expresiones[0].valor);
                                                                                for (int i = 0; i < variableDim3.GetLength(1); i++)
                                                                                {
                                                                                    variableDim3[int.Parse(index[0].valor.ToString()), int.Parse(index[1].valor.ToString()), i] = resultados[0];
                                                                                    resultados[1] = new Simbolo(Simbolo.EnumTipo.dim3, variableDim3);
                                                                                }
                                                                                ent.modificar(auxiliar[0].getValor().ToString(), resultados[1]).ToString();
                                                                            }
                                                                            catch (IndexOutOfRangeException)
                                                                            {
                                                                                reportarError = new Error(archivo, "0", "0", "Semantico", "Index fuera del limite");
                                                                                Errores.Add(reportarError);
                                                                            }
                                                                        }
                                                                        else
                                                                        {
                                                                            reportarError = new Error(archivo, "0", "0", "Semantico", "" + expresiones[0].valor);
                                                                            Errores.Add(reportarError);
                                                                        }
                                                                    }
                                                                    else
                                                                    {
                                                                        reportarError = new Error(archivo, "0", "0", "Semantico", "Se esperaba tipo entero, tipo" + index[1].tipo + " encontrado");
                                                                        Errores.Add(reportarError);
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    reportarError = new Error(archivo, "0", "0", "Semantico", "" + index[1].valor);
                                                                    Errores.Add(reportarError);
                                                                }
                                                            }
                                                            else
                                                            {
                                                                reportarError = new Error(archivo, "0", "0", "Semantico", "Se esperaba tipo entero, tipo" + index[0].tipo + " encontrado");
                                                                Errores.Add(reportarError);
                                                            }
                                                        }
                                                        else
                                                        {
                                                            reportarError = new Error(archivo, "0", "0", "Semantico", "" + index[0].valor);
                                                            Errores.Add(reportarError);
                                                        }
                                                        break;
                                                }
                                            }
                                            else
                                            {
                                                reportarError = new Error(archivo, "0", "0", "Semantico", "La variable " + nuevo.getValor().ToString() + " ya existe");
                                                Errores.Add(reportarError);
                                            }
                                        }
                                        break;
                                        #endregion
                                }
                            }
                            else
                            {
                                reportarError = new Error(archivo, "0", "0", "Semantico", "La variable " + auxiliar[0].getValor().ToString() + " ya existe");
                                Errores.Add(reportarError);
                            }
                            break;
                            #endregion
                    }
                    break;
                #endregion

                #region DIM3
                case "DIM3":
                    switch (nuevo.getNombre())
                    {
                        #region Arreglos
                        case "DIM1":
                            //Tomamos los nodos donde se encuentran los nombres de las variables
                            auxiliar[0] = (NodoSintactico)listaHijos1[0];
                            auxiliar[1] = (NodoSintactico)listaHijos2[0];
                            //Buscamos las variables para verificar el tipo de dato que son
                            simbolos[0] = ent.buscar(auxiliar[0].getValor().ToString(), 0, 0);
                            simbolos[1] = ent.buscar(auxiliar[1].getValor().ToString(), 0, 0);
                            if (simbolos[0] != null)
                            {
                                if (simbolos[1] != null)
                                {
                                    if (simbolos[0].tipo == Simbolo.EnumTipo.dim1)
                                    {
                                        reportarError = new Error(archivo, "0", "0", "Semantico", "El arreglo no puede tener mas de 1 dimension");
                                        Errores.Add(reportarError);
                                    }
                                    else if (simbolos[0].tipo == Simbolo.EnumTipo.dim2)
                                    {
                                        reportarError = new Error(archivo, "0", "0", "Semantico", "El arreglo no puede tener mas de 2 dimensiones");
                                        Errores.Add(reportarError);
                                    }
                                    else if (simbolos[0].tipo == Simbolo.EnumTipo.dim3)
                                    {
                                        switch (simbolos[1].tipo)
                                        {
                                            case Simbolo.EnumTipo.dim1:
                                                //Tomamos los nodos donde se encuentran los indices a modificar
                                                auxiliar[2] = (NodoSintactico)listaHijos1[1];
                                                auxiliar[3] = (NodoSintactico)listaHijos1[2];
                                                auxiliar[4] = (NodoSintactico)listaHijos1[3];
                                                auxiliar[5] = (NodoSintactico)listaHijos2[1];
                                                //Obtenemos los indices
                                                index[0] = resolverExpresion(auxiliar[2], ent, archivo);
                                                index[1] = resolverExpresion(auxiliar[3], ent, archivo);
                                                index[2] = resolverExpresion(auxiliar[4], ent, archivo);
                                                index[3] = resolverExpresion(auxiliar[5], ent, archivo);
                                                if (index[0].tipo != Simbolo.EnumTipo.error)
                                                {
                                                    if (index[0].tipo == Simbolo.EnumTipo.entero)
                                                    {
                                                        if (index[1].tipo != Simbolo.EnumTipo.error)
                                                        {
                                                            if (index[1].tipo == Simbolo.EnumTipo.entero)
                                                            {
                                                                if (index[2].tipo != Simbolo.EnumTipo.error)
                                                                {
                                                                    if (index[2].tipo == Simbolo.EnumTipo.entero)
                                                                    {
                                                                        if (index[2].tipo != Simbolo.EnumTipo.error)
                                                                        {
                                                                            if (index[2].tipo == Simbolo.EnumTipo.entero)
                                                                            {
                                                                                //Operar reasignacion
                                                                                variableDim3 = (Simbolo[,,])simbolos[0].valor;
                                                                                nuevoDim1 = (Simbolo[])simbolos[1].valor;
                                                                                if (variableDim3.GetLength(2) == nuevoDim1.Length)
                                                                                {
                                                                                    try
                                                                                    {
                                                                                        variableDim3[int.Parse(index[0].valor.ToString()), int.Parse(index[1].valor.ToString()), int.Parse(index[2].valor.ToString())] = nuevoDim1[int.Parse(index[3].valor.ToString())];
                                                                                        resultados[0] = new Simbolo(Simbolo.EnumTipo.dim3, variableDim3);
                                                                                        ent.modificar(auxiliar[0].getValor().ToString(), resultados[0]).ToString();
                                                                                    }
                                                                                    catch (IndexOutOfRangeException)
                                                                                    {
                                                                                        reportarError = new Error(archivo, "0", "0", "Semantico", "Index fuera del limite");
                                                                                        Errores.Add(reportarError);
                                                                                    }
                                                                                }
                                                                                else
                                                                                {
                                                                                    reportarError = new Error(archivo, "0", "0", "Semantico", "Los tamaños no coinciden");
                                                                                    Errores.Add(reportarError);
                                                                                }
                                                                            }
                                                                            else
                                                                            {
                                                                                reportarError = new Error(archivo, "0", "0", "Semantico", "Se esperaba tipo entero, tipo" + index[3].tipo + " encontrado");
                                                                                Errores.Add(reportarError);
                                                                            }
                                                                        }
                                                                        else
                                                                        {
                                                                            reportarError = new Error(archivo, "0", "0", "Semantico", "" + index[2].valor);
                                                                            Errores.Add(reportarError);
                                                                        }
                                                                    }
                                                                    else
                                                                    {
                                                                        reportarError = new Error(archivo, "0", "0", "Semantico", "Se esperaba tipo entero, tipo" + index[2].tipo + " encontrado");
                                                                        Errores.Add(reportarError);
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    reportarError = new Error(archivo, "0", "0", "Semantico", "" + index[2].valor);
                                                                    Errores.Add(reportarError);
                                                                }
                                                            }
                                                            else
                                                            {
                                                                reportarError = new Error(archivo, "0", "0", "Semantico", "Se esperaba tipo entero, tipo" + index[1].tipo + " encontrado");
                                                                Errores.Add(reportarError);
                                                            }
                                                        }
                                                        else
                                                        {
                                                            reportarError = new Error(archivo, "0", "0", "Semantico", "" + index[1].valor);
                                                            Errores.Add(reportarError);
                                                        }
                                                    }
                                                    else
                                                    {
                                                        reportarError = new Error(archivo, "0", "0", "Semantico", "Se esperaba tipo entero, tipo" + index[0].tipo + " encontrado");
                                                        Errores.Add(reportarError);
                                                    }
                                                }
                                                else
                                                {
                                                    reportarError = new Error(archivo, "0", "0", "Semantico", "" + index[0].valor);
                                                    Errores.Add(reportarError);
                                                }
                                                break;
                                            case Simbolo.EnumTipo.dim2:
                                                reportarError = new Error(archivo, "0", "0", "Semantico", "Se esperaba dato, arreglo encontrado");
                                                Errores.Add(reportarError);
                                                break;
                                            case Simbolo.EnumTipo.dim3:
                                                reportarError = new Error(archivo, "0", "0", "Semantico", "Se esperaba dato, arreglo encontrado");
                                                Errores.Add(reportarError);
                                                break;
                                        }
                                    }
                                }
                                else
                                {
                                    reportarError = new Error(archivo, "0", "0", "Semantico", "La variable " + auxiliar[1].getValor().ToString() + " ya existe");
                                    Errores.Add(reportarError);
                                }
                            }
                            else
                            {
                                reportarError = new Error(archivo, "0", "0", "Semantico", "La variable " + auxiliar[0].getValor().ToString() + " ya existe");
                                Errores.Add(reportarError);
                            }
                            break;
                        case "DIM2":
                        case "DIM3":
                            reportarError = new Error(archivo, "0", "0", "Semantico", "Se esperaba dato, arreglo encontrado");
                            Errores.Add(reportarError);
                            break;
                        #endregion

                        #region Objeto o funcion
                        case "EOTRO":
                            //Tomamos los nodos donde se encuentran los nombres de las variables
                            auxiliar[0] = (NodoSintactico)listaHijos1[0];
                            //Buscamos las variables para verificar el tipo de dato que son
                            simbolos[0] = ent.buscar(auxiliar[0].getValor().ToString(), 0, 0);
                            if (simbolos[0] != null)
                            {
                                switch (simbolos[0].tipo)
                                {
                                    #region arregloDim1[indice][indice][indice] #ERROR#
                                    case Simbolo.EnumTipo.dim1:
                                        reportarError = new Error(archivo, "0", "0", "Semantico", "El arreglo no puede tener mas de 1 dimension");
                                        Errores.Add(reportarError);
                                        break;
                                    #endregion

                                    #region arregloDim2[indice][indice][indice] #ERROR#
                                    case Simbolo.EnumTipo.dim2:
                                        reportarError = new Error(archivo, "0", "0", "Semantico", "El arreglo no puede tener mas de 1 dimension");
                                        Errores.Add(reportarError);
                                        break;
                                    #endregion

                                    #region arregloDim3[indice][indice][indice] = dim1, dim2, dim3 #ERROR#
                                    case Simbolo.EnumTipo.dim3:
                                        auxiliar[2] = (NodoSintactico)listaHijos1[1];
                                        auxiliar[3] = (NodoSintactico)listaHijos1[2];
                                        auxiliar[4] = (NodoSintactico)listaHijos1[3];
                                        //Obtenemos los indices
                                        index[0] = resolverExpresion(auxiliar[2], ent, archivo);
                                        index[1] = resolverExpresion(auxiliar[3], ent, archivo);
                                        index[2] = resolverExpresion(auxiliar[4], ent, archivo);
                                        if (index[0].tipo != Simbolo.EnumTipo.error)
                                        {
                                            if (index[0].tipo == Simbolo.EnumTipo.entero)
                                            {
                                                if (index[1].tipo != Simbolo.EnumTipo.error)
                                                {
                                                    if (index[1].tipo == Simbolo.EnumTipo.entero)
                                                    {
                                                        if (index[2].tipo != Simbolo.EnumTipo.error)
                                                        {
                                                            if (index[2].tipo == Simbolo.EnumTipo.entero)
                                                            {
                                                                //Operar reasignacion
                                                                variableDim3 = (Simbolo[,,])simbolos[0].valor;
                                                                expresiones[0] = resolverExpresion(nuevo, ent, archivo);
                                                                if (expresiones[0].tipo != Simbolo.EnumTipo.error)
                                                                {
                                                                    try
                                                                    {
                                                                        resultados[0] = new Simbolo(expresiones[0].tipo, expresiones[0].valor);
                                                                        variableDim3[int.Parse(index[0].valor.ToString()), int.Parse(index[1].valor.ToString()), int.Parse(index[2].valor.ToString())] = resultados[0];
                                                                        resultados[1] = new Simbolo(Simbolo.EnumTipo.dim3, variableDim3);
                                                                        ent.modificar(auxiliar[0].getValor().ToString(), resultados[1]).ToString();
                                                                    }
                                                                    catch (IndexOutOfRangeException)
                                                                    {
                                                                        reportarError = new Error(archivo, "0", "0", "Semantico", "Index fuera del limite");
                                                                        Errores.Add(reportarError);
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    reportarError = new Error(archivo, "0", "0", "Semantico", "" + expresiones[0].valor);
                                                                    Errores.Add(reportarError);
                                                                }
                                                            }
                                                        }
                                                    }
                                                    else
                                                    {
                                                        reportarError = new Error(archivo, "0", "0", "Semantico", "Se esperaba tipo entero, tipo" + index[1].tipo + " encontrado");
                                                        Errores.Add(reportarError);
                                                    }
                                                }
                                                else
                                                {
                                                    reportarError = new Error(archivo, "0", "0", "Semantico", "" + index[1].valor);
                                                    Errores.Add(reportarError);
                                                }
                                            }
                                            else
                                            {
                                                reportarError = new Error(archivo, "0", "0", "Semantico", "Se esperaba tipo entero, tipo" + index[0].tipo + " encontrado");
                                                Errores.Add(reportarError);
                                            }
                                        }
                                        else
                                        {
                                            reportarError = new Error(archivo, "0", "0", "Semantico", "" + index[0].valor);
                                            Errores.Add(reportarError);
                                        }
                                        break;
                                        #endregion
                                }
                            }
                            else
                            {
                                reportarError = new Error(archivo, "0", "0", "Semantico", "La variable " + auxiliar[0].getValor().ToString() + " ya existe");
                                Errores.Add(reportarError);
                            }
                            break;
                        #endregion

                        #region Conjuntos de datos D1, D2, D3
                        case "CONJUNTO_D1":
                        case "CONJUNTO_D2":
                        case "CONJUNTO_D3":
                            reportarError = new Error(archivo, "0", "0", "Semantico", "Se esperaba dato, arreglo encontrado");
                            Errores.Add(reportarError);
                            break;
                        #endregion

                        #region Datos puntuales: entero, doble, caracter, cadena, boleano, nulo
                        default:
                            //Tomamos los nodos donde se encuentran los nombres de las variables
                            auxiliar[0] = (NodoSintactico)listaHijos1[0];
                            //Buscamos las variables para verificar el tipo de dato que son
                            simbolos[0] = ent.buscar(auxiliar[0].getValor().ToString(), 0, 0);
                            if (simbolos[0] != null)
                            {
                                switch (simbolos[0].tipo)
                                {
                                    #region arregloDim1[indice][indice][indice] #ERROR#
                                    case Simbolo.EnumTipo.dim1:
                                        reportarError = new Error(archivo, "0", "0", "Semantico", "El arreglo no puede tener mas de 1 dimension");
                                        Errores.Add(reportarError);
                                        break;
                                    #endregion

                                    #region arregloDim2[indice][indice][indice] #ERROR#
                                    case Simbolo.EnumTipo.dim2:
                                        reportarError = new Error(archivo, "0", "0", "Semantico", "El arreglo no puede tener mas de 1 dimension");
                                        Errores.Add(reportarError);
                                        break;
                                    #endregion

                                    #region arregloDim3[indice][indice][indice] = dim1, dim2, dim3 #ERROR#
                                    case Simbolo.EnumTipo.dim3:
                                        if (!nuevo.getNombre().Equals("ID"))
                                        {
                                            auxiliar[2] = (NodoSintactico)listaHijos1[1];
                                            auxiliar[3] = (NodoSintactico)listaHijos1[2];
                                            auxiliar[4] = (NodoSintactico)listaHijos1[3];
                                            //Obtenemos los indices
                                            index[0] = resolverExpresion(auxiliar[2], ent, archivo);
                                            index[1] = resolverExpresion(auxiliar[3], ent, archivo);
                                            index[2] = resolverExpresion(auxiliar[4], ent, archivo);
                                            if (index[0].tipo != Simbolo.EnumTipo.error)
                                            {
                                                if (index[0].tipo == Simbolo.EnumTipo.entero)
                                                {
                                                    if (index[1].tipo != Simbolo.EnumTipo.error)
                                                    {
                                                        if (index[1].tipo == Simbolo.EnumTipo.entero)
                                                        {
                                                            if (index[2].tipo != Simbolo.EnumTipo.error)
                                                            {
                                                                if (index[2].tipo == Simbolo.EnumTipo.entero)
                                                                {
                                                                    //Operar reasignacion
                                                                    variableDim3 = (Simbolo[,,])simbolos[0].valor;
                                                                    expresiones[0] = resolverExpresion(nuevo, ent, archivo);
                                                                    if (expresiones[0].tipo != Simbolo.EnumTipo.error)
                                                                    {
                                                                        try
                                                                        {
                                                                            resultados[0] = new Simbolo(expresiones[0].tipo, expresiones[0].valor);
                                                                            variableDim3[int.Parse(index[0].valor.ToString()), int.Parse(index[1].valor.ToString()), int.Parse(index[2].valor.ToString())] = resultados[0];
                                                                            resultados[1] = new Simbolo(Simbolo.EnumTipo.dim3, variableDim3);
                                                                            ent.modificar(auxiliar[0].getValor().ToString(), resultados[1]).ToString();
                                                                        }
                                                                        catch (IndexOutOfRangeException)
                                                                        {
                                                                            reportarError = new Error(archivo, "0", "0", "Semantico", "Index fuera del limite");
                                                                            Errores.Add(reportarError);
                                                                        }
                                                                    }
                                                                    else
                                                                    {
                                                                        reportarError = new Error(archivo, "0", "0", "Semantico", "" + expresiones[0].valor);
                                                                        Errores.Add(reportarError);
                                                                    }
                                                                }
                                                            }
                                                        }
                                                        else
                                                        {
                                                            reportarError = new Error(archivo, "0", "0", "Semantico", "Se esperaba tipo entero, tipo" + index[1].tipo + " encontrado");
                                                            Errores.Add(reportarError);
                                                        }
                                                    }
                                                    else
                                                    {
                                                        reportarError = new Error(archivo, "0", "0", "Semantico", "" + index[1].valor);
                                                        Errores.Add(reportarError);
                                                    }
                                                }
                                                else
                                                {
                                                    reportarError = new Error(archivo, "0", "0", "Semantico", "Se esperaba tipo entero, tipo" + index[0].tipo + " encontrado");
                                                    Errores.Add(reportarError);
                                                }
                                            }
                                            else
                                            {
                                                reportarError = new Error(archivo, "0", "0", "Semantico", "" + index[0].valor);
                                                Errores.Add(reportarError);
                                            }
                                        }
                                        else
                                        {
                                            simbolos[1] = ent.buscar(nuevo.getValor().ToString(), 0, 0);
                                            if (simbolos[1] != null)
                                            {
                                                switch (simbolos[1].tipo)
                                                {
                                                    case Simbolo.EnumTipo.dim1:
                                                        reportarError = new Error(archivo, "0", "0", "Semantico", "Se esperaba dato, arreglo encontrado");
                                                        Errores.Add(reportarError);
                                                        break;
                                                    case Simbolo.EnumTipo.dim2:
                                                        reportarError = new Error(archivo, "0", "0", "Semantico", "Se esperaba arreglo de 1 dimension, arreglo de 2 dimensiones encontrado");
                                                        Errores.Add(reportarError);
                                                        break;
                                                    case Simbolo.EnumTipo.dim3:
                                                        reportarError = new Error(archivo, "0", "0", "Semantico", "Se esperaba arreglo de 1 dimension, arreglo de 3 dimensiones encontrado");
                                                        Errores.Add(reportarError);
                                                        break;
                                                    default:
                                                        auxiliar[2] = (NodoSintactico)listaHijos1[1];
                                                        auxiliar[3] = (NodoSintactico)listaHijos1[2];
                                                        auxiliar[4] = (NodoSintactico)listaHijos1[3];
                                                        //Obtenemos los indices
                                                        index[0] = resolverExpresion(auxiliar[2], ent, archivo);
                                                        index[1] = resolverExpresion(auxiliar[3], ent, archivo);
                                                        index[2] = resolverExpresion(auxiliar[4], ent, archivo);
                                                        if (index[0].tipo != Simbolo.EnumTipo.error)
                                                        {
                                                            if (index[0].tipo == Simbolo.EnumTipo.entero)
                                                            {
                                                                if (index[1].tipo != Simbolo.EnumTipo.error)
                                                                {
                                                                    if (index[1].tipo == Simbolo.EnumTipo.entero)
                                                                    {
                                                                        if (index[2].tipo != Simbolo.EnumTipo.error)
                                                                        {
                                                                            if (index[2].tipo == Simbolo.EnumTipo.entero)
                                                                            {
                                                                                //Operar reasignacion
                                                                                variableDim3 = (Simbolo[,,])simbolos[0].valor;
                                                                                expresiones[0] = resolverExpresion(nuevo, ent, archivo);
                                                                                if (expresiones[0].tipo != Simbolo.EnumTipo.error)
                                                                                {
                                                                                    try
                                                                                    {
                                                                                        variableDim3[int.Parse(index[0].valor.ToString()), int.Parse(index[1].valor.ToString()), int.Parse(index[2].valor.ToString())] = resultados[0];
                                                                                        resultados[1] = new Simbolo(Simbolo.EnumTipo.dim3, variableDim3);
                                                                                        ent.modificar(auxiliar[0].getValor().ToString(), resultados[1]).ToString();
                                                                                    }
                                                                                    catch (IndexOutOfRangeException)
                                                                                    {
                                                                                        reportarError = new Error(archivo, "0", "0", "Semantico", "Index fuera del limite");
                                                                                        Errores.Add(reportarError);
                                                                                    }
                                                                                }
                                                                                else
                                                                                {
                                                                                    reportarError = new Error(archivo, "0", "0", "Semantico", "" + expresiones[0].valor);
                                                                                    Errores.Add(reportarError);
                                                                                }
                                                                            }
                                                                            else
                                                                            {
                                                                                reportarError = new Error(archivo, "0", "0", "Semantico", "Se esperaba tipo entero, tipo" + index[2].tipo + " encontrado");
                                                                                Errores.Add(reportarError);
                                                                            }
                                                                        }
                                                                        else
                                                                        {
                                                                            reportarError = new Error(archivo, "0", "0", "Semantico", "" + index[2].valor);
                                                                            Errores.Add(reportarError);
                                                                        }
                                                                    }
                                                                    else
                                                                    {
                                                                        reportarError = new Error(archivo, "0", "0", "Semantico", "Se esperaba tipo entero, tipo" + index[1].tipo + " encontrado");
                                                                        Errores.Add(reportarError);
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    reportarError = new Error(archivo, "0", "0", "Semantico", "" + index[1].valor);
                                                                    Errores.Add(reportarError);
                                                                }
                                                            }
                                                            else
                                                            {
                                                                reportarError = new Error(archivo, "0", "0", "Semantico", "Se esperaba tipo entero, tipo" + index[0].tipo + " encontrado");
                                                                Errores.Add(reportarError);
                                                            }
                                                        }
                                                        else
                                                        {
                                                            reportarError = new Error(archivo, "0", "0", "Semantico", "" + index[0].valor);
                                                            Errores.Add(reportarError);
                                                        }
                                                        break;
                                                }
                                            }
                                            else
                                            {
                                                reportarError = new Error(archivo, "0", "0", "Semantico", "La variable " + nuevo.getValor().ToString() + " ya existe");
                                                Errores.Add(reportarError);
                                            }
                                        }
                                        break;
                                        #endregion
                                }
                            }
                            else
                            {
                                reportarError = new Error(archivo, "0", "0", "Semantico", "La variable " + auxiliar[0].getValor().ToString() + " ya existe");
                                Errores.Add(reportarError);
                            }
                            break;
                            #endregion
                    }
                    break;
                #endregion

                #region LINEAL o sin Indice en la variable a modificar (Puede ser dim1, dim2, dim3)
                default:
                    //Buscamos las variables para verificar el tipo de dato que son
                    simbolos[0] = ent.buscar(variable.getValor().ToString(), 0, 0);
                    if (simbolos[0] != null)
                    {
                        switch (simbolos[0].tipo)
                        {
                            #region dim1
                            case Simbolo.EnumTipo.dim1:
                                switch (nuevo.getNombre())
                                {
                                    #region Segundo dato es un arreglo
                                    case "DIM1":
                                    case "DIM2":
                                    case "DIM3":
                                        auxiliar[1] = (NodoSintactico)listaHijos2[0];
                                        simbolos[1] = ent.buscar(auxiliar[1].getValor().ToString(), 0, 0);
                                        if (simbolos[1] != null)
                                        {
                                            switch (simbolos[1].tipo)
                                            {
                                                case Simbolo.EnumTipo.dim2:
                                                    if (listaHijos2.Count == 2)
                                                    {
                                                        auxiliar[2] = (NodoSintactico)listaHijos2[1];
                                                        index[0] = resolverExpresion(auxiliar[2], ent, archivo);
                                                        if (index[0].tipo != Simbolo.EnumTipo.error)
                                                        {
                                                            if (index[0].tipo == Simbolo.EnumTipo.entero)
                                                            {
                                                                try
                                                                {
                                                                    variableDim1 = (Simbolo[])simbolos[0].valor;
                                                                    nuevoDim2 = (Simbolo[,])simbolos[1].valor;
                                                                    if (variableDim1.Length == nuevoDim2.Length / nuevoDim2.Rank)
                                                                    {
                                                                        for (int i = 0; i < variableDim1.Length; i++)
                                                                        {
                                                                            variableDim1[i] = nuevoDim2[int.Parse(index[0].valor.ToString()), i];
                                                                        }
                                                                        ent.modificar(variable.getValor().ToString(), new Simbolo(Simbolo.EnumTipo.dim1, variableDim1));
                                                                    }
                                                                    else
                                                                    {
                                                                        reportarError = new Error(archivo, "0", "0", "Semantico", "Los tamaños no coinciden");
                                                                        Errores.Add(reportarError);
                                                                    }
                                                                }
                                                                catch (IndexOutOfRangeException)
                                                                {
                                                                    reportarError = new Error(archivo, "0", "0", "Semantico", "Indice fuera del rango");
                                                                    Errores.Add(reportarError);
                                                                }
                                                            }
                                                            else
                                                            {
                                                                reportarError = new Error(archivo, "0", "0", "Semantico", "Se esperaba tipo entero, tipo " + index[0].tipo + " encontrado");
                                                                Errores.Add(reportarError);
                                                            }
                                                        }
                                                        else
                                                        {
                                                            reportarError = new Error(archivo, "0", "0", "Semantico", "" + index[0].valor);
                                                            Errores.Add(reportarError);
                                                        }
                                                    }
                                                    else
                                                    {
                                                        reportarError = new Error(archivo, "0", "0", "Semantico", "Se esperaba conjunto de datos de 1 dimension, dato puntual");
                                                        Errores.Add(reportarError);
                                                    }
                                                    break;
                                                case Simbolo.EnumTipo.dim3:
                                                    if (listaHijos2.Count == 3)
                                                    {
                                                        auxiliar[2] = (NodoSintactico)listaHijos2[1];
                                                        auxiliar[3] = (NodoSintactico)listaHijos2[2];
                                                        index[0] = resolverExpresion(auxiliar[2], ent, archivo);
                                                        index[1] = resolverExpresion(auxiliar[3], ent, archivo);
                                                        if (index[0].tipo != Simbolo.EnumTipo.error)
                                                        {
                                                            if (index[0].tipo == Simbolo.EnumTipo.entero)
                                                            {
                                                                if (index[1].tipo != Simbolo.EnumTipo.error)
                                                                {
                                                                    if (index[1].tipo == Simbolo.EnumTipo.entero)
                                                                    {
                                                                        try
                                                                        {
                                                                            variableDim1 = (Simbolo[])simbolos[0].valor;
                                                                            nuevoDim3 = (Simbolo[,,])simbolos[1].valor;
                                                                            if (variableDim1.Length == nuevoDim3.Length / nuevoDim3.Rank)
                                                                            {
                                                                                for (int i = 0; i < variableDim1.Length; i++)
                                                                                {
                                                                                    variableDim1[i] = nuevoDim3[int.Parse(index[0].valor.ToString()), int.Parse(index[1].valor.ToString()), i];
                                                                                }
                                                                                ent.modificar(variable.getValor().ToString(), new Simbolo(Simbolo.EnumTipo.dim1, variableDim1));
                                                                            }
                                                                            else
                                                                            {
                                                                                reportarError = new Error(archivo, "0", "0", "Semantico", "Los tamaños no coinciden");
                                                                                Errores.Add(reportarError);
                                                                            }
                                                                        }
                                                                        catch (IndexOutOfRangeException)
                                                                        {
                                                                            reportarError = new Error(archivo, "0", "0", "Semantico", "Indice fuera del rango");
                                                                            Errores.Add(reportarError);
                                                                        }
                                                                    }
                                                                    else
                                                                    {
                                                                        reportarError = new Error(archivo, "0", "0", "Semantico", "Se esperaba tipo entero, tipo " + index[1].tipo + " encontrado");
                                                                        Errores.Add(reportarError);
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    reportarError = new Error(archivo, "0", "0", "Semantico", "" + index[1].valor);
                                                                    Errores.Add(reportarError);
                                                                }
                                                            }
                                                            else
                                                            {
                                                                reportarError = new Error(archivo, "0", "0", "Semantico", "Se esperaba tipo entero, tipo " + index[0].tipo + " encontrado");
                                                                Errores.Add(reportarError);
                                                            }
                                                        }
                                                        else
                                                        {
                                                            reportarError = new Error(archivo, "0", "0", "Semantico", "" + index[0].valor);
                                                            Errores.Add(reportarError);
                                                        }
                                                    }
                                                    else if (listaHijos2.Count == 2)
                                                    {
                                                        reportarError = new Error(archivo, "0", "0", "Semantico", "Se esperaba arreglo de 1 dimension, arreglo de 2 dimensiones encontrado");
                                                        Errores.Add(reportarError);
                                                    }
                                                    else
                                                    {
                                                        reportarError = new Error(archivo, "0", "0", "Semantico", "Se esperaba arreglo de 1 dimension, dato puntual encontrado");
                                                        Errores.Add(reportarError);
                                                    }
                                                    break;
                                            }
                                        }
                                        else
                                        {
                                            reportarError = new Error(archivo, "0", "0", "Semantico", "La variable " + auxiliar[1].getValor().ToString() + " ya existe");
                                            Errores.Add(reportarError);
                                        }
                                        break;
                                    #endregion

                                    #region Conjuntos de datos D1, D2, D3
                                    case "CONJUNTO_D1":
                                    case "CONJUNTO_D2":
                                    case "CONJUNTO_D3":
                                        if (variable.getNombre().Equals("ID"))
                                        {
                                            switch (nuevo.getNombre())
                                            {
                                                //dim1 y conjunto D1
                                                case "CONJUNTO_D1":
                                                    variableDim1 = (Simbolo[])simbolos[0].valor;
                                                    if (listaHijos2.Count == variableDim1.Length)
                                                    {
                                                        //La cantidad de datos en el conjunto equivale a la cantidad de espacios del arreglo
                                                        for (int i = 0; i < listaHijos2.Count; i++)
                                                        {
                                                            auxiliar[1] = (NodoSintactico)listaHijos2[i];
                                                            expresiones[0] = resolverExpresion(auxiliar[1], ent, archivo);
                                                            if (expresiones[0].tipo == Simbolo.EnumTipo.error)
                                                            {
                                                                reportarError = new Error(archivo, "0", "0", "Semantico", "" + expresiones[0].tipo);
                                                                Errores.Add(reportarError);
                                                                variableDim1 = null;
                                                                break;
                                                            }
                                                            variableDim1[i] = new Simbolo(expresiones[0].tipo, expresiones[0].valor);
                                                        }
                                                        if (variableDim1 != null)
                                                        {
                                                            ent.modificar(variable.getValor().ToString(), new Simbolo(Simbolo.EnumTipo.dim1, variableDim1));
                                                        }
                                                    }
                                                    else
                                                    {
                                                        reportarError = new Error(archivo, "0", "0", "Semantico", "Los tamaños no coinciden");
                                                        Errores.Add(reportarError);
                                                    }
                                                    break;
                                                case "CONJUNTO_D2":
                                                    reportarError = new Error(archivo, "0", "0", "Semantico", "Se esperaba arreglo de 1 dimension, arreglo de 2 dimensiones encontrado");
                                                    Errores.Add(reportarError);
                                                    break;
                                                case "CONJUNTO_D3":
                                                    reportarError = new Error(archivo, "0", "0", "Semantico", "Se esperaba arreglo de 1 dimension, arreglo de 3 dimensiones encontrado");
                                                    Errores.Add(reportarError);
                                                    break;
                                            }
                                        }
                                        else
                                        {
                                            reportarError = new Error(archivo, "0", "0", "Semantico", "No se esperaba indice del arreglo");
                                            Errores.Add(reportarError);
                                        }
                                        break;
                                    #endregion

                                    #region Segundo dato es Objeto o funcion
                                    case "EOTRO":
                                        expresiones[0] = resolverExpresion(nuevo, ent, archivo);
                                        if (expresiones[0].tipo != Simbolo.EnumTipo.error)
                                        {
                                            if (expresiones[0].tipo == Simbolo.EnumTipo.dim1)
                                            {
                                                variableDim1 = (Simbolo[])expresiones[0].valor;
                                                if (!ent.modificar(variable.getValor().ToString(), new Simbolo(Simbolo.EnumTipo.dim1, variableDim1)))
                                                {
                                                    reportarError = new Error(archivo, "0", "0", "Semantico", "La variable " + variable.getValor().ToString() + " ya existe");
                                                    Errores.Add(reportarError);
                                                }
                                            }
                                            else
                                            {
                                                reportarError = new Error(archivo, "0", "0", "Semantico", "Se esperaba arreglo de 1 dimension, tipo " + expresiones[0].tipo + " encontrado");
                                                Errores.Add(reportarError);
                                            }
                                        }
                                        else
                                        {
                                            reportarError = new Error(archivo, "0", "0", "Semantico", "" + expresiones[0].valor);
                                            Errores.Add(reportarError);
                                        }
                                        break;
                                    #endregion

                                    #region El segundo dato es un Lineal o sin Indice en la variable dato (Puede ser dim1, dim2, dim3)
                                    default:
                                        simbolos[1] = ent.buscar(nuevo.getValor().ToString(), 0, 0);
                                        if (simbolos[1] != null)
                                        {
                                            if (simbolos[1].tipo == Simbolo.EnumTipo.dim1)
                                            {
                                                ent.modificar(variable.getValor().ToString(), simbolos[1]);
                                            }
                                            else if (simbolos[1].tipo == Simbolo.EnumTipo.dim2)
                                            {
                                                reportarError = new Error(archivo, "0", "0", "Semantico", "Se esperaba arreglo de 1 dimension, arreglo de 2 dimensiones encontrado");
                                                Errores.Add(reportarError);
                                            }
                                            else if (simbolos[1].tipo == Simbolo.EnumTipo.dim3)
                                            {
                                                reportarError = new Error(archivo, "0", "0", "Semantico", "Se esperaba arreglo de 1 dimension, arreglo de 3 dimensiones encontrado");
                                                Errores.Add(reportarError);
                                            }
                                            else
                                            {
                                                expresiones[0] = resolverExpresion(nuevo, ent, archivo);
                                                if (expresiones[0].tipo != Simbolo.EnumTipo.error)
                                                {
                                                    simbolos[1] = new Simbolo(expresiones[0].tipo, expresiones[0].valor);
                                                    variableDim1 = (Simbolo[])simbolos[0].valor;
                                                    for (int i = 0; i < variableDim1.Length; i++)
                                                    {
                                                        variableDim1[i] = simbolos[1];
                                                    }
                                                    simbolos[2] = new Simbolo(Simbolo.EnumTipo.dim1, variableDim1);
                                                    if (!ent.modificar(variable.getValor().ToString(), simbolos[2]))
                                                    {
                                                        reportarError = new Error(archivo, "0", "0", "Semantico", "La variable " + variable.getValor().ToString() + " ya existe");
                                                        Errores.Add(reportarError);
                                                    }
                                                }
                                                else
                                                {
                                                    reportarError = new Error(archivo, "0", "0", "Semantico", "" + expresiones[0].valor);
                                                    Errores.Add(reportarError);
                                                }
                                            }
                                        }
                                        else
                                        {
                                            expresiones[0] = resolverExpresion(nuevo, ent, archivo);
                                            if (expresiones[0].tipo != Simbolo.EnumTipo.error)
                                            {
                                                simbolos[1] = new Simbolo(expresiones[0].tipo, expresiones[0].valor);
                                                variableDim1 = (Simbolo[])simbolos[0].valor;
                                                for (int i = 0; i < variableDim1.Length; i++)
                                                {
                                                    variableDim1[i] = simbolos[1];
                                                }
                                                simbolos[2] = new Simbolo(Simbolo.EnumTipo.dim1, variableDim1);
                                                if (!ent.modificar(variable.getValor().ToString(), simbolos[2]))
                                                {
                                                    reportarError = new Error(archivo, "0", "0", "Semantico", "La variable " + variable.getValor().ToString() + " ya existe");
                                                    Errores.Add(reportarError);
                                                }
                                            }
                                            else
                                            {
                                                reportarError = new Error(archivo, "0", "0", "Semantico", "" + expresiones[0].valor);
                                                Errores.Add(reportarError);
                                            }
                                        }
                                        break;
                                        #endregion
                                }
                                break;
                            #endregion

                            #region dim2
                            case Simbolo.EnumTipo.dim2:
                                switch (nuevo.getNombre())
                                {
                                    #region Segundo dato es un arreglo
                                    case "DIM1":
                                    case "DIM2":
                                    case "DIM3":
                                        auxiliar[1] = (NodoSintactico)listaHijos2[0];
                                        simbolos[1] = ent.buscar(auxiliar[1].getValor().ToString(), 0, 0);
                                        if (simbolos[1] != null)
                                        {
                                            switch (simbolos[1].tipo)
                                            {
                                                case Simbolo.EnumTipo.dim1:
                                                    if (listaHijos2.Count == 2)
                                                    {
                                                        reportarError = new Error(archivo, "0", "0", "Semantico", "Se esperaba arreglo de 2 dimension, dato puntual encontrado");
                                                        Errores.Add(reportarError);
                                                    }
                                                    break;
                                                case Simbolo.EnumTipo.dim2:
                                                    if (listaHijos2.Count == 2)
                                                    {
                                                        reportarError = new Error(archivo, "0", "0", "Semantico", "Se esperaba arreglo de 1 dimension, arreglo de 1 dimension encontrado");
                                                        Errores.Add(reportarError);
                                                    }
                                                    else if (listaHijos2.Count == 3)
                                                    {
                                                        reportarError = new Error(archivo, "0", "0", "Semantico", "Se esperaba arreglo de 2 dimension, dato puntual encontrado");
                                                        Errores.Add(reportarError);
                                                    }
                                                    break;
                                                case Simbolo.EnumTipo.dim3:
                                                    if (listaHijos2.Count == 2)
                                                    {
                                                        auxiliar[2] = (NodoSintactico)listaHijos2[1];
                                                        index[0] = resolverExpresion(auxiliar[2], ent, archivo);
                                                        if (index[0].tipo != Simbolo.EnumTipo.error)
                                                        {
                                                            if (index[0].tipo == Simbolo.EnumTipo.entero)
                                                            {
                                                                try
                                                                {
                                                                    variableDim2 = (Simbolo[,])simbolos[0].valor;
                                                                    nuevoDim3 = (Simbolo[,,])simbolos[1].valor;
                                                                    if (variableDim2.GetLength(0) == nuevoDim3.GetLength(1) && variableDim2.GetLength(1) == nuevoDim3.GetLength(2))
                                                                    {
                                                                        for (int i = 0; i < nuevoDim3.GetLength(1); i++)
                                                                        {
                                                                            for (int j = 0; j < nuevoDim3.GetLength(0); j++)
                                                                            {
                                                                                variableDim2[i, j] = nuevoDim3[int.Parse(index[0].valor.ToString()), i, j];
                                                                            }

                                                                        }
                                                                        ent.modificar(variable.getValor().ToString(), new Simbolo(Simbolo.EnumTipo.dim2, variableDim2));
                                                                    }
                                                                    else
                                                                    {
                                                                        reportarError = new Error(archivo, "0", "0", "Semantico", "Los tamaños no coinciden");
                                                                        Errores.Add(reportarError);
                                                                    }
                                                                }
                                                                catch (IndexOutOfRangeException)
                                                                {
                                                                    reportarError = new Error(archivo, "0", "0", "Semantico", "Indice fuera del rango");
                                                                    Errores.Add(reportarError);
                                                                }
                                                            }
                                                            else
                                                            {
                                                                reportarError = new Error(archivo, "0", "0", "Semantico", "Se esperaba tipo entero, tipo " + index[0].tipo + " encontrado");
                                                                Errores.Add(reportarError);
                                                            }
                                                        }
                                                        else
                                                        {
                                                            reportarError = new Error(archivo, "0", "0", "Semantico", "" + index[0].valor);
                                                            Errores.Add(reportarError);
                                                        }
                                                    }
                                                    else if (listaHijos2.Count == 3)
                                                    {
                                                        reportarError = new Error(archivo, "0", "0", "Semantico", "Se esperaba arreglo de 1 dimension, arreglo de 1 dimension encontrado");
                                                        Errores.Add(reportarError);
                                                    }
                                                    else
                                                    {
                                                        reportarError = new Error(archivo, "0", "0", "Semantico", "Se esperaba arreglo de 2 dimension, dato puntual encontrado");
                                                        Errores.Add(reportarError);
                                                    }
                                                    break;
                                            }
                                        }
                                        else
                                        {
                                            reportarError = new Error(archivo, "0", "0", "Semantico", "La variable " + auxiliar[1].getValor().ToString() + " ya existe");
                                            Errores.Add(reportarError);
                                        }
                                        break;
                                    #endregion

                                    #region Conjuntos de datos D1, D2, D3
                                    case "CONJUNTO_D1":
                                    case "CONJUNTO_D2":
                                    case "CONJUNTO_D3":
                                        if (variable.getNombre().Equals("ID"))
                                        {
                                            switch (nuevo.getNombre())
                                            {
                                                //dim1 y conjunto D1
                                                case "CONJUNTO_D1":
                                                    reportarError = new Error(archivo, "0", "0", "Semantico", "Se esperaba conjunto de datos de 2 dimensiones, conjunto de datos de 1 dimension encontrado");
                                                    Errores.Add(reportarError);
                                                    break;
                                                case "CONJUNTO_D2":
                                                    variableDim2 = (Simbolo[,])simbolos[0].valor;
                                                    if (listaHijos2.Count == variableDim2.GetLength(0))
                                                    {
                                                        //La cantidad de arreglos de dimension 1 en el conjunto equivale a la cantidad de espacios de dimension 1 del arreglo
                                                        for (int i = 0; i < listaHijos2.Count; i++)
                                                        {
                                                            auxiliar[1] = (NodoSintactico)listaHijos2[i];
                                                            listaHijos3 = auxiliar[1].getHijos();
                                                            if (listaHijos3.Count == variableDim2.GetLength(1))
                                                            {
                                                                for (int j = 0; j < listaHijos3.Count; j++)
                                                                {
                                                                    auxiliar[2] = (NodoSintactico)listaHijos3[j];
                                                                    expresiones[0] = resolverExpresion(auxiliar[2], ent, archivo);
                                                                    if (expresiones[0].tipo == Simbolo.EnumTipo.error)
                                                                    {
                                                                        reportarError = new Error(archivo, "0", "0", "Semantico", "" + expresiones[0].tipo);
                                                                        Errores.Add(reportarError);
                                                                        variableDim1 = null;
                                                                        break;
                                                                    }
                                                                    variableDim2[i, j] = new Simbolo(expresiones[0].tipo, expresiones[0].valor);
                                                                }
                                                            }
                                                            else
                                                            {
                                                                reportarError = new Error(archivo, "0", "0", "Semantico", "Los tamaños no coinciden");
                                                                Errores.Add(reportarError);
                                                            }
                                                        }
                                                        if (variableDim2 != null)
                                                        {
                                                            ent.modificar(variable.getValor().ToString(), new Simbolo(Simbolo.EnumTipo.dim2, variableDim2));
                                                        }
                                                    }
                                                    else
                                                    {
                                                        reportarError = new Error(archivo, "0", "0", "Semantico", "Los tamaños no coinciden");
                                                        Errores.Add(reportarError);
                                                    }
                                                    break;
                                                case "CONJUNTO_D3":
                                                    reportarError = new Error(archivo, "0", "0", "Semantico", "Se esperaba conjunto de datos de 2 dimensiones, conjunto de datos de 3 dimensiones encontrado");
                                                    Errores.Add(reportarError);
                                                    break;
                                            }
                                        }
                                        else
                                        {
                                            reportarError = new Error(archivo, "0", "0", "Semantico", "No se esperaba indice del arreglo");
                                            Errores.Add(reportarError);
                                        }
                                        break;
                                    #endregion

                                    #region Segundo dato es Objeto o funcion
                                    case "EOTRO":
                                        expresiones[0] = resolverExpresion(nuevo, ent, archivo);
                                        if (expresiones[0].tipo != Simbolo.EnumTipo.error)
                                        {
                                            if (expresiones[0].tipo == Simbolo.EnumTipo.dim2)
                                            {
                                                variableDim2 = (Simbolo[,])expresiones[0].valor;
                                                if (!ent.modificar(variable.getValor().ToString(), new Simbolo(Simbolo.EnumTipo.dim2, variableDim2)))
                                                {
                                                    reportarError = new Error(archivo, "0", "0", "Semantico", "La variable " + variable.getValor().ToString() + " ya existe");
                                                    Errores.Add(reportarError);
                                                }
                                            }
                                            else
                                            {
                                                reportarError = new Error(archivo, "0", "0", "Semantico", "Se esperaba arreglo de 1 dimension, tipo " + expresiones[0].tipo + " encontrado");
                                                Errores.Add(reportarError);
                                            }
                                        }
                                        else
                                        {
                                            reportarError = new Error(archivo, "0", "0", "Semantico", "" + expresiones[0].valor);
                                            Errores.Add(reportarError);
                                        }
                                        break;
                                    #endregion

                                    #region El segundo dato es un Lineal o sin Indice en la variable dato (Puede ser dim1, dim2, dim3)
                                    default:
                                        simbolos[1] = ent.buscar(nuevo.getValor().ToString(), 0, 0);
                                        if (simbolos[1] != null)
                                        {
                                            if (simbolos[1].tipo == Simbolo.EnumTipo.dim1)
                                            {
                                                reportarError = new Error(archivo, "0", "0", "Semantico", "Se esperaba arreglo de 1 dimension, arreglo de 1 dimension encontrado");
                                                Errores.Add(reportarError);
                                            }
                                            else if (simbolos[1].tipo == Simbolo.EnumTipo.dim2)
                                            {
                                                ent.modificar(variable.getValor().ToString(), simbolos[1]);
                                            }
                                            else if (simbolos[1].tipo == Simbolo.EnumTipo.dim3)
                                            {
                                                reportarError = new Error(archivo, "0", "0", "Semantico", "Se esperaba arreglo de 1 dimension, arreglo de 3 dimensiones encontrado");
                                                Errores.Add(reportarError);
                                            }
                                            else
                                            {
                                                expresiones[0] = resolverExpresion(nuevo, ent, archivo);
                                                if (expresiones[0].tipo != Simbolo.EnumTipo.error)
                                                {
                                                    simbolos[1] = new Simbolo(expresiones[0].tipo, expresiones[0].valor);
                                                    variableDim2 = (Simbolo[,])simbolos[0].valor;
                                                    for (int i = 0; i < variableDim2.GetLength(0); i++)
                                                    {
                                                        for (int j = 0; j < variableDim2.GetLength(1); j++)
                                                        {
                                                            variableDim2[i, j] = simbolos[1];
                                                        }
                                                    }
                                                    if (!ent.modificar(variable.getValor().ToString(), new Simbolo(Simbolo.EnumTipo.dim2, variableDim2)))
                                                    {
                                                        reportarError = new Error(archivo, "0", "0", "Semantico", "La variable " + variable.getValor().ToString() + " ya existe");
                                                        Errores.Add(reportarError);
                                                    }
                                                }
                                                else
                                                {
                                                    reportarError = new Error(archivo, "0", "0", "Semantico", "" + expresiones[0].valor);
                                                    Errores.Add(reportarError);
                                                }
                                            }
                                        }
                                        else
                                        {
                                            expresiones[0] = resolverExpresion(nuevo, ent, archivo);
                                            if (expresiones[0].tipo != Simbolo.EnumTipo.error)
                                            {
                                                simbolos[1] = new Simbolo(expresiones[0].tipo, expresiones[0].valor);
                                                variableDim2 = (Simbolo[,])simbolos[0].valor;
                                                for (int i = 0; i < variableDim2.GetLength(0); i++)
                                                {
                                                    for (int j = 0; j < variableDim2.GetLength(1); j++)
                                                    {
                                                        variableDim2[i, j] = simbolos[1];
                                                    }
                                                }
                                                if (!ent.modificar(variable.getValor().ToString(), new Simbolo(Simbolo.EnumTipo.dim2, variableDim2)))
                                                {
                                                    reportarError = new Error(archivo, "0", "0", "Semantico", "La variable " + variable.getValor().ToString() + " ya existe");
                                                    Errores.Add(reportarError);
                                                }
                                            }
                                            else
                                            {
                                                reportarError = new Error(archivo, "0", "0", "Semantico", "" + expresiones[0].valor);
                                                Errores.Add(reportarError);
                                            }
                                        }
                                        break;
                                        #endregion
                                }
                                break;
                            #endregion

                            #region dim3
                            case Simbolo.EnumTipo.dim3:
                                switch (nuevo.getNombre())
                                {
                                    #region Segundo dato es un arreglo
                                    case "DIM1":
                                    case "DIM2":
                                    case "DIM3":
                                        auxiliar[1] = (NodoSintactico)listaHijos2[0];
                                        simbolos[1] = ent.buscar(auxiliar[1].getValor().ToString(), 0, 0);
                                        if (simbolos[1] != null)
                                        {
                                            switch (simbolos[1].tipo)
                                            {
                                                case Simbolo.EnumTipo.dim1:
                                                    if (listaHijos2.Count == 2)
                                                    {
                                                        reportarError = new Error(archivo, "0", "0", "Semantico", "Se esperaba arreglo de 3 dimensiones, dato puntual encontrado");
                                                        Errores.Add(reportarError);
                                                    }
                                                    break;
                                                case Simbolo.EnumTipo.dim2:
                                                    if (listaHijos2.Count == 2)
                                                    {
                                                        reportarError = new Error(archivo, "0", "0", "Semantico", "Se esperaba arreglo de 3 dimensiones, conjunto de datos de 1 dimension encontrado");
                                                        Errores.Add(reportarError);
                                                    }
                                                    else if (listaHijos2.Count == 3)
                                                    {
                                                        reportarError = new Error(archivo, "0", "0", "Semantico", "Se esperaba arreglo de 3 dimensiones, dato puntual encontrado");
                                                        Errores.Add(reportarError);
                                                    }
                                                    break;
                                                case Simbolo.EnumTipo.dim3:
                                                    if (listaHijos2.Count == 2)
                                                    {
                                                        reportarError = new Error(archivo, "0", "0", "Semantico", "Se esperaba arreglo de 3 dimensiones, conjunto de datos de 2 dimensiones encontrado");
                                                        Errores.Add(reportarError);
                                                    }
                                                    else if (listaHijos2.Count == 3)
                                                    {
                                                        reportarError = new Error(archivo, "0", "0", "Semantico", "Se esperaba arreglo de 1 dimension, arreglo de 1 dimension encontrado");
                                                        Errores.Add(reportarError);
                                                    }
                                                    else
                                                    {
                                                        reportarError = new Error(archivo, "0", "0", "Semantico", "Se esperaba arreglo de 2 dimension, dato puntual encontrado");
                                                        Errores.Add(reportarError);
                                                    }
                                                    break;
                                            }
                                        }
                                        else
                                        {
                                            reportarError = new Error(archivo, "0", "0", "Semantico", "La variable " + auxiliar[1].getValor().ToString() + " ya existe");
                                            Errores.Add(reportarError);
                                        }
                                        break;
                                    #endregion

                                    #region Conjuntos de datos D1, D2, D3
                                    case "CONJUNTO_D1":
                                    case "CONJUNTO_D2":
                                    case "CONJUNTO_D3":
                                        if (variable.getNombre().Equals("ID"))
                                        {
                                            switch (nuevo.getNombre())
                                            {
                                                //dim1 y conjunto D1
                                                case "CONJUNTO_D1":
                                                    reportarError = new Error(archivo, "0", "0", "Semantico", "Se esperaba arreglo de 2 dimensiones, conjunto de datos de 1 dimension encontrado");
                                                    Errores.Add(reportarError);
                                                    break;
                                                case "CONJUNTO_D2":
                                                    reportarError = new Error(archivo, "0", "0", "Semantico", "Se esperaba arreglo de 3 dimensiones, conjunto de datos de 2 dimensiones encontrado");
                                                    Errores.Add(reportarError);
                                                    break;
                                                case "CONJUNTO_D3":
                                                    variableDim3 = (Simbolo[,,])simbolos[0].valor;
                                                    if (listaHijos2.Count == variableDim3.GetLength(0))
                                                    {
                                                        //La cantidad de arreglos de dimension 1 en el conjunto equivale a la cantidad de espacios de dimension 1 del arreglo
                                                        for (int i = 0; i < listaHijos2.Count; i++)
                                                        {
                                                            auxiliar[1] = (NodoSintactico)listaHijos2[i];
                                                            listaHijos3 = auxiliar[1].getHijos();
                                                            if (listaHijos3.Count == variableDim3.GetLength(1))
                                                            {
                                                                for (int j = 0; j < listaHijos3.Count; j++)
                                                                {
                                                                    auxiliar[2] = (NodoSintactico)listaHijos3[j];
                                                                    listaHijos4 = auxiliar[2].getHijos();
                                                                    if (listaHijos4.Count == variableDim3.GetLength(2))
                                                                    {
                                                                        for (int k = 0; k < listaHijos4.Count; k++)
                                                                        {
                                                                            auxiliar[3] = (NodoSintactico)listaHijos4[k];
                                                                            expresiones[0] = resolverExpresion(auxiliar[3], ent, archivo);
                                                                            if (expresiones[0].tipo == Simbolo.EnumTipo.error)
                                                                            {
                                                                                reportarError = new Error(archivo, "0", "0", "Semantico", "" + expresiones[0].tipo);
                                                                                Errores.Add(reportarError);
                                                                                variableDim1 = null;
                                                                                break;
                                                                            }
                                                                            variableDim3[i, j, k] = new Simbolo(expresiones[0].tipo, expresiones[0].valor);
                                                                        }
                                                                    }
                                                                    else
                                                                    {
                                                                        reportarError = new Error(archivo, "0", "0", "Semantico", "Los tamaños no coinciden");
                                                                        Errores.Add(reportarError);
                                                                    }
                                                                }
                                                            }
                                                            else
                                                            {
                                                                reportarError = new Error(archivo, "0", "0", "Semantico", "Los tamaños no coinciden");
                                                                Errores.Add(reportarError);
                                                            }
                                                        }
                                                        if (variableDim3 != null)
                                                        {
                                                            ent.modificar(variable.getValor().ToString(), new Simbolo(Simbolo.EnumTipo.dim3, variableDim3));
                                                        }
                                                    }
                                                    else
                                                    {
                                                        reportarError = new Error(archivo, "0", "0", "Semantico", "Los tamaños no coinciden");
                                                        Errores.Add(reportarError);
                                                    }
                                                    break;
                                            }
                                        }
                                        else
                                        {
                                            reportarError = new Error(archivo, "0", "0", "Semantico", "No se esperaba indice del arreglo");
                                            Errores.Add(reportarError);
                                        }
                                        break;
                                    #endregion

                                    #region Segundo dato es Objeto o funcion
                                    case "EOTRO":
                                        expresiones[0] = resolverExpresion(nuevo, ent, archivo);
                                        if (expresiones[0].tipo != Simbolo.EnumTipo.error)
                                        {
                                            if (expresiones[0].tipo == Simbolo.EnumTipo.dim3)
                                            {
                                                variableDim3 = (Simbolo[,,])expresiones[0].valor;
                                                ent.modificar(variable.getValor().ToString(), new Simbolo(Simbolo.EnumTipo.dim3, variableDim3));
                                            }
                                            else
                                            {
                                                reportarError = new Error(archivo, "0", "0", "Semantico", "Se esperaba arreglo de 1 dimension, tipo " + expresiones[0].tipo + " encontrado");
                                                Errores.Add(reportarError);
                                            }
                                        }
                                        else
                                        {
                                            reportarError = new Error(archivo, "0", "0", "Semantico", expresiones[0].valor + "");
                                            Errores.Add(reportarError);
                                        }
                                        break;
                                    #endregion

                                    #region El segundo dato es un Lineal o sin Indice en la variable dato (Puede ser dim1, dim2, dim3)
                                    default:
                                        simbolos[1] = ent.buscar(nuevo.getValor().ToString(), 0, 0);
                                        if (simbolos[1] != null)
                                        {
                                            if (simbolos[1].tipo == Simbolo.EnumTipo.dim1)
                                            {
                                                reportarError = new Error(archivo, "0", "0", "Semantico", "Se esperaba arreglo de 1 dimension, arreglo de 1 dimension encontrado");
                                                Errores.Add(reportarError);
                                            }
                                            else if (simbolos[1].tipo == Simbolo.EnumTipo.dim2)
                                            {
                                                reportarError = new Error(archivo, "0", "0", "Semantico", "Se esperaba arreglo de 3 dimensiones, conjunto de datos de 2 dimensiones encontrado");
                                                Errores.Add(reportarError);
                                            }
                                            else if (simbolos[1].tipo == Simbolo.EnumTipo.dim3)
                                            {
                                                ent.modificar(variable.getValor().ToString(), simbolos[1]);
                                            }
                                            else
                                            {
                                                expresiones[0] = resolverExpresion(nuevo, ent, archivo);
                                                if (expresiones[0].tipo != Simbolo.EnumTipo.error)
                                                {
                                                    simbolos[1] = new Simbolo(expresiones[0].tipo, expresiones[0].valor);
                                                    variableDim3 = (Simbolo[,,])simbolos[0].valor;
                                                    for (int i = 0; i < variableDim3.GetLength(0); i++)
                                                    {
                                                        for (int j = 0; j < variableDim3.GetLength(1); j++)
                                                        {
                                                            for (int k = 0; k < variableDim3.GetLength(2); k++)
                                                            {
                                                                variableDim3[i, j, k] = simbolos[1];
                                                            }
                                                        }
                                                    }
                                                    ent.modificar(variable.getValor().ToString(), new Simbolo(Simbolo.EnumTipo.dim3, variableDim3));
                                                }
                                                else
                                                {
                                                    reportarError = new Error(archivo, "0", "0", "Semantico", "" + expresiones[0].valor);
                                                    Errores.Add(reportarError);
                                                }
                                            }
                                        }
                                        else
                                        {
                                            expresiones[0] = resolverExpresion(nuevo, ent, archivo);
                                            if (expresiones[0].tipo != Simbolo.EnumTipo.error)
                                            {
                                                simbolos[1] = new Simbolo(expresiones[0].tipo, expresiones[0].valor);
                                                variableDim3 = (Simbolo[,,])simbolos[0].valor;
                                                for (int i = 0; i < variableDim3.GetLength(0); i++)
                                                {
                                                    for (int j = 0; j < variableDim3.GetLength(1); j++)
                                                    {
                                                        for (int k = 0; k < variableDim3.GetLength(2); k++)
                                                        {
                                                            variableDim3[i, j, k] = simbolos[1];
                                                        }
                                                    }
                                                }
                                                ent.modificar(variable.getValor().ToString(), new Simbolo(Simbolo.EnumTipo.dim3, variableDim3));
                                            }
                                            else
                                            {
                                                reportarError = new Error(archivo, "0", "0", "Semantico", "" + expresiones[0].valor);
                                                Errores.Add(reportarError);
                                            }
                                        }
                                        break;
                                        #endregion
                                }
                                break;
                            #endregion

                            case Simbolo.EnumTipo.error:
                                reportarError = new Error(archivo, "0", "0", "Semantico", "" + simbolos[0].valor);
                                Errores.Add(reportarError);
                                //REPORTAR ERROR: dato2.valor.ToString()
                                break;

                            #region Lineal
                            default:
                                datos[0] = resolverExpresion(nuevo, ent, archivo);
                                simbolos[0] = new Simbolo(datos[0].tipo, datos[0].valor);
                                if (datos[0].tipo == Simbolo.EnumTipo.dim1 || datos[0].tipo == Simbolo.EnumTipo.dim2 || datos[0].tipo == Simbolo.EnumTipo.dim3)
                                {
                                    reportarError = new Error(archivo, "0", "0", "Semantico", "Se esperaba dato, arreglo encontrado");
                                    Errores.Add(reportarError);
                                }
                                else
                                {
                                    if (!ent.modificar(variable.getValor().ToString(), simbolos[0]))
                                    {
                                        reportarError = new Error(archivo, "0", "0", "Semantico", "La variable " + variable.getValor().ToString() + " ya existe");
                                        Errores.Add(reportarError);
                                    }
                                }
                                break;
                                #endregion
                        }
                    }
                    else
                    {
                        reportarError = new Error(archivo, "0", "0", "Semantico", "La variable " + variable.getValor().ToString() + " ya existe");
                        Errores.Add(reportarError);
                    }
                    break;
                    #endregion
            }
        }

        private void ejecutarDeclaracion(NodoSintactico root, Entorno ent, int tipo, String archivo)
        {
            Expresion resultado;
            Expresion index;
            Expresion index1;
            Expresion index2;
            Simbolo nuevo;
            ArrayList listaTemporal;
            NodoSintactico nodoTemporal;
            int contador = 0;
            int contador1 = 0;
            int contador2 = 0;
            bool error = false;
            Simbolo[] objetoDim1;
            Simbolo[,] objetoDim2;
            Simbolo[,,] objetoDim3;
            Error reportarError = null;
            switch (tipo)
            {
                #region DIM 0
                case 0:
                    //Declaracion, inicializacion
                    if (root.getHijos().Count == 2)
                    {
                        listaTemporal = root.getHijos();
                        nodoTemporal = (NodoSintactico)listaTemporal[1];
                        resultado = resolverExpresion(nodoTemporal, ent, archivo);
                        if (resultado.tipo != Simbolo.EnumTipo.error)
                        {
                            if (resultado.tipo == Simbolo.EnumTipo.dim1 || resultado.tipo == Simbolo.EnumTipo.dim2 || resultado.tipo == Simbolo.EnumTipo.dim3)
                            {
                                reportarError = new Error(archivo, "0", "0", "Semantico", "Se esperaba dato, arreglo encontrado");
                                Errores.Add(reportarError);
                            }
                            else
                            {
                                nuevo = new Simbolo(resultado.tipo, resultado.valor);
                                nodoTemporal = (NodoSintactico)listaTemporal[0];
                                if (!ent.insertar(nodoTemporal.getValor().ToString(), nuevo, 0, 0))
                                {
                                    reportarError = new Error(archivo, "0", "0", "Semantico", "La variable " + nodoTemporal.getValor().ToString() + " ya existe");
                                    Errores.Add(reportarError);
                                }
                            }
                        }
                        else
                        {
                            reportarError = new Error(archivo, "0", "0", "Semantico", resultado.valor.ToString());
                            Errores.Add(reportarError);
                        }
                    }
                    //Declaracion
                    else
                    {
                        listaTemporal = root.getHijos();
                        nuevo = new Simbolo(Simbolo.EnumTipo.nulo, 0);
                        nodoTemporal = (NodoSintactico)listaTemporal[0];
                        if (!ent.insertar(nodoTemporal.getValor().ToString(), nuevo, 0, 0))
                        {
                            reportarError = new Error(archivo, "0", "0", "Semantico", "La variable " + nodoTemporal.getValor().ToString() + " ya existe");
                            Errores.Add(reportarError);
                        }
                    }
                    break;
                #endregion

                #region DIM 1
                case 1:
                    //Declaracion, inicializacion
                    if (root.getHijos().Count == 3)
                    {
                        listaTemporal = root.getHijos();
                        nodoTemporal = (NodoSintactico)listaTemporal[2];
                        if (nodoTemporal.getNombre().Equals("CONJUNTO_D1"))
                        {
                            //Inicializacion del tipo arreglo = {0,1,2};
                            contador = 0;
                            nodoTemporal = (NodoSintactico)listaTemporal[0];
                            if (ent.buscar(nodoTemporal.getValor().ToString(), 0, 0) == null)
                            {
                                nodoTemporal = (NodoSintactico)listaTemporal[1];
                                index = resolverExpresion(nodoTemporal, ent, archivo);
                                nodoTemporal = (NodoSintactico)listaTemporal[2];
                                foreach (NodoSintactico valor in nodoTemporal.getHijos())
                                {
                                    contador++;
                                }
                                if (index.tipo != Simbolo.EnumTipo.error)
                                {
                                    if (index.tipo == Simbolo.EnumTipo.entero)
                                    {
                                        if (int.Parse(index.valor.ToString()) == contador)
                                        {
                                            //Creamos el arreglo
                                            objetoDim1 = new Simbolo[int.Parse(index.valor.ToString())];
                                            contador = 0;
                                            foreach (NodoSintactico valor in nodoTemporal.getHijos())
                                            {
                                                resultado = resolverExpresion(valor, ent, archivo);
                                                //Verificamos si hubo un error al momento de generar el dato para el arreglo. Ejemplo: true + 1
                                                if (resultado.tipo != Simbolo.EnumTipo.error)
                                                {
                                                    //Generamos un nuevo Simbolo con el tipo de dato que obtuvimos y el valor del dato
                                                    nuevo = new Simbolo(resultado.tipo, resultado.valor);
                                                    //Obtenemos el nombre que tendra la variable
                                                    nodoTemporal = (NodoSintactico)listaTemporal[0];
                                                    //Almacenamos el dato en el arreglo
                                                    objetoDim1[contador] = nuevo;
                                                }
                                                else
                                                {
                                                    //Se inicia la el espacio de memoria con valor nulo
                                                    nuevo = new Simbolo(Simbolo.EnumTipo.nulo, 0);
                                                    //Almacenamos el dato en el arreglo
                                                    objetoDim1[contador] = nuevo;
                                                }
                                                contador++;
                                            }
                                            nuevo = new Simbolo(Simbolo.EnumTipo.dim1, objetoDim1);
                                            ent.insertar(nodoTemporal.getValor().ToString(), nuevo, 0, 0);
                                        }
                                        else
                                        {
                                            reportarError = new Error(archivo, "0", "0", "Semantico", "Se esperaba " + index.valor + " dato, se encontrado " + contador + " datos");
                                            Errores.Add(reportarError);
                                        }
                                    }
                                    else
                                    {
                                        reportarError = new Error(archivo, "0", "0", "Semantico", "Se esperaba tipo entero, tipo " + index.tipo + " encontrado");
                                        Errores.Add(reportarError);
                                    }
                                }
                                else
                                {
                                    reportarError = new Error(archivo, "0", "0", "Semantico", "" + index.valor);
                                    Errores.Add(reportarError);
                                }
                            }
                            else
                            {
                                reportarError = new Error(archivo, "0", "0", "Semantico", "La variable " + nodoTemporal.getValor().ToString() + " ya existe");
                                Errores.Add(reportarError);
                            }
                        }
                        else if (nodoTemporal.getNombre().Equals("DIM1"))
                        {
                            nodoTemporal = (NodoSintactico)listaTemporal[1];
                            index = resolverExpresion(nodoTemporal, ent, archivo);
                            if (index.tipo != Simbolo.EnumTipo.error)
                            {
                                if (index.tipo == Simbolo.EnumTipo.entero)
                                {
                                    nodoTemporal = (NodoSintactico)listaTemporal[2];
                                    listaTemporal = nodoTemporal.getHijos();
                                    nodoTemporal = (NodoSintactico)listaTemporal[0];
                                    nuevo = ent.buscar(nodoTemporal.getValor().ToString(), 0, 0);
                                    if (nuevo != null)
                                    {
                                        nodoTemporal = (NodoSintactico)listaTemporal[1];
                                        index1 = resolverExpresion(nodoTemporal, ent, archivo);
                                        if (index1.tipo != Simbolo.EnumTipo.error)
                                        {
                                            if (index1.tipo == Simbolo.EnumTipo.entero)
                                            {
                                                try
                                                {
                                                    if (nuevo.tipo == Simbolo.EnumTipo.dim2)
                                                    {
                                                        objetoDim2 = (Simbolo[,])nuevo.valor;
                                                        if (int.Parse(index.valor.ToString()) == objetoDim2.Length / objetoDim2.Rank)
                                                        {
                                                            objetoDim1 = new Simbolo[int.Parse(index.valor.ToString())];
                                                            for (int i = 0; i < int.Parse(index.valor.ToString()); i++)
                                                            {
                                                                objetoDim1[i] = objetoDim2[int.Parse(index1.valor.ToString()), i];
                                                            }
                                                            listaTemporal = root.getHijos();
                                                            nodoTemporal = (NodoSintactico)listaTemporal[0];
                                                            nuevo = new Simbolo(Simbolo.EnumTipo.dim1, objetoDim1);
                                                            if (!ent.insertar(nodoTemporal.getValor().ToString(), nuevo, 0, 0))
                                                            {
                                                                reportarError = new Error(archivo, "0", "0", "Semantico", "La variable " + nodoTemporal.getValor().ToString() + " ya existe");
                                                                Errores.Add(reportarError);
                                                            }
                                                        }
                                                        else
                                                        {
                                                            reportarError = new Error(archivo, "0", "0", "Semantico", "Los tamaños no coinciden");
                                                            Errores.Add(reportarError);
                                                        }
                                                    }
                                                }
                                                catch (IndexOutOfRangeException)
                                                {
                                                    reportarError = new Error(archivo, "0", "0", "Semantico", "Indice fuera del rango");
                                                    Errores.Add(reportarError);
                                                }
                                            }
                                            else
                                            {
                                                reportarError = new Error(archivo, "0", "0", "Semantico", "Se esperaba tipo entero, tipo" + index1.tipo + " encontrado");
                                                Errores.Add(reportarError);
                                            }
                                        }
                                        else
                                        {
                                            reportarError = new Error(archivo, "0", "0", "Semantico", "" + index1.valor);
                                            Errores.Add(reportarError);
                                        }
                                    }
                                    else
                                    {
                                        reportarError = new Error(archivo, "0", "0", "Semantico", "La variable " + nodoTemporal.getNombre() + " ya existe");
                                        Errores.Add(reportarError);
                                    }
                                }
                                else
                                {
                                    reportarError = new Error(archivo, "0", "0", "Semantico", "Se esperaba tipo entero, tipo" + index.tipo + " encontrado");
                                    Errores.Add(reportarError);
                                }
                            }
                            else
                            {
                                reportarError = new Error(archivo, "0", "0", "Semantico", "" + index.valor);
                                Errores.Add(reportarError);
                            }
                        }
                        else if (nodoTemporal.getNombre().Equals("DIM2"))
                        {
                            nodoTemporal = (NodoSintactico)listaTemporal[1];
                            index = resolverExpresion(nodoTemporal, ent, archivo);
                            if (index.tipo != Simbolo.EnumTipo.error)
                            {
                                if (index.tipo == Simbolo.EnumTipo.entero)
                                {
                                    nodoTemporal = (NodoSintactico)listaTemporal[2];
                                    listaTemporal = nodoTemporal.getHijos();
                                    nodoTemporal = (NodoSintactico)listaTemporal[0];
                                    nuevo = ent.buscar(nodoTemporal.getValor().ToString(), 0, 0);
                                    if (nuevo != null)
                                    {
                                        nodoTemporal = (NodoSintactico)listaTemporal[1];
                                        index1 = resolverExpresion(nodoTemporal, ent, archivo);
                                        nodoTemporal = (NodoSintactico)listaTemporal[2];
                                        index2 = resolverExpresion(nodoTemporal, ent, archivo);
                                        if (index1.tipo != Simbolo.EnumTipo.error)
                                        {
                                            if (index1.tipo == Simbolo.EnumTipo.entero)
                                            {
                                                try
                                                {
                                                    if (nuevo.tipo == Simbolo.EnumTipo.dim3)
                                                    {
                                                        if (index2.tipo != Simbolo.EnumTipo.error)
                                                        {
                                                            if (index2.tipo == Simbolo.EnumTipo.entero)
                                                            {
                                                                objetoDim3 = (Simbolo[,,])nuevo.valor;
                                                                if (int.Parse(index.valor.ToString()) == objetoDim3.Length / objetoDim3.Rank)
                                                                {
                                                                    objetoDim1 = new Simbolo[int.Parse(index.valor.ToString())];
                                                                    for (int i = 0; i < int.Parse(index.valor.ToString()); i++)
                                                                    {
                                                                        objetoDim1[i] = objetoDim3[int.Parse(index1.valor.ToString()), int.Parse(index2.valor.ToString()), i];
                                                                    }
                                                                    listaTemporal = root.getHijos();
                                                                    nodoTemporal = (NodoSintactico)listaTemporal[0];
                                                                    nuevo = new Simbolo(Simbolo.EnumTipo.dim1, objetoDim1);
                                                                    if (!ent.insertar(nodoTemporal.getValor().ToString(), nuevo, 0, 0))
                                                                    {
                                                                        reportarError = new Error(archivo, "0", "0", "Semantico", "La variable " + nodoTemporal.getValor().ToString() + " ya existe");
                                                                        Errores.Add(reportarError);
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    reportarError = new Error(archivo, "0", "0", "Semantico", "Los tamaños no coinciden");
                                                                    Errores.Add(reportarError);
                                                                }
                                                            }
                                                            else
                                                            {
                                                                reportarError = new Error(archivo, "0", "0", "Semantico", "Se esperaba tipo entero, tipo" + index2.tipo + " encontrado");
                                                                Errores.Add(reportarError);
                                                            }
                                                        }
                                                        else
                                                        {
                                                            reportarError = new Error(archivo, "0", "0", "Semantico", "" + index2.valor);
                                                            Errores.Add(reportarError);
                                                        }
                                                    }
                                                }
                                                catch (IndexOutOfRangeException)
                                                {
                                                    reportarError = new Error(archivo, "0", "0", "Semantico", "Indice fuera del rango");
                                                    Errores.Add(reportarError);
                                                }
                                            }
                                            else
                                            {
                                                reportarError = new Error(archivo, "0", "0", "Semantico", "Se esperaba tipo entero, tipo" + index1.tipo + " encontrado");
                                                Errores.Add(reportarError);
                                            }
                                        }
                                        else
                                        {
                                            reportarError = new Error(archivo, "0", "0", "Semantico", "" + index1.valor);
                                            Errores.Add(reportarError);
                                        }
                                    }
                                    else
                                    {
                                        reportarError = new Error(archivo, "0", "0", "Semantico", "La variable " + nodoTemporal.getNombre() + " ya existe");
                                        Errores.Add(reportarError);
                                    }
                                }
                                else
                                {
                                    reportarError = new Error(archivo, "0", "0", "Semantico", "Se esperaba tipo entero, tipo" + index.tipo + " encontrado");
                                    Errores.Add(reportarError);
                                }
                            }
                            else
                            {
                                reportarError = new Error(archivo, "0", "0", "Semantico", "" + index.valor);
                                Errores.Add(reportarError);
                            }
                        }
                        else
                        {
                            listaTemporal = root.getHijos();

                            //Tomamos el tamaño del arreglo y lo analizamos
                            nodoTemporal = (NodoSintactico)listaTemporal[1];
                            index = resolverExpresion(nodoTemporal, ent, archivo);

                            //Verificamos si ocurrio un error en las operaciones de analisis del tamaño
                            if (index.tipo != Simbolo.EnumTipo.error)
                            {
                                //Verificamos si el dato resultado es de tipo entero
                                if (index.tipo == Simbolo.EnumTipo.entero)
                                {
                                    //Tomamos el dato inicial que tendran todos los espacios de memoria generados para el arreglo
                                    nodoTemporal = (NodoSintactico)listaTemporal[2];
                                    resultado = resolverExpresion(nodoTemporal, ent, archivo);
                                    //Verificamos si hubo un error al momento de generar el dato para el arreglo. Ejemplo: true + 1
                                    if (resultado.tipo != Simbolo.EnumTipo.error)
                                    {
                                        //Construimos el arreglo a almacenar
                                        objetoDim1 = new Simbolo[int.Parse(index.valor.ToString())];
                                        //Obtenemos el nombre que tendra la variable
                                        nodoTemporal = (NodoSintactico)listaTemporal[0];
                                        //Se crea el elemento que contendra el arreglo
                                        nuevo = new Simbolo(resultado.tipo, resultado.valor);
                                        if (resultado.tipo == Simbolo.EnumTipo.dim1)
                                        {
                                            //Es una variable DIM1
                                            objetoDim1 = (Simbolo[])nuevo.valor;
                                        }
                                        //Es una variable DIM0
                                        else
                                        {
                                            //Llenamos el arreglo
                                            for (int i = 0; i < objetoDim1.Length; i++)
                                            {
                                                objetoDim1[i] = nuevo;
                                            }
                                        }
                                        //Generamos el simbolo
                                        nuevo = new Simbolo(Simbolo.EnumTipo.dim1, objetoDim1);
                                        if (!ent.insertar(nodoTemporal.getValor().ToString(), nuevo, 0, 0))
                                        {
                                            reportarError = new Error(archivo, "0", "0", "Semantico", "La variable " + nodoTemporal.getValor().ToString() + " ya existe");
                                            Errores.Add(reportarError);
                                        }
                                    }
                                    else
                                    {
                                        reportarError = new Error(archivo, "0", "0", "Semantico", resultado.valor.ToString());
                                        Errores.Add(reportarError);
                                    }
                                }
                                else
                                {
                                    reportarError = new Error(archivo, "0", "0", "Semantico", "Se esperaba tipo entero, tipo " + index.tipo + " encontrado");
                                    Errores.Add(reportarError);
                                }
                            }
                            else
                            {
                                reportarError = new Error(archivo, "0", "0", "Semantico", "" + index.valor);
                                Errores.Add(reportarError);
                            }
                        }
                    }
                    //Declaracion
                    else if (root.getHijos().Count == 2)
                    {
                        listaTemporal = root.getHijos();
                        //Tomamos el tamaño del arreglo y lo analizamos
                        nodoTemporal = (NodoSintactico)listaTemporal[1];
                        index = resolverExpresion(nodoTemporal, ent, archivo);

                        //Verificamos si ocurrio un error en las operaciones de analisis del tamaño
                        if (index.tipo != Simbolo.EnumTipo.error)
                        {
                            //Verificamos si el dato resultado es de tipo entero
                            if (index.tipo == Simbolo.EnumTipo.entero)
                            {
                                //Obtenemos el nombre que tendra la variable
                                nodoTemporal = (NodoSintactico)listaTemporal[0];
                                /*
                                 * Se inserta la variable, si el resultado es:
                                 * falso: la variable ya existe, por lo tanto no se procede a crear todos los espacios de memoria
                                 * verdadero: la variable fue insertada, por lo tanto se procede a crear los espacios de memoria
                                 * El valor de la variable sera la cantidad de espacios de memoria que posee por ejemplo:
                                 *      var arreglo[4] = 2;
                                 * Esto quiere decir que:
                                 *      arreglo[0] -> 2
                                 *      arreglo[1] -> 2
                                 *      arreglo[2] -> 2
                                 *      arreglo[3] -> 2
                                 *      arreglo -> 4 (Me indica cuantas posiciones tiene reservadas)
                                 *      
                                 */

                                //Creamos el arreglo
                                objetoDim1 = new Simbolo[int.Parse(index.valor.ToString())];
                                nuevo = new Simbolo(Simbolo.EnumTipo.nulo, 0);
                                for (int i = 0; i < objetoDim1.Length; i++)
                                {
                                    objetoDim1[i] = nuevo;
                                }

                                nuevo = new Simbolo(Simbolo.EnumTipo.dim1, objetoDim1);
                                if (!ent.insertar(nodoTemporal.getValor().ToString(), nuevo, 0, 0))
                                {
                                    reportarError = new Error(archivo, "0", "0", "Semantico", "La variable " + nodoTemporal.getValor().ToString() + " ya existe");
                                    Errores.Add(reportarError);
                                }
                            }
                            else
                            {
                                reportarError = new Error(archivo, "0", "0", "Semantico", "Se esperaba tipo entero, tipo " + index.tipo + " encontrado");
                                Errores.Add(reportarError);
                            }
                        }
                        else
                        {
                            reportarError = new Error(archivo, "0", "0", "Semantico", "" + index.valor);
                            Errores.Add(reportarError);
                        }
                    }
                    break;
                #endregion

                #region DIM 2
                case 2:
                    //Declaracion, inicializacion
                    if (root.getHijos().Count == 4)
                    {
                        listaTemporal = root.getHijos();

                        //Tomamos el tamaño del arreglo y lo analizamos
                        nodoTemporal = (NodoSintactico)listaTemporal[1];
                        index = resolverExpresion(nodoTemporal, ent, archivo);
                        nodoTemporal = (NodoSintactico)listaTemporal[2];
                        index1 = resolverExpresion(nodoTemporal, ent, archivo);

                        //Verificamos si ocurrio un error en las operaciones de analisis del tamaño
                        if (index.tipo != Simbolo.EnumTipo.error)
                        {
                            //Verificamos si el dato resultado es de tipo entero
                            if (index.tipo == Simbolo.EnumTipo.entero)
                            {
                                //Verificamos si ocurrio un error en las operaciones de analisis del tamaño
                                if (index1.tipo != Simbolo.EnumTipo.error)
                                {
                                    //Verificamos si el dato resultado es de tipo entero
                                    if (index1.tipo == Simbolo.EnumTipo.entero)
                                    {
                                        nodoTemporal = (NodoSintactico)listaTemporal[3];
                                        //Creamos el arreglo
                                        objetoDim2 = new Simbolo[int.Parse(index.valor.ToString()), int.Parse(index1.valor.ToString())];
                                        // Declaracion, inicializacion del tipo: arreglo = {{0,1}, {1,2}}
                                        if (nodoTemporal.getNombre().Equals("CONJUNTO_D2"))
                                        {
                                            contador = 0;
                                            listaTemporal = root.getHijos();
                                            nodoTemporal = (NodoSintactico)listaTemporal[0];
                                            if (ent.buscar(nodoTemporal.getValor().ToString(), 0, 0) == null)
                                            {
                                                nodoTemporal = (NodoSintactico)listaTemporal[3];
                                                foreach (NodoSintactico valor in nodoTemporal.getHijos())
                                                {
                                                    contador1 = 0;
                                                    foreach (NodoSintactico valor2 in valor.getHijos())
                                                    {
                                                        contador1++;
                                                    }
                                                    if (int.Parse(index1.valor.ToString()) != contador1)
                                                    {
                                                        error = true;
                                                        break;
                                                    }
                                                    contador++;
                                                }
                                                if (int.Parse(index.valor.ToString()) != contador)
                                                {
                                                    error = true;
                                                }
                                                if (!error)
                                                {
                                                    //Creamos el arreglo
                                                    objetoDim2 = new Simbolo[int.Parse(index.valor.ToString()), int.Parse(index1.valor.ToString())];
                                                    contador = 0;
                                                    nodoTemporal = (NodoSintactico)listaTemporal[3];
                                                    foreach (NodoSintactico valor in nodoTemporal.getHijos())
                                                    {
                                                        contador1 = 0;
                                                        foreach (NodoSintactico valor2 in valor.getHijos())
                                                        {
                                                            resultado = resolverExpresion(valor2, ent, archivo);
                                                            //Verificamos si hubo un error al momento de generar el dato para el arreglo. Ejemplo: true + 1
                                                            if (resultado.tipo != Simbolo.EnumTipo.error)
                                                            {
                                                                nuevo = new Simbolo(resultado.tipo, resultado.valor);
                                                                nodoTemporal = (NodoSintactico)listaTemporal[0];
                                                                objetoDim2[contador, contador1] = nuevo;
                                                            }
                                                            else
                                                            {
                                                                nuevo = new Simbolo(Simbolo.EnumTipo.nulo, 0);
                                                                objetoDim2[contador, contador1] = nuevo;
                                                            }
                                                            contador1++;
                                                        }
                                                        contador++;
                                                    }
                                                    //Se guarda el la variable con el numero de indices posibles
                                                    nodoTemporal = (NodoSintactico)listaTemporal[0];
                                                    nuevo = new Simbolo(Simbolo.EnumTipo.dim2, objetoDim2);
                                                    ent.insertar(nodoTemporal.getValor().ToString(), nuevo, 0, 0);
                                                }
                                                else
                                                {
                                                    reportarError = new Error(archivo, "0", "0", "Semantico", "Indice fuera del limite");
                                                    Errores.Add(reportarError);
                                                }
                                            }
                                            else
                                            {
                                                reportarError = new Error(archivo, "0", "0", "Semantico", "La variable " + nodoTemporal.getValor().ToString() + " ya existe");
                                                Errores.Add(reportarError);
                                            }
                                        }
                                        else if (nodoTemporal.getNombre().Equals("DIM1"))
                                        {
                                            nodoTemporal = (NodoSintactico)listaTemporal[3];
                                            listaTemporal = nodoTemporal.getHijos();
                                            nodoTemporal = (NodoSintactico)listaTemporal[0];
                                            nuevo = ent.buscar(nodoTemporal.getValor().ToString(), 0, 0);
                                            if (nuevo != null)
                                            {
                                                nodoTemporal = (NodoSintactico)listaTemporal[1];
                                                index2 = resolverExpresion(nodoTemporal, ent, archivo);
                                                if (index2.tipo != Simbolo.EnumTipo.error)
                                                {
                                                    if (index2.tipo == Simbolo.EnumTipo.entero)
                                                    {
                                                        try
                                                        {
                                                            if (nuevo.tipo == Simbolo.EnumTipo.dim3)
                                                            {
                                                                if (index2.tipo != Simbolo.EnumTipo.error)
                                                                {
                                                                    if (index2.tipo == Simbolo.EnumTipo.entero)
                                                                    {
                                                                        objetoDim3 = (Simbolo[,,])nuevo.valor;
                                                                        if (int.Parse(index1.valor.ToString()) == objetoDim3.Length / objetoDim3.Rank)
                                                                        {
                                                                            objetoDim2 = new Simbolo[int.Parse(index.valor.ToString()), int.Parse(index1.valor.ToString())];
                                                                            for (int i = 0; i < int.Parse(index.valor.ToString()); i++)
                                                                            {
                                                                                for (int j = 0; j < int.Parse(index1.valor.ToString()); j++)
                                                                                {
                                                                                    objetoDim2[i, j] = objetoDim3[int.Parse(index2.valor.ToString()), i, j];
                                                                                }
                                                                            }
                                                                            listaTemporal = root.getHijos();
                                                                            nodoTemporal = (NodoSintactico)listaTemporal[0];
                                                                            nuevo = new Simbolo(Simbolo.EnumTipo.dim2, objetoDim2);
                                                                            if (!ent.insertar(nodoTemporal.getValor().ToString(), nuevo, 0, 0))
                                                                            {
                                                                                reportarError = new Error(archivo, "0", "0", "Semantico", "La variable " + nodoTemporal.getValor().ToString() + " ya existe");
                                                                                Errores.Add(reportarError);
                                                                            }
                                                                        }
                                                                        else
                                                                        {
                                                                            reportarError = new Error(archivo, "0", "0", "Semantico", "Los tamaños no coinciden");
                                                                            Errores.Add(reportarError);
                                                                        }
                                                                    }
                                                                    else
                                                                    {
                                                                        reportarError = new Error(archivo, "0", "0", "Semantico", "Se esperaba tipo entero, tipo" + index2.tipo + " encontrado");
                                                                        Errores.Add(reportarError);
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    reportarError = new Error(archivo, "0", "0", "Semantico", "" + index2.valor);
                                                                    Errores.Add(reportarError);
                                                                }
                                                            }
                                                        }
                                                        catch (IndexOutOfRangeException)
                                                        {
                                                            reportarError = new Error(archivo, "0", "0", "Semantico", "Indice fuera del rango");
                                                            Errores.Add(reportarError);
                                                        }
                                                    }
                                                    else
                                                    {
                                                        reportarError = new Error(archivo, "0", "0", "Semantico", "Se esperaba tipo entero, tipo" + index2.tipo + " encontrado");
                                                        Errores.Add(reportarError);
                                                    }
                                                }
                                                else
                                                {
                                                    reportarError = new Error(archivo, "0", "0", "Semantico", "" + index2.valor);
                                                    Errores.Add(reportarError);
                                                }
                                            }
                                            else
                                            {
                                                reportarError = new Error(archivo, "0", "0", "Semantico", "La variable " + nodoTemporal.getNombre() + " ya existe");
                                                Errores.Add(reportarError);
                                            }
                                        }
                                        else
                                        {
                                            //Tomamos el dato inicial que tendran todos los espacios de memoria generados para el arreglo
                                            nodoTemporal = (NodoSintactico)listaTemporal[3];
                                            resultado = resolverExpresion(nodoTemporal, ent, archivo);
                                            //Verificamos si hubo un error al momento de generar el dato para el arreglo. Ejemplo: true + 1
                                            if (resultado.tipo != Simbolo.EnumTipo.error)
                                            {
                                                nuevo = new Simbolo(resultado.tipo, resultado.valor);
                                                if (resultado.tipo == Simbolo.EnumTipo.dim2)
                                                {
                                                    //Es una variable DIM2
                                                    objetoDim2 = (Simbolo[,])nuevo.valor;
                                                }
                                                //Es una variable DIM0
                                                else
                                                {
                                                    //Llenamos el arreglo
                                                    for (int i = 0; i < int.Parse(index.valor.ToString()); i++)
                                                    {
                                                        for (int j = 0; j < int.Parse(index1.valor.ToString()); j++)
                                                        {
                                                            objetoDim2[i, j] = nuevo;
                                                        }
                                                    }
                                                }
                                                nodoTemporal = (NodoSintactico)listaTemporal[0];
                                                nuevo = new Simbolo(Simbolo.EnumTipo.dim2, objetoDim2);
                                                if (!ent.insertar(nodoTemporal.getValor().ToString(), nuevo, 0, 0))
                                                {
                                                    reportarError = new Error(archivo, "0", "0", "Semantico", "La variable " + nodoTemporal.getValor().ToString() + " ya existe");
                                                    Errores.Add(reportarError);
                                                }
                                            }
                                            else
                                            {
                                                reportarError = new Error(archivo, "0", "0", "Semantico", resultado.valor.ToString());
                                                Errores.Add(reportarError);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        reportarError = new Error(archivo, "0", "0", "Semantico", "Se esperaba tipo entero, tipo " + index.tipo + " encontrado");
                                        Errores.Add(reportarError);
                                    }
                                }
                                else
                                {
                                    reportarError = new Error(archivo, "0", "0", "Semantico", "" + index1.valor);
                                    Errores.Add(reportarError);
                                }
                            }
                            else
                            {
                                reportarError = new Error(archivo, "0", "0", "Semantico", "Se esperaba tipo entero, tipo " + index.tipo + " encontrado");
                                Errores.Add(reportarError);
                            }
                        }
                        else
                        {
                            reportarError = new Error(archivo, "0", "0", "Semantico", "" + index.valor);
                            Errores.Add(reportarError);
                        }
                    }
                    //Declaracion
                    else if (root.getHijos().Count == 3)
                    {
                        listaTemporal = root.getHijos();

                        //Tomamos el tamaño del arreglo y lo analizamos
                        nodoTemporal = (NodoSintactico)listaTemporal[1];
                        index = resolverExpresion(nodoTemporal, ent, archivo);
                        nodoTemporal = (NodoSintactico)listaTemporal[2];
                        index1 = resolverExpresion(nodoTemporal, ent, archivo);

                        //Verificamos si ocurrio un error en las operaciones de analisis del tamaño
                        if (index.tipo != Simbolo.EnumTipo.error)
                        {
                            //Verificamos si el dato resultado es de tipo entero
                            if (index.tipo == Simbolo.EnumTipo.entero)
                            {
                                //Verificamos si ocurrio un error en las operaciones de analisis del tamaño
                                if (index1.tipo != Simbolo.EnumTipo.error)
                                {
                                    //Verificamos si el dato resultado es de tipo entero
                                    if (index1.tipo == Simbolo.EnumTipo.entero)
                                    {
                                        nodoTemporal = (NodoSintactico)listaTemporal[0];
                                        //Creamos el arreglo
                                        objetoDim2 = new Simbolo[int.Parse(index.valor.ToString()), int.Parse(index1.valor.ToString())];
                                        nuevo = new Simbolo(Simbolo.EnumTipo.nulo, 0);
                                        for (int i = 0; i < int.Parse(index.valor.ToString()); i++)
                                        {
                                            for (int j = 0; j < int.Parse(index1.valor.ToString()); j++)
                                            {
                                                objetoDim2[i, j] = nuevo;
                                            }
                                        }
                                        nuevo = new Simbolo(Simbolo.EnumTipo.dim2, objetoDim2);
                                        if (!ent.insertar(nodoTemporal.getValor().ToString(), nuevo, 0, 0))
                                        {
                                            reportarError = new Error(archivo, "0", "0", "Semantico", "La variable " + nodoTemporal.getValor().ToString() + " ya existe");
                                            Errores.Add(reportarError);
                                        }
                                    }
                                    else
                                    {
                                        reportarError = new Error(archivo, "0", "0", "Semantico", "Se esperaba tipo entero, tipo " + index.tipo + " encontrado");
                                        Errores.Add(reportarError);
                                    }
                                }
                                else
                                {
                                    reportarError = new Error(archivo, "0", "0", "Semantico", "" + index1.valor);
                                    Errores.Add(reportarError);
                                }
                            }
                            else
                            {
                                reportarError = new Error(archivo, "0", "0", "Semantico", "Se esperaba tipo entero, tipo " + index.tipo + " encontrado");
                                Errores.Add(reportarError);
                            }
                        }
                        else
                        {
                            reportarError = new Error(archivo, "0", "0", "Semantico", "" + index.valor);
                            Errores.Add(reportarError);
                        }
                    }
                    break;
                #endregion

                #region DIM 3
                case 3:
                    //Declaracion, inicializacion
                    if (root.getHijos().Count == 5)
                    {
                        listaTemporal = root.getHijos();

                        //Tomamos el tamaño del arreglo y lo analizamos
                        nodoTemporal = (NodoSintactico)listaTemporal[1];
                        index = resolverExpresion(nodoTemporal, ent, archivo);
                        nodoTemporal = (NodoSintactico)listaTemporal[2];
                        index1 = resolverExpresion(nodoTemporal, ent, archivo);
                        nodoTemporal = (NodoSintactico)listaTemporal[3];
                        index2 = resolverExpresion(nodoTemporal, ent, archivo);

                        //Verificamos si ocurrio un error en las operaciones de analisis del tamaño
                        if (index.tipo != Simbolo.EnumTipo.error)
                        {
                            //Verificamos si el dato resultado es de tipo entero
                            if (index.tipo == Simbolo.EnumTipo.entero)
                            {
                                //Verificamos si ocurrio un error en las operaciones de analisis del tamaño
                                if (index1.tipo != Simbolo.EnumTipo.error)
                                {
                                    //Verificamos si el dato resultado es de tipo entero
                                    if (index1.tipo == Simbolo.EnumTipo.entero)
                                    {
                                        //Verificamos si ocurrio un error en las operaciones de analisis del tamaño
                                        if (index2.tipo != Simbolo.EnumTipo.error)
                                        {
                                            //Verificamos si el dato resultado es de tipo entero
                                            if (index2.tipo == Simbolo.EnumTipo.entero)
                                            {
                                                nodoTemporal = (NodoSintactico)listaTemporal[4];
                                                if (!nodoTemporal.getNombre().Equals("CONJUNTO_D3"))
                                                {
                                                    //Tomamos el dato inicial que tendran todos los espacios de memoria generados para el arreglo
                                                    nodoTemporal = (NodoSintactico)listaTemporal[4];
                                                    resultado = resolverExpresion(nodoTemporal, ent, archivo);
                                                    //Verificamos si hubo un error al momento de generar el dato para el arreglo. Ejemplo: true + 1
                                                    if (resultado.tipo != Simbolo.EnumTipo.error)
                                                    {
                                                        //Creamos el arreglo
                                                        objetoDim3 = new Simbolo[int.Parse(index.valor.ToString()), int.Parse(index1.valor.ToString()), int.Parse(index2.valor.ToString())];
                                                        //Llenamos el arreglo
                                                        nuevo = new Simbolo(resultado.tipo, resultado.valor);
                                                        if (resultado.tipo == Simbolo.EnumTipo.dim3)
                                                        {
                                                            //Es una variable DIM3
                                                            objetoDim3 = (Simbolo[,,])nuevo.valor;
                                                        }
                                                        //Es una variable DIM0
                                                        else
                                                        {
                                                            //Llenamos el arreglo
                                                            for (int i = 0; i < int.Parse(index.valor.ToString()); i++)
                                                            {
                                                                for (int j = 0; j < int.Parse(index1.valor.ToString()); j++)
                                                                {
                                                                    for (int k = 0; k < int.Parse(index2.valor.ToString()); k++)
                                                                    {
                                                                        objetoDim3[i, j, k] = nuevo;
                                                                    }
                                                                }
                                                            }
                                                        }
                                                        //Obtenemos el nombre que tendra la variable
                                                        nodoTemporal = (NodoSintactico)listaTemporal[0];
                                                        nuevo = new Simbolo(Simbolo.EnumTipo.dim3, objetoDim3);
                                                        if (!ent.insertar(nodoTemporal.getValor().ToString(), nuevo, 0, 0))
                                                        {
                                                            reportarError = new Error(archivo, "0", "0", "Semantico", "La variable " + nodoTemporal.getValor().ToString() + " ya existe");
                                                            Errores.Add(reportarError);
                                                        }
                                                    }
                                                    else
                                                    {
                                                        reportarError = new Error(archivo, "0", "0", "Semantico", resultado.valor.ToString());
                                                        Errores.Add(reportarError);
                                                    }
                                                }
                                                // Declaracion, inicializacion del tipo: arreglo = {{0,1}, {1,2}}
                                                else
                                                {
                                                    contador = 0;
                                                    listaTemporal = root.getHijos();
                                                    nodoTemporal = (NodoSintactico)listaTemporal[0];
                                                    if (ent.buscar(nodoTemporal.getValor().ToString(), 0, 0) == null)
                                                    {
                                                        nodoTemporal = (NodoSintactico)listaTemporal[4];
                                                        foreach (NodoSintactico valor in nodoTemporal.getHijos())
                                                        {
                                                            contador1 = 0;
                                                            foreach (NodoSintactico valor2 in valor.getHijos())
                                                            {
                                                                contador2 = 0;
                                                                foreach (NodoSintactico valor3 in valor2.getHijos())
                                                                {
                                                                    contador2++;
                                                                }
                                                                if (int.Parse(index2.valor.ToString()) != contador2)
                                                                {
                                                                    error = true;
                                                                    break;
                                                                }
                                                                contador1++;
                                                            }
                                                            if (error)
                                                            {
                                                                break;
                                                            }
                                                            if (int.Parse(index1.valor.ToString()) != contador1)
                                                            {
                                                                error = true;
                                                                break;
                                                            }
                                                            contador++;
                                                        }
                                                        if (int.Parse(index.valor.ToString()) != contador)
                                                        {
                                                            error = true;
                                                        }
                                                        if (!error)
                                                        {
                                                            objetoDim3 = new Simbolo[int.Parse(index.valor.ToString()), int.Parse(index1.valor.ToString()), int.Parse(index2.valor.ToString())];
                                                            contador = 0;
                                                            nodoTemporal = (NodoSintactico)listaTemporal[4];
                                                            foreach (NodoSintactico valor in nodoTemporal.getHijos())
                                                            {
                                                                contador1 = 0;
                                                                foreach (NodoSintactico valor2 in valor.getHijos())
                                                                {
                                                                    contador2 = 0;
                                                                    foreach (NodoSintactico valor3 in valor2.getHijos())
                                                                    {
                                                                        resultado = resolverExpresion(valor3, ent, archivo);
                                                                        //Verificamos si hubo un error al momento de generar el dato para el arreglo. Ejemplo: true + 1
                                                                        if (resultado.tipo != Simbolo.EnumTipo.error)
                                                                        {
                                                                            nuevo = new Simbolo(resultado.tipo, resultado.valor);
                                                                            objetoDim3[contador, contador1, contador2] = nuevo;
                                                                        }
                                                                        else
                                                                        {
                                                                            nuevo = new Simbolo(Simbolo.EnumTipo.nulo, 0);
                                                                            objetoDim3[contador, contador1, contador2] = nuevo;
                                                                        }
                                                                        contador2++;
                                                                    }
                                                                    contador1++;
                                                                }
                                                                contador++;
                                                            }
                                                            //Se guarda el la variable con el numero de indices posibles
                                                            nodoTemporal = (NodoSintactico)listaTemporal[0];
                                                            nuevo = new Simbolo(Simbolo.EnumTipo.dim3, objetoDim3);
                                                            ent.insertar(nodoTemporal.getValor().ToString(), nuevo, 0, 0);
                                                        }
                                                        else
                                                        {
                                                            reportarError = new Error(archivo, "0", "0", "Semantico", "Indice fuera del limite");
                                                            Errores.Add(reportarError);
                                                        }
                                                    }
                                                    else
                                                    {
                                                        reportarError = new Error(archivo, "0", "0", "Semantico", "La variable " + nodoTemporal.getValor().ToString() + " ya existe");
                                                        Errores.Add(reportarError);
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                reportarError = new Error(archivo, "0", "0", "Semantico", "Se esperaba tipo entero, tipo " + index2.tipo + " encontrado");
                                                Errores.Add(reportarError);
                                            }
                                        }
                                        else
                                        {
                                            reportarError = new Error(archivo, "0", "0", "Semantico", "" + index2.valor);
                                            Errores.Add(reportarError);
                                        }
                                    }
                                    else
                                    {
                                        reportarError = new Error(archivo, "0", "0", "Semantico", "Se esperaba tipo entero, tipo " + index1.tipo + " encontrado");
                                        Errores.Add(reportarError);
                                    }
                                }
                                else
                                {
                                    reportarError = new Error(archivo, "0", "0", "Semantico", "" + index1.valor);
                                    Errores.Add(reportarError);
                                }
                            }
                            else
                            {
                                reportarError = new Error(archivo, "0", "0", "Semantico", "Se esperaba tipo entero, tipo " + index.tipo + " encontrado");
                                Errores.Add(reportarError);
                            }
                        }
                        else
                        {
                            reportarError = new Error(archivo, "0", "0", "Semantico", "" + index.valor);
                            Errores.Add(reportarError);
                        }
                    }
                    //Declaracion
                    else if (root.getHijos().Count == 4)
                    {
                        listaTemporal = root.getHijos();

                        //Tomamos el tamaño del arreglo y lo analizamos
                        nodoTemporal = (NodoSintactico)listaTemporal[1];
                        index = resolverExpresion(nodoTemporal, ent, archivo);
                        nodoTemporal = (NodoSintactico)listaTemporal[2];
                        index1 = resolverExpresion(nodoTemporal, ent, archivo);
                        nodoTemporal = (NodoSintactico)listaTemporal[3];
                        index2 = resolverExpresion(nodoTemporal, ent, archivo);

                        //Verificamos si ocurrio un error en las operaciones de analisis del tamaño
                        if (index.tipo != Simbolo.EnumTipo.error)
                        {
                            //Verificamos si el dato resultado es de tipo entero
                            if (index.tipo == Simbolo.EnumTipo.entero)
                            {
                                //Verificamos si ocurrio un error en las operaciones de analisis del tamaño
                                if (index1.tipo != Simbolo.EnumTipo.error)
                                {
                                    //Verificamos si el dato resultado es de tipo entero
                                    if (index1.tipo == Simbolo.EnumTipo.entero)
                                    {
                                        //Verificamos si ocurrio un error en las operaciones de analisis del tamaño
                                        if (index2.tipo != Simbolo.EnumTipo.error)
                                        {
                                            //Verificamos si el dato resultado es de tipo entero
                                            if (index2.tipo == Simbolo.EnumTipo.entero)
                                            {
                                                //Obtenemos el nombre que tendra la variable
                                                nodoTemporal = (NodoSintactico)listaTemporal[0];
                                                objetoDim3 = new Simbolo[int.Parse(index.valor.ToString()), int.Parse(index1.valor.ToString()), int.Parse(index2.valor.ToString())];
                                                nuevo = new Simbolo(Simbolo.EnumTipo.nulo, 0);
                                                //Llenamos el arreglo
                                                for (int i = 0; i < int.Parse(index.valor.ToString()); i++)
                                                {
                                                    for (int j = 0; j < int.Parse(index1.valor.ToString()); j++)
                                                    {
                                                        for (int k = 0; k < int.Parse(index2.valor.ToString()); k++)
                                                        {
                                                            objetoDim3[i, j, k] = nuevo;
                                                        }
                                                    }
                                                }
                                                nuevo = new Simbolo(Simbolo.EnumTipo.dim3, objetoDim3);
                                                if (!ent.insertar(nodoTemporal.getValor().ToString(), nuevo, 0, 0))
                                                {
                                                    reportarError = new Error(archivo, "0", "0", "Semantico", "La variable " + nodoTemporal.getValor().ToString() + " ya existe");
                                                    Errores.Add(reportarError);
                                                }
                                            }
                                            else
                                            {
                                                reportarError = new Error(archivo, "0", "0", "Semantico", "Se esperaba tipo entero, tipo " + index2.tipo + " encontrado");
                                                Errores.Add(reportarError);
                                            }
                                        }
                                        else
                                        {
                                            reportarError = new Error(archivo, "0", "0", "Semantico", "" + index2.valor);
                                            Errores.Add(reportarError);
                                        }
                                    }
                                    else
                                    {
                                        reportarError = new Error(archivo, "0", "0", "Semantico", "Se esperaba tipo entero, tipo " + index1.tipo + " encontrado");
                                        Errores.Add(reportarError);
                                    }
                                }
                                else
                                {
                                    reportarError = new Error(archivo, "0", "0", "Semantico", "" + index1.valor);
                                    Errores.Add(reportarError);
                                }
                            }
                            else
                            {
                                reportarError = new Error(archivo, "0", "0", "Semantico", "Se esperaba tipo entero, tipo " + index.tipo + " encontrado");
                                Errores.Add(reportarError);
                            }
                        }
                        else
                        {
                            reportarError = new Error(archivo, "0", "0", "Semantico", "" + index.valor);
                            Errores.Add(reportarError);
                        }
                    }
                    break;
                    #endregion
            }
        }

        public Expresion resolverExpresion(NodoSintactico root, Entorno ent, String archivo)
        {
            ArrayList listaHijos = root.getHijos();
            switch (root.getNombre())
            {
                case "ENTERO":
                    return new Expresion(Simbolo.EnumTipo.entero, root.getValor());
                case "DOBLE":
                    return new Expresion(Simbolo.EnumTipo.doble, root.getValor());
                case "CADENA":
                    String cadena = root.getValor().ToString();
                    if (cadena.Contains("\\n"))
                    {
                        String[] stringSeparators = new string[] { "\\n" };
                        String[] texto = cadena.Split(stringSeparators, StringSplitOptions.None);
                        String nuevaCadena = "";
                        for (int i = 0; i < texto.Length; i++)
                        {
                            if (i == 0)
                            {
                                nuevaCadena += texto[i];
                            }
                            else
                            {
                                nuevaCadena += "\n" + texto[i];
                            }
                        }
                        return new Expresion(Simbolo.EnumTipo.cadena, nuevaCadena);
                    }
                    return new Expresion(Simbolo.EnumTipo.cadena, root.getValor());
                case "CARACTER":
                    return new Expresion(Simbolo.EnumTipo.caracter, root.getValor().ToString().Replace("'", ""));
                case "BOLEANO":
                    return new Expresion(Simbolo.EnumTipo.boleano, root.getValor());
                case "NULO":
                    return new Expresion(Simbolo.EnumTipo.nulo, 0);
                case "EOTRO":
                    listaHijos = root.getHijos();
                    if (listaHijos.Count == 1)
                    {
                        retorno = null;
                        return resolverEotro(root, ent, archivo, "");
                    }
                    else if (listaHijos.Count == 2)
                    {
                        if (((NodoSintactico)listaHijos[0]).getValor().ToString().Equals("new"))
                        {
                            if (Clases.ContainsKey(((NodoSintactico)listaHijos[1]).getValor().ToString()))
                            {
                                Clases.TryGetValue(((NodoSintactico)listaHijos[1]).getValor().ToString(), out Clase clase);

                                Entorno nuevoEntornoVariables = new Entorno(null);
                                nuevoEntornoVariables.anterior = clase.contenedorDeVariables.anterior;
                                Entorno nuevoEntornoMFC = new Entorno(null);
                                nuevoEntornoMFC.anterior = nuevoEntornoVariables;

                                foreach (String llave in clase.contenedorDeVariables.tabla.Keys)
                                {
                                    clase.contenedorDeVariables.tabla.TryGetValue(llave, out Simbolo simx);
                                    Simbolo s = new Simbolo(simx.tipo, simx.valor);
                                    nuevoEntornoVariables.tabla.Add(llave, s);
                                }

                                foreach (String llave in clase.contenedorDeFuncionesYMetodos.tabla.Keys)
                                {
                                    clase.contenedorDeFuncionesYMetodos.tabla.TryGetValue(llave, out Simbolo simx);
                                    Simbolo s = new Simbolo(simx.tipo, simx.valor);
                                    nuevoEntornoMFC.tabla.Add(llave, s);
                                }

                                Clase nueva = new Clase(nuevoEntornoVariables, nuevoEntornoMFC);
                                return new Expresion(Simbolo.EnumTipo.clase, nueva);
                            }
                            else
                            {
                                return new Expresion(Simbolo.EnumTipo.error, "La clase: " + ((NodoSintactico)listaHijos[1]).getValor().ToString() + " no existe");
                            }
                        }
                        else
                        {
                            if (!((NodoSintactico)listaHijos[1]).getNombre().ToString().Equals("PARAMETROS"))
                            {
                                //Es el metodo o la funcion de una clase
                                retorno = null;
                                NodoSintactico auxi = (NodoSintactico)listaHijos[0];
                                NodoSintactico nuevoRoot = new NodoSintactico(root.getNombre(), root.getNumNodo());
                                for (int i = 1; i < root.getHijos().Count; i++)
                                {
                                    nuevoRoot.addHijo((NodoSintactico)listaHijos[i]);
                                }
                                nuevoRoot.setValor(root.getValor());
                                return resolverEotro(nuevoRoot, ent, archivo, auxi.getValor().ToString());
                            }
                            else
                            {
                                //Es el metodo o la funcion de una clase
                                retorno = null;
                                NodoSintactico auxi = (NodoSintactico)listaHijos[0];
                                return resolverEotro(root, ent, archivo, auxi.getValor().ToString());
                            }
                        }
                    }
                    return new Expresion(Simbolo.EnumTipo.error, "ERROR");
                case "CAMPOCLASE":
                    listaHijos = root.getHijos();
                    Simbolo sim = ent.buscar(((NodoSintactico)listaHijos[0]).getValor().ToString(), 0, 0);
                    if (sim != null)
                    {
                        if (sim.tipo == Simbolo.EnumTipo.clase)
                        {
                            Clase cl = (Clase)sim.valor;
                            Entorno entVariables = cl.contenedorDeVariables;

                            Simbolo var = entVariables.buscar(((NodoSintactico)listaHijos[1]).getValor().ToString(), 0, 0);

                            if (var != null)
                            {
                                return new Expresion(var.tipo, var.valor);
                            }
                            return new Expresion(Simbolo.EnumTipo.error, "La variable: " + ((NodoSintactico)listaHijos[1]).getValor().ToString() + " no existe");
                        }
                    }
                    return new Expresion(Simbolo.EnumTipo.error, "La variable: " + ((NodoSintactico)listaHijos[0]).getValor().ToString() + " no existe");
                case "DIM1":
                case "DIM2":
                case "DIM3":
                case "ID":
                    Expresion uno = buscarVariable(root, ent, 0, archivo);
                    if (uno != null)
                    {
                        if (uno.tipo == Simbolo.EnumTipo.clase)
                        {
                            Clase cl = (Clase)uno.valor;

                            Entorno nuevoEntornoVariables = new Entorno(null);
                            nuevoEntornoVariables.anterior = cl.contenedorDeVariables.anterior;
                            Entorno nuevoEntornoMFC = new Entorno(null);
                            nuevoEntornoMFC.anterior = nuevoEntornoVariables;

                            foreach (String llave in cl.contenedorDeVariables.tabla.Keys)
                            {
                                cl.contenedorDeVariables.tabla.TryGetValue(llave, out Simbolo simx);
                                Simbolo s = new Simbolo(simx.tipo, simx.valor);
                                nuevoEntornoVariables.tabla.Add(llave, s);
                            }

                            foreach (String llave in cl.contenedorDeFuncionesYMetodos.tabla.Keys)
                            {
                                cl.contenedorDeFuncionesYMetodos.tabla.TryGetValue(llave, out Simbolo simx);
                                Simbolo s = new Simbolo(simx.tipo, simx.valor);
                                nuevoEntornoMFC.tabla.Add(llave, s);
                            }

                            Clase nueva = new Clase(nuevoEntornoVariables, nuevoEntornoMFC);

                            return new Expresion(uno.tipo, cl);
                        }
                    }
                    return uno;
                case "E":
                    return resolverExpresion((NodoSintactico)listaHijos[0], ent, archivo);
                case "MULTIPLICACION":
                    return OperarMultiplicacion(resolverExpresion((NodoSintactico)listaHijos[0], ent, archivo), resolverExpresion((NodoSintactico)listaHijos[1], ent, archivo), archivo);
                case "RESTA":
                    return OperarResta(resolverExpresion((NodoSintactico)listaHijos[0], ent, archivo), resolverExpresion((NodoSintactico)listaHijos[1], ent, archivo), archivo);
                case "SUMA":
                    return OperarSuma(resolverExpresion((NodoSintactico)listaHijos[0], ent, archivo), resolverExpresion((NodoSintactico)listaHijos[1], ent, archivo), archivo);
                case "POTENCIA":
                    return OperarPotencia(resolverExpresion((NodoSintactico)listaHijos[0], ent, archivo), resolverExpresion((NodoSintactico)listaHijos[1], ent, archivo), archivo);
                case "DIVISION":
                    return OperarDivision(resolverExpresion((NodoSintactico)listaHijos[0], ent, archivo), resolverExpresion((NodoSintactico)listaHijos[1], ent, archivo), archivo);
                case "NEGATIVO":
                    return OperarNegativo(resolverExpresion((NodoSintactico)listaHijos[0], ent, archivo), archivo);
                case "INCREMENTO":
                    switch (((NodoSintactico)listaHijos[0]).getNombre())
                    {
                        case "ID":
                        case "DIM1":
                        case "DIM2":
                        case "DIM3":
                            buscarVariable((NodoSintactico)listaHijos[0], ent, 1, archivo);
                            return OperarIncremento(resolverExpresion((NodoSintactico)listaHijos[0], ent, archivo), false, archivo);
                    }
                    return OperarIncremento(resolverExpresion((NodoSintactico)listaHijos[0], ent, archivo), true, archivo);
                case "DECREMENTO":
                    switch (((NodoSintactico)listaHijos[0]).getNombre())
                    {
                        case "ID":
                        case "DIM1":
                        case "DIM2":
                        case "DIM3":
                            buscarVariable((NodoSintactico)listaHijos[0], ent, 2, archivo);
                            return OperarDecremento(resolverExpresion((NodoSintactico)listaHijos[0], ent, archivo), false, archivo);
                    }
                    return OperarDecremento(resolverExpresion((NodoSintactico)listaHijos[0], ent, archivo), true, archivo);
                case "AND":
                    return OperarAnd(resolverExpresion((NodoSintactico)listaHijos[0], ent, archivo), resolverExpresion((NodoSintactico)listaHijos[1], ent, archivo), archivo);
                case "OR":
                    return OperarOr(resolverExpresion((NodoSintactico)listaHijos[0], ent, archivo), resolverExpresion((NodoSintactico)listaHijos[1], ent, archivo), archivo);
                case "XOR":
                    return OperarXor(resolverExpresion((NodoSintactico)listaHijos[0], ent, archivo), resolverExpresion((NodoSintactico)listaHijos[1], ent, archivo), archivo);
                case "NOT":
                    return OperarNot(resolverExpresion((NodoSintactico)listaHijos[0], ent, archivo), archivo);
                case "MAYOR":
                    return OperarMayor(resolverExpresion((NodoSintactico)listaHijos[0], ent, archivo), resolverExpresion((NodoSintactico)listaHijos[1], ent, archivo), archivo);
                case "MENOR":
                    return OperarMenor(resolverExpresion((NodoSintactico)listaHijos[0], ent, archivo), resolverExpresion((NodoSintactico)listaHijos[1], ent, archivo), archivo);
                case "MAYORIGUAL":
                    return OperarMayorIgual(resolverExpresion((NodoSintactico)listaHijos[0], ent, archivo), resolverExpresion((NodoSintactico)listaHijos[1], ent, archivo), archivo);
                case "MENORIGUAL":
                    return OperarMenorIgual(resolverExpresion((NodoSintactico)listaHijos[0], ent, archivo), resolverExpresion((NodoSintactico)listaHijos[1], ent, archivo), archivo);
                case "IGUALIGUAL":
                    return OperarIgualIgual(resolverExpresion((NodoSintactico)listaHijos[0], ent, archivo), resolverExpresion((NodoSintactico)listaHijos[1], ent, archivo), archivo);
                case "DESIGUAL":
                    return OperarDesIgual(resolverExpresion((NodoSintactico)listaHijos[0], ent, archivo), resolverExpresion((NodoSintactico)listaHijos[1], ent, archivo), archivo);
            }
            return new Expresion(Simbolo.EnumTipo.error, "ERROR");
        }

        public Expresion buscarVariable(NodoSintactico root, Entorno ent, int modificar, String archivo)
        {
            ArrayList listaHijos = root.getHijos();
            Simbolo resultado;
            String nombre;
            NodoSintactico auxiliar;
            Expresion expresion;
            Expresion expresion2;
            Expresion expresion3;
            int index;
            int index2;
            int index3;
            int entero;
            Double doble;
            Simbolo[] objetoDim1;
            Simbolo[,] objetoDim2;
            Simbolo[,,] objetoDim3;
            switch (root.getNombre())
            {
                case "DIM1":
                    auxiliar = (NodoSintactico)listaHijos[0];
                    nombre = auxiliar.getValor().ToString();
                    resultado = ent.buscar(nombre, 0, 0);
                    if (resultado != null)
                    {
                        //El arreglo existe
                        auxiliar = (NodoSintactico)listaHijos[1];
                        expresion = resolverExpresion(auxiliar, ent, archivo);
                        if (expresion.tipo != Simbolo.EnumTipo.error)
                        {
                            if (expresion.tipo == Simbolo.EnumTipo.entero)
                            {
                                index = int.Parse(expresion.valor.ToString());
                                if (index >= 0)
                                {
                                    try
                                    {
                                        objetoDim1 = (Simbolo[])resultado.valor;
                                        if (modificar == 0)
                                        {
                                            resultado = objetoDim1[index];
                                            return new Expresion(resultado.tipo, resultado.valor);
                                        }
                                        else
                                        {
                                            if (objetoDim1[index].tipo == Simbolo.EnumTipo.entero || objetoDim1[index].tipo == Simbolo.EnumTipo.doble)
                                            {
                                                if (modificar == 1)
                                                {
                                                    if (objetoDim1[index].tipo == Simbolo.EnumTipo.entero)
                                                    {
                                                        entero = int.Parse(objetoDim1[index].valor.ToString()) + 1;
                                                        objetoDim1[index] = new Simbolo(objetoDim1[index].tipo, entero);
                                                    }
                                                    else if (objetoDim1[index].tipo == Simbolo.EnumTipo.doble)
                                                    {
                                                        doble = Double.Parse(objetoDim1[index].valor.ToString()) + 1;
                                                        objetoDim1[index] = new Simbolo(objetoDim1[index].tipo, doble);
                                                    }
                                                }
                                                else if (modificar == 2)
                                                {
                                                    if (objetoDim1[index].tipo == Simbolo.EnumTipo.entero)
                                                    {
                                                        entero = int.Parse(objetoDim1[index].valor.ToString()) - 1;
                                                        objetoDim1[index] = new Simbolo(objetoDim1[index].tipo, entero);
                                                    }
                                                    else if (objetoDim1[index].tipo == Simbolo.EnumTipo.doble)
                                                    {
                                                        doble = Double.Parse(objetoDim1[index].valor.ToString()) - 1;
                                                        objetoDim1[index] = new Simbolo(objetoDim1[index].tipo, doble);
                                                    }
                                                }
                                                ent.modificar(nombre, new Simbolo(Simbolo.EnumTipo.dim1, objetoDim1));
                                                return null;
                                            }
                                            resultado = objetoDim1[index];
                                            return new Expresion(resultado.tipo, resultado.valor);
                                        }
                                    }
                                    catch (IndexOutOfRangeException)
                                    {
                                        return new Expresion(Simbolo.EnumTipo.error, "Index fuera del limite");
                                    }
                                    catch (InvalidCastException)
                                    {
                                        try
                                        {
                                            objetoDim2 = (Simbolo[,])resultado.valor;
                                            objetoDim1 = new Simbolo[objetoDim2.GetLength(1)];
                                            if (modificar == 0)
                                            {
                                                for (int i = 0; i < objetoDim2.GetLength(1); i++)
                                                {
                                                    objetoDim1[i] = objetoDim2[index, i];
                                                }
                                                return new Expresion(Simbolo.EnumTipo.dim1, objetoDim1);
                                            }
                                            return new Expresion(Simbolo.EnumTipo.error, "Operacion incremento o decremento no valida para arreglos");
                                        }
                                        catch (IndexOutOfRangeException)
                                        {
                                            return new Expresion(Simbolo.EnumTipo.error, "Index fuera del limite");
                                        }
                                        catch (InvalidCastException)
                                        {
                                            try
                                            {
                                                objetoDim3 = (Simbolo[,,])resultado.valor;
                                                objetoDim2 = new Simbolo[objetoDim3.GetLength(1), objetoDim3.GetLength(2)];
                                                if (modificar == 0)
                                                {
                                                    for (int i = 0; i < objetoDim3.GetLength(1); i++)
                                                    {
                                                        for (int j = 0; j < objetoDim3.GetLength(2); j++)
                                                        {
                                                            objetoDim2[i, j] = objetoDim3[index, i, j];
                                                        }
                                                    }
                                                    return new Expresion(Simbolo.EnumTipo.dim2, objetoDim2);
                                                }
                                                return new Expresion(Simbolo.EnumTipo.error, "Operacion incremento o decremento no valida para arreglos");
                                            }
                                            catch (IndexOutOfRangeException)
                                            {
                                                return new Expresion(Simbolo.EnumTipo.error, "Index fuera del limite");
                                            }
                                            catch (InvalidCastException)
                                            {
                                                return new Expresion(Simbolo.EnumTipo.error, "Se esperaba arreglo, " + resultado.tipo + " encontrado");
                                            }
                                        }
                                    }
                                }
                                return new Expresion(Simbolo.EnumTipo.error, "Index debe ser entero positivo");
                            }
                            return new Expresion(Simbolo.EnumTipo.error, "Se esperaba tipo entero, tipo: " + expresion.tipo + " encontrado");
                        }
                        return new Expresion(expresion.tipo, expresion.valor);
                    }
                    return new Expresion(Simbolo.EnumTipo.error, "Variable: " + nombre + " no Declarada");
                case "DIM2":
                    auxiliar = (NodoSintactico)listaHijos[0];
                    nombre = auxiliar.getValor().ToString();
                    resultado = ent.buscar(nombre, 0, 0);
                    if (resultado != null)
                    {
                        //El arreglo existe
                        expresion = resolverExpresion((NodoSintactico)listaHijos[1], ent, archivo);
                        expresion2 = resolverExpresion((NodoSintactico)listaHijos[2], ent, archivo);
                        if (expresion.tipo != Simbolo.EnumTipo.error)
                        {
                            if (expresion.tipo == Simbolo.EnumTipo.entero)
                            {
                                if (expresion2.tipo != Simbolo.EnumTipo.error)
                                {
                                    if (expresion2.tipo == Simbolo.EnumTipo.entero)
                                    {
                                        index = int.Parse(expresion.valor.ToString());
                                        index2 = int.Parse(expresion2.valor.ToString());
                                        if (index >= 0 && index2 >= 0)
                                        {
                                            try
                                            {
                                                objetoDim2 = (Simbolo[,])resultado.valor;
                                                if (modificar == 0)
                                                {
                                                    resultado = objetoDim2[index, index2];
                                                    return new Expresion(resultado.tipo, resultado.valor);
                                                }
                                                else
                                                {
                                                    if (objetoDim2[index, index2].tipo == Simbolo.EnumTipo.entero || objetoDim2[index, index2].tipo == Simbolo.EnumTipo.doble)
                                                    {
                                                        if (modificar == 1)
                                                        {
                                                            if (objetoDim2[index, index2].tipo == Simbolo.EnumTipo.entero)
                                                            {
                                                                entero = int.Parse(objetoDim2[index, index2].valor.ToString()) + 1;
                                                                objetoDim2[index, index2] = new Simbolo(objetoDim2[index, index2].tipo, entero);
                                                            }
                                                            else if (objetoDim2[index, index2].tipo == Simbolo.EnumTipo.doble)
                                                            {
                                                                doble = Double.Parse(objetoDim2[index, index2].valor.ToString()) + 1;
                                                                objetoDim2[index, index2] = new Simbolo(objetoDim2[index, index2].tipo, doble);
                                                            }
                                                        }
                                                        else if (modificar == 2)
                                                        {
                                                            if (objetoDim2[index, index2].tipo == Simbolo.EnumTipo.entero)
                                                            {
                                                                entero = int.Parse(objetoDim2[index, index2].valor.ToString()) - 1;
                                                                objetoDim2[index, index2] = new Simbolo(objetoDim2[index, index2].tipo, entero);
                                                            }
                                                            else if (objetoDim2[index, index2].tipo == Simbolo.EnumTipo.doble)
                                                            {
                                                                doble = Double.Parse(objetoDim2[index, index2].valor.ToString()) - 1;
                                                                objetoDim2[index, index2] = new Simbolo(objetoDim2[index, index2].tipo, doble);
                                                            }
                                                        }
                                                        ent.modificar(nombre, new Simbolo(Simbolo.EnumTipo.dim2, objetoDim2));
                                                        return null;
                                                    }
                                                    resultado = objetoDim2[index, index2];
                                                    return new Expresion(resultado.tipo, resultado.valor);
                                                }
                                            }
                                            catch (IndexOutOfRangeException)
                                            {
                                                return new Expresion(Simbolo.EnumTipo.error, "Index fuera del limite");
                                            }
                                            catch (InvalidCastException)
                                            {
                                                try
                                                {
                                                    objetoDim3 = (Simbolo[,,])resultado.valor;
                                                    objetoDim1 = new Simbolo[objetoDim3.GetLength(2)];
                                                    if (modificar == 0)
                                                    {
                                                        for (int i = 0; i < objetoDim3.GetLength(2); i++)
                                                        {
                                                            objetoDim1[i] = objetoDim3[index, index2, i];
                                                        }
                                                        return new Expresion(Simbolo.EnumTipo.dim1, objetoDim1);
                                                    }
                                                    return new Expresion(Simbolo.EnumTipo.error, "Operacion incremento o decremento no valida para arreglos");
                                                }
                                                catch (IndexOutOfRangeException)
                                                {
                                                    return new Expresion(Simbolo.EnumTipo.error, "Index fuera del limite");
                                                }
                                                catch (InvalidCastException)
                                                {
                                                    return new Expresion(Simbolo.EnumTipo.error, "Se esperaba arreglo de dim2 o de dim3, " + resultado.tipo + " encontrado");
                                                }
                                            }
                                        }
                                        return new Expresion(Simbolo.EnumTipo.error, "Index debe ser entero positivo");
                                    }
                                    return new Expresion(Simbolo.EnumTipo.error, "Se esperaba tipo entero, tipo: " + expresion2.tipo + " encontrado");
                                }
                                return new Expresion(expresion.tipo, expresion.valor);
                            }
                            return new Expresion(Simbolo.EnumTipo.error, "Se esperaba tipo entero, tipo: " + expresion.tipo + " encontrado");
                        }
                        return new Expresion(expresion.tipo, expresion.valor);
                    }
                    return new Expresion(Simbolo.EnumTipo.error, "Variable: " + nombre + " no Declarada");
                case "DIM3":
                    auxiliar = (NodoSintactico)listaHijos[0];
                    nombre = auxiliar.getValor().ToString();
                    resultado = ent.buscar(nombre, 0, 0);
                    if (resultado != null)
                    {
                        //El arreglo existe
                        expresion = resolverExpresion((NodoSintactico)listaHijos[1], ent, archivo);
                        expresion2 = resolverExpresion((NodoSintactico)listaHijos[2], ent, archivo);
                        expresion3 = resolverExpresion((NodoSintactico)listaHijos[3], ent, archivo);
                        if (expresion.tipo != Simbolo.EnumTipo.error)
                        {
                            if (expresion.tipo == Simbolo.EnumTipo.entero)
                            {
                                if (expresion2.tipo != Simbolo.EnumTipo.error)
                                {
                                    if (expresion2.tipo == Simbolo.EnumTipo.entero)
                                    {
                                        if (expresion3.tipo != Simbolo.EnumTipo.error)
                                        {
                                            if (expresion3.tipo == Simbolo.EnumTipo.entero)
                                            {
                                                index = int.Parse(expresion.valor.ToString());
                                                index2 = int.Parse(expresion2.valor.ToString());
                                                index3 = int.Parse(expresion3.valor.ToString());
                                                if (index >= 0 && index2 >= 0 && index3 >= 0)
                                                {
                                                    try
                                                    {
                                                        objetoDim3 = (Simbolo[,,])resultado.valor;
                                                        if (modificar == 0)
                                                        {
                                                            resultado = objetoDim3[index, index2, index3];
                                                            return new Expresion(resultado.tipo, resultado.valor);
                                                        }
                                                        else
                                                        {
                                                            if (objetoDim3[index, index2, index3].tipo == Simbolo.EnumTipo.entero || objetoDim3[index, index2, index3].tipo == Simbolo.EnumTipo.doble)
                                                            {
                                                                if (modificar == 1)
                                                                {
                                                                    if (objetoDim3[index, index2, index3].tipo == Simbolo.EnumTipo.entero)
                                                                    {
                                                                        entero = int.Parse(objetoDim3[index, index2, index3].valor.ToString()) + 1;
                                                                        objetoDim3[index, index2, index3] = new Simbolo(objetoDim3[index, index2, index3].tipo, entero);
                                                                    }
                                                                    else if (objetoDim3[index, index2, index3].tipo == Simbolo.EnumTipo.doble)
                                                                    {
                                                                        doble = Double.Parse(objetoDim3[index, index2, index3].valor.ToString()) + 1;
                                                                        objetoDim3[index, index2, index3] = new Simbolo(objetoDim3[index, index2, index3].tipo, doble);
                                                                    }
                                                                }
                                                                else if (modificar == 2)
                                                                {
                                                                    if (objetoDim3[index, index2, index3].tipo == Simbolo.EnumTipo.entero)
                                                                    {
                                                                        entero = int.Parse(objetoDim3[index, index2, index3].valor.ToString()) - 1;
                                                                        objetoDim3[index, index2, index3] = new Simbolo(objetoDim3[index, index2, index3].tipo, entero);
                                                                    }
                                                                    else if (objetoDim3[index, index2, index3].tipo == Simbolo.EnumTipo.doble)
                                                                    {
                                                                        doble = Double.Parse(objetoDim3[index, index2, index3].valor.ToString()) - 1;
                                                                        objetoDim3[index, index2, index3] = new Simbolo(objetoDim3[index, index2, index3].tipo, doble);
                                                                    }
                                                                }
                                                                ent.modificar(nombre, new Simbolo(Simbolo.EnumTipo.dim3, objetoDim3));
                                                                return null;
                                                            }
                                                            resultado = objetoDim3[index, index2, index3];
                                                            return new Expresion(resultado.tipo, resultado.valor);
                                                        }
                                                    }
                                                    catch (IndexOutOfRangeException)
                                                    {
                                                        return new Expresion(Simbolo.EnumTipo.error, "Index fuera del limite");
                                                    }
                                                    catch (InvalidCastException)
                                                    {
                                                        return new Expresion(Simbolo.EnumTipo.error, "Se esperaba arreglo de dim3, " + resultado.tipo + " encontrado");
                                                    }
                                                }
                                                return new Expresion(Simbolo.EnumTipo.error, "Index debe ser entero positivo");
                                            }
                                            return new Expresion(Simbolo.EnumTipo.error, "Se esperaba tipo entero, tipo: " + expresion3.tipo + " encontrado");
                                        }
                                        return new Expresion(expresion.tipo, expresion.valor);
                                    }
                                    return new Expresion(Simbolo.EnumTipo.error, "Se esperaba tipo entero, tipo: " + expresion2.tipo + " encontrado");
                                }
                                return new Expresion(expresion.tipo, expresion.valor);
                            }
                            return new Expresion(Simbolo.EnumTipo.error, "Se esperaba tipo entero, tipo: " + expresion.tipo + " encontrado");
                        }
                        return new Expresion(expresion.tipo, expresion.valor);
                    }
                    return new Expresion(Simbolo.EnumTipo.error, "Variable: " + nombre + " no Declarada");
                case "ID":
                    nombre = root.getValor().ToString();
                    resultado = ent.buscar(nombre, 0, 0);
                    if (resultado != null)
                    {
                        if (modificar == 0)
                        {
                            return new Expresion(resultado.tipo, resultado.valor);
                        }
                        else if (resultado.tipo == Simbolo.EnumTipo.entero || resultado.tipo == Simbolo.EnumTipo.doble)
                        {
                            if (modificar == 1)
                            {
                                if (resultado.tipo == Simbolo.EnumTipo.entero)
                                {
                                    entero = int.Parse(resultado.valor.ToString()) + 1;
                                    resultado = new Simbolo(resultado.tipo, entero);
                                }
                                else if (resultado.tipo == Simbolo.EnumTipo.doble)
                                {
                                    doble = Double.Parse(resultado.valor.ToString()) + 1;
                                    resultado = new Simbolo(resultado.tipo, doble);
                                }
                            }
                            else if (modificar == 2)
                            {
                                if (resultado.tipo == Simbolo.EnumTipo.entero)
                                {
                                    entero = int.Parse(resultado.valor.ToString()) - 1;
                                    resultado = new Simbolo(resultado.tipo, entero);
                                }
                                else if (resultado.tipo == Simbolo.EnumTipo.doble)
                                {
                                    doble = Double.Parse(resultado.valor.ToString()) - 1;
                                    resultado = new Simbolo(resultado.tipo, doble);
                                }
                            }
                            ent.modificar(nombre, new Simbolo(resultado.tipo, resultado.valor));
                            return null;
                        }
                        return new Expresion(resultado.tipo, resultado.valor);

                    }
                    return new Expresion(Simbolo.EnumTipo.error, "Variable: " + nombre + " no Declarada");
            }
            return new Expresion(Simbolo.EnumTipo.error, "Error Desconocido");
        }

        private Expresion OperarSuma(Expresion expresion1, Expresion expresion2, String archivo)
        {
            String exp1 = expresion1.valor.ToString();
            String exp2 = expresion2.valor.ToString();
            int numero1;
            int numero2;
            switch (expresion1.tipo)
            {
                case Simbolo.EnumTipo.cadena:
                    //Cadena (Entero || Doble || Caracter || boleano || Cadena)
                    return new Expresion(Simbolo.EnumTipo.cadena, expresion1.valor.ToString() + expresion2.valor.ToString());
                case Simbolo.EnumTipo.doble:
                    switch (expresion2.tipo)
                    {
                        case Simbolo.EnumTipo.cadena:
                            //Doble Cadena
                            return new Expresion(Simbolo.EnumTipo.cadena, expresion1.valor.ToString() + expresion2.valor.ToString());
                        case Simbolo.EnumTipo.entero:
                        case Simbolo.EnumTipo.doble:
                            //Doble Entero
                            return new Expresion(Simbolo.EnumTipo.doble, Double.Parse(expresion1.valor.ToString()) + Double.Parse(expresion2.valor.ToString()));
                        case Simbolo.EnumTipo.caracter:
                            numero1 = exp1[0];
                            numero2 = exp2[0];
                            //Doble Caracter
                            return new Expresion(Simbolo.EnumTipo.doble, Double.Parse(expresion1.valor.ToString()) + Double.Parse(numero2.ToString()));
                        case Simbolo.EnumTipo.error:
                            return expresion2;
                        default:
                            return new Expresion(Simbolo.EnumTipo.error, "Suma no definida entre los tipos " + expresion1.tipo + " y " + expresion2.tipo);
                    }
                case Simbolo.EnumTipo.entero:
                    switch (expresion2.tipo)
                    {
                        case Simbolo.EnumTipo.entero:
                            //Entero Entero
                            return new Expresion(Simbolo.EnumTipo.entero, int.Parse(expresion1.valor.ToString()) + int.Parse(expresion2.valor.ToString()));
                        case Simbolo.EnumTipo.cadena:
                            //Entero Cadena
                            return new Expresion(Simbolo.EnumTipo.cadena, expresion1.valor.ToString() + expresion2.valor.ToString());
                        case Simbolo.EnumTipo.caracter:
                            //Entero Caracter
                            return new Expresion(Simbolo.EnumTipo.entero, int.Parse(expresion1.valor.ToString()) + exp2[0]);
                        case Simbolo.EnumTipo.doble:
                            //Entero Doble
                            return new Expresion(Simbolo.EnumTipo.doble, Double.Parse(expresion1.valor.ToString()) + Double.Parse(expresion2.valor.ToString()));
                        case Simbolo.EnumTipo.error:
                            return expresion2;
                        default:
                            return new Expresion(Simbolo.EnumTipo.error, "Suma no definida entre los tipos " + expresion1.tipo + " y " + expresion2.tipo);
                    }
                case Simbolo.EnumTipo.caracter:
                    switch (expresion2.tipo)
                    {
                        case Simbolo.EnumTipo.cadena:
                            //Caracter Cadena
                            return new Expresion(Simbolo.EnumTipo.cadena, expresion1.valor.ToString() + expresion2.valor.ToString());
                        case Simbolo.EnumTipo.doble:
                            numero1 = exp1[0];
                            numero2 = exp2[0];
                            //Caracter Doble
                            return new Expresion(Simbolo.EnumTipo.doble, Double.Parse(numero1.ToString()) + Double.Parse(expresion2.valor.ToString()));
                        case Simbolo.EnumTipo.entero:
                            //Caracter Entero
                            return new Expresion(Simbolo.EnumTipo.entero, exp1[0] + int.Parse(expresion2.valor.ToString()));
                        case Simbolo.EnumTipo.caracter:
                            //Caracter Caracter
                            return new Expresion(Simbolo.EnumTipo.entero, exp1[0] + exp2[0]);
                        case Simbolo.EnumTipo.error:
                            return expresion2;
                        default:
                            return new Expresion(Simbolo.EnumTipo.error, "Suma no definida entre los tipos " + expresion1.tipo + " y " + expresion2.tipo);
                    }
                case Simbolo.EnumTipo.error:
                    return expresion1;
                default:
                    return new Expresion(Simbolo.EnumTipo.error, "Suma no definida entre los tipos " + expresion1.tipo + " y " + expresion2.tipo);
            }
        }

        private Expresion OperarResta(Expresion expresion1, Expresion expresion2, String archivo)
        {
            String exp1 = expresion1.valor.ToString();
            String exp2 = expresion2.valor.ToString();
            int numero1;
            int numero2;
            switch (expresion1.tipo)
            {
                case Simbolo.EnumTipo.doble:
                    switch (expresion2.tipo)
                    {
                        case Simbolo.EnumTipo.entero:
                        case Simbolo.EnumTipo.doble:
                            //Doble Entero
                            return new Expresion(Simbolo.EnumTipo.doble, Double.Parse(expresion1.valor.ToString()) - Double.Parse(expresion2.valor.ToString()));
                        case Simbolo.EnumTipo.caracter:
                            numero1 = exp1[0];
                            numero2 = exp2[0];
                            //Doble Caracter
                            return new Expresion(Simbolo.EnumTipo.doble, Double.Parse(expresion1.valor.ToString()) - Double.Parse(numero2.ToString()));
                        case Simbolo.EnumTipo.error:
                            return expresion2;
                        default:
                            return new Expresion(Simbolo.EnumTipo.error, "Resta no definida entre los tipos " + expresion1.tipo + " y " + expresion2.tipo);
                    }
                case Simbolo.EnumTipo.entero:
                    switch (expresion2.tipo)
                    {
                        case Simbolo.EnumTipo.entero:
                            //Entero Entero
                            return new Expresion(Simbolo.EnumTipo.entero, int.Parse(expresion1.valor.ToString()) - int.Parse(expresion2.valor.ToString()));
                        case Simbolo.EnumTipo.caracter:
                            //Entero Caracter
                            return new Expresion(Simbolo.EnumTipo.entero, int.Parse(expresion1.valor.ToString()) - exp2[0]);
                        case Simbolo.EnumTipo.doble:
                            //Entero Doble
                            return new Expresion(Simbolo.EnumTipo.doble, Double.Parse(expresion1.valor.ToString()) - Double.Parse(expresion2.valor.ToString()));
                        case Simbolo.EnumTipo.error:
                            return expresion2;
                        default:
                            return new Expresion(Simbolo.EnumTipo.error, "Resta no definida entre los tipos " + expresion1.tipo + " y " + expresion2.tipo);
                    }
                case Simbolo.EnumTipo.caracter:
                    switch (expresion2.tipo)
                    {
                        case Simbolo.EnumTipo.doble:
                            //Caracter Doble
                            return new Expresion(Simbolo.EnumTipo.doble, exp1[0] - Double.Parse(expresion2.valor.ToString()));
                        case Simbolo.EnumTipo.entero:
                            //Caracter Entero
                            return new Expresion(Simbolo.EnumTipo.entero, exp1[0] - int.Parse(expresion2.valor.ToString()));
                        case Simbolo.EnumTipo.caracter:
                            //Caracter Caracter
                            return new Expresion(Simbolo.EnumTipo.entero, exp1[0] - exp2[0]);
                        case Simbolo.EnumTipo.error:
                            return expresion2;
                        default:
                            return new Expresion(Simbolo.EnumTipo.error, "Resta no definida entre los tipos " + expresion1.tipo + " y " + expresion2.tipo);
                    }
                case Simbolo.EnumTipo.error:
                    return expresion1;
                default:
                    return new Expresion(Simbolo.EnumTipo.error, "Resta no definida entre los tipos " + expresion1.tipo + " y " + expresion2.tipo);
            }
        }

        private Expresion OperarMultiplicacion(Expresion expresion1, Expresion expresion2, String archivo)
        {
            String exp1 = expresion1.valor.ToString();
            String exp2 = expresion2.valor.ToString();
            int numero1;
            int numero2;
            switch (expresion1.tipo)
            {
                case Simbolo.EnumTipo.doble:
                    switch (expresion2.tipo)
                    {
                        case Simbolo.EnumTipo.entero:
                        case Simbolo.EnumTipo.doble:
                            //Doble Entero
                            return new Expresion(Simbolo.EnumTipo.doble, Double.Parse(expresion1.valor.ToString()) * Double.Parse(expresion2.valor.ToString()));
                        case Simbolo.EnumTipo.caracter:
                            numero1 = exp1[0];
                            numero2 = exp2[0];
                            //Doble Caracter
                            return new Expresion(Simbolo.EnumTipo.doble, Double.Parse(expresion1.valor.ToString()) * Double.Parse(numero2.ToString()));
                        case Simbolo.EnumTipo.error:
                            return expresion2;
                        default:
                            return new Expresion(Simbolo.EnumTipo.error, "Multiplicacion no definida entre los tipos " + expresion1.tipo + " y " + expresion2.tipo);
                    }
                case Simbolo.EnumTipo.entero:
                    switch (expresion2.tipo)
                    {
                        case Simbolo.EnumTipo.entero:
                            //Entero Entero
                            return new Expresion(Simbolo.EnumTipo.entero, int.Parse(expresion1.valor.ToString()) * int.Parse(expresion2.valor.ToString()));
                        case Simbolo.EnumTipo.caracter:
                            //Entero Caracter
                            return new Expresion(Simbolo.EnumTipo.entero, int.Parse(expresion1.valor.ToString()) * exp2[0]);
                        case Simbolo.EnumTipo.doble:
                            //Entero Doble
                            return new Expresion(Simbolo.EnumTipo.doble, Double.Parse(expresion1.valor.ToString()) * Double.Parse(expresion2.valor.ToString()));
                        case Simbolo.EnumTipo.error:
                            return expresion2;
                        default:
                            return new Expresion(Simbolo.EnumTipo.error, "Multiplicacion no definida entre los tipos " + expresion1.tipo + " y " + expresion2.tipo);
                    }
                case Simbolo.EnumTipo.caracter:
                    switch (expresion2.tipo)
                    {
                        case Simbolo.EnumTipo.doble:
                            numero1 = exp1[0];
                            numero2 = exp2[0];
                            //Caracter Doble
                            return new Expresion(Simbolo.EnumTipo.doble, Double.Parse(numero1.ToString()) * Double.Parse(expresion2.valor.ToString()));
                        case Simbolo.EnumTipo.entero:
                            //Caracter Entero
                            return new Expresion(Simbolo.EnumTipo.entero, exp1[0] * int.Parse(expresion2.valor.ToString()));
                        case Simbolo.EnumTipo.caracter:
                            //Caracter Caracter
                            return new Expresion(Simbolo.EnumTipo.entero, exp1[0] * exp2[0]);
                        case Simbolo.EnumTipo.error:
                            return expresion2;
                        default:
                            return new Expresion(Simbolo.EnumTipo.error, "Multiplicacion no definida entre los tipos " + expresion1.tipo + " y " + expresion2.tipo);
                    }
                case Simbolo.EnumTipo.error:
                    return expresion1;
                default:
                    return new Expresion(Simbolo.EnumTipo.error, "Multiplicacion no definida entre los tipos " + expresion1.tipo + " y " + expresion2.tipo);
            }
        }

        private Expresion OperarPotencia(Expresion expresion1, Expresion expresion2, String archivo)
        {
            String exp1 = expresion1.valor.ToString();
            String exp2 = expresion2.valor.ToString();
            int numero1;
            int numero2;
            switch (expresion1.tipo)
            {
                case Simbolo.EnumTipo.doble:
                    switch (expresion2.tipo)
                    {
                        case Simbolo.EnumTipo.entero:
                        case Simbolo.EnumTipo.doble:
                            //Doble Entero
                            return new Expresion(Simbolo.EnumTipo.doble, Math.Pow(Double.Parse(expresion1.valor.ToString()), Double.Parse(expresion2.valor.ToString())));
                        case Simbolo.EnumTipo.caracter:
                            numero1 = exp1[0];
                            numero2 = exp2[0];
                            //Doble Caracter
                            return new Expresion(Simbolo.EnumTipo.doble, Math.Pow(Double.Parse(expresion1.valor.ToString()), Double.Parse(numero2.ToString())));
                        case Simbolo.EnumTipo.error:
                            return expresion2;
                        default:
                            return new Expresion(Simbolo.EnumTipo.error, "Potencia no definida entre los tipos " + expresion1.tipo + " y " + expresion2.tipo);
                    }
                case Simbolo.EnumTipo.entero:
                    switch (expresion2.tipo)
                    {
                        case Simbolo.EnumTipo.entero:
                            //Entero Entero
                            return new Expresion(Simbolo.EnumTipo.entero, Math.Pow(int.Parse(expresion1.valor.ToString()), int.Parse(expresion2.valor.ToString())));
                        case Simbolo.EnumTipo.caracter:
                            numero1 = exp1[0];
                            numero2 = exp2[0];
                            //Entero Caracter
                            return new Expresion(Simbolo.EnumTipo.entero, Math.Pow(int.Parse(expresion1.valor.ToString()), Double.Parse(numero2.ToString())));
                        case Simbolo.EnumTipo.doble:
                            //Entero Doble
                            return new Expresion(Simbolo.EnumTipo.doble, Math.Pow(Double.Parse(expresion1.valor.ToString()), Double.Parse(expresion2.valor.ToString())));
                        case Simbolo.EnumTipo.error:
                            return expresion2;
                        default:
                            return new Expresion(Simbolo.EnumTipo.error, "Potencia no definida entre los tipos " + expresion1.tipo + " y " + expresion2.tipo);
                    }
                case Simbolo.EnumTipo.caracter:
                    switch (expresion2.tipo)
                    {
                        case Simbolo.EnumTipo.doble:
                            numero1 = exp1[0];
                            numero2 = exp2[0];
                            //Caracter Doble
                            return new Expresion(Simbolo.EnumTipo.doble, Math.Pow(Double.Parse(numero1.ToString()), Double.Parse(expresion2.valor.ToString())));
                        case Simbolo.EnumTipo.entero:
                            numero1 = exp1[0];
                            numero2 = exp2[0];
                            //Caracter Entero
                            return new Expresion(Simbolo.EnumTipo.entero, Math.Pow(Double.Parse(numero1.ToString()), int.Parse(expresion2.valor.ToString())));
                        case Simbolo.EnumTipo.caracter:
                            numero1 = exp1[0];
                            numero2 = exp2[0];
                            //Caracter Caracter
                            return new Expresion(Simbolo.EnumTipo.entero, Math.Pow(Double.Parse(numero1.ToString()), Double.Parse(numero2.ToString())));
                        case Simbolo.EnumTipo.error:
                            return expresion2;
                        default:
                            return new Expresion(Simbolo.EnumTipo.error, "Potencia no definida entre los tipos " + expresion1.tipo + " y " + expresion2.tipo);
                    }
                case Simbolo.EnumTipo.error:
                    return expresion1;
                default:
                    return new Expresion(Simbolo.EnumTipo.error, "Potencia no definida entre los tipos " + expresion1.tipo + " y " + expresion2.tipo);
            }
        }

        private Expresion OperarDivision(Expresion expresion1, Expresion expresion2, String archivo)
        {
            String exp1 = expresion1.valor.ToString();
            String exp2 = expresion2.valor.ToString();
            int numero1;
            int numero2;
            if (expresion2.valor.ToString().Equals("0") || expresion2.valor.ToString().Equals("0.0"))
            {
                return new Expresion(Simbolo.EnumTipo.error, "Division por 0 indefinida");
            }
            else
            {
                switch (expresion1.tipo)
                {
                    case Simbolo.EnumTipo.doble:
                        switch (expresion2.tipo)
                        {
                            case Simbolo.EnumTipo.entero:
                            case Simbolo.EnumTipo.doble:
                                //Doble Entero
                                return new Expresion(Simbolo.EnumTipo.doble, Double.Parse(expresion1.valor.ToString()) / Double.Parse(expresion2.valor.ToString()));
                            case Simbolo.EnumTipo.caracter:
                                numero1 = exp1[0];
                                numero2 = exp2[0];
                                //Doble Caracter
                                return new Expresion(Simbolo.EnumTipo.doble, Double.Parse(expresion1.valor.ToString()) / Double.Parse(numero2.ToString()));
                            case Simbolo.EnumTipo.error:
                                return expresion2;
                            default:
                                return new Expresion(Simbolo.EnumTipo.error, "Division no definida entre los tipos " + expresion1.tipo + " y " + expresion2.tipo);
                        }
                    case Simbolo.EnumTipo.entero:
                        switch (expresion2.tipo)
                        {
                            case Simbolo.EnumTipo.entero:
                                //Entero Entero
                                return new Expresion(Simbolo.EnumTipo.doble, Double.Parse(expresion1.valor.ToString()) / Double.Parse(expresion2.valor.ToString()));
                            case Simbolo.EnumTipo.caracter:
                                numero1 = exp1[0];
                                numero2 = exp2[0];
                                //Entero Caracter
                                return new Expresion(Simbolo.EnumTipo.doble, Double.Parse(expresion1.valor.ToString()) / Double.Parse(numero2.ToString()));
                            case Simbolo.EnumTipo.doble:
                                //Entero Doble
                                return new Expresion(Simbolo.EnumTipo.doble, Double.Parse(expresion1.valor.ToString()) / Double.Parse(expresion2.valor.ToString()));
                            case Simbolo.EnumTipo.error:
                                return expresion2;
                            default:
                                return new Expresion(Simbolo.EnumTipo.error, "Division no definida entre los tipos " + expresion1.tipo + " y " + expresion2.tipo);
                        }
                    case Simbolo.EnumTipo.caracter:
                        switch (expresion2.tipo)
                        {
                            case Simbolo.EnumTipo.doble:
                                numero1 = exp1[0];
                                numero2 = exp2[0];
                                //Caracter Doble
                                return new Expresion(Simbolo.EnumTipo.doble, Double.Parse(numero1.ToString()) / Double.Parse(expresion2.valor.ToString()));
                            case Simbolo.EnumTipo.entero:
                                numero1 = exp1[0];
                                numero2 = exp2[0];
                                //Caracter Entero
                                return new Expresion(Simbolo.EnumTipo.doble, Double.Parse(numero1.ToString()) / Double.Parse(expresion2.valor.ToString()));
                            case Simbolo.EnumTipo.caracter:
                                numero1 = exp1[0];
                                numero2 = exp2[0];
                                //Caracter Caracter
                                return new Expresion(Simbolo.EnumTipo.doble, Double.Parse(numero1.ToString()) / Double.Parse(numero2.ToString()));
                            case Simbolo.EnumTipo.error:
                                return expresion2;
                            default:
                                return new Expresion(Simbolo.EnumTipo.error, "Division no definida entre los tipos " + expresion1.tipo + " y " + expresion2.tipo);
                        }
                    case Simbolo.EnumTipo.error:
                        return expresion1;
                    default:
                        return new Expresion(Simbolo.EnumTipo.error, "Division no definida entre los tipos " + expresion1.tipo + " y " + expresion2.tipo);
                }
            }
        }

        private Expresion OperarNegativo(Expresion expresion1, String archivo)
        {
            String exp1 = expresion1.valor.ToString();
            switch (expresion1.tipo)
            {
                case Simbolo.EnumTipo.doble:
                    return new Expresion(Simbolo.EnumTipo.doble, 0 - int.Parse(expresion1.valor.ToString()));
                case Simbolo.EnumTipo.entero:
                    return new Expresion(Simbolo.EnumTipo.entero, 0 - int.Parse(expresion1.valor.ToString()));
                case Simbolo.EnumTipo.caracter:
                    return new Expresion(Simbolo.EnumTipo.entero, 0 - exp1[0]);
                case Simbolo.EnumTipo.error:
                    return expresion1;
                default:
                    return new Expresion(Simbolo.EnumTipo.error, "Negativo no definido para el tipo " + expresion1.tipo);
            }
        }

        private Expresion OperarIncremento(Expresion expresion1, bool variable, String archivo)
        {
            String exp1 = expresion1.valor.ToString();
            if (variable)
            {
                switch (expresion1.tipo)
                {
                    case Simbolo.EnumTipo.doble:
                        return new Expresion(Simbolo.EnumTipo.doble, int.Parse(expresion1.valor.ToString()) + 1);
                    case Simbolo.EnumTipo.entero:
                        return new Expresion(Simbolo.EnumTipo.entero, int.Parse(expresion1.valor.ToString()) + 1);
                    case Simbolo.EnumTipo.caracter:
                        return new Expresion(Simbolo.EnumTipo.entero, exp1[0] + 1);
                    case Simbolo.EnumTipo.error:
                        return expresion1;
                    default:
                        return new Expresion(Simbolo.EnumTipo.error, "Incremento no definido para el tipo " + expresion1.tipo);
                }
            }
            else
            {
                switch (expresion1.tipo)
                {
                    case Simbolo.EnumTipo.doble:
                        return new Expresion(Simbolo.EnumTipo.doble, int.Parse(expresion1.valor.ToString()));
                    case Simbolo.EnumTipo.entero:
                        return new Expresion(Simbolo.EnumTipo.entero, int.Parse(expresion1.valor.ToString()));
                    case Simbolo.EnumTipo.caracter:
                        return new Expresion(Simbolo.EnumTipo.entero, exp1[0]);
                    case Simbolo.EnumTipo.error:
                        return expresion1;
                    default:
                        return new Expresion(Simbolo.EnumTipo.error, "Incremento no definido para el tipo " + expresion1.tipo);
                }
            }
        }

        private Expresion OperarDecremento(Expresion expresion1, bool variable, String archivo)
        {
            String exp1 = expresion1.valor.ToString();
            if (variable)
            {
                switch (expresion1.tipo)
                {
                    case Simbolo.EnumTipo.doble:
                        return new Expresion(Simbolo.EnumTipo.doble, int.Parse(expresion1.valor.ToString()) - 1);
                    case Simbolo.EnumTipo.entero:
                        return new Expresion(Simbolo.EnumTipo.entero, int.Parse(expresion1.valor.ToString()) - 1);
                    case Simbolo.EnumTipo.caracter:
                        return new Expresion(Simbolo.EnumTipo.entero, exp1[0] - 1);
                    case Simbolo.EnumTipo.error:
                        return expresion1;
                    default:
                        return new Expresion(Simbolo.EnumTipo.error, "Decremento no definido para el tipo " + expresion1.tipo);
                }
            }
            else
            {
                switch (expresion1.tipo)
                {
                    case Simbolo.EnumTipo.doble:
                        return new Expresion(Simbolo.EnumTipo.doble, int.Parse(expresion1.valor.ToString()));
                    case Simbolo.EnumTipo.entero:
                        return new Expresion(Simbolo.EnumTipo.entero, int.Parse(expresion1.valor.ToString()));
                    case Simbolo.EnumTipo.caracter:
                        exp1 = expresion1.valor.ToString();
                        return new Expresion(Simbolo.EnumTipo.entero, exp1[0]);
                    case Simbolo.EnumTipo.error:
                        return expresion1;
                    default:
                        return new Expresion(Simbolo.EnumTipo.error, "Decremento no definido para el tipo " + expresion1.tipo);
                }
            }
        }

        private Expresion OperarAnd(Expresion expresion1, Expresion expresion2, String archivo)
        {
            bool resultado1;
            bool resultado2;
            switch (expresion1.tipo)
            {
                case Simbolo.EnumTipo.boleano:
                    if (expresion1.valor.ToString().ToLower().Equals("true"))
                    {
                        resultado1 = true;
                    }
                    else
                    {
                        resultado1 = false;
                    }
                    switch (expresion2.tipo)
                    {
                        case Simbolo.EnumTipo.boleano:
                            if (expresion2.valor.ToString().ToLower().Equals("true"))
                            {
                                resultado2 = true;
                            }
                            else
                            {
                                resultado2 = false;
                            }
                            return new Expresion(Simbolo.EnumTipo.boleano, resultado1 && resultado2);
                        default:
                            return new Expresion(Simbolo.EnumTipo.error, "Operacion AND no definida entre los tipos " + expresion1.tipo + " y " + expresion2.tipo);
                    }
                case Simbolo.EnumTipo.error:
                    return expresion1;
                default:
                    return new Expresion(Simbolo.EnumTipo.error, "Operacion AND no definida entre los tipos " + expresion1.tipo + " y " + expresion2.tipo);
            }
        }

        private Expresion OperarOr(Expresion expresion1, Expresion expresion2, String archivo)
        {
            bool resultado1;
            bool resultado2;
            switch (expresion1.tipo)
            {
                case Simbolo.EnumTipo.boleano:
                    if (expresion1.valor.ToString().ToLower().Equals("true"))
                    {
                        resultado1 = true;
                    }
                    else
                    {
                        resultado1 = false;
                    }
                    switch (expresion2.tipo)
                    {
                        case Simbolo.EnumTipo.boleano:
                            if (expresion2.valor.ToString().ToLower().Equals("true"))
                            {
                                resultado2 = true;
                            }
                            else
                            {
                                resultado2 = false;
                            }
                            return new Expresion(Simbolo.EnumTipo.boleano, resultado1 || resultado2);
                        default:
                            return new Expresion(Simbolo.EnumTipo.error, "Operacion OR no definida entre los tipos " + expresion1.tipo + " y " + expresion2.tipo);
                    }
                case Simbolo.EnumTipo.error:
                    return expresion1;
                default:
                    return new Expresion(Simbolo.EnumTipo.error, "Operacion OR no definida entre los tipos " + expresion1.tipo + " y " + expresion2.tipo);
            }
        }

        private Expresion OperarXor(Expresion expresion1, Expresion expresion2, String archivo)
        {
            bool resultado1;
            bool resultado2;
            switch (expresion1.tipo)
            {
                case Simbolo.EnumTipo.boleano:
                    if (expresion1.valor.ToString().ToLower().Equals("true"))
                    {
                        resultado1 = true;
                    }
                    else
                    {
                        resultado1 = false;
                    }
                    switch (expresion2.tipo)
                    {
                        case Simbolo.EnumTipo.boleano:
                            if (expresion2.valor.ToString().ToLower().Equals("true"))
                            {
                                resultado2 = true;
                            }
                            else
                            {
                                resultado2 = false;
                            }
                            return new Expresion(Simbolo.EnumTipo.boleano, resultado1 ^ resultado2);
                        default:
                            return new Expresion(Simbolo.EnumTipo.error, "Operacion XOR no definida entre los tipos " + expresion1.tipo + " y " + expresion2.tipo);
                    }
                case Simbolo.EnumTipo.error:
                    return expresion1;
                default:
                    return new Expresion(Simbolo.EnumTipo.error, "Operacion XOR no definida entre los tipos " + expresion1.tipo + " y " + expresion2.tipo);
            }
        }

        private Expresion OperarNot(Expresion expresion1, String archivo)
        {
            bool resultado1;
            switch (expresion1.tipo)
            {
                case Simbolo.EnumTipo.boleano:
                    if (expresion1.valor.ToString().ToLower().Equals("true"))
                    {
                        resultado1 = true;
                    }
                    else
                    {
                        resultado1 = false;
                    }
                    return new Expresion(Simbolo.EnumTipo.boleano, !resultado1);
                case Simbolo.EnumTipo.error:
                    return expresion1;
                default:
                    return new Expresion(Simbolo.EnumTipo.error, "Operacion NOT no definida para el tipo " + expresion1.tipo);
            }
        }

        private Expresion OperarMayor(Expresion expresion1, Expresion expresion2, String archivo)
        {
            String exp1 = expresion1.valor.ToString();
            String exp2 = expresion2.valor.ToString();
            int numero2;
            switch (expresion1.tipo)
            {
                case Simbolo.EnumTipo.doble:
                    switch (expresion2.tipo)
                    {
                        case Simbolo.EnumTipo.entero:
                            return new Expresion(Simbolo.EnumTipo.boleano, Double.Parse(expresion1.valor.ToString()) > int.Parse(expresion2.valor.ToString()));
                        case Simbolo.EnumTipo.doble:
                            //Doble Entero
                            return new Expresion(Simbolo.EnumTipo.boleano, Double.Parse(expresion1.valor.ToString()) > Double.Parse(expresion2.valor.ToString()));
                        case Simbolo.EnumTipo.caracter:
                            numero2 = exp2[0];
                            //Doble Caracter
                            return new Expresion(Simbolo.EnumTipo.boleano, Double.Parse(expresion1.valor.ToString()) > numero2);
                        case Simbolo.EnumTipo.error:
                            return expresion2;
                        default:
                            return new Expresion(Simbolo.EnumTipo.error, "Operacion mayor no definida entre los tipos " + expresion1.tipo + " y " + expresion2.tipo);
                    }
                case Simbolo.EnumTipo.entero:
                    switch (expresion2.tipo)
                    {
                        case Simbolo.EnumTipo.entero:
                            //Entero Entero
                            return new Expresion(Simbolo.EnumTipo.boleano, int.Parse(expresion1.valor.ToString()) > int.Parse(expresion2.valor.ToString()));
                        case Simbolo.EnumTipo.caracter:
                            //Entero Caracter
                            return new Expresion(Simbolo.EnumTipo.boleano, int.Parse(expresion1.valor.ToString()) > exp2[0]);
                        case Simbolo.EnumTipo.doble:
                            //Entero Doble
                            return new Expresion(Simbolo.EnumTipo.boleano, int.Parse(expresion1.valor.ToString()) > Double.Parse(expresion2.valor.ToString()));
                        case Simbolo.EnumTipo.error:
                            return expresion2;
                        default:
                            return new Expresion(Simbolo.EnumTipo.error, "Operacion mayor no definida entre los tipos " + expresion1.tipo + " y " + expresion2.tipo);
                    }
                case Simbolo.EnumTipo.caracter:
                    switch (expresion2.tipo)
                    {
                        case Simbolo.EnumTipo.doble:
                            //Caracter Doble
                            return new Expresion(Simbolo.EnumTipo.boleano, exp1[0] > Double.Parse(expresion2.valor.ToString()));
                        case Simbolo.EnumTipo.entero:
                            //Caracter Entero
                            return new Expresion(Simbolo.EnumTipo.boleano, exp1[0] > int.Parse(expresion2.valor.ToString()));
                        case Simbolo.EnumTipo.caracter:
                            //Caracter Caracter
                            return new Expresion(Simbolo.EnumTipo.boleano, exp1[0] > exp2[0]);
                        case Simbolo.EnumTipo.error:
                            return expresion2;
                        default:
                            return new Expresion(Simbolo.EnumTipo.error, "Operacion mayor no definida entre los tipos " + expresion1.tipo + " y " + expresion2.tipo);
                    }
                case Simbolo.EnumTipo.error:
                    return expresion1;
                default:
                    return new Expresion(Simbolo.EnumTipo.error, "Operacion mayor no definida entre los tipos " + expresion1.tipo + " y " + expresion2.tipo);
            }
        }

        private Expresion OperarMenor(Expresion expresion1, Expresion expresion2, String archivo)
        {
            String exp1 = expresion1.valor.ToString();
            String exp2 = expresion2.valor.ToString();
            int numero2;
            switch (expresion1.tipo)
            {
                case Simbolo.EnumTipo.doble:
                    switch (expresion2.tipo)
                    {
                        case Simbolo.EnumTipo.entero:
                            return new Expresion(Simbolo.EnumTipo.boleano, Double.Parse(expresion1.valor.ToString()) < int.Parse(expresion2.valor.ToString()));
                        case Simbolo.EnumTipo.doble:
                            //Doble Entero
                            return new Expresion(Simbolo.EnumTipo.boleano, Double.Parse(expresion1.valor.ToString()) < Double.Parse(expresion2.valor.ToString()));
                        case Simbolo.EnumTipo.caracter:
                            numero2 = exp2[0];
                            //Doble Caracter
                            return new Expresion(Simbolo.EnumTipo.boleano, Double.Parse(expresion1.valor.ToString()) < numero2);
                        case Simbolo.EnumTipo.error:
                            return expresion2;
                        default:
                            return new Expresion(Simbolo.EnumTipo.error, "Operacion menor no definida entre los tipos " + expresion1.tipo + " y " + expresion2.tipo);
                    }
                case Simbolo.EnumTipo.entero:
                    switch (expresion2.tipo)
                    {
                        case Simbolo.EnumTipo.entero:
                            //Entero Entero
                            return new Expresion(Simbolo.EnumTipo.boleano, int.Parse(expresion1.valor.ToString()) < int.Parse(expresion2.valor.ToString()));
                        case Simbolo.EnumTipo.caracter:
                            //Entero Caracter
                            return new Expresion(Simbolo.EnumTipo.boleano, int.Parse(expresion1.valor.ToString()) < exp2[0]);
                        case Simbolo.EnumTipo.doble:
                            //Entero Doble
                            return new Expresion(Simbolo.EnumTipo.boleano, int.Parse(expresion1.valor.ToString()) < Double.Parse(expresion2.valor.ToString()));
                        case Simbolo.EnumTipo.error:
                            return expresion2;
                        default:
                            return new Expresion(Simbolo.EnumTipo.error, "Operacion menor no definida entre los tipos " + expresion1.tipo + " y " + expresion2.tipo);
                    }
                case Simbolo.EnumTipo.caracter:
                    switch (expresion2.tipo)
                    {
                        case Simbolo.EnumTipo.doble:
                            //Caracter Doble
                            return new Expresion(Simbolo.EnumTipo.boleano, exp1[0] < Double.Parse(expresion2.valor.ToString()));
                        case Simbolo.EnumTipo.entero:
                            //Caracter Entero
                            return new Expresion(Simbolo.EnumTipo.boleano, exp1[0] < int.Parse(expresion2.valor.ToString()));
                        case Simbolo.EnumTipo.caracter:
                            //Caracter Caracter
                            return new Expresion(Simbolo.EnumTipo.boleano, exp1[0] < exp2[0]);
                        case Simbolo.EnumTipo.error:
                            return expresion2;
                        default:
                            return new Expresion(Simbolo.EnumTipo.error, "Operacion menor no definida entre los tipos " + expresion1.tipo + " y " + expresion2.tipo);
                    }
                case Simbolo.EnumTipo.error:
                    return expresion1;
                default:
                    return new Expresion(Simbolo.EnumTipo.error, "Operacion menor no definida entre los tipos " + expresion1.tipo + " y " + expresion2.tipo);
            }
        }

        private Expresion OperarMayorIgual(Expresion expresion1, Expresion expresion2, String archivo)
        {
            String exp1 = expresion1.valor.ToString();
            String exp2 = expresion2.valor.ToString();
            int numero2;
            switch (expresion1.tipo)
            {
                case Simbolo.EnumTipo.doble:
                    switch (expresion2.tipo)
                    {
                        case Simbolo.EnumTipo.entero:
                            return new Expresion(Simbolo.EnumTipo.boleano, Double.Parse(expresion1.valor.ToString()) >= int.Parse(expresion2.valor.ToString()));
                        case Simbolo.EnumTipo.doble:
                            //Doble Entero
                            return new Expresion(Simbolo.EnumTipo.boleano, Double.Parse(expresion1.valor.ToString()) >= Double.Parse(expresion2.valor.ToString()));
                        case Simbolo.EnumTipo.caracter:
                            numero2 = exp2[0];
                            //Doble Caracter
                            return new Expresion(Simbolo.EnumTipo.boleano, Double.Parse(expresion1.valor.ToString()) >= numero2);
                        case Simbolo.EnumTipo.error:
                            return expresion2;
                        default:
                            return new Expresion(Simbolo.EnumTipo.error, "Operacion mayor o igual no definida entre los tipos " + expresion1.tipo + " y " + expresion2.tipo);
                    }
                case Simbolo.EnumTipo.entero:
                    switch (expresion2.tipo)
                    {
                        case Simbolo.EnumTipo.entero:
                            //Entero Entero
                            return new Expresion(Simbolo.EnumTipo.boleano, int.Parse(expresion1.valor.ToString()) >= int.Parse(expresion2.valor.ToString()));
                        case Simbolo.EnumTipo.caracter:
                            //Entero Caracter
                            return new Expresion(Simbolo.EnumTipo.boleano, int.Parse(expresion1.valor.ToString()) >= exp2[0]);
                        case Simbolo.EnumTipo.doble:
                            //Entero Doble
                            return new Expresion(Simbolo.EnumTipo.boleano, int.Parse(expresion1.valor.ToString()) >= Double.Parse(expresion2.valor.ToString()));
                        case Simbolo.EnumTipo.error:
                            return expresion2;
                        default:
                            return new Expresion(Simbolo.EnumTipo.error, "Operacion mayor o igual no definida entre los tipos " + expresion1.tipo + " y " + expresion2.tipo);
                    }
                case Simbolo.EnumTipo.caracter:
                    switch (expresion2.tipo)
                    {
                        case Simbolo.EnumTipo.doble:
                            //Caracter Doble
                            return new Expresion(Simbolo.EnumTipo.boleano, exp1[0] >= Double.Parse(expresion2.valor.ToString()));
                        case Simbolo.EnumTipo.entero:
                            //Caracter Entero
                            return new Expresion(Simbolo.EnumTipo.boleano, exp1[0] >= int.Parse(expresion2.valor.ToString()));
                        case Simbolo.EnumTipo.caracter:
                            //Caracter Caracter
                            return new Expresion(Simbolo.EnumTipo.boleano, exp1[0] >= exp2[0]);
                        case Simbolo.EnumTipo.error:
                            return expresion2;
                        default:
                            return new Expresion(Simbolo.EnumTipo.error, "Operacion mayor o igual no definida entre los tipos " + expresion1.tipo + " y " + expresion2.tipo);
                    }
                case Simbolo.EnumTipo.error:
                    return expresion1;
                default:
                    return new Expresion(Simbolo.EnumTipo.error, "Operacion mayor o igual no definida entre los tipos " + expresion1.tipo + " y " + expresion2.tipo);
            }
        }

        private Expresion OperarMenorIgual(Expresion expresion1, Expresion expresion2, String archivo)
        {
            String exp1 = expresion1.valor.ToString();
            String exp2 = expresion2.valor.ToString();
            int numero2;
            switch (expresion1.tipo)
            {
                case Simbolo.EnumTipo.doble:
                    switch (expresion2.tipo)
                    {
                        case Simbolo.EnumTipo.entero:
                            return new Expresion(Simbolo.EnumTipo.boleano, Double.Parse(expresion1.valor.ToString()) <= int.Parse(expresion2.valor.ToString()));
                        case Simbolo.EnumTipo.doble:
                            //Doble Entero
                            return new Expresion(Simbolo.EnumTipo.boleano, Double.Parse(expresion1.valor.ToString()) <= Double.Parse(expresion2.valor.ToString()));
                        case Simbolo.EnumTipo.caracter:
                            numero2 = exp2[0];
                            //Doble Caracter
                            return new Expresion(Simbolo.EnumTipo.boleano, Double.Parse(expresion1.valor.ToString()) <= numero2);
                        case Simbolo.EnumTipo.error:
                            return expresion2;
                        default:
                            return new Expresion(Simbolo.EnumTipo.error, "Operacion menor o igual no definida entre los tipos " + expresion1.tipo + " y " + expresion2.tipo);
                    }
                case Simbolo.EnumTipo.entero:
                    switch (expresion2.tipo)
                    {
                        case Simbolo.EnumTipo.entero:
                            //Entero Entero
                            return new Expresion(Simbolo.EnumTipo.boleano, int.Parse(expresion1.valor.ToString()) <= int.Parse(expresion2.valor.ToString()));
                        case Simbolo.EnumTipo.caracter:
                            //Entero Caracter
                            return new Expresion(Simbolo.EnumTipo.boleano, int.Parse(expresion1.valor.ToString()) <= exp2[0]);
                        case Simbolo.EnumTipo.doble:
                            //Entero Doble
                            return new Expresion(Simbolo.EnumTipo.boleano, int.Parse(expresion1.valor.ToString()) <= Double.Parse(expresion2.valor.ToString()));
                        case Simbolo.EnumTipo.error:
                            return expresion2;
                        default:
                            return new Expresion(Simbolo.EnumTipo.error, "Operacion menor o igual no definida entre los tipos " + expresion1.tipo + " y " + expresion2.tipo);
                    }
                case Simbolo.EnumTipo.caracter:
                    switch (expresion2.tipo)
                    {
                        case Simbolo.EnumTipo.doble:
                            //Caracter Doble
                            return new Expresion(Simbolo.EnumTipo.boleano, exp1[0] <= Double.Parse(expresion2.valor.ToString()));
                        case Simbolo.EnumTipo.entero:
                            //Caracter Entero
                            return new Expresion(Simbolo.EnumTipo.boleano, exp1[0] <= int.Parse(expresion2.valor.ToString()));
                        case Simbolo.EnumTipo.caracter:
                            //Caracter Caracter
                            return new Expresion(Simbolo.EnumTipo.boleano, exp1[0] <= exp2[0]);
                        case Simbolo.EnumTipo.error:
                            return expresion2;
                        default:
                            return new Expresion(Simbolo.EnumTipo.error, "Operacion menor o igual no definida entre los tipos " + expresion1.tipo + " y " + expresion2.tipo);
                    }
                case Simbolo.EnumTipo.error:
                    return expresion1;
                default:
                    return new Expresion(Simbolo.EnumTipo.error, "Operacion menor o igual no definida entre los tipos " + expresion1.tipo + " y " + expresion2.tipo);
            }
        }

        private Expresion OperarIgualIgual(Expresion expresion1, Expresion expresion2, String archivo)
        {
            String exp1 = expresion1.valor.ToString();
            String exp2 = expresion2.valor.ToString();
            int numero2;
            switch (expresion1.tipo)
            {
                case Simbolo.EnumTipo.boleano:
                    switch (expresion2.tipo)
                    {
                        case Simbolo.EnumTipo.boleano:
                            return new Expresion(Simbolo.EnumTipo.boleano, expresion1.valor.ToString().ToLower().Equals(expresion2.valor.ToString().ToLower()));
                        default:
                            return new Expresion(Simbolo.EnumTipo.error, "Operacion igual no definida entre los tipos " + expresion1.tipo + " y " + expresion2.tipo);
                    }
                case Simbolo.EnumTipo.cadena:
                    switch (expresion2.tipo)
                    {
                        case Simbolo.EnumTipo.cadena:
                            return new Expresion(Simbolo.EnumTipo.boleano, expresion1.valor.ToString().Equals(expresion2.valor.ToString()));
                        case Simbolo.EnumTipo.nulo:
                            return new Expresion(Simbolo.EnumTipo.boleano, false);
                        default:
                            return new Expresion(Simbolo.EnumTipo.error, "Operacion igual no definida entre los tipos " + expresion1.tipo + " y " + expresion2.tipo);
                    }
                case Simbolo.EnumTipo.doble:
                    switch (expresion2.tipo)
                    {
                        case Simbolo.EnumTipo.entero:
                            return new Expresion(Simbolo.EnumTipo.boleano, Double.Parse(expresion1.valor.ToString()) == int.Parse(expresion2.valor.ToString()));
                        case Simbolo.EnumTipo.doble:
                            //Doble Entero
                            return new Expresion(Simbolo.EnumTipo.boleano, Double.Parse(expresion1.valor.ToString()) == Double.Parse(expresion2.valor.ToString()));
                        case Simbolo.EnumTipo.caracter:
                            numero2 = exp2[0];
                            //Doble Caracter
                            return new Expresion(Simbolo.EnumTipo.boleano, Double.Parse(expresion1.valor.ToString()) == numero2);
                        case Simbolo.EnumTipo.error:
                            return expresion2;
                        case Simbolo.EnumTipo.nulo:
                            return new Expresion(Simbolo.EnumTipo.boleano, false);
                        default:
                            return new Expresion(Simbolo.EnumTipo.error, "Operacion igual no definida entre los tipos " + expresion1.tipo + " y " + expresion2.tipo);
                    }
                case Simbolo.EnumTipo.entero:
                    switch (expresion2.tipo)
                    {
                        case Simbolo.EnumTipo.entero:
                            //Entero Entero
                            return new Expresion(Simbolo.EnumTipo.boleano, int.Parse(expresion1.valor.ToString()) == int.Parse(expresion2.valor.ToString()));
                        case Simbolo.EnumTipo.caracter:
                            //Entero Caracter
                            return new Expresion(Simbolo.EnumTipo.boleano, int.Parse(expresion1.valor.ToString()) == exp2[0]);
                        case Simbolo.EnumTipo.doble:
                            //Entero Doble
                            return new Expresion(Simbolo.EnumTipo.boleano, int.Parse(expresion1.valor.ToString()) == Double.Parse(expresion2.valor.ToString()));
                        case Simbolo.EnumTipo.error:
                            return expresion2;
                        case Simbolo.EnumTipo.nulo:
                            return new Expresion(Simbolo.EnumTipo.boleano, false);
                        default:
                            return new Expresion(Simbolo.EnumTipo.error, "Operacion igual no definida entre los tipos " + expresion1.tipo + " y " + expresion2.tipo);
                    }
                case Simbolo.EnumTipo.caracter:
                    switch (expresion2.tipo)
                    {
                        case Simbolo.EnumTipo.doble:
                            //Caracter Doble
                            return new Expresion(Simbolo.EnumTipo.boleano, exp1[0] == Double.Parse(expresion2.valor.ToString()));
                        case Simbolo.EnumTipo.entero:
                            //Caracter Entero
                            return new Expresion(Simbolo.EnumTipo.boleano, exp1[0] == int.Parse(expresion2.valor.ToString()));
                        case Simbolo.EnumTipo.caracter:
                            //Caracter Caracter
                            return new Expresion(Simbolo.EnumTipo.boleano, exp1[0] == exp2[0]);
                        case Simbolo.EnumTipo.error:
                            return expresion2;
                        case Simbolo.EnumTipo.nulo:
                            return new Expresion(Simbolo.EnumTipo.boleano, false);
                        default:
                            return new Expresion(Simbolo.EnumTipo.error, "Operacion igual no definida entre los tipos " + expresion1.tipo + " y " + expresion2.tipo);
                    }
                case Simbolo.EnumTipo.error:
                    return expresion1;
                case Simbolo.EnumTipo.nulo:
                    switch (expresion2.tipo)
                    {
                        case Simbolo.EnumTipo.nulo:
                            return new Expresion(Simbolo.EnumTipo.boleano, true);
                        case Simbolo.EnumTipo.clase:
                            return new Expresion(Simbolo.EnumTipo.boleano, false);
                        case Simbolo.EnumTipo.error:
                            return expresion2;
                        default:
                            return new Expresion(Simbolo.EnumTipo.boleano, false);
                    }
                case Simbolo.EnumTipo.clase:
                    switch (expresion2.tipo)
                    {
                        case Simbolo.EnumTipo.nulo:
                            return new Expresion(Simbolo.EnumTipo.boleano, false);
                        case Simbolo.EnumTipo.error:
                            return expresion2;
                        default:
                            return new Expresion(Simbolo.EnumTipo.error, "Operacion igual no definida entre los tipos " + expresion1.tipo + " y " + expresion2.tipo);
                    }
                default:
                    return new Expresion(Simbolo.EnumTipo.error, "Operacion igual no definida entre los tipos " + expresion1.tipo + " y " + expresion2.tipo);
            }
        }

        private Expresion OperarDesIgual(Expresion expresion1, Expresion expresion2, String archivo)
        {
            String exp1 = expresion1.valor.ToString();
            String exp2 = expresion2.valor.ToString();
            int numero2;
            switch (expresion1.tipo)
            {
                case Simbolo.EnumTipo.boleano:
                    switch (expresion2.tipo)
                    {
                        case Simbolo.EnumTipo.boleano:
                            return new Expresion(Simbolo.EnumTipo.boleano, !expresion1.valor.ToString().ToLower().Equals(expresion2.valor.ToString().ToLower()));
                        default:
                            return new Expresion(Simbolo.EnumTipo.error, "Operacion igual no definida entre los tipos " + expresion1.tipo + " y " + expresion2.tipo);
                    }
                case Simbolo.EnumTipo.cadena:
                    switch (expresion2.tipo)
                    {
                        case Simbolo.EnumTipo.cadena:
                            return new Expresion(Simbolo.EnumTipo.boleano, !expresion1.valor.ToString().Equals(expresion2.valor.ToString()));
                        case Simbolo.EnumTipo.nulo:
                            return new Expresion(Simbolo.EnumTipo.boleano, true);
                        default:
                            return new Expresion(Simbolo.EnumTipo.error, "Operacion desigual no definida entre los tipos " + expresion1.tipo + " y " + expresion2.tipo);
                    }
                case Simbolo.EnumTipo.doble:
                    switch (expresion2.tipo)
                    {
                        case Simbolo.EnumTipo.entero:
                            return new Expresion(Simbolo.EnumTipo.boleano, Double.Parse(expresion1.valor.ToString()) != int.Parse(expresion2.valor.ToString()));
                        case Simbolo.EnumTipo.doble:
                            //Doble Entero
                            return new Expresion(Simbolo.EnumTipo.boleano, Double.Parse(expresion1.valor.ToString()) != Double.Parse(expresion2.valor.ToString()));
                        case Simbolo.EnumTipo.caracter:
                            numero2 = exp2[0];
                            //Doble Caracter
                            return new Expresion(Simbolo.EnumTipo.boleano, Double.Parse(expresion1.valor.ToString()) != numero2);
                        case Simbolo.EnumTipo.error:
                            return expresion2;
                        case Simbolo.EnumTipo.nulo:
                            return new Expresion(Simbolo.EnumTipo.boleano, true);
                        default:
                            return new Expresion(Simbolo.EnumTipo.error, "Operacion desigual no definida entre los tipos " + expresion1.tipo + " y " + expresion2.tipo);
                    }
                case Simbolo.EnumTipo.entero:
                    switch (expresion2.tipo)
                    {
                        case Simbolo.EnumTipo.entero:
                            //Entero Entero
                            return new Expresion(Simbolo.EnumTipo.boleano, int.Parse(expresion1.valor.ToString()) != int.Parse(expresion2.valor.ToString()));
                        case Simbolo.EnumTipo.caracter:
                            //Entero Caracter
                            return new Expresion(Simbolo.EnumTipo.boleano, int.Parse(expresion1.valor.ToString()) != exp2[0]);
                        case Simbolo.EnumTipo.doble:
                            //Entero Doble
                            return new Expresion(Simbolo.EnumTipo.boleano, int.Parse(expresion1.valor.ToString()) != Double.Parse(expresion2.valor.ToString()));
                        case Simbolo.EnumTipo.error:
                            return expresion2;
                        case Simbolo.EnumTipo.nulo:
                            return new Expresion(Simbolo.EnumTipo.boleano, true);
                        default:
                            return new Expresion(Simbolo.EnumTipo.error, "Operacion desigual no definida entre los tipos " + expresion1.tipo + " y " + expresion2.tipo);
                    }
                case Simbolo.EnumTipo.caracter:
                    switch (expresion2.tipo)
                    {
                        case Simbolo.EnumTipo.doble:
                            //Caracter Doble
                            return new Expresion(Simbolo.EnumTipo.boleano, exp1[0] != Double.Parse(expresion2.valor.ToString()));
                        case Simbolo.EnumTipo.entero:
                            //Caracter Entero
                            return new Expresion(Simbolo.EnumTipo.boleano, exp1[0] != int.Parse(expresion2.valor.ToString()));
                        case Simbolo.EnumTipo.caracter:
                            //Caracter Caracter
                            return new Expresion(Simbolo.EnumTipo.boleano, exp1[0] != exp2[0]);
                        case Simbolo.EnumTipo.error:
                            return expresion2;
                        case Simbolo.EnumTipo.nulo:
                            return new Expresion(Simbolo.EnumTipo.boleano, true);
                        default:
                            return new Expresion(Simbolo.EnumTipo.error, "Operacion desigual no definida entre los tipos " + expresion1.tipo + " y " + expresion2.tipo);
                    }
                case Simbolo.EnumTipo.error:
                    return expresion1;
                case Simbolo.EnumTipo.nulo:
                    switch (expresion2.tipo)
                    {
                        case Simbolo.EnumTipo.nulo:
                            return new Expresion(Simbolo.EnumTipo.boleano, false);
                        case Simbolo.EnumTipo.error:
                            return expresion2;
                        case Simbolo.EnumTipo.clase:
                            return new Expresion(Simbolo.EnumTipo.boleano, true);
                        default:
                            return new Expresion(Simbolo.EnumTipo.boleano, true);
                    }
                case Simbolo.EnumTipo.clase:
                    switch (expresion2.tipo)
                    {
                        case Simbolo.EnumTipo.nulo:
                            return new Expresion(Simbolo.EnumTipo.boleano, true);
                        case Simbolo.EnumTipo.error:
                            return expresion2;
                        default:
                            return new Expresion(Simbolo.EnumTipo.error, "Operacion igual no definida entre los tipos " + expresion1.tipo + " y " + expresion2.tipo);
                    }
                default:
                    return new Expresion(Simbolo.EnumTipo.error, "Operacion desigual no definida entre los tipos " + expresion1.tipo + " y " + expresion2.tipo);
            }
        }

        private void recorrerArbol(NodoSintactico padreSintactico, ParseTreeNode padreIrony)
        {
            NodoSintactico nuevo;
            NodoSintactico nuevo2;
            string valor;
            switch (padreIrony.ToString())
            {
                #region inicio, import, main
                case "INICIO":
                    rootArbol = new NodoSintactico("INICIO", contadorNodos++);
                    foreach (ParseTreeNode hijo in padreIrony.ChildNodes)
                    {
                        recorrerArbol(rootArbol, hijo);
                    }
                    break;
                case "GLOBAL":
                    rootArbol = new NodoSintactico("GLOBAL", contadorNodos++);
                    foreach (ParseTreeNode hijo in padreIrony.ChildNodes)
                    {
                        recorrerArbol(rootArbol, hijo);
                    }
                    break;
                case "IMPORT":
                    if (padreIrony.ChildNodes.Count > 1)
                    {
                        if (!padreSintactico.getNombre().Equals("IMPORT"))
                        {
                            nuevo = new NodoSintactico("IMPORT", contadorNodos++);
                            recorrerArbol(nuevo, padreIrony.ChildNodes[0]);
                            padreSintactico.addHijo(nuevo);
                            valor = padreIrony.ChildNodes[1].Token.ToString().Replace("\"", "");
                            valor = valor.Replace(" (cadena)", "");
                            nuevo2 = new NodoSintactico("IMPORT", contadorNodos++);
                            nuevo2.setValor(valor);
                            nuevo.addHijo(nuevo2);
                        }
                        else
                        {
                            nuevo = new NodoSintactico("IMPORT", contadorNodos++);
                            recorrerArbol(padreSintactico, padreIrony.ChildNodes[0]);
                            valor = padreIrony.ChildNodes[1].Token.ToString().Replace("\"", "");
                            valor = valor.Replace(" (cadena)", "");
                            nuevo.setValor(valor);
                            padreSintactico.addHijo(nuevo);
                        }
                    }
                    else
                    {
                        nuevo = new NodoSintactico("IMPORT", contadorNodos++);
                        valor = padreIrony.ChildNodes[0].Token.ToString().Replace("\"", "");
                        valor = valor.Replace(" (cadena)", "");
                        nuevo.setValor(valor);
                        padreSintactico.addHijo(nuevo);
                    }
                    break;
                case "MAIN":
                    nuevo = new NodoSintactico("MAIN", contadorNodos++);
                    padreSintactico.addHijo(nuevo);
                    foreach (ParseTreeNode hijo in padreIrony.ChildNodes)
                    {
                        recorrerArbol(nuevo, hijo);
                    }
                    break;
                #endregion

                #region listas: instrucciones, declaraciones, parametros
                case "L_INSTRUCCIONES":
                    if (padreIrony.ChildNodes.Count > 1)
                    {
                        if (!padreSintactico.getNombre().Equals("INSTRUCCION"))
                        {
                            //DATO PRIMERO
                            nuevo = new NodoSintactico("INSTRUCCION", contadorNodos++);
                            padreSintactico.addHijo(nuevo);
                            recorrerArbol(nuevo, padreIrony.ChildNodes[0]);
                            recorrerArbol(nuevo, padreIrony.ChildNodes[1]);
                        }
                        else
                        {
                            //DATO INTERMEDIO
                            recorrerArbol(padreSintactico, padreIrony.ChildNodes[0]);
                            recorrerArbol(padreSintactico, padreIrony.ChildNodes[1]);
                        }
                    }
                    else
                    {
                        //DATO ULTIMO
                        if (!padreSintactico.getNombre().Equals("INSTRUCCION"))
                        {
                            nuevo = new NodoSintactico("INSTRUCCION", contadorNodos++);
                            padreSintactico.addHijo(nuevo);
                            recorrerArbol(nuevo, padreIrony.ChildNodes[0]);
                        }
                        else
                        {
                            recorrerArbol(padreSintactico, padreIrony.ChildNodes[0]);
                        }
                    }
                    break;
                case "L_DECLARACIONES":
                    if (padreIrony.ChildNodes.Count > 1)
                    {
                        if (!padreSintactico.getNombre().Equals("DECLARACION"))
                        {
                            //DATO PRIMERO
                            nuevo = new NodoSintactico("DECLARACION", contadorNodos++);
                            padreSintactico.addHijo(nuevo);
                            recorrerArbol(nuevo, padreIrony.ChildNodes[0]);
                            recorrerArbol(nuevo, padreIrony.ChildNodes[1]);
                        }
                        else
                        {
                            //DATO INTERMEDIO
                            recorrerArbol(padreSintactico, padreIrony.ChildNodes[0]);
                            recorrerArbol(padreSintactico, padreIrony.ChildNodes[1]);
                        }
                    }
                    else
                    {
                        //DATO ULTIMO
                        if (!padreSintactico.getNombre().Equals("DECLARACION"))
                        {
                            nuevo = new NodoSintactico("DECLARACION", contadorNodos++);
                            padreSintactico.addHijo(nuevo);
                            recorrerArbol(nuevo, padreIrony.ChildNodes[0]);
                        }
                        else
                        {
                            recorrerArbol(padreSintactico, padreIrony.ChildNodes[0]);
                        }
                    }
                    break;
                case "L_PARAMETROS":
                    if (padreIrony.ChildNodes.Count > 1)
                    {
                        if (!padreSintactico.getNombre().Equals("PARAMETROS"))
                        {
                            //DATO PRIMERO
                            nuevo = new NodoSintactico("PARAMETROS", contadorNodos++);
                            padreSintactico.addHijo(nuevo);
                            recorrerArbol(nuevo, padreIrony.ChildNodes[0]);
                            recorrerArbol(nuevo, padreIrony.ChildNodes[1]);
                        }
                        else
                        {
                            //DATO INTERMEDIO
                            recorrerArbol(padreSintactico, padreIrony.ChildNodes[0]);
                            recorrerArbol(padreSintactico, padreIrony.ChildNodes[1]);
                        }
                    }
                    else
                    {
                        //DATO ULTIMO
                        if (!padreSintactico.getNombre().Equals("PARAMETROS"))
                        {
                            nuevo = new NodoSintactico("PARAMETROS", contadorNodos++);
                            padreSintactico.addHijo(nuevo);
                            recorrerArbol(nuevo, padreIrony.ChildNodes[0]);
                        }
                        else
                        {
                            recorrerArbol(padreSintactico, padreIrony.ChildNodes[0]);
                        }
                    }
                    break;
                #endregion

                #region lineal, arreglos: dim1, dim2 y dim3
                case "LINEAL":
                    nuevo = new NodoSintactico("DIM0", contadorNodos++);
                    foreach (ParseTreeNode hijo in padreIrony.ChildNodes)
                    {
                        recorrerArbol(nuevo, hijo);
                    }
                    padreSintactico.addHijo(nuevo);
                    break;
                case "ARREGLO":
                    recorrerArbol(padreSintactico, padreIrony.ChildNodes[0]);
                    break;
                case "DIM1":
                    nuevo = new NodoSintactico("DIM1", contadorNodos++);
                    padreSintactico.addHijo(nuevo);
                    foreach (ParseTreeNode hijo in padreIrony.ChildNodes)
                    {
                        recorrerArbol(nuevo, hijo);
                    }
                    break;
                case "DIM2":
                    nuevo = new NodoSintactico("DIM2", contadorNodos++);
                    padreSintactico.addHijo(nuevo);
                    foreach (ParseTreeNode hijo in padreIrony.ChildNodes)
                    {
                        recorrerArbol(nuevo, hijo);
                    }
                    break;
                case "DIM3":
                    nuevo = new NodoSintactico("DIM3", contadorNodos++);
                    padreSintactico.addHijo(nuevo);
                    foreach (ParseTreeNode hijo in padreIrony.ChildNodes)
                    {
                        recorrerArbol(nuevo, hijo);
                    }
                    break;
                case "I_DIM":
                    if (padreIrony.ChildNodes.Count > 1)
                    {
                        if (!padreSintactico.getNombre().Equals("CONJUNTO_D1"))
                        {
                            //DATO PRIMERO
                            nuevo = new NodoSintactico("CONJUNTO_D1", contadorNodos++);
                            padreSintactico.addHijo(nuevo);
                            recorrerArbol(nuevo, padreIrony.ChildNodes[0]);
                            recorrerArbol(nuevo, padreIrony.ChildNodes[1]);
                        }
                        else
                        {
                            //DATO INTERMEDIO
                            recorrerArbol(padreSintactico, padreIrony.ChildNodes[0]);
                            recorrerArbol(padreSintactico, padreIrony.ChildNodes[1]);
                        }
                    }
                    else
                    {
                        //DATO ULTIMO
                        if (!padreSintactico.getNombre().Equals("CONJUNTO_D1"))
                        {
                            nuevo = new NodoSintactico("CONJUNTO_D1", contadorNodos++);
                            padreSintactico.addHijo(nuevo);
                            recorrerArbol(nuevo, padreIrony.ChildNodes[0]);
                        }
                        else
                        {
                            recorrerArbol(padreSintactico, padreIrony.ChildNodes[0]);
                        }
                    }
                    break;
                case "E_DIM":
                    if (padreIrony.ChildNodes.Count > 1)
                    {
                        if (!padreSintactico.getNombre().Equals("CONJUNTO_D2"))
                        {
                            //DATO PRIMERO
                            nuevo = new NodoSintactico("CONJUNTO_D2", contadorNodos++);
                            padreSintactico.addHijo(nuevo);
                            recorrerArbol(nuevo, padreIrony.ChildNodes[0]);
                            recorrerArbol(nuevo, padreIrony.ChildNodes[1]);
                        }
                        else
                        {
                            //DATO INTERMEDIO
                            recorrerArbol(padreSintactico, padreIrony.ChildNodes[0]);
                            recorrerArbol(padreSintactico, padreIrony.ChildNodes[1]);
                        }
                    }
                    else
                    {
                        //DATO ULTIMO
                        if (!padreSintactico.getNombre().Equals("CONJUNTO_D2"))
                        {
                            nuevo = new NodoSintactico("CONJUNTO_D2", contadorNodos++);
                            padreSintactico.addHijo(nuevo);
                            recorrerArbol(nuevo, padreIrony.ChildNodes[0]);
                        }
                        else
                        {
                            recorrerArbol(padreSintactico, padreIrony.ChildNodes[0]);
                        }
                    }
                    break;
                case "E_DIM1":
                    if (padreIrony.ChildNodes.Count > 1)
                    {
                        if (!padreSintactico.getNombre().Equals("CONJUNTO_D3"))
                        {
                            //DATO PRIMERO
                            nuevo = new NodoSintactico("CONJUNTO_D3", contadorNodos++);
                            padreSintactico.addHijo(nuevo);
                            recorrerArbol(nuevo, padreIrony.ChildNodes[0]);
                            recorrerArbol(nuevo, padreIrony.ChildNodes[1]);
                        }
                        else
                        {
                            //DATO INTERMEDIO
                            recorrerArbol(padreSintactico, padreIrony.ChildNodes[0]);
                            recorrerArbol(padreSintactico, padreIrony.ChildNodes[1]);
                        }
                    }
                    else
                    {
                        //DATO ULTIMO
                        if (!padreSintactico.getNombre().Equals("CONJUNTO_D3"))
                        {
                            nuevo = new NodoSintactico("CONJUNTO_D3", contadorNodos++);
                            padreSintactico.addHijo(nuevo);
                            recorrerArbol(nuevo, padreIrony.ChildNodes[0]);
                        }
                        else
                        {
                            recorrerArbol(padreSintactico, padreIrony.ChildNodes[0]);
                        }
                    }
                    break;
                #endregion

                #region Expresion, dato, EOTRO
                case "EXPRESION":
                    if (padreIrony.ChildNodes.Count == 3)
                    {
                        nuevo = buscarOperacion(padreSintactico, padreIrony.ChildNodes[1]);
                        recorrerArbol(nuevo, padreIrony.ChildNodes[0]);
                        recorrerArbol(nuevo, padreIrony.ChildNodes[2]);
                        padreSintactico.addHijo(nuevo);
                    }
                    else if (padreIrony.ChildNodes.Count == 2)
                    {
                        if (padreIrony.ChildNodes[1].ToString().Equals("EXPRESION"))
                        {
                            if (padreIrony.ChildNodes[0].ToString().Equals("- (Key symbol)"))
                            {
                                nuevo = new NodoSintactico("NEGATIVO", contadorNodos++);
                                recorrerArbol(nuevo, padreIrony.ChildNodes[1]);
                                padreSintactico.addHijo(nuevo);
                            }
                            else
                            {
                                nuevo = new NodoSintactico("NOT", contadorNodos++);
                                recorrerArbol(nuevo, padreIrony.ChildNodes[1]);
                                padreSintactico.addHijo(nuevo);
                            }
                        }
                        else
                        {
                            nuevo = buscarOperacion(padreSintactico, padreIrony.ChildNodes[1]);
                            recorrerArbol(nuevo, padreIrony.ChildNodes[0]);
                            padreSintactico.addHijo(nuevo);
                        }
                    }
                    else
                    {
                        foreach (ParseTreeNode hijo in padreIrony.ChildNodes)
                        {
                            recorrerArbol(padreSintactico, hijo);
                        }
                    }
                    break;
                case "DATO":
                    if (padreIrony.ChildNodes.Count > 1)
                    {
                        nuevo = new NodoSintactico("CAMPOCLASE", contadorNodos++);
                        padreSintactico.addHijo(nuevo);
                        recorrerArbol(nuevo, padreIrony.ChildNodes[0]);
                        recorrerArbol(nuevo, padreIrony.ChildNodes[1]);
                    }
                    else
                    {
                        recorrerArbol(padreSintactico, padreIrony.ChildNodes[0]);
                    }
                    break;
                case "INSTRUCCION":
                    foreach (ParseTreeNode hijo in padreIrony.ChildNodes)
                    {
                        recorrerArbol(padreSintactico, hijo);
                    }
                    break;
                case "EOTRO":
                    nuevo = new NodoSintactico("EOTRO", contadorNodos++);
                    padreSintactico.addHijo(nuevo);
                    foreach (ParseTreeNode hijo in padreIrony.ChildNodes)
                    {
                        recorrerArbol(nuevo, hijo);
                    }
                    break;
                #endregion

                #region log, alert graph
                case "LOG":
                    nuevo = new NodoSintactico("LOG", contadorNodos++);
                    padreSintactico.addHijo(nuevo);
                    foreach (ParseTreeNode hijo in padreIrony.ChildNodes)
                    {
                        recorrerArbol(nuevo, hijo);
                    }
                    break;
                case "ALERT":
                    nuevo = new NodoSintactico("ALERT", contadorNodos++);
                    padreSintactico.addHijo(nuevo);
                    foreach (ParseTreeNode hijo in padreIrony.ChildNodes)
                    {
                        recorrerArbol(nuevo, hijo);
                    }
                    break;
                case "GRAPH":
                    nuevo = new NodoSintactico("GRAPH", contadorNodos++);
                    padreSintactico.addHijo(nuevo);
                    foreach (ParseTreeNode hijo in padreIrony.ChildNodes)
                    {
                        recorrerArbol(nuevo, hijo);
                    }
                    break;
                #endregion

                #region if, else if, else
                case "IF":
                    nuevo = new NodoSintactico("IF", contadorNodos++);
                    padreSintactico.addHijo(nuevo);
                    foreach (ParseTreeNode hijo in padreIrony.ChildNodes)
                    {
                        recorrerArbol(nuevo, hijo);
                    }
                    break;
                case "ELSEIF":
                    nuevo = new NodoSintactico("ELSE", contadorNodos++);
                    padreSintactico.addHijo(nuevo);
                    foreach (ParseTreeNode hijo in padreIrony.ChildNodes)
                    {
                        recorrerArbol(nuevo, hijo);
                    }
                    break;
                case "ELSE":
                    nuevo = new NodoSintactico("ELSE", contadorNodos++);
                    padreSintactico.addHijo(nuevo);
                    foreach (ParseTreeNode hijo in padreIrony.ChildNodes)
                    {
                        recorrerArbol(nuevo, hijo);
                    }
                    break;
                #endregion

                #region switch, case, default
                case "SWITCH":
                    nuevo = new NodoSintactico("SWITCH", contadorNodos++);
                    padreSintactico.addHijo(nuevo);
                    foreach (ParseTreeNode hijo in padreIrony.ChildNodes)
                    {
                        recorrerArbol(nuevo, hijo);
                    }
                    break;
                case "CASE":
                    nuevo = new NodoSintactico("CASE", contadorNodos++);
                    foreach (ParseTreeNode hijo in padreIrony.ChildNodes)
                    {
                        if (!hijo.ToString().Equals("DEFAULT"))
                        {
                            recorrerArbol(nuevo, hijo);
                        }
                        else
                        {
                            recorrerArbol(padreSintactico, hijo);
                        }
                    }
                    padreSintactico.addHijo(nuevo);
                    break;
                case "DEFAULT":
                    nuevo = new NodoSintactico("DEFAULT", contadorNodos++);
                    padreSintactico.addHijo(nuevo);
                    foreach (ParseTreeNode hijo in padreIrony.ChildNodes)
                    {
                        recorrerArbol(nuevo, hijo);
                    }
                    break;
                case "L_CASE":
                    if (padreIrony.ChildNodes.Count > 1)
                    {
                        if (!padreSintactico.getNombre().Equals("L_CASE"))
                        {
                            //DATO PRIMERO
                            nuevo = new NodoSintactico("L_CASE", contadorNodos++);
                            padreSintactico.addHijo(nuevo);
                            recorrerArbol(nuevo, padreIrony.ChildNodes[0]);
                            recorrerArbol(nuevo, padreIrony.ChildNodes[1]);
                        }
                        else
                        {
                            //DATO INTERMEDIO
                            recorrerArbol(padreSintactico, padreIrony.ChildNodes[0]);
                            recorrerArbol(padreSintactico, padreIrony.ChildNodes[1]);
                        }
                    }
                    else
                    {
                        //DATO ULTIMO
                        if (!padreSintactico.getNombre().Equals("L_CASE"))
                        {
                            nuevo = new NodoSintactico("L_CASE", contadorNodos++);
                            padreSintactico.addHijo(nuevo);
                            recorrerArbol(nuevo, padreIrony.ChildNodes[0]);
                        }
                        else
                        {
                            recorrerArbol(padreSintactico, padreIrony.ChildNodes[0]);
                        }
                    }
                    break;
                #endregion

                #region while, do while, for
                case "WHILE":
                    nuevo = new NodoSintactico("WHILE", contadorNodos++);
                    padreSintactico.addHijo(nuevo);
                    foreach (ParseTreeNode hijo in padreIrony.ChildNodes)
                    {
                        recorrerArbol(nuevo, hijo);
                    }
                    break;
                case "FOR":
                    nuevo = new NodoSintactico("FOR", contadorNodos++);
                    padreSintactico.addHijo(nuevo);
                    foreach (ParseTreeNode hijo in padreIrony.ChildNodes)
                    {
                        recorrerArbol(nuevo, hijo);
                    }
                    break;
                case "OPCIONESFOR":
                    nuevo = new NodoSintactico("OPCIONESFOR", contadorNodos++);
                    padreSintactico.addHijo(nuevo);
                    foreach (ParseTreeNode hijo in padreIrony.ChildNodes)
                    {
                        recorrerArbol(nuevo, hijo);
                    }
                    break;
                case "CONTROLFOR":
                    foreach (ParseTreeNode hijo in padreIrony.ChildNodes)
                    {
                        recorrerArbol(padreSintactico, hijo);
                    }
                    break;
                case "CONDICIONFOR":
                    foreach (ParseTreeNode hijo in padreIrony.ChildNodes)
                    {
                        recorrerArbol(padreSintactico, hijo);
                    }
                    break;
                case "UPDATEFOR":
                    foreach (ParseTreeNode hijo in padreIrony.ChildNodes)
                    {
                        recorrerArbol(padreSintactico, hijo);
                    }
                    break;
                case "DOWHILE":
                    nuevo = new NodoSintactico("DOWHILE", contadorNodos++);
                    padreSintactico.addHijo(nuevo);
                    foreach (ParseTreeNode hijo in padreIrony.ChildNodes)
                    {
                        recorrerArbol(nuevo, hijo);
                    }
                    break;
                #endregion

                #region metodo, funcion, clase, return
                case "METODO":
                    nuevo = new NodoSintactico("METODO", contadorNodos++);
                    padreSintactico.addHijo(nuevo);
                    foreach (ParseTreeNode hijo in padreIrony.ChildNodes)
                    {
                        recorrerArbol(nuevo, hijo);
                    }
                    break;
                case "FUNCION":
                    nuevo = new NodoSintactico("FUNCION", contadorNodos++);
                    padreSintactico.addHijo(nuevo);
                    foreach (ParseTreeNode hijo in padreIrony.ChildNodes)
                    {
                        recorrerArbol(nuevo, hijo);
                    }
                    break;
                case "CLASE":
                    nuevo = new NodoSintactico("CLASE", contadorNodos++);
                    padreSintactico.addHijo(nuevo);
                    foreach (ParseTreeNode hijo in padreIrony.ChildNodes)
                    {
                        recorrerArbol(nuevo, hijo);
                    }
                    break;
                case "PARAMETROS":
                    if (padreIrony.ChildNodes.Count > 1)
                    {
                        if (!padreSintactico.getNombre().Equals("PARAMETROS"))
                        {
                            //DATO PRIMERO
                            nuevo = new NodoSintactico("PARAMETROS", contadorNodos++);
                            padreSintactico.addHijo(nuevo);
                            recorrerArbol(nuevo, padreIrony.ChildNodes[0]);
                            recorrerArbol(nuevo, padreIrony.ChildNodes[1]);
                        }
                        else
                        {
                            //DATO INTERMEDIO
                            recorrerArbol(padreSintactico, padreIrony.ChildNodes[0]);
                            recorrerArbol(padreSintactico, padreIrony.ChildNodes[1]);
                        }
                    }
                    else
                    {
                        //DATO ULTIMO
                        if (!padreSintactico.getNombre().Equals("PARAMETROS"))
                        {
                            nuevo = new NodoSintactico("PARAMETROS", contadorNodos++);
                            padreSintactico.addHijo(nuevo);
                            recorrerArbol(nuevo, padreIrony.ChildNodes[0]);
                        }
                        else
                        {
                            recorrerArbol(padreSintactico, padreIrony.ChildNodes[0]);
                        }
                    }
                    break;
                case "RETURN":
                    nuevo = new NodoSintactico("RETURN", contadorNodos++);
                    padreSintactico.addHijo(nuevo);
                    foreach (ParseTreeNode hijo in padreIrony.ChildNodes)
                    {
                        recorrerArbol(nuevo, hijo);
                    }
                    break;
                #endregion

                #region reasignacion
                case "REASIGNACION":
                    nuevo = new NodoSintactico("REASIGNACION", contadorNodos++);
                    padreSintactico.addHijo(nuevo);
                    foreach (ParseTreeNode hijo in padreIrony.ChildNodes)
                    {
                        recorrerArbol(nuevo, hijo);
                    }
                    break;
                #endregion

                #region default: entero, doble, cadena, caracter, boleano, id, operadores, break, continue
                default:
                    if (padreIrony.Token != null)
                    {
                        string token = padreIrony.Token.ToString();
                        if (token.EndsWith(" (entero)"))
                        {
                            nuevo = new NodoSintactico("ENTERO", contadorNodos++);
                            valor = padreIrony.Token.ToString().Replace("\"", "");
                            valor = valor.Replace(" (entero)", "");
                            nuevo.setValor(valor);
                            padreSintactico.addHijo(nuevo);
                        }
                        else if (token.EndsWith(" (doble)"))
                        {
                            nuevo = new NodoSintactico("DOBLE", contadorNodos++);
                            valor = padreIrony.Token.ToString().Replace("\"", "");
                            valor = valor.Replace(" (doble)", "");
                            nuevo.setValor(valor);
                            padreSintactico.addHijo(nuevo);
                        }
                        else if (token.EndsWith(" (cadena)"))
                        {
                            nuevo = new NodoSintactico("CADENA", contadorNodos++);
                            valor = padreIrony.Token.ToString();
                            valor = valor.Replace(" (cadena)", "");
                            valor = valor.Substring(0, valor.Length);
                            valor = valor.Replace("\\\"", "\"");
                            nuevo.setValor(valor);
                            padreSintactico.addHijo(nuevo);
                        }
                        else if (token.EndsWith(" (caracter)"))
                        {
                            nuevo = new NodoSintactico("CARACTER", contadorNodos++);
                            valor = padreIrony.Token.ToString().Replace("\"", "");
                            valor = valor.Replace(" (caracter)", "");
                            nuevo.setValor(valor);
                            padreSintactico.addHijo(nuevo);
                        }
                        else if (token.EndsWith(" (boleano)"))
                        {
                            nuevo = new NodoSintactico("BOLEANO", contadorNodos++);
                            valor = padreIrony.Token.ToString().Replace("\"", "");
                            valor = valor.Replace(" (boleano)", "");
                            nuevo.setValor(valor);
                            padreSintactico.addHijo(nuevo);
                        }
                        else if (token.EndsWith(" (id)"))
                        {
                            nuevo = new NodoSintactico("ID", contadorNodos++);
                            valor = padreIrony.Token.ToString().Replace("\"", "");
                            valor = valor.Replace(" (id)", "");
                            nuevo.setValor(valor);
                            padreSintactico.addHijo(nuevo);
                        }
                        if (token.Equals("* (Key symbol)"))
                        {
                            nuevo = new NodoSintactico("MULTIPLICACION", contadorNodos++);
                            valor = "*";
                            nuevo.setValor(valor);
                            padreSintactico.addHijo(nuevo);
                        }
                        else if (token.Equals("+ (Key symbol)"))
                        {
                            nuevo = new NodoSintactico("SUMA", contadorNodos++);
                            valor = "+";
                            nuevo.setValor(valor);
                            padreSintactico.addHijo(nuevo);
                        }
                        else if (token.Equals("/ (Key symbol)"))
                        {
                            nuevo = new NodoSintactico("DIVISION", contadorNodos++);
                            valor = "/";
                            nuevo.setValor(valor);
                            padreSintactico.addHijo(nuevo);
                        }
                        else if (token.Equals("- (Key symbol)"))
                        {
                            nuevo = new NodoSintactico("RESTA", contadorNodos++);
                            valor = "-";
                            nuevo.setValor(valor);
                            padreSintactico.addHijo(nuevo);
                        }
                        else if (token.Equals("pow (Keyword)"))
                        {
                            nuevo = new NodoSintactico("POTENCIA", contadorNodos++);
                            valor = "pow";
                            nuevo.setValor(valor);
                            padreSintactico.addHijo(nuevo);
                        }
                        else if (token.Equals("null (Keyword)"))
                        {
                            nuevo = new NodoSintactico("NULO", contadorNodos++);
                            valor = "pow";
                            nuevo.setValor(valor);
                            padreSintactico.addHijo(nuevo);
                        }
                        else if (token.Equals("&& (Key symbol)"))
                        {
                            nuevo = new NodoSintactico("AND", contadorNodos++);
                            valor = "&&";
                            nuevo.setValor(valor);
                            padreSintactico.addHijo(nuevo);
                        }
                        else if (token.Equals("|| (Key symbol)"))
                        {
                            nuevo = new NodoSintactico("OR", contadorNodos++);
                            valor = "||";
                            nuevo.setValor(valor);
                            padreSintactico.addHijo(nuevo);
                        }
                        else if (token.Equals("^ (Key symbol)"))
                        {
                            nuevo = new NodoSintactico("XOR", contadorNodos++);
                            valor = "^";
                            nuevo.setValor(valor);
                            padreSintactico.addHijo(nuevo);
                        }
                        else if (token.Equals("! (Key symbol)"))
                        {
                            nuevo = new NodoSintactico("NOT", contadorNodos++);
                            valor = "!";
                            nuevo.setValor(valor);
                            padreSintactico.addHijo(nuevo);
                        }
                        else if (token.Equals("> (Key symbol)"))
                        {
                            nuevo = new NodoSintactico("MAYOR", contadorNodos++);
                            valor = ">";
                            nuevo.setValor(valor);
                            padreSintactico.addHijo(nuevo);
                        }
                        else if (token.Equals("< (Key symbol)"))
                        {
                            nuevo = new NodoSintactico("MENOR", contadorNodos++);
                            valor = "<";
                            nuevo.setValor(valor);
                            padreSintactico.addHijo(nuevo);
                        }
                        else if (token.Equals(">= (Key symbol)"))
                        {
                            nuevo = new NodoSintactico("MAYORIGUAL", contadorNodos++);
                            valor = ">=";
                            nuevo.setValor(valor);
                            padreSintactico.addHijo(nuevo);
                        }
                        else if (token.Equals("<= (Key symbol)"))
                        {
                            nuevo = new NodoSintactico("MENORIGUAL", contadorNodos++);
                            valor = ">=";
                            nuevo.setValor(valor);
                            padreSintactico.addHijo(nuevo);
                        }
                        else if (token.Equals("== (Key symbol)"))
                        {
                            nuevo = new NodoSintactico("IGUALIGUAL", contadorNodos++);
                            valor = "==";
                            nuevo.setValor(valor);
                            padreSintactico.addHijo(nuevo);
                        }
                        else if (token.Equals("<> (Key symbol)"))
                        {
                            nuevo = new NodoSintactico("DESIGUAL", contadorNodos++);
                            valor = "<>";
                            nuevo.setValor(valor);
                            padreSintactico.addHijo(nuevo);
                        }
                        else if (token.Equals("++ (Key symbol)"))
                        {
                            nuevo = new NodoSintactico("INCREMENTO", contadorNodos++);
                            valor = "++";
                            nuevo.setValor(valor);
                            padreSintactico.addHijo(nuevo);
                        }
                        else if (token.Equals("-- (Key symbol)"))
                        {
                            nuevo = new NodoSintactico("DECREMENTO", contadorNodos++);
                            valor = "--";
                            nuevo.setValor(valor);
                            padreSintactico.addHijo(nuevo);
                        }
                        else if (token.Equals("break (Keyword)"))
                        {
                            nuevo = new NodoSintactico("BREAK", contadorNodos++);
                            valor = "break";
                            nuevo.setValor(valor);
                            padreSintactico.addHijo(nuevo);
                        }
                        else if (token.Equals("continue (Keyword)"))
                        {
                            nuevo = new NodoSintactico("CONTINUE", contadorNodos++);
                            valor = "continue";
                            nuevo.setValor(valor);
                            padreSintactico.addHijo(nuevo);
                        }
                        else if (token.Equals("new (Keyword)"))
                        {
                            nuevo = new NodoSintactico("NEW", contadorNodos++);
                            valor = "new";
                            nuevo.setValor(valor);
                            padreSintactico.addHijo(nuevo);
                        }
                    }
                    break;
                    #endregion
            }
        }

        private NodoSintactico buscarOperacion(NodoSintactico padreSintactico, ParseTreeNode padreIrony)
        {
            NodoSintactico nuevo;
            string valor;
            if (padreIrony.Token != null)
            {
                string token = padreIrony.Token.ToString();
                if (token.EndsWith(" (entero)"))
                {
                    nuevo = new NodoSintactico("ENTERO", contadorNodos++);
                    valor = padreIrony.Token.ToString().Replace("\"", "");
                    valor = valor.Replace(" (entero)", "");
                    nuevo.setValor(valor);
                    return nuevo;
                }
                else if (token.EndsWith(" (doble)"))
                {
                    nuevo = new NodoSintactico("DOBLE", contadorNodos++);
                    valor = padreIrony.Token.ToString().Replace("\"", "");
                    valor = valor.Replace(" (doble)", "");
                    nuevo.setValor(valor);
                    return nuevo;
                }
                else if (token.EndsWith(" (cadena)"))
                {
                    nuevo = new NodoSintactico("CADENA", contadorNodos++);
                    valor = padreIrony.Token.ToString();
                    valor = valor.Replace(" (cadena)", "");
                    valor = valor.Substring(0, valor.Length);
                    valor = valor.Replace("\\\"", "\"");
                    nuevo.setValor(valor);
                    return nuevo;
                }
                else if (token.EndsWith(" (caracter)"))
                {
                    nuevo = new NodoSintactico("CARACTER", contadorNodos++);
                    valor = padreIrony.Token.ToString().Replace("\"", "");
                    valor = valor.Replace(" (caracter)", "");
                    nuevo.setValor(valor);
                    return nuevo;
                }
                else if (token.EndsWith(" (boleano)"))
                {
                    nuevo = new NodoSintactico("BOLEANO", contadorNodos++);
                    valor = padreIrony.Token.ToString().Replace("\"", "");
                    valor = valor.Replace(" (boleano)", "");
                    nuevo.setValor(valor);
                    return nuevo;
                }
                else if (token.EndsWith(" (id)"))
                {
                    nuevo = new NodoSintactico("ID", contadorNodos++);
                    valor = padreIrony.Token.ToString().Replace("\"", "");
                    valor = valor.Replace(" (id)", "");
                    nuevo.setValor(valor);
                    return nuevo;
                }
                if (token.Equals("* (Key symbol)"))
                {
                    nuevo = new NodoSintactico("MULTIPLICACION", contadorNodos++);
                    valor = "*";
                    nuevo.setValor(valor);
                    return nuevo;
                }
                else if (token.Equals("+ (Key symbol)"))
                {
                    nuevo = new NodoSintactico("SUMA", contadorNodos++);
                    valor = "+";
                    nuevo.setValor(valor);
                    return nuevo;
                }
                else if (token.Equals("/ (Key symbol)"))
                {
                    nuevo = new NodoSintactico("DIVISION", contadorNodos++);
                    valor = "/";
                    nuevo.setValor(valor);
                    return nuevo;
                }
                else if (token.Equals("- (Key symbol)"))
                {
                    nuevo = new NodoSintactico("RESTA", contadorNodos++);
                    valor = "-";
                    nuevo.setValor(valor);
                    return nuevo;
                }
                else if (token.Equals("pow (Keyword)"))
                {
                    nuevo = new NodoSintactico("POTENCIA", contadorNodos++);
                    valor = "pow";
                    nuevo.setValor(valor);
                    return nuevo;
                }
                else if (token.Equals("&& (Key symbol)"))
                {
                    nuevo = new NodoSintactico("AND", contadorNodos++);
                    valor = "&&";
                    nuevo.setValor(valor);
                    return nuevo;
                }
                else if (token.Equals("|| (Key symbol)"))
                {
                    nuevo = new NodoSintactico("OR", contadorNodos++);
                    valor = "||";
                    nuevo.setValor(valor);
                    return nuevo;
                }
                else if (token.Equals("^ (Key symbol)"))
                {
                    nuevo = new NodoSintactico("XOR", contadorNodos++);
                    valor = "^";
                    nuevo.setValor(valor);
                    return nuevo;
                }
                else if (token.Equals("! (Key symbol)"))
                {
                    nuevo = new NodoSintactico("NOT", contadorNodos++);
                    valor = "!";
                    nuevo.setValor(valor);
                    return nuevo;
                }
                else if (token.Equals("> (Key symbol)"))
                {
                    nuevo = new NodoSintactico("MAYOR", contadorNodos++);
                    valor = ">";
                    nuevo.setValor(valor);
                    return nuevo;
                }
                else if (token.Equals("< (Key symbol)"))
                {
                    nuevo = new NodoSintactico("MENOR", contadorNodos++);
                    valor = "<";
                    nuevo.setValor(valor);
                    return nuevo;
                }
                else if (token.Equals(">= (Key symbol)"))
                {
                    nuevo = new NodoSintactico("MAYORIGUAL", contadorNodos++);
                    valor = ">=";
                    nuevo.setValor(valor);
                    return nuevo;
                }
                else if (token.Equals("<= (Key symbol)"))
                {
                    nuevo = new NodoSintactico("MENORIGUAL", contadorNodos++);
                    valor = ">=";
                    nuevo.setValor(valor);
                    return nuevo;
                }
                else if (token.Equals("== (Key symbol)"))
                {
                    nuevo = new NodoSintactico("IGUALIGUAL", contadorNodos++);
                    valor = "==";
                    nuevo.setValor(valor);
                    return nuevo;
                }
                else if (token.Equals("<> (Key symbol)"))
                {
                    nuevo = new NodoSintactico("DESIGUAL", contadorNodos++);
                    valor = "<>";
                    nuevo.setValor(valor);
                    return nuevo;
                }
                else if (token.Equals("++ (Key symbol)"))
                {
                    nuevo = new NodoSintactico("INCREMENTO", contadorNodos++);
                    valor = "++";
                    nuevo.setValor(valor);
                    return nuevo;
                }
                else if (token.Equals("-- (Key symbol)"))
                {
                    nuevo = new NodoSintactico("DECREMENTO", contadorNodos++);
                    valor = "--";
                    nuevo.setValor(valor);
                    return nuevo;
                }
                else if (token.Equals("break (Keyword)"))
                {
                    nuevo = new NodoSintactico("BREAK", contadorNodos++);
                    valor = "break";
                    nuevo.setValor(valor);
                    return nuevo;
                }
                else if (token.Equals("continue (Keyword)"))
                {
                    nuevo = new NodoSintactico("CONTINUE", contadorNodos++);
                    valor = "continue";
                    nuevo.setValor(valor);
                    return nuevo;
                }
                else if (token.Equals("new (Keyword)"))
                {
                    nuevo = new NodoSintactico("NEW", contadorNodos++);
                    valor = "new";
                    nuevo.setValor(valor);
                    return nuevo;
                }
            }
            return null;
        }
    }
}