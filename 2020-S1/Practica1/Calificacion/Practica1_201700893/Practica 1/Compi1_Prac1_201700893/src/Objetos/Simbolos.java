/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package Objetos;

import java.util.regex.Pattern;

/**
 *
 * @author Eleazar Lopez <Universidad de San Carlos de Guatemala>
 */
public class Simbolos {

    private EnumTipo tipo;
    private String valor;
    private int linea;
    private int columna;

    public Simbolos() {

    }

    public Simbolos(EnumTipo tipo, String valor, int linea, int columna) {
        if (tipo == EnumTipo.variable) {
            valor = valor.replace("{", "");
            valor = valor.replace("}", "");
        }
        if(tipo == EnumTipo.cadena){
            valor = valor.replace("\"", "");
        }
        if(tipo == EnumTipo.expOr){
            valor = "\\|";
        }
        this.tipo = tipo;
        this.valor = valor;
        this.linea = linea;
        this.columna = columna;
    }

    public enum EnumTipo {
        id, cadena, dosPuntos, guion, mayorQue, menorQue, puntoComa,
        llaveAbierta, llaveCerrada, coma, virgulilla, admiracionCierra, porciento,
        comillasDobles, expOr, expConcatenacion, expMas, expPor, expPregunta, slash, error, numero, reservada,
        intervalo, lista, variable
    }

    public EnumTipo getTipo() {
        return tipo;
    }

    public void setTipo(EnumTipo tipo) {
        this.tipo = tipo;
    }

    public String getValor() {
        return valor;
    }

    public void setValor(String valor) {
        this.valor = valor;
    }

    public int getLinea() {
        return linea;
    }

    public void setLinea(int linea) {
        this.linea = linea;
    }

    public int getColumna() {
        return columna;
    }

    public void setColumna(int columna) {
        this.columna = columna;
    }

    public EnumTipo ObtenerTipo(String tipo) {
        if (tipo.length() == 1) {
            switch (tipo) {
                case ":":
                    return EnumTipo.dosPuntos;
                case "-":
                    return EnumTipo.guion;
                case "<":
                    return EnumTipo.menorQue;
                case ">":
                    return EnumTipo.mayorQue;
                case ";":
                    return EnumTipo.puntoComa;
                case "{":
                    return EnumTipo.llaveAbierta;
                case "}":
                    return EnumTipo.llaveCerrada;
                case ",":
                    return EnumTipo.coma;
                case "~":
                    return EnumTipo.virgulilla;
                case "!":
                    return EnumTipo.admiracionCierra;
                case "%":
                    return EnumTipo.porciento;
                case "\"":
                    return EnumTipo.comillasDobles;
                case "|":
                    return EnumTipo.expOr;
                case ".":
                    return EnumTipo.expConcatenacion;
                case "+":
                    return EnumTipo.expMas;
                case "*":
                    return EnumTipo.expPor;
                case "?":
                    return EnumTipo.expPregunta;
                case "/":
                    return EnumTipo.slash;
            }
        }
        if (tipo.equals("CONJ") || tipo.equals("conj")) {
            return EnumTipo.reservada;
        }
        if (tipo.contains("~")) {
            return EnumTipo.intervalo;
        }
        if (tipo.startsWith("\"")) {
            return EnumTipo.cadena;
        }
        if (Pattern.matches("[a-zA-Z][a-zA-Z0-9]*", tipo)) {
            return EnumTipo.id;
        }
        if (Pattern.matches("[0-9]+(.[0-9]+)?", tipo)) {
            return EnumTipo.numero;
        }
        if (tipo.contains(",")) {
            return EnumTipo.lista;
        }
        if (tipo.contains("{") || tipo.contains("}")) {
            return EnumTipo.variable;
        }
        return EnumTipo.error;
    }
}
