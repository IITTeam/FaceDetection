using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Emgu.CV;
using Emgu.CV.Structure;
using SQLite;

namespace FaceDetection.Core
{
    public class Human
    {
        [PrimaryKey]
        public int Id { get; private set; }
        public string Name { get; private set; }
        public List<Bitmap> Images { get; set; }
        public bool Gender { get; private set; }
        public List<Image<Gray, byte>> ImagesEmgu;

        public Human(int id, string name, List<Image<Gray, byte>> images)
        {
            Id = id;
            Name = name;
            ImagesEmgu = new List<Image<Gray, byte>>(images);
            Images = images.Select(i => i.Bitmap).ToList();
        }

        public Human()
        {
        }
    }
}