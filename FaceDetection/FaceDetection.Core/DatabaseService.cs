using System;
using System.Collections.Generic;
using System.Linq;
using NDatabase;
using NDatabase.Api;

namespace FaceDetection.Core
{
    public class DatabaseService
    {
        private const string DbName = "humans.ndb";

        public void Insert<T>(int id, object obj) where T : class
        {
            if (Query<T>(new Dictionary<string, object> {{"Id", id}}) == null)
            {
                using (var db = OdbFactory.Open(DbName))
                {
                    db.Store(obj);
                }
            }
        }

        public void Delete(object obj)
        {
            using (var db = OdbFactory.Open(DbName))
            {
                db.Delete(obj);
            }
        }

        public T Query<T>(Dictionary<string, object> @params) where T : class
        {
            try
            {
                using (var db = OdbFactory.Open(DbName))
                {
                    IObjectSet<T> result = null;
                    foreach (var param in @params)
                    {
                        var query = db.Query<T>();
                        query.Descend(param.Key).Constrain(param.Value).Equal();
                        result = query.Execute<T>();
                    }
                    return result.GetFirst();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public List<T> QueryByClassName<T>() where T : class
        {
            using (var db = OdbFactory.Open(DbName))
            {
                var result = db.Query<T>().Execute<T>();
                return result.ToList();
            }
        }

        public void Clear()
        {
            OdbFactory.Delete(DbName);
        }
    }
}