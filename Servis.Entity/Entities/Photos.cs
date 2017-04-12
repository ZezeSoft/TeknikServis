using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servis.Entity.Entities
{
    [Table("Fotograflar")]
    public class Photos
    {
        [Key]
        public int ID { get; set; }
        [Required]
        public string Path { get; set; }
        public int ArizaID { get; set; }
        [ForeignKey("ArizaID")]
        public virtual Repairment Ariza { get; set; }
    }
}
