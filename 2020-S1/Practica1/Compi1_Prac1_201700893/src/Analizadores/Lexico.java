/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package Analizadores;

import Objetos.*;
import java.io.BufferedReader;
import java.io.IOException;
import java.io.StringReader;
import java.util.ArrayList;

/**
 *
 * @author Eleazar Lopez <Universidad de San Carlos de Guatemala>
 */
public class Lexico {

    private String entrada;
    private ArrayList<Simbolos> simbolos;
    private ArrayList<Errores> errores;

    public Lexico(String entrada) {
        this.entrada = entrada;
        simbolos = new ArrayList<>();
        errores = new ArrayList<>();
    }

    public ArrayList<Simbolos> getSimbolos() {
        return simbolos;
    }

    public void setSimbolos(ArrayList<Simbolos> simbolos) {
        this.simbolos = simbolos;
    }

    public void analizar() throws IOException {
        String lexema = "";
        String linea = null;
        String lexemaAuxiliar = "";
        int fila = 0;
        boolean salir = false;
        boolean multilinea = false;
        boolean empezo = false;
        boolean expresion = false;
        boolean conjunto = false;
        boolean identificador = false;
        Simbolos simbolo = new Simbolos();
        int salirBloqueExpresiones = 0;
        boolean enCadena = false;
        boolean lista = false;
        BufferedReader reader = new BufferedReader(new StringReader(entrada));
        while ((linea = reader.readLine()) != null) {
            for (int columna = 0; columna < linea.length(); columna++) {
                char caracter = linea.charAt(columna);
                char auxiliar;
                salir = false;
                if (multilinea == true) {
                    if (linea.contains("!>")) {
                        //Fin del comentario multiLinea
                        multilinea = false;
                        //Mover hsata la ocurrencia de fin de comentario
                        columna = linea.indexOf("!>") + 1;
                    } else {
                        salir = true;
                    }
                } else {
                    switch (caracter) {
                        case 47:
                            if (lexema.isEmpty()) {
                                auxiliar = linea.charAt(columna + 1);
                                if (auxiliar == 47) {
                                    //Es Comentario de una linea
                                    salir = true;
                                }
                            } else {
                                if (enCadena) {
                                    lexema += caracter;
                                }
                            }
                            break;
                        case 60:
                            if (lexema.isEmpty()) {
                                auxiliar = linea.charAt(columna + 1);
                                if (auxiliar == 33) {
                                    //Es Comentario multilinea
                                    multilinea = true;
                                }
                            } else {
                                if (enCadena) {
                                    lexema += caracter;
                                }
                            }
                            break;
                        //Letras y Numeros  
                        case 48:
                        case 49:
                        case 50:
                        case 51:
                        case 52:
                        case 53:
                        case 54:
                        case 55:
                        case 56:
                        case 57:
                        case 65:
                        case 66:
                        case 67:
                        case 68:
                        case 69:
                        case 70:
                        case 71:
                        case 72:
                        case 73:
                        case 74:
                        case 75:
                        case 76:
                        case 77:
                        case 78:
                        case 79:
                        case 80:
                        case 81:
                        case 82:
                        case 83:
                        case 84:
                        case 85:
                        case 86:
                        case 87:
                        case 88:
                        case 89:
                        case 90:
                        case 97:
                        case 98:
                        case 99:
                        case 100:
                        case 101:
                        case 102:
                        case 103:
                        case 104:
                        case 105:
                        case 106:
                        case 107:
                        case 108:
                        case 109:
                        case 110:
                        case 111:
                        case 112:
                        case 113:
                        case 114:
                        case 115:
                        case 116:
                        case 117:
                        case 118:
                        case 119:
                        case 120:
                        case 121:
                        case 122:
                        case 164:
                        case 165:
                        case 126:
                            lexema += caracter;
                            break;
                        case 95:
                            if (!lexema.isEmpty()) {
                                lexema += caracter;
                            } else {
                                simbolo = new Simbolos(simbolo.ObtenerTipo(String.valueOf(caracter)), String.valueOf(caracter), fila, columna);
                                simbolos.add(simbolo);
                            }
                            break;
                        case 32:
                            //Verificar si es cadena
                            if (enCadena) {
                                lexema += caracter;
                            } else {
                                if (!lexema.isEmpty()) {
                                    simbolo = new Simbolos(simbolo.ObtenerTipo(lexema), lexema, fila, columna);
                                    simbolos.add(simbolo);
                                    lexema = "";
                                }
                            }
                            break;
                        case 34:
                            if (enCadena) {
                                lexema += caracter;
                                simbolo = new Simbolos(simbolo.ObtenerTipo(lexema), lexema, fila, columna);
                                simbolos.add(simbolo);
                                lexema = "";
                            } else {
                                lexema += caracter;

                            }
                            enCadena = !enCadena;
                            break;
                        case 44:
                            lexema += caracter;
                            break;
                        case 39:
                        case 33:
                        case 35:
                        case 36:
                        case 37:
                        case 38:
                        case 40:
                        case 41:
                        case 42:
                        case 43:
                        case 45:
                        case 46:
                        case 58:
                        case 59:
                        case 61:
                        case 62:
                        case 63:
                        case 64:
                        case 91:
                        case 92:
                        case 93:
                        case 94:
                        case 96:
                        case 124:
                            if (!lexema.isEmpty()) {
                                if (enCadena) {
                                    lexema += caracter;
                                } else {
                                    simbolo = new Simbolos(simbolo.ObtenerTipo(lexema), lexema, fila, columna);
                                    simbolos.add(simbolo);
                                    simbolo = new Simbolos(simbolo.ObtenerTipo(String.valueOf(caracter)), String.valueOf(caracter), fila, columna);
                                    simbolos.add(simbolo);
                                    lexema = "";
                                }
                            } else {
                                simbolo = new Simbolos(simbolo.ObtenerTipo(String.valueOf(caracter)), String.valueOf(caracter), fila, columna);
                                simbolos.add(simbolo);
                                lexema = "";
                            }
                            break;
                        case 123:
                            if (empezo) {
                                lexema += caracter;
                            } else {
                                empezo = true;
                                simbolo = new Simbolos(simbolo.ObtenerTipo(String.valueOf(caracter)), String.valueOf(caracter), fila, columna);
                                simbolos.add(simbolo);
                            }
                            break;
                        case 125:
                            if (lexema.contains("{")) {
                                lexema += caracter;
                            } else {
                                simbolo = new Simbolos(simbolo.ObtenerTipo(lexema), lexema, fila, columna);
                                simbolos.add(simbolo);
                                simbolo = new Simbolos(simbolo.ObtenerTipo(String.valueOf(caracter)), String.valueOf(caracter), fila, columna);
                                simbolos.add(simbolo);
                                lexema = "";
                            }
                            break;
                        default:
                            errores.add(new Errores(Errores.EnumTipo.lexico, "Caracter: " + caracter + " no reconocido", fila, columna));
                            break;
                    }
                }
                if (salir) {
                    break;
                }
            }
            fila++;
        }
    }

    public ArrayList<Errores> getErrores() {
        return errores;
    }

}
