/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package Estructuras;
import Nodos.NodoCola;
import java.util.ArrayList;

/**
 *
 * @author USER
 */
public class Cola {

    public NodoCola cabeza;
    public int contador;

    public Cola() {
        cabeza = null;
        contador = 0;
    }

    public Boolean empty() {
        if (cabeza == null) {
            return true;
        }
        return false;
    }

    public Boolean insertar(Object dato) {
        NodoCola nuevo = new NodoCola(dato);
        this.contador++;
        if (!empty()) {
            NodoCola temp = cabeza;
            while (temp != null) {
                if (temp.getSiguiente() == null) {
                    temp.setSiguiente(nuevo);
                    break;
                }
                temp = temp.getSiguiente();
            }
            return true;
        } else {
            cabeza = nuevo;
            return true;
        }
    }
    
    public ArrayList<Object> obtenerDatos(){
        ArrayList<Object> datos = new ArrayList<>();
        NodoCola temp = cabeza;
        while(temp != null){
            datos.add(temp.getContenido());
            temp = temp.getSiguiente();
        }
        return datos;
    }
}
