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
            ts.genderRecognized += WriteResult;
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
                    //case "traingender":
                    //    ts.TrainGender();
                    //    break;
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

        private static void WriteResult(string name, double distance)
        {
            Console.WriteLine(name + " " + distance);
        }

        private static void WriteResult(bool gender, double distance)
        {
            Console.WriteLine(gender?"М":"Ж" + " " + distance);
        }
    }
}