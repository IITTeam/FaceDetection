using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Emgu.CV;
using Emgu.CV.Structure;

namespace FaceDetection.Core
{
    class HumanCreatorService
    {
        List<Human> People {get; set; }

        public HumanCreatorService()
        {
            People = new List<Human>();
        }

        public void AddHuman(string label, List<Image<Bgr, byte>> images)
        {
            People.Add(new Human(People.Count, label, images));
        }

        public void AddHuman(Human human)
        {
            People.Add(human);
        }
    }
}
