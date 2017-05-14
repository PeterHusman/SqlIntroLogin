using ConsoleGameLib.CoreTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Login
{
    class Farm : IConstruction
    {
        public Point Location { get; set; }
        public int Level { get; set; }
    }
}
