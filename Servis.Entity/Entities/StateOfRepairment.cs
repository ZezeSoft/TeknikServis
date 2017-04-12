using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servis.Entity.Entities
{
    [Table("ArızaDurumu")]
    public class StateOfRepairment
    {
        public StateOfRepairment()
        {
            Arizalar = new List<Repairment>();
        }
        [Key]
        public int ID { get; set; }
        [StringLength(50)]
        [Required]
        public string State { get; set; }
        public virtual List<Repairment> Arizalar { get; set; }
    }
}
