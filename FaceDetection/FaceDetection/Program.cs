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
            ts.OnGenderCount += WriteGenderRecognizedCount;
            ts.OnCount += WriteCount;

            while (!exitFlag)
            {
                Console.Write(">> ");
                var inputCommand = Console.ReadLine();

                switch (inputCommand)
                {
                    case "sample":
                        Console.WriteLine(@"Введите имя человека и встаньте перед камерой.");
                        Console.WriteLine(@"Пожалуйста, во время записи постарайтесь" + "\n" + @"продемонстрировать разные выражения лица");
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
                        vs.SayText("Привет, незнакомка !");
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

        private static void WriteGenderRecognizedCount(int f, int m)
        {
            Console.WriteLine("F: " + f + ", M: " + m);
        }
        private static void WriteCount(int count, int average)
        {
            Console.WriteLine("Записано " + count + "/" + average);
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
                vs.SayText("Привет " + name);
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
                vs.SayText("Привет " + text);
            }
        }

        private static List<Image<Gray, byte>> GetSamples(string pathName)
        {
            var fileNames = Directory.GetFiles("Images\\" + pathName + "\\", "*.jpg");
            return fileNames.Select(fileName => new Image<Gray, byte>(fileName)).ToList();
        }
    }
}