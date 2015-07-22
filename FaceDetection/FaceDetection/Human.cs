using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Emgu.CV;
using Emgu.CV.Structure;

namespace FaceDetection
{
    class Human
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public List<Image<Bgr, float>> Images { get; private set; }

        public Human(int id, string name)
        {
            this.Id = id;
            this.Name = name;
            this.Images = new List<Image<Bgr, float>>();
        }
    }
}
