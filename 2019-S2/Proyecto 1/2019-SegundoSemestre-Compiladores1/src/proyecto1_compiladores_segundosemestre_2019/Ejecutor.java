/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package proyecto1_compiladores_segundosemestre_2019;

import Modelos.*;
import Nodos.NodoSintactico;
import java.io.BufferedReader;
import java.io.File;
import java.io.FileInputStream;
import java.io.IOException;
import java.io.InputStreamReader;
import java.io.StringReader;
import java.util.ArrayList;
import java.util.HashMap;
import java.util.logging.Level;
import java.util.logging.Logger;
import javax.swing.JOptionPane;

/**
 *
 * @author Eleazar Lopez <Universidad de San Carlos de Guatemala>
 */
public class Ejecutor {

    public Ufe componentesUfex;
    public String path;
    public String nombreComponenteActual = "";
    public HashMap<String, Componente> paraGraficar;
    public HashMap<String, Estilo> estilos;
    public Analizadores.HTML.Analisis_Sintactico sintactico_html;
    public Analizadores.CSS.Analisis_Sintactico sintactico_css;
    public String error;
    public ArrayList<String> errores;

    public void Ejecutar(NodoSintactico raiz, String nombreArchivo, String path) {
        paraGraficar = new HashMap<>();
        estilos = new HashMap<>();
        errores = new ArrayList<>();
        componentesUfex = new Ufe(nombreArchivo);
        this.path = path;
        analizarHtml();
        analizarCss();
        recorrerImports(raiz, null);
        recorrer(raiz, null, null);
        recorrerRender(raiz, null);
    }

    private void analizarCss() {
        File archivoC = new File(path + "src/App.css");
        String documentoC = "";
        if (archivoC.exists()) {
            if (archivoC.canRead()) {
                if (archivoC.getName().endsWith("css")) {
                    try {
                        try (BufferedReader in = new BufferedReader(
                                new InputStreamReader(
                                        new FileInputStream(archivoC), "UTF8"))) {
                            String str;
                            while ((str = in.readLine()) != null) {
                                documentoC += str + "\n";
                            }
                        }
                    } catch (IOException e) {
                    }
                    documentoC = documentoC.replace("á", "a");
                    documentoC = documentoC.replace("é", "e");
                    documentoC = documentoC.replace("í", "i");
                    documentoC = documentoC.replace("ó", "o");
                    documentoC = documentoC.replace("ú", "u");
                    documentoC = documentoC.replace("ñ", "n");

                    //Se ejecuta el analizador
                    Analizadores.CSS.Analisis_Lexico lexico_css = new Analizadores.CSS.Analisis_Lexico(new BufferedReader(new StringReader(documentoC)));
                    sintactico_css = new Analizadores.CSS.Analisis_Sintactico(lexico_css);
                    try {
                        sintactico_css.parse();
                        for (Estilo estilo : sintactico_css.estilos) {
                            if (estilos.containsKey(estilo.getNombre())) {
                                estilos.replace(estilo.getNombre(), estilo);
                            } else {
                                estilos.put(estilo.getNombre(), estilo);
                            }
                        }
                    } catch (Exception ex) {
                        Logger.getLogger(Test.class.getName()).log(Level.SEVERE, null, ex);
                    }
                } else {
                    JOptionPane.showMessageDialog(null, "Extension no compatible");
                }
            } else {
                errores.add("Error al leer el archivo");
            }
        } else {
            errores.add("El archivo no existe");
        }
    }

    private void analizarHtml() {
        File archivoC = new File(path + "public/index.html");
        String documentoC = "";
        if (archivoC.exists()) {
            if (archivoC.canRead()) {
                if (archivoC.getName().endsWith("html")) {
                    try {
                        try (BufferedReader in = new BufferedReader(
                                new InputStreamReader(
                                        new FileInputStream(archivoC), "UTF8"))) {
                            String str;
                            while ((str = in.readLine()) != null) {
                                documentoC += str + "\n";
                            }
                        }
                    } catch (IOException e) {
                    }
                    documentoC = documentoC.replace("á", "a");
                    documentoC = documentoC.replace("é", "e");
                    documentoC = documentoC.replace("í", "i");
                    documentoC = documentoC.replace("ó", "o");
                    documentoC = documentoC.replace("ú", "u");
                    documentoC = documentoC.replace("ñ", "n");

                    //Se ejecuta el analizador
                    Analizadores.HTML.Analisis_Lexico lexico_html = new Analizadores.HTML.Analisis_Lexico(new BufferedReader(new StringReader(documentoC)));
                    sintactico_html = new Analizadores.HTML.Analisis_Sintactico(lexico_html);
                    try {
                        sintactico_html.parse();

                    } catch (Exception ex) {
                        Logger.getLogger(Test.class
                                .getName()).log(Level.SEVERE, null, ex);
                    }
                } else {
                    JOptionPane.showMessageDialog(null, "Extension no compatible");
                }
            } else {
                errores.add("Error al leer el archivo");
            }
        } else {
            errores.add("El archivo no existe");
        }
    }

    private void recorrerRender(NodoSintactico raiz, Entorno ent) {
        switch (raiz.getNombre()) {
            case "INICIO":
                Entorno nuevo = new Entorno(ent);
                recorrerRender(raiz.getHijos().get(0), nuevo);
                break;
            case "OPCIONES":
                for (NodoSintactico hijo : raiz.getHijos()) {
                    recorrerRender(hijo, ent);
                }
                break;
            case "RENDER":
                String componenteRender = (String) raiz.getHijos().get(0).getValor();
                String divRender = (String) raiz.getHijos().get(1).getValor();
                if (componentesUfex.getComponentes().containsKey(componenteRender)) {
                    Boolean encontrado = false;
                    for (Div div : sintactico_html.divs) {
                        if (div.getNombre().equals(divRender)) {
                            encontrado = true;
                            break;
                        }
                    }
                    if (encontrado) {
                        Componente comp = componentesUfex.getComponentes().get(componenteRender);
                        if (paraGraficar.containsKey(divRender)) {
                            paraGraficar.replace(divRender, comp);
                        } else {
                            paraGraficar.put(divRender, comp);
                        }
                        error = "";
                    } else {
                        paraGraficar.put("ERROR", null);
                        error = sintactico_html.noufe;
                    }
                } else {
                    errores.add("El componente: " + componenteRender + " no existe");
                }
                break;
            default:
                break;
        }
    }

    private void recorrerImports(NodoSintactico raiz, Entorno ent) {
        switch (raiz.getNombre()) {
            case "INICIO":
                Entorno nuevo = new Entorno(ent);
                recorrerImports(raiz.getHijos().get(0), nuevo);
                break;
            case "OPCIONES":
                for (NodoSintactico hijo : raiz.getHijos()) {
                    recorrerImports(hijo, ent);
                }
                break;
            case "IMPORT_CSS":
                Expresion rutaC = resolverAritmetica(raiz.getHijos().get(0), ent);
                String direccionCSS = "ERROR";
                switch (rutaC.tipo) {
                    case cadena:
                        direccionCSS = (String) rutaC.valor;
                        direccionCSS = direccionCSS.replace("./", path);
                        break;
                    case error:
                        errores.add(String.valueOf(rutaC.valor));
                        break;
                    default:
                        errores.add("Se esperaba cadena");
                        break;
                }
                File archivoC = new File(direccionCSS);
                String documentoC = "";
                if (archivoC.exists()) {
                    if (archivoC.canRead()) {
                        if (archivoC.getName().endsWith("css")) {
                            try {
                                try (BufferedReader in = new BufferedReader(
                                        new InputStreamReader(
                                                new FileInputStream(archivoC), "UTF8"))) {
                                    String str;
                                    while ((str = in.readLine()) != null) {
                                        documentoC += str + "\n";
                                    }
                                }
                            } catch (IOException e) {
                            }
                            documentoC = documentoC.replace("á", "a");
                            documentoC = documentoC.replace("é", "e");
                            documentoC = documentoC.replace("í", "i");
                            documentoC = documentoC.replace("ó", "o");
                            documentoC = documentoC.replace("ú", "u");
                            documentoC = documentoC.replace("ñ", "n");

                            //Se ejecuta el analizador
                            Analizadores.CSS.Analisis_Lexico lexico_css = new Analizadores.CSS.Analisis_Lexico(new BufferedReader(new StringReader(documentoC)));
                            sintactico_css = new Analizadores.CSS.Analisis_Sintactico(lexico_css);
                            try {
                                sintactico_css.parse();
                                for (Estilo estilo : sintactico_css.estilos) {
                                    if (estilos.containsKey(estilo.getNombre())) {
                                        estilos.replace(estilo.getNombre(), estilo);
                                    } else {
                                        estilos.put(estilo.getNombre(), estilo);
                                    }
                                }
                            } catch (Exception ex) {
                                Logger.getLogger(Test.class
                                        .getName()).log(Level.SEVERE, null, ex);
                            }
                        } else {
                            JOptionPane.showMessageDialog(null, "Extension no compatible");
                        }
                    } else {
                        errores.add("Error al leer el archivo");
                    }
                } else {
                    errores.add("El archivo no existe");
                }
                break;
            case "IMPORT_COMPONENTE":
                String nombreComponente = (String) raiz.getHijos().get(0).getValor();
                Expresion ruta = resolverAritmetica(raiz.getHijos().get(1).getHijos().get(0), ent);
                String direccion = "ERROR";
                switch (ruta.tipo) {
                    case cadena:
                        direccion = (String) ruta.valor;
                        direccion = direccion.replace("./", path);
                        break;
                    case error:
                        errores.add(String.valueOf(ruta.valor));
                        break;
                    default:
                        errores.add("Se esperaba cadena");
                        break;
                }
                File archivo = new File(direccion);
                String documento = "";
                if (archivo.exists()) {
                    if (archivo.canRead()) {
                        if (archivo.getName().endsWith("ufe")) {
                            try {
                                try (BufferedReader in = new BufferedReader(
                                        new InputStreamReader(
                                                new FileInputStream(archivo), "UTF8"))) {
                                    String str;
                                    while ((str = in.readLine()) != null) {
                                        documento += str + "\n";
                                    }
                                }
                            } catch (IOException e) {
                            }
                            documento = documento.replace("á", "a");
                            documento = documento.replace("é", "e");
                            documento = documento.replace("í", "i");
                            documento = documento.replace("ó", "o");
                            documento = documento.replace("ú", "u");
                            documento = documento.replace("ñ", "n");

                            //Se ejecuta el analizador
                            Analizadores.UFE.Analisis_Lexico lexico_ufe = new Analizadores.UFE.Analisis_Lexico(new BufferedReader(new StringReader(documento)));
                            Analizadores.UFE.Analisis_Sintactico sintactico_ufe = new Analizadores.UFE.Analisis_Sintactico(lexico_ufe);
                            try {
                                sintactico_ufe.parse();
                                Ejecutor ejecutor = new Ejecutor();
                                ejecutor.Ejecutar(sintactico_ufe.padre, "Main", path);
                                if (ejecutor.componentesUfex.getComponentes().containsKey(nombreComponente)) {
                                    Componente compo = ejecutor.componentesUfex.getComponentes().get(nombreComponente);
                                    componentesUfex.getComponentes().put(nombreComponente, compo);

                                }
                                for (String errorX : ejecutor.errores) {
                                    errores.add(direccion + "     " + errorX);
                                }
                            } catch (Exception ex) {
                                Logger.getLogger(Test.class
                                        .getName()).log(Level.SEVERE, null, ex);
                            }

                        } else {
                            JOptionPane.showMessageDialog(null, "Extension no compatible");
                        }
                    } else {
                        errores.add("Error al leer el archivo");
                    }
                } else {
                    errores.add("El archivo no existe");
                }
                break;
            default:
                break;
        }
    }

    private void recorrer(NodoSintactico raiz, Entorno ent, Componente componente) {
        Expresion resultado;
        Entorno nuevo;
        boolean boleano;
        switch (raiz.getNombre()) {
            case "INICIO":
                nuevo = new Entorno(ent);
                recorrer(raiz.getHijos().get(0), nuevo, componente);
                break;
            case "FUNCION":
            case "SINO":
            case "SINO-SI":
            case "OPCIONES":
            case "ASIGNACIONES":
            case "FUNCIONES":
            case "INSTRUCCIONES":
            case "OPCIONESLISTA":
            case "ELEMENTOS":
                for (NodoSintactico hijo : raiz.getHijos()) {
                    recorrer(hijo, ent, componente);
                }
                break;
            case "E":
                Expresion hola = resolverExpresion(raiz.getHijos().get(0), ent);
                break;
            case "SDA":
            case "ADA":
            case "AD":
            case "SD":
                //SDA - Simple Declaracion Asignacion
                //ADA - Arreglo Declaracion Asignacion
                //AD - Arreglo Declaracion
                //SD - Simple Declaracion
                EjecutarDeclaracion(raiz, ent);
                break;
            case "SI":
                resultado = resolverExpresion(raiz.getHijos().get(0), ent);
                switch (resultado.tipo) {
                    case booleano:
                        boleano = (boolean) resultado.valor;
                        if (boleano) {
                            nuevo = new Entorno(ent);
                            recorrer(raiz.getHijos().get(1), nuevo, componente);
                        }
                        break;
                    default:
                        errores.add(String.valueOf(resultado.valor));
                        break;
                }
                break;
            case "SI-SINO":
                resultado = resolverExpresion(raiz.getHijos().get(0), ent);
                switch (resultado.tipo) {
                    case booleano:
                        boleano = (boolean) resultado.valor;
                        if (boleano) {
                            nuevo = new Entorno(ent);
                            recorrer(raiz.getHijos().get(1), nuevo, componente);
                        } else {
                            nuevo = new Entorno(ent);
                            recorrer(raiz.getHijos().get(2), nuevo, componente);
                        }
                        break;
                    default:
                        errores.add(String.valueOf(resultado.valor));
                        break;
                }
                break;
            case "IMPRIMIR":
                resultado = resolverExpresion(raiz.getHijos().get(0), ent);
                if (resultado.tipo != Simbolo.EnumTipo.error) {
                    System.out.println(resultado.valor);
                    errores.add(String.valueOf(resultado.valor));
                } else {
                    errores.add(String.valueOf(resultado.valor));
                }
                break;
            case "REPETIR":
                if (raiz.getHijos().size() == 2) {
                    nuevo = new Entorno(ent);
                    EjecutarRepetir(raiz, nuevo, componente);
                }
                break;
            case "MIENTRAS":
                if (raiz.getHijos().size() == 2) {
                    nuevo = new Entorno(ent);
                    EjecutarMientras(raiz, nuevo, componente);
                }
                break;
            case "REASIGNACION":
                EjecutarReasignacion(raiz, ent);
                break;
            case "COMPONENTE":
                nuevo = new Entorno(ent);
                String nombre = (String) raiz.getHijos().get(0).getValor();
                nombreComponenteActual = nombre;
                Componente nuevoComponente = new Componente(Componente.EnumTipo.ufex);
                nuevoComponente.setNombre(nombre);
                for (int i = 1; i < raiz.getHijos().size(); i++) {
                    recorrer(raiz.getHijos().get(i), nuevo, nuevoComponente);
                }
                break;
            case "RETORNO":
                NodoSintactico nuevaRaiz = raiz.getHijos().get(0);
                nuevaRaiz.getHijos().forEach((hijo) -> {
                    componente.getComponentes().add(EjecutarComponente(hijo, ent));
                });
                if (!componentesUfex.getComponentes().containsKey(nombreComponenteActual)) {
                    componentesUfex.getComponentes().put(nombreComponenteActual, componente);
                } else {
                    errores.add("Componente: " + nombreComponenteActual + " ya existe");
                }
                break;
            case "ITEMS":
                for (NodoSintactico hijo : raiz.getHijos()) {
                    String valorItem = "";
                    if (hijo.getHijos().size() > 0) {
                        Expresion exp = resolverExpresion(hijo.getHijos().get(0), ent);
                        if (exp.tipo != Simbolo.EnumTipo.error) {
                            valorItem = String.valueOf(exp.valor);
                        } else {
                            errores.add(String.valueOf(exp.valor));
                        }
                    } else {
                        valorItem = (String) hijo.getValor();
                    }
                    if (!valorItem.isEmpty()) {
                        componente.getItems().add(valorItem);
                    }
                }
                break;
            case "DEFECTO":
                Expresion defecto = resolverAritmetica(raiz.getHijos().get(0).getHijos().get(0), ent);
                if (defecto.tipo == Simbolo.EnumTipo.entero) {
                    int numeroDefecto = (int) defecto.valor;
                    componente.setDefecto(numeroDefecto);
                } else {
                    errores.add("Se esperaba tipo Entero");
                }
                break;
            default:
                break;
        }
    }

    private Componente EjecutarComponente(NodoSintactico raiz, Entorno ent) {
        Componente retorno = null;
        switch (raiz.getNombre()) {
            case "PANEL":
                retorno = new Componente(Componente.EnumTipo.panel);
                if (raiz.getHijos().size() == 1) {
                    if (raiz.getHijos().get(0).getNombre().equals("ELEMENTOS")) {
                        EjecutarElementos(retorno, raiz.getHijos().get(0), ent);
                    } else {
                        EjecutarPropiedades(retorno, raiz.getHijos().get(0), ent);
                    }
                } else {
                    EjecutarPropiedades(retorno, raiz.getHijos().get(0), ent);
                    EjecutarElementos(retorno, raiz.getHijos().get(1), ent);
                }
                break;
            case "TEXT":
                retorno = new Componente(Componente.EnumTipo.text);
                EjecutarPropiedades(retorno, raiz.getHijos().get(0), ent);
                if (raiz.getHijos().size() > 1) {
                    EjecutarContenido(retorno, raiz.getHijos().get(1), ent);
                }
                break;
            case "TEXTFIELD":
                retorno = new Componente(Componente.EnumTipo.textfield);
                EjecutarPropiedades(retorno, raiz.getHijos().get(0), ent);
                if (raiz.getHijos().size() > 1) {
                    EjecutarContenido(retorno, raiz.getHijos().get(1), ent);
                }
                break;
            case "BUTTON":
                retorno = new Componente(Componente.EnumTipo.button);
                EjecutarPropiedades(retorno, raiz.getHijos().get(0), ent);
                if (raiz.getHijos().size() > 1) {
                    EjecutarNombre(retorno, raiz.getHijos().get(1), ent);
                }
                break;
            case "IMAGEN":
                retorno = new Componente(Componente.EnumTipo.image);
                EjecutarPropiedades(retorno, raiz.getHijos().get(0), ent);
                break;
            case "SPINNER":
                retorno = new Componente(Componente.EnumTipo.spinner);
                EjecutarPropiedades(retorno, raiz.getHijos().get(0), ent);
                if (raiz.getHijos().size() > 1) {
                    EjecutarDefault(retorno, raiz.getHijos().get(1), ent);
                }
                break;
            case "LISTA":
                retorno = new Componente(Componente.EnumTipo.list);
                EjecutarPropiedades(retorno, raiz.getHijos().get(0), ent);
                recorrer(raiz.getHijos().get(1), ent, retorno);
                if (retorno.getDefecto() >= 0) {
                    if ((retorno.getDefecto()) >= retorno.getItems().size()) {
                        errores.add("Index fuera de los limites");
                        retorno.setDefecto(0);
                    }
                } else {
                    errores.add("Se esperaba un index positivo");
                    retorno.setDefecto(0);
                }
                break;
            case "EXTERNO":
                String nombreComponente = (String) raiz.getHijos().get(0).getValor();
                if (componentesUfex.getComponentes().containsKey(nombreComponente)) {
                    retorno = componentesUfex.getComponentes().get(nombreComponente);
                }
                break;
            default:
                break;
        }
        return retorno;
    }

    private void EjecutarContenido(Componente comp, NodoSintactico raiz, Entorno ent) {
        if (raiz.getHijos().size() > 0) {
            Expresion expresion = resolverExpresion(raiz.getHijos().get(0), ent);
            if (expresion.tipo != Simbolo.EnumTipo.error) {
                comp.setContenido(String.valueOf(expresion.valor));
            }
        } else {
            comp.setContenido(String.valueOf(raiz.getValor()));
        }
    }

    private void EjecutarNombre(Componente comp, NodoSintactico raiz, Entorno ent) {
        if (raiz.getHijos().size() > 0) {
            Expresion expresion = resolverExpresion(raiz.getHijos().get(0), ent);
            if (expresion.tipo != Simbolo.EnumTipo.error) {
                comp.setNombre(String.valueOf(expresion.valor));
            }
        } else {
            comp.setNombre(String.valueOf(raiz.getValor()));
        }
    }

    private void EjecutarDefault(Componente comp, NodoSintactico raiz, Entorno ent) {
        Expresion exp = resolverAritmetica(raiz, ent);
        switch (exp.tipo) {
            case entero:
                int valor = (int) exp.valor;
                comp.setDefecto(valor);
                break;
            case error:
                errores.add(String.valueOf(exp.valor));
                break;
            default:
                errores.add("Se esperaba tipo entero");
                break;
        }
    }

    private void EjecutarElementos(Componente comp, NodoSintactico raiz, Entorno ent) {
        raiz.getHijos().forEach((hijo) -> {
            Componente nuevo = null;
            String nombreComponente = (String) hijo.getHijos().get(0).getValor();
            if (hijo.getNombre().equals("EXTERNO")) {
                if (componentesUfex.getComponentes().containsKey(nombreComponente)) {
                    nuevo = componentesUfex.getComponentes().get(nombreComponente);
                    comp.getComponentes().add(nuevo);
                } else {
                    errores.add("Componente no encontrado");
                }
            } else {
                comp.getComponentes().add(EjecutarComponente(hijo, ent));
            }
        });
    }

    private void EjecutarPropiedades(Componente comp, NodoSintactico raiz, Entorno ent) {
        Expresion exp;
        Boolean aplicado = false;
        for (NodoSintactico hijo : raiz.getHijos()) {
            if (aplicado) {
                break;
            }
            switch (hijo.getNombre()) {
                case "AT_ID":
                    comp.setId(String.valueOf(hijo.getValor()));
                    break;
                case "AT_X":
                    exp = resolverAritmetica(hijo.getHijos().get(0).getHijos().get(0), ent);
                    comp.setPosX(Integer.valueOf(String.valueOf(exp.valor)));
                    break;
                case "AT_Y":
                    exp = resolverAritmetica(hijo.getHijos().get(0).getHijos().get(0), ent);
                    comp.setPosY(Integer.valueOf(String.valueOf(exp.valor)));
                    break;
                case "AT_HEIGHT":
                    exp = resolverAritmetica(hijo.getHijos().get(0).getHijos().get(0), ent);
                    comp.setHeight(Integer.valueOf(String.valueOf(exp.valor)));
                    break;
                case "AT_WIDTH":
                    exp = resolverAritmetica(hijo.getHijos().get(0).getHijos().get(0), ent);
                    comp.setWidth(Integer.valueOf(String.valueOf(exp.valor)));
                    break;
                case "AT_COLOR":
                    exp = resolverAritmetica(hijo.getHijos().get(0), ent);
                    comp.setColor(String.valueOf(hijo.getHijos().get(0).getValor()));
                    break;
                case "AT_BORDER":
                    exp = resolverAritmetica(hijo.getHijos().get(0).getHijos().get(0), ent);
                    comp.setBorder(Integer.valueOf(String.valueOf(exp.valor)));
                    break;
                case "AT_MIN":
                    exp = resolverAritmetica(hijo.getHijos().get(0).getHijos().get(0), ent);
                    comp.setMin(Integer.valueOf(String.valueOf(exp.valor)));
                    break;
                case "AT_MAX":
                    exp = resolverAritmetica(hijo.getHijos().get(0).getHijos().get(0), ent);
                    comp.setMax(Integer.valueOf(String.valueOf(exp.valor)));
                    break;
                case "AT_FUENTE":
                    exp = resolverAritmetica(hijo.getHijos().get(0).getHijos().get(0), ent);
                    comp.setFuente(String.valueOf(exp.valor));
                    break;
                case "AT_ONCLICK":
                    exp = resolverExpresion(hijo.getHijos().get(0).getHijos().get(0), ent);
                    comp.setOnClick(String.valueOf(exp.valor));
                    break;
                case "AT_CLASSNAME":
                    String nombreCss = (String) hijo.getHijos().get(0).getValor();
                    String[] style = nombreCss.split(" ");
                    boolean encontrado = false;
                    for (int i = 0; i < style.length; i++) {
                        if (estilos.containsKey(style[i])) {
                            encontrado = true;
                        } else {
                            errores.add("Estilo: " + style[i] + " no encontrado");
                            encontrado = false;
                            break;
                        }
                    }
                    if (encontrado) {
                        for (int i = 0; i < style.length; i++) {
                            if (estilos.containsKey(style[i])) {
                                comp.getEstilos().add(estilos.get(style[i]));
                                aplicado = true;
                            } else {
                                aplicado = false;
                            }
                        }
                    }
                    comp.setClassName(nombreCss);
                    break;
            }
        }
    }

    private Expresion resolverAritmetica(NodoSintactico raiz, Entorno ent) {
        switch (raiz.getNombre()) {
            case "COLOR":
                return new Expresion(Simbolo.EnumTipo.cadena, raiz.getValor());
            case "ENTERO":
                return new Expresion(Simbolo.EnumTipo.entero, raiz.getValor());
            case "DOBLE":
                return new Expresion(Simbolo.EnumTipo.doble, raiz.getValor());
            case "CADENA":
                return new Expresion(Simbolo.EnumTipo.cadena, raiz.getValor());
            case "ID":
                if (raiz.getHijos().isEmpty()) {
                    Simbolo sim = ent.buscar(String.valueOf(raiz.getValor()), raiz.getLinea(), raiz.getColumna());
                    if (sim != null) {
                        if (sim.tipo != Simbolo.EnumTipo.vacio) {
                            return new Expresion(sim.tipo, sim.valor);
                        } else {
                            return new Expresion(Simbolo.EnumTipo.error, "Variable Vacia");
                        }
                    }
                    return new Expresion(Simbolo.EnumTipo.error, "La Variable no existe");
                } else {
                    Expresion index = resolverAritmetica(raiz.getHijos().get(0), ent);
                    if (index.tipo == Simbolo.EnumTipo.entero) {
                        String nombreVariable = String.valueOf(index.valor) + "_" + String.valueOf(raiz.getValor());
                        Simbolo var = ent.buscar(String.valueOf(raiz.getValor()), raiz.getLinea(), raiz.getColumna());
                        if (var != null) {
                            int maximo = (int) var.valor;
                            int indice = (int) index.valor;
                            if (indice < maximo) {
                                if (indice >= 0) {
                                    Simbolo sim = ent.buscar(nombreVariable, raiz.getLinea(), raiz.getColumna());
                                    if (sim != null) {
                                        return new Expresion(sim.tipo, sim.valor);
                                    } else {
                                        return new Expresion(Simbolo.EnumTipo.error, "Variable Vacia");
                                    }
                                } else {
                                    return new Expresion(Simbolo.EnumTipo.error, "Indice debe ser un Entero Positivo");
                                }
                            } else {
                                return new Expresion(Simbolo.EnumTipo.error, "Indice fuera del limite");
                            }
                        } else {
                            return new Expresion(Simbolo.EnumTipo.error, "La Variable no Existe");
                        }
                    } else {
                        return new Expresion(Simbolo.EnumTipo.error, "Se esperaba un indice tipo entero, tipo " + index.tipo + " encontrado");
                    }
                }
            case "CARACTER":
                return new Expresion(Simbolo.EnumTipo.caracter, raiz.getValor());
            case "BOOLEANO":
                return new Expresion(Simbolo.EnumTipo.booleano, raiz.getValor());
            case "+":
                return OperarSuma(resolverAritmetica(raiz.getHijos().get(0), ent), resolverAritmetica(raiz.getHijos().get(1), ent));
            case "-":
                return OperarResta(resolverAritmetica(raiz.getHijos().get(0), ent), resolverAritmetica(raiz.getHijos().get(1), ent));
            case "*":
                return OperarMultiplicacion(resolverAritmetica(raiz.getHijos().get(0), ent), resolverAritmetica(raiz.getHijos().get(1), ent));
            case "/":
                return OperarDivision(resolverAritmetica(raiz.getHijos().get(0), ent), resolverAritmetica(raiz.getHijos().get(1), ent));
            case "NEGATIVO":
                return OperarNegativo(resolverAritmetica(raiz.getHijos().get(0), ent));
            case "pow":
                return OperarPotencia(resolverAritmetica(raiz.getHijos().get(0), ent), resolverAritmetica(raiz.getHijos().get(1), ent));
            case "||":
                return OperarOrLogico(resolverAritmetica(raiz.getHijos().get(0), ent), resolverAritmetica(raiz.getHijos().get(1), ent));
            case "&&":
                return OperarAndLogico(resolverAritmetica(raiz.getHijos().get(0), ent), resolverAritmetica(raiz.getHijos().get(1), ent));
            case "!":
                return OperarNotLogico(resolverAritmetica(raiz.getHijos().get(0), ent));
            case "^":
                return OperarXorLogico(resolverAritmetica(raiz.getHijos().get(0), ent), resolverAritmetica(raiz.getHijos().get(1), ent));
            case ">":
                return OperarMayorQue(resolverAritmetica(raiz.getHijos().get(0), ent), resolverAritmetica(raiz.getHijos().get(1), ent));
            case "<":
                return OperarMenorQue(resolverAritmetica(raiz.getHijos().get(0), ent), resolverAritmetica(raiz.getHijos().get(1), ent));
            case ">=":
                return OperarMayorIgualQue(resolverAritmetica(raiz.getHijos().get(0), ent), resolverAritmetica(raiz.getHijos().get(1), ent));
            case "<=":
                return OperarMenorIgualQue(resolverAritmetica(raiz.getHijos().get(0), ent), resolverAritmetica(raiz.getHijos().get(1), ent));
            case "==":
                return OperarIgual(resolverAritmetica(raiz.getHijos().get(0), ent), resolverAritmetica(raiz.getHijos().get(1), ent));
            case "!=":
                return OperarDiferente(resolverAritmetica(raiz.getHijos().get(0), ent), resolverAritmetica(raiz.getHijos().get(1), ent));
        }
        return new Expresion(Simbolo.EnumTipo.error, "ERROR");
    }

    private void EjecutarReasignacion(NodoSintactico raiz, Entorno ent) {
        String nombre = (String) raiz.getHijos().get(0).getValor();
        Expresion exp = null;
        if (raiz.getHijos().size() == 2) {
            Simbolo sim;
            if (raiz.getHijos().get(0).getHijos().size() == 1) {
                exp = resolverExpresion(raiz.getHijos().get(0).getHijos().get(0), ent);
                if (exp.tipo == Simbolo.EnumTipo.entero) {
                    sim = ent.buscar(exp.valor + "_" + nombre, raiz.getLinea(), raiz.getColumna());
                } else {
                    sim = null;
                }
            } else {
                sim = ent.buscar(nombre, raiz.getLinea(), raiz.getColumna());
            }
            if (sim != null) {
                Expresion resultado = resolverExpresion(raiz.getHijos().get(1), ent);
                if (resultado.tipo != Simbolo.EnumTipo.error) {
                    sim = new Simbolo(resultado.tipo, resultado.valor);
                    if (exp != null) {
                        if (!ent.modificar(exp.valor + "_" + nombre, sim)) {
                            errores.add("Error al reasignar la variable");
                        }
                    } else {
                        if (!ent.modificar(nombre, sim)) {
                            errores.add("Error al reasignar la variable");
                        }
                    }
                } else {
                    errores.add(String.valueOf(resultado.tipo));
                }
            } else {
                errores.add("La variable no existe");
            }
        } else {
            Expresion index = resolverExpresion(raiz.getHijos().get(1), ent);
            switch (index.tipo) {
                case entero:
                    String in = String.valueOf(index.valor);
                    Simbolo sim = ent.buscar(in + "_" + nombre, raiz.getLinea(), raiz.getColumna());
                    if (sim != null) {
                        Expresion resultado = resolverExpresion(raiz.getHijos().get(2), ent);
                        if (resultado.tipo != Simbolo.EnumTipo.error) {
                            sim = new Simbolo(resultado.tipo, resultado.valor);
                            if (!ent.modificar(in + "_" + nombre, sim)) {
                                errores.add("Error al reasignar la variable");
                            }
                        } else {
                            errores.add(String.valueOf(resultado.tipo));
                        }
                    } else {
                        errores.add("La variable no existe");
                    }
                    break;
                case error:
                    errores.add(String.valueOf(index.valor));
                    break;
                default:
                    errores.add("Se esperaba valor entero");
                    break;
            }
        }
    }

    private void EjecutarMientras(NodoSintactico raiz, Entorno ent, Componente componente) {
        Expresion resultado = resolverExpresion(raiz.getHijos().get(0), ent);
        switch (resultado.tipo) {
            case booleano:
                boolean comprobador = (boolean) resultado.valor;
                while (comprobador) {
                    Entorno nuevo = new Entorno(ent);
                    recorrer(raiz.getHijos().get(1), nuevo, componente);
                    resultado = resolverExpresion(raiz.getHijos().get(0), ent);
                    if (String.valueOf(resultado.valor).toLowerCase().equals("true")) {
                        comprobador = true;
                    } else if (String.valueOf(resultado.valor).toLowerCase().equals("false")) {
                        comprobador = false;
                    } else {
                        comprobador = false;
                    }
                }
                break;
            case error:
                errores.add(String.valueOf(resultado.valor));
                break;
            default:
                errores.add("Se esperaba tipo booleano, tipo " + resultado.tipo + " encontrado");
                break;
        }
    }

    private void EjecutarRepetir(NodoSintactico raiz, Entorno ent, Componente componente) {
        Expresion resultado = resolverExpresion(raiz.getHijos().get(0), ent);
        switch (resultado.tipo) {
            case entero:
                int contador = (int) resultado.valor;
                while (contador > 0) {
                    Entorno nuevo = new Entorno(ent);
                    recorrer(raiz.getHijos().get(1), nuevo, componente);
                    contador--;
                }
                break;
            case error:
                errores.add(String.valueOf(resultado.valor));
                break;
            default:
                errores.add("Se esperaba tipo entero, tipo " + resultado.tipo + " encontrado");
                break;
        }
    }

    private void EjecutarDeclaracion(NodoSintactico raiz, Entorno ent) {
        Expresion resultado;
        Expresion index;
        Simbolo nuevo;
        switch (raiz.getNombre()) {
            case "SDA":
                resultado = resolverExpresion(raiz.getHijos().get(1), ent);
                if (resultado.tipo != Simbolo.EnumTipo.error) {
                    nuevo = new Simbolo(resultado.tipo, resultado.valor);
                    if (!ent.insertar(String.valueOf(raiz.getHijos().get(0).getValor()), nuevo, raiz.getLinea(), raiz.getColumna())) {
                        errores.add("Error la variable ya existe");
                    }
                } else {
                    errores.add(String.valueOf(resultado.valor));
                }
                break;
            case "SD":
                nuevo = new Simbolo(Simbolo.EnumTipo.vacio, null);
                if (!ent.insertar(String.valueOf(raiz.getHijos().get(0).getValor()), nuevo, raiz.getLinea(), raiz.getColumna())) {
                    errores.add("Error la variable ya existe");
                }
                break;
            case "ADA":
                if (!raiz.getHijos().get(1).getNombre().equals("DATARR")) {
                    index = resolverExpresion(raiz.getHijos().get(1), ent);
                    switch (index.tipo) {
                        case entero:
                            resultado = resolverExpresion(raiz.getHijos().get(2), ent);
                            int repeticiones = (int) index.valor - 1;
                            nuevo = new Simbolo(index.tipo, index.valor);
                            if (!ent.insertar(String.valueOf(raiz.getHijos().get(0).getValor()), nuevo, raiz.getLinea(), raiz.getColumna())) {
                                errores.add("Error la variable ya existe");
                                break;
                            } else {
                                while (0 <= repeticiones) {
                                    String nombreVariable = repeticiones + "_" + String.valueOf(raiz.getHijos().get(0).getValor());
                                    nuevo = new Simbolo(resultado.tipo, resultado.valor);
                                    ent.insertar(nombreVariable, nuevo, raiz.getLinea(), raiz.getColumna());
                                    repeticiones--;
                                }
                            }
                            break;
                        case error:
                            errores.add(String.valueOf(index.valor));
                            break;
                        default:
                            errores.add("Se esperaba indice entero");
                            break;
                    }
                } else {
                    NodoSintactico elementos = raiz.getHijos().get(1);
                    int hijos = elementos.getHijos().size();
                    int error = 0;
                    nuevo = new Simbolo(Simbolo.EnumTipo.entero, elementos.getHijos().size());
                    if (!ent.insertar(String.valueOf(raiz.getHijos().get(0).getValor()), nuevo, raiz.getLinea(), raiz.getColumna())) {
                        errores.add("Error la variable ya existe");
                        break;
                    } else {
                        for (int i = 0; i < hijos; i++) {
                            resultado = resolverExpresion(elementos.getHijos().get(i), ent);
                            if (resultado == null) {
                                error++;
                                break;
                            } else if (resultado.tipo == Simbolo.EnumTipo.error) {
                                error++;
                                break;
                            }
                        }
                        if (error == 0) {
                            for (int i = 0; i < hijos; i++) {
                                resultado = resolverExpresion(elementos.getHijos().get(i), ent);
                                String nombreVariable = i + "_" + String.valueOf(raiz.getHijos().get(0).getValor());
                                nuevo = new Simbolo(resultado.tipo, resultado.valor);
                                ent.insertar(nombreVariable, nuevo, raiz.getLinea(), raiz.getColumna());
                            }
                        } else {
                            errores.add("La variable no ha sido inicializada");
                        }
                    }

                }
                break;
            case "AD":
                index = resolverExpresion(raiz.getHijos().get(1), ent);
                switch (index.tipo) {
                    case entero:
                        int repeticiones = (int) index.valor - 1;
                        nuevo = new Simbolo(index.tipo, index.valor);
                        if (!ent.insertar(String.valueOf(raiz.getHijos().get(0).getValor()), nuevo, raiz.getLinea(), raiz.getColumna())) {
                            errores.add("Error la variable ya existe");
                            break;
                        } else {
                            while (0 <= repeticiones) {
                                String nombreVariable = repeticiones + "_" + String.valueOf(raiz.getHijos().get(0).getValor());
                                nuevo = new Simbolo(Simbolo.EnumTipo.entero, 0);
                                ent.insertar(nombreVariable, nuevo, raiz.getLinea(), raiz.getColumna());
                                repeticiones--;
                            }
                        }
                        break;
                    case error:
                        errores.add(String.valueOf(index.valor));
                        break;
                    default:
                        errores.add("Se esperaba indice entero");
                        break;
                }
                break;
        }
    }

    private Expresion resolverExpresion(NodoSintactico raiz, Entorno ent) {
        switch (raiz.getNombre()) {
            case "ENTERO":
                return new Expresion(Simbolo.EnumTipo.entero, raiz.getValor());
            case "DOBLE":
                return new Expresion(Simbolo.EnumTipo.doble, raiz.getValor());
            case "CADENA":
                return new Expresion(Simbolo.EnumTipo.cadena, raiz.getValor());
            case "ID":
                if (raiz.getHijos().isEmpty()) {
                    Simbolo sim = ent.buscar(String.valueOf(raiz.getValor()), raiz.getLinea(), raiz.getColumna());
                    if (sim != null) {
                        if (sim.tipo != Simbolo.EnumTipo.vacio) {
                            return new Expresion(sim.tipo, sim.valor);
                        } else {
                            return new Expresion(Simbolo.EnumTipo.error, "Variable Vacia");
                        }
                    }
                    return new Expresion(Simbolo.EnumTipo.error, "La Variable no existe");
                } else {
                    Expresion index = resolverExpresion(raiz.getHijos().get(0), ent);
                    if (index.tipo == Simbolo.EnumTipo.entero) {
                        String nombreVariable = String.valueOf(index.valor) + "_" + String.valueOf(raiz.getValor());
                        Simbolo var = ent.buscar(String.valueOf(raiz.getValor()), raiz.getLinea(), raiz.getColumna());
                        if (var != null) {
                            int maximo = (int) var.valor;
                            int indice = (int) index.valor;
                            if (indice < maximo) {
                                if (indice >= 0) {
                                    Simbolo sim = ent.buscar(nombreVariable, raiz.getLinea(), raiz.getColumna());
                                    if (sim != null) {
                                        return new Expresion(sim.tipo, sim.valor);
                                    } else {
                                        return new Expresion(Simbolo.EnumTipo.error, "Variable Vacia");
                                    }
                                } else {
                                    return new Expresion(Simbolo.EnumTipo.error, "Indice debe ser un Entero Positivo");
                                }
                            } else {
                                return new Expresion(Simbolo.EnumTipo.error, "Indice fuera del limite");
                            }
                        } else {
                            return new Expresion(Simbolo.EnumTipo.error, "La Variable no Existe");
                        }
                    } else {
                        return new Expresion(Simbolo.EnumTipo.error, "Se esperaba un indice tipo entero, tipo " + index.tipo + " encontrado");
                    }
                }
            case "CARACTER":
                return new Expresion(Simbolo.EnumTipo.caracter, raiz.getValor());
            case "BOOLEANO":
                return new Expresion(Simbolo.EnumTipo.booleano, raiz.getValor());
            case "+":
                return OperarSuma(resolverExpresion(raiz.getHijos().get(0), ent), resolverExpresion(raiz.getHijos().get(1), ent));
            case "-":
                return OperarResta(resolverExpresion(raiz.getHijos().get(0), ent), resolverExpresion(raiz.getHijos().get(1), ent));
            case "*":
                return OperarMultiplicacion(resolverExpresion(raiz.getHijos().get(0), ent), resolverExpresion(raiz.getHijos().get(1), ent));
            case "/":
                return OperarDivision(resolverExpresion(raiz.getHijos().get(0), ent), resolverExpresion(raiz.getHijos().get(1), ent));
            case "NEGATIVO":
                return OperarNegativo(resolverExpresion(raiz.getHijos().get(0), ent));
            case "pow":
                return OperarPotencia(resolverExpresion(raiz.getHijos().get(0), ent), resolverExpresion(raiz.getHijos().get(1), ent));
            case "||":
                return OperarOrLogico(resolverExpresion(raiz.getHijos().get(0), ent), resolverExpresion(raiz.getHijos().get(1), ent));
            case "&&":
                return OperarAndLogico(resolverExpresion(raiz.getHijos().get(0), ent), resolverExpresion(raiz.getHijos().get(1), ent));
            case "!":
                return OperarNotLogico(resolverExpresion(raiz.getHijos().get(0), ent));
            case "^":
                return OperarXorLogico(resolverExpresion(raiz.getHijos().get(0), ent), resolverExpresion(raiz.getHijos().get(1), ent));
            case ">":
                return OperarMayorQue(resolverExpresion(raiz.getHijos().get(0), ent), resolverExpresion(raiz.getHijos().get(1), ent));
            case "<":
                return OperarMenorQue(resolverExpresion(raiz.getHijos().get(0), ent), resolverExpresion(raiz.getHijos().get(1), ent));
            case ">=":
                return OperarMayorIgualQue(resolverExpresion(raiz.getHijos().get(0), ent), resolverExpresion(raiz.getHijos().get(1), ent));
            case "<=":
                return OperarMenorIgualQue(resolverExpresion(raiz.getHijos().get(0), ent), resolverExpresion(raiz.getHijos().get(1), ent));
            case "==":
                return OperarIgual(resolverExpresion(raiz.getHijos().get(0), ent), resolverExpresion(raiz.getHijos().get(1), ent));
            case "!=":
                return OperarDiferente(resolverExpresion(raiz.getHijos().get(0), ent), resolverExpresion(raiz.getHijos().get(1), ent));
        }
        return new Expresion(Simbolo.EnumTipo.error, "ERROR");
    }

    private Expresion OperarSuma(Expresion expresion1, Expresion expresion2) {
        switch (expresion1.tipo) {
            case cadena:
                //Cadena (Entero || Doble || Caracter || Booleano || Cadena)
                return new Expresion(Simbolo.EnumTipo.cadena, String.valueOf(expresion1.valor) + String.valueOf(expresion2.valor));
            case doble:
                switch (expresion2.tipo) {
                    case cadena:
                        //Doble Cadena
                        return new Expresion(Simbolo.EnumTipo.cadena, String.valueOf(expresion1.valor) + String.valueOf(expresion2.valor));
                    case entero:
                    case doble:
                        //Doble Entero
                        return new Expresion(Simbolo.EnumTipo.doble, Double.valueOf(String.valueOf(expresion1.valor)) + Double.valueOf(String.valueOf(expresion2.valor)));
                    case caracter:
                        //Doble Caracter
                        return new Expresion(Simbolo.EnumTipo.doble, Double.valueOf(String.valueOf(expresion1.valor)) + Double.valueOf(String.valueOf(expresion2.valor).charAt(0)));
                    case error:
                        return expresion2;
                    default:
                        String cadena = "Suma no definida entre los tipos " + expresion1.tipo + " y " + expresion2.tipo;
                        return new Expresion(Simbolo.EnumTipo.error, cadena);
                }
            case entero:
                switch (expresion2.tipo) {
                    case entero:
                        //Entero Entero
                        return new Expresion(Simbolo.EnumTipo.entero, Integer.valueOf(String.valueOf(expresion1.valor)) + Integer.valueOf(String.valueOf(expresion2.valor)));
                    case cadena:
                        //Entero Cadena
                        return new Expresion(Simbolo.EnumTipo.cadena, String.valueOf(expresion1.valor) + String.valueOf(expresion2.valor));
                    case caracter:
                        //Entero Caracter
                        return new Expresion(Simbolo.EnumTipo.doble, Integer.valueOf(String.valueOf(expresion1.valor)) + Integer.valueOf(String.valueOf(expresion2.valor).charAt(0)));
                    case doble:
                        //Entero Doble
                        return new Expresion(Simbolo.EnumTipo.doble, Double.valueOf(String.valueOf(expresion1.valor)) + Double.valueOf(String.valueOf(expresion2.valor)));
                    case error:
                        return expresion2;
                    default:
                        String cadena = "Suma no definida entre los tipos " + expresion1.tipo + " y " + expresion2.tipo;
                        return new Expresion(Simbolo.EnumTipo.error, cadena);
                }
            case caracter:
                switch (expresion2.tipo) {
                    case cadena:
                        //Caracter Cadena
                        return new Expresion(Simbolo.EnumTipo.cadena, String.valueOf(expresion1.valor) + String.valueOf(expresion2.valor));
                    case doble:
                        //Caracter Doble
                        return new Expresion(Simbolo.EnumTipo.doble, Double.valueOf(String.valueOf(expresion1.valor)) + Double.valueOf(String.valueOf(expresion2.valor)));
                    case entero:
                        //Caracter Entero
                        return new Expresion(Simbolo.EnumTipo.entero, Integer.valueOf(String.valueOf(expresion1.valor)) + Integer.valueOf(String.valueOf(expresion2.valor)));
                    case caracter:
                        //Caracter Caracter
                        return new Expresion(Simbolo.EnumTipo.entero, Integer.valueOf(String.valueOf(expresion1.valor)) + Integer.valueOf(String.valueOf(expresion2.valor)));
                    case error:
                        return expresion2;
                    default:
                        String cadena = "Suma no definida entre los tipos " + expresion1.tipo + " y " + expresion2.tipo;
                        return new Expresion(Simbolo.EnumTipo.error, cadena);
                }
            case error:
                return expresion1;
            default:
                String cadena = "Suma no definida entre los tipos " + expresion1.tipo + " y " + expresion2.tipo;
                return new Expresion(Simbolo.EnumTipo.error, cadena);
        }
    }

    private Expresion OperarResta(Expresion expresion1, Expresion expresion2) {
        switch (expresion1.tipo) {
            case doble:
                switch (expresion2.tipo) {
                    case entero:
                    case doble:
                        //Doble Entero
                        return new Expresion(Simbolo.EnumTipo.doble, Double.valueOf(String.valueOf(expresion1.valor)) - Double.valueOf(String.valueOf(expresion2.valor)));
                    case caracter:
                        //Doble Caracter
                        return new Expresion(Simbolo.EnumTipo.doble, Double.valueOf(String.valueOf(expresion1.valor)) - Double.valueOf(String.valueOf(expresion2.valor).charAt(0)));
                    default:
                        String cadena = "Resta no definida entre los tipos " + expresion1.tipo + " y " + expresion2.tipo;
                        return new Expresion(Simbolo.EnumTipo.error, cadena);
                }
            case entero:
                switch (expresion2.tipo) {
                    case entero:
                        //Entero Entero
                        return new Expresion(Simbolo.EnumTipo.entero, Integer.valueOf(String.valueOf(expresion1.valor)) - Integer.valueOf(String.valueOf(expresion2.valor)));
                    case caracter:
                        //Entero Caracter
                        return new Expresion(Simbolo.EnumTipo.entero, Integer.valueOf(String.valueOf(expresion1.valor)) - Integer.valueOf(String.valueOf(expresion2.valor).charAt(0)));
                    case doble:
                        //Entero Doble
                        return new Expresion(Simbolo.EnumTipo.doble, Double.valueOf(String.valueOf(expresion1.valor)) - Double.valueOf(String.valueOf(expresion2.valor)));
                    default:
                        String cadena = "Resta no definida entre los tipos " + expresion1.tipo + " y " + expresion2.tipo;
                        return new Expresion(Simbolo.EnumTipo.error, cadena);
                }
            case caracter:
                switch (expresion2.tipo) {
                    case doble:
                        //Caracter Doble
                        return new Expresion(Simbolo.EnumTipo.doble, Double.valueOf(String.valueOf(expresion1.valor)) - Double.valueOf(String.valueOf(expresion2.valor)));
                    case entero:
                        //Caracter Entero
                        return new Expresion(Simbolo.EnumTipo.entero, Integer.valueOf(String.valueOf(expresion1.valor)) - Integer.valueOf(String.valueOf(expresion2.valor)));
                    case caracter:
                        //Caracter Caracter
                        return new Expresion(Simbolo.EnumTipo.entero, Integer.valueOf(String.valueOf(expresion1.valor)) - Integer.valueOf(String.valueOf(expresion2.valor)));
                    default:
                        String cadena = "Resta no definida entre los tipos " + expresion1.tipo + " y " + expresion2.tipo;
                        return new Expresion(Simbolo.EnumTipo.error, cadena);
                }
            case error:
                return expresion1;
            default:
                String cadena = "Resta no definida entre los tipos " + expresion1.tipo + " y " + expresion2.tipo;
                return new Expresion(Simbolo.EnumTipo.error, cadena);
        }
    }

    private Expresion OperarMultiplicacion(Expresion expresion1, Expresion expresion2) {
        switch (expresion1.tipo) {
            case doble:
                switch (expresion2.tipo) {
                    case entero:
                    case doble:
                        //Doble Entero
                        return new Expresion(Simbolo.EnumTipo.doble, Double.valueOf(String.valueOf(expresion1.valor)) * Double.valueOf(String.valueOf(expresion2.valor)));
                    case caracter:
                        //Doble Caracter
                        return new Expresion(Simbolo.EnumTipo.doble, Double.valueOf(String.valueOf(expresion1.valor)) * Double.valueOf(String.valueOf(expresion2.valor).charAt(0)));
                    default:
                        String cadena = "Multiplicacion no definida entre los tipos " + expresion1.tipo + " y " + expresion2.tipo;
                        return new Expresion(Simbolo.EnumTipo.error, cadena);
                }
            case entero:
                switch (expresion2.tipo) {
                    case entero:
                        //Entero Entero
                        return new Expresion(Simbolo.EnumTipo.entero, Integer.valueOf(String.valueOf(expresion1.valor)) * Integer.valueOf(String.valueOf(expresion2.valor)));
                    case caracter:
                        //Entero Caracter
                        return new Expresion(Simbolo.EnumTipo.entero, Integer.valueOf(String.valueOf(expresion1.valor)) * Integer.valueOf(String.valueOf(expresion2.valor).charAt(0)));
                    case doble:
                        //Entero Doble
                        return new Expresion(Simbolo.EnumTipo.doble, Double.valueOf(String.valueOf(expresion1.valor)) * Double.valueOf(String.valueOf(expresion2.valor)));
                    default:
                        String cadena = "Multiplicacion no definida entre los tipos " + expresion1.tipo + " y " + expresion2.tipo;
                        return new Expresion(Simbolo.EnumTipo.error, cadena);
                }
            case caracter:
                switch (expresion2.tipo) {
                    case doble:
                        //Caracter Doble
                        return new Expresion(Simbolo.EnumTipo.doble, Double.valueOf(String.valueOf(expresion1.valor)) * Double.valueOf(String.valueOf(expresion2.valor)));
                    case entero:
                        //Caracter Entero
                        return new Expresion(Simbolo.EnumTipo.entero, Integer.valueOf(String.valueOf(expresion1.valor)) * Integer.valueOf(String.valueOf(expresion2.valor)));
                    case caracter:
                        //Caracter Caracter
                        return new Expresion(Simbolo.EnumTipo.entero, Integer.valueOf(String.valueOf(expresion1.valor)) * Integer.valueOf(String.valueOf(expresion2.valor)));
                    default:
                        String cadena = "Multiplicacion no definida entre los tipos " + expresion1.tipo + " y " + expresion2.tipo;
                        return new Expresion(Simbolo.EnumTipo.error, cadena);
                }
            case error:
                return expresion1;
            default:
                String cadena = "Multiplicacion no definida entre los tipos " + expresion1.tipo + " y " + expresion2.tipo;
                return new Expresion(Simbolo.EnumTipo.error, cadena);
        }
    }

    private Expresion OperarDivision(Expresion expresion1, Expresion expresion2) {
        switch (expresion1.tipo) {
            case doble:
                switch (expresion2.tipo) {
                    case entero:
                    case doble:
                        //Doble Entero
                        if (String.valueOf(expresion2).equals("0") || String.valueOf(expresion2).equals("0.0")) {
                            return new Expresion(Simbolo.EnumTipo.error, "Division por 0");
                        } else {
                            return new Expresion(Simbolo.EnumTipo.doble, Double.valueOf(String.valueOf(expresion1.valor)) / Double.valueOf(String.valueOf(expresion2.valor)));
                        }
                    case caracter:
                        //Doble Caracter
                        if (String.valueOf(expresion2).equals("0") || String.valueOf(expresion2).equals("0.0")) {
                            return new Expresion(Simbolo.EnumTipo.error, "Division por 0");
                        } else {
                            return new Expresion(Simbolo.EnumTipo.doble, Double.valueOf(String.valueOf(expresion1.valor)) / Double.valueOf(String.valueOf(expresion2.valor).charAt(0)));
                        }
                    default:
                        String cadena = "Division no definida entre los tipos " + expresion1.tipo + " y " + expresion2.tipo;
                        return new Expresion(Simbolo.EnumTipo.error, cadena);
                }
            case entero:
                switch (expresion2.tipo) {
                    case entero:
                        //Entero Entero
                        if (String.valueOf(expresion2).equals("0") || String.valueOf(expresion2).equals("0.0")) {
                            return new Expresion(Simbolo.EnumTipo.error, "Division por 0");
                        } else {
                            return new Expresion(Simbolo.EnumTipo.doble, Double.valueOf(String.valueOf(expresion1.valor)) / Double.valueOf(String.valueOf(expresion2.valor)));
                        }
                    case caracter:
                        //Entero Caracter
                        if (String.valueOf(expresion2).equals("0") || String.valueOf(expresion2).equals("0.0")) {
                            return new Expresion(Simbolo.EnumTipo.error, "Division por 0");
                        } else {
                            return new Expresion(Simbolo.EnumTipo.doble, Double.valueOf(String.valueOf(expresion1.valor)) / Double.valueOf(String.valueOf(expresion2.valor).charAt(0)));
                        }
                    case doble:
                        //Entero Doble
                        if (String.valueOf(expresion2).equals("0") || String.valueOf(expresion2).equals("0.0")) {
                            return new Expresion(Simbolo.EnumTipo.error, "Division por 0");
                        } else {
                            return new Expresion(Simbolo.EnumTipo.doble, Double.valueOf(String.valueOf(expresion1.valor)) / Double.valueOf(String.valueOf(expresion2.valor)));
                        }
                    default:
                        String cadena = "Division no definida entre los tipos " + expresion1.tipo + " y " + expresion2.tipo;
                        return new Expresion(Simbolo.EnumTipo.error, cadena);
                }
            case caracter:
                switch (expresion2.tipo) {
                    case doble:
                        //Caracter Doble
                        return new Expresion(Simbolo.EnumTipo.doble, Double.valueOf(String.valueOf(expresion1.valor)) / Double.valueOf(String.valueOf(expresion2.valor)));
                    case entero:
                        //Caracter Entero
                        return new Expresion(Simbolo.EnumTipo.doble, Double.valueOf(String.valueOf(expresion1.valor)) / Double.valueOf(String.valueOf(expresion2.valor)));
                    case caracter:
                        //Caracter Caracter
                        return new Expresion(Simbolo.EnumTipo.doble, Double.valueOf(String.valueOf(expresion1.valor)) / Double.valueOf(String.valueOf(expresion2.valor)));
                    default:
                        String cadena = "Division no definida entre los tipos " + expresion1.tipo + " y " + expresion2.tipo;
                        return new Expresion(Simbolo.EnumTipo.error, cadena);
                }
            case error:
                return expresion1;
            default:
                String cadena = "Division no definida entre los tipos " + expresion1.tipo + " y " + expresion2.tipo;
                return new Expresion(Simbolo.EnumTipo.error, cadena);
        }
    }

    private Expresion OperarPotencia(Expresion expresion1, Expresion expresion2) {
        switch (expresion1.tipo) {
            case doble:
            case entero:
                switch (expresion2.tipo) {
                    case entero:
                    case doble:
                        //Doble Entero
                        return new Expresion(Simbolo.EnumTipo.doble, Math.pow(Double.valueOf(String.valueOf(expresion1.valor)), Double.valueOf(String.valueOf(expresion2.valor))));
                    case caracter:
                        //Doble Caracter
                        return new Expresion(Simbolo.EnumTipo.doble, Math.pow(Double.valueOf(String.valueOf(expresion1.valor)), Double.valueOf(String.valueOf(expresion2.valor).charAt(0))));
                    default:
                        String cadena = "Potencia no definida entre los tipos " + expresion1.tipo + " y " + expresion2.tipo;
                        return new Expresion(Simbolo.EnumTipo.error, cadena);
                }
            case caracter:
                switch (expresion2.tipo) {
                    case doble:
                    case entero:
                        //Caracter Doble
                        return new Expresion(Simbolo.EnumTipo.doble, Math.pow(Double.valueOf(String.valueOf(expresion1.valor).charAt(0)), Double.valueOf(String.valueOf(expresion2.valor))));
                    case caracter:
                        //Caracter Caracter
                        return new Expresion(Simbolo.EnumTipo.doble, Math.pow(Double.valueOf(String.valueOf(expresion1.valor).charAt(0)), Double.valueOf(String.valueOf(expresion2.valor).charAt(0))));
                    default:
                        String cadena = "Potencia no definida entre los tipos " + expresion1.tipo + " y " + expresion2.tipo;
                        return new Expresion(Simbolo.EnumTipo.error, cadena);
                }
            case error:
                return expresion1;
            default:
                String cadena = "Potencia no definida entre los tipos " + expresion1.tipo + " y " + expresion2.tipo;
                return new Expresion(Simbolo.EnumTipo.error, cadena);
        }
    }

    private Expresion OperarNegativo(Expresion expresion1) {
        Object valor;
        switch (expresion1.tipo) {
            case entero:
            case caracter:
                valor = -(int) expresion1.valor;
                return new Expresion(Simbolo.EnumTipo.entero, valor);
            case doble:
                valor = -(double) expresion1.valor;
                return new Expresion(Simbolo.EnumTipo.doble, valor);
            default:
                return new Expresion(Simbolo.EnumTipo.error, "Se esperaba valor numerico, tipo " + expresion1.tipo + " encontrado");
        }
    }

    private Expresion OperarOrLogico(Expresion expresion1, Expresion expresion2) {
        if (expresion1.tipo == Simbolo.EnumTipo.booleano) {
            if (expresion2.tipo == Simbolo.EnumTipo.booleano) {
                boolean resultado = (boolean) expresion1.valor || (boolean) expresion2.valor;
                return new Expresion(Simbolo.EnumTipo.booleano, resultado);
            }
            return new Expresion(Simbolo.EnumTipo.error, "Tipo booleano requerido, tipo " + expresion2.tipo + " encontrado");
        }
        return new Expresion(Simbolo.EnumTipo.error, "Tipo booleano requerido, tipo " + expresion1.tipo + " encontrado");
    }

    private Expresion OperarAndLogico(Expresion expresion1, Expresion expresion2) {
        if (expresion1.tipo == Simbolo.EnumTipo.booleano) {
            if (expresion2.tipo == Simbolo.EnumTipo.booleano) {
                boolean resultado = (boolean) expresion1.valor && (boolean) expresion2.valor;
                return new Expresion(Simbolo.EnumTipo.booleano, resultado);
            }
            return new Expresion(Simbolo.EnumTipo.error, "Tipo booleano requerido, tipo " + expresion2.tipo + " encontrado");
        }
        return new Expresion(Simbolo.EnumTipo.error, "Tipo booleano requerido, tipo " + expresion1.tipo + " encontrado");
    }

    private Expresion OperarNotLogico(Expresion expresion1) {
        if (expresion1.tipo == Simbolo.EnumTipo.booleano) {
            boolean resultado = (boolean) expresion1.valor;
            return new Expresion(Simbolo.EnumTipo.booleano, !resultado);
        }
        return new Expresion(Simbolo.EnumTipo.error, "Tipo booleano requerido, tipo " + expresion1.tipo + " encontrado");
    }

    private Expresion OperarXorLogico(Expresion expresion1, Expresion expresion2) {
        if (expresion1.tipo == Simbolo.EnumTipo.booleano) {
            if (expresion2.tipo == Simbolo.EnumTipo.booleano) {
                boolean resultado = (boolean) expresion1.valor ^ (boolean) expresion2.valor;
                return new Expresion(Simbolo.EnumTipo.booleano, resultado);
            }
            return new Expresion(Simbolo.EnumTipo.error, "Tipo booleano requerido, tipo " + expresion2.tipo + " encontrado");
        }
        return new Expresion(Simbolo.EnumTipo.error, "Tipo booleano requerido, tipo " + expresion1.tipo + " encontrado");
    }

    private Expresion OperarMayorQue(Expresion expresion1, Expresion expresion2) {
        boolean resultado;
        switch (expresion1.tipo) {
            case entero:
                switch (expresion2.tipo) {
                    case entero:
                        //Entero Entero
                        resultado = (int) expresion1.valor > (int) expresion2.valor;
                        return new Expresion(Simbolo.EnumTipo.booleano, resultado);
                    case doble:
                        //Entero Doble
                        resultado = (int) expresion1.valor > (double) expresion2.valor;
                        return new Expresion(Simbolo.EnumTipo.booleano, resultado);
                    case caracter:
                        //Entero Caracter
                        resultado = (int) expresion1.valor > (int) expresion2.valor;
                        return new Expresion(Simbolo.EnumTipo.booleano, resultado);
                }
                break;
            case doble:
                switch (expresion2.tipo) {
                    case entero:
                        //Doble Entero
                        resultado = (double) expresion1.valor > (int) expresion2.valor;
                        return new Expresion(Simbolo.EnumTipo.booleano, resultado);
                    case doble:
                        //Doble Doble
                        resultado = (double) expresion1.valor > (double) expresion2.valor;
                        return new Expresion(Simbolo.EnumTipo.booleano, resultado);
                    case caracter:
                        //Doble Caracter
                        resultado = (double) expresion1.valor > (int) expresion2.valor;
                        return new Expresion(Simbolo.EnumTipo.booleano, resultado);
                }
                break;
            case caracter:
                switch (expresion2.tipo) {
                    case entero:
                        //Caracter Entero
                        resultado = (int) expresion1.valor > (int) expresion2.valor;
                        return new Expresion(Simbolo.EnumTipo.booleano, resultado);
                    case doble:
                        //Caracter Doble
                        resultado = (int) expresion1.valor > (double) expresion2.valor;
                        return new Expresion(Simbolo.EnumTipo.booleano, resultado);
                    case caracter:
                        //Caracter Caracter
                        resultado = (int) expresion1.valor > (int) expresion2.valor;
                        return new Expresion(Simbolo.EnumTipo.booleano, resultado);
                }
                break;
        }
        return new Expresion(Simbolo.EnumTipo.error, "Operacion entre " + expresion1.tipo + " y " + expresion2.tipo + " no definida");
    }

    private Expresion OperarMenorQue(Expresion expresion1, Expresion expresion2) {
        boolean resultado;
        switch (expresion1.tipo) {
            case entero:
                switch (expresion2.tipo) {
                    case entero:
                        //Entero Entero
                        resultado = (int) expresion1.valor < (int) expresion2.valor;
                        return new Expresion(Simbolo.EnumTipo.booleano, resultado);
                    case doble:
                        //Entero Doble
                        resultado = (int) expresion1.valor < (double) expresion2.valor;
                        return new Expresion(Simbolo.EnumTipo.booleano, resultado);
                    case caracter:
                        //Entero Caracter
                        resultado = (int) expresion1.valor < (int) expresion2.valor;
                        return new Expresion(Simbolo.EnumTipo.booleano, resultado);
                }
                break;
            case doble:
                switch (expresion2.tipo) {
                    case entero:
                        //Doble Entero
                        resultado = (double) expresion1.valor < (int) expresion2.valor;
                        return new Expresion(Simbolo.EnumTipo.booleano, resultado);
                    case doble:
                        //Doble Doble
                        resultado = (double) expresion1.valor < (double) expresion2.valor;
                        return new Expresion(Simbolo.EnumTipo.booleano, resultado);
                    case caracter:
                        //Doble Caracter
                        resultado = (double) expresion1.valor < (int) expresion2.valor;
                        return new Expresion(Simbolo.EnumTipo.booleano, resultado);
                }
                break;
            case caracter:
                switch (expresion2.tipo) {
                    case entero:
                        //Caracter Entero
                        resultado = (int) expresion1.valor < (int) expresion2.valor;
                        return new Expresion(Simbolo.EnumTipo.booleano, resultado);
                    case doble:
                        //Caracter Doble
                        resultado = (int) expresion1.valor < (double) expresion2.valor;
                        return new Expresion(Simbolo.EnumTipo.booleano, resultado);
                    case caracter:
                        //Caracter Caracter
                        resultado = (int) expresion1.valor < (int) expresion2.valor;
                        return new Expresion(Simbolo.EnumTipo.booleano, resultado);
                }
                break;
        }
        return new Expresion(Simbolo.EnumTipo.error, "Operacion entre " + expresion1.tipo + " y " + expresion2.tipo + " no definida");
    }

    private Expresion OperarMayorIgualQue(Expresion expresion1, Expresion expresion2) {
        boolean resultado;
        switch (expresion1.tipo) {
            case entero:
                switch (expresion2.tipo) {
                    case entero:
                        //Entero Entero
                        resultado = (int) expresion1.valor >= (int) expresion2.valor;
                        return new Expresion(Simbolo.EnumTipo.booleano, resultado);
                    case doble:
                        //Entero Doble
                        resultado = (int) expresion1.valor >= (double) expresion2.valor;
                        return new Expresion(Simbolo.EnumTipo.booleano, resultado);
                    case caracter:
                        //Entero Caracter
                        resultado = (int) expresion1.valor >= (int) expresion2.valor;
                        return new Expresion(Simbolo.EnumTipo.booleano, resultado);
                }
                break;
            case doble:
                switch (expresion2.tipo) {
                    case entero:
                        //Doble Entero
                        resultado = (double) expresion1.valor >= (int) expresion2.valor;
                        return new Expresion(Simbolo.EnumTipo.booleano, resultado);
                    case doble:
                        //Doble Doble
                        resultado = (double) expresion1.valor >= (double) expresion2.valor;
                        return new Expresion(Simbolo.EnumTipo.booleano, resultado);
                    case caracter:
                        //Doble Caracter
                        resultado = (double) expresion1.valor >= (int) expresion2.valor;
                        return new Expresion(Simbolo.EnumTipo.booleano, resultado);
                }
                break;
            case caracter:
                switch (expresion2.tipo) {
                    case entero:
                        //Caracter Entero
                        resultado = (int) expresion1.valor >= (int) expresion2.valor;
                        return new Expresion(Simbolo.EnumTipo.booleano, resultado);
                    case doble:
                        //Caracter Doble
                        resultado = (int) expresion1.valor >= (double) expresion2.valor;
                        return new Expresion(Simbolo.EnumTipo.booleano, resultado);
                    case caracter:
                        //Caracter Caracter
                        resultado = (int) expresion1.valor >= (int) expresion2.valor;
                        return new Expresion(Simbolo.EnumTipo.booleano, resultado);
                }
                break;
        }
        return new Expresion(Simbolo.EnumTipo.error, "Operacion entre " + expresion1.tipo + " y " + expresion2.tipo + " no definida");
    }

    private Expresion OperarMenorIgualQue(Expresion expresion1, Expresion expresion2) {
        boolean resultado;
        switch (expresion1.tipo) {
            case entero:
                switch (expresion2.tipo) {
                    case entero:
                        //Entero Entero
                        resultado = (int) expresion1.valor <= (int) expresion2.valor;
                        return new Expresion(Simbolo.EnumTipo.booleano, resultado);
                    case doble:
                        //Entero Doble
                        resultado = (int) expresion1.valor <= (double) expresion2.valor;
                        return new Expresion(Simbolo.EnumTipo.booleano, resultado);
                    case caracter:
                        //Entero Caracter
                        resultado = (int) expresion1.valor <= (int) expresion2.valor;
                        return new Expresion(Simbolo.EnumTipo.booleano, resultado);
                }
                break;
            case doble:
                switch (expresion2.tipo) {
                    case entero:
                        //Doble Entero
                        resultado = (double) expresion1.valor <= (int) expresion2.valor;
                        return new Expresion(Simbolo.EnumTipo.booleano, resultado);
                    case doble:
                        //Doble Doble
                        resultado = (double) expresion1.valor <= (double) expresion2.valor;
                        return new Expresion(Simbolo.EnumTipo.booleano, resultado);
                    case caracter:
                        //Doble Caracter
                        resultado = (double) expresion1.valor <= (int) expresion2.valor;
                        return new Expresion(Simbolo.EnumTipo.booleano, resultado);
                }
                break;
            case caracter:
                switch (expresion2.tipo) {
                    case entero:
                        //Caracter Entero
                        resultado = (int) expresion1.valor <= (int) expresion2.valor;
                        return new Expresion(Simbolo.EnumTipo.booleano, resultado);
                    case doble:
                        //Caracter Doble
                        resultado = (int) expresion1.valor <= (double) expresion2.valor;
                        return new Expresion(Simbolo.EnumTipo.booleano, resultado);
                    case caracter:
                        //Caracter Caracter
                        resultado = (int) expresion1.valor <= (int) expresion2.valor;
                        return new Expresion(Simbolo.EnumTipo.booleano, resultado);
                }
                break;
        }
        return new Expresion(Simbolo.EnumTipo.error, "Operacion entre " + expresion1.tipo + " y " + expresion2.tipo + " no definida");
    }

    private Expresion OperarIgual(Expresion expresion1, Expresion expresion2) {
        boolean resultado;
        switch (expresion1.tipo) {
            case entero:
                switch (expresion2.tipo) {
                    case entero:
                        //Entero Entero
                        resultado = (int) expresion1.valor == (int) expresion2.valor;
                        return new Expresion(Simbolo.EnumTipo.booleano, resultado);
                    case doble:
                        //Entero Doble
                        resultado = (int) expresion1.valor == (double) expresion2.valor;
                        return new Expresion(Simbolo.EnumTipo.booleano, resultado);
                    case caracter:
                        //Entero Caracter
                        resultado = (int) expresion1.valor == (int) expresion2.valor;
                        return new Expresion(Simbolo.EnumTipo.booleano, resultado);
                }
                break;
            case doble:
                switch (expresion2.tipo) {
                    case entero:
                        //Doble Entero
                        resultado = (double) expresion1.valor == (int) expresion2.valor;
                        return new Expresion(Simbolo.EnumTipo.booleano, resultado);
                    case doble:
                        //Doble Doble
                        resultado = (double) expresion1.valor == (double) expresion2.valor;
                        return new Expresion(Simbolo.EnumTipo.booleano, resultado);
                    case caracter:
                        //Doble Caracter
                        resultado = (double) expresion1.valor == (int) expresion2.valor;
                        return new Expresion(Simbolo.EnumTipo.booleano, resultado);
                }
                break;
            case caracter:
                switch (expresion2.tipo) {
                    case entero:
                        //Caracter Entero
                        resultado = (int) expresion1.valor == (int) expresion2.valor;
                        return new Expresion(Simbolo.EnumTipo.booleano, resultado);
                    case doble:
                        //Caracter Doble
                        resultado = (int) expresion1.valor == (double) expresion2.valor;
                        return new Expresion(Simbolo.EnumTipo.booleano, resultado);
                    case caracter:
                        //Caracter Caracter
                        resultado = (int) expresion1.valor == (int) expresion2.valor;
                        return new Expresion(Simbolo.EnumTipo.booleano, resultado);
                }
                break;
            case cadena:
                switch (expresion2.tipo) {
                    case cadena:
                        //Cadena Cadena
                        String cadena1 = (String) expresion1.valor;
                        String cadena2 = (String) expresion2.valor;
                        resultado = cadena1.equals(cadena2);
                        return new Expresion(Simbolo.EnumTipo.booleano, resultado);
                }
                break;
        }
        return new Expresion(Simbolo.EnumTipo.error, "Operacion entre " + expresion1.tipo + " y " + expresion2.tipo + " no definida");
    }

    private Expresion OperarDiferente(Expresion expresion1, Expresion expresion2) {
        boolean resultado;
        switch (expresion1.tipo) {
            case entero:
                switch (expresion2.tipo) {
                    case entero:
                        //Entero Entero
                        resultado = (int) expresion1.valor != (int) expresion2.valor;
                        return new Expresion(Simbolo.EnumTipo.booleano, resultado);
                    case doble:
                        //Entero Doble
                        resultado = (int) expresion1.valor != (double) expresion2.valor;
                        return new Expresion(Simbolo.EnumTipo.booleano, resultado);
                    case caracter:
                        //Entero Caracter
                        resultado = (int) expresion1.valor != (int) expresion2.valor;
                        return new Expresion(Simbolo.EnumTipo.booleano, resultado);
                }
                break;
            case doble:
                switch (expresion2.tipo) {
                    case entero:
                        //Doble Entero
                        resultado = (double) expresion1.valor != (int) expresion2.valor;
                        return new Expresion(Simbolo.EnumTipo.booleano, resultado);
                    case doble:
                        //Doble Doble
                        resultado = (double) expresion1.valor != (double) expresion2.valor;
                        return new Expresion(Simbolo.EnumTipo.booleano, resultado);
                    case caracter:
                        //Doble Caracter
                        resultado = (double) expresion1.valor != (int) expresion2.valor;
                        return new Expresion(Simbolo.EnumTipo.booleano, resultado);
                }
                break;
            case caracter:
                switch (expresion2.tipo) {
                    case entero:
                        //Caracter Entero
                        resultado = (int) expresion1.valor != (int) expresion2.valor;
                        return new Expresion(Simbolo.EnumTipo.booleano, resultado);
                    case doble:
                        //Caracter Doble
                        resultado = (int) expresion1.valor != (double) expresion2.valor;
                        return new Expresion(Simbolo.EnumTipo.booleano, resultado);
                    case caracter:
                        //Caracter Caracter
                        resultado = (int) expresion1.valor != (int) expresion2.valor;
                        return new Expresion(Simbolo.EnumTipo.booleano, resultado);
                }
                break;
            case cadena:
                switch (expresion2.tipo) {
                    case cadena:
                        //Cadena Cadena
                        String cadena1 = (String) expresion1.valor;
                        String cadena2 = (String) expresion2.valor;
                        resultado = !cadena1.equals(cadena2);
                        return new Expresion(Simbolo.EnumTipo.booleano, resultado);
                }
                break;
        }
        return new Expresion(Simbolo.EnumTipo.error, "Operacion entre " + expresion1.tipo + " y " + expresion2.tipo + " no definida");
    }
}
