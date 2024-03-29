/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package compi1_prac1_201700893;

import Analizadores.*;
import Objetos.*;
import java.awt.HeadlessException;
import java.awt.Image;
import java.io.BufferedReader;
import java.io.File;
import java.io.FileNotFoundException;
import java.io.FileReader;
import java.io.FileWriter;
import java.io.IOException;
import java.util.ArrayList;
import java.util.HashMap;
import java.util.Map;
import java.util.logging.Level;
import java.util.logging.Logger;
import javax.swing.ImageIcon;
import javax.swing.JFileChooser;
import javax.swing.JOptionPane;
import static javax.swing.JOptionPane.showMessageDialog;
import javax.swing.filechooser.FileNameExtensionFilter;

/**
 *
 * @author Eleazar Lopez <Universidad de San Carlos de Guatemala>
 */
public class Principal extends javax.swing.JFrame {

    /**
     * Creates new form Principal
     */
    Map<String, Object> identificadores;
    Map<String, ArrayList<String>> conjuntos;
    Map<String, Arbol> expresiones;
    ArrayList<Arbol> listaArboles;
    Map<String, ArrayList<EstadosAnalizador>> estadosAnalizador;
    String path = "";
    ArrayList<String> paths;
    int indexPath = 0;
    int letrasNumeros[];
    String estadoActual;
    String cadenaActual;
    int cadenaCorrecta;
    int estadoAceptacion;

    public Principal() {
        initComponents();
        estadoActual = "";
        cadenaActual = "";
        paths = new ArrayList<>();
        estadosAnalizador = new HashMap<>();
        conjuntos = new HashMap<>();
        cadenaCorrecta = 0;
        estadoAceptacion = 0;
        letrasNumeros = new int[]{48, 49, 50, 51, 52, 53, 54, 55, 56, 57,
            65, 66, 67, 68, 69, 70, 71, 72, 73, 74, 75, 76, 77, 78, 79, 80, 81, 82, 83, 84, 85, 86, 87, 88, 89, 90,
            97, 98, 99, 100, 101, 102, 103, 104, 105, 106, 107, 108, 109, 110, 111, 112, 113, 114, 115, 116, 117, 118,
            119, 120, 121, 122};
    }

    /**
     * This method is called from within the constructor to initialize the form.
     * WARNING: Do NOT modify this code. The content of this method is always
     * regenerated by the Form Editor.
     */
    @SuppressWarnings("unchecked")
    // <editor-fold defaultstate="collapsed" desc="Generated Code">//GEN-BEGIN:initComponents
    private void initComponents() {

        jMenuBar2 = new javax.swing.JMenuBar();
        jMenu3 = new javax.swing.JMenu();
        jMenu4 = new javax.swing.JMenu();
        jMenu1 = new javax.swing.JMenu();
        jMenuItem3 = new javax.swing.JMenuItem();
        jMenu7 = new javax.swing.JMenu();
        jScrollPane1 = new javax.swing.JScrollPane();
        jTextArea1 = new javax.swing.JTextArea();
        jScrollPane2 = new javax.swing.JScrollPane();
        jTextArea2 = new javax.swing.JTextArea();
        jLabel1 = new javax.swing.JLabel();
        jLabel2 = new javax.swing.JLabel();
        jButton1 = new javax.swing.JButton();
        jButton2 = new javax.swing.JButton();
        jLabel3 = new javax.swing.JLabel();
        jMenuBar3 = new javax.swing.JMenuBar();
        jMenu5 = new javax.swing.JMenu();
        jMenuItem1 = new javax.swing.JMenuItem();
        jMenuItem9 = new javax.swing.JMenuItem();
        jMenuItem10 = new javax.swing.JMenuItem();
        jMenu6 = new javax.swing.JMenu();
        jMenuItem2 = new javax.swing.JMenuItem();
        jMenuItem8 = new javax.swing.JMenuItem();
        jMenu2 = new javax.swing.JMenu();
        jMenuItem4 = new javax.swing.JMenuItem();
        jMenuItem5 = new javax.swing.JMenuItem();
        jMenuItem6 = new javax.swing.JMenuItem();
        jMenuItem7 = new javax.swing.JMenuItem();

        jMenu3.setText("File");
        jMenuBar2.add(jMenu3);

        jMenu4.setText("Edit");
        jMenuBar2.add(jMenu4);

        jMenu1.setText("jMenu1");

        jMenuItem3.setText("jMenuItem3");

        jMenu7.setText("jMenu7");

        setDefaultCloseOperation(javax.swing.WindowConstants.EXIT_ON_CLOSE);
        setLocation(new java.awt.Point(50, 23));

        jTextArea1.setColumns(20);
        jTextArea1.setRows(5);
        jScrollPane1.setViewportView(jTextArea1);

        jTextArea2.setColumns(20);
        jTextArea2.setRows(5);
        jScrollPane2.setViewportView(jTextArea2);

        jLabel1.setText("Consola");

        jButton1.setText("Anterior");
        jButton1.addActionListener(new java.awt.event.ActionListener() {
            public void actionPerformed(java.awt.event.ActionEvent evt) {
                jButton1ActionPerformed(evt);
            }
        });

        jButton2.setText("Siguiente");
        jButton2.addActionListener(new java.awt.event.ActionListener() {
            public void actionPerformed(java.awt.event.ActionEvent evt) {
                jButton2ActionPerformed(evt);
            }
        });

        jMenu5.setText("Archivo");

        jMenuItem1.setText("Abrir");
        jMenuItem1.addActionListener(new java.awt.event.ActionListener() {
            public void actionPerformed(java.awt.event.ActionEvent evt) {
                jMenuItem1ActionPerformed(evt);
            }
        });
        jMenu5.add(jMenuItem1);

        jMenuItem9.setText("Guardar");
        jMenuItem9.addActionListener(new java.awt.event.ActionListener() {
            public void actionPerformed(java.awt.event.ActionEvent evt) {
                jMenuItem9ActionPerformed(evt);
            }
        });
        jMenu5.add(jMenuItem9);

        jMenuItem10.setText("Guardar Como");
        jMenuItem10.addActionListener(new java.awt.event.ActionListener() {
            public void actionPerformed(java.awt.event.ActionEvent evt) {
                jMenuItem10ActionPerformed(evt);
            }
        });
        jMenu5.add(jMenuItem10);

        jMenuBar3.add(jMenu5);

        jMenu6.setText("Acciones");

        jMenuItem2.setText("Compilar");
        jMenuItem2.addActionListener(new java.awt.event.ActionListener() {
            public void actionPerformed(java.awt.event.ActionEvent evt) {
                jMenuItem2ActionPerformed(evt);
            }
        });
        jMenu6.add(jMenuItem2);

        jMenuItem8.setText("Ver Errores");
        jMenu6.add(jMenuItem8);

        jMenuBar3.add(jMenu6);

        jMenu2.setText("Ver Imagenes");
        jMenu2.setEnabled(false);

        jMenuItem4.setText("Arboles");
        jMenuItem4.addActionListener(new java.awt.event.ActionListener() {
            public void actionPerformed(java.awt.event.ActionEvent evt) {
                jMenuItem4ActionPerformed(evt);
            }
        });
        jMenu2.add(jMenuItem4);

        jMenuItem5.setText("Automatas");
        jMenuItem5.addActionListener(new java.awt.event.ActionListener() {
            public void actionPerformed(java.awt.event.ActionEvent evt) {
                jMenuItem5ActionPerformed(evt);
            }
        });
        jMenu2.add(jMenuItem5);

        jMenuItem6.setText("Siguientes");
        jMenuItem6.addActionListener(new java.awt.event.ActionListener() {
            public void actionPerformed(java.awt.event.ActionEvent evt) {
                jMenuItem6ActionPerformed(evt);
            }
        });
        jMenu2.add(jMenuItem6);

        jMenuItem7.setText("Transiciones");
        jMenuItem7.addActionListener(new java.awt.event.ActionListener() {
            public void actionPerformed(java.awt.event.ActionEvent evt) {
                jMenuItem7ActionPerformed(evt);
            }
        });
        jMenu2.add(jMenuItem7);

        jMenuBar3.add(jMenu2);

        setJMenuBar(jMenuBar3);

        javax.swing.GroupLayout layout = new javax.swing.GroupLayout(getContentPane());
        getContentPane().setLayout(layout);
        layout.setHorizontalGroup(
            layout.createParallelGroup(javax.swing.GroupLayout.Alignment.LEADING)
            .addGroup(layout.createSequentialGroup()
                .addContainerGap()
                .addGroup(layout.createParallelGroup(javax.swing.GroupLayout.Alignment.LEADING)
                    .addGroup(layout.createSequentialGroup()
                        .addComponent(jScrollPane2)
                        .addContainerGap())
                    .addGroup(layout.createSequentialGroup()
                        .addGroup(layout.createParallelGroup(javax.swing.GroupLayout.Alignment.TRAILING)
                            .addComponent(jLabel1, javax.swing.GroupLayout.Alignment.LEADING)
                            .addComponent(jScrollPane1, javax.swing.GroupLayout.PREFERRED_SIZE, 593, javax.swing.GroupLayout.PREFERRED_SIZE))
                        .addGroup(layout.createParallelGroup(javax.swing.GroupLayout.Alignment.LEADING)
                            .addGroup(layout.createSequentialGroup()
                                .addPreferredGap(javax.swing.LayoutStyle.ComponentPlacement.RELATED)
                                .addGroup(layout.createParallelGroup(javax.swing.GroupLayout.Alignment.LEADING)
                                    .addGroup(layout.createSequentialGroup()
                                        .addComponent(jLabel3, javax.swing.GroupLayout.PREFERRED_SIZE, 402, javax.swing.GroupLayout.PREFERRED_SIZE)
                                        .addGap(0, 219, Short.MAX_VALUE))
                                    .addComponent(jLabel2, javax.swing.GroupLayout.DEFAULT_SIZE, javax.swing.GroupLayout.DEFAULT_SIZE, Short.MAX_VALUE))
                                .addContainerGap())
                            .addGroup(javax.swing.GroupLayout.Alignment.TRAILING, layout.createSequentialGroup()
                                .addPreferredGap(javax.swing.LayoutStyle.ComponentPlacement.RELATED, javax.swing.GroupLayout.DEFAULT_SIZE, Short.MAX_VALUE)
                                .addComponent(jButton1)
                                .addGap(18, 18, 18)
                                .addComponent(jButton2)
                                .addGap(250, 250, 250))))))
        );
        layout.setVerticalGroup(
            layout.createParallelGroup(javax.swing.GroupLayout.Alignment.LEADING)
            .addGroup(layout.createSequentialGroup()
                .addContainerGap()
                .addGroup(layout.createParallelGroup(javax.swing.GroupLayout.Alignment.LEADING)
                    .addGroup(layout.createSequentialGroup()
                        .addComponent(jScrollPane1, javax.swing.GroupLayout.PREFERRED_SIZE, 391, javax.swing.GroupLayout.PREFERRED_SIZE)
                        .addGap(49, 49, 49)
                        .addComponent(jLabel1))
                    .addGroup(layout.createSequentialGroup()
                        .addComponent(jLabel3, javax.swing.GroupLayout.PREFERRED_SIZE, 37, javax.swing.GroupLayout.PREFERRED_SIZE)
                        .addPreferredGap(javax.swing.LayoutStyle.ComponentPlacement.RELATED)
                        .addComponent(jLabel2, javax.swing.GroupLayout.PREFERRED_SIZE, 364, javax.swing.GroupLayout.PREFERRED_SIZE)
                        .addGap(18, 18, 18)
                        .addGroup(layout.createParallelGroup(javax.swing.GroupLayout.Alignment.BASELINE)
                            .addComponent(jButton1)
                            .addComponent(jButton2))))
                .addPreferredGap(javax.swing.LayoutStyle.ComponentPlacement.RELATED)
                .addComponent(jScrollPane2, javax.swing.GroupLayout.DEFAULT_SIZE, 162, Short.MAX_VALUE)
                .addContainerGap())
        );

        pack();
    }// </editor-fold>//GEN-END:initComponents

    private void jMenuItem1ActionPerformed(java.awt.event.ActionEvent evt) {//GEN-FIRST:event_jMenuItem1ActionPerformed
        // TODO add your handling code here:
        JFileChooser fc = new JFileChooser();
        fc.setCurrentDirectory(new File("C:/Users/USER/Desktop/PruebaPractica1/"));
        fc.setMultiSelectionEnabled(false);
        FileNameExtensionFilter filter = new FileNameExtensionFilter("Expresiones Regulares", "er", "text");
        fc.setFileFilter(filter);
        fc.showOpenDialog(this);
        File file = fc.getSelectedFile();
        String texto = "";
        String aux = "";
        if (file != null) {
            try {
                FileReader archivo = new FileReader(file);
                try (BufferedReader br = new BufferedReader(archivo)) {
                    while ((aux = br.readLine()) != null) {
                        texto += aux + "\n";
                    }
                    path = file.getPath();
                    jTextArea1.setText(texto);
                }
            } catch (FileNotFoundException ex) {
                Logger.getLogger(Principal.class.getName()).log(Level.SEVERE, null, ex);
            } catch (IOException ex) {
                Logger.getLogger(Principal.class.getName()).log(Level.SEVERE, null, ex);
                JOptionPane.showMessageDialog(this, "No se encontro el archivo");
            }
        }
    }//GEN-LAST:event_jMenuItem1ActionPerformed

    private void rangoIdentificadores() {
        Map<String, ArrayList<String>> tmp = new HashMap<>();

        identificadores.entrySet().forEach((identificador) -> {
            String valores = identificador.getValue().toString();
            ArrayList<String> listaValores = new ArrayList<>();
            if (valores.contains("~")) {
                char inicio = valores.charAt(0);
                char fin = valores.charAt(2);
                char agregar;
                if (inicio == 32) {
                    for (int i = inicio; i <= fin; i++) {
                        for (int j = 0; j < letrasNumeros.length; j++) {
                            if (i != letrasNumeros[j]) {
                                agregar = (char) i;
                                listaValores.add(String.valueOf(agregar));
                            }
                        }
                    }
                } else {
                    for (int i = inicio; i <= fin; i++) {
                        agregar = (char) i;
                        listaValores.add(String.valueOf(agregar));
                    }
                }
            } else {
                String[] individuales = valores.split(",");
                for (int i = 0; i < individuales.length; i++) {
                    listaValores.add(individuales[i]);
                }
            }
            tmp.put(identificador.getKey(), listaValores);
        });

        conjuntos = tmp;
    }

    private void generarAutomata(Arbol arbol) {
        if (conjuntos.isEmpty()) {
            rangoIdentificadores();
        }
        /*
            Encontramos el numero de aceptacion
         */
        int aceptacion = 0;
        for (Item item : arbol.getSiguientes()) {
            if (item.getEtiqueta().equals("#")) {
                aceptacion = item.getNumero();
            }
        }

        /*
            Obtengo las transiciones del arbol
            Expresion1:
                                                    TERMINALES
                Estado          Los     otrasLetras     El      vocales     abecedario      _
                S0              S1                      S1
                S1                          S2                    S2
                S2                          S2                    S2           S3           S3
                S3                                                             S3           S3
         */
        for (Transicion transicion : arbol.getTransiciones()) {
            String cadena = "";
            boolean esAceptacion = false;
            //Generamos el nombre del estado con el siguiente formato numero (, numero)*
            //Se comprueba si el estado es de aceptacion
            for (Object obj : transicion.getEstado()) {
                if (String.valueOf(obj).equals(String.valueOf(aceptacion))) {
                    esAceptacion = true;
                }

                if (cadena.isEmpty()) {
                    cadena = obj.toString();
                } else {
                    cadena += ", " + obj.toString();
                }
            }

            EstadosAnalizador state = new EstadosAnalizador(cadena, esAceptacion);
            Map<String, ArrayList<String>> mapa = new HashMap<>();
            transicion.getMovimientos().entrySet().forEach((movimiento) -> {
                if (!movimiento.getKey().equals("#")) {
                    String terminal = movimiento.getKey();
                    ArrayList<String> tmp = new ArrayList<>();
                    if (conjuntos.containsKey(terminal)) {
                        tmp = conjuntos.get(terminal);
                    } else {
                        tmp.add(terminal);
                    }
                    String estadoSiguiente = "";
                    for (Object objeto : movimiento.getValue()) {
                        if (estadoSiguiente.isEmpty()) {
                            estadoSiguiente = objeto.toString();
                        } else {
                            estadoSiguiente += ", " + objeto.toString();
                        }
                    }
                    if (mapa.containsKey(estadoSiguiente)) {
                        for (String str : tmp) {
                            if (!mapa.get(estadoSiguiente).contains(str)) {
                                mapa.get(estadoSiguiente).add(str);
                            } else {
                                arbol.setAmbiguedad(true);
                            }
                        }
                    } else {
                        mapa.put(estadoSiguiente, tmp);
                    }
                }
            });

            arbol.getMapeoEstados().put(state.getEstado(), mapa);
        }
    }

    private void jMenuItem2ActionPerformed(java.awt.event.ActionEvent evt) {//GEN-FIRST:event_jMenuItem2ActionPerformed
        // TODO add your handling code here:

        //Limpiando las Variables
        estadoActual = "";
        cadenaActual = "";
        paths = new ArrayList<>();
        estadosAnalizador = new HashMap<>();
        conjuntos = new HashMap<>();
        cadenaCorrecta = 0;
        estadoAceptacion = 0;
        identificadores = new HashMap<>();
        expresiones = new HashMap<>();
        listaArboles = new ArrayList<>();

        
        
        
        String entrada = jTextArea1.getText();
        jTextArea2.setText("");
        if (!entrada.isEmpty()) {
            Lexico analisisLexico = new Lexico(entrada);
            ArrayList<Simbolos> simbolos;
            Map<String, ArrayList<String>> transiciones;
            try {
                analisisLexico.analizar();
                simbolos = analisisLexico.getSimbolos();
                if (analisisLexico.getErrores().isEmpty()) {
                    identificadores = new HashMap<>();
                    expresiones = new HashMap<>();
                    listaArboles = new ArrayList<>();
                    ArrayList<Simbolos> elementos = new ArrayList<>();
                    Arbol arbol;
                    String iden = "";
                    int esExpresion = 0;
                    boolean declaraciones = true;
                    for (Simbolos simbolo : simbolos) {
                        if (simbolo.getTipo() == Simbolos.EnumTipo.id) {
                            iden = simbolo.getValor();
                            esExpresion++;
                        }
                        if (!iden.isEmpty()) {
                            if (simbolo.getTipo() == Simbolos.EnumTipo.lista || simbolo.getTipo() == Simbolos.EnumTipo.intervalo) {
                                identificadores.put(iden, simbolo.getValor());
                                iden = "";
                                esExpresion = 0;
                            } else {
                                if (declaraciones) {
                                    if (esExpresion == 1 && simbolo.getTipo() == Simbolos.EnumTipo.mayorQue) {
                                        elementos = new ArrayList<>();
                                        esExpresion++;
                                    } else if (esExpresion == 2 && simbolo.getTipo() == Simbolos.EnumTipo.puntoComa) {
                                        esExpresion = 0;
                                        arbol = new Arbol(elementos, iden);
                                        listaArboles.add(arbol);
                                        expresiones.put(iden, arbol);
                                        generarAutomata(arbol);
                                        iden = "";
                                    } else if (esExpresion == 2) {
                                        elementos.add(simbolo);
                                    }
                                } else {
                                    //Evaluaciones  
                                    if (simbolo.getTipo() == Simbolos.EnumTipo.cadena) {
                                        evaluarCadena(iden, simbolo.getValor());
                                    }
                                }
                            }
                        }
                        if (simbolo.getTipo() == Simbolos.EnumTipo.porciento) {
                            declaraciones = false;
                        }
                    }
                    /*listaArboles.forEach((tree) -> {
                        System.out.println("-----------------Nuevo Arbol-----------------------");
                        System.out.println(tree.preOrden(tree.getRaiz()));
                    });*/
 /*identificadores.entrySet().forEach((identificador) -> {
                        System.out.println("Identificador: " + identificador.getKey() + " Valor: " + identificador.getValue());
                    });
                    /*expresiones.entrySet().forEach((expresion) -> {
                        System.out.println("Expresion: " + expresion.getKey());
                    });*/
                } else {
                    System.out.println("Hubo error en el analisis lexico");
                }
            } catch (IOException ex) {
                Logger.getLogger(Principal.class.getName()).log(Level.SEVERE, null, ex);
            }
        }
        jMenu2.setEnabled(true);
    }//GEN-LAST:event_jMenuItem2ActionPerformed

    private void evaluarCadena(String expresion, String cadena) {
        for (Arbol arbol : listaArboles) {
            if (arbol.getNombre().equals(expresion)) {
                if (arbol.isAmbiguedad()) {
                    jTextArea2.setText(jTextArea2.getText() + "La expresion: " + cadena + " es invalida para la Expresion Regular: " + expresion + "\n");
                } else {
                    cadenaActual = cadena;
                    Map<String, Map<String, ArrayList<String>>> estados = arbol.getMapeoEstados();
                    Transicion transicion = arbol.getTransiciones().get(0);
                    estadoActual = "";
                    arbol.getTerminales().entrySet().forEach((terminal) -> {
                        if (terminal.getKey().equals("#")) {
                            estadoAceptacion = Integer.parseInt(String.valueOf(terminal.getValue().get(0)));
                        }
                    });
                    for (Object obj : transicion.getEstado()) {
                        if (estadoActual.isEmpty()) {
                            estadoActual = String.valueOf(obj);
                        } else {
                            estadoActual += ", " + obj;
                        }
                    }

                    while (true) {
                        cadenaCorrecta = 0;
                        if (!cadenaActual.isEmpty()) {
                            estados.entrySet().forEach((estado) -> {
                                if (estado.getKey().equals(estadoActual)) {
                                    estado.getValue().entrySet().forEach((valor) -> {
                                        for (String str : valor.getValue()) {
                                            if (cadenaActual.startsWith(str)) {
                                                cadenaCorrecta++;
                                                cadenaActual = cadenaActual.replaceFirst(str, "");
                                                estadoActual = valor.getKey();
                                            }
                                        }
                                    });
                                }
                            });
                        } else {
                            if (estadoActual.contains(", " + estadoAceptacion) || estadoActual.contains(", " + estadoAceptacion + ",")) {
                                jTextArea2.setText(jTextArea2.getText() + "La expresion: " + cadena + " es valida para la Expresion Regular: " + expresion + "\n");
                                break;
                            } else {
                                jTextArea2.setText(jTextArea2.getText() + "La expresion: " + cadena + " es invalida para la Expresion Regular: " + expresion + "\n");
                                break;
                            }
                        }
                        if (cadenaCorrecta == 0) {
                            jTextArea2.setText(jTextArea2.getText() + "La expresion: " + cadena + " es invalida para la Expresion Regular: " + expresion + "\n");
                            break;
                        }
                    }
                }
            }
        }
    }

    private void jMenuItem9ActionPerformed(java.awt.event.ActionEvent evt) {//GEN-FIRST:event_jMenuItem9ActionPerformed
        // TODO add your handling code here:
        if (path.isEmpty()) {
            JFileChooser fc = new JFileChooser();
            fc.setCurrentDirectory(new File("C:/Users/USER/Desktop/PruebaPractica1/"));
            fc.setMultiSelectionEnabled(false);
            FileNameExtensionFilter filter = new FileNameExtensionFilter("Expresiones Regulares", "er", "text");
            fc.setFileFilter(filter);
            try {
                int returnVal = fc.showSaveDialog(this);
                if (returnVal == JFileChooser.APPROVE_OPTION) {
                    File file = fc.getSelectedFile();
                    path = file.getPath();
                    File nuevo = new File(path + ".er");
                    if (nuevo.createNewFile()) {
                        try (FileWriter myWriter = new FileWriter(nuevo)) {
                            myWriter.write(jTextArea1.getText());
                            myWriter.close();
                        }
                    } else {
                        System.out.println("Archivo ya existe");
                    }
                }
            } catch (HeadlessException e) {
                showMessageDialog(null, "Error: por favor verifique la ruta de su archivo.");

            } catch (IOException ex) {
                Logger.getLogger(Principal.class.getName()).log(Level.SEVERE, null, ex);
            }
        } else {
            File nuevo = new File(path);
            nuevo.delete();
            try {
                if (nuevo.createNewFile()) {
                    try (FileWriter myWriter = new FileWriter(nuevo)) {
                        myWriter.write(jTextArea1.getText());
                        myWriter.close();
                    }
                }
            } catch (IOException ex) {
                Logger.getLogger(Principal.class.getName()).log(Level.SEVERE, null, ex);
            }
        }
    }//GEN-LAST:event_jMenuItem9ActionPerformed

    private void jMenuItem10ActionPerformed(java.awt.event.ActionEvent evt) {//GEN-FIRST:event_jMenuItem10ActionPerformed
        // TODO add your handling code here:
        JFileChooser fc = new JFileChooser();
        fc.setCurrentDirectory(new File("C:/Users/USER/Desktop/PruebaPractica1/"));
        fc.setMultiSelectionEnabled(false);
        FileNameExtensionFilter filter = new FileNameExtensionFilter("Expresiones Regulares", "er", "text");
        fc.setFileFilter(filter);
        try {
            int returnVal = fc.showSaveDialog(this);
            if (returnVal == JFileChooser.APPROVE_OPTION) {
                File file = fc.getSelectedFile();
                path = file.getPath();
                File nuevo = new File(path + ".er");
                if (nuevo.createNewFile()) {
                    try (FileWriter myWriter = new FileWriter(nuevo)) {
                        myWriter.write(jTextArea1.getText());
                        myWriter.close();
                    }
                } else {
                    System.out.println("Archivo ya existe");
                }
            }
        } catch (HeadlessException e) {
            showMessageDialog(null, "Error: por favor verifique la ruta de su archivo.");

        } catch (IOException ex) {
            Logger.getLogger(Principal.class.getName()).log(Level.SEVERE, null, ex);
        }
    }//GEN-LAST:event_jMenuItem10ActionPerformed

    private void jMenuItem4ActionPerformed(java.awt.event.ActionEvent evt) {//GEN-FIRST:event_jMenuItem4ActionPerformed
        // TODO add your handling code here:
        String ruta = "C:/Users/USER/Desktop/PruebaPractica1/Arboles/";
        indexPath = 0;
        paths = new ArrayList<>();
        for (Arbol arbol : listaArboles) {
            paths.add(ruta + arbol.getNombre() + ".png");
        }
        if (!paths.isEmpty()) {
            ImageIcon icono = new ImageIcon(paths.get(indexPath));
            ImageIcon icon = new ImageIcon(icono.getImage().getScaledInstance(jLabel2.getWidth(), jLabel2.getHeight(), Image.SCALE_DEFAULT));
            jLabel2.setIcon(icon);
            jLabel3.setText(listaArboles.get(indexPath).getNombre());
        }
    }//GEN-LAST:event_jMenuItem4ActionPerformed

    private void jButton2ActionPerformed(java.awt.event.ActionEvent evt) {//GEN-FIRST:event_jButton2ActionPerformed
        // TODO add your handling code here:
        if (indexPath == (paths.size() - 1)) {
            indexPath = 0;
        } else {
            indexPath++;
        }

        if (!paths.isEmpty()) {
            ImageIcon icono = new ImageIcon(paths.get(indexPath));
            ImageIcon icon = new ImageIcon(icono.getImage().getScaledInstance(jLabel2.getWidth(), jLabel2.getHeight(), Image.SCALE_DEFAULT));
            jLabel2.setIcon(icon);
            jLabel3.setText(listaArboles.get(indexPath).getNombre());
        }
    }//GEN-LAST:event_jButton2ActionPerformed

    private void jButton1ActionPerformed(java.awt.event.ActionEvent evt) {//GEN-FIRST:event_jButton1ActionPerformed
        // TODO add your handling code here:
        if (indexPath == 0) {
            indexPath = paths.size() - 1;
        } else {
            indexPath--;
        }

        if (!paths.isEmpty()) {
            ImageIcon icono = new ImageIcon(paths.get(indexPath));
            ImageIcon icon = new ImageIcon(icono.getImage().getScaledInstance(jLabel2.getWidth(), jLabel2.getHeight(), Image.SCALE_DEFAULT));
            jLabel2.setIcon(icon);
            jLabel3.setText(listaArboles.get(indexPath).getNombre());
        }
    }//GEN-LAST:event_jButton1ActionPerformed

    private void jMenuItem5ActionPerformed(java.awt.event.ActionEvent evt) {//GEN-FIRST:event_jMenuItem5ActionPerformed
        // TODO add your handling code here:
        String ruta = "C:/Users/USER/Desktop/PruebaPractica1/Automatas/";
        indexPath = 0;
        paths = new ArrayList<>();
        for (Arbol arbol : listaArboles) {
            paths.add(ruta + arbol.getNombre() + ".png");
        }
        if (!paths.isEmpty()) {
            ImageIcon icono = new ImageIcon(paths.get(indexPath));
            ImageIcon icon = new ImageIcon(icono.getImage().getScaledInstance(jLabel2.getWidth(), jLabel2.getHeight(), Image.SCALE_DEFAULT));
            jLabel2.setIcon(icon);
            jLabel3.setText(listaArboles.get(indexPath).getNombre());
        }
    }//GEN-LAST:event_jMenuItem5ActionPerformed

    private void jMenuItem6ActionPerformed(java.awt.event.ActionEvent evt) {//GEN-FIRST:event_jMenuItem6ActionPerformed
        // TODO add your handling code here:
        String ruta = "C:/Users/USER/Desktop/PruebaPractica1/Siguientes/";
        indexPath = 0;
        paths = new ArrayList<>();
        for (Arbol arbol : listaArboles) {
            paths.add(ruta + arbol.getNombre() + ".png");
        }
        if (!paths.isEmpty()) {
            ImageIcon icono = new ImageIcon(paths.get(indexPath));
            ImageIcon icon = new ImageIcon(icono.getImage().getScaledInstance(jLabel2.getWidth(), jLabel2.getHeight(), Image.SCALE_DEFAULT));
            jLabel2.setIcon(icon);
            jLabel3.setText(listaArboles.get(indexPath).getNombre());
        }
    }//GEN-LAST:event_jMenuItem6ActionPerformed

    private void jMenuItem7ActionPerformed(java.awt.event.ActionEvent evt) {//GEN-FIRST:event_jMenuItem7ActionPerformed
        // TODO add your handling code here:
        String ruta = "C:/Users/USER/Desktop/PruebaPractica1/Transiciones/";
        indexPath = 0;
        paths = new ArrayList<>();
        for (Arbol arbol : listaArboles) {
            paths.add(ruta + arbol.getNombre() + ".png");
        }
        if (!paths.isEmpty()) {
            ImageIcon icono = new ImageIcon(paths.get(indexPath));
            ImageIcon icon = new ImageIcon(icono.getImage().getScaledInstance(jLabel2.getWidth(), jLabel2.getHeight(), Image.SCALE_DEFAULT));
            jLabel2.setIcon(icon);
            jLabel3.setText(listaArboles.get(indexPath).getNombre());
        }
    }//GEN-LAST:event_jMenuItem7ActionPerformed

    /**
     * @param args the command line arguments
     */
    public static void main(String args[]) {
        /* Set the Nimbus look and feel */
        //<editor-fold defaultstate="collapsed" desc=" Look and feel setting code (optional) ">
        /* If Nimbus (introduced in Java SE 6) is not available, stay with the default look and feel.
         * For details see http://download.oracle.com/javase/tutorial/uiswing/lookandfeel/plaf.html 
         */
        try {
            for (javax.swing.UIManager.LookAndFeelInfo info : javax.swing.UIManager.getInstalledLookAndFeels()) {
                if ("Nimbus".equals(info.getName())) {
                    javax.swing.UIManager.setLookAndFeel(info.getClassName());
                    break;
                }
            }
        } catch (ClassNotFoundException ex) {
            java.util.logging.Logger.getLogger(Principal.class.getName()).log(java.util.logging.Level.SEVERE, null, ex);
        } catch (InstantiationException ex) {
            java.util.logging.Logger.getLogger(Principal.class.getName()).log(java.util.logging.Level.SEVERE, null, ex);
        } catch (IllegalAccessException ex) {
            java.util.logging.Logger.getLogger(Principal.class.getName()).log(java.util.logging.Level.SEVERE, null, ex);
        } catch (javax.swing.UnsupportedLookAndFeelException ex) {
            java.util.logging.Logger.getLogger(Principal.class.getName()).log(java.util.logging.Level.SEVERE, null, ex);
        }
        //</editor-fold>

        /* Create and display the form */
        java.awt.EventQueue.invokeLater(new Runnable() {
            public void run() {
                new Principal().setVisible(true);
            }
        });
    }

    // Variables declaration - do not modify//GEN-BEGIN:variables
    private javax.swing.JButton jButton1;
    private javax.swing.JButton jButton2;
    private javax.swing.JLabel jLabel1;
    private javax.swing.JLabel jLabel2;
    private javax.swing.JLabel jLabel3;
    private javax.swing.JMenu jMenu1;
    private javax.swing.JMenu jMenu2;
    private javax.swing.JMenu jMenu3;
    private javax.swing.JMenu jMenu4;
    private javax.swing.JMenu jMenu5;
    private javax.swing.JMenu jMenu6;
    private javax.swing.JMenu jMenu7;
    private javax.swing.JMenuBar jMenuBar2;
    private javax.swing.JMenuBar jMenuBar3;
    private javax.swing.JMenuItem jMenuItem1;
    private javax.swing.JMenuItem jMenuItem10;
    private javax.swing.JMenuItem jMenuItem2;
    private javax.swing.JMenuItem jMenuItem3;
    private javax.swing.JMenuItem jMenuItem4;
    private javax.swing.JMenuItem jMenuItem5;
    private javax.swing.JMenuItem jMenuItem6;
    private javax.swing.JMenuItem jMenuItem7;
    private javax.swing.JMenuItem jMenuItem8;
    private javax.swing.JMenuItem jMenuItem9;
    private javax.swing.JScrollPane jScrollPane1;
    private javax.swing.JScrollPane jScrollPane2;
    private javax.swing.JTextArea jTextArea1;
    private javax.swing.JTextArea jTextArea2;
    // End of variables declaration//GEN-END:variables
}
