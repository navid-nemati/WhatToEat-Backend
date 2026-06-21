using System.ComponentModel.DataAnnotations;

namespace Authentication_Practice.Models
{
    public class BaseEntity
    {
        [Key]
        public Guid Id { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime LastUpdateDate { get; set; }
        //public bool IsDeleted { get; set; }
    }
}
