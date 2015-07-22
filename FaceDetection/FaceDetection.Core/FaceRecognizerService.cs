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

        public double StartCapture(Image<Gray, float> image)
        {
            return Recognize(image);
        }

        public double StartCapture()
        {
            var capture = new Capture();
            var image = capture.QueryFrame(); //draw the image obtained from camera
            return Recognize(image.ToImage<Gray, float>());
        }



        private double Recognize(Image<Gray, float> image)
        {
            var result = faceRecognizer.Predict(image);
            return result.Label;
        }

        public void Train(Dictionary<int, List<Image<Gray, float>>> trainSamples)
        {
            var labels = new List<int>();
            var images = new List<Image<Gray, float>>();
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
