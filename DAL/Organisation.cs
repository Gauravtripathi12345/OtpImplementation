using System.ComponentModel.DataAnnotations;

namespace DAL
{
    public class Organisation
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
