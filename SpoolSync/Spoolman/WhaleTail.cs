using System.ComponentModel.DataAnnotations.Schema;

namespace SpoolSync.Spoolman
{
    public class WhaleTail
    {
        protected int _bedTemp, _nozzleTemp;
        public string Name { get; set; } = "Default Name";
        public string[] FilamentSettingsId { get; set; } = ["-1"];
        public string[] FilamentStartGcode { get; set; } = ["SET_ACTIVE_SPOOL ID="];
        public string[] FilamentEndGcode { get; set; } = ["CLEAR_ACTIVE_SPOOL"];
        public string[] FilamentVendor { get; set; } = ["Generic"];
        public string From { get; set; } = "User";
        public string Inherits { get; set; } = "Flashforge Generic PLA";
        public string Version { get; set; } = "2.2.0.4";
        public string IsCustomDefined { get; set; } = "0";
        public string[] FilamentFlowRatio { get; set; } = ["0.98"];
        public string[] FilamentType { get; set; } = ["PLA"];
        public string[] FilamentDensity { get; set; } = ["1.24"];
        public string[] FilamentDiameter { get; set; } = ["1.75"];
        public string[] FilamentSpoolWeight { get; set; } = ["1000"];
        public string[] FilamentCost { get; set; } = ["18"];
        public string[] DefaultFilamentColour { get; set; } = ["#000000"];
        public string[] CompatiblePrinters { get; set; } = [];

        public string[] NozzleTemperature
        {
            get => [_nozzleTemp.ToString()];
            set
            {
                if (value.Length < 1)
                    return;
                int nozzleTemp;
                if (int.TryParse(value[0], out nozzleTemp))
                    _nozzleTemp = nozzleTemp;

            }
        }
        public string[] NozzleTemperatureInitialLayer
        {
            get
            {
                return [(_nozzleTemp + 10).ToString()];
            }
            set
            {
                return;
            }
        }
        public string[] CoolPlateTemp
        {
            get => [_bedTemp.ToString()];
            set { return; }
        }
        public string[] EngPlateTemp
        {
            get => [_bedTemp.ToString()];
            set { return; }
        }
        public string[] HotPlateTemp
        {
            get => [_bedTemp.ToString()];
            set { return; }
        }
        public string[] TexturedPlateTemp
        {
            get => [_bedTemp.ToString()];
            set
            {
                if (value.Length < 1)
                    return;
                int bedTemp;
                if (int.TryParse(value[0], out bedTemp))
                    _bedTemp = bedTemp;
            }
        }
        public string[] CoolPlateTempInitialLayer
        {
            get => [(_bedTemp + 10).ToString()];
            set { return; }
        }
        public string[] EngPlateTempInitialLayer
        {
            get => [(_bedTemp + 10).ToString()];
            set { return; }
        }
        public string[] HotPlateTempInitialLayer
        {
            get => [(_bedTemp + 10).ToString()];
            set { return; }
        }
        public string[] TexturedPlateTempInitialLayer
        {
            get => [(_bedTemp + 10).ToString()];
            set { return; }
        }


        public string _comment { get; set; } = "Generated";

        [NotMapped]
        public bool Active { get; set; } = false;
        [NotMapped]
        public bool MarkForDeletion { get => !Active; }


        public WhaleTail()
        {
            NozzleTemperatureInitialLayer = ["220"];
            _bedTemp = 55;
            _nozzleTemp = 210;
        }


        public string GetJsonFileName()
        {
            return Name + ".json";
        }
        public string GetInfoFileName()
        {
            return Name + ".info";
        }

        [NotMapped]
        public string? CurrentFilePath { get; set; }

        public string GetInfoPath()
        {
            return Path.Combine(Path.GetDirectoryName(CurrentFilePath) ?? "", Path.GetFileNameWithoutExtension(CurrentFilePath) + ".info");
        }

        public string GetInfoContent()
        {
            return "sync_info = create" + Environment.NewLine +
                "user_id =" + Environment.NewLine +
                "setting_id =" + Environment.NewLine +
                "base_id = GFSA04" + Environment.NewLine +
                "updated_time = " + DateTime.Now.Ticks;
        }

        public WhaleTail SetCurrentPath(string path)
        {
            CurrentFilePath = path;
            return this;
        }

    }
}
