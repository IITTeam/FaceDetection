using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Face;
using Emgu.CV.Structure;

namespace FaceDetection.Core
{
    public class FaceRecognizerService
    {
        public delegate void CounterContainer(int c, int a);

        public delegate void CounterGenderContainer(int f, int m);

        public delegate void GenderRecognizeContainer(bool gender, double distance);

        public delegate void RecognizeContainer(string name, double distance);

        private const int FaceCount = 10;
        private readonly FaceRecognizer _faceRecognizer;
        private readonly FaceRecognizer _genderFaceRecognizer;
        private bool _faceRecognizerTrained;

        public FaceRecognizerService()
        {
            _faceRecognizer = new LBPHFaceRecognizer(1, 8, 8, 8, 80);
            _genderFaceRecognizer = new LBPHFaceRecognizer(1, 8, 8, 8, 100);
        }

        public CascadeClassifier FaceCascadeClassifier { get; set; }
        public event RecognizeContainer Recognized;
        public event GenderRecognizeContainer GenderRecognized;
        public event CounterGenderContainer OnGenderCount;
        public event CounterContainer OnCount;

        public void DetectHuman()
        {
            var capture = new Capture();
            while (true)
            {
                try
                {
                    var image = capture.QueryFrame().ToImage<Gray, byte>();
                    var detectedFace = DetectFace(image);
                    if (detectedFace != null)
                    {
                        if (_faceRecognizerTrained)
                        {
                            if (RecognizeFamiliarPerson(detectedFace))
                                continue;
                        }
                        var gender = _genderFaceRecognizer.Predict(detectedFace);
                        if (gender.Label != -1)
                        {
                            GenderRecognized?.Invoke(gender.Label != 0, gender.Distance);
                        }
                    }
                }
                catch (ThreadAbortException ex)
                {
                    capture.Dispose();
                }
            }
        }

        private bool RecognizeFamiliarPerson(Image<Gray, byte> detectedFace)
        {
            var result = _faceRecognizer.Predict(detectedFace);
            if (result.Label != -1)
            {
                var human = ServicesWorker.GetInstance<HumanService>().GetHumanFromId(result.Label);
                if (human != null)
                {
                    Recognized?.Invoke(human.Name, result.Distance);
                }
                else
                {
                    Recognized?.Invoke(result.Label.ToString(), result.Distance);
                }
                return true;
            }
            return false;
        }

        public List<Image<Gray, byte>> AddImagesToHuman(string name)
        {
            var images = new List<Image<Gray, byte>>();
            var count = 0;
            var capture = new Capture();
            while (count < FaceCount)
            {
                var image = capture.QueryFrame().ToImage<Gray, byte>();
                var detectedFace = DetectFace(image);
                if (detectedFace != null)
                {
                    images.Add(detectedFace);
                    count++;
                    OnCount(count, FaceCount);
                    Thread.Sleep(500);
                }
            }
            ServicesWorker.GetInstance<HumanService>().AddHuman(name, images);
            capture.Dispose();
            return images;
        }

        public void TrainFaceRecognizer()
        {
            var allImages = new List<Image<Gray, byte>>();
            var idList = new List<int>();
            foreach (var human in ServicesWorker.GetInstance<HumanService>().People)
            {
                allImages.AddRange(human.ImagesEmgu);
                idList.AddRange(human.ImagesEmgu.Select(hm => human.Id));
            }
            if (idList.Count == 0 || allImages.Count == 0)
            {
                throw new NoHumanException("Нет данных о людях для обучения!");
            }
            _faceRecognizer.Train(allImages.ToArray(), idList.ToArray());
            _faceRecognizerTrained = true;
            _faceRecognizer.Save("facerecognizer");
        }

        public void Load()
        {
            ServicesWorker.GetInstance<HumanService>().LoadFromDb();
            if (File.Exists("facerecognizer"))
            {
                _faceRecognizer.Load("facerecognizer");
                _faceRecognizerTrained = true;
            }
            if (File.Exists("genderfacerecognizer"))
                _genderFaceRecognizer.Load("genderfacerecognizer");
        }

        private Image<Gray, byte> DetectFace(Image<Gray, byte> srcImage)
        {
            try
            {
                var facesDetected = FaceCascadeClassifier.DetectMultiScale(srcImage, 1.2, 10, new Size(50, 50),
                    Size.Empty);
                if (facesDetected.Length <= 0) return null;
                var maxRectangle = facesDetected.First();
                foreach (
                    var tRectangle in
                        facesDetected.Where(tRectangle => tRectangle.Size.Width > maxRectangle.Size.Width))
                {
                    maxRectangle = tRectangle;
                }
                maxRectangle.X += (int) (maxRectangle.Height*0.15);
                maxRectangle.Y += (int) (maxRectangle.Width*0.22);
                maxRectangle.Height -= (int) (maxRectangle.Height*0.3);
                maxRectangle.Width -= (int) (maxRectangle.Width*0.35);

                var result = srcImage.Copy(maxRectangle)
                    .Resize(100, 100, Inter.Cubic);
                result._EqualizeHist();

                return result;
            }
            catch (Exception ex)
            {
                srcImage.Save("Images\\badImg.jpg");
                return null;
            }
        }

        public void DetectGender(List<Image<Gray, byte>> maleImages, List<Image<Gray, byte>> femaleImages)
        {
            var fCount = 0;
            var mCount = 0;

            try
            {
                foreach (var femaleImage in femaleImages)
                {
                    var grayImage = femaleImage.Convert<Gray, byte>();
                    var detectedFace = DetectFace(grayImage);
                    if (detectedFace != null)
                    {
                        detectedFace.Save("Images\\DetFemale\\" + femaleImages.IndexOf(femaleImage) + ".jpg");
                        fCount++;
                    }
                }

                foreach (var maleImage in maleImages)
                {
                    var grayImage = maleImage.Convert<Gray, byte>();
                    var detectedFace = DetectFace(grayImage);
                    if (detectedFace != null)
                    {
                        detectedFace.Save("Images\\DetMale\\" + maleImages.IndexOf(maleImage) + ".jpg");
                        mCount++;
                    }
                }
            }
            catch (Exception ex)
            {
            }

            OnGenderCount(fCount, mCount);
        }

        /// <summary>
        ///     Обучение на распознавание пола человека
        /// </summary>
        /// <param name="maleImages"></param>
        /// <param name="femaleImages"></param>
        public void TrainGender(List<Image<Gray, byte>> maleImages, List<Image<Gray, byte>> femaleImages)
        {
            var allImages = new List<Image<Gray, byte>>();
            var idList = new List<int>();

            var fCount = 0;
            var mCount = 0;

            try
            {
                foreach (var femaleImage in femaleImages)
                {
                    allImages.Add(femaleImage);
                    idList.Add(0);
                    fCount++;
                }

                foreach (var maleImage in maleImages)
                {
                    allImages.Add(maleImage);
                    idList.Add(1);
                    mCount++;
                }
            }
            catch (Exception ex)
            {
            }

            _genderFaceRecognizer.Train(allImages.ToArray(), idList.ToArray());
            _genderFaceRecognizer.Save("genderfacerecognizer");
            OnGenderCount(fCount, mCount);
        }
    }
}