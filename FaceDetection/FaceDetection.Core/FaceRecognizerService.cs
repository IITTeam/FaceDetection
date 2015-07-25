using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Threading;
using System.IO;
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
        FaceRecognizer genderFaceRecognizer;
        private bool anyKeyPress = false;
        public delegate void RecognizeContainer(string name, double distance);
        public event RecognizeContainer recognized;
        public delegate void GenderRecognizeContainer(bool gender, double distance);
        public event GenderRecognizeContainer genderRecognized;
        public HumanService humanService;


        public delegate void CounterContainer(int f, int m);
        public event CounterContainer onCount;

        public CascadeClassifier FaceCascadeClassifier { get; set; }

        private bool faceRecognizerTrained = false;

        public FaceRecognizerService()
        {
            faceRecognizer = new LBPHFaceRecognizer(1, 8, 8, 8, 100);
            genderFaceRecognizer = new LBPHFaceRecognizer(1, 8, 8, 8, 100);
            humanService = new HumanService();
            //labels = new List<int>();
            //images = new List<Image<Gray, byte>>();
        }

        //public double StartCapture(Image<Bgr, byte> image)
        //{
        //    return Recognize(image);
        //}

        public void StartCapture()
        {
            var capture = new Capture();
            while (true)
            {
                try
                {
                    var image = capture.QueryFrame().ToImage<Bgr, byte>();
                    image._EqualizeHist();
                    var grayImage = image.Convert<Gray, byte>();


                    var detectedFace = DetectFace(grayImage);
                    if (detectedFace != null)
                    {
                        if (faceRecognizerTrained)
                        {
                            var result = faceRecognizer.Predict(detectedFace);
                            if (result.Label != -1)
                            {
                                var human = humanService.People.Find(x => x.Id == result.Label);
                                if (human != null)
                                {
                                    if (recognized != null) recognized(human.Name, result.Distance);
                                }
                                else
                                {
                                    if (recognized != null) recognized(result.Label.ToString(), result.Distance);
                                }
                                continue;
                            }
                        }
                        var gender = genderFaceRecognizer.Predict(detectedFace);
                        if (gender.Label != -1)
                        {
                            if (genderRecognized != null) genderRecognized(gender.Label != 0, gender.Distance);
                        }
                    }

                }
                catch (ThreadAbortException ex)
                {
                    capture.Dispose();
                }
            }
        }


        //List<int> labels;
        //List<Image<Gray, byte>> images;

        public void AddFaces(string label)
        {
            List<Image<Gray, byte>> images = new List<Image<Gray, byte>>();
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
                    //labels.Add(label);
                    count++;
                }

            }
            humanService.AddHuman(label, images);
            capture.Dispose();
        }

        public void Train()
        {
            List<Image<Gray, byte>> allImages = new List<Image<Gray, byte>>();
            List<int> idList = new List<int>();
            foreach (var human in humanService.People)
            {
                allImages.AddRange(human.Images);
                for (int i = 0; i < human.Images.Count; i++)
                    idList.Add(human.Id);
            }

            faceRecognizer.Train(allImages.ToArray(), idList.ToArray());
            faceRecognizerTrained = true;
            faceRecognizer.Save("facerecognizer");
        }

        public void Load()
        {
            if (File.Exists("facerecognizer"))
            {
                faceRecognizer.Load("facerecognizer");
                faceRecognizerTrained = true;
            }
            if (File.Exists("genderfacerecognizer"))
                genderFaceRecognizer.Load("genderfacerecognizer");
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

        /// <summary>
        /// Обучение на распознавание пола человека
        /// </summary>
        /// <param name="maleImages"></param>
        /// <param name="femaleImages"></param>
        public void TrainGender(List<Image<Bgr, byte>> maleImages, List<Image<Bgr, byte>> femaleImages)
        {
            var allImages = new List<Image<Gray, byte>>();
            var idList = new List<int>();

            var fCount = 0;
            var mCount = 0;


            foreach (var femaleImage in femaleImages)
            {
                femaleImage._EqualizeHist();
                var grayImage = femaleImage.Convert<Gray, byte>();
                var detectedFace = DetectFace(grayImage);
                if (detectedFace != null)
                {
                    allImages.Add(detectedFace);
                    idList.Add(0);
                    fCount++;
                }
            }


            foreach (var maleImage in maleImages)
            {
                maleImage._EqualizeHist();
                var grayImage = maleImage.Convert<Gray, byte>();
                var detectedFace = DetectFace(grayImage);
                if (detectedFace != null)
                {
                    allImages.Add(detectedFace);
                    idList.Add(1);
                    mCount++;
                }
            }

            genderFaceRecognizer.Train(allImages.ToArray(), idList.ToArray());
            genderFaceRecognizer.Save("genderfacerecognizer");
            onCount(fCount, mCount);
        }
    }
}
