using System;
using System.Collections.Generic;
using System.Windows.Forms;
using FaceDetection.Core;
using Emgu.CV;
using Emgu.CV.Structure;

namespace FaceDetection
{
    class Program
    {
        static void Main(string[] args)
        {
            var exitFlag = false;
            ServicesWorker.Registration(new CoreModule());

            //Попросим у контейнера сервис 
            var ts = ServicesWorker.GetInstance<FaceRecognizerService>();
            //Дальше можно работать как просто с объектом
            while (!exitFlag)
            {
                Console.Write(">> ");
                var inputCommand = Console.ReadLine();

                switch (inputCommand)
                {
                    case "train":
                        try
                        {
                            ts.Train(GetSampleList());
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                        break;
                    case "check":

                        //Зарегистрируем(создадим) модуль-"ядро"
                        
                        ts.StartCapture();
                        //TODO
                        break;
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

        public static Dictionary<int, List<Image<Bgr, float>>> GetSampleList()
        {
            var resultDict = new Dictionary<int, List<Image<Bgr, float>>>();

            var image1 = new Image<Bgr, float>("Images\\1.jpg");
            var image2 = new Image<Bgr, float>("Images\\2.jpg");
            var imageList = new List<Image<Bgr, float>> {image1, image2};
            resultDict.Add(1, imageList);

            var image3 = new Image<Bgr, float>("Images\\3.jpg");
            var image4 = new Image<Bgr, float>("Images\\4.jpg");
            imageList = new List<Image<Bgr, float>> {image3, image4};
            resultDict.Add(2, imageList);

            return resultDict;
        }
    }
}
