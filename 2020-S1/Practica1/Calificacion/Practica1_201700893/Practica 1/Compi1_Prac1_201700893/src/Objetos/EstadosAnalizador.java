/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package Objetos;

import java.util.ArrayList;
import java.util.HashMap;
import java.util.Map;
import java.util.Set;

/**
 *
 * @author Eleazar Lopez <Universidad de San Carlos de Guatemala>
 */
public class EstadosAnalizador {
    private String estado;
    private Map<String, ArrayList<String>> movimientos;
    private boolean aceptacion;
    private String estadoSiguiente;
    
    public EstadosAnalizador(String estado, boolean aceptacion){
        this.estado = estado;
        this.aceptacion = aceptacion;
        movimientos = new HashMap<>();
        estadoSiguiente = "";
    }

    public String getEstadoSiguiente() {
        return estadoSiguiente;
    }

    public void setEstadoSiguiente(String estadoSiguiente) {
        this.estadoSiguiente = estadoSiguiente;
    }
    
    public String getEstadoSiguiente(String cadena){
        movimientos.entrySet().forEach((movimiento) -> {
            ArrayList<String> tmp = movimiento.getValue();
            if(tmp.contains(cadena)){
                estadoSiguiente = movimiento.getKey();
            }
        });
        if(!estadoSiguiente.isEmpty()){
            return estadoSiguiente;
        } 
        return "EXPRESION NO VALIDA";
    }

    public String getEstado() {
        return estado;
    }

    public void setEstado(String estado) {
        this.estado = estado;
    }

    public Map<String, ArrayList<String>> getMovimientos() {
        return movimientos;
    }

    public void setMovimientos(Map<String, ArrayList<String>> movimientos) {
        this.movimientos = movimientos;
    }

    public boolean isAceptacion() {
        return aceptacion;
    }

    public void setAceptacion(boolean aceptacion) {
        this.aceptacion = aceptacion;
    }
    
    
}
