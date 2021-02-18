using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto1Compi12020.Modelos
{
    class modelosThompson
    {
        public nodoTh nodoInicio { get; set; }
        public nodoTh nodoFinal { get; set; }

        public modelosThompson()
        {
            nodoInicio = nodoFinal = null;
        }

        public class terminalTh : modelosThompson
        {

            public terminalTh(int inicio, String transicion)
            {
                nodoInicio = new nodoTh(inicio);
                nodoFinal = new nodoTh(inicio + 1);
                nodoInicio.transiciones.Add(new Transicion(transicion, nodoFinal));
            }
        }

        public class interseccionTh : modelosThompson
        {
            public interseccionTh(modelosThompson modelo1, modelosThompson modelo2)
            {
                modelo1.nodoFinal.transiciones.Add(new Transicion("ε", modelo2.nodoInicio));

                nodoInicio = modelo1.nodoInicio;
                nodoFinal = modelo2.nodoFinal;
            }
        }

        public class interrogacionTh : modelosThompson
        {
            public interrogacionTh(modelosThompson modelo)
            {
                modelo.nodoInicio.transiciones.Add(new Transicion("ε", modelo.nodoFinal));

                nodoInicio = modelo.nodoInicio;
                nodoFinal = modelo.nodoFinal;
            }
        }

        public class kleeneTh : modelosThompson
        {
            public kleeneTh(modelosThompson modelo)
            {
                nodoInicio = new nodoTh(modelo.nodoFinal.numeroNodo + 1);
                nodoInicio.transiciones.Add(new Transicion("ε", modelo.nodoInicio));

                nodoFinal = new nodoTh(modelo.nodoFinal.numeroNodo + 2);

                nodoInicio.transiciones.Add(new Transicion("ε", nodoFinal));

                modelo.nodoFinal.transiciones.Add(new Transicion("ε", modelo.nodoInicio));
                modelo.nodoFinal.transiciones.Add(new Transicion("ε", nodoFinal));
            }
        }

        public class positivaTh : modelosThompson
        {
            public positivaTh(modelosThompson modelo)
            {
                nodoInicio = modelo.nodoInicio;

                nodoFinal = new nodoTh(modelo.nodoFinal.numeroNodo + 2);

                modelo.nodoFinal.transiciones.Add(new Transicion("ε", modelo.nodoInicio));
                modelo.nodoFinal.transiciones.Add(new Transicion("ε", nodoFinal));
            }
        }

        public class unionTh : modelosThompson
        {
            public unionTh(int numero, modelosThompson modelo1, modelosThompson modelo2)
            {
                nodoInicio = new nodoTh(numero);
                nodoInicio.transiciones.Add(new Transicion("ε", modelo1.nodoInicio));
                nodoInicio.transiciones.Add(new Transicion("ε", modelo2.nodoInicio));

                nodoFinal = new nodoTh(numero + 1);
                modelo1.nodoFinal.transiciones.Add(new Transicion("ε", nodoFinal));
                modelo2.nodoFinal.transiciones.Add(new Transicion("ε", nodoFinal));
            }
        }

        public class nodoTh
        {
            public int numeroNodo { get; set; }
            public List<Transicion> transiciones { get; set; }

            public nodoTh(int numeroNodo)
            {
                this.numeroNodo = numeroNodo;
                transiciones = new List<Transicion>();
            }
        }

        public class Transicion
        {
            public String cadena { get; set; }
            public nodoTh nodo { get; set; }

            public Transicion(String cadena, nodoTh nodo)
            {
                this.cadena = cadena;
                this.nodo = nodo;
            }
        }
    }
}
