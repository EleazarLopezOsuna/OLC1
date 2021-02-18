using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Irony.Ast;
using Irony.Parsing;
using Proyecto2_201700893.Graficador;
using Proyecto2_201700893.Modelos;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Diagnostics;
using System.Windows.Forms;

namespace Proyecto2_201700893.Analizador
{
    class Sintactico : Grammar
    {

        public static ParseTree Analizar(String cadena)
        {
            Gramatica gramatica = new Gramatica();
            LanguageData lenguaje = new LanguageData(gramatica);
            Parser parser = new Parser(lenguaje);
            ParseTree arbol = parser.Parse(cadena);
            return arbol;
        }

        public static void crearImagen(ParseTreeNode raiz, NodoSintactico root, int tipo)
        {
            String grafoDot = Arbol.getDot(raiz,tipo, root);
            try
            {
                // Create the file.
                if (tipo == 0)
                {
                    using (FileStream fs = File.Create("AST0.txt"))
                    {
                        Byte[] info = new UTF8Encoding(true).GetBytes(grafoDot);
                        // Add some information to the file.
                        fs.Write(info, 0, info.Length);
                    }
                }else if (tipo == 1)
                {
                    using (FileStream fs = File.Create("AST1.txt"))
                    {
                        Byte[] info = new UTF8Encoding(true).GetBytes(grafoDot);
                        // Add some information to the file.
                        fs.Write(info, 0, info.Length);
                    }
                }
                Process process = new Process();
                ProcessStartInfo startInfo = new ProcessStartInfo();
                startInfo.WindowStyle = ProcessWindowStyle.Hidden;
                startInfo.FileName = "cmd.exe";
                if (tipo == 0)
                {
                    startInfo.Arguments = "/C dot -Tpng AST0.txt -o AST0.png";
                }
                else if (tipo == 1)
                {
                    startInfo.Arguments = "/C dot -Tpng AST1.txt -o AST1.png";
                }
                process.StartInfo = startInfo;
                process.Start();

            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        
    }
}
