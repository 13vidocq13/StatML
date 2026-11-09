namespace StatML.Models
{
    public class Sett
    {
        public int Id { get; set; }
        public string? Name { get; set; }    
        public int Level { get; set; }
        public List<Artifact>? ArtifactsEntities { get; set; }
    }
}
