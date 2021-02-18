using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Proyecto1Compi12020.Modelos;

namespace Proyecto1Compi12020.Generadores
{
    class GeneradorXML
    {

        public string generadorTokens(Lista listaTokens)
        {
            Node tmp = listaTokens.Head;
            string xmlString = "<ListaTokens>\n";
            while (tmp != null)
            {
                xmlString += "\t<Token>\n";
                xmlString += "\t\t<Nombre> " + tmp.Token.Name + " </Nombre>\n";
                xmlString += "\t\t<Valor> " + tmp.Token.Value + " </Valor>\n";
                xmlString += "\t\t<Fila> " + tmp.Token.Row + " </Fila>\n";
                xmlString += "\t\t<Columna> " + tmp.Token.Column + " </Columna>\n";
                xmlString += "\t</Token>\n";
                tmp = tmp.Next;
            }
            xmlString += "</ListaTokens>\n";

            return xmlString;
        }

        public string generadorErrores(Lista listaErrores)
        {
            Node tmp = listaErrores.Head;
            string xmlString = "<ListaErrores>\n";
            while (tmp != null)
            {
                xmlString += "\t<Error>\n";
                xmlString += "\t\t<Valor> " + tmp.Error.Value + " </Valor>\n";
                xmlString += "\t\t<Fila> " + tmp.Error.Row + " </Fila>\n";
                xmlString += "\t\t<Columna> " + tmp.Error.Column + " </Columna>\n";
                xmlString += "\t</Error>\n";
                tmp = tmp.Next;
            }
            xmlString += "</ListaErrores>\n";

            return xmlString;
        }
    }
}
