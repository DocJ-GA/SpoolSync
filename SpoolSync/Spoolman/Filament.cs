using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpoolSync.Spoolman
{
    public class Filament
    {
        IList<string> _colors = [];
        public int Id { get; set; }
        public DateTime Registered { get; set; }
        public string? Name { get; set; }
        public Vendor? Vendor { get; set; }
        public string? Material { get; set; }
        public FilamentType FilamentType
        {
            get
            {
                if (Material == null)
                    Material = "PLA";
                return Material.GetFilament();
            }
            set
            {
                Material = value.GetTypeName();
            }
        }
        public float? Price { get; set; }
        public float Density { get; set; }
        public float Diameter { get; set; }
        public float? Weight { get; set; }
        public float? SpoolWeight { get; set; }
        public string? ArticleNumber { get; set; }
        public string? Comment { get; set; }
        public int? SettingsExtruderTemp { get; set; }
        public int? SettingsBedTemp { get; set; }
        public string? ColorHex { get; set; }
        public string? MultiColorHexes
        {
            get
            {
                return String.Join(",", _colors);
            }
            set
            {
                if (value == null)
                    return;
                _colors = new List<string>(value.Split(','));
            }
        }
        [NotMapped]
        public IList<string> ColorHexes { get => _colors; }
        public string? MultiColorDirection { get; set; }
        public int? ExternalId { get; set; }
        public Extra Extra { get; set; } = new Extra();

        public WhaleTail ToWhale()
        {
            var name = string.Format("F#{0:D4} - {1} - {2}", Id, Vendor?.Name ?? "Generic", Name);
            var whale = new WhaleTail()
            {
                FilamentCost = [Price.ToString() ?? "18"],
                FilamentSettingsId = ["F" + Id.ToString()],
                FilamentVendor = [Vendor?.Name ?? "Genderic"],
                Name = name,
                FilamentType = [FilamentType.GetTypeName()],
                FilamentDensity = [Density.ToString()],
                FilamentDiameter = [Diameter.ToString()],
                FilamentSpoolWeight = [SpoolWeight?.ToString() ?? "1000"],
                FilamentFlowRatio = [Extra?.FlowRatio ?? "0.98"],
                TexturedPlateTemp = [SettingsBedTemp?.ToString() ?? "55"],
                Inherits = FilamentType.GetInheritName(),
                NozzleTemperature = [SettingsExtruderTemp?.ToString() ?? "210"]
            };
            whale.FilamentStartGcode[0] += Id.ToString();

            if (ColorHexes.Count > 0)
                whale.DefaultFilamentColour = ColorHexes.Select(c => "#" + c).ToArray();
            else
                whale.DefaultFilamentColour = [(ColorHex != null ? "#" + ColorHex : "#000000")];

            return whale;
        }
    }
}
