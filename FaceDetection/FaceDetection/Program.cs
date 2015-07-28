using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.Structure;
using FaceDetection.Core;

namespace FaceDetection
{
    internal class Program
    {
        public static string CurrentName = "";
        private static int _counter;

        private static void Main(string[] args)
        {
            var exitFlag = false;
            ServicesWorker.Registration(new CoreModule());

            var ts = ServicesWorker.GetInstance<FaceRecognizerService>();
            var dbs = ServicesWorker.GetInstance<DatabaseService>();
            var vs = ServicesWorker.GetInstance<VoiceAssistantService>();
            ts.FaceCascadeClassifier =
                new CascadeClassifier(Application.StartupPath + "/Cascade/haarcascade_frontalface_default.xml");
            ts.Recognized += WriteResult;
            ts.GenderRecognized += WriteResult;
            ts.OnCount += WriteCount;

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
                    case "traingender":
                        ts.TrainGender(GetSamples("DetMale"), GetSamples("DetFemale"));
                        break;
                    case "detectGender":
                        ts.DetectGender(GetSamples("Male"), GetSamples("Female"));
                        break;
                    case "load":
                        ts.Load();
                        break;
                    case "db":
                        dbs.QueryByClassName<Human>();
                        //var d = new Dictionary<string, object>();
                        //d.Add("Id", 111);
                        //dbs.Query<Human>(d);
                        //dbs.Insert<Human>(new Human(111,"test",new List<Image<Gray, byte>>()));
                        dbs.QueryByClassName<Human>();
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
                            i++;
                        }
                        Console.WriteLine("Выберите голос>>");
                        var input = Convert.ToInt32(Console.ReadLine());
                        vs.SayText("Привет, незнакомка !", input - 1);
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

        private static void WriteCount(int f, int m)
        {
            Console.WriteLine("F: " + f + ", M: " + m);
        }

        private static void WriteResult(string name, double distance)
        {
            var vs = ServicesWorker.GetInstance<VoiceAssistantService>();
            if (name == CurrentName)
                _counter++;
            else
            {
                _counter = 0;
                CurrentName = name;
            }
            Console.WriteLine(name + " " + distance);
            if (_counter == 5)
            {
                vs.SayText("Привет " + name, 0);
            }
        }

        private static void WriteResult(bool gender, double distance)
        {
            var text = "";
            text = gender ? "незнакомец" : "незнакомка";
            var vs = ServicesWorker.GetInstance<VoiceAssistantService>();
            if (text == CurrentName)
                _counter++;
            else
            {
                _counter = 0;
                CurrentName = text;
            }
            var g = gender ? "М" : "Ж";
            Console.WriteLine(g + " " + distance);
            if (_counter == 5)
            {
                vs.SayText("Привет " + text, 0);
            }
        }

        private static List<Image<Gray, byte>> GetSamples(string pathName)
        {
            var fileNames = Directory.GetFiles("Images\\" + pathName + "\\", "*.jpg");
            return fileNames.Select(fileName => new Image<Gray, byte>(fileName)).ToList();
        }
    }
}