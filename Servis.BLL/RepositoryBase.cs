using Servis.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servis.BLL.Repository
{
    public abstract class RepositoryBase<T,ID> where T:class
    {
        protected internal static ServisContex dbContext;

        public List<T> GetAll()
        {
            dbContext = new ServisContex();
            return dbContext.Set<T>().ToList();
        }
        public T GetById(ID id)
        {
            dbContext = new ServisContex();
            return dbContext.Set<T>().Find(id);
        }
        public int Insert(T entity)
        {
            dbContext = dbContext ?? new ServisContex();

            dbContext.Set<T>().Add(entity);
            return dbContext.SaveChanges();
        }
        public int Delete(T entity)
        {
            dbContext = dbContext ?? new ServisContex();
            dbContext.Set<T>().Remove(entity);
            return dbContext.SaveChanges();
        }
        public int Update()
        {
            dbContext = dbContext ?? new ServisContex();
            return dbContext.SaveChanges();
        }
    }
}
