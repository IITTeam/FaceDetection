

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

        public void Insert(object obj)
        {
            using (var db = OdbFactory.Open(DbName))
            {
                var obd = db.Store(obj);
                if (db.GetObjectFromId(obd) != null)
                    db.Rollback();
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