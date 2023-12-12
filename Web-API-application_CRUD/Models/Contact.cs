using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models
{
    public class Contact
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }

        [ForeignKey("Company")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]

        public int CompanyId { get; set; }
        public Company? Company { get; set; }

        [ForeignKey("Country")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]

        public int CountryId { get; set; }
        public Country? Country { get; set; }
    }
}
