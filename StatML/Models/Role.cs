namespace StatML.Models
{
    public class Role
    {
        public int Id { get; set; } 
        public string Name { get; set; }    

        public IList<Login>? LoginEntities { get; set; } 
    }
}
