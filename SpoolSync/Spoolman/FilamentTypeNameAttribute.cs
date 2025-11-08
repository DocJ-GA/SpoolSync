using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpoolSync.Spoolman
{
    public class FilamentTypeNameAttribute
        : Attribute
    {
        public string Value { get; set; } = "Flashforge Generic PLA";

        public string GetName() => Value;
    }
}
