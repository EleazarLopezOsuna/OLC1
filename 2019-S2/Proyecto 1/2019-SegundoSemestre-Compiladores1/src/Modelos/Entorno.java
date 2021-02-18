/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package Modelos;

import java.util.HashMap;

/**
 *
 * @author Eleazar Lopez <Universidad de San Carlos de Guatemala>
 */
public class Entorno {

    public HashMap<String, Simbolo> tabla;
    Entorno anterior;

    public Entorno(Entorno anterior) {
        this.anterior = anterior;
        this.tabla = new HashMap<>();
    }

    public boolean insertar(String nombre, Simbolo sim, int linea, int columna) {
        if (tabla.containsKey(nombre)) {
            return false;
        }
        tabla.put(nombre, sim);
        return true;
    }

    public Simbolo buscar(String nombre, int linea, int columna) {
        for (Entorno e = this; e != null; e = e.anterior) {
            if (e.tabla.containsKey(nombre)) {
                Simbolo sim = e.tabla.get(nombre);
                return sim;
            }
        }
        return null;
    }

    public boolean modificar(String nombre, Simbolo simbolo) {
        for (Entorno e = this; e != null; e = e.anterior) {
            if (e.tabla.containsKey(nombre)) {
                e.tabla.replace(nombre, simbolo);
                return true;
            }
        }
        return false;
    }
}
