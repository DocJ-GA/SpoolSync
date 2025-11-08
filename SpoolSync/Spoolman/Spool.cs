using System.ComponentModel.DataAnnotations.Schema;

namespace SpoolSync.Spoolman
{
    public class Spool
    {
        float? _price, _spoolWeight;
        public int Id { get; set; }
        public DateTime Registered { get; set; }
        public DateTime? FirstUsed { get; set; }
        public DateTime? LastUsed { get; set; }
        public Filament Filament { get; set; } = null!;
        public float? Price
        {
            get
            {
                if (_price == null)
                    return Filament.Price;
                return _price;
            }
            set
            {
                _price = value;
            }
        }
        public float? RemainingWeight { get; set; }
        public float? InitialWeight { get; set; }
        public float? SpoolWeight { get; set; }
        public float UsedWeight { get; set; }
        public float? RemainingLength { get; set; }
        public float UsedLength { get; set; }
        public string? Location { get; set; }
        public string? LotNr { get; set; }
        public string? Comment { get; set; }
        public bool Archived { get; set; }
        public Extra Extra { get; set; } = new Extra();

        [NotMapped]
        public bool IsWhale { get; set; } = false;

        public WhaleTail ToWhaleTail()
        {
            var whale = new WhaleTail()
            {
                FilamentCost = [Price.ToString() ?? "18"],
                FilamentSettingsId = [Id.ToString()],
                FilamentVendor = [Filament.Vendor?.Name ?? "Generic"],
                Name = string.Format("#{0:D4} - {1} - {2} - {3}", Id, Filament.FilamentType.GetTypeName(), Filament.Name, Filament.Vendor?.Name ?? "Generic"),
                FilamentType = [Filament.FilamentType.GetTypeName()],
                FilamentDensity = [Filament.Density.ToString()],
                FilamentDiameter = [Filament.Diameter.ToString()],
                FilamentSpoolWeight = [Filament.SpoolWeight?.ToString() ?? "1000"],
                TexturedPlateTemp = [Filament.SettingsBedTemp?.ToString() ?? "55"],
                Inherits = Filament.FilamentType.GetInheritName(),
                NozzleTemperature = [Filament.SettingsExtruderTemp?.ToString() ?? "210"]
            };
            whale.FilamentStartGcode[0] += Id.ToString();
            whale.FilamentFlowRatio = [Filament.Extra.FlowRatio ?? "0.98"];

            whale.FilamentCost = [((1000 / Filament.Weight ?? 1000) * Price ?? 14f).ToString()];

            if (Filament.ColorHexes.Count > 0)
                whale.DefaultFilamentColour = Filament.ColorHexes.Select(c => "#" + c).ToArray();
            else
                whale.DefaultFilamentColour = [(Filament.ColorHex != null ? "#" + Filament.ColorHex : "#000000")];

            return whale;
        }

    }
}
