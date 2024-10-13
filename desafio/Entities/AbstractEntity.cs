using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace desafio.Entities
{
    public class AbstractEntity
    {
        [Column]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        [Column("update_at")]
        public DateTime UpdateAt { get; set; } = DateTime.UtcNow;
        [Column("is_deleted")]
        public bool IsDeleted { get; set; } = false;

        public AbstractEntity() { }
    }
}
