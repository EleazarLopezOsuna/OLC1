using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto1Compi12020.Modelos
{
    class ArbolExpresiones
    {
        public Node root { get; set; }
        public modelosThompson automata { get; }
        public List<Node> terminales { get; } 
        public List<Transicion> transiciones { get; }
        public List<Estados> estados { get; }

        List<int> recorridos;
        int contador = 1;

        public ArbolExpresiones(Lista elementos)
        {
            root = elementos.Head;
            insertarElementos(elementos, verificarHijos(elementos), root);
            recorridos = new List<int>();
            terminales = new List<Node>();
            transiciones = new List<Transicion>();
            automata = new modelosThompson();
            automata = crearThompson(root);
            buscarTerminales(root);
            recorridos = new List<int>();
            estados = new List<Estados>();
            transiciones = generarTransiciones(automata.nodoInicio);
            generarEstado(generarCerradura(automata.nodoInicio.numeroNodo));
        }

        public List<int> generarCerradura(int numeroEstado)
        {
            List<int> retorno = new List<int>();
            List<int> tmp;

            retorno.Add(numeroEstado);
            foreach (Transicion t in transiciones)
            {
                if (t.estadoInicial == numeroEstado && t.label.Equals("ε"))
                {
                    tmp = generarCerradura(t.estadoFinal);

                    foreach (int i in tmp)
                    {
                        if (!retorno.Contains(i))
                            retorno.Add(i);
                    }
                }
            }

            return retorno;
        }

        private void generarEstado(List<int> lista)
        {
            lista.Sort();
            String nombreEstado = "";
            foreach (int i in lista)
            {
                if (nombreEstado.Equals(""))
                {
                    nombreEstado = "" + i;
                }
                else
                {
                    nombreEstado += ", " + i;
                }
            }

            bool encontrado = false;

            foreach (Estados state in estados)
            {
                if (state.nombreEstado.Equals(nombreEstado))
                {
                    encontrado = true;
                    break;
                }
            }

            if (!encontrado)
            {
                Estados estado = new Estados(nombreEstado, (estados.Count + 1), esAceptacion(lista));
                estados.Add(estado);
                generarMover(estado, lista);
            }
        }

        private void generarMover(Estados estado, List<int> lista)
        {
            string nombreTerminal = "";
            foreach (Node node in terminales)
            {
                nombreTerminal = node.Token.Value;
                List<int> tmp = new List<int>();
                foreach (Transicion transicion in transiciones)
                {
                    if (transicion.label.Equals(nombreTerminal))
                    {
                        if (lista.Contains(transicion.estadoInicial))
                        {
                            if (!tmp.Contains(transicion.estadoFinal))
                            {
                                tmp.Add(transicion.estadoFinal);
                            }
                        }
                    }
                }

                List<int> nuevaCerradura = new List<int>();

                foreach (int i in tmp)
                {
                    nuevaCerradura = generarCerradura(i);
                }

                if(nuevaCerradura.Count != 0)
                    generarEstado(nuevaCerradura, estado, nombreTerminal);
            }
        }

        private void generarEstado(List<int> lista, Estados estadoAnterior, string nombreTerminal)
        {
            lista.Sort();
            String nombreEstado = "";
            foreach (int i in lista)
            {
                if (nombreEstado.Equals(""))
                {
                    nombreEstado = "" + i;
                }
                else
                {
                    nombreEstado += ", " + i;
                }
            }

            bool encontrado = false;

            Estados estado = null;
            foreach (Estados state in estados)
            {
                if (state.nombreEstado.Equals(nombreEstado))
                {
                    encontrado = true;
                    estado = state;
                    break;
                }
            }

            if (!encontrado)
            {
                estado = new Estados(nombreEstado, (estados.Count + 1), esAceptacion(lista));
                estadoAnterior.transiciones.Add(new Transicion(estadoAnterior.numeroEstado, estado.numeroEstado, nombreTerminal));
                estados.Add(estado);
                generarMover(estado, lista);
            }
            else
            {
                estadoAnterior.transiciones.Add(new Transicion(estadoAnterior.numeroEstado, estado.numeroEstado, nombreTerminal));
            }
        }

        private bool esAceptacion(List<int> lista)
        {
            if (lista.Contains(automata.nodoFinal.numeroNodo))
                return true;
            return false;
        }

        public class Estados
        {
            public bool esAceptacion { get; set; }
            public int numeroEstado { get; set; }
            public string nombreEstado { get; set; }
            public List<Transicion> transiciones { get; }

            public Estados(string nombreEstado, int numeroEstado, bool esAceptacion)
            {
                this.nombreEstado = nombreEstado;
                this.numeroEstado = numeroEstado;
                this.esAceptacion = esAceptacion;

                transiciones = new List<Transicion>();
            }
        }

        public List<Transicion> generarTransiciones(modelosThompson.nodoTh nodo)
        {
            List<Transicion> retorno = new List<Transicion>();
            List<Transicion> tmp;
            bool existe;

            recorridos.Add(nodo.numeroNodo);

            for (int i = 0; i < nodo.transiciones.Count; i++)
            {
                retorno.Add(new Transicion(nodo.numeroNodo, nodo.transiciones[i].nodo.numeroNodo, nodo.transiciones[i].cadena));

                if (!recorridos.Contains(nodo.transiciones[i].nodo.numeroNodo))
                {
                    tmp = generarTransiciones(nodo.transiciones[i].nodo);

                    foreach (Transicion t1 in tmp)
                    {
                        existe = false;
                        foreach (Transicion t2 in retorno)
                        {
                            if (t2.estadoInicial == t1.estadoInicial && t2.estadoFinal == t1.estadoFinal && t2.label.Equals(t1.label))
                            {
                                existe = true;
                                break;
                            }
                        }
                        if (!existe)
                        {
                            retorno.Add(t1);
                        }
                    }
                }
                
            }

            return retorno;
        }

        public void buscarTerminales(Node nodo)
        {
            if(nodo != null)
            {
                if(nodo.hijoIzquierdo == null)
                {
                    bool existe = false;

                    foreach(Node t in terminales)
                    {
                        if (nodo.Token.Name.Equals(t.Token.Name) && nodo.Token.Value.Equals(t.Token.Value))
                        {
                            existe = true;
                        }
                    }

                    if(!existe)
                    {
                        terminales.Add(nodo);
                    }
                }
                else
                {
                    buscarTerminales(nodo.hijoIzquierdo);
                    buscarTerminales(nodo.hijoDerecho);
                }
            }
        }

        public void insertarElementos(Lista tmp, int hijos, Node padre)
        {
            Node izquierdo, derecho;
            if(tmp.Head != null)
            {
                tmp.RemoveNode();
                if(tmp != null)
                {
                    switch (hijos)
                    {
                        case 1:
                            izquierdo = tmp.Head;
                            padre.hijoIzquierdo = izquierdo;
                            insertarElementos(tmp, verificarHijos(tmp), izquierdo);
                            break;
                        case 2:
                            izquierdo = tmp.Head;
                            padre.hijoIzquierdo = izquierdo;
                            insertarElementos(tmp, verificarHijos(tmp), izquierdo);
                            derecho = tmp.Head;
                            padre.hijoDerecho = derecho;
                            insertarElementos(tmp, verificarHijos(tmp), derecho);
                            break;
                    }
                }
            }
        }

        public int verificarHijos(Lista tmp)
        {
            if (tmp.Head.Token.Value.Equals(".") || tmp.Head.Token.Value.Equals("|"))
            {
                return 2;
            }
            if (tmp.Head.Token.Value.Equals("+") || tmp.Head.Token.Value.Equals("*") || tmp.Head.Token.Value.Equals("?"))
            {
                return 1;
            }
            return 0;
        }

        public string preOrden(Node tmp)
        {
            if(tmp != null)
            {
                if(tmp.hijoIzquierdo != null)
                {
                    if(tmp.hijoDerecho != null)
                    {
                        return tmp.Token.Value + " " + preOrden(tmp.hijoIzquierdo) + " " + preOrden(tmp.hijoDerecho);
                    }
                    else
                    {
                        return tmp.Token.Value + " " + preOrden(tmp.hijoIzquierdo);
                    }
                }
                else
                {
                    return tmp.Token.Value;
                }
            }
            return "";
        }

        public modelosThompson crearThompson(Node padre)
        {
            modelosThompson modelo1;
            modelosThompson modelo2;
            switch (padre.Token.Value)
            {
                case ".":
                    modelo1 = crearThompson(padre.hijoIzquierdo);
                    modelo2 = crearThompson(padre.hijoDerecho);
                    modelosThompson.interseccionTh interseccion = new modelosThompson.interseccionTh(modelo1, modelo2);

                    return interseccion;
                case "*":
                    modelo1 = crearThompson(padre.hijoIzquierdo);
                    modelosThompson.kleeneTh kleene = new modelosThompson.kleeneTh(modelo1);
                    contador += 2;
                    return kleene;
                case "+":
                    modelo1 = crearThompson(padre.hijoIzquierdo);
                    modelosThompson.positivaTh positiva = new modelosThompson.positivaTh(modelo1);
                    contador += 2;

                    return positiva;
                case "?":
                    modelo1 = crearThompson(padre.hijoIzquierdo);
                    modelosThompson.interrogacionTh interrogacion = new modelosThompson.interrogacionTh(modelo1);

                    return interrogacion;
                case "|":
                    modelo1 = crearThompson(padre.hijoIzquierdo);
                    modelo2 = crearThompson(padre.hijoDerecho);
                    modelosThompson.unionTh union = new modelosThompson.unionTh(contador, modelo1, modelo2);
                    contador += 2;

                    return union;
                default:
                    modelosThompson.terminalTh terminal = new modelosThompson.terminalTh(contador, padre.Token.Value);
                    contador += 2;

                    return terminal;
            }
        }

        public string recorrer(modelosThompson.nodoTh nodo)
        {
            string retorno = "";
            modelosThompson.nodoTh tmp;
            recorridos.Add(nodo.numeroNodo);
            for (int i = 0; i < nodo.transiciones.Count; i++)
            {
                tmp = nodo.transiciones[i].nodo;
                retorno += "nodo_" + nodo.numeroNodo + " -> nodo_" + tmp.numeroNodo + " [label = \"" + nodo.transiciones[i].cadena + "\"];\n";
                if (!recorridos.Contains(tmp.numeroNodo))
                {
                    retorno += recorrer(tmp);
                }
            }

            return retorno;
        }

        public string dibujar()
        {
            recorridos = new List<int>();
            String mostrar = "";
            mostrar += recorrer(automata.nodoInicio);
            foreach (int i in recorridos)
            {
                mostrar += "nodo_" + i + "[label = \"" + i + "\"];\n";
            }

            return mostrar;
        }

        public bool evaluarCadena(string cadena, int index, Dictionary<string, List<char>> conjuntos)
        {
            //System.Windows.Forms.MessageBox.Show(cadena);
            //Termino de evaluar, la cadena está vacia
            if (cadena.Length < 1)
            {
                //List<Estados> estados
                Estados etmp = estados.ElementAt(index);
                if (etmp.esAceptacion)
                {
                    return true; // Es un estado de aceptacion, la cadena es valida
                }
                //System.Windows.Forms.MessageBox.Show("Estado: " + index);
                return false; // No es un estado de aceptacion, la cadena es invalida
            }

            //La cadena aun no ha terminado, seguir evaluando

            //Guardamos el caracter y recortamos la cadena
            string caracter = Convert.ToString(cadena[0]);
            char character = cadena[0];
            cadena = cadena.Remove(0, 1);

            //Variable para verificar ambiguedad con los conjuntos
            int contadorEncontradas = 0;
            bool ejecutarConj = true;

            //Variable para saber donde se encontro la coincidencia
            string coincidencia = "";

            //Se obtiene el estado actual
            Estados state = estados.ElementAt(index);

            foreach (Transicion transicion in state.transiciones)
            {
                //La transicion es con [:TODO:] o con un conjunto
                if (transicion.label.Length > 1)
                {
                    //La transicion es con [:TODO:]
                    switch(transicion.label)
                    {
                        //El caracter es diferente al salto de linea -> hace match con [:TODO:]
                        case "[:todo:]":
                            if (!caracter.Equals("▀"))
                            {
                                coincidencia = "Todo";
                                contadorEncontradas++;
                                ejecutarConj = false;
                                //System.Windows.Forms.MessageBox.Show("TODO");
                            }
                            break;
                        case "\\\\n":
                            if (caracter.Equals("▀"))
                            {
                                coincidencia = "Salto";
                                contadorEncontradas++;
                                //System.Windows.Forms.MessageBox.Show("SALTO");
                            }
                            break;
                        case "\\\\t":
                            if (caracter.Equals("\t"))
                            {
                                coincidencia = "Tabulacion";
                                contadorEncontradas++;
                                //System.Windows.Forms.MessageBox.Show("TABULACION");
                            }
                            break;
                        case "\\\\'":
                            if (caracter.Equals("'"))
                            {
                                coincidencia = "Comilla";
                                contadorEncontradas++;
                                //System.Windows.Forms.MessageBox.Show("COMILLA");
                            }
                            break;
                        case "\\\\\"":
                            if (caracter.Equals("\""))
                            {
                                coincidencia = "Comilla Doble";
                                //System.Windows.Forms.MessageBox.Show("DOBLE");
                                contadorEncontradas++;
                            }
                            break;
                    }
                }
                else
                {
                    if (transicion.label.Equals(caracter))
                    {
                        coincidencia = "Transicion";
                        //System.Windows.Forms.MessageBox.Show("TRANSICION");
                        contadorEncontradas++;
                    }
                }
            }

            //Se busca coincidencias con conjuntos
            if (ejecutarConj)
            {
                foreach (string nombreConjunto in conjuntos.Keys)
                {
                    conjuntos.TryGetValue(nombreConjunto, out List<char> valor);

                    if (valor.Contains(character))
                    {
                        //System.Windows.Forms.MessageBox.Show("CONJUNTO");
                        coincidencia = "Conjunto";
                        contadorEncontradas++;
                    }
                }
            }
            if (contadorEncontradas == 1)
            {
                if (state.transiciones.Count == 0)
                    return false;
                foreach (Transicion transicion in state.transiciones)
                {
                    switch (coincidencia)
                    {
                        case "Transicion":
                            if (transicion.label.Equals(caracter))
                            {
                                return evaluarCadena(cadena, transicion.estadoFinal - 1, conjuntos);
                            }
                            break;
                        case "Conjunto":
                            foreach (string nombreConjunto in conjuntos.Keys)
                            {
                                conjuntos.TryGetValue(nombreConjunto, out List<char> valor);
                                //System.Windows.Forms.MessageBox.Show("LABEL: " + transicion.label + " CCONJUNTO: " + nombreConjunto);
                                if (transicion.label.Equals(nombreConjunto))
                                {
                                    if (valor.Contains(character))
                                    {
                                        return evaluarCadena(cadena, transicion.estadoFinal - 1, conjuntos);
                                    }
                                }
                            }
                            break;
                        case "Todo":
                            if (transicion.label.Equals("[:todo:]"))
                            {
                                return evaluarCadena(cadena, transicion.estadoFinal - 1, conjuntos);
                            }
                            break;
                        case "Salto":
                            if (transicion.label.Equals("\\\\n"))
                            {
                                return evaluarCadena(cadena, transicion.estadoFinal - 1, conjuntos);
                            }
                            break;
                        case "Tabulacion":
                            if (transicion.label.Equals("\\\\t"))
                            {
                                return evaluarCadena(cadena, transicion.estadoFinal - 1, conjuntos);
                            }
                            break;
                        case "Comilla":
                            if (transicion.label.Equals("\\\\'"))
                            {
                                return evaluarCadena(cadena, transicion.estadoFinal - 1, conjuntos);
                            }
                            break;
                        case "Comilla Doble":
                            if (transicion.label.Equals("\\\\\""))
                            {
                                return evaluarCadena(cadena, transicion.estadoFinal - 1, conjuntos);
                            }
                            break;
                    }
                }
                return false;
            }
            else if(contadorEncontradas == 0){
                return false;
            }

            System.Windows.Forms.MessageBox.Show("La expresion regular contiene ambiguedades, revisar valores en conjuntos");
            return false;
        }

        public class Transicion
        {
            public int estadoInicial { get; }
            public int estadoFinal { get; }
            public string label { get; }

            public Transicion(int estadoInicial, int estadoFinal, string label)
            {
                this.estadoInicial = estadoInicial;
                this.estadoFinal = estadoFinal;
                this.label = label;
            }
        } 

        public string crearTabla()
        {
            string retorno = "node[shape=plaintext];\nvacio[label=\"\", pos=\"0,0!\"];\n";
            string nodo = "";
            string trans = "";
            int contador = 1;
            foreach (Node node in terminales)
            {
                switch (node.Token.Value)
                {
                    case "[:todo:]":
                        nodo = "terminal_TODO[shape=record, label=\"" + node.Token.Value + "\", pos=\"" + contador + ",0!\"];\n";
                        break;
                    case "\\\\n":
                        nodo = "terminal_SALTO[shape=record, label=\"" + node.Token.Value + "\", pos=\"" + contador + ",0!\"];\n";
                        break;
                    case "\\\\'":
                        nodo = "terminal_COMILLA[shape=record, label=\"" + node.Token.Value + "\", pos=\"" + contador + ",0!\"];\n";
                        break;
                    case "\\\\\"":
                        nodo = "terminal_DOBLE[shape=record, label=\"" + node.Token.Value + "\", pos=\"" + contador + ",0!\"];\n";
                        break;
                    case "\\\\t":
                        nodo = "terminal_TABULACION[shape=record, label=\"" + node.Token.Value + "\", pos=\"" + contador + ",0!\"];\n";
                        break;
                    default:
                        nodo = "terminal_" + node.Token.Value + "[shape=record, label=\"" + node.Token.Value + "\", pos=\"" + contador + ",0!\"];\n";
                        break;
                }
                contador++;
                retorno += nodo;
            }

            foreach(Estados estado in estados)
            {
                if(estado.numeroEstado == 1)
                {
                    if (estado.esAceptacion)
                    {
                        nodo = "estado_" + estado.numeroEstado + "[shape = circle, color=green, style= filled, label=\"" + estado.numeroEstado + "\", pos=\"0," + (0 - estado.numeroEstado) + "!\"];\n";
                    }
                    else
                    {
                        nodo = "estado_" + estado.numeroEstado + "[shape = circle, label=\"" + estado.numeroEstado + "\", pos=\"0," + (0 - estado.numeroEstado) + "!\"];\n";
                    }
                }
                else
                {
                    if (estado.esAceptacion)
                    {
                        nodo = "estado_" + estado.numeroEstado + "[color=green, style= filled, label=\"" + estado.numeroEstado + "\", pos=\"0," + (0 - estado.numeroEstado) + "!\"];\n";
                    }
                    else
                    {
                        nodo = "estado_" + estado.numeroEstado + "[shape = record, label=\"" + estado.numeroEstado + "\", pos=\"0," + (0 - estado.numeroEstado) + "!\"];\n";
                    }
                }

                foreach(Transicion transicion in estado.transiciones)
                {
                    int posicionTerminal = 0;
                    contador = 1;
                    foreach (Node node in terminales)
                    {
                        if (node.Token.Value.Equals(transicion.label))
                        {
                            posicionTerminal = contador;
                            break;
                        }
                        contador++;
                    }
                    trans = "transicion_" + transicion.estadoInicial + "_" + transicion.estadoFinal + 
                        "[label=\"" + transicion.estadoFinal + "\", pos=\"" + contador + "," + (0 - transicion.estadoInicial) + "!\"];\n";
                    retorno += trans;
                }
                retorno += nodo;
            }


            return retorno;
        }

        public void crearImagenes(string nombre)
        {
            string inicioGrafica = "digraph G {\nrankdir = LR;\n"; ;
            string finGrafica = "}";
            string _1 = "";
            string _2 = "";
            string _3 = "";

            //Area para graficar el AFD
            _1 = inicioGrafica;
            foreach (Estados estado in estados)
            {
                foreach (Transicion transicion in estado.transiciones)
                {
                    
                    _1 += "S" + transicion.estadoInicial + " -> S" + transicion.estadoFinal + " [label = \"" + transicion.label + "\"];\n";
                }
                if (estado.esAceptacion)
                {
                    _1 += "S" + estado.numeroEstado + "[shape=doublecircle];\n";
                }
            }
            _1 += finGrafica;

            //Area para graficar el AFN
            _2 = inicioGrafica + dibujar() + finGrafica;

            //Area para graficar Transiciones
            _3 = inicioGrafica + crearTabla() + finGrafica;


            try
            {
                //Crear AFD
                string fileName = nombre + "_AFD";
                using (FileStream fs = File.Create(fileName + ".txt"))
                {
                    Byte[] info = new UTF8Encoding(true).GetBytes(_1);
                    fs.Write(info, 0, info.Length);
                }

                Process process = new Process();
                ProcessStartInfo startInfo = new ProcessStartInfo();
                startInfo.WindowStyle = ProcessWindowStyle.Hidden;
                startInfo.FileName = "cmd.exe";
                startInfo.Arguments = "/C dot -Tpng " + fileName + ".txt -o C:\\Users\\USER\\Desktop\\PruebasProyecto1\\Automatas\\" + nombre + ".png";
                process.StartInfo = startInfo;
                process.Start();

                //Crear AFN
                fileName = nombre + "_AFN";
                using (FileStream fs = File.Create(fileName + ".txt"))
                {
                    Byte[] info = new UTF8Encoding(true).GetBytes(_2);
                    fs.Write(info, 0, info.Length);
                }

                process = new Process();
                startInfo = new ProcessStartInfo();
                startInfo.WindowStyle = ProcessWindowStyle.Hidden;
                startInfo.FileName = "cmd.exe";
                startInfo.Arguments = "/C dot -Tpng " + fileName + ".txt -o C:\\Users\\USER\\Desktop\\PruebasProyecto1\\Thompson\\" + nombre + ".png";
                process.StartInfo = startInfo;
                process.Start();

                //Crear Tabla
                fileName = nombre + "_TABLE";
                using (FileStream fs = File.Create(fileName + ".txt"))
                {
                    Byte[] info = new UTF8Encoding(true).GetBytes(_3);
                    fs.Write(info, 0, info.Length);
                }

                process = new Process();
                startInfo = new ProcessStartInfo();
                startInfo.WindowStyle = ProcessWindowStyle.Hidden;
                startInfo.FileName = "cmd.exe";
                startInfo.Arguments = "/C neato -Tpng " + fileName + ".txt -o C:\\Users\\USER\\Desktop\\PruebasProyecto1\\Transiciones\\" + nombre + ".png";
                process.StartInfo = startInfo;
                process.Start();
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}
