using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Web;


namespace Azure_0517.Models.Repository
{
    public class GenericRepository<TEntity> : IRepository<TEntity> 
        where TEntity : class
    {
        protected Taste_ItEntities dbContext = new Taste_ItEntities();

        public virtual bool Delete(TEntity entity)
        {
            dbContext.Set<TEntity>().Remove(entity);
            dbContext.SaveChanges();
            return true;
            //return TryCatch<TEntity>.RepositoryTryCatch(
            //    E =>
            //    {
            //        dbContext.Set<TEntity>().Remove(E);
            //        dbContext.SaveChanges();
            //        return true;
            //    }, entity);
        }

        public virtual IEnumerable<TEntity> GetAll()
        {
            IEnumerable<TEntity> entities = dbContext.Set<TEntity>().ToList();
            if (entities.Count() == 0)
            {
                //throw new NullReferenceException();
            }
            return entities;
            //return TryCatch<TEntity>.RepositoryTryCatch(
            //    () =>
            //    {
            //        IEnumerable<TEntity> entities = dbContext.Set<TEntity>().ToList();
            //        if (entities.Count() == 0)
            //        {
            //            throw new NullReferenceException();
            //        }
            //        return entities;
            //    });
        }

        public virtual TEntity GetByID(int id)
        {

            TEntity entity = dbContext.Set<TEntity>().Find(id);
            if (entity == null)
            {
                //throw new NullReferenceException();
            }
            return entity;
            //return TryCatch<TEntity>.RepositoryTryCatch(
            //    ID =>
            //    {
            //        TEntity entity = dbContext.Set<TEntity>().Find(ID);
            //        if (entity == null)
            //        {
            //            throw new NullReferenceException();
            //        }
            //        return entity;
            //    }, id);

        }

        public virtual bool Insert(TEntity entity)
        {
            dbContext.Set<TEntity>().Add(entity);
            dbContext.SaveChanges();
            return true;
            //return TryCatch<TEntity>.RepositoryTryCatch(
            //    E =>
            //    {
            //        dbContext.Set<TEntity>().Add(E);
            //        dbContext.SaveChanges();
            //        return true;
            //    }, entity);
        }

        public virtual bool Update(TEntity entity)
        {
            dbContext.Entry(entity).State = System.Data.Entity.EntityState.Modified;
            dbContext.SaveChanges();
            return true;
            //return TryCatch<TEntity>.RepositoryTryCatch(
            //    E =>
            //    {
            //        dbContext.Entry(E).State = System.Data.Entity.EntityState.Modified;
            //        dbContext.SaveChanges();
            //        return true;
            //    }, entity);
        }

       







        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
            {
                return;
            }
            if (disposing)
            {
                dbContext.Dispose();
            }
            disposed = true;
        }

        bool disposed = false;

        //?????
        ~GenericRepository()
        {
            this.Dispose(false);
            Debug.WriteLine("呼叫的類別物件已被記憶體回收程序釋放");
        }
    }
}