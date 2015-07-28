using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Emgu.CV;
using Emgu.CV.Structure;

namespace FaceDetection.Core
{
    public class HumanService
    {
        public List<Human> People { get; set; }

        public HumanService()
        {
            People = new List<Human>();
        }

        public void AddHuman(string label, List<Image<Gray, byte>> images)
        {
            People.Add(new Human(People.Count, label, images));
        }

        public void AddHuman(Human human)
        {
            People.Add(human);
        }

        public Human GetHumanFromId(int id)
        {
            //new Dictionary<string, object> { { "Id", id } }
            return ServicesWorker.GetInstance<DatabaseService>().Query<Human>(id) as Human;
        }
    }
}
