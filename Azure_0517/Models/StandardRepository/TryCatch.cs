using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace Azure_0517.Models.Repository
{
    static public class TryCatch<TEntity>
    {

        public static bool RepositoryTryCatch(Func<TEntity, bool> func, TEntity entity)
        {
            try
            {
                return func.Invoke(entity);
            }
            catch (SqlException SQLex)
            {
                Debug.WriteLine($"{SQLex.Data} cause SqlException.");
                Debug.WriteLine("StackTrace:\n" + SQLex.StackTrace);
                Debug.WriteLine("Message:\n" + SQLex.Message);
                return default(bool);
            }
            catch (DbEntityValidationException EFex)
            {
                Debug.WriteLine($"{EFex.Data} cause DbEntityValidationException.");
                Debug.WriteLine("StackTrace:\n" + EFex.StackTrace);
                Debug.WriteLine("Message:\n" + EFex.Message);
                return default(bool);
            }
            catch (NullReferenceException Nex)
            {
                Debug.WriteLine($"{Nex.Data} cause NullReferenceException.");
                Debug.WriteLine("StackTrace:\n" + Nex.StackTrace);
                Debug.WriteLine("Message:\n" + Nex.Message);
                return default(bool);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{ex.Data} cause Exception.");
                Debug.WriteLine("StackTrace:\n" + ex.StackTrace);
                Debug.WriteLine("Message:\n" + ex.Message);
                return default(bool);
            }
        }

        public static int RepositoryTryCatch(Func<int> p)
        {
            try
            {
                return p.Invoke();
            }
            catch (SqlException SQLex)
            {
                Debug.WriteLine($"{SQLex.Data} cause SqlException.");
                Debug.WriteLine("StackTrace:\n" + SQLex.StackTrace);
                Debug.WriteLine("Message:\n" + SQLex.Message);
                return default(int);
            }
            catch (DbEntityValidationException EFex)
            {
                Debug.WriteLine($"{EFex.Data} cause DbEntityValidationException.");
                Debug.WriteLine("StackTrace:\n" + EFex.StackTrace);
                Debug.WriteLine("Message:\n" + EFex.Message);
                return default(int);
            }
            catch (NullReferenceException Nex)
            {
                Debug.WriteLine($"{Nex.Data} cause NullReferenceException.");
                Debug.WriteLine("StackTrace:\n" + Nex.StackTrace);
                Debug.WriteLine("Message:\n" + Nex.Message);
                return default(int);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{ex.Data} cause Exception.");
                Debug.WriteLine("StackTrace:\n" + ex.StackTrace);
                Debug.WriteLine("Message:\n" + ex.Message);
                return default(int);
            }
        }

        public static TEntity RepositoryTryCatch(Func<int, TEntity> func, int Key)
        {
            try
            {
                return func.Invoke(Key);
            }
            catch (SqlException SQLex)
            {
                Debug.WriteLine($"{SQLex.Data} cause SqlException.");
                Debug.WriteLine("StackTrace:\n" + SQLex.StackTrace);
                Debug.WriteLine("Message:\n" + SQLex.Message);
                return default(TEntity);
            }
            catch (DbEntityValidationException EFex)
            {
                Debug.WriteLine($"{EFex.Data} cause DbEntityValidationException.");
                Debug.WriteLine("StackTrace:\n" + EFex.StackTrace);
                Debug.WriteLine("Message:\n" + EFex.Message);
                return default(TEntity);
            }
            catch (NullReferenceException Nex)
            {
                Debug.WriteLine($"{Nex.Data} cause NullReferenceException.");
                Debug.WriteLine("StackTrace:\n" + Nex.StackTrace);
                Debug.WriteLine("Message:\n" + Nex.Message);
                return default(TEntity);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{ex.Data} cause Exception.");
                Debug.WriteLine("StackTrace:\n" + ex.StackTrace);
                Debug.WriteLine("Message:\n" + ex.Message);
                return default(TEntity);
            }
        }

        public static IEnumerable<TEntity> RepositoryTryCatch(Func<IEnumerable<TEntity>> func)
        {
            try
            {
                return func.Invoke();
            }
            catch (SqlException SQLex)
            {
                Debug.WriteLine($"{SQLex.Data} cause SqlException.");
                Debug.WriteLine("StackTrace:\n" + SQLex.StackTrace);
                Debug.WriteLine("Message:\n" + SQLex.Message);
                return default(IEnumerable<TEntity>);
            }
            catch (DbEntityValidationException EFex)
            {
                Debug.WriteLine($"{EFex.Data} cause DbEntityValidationException.");
                Debug.WriteLine("StackTrace:\n" + EFex.StackTrace);
                Debug.WriteLine("Message:\n" + EFex.Message);
                return default(IEnumerable<TEntity>);
            }
            catch (NullReferenceException Nex)
            {
                Debug.WriteLine($"{Nex.Data} cause NullReferenceException.");
                Debug.WriteLine("StackTrace:\n" + Nex.StackTrace);
                Debug.WriteLine("Message:\n" + Nex.Message);
                return default(IEnumerable<TEntity>);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{ex.Data} cause Exception.");
                Debug.WriteLine("StackTrace:\n" + ex.StackTrace);
                Debug.WriteLine("Message:\n" + ex.Message);
                return default(IEnumerable<TEntity>);
            }

        }

        public static IEnumerable<TEntity> RepositoryTryCatch(Func<int,IEnumerable<TEntity>> func, int Key)
        {
            try
            {
                return func.Invoke(Key);
            }
            catch (SqlException SQLex)
            {
                Debug.WriteLine($"{SQLex.Data} cause SqlException.");
                Debug.WriteLine("StackTrace:\n" + SQLex.StackTrace);
                Debug.WriteLine("Message:\n" + SQLex.Message);
                return default(IEnumerable<TEntity>);
            }
            catch (DbEntityValidationException EFex)
            {
                Debug.WriteLine($"{EFex.Data} cause DbEntityValidationException.");
                Debug.WriteLine("StackTrace:\n" + EFex.StackTrace);
                Debug.WriteLine("Message:\n" + EFex.Message);
                return default(IEnumerable<TEntity>);
            }
            catch (NullReferenceException Nex)
            {
                Debug.WriteLine($"{Nex.Data} cause NullReferenceException.");
                Debug.WriteLine("StackTrace:\n" + Nex.StackTrace);
                Debug.WriteLine("Message:\n" + Nex.Message);
                return default(IEnumerable<TEntity>);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{ex.Data} cause Exception.");
                Debug.WriteLine("StackTrace:\n" + ex.StackTrace);
                Debug.WriteLine("Message:\n" + ex.Message);
                return default(IEnumerable<TEntity>);
            }

        }

        public static IEnumerable<int> RepositoryTryCatch(Func<int, IEnumerable<int>> func, int Key)
        {
            try
            {
                return func.Invoke(Key);
            }
            catch (SqlException SQLex)
            {
                Debug.WriteLine($"{SQLex.Data} cause SqlException.");
                Debug.WriteLine("StackTrace:\n" + SQLex.StackTrace);
                Debug.WriteLine("Message:\n" + SQLex.Message);
                return default(IEnumerable<int>);
            }
            catch (DbEntityValidationException EFex)
            {
                Debug.WriteLine($"{EFex.Data} cause DbEntityValidationException.");
                Debug.WriteLine("StackTrace:\n" + EFex.StackTrace);
                Debug.WriteLine("Message:\n" + EFex.Message);
                return default(IEnumerable<int>);
            }
            catch (NullReferenceException Nex)
            {
                Debug.WriteLine($"{Nex.Data} cause NullReferenceException.");
                Debug.WriteLine("StackTrace:\n" + Nex.StackTrace);
                Debug.WriteLine("Message:\n" + Nex.Message);
                return default(IEnumerable<int>);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{ex.Data} cause Exception.");
                Debug.WriteLine("StackTrace:\n" + ex.StackTrace);
                Debug.WriteLine("Message:\n" + ex.Message);
                return default(IEnumerable<int>);
            }

        }

        public static IEnumerable<int> RepositoryTryCatch(Func<IEnumerable<int>> func)
        {
            try
            {
                return func.Invoke();
            }
            catch (SqlException SQLex)
            {
                Debug.WriteLine($"{SQLex.Data} cause SqlException.");
                Debug.WriteLine("StackTrace:\n" + SQLex.StackTrace);
                Debug.WriteLine("Message:\n" + SQLex.Message);
                return default(IEnumerable<int>);
            }
            catch (DbEntityValidationException EFex)
            {
                Debug.WriteLine($"{EFex.Data} cause DbEntityValidationException.");
                Debug.WriteLine("StackTrace:\n" + EFex.StackTrace);
                Debug.WriteLine("Message:\n" + EFex.Message);
                return default(IEnumerable<int>);
            }
            catch (NullReferenceException Nex)
            {
                Debug.WriteLine($"{Nex.Data} cause NullReferenceException.");
                Debug.WriteLine("StackTrace:\n" + Nex.StackTrace);
                Debug.WriteLine("Message:\n" + Nex.Message);
                return default(IEnumerable<int>);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{ex.Data} cause Exception.");
                Debug.WriteLine("StackTrace:\n" + ex.StackTrace);
                Debug.WriteLine("Message:\n" + ex.Message);
                return default(IEnumerable<int>);
            }

        }
    }
}