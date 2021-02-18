/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package Modelos;

import Estructuras.*;
import java.util.ArrayList;

/**
 *
 * @author USER
 */
public class Archivo {

    private String nombre;
    private ArrayList<Cabecera> cabeceras;
    private ArrayList<Registro> registros;
    private Cola errores;

    public Archivo() {
        this.nombre = "";
        this.cabeceras = new ArrayList<>();
        this.registros = new ArrayList<>();
        this.errores = new Cola();
    }

    public String getNombre() {
        return nombre;
    }

    public void setNombre(String nombre) {
        this.nombre = nombre;
    }

    public ArrayList<Cabecera> getCabeceras() {
        return cabeceras;
    }

    public void setCabeceras(ArrayList<Cabecera> cabeceras) {
        this.cabeceras = cabeceras;
    }

    public ArrayList<Registro> getRegistros() {
        return registros;
    }

    public void setRegistros(ArrayList<Registro> registros) {
        this.registros = registros;
    }

    public Cola getErrores() {
        return errores;
    }

    public void setErrores(Cola errores) {
        this.errores = errores;
    }

}
