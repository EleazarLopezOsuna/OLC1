/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package Objetos;

import Objetos.Arbol.Nodo;
import java.io.File;
import java.io.FileWriter;
import java.io.IOException;
import java.util.ArrayList;
import java.util.Collections;
import java.util.Map;

/**
 *
 * @author Eleazar Lopez <Universidad de San Carlos de Guatemala>
 */
public class Graficador {

    private String graficaArbol;
    private String graficaSiguientes;
    private String graficaTransiciones;
    private String graficaAutomata;
    private int auxiliarGraph;
    private ArrayList<ArrayList<String>> transicionesAutomata;
    private ArrayList<Estados> estados;
    private String rutaArbol;
    private String rutaSiguientes;
    private String rutaTransiciones;
    private String rutaAutomata;
    private String nombre;

    public Graficador(String nombre) {
        graficaArbol = "";
        graficaSiguientes = "";
        graficaTransiciones = "";
        graficaAutomata = "";
        auxiliarGraph = 0;
        transicionesAutomata = new ArrayList<>();
        estados = new ArrayList<>();
        rutaArbol = "";
        rutaSiguientes = "";
        rutaTransiciones = "";
        this.nombre = nombre;
    }

    public int getAuxiliarGraph() {
        return auxiliarGraph;
    }

    public void setAuxiliarGraph(int auxiliarGraph) {
        this.auxiliarGraph = auxiliarGraph;
    }

    public ArrayList<ArrayList<String>> getTransicionesAutomata() {
        return transicionesAutomata;
    }

    public void setTransicionesAutomata(ArrayList<ArrayList<String>> transicionesAutomata) {
        this.transicionesAutomata = transicionesAutomata;
    }

    public ArrayList<Estados> getEstados() {
        return estados;
    }

    public void setEstados(ArrayList<Estados> estados) {
        this.estados = estados;
    }

    public String getRutaArbol() {
        return rutaArbol;
    }

    public void setRutaArbol(String rutaArbol) {
        this.rutaArbol = rutaArbol;
    }

    public String getRutaSiguientes() {
        return rutaSiguientes;
    }

    public void setRutaSiguientes(String rutaSiguientes) {
        this.rutaSiguientes = rutaSiguientes;
    }

    public String getRutaTransiciones() {
        return rutaTransiciones;
    }

    public void setRutaTransiciones(String rutaTransiciones) {
        this.rutaTransiciones = rutaTransiciones;
    }

    public String getRutaAutomata() {
        return rutaAutomata;
    }

    public void setRutaAutomata(String rutaAutomata) {
        this.rutaAutomata = rutaAutomata;
    }

    public String getNombre() {
        return nombre;
    }

    public void setNombre(String nombre) {
        this.nombre = nombre;
    }

    public String getGraficaArbol() {
        return graficaArbol;
    }

    public void setGraficaArbol(String graficaArbol) {
        String inicio = "digraph{\n"
                + "node[shape=record];\n";
        String fin = "}";
        this.graficaArbol = inicio + graficaArbol + fin;
    }

    public String getGraficaSiguientes() {
        return graficaSiguientes;
    }

    public void setGraficaSiguientes(String graficaSiguientes) {
        this.graficaSiguientes = graficaSiguientes;
    }

    public String getGraficaTransiciones() {
        return graficaTransiciones;
    }

    public void setGraficaTransiciones(String graficaTransiciones) {
        this.graficaTransiciones = graficaTransiciones;
    }

    public String getGraficaAutomata() {
        return graficaAutomata;
    }

    public void setGraficaAutomata(String graficaAutomata) {
        this.graficaAutomata = graficaAutomata;
    }

    public String graficarAutomata() {
        String retorno = "digraph{\n"
                + "rankdir = LR;\n";
        for (ArrayList<String> datos : transicionesAutomata) {
            String estado = datos.get(0);
            Estados estadoAutomata = new Estados(estado);
            if (datos.size() == 1) {
                retorno += estado + "[shape=doublecircle];\n";
            } else {
                String key = datos.get(1);
                String movimiento = datos.get(2);
                if (!movimiento.isEmpty()) {
                    estadoAutomata.agregarMovimiento(key, movimiento);
                    retorno += estado + " -> " + movimiento + "[label=\"" + key + "\"];\n";
                }
            }
            estados.add(estadoAutomata);
        }
        return retorno + "}";
    }

    public String graficarTransiciones(ArrayList<Transicion> transiciones, Map<String, ArrayList<Object>> terminales, Map<String, String> estados, int aceptacion) {
        String retorno = "digraph{\n"
                + "tablaSiguientes[shape=none; margin=0, label = <\n"
                + "<TABLE BORDER = \"0\" CELLBORDER = \"1\" CELLSPACING = \"0\" CELLPADDING = \"4\">"
                + "<TR>\n"
                + "<TD ROWSPAN = \"2\"> Estado </TD>\n"
                + "<TD COLSPAN = \"" + (terminales.size() - 1) + "\"> Terminales </TD>\n"
                + "</TR>\n"
                + "<TR>\n";
        ArrayList<String> keys = new ArrayList<>();
        terminales.entrySet().forEach((terminal) -> {
            if (!terminal.getKey().equals("#")) {
                keys.add(terminal.getKey());
            }
        });
        for (String key : keys) {
            if (key.equals("> ")) {
                retorno += "<TD> MayorQue </TD>\n";
            } else if (key.equals("< ")) {
                retorno += "<TD> MenorQue </TD>\n";
            } else {
                retorno += "<TD> " + key + " </TD>\n";
            }
        }
        retorno += "</TR>\n";

        for (Transicion transicion : transiciones) {
            String estado = "";
            ArrayList<String> tempEstado = new ArrayList<>();
            for (Object obj : transicion.getEstado()) {
                tempEstado.add(obj.toString());
            }
            Collections.sort(tempEstado);
            boolean aceptado = false;
            for (String str : tempEstado) {
                if (str.equals(String.valueOf(aceptacion))) {
                    aceptado = true;
                }
                if (estado.isEmpty()) {
                    estado = str;
                } else {
                    estado += ", " + str;
                }
            }
            estado = estados.get(estado);
            retorno += "<TR>\n"
                    + "<TD> " + estado + " </TD>\n";

            for (Object key : keys) {
                if (key != "#") {
                    String movimiento = "";
                    Map<String, ArrayList<Object>> movimientos = transicion.getMovimientos();
                    if (movimientos.containsKey(key.toString())) {
                        ArrayList<Object> mov = movimientos.get(key.toString());
                        if (!mov.isEmpty()) {
                            for (Object obj : mov) {
                                if (movimiento.isEmpty()) {
                                    movimiento = obj.toString();
                                } else {
                                    movimiento += ", " + obj;
                                }
                            }
                        }
                    }
                    movimiento = estados.get(movimiento);
                    if (movimiento == null) {
                        movimiento = "";
                    }
                    ArrayList<String> tmp = new ArrayList<>();
                    tmp.add(estado);
                    tmp.add(key.toString());
                    tmp.add(movimiento);
                    transicionesAutomata.add(tmp);
                    retorno += "<TD> " + movimiento + " </TD>\n";
                }
            }
            ArrayList<String> tmp = new ArrayList<>();
            tmp.add(estado);
            if (aceptado) {
                transicionesAutomata.add(tmp);
            }
            retorno += "</TR>\n";
        }

        retorno += "</TABLE>\n"
                + ">];\n"
                + "}";
        return retorno;
    }

    public String graficarSiguientes(ArrayList<Item> siguientes) {
        String retorno = "digraph{\n"
                + "tablaSiguientes[shape=none; margin=0, label = <\n"
                + "<TABLE BORDER = \"0\" CELLBORDER = \"1\" CELLSPACING = \"0\" CELLPADDING = \"4\">"
                + "<TR>\n"
                + "<TD COLSPAN = \"2\"> Hoja </TD>\n"
                + "<TD ROWSPAN = \"2\"> Siguientes </TD>\n"
                + "</TR>\n"
                + "<TR>\n"
                + "<TD> Etiqueta </TD>\n"
                + "<TD> Numero </TD>\n"
                + "</TR>\n";
        for (Item item : siguientes) {
            String etiqueta = item.getEtiqueta();
            String numero = String.valueOf(item.getNumero());
            String next = "";
            ArrayList<String> temp = new ArrayList<>();
            for (Object obj : item.getSiguientes()) {
                temp.add(String.valueOf(obj));
            }
            Collections.sort(temp);
            for (String str : temp) {
                if (next.isEmpty()) {
                    next = str;
                } else {
                    next += ", " + str;
                }
            }
            if (etiqueta.equals("> ")) {
                retorno += "<TR>\n"
                    + "<TD> MayorQue </TD>\n"
                    + "<TD> " + numero + " </TD>\n"
                    + "<TD> " + next + " </TD>\n"
                    + "</TR>\n";
            } else if (etiqueta.equals("< ")) {
                retorno += "<TR>\n"
                    + "<TD> MenorQue </TD>\n"
                    + "<TD> " + numero + " </TD>\n"
                    + "<TD> " + next + " </TD>\n"
                    + "</TR>\n";
            } else {
                retorno += "<TR>\n"
                    + "<TD> " + etiqueta + " </TD>\n"
                    + "<TD> " + numero + " </TD>\n"
                    + "<TD> " + next + " </TD>\n"
                    + "</TR>\n";
            }
        }
        retorno += "</TABLE>\n"
                + ">];\n"
                + "}";
        return retorno;
    }

    public String graficarArbol(Nodo nodo) {
        String retorno = "";
        String raizGraph = "";
        String izquierdoGraph = "";
        String derechoGraph = "";
        String apuntadorGraph = "";
        String listaP = "";
        String listaU = "";
        if (nodo != null) {
            for (Object primero : nodo.getPrimeros()) {
                if (listaP.isEmpty()) {
                    listaP = primero.toString();
                } else {
                    listaP += ", " + primero.toString();
                }
            }
            for (Object ultimo : nodo.getUltimos()) {
                if (listaU.isEmpty()) {
                    listaU = ultimo.toString();
                } else {
                    listaU += ", " + ultimo.toString();
                }
            }
            raizGraph = "Nodo_" + nodo.getIdentificador() + "[label=\"" + listaP + " | {<lH>" + nodo.isAnulable2();
            if (nodo.getHijoIzquierdo() != null) {
                apuntadorGraph = "Nodo_" + nodo.getIdentificador() + ":lL -> " + "Nodo_" + nodo.getHijoIzquierdo().getIdentificador() + ":lH;\n";
                if (nodo.getHijoDerecho() != null) {
                    apuntadorGraph += "Nodo_" + nodo.getIdentificador() + ":lL -> " + "Nodo_" + nodo.getHijoDerecho().getIdentificador() + ":lH;\n";
                }
                raizGraph += "|{<lL>" + nodo.getValor() + "}}";
            } else {
                raizGraph += "|{" + nodo.getValor() + "}|{<lL>" + nodo.getNumero() + "}}";
            }
            raizGraph += " | " + listaU + "\"];\n";
            auxiliarGraph++;
            retorno = raizGraph + izquierdoGraph + derechoGraph + apuntadorGraph;
            return retorno + graficarArbol(nodo.getHijoIzquierdo()) + graficarArbol(nodo.getHijoDerecho());
        } else {
            return "";
        }
    }

    public void generarGraph(Nodo raiz, ArrayList<Item> siguientes, ArrayList<Transicion> transiciones, Map<String, ArrayList<Object>> terminales, Map<String, String> estados, int aceptacion) {
        graficaArbol = graficarArbol(raiz);
        graficaSiguientes = graficarSiguientes(siguientes);
        graficaTransiciones = graficarTransiciones(transiciones, terminales, estados, aceptacion);
        graficaAutomata = graficarAutomata();
    }

    public void generarArchivos() throws IOException {
        String path = "C:/Users/USER/Desktop/PruebaPractica1/";
        File fileArbol = new File(path + "Arboles/" + nombre + ".dot");
        if (fileArbol.exists()) {
            fileArbol.delete();
        }

        if (fileArbol.createNewFile()) {
            try (FileWriter myWriter = new FileWriter(fileArbol)) {
                String extra = "digraph{\n"
                        + "node[shape=record];\n";
                myWriter.write(extra + graficaArbol + "}");
                myWriter.close();
            }
        }

        String cmd = "dot -Tpng " + path + "Arboles/" + nombre + ".dot" + " -o " + path + "Arboles/" + nombre + ".png";
        Runtime.getRuntime().exec(cmd);

        File fileSiguiente = new File(path + "Siguientes/" + nombre + ".dot");

        if (fileSiguiente.exists()) {
            fileSiguiente.delete();
        }

        if (fileSiguiente.createNewFile()) {
            try (FileWriter myWriter = new FileWriter(fileSiguiente)) {
                myWriter.write(graficaSiguientes);
                myWriter.close();
            }
        }

        cmd = "dot -Tpng " + path + "Siguientes/" + nombre + ".dot" + " -o " + path + "Siguientes/" + nombre + ".png";
        Runtime.getRuntime().exec(cmd);

        File fileTransiciones = new File(path + "Transiciones/" + nombre + ".dot");

        if (fileTransiciones.exists()) {
            fileTransiciones.delete();
        }

        if (fileTransiciones.createNewFile()) {
            try (FileWriter myWriter = new FileWriter(fileTransiciones)) {
                myWriter.write(graficaTransiciones);
                myWriter.close();
            }
        }

        cmd = "dot -Tpng " + path + "Transiciones/" + nombre + ".dot" + " -o " + path + "Transiciones/" + nombre + ".png";
        Runtime.getRuntime().exec(cmd);

        File fileAutomata = new File(path + "Automatas/" + nombre + ".dot");

        if (fileAutomata.exists()) {
            fileAutomata.delete();
        }

        if (fileAutomata.createNewFile()) {
            try (FileWriter myWriter = new FileWriter(fileAutomata)) {
                myWriter.write(graficaAutomata);
                myWriter.close();
            }
        }

        cmd = "dot -Tpng " + path + "Automatas/" + nombre + ".dot" + " -o " + path + "Automatas/" + nombre + ".png";
        Runtime.getRuntime().exec(cmd);
    }

}
