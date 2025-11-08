using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpoolSync.Spoolman
{
    public enum FilamentType
    {
        [FilamentTypeInheritable(Value = "Flashforge Generic PLA")]
        [FilamentTypeName(Value = "PLA")]
        PLA,
        [FilamentTypeInheritable(Value = "Flashforge Generic PLA")]
        [FilamentTypeName(Value = "PLA")]
        PLA_HS,
        [FilamentTypeInheritable(Value = "Flashforge Generic PLA-CF")]
        [FilamentTypeName(Value = "PLA-CF")]
        PLA_CF10,
        [FilamentTypeInheritable(Value = "Flashforge Generic PLA")]
        [FilamentTypeName(Value = "PLA")]
        PLA_SILK,
        [FilamentTypeInheritable(Value = "Flashforge Generic PETG")]
        [FilamentTypeName(Value = "PETG")]
        PETG,
        [FilamentTypeInheritable(Value = "Flashforge Generic PETG-CF")]
        [FilamentTypeName(Value = "PETG-CF")]
        PETG_CF10,
        [FilamentTypeInheritable(Value = "Flashforge Generic TPU")]
        [FilamentTypeName(Value = "TPU")]
        TPU
    }
}
