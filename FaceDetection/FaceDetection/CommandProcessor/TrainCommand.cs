using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
            ServicesWorker.GetInstance<FaceRecognizerService>().TrainFaceRecognizer();
            //Console.WriteLine(@"1 - Обучение ранее запомненным  лицам" + @"2 - Обучение для распознавания пола");
            //var choice = Convert.ToInt32(Console.ReadLine());
            //switch (choice)
            //{
            //    case 1:
            //        ServicesWorker.GetInstance<FaceRecognizerService>().TrainFaceRecognizer();
            //        break;
            //    case 2:
            //        ServicesWorker.GetInstance<FaceRecognizerService>()
            //            .TrainGender(GetSamples("DetMale"), GetSamples("DetFemale"));
            //        break;
            //    default:
            //        Console.WriteLine(@"Нет такой команды");
            //        break;
            //}
        }

    }
}