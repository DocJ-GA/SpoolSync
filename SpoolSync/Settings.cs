using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SpoolSync
{
    public class Settings
    {
        public const string AppName = "SpoolSync";
        public const string Version = "1.0.0";
        public string OrcaPath { get; set; } = $"C:\\Users\\andy\\AppData\\Roaming\\OrcaSlicer\\user\\default\\filament";
        public string SpoolmanApi { get; set; } = $"http://10.0.10.1:7912/api/v1/";
        public List<string> Printers = ["JKSN - 0.25 - Flashforge AD5M", "JKSN - 0.4 - Flashforge AD5M", "JKSN - 0.6 - Flashforge AD5M"];

    }
}
