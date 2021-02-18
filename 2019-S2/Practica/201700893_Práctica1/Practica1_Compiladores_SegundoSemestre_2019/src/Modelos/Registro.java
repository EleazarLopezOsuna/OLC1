/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package Modelos;

import java.util.ArrayList;

/**
 *
 * @author USER
 */
public class Registro {

    private ArrayList<Object> registros;
    
    public Registro(){
        this.registros = new ArrayList<>();
    }
    
    public ArrayList<Object> getRegistros() {
        return registros;
    }

    public void setRegistros(ArrayList<Object> registros) {
        this.registros = registros;
    }

}
