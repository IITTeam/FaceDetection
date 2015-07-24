using System.Collections.Generic;
using Emgu.CV;
using Emgu.CV.Structure;

namespace FaceDetection.Core
{
    class Human
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public List<Image<Bgr, byte>> Images { get; private set; }

        public Human(int id, string name, List<Image<Bgr, byte>> images)
        {
            this.Id = id;
            this.Name = name;
            this.Images = new List<Image<Bgr, byte>>(images);
        }
    }
}
