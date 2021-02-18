/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package Objetos;

import java.io.IOException;
import java.util.ArrayList;
import java.util.Collections;
import java.util.HashMap;
import java.util.Map;

/**
 *
 * @author Eleazar Lopez <Universidad de San Carlos de Guatemala>
 */
public class Arbol {

    public class Nodo {

        private Nodo hijoIzquierdo;
        private Nodo hijoDerecho;
        private String valor;
        private int numero;
        private boolean anulable;
        private ArrayList<Object> primeros;
        private ArrayList<Object> ultimos;
        private boolean hoja;
        private int identificador;
        private boolean anulableCalculado;
        private boolean primeroCalculado;
        private boolean ultimoCalculado;
        private boolean siguienteCalculado;

        public Nodo(String valor) {
            this.valor = valor;
            hijoIzquierdo = null;
            hijoDerecho = null;
            hoja = true;
            numero = 0;
            identificador = 0;
            primeros = new ArrayList<>();
            ultimos = new ArrayList<>();
            anulable = false;
            anulableCalculado = false;
            primeroCalculado = false;
            ultimoCalculado = false;
            siguienteCalculado = false;
        }

        public boolean isAnulableCalculado() {
            return anulableCalculado;
        }

        public void setAnulableCalculado(boolean anulableCalculado) {
            this.anulableCalculado = anulableCalculado;
        }

        public Nodo getHijoIzquierdo() {
            return hijoIzquierdo;
        }

        public void setHijoIzquierdo(Nodo hijoIzquierdo) {
            this.hijoIzquierdo = hijoIzquierdo;
        }

        public Nodo getHijoDerecho() {
            return hijoDerecho;
        }

        public void setHijoDerecho(Nodo hijoDerecho) {
            this.hijoDerecho = hijoDerecho;
        }

        public String getValor() {
            return valor;
        }

        public void setValor(String valor) {
            this.valor = valor;
        }

        public int getNumero() {
            return numero;
        }

        public void setNumero(int numero) {
            this.numero = numero;
        }

        public boolean isAnulable() {
            return anulable;
        }

        public String isAnulable2() {
            if (anulable) {
                return "V";
            }
            return "F";
        }

        public void setAnulable(boolean anulable) {
            this.anulable = anulable;
        }

        public ArrayList<Object> getPrimeros() {
            return primeros;
        }

        public void setPrimeros(ArrayList<Object> primeros) {
            this.primeros = primeros;
        }

        public ArrayList<Object> getUltimos() {
            return ultimos;
        }

        public void setUltimos(ArrayList<Object> ultimos) {
            this.ultimos = ultimos;
        }

        public boolean isHoja() {
            return hoja;
        }

        public void setHoja(boolean hoja) {
            this.hoja = hoja;
        }

        public int getIdentificador() {
            return identificador;
        }

        public void setIdentificador(int identificador) {
            this.identificador = identificador;
        }

        public boolean isPrimeroCalculado() {
            return primeroCalculado;
        }

        public void setPrimeroCalculado(boolean primeroCalculado) {
            this.primeroCalculado = primeroCalculado;
        }

        public boolean isUltimoCalculado() {
            return ultimoCalculado;
        }

        public void setUltimoCalculado(boolean ultimoCalculado) {
            this.ultimoCalculado = ultimoCalculado;
        }

    }

    private Nodo raiz;
    private ArrayList<Simbolos> elementos;
    private int numeroX;
    private int auxiliarGraph;
    private ArrayList<Item> siguientes;
    private Map<String, ArrayList<Object>> terminales;
    private Map<String, String> estadosM;
    private ArrayList<Transicion> transiciones;
    private Graficador graficador;
    private String nombre;
    private Map<String, Map<String, ArrayList<String>>> mapeoEstados;
    private boolean ambiguedad;

    public Arbol(ArrayList<Simbolos> elem, String nombre) throws IOException {
        numeroX = 1;
        raiz = new Nodo(elem.get(0).getValor());
        this.elementos = elem;
        siguientes = new ArrayList<>();
        transiciones = new ArrayList<>();
        estadosM = new HashMap<>();
        auxiliarGraph = 1;
        terminales = new HashMap<>();
        mapeoEstados = new HashMap<>();
        ambiguedad = false;
        this.nombre = nombre;
        insertarElementos(elementos, verificarHijos(elem), raiz);
        ampliarArbol();
        verificarHojas(raiz);
        graficador = new Graficador(nombre);
        while (!raiz.anulableCalculado) {
            calcularAnulable(raiz);
        }
        while (!raiz.primeroCalculado) {
            calcularPrimeros(raiz);
        }
        while (!raiz.ultimoCalculado) {
            calcularUltimos(raiz);
        }
        while (!raiz.siguienteCalculado) {
            calcularSiguientes(raiz);
        }
        generarTransicionInicial();
        while (comprobarTransiciones()) {
        }
        for (int i = 0; i < transiciones.size(); i++) {
            String estado = "";
            for (Object estado1 : transiciones.get(i).getEstado()) {
                if (estado.isEmpty()) {
                    estado = estado1.toString();
                } else {
                    estado += ", " + estado1;
                }
            }
            transiciones.get(i).getMovimientos().entrySet().forEach((terminal) -> {
                String conjunto = "";
                for (Object value : terminal.getValue()) {
                    if (conjunto.isEmpty()) {
                        conjunto = value.toString();
                    } else {
                        conjunto += ", " + value;
                    }
                }
            });
        }
        generarEstados();
        graficar();
    }

    public boolean isAmbiguedad() {
        return ambiguedad;
    }

    public void setAmbiguedad(boolean ambiguedad) {
        this.ambiguedad = ambiguedad;
    }

    public Map<String, Map<String, ArrayList<String>>> getMapeoEstados() {
        return mapeoEstados;
    }

    public void setMapeoEstados(Map<String, Map<String, ArrayList<String>>> mapeoEstados) {
        this.mapeoEstados = mapeoEstados;
    }

    public Map<String, ArrayList<Object>> getTerminales() {
        return terminales;
    }

    public void setTerminales(Map<String, ArrayList<Object>> terminales) {
        this.terminales = terminales;
    }

    public Map<String, String> getEstadosM() {
        return estadosM;
    }

    public void setEstadosM(Map<String, String> estadosM) {
        this.estadosM = estadosM;
    }

    public ArrayList<Transicion> getTransiciones() {
        return transiciones;
    }

    public void setTransiciones(ArrayList<Transicion> transiciones) {
        this.transiciones = transiciones;
    }

    public Graficador getGraficador() {
        return graficador;
    }

    public void setGraficador(Graficador graficador) {
        this.graficador = graficador;
    }

    public String getNombre() {
        return nombre;
    }

    public void setNombre(String nombre) {
        this.nombre = nombre;
    }

    private boolean comprobarTransiciones() {
        ArrayList<ArrayList<Object>> estados = new ArrayList<>();
        ArrayList<Object> estadoNuevo = new ArrayList<>();
        //Genero los estados actuales
        transiciones.forEach((transicion) -> {
            estados.add(transicion.getEstado());
        });
        //Obtengo todas las transiciones
        transiciones.forEach((transicion) -> {
            //Obtengo los movimientos de las transiciones
            transicion.getMovimientos().entrySet().forEach((movimiento) -> {
                boolean agregar = false;
                //Obtengo cada estado actual
                for (ArrayList<Object> estado : estados) {
                    int contadorIguales = 0;
                    for (Object obj : movimiento.getValue()) {
                        if (!estado.contains(obj)) {
                            contadorIguales++;
                        }
                    }
                    if (contadorIguales != 0) {
                        agregar = true;
                    } else {
                        if (estado.size() == movimiento.getValue().size()) {
                            agregar = false;
                            break;
                        } else {
                            agregar = true;
                        }
                    }
                }
                if (agregar && estadoNuevo.isEmpty()) {
                    movimiento.getValue().forEach((obj) -> {
                        estadoNuevo.add(obj);
                    });
                }
            });
        });
        if (estadoNuevo.isEmpty()) {
            return false;
        } else {
            generarTransicion(estadoNuevo);
            return true;
        }
    }

    private void generarTransicion(ArrayList<Object> lista) {
        Transicion transicion = new Transicion();
        transicion.setEstado(lista);
        transicion.agregarMovimientos(terminales, siguientes);
        transiciones.add(transicion);
    }

    private void generarTransicionInicial() {
        Transicion transicion = new Transicion();
        transicion.setEstado(raiz.getPrimeros());
        transicion.agregarMovimientos(terminales, siguientes);
        transiciones.add(transicion);
    }

    private void calcularSiguientes(Nodo nodo) {
        if (nodo != null) {
            switch (nodo.getValor()) {
                case ".":
                    if (nodo.hijoIzquierdo != null) {
                        nodo.hijoIzquierdo.ultimos.forEach((ultimo) -> {
                            for (Item siguiente : siguientes) {
                                if (ultimo.toString().equals(String.valueOf(siguiente.getNumero()))) {
                                    for (Object primero : nodo.hijoDerecho.primeros) {
                                        if (!siguiente.getSiguientes().contains(primero.toString())) {
                                            siguiente.getSiguientes().add(primero.toString());
                                        }
                                    }
                                }
                            }
                        });
                    }
                    break;
                case "*":
                case "+":
                    if(nodo.hijoIzquierdo != null){
                        nodo.hijoIzquierdo.ultimos.forEach((ultimo) -> {
                        for (Item siguiente : siguientes) {
                            if (ultimo.toString().equals(String.valueOf(siguiente.getNumero()))) {
                                for (Object primero : nodo.hijoIzquierdo.primeros) {
                                    if (!siguiente.getSiguientes().contains(primero.toString())) {
                                        siguiente.getSiguientes().add(primero.toString());
                                    }
                                }
                            }
                        }
                    });
                    }
                    break;
            }
            nodo.siguienteCalculado = true;
            calcularSiguientes(nodo.hijoIzquierdo);
            calcularSiguientes(nodo.hijoDerecho);
        }
    }

    public int getNumeroX() {
        return numeroX;
    }

    public void setNumeroX(int numeroX) {
        this.numeroX = numeroX;
    }

    public int getAuxiliarGraph() {
        return auxiliarGraph;
    }

    public void setAuxiliarGraph(int auxiliarGraph) {
        this.auxiliarGraph = auxiliarGraph;
    }

    public ArrayList<Item> getSiguientes() {
        return siguientes;
    }

    public void setSiguientes(ArrayList<Item> siguientes) {
        this.siguientes = siguientes;
    }

    public Nodo getRaiz() {
        return raiz;
    }

    public void setRaiz(Nodo raiz) {
        this.raiz = raiz;
    }

    public ArrayList<Simbolos> getElementos() {
        return elementos;
    }

    public void setElementos(ArrayList<Simbolos> elementos) {
        this.elementos = elementos;
    }

    private void verificarHojas(Nodo nodo) {
        if (nodo != null) {
            if (nodo.getHijoIzquierdo() == null) {
                nodo.setHoja(true);
                nodo.setAnulable(false);
                nodo.setAnulableCalculado(true);
                nodo.setPrimeroCalculado(true);
                nodo.setUltimoCalculado(true);
                nodo.setNumero(numeroX++);
                nodo.getPrimeros().add(nodo.getNumero());
                nodo.getUltimos().add(nodo.getNumero());
                siguientes.add(new Item(nodo.getValor(), nodo.getNumero()));
                if (terminales.containsKey(nodo.getValor())) {
                    terminales.entrySet().forEach((terminal) -> {
                        if (terminal.getKey().equals(nodo.getValor())) {
                            terminal.getValue().add(nodo.getNumero());
                        }
                    });
                } else {
                    ArrayList<Object> temp = new ArrayList<>();
                    temp.add(nodo.getNumero());
                    terminales.put(nodo.getValor(), temp);
                }
            }
            verificarHojas(nodo.getHijoIzquierdo());
            verificarHojas(nodo.getHijoDerecho());
        }
    }

    private void calcularUltimos(Nodo nodo) {
        if (nodo != null) {
            if (nodo.hijoIzquierdo != null) {
                if (nodo.hijoDerecho != null) {
                    if (nodo.hijoIzquierdo.ultimoCalculado && nodo.hijoDerecho.ultimoCalculado && nodo.hijoIzquierdo != null) {
                        switch (nodo.getValor()) {
                            case "\\|":
                                nodo.hijoIzquierdo.ultimos.forEach((ultimo) -> {
                                    if (!nodo.ultimos.contains(ultimo)) {
                                        nodo.ultimos.add(ultimo);
                                    }
                                });
                                nodo.hijoDerecho.ultimos.forEach((ultimo) -> {
                                    if (!nodo.ultimos.contains(ultimo)) {
                                        nodo.ultimos.add(ultimo);
                                    }
                                });
                                break;
                            case ".":
                                if (nodo.hijoDerecho.anulable) {
                                    nodo.hijoIzquierdo.ultimos.forEach((ultimo) -> {
                                        if (!nodo.ultimos.contains(ultimo)) {
                                            nodo.ultimos.add(ultimo);
                                        }
                                    });
                                    nodo.hijoDerecho.ultimos.forEach((ultimo) -> {
                                        if (!nodo.ultimos.contains(ultimo)) {
                                            nodo.ultimos.add(ultimo);
                                        }
                                    });
                                } else {
                                    nodo.hijoDerecho.ultimos.forEach((ultimo) -> {
                                        if (!nodo.ultimos.contains(ultimo)) {
                                            nodo.ultimos.add(ultimo);
                                        }
                                    });
                                }
                                break;
                        }
                        nodo.setUltimoCalculado(true);
                    }
                } else {
                    if (nodo.hijoIzquierdo.ultimoCalculado && nodo.hijoIzquierdo != null) {
                        switch (nodo.getValor()) {
                            case "*":
                            case "?":
                            case "+":
                                nodo.hijoIzquierdo.ultimos.forEach((ultimo) -> {
                                    if (!nodo.ultimos.contains(ultimo)) {
                                        nodo.ultimos.add(ultimo);
                                    }
                                });
                                break;
                        }
                        nodo.setUltimoCalculado(true);
                    }
                }
            }
            calcularUltimos(nodo.hijoIzquierdo);
            calcularUltimos(nodo.hijoDerecho);
        }
    }

    private void calcularPrimeros(Nodo nodo) {
        if (nodo != null) {
            if (nodo.hijoIzquierdo != null) {
                if (nodo.hijoDerecho != null) {
                    if (nodo.hijoIzquierdo.primeroCalculado && nodo.hijoDerecho.primeroCalculado && nodo.hijoIzquierdo != null) {
                        switch (nodo.getValor()) {
                            case "\\|":
                                nodo.hijoIzquierdo.primeros.forEach((primero) -> {
                                    if (!nodo.primeros.contains(primero)) {
                                        nodo.primeros.add(primero);
                                    }
                                });
                                nodo.hijoDerecho.primeros.forEach((primero) -> {
                                    if (!nodo.primeros.contains(primero)) {
                                        nodo.primeros.add(primero);
                                    }
                                });
                                break;
                            case ".":
                                if (nodo.hijoIzquierdo.anulable) {
                                    nodo.hijoIzquierdo.primeros.forEach((primero) -> {
                                        if (!nodo.primeros.contains(primero)) {
                                            nodo.primeros.add(primero);
                                        }
                                    });
                                    nodo.hijoDerecho.primeros.forEach((primero) -> {
                                        if (!nodo.primeros.contains(primero)) {
                                            nodo.primeros.add(primero);
                                        }
                                    });
                                } else {
                                    nodo.hijoIzquierdo.primeros.forEach((primero) -> {
                                        if (!nodo.primeros.contains(primero)) {
                                            nodo.primeros.add(primero);
                                        }
                                    });
                                }
                                break;
                        }
                        nodo.setPrimeroCalculado(true);
                    }
                } else {
                    if (nodo.hijoIzquierdo.primeroCalculado && nodo.hijoIzquierdo != null) {
                        switch (nodo.getValor()) {
                            case "*":
                            case "?":
                            case "+":
                                nodo.hijoIzquierdo.primeros.forEach((primero) -> {
                                    if (!nodo.primeros.contains(primero)) {
                                        nodo.primeros.add(primero);
                                    }
                                });
                                break;
                        }
                        nodo.setPrimeroCalculado(true);
                    }
                }
            }
            calcularPrimeros(nodo.hijoIzquierdo);
            calcularPrimeros(nodo.hijoDerecho);
        }
    }

    private void calcularAnulable(Nodo nodo) {
        if (nodo != null) {
            if (nodo.hijoIzquierdo != null) {
                if (nodo.hijoDerecho != null) {
                    if (nodo.hijoIzquierdo.anulableCalculado && nodo.hijoDerecho.anulableCalculado && nodo.hijoIzquierdo != null) {
                        switch (nodo.getValor()) {
                            case "\\|":
                                if (nodo.hijoIzquierdo.anulable || nodo.hijoDerecho.anulable) {
                                    nodo.setAnulable(true);
                                }
                                break;
                            case ".":
                                if (nodo.hijoIzquierdo.anulable && nodo.hijoDerecho.anulable) {
                                    nodo.setAnulable(true);
                                }
                                break;
                        }
                        nodo.setAnulableCalculado(true);
                    }
                } else {
                    if (nodo.hijoIzquierdo.anulableCalculado && nodo.hijoIzquierdo != null) {
                        switch (nodo.getValor()) {
                            case "*":
                            case "?":
                                nodo.setAnulable(true);
                                break;
                            case "+":
                                nodo.setAnulable(nodo.hijoIzquierdo.anulable);
                                break;
                        }
                        nodo.setAnulableCalculado(true);
                    }
                }
            }
            calcularAnulable(nodo.hijoIzquierdo);
            calcularAnulable(nodo.hijoDerecho);
        }
    }

    private void ampliarArbol() {
        Nodo concatenacion = new Nodo(".");
        concatenacion.setIdentificador(0);
        Nodo fin = new Nodo("#");
        fin.setIdentificador(auxiliarGraph++);
        concatenacion.setHijoDerecho(fin);
        concatenacion.setHijoIzquierdo(raiz);
        raiz = concatenacion;
    }

    private void insertarElementos(ArrayList<Simbolos> elementos, int hijos, Nodo nodo) {
        Nodo izquierdo;
        Nodo derecho;
        if (!elementos.isEmpty()) {
            nodo.setValor(elementos.get(0).getValor().replace("_201700893", ""));
            nodo.setIdentificador(auxiliarGraph++);
            elementos.remove(0);
            if (!elementos.isEmpty()) {
                switch (hijos) {
                    case 2:
                        izquierdo = new Nodo(elementos.get(0).getValor());
                        nodo.setHijoIzquierdo(izquierdo);
                        insertarElementos(elementos, verificarHijos(elementos), izquierdo);
                        derecho = new Nodo(elementos.get(0).getValor());
                        nodo.setHijoDerecho(derecho);
                        insertarElementos(elementos, verificarHijos(elementos), derecho);
                        break;
                    case 1:
                        izquierdo = new Nodo(elementos.get(0).getValor());
                        nodo.setHijoIzquierdo(izquierdo);
                        insertarElementos(elementos, verificarHijos(elementos), izquierdo);
                        break;
                }
            }
        }
    }

    private int verificarHijos(ArrayList<Simbolos> elementos) {
        if (elementos.get(0).getTipo() == Simbolos.EnumTipo.expConcatenacion || elementos.get(0).getTipo() == Simbolos.EnumTipo.expOr) {
            return 2;
        }
        if (elementos.get(0).getTipo() == Simbolos.EnumTipo.expMas || elementos.get(0).getTipo() == Simbolos.EnumTipo.expPor || elementos.get(0).getTipo() == Simbolos.EnumTipo.expPregunta) {
            return 1;
        }
        return 0;
    }

    private void generarEstados() {
        int contador = 0;
        ArrayList<ArrayList<Object>> estados = new ArrayList<>();
        //Genero los estados actuales
        transiciones.forEach((transicion) -> {
            estados.add(transicion.getEstado());
        });
        ArrayList<String> tmp = new ArrayList<>();
        for (ArrayList<Object> arL : estados) {
            String conjunto = "";
            for (Object obj : arL) {
                if (conjunto.isEmpty()) {
                    conjunto = obj.toString();
                } else {
                    conjunto += ", " + obj.toString();
                }
            }
            tmp.add(conjunto);
        }
        Collections.sort(tmp);
        for (String str : tmp) {
            estadosM.put(str, "S" + (contador));
            contador++;
        }
    }

    private void graficar() throws IOException {
        int aceptacion = 0;
        for (Item item : siguientes) {
            if (item.getEtiqueta().equals("#")) {
                aceptacion = item.getNumero();
            }
        }
        graficador.generarGraph(raiz, siguientes, transiciones, terminales, estadosM, aceptacion);
        graficador.generarArchivos();
    }

}
