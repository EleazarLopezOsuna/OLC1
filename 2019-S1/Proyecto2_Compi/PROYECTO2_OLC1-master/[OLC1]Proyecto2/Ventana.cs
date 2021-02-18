using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Irony.Parsing;
using Microsoft.VisualBasic;

namespace _OLC1_Proyecto2
{
    public partial class Ventana : Form

    {

       

        SaveFileDialog Info = new SaveFileDialog();
        Boolean textoguardado = false;
        Boolean correcto = false;
        ParseTree raizcorrecta;
        ParseTree raizincorrecta;
        RichTextBox selected;
        private TokenList _tokens = new TokenList(); // llevar
        public Ventana()
        {

            InitializeComponent();


            selected = txtArchivo1;
        }



        private void compilarToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void erroresToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void BtnAbrir_Click(object sender, EventArgs e)
        {

            Dialogo.Filter = "Archivo|*.txt";
            Dialogo.FileName = "Seleccione un archivo de texto";
            Dialogo.Title = "Explorador de Archivos";
            Dialogo.InitialDirectory = "c:/";
            if (Dialogo.ShowDialog() == DialogResult.OK)
            {

                System.IO.StreamReader str = new System.IO.StreamReader(Dialogo.FileName, Encoding.Default);

                String Contenido = str.ReadToEnd();
                str.Close();
                getTXT().Text = Contenido;
                Info.FileName = Dialogo.FileName;
                textoguardado = true;
            }
        }

        private void BtnGuardar_Click(object sender, EventArgs e)
        {
            if (textoguardado == false)
            {
                Info.Filter = "Archivo|*.txt";
                Info.Title = "Seleccione Destino";
                Info.FileName = "Sin titulo";
                var resul = Info.ShowDialog();
                if (resul == DialogResult.OK)
                {
                    StreamWriter escribir = new StreamWriter(Info.FileName);
                    foreach (object line in getTXT().Lines)
                    {
                        escribir.WriteLine(line);

                    }
                    escribir.Close();
                }
                textoguardado = true;
            }
            else
            {
                SaveFileDialog info2 = new SaveFileDialog();
                StreamWriter escribir2 = new StreamWriter(Info.FileName);
                foreach (object line in getTXT().Lines)
                {
                    escribir2.WriteLine(line);

                }
                escribir2.Close();

            }
        }

        private void BtnGuardarComo_Click(object sender, EventArgs e)
        {
            Info.Filter = "Archivo|*.txt";
            Info.Title = "Seleccione Destino";
            Info.FileName = "Sin titulo";
            var resul = Info.ShowDialog();
            if (resul == DialogResult.OK)
            {
                StreamWriter escribir = new StreamWriter(Info.FileName);
                foreach (object line in getTXT().Lines)
                {
                    escribir.WriteLine(line);

                }
                escribir.Close();
            }
            textoguardado = true;
        }

        public RichTextBox getTXT()
        {
            return selected;
        }

        public void setTXT(RichTextBox txt)
        {
            selected = txt;
        }



        private void txtArchivo2_MouseClick(object sender, MouseEventArgs e)
        {
            setTXT(txtArchivo2);

        }

        private void txtArchivo1_MouseClick(object sender, MouseEventArgs e)
        {
            setTXT(txtArchivo1);
        }

        private void txtArchivo3_MouseClick(object sender, MouseEventArgs e)
        {
            setTXT(txtArchivo3);
        }

        private void Ventana_Load(object sender, EventArgs e)
        {
            this.AutoScaleMode = AutoScaleMode.Dpi;
        }

        private void BtnCrear_Click(object sender, EventArgs e)
        {
            string nombre = "";

            Object dialgo = Interaction.InputBox("Agregar Nombre a la Nueva Pestaña", "", nombre, -1, -1);
            String n = dialgo.ToString();
            TabPage paginanueva = new TabPage(n);
            RichTextBox nuevo = new RichTextBox();
            nuevo.Width = 705;
            nuevo.Height = 302;
            nuevo.BorderStyle = BorderStyle.None;
            nuevo.Font = new Font("Consolas", 10);

            paginanueva.Controls.Add(nuevo);
            tabControl1.TabPages.Add(paginanueva);


        }

        private void iniciarCompilacionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            reporteErroresToolStripMenuItem.Enabled = true;
            generarASTToolStripMenuItem.Enabled = true;
            reporteTokensToolStripMenuItem.Enabled = true;

            correcto = false;
            Gramatica grammar = new Gramatica();
            LanguageData lenguaje = new LanguageData(grammar);
            Parser p = new Parser(lenguaje);
            ParseTree arbol = p.Parse(getTXT().Text);
            if (arbol.Root != null)
            {
                correcto = true;
                Console.WriteLine("Correcto");
                Console.WriteLine("Errores"+arbol.ParserMessages.Count);
                //Console.WriteLine(arbol.ParserMessages.ElementAt(0).Message);
                raizcorrecta = arbol;
                Recorrido.CrearTablaTokens(arbol.Root);
                generarASTToolStripMenuItem.BackColor = System.Drawing.ColorTranslator.FromHtml("#36bb4c");
                reporteTokensToolStripMenuItem.BackColor = System.Drawing.ColorTranslator.FromHtml("#36bb4c");
                reporteErroresToolStripMenuItem.BackColor = System.Drawing.ColorTranslator.FromHtml("#ca2c22");
                reporteErroresToolStripMenuItem.Enabled = false;
                MessageBox.Show("Gramatica Compilada Con Exito", "Felicidades!");



                //Interprete i = new Interprete(arbol.Root.ChildNodes[0], arbol.Root.ChildNodes[0]);
            }
            else
            {
                correcto = false;
                raizincorrecta = arbol;
                Console.WriteLine("Error en el archivo de entrada");
                generarASTToolStripMenuItem.BackColor = System.Drawing.ColorTranslator.FromHtml("#ca2c22");
                reporteTokensToolStripMenuItem.BackColor = System.Drawing.ColorTranslator.FromHtml("#ca2c22");
                reporteErroresToolStripMenuItem.BackColor = System.Drawing.ColorTranslator.FromHtml("#36bb4c");
                generarASTToolStripMenuItem.Enabled = false;
                reporteTokensToolStripMenuItem.Enabled = false;
                MessageBox.Show("Gramatica Con Errores Revise Reportes", "Advertencia!");

            }
        }

        private void generarASTToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (correcto == true)
            {
                GraphvizGenerator.ConstruirArbol(raizcorrecta.Root);
                GraphvizGenerator.GraficarArbol("AST.txt", "C:\\Users\\JavierG\\Pictures");
                VisorAST img = new VisorAST();
                img.Show();
            }
            else
            {
                MessageBox.Show("Compilacion Incorrecta", "Advertencia!");
            }
        }

        private void reporteErroresToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (correcto == false)
            {
                GenerateHTMLMistake();
            }
            else
            {
                MessageBox.Show("Compilacion Correcta", "Mensaje!");
            }
        
        }

        public void GenerateHTMLMistake()
        {
            StreamWriter write = new StreamWriter("Errores.html");
            write.Write(BODYHTML());
            write.Close();
            Process.Start("Errores.html");
        }
        public void GenerateHTMLTokens()
        {
            StreamWriter write = new StreamWriter("Tokens.html");
            write.Write(BODYHTML2());
            write.Close();
            Process.Start("Tokens.html");
        }
        public String BODYHTML2()
        {
            String info = " <html> \n" +
      "<head><title>Tabla Tokens</title></head> \n" +
      "<style style=\"text/css\"> \n" +
                             ".hoverTable{ \n" +
      "width:100%; \n" +
      "border-collapse:collapse;} \n" +
      ".hoverTable td{ \n" +
      "padding:7px; border:#e1eaf6 1px solid;}" +
      "/* Define the default color for all the table rows */" +
      ".hoverTable tr{" +
      "background: #b8d1f3;}" +
      "/* Define the hover highlight color for the table row */" +
      ".hoverTable tr:hover {" +
      "background-color: #1d5fb7;}" +
      "</style>" +
      "<body>" +
      "<h1>Tabla Tokens</h1>" +
      "<table class=\"hoverTable\">" +
                               "<tr>" +
      "<th>Tipo</th>" +
      "<th>Lexema</th>" +
      "<th>Fila</th>" +
      "<th>Columna</th>" +

      "</tr>" + CuerpoTokens(raizcorrecta) + "</table>" +
      "</body>" +
      "</html>";
            return info;

        }


        public String BODYHTML()
        {
            String info = " <html> \n" +
      "<head><title>Tabla Errores Lexicos</title></head> \n" +
      "<style style=\"text/css\"> \n" +
                             ".hoverTable{ \n" +
      "width:100%; \n" +
      "border-collapse:collapse;} \n" +
      ".hoverTable td{ \n" +
      "padding:7px; border:#e1eaf6 1px solid;}" +
      "/* Define the default color for all the table rows */" +
      ".hoverTable tr{" +
      "background: #b8d1f3;}" +
      "/* Define the hover highlight color for the table row */" +
      ".hoverTable tr:hover {" +
      "background-color: #1d5fb7;}" +
      "</style>" +
      "<body>" +
      "<h1>Errores Lexicos</h1>" +
      "<table class=\"hoverTable\">" +
                               "<tr>" +
      "<th>Descripcion</th>" +
      "<th>Tipo</th>" +
      "<th>Fila</th>" +
      "<th>Columna</th>" +
      
      "</tr>" + CuerpoError(raizincorrecta) + "</table>" +
      "</body>" +
      "</html>";
            return info;

        }

        public String CuerpoError(ParseTree root)
        {
            String cuerpo = "";
            for (int i = 0; i < root.ParserMessages.Count; i++)
            {
                cuerpo += "<tr>" + "\n" +
                "<td>" + root.ParserMessages.ElementAt(i).Level.ToString() + "</td>" +
                "<td>" + root.ParserMessages.ElementAt(i).Message + "</td>" +
                "<td>" + root.ParserMessages.ElementAt(i).Location.Line + "</td>" +
                "<td>" + root.ParserMessages.ElementAt(i).Location.Column + "</td>" + "</tr>" + "\n";
            }
            return cuerpo;
        }

        public String CuerpoTokens(ParseTree root)
        {
            String body = "";
            foreach (Token tk in root.Tokens)
            {
                if (tk == root.Tokens.Last())
                {

                }
                else
                {
                    String[] temp = (tk.ToString()).Split(new char[0], 2);
                    body += "<tr>" + "\n" +
                        "<td>" + temp[1] + "</td>" +
                        "<td>" + tk.Value.ToString() + "</td>" +
                        "<td>" + tk.Location.Line + "</td>" +
                        "<td>" + tk.Location.Column + "</td>" +
                        "</tr>" + "\n";
                }
            }
            return body;

          
        }

        public static String PrintTerminal(LanguageData language)
        {
            var termList = language.GrammarData.Terminals.ToList();
            termList.Sort((x, y) => string.Compare(x.Name, y.Name));
            var result = string.Join(Environment.NewLine, termList);
            return result;
        }

        private void reporteTokensToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (correcto == true)
            {
                GenerateHTMLTokens();
            }
            else
            {
                MessageBox.Show("Compilacion Incorrecta", "Advertencia!");
            
            }

        }

        private void generarASTToolStripMenuItem_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawRectangle(Pens.White, generarASTToolStripMenuItem.ContentRectangle);
        }

        private void reporteErroresToolStripMenuItem_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawRectangle(Pens.White, reporteErroresToolStripMenuItem.ContentRectangle);
        }

        private void reporteTokensToolStripMenuItem_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawRectangle(Pens.White, reporteTokensToolStripMenuItem.ContentRectangle);
        }
    }
}
