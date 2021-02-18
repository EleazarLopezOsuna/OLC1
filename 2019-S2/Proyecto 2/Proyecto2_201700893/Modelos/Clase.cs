using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto2_201700893.Modelos
{
    class Clase
    {
        public Entorno contenedorDeVariables;
        public Entorno contenedorDeFuncionesYMetodos;

        public Clase(Entorno variables, Entorno metodosfunciones)
        {
            this.contenedorDeVariables = variables;
            this.contenedorDeFuncionesYMetodos = metodosfunciones;
        }

    }
}
