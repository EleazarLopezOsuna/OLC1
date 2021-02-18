/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package Modelos;

import java.awt.Color;
import java.util.ArrayList;

/**
 *
 * @author Eleazar Lopez <Universidad de San Carlos de Guatemala>
 */
public class Componente {
    
    private EnumTipo tipo;
    private String id;
    private int posX;
    private int posY;
    private int height;
    private int width;
    private Color color;
    private int border;
    private String className;
    private String onClick;
    private String nombre;
    private String fuente;
    private ArrayList<String> items;
    private int defecto;
    private int min;
    private int max;
    private String contenido;
    private ArrayList<Componente> componentes;
    private ArrayList<Estilo> estilos;

    public Componente(EnumTipo tipo) {
        this.tipo = tipo;
        id = "";
        posX = 0;
        posY = 0;
        height = 100;
        width = 100;
        color = Color.white;
        border = 0;
        className = "";
        onClick = "";
        fuente = "";
        items = new ArrayList<>();
        defecto = 0;
        min = -100;
        max = 100;
        contenido = "";
        componentes = new ArrayList<>();
        estilos = new ArrayList<>();
    }

    public ArrayList<Estilo> getEstilos() {
        return estilos;
    }

    public void setEstilos(ArrayList<Estilo> estilos) {
        this.estilos = estilos;
    }
    
    public EnumTipo getTipo() {
        return tipo;
    }

    public void setTipo(EnumTipo tipo) {
        this.tipo = tipo;
    }
    
    public String getOnClick() {
        return onClick;
    }

    public void setOnClick(String onClick) {
        this.onClick = onClick;
    }

    public String getNombre() {
        return nombre;
    }

    public void setNombre(String nombre) {
        this.nombre = nombre;
    }

    public String getFuente() {
        return fuente;
    }

    public void setFuente(String fuente) {
        this.fuente = fuente;
    }

    public ArrayList<String> getItems() {
        return items;
    }

    public void setItems(ArrayList<String> items) {
        this.items = items;
    }

    public int getDefecto() {
        return defecto;
    }

    public void setDefecto(int defecto) {
        this.defecto = defecto;
    }

    public int getMin() {
        return min;
    }

    public void setMin(int min) {
        this.min = min;
    }

    public int getMax() {
        return max;
    }

    public void setMax(int max) {
        this.max = max;
    }

    public String getContenido() {
        return contenido;
    }

    public void setContenido(String contenido) {
        this.contenido = contenido;
    }

    public ArrayList<Componente> getComponentes() {
        return componentes;
    }

    public void setComponentes(ArrayList<Componente> componentes) {
        this.componentes = componentes;
    }
    
    public String getId() {
        return id;
    }

    public void setId(String id) {
        this.id = id;
    }

    public int getPosX() {
        return posX;
    }

    public void setPosX(int posX) {
        this.posX = posX;
    }

    public int getPosY() {
        return posY;
    }

    public void setPosY(int posY) {
        this.posY = posY;
    }

    public int getHeight() {
        return height;
    }

    public void setHeight(int height) {
        this.height = height;
    }

    public int getWidth() {
        return width;
    }

    public void setWidth(int width) {
        this.width = width;
    }

    public Color getColor() {
        return color;
    }
    
    public void setColor(Color color){
        this.color = color;
    }

    public void setColor(String NombreColor) {
        switch(NombreColor.toLowerCase()){
            case "red":
                color = Color.red;
                break;
            case "pink":
                color = Color.pink;
                break;
            case "orange":
                color = Color.orange;
                break;
            case "yellow":
                color = Color.yellow;
                break;
            case "purple":
                color = new Color(87,35,100);
                break;
            case "magenta":
                color = Color.magenta;
                break;
            case "green":
                color = Color.green;
                break;
            case "blue":
                color = Color.blue;
                break;
            case "brown":
                color = new Color(138,114,104);
                break;
            case "white":
                color = Color.white;
                break;
            case "gray":
                color = Color.gray;
                break;
            case "black":
                color = Color.black;
                break;
            default:
                if(NombreColor.contains("#")){
                    color = Color.decode(NombreColor);
                }
                break;
        }
    }

    public int getBorder() {
        return border;
    }

    public void setBorder(int border) {
        this.border = border;
    }

    public String getClassName() {
        return className;
    }

    public void setClassName(String className) {
        this.className = className;
    }
    
    
    public enum EnumTipo{
        panel, text, textfield, button, image, spinner, list, ufex
    }
}