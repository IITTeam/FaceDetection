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
            faceRecognizer = new EigenFaceRecognizer(80, 3000);
        }

        //public double StartCapture(Image<Bgr, byte> image)
        //{
        //    return Recognize(image);
        //}

        public void StartCapture()
        {
            var capture = new Capture();
            while (!anyKeyPress)
            {
                try
                {
                    var image = capture.QueryFrame().ToImage<Bgr, byte>();

                    var resultImage = image.Resize(100, 100, Emgu.CV.CvEnum.Inter.Cubic);
                    resultImage._EqualizeHist();
                    var grayImage = resultImage.Convert<Gray, byte>();

                    var result = faceRecognizer.Predict(grayImage);
                    if (recognized != null) recognized(result.Label, result.Distance);
                }
                catch (Exception ex)
                {

                }
            }
        }

        

        public void Train(Dictionary<int, List<Image<Gray, byte>>> trainSamples)
        {
            var labels = new List<int>();
            var images = new List<Image<Gray, byte>>();
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
