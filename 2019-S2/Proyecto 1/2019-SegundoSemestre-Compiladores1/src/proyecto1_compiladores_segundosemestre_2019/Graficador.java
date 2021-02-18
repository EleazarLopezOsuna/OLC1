/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package proyecto1_compiladores_segundosemestre_2019;

import Modelos.Componente;
import Modelos.Estilo;
import java.awt.Color;
import java.awt.Component;
import java.awt.Dimension;
import java.awt.Font;
import java.awt.event.ActionEvent;
import java.net.MalformedURLException;
import java.net.URL;
import java.util.ArrayList;
import java.util.HashMap;
import java.util.logging.Level;
import java.util.logging.Logger;
import javax.swing.BorderFactory;
import javax.swing.ImageIcon;
import javax.swing.JButton;
import javax.swing.JComboBox;
import javax.swing.JFrame;
import javax.swing.JLabel;
import javax.swing.JOptionPane;
import javax.swing.JPanel;
import javax.swing.JSpinner;
import javax.swing.JTextField;
import javax.swing.SpinnerModel;
import javax.swing.SpinnerNumberModel;
import javax.swing.SwingConstants;
import javax.swing.WindowConstants;
import javax.swing.border.Border;

/**
 *
 * @author Eleazar Lopez <Universidad de San Carlos de Guatemala>
 */
public class Graficador {

    HashMap<String, Componente> componentes;

    public Graficador(HashMap<String, Componente> componentes) {
        this.componentes = componentes;
    }

    public void agregarPanel(ArrayList<Componente> componentes, JPanel panelPrincipal, JFrame frame) {
        for (Componente componente : componentes) {
            switch (componente.getTipo()) {
                case panel:
                    JPanel panel = new JPanel();
                    panel.setName(componente.getId());
                    panel.setBounds(componente.getPosX(), componente.getPosY(), componente.getWidth(), componente.getHeight());
                    panel.setBackground(componente.getColor());
                    if (componente.getBorder() > 0) {
                        Border border = BorderFactory.createLineBorder(Color.black, componente.getBorder());
                        panel.setBorder(border);
                    }
                    panel.setLayout(null);
                    panel.setName(componente.getNombre());
                    agregarPanel(componente.getComponentes(), panel, frame);
                    for (Estilo estilo : componente.getEstilos()) {
                        if (estilo.getBackground() != null) {
                            panel.setBackground(estilo.getBackground());
                        }
                        if (estilo.getBorde() != null) {
                            if (estilo.getBorde()) {
                                if (estilo.getColorBorde() != null) {
                                    if (estilo.getAnchoBorde() >= 0) {
                                        Border border = BorderFactory.createLineBorder(estilo.getColorBorde(), estilo.getAnchoBorde());
                                        panel.setBorder(border);
                                    }
                                }
                            } else {
                                panel.setBorder(null);
                            }
                        }
                        if (estilo.getFuente() != null) {
                            if (estilo.getAlturaFuente() >= 0) {
                                Font fuente = new Font(estilo.getFuente(), Font.PLAIN, estilo.getAlturaFuente());
                                panel.setFont(fuente);
                            }
                        }
                        if (estilo.getColorFuente() != null) {
                            panel.setForeground(estilo.getColorFuente());
                        }
                    }
                    panelPrincipal.add(panel);
                    break;
                case text:
                    JLabel label = new JLabel();
                    label.setName(componente.getId());
                    label.setBounds(componente.getPosX(), componente.getPosY(), componente.getWidth(), componente.getHeight());
                    label.setBackground(componente.getColor());
                    if (componente.getBorder() > 0) {
                        Border border = BorderFactory.createLineBorder(Color.black, componente.getBorder());
                        label.setBorder(border);
                    }
                    label.setText(componente.getContenido());
                    for (Estilo estilo : componente.getEstilos()) {
                        if (estilo.getBackground() != null) {
                            label.setBackground(estilo.getBackground());
                        }
                        if (estilo.getBorde() != null) {
                            if (estilo.getBorde()) {
                                if (estilo.getColorBorde() != null) {
                                    if (estilo.getAnchoBorde() >= 0) {
                                        Border border = BorderFactory.createLineBorder(estilo.getColorBorde(), estilo.getAnchoBorde());
                                        label.setBorder(border);
                                    }
                                }
                            } else {
                                label.setBorder(null);
                            }
                        }
                        if (estilo.getAlineacion() != null) {
                            switch (estilo.getAlineacion().toLowerCase()) {
                                case "left":
                                    label.setHorizontalAlignment(SwingConstants.LEFT);
                                    break;
                                case "right":
                                    label.setHorizontalAlignment(SwingConstants.RIGHT);
                                    break;
                                case "center":
                                    label.setHorizontalAlignment(SwingConstants.CENTER);
                                    break;
                            }
                        }
                        if (estilo.getFuente() != null) {
                            if (estilo.getAlturaFuente() >= 0) {
                                Font fuente = new Font(estilo.getFuente(), Font.PLAIN, estilo.getAlturaFuente());
                                label.setFont(fuente);
                            }
                        }
                        if (estilo.getColorFuente() != null) {
                            label.setForeground(estilo.getColorFuente());
                        }
                    }
                    panelPrincipal.add(label);
                    break;
                case textfield:
                    JTextField textField = new JTextField();
                    textField.setName(componente.getId());
                    textField.setBounds(componente.getPosX(), componente.getPosY(), componente.getWidth(), componente.getHeight());
                    textField.setBackground(componente.getColor());
                    if (componente.getBorder() > 0) {
                        Border border = BorderFactory.createLineBorder(Color.black, componente.getBorder());
                        textField.setBorder(border);
                    }
                    textField.setText(componente.getContenido());
                    for (Estilo estilo : componente.getEstilos()) {
                        if (estilo.getBackground() != null) {
                            textField.setBackground(estilo.getBackground());
                        }
                        if (estilo.getBorde() != null) {
                            if (estilo.getBorde()) {
                                if (estilo.getColorBorde() != null) {
                                    if (estilo.getAnchoBorde() >= 0) {
                                        Border border = BorderFactory.createLineBorder(estilo.getColorBorde(), estilo.getAnchoBorde());
                                        textField.setBorder(border);
                                    }
                                }
                            } else {
                                textField.setBorder(null);
                            }
                        }
                        if (estilo.getAlineacion() != null) {
                            switch (estilo.getAlineacion().toLowerCase()) {
                                case "left":
                                    textField.setHorizontalAlignment(SwingConstants.LEFT);
                                    break;
                                case "right":
                                    textField.setHorizontalAlignment(SwingConstants.RIGHT);
                                    break;
                                case "center":
                                    textField.setHorizontalAlignment(SwingConstants.CENTER);
                                    break;
                            }
                        }
                        if (estilo.getFuente() != null) {
                            if (estilo.getAlturaFuente() >= 0) {
                                Font fuente = new Font(estilo.getFuente(), Font.PLAIN, estilo.getAlturaFuente());
                                textField.setFont(fuente);
                            }
                        }
                        if (estilo.getColorFuente() != null) {
                            textField.setForeground(estilo.getColorFuente());
                        }
                    }
                    panelPrincipal.add(textField);
                    break;
                case button:
                    JButton boton = new JButton();
                    boton.setName(componente.getNombre());
                    boton.setBounds(componente.getPosX(), componente.getPosY(), componente.getWidth(), componente.getHeight());
                    boton.setBackground(componente.getColor());
                    if (componente.getBorder() > 0) {
                        Border border = BorderFactory.createLineBorder(Color.black, componente.getBorder());
                        boton.setBorder(border);
                    }
                    boton.setText(componente.getNombre());
                    boton.addActionListener((ActionEvent e) -> {
                        JOptionPane.showMessageDialog(frame, componente.getOnClick());
                    });
                    for (Estilo estilo : componente.getEstilos()) {
                        if (estilo.getBackground() != null) {
                            boton.setBackground(estilo.getBackground());
                        }
                        if (estilo.getBorde() != null) {
                            if (estilo.getBorde()) {
                                if (estilo.getColorBorde() != null) {
                                    if (estilo.getAnchoBorde() >= 0) {
                                        Border border = BorderFactory.createLineBorder(estilo.getColorBorde(), estilo.getAnchoBorde());
                                        boton.setBorder(border);
                                    }
                                }
                            } else {
                                boton.setBorder(null);
                            }
                        }
                        if (estilo.getAlineacion() != null) {
                            switch (estilo.getAlineacion().toLowerCase()) {
                                case "left":
                                    boton.setHorizontalAlignment(SwingConstants.LEFT);
                                    break;
                                case "right":
                                    boton.setHorizontalAlignment(SwingConstants.RIGHT);
                                    break;
                                case "center":
                                    boton.setHorizontalAlignment(SwingConstants.CENTER);
                                    break;
                            }
                        }
                        if (estilo.getFuente() != null) {
                            if (estilo.getAlturaFuente() >= 0) {
                                Font fuente = new Font(estilo.getFuente(), Font.PLAIN, estilo.getAlturaFuente());
                                boton.setFont(fuente);
                            }
                        }
                        if (estilo.getColorFuente() != null) {
                            boton.setForeground(estilo.getColorFuente());
                        }
                    }
                    panelPrincipal.add(boton);
                    break;
                case image:
                    URL direccion;
                    try {
                        direccion = new URL(componente.getFuente());
                        ImageIcon imagen = new ImageIcon(direccion);
                        label = new JLabel();
                        label.setName(componente.getId());
                        label.setBounds(componente.getPosX(), componente.getPosY(), componente.getWidth(), componente.getHeight());
                        label.setBackground(componente.getColor());
                        if (componente.getBorder() > 0) {
                            Border border = BorderFactory.createLineBorder(Color.black, componente.getBorder());
                            label.setBorder(border);
                        }
                        label.setText(componente.getContenido());
                        label.setIcon(imagen);
                        for (Estilo estilo : componente.getEstilos()) {
                            if (estilo.getBackground() != null) {
                                label.setBackground(estilo.getBackground());
                            }
                            if (estilo.getBorde() != null) {
                                if (estilo.getBorde()) {
                                    if (estilo.getColorBorde() != null) {
                                        if (estilo.getAnchoBorde() >= 0) {
                                            Border border = BorderFactory.createLineBorder(estilo.getColorBorde(), estilo.getAnchoBorde());
                                            label.setBorder(border);
                                        }
                                    }
                                } else {
                                    label.setBorder(null);
                                }
                            }
                            if (estilo.getAlineacion() != null) {
                                switch (estilo.getAlineacion().toLowerCase()) {
                                    case "left":
                                        label.setHorizontalAlignment(SwingConstants.LEFT);
                                        break;
                                    case "right":
                                        label.setHorizontalAlignment(SwingConstants.RIGHT);
                                        break;
                                    case "center":
                                        label.setHorizontalAlignment(SwingConstants.CENTER);
                                        break;
                                }
                            }
                            if (estilo.getFuente() != null) {
                                if (estilo.getAlturaFuente() >= 0) {
                                    Font fuente = new Font(estilo.getFuente(), Font.PLAIN, estilo.getAlturaFuente());
                                    label.setFont(fuente);
                                }
                            }
                            if (estilo.getColorFuente() != null) {
                                label.setForeground(estilo.getColorFuente());
                            }
                        }
                        panelPrincipal.add(label);
                    } catch (MalformedURLException ex) {
                        Logger.getLogger(Graficador.class.getName()).log(Level.SEVERE, null, ex);
                    }
                    break;
                case spinner:
                    SpinnerModel modelo = new SpinnerNumberModel(componente.getDefecto(), componente.getMin(), componente.getMax(), 1);
                    JSpinner spinner = new JSpinner(modelo);
                    spinner.setName(componente.getId());
                    spinner.setBounds(componente.getPosX(), componente.getPosY(), componente.getWidth(), componente.getHeight());
                    spinner.setBackground(componente.getColor());
                    if (componente.getBorder() > 0) {
                        Border border = BorderFactory.createLineBorder(Color.black, componente.getBorder());
                        spinner.setBorder(border);
                    }
                    for (Estilo estilo : componente.getEstilos()) {
                        if (estilo.getBackground() != null) {
                            spinner.setBackground(estilo.getBackground());
                        }
                        if (estilo.getBorde() != null) {
                            if (estilo.getBorde()) {
                                if (estilo.getColorBorde() != null) {
                                    if (estilo.getAnchoBorde() >= 0) {
                                        Border border = BorderFactory.createLineBorder(estilo.getColorBorde(), estilo.getAnchoBorde());
                                        spinner.setBorder(border);
                                    }
                                }
                            } else {
                                spinner.setBorder(null);
                            }
                        }
                        if (estilo.getFuente() != null) {
                            if (estilo.getAlturaFuente() >= 0) {
                                Font fuente = new Font(estilo.getFuente(), Font.PLAIN, estilo.getAlturaFuente());
                                spinner.setFont(fuente);
                            }
                        }
                        if (estilo.getColorFuente() != null) {
                            spinner.setForeground(estilo.getColorFuente());
                        }
                    }
                    panelPrincipal.add(spinner);
                    break;
                case list:
                    JComboBox comboBox = new JComboBox();
                    comboBox.setName(componente.getId());
                    comboBox.setBounds(componente.getPosX(), componente.getPosY(), componente.getWidth(), componente.getHeight());
                    comboBox.setBackground(componente.getColor());
                    if (componente.getBorder() > 0) {
                        Border border = BorderFactory.createLineBorder(Color.black, componente.getBorder());
                        comboBox.setBorder(border);
                    }
                    componente.getItems().forEach(comboBox::addItem);
                    comboBox.setSelectedIndex(componente.getDefecto());
                    for (Estilo estilo : componente.getEstilos()) {
                        if (estilo.getBackground() != null) {
                            comboBox.setBackground(estilo.getBackground());
                        }
                        if (estilo.getBorde() != null) {
                            if (estilo.getBorde()) {
                                if (estilo.getColorBorde() != null) {
                                    if (estilo.getAnchoBorde() >= 0) {
                                        Border border = BorderFactory.createLineBorder(estilo.getColorBorde(), estilo.getAnchoBorde());
                                        comboBox.setBorder(border);
                                    }
                                }
                            } else {
                                comboBox.setBorder(null);
                            }
                        }
                        if (estilo.getFuente() != null) {
                            if (estilo.getAlturaFuente() >= 0) {
                                Font fuente = new Font(estilo.getFuente(), Font.PLAIN, estilo.getAlturaFuente());
                                comboBox.setFont(fuente);
                            }
                        }
                        if (estilo.getColorFuente() != null) {
                            comboBox.setForeground(estilo.getColorFuente());
                        }
                    }
                    panelPrincipal.add(comboBox);
                    break;
            }
        }
    }

    public void agregarFrame(ArrayList<Componente> componentes, JFrame frame, String div) {
        for (Componente componente : componentes) {
            switch (componente.getTipo()) {
                case panel:
                    JPanel panel = new JPanel();
                    panel.setName(componente.getId());
                    panel.setBounds(componente.getPosX(), componente.getPosY(), componente.getWidth(), componente.getHeight());
                    panel.setBackground(componente.getColor());
                    if (componente.getBorder() > 0) {
                        Border border = BorderFactory.createLineBorder(Color.black, componente.getBorder());
                        panel.setBorder(border);
                    }
                    panel.setName(div);
                    panel.setLayout(null);
                    agregarPanel(componente.getComponentes(), panel, frame);
                    frame.add(panel);
                    break;
                case text:
                    JLabel label = new JLabel();
                    label.setName(componente.getId());
                    label.setBounds(componente.getPosX(), componente.getPosY(), componente.getWidth(), componente.getHeight());
                    label.setBackground(componente.getColor());
                    if (componente.getBorder() > 0) {
                        Border border = BorderFactory.createLineBorder(Color.black, componente.getBorder());
                        label.setBorder(border);
                    }
                    label.setText(componente.getContenido());
                    frame.add(label);
                    break;
                case textfield:
                    JTextField textField = new JTextField();
                    textField.setName(componente.getId());
                    textField.setBounds(componente.getPosX(), componente.getPosY(), componente.getWidth(), componente.getHeight());
                    textField.setBackground(componente.getColor());
                    if (componente.getBorder() > 0) {
                        Border border = BorderFactory.createLineBorder(Color.black, componente.getBorder());
                        textField.setBorder(border);
                    }
                    textField.setText(componente.getContenido());
                    frame.add(textField);
                    break;
                case button:
                    JButton boton = new JButton();
                    boton.setName(componente.getNombre());
                    boton.setBounds(componente.getPosX(), componente.getPosY(), componente.getWidth(), componente.getHeight());
                    boton.setBackground(componente.getColor());
                    if (componente.getBorder() > 0) {
                        Border border = BorderFactory.createLineBorder(Color.black, componente.getBorder());
                        boton.setBorder(border);
                    }
                    boton.setText(componente.getNombre());
                    boton.addActionListener((ActionEvent e) -> {
                        JOptionPane.showMessageDialog(frame, componente.getOnClick());
                    });
                    frame.add(boton);
                    break;
                case image:
                    URL direccion;
                    try {
                        direccion = new URL(componente.getFuente());
                        ImageIcon imagen = new ImageIcon(direccion);
                        label = new JLabel();
                        label.setName(componente.getId());
                        label.setBounds(componente.getPosX(), componente.getPosY(), componente.getWidth(), componente.getHeight());
                        label.setBackground(componente.getColor());
                        if (componente.getBorder() > 0) {
                            Border border = BorderFactory.createLineBorder(Color.black, componente.getBorder());
                            label.setBorder(border);
                        }
                        label.setText(componente.getContenido());
                        label.setIcon(imagen);
                        frame.add(label);
                    } catch (MalformedURLException ex) {
                        Logger.getLogger(Graficador.class.getName()).log(Level.SEVERE, null, ex);
                    }
                    break;
                case spinner:
                    SpinnerModel modelo = new SpinnerNumberModel(componente.getDefecto(), componente.getMin(), componente.getMax(), 1);
                    JSpinner spinner = new JSpinner(modelo);
                    spinner.setName(componente.getId());
                    spinner.setBounds(componente.getPosX(), componente.getPosY(), componente.getWidth(), componente.getHeight());
                    spinner.setBackground(componente.getColor());
                    if (componente.getBorder() > 0) {
                        Border border = BorderFactory.createLineBorder(Color.black, componente.getBorder());
                        spinner.setBorder(border);
                    }
                    frame.add(spinner);
                    break;
                case list:
                    JComboBox comboBox = new JComboBox();
                    comboBox.setName(componente.getId());
                    comboBox.setBounds(componente.getPosX(), componente.getPosY(), componente.getWidth(), componente.getHeight());
                    comboBox.setBackground(componente.getColor());
                    if (componente.getBorder() > 0) {
                        Border border = BorderFactory.createLineBorder(Color.black, componente.getBorder());
                        comboBox.setBorder(border);
                    }
                    componente.getItems().forEach(comboBox::addItem);
                    comboBox.setSelectedIndex(componente.getDefecto());
                    frame.add(comboBox);
                    break;
            }
        }
    }

    public void Graficar(String error) {
        JFrame frame = new JFrame();
        frame.setLayout(null);
        if (error.isEmpty()) {
            componentes.forEach((div, componente) -> {
                switch (componente.getTipo()) {
                    case ufex:
                        agregarFrame(componente.getComponentes(), frame, div);
                        break;
                    default:
                        break;
                }
            });
        } else {
            JLabel errorL = new JLabel();
            errorL.setBounds(0, 0, 1000, 100);
            errorL.setText(error);
            frame.add(errorL);
        }
        frame.setDefaultCloseOperation(WindowConstants.DISPOSE_ON_CLOSE);
        frame.setPreferredSize(new Dimension(500, 500));
        frame.pack();
        frame.setLocationRelativeTo(null);
        frame.setVisible(true);
    }
}
