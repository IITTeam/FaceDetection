using System.Collections.Generic;
using System.IO;
using System.Linq;
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
            bool has_deleting_objs = false;
            foreach (var human in ServicesWorker.GetInstance<DatabaseService>().QueryByClassName<Human>())
            {
                if (Directory.Exists("Images\\" + human.Name))
                {
                    var fileNames = Directory.GetFiles("Images\\" + human.Name, "*.jpg");
                    human.ImagesEmgu =
                        new List<Image<Gray, byte>>(fileNames.Select(fileName => new Image<Gray, byte>(fileName)).ToList());
                    People.Add(human);
                }
                else
                    has_deleting_objs = true;
            }
            if (has_deleting_objs)
            {
                ServicesWorker.GetInstance<DatabaseService>().Clear();
                foreach (var human in People)
                {
                    ServicesWorker.GetInstance<DatabaseService>().Insert<Human>(human.Id, human);
                }
            }
        }
    }
}