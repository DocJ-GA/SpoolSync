using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpoolSync.Spoolman
{
    public static class EnumerationExtensions
    {
        public static string GetTypeName(this FilamentType tObj)
        {
            var type = tObj.GetType();
            var memInfo = type.GetMember(tObj.ToString());
            var attributes = memInfo[0].GetCustomAttributes(typeof(FilamentTypeNameAttribute), false);
            return (attributes.Length > 0) ? ((FilamentTypeNameAttribute)attributes[0]).GetName() : "UNK";
        }

        public static string GetInheritName(this FilamentType tObj)
        {
            var type = tObj.GetType();
            var memInfo = type.GetMember(tObj.ToString());
            var attributes = memInfo[0].GetCustomAttributes(typeof(FilamentTypeInheritableAttribute), false);
            return (attributes.Length > 0) ? ((FilamentTypeInheritableAttribute)attributes[0]).GetName() : "UNK";
        }

        public static FilamentType GetFilament(this string filament)
        {
            switch (filament.ToLower())
            {
                case "pla":
                    return FilamentType.PLA;
                case "pla hs":
                case "pla+":
                    return FilamentType.PLA_HS;
                case "pla-silk":
                case "pla silk":
                    return FilamentType.PLA_SILK;
                case "pla-cf":
                case "pla-cf10":
                case "pla cf":
                case "pla cf10":
                    return FilamentType.PLA_CF10;
                case "petg":
                    return FilamentType.PETG;
                case "petg+":
                    return FilamentType.PETG;
                case "petg-cf":
                case "petg-cf10":
                case "petg cf":
                case "petg cf10":
                    return FilamentType.PETG_CF10;
                case "tpu":
                    return FilamentType.TPU;
                default:
                    return FilamentType.PLA;
            }
        }

    }
}
