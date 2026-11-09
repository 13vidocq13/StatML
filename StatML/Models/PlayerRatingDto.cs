namespace StatML.Models
{
    public class PlayerRatingDto
    {
        public string Username { get; set; } = string.Empty;
        public long ArmyPower { get; set; }
        public DateTime SnapshotDate { get; set; }
    }
}
