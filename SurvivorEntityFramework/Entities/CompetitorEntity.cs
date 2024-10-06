using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SurvivorEntityFramework.Entities
{
    [Table("Competitor")]
    public class CompetitorEntity : BaseEntity
    {
        
        public string FirstName { get; set; }

        
        public string LastName { get; set; }

        
        public int CategoryId { get; set; }

        [ForeignKey("CategoryId")]
        public CategoryEntity Category { get; set; }
    }
}
