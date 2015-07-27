using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Face;
using Emgu.CV.Structure;

namespace FaceDetection.Core
{
    public class FaceRecognizerService
    {
        public delegate void CounterContainer(int f, int m);

        public delegate void GenderRecognizeContainer(bool gender, double distance);

        public delegate void RecognizeContainer(string name, double distance);

        private bool _anyKeyPress = false;
        private bool _faceRecognizerTrained;
        public HumanService HumanService;
        private readonly FaceRecognizer _genderFaceRecognizer;
        private readonly FaceRecognizer _faceRecognizer;
        private DatabaseService dbs;

        public FaceRecognizerService()
        {
            _faceRecognizer = new LBPHFaceRecognizer(1, 8, 8, 8, 100);
            _genderFaceRecognizer = new LBPHFaceRecognizer(1, 8, 8, 8, 100);
            HumanService = new HumanService();
            dbs = ServicesWorker.GetInstance<DatabaseService>();
            //labels = new List<int>();
            //images = new List<Image<Gray, byte>>();
        }

        public CascadeClassifier FaceCascadeClassifier { get; set; }
        public event RecognizeContainer Recognized;
        public event GenderRecognizeContainer GenderRecognized;
        public event CounterContainer OnCount;
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
                        if (_faceRecognizerTrained)
                        {
                            var result = _faceRecognizer.Predict(detectedFace);
                            if (result.Label != -1)
                            {
                                //var human = HumanService.People.Find(x => x.Id == result.Label);
                                var human = HumanService.GetHumanFromId(result.Label);
                                if (human != null)
                                {
                                    if (Recognized != null) Recognized(human.Name, result.Distance);
                                }
                                else
                                {
                                    if (Recognized != null) Recognized(result.Label.ToString(), result.Distance);
                                }
                                continue;
                            }
                        }
                        var gender = _genderFaceRecognizer.Predict(detectedFace);
                        if (gender.Label != -1)
                        {
                            if (GenderRecognized != null) GenderRecognized(gender.Label != 0, gender.Distance);
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
            var images = new List<Image<Gray, byte>>();
            var count = 0;
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
            HumanService.AddHuman(label, images);
            capture.Dispose();
        }

        public void Train()
        {
            var allImages = new List<Image<Gray, byte>>();
            var idList = new List<int>();
            foreach (var human in HumanService.People)
            {
                allImages.AddRange(human.ImagesEmgu);
                idList.AddRange(human.Images.Select(hm => human.Id));
                dbs.Insert(human);
                //for (var i = 0; i < human.Images.Count; i++)
                //    idList.Add(human.Id);
            }

            _faceRecognizer.Train(allImages.ToArray(), idList.ToArray());
            _faceRecognizerTrained = true;
            _faceRecognizer.Save("facerecognizer");
        }

        public void Load()
        {
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
                var facesDetected = FaceCascadeClassifier.DetectMultiScale(srcImage, 1.2, 10, new Size(50, 50), Size.Empty);

                if (facesDetected.Length > 0)
                {
                    var maxRectangle = facesDetected.First();
                    //Action for each element detected
                    foreach (Rectangle tRectangle in facesDetected.Where(tRectangle => tRectangle.Size.Width > maxRectangle.Size.Width))
                    {
                        maxRectangle = tRectangle;
                    }
                    //This will focus in on the face from the haar results its not perfect but it will remove a majoriy
                    //of the background noise
                    maxRectangle.X += (int)(maxRectangle.Height * 0.15);
                    maxRectangle.Y += (int)(maxRectangle.Width * 0.22);
                    maxRectangle.Height -= (int)(maxRectangle.Height * 0.3);
                    maxRectangle.Width -= (int)(maxRectangle.Width * 0.35);

                    var result = srcImage.Copy(maxRectangle)
                        .Resize(100, 100, Inter.Cubic);
                    result._EqualizeHist();

                    return result;
                }
                return null;
            }
            catch (Exception ex)
            {
                srcImage.Save("Images\\badImg.jpg");
                return null;
            }
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
                    //femaleImage._EqualizeHist();
                    //var grayImage = femaleImage.Convert<Gray, byte>();
                    //var detectedFace = DetectFace(grayImage);
                    //if (detectedFace != null)
                    //{
                    allImages.Add(femaleImage);
                      //  detectedFace.Save("Images\\DetFemale\\" + femaleImages.IndexOf(femaleImage) + ".jpg");
                        idList.Add(0);
                        fCount++;
                    //}
                }


                foreach (var maleImage in maleImages)
                {
                    //maleImage._EqualizeHist();
                    //var grayImage = maleImage.Convert<Gray, byte>();
                    //var detectedFace = DetectFace(grayImage);
                    //if (detectedFace != null)
                    //{
                    allImages.Add(maleImage);
                       // detectedFace.Save("Images\\DetMale\\" + maleImages.IndexOf(maleImage) + ".jpg");
                        idList.Add(1);
                        mCount++;
                   // }
                }
            }
            catch (Exception ex)
            {
                
            }

            _genderFaceRecognizer.Train(allImages.ToArray(), idList.ToArray());
            _genderFaceRecognizer.Save("genderfacerecognizer");
            OnCount(fCount, mCount);
        }
    }
}