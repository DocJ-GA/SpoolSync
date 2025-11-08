using SpoolSync.Spoolman;
using System.Net.Http.Json;
using System.Text.Json;
using SpoolSync.Extensions;

namespace SpoolSync
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly JsonSerializerOptions _jsonOptions = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower, };

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {

            if (_logger.IsEnabled(LogLevel.Information))
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            }

            while (!stoppingToken.IsCancellationRequested)
            {
                var client = new HttpClient();
                var spools = new List<Spool>();
                var whales = new List<WhaleTail>();
                var whaleFiles = new List<string>();
                
                _logger.LogInformation("Grabbings spools.");

                // Get spools from SpoolMan API
                try
                {
                    spools.CAddRange(client.GetFromJsonAsync<List<Spool>>(Program.AppSettings.SpoolmanApi + "spool", _jsonOptions).Result);
                }
                catch (Exception ex)
                {
                    _logger.LogError($"There was an error getting the spools from the SpoolMan API. Error Message: '{ex.Message}'.");
                    Environment.Exit(3);
                }
                _logger.LogInformation($"Spools retrieved. Number: {spools.Count}.");

                // Get filaments from OrcaSlicer directory
                try
                {
                    whaleFiles.CAddRange(Directory.GetFiles(Program.AppSettings.OrcaPath, "*.json"));
                }
                catch (DirectoryNotFoundException ex)
                {
                    _logger.LogError($"Directory for OrcaSlicer filaments was not found.  Directory: '{Program.AppSettings.OrcaPath}'.  Error Message: '" + ex.Message + '.');
                    Environment.Exit(2);
                }
                catch (IOException ex)
                {
                    _logger.LogError($"There was an error reading the OrcaSlicer filament directory files.There was an error reading the OrcaSlicer filament directory files.  Directory: '{Program.AppSettings.OrcaPath}'.  Error Message: '{ex.Message}'.");
                    Environment.Exit(2);
                }
                catch (UnauthorizedAccessException ex)
                {
                    _logger.LogError($"Accessing the directory for OrcaSlicer filaments was denied.  Check permissions.  Accessing the directory for OrcaSlicer filaments was denied.  Check permsions.  Directory: '{Program.AppSettings.OrcaPath}'.  Error Message: '{ex.Message}'.");
                    Environment.Exit(2);
                }

                foreach (var tailPath in whaleFiles)
                {
                    string fileText = string.Empty;
                    try
                    {
                        fileText = File.ReadAllText(tailPath);
                    }
                    catch (DirectoryNotFoundException ex)
                    {
                        _logger.LogError($"Directory for OrcaSlicer filaments was not found.  Directory for OrcaSlicer filaments was not found.  Directory: '{Program.AppSettings.OrcaPath}'.  Error Message: '{ex.Message}'.");
                        Environment.Exit(2);
                    }
                    catch (FileNotFoundException ex)
                    {
                        _logger.LogError($"File for OrcaSlicer filament was not found.  Directory for OrcaSlicer filaments was not found.  File Path: '{tailPath}'.  Error Message: '{ex.Message}'.");
                        continue;
                    }
                    catch (IOException ex)
                    {
                        _logger.LogError($"There was an error reading the OrcaSlicer filament file.  There was an error reading the OrcaSlicer filament file.  File Path: '{tailPath}'.  Error Message: '{ex.Message}'.");
                        continue;
                    }
                    catch (UnauthorizedAccessException ex)
                    {
                        _logger.LogError($"Accessing the directory for OrcaSlicer filaments was denied.  Check permissions.  Accessing the directory for OrcaSlicer filaments was denied.  Check permsions.  FilePath: '{tailPath}'.  Error Message: '{ex.Message}'.");
                        continue;
                    }
                    var tail = JsonSerializer.Deserialize<WhaleTail>(fileText, _jsonOptions);
                    if (tail != null)
                        whales.Add(tail.SetCurrentPath(tailPath));
                }
                _logger.LogInformation($"Whale Tails retrieved.  Number: {whales.Count}.");

                // Syncing
                foreach (var spool in spools)
                {
                   _logger.LogInformation($"Checking for spool's whale tail.  Id: {spool.Id}.  Name: {spool.Filament.Name}");
                    var match = whales.FirstOrDefault(whaleTail => whaleTail.FilamentSettingsId.Length >= 1 && whaleTail.FilamentSettingsId[0] == spool.Id.ToString());
                    spool.IsWhale = match != null;
                    if (match != null)
                        match.Active = true;
                }

                // Remove dead whale tails
                _logger.LogInformation($"Removing dead whale tails.  Removing dead whale tails.  Hit List Count: {whales.Count(w => !w.Active)}");
                var killed = 0;
                foreach (var whaleTail in whales.Where(w => !w.Active))
                {
                    _logger.LogInformation($"Removing whale tail.  Name: {whaleTail.Name}.  Id: {whaleTail.FilamentSettingsId[0]}.");
                    try
                    {
                        File.Delete(whaleTail.CurrentFilePath!);
                        File.Delete(whaleTail.GetInfoPath());
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError($"There was an error deleting the whale tail file.  File Path: '{whaleTail.CurrentFilePath}'.  Error Message: '{ex.Message}'.");
                    }
                    killed++;
                }
                _logger.LogInformation($"Whale tails killed.  Kill count: {killed}.");

                _logger.LogInformation("Addig new whale tails.");
                foreach (var spool in spools.Where(s => !s.IsWhale))
                {
                    _logger.LogInformation($"Adding spool's whaletail.  Id: {spool.Id}.  Name: {spool.Filament.Name}");
                    var whaleTail = spool.ToWhaleTail();
                    File.WriteAllText(Path.Combine(Program.AppSettings.OrcaPath, whaleTail.GetJsonFileName()), JsonSerializer.Serialize(whaleTail, _jsonOptions));
                    File.WriteAllText(Path.Combine(Program.AppSettings.OrcaPath, whaleTail.GetInfoFileName()), whaleTail.GetInfoContent());
                }


                // Remove empty spools from spoolman
                foreach (var spool in spools.Where(s => s.RemainingWeight == 0))
                {
                    spool.Archived = true;
                    var response = client.PatchAsJsonAsync(Program.AppSettings.SpoolmanApi + "spool/" + spool.Id, new { archived = true }, _jsonOptions).Result;
                }

                await Task.Delay(300000, stoppingToken);
            }
        }
    }
}
