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
public class Ufe {
    private HashMap<String, Componente> componentes;
    private String nombre;

    public Ufe(String nombre){
        componentes = new HashMap<>();
        this.nombre = nombre;
    }

    public HashMap<String, Componente> getComponentes() {
        return componentes;
    }

    public void setComponentes(HashMap<String, Componente> componentes) {
        this.componentes = componentes;
    }

    public String getNombre() {
        return nombre;
    }

    public void setNombre(String nombre) {
        this.nombre = nombre;
    }
    
    
}
