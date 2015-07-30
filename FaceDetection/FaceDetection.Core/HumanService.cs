using System.Collections.Generic;
using System.IO;
using System.Linq;
using Emgu.CV;
using Emgu.CV.Structure;

namespace FaceDetection.Core
{
    public class HumanService
    {
        public HumanService()
        {
            People = new List<Human>();
        }

        public List<Human> People { get; set; }

        public void AddHuman(string label, List<Image<Gray, byte>> images)
        {
            var id = People.Count;
            var human = new Human(id, label, images);
            People.Add(human);
            ServicesWorker.GetInstance<DatabaseService>().Insert<Human>(id, human);
        }

        public Human GetHumanFromId(int id)
        {
            return
                ServicesWorker.GetInstance<DatabaseService>().Query<Human>(new Dictionary<string, object> { { "Id", id } });
        }

        public void LoadFromDb()
        {
            People.Clear();
            foreach (var human in ServicesWorker.GetInstance<DatabaseService>().QueryByClassName<Human>())
            {
                var fileNames = Directory.GetFiles("Images\\" + human.Name, "*.jpg");
                human.ImagesEmgu =
                    new List<Image<Gray, byte>>(fileNames.Select(fileName => new Image<Gray, byte>(fileName)).ToList());
                People.Add(human);
            }
        }
    }
}