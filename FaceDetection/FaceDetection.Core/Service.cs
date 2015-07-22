using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Emgu.CV;
using Emgu.Util;
using Emgu.CV.Face;
using Emgu.CV.Structure;

namespace FaceDetection.Core
{
    public class FaceRecognizerService
    {
        FaceRecognizer faceRecognizer;

        public FaceRecognizerService()
        {
            faceRecognizer = new FisherFaceRecognizer();
        }

        public void StartCapture()
        {
            
        }

        public void Train(Dictionary<int, List<Image<Bgr, float>>> trainSamples)
        {
            var labels = new List<int>();
            var images = new List<Image<Bgr, float>>();
            foreach (var key in trainSamples.Keys)
            {
                foreach (var image in trainSamples[key])
                {
                    labels.Add(key);
                    images.Add(image);
                }
            }
            faceRecognizer.Train(images.ToArray(), labels.ToArray());
        }
    }
}
