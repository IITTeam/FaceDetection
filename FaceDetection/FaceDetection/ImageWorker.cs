using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

using Emgu.CV.Face;
using Emgu.CV;
using Emgu.CV.Structure;

namespace FaceDetection
{
    class ImageWorker
    {
        static public Image<Bgr, float> GetImage(string fileName)
        {
            var image = Image.FromFile(fileName);
            return null;
        }
    }
}
