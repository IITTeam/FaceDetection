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

        public void Execute()
        {
            try
            {
                ServicesWorker.GetInstance<FaceRecognizerService>().TrainFaceRecognizer();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

    }
}