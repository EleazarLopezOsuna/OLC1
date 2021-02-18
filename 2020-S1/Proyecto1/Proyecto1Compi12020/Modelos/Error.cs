using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto1Compi12020.Modelos
{
    class Error
    {
        public string Value { get; set; }
        public int Column { get; set; }
        public int Row { get; set; }

        public Error(String value, int column, int row)
        {
            Value = value;
            Column = column;
            Row = row;
        }
    }
}
