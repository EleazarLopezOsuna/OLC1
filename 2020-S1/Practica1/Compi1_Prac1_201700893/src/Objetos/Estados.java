/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package Objetos;

import java.util.HashMap;
import java.util.Map;

/**
 *
 * @author Eleazar Lopez <Universidad de San Carlos de Guatemala>
 */
public class Estados {
    private String estado;
    private Map<String, String> movimientos;
    private boolean aceptacion;

    public Estados(String estado) {
        this.estado = estado;
        aceptacion = false;
        movimientos = new HashMap<>();
    }
    
    public void agregarMovimiento(String terminal, String nuevoEstado){
        movimientos.put(terminal, nuevoEstado);
    }

    public boolean isAceptacion() {
        return aceptacion;
    }

    public void setAceptacion(boolean aceptacion) {
        this.aceptacion = aceptacion;
    }

    public String getEstado() {
        return estado;
    }

    public void setEstado(String estado) {
        this.estado = estado;
    }

    public Map<String, String> getMovimientos() {
        return movimientos;
    }

    public void setMovimientos(Map<String, String> movimientos) {
        this.movimientos = movimientos;
    }
    
    
}
