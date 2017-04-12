using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servis.Entity.Entities
{
    [Table("AnketCevaplari")]
    public class AnswersOfSurvey
    {
        [Key]
        public int ID { get; set; }
        public string Cevap { get; set; }
        public int Puan { get; set; }
        public virtual List<Survey> Anketi { get; set; }
    }
}
