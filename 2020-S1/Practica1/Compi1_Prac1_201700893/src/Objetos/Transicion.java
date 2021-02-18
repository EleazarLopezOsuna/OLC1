/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package Objetos;

import java.util.ArrayList;
import java.util.Collections;
import java.util.HashMap;
import java.util.Map;

/**
 *
 * @author Eleazar Lopez <Universidad de San Carlos de Guatemala>
 */
public class Transicion {

    private ArrayList<Object> estado;
    private Map<String, ArrayList<Object>> movimientos;

    public Transicion() {
        estado = new ArrayList<>();
        movimientos = new HashMap<>();
    }

    public ArrayList<Object> transformar(ArrayList<Object> cambio) {
        ArrayList<String> temporal = new ArrayList<>();
        ArrayList<Object> retorno = new ArrayList<>();
        for (Object ob : cambio) {
            temporal.add(ob.toString());
        }
        Collections.sort(temporal);
        for (String s : temporal) {
            retorno.add(s);
        }
        return retorno;
    }

    public void agregarMovimientos(Map<String, ArrayList<Object>> terminales, ArrayList<Item> siguientes) {
        terminales.entrySet().forEach((terminal) -> {
            ArrayList<Object> temporal = new ArrayList<>();
            for (Object objeto : estado) {
                boolean loContiene = false;
                for (Object obj : terminal.getValue()) {
                    if (String.valueOf(obj).equals(String.valueOf(objeto))) {
                        loContiene = true;
                    }
                }
                if (loContiene) {
                    for (Item item : siguientes) {
                        if (item.getNumero() == Integer.parseInt(String.valueOf(objeto))) {
                            for (Object siguiente : item.getSiguientes()) {
                                if (!temporal.contains(siguiente)) {
                                    temporal.add(siguiente);
                                }
                            }
                        }
                    }
                    movimientos.put(terminal.getKey(), transformar(temporal));
                }
            }
        });
    }

    public ArrayList<Object> getEstado() {
        return estado;
    }

    public void setEstado(ArrayList<Object> estado) {
        this.estado = estado;
    }

    public Map<String, ArrayList<Object>> getMovimientos() {
        return movimientos;
    }

    public void setMovimientos(Map<String, ArrayList<Object>> movimientos) {
        this.movimientos = movimientos;
    }
}
