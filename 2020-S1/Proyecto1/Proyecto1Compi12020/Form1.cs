using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Proyecto1Compi12020.Modelos;
using Proyecto1Compi12020.Analizador;
using Proyecto1Compi12020.Generadores;
using System.Text.RegularExpressions;

namespace Proyecto1Compi12020
{
    public partial class Form1 : Form
    {
        String path = "C:\\Users\\USER\\Desktop\\PruebasProyecto1\\";
        private List<String> archivos;
        private List<String> pathAutomatas;
        private List<String> pathTransiciones;
        private List<String> expresionRegular;
        private int posicionPath;
        Lista listaTokens;
        AnalizadorLexico analizadorLexico;

        public Form1()
        {
            InitializeComponent();
            openFileDialog1.InitialDirectory = path;
            saveFileDialog1.InitialDirectory = path;
            openFileDialog1.Filter = saveFileDialog1.Filter = "Expresiones Regulares | *.er";
            archivos = new List<String>();
            listaTokens = new Lista();
            analizadorLexico = new AnalizadorLexico();
            pathAutomatas = new List<string>();
            pathTransiciones = new List<string>();
            expresionRegular = new List<string>();
            posicionPath = 0;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void cerrarItem(object sender, EventArgs e)
        {
            ToolStripMenuItem item = sender as ToolStripMenuItem;
            currentFilesToolStripMenuItem.DropDownItems.Remove(item);
            tabControl1.TabPages.RemoveByKey(item.Name);

            //Adding the file name to the file List
            archivos.Remove(item.Name);
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Prevent "File already exist" message
            saveFileDialog1.OverwritePrompt = false;

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                String path = saveFileDialog1.FileName;

                if (!File.Exists(path))
                {
                    var nuevo = File.Create(path);
                    nuevo.Close();

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

                    //Adding the file name to the file List
                    archivos.Add(newTab.Name);

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
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog1.FileName = "";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                String path = openFileDialog1.FileName;

                String key = "text_" + Path.GetFileName(path);

                if (!archivos.Contains(path))
                {
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

                    //Adding the file name to the file List
                    archivos.Add(newTab.Name);

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
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (archivos.Count != 0)
            {
                String key = "text_" + tabControl1.SelectedTab.Text;

                //Fetching the RichTextBox on the TabPage
                Control[] control = tabControl1.SelectedTab.Controls.Find(key, true);
                RichTextBox text = (RichTextBox)control[0];
                File.WriteAllText(tabControl1.SelectedTab.Name, text.Text);
            }
        }

        private void lexicalErrorsToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void loadThompsonToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox1.Text = "";
            pathAutomatas = new List<string>();
            pathTransiciones = new List<string>();
            expresionRegular = new List<string>();
            string path = "";
            posicionPath = 0;
            if (archivos.Count != 0)
            {
                String key = "text_" + tabControl1.SelectedTab.Text;

                //Fetching the RichTextBox on the TabPage
                Control[] control = tabControl1.SelectedTab.Controls.Find(key, true);
                RichTextBox text = (RichTextBox)control[0];
                analizadorLexico = new AnalizadorLexico();
                MatchCollection col = Regex.Matches(text.Text, "\"[^\"]*\"");
                string enviar = text.Text;
                foreach(Match m in col)
                {
                    if (m.Value.Contains("\n"))
                    {
                        enviar = enviar.Replace(m.Value, m.Value.Replace("\n", "▀"));
                    }
                }
                analizadorLexico.AnalizarCodigo(enviar);
                GeneradorXML generadorXML = new GeneradorXML();
                if(analizadorLexico.listaTokens.Head != null && analizadorLexico.listaErrores.Head == null)
                {
                    //richTextBox1.Text = generadorXML.generadorTokens(analizadorLexico.listaTokens);

                    foreach(string[] eval in analizadorLexico.evaluaciones)
                    {
                        richTextBox1.Text += evaluarCadena(eval[0], eval[1]) + "\n";
                    }

                    foreach (String llave in analizadorLexico.arboles.Keys)
                    {
                        analizadorLexico.arboles.TryGetValue(llave, out ArbolExpresiones arbol);
                        arbol.crearImagenes(llave);
                        path = "C:\\Users\\USER\\Desktop\\PruebasProyecto1\\Automatas\\" + llave + ".png";
                        pathAutomatas.Add(path);
                        path = "C:\\Users\\USER\\Desktop\\PruebasProyecto1\\Transiciones\\" + llave + ".png";
                        pathTransiciones.Add(path);
                        expresionRegular.Add(llave);
                    }

                    if (pathAutomatas.Count > 0)
                    {
                        panel2.BackgroundImage = Image.FromFile(pathAutomatas[0]);
                        panel2.BackgroundImageLayout = ImageLayout.Stretch;
                        panel3.BackgroundImage = Image.FromFile(pathTransiciones[0]);
                        panel3.BackgroundImageLayout = ImageLayout.Stretch;
                        label2.Text = expresionRegular[0];
                    }
                }
                if(analizadorLexico.listaErrores.Head != null)
                {
                    generadorXML.generadorErrores(analizadorLexico.listaErrores);
                }
            }
        }

        private string evaluarCadena(string nombreExpresion, string cadena)
        {
            bool encontrada = false;
            ArbolExpresiones arbol = null;
            foreach (string key in analizadorLexico.arboles.Keys)
            {
                if (key.Equals(nombreExpresion))
                {
                    analizadorLexico.arboles.TryGetValue(key, out arbol);
                    encontrada = true;
                    break;
                }
            }

            if (!encontrada)
            {
                return "La expresion regular " + nombreExpresion + " no esta definida";
            }

            //encontrada representa el resultado de la evaluacion, false -> error; true -> valida
            encontrada = arbol.evaluarCadena(cadena, 0, analizadorLexico.conjuntos);

            cadena = cadena.Replace("▀", "\n");
            if (!encontrada)
            {
                return "La cadena \"" + cadena + "\" es invalida para la expresion regular " + nombreExpresion;
            }
            return "La cadena \"" + cadena + "\" es valida para la expresion regular " + nombreExpresion;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //Siguiente
            if (pathAutomatas.Count > 0)
            {
                posicionPath++;
                if (posicionPath == pathAutomatas.Count)
                {
                    posicionPath = 0;
                }
                panel2.BackgroundImage = Image.FromFile(pathAutomatas[posicionPath]);
                panel2.BackgroundImageLayout = ImageLayout.Stretch;
                panel3.BackgroundImage = Image.FromFile(pathTransiciones[posicionPath]);
                panel3.BackgroundImageLayout = ImageLayout.Stretch;
                label2.Text = expresionRegular[posicionPath];
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //Anterior
            if (pathAutomatas.Count > 0)
            {
                posicionPath--;
                if (posicionPath < 0)
                {
                    posicionPath = pathAutomatas.Count - 1;
                }
                panel2.BackgroundImage = Image.FromFile(pathAutomatas[posicionPath]);
                panel2.BackgroundImageLayout = ImageLayout.Stretch;
                panel3.BackgroundImage = Image.FromFile(pathTransiciones[posicionPath]);
                panel3.BackgroundImageLayout = ImageLayout.Stretch;
                label2.Text = expresionRegular[posicionPath];
            }
        }

        private void saveTokensToolStripMenuItem_Click(object sender, EventArgs e)
        {
            analizadorLexico.generarXMLTokens();
        }

        private void saveErrorsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            analizadorLexico.generarXMLErrores();
        }
    }
}
