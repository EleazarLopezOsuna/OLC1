/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package Modelos;

import java.awt.Color;

/**
 *
 * @author Eleazar Lopez <Universidad de San Carlos de Guatemala>
 */
public class Estilo {
    
    String nombre;
    Color background;
    Boolean borde;
    Color colorBorde;
    int anchoBorde;
    String alineacion;
    String fuente;
    int alturaFuente;
    Color colorFuente;
    int alto;
    int ancho;

    public Estilo(String nombre) {
        this.nombre = nombre;
        background = null;
        borde = null;
        colorBorde = null;
        anchoBorde = -1;
        alineacion = null;
        fuente = null;
        alturaFuente = -1;
        colorFuente = null;
        alto = -1;
        ancho = -1;
    }

    public String getNombre() {
        return nombre;
    }

    public void setNombre(String nombre) {
        this.nombre = nombre;
    }

    public Color getBackground() {
        return background;
    }

    public void setBackground(Color background) {
        this.background = background;
    }

    public Boolean getBorde() {
        return borde;
    }

    public void setBorde(Boolean borde) {
        this.borde = borde;
    }

    public Color getColorBorde() {
        return colorBorde;
    }

    public void setColorBorde(Color colorBorde) {
        this.colorBorde = colorBorde;
    }

    public int getAnchoBorde() {
        return anchoBorde;
    }

    public void setAnchoBorde(int anchoBorde) {
        this.anchoBorde = anchoBorde;
    }

    public String getAlineacion() {
        return alineacion;
    }

    public void setAlineacion(String alineacion) {
        this.alineacion = alineacion;
    }

    public String getFuente() {
        return fuente;
    }

    public void setFuente(String fuente) {
        this.fuente = fuente;
    }

    public int getAlturaFuente() {
        return alturaFuente;
    }

    public void setAlturaFuente(int alturaFuente) {
        this.alturaFuente = alturaFuente;
    }

    public Color getColorFuente() {
        return colorFuente;
    }

    public void setColorFuente(Color colorFuente) {
        this.colorFuente = colorFuente;
    }

    public int getAlto() {
        return alto;
    }

    public void setAlto(int alto) {
        this.alto = alto;
    }

    public int getAncho() {
        return ancho;
    }

    public void setAncho(int ancho) {
        this.ancho = ancho;
    }
    
    

}
