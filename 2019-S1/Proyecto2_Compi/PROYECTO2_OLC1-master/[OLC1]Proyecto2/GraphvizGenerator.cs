using Irony.Parsing;
using System;
using System.IO;

namespace _OLC1_Proyecto2
{
    class GraphvizGenerator
    {
        public static String graph = "";

        public static void ConstruirArbol(ParseTreeNode raiz)
        {
            System.IO.StreamWriter f = new System.IO.StreamWriter("C:\\Users\\JavierG\\Pictures\\AST.txt");
            f.Write("digraph lista{ rankdir=TB;node[shape = box, style = filled, color = white]; ");
            graph = "";
            Generar(raiz);
            f.Write(graph);
            f.Write("}");
            f.Close();
        }

        public static void Generar(ParseTreeNode raiz)
        {
            graph = graph + "nodo" + raiz.GetHashCode() + "[label=\"" + raiz.ToString().Replace("\"", "\\\"") + " \", fillcolor=\"LightBlue\", style =\"filled\", shape=\"box\"]; \n";
            if (raiz.ChildNodes.Count > 0)
            {
                ParseTreeNode[] hijos = raiz.ChildNodes.ToArray();
                for (int i = 0; i < raiz.ChildNodes.Count; i++)
                {
                    Generar(hijos[i]);
                    graph = graph + "\"nodo" + raiz.GetHashCode() + "\"-> \"nodo" + hijos[i].GetHashCode() + "\" \n";
                }
            }
        }
        public static void GraficarArbol(string fileName, string path)
        {
            try
            {
                var command = string.Format("dot.exe -Tjpg {0} -o {1}", Path.Combine(path, fileName), Path.Combine(path, fileName.Replace(".txt", ".jpg")));
                var procStartInfo = new System.Diagnostics.ProcessStartInfo("cmd", "/C " + command);
                procStartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                var proc = new System.Diagnostics.Process();
                procStartInfo.CreateNoWindow = true;

                proc.StartInfo = procStartInfo;
                proc.Start();
                proc.WaitForExit();
            }
            catch (Exception x)
            {

            }
        }

        public void CreateTableErrores()
        {

        }

        

    }
}
