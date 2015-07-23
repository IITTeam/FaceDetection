using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using Emgu.CV;
using Emgu.Util;
using Emgu.CV.Face;
using Emgu.CV.Features2D;
using Emgu.CV.Structure;

namespace FaceDetection.Core
{
    public class FaceRecognizerService
    {
        FaceRecognizer faceRecognizer;
        private bool anyKeyPress = false;
        public delegate void RecognizeContainer(int label, double distance);
        public event RecognizeContainer recognized;

        public CascadeClassifier FaceCascadeClassifier { get; set; }

        public FaceRecognizerService()
        {
            faceRecognizer = new EigenFaceRecognizer(80, 3000);
            labels = new List<int>();
            images = new List<Image<Gray, byte>>();
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
                    image._EqualizeHist();
                    var grayImage = image.Convert<Gray, byte>();


                    var detectedFace = DetectFace(grayImage);
                    if (detectedFace != null)
                    {
                        var result = faceRecognizer.Predict(detectedFace);
                        if (recognized != null) recognized(result.Label, result.Distance);
                    }
                    
                }
                catch (Exception ex)
                {

                }
            }
        }


        List<int> labels;
        List<Image<Gray, byte>> images;

        public void AddFaces(int label)
        {
            int count = 0;
            var capture = new Capture();
            while (count < 5)
            {
                var image = capture.QueryFrame().ToImage<Bgr, byte>();
                image._EqualizeHist();
                var grayImage = image.Convert<Gray, byte>();

                var detectedFace = DetectFace(grayImage);
                if (detectedFace != null)
                {
                    images.Add(detectedFace);
                    labels.Add(label);
                    count++;
                }

            }
            capture.Dispose();
        }

        public void Train()
        {
            faceRecognizer.Train(images.ToArray(), labels.ToArray());
            faceRecognizer.Save("facerecognizer");
        }

        public void Load()
        {
            faceRecognizer.Load("facerecognizer");
        }


        public void StopCapture()
        {
            this.anyKeyPress = true;
        }

        private Image<Gray, byte> DetectFace(Image<Gray, byte> srcImage)
        {
            Rectangle[] facesDetected = FaceCascadeClassifier.DetectMultiScale(srcImage, 1.2, 10, new Size(50, 50), Size.Empty);

            if (facesDetected.Length > 0)
            {
                Rectangle maxRectangle = facesDetected.First();
                //Action for each element detected
                for (int i = 0; i < facesDetected.Length; i++) // (Rectangle face_found in facesDetected)
                {
                    if (facesDetected[i].Size.Width > maxRectangle.Size.Width)
                    {
                        maxRectangle = facesDetected[i];
                    }
                }
                //This will focus in on the face from the haar results its not perfect but it will remove a majoriy
                //of the background noise
                maxRectangle.X += (int)(maxRectangle.Height * 0.15);
                maxRectangle.Y += (int)(maxRectangle.Width * 0.22);
                maxRectangle.Height -= (int)(maxRectangle.Height * 0.3);
                maxRectangle.Width -= (int)(maxRectangle.Width * 0.35);

                Image<Gray, byte> result = srcImage.Copy(maxRectangle)
                    .Resize(100, 100, Emgu.CV.CvEnum.Inter.Cubic);
                result._EqualizeHist();

                return result;
            }
            return null;
        }
    }
}
