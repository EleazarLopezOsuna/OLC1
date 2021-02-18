using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto2_201700893.Modelos
{
    class Error
    {
        public String archivo;
        public String fila;
        public String columna;
        public String tipo;
        public String error;

        public Error(String archivo, String fila, String columna, String tipo, String error)
        {
            this.archivo = archivo;
            this.fila = fila;
            this.columna = columna;
            this.tipo = tipo;
            this.error = error;
        }
    }
}
