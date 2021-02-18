using Irony.Parsing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _OLC1_Proyecto2
{
    class Recorrido
    {

        public static void CrearTablaTokens(ParseTreeNode root)
        {
            expresion(root.ChildNodes.ElementAt(0));
        }

        public static void expresion(ParseTreeNode root)
        {
            Console.WriteLine("expresion");
            switch (root.ChildNodes.Count)
            {
                
                case 1:
                    Console.WriteLine("dentro");
                    String asdf = (root.ChildNodes.ElementAt(0).Token.Value.ToString());
                    Console.WriteLine(asdf);
                    break;
            }
        }

    }
}
