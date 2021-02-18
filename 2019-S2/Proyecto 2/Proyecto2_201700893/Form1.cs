using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Irony.Interpreter;
using Irony.Parsing;
using Proyecto2_201700893.Analizador;
using Proyecto2_201700893.Modelos;

namespace Proyecto2_201700893
{
    public partial class Form1 : Form
    {
        private ParseTree resultadoAnalisis = null;
        private NodoSintactico root = null;
        private int contadorNodos = 0;

        public Form1()
        {
            InitializeComponent();
        }

        //Boton Nuevo Archivo
        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Prevent "File already exist" message
            saveFileDialog1.OverwritePrompt = false;

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                String path = saveFileDialog1.FileName;
                if (!File.Exists(path))
                {
                    File.Create(path);

                    //Creation of new Controls
                    TabPage newTab = new TabPage();
                    RichTextBox newText = new RichTextBox();
                    ToolStripMenuItem newItem = new ToolStripMenuItem();

                    //Setting properties to the new controls
                    newText.Name = "text_" + Path.GetFileName(path);
                    newText.Dock = DockStyle.Fill;

                    newTab.Name = path;
                    newTab.Text = Path.GetFileName(path);

                    newItem.Name = path;
                    newItem.Text = Path.GetFileName(path);
                    newItem.Click += new EventHandler(cerrarItem);
                    newItem.Image = Properties.Resources.eliminar;

                    //Adding the RichText to the TabPage
                    newTab.Controls.Add(newText);

                    //Adding the TabPage to the TabControl
                    tabControl1.Controls.Add(newTab);

                    //Reseting saveFileDialog parameters
                    saveFileDialog1.FileName = "";

                    //Adding the new Item to the CurrentFile menu
                    currentFilesToolStripMenuItem.DropDownItems.Add(newItem);
                }
                else
                {
                    MessageBox.Show("El archivo: " + Path.GetFileNameWithoutExtension(path) + " ya existe");
                }
            }
            if (tabControl1.TabPages.Count == 0)
            {
                label1.Show();
                label2.Show();
                setEnabled(false);
            }
            else
            {
                label1.Hide();
                label2.Hide();
                setEnabled(true);
            }
        }

        private void cerrarItem(object sender, EventArgs e)
        {
            ToolStripMenuItem item = sender as ToolStripMenuItem;
            currentFilesToolStripMenuItem.DropDownItems.Remove(item);
            tabControl1.TabPages.RemoveByKey(item.Name);
            if (tabControl1.TabPages.Count == 0)
            {
                label1.Show();
                label2.Show();
                setEnabled(false);
            }
        }

        private void setEnabled(bool valor)
        {
            runFileToolStripMenuItem.Enabled = valor;
            saveFileAsToolStripMenuItem.Enabled = valor;
            saveFileToolStripMenuItem.Enabled = valor;
        }

        private void openFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                String path = openFileDialog1.FileName;

                //Creation of new Controls
                TabPage newTab = new TabPage();
                RichTextBox newText = new RichTextBox();
                ToolStripMenuItem newItem = new ToolStripMenuItem();

                //Setting properties to the new controls
                newText.Name = "text_" + Path.GetFileName(path);
                newText.Dock = DockStyle.Fill;

                newTab.Name = path;
                newTab.Text = Path.GetFileName(path);

                newItem.Name = path;
                newItem.Text = Path.GetFileName(path);
                newItem.Click += new EventHandler(cerrarItem);
                newItem.Image = Properties.Resources.eliminar;

                //Filling the newText area
                newText.Text = File.ReadAllText(path);

                //Adding the RichText to the TabPage
                newTab.Controls.Add(newText);

                //Adding the TabPage to the TabControl
                tabControl1.Controls.Add(newTab);

                //Reseting saveFileDialog parameters
                saveFileDialog1.FileName = "";

                //Adding the new Item to the CurrentFile menu
                currentFilesToolStripMenuItem.DropDownItems.Add(newItem);
            }
            if (tabControl1.TabPages.Count == 0)
            {
                label1.Show();
                label2.Show();
                setEnabled(false);
            }
            else
            {
                label1.Hide();
                label2.Hide();
                setEnabled(true);
            }
        }

        private void saveFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string key = "text_" + tabControl1.SelectedTab.Text;

            //Fetching the RichTextBox on the TabPage
            Control[] control = tabControl1.SelectedTab.Controls.Find(key, true);
            RichTextBox text = (RichTextBox)control[0];
            File.WriteAllText(tabControl1.SelectedTab.Name, text.Text);
        }

        private void saveFileAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Prevent "File already exist" message
            saveFileDialog1.OverwritePrompt = false;

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                String fileName = saveFileDialog1.FileName;
                if (!File.Exists(fileName))
                {
                    string key = "text_" + tabControl1.SelectedTab.Text;

                    //Fetching the RichTextBox on the TabPage
                    Control[] control = tabControl1.SelectedTab.Controls.Find(key, true);
                    RichTextBox text = (RichTextBox)control[0];

                    //Create and write on the new File
                    File.WriteAllText(fileName, text.Text);

                    //Renaming the RichTextBox
                    text.Name = "text_" + Path.GetFileName(fileName);

                    //Remove the Previous File from the Current Files menu
                    currentFilesToolStripMenuItem.DropDownItems.RemoveByKey(tabControl1.SelectedTab.Name);

                    //Renaming the TabPage
                    tabControl1.SelectedTab.Name = fileName;

                    //Creating new Item for the Current Files menu
                    ToolStripMenuItem newItem = new ToolStripMenuItem();
                    newItem.Name = fileName;
                    newItem.Text = Path.GetFileName(fileName);
                    newItem.Click += new EventHandler(cerrarItem);
                    newItem.Image = Properties.Resources.eliminar;
                    currentFilesToolStripMenuItem.DropDownItems.Add(newItem);

                    //Setting new Text for the TabPage
                    tabControl1.SelectedTab.Text = Path.GetFileName(fileName);
                }
                else
                {
                    MessageBox.Show("El archivo: " + Path.GetFileNameWithoutExtension(fileName) + " ya existe");
                }
            }
        }

        private void runFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            resultadoAnalisis = null;
            aSTToolStripMenuItem.Enabled = false;
            dataGridView1.Rows.Clear();

            string key = "text_" + tabControl1.SelectedTab.Text;

            //Fetching the RichTextBox on the TabPage
            Control[] control = tabControl1.SelectedTab.Controls.Find(key, true);
            RichTextBox text = (RichTextBox)control[0];

            //Text analysis
            resultadoAnalisis = Sintactico.Analizar(text.Text);
            if (resultadoAnalisis != null)
            {
                if (resultadoAnalisis.ParserMessages.Count == 0)
                {
                    consoleText.Text = "";
                    recorrer(root, resultadoAnalisis.Root);
                    Semantico semantico = new Semantico();
                    semantico.IniciarAnalisisSemantico(root);
                    String cadena = "";
                    for (int i = 0; i < semantico.Consola.Count; i++)
                    {
                        cadena += semantico.Consola[i] + "\n";
                    }
                    if (semantico.Errores.Count > 0)
                    {
                        foreach (Error error in semantico.Errores)
                        {
                            //Creamos una nueva fila
                            int rowIndex = dataGridView1.Rows.Add();

                            //Seleccionamos la fila recien creada
                            DataGridViewRow row = dataGridView1.Rows[rowIndex];

                            //Llenamos la fila
                            if (error.archivo.Equals("2_0_1_7_0_0_8_9_3"))
                            {
                                row.Cells["Archivo"].Value = tabControl1.SelectedTab.Text;
                            }
                            else
                            {
                                row.Cells["Archivo"].Value = error.archivo;
                            }
                            row.Cells["Fila"].Value = error.fila;
                            row.Cells["Columna"].Value = error.columna;
                            row.Cells["Tipo"].Value = error.tipo;
                            row.Cells["Error"].Value = error.error;
                        }
                        tabControl2.SelectedIndex = 1;
                    }
                    consoleText.Text = cadena;
                    aSTToolStripMenuItem.Enabled = true;
                }
                else
                {
                    foreach (Irony.LogMessage error in resultadoAnalisis.ParserMessages)
                    {
                        //Creamos una nueva fila
                        int rowIndex = dataGridView1.Rows.Add();

                        //Seleccionamos la fila recien creada
                        DataGridViewRow row = dataGridView1.Rows[rowIndex];

                        //Llenamos la fila
                        row.Cells["Archivo"].Value = tabControl1.SelectedTab.Text;
                        row.Cells["Fila"].Value = error.Location.Line;
                        row.Cells["Columna"].Value = error.Location.Column;
                        if (error.Message.Contains("Invalid character"))
                        {
                            row.Cells["Tipo"].Value = "Lexico";
                            String errorx = error.Message.Replace("Invalid character: ", "");
                            errorx = errorx.Remove(errorx.Length - 1);
                            row.Cells["Error"].Value = "Caracter no reconocido: " + errorx;
                        }
                        else
                        {
                            row.Cells["Tipo"].Value = "Sintactico";
                            String errorx = error.Message.Replace("Syntax error, expected: ", "");
                            row.Cells["Error"].Value = "Se esperaba: " + errorx;
                        }
                    }
                    tabControl2.SelectedIndex = 1;
                }
            }
        }

        private void recorrer(NodoSintactico padreSintactico, ParseTreeNode padreIrony)
        {
            NodoSintactico nuevo;
            NodoSintactico nuevo2;
            string valor;
            switch (padreIrony.ToString())
            {
                #region inicio, import, main
                case "INICIO":
                    root = new NodoSintactico("INICIO", contadorNodos++);
                    foreach (ParseTreeNode hijo in padreIrony.ChildNodes)
                    {
                        recorrer(root, hijo);
                    }
                    break;
                case "GLOBAL":
                    root = new NodoSintactico("GLOBAL", contadorNodos++);
                    foreach (ParseTreeNode hijo in padreIrony.ChildNodes)
                    {
                        recorrer(root, hijo);
                    }
                    break;
                case "IMPORT":
                    if (padreIrony.ChildNodes.Count > 1)
                    {
                        if (!padreSintactico.getNombre().Equals("IMPORT"))
                        {
                            nuevo = new NodoSintactico("IMPORT", contadorNodos++);
                            recorrer(nuevo, padreIrony.ChildNodes[0]);
                            padreSintactico.addHijo(nuevo);
                            valor = padreIrony.ChildNodes[1].Token.ToString().Replace("\"", "");
                            valor = valor.Replace(" (cadena)", "");
                            nuevo2 = new NodoSintactico("IMPORT", contadorNodos++);
                            nuevo2.setValor(valor);
                            nuevo.addHijo(nuevo2);
                        }
                        else
                        {
                            nuevo = new NodoSintactico("IMPORT", contadorNodos++);
                            recorrer(padreSintactico, padreIrony.ChildNodes[0]);
                            valor = padreIrony.ChildNodes[1].Token.ToString().Replace("\"", "");
                            valor = valor.Replace(" (cadena)", "");
                            nuevo.setValor(valor);
                            padreSintactico.addHijo(nuevo);
                        }
                    }
                    else
                    {
                        nuevo = new NodoSintactico("IMPORT", contadorNodos++);
                        valor = padreIrony.ChildNodes[0].Token.ToString().Replace("\"", "");
                        valor = valor.Replace(" (cadena)", "");
                        nuevo.setValor(valor);
                        padreSintactico.addHijo(nuevo);
                    }
                    break;
                case "MAIN":
                    nuevo = new NodoSintactico("MAIN", contadorNodos++);
                    padreSintactico.addHijo(nuevo);
                    foreach (ParseTreeNode hijo in padreIrony.ChildNodes)
                    {
                        recorrer(nuevo, hijo);
                    }
                    break;
                #endregion

                #region listas: instrucciones, declaraciones, parametros
                case "L_INSTRUCCIONES":
                    if (padreIrony.ChildNodes.Count > 1)
                    {
                        if (!padreSintactico.getNombre().Equals("INSTRUCCION"))
                        {
                            //DATO PRIMERO
                            nuevo = new NodoSintactico("INSTRUCCION", contadorNodos++);
                            padreSintactico.addHijo(nuevo);
                            recorrer(nuevo, padreIrony.ChildNodes[0]);
                            recorrer(nuevo, padreIrony.ChildNodes[1]);
                        }
                        else
                        {
                            //DATO INTERMEDIO
                            recorrer(padreSintactico, padreIrony.ChildNodes[0]);
                            recorrer(padreSintactico, padreIrony.ChildNodes[1]);
                        }
                    }
                    else
                    {
                        //DATO ULTIMO
                        if (!padreSintactico.getNombre().Equals("INSTRUCCION"))
                        {
                            nuevo = new NodoSintactico("INSTRUCCION", contadorNodos++);
                            padreSintactico.addHijo(nuevo);
                            recorrer(nuevo, padreIrony.ChildNodes[0]);
                        }
                        else
                        {
                            recorrer(padreSintactico, padreIrony.ChildNodes[0]);
                        }
                    }
                    break;
                case "L_DECLARACIONES":
                    if (padreIrony.ChildNodes.Count > 1)
                    {
                        if (!padreSintactico.getNombre().Equals("DECLARACION"))
                        {
                            //DATO PRIMERO
                            nuevo = new NodoSintactico("DECLARACION", contadorNodos++);
                            padreSintactico.addHijo(nuevo);
                            recorrer(nuevo, padreIrony.ChildNodes[0]);
                            recorrer(nuevo, padreIrony.ChildNodes[1]);
                        }
                        else
                        {
                            //DATO INTERMEDIO
                            recorrer(padreSintactico, padreIrony.ChildNodes[0]);
                            recorrer(padreSintactico, padreIrony.ChildNodes[1]);
                        }
                    }
                    else
                    {
                        //DATO ULTIMO
                        if (!padreSintactico.getNombre().Equals("DECLARACION"))
                        {
                            nuevo = new NodoSintactico("DECLARACION", contadorNodos++);
                            padreSintactico.addHijo(nuevo);
                            recorrer(nuevo, padreIrony.ChildNodes[0]);
                        }
                        else
                        {
                            recorrer(padreSintactico, padreIrony.ChildNodes[0]);
                        }
                    }
                    break;
                case "L_PARAMETROS":
                    if (padreIrony.ChildNodes.Count > 1)
                    {
                        if (!padreSintactico.getNombre().Equals("PARAMETROS"))
                        {
                            //DATO PRIMERO
                            nuevo = new NodoSintactico("PARAMETROS", contadorNodos++);
                            padreSintactico.addHijo(nuevo);
                            recorrer(nuevo, padreIrony.ChildNodes[0]);
                            recorrer(nuevo, padreIrony.ChildNodes[1]);
                        }
                        else
                        {
                            //DATO INTERMEDIO
                            recorrer(padreSintactico, padreIrony.ChildNodes[0]);
                            recorrer(padreSintactico, padreIrony.ChildNodes[1]);
                        }
                    }
                    else
                    {
                        //DATO ULTIMO
                        if (!padreSintactico.getNombre().Equals("PARAMETROS"))
                        {
                            nuevo = new NodoSintactico("PARAMETROS", contadorNodos++);
                            padreSintactico.addHijo(nuevo);
                            recorrer(nuevo, padreIrony.ChildNodes[0]);
                        }
                        else
                        {
                            recorrer(padreSintactico, padreIrony.ChildNodes[0]);
                        }
                    }
                    break;
                #endregion

                #region lineal, arreglos: dim1, dim2 y dim3
                case "LINEAL":
                    nuevo = new NodoSintactico("DIM0", contadorNodos++);
                    foreach (ParseTreeNode hijo in padreIrony.ChildNodes)
                    {
                        recorrer(nuevo, hijo);
                    }
                    padreSintactico.addHijo(nuevo);
                    break;
                case "ARREGLO":
                    recorrer(padreSintactico, padreIrony.ChildNodes[0]);
                    break;
                case "DIM1":
                    nuevo = new NodoSintactico("DIM1", contadorNodos++);
                    padreSintactico.addHijo(nuevo);
                    foreach (ParseTreeNode hijo in padreIrony.ChildNodes)
                    {
                        recorrer(nuevo, hijo);
                    }
                    break;
                case "DIM2":
                    nuevo = new NodoSintactico("DIM2", contadorNodos++);
                    padreSintactico.addHijo(nuevo);
                    foreach (ParseTreeNode hijo in padreIrony.ChildNodes)
                    {
                        recorrer(nuevo, hijo);
                    }
                    break;
                case "DIM3":
                    nuevo = new NodoSintactico("DIM3", contadorNodos++);
                    padreSintactico.addHijo(nuevo);
                    foreach (ParseTreeNode hijo in padreIrony.ChildNodes)
                    {
                        recorrer(nuevo, hijo);
                    }
                    break;
                case "I_DIM":
                    if (padreIrony.ChildNodes.Count > 1)
                    {
                        if (!padreSintactico.getNombre().Equals("CONJUNTO_D1"))
                        {
                            //DATO PRIMERO
                            nuevo = new NodoSintactico("CONJUNTO_D1", contadorNodos++);
                            padreSintactico.addHijo(nuevo);
                            recorrer(nuevo, padreIrony.ChildNodes[0]);
                            recorrer(nuevo, padreIrony.ChildNodes[1]);
                        }
                        else
                        {
                            //DATO INTERMEDIO
                            recorrer(padreSintactico, padreIrony.ChildNodes[0]);
                            recorrer(padreSintactico, padreIrony.ChildNodes[1]);
                        }
                    }
                    else
                    {
                        //DATO ULTIMO
                        if (!padreSintactico.getNombre().Equals("CONJUNTO_D1"))
                        {
                            nuevo = new NodoSintactico("CONJUNTO_D1", contadorNodos++);
                            padreSintactico.addHijo(nuevo);
                            recorrer(nuevo, padreIrony.ChildNodes[0]);
                        }
                        else
                        {
                            recorrer(padreSintactico, padreIrony.ChildNodes[0]);
                        }
                    }
                    break;
                case "E_DIM":
                    if (padreIrony.ChildNodes.Count > 1)
                    {
                        if (!padreSintactico.getNombre().Equals("CONJUNTO_D2"))
                        {
                            //DATO PRIMERO
                            nuevo = new NodoSintactico("CONJUNTO_D2", contadorNodos++);
                            padreSintactico.addHijo(nuevo);
                            recorrer(nuevo, padreIrony.ChildNodes[0]);
                            recorrer(nuevo, padreIrony.ChildNodes[1]);
                        }
                        else
                        {
                            //DATO INTERMEDIO
                            recorrer(padreSintactico, padreIrony.ChildNodes[0]);
                            recorrer(padreSintactico, padreIrony.ChildNodes[1]);
                        }
                    }
                    else
                    {
                        //DATO ULTIMO
                        if (!padreSintactico.getNombre().Equals("CONJUNTO_D2"))
                        {
                            nuevo = new NodoSintactico("CONJUNTO_D2", contadorNodos++);
                            padreSintactico.addHijo(nuevo);
                            recorrer(nuevo, padreIrony.ChildNodes[0]);
                        }
                        else
                        {
                            recorrer(padreSintactico, padreIrony.ChildNodes[0]);
                        }
                    }
                    break;
                case "E_DIM1":
                    if (padreIrony.ChildNodes.Count > 1)
                    {
                        if (!padreSintactico.getNombre().Equals("CONJUNTO_D3"))
                        {
                            //DATO PRIMERO
                            nuevo = new NodoSintactico("CONJUNTO_D3", contadorNodos++);
                            padreSintactico.addHijo(nuevo);
                            recorrer(nuevo, padreIrony.ChildNodes[0]);
                            recorrer(nuevo, padreIrony.ChildNodes[1]);
                        }
                        else
                        {
                            //DATO INTERMEDIO
                            recorrer(padreSintactico, padreIrony.ChildNodes[0]);
                            recorrer(padreSintactico, padreIrony.ChildNodes[1]);
                        }
                    }
                    else
                    {
                        //DATO ULTIMO
                        if (!padreSintactico.getNombre().Equals("CONJUNTO_D3"))
                        {
                            nuevo = new NodoSintactico("CONJUNTO_D3", contadorNodos++);
                            padreSintactico.addHijo(nuevo);
                            recorrer(nuevo, padreIrony.ChildNodes[0]);
                        }
                        else
                        {
                            recorrer(padreSintactico, padreIrony.ChildNodes[0]);
                        }
                    }
                    break;
                #endregion

                #region Expresion, dato, EOTRO
                case "EXPRESION":
                    if (padreIrony.ChildNodes.Count == 3)
                    {
                        nuevo = buscarOperacion(padreSintactico, padreIrony.ChildNodes[1]);
                        recorrer(nuevo, padreIrony.ChildNodes[0]);
                        recorrer(nuevo, padreIrony.ChildNodes[2]);
                        padreSintactico.addHijo(nuevo);
                    }
                    else if (padreIrony.ChildNodes.Count == 2)
                    {
                        if (padreIrony.ChildNodes[1].ToString().Equals("EXPRESION"))
                        {
                            if (padreIrony.ChildNodes[0].ToString().Equals("- (Key symbol)"))
                            {
                                nuevo = new NodoSintactico("NEGATIVO", contadorNodos++);
                                recorrer(nuevo, padreIrony.ChildNodes[1]);
                                padreSintactico.addHijo(nuevo);
                            }
                            else
                            {
                                nuevo = new NodoSintactico("NOT", contadorNodos++);
                                recorrer(nuevo, padreIrony.ChildNodes[1]);
                                padreSintactico.addHijo(nuevo);
                            }
                        }
                        else
                        {
                            nuevo = buscarOperacion(padreSintactico, padreIrony.ChildNodes[1]);
                            recorrer(nuevo, padreIrony.ChildNodes[0]);
                            padreSintactico.addHijo(nuevo);
                        }
                    }
                    else
                    {
                        foreach (ParseTreeNode hijo in padreIrony.ChildNodes)
                        {
                            recorrer(padreSintactico, hijo);
                        }
                    }
                    break;
                case "DATO":
                    if (padreIrony.ChildNodes.Count > 1)
                    {
                        nuevo = new NodoSintactico("CAMPOCLASE", contadorNodos++);
                        padreSintactico.addHijo(nuevo);
                        recorrer(nuevo, padreIrony.ChildNodes[0]);
                        recorrer(nuevo, padreIrony.ChildNodes[1]);
                    }
                    else
                    {
                        recorrer(padreSintactico, padreIrony.ChildNodes[0]);
                    }
                    break;
                case "INSTRUCCION":
                    foreach (ParseTreeNode hijo in padreIrony.ChildNodes)
                    {
                        recorrer(padreSintactico, hijo);
                    }
                    break;
                case "EOTRO":
                    nuevo = new NodoSintactico("EOTRO", contadorNodos++);
                    padreSintactico.addHijo(nuevo);
                    foreach (ParseTreeNode hijo in padreIrony.ChildNodes)
                    {
                        recorrer(nuevo, hijo);
                    }
                    break;
                #endregion

                #region log, alert graph
                case "LOG":
                    nuevo = new NodoSintactico("LOG", contadorNodos++);
                    padreSintactico.addHijo(nuevo);
                    foreach (ParseTreeNode hijo in padreIrony.ChildNodes)
                    {
                        recorrer(nuevo, hijo);
                    }
                    break;
                case "ALERT":
                    nuevo = new NodoSintactico("ALERT", contadorNodos++);
                    padreSintactico.addHijo(nuevo);
                    foreach (ParseTreeNode hijo in padreIrony.ChildNodes)
                    {
                        recorrer(nuevo, hijo);
                    }
                    break;
                case "GRAPH":
                    nuevo = new NodoSintactico("GRAPH", contadorNodos++);
                    padreSintactico.addHijo(nuevo);
                    foreach (ParseTreeNode hijo in padreIrony.ChildNodes)
                    {
                        recorrer(nuevo, hijo);
                    }
                    break;
                #endregion

                #region if, else if, else
                case "IF":
                    nuevo = new NodoSintactico("IF", contadorNodos++);
                    padreSintactico.addHijo(nuevo);
                    foreach (ParseTreeNode hijo in padreIrony.ChildNodes)
                    {
                        recorrer(nuevo, hijo);
                    }
                    break;
                case "ELSEIF":
                    nuevo = new NodoSintactico("ELSE", contadorNodos++);
                    padreSintactico.addHijo(nuevo);
                    foreach (ParseTreeNode hijo in padreIrony.ChildNodes)
                    {
                        recorrer(nuevo, hijo);
                    }
                    break;
                case "ELSE":
                    nuevo = new NodoSintactico("ELSE", contadorNodos++);
                    padreSintactico.addHijo(nuevo);
                    foreach (ParseTreeNode hijo in padreIrony.ChildNodes)
                    {
                        recorrer(nuevo, hijo);
                    }
                    break;
                #endregion

                #region switch, case, default
                case "SWITCH":
                    nuevo = new NodoSintactico("SWITCH", contadorNodos++);
                    padreSintactico.addHijo(nuevo);
                    foreach (ParseTreeNode hijo in padreIrony.ChildNodes)
                    {
                        recorrer(nuevo, hijo);
                    }
                    break;
                case "CASE":
                    nuevo = new NodoSintactico("CASE", contadorNodos++);
                    foreach (ParseTreeNode hijo in padreIrony.ChildNodes)
                    {
                        if (!hijo.ToString().Equals("DEFAULT"))
                        {
                            recorrer(nuevo, hijo);
                        }
                        else
                        {
                            recorrer(padreSintactico, hijo);
                        }
                    }
                    padreSintactico.addHijo(nuevo);
                    break;
                case "DEFAULT":
                    nuevo = new NodoSintactico("DEFAULT", contadorNodos++);
                    padreSintactico.addHijo(nuevo);
                    foreach (ParseTreeNode hijo in padreIrony.ChildNodes)
                    {
                        recorrer(nuevo, hijo);
                    }
                    break;
                case "L_CASE":
                    if (padreIrony.ChildNodes.Count > 1)
                    {
                        if (!padreSintactico.getNombre().Equals("L_CASE"))
                        {
                            //DATO PRIMERO
                            nuevo = new NodoSintactico("L_CASE", contadorNodos++);
                            padreSintactico.addHijo(nuevo);
                            recorrer(nuevo, padreIrony.ChildNodes[0]);
                            recorrer(nuevo, padreIrony.ChildNodes[1]);
                        }
                        else
                        {
                            //DATO INTERMEDIO
                            recorrer(padreSintactico, padreIrony.ChildNodes[0]);
                            recorrer(padreSintactico, padreIrony.ChildNodes[1]);
                        }
                    }
                    else
                    {
                        //DATO ULTIMO
                        if (!padreSintactico.getNombre().Equals("L_CASE"))
                        {
                            nuevo = new NodoSintactico("L_CASE", contadorNodos++);
                            padreSintactico.addHijo(nuevo);
                            recorrer(nuevo, padreIrony.ChildNodes[0]);
                        }
                        else
                        {
                            recorrer(padreSintactico, padreIrony.ChildNodes[0]);
                        }
                    }
                    break;
                #endregion

                #region while, do while, for
                case "WHILE":
                    nuevo = new NodoSintactico("WHILE", contadorNodos++);
                    padreSintactico.addHijo(nuevo);
                    foreach (ParseTreeNode hijo in padreIrony.ChildNodes)
                    {
                        recorrer(nuevo, hijo);
                    }
                    break;
                case "FOR":
                    nuevo = new NodoSintactico("FOR", contadorNodos++);
                    padreSintactico.addHijo(nuevo);
                    foreach (ParseTreeNode hijo in padreIrony.ChildNodes)
                    {
                        recorrer(nuevo, hijo);
                    }
                    break;
                case "OPCIONESFOR":
                    nuevo = new NodoSintactico("OPCIONESFOR", contadorNodos++);
                    padreSintactico.addHijo(nuevo);
                    foreach (ParseTreeNode hijo in padreIrony.ChildNodes)
                    {
                        recorrer(nuevo, hijo);
                    }
                    break;
                case "CONTROLFOR":
                    foreach (ParseTreeNode hijo in padreIrony.ChildNodes)
                    {
                        recorrer(padreSintactico, hijo);
                    }
                    break;
                case "CONDICIONFOR":
                    foreach (ParseTreeNode hijo in padreIrony.ChildNodes)
                    {
                        recorrer(padreSintactico, hijo);
                    }
                    break;
                case "UPDATEFOR":
                    foreach (ParseTreeNode hijo in padreIrony.ChildNodes)
                    {
                        recorrer(padreSintactico, hijo);
                    }
                    break;
                case "DOWHILE":
                    nuevo = new NodoSintactico("DOWHILE", contadorNodos++);
                    padreSintactico.addHijo(nuevo);
                    foreach (ParseTreeNode hijo in padreIrony.ChildNodes)
                    {
                        recorrer(nuevo, hijo);
                    }
                    break;
                #endregion

                #region metodo, funcion, clase, return
                case "METODO":
                    nuevo = new NodoSintactico("METODO", contadorNodos++);
                    padreSintactico.addHijo(nuevo);
                    foreach (ParseTreeNode hijo in padreIrony.ChildNodes)
                    {
                        recorrer(nuevo, hijo);
                    }
                    break;
                case "FUNCION":
                    nuevo = new NodoSintactico("FUNCION", contadorNodos++);
                    padreSintactico.addHijo(nuevo);
                    foreach (ParseTreeNode hijo in padreIrony.ChildNodes)
                    {
                        recorrer(nuevo, hijo);
                    }
                    break;
                case "CLASE":
                    nuevo = new NodoSintactico("CLASE", contadorNodos++);
                    padreSintactico.addHijo(nuevo);
                    foreach (ParseTreeNode hijo in padreIrony.ChildNodes)
                    {
                        recorrer(nuevo, hijo);
                    }
                    break;
                case "PARAMETROS":
                    if (padreIrony.ChildNodes.Count > 1)
                    {
                        if (!padreSintactico.getNombre().Equals("PARAMETROS"))
                        {
                            //DATO PRIMERO
                            nuevo = new NodoSintactico("PARAMETROS", contadorNodos++);
                            padreSintactico.addHijo(nuevo);
                            recorrer(nuevo, padreIrony.ChildNodes[0]);
                            recorrer(nuevo, padreIrony.ChildNodes[1]);
                        }
                        else
                        {
                            //DATO INTERMEDIO
                            recorrer(padreSintactico, padreIrony.ChildNodes[0]);
                            recorrer(padreSintactico, padreIrony.ChildNodes[1]);
                        }
                    }
                    else
                    {
                        //DATO ULTIMO
                        if (!padreSintactico.getNombre().Equals("PARAMETROS"))
                        {
                            nuevo = new NodoSintactico("PARAMETROS", contadorNodos++);
                            padreSintactico.addHijo(nuevo);
                            recorrer(nuevo, padreIrony.ChildNodes[0]);
                        }
                        else
                        {
                            recorrer(padreSintactico, padreIrony.ChildNodes[0]);
                        }
                    }
                    break;
                case "RETURN":
                    nuevo = new NodoSintactico("RETURN", contadorNodos++);
                    padreSintactico.addHijo(nuevo);
                    foreach (ParseTreeNode hijo in padreIrony.ChildNodes)
                    {
                        recorrer(nuevo, hijo);
                    }
                    break;
                #endregion

                #region reasignacion
                case "REASIGNACION":
                    nuevo = new NodoSintactico("REASIGNACION", contadorNodos++);
                    padreSintactico.addHijo(nuevo);
                    foreach (ParseTreeNode hijo in padreIrony.ChildNodes)
                    {
                        recorrer(nuevo, hijo);
                    }
                    break;
                #endregion

                #region default: entero, doble, cadena, caracter, boleano, id, operadores, break, continue
                default:
                    if (padreIrony.Token != null)
                    {
                        string token = padreIrony.Token.ToString();
                        if (token.EndsWith(" (entero)"))
                        {
                            nuevo = new NodoSintactico("ENTERO", contadorNodos++);
                            valor = padreIrony.Token.ToString().Replace("\"", "");
                            valor = valor.Replace(" (entero)", "");
                            nuevo.setValor(valor);
                            padreSintactico.addHijo(nuevo);
                        }
                        else if (token.EndsWith(" (doble)"))
                        {
                            nuevo = new NodoSintactico("DOBLE", contadorNodos++);
                            valor = padreIrony.Token.ToString().Replace("\"", "");
                            valor = valor.Replace(" (doble)", "");
                            nuevo.setValor(valor);
                            padreSintactico.addHijo(nuevo);
                        }
                        else if (token.EndsWith(" (cadena)"))
                        {
                            nuevo = new NodoSintactico("CADENA", contadorNodos++);
                            valor = padreIrony.Token.ToString();
                            valor = valor.Replace(" (cadena)", "");
                            valor = valor.Substring(0, valor.Length);
                            valor = valor.Replace("\\\"", "\"");
                            nuevo.setValor(valor);
                            padreSintactico.addHijo(nuevo);
                        }
                        else if (token.EndsWith(" (caracter)"))
                        {
                            nuevo = new NodoSintactico("CARACTER", contadorNodos++);
                            valor = padreIrony.Token.ToString();
                            valor = valor.Replace(" (cadena)", "");
                            valor = valor.Substring(0, valor.Length);
                            valor = valor.Replace("\\\"", "\"");
                            nuevo.setValor(valor);
                            padreSintactico.addHijo(nuevo);
                        }
                        else if (token.EndsWith(" (boleano)"))
                        {
                            nuevo = new NodoSintactico("BOLEANO", contadorNodos++);
                            valor = padreIrony.Token.ToString().Replace("\"", "");
                            valor = valor.Replace(" (boleano)", "");
                            nuevo.setValor(valor);
                            padreSintactico.addHijo(nuevo);
                        }
                        else if (token.EndsWith(" (id)"))
                        {
                            nuevo = new NodoSintactico("ID", contadorNodos++);
                            valor = padreIrony.Token.ToString().Replace("\"", "");
                            valor = valor.Replace(" (id)", "");
                            nuevo.setValor(valor);
                            padreSintactico.addHijo(nuevo);
                        }
                        if (token.Equals("* (Key symbol)"))
                        {
                            nuevo = new NodoSintactico("MULTIPLICACION", contadorNodos++);
                            valor = "*";
                            nuevo.setValor(valor);
                            padreSintactico.addHijo(nuevo);
                        }
                        else if (token.Equals("+ (Key symbol)"))
                        {
                            nuevo = new NodoSintactico("SUMA", contadorNodos++);
                            valor = "+";
                            nuevo.setValor(valor);
                            padreSintactico.addHijo(nuevo);
                        }
                        else if (token.Equals("/ (Key symbol)"))
                        {
                            nuevo = new NodoSintactico("DIVISION", contadorNodos++);
                            valor = "/";
                            nuevo.setValor(valor);
                            padreSintactico.addHijo(nuevo);
                        }
                        else if (token.Equals("- (Key symbol)"))
                        {
                            nuevo = new NodoSintactico("RESTA", contadorNodos++);
                            valor = "-";
                            nuevo.setValor(valor);
                            padreSintactico.addHijo(nuevo);
                        }
                        else if (token.Equals("pow (Keyword)"))
                        {
                            nuevo = new NodoSintactico("POTENCIA", contadorNodos++);
                            valor = "pow";
                            nuevo.setValor(valor);
                            padreSintactico.addHijo(nuevo);
                        }
                        else if (token.Equals("null (Keyword)"))
                        {
                            nuevo = new NodoSintactico("NULO", contadorNodos++);
                            valor = "pow";
                            nuevo.setValor(valor);
                            padreSintactico.addHijo(nuevo);
                        }
                        else if (token.Equals("&& (Key symbol)"))
                        {
                            nuevo = new NodoSintactico("AND", contadorNodos++);
                            valor = "&&";
                            nuevo.setValor(valor);
                            padreSintactico.addHijo(nuevo);
                        }
                        else if (token.Equals("|| (Key symbol)"))
                        {
                            nuevo = new NodoSintactico("OR", contadorNodos++);
                            valor = "||";
                            nuevo.setValor(valor);
                            padreSintactico.addHijo(nuevo);
                        }
                        else if (token.Equals("^ (Key symbol)"))
                        {
                            nuevo = new NodoSintactico("XOR", contadorNodos++);
                            valor = "^";
                            nuevo.setValor(valor);
                            padreSintactico.addHijo(nuevo);
                        }
                        else if (token.Equals("! (Key symbol)"))
                        {
                            nuevo = new NodoSintactico("NOT", contadorNodos++);
                            valor = "!";
                            nuevo.setValor(valor);
                            padreSintactico.addHijo(nuevo);
                        }
                        else if (token.Equals("> (Key symbol)"))
                        {
                            nuevo = new NodoSintactico("MAYOR", contadorNodos++);
                            valor = ">";
                            nuevo.setValor(valor);
                            padreSintactico.addHijo(nuevo);
                        }
                        else if (token.Equals("< (Key symbol)"))
                        {
                            nuevo = new NodoSintactico("MENOR", contadorNodos++);
                            valor = "<";
                            nuevo.setValor(valor);
                            padreSintactico.addHijo(nuevo);
                        }
                        else if (token.Equals(">= (Key symbol)"))
                        {
                            nuevo = new NodoSintactico("MAYORIGUAL", contadorNodos++);
                            valor = ">=";
                            nuevo.setValor(valor);
                            padreSintactico.addHijo(nuevo);
                        }
                        else if (token.Equals("<= (Key symbol)"))
                        {
                            nuevo = new NodoSintactico("MENORIGUAL", contadorNodos++);
                            valor = ">=";
                            nuevo.setValor(valor);
                            padreSintactico.addHijo(nuevo);
                        }
                        else if (token.Equals("== (Key symbol)"))
                        {
                            nuevo = new NodoSintactico("IGUALIGUAL", contadorNodos++);
                            valor = "==";
                            nuevo.setValor(valor);
                            padreSintactico.addHijo(nuevo);
                        }
                        else if (token.Equals("<> (Key symbol)"))
                        {
                            nuevo = new NodoSintactico("DESIGUAL", contadorNodos++);
                            valor = "<>";
                            nuevo.setValor(valor);
                            padreSintactico.addHijo(nuevo);
                        }
                        else if (token.Equals("++ (Key symbol)"))
                        {
                            nuevo = new NodoSintactico("INCREMENTO", contadorNodos++);
                            valor = "++";
                            nuevo.setValor(valor);
                            padreSintactico.addHijo(nuevo);
                        }
                        else if (token.Equals("-- (Key symbol)"))
                        {
                            nuevo = new NodoSintactico("DECREMENTO", contadorNodos++);
                            valor = "--";
                            nuevo.setValor(valor);
                            padreSintactico.addHijo(nuevo);
                        }
                        else if (token.Equals("break (Keyword)"))
                        {
                            nuevo = new NodoSintactico("BREAK", contadorNodos++);
                            valor = "break";
                            nuevo.setValor(valor);
                            padreSintactico.addHijo(nuevo);
                        }
                        else if (token.Equals("continue (Keyword)"))
                        {
                            nuevo = new NodoSintactico("CONTINUE", contadorNodos++);
                            valor = "continue";
                            nuevo.setValor(valor);
                            padreSintactico.addHijo(nuevo);
                        }
                        else if (token.Equals("new (Keyword)"))
                        {
                            nuevo = new NodoSintactico("NEW", contadorNodos++);
                            valor = "new";
                            nuevo.setValor(valor);
                            padreSintactico.addHijo(nuevo);
                        }
                    }
                    break;
                    #endregion
            }
        }

        private NodoSintactico buscarOperacion(NodoSintactico padreSintactico, ParseTreeNode padreIrony)
        {
            NodoSintactico nuevo;
            string valor;
            if (padreIrony.Token != null)
            {
                string token = padreIrony.Token.ToString();
                if (token.EndsWith(" (entero)"))
                {
                    nuevo = new NodoSintactico("ENTERO", contadorNodos++);
                    valor = padreIrony.Token.ToString().Replace("\"", "");
                    valor = valor.Replace(" (entero)", "");
                    nuevo.setValor(valor);
                    return nuevo;
                }
                else if (token.EndsWith(" (doble)"))
                {
                    nuevo = new NodoSintactico("DOBLE", contadorNodos++);
                    valor = padreIrony.Token.ToString().Replace("\"", "");
                    valor = valor.Replace(" (doble)", "");
                    nuevo.setValor(valor);
                    return nuevo;
                }
                else if (token.EndsWith(" (cadena)"))
                {
                    nuevo = new NodoSintactico("CADENA", contadorNodos++);
                    valor = padreIrony.Token.ToString();
                    valor = valor.Replace(" (cadena)", "");
                    valor = valor.Substring(0, valor.Length);
                    valor = valor.Replace("\\\"", "\"");
                    nuevo.setValor(valor);
                    return nuevo;
                }
                else if (token.EndsWith(" (caracter)"))
                {
                    nuevo = new NodoSintactico("CARACTER", contadorNodos++);
                    valor = padreIrony.Token.ToString().Replace("\"", "");
                    valor = valor.Replace(" (caracter)", "");
                    nuevo.setValor(valor);
                    return nuevo;
                }
                else if (token.EndsWith(" (boleano)"))
                {
                    nuevo = new NodoSintactico("BOLEANO", contadorNodos++);
                    valor = padreIrony.Token.ToString().Replace("\"", "");
                    valor = valor.Replace(" (boleano)", "");
                    nuevo.setValor(valor);
                    return nuevo;
                }
                else if (token.EndsWith(" (id)"))
                {
                    nuevo = new NodoSintactico("ID", contadorNodos++);
                    valor = padreIrony.Token.ToString().Replace("\"", "");
                    valor = valor.Replace(" (id)", "");
                    nuevo.setValor(valor);
                    return nuevo;
                }
                if (token.Equals("* (Key symbol)"))
                {
                    nuevo = new NodoSintactico("MULTIPLICACION", contadorNodos++);
                    valor = "*";
                    nuevo.setValor(valor);
                    return nuevo;
                }
                else if (token.Equals("+ (Key symbol)"))
                {
                    nuevo = new NodoSintactico("SUMA", contadorNodos++);
                    valor = "+";
                    nuevo.setValor(valor);
                    return nuevo;
                }
                else if (token.Equals("/ (Key symbol)"))
                {
                    nuevo = new NodoSintactico("DIVISION", contadorNodos++);
                    valor = "/";
                    nuevo.setValor(valor);
                    return nuevo;
                }
                else if (token.Equals("- (Key symbol)"))
                {
                    nuevo = new NodoSintactico("RESTA", contadorNodos++);
                    valor = "-";
                    nuevo.setValor(valor);
                    return nuevo;
                }
                else if (token.Equals("pow (Keyword)"))
                {
                    nuevo = new NodoSintactico("POTENCIA", contadorNodos++);
                    valor = "pow";
                    nuevo.setValor(valor);
                    return nuevo;
                }
                else if (token.Equals("&& (Key symbol)"))
                {
                    nuevo = new NodoSintactico("AND", contadorNodos++);
                    valor = "&&";
                    nuevo.setValor(valor);
                    return nuevo;
                }
                else if (token.Equals("|| (Key symbol)"))
                {
                    nuevo = new NodoSintactico("OR", contadorNodos++);
                    valor = "||";
                    nuevo.setValor(valor);
                    return nuevo;
                }
                else if (token.Equals("^ (Key symbol)"))
                {
                    nuevo = new NodoSintactico("XOR", contadorNodos++);
                    valor = "^";
                    nuevo.setValor(valor);
                    return nuevo;
                }
                else if (token.Equals("! (Key symbol)"))
                {
                    nuevo = new NodoSintactico("NOT", contadorNodos++);
                    valor = "!";
                    nuevo.setValor(valor);
                    return nuevo;
                }
                else if (token.Equals("> (Key symbol)"))
                {
                    nuevo = new NodoSintactico("MAYOR", contadorNodos++);
                    valor = ">";
                    nuevo.setValor(valor);
                    return nuevo;
                }
                else if (token.Equals("< (Key symbol)"))
                {
                    nuevo = new NodoSintactico("MENOR", contadorNodos++);
                    valor = "<";
                    nuevo.setValor(valor);
                    return nuevo;
                }
                else if (token.Equals(">= (Key symbol)"))
                {
                    nuevo = new NodoSintactico("MAYORIGUAL", contadorNodos++);
                    valor = ">=";
                    nuevo.setValor(valor);
                    return nuevo;
                }
                else if (token.Equals("<= (Key symbol)"))
                {
                    nuevo = new NodoSintactico("MENORIGUAL", contadorNodos++);
                    valor = ">=";
                    nuevo.setValor(valor);
                    return nuevo;
                }
                else if (token.Equals("== (Key symbol)"))
                {
                    nuevo = new NodoSintactico("IGUALIGUAL", contadorNodos++);
                    valor = "==";
                    nuevo.setValor(valor);
                    return nuevo;
                }
                else if (token.Equals("<> (Key symbol)"))
                {
                    nuevo = new NodoSintactico("DESIGUAL", contadorNodos++);
                    valor = "<>";
                    nuevo.setValor(valor);
                    return nuevo;
                }
                else if (token.Equals("++ (Key symbol)"))
                {
                    nuevo = new NodoSintactico("INCREMENTO", contadorNodos++);
                    valor = "++";
                    nuevo.setValor(valor);
                    return nuevo;
                }
                else if (token.Equals("-- (Key symbol)"))
                {
                    nuevo = new NodoSintactico("DECREMENTO", contadorNodos++);
                    valor = "--";
                    nuevo.setValor(valor);
                    return nuevo;
                }
                else if (token.Equals("break (Keyword)"))
                {
                    nuevo = new NodoSintactico("BREAK", contadorNodos++);
                    valor = "break";
                    nuevo.setValor(valor);
                    return nuevo;
                }
                else if (token.Equals("continue (Keyword)"))
                {
                    nuevo = new NodoSintactico("CONTINUE", contadorNodos++);
                    valor = "continue";
                    nuevo.setValor(valor);
                    return nuevo;
                }
                else if (token.Equals("new (Keyword)"))
                {
                    nuevo = new NodoSintactico("NEW", contadorNodos++);
                    valor = "new";
                    nuevo.setValor(valor);
                    return nuevo;
                }
            }
            return null;
        }

        private void aSTToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Sintactico.crearImagen(resultadoAnalisis.Root, null, 0);
            Sintactico.crearImagen(null, root, 1);
            Thread.Sleep(1000);
            Process.Start(Environment.CurrentDirectory + "/AST0.png");
            Process.Start(Environment.CurrentDirectory + "/AST1.png");
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
