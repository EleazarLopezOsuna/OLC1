/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package Modelos;

/**
 *
 * @author Eleazar Lopez <Universidad de San Carlos de Guatemala>
 */
public class Expresion {
    public Simbolo.EnumTipo tipo;
    public Object valor;
    
    public Expresion(Simbolo.EnumTipo tipo, Object valor){
        this.tipo = tipo;
        this.valor = valor;
    }
}
