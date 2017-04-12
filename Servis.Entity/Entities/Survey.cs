using Servis.Entity.IdentityModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servis.Entity.Entities
{
    [Table("Anket")]
    public class Survey
    {
             
        [Key]
        public int ID { get; set; }
        public int? SoruID { get; set; }
        [ForeignKey("SoruID")]
        public virtual QuestionsOfSurvey Sorusu { get; set; }
        public int? CevapID { get; set; }
        [ForeignKey("CevapID")]
        public virtual AnswersOfSurvey Cevabi { get; set; }
        //public string KullaniciID { get; set; }
        //[ForeignKey("KullaniciID")]
        //public virtual ApplicationUser AnketYapilanKisi { get; set; }
        //public virtual List<Repairment> AnketinArizalari { get; set; }
        public int? ArizaID { get; set; }
        [ForeignKey("ArizaID")]
        public virtual Repairment Ariza { get; set; }
    }
}
