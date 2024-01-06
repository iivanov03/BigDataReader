using System.ComponentModel.DataAnnotations;

namespace BigDataReader.Data.Entities
{
    public class Country
    {
        public Country()
        {
            Organizations = new HashSet<Organization>();
        }

        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public ICollection<Organization> Organizations { get; set; }
    }
}
