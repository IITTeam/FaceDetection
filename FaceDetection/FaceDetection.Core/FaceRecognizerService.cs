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
        private bool anyKeyPress = false;
        public delegate void RecognizeContainer(int label, double distance);
        public event RecognizeContainer recognized;

        public FaceRecognizerService()
        {
            faceRecognizer = new FisherFaceRecognizer(0, 3500);
        }

        //public double StartCapture(Image<Gray, float> image)
        //{
        //    return Recognize(image);
        //}

        public void StartCapture()
        {
            var capture = new Capture();
            while (!anyKeyPress)
            {
                var image = capture.QueryFrame().ToImage<Gray, float>();
                var result = faceRecognizer.Predict(image);
                if (recognized != null) recognized(result.Label, result.Distance);
            }
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


        public void StopCapture()
        {
            this.anyKeyPress = true;
        }
    }
}
