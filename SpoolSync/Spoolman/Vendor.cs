namespace SpoolSync.Spoolman
{
    /// <summary>
    /// Vendor information from Spoolman.
    /// </summary>
    public class Vendor
    {
        public int Id { get; set; }
        public DateTime Registered { get; set; }
        public string Name { get; set; } = "Not Set";
        public string? Comment { get; set; }
        public float? EmptySpoolWeight { get; set; }
        public string? ExternalId { get; set; }
        public dynamic Extra { get; set; } = new object();
    }
}
