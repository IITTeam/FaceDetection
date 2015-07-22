using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.IO;
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
            var vs = ServicesWorker.GetInstance<VoiceAssistantService>();
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
                        try
                        {
                            Console.WriteLine(ts.StartCapture());
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                        
                        break;

                    case "say":
                        vs.SayText("Hi, man!", "Microsoft Hazel Desktop");
                        break;;
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

        public static Dictionary<int, List<Image<Gray, float>>> GetSampleList()
        {
            var fileNames = Directory.GetFiles("Images\\Danil\\", "*.jpg");
            var resultDict = new Dictionary<int, List<Image<Gray, float>>>();
            var imageList = new List<Image<Gray, float>>();
            foreach (string fileName in fileNames)
            {
                var image = new Image<Gray, float>(fileName);
                imageList.Add(image);
            }
            resultDict.Add(1, imageList);

            fileNames = Directory.GetFiles("Images\\Anna\\", "*.jpg");
            imageList = new List<Image<Gray, float>>();
            foreach (string fileName in fileNames)
            {
                var image = new Image<Gray, float>(fileName);
                imageList.Add(image);
            }
            resultDict.Add(2, imageList);

            return resultDict;
        }
    }
}
