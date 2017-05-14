using ConsoleGameLib.CoreTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Login
{
    public interface IConstruction
    {
        Point Location { get; set; }
        int Level { get; set; }

    }
}
