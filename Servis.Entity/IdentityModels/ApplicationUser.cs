using Microsoft.AspNet.Identity.EntityFramework;
using Servis.Entity.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servis.Entity.IdentityModels
{
    public class ApplicationUser : IdentityUser
    {
        public ApplicationUser()
        {
            RegisterDate = DateTime.Now;
            Arizalar = new List<Repairment>();
            //Anketler = new List<Survey>();
        }

        [StringLength(25)]
        public string Name { get; set; }

        [StringLength(35)]
        public string SurName { get; set; }

        [Column(TypeName="smalldatetime")]
        public DateTime RegisterDate { get; set; }
        public string AvatarPath { get; set; }
        public bool Available { get; set; }
        public string ActivationCode { get; set; }

        public virtual List<Repairment> Arizalar { get; set; }
        //public virtual List<Survey> Anketler { get; set; }
    }
}
