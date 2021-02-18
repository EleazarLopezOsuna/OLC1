using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto1Compi12020.Modelos
{

    class Lista
    {
        public Node Head { get; set; }
        public Lista()
        {
            Head = null;
        }

        public void AddNode(Node NewNode)
        {
            if (Head == null)
            {
                Head = NewNode;
            }
            else
            {
                Node temp = Head;
                while(temp.Next != null)
                {
                    temp = temp.Next;
                }
                temp.Next = NewNode;
            }
        }

        public void RemoveNode()
        {
            if (Head != null)
            {
                Head = Head.Next;
            }
        }

        public string imprimirTokens()
        {
            String retorno = "";
            Node tmp = Head;

            while(tmp != null)
            {
                retorno += tmp.Token.Value + "\n";
                tmp = tmp.Next;
            }

            return retorno;
        }
    }
}
