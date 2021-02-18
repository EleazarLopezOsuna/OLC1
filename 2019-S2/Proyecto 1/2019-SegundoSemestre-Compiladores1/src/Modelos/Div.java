/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package Modelos;

import java.util.ArrayList;

/**
 *
 * @author Eleazar Lopez <Universidad de San Carlos de Guatemala>
 */
public class Div {
    String nombre;
    ArrayList<Div> hijos;

    public Div(String nombre) {
        this.nombre = nombre;
        hijos = new ArrayList<>();
    }
    
    public String getNombre() {
        return nombre;
    }

    public void setNombre(String nombre) {
        this.nombre = nombre;
    }

    public ArrayList<Div> getHijos() {
        return hijos;
    }

    public void setHijos(ArrayList<Div> hijos) {
        this.hijos = hijos;
    }
    
    
}
