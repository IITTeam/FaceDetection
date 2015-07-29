using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Emgu.CV;
using Emgu.CV.Structure;
using FaceDetection.Core;

namespace FaceDetection.CommandProcessor
{
    public class TrainCommand : ICommand
    {
        public string Name
        {
            get { return "train"; }
        }

        public string Description
        {
            get { return "- обучение программы"; }
        }

        public void Execute(List<string> parameters)
        {
            Console.WriteLine(@"1 - Обучение ранее запомненным  лицам" + @"2 - Обучение для распознования пола");
            var choice = Convert.ToInt32(Console.ReadLine());
            switch (choice)
            {
                case 1:
                    ServicesWorker.GetInstance<FaceRecognizerService>().Train();
                    break;
                case 2:
                    ServicesWorker.GetInstance<FaceRecognizerService>()
                        .TrainGender(GetSamples("DetMale"), GetSamples("DetFemale"));
                    break;
                default:
                    Console.WriteLine(@"Нет такой команды");
                    break;
            }
        }

        private static List<Image<Gray, byte>> GetSamples(string pathName)
        {
            var fileNames = Directory.GetFiles("Images\\" + pathName + "\\", "*.jpg");
            return fileNames.Select(fileName => new Image<Gray, byte>(fileName)).ToList();
        }
    }
}