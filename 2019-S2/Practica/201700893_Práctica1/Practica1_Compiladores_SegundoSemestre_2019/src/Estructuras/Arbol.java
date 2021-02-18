/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package Estructuras;

import Modelos.Variable;
import Nodos.NodoArbol;

/**
 *
 * @author USER
 */
public class Arbol {

    private NodoArbol raiz;

    public Arbol() {
        raiz = null;
    }

    public NodoArbol getRaiz() {
        return raiz;
    }

    public void setRaiz(NodoArbol raiz) {
        this.raiz = raiz;
    }

    public Boolean buscar(Variable variable, NodoArbol nodo) {
        if (nodo == null) {
            return false;
        } else {
            String nombreVariable = variable.getNombre();
            String nombreNodo = nodo.getVariable().getNombre();
            return (nombreVariable.equals(nombreNodo)) ? true : (nombreVariable.compareTo(nombreNodo) < 0)
                    ? buscar(variable, nodo.getHijoIzquierdo()) : buscar(variable, nodo.getHijoDerecho());
        }
    }

    public Variable obtenerValor(String nombre, NodoArbol nodo) {
        if (nodo == null) {
            return null;
        } else {
            String nombreVariable = nombre;
            String nombreNodo = nodo.getVariable().getNombre();
            return (nombreVariable.equals(nombreNodo)) ? nodo.getVariable() : (nombreVariable.compareTo(nombreNodo) < 0)
                    ? obtenerValor(nombre, nodo.getHijoIzquierdo()) : obtenerValor(nombre, nodo.getHijoDerecho());
        }
    }

    public String insertar(Variable variable) {
        NodoArbol nuevo = new NodoArbol(variable);
        if (raiz == null) {
            raiz = nuevo;
            return "Ingresada";
        } else {
            if (!buscar(variable, raiz)) {
                NodoArbol temp = raiz;
                NodoArbol root = temp;
                while (true) {
                    root = temp;
                    String nombreVariable = variable.getNombre();
                    String nombreNodo = temp.getVariable().getNombre();
                    if (nombreVariable.compareTo(nombreNodo) < 0) {
                        temp = temp.getHijoIzquierdo();
                        if (temp == null) {
                            root.setHijoIzquierdo(nuevo);
                            return "Ingresada";
                        }
                    } else {
                        temp = temp.getHijoDerecho();
                        if (temp == null) {
                            root.setHijoDerecho(nuevo);
                            return "Ingresada";
                        }
                    }
                }
            } else {
                return "Variable Duplicada";
            }
        }
    }

    public void preOrden(NodoArbol root) {
        if (root != null) {
            preOrden(root.getHijoIzquierdo());
            System.out.println(root.getVariable().getNombre());
            preOrden(root.getHijoDerecho());
        }
    }
}
