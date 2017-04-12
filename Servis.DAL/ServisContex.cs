using Microsoft.AspNet.Identity.EntityFramework;
using Servis.Entity.Entities;
using Servis.Entity.IdentityModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servis.DAL
{
    public class ServisContex : IdentityDbContext<ApplicationUser>
    {
        public ServisContex() : base("name=ServisCon")
        {

        }

        public virtual DbSet<Brand> Marka { get; set; }
        public virtual DbSet<Device> Cihaz { get; set; }
        public virtual DbSet<Photos> Fotograflar { get; set; }
        public virtual DbSet<Repairment> Ariza { get; set; }
        public virtual DbSet<Survey> Anket { get; set; }
        public virtual DbSet<AnswersOfSurvey> AnketCevaplari { get; set; }
        public virtual DbSet<QuestionsOfSurvey> AnketSorulari { get; set; }
        public virtual DbSet<StateOfRepairment> ArizaDurumu { get; set; }
    }
}
