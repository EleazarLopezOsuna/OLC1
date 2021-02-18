using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto2_201700893.Modelos
{
    class Expresion
    {
        public Simbolo.EnumTipo tipo;
        public Object valor;

        public Expresion(Simbolo.EnumTipo tipo, Object valor)
        {
            this.tipo = tipo;
            this.valor = valor;
        }
    }
}
