using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto1Compi12020.Modelos
{
    class Node
    {
        public Node Next { get; set; }
        public Token Token { get; set; }
        public Error Error { get; set; }
        public Node hijoIzquierdo { get; set; }
        public Node hijoDerecho { get; set; }

        public Node(Token token)
        {
            Token = token;
            Next = null;
            Error = null;
            hijoIzquierdo = hijoDerecho = null;
        }

        public Node(Error error)
        {
            Error = error;
            Next = null;
            Token = null;
            hijoIzquierdo = hijoDerecho = null;
        }
    }
}
