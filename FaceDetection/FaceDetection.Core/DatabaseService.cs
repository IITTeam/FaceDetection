

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Emgu.CV;
using Emgu.CV.Structure;
using NDatabase;
using NDatabase.Api;
using NDatabase.Api.Query;

namespace FaceDetection.Core
{
    public class DatabaseService
    {
        private const string DbName = "humans.ndb";

        public DatabaseService()
        {
        }

        public void Insert<T>(int id, object obj) where T : class
        {
            if (this.Query<T>(new Dictionary<string, object> { { "Id", id } }) == null)
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
                        IQuery query = db.Query<T>();
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
    }
}