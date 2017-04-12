using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servis.Entity.Entities
{
    [Table("AnketSorulari")]
    public class QuestionsOfSurvey
    {
        [Key]
        public int ID { get; set; }
        public string Soru { get; set; }
        public virtual List<Survey> Anketi { get; set; }
    }
}
