using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
            ServicesWorker.Registration(new CoreModule());

            var faceRecognizerService = ServicesWorker.GetInstance<FaceRecognizerService>();
            faceRecognizerService.FaceCascadeClassifier =
                new CascadeClassifier(Application.StartupPath + "\\Cascade\\haarcascade_frontalface_default.xml");
            faceRecognizerService.Recognized += WriteResult;
            faceRecognizerService.GenderRecognized += WriteResult;
            faceRecognizerService.OnGenderCount += WriteGenderRecognizedCount;
            faceRecognizerService.OnCount += WriteCount;
            faceRecognizerService.Load();

            var app = new CommandProcessor.Application();
            app.Run();
            Console.ReadKey();
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