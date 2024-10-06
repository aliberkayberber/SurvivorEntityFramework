using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SurvivorEntityFramework.Entities
{
    [Table("Category")]
    public class CategoryEntity : BaseEntity
    {
        
        public  string Name { get; set; }

        public List<CompetitorEntity> Competitors { get; set; }
    }
}
