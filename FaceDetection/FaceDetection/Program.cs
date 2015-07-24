using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using FaceDetection.Core;

namespace FaceDetection
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var exitFlag = false;
            ServicesWorker.Registration(new CoreModule());


            //Попросим у контейнера сервис 
            var ts = ServicesWorker.GetInstance<FaceRecognizerService>();
            var vs = ServicesWorker.GetInstance<VoiceAssistantService>();

            ts.FaceCascadeClassifier =
                new CascadeClassifier(Application.StartupPath + "/Cascade/haarcascade_frontalface_default.xml");
            ts.recognized += WriteResult;
            //Дальше можно работать как просто с объектом
            while (!exitFlag)
            {
                Console.Write(">> ");
                var inputCommand = Console.ReadLine();

                switch (inputCommand)
                {
                    case "sample":
                        Console.WriteLine("Введите имя человека и встаньте перед камерой ");
                        var name = Console.ReadLine();
                        try
                        {
                            ts.AddFaces(name);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                        break;
                    case "train":
                        ts.Train();
                        break;
                    case "load":
                        ts.Load();
                        break;
                    case "check":
                        try
                        {
                            var t = new Thread(ts.StartCapture);
                            t.Start();

                            if (Console.ReadKey() != null)
                            {
                                t.Abort();
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }

                        break;

                    case "say":
                        var i = 1;
                        foreach (var voice in vs.GetAllVoices())
                        {
                            Console.WriteLine(i + "-" + voice);
                        }
                        Console.WriteLine("Выберите голос>>");
                        var input = Convert.ToInt32(Console.ReadLine());
                        vs.SayText("Hi, man!", input);
                        break;
                        ;
                    case "exit":
                        exitFlag = true;
                        break;
                    case "help":
                        Console.WriteLine("train check exit");
                        break;
                    default:
                        Console.WriteLine("Неверная команда!");
                        break;
                }
            }
        }

        public static Dictionary<int, List<Image<Gray, byte>>> GetSampleList()
        {
            var fileNames = Directory.GetFiles("Images\\Danil\\", "*.jpg");
            var resultDict = new Dictionary<int, List<Image<Gray, byte>>>();
            var imageList = new List<Image<Gray, byte>>();
            foreach (var fileName in fileNames)
            {
                var image = new Image<Bgr, byte>(fileName);
                var resultImage = image.Resize(100, 100, Inter.Cubic);
                resultImage._EqualizeHist();
                var grayImage = resultImage.Convert<Gray, byte>();
                imageList.Add(grayImage);
            }
            resultDict.Add(1, imageList);

            fileNames = Directory.GetFiles("Images\\Anna\\", "*.jpg");
            imageList = new List<Image<Gray, byte>>();
            foreach (var fileName in fileNames)
            {
                var image = new Image<Bgr, byte>(fileName);
                var resultImage = image.Resize(100, 100, Inter.Cubic);
                resultImage._EqualizeHist();

                var grayImage = resultImage.Convert<Gray, byte>();
                imageList.Add(grayImage);
            }
            resultDict.Add(2, imageList);

            return resultDict;
        }

        private static void WriteResult(Human human, double distance)
        {
            
            Console.WriteLine(human.Name + " " + distance);
        }
    }
}