using System.Collections.Generic;
using Emgu.CV;
using Emgu.CV.Structure;

namespace FaceDetection.Core
{
    public class Human
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public List<Image<Gray, byte>> Images { get; private set; }
        public bool Gender { get; private set; }
        public Human(int id, string name, List<Image<Gray, byte>> images)
        {
            Id = id;
            Name = name;
            Images = new List<Image<Gray, byte>>(images);
        }
    }
}