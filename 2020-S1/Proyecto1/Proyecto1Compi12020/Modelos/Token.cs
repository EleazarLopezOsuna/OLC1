using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto1Compi12020.Modelos
{
    class Token
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public int Column { get; set; }
        public int Row { get; set; }

        public Token(String name, String value, int column, int row)
        {
            Name = name;
            Value = value;
            Column = column;
            Row = row;
        }
    }
}
