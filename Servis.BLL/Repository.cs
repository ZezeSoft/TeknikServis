using Servis.Entity.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servis.BLL.Repository
{
    public class MarkaRepo : RepositoryBase<Brand, int>
    {

    }
    public class CihazRepo : RepositoryBase<Device, int>
    {

    }
    public class FotografRepo : RepositoryBase<Photos, int>
    {

    }
    public class ArizaRepo : RepositoryBase<Repairment, int>
    {

    }

    public class AnketRepo : RepositoryBase<Survey, int>
    {

    }
    public class AnketCevapRepo : RepositoryBase<AnswersOfSurvey, int>
    {

    }
    public class AnketSoruRepo : RepositoryBase<QuestionsOfSurvey, int>
    {

    }
    public class ArizaDurumuRepo : RepositoryBase<StateOfRepairment, int>
    {

    }
}
