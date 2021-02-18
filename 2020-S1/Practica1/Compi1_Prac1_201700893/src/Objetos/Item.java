/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package Objetos;

import java.util.ArrayList;

/**
 *
 * @author Eleazar Lopez <Universidad de San Carlos de Guatemala>
 */
public class Item {

    private String etiqueta;
    private int numero;
    private ArrayList<String> siguientes;
    
    public Item(String etiqueta, int numero){
        this.etiqueta = etiqueta;
        this.numero = numero;
        siguientes = new ArrayList<>();
    }

    public String getEtiqueta() {
        return etiqueta;
    }

    public void setEtiqueta(String etiqueta) {
        this.etiqueta = etiqueta;
    }

    public int getNumero() {
        return numero;
    }

    public void setNumero(int numero) {
        this.numero = numero;
    }

    public ArrayList<String> getSiguientes() {
        return siguientes;
    }

    public void setSiguientes(ArrayList<String> siguientes) {
        this.siguientes = siguientes;
    }
}
