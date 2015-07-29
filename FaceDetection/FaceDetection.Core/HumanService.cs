using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Emgu.CV;
using Emgu.CV.Structure;

namespace FaceDetection.Core
{
    public class HumanService
    {
        public List<Human> People { get; set; }
        private DatabaseService dbs;

        public HumanService()
        {
            People = new List<Human>();
            dbs = ServicesWorker.GetInstance<DatabaseService>();
        }

        public void AddHuman(string label, List<Image<Gray, byte>> images)
        {
            int id = People.Count;
            var human = new Human(id, label, images);
            People.Add(human);
            dbs.Insert<Human>(id, human);
        }

        public Human GetHumanFromId(int id)
        {
            return ServicesWorker.GetInstance<DatabaseService>().Query<Human>(new Dictionary<string, object> { { "Id", id } });
        }

        public void LoadFromDB()
        {
            People.Clear();
            foreach (var human in dbs.QueryByClassName<Human>())
            {
                var fileNames = Directory.GetFiles("Images\\" + human.Name, "*.jpg");
                human.ImagesEmgu = new List<Image<Gray, byte>>(fileNames.Select(fileName => new Image<Gray, byte>(fileName)).ToList());
                People.Add(human);
            }
        }
    }
}
