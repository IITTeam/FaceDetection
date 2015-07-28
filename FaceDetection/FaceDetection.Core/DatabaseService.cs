

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.Structure;
using SQLite;

//using NDatabase;
//using NDatabase.Api;
//using NDatabase.Api.Query;

namespace FaceDetection.Core
{
    public class DatabaseService
    {
        private SQLiteConnection db;

        public DatabaseService()
        {
            try
            {
                var sqlFolder = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
                db = new SQLiteConnection(System.IO.Path.Combine(sqlFolder, "human.db"));
                if (db.Table<Human>().Table == null)
                {
                    db.CreateTable<Human>();
                }
            }
            catch (Exception ex)
            {
                var s = ex.Message;
            }

        }

        public void Insert(object obj)
        {
            db.Update(obj);

            //using (var db = OdbFactory.Open(DbName))
            //{
            //    var obd = db.Store(obj);
            //    if (db.GetObjectFromId(obd) != null)
            //        db.Rollback();
            //}
        }

        public void Delete(object obj)
        {
            //using (var db = OdbFactory.Open(DbName))
            //{
            //    db.Delete(obj);
            //}
        }

        public object Query<T>(int id) where T : class
        {
            var query = db.Get<object>(id);
            return query;
            //using (var db = OdbFactory.Open(DbName))
            //{
            //    IObjectSet<T> result = null;
            //    foreach (var param in @params)
            //    {
            //        IQuery query = db.Query<T>();
            //        query.Descend(param.Key).Constrain(param.Value).Equal();
            //        result = query.Execute<T>();
            //    }
            //    return result.GetFirst();
            //}

            return null;
        }

        public List<T> QueryByClassName<T>() where T : class
        {
            //using (var db = OdbFactory.Open(DbName))
            //{
            //    var result = db.Query<T>().Execute<T>();
            //    return result.ToList();
            //}

            return null;
        }
    }
}